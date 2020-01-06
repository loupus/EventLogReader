using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace EventLogReader
{
    public delegate void FsEventHandler(fsArgument arg);
    public delegate void FsErrorEventHandler(string arg);
    public delegate void FsMessageEventHandler(string arg);
    public delegate void EwEventHandler(ewArgument arg);

    public static class Globals
    {
        public static readonly object _BufferLock = new object();
        public static string localPath = @"C:\Users\hakansoyalp\Desktop\watch\";
        public static string remotePath = @"C:\Users\hakansoyalp\Desktop\watch\";

        public static int[] EventIds =
        {
          // https://docs.microsoft.com/en-us/windows/security/threat-protection/auditing/audit-file-system
         5145, // detailed file share
         5140, // file share        
         4656, // file system // A handle to an object was  requested //The name of the file
         4663, // file system // An attempt was made to access an object // What exactly was done
         4660, // file system // An object was deleted // The only way to verify an activity is actually a delete
         4658, // file system // The handle to an object was closed // How much time it took
    };

    };

    public class fsArgument
    {
        public string Name;     
        public string FullName;
        public string OldName;
        public string OldFullName;
        public int ChangeType;
        public string User;
        public string SourceIp;
        public DateTime WhenHappened;
    };

    public class ewArgument
    {
        public int EventID;
        public long RecordID;
        public string MachineName;
        public string Name;
        public string UserName;
        public string DomainName;
        public string IpAddress;
        public string ObjectName;
        public object HandleID;
        public object AccessList;
        public object AccessMask;
        public string ProcessName;
        public DateTime TimeGenerated;
    };

 
    
    
}
