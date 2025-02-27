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
using Microsoft.SqlServer.Server;
using System.Diagnostics;
using System.Security.Policy;
using System.Reflection;
using System.Xml.Serialization;
using System.Diagnostics.SymbolStore;
using System.Reflection.Emit;
using static DatabaseRestore.Program;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Runtime.Remoting.Messaging;

namespace DatabaseRestore
{
    public class Program
    {
        public const string BUILDRELEASE = "alpha1";
        public const int KEYITERATIONS = 1000;

        public class UserRightItem
        {
            public string Name { get; set; }
            public bool Read { get; set; }
            public bool Write { get; set; }
            public bool Owner { get; set; }
        }

        public class MoveItem
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

        public class OptionsClass
        {
            public OptionsClass()
            {
                UserRights = new List<UserRightItem>();
                MoveItems = new List<MoveItem>();
                MoveAllFiles = false;
                ReplaceDatabase = false;
                DbccCheckDB = false;
                EncryptSQL = false;
                TrustServerCert = false;
                SMTPPassword = "";
            }
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
            // --encrypt
            public bool EncryptSQL {  get; set; }
            // --trustservercert
            public bool TrustServerCert { get; set; }
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
            // --logfile
            public string LogFile { get; set; }
            // --logappend
            public string LogAppend { get; set; }
            // --smtpprofile
            public string SMTPProfile { get; set; }
            // --smtppassword
            public string SMTPPassword { get; set; }
        }

        public class SMTPProfileClass
        {
            public string SMTPServer { get; set; }
            public int Port { get; set; }
            public bool TLS { get; set; }
            public bool RequireAuth { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string EmailFrom { get; set; }
            public string EmailTo { get; set; }
            public string EmailSubjectTemplate { get; set; }
            public string EmailBodyTemplte { get; set; }
            public bool AttachLog { get; set; }
        }

        /*
        private static void TestCrypto()
        {
            string Loremipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
            MemoryStream TestStream = new MemoryStream();
            byte[] testBuffer = Encoding.UTF8.GetBytes(Loremipsum);
            TestStream.Write(testBuffer, 0, testBuffer.Length);
            TestStream.Seek(0, SeekOrigin.Begin);

            if (!EncryptAndSaveFile(@"C:\temp\testfile.XSF", "hijkelemenop", TestStream))
            {
                Console.WriteLine("Failed to Encrypt.");
                return;
            }

            MemoryStream result = LoadAndDecryptFile(@"C:\temp\testfile.XSF", "hijkelemenop");
            byte[] decryptBuff = result.ToArray();
            string decryptString = Encoding.UTF8.GetString(decryptBuff);
            if (decryptString == Loremipsum)
            {
                Console.WriteLine("SUCCESS!");
            }
            else
            {
                Console.WriteLine("Data mismatch.");
            }
        }*/

        static int Main(string[] args)
        {
            StartTime = DateTime.Now;
            LogString(ProgramNameVersionString);
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
            int result = RunJob(options);               

            return result;
        }

        public static int RunJob(OptionsClass options)
        {
            try
            { 
                if (!CheckOptions(options))
                {
                    return -2;
                }
                if (!string.IsNullOrEmpty(options.TempFile) && !CleanTemp(options.TempFile))
                {
                    LogString("Failed deleting previous temp file.");
                    return -3;
                }
                if (!string.IsNullOrEmpty(options.TempFile))
                {
                    if (!CopyFile(options.SourceFile, options.TempFile))
                    {
                        LogString("Failed copying source backup file to temp path.");
                        return -4;
                    }
                    // now that we have a copy of the source file at temp, update the source var so that it gets passed to SQL server instead of the specified source file
                    options.SourceFile = options.TempFile;
                }
                if (!PrepareDatabaseFiles(options))
                {
                    LogString("Failed preparing database files.");
                    return -5;
                }
                if (!RestoreDatabase(options))
                {
                    LogString("Failed restoring the database.");
                    return -6;
                }
                if (!SetDatabasePermissions(options))
                {
                    LogString("Failed setting permissions for the database.");
                    return -7;
                }
                if (!RunDBCC(options))
                {
                    LogString("Failed running DBCC CHECKDB.");
                    return -8;
                }
                if (!string.IsNullOrEmpty(options.TempFile) && !CleanTemp(options.TempFile))
                {
                    LogString("Failed deleting temp file.");
                    return -9;
                }
                LogString("Operation Completed.");
            }
            catch (Exception ex)
            {
                LogString(string.Format("An excption occurred: {0}", ex.Message));
            }
            finally
            {
                EndTime = DateTime.Now;
                WriteLogs(options);
            }
            return 0;
        }


        public static StringBuilder LogOutput = new StringBuilder();

        public static DateTime StartTime;
        public static DateTime EndTime;

        public static void LogString(string entry, bool NewLine = true)
        {
            LogOutput.Append(entry);
            Console.Write(entry);
            if (NewLine)
            {
                LogOutput.Append("\r\n");
                Console.WriteLine();
            }
        }
        public static string ProgramNameVersionString
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                var name = assembly.GetName();
                return string.Format("{0} v{1}.{2}-{3}", name.Name, name.Version.Major, name.Version.Minor, BUILDRELEASE);
            }
        }

