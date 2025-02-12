﻿/* The MIT License
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
 * IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;

namespace DatabaseRestore
{
    internal class Program
    {
        private class UserRightItem
        {
            public string Name { get; set; }
            public bool Read { get; set; }
            public bool Write { get; set; }
            public bool Owner { get; set; }
        }

        private class MoveItem
        {
            public string LogicalName { get; set; }
            public string PhysicalName { get; set; }
        }

        public enum AutoSourceMode
        {
            None = 0,
            lastcreated = 1,
            lastmodified = 2
        }

        private class OptionsClass
        {
            // --autosource mode
            public AutoSourceMode AutoSourceMode { get; set; }
            // --autosource path
            public string SourcePath { get; set; }
            // --source
            public string SourceFile { get; set; }
            // -- temp
            public string TempFile { get; set; }
            // --serverip
            public string SQLServerIP { get; set; }
            // --servername
            public string SQLServerName { get; set; }
            // --serverport
            public int ServerPort = 1433;
            // --database
            public string DatabaseName { get; set; }
            // inferred if not --username and --password specified
            public bool SSPI = true;
            // --username
            public string SQLUsername { get; set; }
            // --password
            public string SQLPassword { get; set; }
            // --rights
            public string UserRightsString { get; set; }
            // parsed from --rights
            public List<UserRightItem> UserRights { get; set; }
            // --moveallfiles
            public bool MoveAllFiles { get; set; }
            public string MoveAllPath { get; set; }
            // --movefile
            public List<MoveItem> MoveItems { get; set; }
            // --replacedatabase
            public bool ReplaceDatabase { get; set; }
            // --dbcccheckdb
            public bool DbccCheckDB { get; set; }
        }

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                return 0;
            }
            OptionsClass options = null;
            if (!ParseOptions(args, out options))
            {
                return -1;
            }
            if (!CheckOptions(options))
            {
                return -2;
            }
            if (!string.IsNullOrEmpty(options.TempFile) && !CleanTemp(options.TempFile))
            {
                Console.WriteLine("Failed deleting previous temp file.");
                return -3;
            }
            if (!string.IsNullOrEmpty(options.TempFile))
            {
                if (!CopyFile(options.SourceFile, options.TempFile))
                {
                    Console.WriteLine("Failed copying source backup file to temp path.");
                    return -4;
                }
                // now that we have a copy of the source file at temp, update the source var so that it gets passed to SQL server instead of the specified source file
                options.SourceFile = options.TempFile;
            }
            if (!PrepareDatabaseFiles(options))
            {
                Console.WriteLine("Failed preparing database files.");
                return -5;
            }
            if (!RestoreDatabase(options))
            {
                Console.WriteLine("Failed restoring the database.");
                return -6;
            }
            if (!SetDatabasePermissions(options))
            {
                Console.WriteLine("Failed setting permissions for the database.");
                return -7;
            }
            if (!RunDBCC(options))
            {
                Console.WriteLine("Failed running DBCC CHECKDB.");
                return -8;
            }
            if (!string.IsNullOrEmpty(options.TempFile) && !CleanTemp(options.TempFile))
            {
                Console.WriteLine("Failed deleting temp file.");
                return -9;
            }
            Console.WriteLine("Operation Completed.");
            return 0;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  --autosource -a <mode> <path>   : select a source file from specified path based on specified mode.");
            Console.WriteLine("  --source -s <filepath>          : source database backup file.");
            Console.WriteLine("  --temp -t <filepath>            : temporary directory and file name to copy the source to before restoring.");
            Console.WriteLine("  --serverip -i <ip>              : IP address of the server to connect to.");
            Console.WriteLine("  --servername -n <name>          : name of the server to connect to.");
            Console.WriteLine("  --serverport -p <port>          : port of the server to connect to.");
            Console.WriteLine("  --database -d <name>            : name of the database to overwrite with this backup.");
            Console.WriteLine("  --username -u <username>        : username to connect to SQL server as.");
            Console.WriteLine("  --password -p <password>        : password to connect to SQL server (INSECURE)");
            Console.WriteLine("  --rights -r <userlist>          : list of users/groups and permission to grant to each.");
            Console.WriteLine("  --movefile -m <name> <filepath> : Tells SQL server to move the database file to the specified filepath when restoring.");
            Console.WriteLine("  --moveallfiles <filepath>       : Tells SQL server to move all database files to the specified path when restoring, preserving file names.");
            Console.WriteLine("  --replacedatabase               : Tells SQL server to replace the existing database with this backup.");
            Console.WriteLine("  --dbcccheckdb                   : Runs DBCC CHECKDB on the restored database to verify there is no corruption.");
            Console.WriteLine();
            Console.WriteLine("Autosource mode specifies which file to choose in the specified directory.");
            Console.WriteLine(" lastcreated : selects the newest file by creation date");
            Console.WriteLine(" lastmodified : selects the newest file by modified date");
            Console.WriteLine("Example: --autosource lastcreated c:\\backups\\database_20241231.bak");
            Console.WriteLine();
            Console.WriteLine("Users List accepts a list of users and group separated by semi-colon (;).");
            Console.WriteLine("Separate the user or group from the access modifier by a colon (:)");
            Console.WriteLine("R grants DataReader, W grants DataWriter, O grants Owner.");
            Console.WriteLine("Example: domain\\UserName:O;domain\\group:R;SQLUser:RW");
            Console.WriteLine();
            Console.WriteLine("Only specify --serverip OR --servername, not both. If neither is specified, localhost is assumed.");
            Console.WriteLine("If no --serverport is specified, 1433 is assume.");
            Console.WriteLine("If no --username and --password are specified, then integrated (SSPI) authentication will be used.");
            Console.WriteLine("If no --temp is specified, then will attempt to restore from --source directly.");
            Console.WriteLine("In that case, User account for SQL server process must have access to the source file.");
            Console.WriteLine();
            return;
        }

        private static bool ParseOptions(string[] args, out OptionsClass options)
        {
            options = new OptionsClass();
            options.UserRights = new List<UserRightItem>();
            options.MoveItems = new List<MoveItem>();
            options.MoveAllFiles = false;
            options.ReplaceDatabase = false;
            options.DbccCheckDB = false;
            int pos = 0;
            while (pos < args.Length)
            {
                if (args[pos].ToLower() == "--autosource" || args[pos].ToLower() == "-a")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("--autosource mode not specified.");
                        return false;
                    }
                    string amode = args[pos];
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("--autosource path not specified.");
                        return false;
                    }
                    options.SourcePath = args[pos];
                    pos++;
                    switch (amode.ToLower())
                    {
                        case "lastcreated":
                            options.AutoSourceMode = AutoSourceMode.lastcreated;
                            break;
                        case "lastmodified":
                            options.AutoSourceMode = AutoSourceMode.lastmodified;
                            break;
                        default:
                            Console.WriteLine(string.Format("Invalid --autosource mode specified: {0}", amode));
                            return false;
                    }
                }
                else if (args[pos].ToLower() == "--source" || args[pos].ToLower() == "-s")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("Missing Source Path.");
                        return false;
                    }
                    options.SourceFile = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--temp" || args[pos].ToLower() == "-t")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("Missing Temp File Name.");
                        return false;
                    }
                    options.TempFile = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--database" || args[pos].ToLower() == "-d")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("Missing Destination Database Name.");
                        return false;
                    }
                    options.DatabaseName = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--rights" || args[pos].ToLower() == "-r")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("Missing User Accounts to grant permission.");
                        return false;
                    }
                    options.UserRightsString = args[pos];
                    string[] tempRights = options.UserRightsString.Split(';');
                    foreach (var r in tempRights)
                    {
                        string[] tempItem = r.Split(':');
                        if (tempItem.Length != 2)
                        {
                            Console.WriteLine(string.Format("Unable to parse user permission: {0}", r));
                            return false;
                        }
                        UserRightItem newRight = new UserRightItem();
                        newRight.Name = tempItem[0];
                        if (tempItem[1].Contains('r') || tempItem[1].Contains('R'))
                        {
                            newRight.Read = true;
                        }
                        if (tempItem[1].Contains('w') || tempItem[1].Contains('W'))
                        {
                            newRight.Write = true;
                        }
                        if (tempItem[1].Contains('o') || tempItem[1].Contains('O'))
                        {
                            newRight.Owner = true;
                        }
                        options.UserRights.Add(newRight);
                    }
                    pos++;
                }
                else if (args[pos].ToLower() == "--serverip" || args[pos].ToLower() == "-i")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("Missing Server IP Address.");
                        return false;
                    }
                    IPAddress testIP = null;
                    if (IPAddress.TryParse(args[pos], out testIP))
                    {
                        options.SQLServerIP = args[pos];
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Invalid IP address specified. To override, you can use --servername instead. {0}", args[pos]));
                        return false;
                    }
                    pos++;
                }
                else if (args[pos].ToLower() == "--servername" || args[pos].ToLower() == "-n")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("Missing Server Name.");
                        return false;
                    }
                    options.SQLServerName = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--serverport" || args[pos].ToLower() == "-p")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("Missing Server Port.");
                        return false;
                    }
                    int testInt = 0;
                    if (int.TryParse(args[pos], out testInt))
                    {
                        if (testInt <= 0 || testInt > 65535)
                        {
                            Console.WriteLine(string.Format("Invalid port specified: {0}", args[pos]));
                            return false;
                        }
                        options.ServerPort = testInt;
                    }
                    pos++;
                }
                else if (args[pos].ToLower() == "--username" || args[pos].ToLower() == "-u")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("No username specified.");
                        return false;
                    }
                    options.SQLUsername = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--password" || args[pos].ToLower() == "-p")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("No password specified.");
                        return false;
                    }
                    options.SQLPassword = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--movefile" || args[pos].ToLower() == "-m")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("No Logical file specified for --movefile <name> parameter.");
                        return false;
                    }
                    string logical = args[pos];
                    pos++;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("No Physical file specified for --movefile <filepath> parameter.");
                        return false;
                    }
                    string physical = args[pos];
                    pos++;
                    options.MoveItems.Add(new MoveItem()
                    {
                        LogicalName = logical,
                        PhysicalName = physical,
                    });
                }
                else if (args[pos].ToLower() == "--moveallfiles")
                {
                    pos++;
                    options.MoveAllFiles = true;
                    if (pos >= args.Length)
                    {
                        Console.WriteLine("No path specified for --moveallfiles <filepath> parameter.");
                        return false;
                    }
                    options.MoveAllPath = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--replacedatabase")
                {
                    pos++;
                    options.ReplaceDatabase = true;
                }
                else if (args[pos].ToLower() == "--dbcccheckdb")
                {
                    pos++;
                    options.DbccCheckDB = true;
                }
                else
                {
                    Console.WriteLine(string.Format("Unrecognized command line option {0}", args[pos]));
                    return false;
                }
            }
            return true;
        }

        private static bool CheckOptions(OptionsClass options)
        {
            // verify that --autosource and --source weren't specified together
            if (!string.IsNullOrEmpty(options.SourceFile) && !string.IsNullOrEmpty(options.SourcePath))
            {
                Console.WriteLine("--autosource and --source were both specified, but only one is needed.");
                return false;
            }
            if (!string.IsNullOrEmpty(options.SourcePath) && !System.IO.Directory.Exists(options.SourcePath))
            {
                Console.WriteLine(string.Format("Source path does not exist or access denied: {0}", options.SourcePath));
                return false;
            }
            if (!string.IsNullOrEmpty(options.SourcePath))
            {
                if (options.AutoSourceMode == AutoSourceMode.None)
                {
                    Console.WriteLine("No --autosource mode was specified.");
                    return false;
                }
                options.SourceFile = null;
                DirectoryInfo dirInfo = new DirectoryInfo(options.SourcePath);
                if (dirInfo == null || !dirInfo.Exists)
                {
                    Console.WriteLine(string.Format("Source path does not exist or access denied: {0}", options.SourcePath));
                    return false;
                }
                FileInfo[] files = dirInfo.GetFiles();
                DateTime recentCreate = DateTime.MinValue;
                DateTime recentModify = DateTime.MinValue;
                FileInfo lastCreated = null;
                FileInfo lastModified = null;
                foreach (FileInfo file in files)
                {
                    if (file.CreationTime > recentCreate)
                    {
                        recentCreate = file.CreationTime;
                        lastCreated = file;
                    }
                    if (file.LastWriteTime > recentModify)
                    {
                        recentModify = file.LastWriteTime;
                        lastModified = file;
                    }
                }
                // a little redundant, but that's because I want to be able to add different filter types in the future and don't know for sure which checks I'll need.
                if (options.AutoSourceMode == AutoSourceMode.lastcreated)
                {
                    if (lastCreated == null)
                    {
                        Console.WriteLine(string.Format("Couldn't find the last created file in the directory: {0}", options.SourcePath));
                        return false;
                    }
                    else
                    {
                        options.SourceFile = lastCreated.FullName;
                        Console.WriteLine(string.Format("Autoselected newest source file: {0}", options.SourceFile));
                    }
                }
                else if (options.AutoSourceMode == AutoSourceMode.lastmodified)
                {
                    if (lastModified == null)
                    {
                        Console.WriteLine(string.Format("Couldn't find the last modified file in the directory: {0}", options.SourcePath));
                        return false;
                    }
                    else
                    {
                        options.SourceFile = lastModified.FullName;
                        Console.WriteLine(string.Format("Autoselected most recently modified source file: {0}", options.SourceFile));
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Didn't understand the --autosource mode: {0}", options.AutoSourceMode.ToString()));
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(options.SourceFile) && !System.IO.File.Exists(options.SourceFile))
            {
                Console.WriteLine(string.Format("Source file does not exist or access denied: {0}", options.SourceFile));
                return false;
            }

            if (!string.IsNullOrEmpty(options.TempFile))
            {
                if (options.TempFile.StartsWith("\\\\"))
                {
                    // this is a UNC path. 
                    Console.WriteLine("Detected temp folder is a UNC path. Skipping checks.");
                }
                else if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(options.TempFile)))
                {
                    Console.WriteLine(string.Format("Temp Directory: {0}", System.IO.Path.GetDirectoryName(options.TempFile)));
                    if (System.IO.File.Exists(options.TempFile))
                    {
                        Console.WriteLine(string.Format("Temp File exists, will replace: {0}", System.IO.Path.GetFileName(options.TempFile)));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Temp File: {0}", System.IO.Path.GetFileName(options.TempFile)));
                    }
                }
                else
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(options.TempFile));
                        Console.WriteLine(string.Format("Created temp directory specified: {0}", System.IO.Path.GetDirectoryName(options.TempFile)));
                    }
                    catch
                    {
                        Console.WriteLine(string.Format("Temp Directory does not exist: {0}", System.IO.Path.GetDirectoryName(options.TempFile)));
                        return false;
                    }
                }
            }
            else
            {
                Console.WriteLine("No temp file specified. Will attempt to restore directly from source file.");
            }
            if (string.IsNullOrEmpty(options.SQLServerIP) && string.IsNullOrEmpty(options.SQLServerName))
            {
                Console.WriteLine("No SQL Server specified. Assuming localhost.");
                options.SQLServerName = "localhost";
            }
            else if (!string.IsNullOrEmpty(options.SQLServerIP) && !string.IsNullOrEmpty(options.SQLServerName))
            {
                Console.WriteLine("Don't specify both --servername and --serverip. Use one or the other.");
                return false;
            }
            else if (!string.IsNullOrEmpty(options.SQLServerIP))
            {
                Console.WriteLine(string.Format("SQL Server IP: {0}", options.SQLServerIP));
                // ip address validation was already done, so we can just copy the value to ServerName to simplify building the connection string.
                options.SQLServerName = options.SQLServerIP;
            }
            else if (!string.IsNullOrEmpty(options.SQLServerName))
            {
                Console.WriteLine(string.Format("SQL Server Name: {0}", options.SQLServerName));
            }
            if (options.ServerPort != 1433)
            {
                Console.WriteLine(string.Format("SQL server port: {0}", options.ServerPort));
            }
            if (string.IsNullOrEmpty(options.DatabaseName))
            {
                Console.WriteLine("No Database name was specified.");
                return false;
            }
            else
            {
                Console.WriteLine(string.Format("Restoring (with replace) database: [{0}]", options.DatabaseName));
            }
            if (string.IsNullOrEmpty(options.SQLUsername) && string.IsNullOrEmpty(options.SQLPassword))
            {
                options.SSPI = true;
            }
            else
            {
                options.SSPI = false;
                if (string.IsNullOrEmpty(options.SQLUsername))
                {
                    Console.WriteLine("Missing SQL username. If specifying credentials, specify both --username and --password.");
                    return false;
                }
                if (string.IsNullOrEmpty(options.SQLPassword))
                {
                    Console.WriteLine("Missing SQL password. If specifying credentials, specify both --username and --password.");
                    return false;
                }
            }

            if (options.UserRights.Count > 0)
            {
                Console.WriteLine("Restoring access rights for users:");
                foreach (var userRight in options.UserRights)
                {
                    Console.Write("   ");
                    Console.Write(userRight.Name);
                    Console.Write(": ");
                    if (userRight.Read)
                    {
                        Console.Write("Read ");
                    }
                    if (userRight.Write)
                    {
                        Console.Write("Write ");
                    }
                    if (userRight.Owner)
                    {
                        Console.Write("Owner");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No user rights specified to create.");
            }
            if (options.ReplaceDatabase)
            {
                Console.WriteLine(string.Format("Database [{0}] will be restore WITH REPLACE option, overwritting any existing database.", options.DatabaseName));
            }
            if (options.MoveAllFiles)
            {
                if (options.MoveAllPath.EndsWith("\""))
                {
                    options.MoveAllPath = options.MoveAllPath.Substring(0, options.MoveAllPath.Length - 1);
                }
                if (options.MoveItems.Count == 0)
                {
                    Console.WriteLine(string.Format("Backup set will be queried and all database files will be moved to \"{0}\"", options.MoveAllPath));
                }
                else
                {
                    Console.WriteLine(string.Format("Backup set will be queried and any database files not listed below will be moved to \"{0}\"", options.MoveAllPath));
                    Console.WriteLine("These files (if they exist) will be moved to:");
                    foreach (var item in options.MoveItems)
                    {
                        Console.WriteLine(string.Format(" '{0}' -> \"{1}\"", item.LogicalName, item.PhysicalName));
                    }
                }
            }
            else
            {
                if (options.MoveItems.Count > 0)
                {
                    Console.WriteLine("Backup set will be queried and the following database files will be moved (if they exist):");
                    foreach (var item in options.MoveItems)
                    {
                        Console.WriteLine(string.Format(" '{0}' -> \"{1}\"", item.LogicalName, item.PhysicalName));
                    }
                }
                else
                {
                    Console.WriteLine("Database files will be restored to their original location. If this fails, try the --movefile or --moveallfiles options.");
                }
            }
            if (options.DbccCheckDB)
            {
                Console.WriteLine("After restoring, run DBCC CHECKDB to verify no corruption is present.");
            }
            return true;
        }

        private static bool CleanTemp(string tempPath)
        {
            if (System.IO.File.Exists(tempPath))
            {
                try
                {
                    System.IO.File.Delete(tempPath);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CopyFile(string sourceFile, string tempFile)
        {
            if (System.IO.File.Exists(sourceFile))
            {
                try
                {
                    Console.WriteLine(string.Format("Copying file \"{0}\" to \"{1}\"", sourceFile, tempFile));
                    System.IO.File.Copy(sourceFile, tempFile);
                    Console.WriteLine("Copy successful.");
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private static string BuildConnectionString(OptionsClass options, bool useMasterDB = false)
        {
            // connect with master as default database, as we're going to overwrite the database.
            string SQLConnectionString = null;
            if (options.ServerPort == 1433)
            {
                SQLConnectionString = string.Format("Data Source={0};", options.SQLServerName);
            }
            else
            {
                SQLConnectionString = string.Format("Data Source={0},{1};", options.SQLServerName, options.ServerPort);
            }
            SQLConnectionString += string.Format("Initial Catalog={0};", useMasterDB ? "master" : options.DatabaseName);
            if (options.SSPI)
            {
                SQLConnectionString += "Trusted_Connection=Yes;";
            }
            else
            {
                SQLConnectionString += string.Format("User ID={0};Password={1};", options.SQLUsername, options.SQLPassword);
            }
            return SQLConnectionString;
        }

        /// <summary>
        /// If --moveallfiles or -movefile is specified, query the database with RESTORE FILELISTONLY to 
        /// prepare a list of WITH MOVE items for later.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static bool PrepareDatabaseFiles(OptionsClass options)
        {
            if (options.MoveAllFiles || options.MoveItems.Count > 0)
            {
                string SQLConnectionString = BuildConnectionString(options, useMasterDB: true);
                List<MoveItem> moveItems = new List<MoveItem>();
                Console.WriteLine("Preparing to query SQL server for backup set file list.");
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    string query = "RESTORE FILELISTONLY FROM DISK=\'" + options.SourceFile + "\'";
                    try
                    {
                        Console.Write("Opening connection to SQL server... ");
                        connection.Open();
                        Console.WriteLine("Successful");
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            Console.Write("Getting file list... ");
                            SqlDataReader reader = cmd.ExecuteReader();
                            Console.WriteLine("Successful");
                            while (reader.Read())
                            {
                                string logicalName = reader.GetString(0);
                                string physicalName = reader.GetString(1);
                                bool found = false;
                                foreach (var optItem in options.MoveItems)
                                {
                                    if (string.Compare(optItem.LogicalName, logicalName, StringComparison.InvariantCultureIgnoreCase) == 0)
                                    {
                                        found = true;
                                        moveItems.Add(new MoveItem()
                                        {
                                            LogicalName = logicalName,
                                            PhysicalName = optItem.PhysicalName
                                        });
                                        break;
                                    }
                                }
                                if (!found && options.MoveAllFiles)
                                {
                                    string oldFileName = System.IO.Path.GetFileName(physicalName);
                                    physicalName = Path.Combine(options.MoveAllPath, oldFileName);
                                    moveItems.Add(new MoveItem()
                                    {
                                        LogicalName = logicalName,
                                        PhysicalName = physicalName
                                    });
                                    found = true;
                                }
                                if (found)
                                {
                                    Console.WriteLine(string.Format("Database file {0} will be moved to {1}", logicalName, physicalName));
                                }
                            }
                        }
                        options.MoveItems = moveItems;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An exception occurred querying the backup files.");
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
            }
            return true;
        }
        private static bool RestoreDatabase(OptionsClass options)
        {
            string SQLConnectionString = BuildConnectionString(options, useMasterDB: true);
            Console.WriteLine("Preparing to restore SQL Database.");
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                bool WithAdded = false;
                string query = "RESTORE DATABASE [" + options.DatabaseName + "] \r\n" +
                    "  FROM DISK = \'" + options.SourceFile + "\'\r\n";
                if (options.ReplaceDatabase)
                {
                    WithAdded = true;
                    query += "  WITH REPLACE";
                }
                foreach (var mi in options.MoveItems)
                {
                    if (WithAdded)
                    {
                        query += string.Format(",\r\n  MOVE \'{0}\' TO \'{1}\'", mi.LogicalName, mi.PhysicalName);
                    }
                    else
                    {
                        query += string.Format("  WITH MOVE \'{0}\' TO \'{1}\'", mi.LogicalName, mi.PhysicalName);
                        WithAdded = true;
                    }
                }
                query += ";";
                try
                {
                    Console.Write("Opening connection to SQL server... ");
                    connection.Open();
                    Console.WriteLine("Successful");
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        Console.Write("Restoring database... ");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Successful");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An exception occurred restoring the database");
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        private static bool SetDatabasePermissions(OptionsClass options)
        {
            if (options.UserRights.Count > 0)
            {
                string SQLConnectionString = BuildConnectionString(options);
                Console.WriteLine("Preparing to restore user rights.");
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    try
                    {
                        string query = "";
                        Console.Write("Opening connection to SQL server... ");
                        connection.Open();
                        Console.WriteLine("Successful");

                        foreach (var user in options.UserRights)
                        {
                            // create the user
                            query = string.Format("CREATE USER [{0}] FOR LOGIN [{0}]", user.Name);
                            using (SqlCommand cmd = new SqlCommand(query, connection))
                            {
                                Console.Write(string.Format("Creating login for user [{0}]... ", user.Name));
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Successful");
                            }
                            if (user.Read)
                            {
                                query = string.Format("ALTER ROLE [db_datareader] ADD MEMBER [{0}]", user.Name);
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    Console.Write(string.Format("Granting db_datareader to [{0}]... ", user.Name));
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Successful");
                                }
                            }
                            if (user.Write)
                            {
                                query = string.Format("ALTER ROLE [db_datawriter] ADD MEMBER [{0}]", user.Name);
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    Console.Write(string.Format("Granting db_datawriter to [{0}]... ", user.Name));
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Successful");
                                }
                            }
                            if (user.Owner)
                            {
                                query = string.Format("ALTER ROLE [db_owner] ADD MEMBER [{0}]", user.Name);
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    Console.Write(string.Format("Granting db_owner to [{0}]... ", user.Name));
                                    cmd.ExecuteNonQuery();
                                    Console.WriteLine("Successful");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An exception occurred assigning roles.");
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
            }
            return true;
        }
    
        private static bool RunDBCC(OptionsClass options)
        {
            if (options.DbccCheckDB)
            {
                string SQLConnectionString = BuildConnectionString(options);
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    connection.InfoMessage += Connection_InfoMessage;
                    try
                    {
                        string query = "DBCC CHECKDB";
                        Console.Write("Opening connection to SQL server... ");
                        connection.Open();
                        Console.WriteLine("Successful");
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            Console.Write("Running DBCC CHECKEDB... ");
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Successful. See Info messages below for any DBCC errors reported.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error:");
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                    connection.InfoMessage -= Connection_InfoMessage;
                    Console.WriteLine("Information Messages from SQL Server:");
                    Console.WriteLine(InfoMessageSB.ToString());
                    InfoMessageSB.Clear();
                } 
            }
            return true;
        }

        private static StringBuilder InfoMessageSB = new StringBuilder();
        private static void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            InfoMessageSB.AppendLine(e.Message);
        }
    }
}
