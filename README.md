# Database Restore
This repository contains a command line tool for restoring Microsoft SQL Server databases from backup files. 

It suppors copying a backup file from a directory or UNC path to another temporary location, connecting to SQL server and restoring that database, setting permissions, checking the database with dbcc checkdb, and deleting the copied temp file when done. The intended usage of this is for automated testing of database backups, but can be used to restore a backup of a production server to a different server for testing or reporting puroses, etc.

The tool doesn't have any scheduling capabilities built in, but works with batch files, powershell scripts, task scheduler, SQL Server Agent, pretty much any program that can call an executable command line program and pass arguments to it. 

Permissions for file copying inherit from the user account running the program. You'll need at least read permissions on the source folder where the backup files are. Either the SQL server needs read permissions to that location as well, or you need a temp directory that SQL server has access to. I suggest copying the backup files from the backup location to a subfolder in the SQL server backups directory of the target server, restoring from that, then the tool will delete the temporary file when done. If you are restoring a bak file to the same server that made the backup, then you don't need the temp directory, and the backup file will not be deleted (only temp files created by the tool are deleted).

SQL server permissions can be either integrated authentication (SSPI), or passed via command line arguments to the program as username and password. Passing credentials through command line is inherently insecure, so I recommend using integrated authentication.

All steps of the process are outputted to the standard output stream, so you can pipe that text with | to another program, or output to a file on disk with >.



## Usage
Starting the tool with no arguments will display the usage instructions. 

  --autosource -a <mode> <path>   : select a source file from specified path based on specified mode.
  
  --source -s <filepath>          : source database backup file.
  
  --temp -t <filepath>            : temporary directory and file name to copy the source to before restoring.
  
  --serverip -i <ip>              : IP address of the server to connect to.
  
  --servername -n <name>          : name of the server to connect to.
  
  --serverport -p <port>          : port of the server to connect to.
  
  --database -d <name>            : name of the database to overwrite with this backup.
  
  --username -u <username>        : username to connect to SQL server as.
  
  --password -p <password>        : password to connect to SQL server (INSECURE)
  
  --rights -r <userlist>          : list of users/groups and permission to grant to each.
  
  --movefile -m <name> <filepath> : Tells SQL server to move the database file to the specified filepath when restoring.
  
  --moveallfiles <filepath>       : Tells SQL server to move all database files to the specified path when restoring, preserving file names.
  
  --replacedatabase               : Tells SQL server to replace the existing database with this backup.
  
  --dbcccheckdb                   : Runs DBCC CHECKDB on the restored database to verify there is no corruption.

Autosource mode specifies which file to choose in the specified directory.

 lastcreated : selects the newest file by creation date
 
 lastmodified : selects the newest file by modified date
 
Example: --autosource lastcreated c:\backups\database_20241231.bak



Users List accepts a list of users and group separated by semi-colon (;).

Separate the user or group from the access modifier by a colon (:)

R grants DataReader, W grants DataWriter, O grants Owner.

Example: domain\UserName:O;domain\group:R;SQLUser:RW


Only specify --serverip OR --servername, not both. If neither is specified, localhost is assumed.

If no --serverport is specified, 1433 is assume.

If no --username and --password are specified, then integrated (SSPI) authentication will be used.

If no --temp is specified, then will attempt to restore from --source directly.

In that case, User account for SQL server process must have access to the source file.


Argumnents such as paths which contain spaces should be enclosed in double quotes, like this:

"c:\Program Files\Microsoft SQL Server\Backups"

Don't put a trailing \ on the end of a folder path, as that will result in escaping the ". This:

"c:\Program Files\Test\"

Will break parsing and become

c:\Program Files\Test"

Which will essentially break parsing of the arguments and likely result in unexpected behavior. Combining paths will definitely break, as the illegal character " will be used as part of the path.



The database will fail to restore if it already exists and the --replacedatabase is not specified.

The database will fail to restore if it is in use, has any active connections to it, or the user account provided doesn't have rights to restore.

The database will fail to restore if the database file path on the source server (where the backup as created) does not match that of target server (where the backup will be restored), unless either --movefile argument specifies all database files to be placed in valid locations, or --moveallfiles specifies a path to move everything. If a file is specified for --movefile that doens't actually exist in the backup, it will be ignored. If any files aren't specified by --movefile, then --moveallfiles can also be used to move everything else that --movefile didn't specify. Kind of a "move to of last resort" situation.



## Example Use
--autosource lastcreated "C:\Temp\SQL Backups" --temp "\\testsql\$backup\adventureworks.bak" --servername testsql --database AdventureWorks --rights "domain\usera:RWO;domain\userb:rw;sqluser:r" --replacedatabase --moveallfiles "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA" --dbcccheckdb

This will look in the directory "C:\Temp\SQL Backups" and locate the youngest file there by creation date. It will copy it to the location "\\testsql\$backup\adventureworks.bak".

Then a connection will be made to the SQL server testsql using the initial catalog master. The bak file will be restored over top of the database "AdventureWorks" if it exists, or a new database will be created. 

SQL Server will inspect the bak file and return the list of database files, and then all of those files will get a WITH MOVE option added to the restore to relocate them to "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA" with their original file names. If these files exist already, SQL server will overwrite them (unless they are in use, in which case it will fail).

After the restore is done, a new connection to SQL server will be made and users will be added to the database roles. These users must already exist as logins in SQL server or the process will fail.

User "domain\usera" will be added to db_datareader, db_datawriter, and db_owner roles.

User "domain\userb" will be added to db_datareader, and db_datawriter roles.

User "sqluser" will be added to db_datareader role.

Finally, dbcc checkdb will be run on the database to verify that there is no database corruption.



## Known Issues
I've tested the program only on a test SQL server 2019 Express server so far, using integrated authentication only. I'm sure there's plenty of ways that passing passwords through the command line can expose all sorts of security issues that I haven't investigated yet, and probably special characters will break commandline argument parsing too.

I know that older SQL Server versions don't support "CREATE USER x FOR LOGIN x" syntax, nor do they support "ALTER ROLE x ADD MEMBER y". It should work on all currently Microsoft supported versions of SQL server (2016, 2017, 2019, 2022), but I have only tested 2019 as that is my use case. 

There are probably differences for AWS/Azure SQL hosted servers that won't work with this at all. You'll have to test and submit a bug to let me know as I don't have access to those to test. 

Because of the way the SQL queries are constructed, it is absolutely possible to pass malicious arguments to do horrible things to your SQL server. Since running this tool requires command line access to a server and SQL server credentials (or access granted through some other means), then I figure it's already possible to do total destruction to the SQL server via other, more direct means than this tool. If you do find a bug that causes a problem, please do submit a bug report with your command line argument so I can address it if possible. I want the tool to be useful, not destructive.

As with all programs downloaded from the Internet, you need to be careful and test things before deploying them on a production environment. You must set up a test server and test your use-case of the tool to make sure it does what you need without any unacceptable side-effects before deploying it to production. If you break something using this tool, please do let me know via a bug report, but ultimately I can't be responsible for other people's use scenarios. So BE CAREFUL!



## Future Plans
Add logging options to allow the program to output to a log file instead of the command window, either appending to an existing file, generating a new log file each time, or replacing the log file.

Add the option to automatically email out the log file on success, failure, or both to a specified email address.

Capture the output of dbcc checkdb and look for failure messages, or at least capture the output and append it to the log and email.

Add an option to execute a specified SQL script after restoration is complete, so that it can be used to rebuild indexes, or run reports automatically, run arbitrary SQL instructions, etc.

