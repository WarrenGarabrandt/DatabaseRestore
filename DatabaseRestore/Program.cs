using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        static void Main(string[] args)
        {
            string SourcePath = "";
            string TempPath = "";
            string DatabaseName = "";
            string UserString = "";
            List<UserRightItem> UserRightsToAssign = new List<UserRightItem>();
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  --source -s <path>        : source database backup file.");
                Console.WriteLine("  --temp -t <path>          : temporary directory and file name to copy the source to before restoring.");
                Console.WriteLine("  --serverip -i <ip>        : IP address of the server to connect to.");
                Console.WriteLine("  --servername -n <name>    : name of the server to connect to.");
                Console.WriteLine("  --serverport -p <port>    : port of the server to connect to.");
                Console.WriteLine("  --username -u <username>  : username to connect to SQL server as.");
                Console.WriteLine("  --password -p <password>  : password to connect to SQL server (INSECURE)");
                Console.WriteLine("  --database -d <name>      : name of the database to overwrite with this backup.");
                Console.WriteLine("  --rights -r <userlist>    : list of users/groups and permission to grant to each.");
                Console.WriteLine();
                Console.WriteLine("Users List accepts a list of users and group separated by semi-colon (;).");
                Console.WriteLine("Separate the user or group from the access modifier by a colon (:)");
                Console.WriteLine("R grants DataReader, W grants DataWriter, O grants Owner.");
                Console.WriteLine("Example: domain\\UserName:O;domain\\group:R;SQLUser:RW");
                Console.WriteLine();
                Console.WriteLine("Only specify --serverip OR --servername, not both. If neither is specified, localhost is assumed.");
                Console.WriteLine("If no --serverport is specified, 1433 is assume.");
                Console.WriteLine("If no --username and --password are specified, then integrated (SSPI) authentication will be used.");
                Console.WriteLine();

                return;
            }
            int pos = 0;
            while (pos < args.Length)
            {
                if (args[pos].ToLower() == "--source" || args[pos].ToLower() == "-s")
                {
                    pos++;
                    if (pos < args.Length)
                    {
                        Console.WriteLine("Missing Source Path.");
                        return;
                    }
                    SourcePath = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--temp" || args[pos].ToLower() == "-t")
                {
                    pos++;
                    if (pos < args.Length)
                    {
                        Console.WriteLine("Missing Temp File Name.");
                        return;
                    }
                    TempPath = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--database" || args[pos].ToLower() == "-d")
                {
                    pos++;
                    if (pos < args.Length)
                    {
                        Console.WriteLine("Missing Destination Database Name.");
                        return;
                    }
                    DatabaseName = args[pos];
                    pos++;
                }
                else if (args[pos].ToLower() == "--rights" || args[pos].ToLower() == "-r")
                {
                    pos++;
                    if (pos < args.Length)
                    {
                        Console.WriteLine("Missing User Accounts to grant permission.");
                        return;
                    }
                    UserString = args[pos];
                    string[] tempRights = UserString.Split(';');
                    foreach (var r in tempRights)
                    {
                        string[] tempItem = r.Split(':');
                        if (tempItem.Length != 2)
                        {
                            Console.WriteLine(string.Format("Unable to parse user permission: {0}", r));
                            return;
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
                        UserRightsToAssign.Add(newRight);
                    }
                    pos++;
                }
                else
                {
                    Console.WriteLine(string.Format("Unrecognized command line option {0}", args[pos]));
                    return;
                }
            }
            if (SourcePath.Length > 0 && System.IO.File.Exists(SourcePath))
            {
                Console.WriteLine(string.Format("Source database backup: {0}", SourcePath));
            }
            else
            {
                Console.WriteLine(string.Format("Can't find source database backup: {0}", SourcePath));
                return;
            }
            if (TempPath.Length > 0 && System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(TempPath)))
            {
                Console.WriteLine(string.Format("Temp Directory: {0}", System.IO.Path.GetDirectoryName(TempPath)));
                if (System.IO.File.Exists(TempPath))
                {
                    Console.WriteLine(string.Format("Temp File exists, will replace: {0}", System.IO.Path.GetFileName(TempPath)));
                }
                else
                {
                    Console.WriteLine(string.Format("Temp File: {0}", System.IO.Path.GetFileName(TempPath)));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Temp Directory does not exist: {0}", System.IO.Path.GetDirectoryName(TempPath)));
                return;
            }
            if (DatabaseName.Length > 0)
            {
                Console.WriteLine(string.Format("Database name: {0}", DatabaseName));
            }
            else
            {
                Console.WriteLine("Missing Destination Database Name.");
                return;
            }
            if (UserRightsToAssign.Count > 0)
            {
                Console.WriteLine("Restoring access rights for users:");
                foreach (var userRight in UserRightsToAssign)
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
                Console.WriteLine("No user rights specified to restore.");
                return;
            }

            if (!CleanTemp(TempPath))
            {
                Console.WriteLine("Failed deleting previous temp file.");
                return;
            }
            if (!CopyFile(SourcePath, TempPath))
            {
                Console.WriteLine("Failed copying source backup file to temp path.");
                return;
            }
            if (!RestoreDatabase(TempPath, DatabaseName, UserRightsToAssign))
            {
                Console.WriteLine("Failed restoring the database.");
                return;
            }
            if (!CleanTemp(TempPath))
            {
                Console.WriteLine("Failed deleting temp file.");
                return;
            }
            Console.WriteLine("Operation Completed.");
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
                    System.IO.File.Copy(sourceFile, tempFile);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        
        private static bool RestoreDatabase(string tempFile, string databaseName, List<UserRightItem> rights)
        {
            string SQLCommand = @"RESTORE DATABASE [" + databaseName + "] \r\n" +
                "  FROM DISK = \'" + tempFile + "\'\r\n" +
                "  WITH REPLACE;";

            return true;
        }
    }
}
