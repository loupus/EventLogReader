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
    public delegate void MUpdate();

    public static class Globals
    {
        public static string ConfigFile = "config.xml";

        public static readonly object _FsBufferLock = new object();
        public static readonly object _EwBufferLock = new object();
        public static readonly object _DBLock = new object();
        public static string localPath = @"C:\Users\hakansoyalp\Desktop\watch\";
        public static string remotePath = @"C:\Users\hakansoyalp\Desktop\watch\";

        //public static string SqldServer = "127.0.0.1";
        //public static string SqldDb = "FileAudit";
        //public static bool SqlIstrusted = true;
        //public static string SqldUser;
        //public static string SqldPassword;

        public static string ConnectionString = "";

        public static class Config
        {
            public static string Directory;
            public static int OffsetSeconds;
            public static int ClearMinutes;

            public static string SqldServer;
            public static string SqldDb;
            public static bool SqlIsTrusted;
            public static string SqldUser;
            public static string SqldPassword;

        }

        public static int GetIntValue(Object obj, int Insteadtrue = 0)
        {
            int back = 0;
            if (obj != null || obj != DBNull.Value)
            {
                if (!Int32.TryParse(obj.ToString(), out back))
                    back = Insteadtrue;
            }
            return back;
        }


        public static void SetSqlConStr()
        {
            if(Config.SqlIsTrusted)
            {
                ConnectionString = string.Format("Server={0};Database={1};Trusted_Connection=True;", Config.SqldServer, Config.SqldDb);
            }
            else
                ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", Config.SqldServer, Config.SqldDb, Config.SqldUser, Config.SqldPassword);
          
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

        public static fsArgument GetNextFs()
        {
            fsArgument back = null;
            lock (_FsBufferLock)
            {
                back = FsArgs.OrderBy(s => s.ScanCount).FirstOrDefault(x => x.Stat == FStat.None);
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

        public static void ClearEwList()
        {
            lock (_EwBufferLock)
            {
                EwArgs.RemoveAll(x => (DateTime.Now - x.TimeGenerated).TotalMinutes > Globals.Config.ClearMinutes); 
            }
        }

        public static void ClearFsList()
        {
            lock (_FsBufferLock)
            {
                 FsArgs.RemoveAll(x => (DateTime.Now - x.WhenHappened).TotalMinutes > Globals.Config.ClearMinutes);
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


    
    
}