        public static string ProgramVersionString
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                var name = assembly.GetName();
                return string.Format("{0}.{1}-{2}", name.Version.Major, name.Version.Minor, BUILDRELEASE);
            }
        }

        public static void WriteLogs(OptionsClass options)
        {
            if (options == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(options.LogFile))
            {
                try
                {
                    using (TextWriter writer = File.CreateText(options.LogFile))
                    {
                        writer.WriteLine("############################################################################################");
                        writer.WriteLine(string.Format("Starting process at {0:F}", StartTime));
                        writer.Write(LogOutput.ToString());
                        writer.WriteLine(string.Format("Ending process at {0:F}", EndTime));
                        writer.WriteLine("############################################################################################");
                        writer.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to write out log file.");
                    Console.WriteLine(ex.Message);
                }
            }
            if (!string.IsNullOrEmpty(options.LogAppend))
            {
                try
                {
                    using (TextWriter writer = File.AppendText(options.LogAppend))
                    {
                        writer.WriteLine("############################################################################################");
                        writer.WriteLine(string.Format("Starting process at {0:F}", StartTime));
                        writer.Write(LogOutput.ToString());
                        writer.WriteLine(string.Format("Ending process at {0:F}", EndTime));
                        writer.WriteLine("############################################################################################");
                        writer.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to write out append log file.");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  --loadsettings <path>           : Loads settings from a settings file (created with the GUI).");
            Console.WriteLine("  --settingspassword <password>   : Specifies a password to decrypt the --loadsettings file, only specify if that file is password protected.");
            Console.WriteLine("  --autosource -a <mode> <path>   : select a source file from specified path based on specified mode.");
            Console.WriteLine("  --source -s <filepath>          : source database backup file.");
            Console.WriteLine("  --temp -t <filepath>            : temporary directory and file name to copy the source to before restoring.");
            Console.WriteLine("  --serverip -i <ip>              : IP address of the server to connect to.");
            Console.WriteLine("  --servername -n <name>          : name of the server to connect to.");
            Console.WriteLine("  --serverport -p <port>          : port of the server to connect to.");
            Console.WriteLine("  --encrypt -e                    : Force encryption of the SQL connection.");
            Console.WriteLine("  --trustservercert -t            : Trust the server's certificate. Required to connect for self-signed or untrusted certs.");
            Console.WriteLine("  --database -d <name>            : name of the database to overwrite with this backup.");
            Console.WriteLine("  --username -u <username>        : username to connect to SQL server as.");
            Console.WriteLine("  --password -p <password>        : password to connect to SQL server (INSECURE)");
            Console.WriteLine("  --rights -r <userlist>          : list of users/groups and permission to grant to each.");
            Console.WriteLine("  --movefile -m <name> <filepath> : Tells SQL server to move the database file to the specified filepath when restoring.");
            Console.WriteLine("  --moveallfiles <filepath>       : Tells SQL server to move all database files to the specified path when restoring, preserving file names.");
            Console.WriteLine("  --replacedatabase               : Tells SQL server to replace the existing database with this backup.");
            Console.WriteLine("  --dbcccheckdb                   : Runs DBCC CHECKDB on the restored database to verify there is no corruption.");
            Console.WriteLine("  --logfile <filepath>            : Writes log output to the specified file, overwriting the file if it exists.");
            Console.WriteLine("  --logappend <filepath>          : Appends a new log entry to the end of the specified file, or creates one if it doesn't exist.");
            Console.WriteLine("  --smtpprofile <filepath>        : Load a SMTP profile file in order to send an email of the log.");
            Console.WriteLine("  --smtppassword <password>       : Password to decrypt the SMTP Profile file.");
            Console.WriteLine();
            Console.WriteLine("If --loadsettings is specified, any command line arguments provided override the settings in the file.");
            Console.WriteLine("If the settings file is password protected, use the --settingspassword argument to specify the password to decrypt it.");
            Console.WriteLine("It is still insecure to pass password through command line arguments, so if an attacker has access to the computer, then");
            Console.WriteLine("they can capture the command line argument and decrypt the file themselves. For best security, us integrated authentication");
            Console.WriteLine("for SQL server so that no SQL server password has to be saved or passed through command line anywhere.");
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
            Console.WriteLine("SMTP Profile encapsulates settings and email template for sending an email log. Use the GUI to create one of these.");
            return;
        }

        public static bool PathContainsIllegalChars(string path)
        {
            char[] osillegal = Path.GetInvalidFileNameChars();
            // we need to remove the slashes and colon from this list, because while those aren't legal for a file name, they are legal for a path, which is what we are testing.
            char[] chars = osillegal.Where(c => c != '\\' && c != '/' && c != ':').ToArray();
            return path.Any(chars.Contains);
        }

        public static bool ParseOptions(string[] args, out OptionsClass options)
        {
            options = null;
            // first, scan the arguments to see if a settings file is specified.  If so, load that, then parse other arguments to override the settings file.
            int pos = 0;
            string SettingsFile = null;
            string SettingsPassword = "";
            while (pos < args.Length)
            {
                if (args[pos].ToLower() == "--loadsettings")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("--loadsettings file not specified.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--loadsettings path argument contains invalid characters.");
                        return false;
                    }
                    if (!string.IsNullOrEmpty(SettingsFile))
                    {
                        LogString("Only one --loadsettings argument is allowed.");
                        return false;
                    }
                    SettingsFile = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--settingspassword")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("--settingspassword password not specified.");
                        return false;
                    }
                    if (!string.IsNullOrEmpty(SettingsPassword))
                    {
                        LogString("Only one --settingspassword argument is allowed.");
                        return false;
                    }
                    SettingsPassword = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--autosource" || args[pos].ToLower() == "-a")
                {
                    pos+= 3;
                }
                else if (args[pos].ToLower() == "--source" || args[pos].ToLower() == "-s")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--temp" || args[pos].ToLower() == "-t")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--database" || args[pos].ToLower() == "-d")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--rights" || args[pos].ToLower() == "-r")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--serverip" || args[pos].ToLower() == "-i")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--servername" || args[pos].ToLower() == "-n")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--serverport" || args[pos].ToLower() == "-p")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--username" || args[pos].ToLower() == "-u")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--password" || args[pos].ToLower() == "-p")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--encrypt" || args[pos].ToLower() == "-e")
                {
                    pos++;
                }
                else if (args[pos].ToLower() == "--trustservercert" || args[pos].ToLower() == "-t")
                {
                    pos++;
                }
                else if (args[pos].ToLower() == "--movefile" || args[pos].ToLower() == "-m")
                {
                    pos += 3;
                }
                else if (args[pos].ToLower() == "--moveallfiles")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--replacedatabase")
                {
                    pos++;
                }
                else if (args[pos].ToLower() == "--dbcccheckdb")
                {
                    pos++;
                }
                else if (args[pos].ToLower() == "--logfile")
                {
                    pos+= 2;
                }
                else if (args[pos].ToLower() == "--logappend")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--smtpprofile")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--smtppassword")
                {
                    pos += 2;
                }
                else
                {
                    LogString(string.Format("Unrecognized command line option {0}", args[pos]));
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(SettingsFile))
            {
                options = LoadOptionsFile(SettingsFile, SettingsPassword);
                if (options == null)
                {
                    LogString("An error occurred processing the --loadsettings file. Verify the file exists and that the password is correct.");
                    return false;
                }
            }
            else
            {
                options = new OptionsClass();
            }
            pos = 0;

            while (pos < args.Length)
            {
                if (args[pos].ToLower() == "--autosource" || args[pos].ToLower() == "-a")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("--autosource mode not specified.");
                        return false;
                    }
                    string amode = args[pos];
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("--autosource path not specified.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--autosource path parameter contains invalid characters.");
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
                            LogString(string.Format("Invalid --autosource mode specified: {0}", amode));
                            return false;
                    }
                }
                else if (args[pos].ToLower() == "--source" || args[pos].ToLower() == "-s")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("Missing Source Path.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--source path parameter contains invalid characters.");
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
                        LogString("Missing Temp File Name.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--temp path parameter contains invalid characters.");
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
                        LogString("Missing Destination Database Name.");
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
                        LogString("Missing User Accounts to grant permission.");
                        return false;
                    }
                    options.UserRightsString = args[pos];
                    string[] tempRights = options.UserRightsString.Split(';');
                    foreach (var r in tempRights)
                    {
                        string[] tempItem = r.Split(':');
                        if (tempItem.Length != 2)
                        {
                            LogString(string.Format("Unable to parse --rights user permission: {0}", r));
                            return false;
                        }
                        if (tempItem[0].Contains(']'))
                        {
                            LogString(string.Format("--rights user account {0} must not contain the ] character. ", r));
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
                        LogString("Missing Server IP Address.");
                        return false;
                    }
                    IPAddress testIP = null;
                    if (IPAddress.TryParse(args[pos], out testIP))
                    {
                        options.SQLServerIP = args[pos];
                    }
                    else
                    {
                        LogString(string.Format("Invalid IP address specified. To override, you can use --servername instead. {0}", args[pos]));
                        return false;
                    }
                    pos++;
                }
                else if (args[pos].ToLower() == "--servername" || args[pos].ToLower() == "-n")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("Missing Server Name.");
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
                        LogString("Missing Server Port.");
                        return false;
                    }
                    int testInt = 0;
                    if (int.TryParse(args[pos], out testInt))
                    {
                        if (testInt <= 0 || testInt > 65535)
                        {
                            LogString(string.Format("Invalid port specified: {0}", args[pos]));
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
                        LogString("No username specified.");
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
                        LogString("No password specified.");
                        return false;
                    }
                    options.SQLPassword = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--encrypt" || args[pos].ToLower() == "-e")
                {
                    pos++;
                    options.EncryptSQL = true;
                }
                else if (args[pos].ToLower() == "--trustservercert" || args[pos].ToLower() == "-t")
                {
                    pos++;
                    options.TrustServerCert = true;
                }
                else if (args[pos].ToLower() == "--movefile" || args[pos].ToLower() == "-m")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("No Logical file specified for --movefile <name> parameter.");
                        return false;
                    }
                    string logical = args[pos];
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("No Physical file specified for --movefile <filepath> parameter.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--movefile <filepath> parameter contains invalid characters.");
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
                        LogString("No path specified for --moveallfiles <filepath> parameter.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--moveallfiles <filepath> parameter contains invalid characters.");
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
                else if (args[pos].ToLower() == "--logfile")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("No path specified for --logfile <filepath> parameter.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--logfile <filepath> parameter contains invalid characters.");
                        return false;
                    }
                    options.LogFile = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--logappend")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("No path specified for --logappend <filepath> parameter.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--logappend <filepath> parameter contains invalid characters.");
                        return false;
                    }
                    options.LogAppend = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--smtpprofile")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("No path specified for --smtpprofile <filepath> parameter.");
                        return false;
                    }
                    if (PathContainsIllegalChars(args[pos]))
                    {
                        LogString("--smtpprofile <filepath> parameter contains invalid characters.");
                        return false;
                    }
                    options.SMTPProfile = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--smtppassword")
                {
                    pos++;
                    if (pos >= args.Length)
                    {
                        LogString("No password specified for --smtppassword <password> parameter.");
                        return false;
                    }
                    options.SMTPPassword = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--loadsettings")
                {
                    pos += 2;
                }
                else if (args[pos].ToLower() == "--settingspassword")
                {
                    pos += 2;
                }
                else
                {
                    LogString(string.Format("Unrecognized command line option {0}", args[pos]));
                    return false;
                }
            }
            return true;
        }

        public static bool CheckOptions(OptionsClass options)
        {
            if (!string.IsNullOrEmpty(options.SourceFile) && !string.IsNullOrEmpty(options.SourcePath))
            {
                LogString("--autosource and --source were both specified, but only one is needed.");
                return false;
            }
            if (!string.IsNullOrEmpty(options.SourcePath) && !System.IO.Directory.Exists(options.SourcePath))
            {
                LogString(string.Format("Source path does not exist or access denied: {0}", options.SourcePath));
                return false;
            }
            LogString("Preparing processing plan and checking options.");
            if (!string.IsNullOrEmpty(options.SourcePath))
            {
                if (options.AutoSourceMode == AutoSourceMode.None)
                {
                    LogString("No --autosource mode was specified.");
                    return false;
                }
                options.SourceFile = null;
                DirectoryInfo dirInfo = new DirectoryInfo(options.SourcePath);
                if (dirInfo == null || !dirInfo.Exists)
                {
                    LogString(string.Format("Source path does not exist or access denied: {0}", options.SourcePath));
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
                        LogString(string.Format("Couldn't find the last created file in the directory: {0}", options.SourcePath));
                        return false;
                    }
                    else
                    {
                        options.SourceFile = lastCreated.FullName;
                        LogString(string.Format("Autoselected newest source file: {0}", options.SourceFile));
                    }
                }
                else if (options.AutoSourceMode == AutoSourceMode.lastmodified)
                {
                    if (lastModified == null)
                    {
                        LogString(string.Format("Couldn't find the last modified file in the directory: {0}", options.SourcePath));
                        return false;
                    }
                    else
                    {
                        options.SourceFile = lastModified.FullName;
                        LogString(string.Format("Autoselected most recently modified source file: {0}", options.SourceFile));
                    }
                }
                else
                {
                    LogString(string.Format("Didn't understand the --autosource mode: {0}", options.AutoSourceMode.ToString()));
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(options.SourceFile) && !System.IO.File.Exists(options.SourceFile))
            {
                LogString(string.Format("Source file does not exist or access denied: {0}", options.SourceFile));
                return false;
            }

            if (!string.IsNullOrEmpty(options.TempFile))
            {
                if (options.TempFile.StartsWith("\\\\"))
                {
                    // this is a UNC path. 
                    LogString(String.Format("Detected temp folder is a UNC path. Skipping filepath checks: {0}", options.TempFile));
                }
                else if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(options.TempFile)))
                {
                    LogString(string.Format("Temp Directory: {0}", System.IO.Path.GetDirectoryName(options.TempFile)));
                    if (System.IO.File.Exists(options.TempFile))
                    {
                        LogString(string.Format("Temp File exists, will replace: {0}", System.IO.Path.GetFileName(options.TempFile)));
                    }
                    else
                    {
                        LogString(string.Format("Temp File: {0}", System.IO.Path.GetFileName(options.TempFile)));
                    }
                }
                else
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(options.TempFile));
                        LogString(string.Format("Created temp directory specified: {0}", System.IO.Path.GetDirectoryName(options.TempFile)));
                    }
                    catch
                    {
                        LogString(string.Format("Temp Directory does not exist: {0}", System.IO.Path.GetDirectoryName(options.TempFile)));
                        return false;
                    }
                }
            }
            else
            {
                LogString("No temp file specified. Will attempt to restore directly from source file.");
            }
            if (string.IsNullOrEmpty(options.SQLServerIP) && string.IsNullOrEmpty(options.SQLServerName))
            {
                LogString("No SQL Server specified. Assuming localhost.");
                options.SQLServerName = "localhost";
            }
            else if (!string.IsNullOrEmpty(options.SQLServerIP) && !string.IsNullOrEmpty(options.SQLServerName))
            {
                LogString("Don't specify both --servername and --serverip. Use one or the other.");
                return false;
            }
            else if (!string.IsNullOrEmpty(options.SQLServerIP))
            {
                LogString(string.Format("SQL Server IP: {0}", options.SQLServerIP));
                // ip address validation was already done, so we can just copy the value to ServerName to simplify building the connection string.
                options.SQLServerName = options.SQLServerIP;
            }
            else if (!string.IsNullOrEmpty(options.SQLServerName))
            {
                LogString(string.Format("SQL Server Name: {0}", options.SQLServerName));
            }
            if (options.ServerPort != 1433)
            {
                LogString(string.Format("SQL server port: {0}", options.ServerPort));
            }
            if (options.EncryptSQL)
            {
                LogString("SQL server encrypted connection set to forced.");   
            }
            if (options.TrustServerCert)
            {
                LogString("SQL server certificate will be trusted (allow self-signed).");
            }
            if (string.IsNullOrEmpty(options.DatabaseName))
            {
                LogString("No Database name was specified.");
                return false;
            }
            else
            {
                LogString(string.Format("Database to restore/replace: [{0}]", options.DatabaseName));
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
                    LogString("Missing SQL username. If specifying credentials, specify both --username and --password.");
                    return false;
                }
                if (string.IsNullOrEmpty(options.SQLPassword))
                {
                    LogString("Missing SQL password. If specifying credentials, specify both --username and --password.");
                    return false;
                }
            }

            if (options.UserRights.Count > 0)
            {
                LogString("Access rights to set for users:");
                foreach (var userRight in options.UserRights)
                {
                    LogString("   ", NewLine: false);
                    LogString(userRight.Name, NewLine: false);
                    LogString(": ", NewLine: false);
                    bool roleAssigned = userRight.Read || userRight.Write || userRight.Owner;
                    if (!roleAssigned)
                    {
                        LogString("No Roles have been selected. Check Role assignments and try again.");
                        return false;
                    }
                    if (userRight.Read)
                    {
                        LogString("Read ", NewLine: false);
                    }
                    if (userRight.Write)
                    {
                        LogString("Write ", NewLine: false);
                    }
                    if (userRight.Owner)
                    {
                        LogString("Owner", NewLine: false);
                    }
                    LogString("");
                }
            }
            else
            {
                LogString("No user rights specified to create.");
            }
            // check logins to make sure they exist on the target sql server.
            List<string> logins;
            try
            {
                LogString("Connecting to SQL server to get a list of logins...", NewLine: false);
                logins = GetSQLLogins(options);
                LogString("Successful.");
                foreach (var userRight in options.UserRights)
                {
                    if (!logins.Contains(userRight.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        LogString(String.Format("The Login {0} does not exist on the SQL server.", userRight.Name));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogString(string.Format("Unable to connect to SQL server to verify logins. Exception: {0}", ex.Message));
                return false;
            }
            if (options.ReplaceDatabase)
            {
                LogString(string.Format("Database [{0}] will be restore WITH REPLACE option, overwriting any existing database.", options.DatabaseName));
            }
            if (options.MoveAllFiles)
            {
                if (options.MoveAllPath.EndsWith("\""))
                {
                    options.MoveAllPath = options.MoveAllPath.Substring(0, options.MoveAllPath.Length - 1);
                }
                if (options.MoveItems.Count == 0)
                {
                    LogString(string.Format("Backup set will be queried and all database files will be moved to \"{0}\"", options.MoveAllPath));
                }
                else
                {
                    LogString(string.Format("Backup set will be queried and any database files not listed below will be moved to \"{0}\"", options.MoveAllPath));
                    LogString("These files (if they exist) will be moved to:");
                    foreach (var item in options.MoveItems)
                    {
                        LogString(string.Format(" '{0}' -> \"{1}\"", item.LogicalName, item.PhysicalName));
                    }
                }
            }
            else
            {
                if (options.MoveItems.Count > 0)
                {
                    LogString("Backup set will be queried and the following database files will be moved (if they exist):");
                    foreach (var item in options.MoveItems)
                    {
                        LogString(string.Format(" '{0}' -> \"{1}\"", item.LogicalName, item.PhysicalName));
                    }
                }
                else
                {
                    LogString("Database files will be restored to their original location. If this fails, try the --movefile or --moveallfiles options.");
                }
            }
            if (options.DbccCheckDB)
            {
                LogString("After restoring, run DBCC CHECKDB to verify no corruption is present.");
            }
            return true;
        }

        public static bool CleanTemp(string tempPath)
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

        public static bool CopyFile(string sourceFile, string tempFile)
        {
            if (System.IO.File.Exists(sourceFile))
            {
                try
                {
                    LogString(string.Format("Copying file \"{0}\" to \"{1}\"", sourceFile, tempFile));
                    System.IO.File.Copy(sourceFile, tempFile);
                    LogString("Copy successful.");
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static string BuildConnectionString(OptionsClass options, bool useMasterDB = false)
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
            if (options.EncryptSQL)
            {
                SQLConnectionString += "Encrypt=True;";
            }
            if (options.TrustServerCert)
            {
                SQLConnectionString += "TrustServerCertificate=True;";
            }
            return SQLConnectionString;
        }

        public static List<string> GetUserDatabases(OptionsClass options)
        {
            List<string> databases = new List<string>();
            string SQLConnectionString = BuildConnectionString(options, useMasterDB: true);
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                string query = "SELECT name FROM master.dbo.sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        databases.Add(reader.GetString(0));
                    }
                }
            }
            return databases;
        }

        public static List<MoveItem> GetDatabaseFiles(OptionsClass options)
        {
            string SQLConnectionString = BuildConnectionString(options, useMasterDB: true);
            List<MoveItem> moveItems = new List<MoveItem>();
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                StringBuilder querysb = new StringBuilder();
                querysb.Append("SELECT mf.Name, mf.physical_name FROM sys.master_files mf INNER JOIN sys.databases db ON db.database_id = mf.database_id WHERE db.name = @DBNAME;");
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(querysb.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@DBNAME", options.DatabaseName);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string logicalName = reader.GetString(0);
                        string physicalName = reader.GetString(1);
                        moveItems.Add(new MoveItem()
                        {
                            LogicalName = logicalName,
                            PhysicalName = physicalName
                        });
                    }
                }
            }
            return moveItems;
        }

        public static List<string> GetSQLLogins(OptionsClass options)
        {
            string SQLConnectionString = BuildConnectionString(options, useMasterDB: true);
            List<string> logins = new List<string>();
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                StringBuilder querysb = new StringBuilder();
                querysb.Append("SELECT name FROM syslogins WHERE name NOT LIKE '##%';");
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(querysb.ToString(), connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        logins.Add(reader.GetString(0));
                    }
                }
            }
            return logins;
        }

        /// <summary>
        /// If --moveallfiles or -movefile is specified, query the database with RESTORE FILELISTONLY to 
        /// prepare a list of WITH MOVE items for later.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool PrepareDatabaseFiles(OptionsClass options)
        {
            if (options.MoveAllFiles || options.MoveItems.Count > 0)
            {
                string SQLConnectionString = BuildConnectionString(options, useMasterDB: true);
                List<MoveItem> moveItems = new List<MoveItem>();
                LogString("Preparing to query SQL server for backup set file list.");
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    StringBuilder querysb = new StringBuilder();
                    querysb.Append("RESTORE FILELISTONLY FROM DISK = @BAKPATH");
                    try
                    {
                        LogString("Opening connection to SQL server... ", NewLine: false);
                        connection.Open();
                        LogString("Successful");
                        using (SqlCommand cmd = new SqlCommand(querysb.ToString(), connection))
                        {
                            cmd.Parameters.AddWithValue("@BAKPATH", options.SourceFile);
                            LogString("Getting file list... ", NewLine: false);
                            SqlDataReader reader = cmd.ExecuteReader();
                            LogString("Successful");
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
                                    LogString(string.Format("Database file {0} will be moved to {1}", logicalName, physicalName));
                                }
                            }
                        }
                        options.MoveItems = moveItems;
                    }
                    catch (Exception ex)
                    {
                        LogString("An exception occurred querying the backup files.");
                        LogString(ex.ToString());
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool RestoreDatabase(OptionsClass options)
        {
            string SQLConnectionString = BuildConnectionString(options, useMasterDB: true);
            LogString("Preparing to restore SQL Database.");
            using (SqlConnection connection = new SqlConnection(SQLConnectionString))
            {
                bool WithAdded = false;
                List<KeyValuePair<string, string>> parms = new List<KeyValuePair<string, string>>();
                int parcount = 0;
                StringBuilder querysb = new StringBuilder();
                querysb.Append("RESTORE DATABASE @DBNAME FROM DISK = @BAKPATH");
                parms.Add(new KeyValuePair<string, string>("@DBNAME", options.DatabaseName));
                parms.Add(new KeyValuePair<string, string>("@BAKPATH", options.SourceFile));

                if (options.ReplaceDatabase)
                {
                    querysb.Append(" WITH REPLACE");
                    WithAdded = true;
                }
                foreach (var mi in options.MoveItems)
                {
                    if (WithAdded)
                    {
                        querysb.AppendFormat(", MOVE {0} TO {1}", ParName("@LNAME", parcount), ParName("@LPATH", parcount));
                        parms.Add(new KeyValuePair<string, string>(ParName("@LNAME", parcount), mi.LogicalName));
                        parms.Add(new KeyValuePair<string, string>(ParName("@LPATH", parcount), mi.PhysicalName));
                        parcount++;
                    }
                    else
                    {
                        querysb.AppendFormat(" WITH MOVE {0} TO {1}", ParName("@LNAME", parcount), ParName("@LPATH", parcount));
                        parms.Add(new KeyValuePair<string, string>(ParName("@LNAME", parcount), mi.LogicalName));
                        parms.Add(new KeyValuePair<string, string>(ParName("@LPATH", parcount), mi.PhysicalName));
                        parcount++;
                        WithAdded = true;
                    }
                }
                querysb.Append(";");
                Debug.WriteLine(querysb.ToString());
                try
                {
                    LogString("Opening connection to SQL server... ", NewLine: false);
                    connection.Open();
                    LogString("Successful");
                    using (SqlCommand cmd = new SqlCommand(querysb.ToString(), connection))
                    {
                        foreach (var p in parms)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value);
                        }
                        LogString("Restoring database... ", NewLine: false);
                        cmd.ExecuteNonQuery();
                        LogString("Successful");
                    }
                }
                catch (Exception ex)
                {
                    LogString("An exception occurred restoring the database");
                    LogString(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// CREATE USER and ALTER ROLE only accept literals, not parameters. So, this can't be parameterized. 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool SetDatabasePermissions(OptionsClass options)
        {
            if (options.UserRights.Count > 0)
            {
                string SQLConnectionString = BuildConnectionString(options);
                LogString("Preparing to restore user rights.");
                using (SqlConnection connection = new SqlConnection(SQLConnectionString))
                {
                    try
                    {
                        string query;
                        LogString("Opening connection to SQL server... ", NewLine: false);
                        connection.Open();
                        LogString("Successful");
                        LogString("Getting list of users from the database... ", NewLine: false);
                        List<string> existingUsers = new List<string>();
                        query = "SELECT name as Username FROM sys.database_principals WHERE type not in ('A', 'G', 'R', 'X') and sid is not null and name != 'guest';";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                existingUsers.Add(reader.GetString(0));
                            }
                            reader.Close();
                        }
                        LogString("Successful");
                        foreach (var user in options.UserRights)
                        {
                            // create the user
                            if (!existingUsers.Contains(user.Name, StringComparer.OrdinalIgnoreCase))
                            {
                                query = string.Format("CREATE USER [{0}] FOR LOGIN [{0}]", user.Name);
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    LogString(string.Format("Creating login for user [{0}]... ", user.Name), NewLine: false);
                                    cmd.ExecuteNonQuery();
                                    LogString("Successful");
                                }
                            }

                            query = string.Format("SELECT IS_ROLEMEMBER('db_datareader', '{0}') AS Reader, " +
                                "IS_ROLEMEMBER('db_datawriter', '{0}') AS Writer, " +
                                "IS_ROLEMEMBER('db_owner', '{0}') AS Owner;", user.Name);
                            bool isdbreader = false;
                            bool isdbwriter = false;
                            bool isdbowner = false;
                            using (SqlCommand cmd = new SqlCommand(query, connection))
                            {
                                LogString(string.Format("Retrieving current Roles for {0}...", user.Name), NewLine: false);
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    isdbreader = reader.GetInt32(0) == 1;
                                    isdbwriter = reader.GetInt32(1) == 1;
                                    isdbowner = reader.GetInt32(2) == 1;
                                }
                                reader.Close();
                                LogString("Successful");
                                LogString(string.Format("Existing Roles for user: {0}{1}{2}", isdbreader ? "READER " : "", isdbwriter ? "WRITER " : "", isdbowner ? "OWNER" : ""));
                            }
                            if (user.Read && !isdbreader)
                            {
                                query = string.Format("ALTER ROLE [db_datareader] ADD MEMBER [{0}]", user.Name);
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    LogString(string.Format("Granting db_datareader to [{0}]... ", user.Name), NewLine: false);
                                    cmd.ExecuteNonQuery();
                                    LogString("Successful");
                                }
                            }
                            if (user.Write && !isdbwriter)
                            {
                                query = string.Format("ALTER ROLE [db_datawriter] ADD MEMBER [{0}]", user.Name);
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    LogString(string.Format("Granting db_datawriter to [{0}]... ", user.Name), NewLine: false);
                                    cmd.ExecuteNonQuery();
                                    LogString("Successful");
                                }
                            }
                            if (user.Owner && !isdbowner)
                            {
                                query = string.Format("ALTER ROLE [db_owner] ADD MEMBER [{0}]", user.Name);
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    LogString(string.Format("Granting db_owner to [{0}]... ", user.Name), NewLine: false);
                                    cmd.ExecuteNonQuery();
                                    LogString("Successful");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogString("An exception occurred assigning roles.");
                        LogString(ex.ToString());
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool RunDBCC(OptionsClass options)
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
                        LogString("Opening connection to SQL server... ", NewLine: false);
                        connection.Open();
                        LogString("Successful");
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            LogString("Running DBCC CHECKEDB... ", NewLine: false);
                            cmd.ExecuteNonQuery();
                            LogString("Successful. See Info messages below for any DBCC errors reported.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogString("Error:");
                        LogString(ex.ToString());
                        return false;
                    }
                    connection.InfoMessage -= Connection_InfoMessage;
                    LogString("Information Messages from SQL Server:");
                    LogString(InfoMessageSB.ToString());
                    InfoMessageSB.Clear();
                } 
            }
            return true;
        }

        public static StringBuilder InfoMessageSB = new StringBuilder();
        public static void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            InfoMessageSB.AppendLine(e.Message);
        }

        public static string ParName(string name, int num)
        {
            return name + "_" + num.ToString();
        }

        public static SMTPProfileClass LoadSMTPProfile(string path, string password = "")
        {
            SMTPProfileClass profile = null;
            try
            {
                XmlSerializer xser = new XmlSerializer(typeof(SMTPProfileClass));
                MemoryStream stream = LoadAndDecryptFile(path, password);
                profile = (SMTPProfileClass)xser.Deserialize(stream);
                stream.Dispose();
                return profile;
            }
            catch
            {
                return null;
            }
        }

        public static bool SaveSMTPProfile(SMTPProfileClass profile, string path, string password = "")
        {
            try
            {
                XmlSerializer xser = new XmlSerializer(typeof(SMTPProfileClass));
                using (MemoryStream stream = new MemoryStream())
                {
                    xser.Serialize(stream, profile);
                    EncryptAndSaveFile(path, password, stream);
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public static OptionsClass LoadOptionsFile(string path, string password = "")
        {
            OptionsClass options = null;
            try
            {
                XmlSerializer xser = new XmlSerializer(typeof(OptionsClass));
                MemoryStream stream = LoadAndDecryptFile(path, password);
                options = (OptionsClass)xser.Deserialize(stream);
                stream.Dispose();
                return options;
            }
            catch
            {
                return null;
            }
        }

        public static bool SaveOptionsFile(OptionsClass options, string path, string password = "")
        {
            try
            {
                XmlSerializer xser = new XmlSerializer (typeof(OptionsClass));
                using (MemoryStream stream = new MemoryStream())
                {
                    xser.Serialize(stream, options);
                    EncryptAndSaveFile(path, password, stream);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool EncryptAndSaveFile(string path, string password, MemoryStream data)
        {
            byte[] HeaderBytes = new byte[28];
            byte[] magicNumber = Encoding.ASCII.GetBytes("EXOF");
            byte[] IV = new byte[16];
            byte[] Salt = new byte[8];
            data.Seek(0, SeekOrigin.Begin);
            byte[] SrcSizeBytes = BitConverter.GetBytes((int)data.Length);
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(IV);
                rng.GetBytes(Salt);
            }
            Array.Copy(magicNumber, 0, HeaderBytes, 0, 4);
            Array.Copy(IV, 0, HeaderBytes, 4, 16);
            Array.Copy(Salt, 0, HeaderBytes, 20, 8);
            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, Salt, KEYITERATIONS);
            using (var aes = new AesManaged())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;
                aes.BlockSize = 128;
                aes.KeySize = 128;
                aes.Key = k1.GetBytes(16);
                aes.IV = IV;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] block = new byte[16];
                int bpos = 0;
                Array.Copy(SrcSizeBytes, 0, block, 0, SrcSizeBytes.Length);
                bpos += SrcSizeBytes.Length;
                using (Stream writer = File.Create(path))
                {
                    writer.Write(HeaderBytes, 0, HeaderBytes.Length);
                    bool first = true;
                    while (first || data.Position < data.Length)
                    {
                        first = false;
                        int avail = block.Length - bpos;
                        int read = data.Read(block, bpos, avail);
                        bpos += read;
                        if (data.Position < data.Length)
                        {
                            encryptor.TransformBlock(block, 0, bpos, block, 0);
                        }
                        else
                        {
                            block = encryptor.TransformFinalBlock(block, 0, 16);
                        }
                        writer.Write(block, 0, block.Length);
                        bpos = 0;
                    }
                }
            }            
            return true;

        }

        private static MemoryStream LoadAndDecryptFile(string path, string password)
        {
            MemoryStream Result = null;
            using (Stream reader = File.OpenRead(path))
            {
                if (reader.Length < 44) // 28 byte header + a single 16 byte block minimum size for valid file.
                {
                    throw new Exception("Decrypt file error.");
                }

                byte[] HeaderBytes = new byte[28];
                int read = reader.Read(HeaderBytes, 0, HeaderBytes.Length);
                if (read != 28)
                {
                    throw new Exception("Decrypt file error.");
                }
                string magicNumber = Encoding.ASCII.GetString(HeaderBytes, 0, 4);
                if (magicNumber != "EXOF")
                {
                    throw new Exception("Decrypt file error.");
                }
                byte[] IV = new byte[16];
                byte[] Salt = new byte[8];
                Array.Copy(HeaderBytes, 4, IV, 0, 16);
                Array.Copy(HeaderBytes, 20, Salt, 0, 8);
                int DecryptedLength = 0;

                Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, Salt, KEYITERATIONS);
                using (var aes = new AesManaged())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.Zeros;
                    aes.BlockSize = 128;
                    aes.KeySize = 128;
                    aes.Key = k1.GetBytes(16);
                    aes.IV = IV;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    bool first = true;
                    byte[] block = new byte[16];
                    int bstart = 0;
                    while (first || reader.Position < reader.Length)
                    {
                        bstart = 0;
                        read = reader.Read(block, 0, block.Length);
                        if (reader.Position < reader.Length)
                        {
                            decryptor.TransformBlock(block, 0, read, block, 0);
                        }
                        else
                        {
                            block = decryptor.TransformFinalBlock(block, 0, read);
                        }
                        if (first)
                        {
                            first = false;
                            DecryptedLength = BitConverter.ToInt32(block, 0);
                            if (DecryptedLength < 0)
                            {
                                throw new Exception("Decrypt file error.");
                            }
                            bstart = 4;
                            Result = new MemoryStream(DecryptedLength);
                        }
                        int copylen = read - bstart;
                        if (Result.Position + copylen > DecryptedLength)
                        {
                            copylen = (int)(DecryptedLength - Result.Position);
                            if (copylen < 0 || copylen > block.Length - bstart)
                            {
                                throw new Exception("Decrypt file error.");
                            }
                        }
                        Result.Write(block, bstart, copylen);
                    }
                }
                Result.Seek(0, SeekOrigin.Begin);
                return Result;
            }
        }
           

    }
}
