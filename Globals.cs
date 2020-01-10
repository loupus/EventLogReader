using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EventLogReader
{
    public delegate void FsEventHandler(fsArgument arg);
    public delegate void FsErrorEventHandler(string arg);
    public delegate void FsMessageEventHandler(string arg);
    public delegate void EwEventHandler(ewArgument arg);

    public static class Globals
    {
        public static readonly object _FsBufferLock = new object();
        public static readonly object _EwBufferLock = new object();
        public static string localPath = @"C:\Users\hakansoyalp\Desktop\watch\";
        public static string remotePath = @"C:\Users\hakansoyalp\Desktop\watch\";

        public static string SqldServer = "127.0.0.1";
        public static string SqldDb = "FileAudit";
        public static bool SqlIstrusted = true;
        public static string SqldUser;
        public static string SqldPassword;

        public static string ConnectionString = "";

        public static void SetSqlConStr()
        {
            if(SqlIstrusted)
            {
                ConnectionString = string.Format("Server={0};Database={1};Trusted_Connection=True;",SqldServer,SqldDb,SqldUser,SqldPassword);
            }
            else
                ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", SqldServer, SqldDb, SqldUser, SqldPassword);
          
        }

        static List<fsArgument> FsArgs = new List<fsArgument>();
        public static List<ewArgument> EwArgs = new List<ewArgument>();
        

        public static void AddFsArg(fsArgument parg)
        {
            if (parg == null) return;
            lock(_FsBufferLock)
            {
                FsArgs.Add(parg);
            }
        }

        public static fsArgument GetFirstFs()
        {
            fsArgument back = null;
            lock (_FsBufferLock)
            {
                back = FsArgs.FirstOrDefault();
            }
            return back;
        }

        public static void AddEwArg(ewArgument parg)
        {
            if (parg == null) return;
            lock (_EwBufferLock)
            {
                EwArgs.Add(parg);
            }
        }

        public static ewArgument GetFirstEw()
        {
            ewArgument back = null;
            lock (_EwBufferLock)
            {
                back = EwArgs.FirstOrDefault();
            }
            return back;
        }


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
        public int Stat;
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
        public string HandleID;
        public string AccessList;
        public string AccessMask;
        public string ProcessName;
        public DateTime TimeGenerated;
        public int Stat;
    };

    public class EwTable :DataTable
    {
        public EwTable()
        {
            Columns.Add("EventID", Type.GetType("System.Int32"));
            Columns.Add("RecordID", Type.GetType("System.Int64"));
            Columns.Add("MachineName", Type.GetType("System.String"));
            Columns.Add("Name", Type.GetType("System.String"));
            Columns.Add("UserName", Type.GetType("System.String"));
            Columns.Add("DomainName", Type.GetType("System.String"));
            Columns.Add("IpAddress", Type.GetType("System.String"));
            Columns.Add("ObjectName", Type.GetType("System.String"));
            Columns.Add("HandleID", Type.GetType("System.String"));
            Columns.Add("AccessList", Type.GetType("System.String"));
            Columns.Add("AccessMask", Type.GetType("System.String"));
            Columns.Add("ProcessName", Type.GetType("System.String"));
            Columns.Add("TimeGenerated", Type.GetType("System.DateTime"));
            Columns.Add("Stat", Type.GetType("System.Boolean"));
        }

        ~EwTable()
        {
            Rows.Clear();
            Columns.Clear();
        }
    }
    
    
}
