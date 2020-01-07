using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace EventLogReader
{
    class fsWatcher
    {
        string dirpath = "";     
        FileSystemWatcher fsw;
        DataAccess da;
      
        public event FsMessageEventHandler eOnMessage;
        public event FsErrorEventHandler eOnError;
        public event FsEventHandler eOnEvet;

        public fsWatcher()
        {
        }

        ~fsWatcher()
        {
            if (fsw != null)
                fsw = null;            
        }

        public bool SetDir(string pDirPath)
        {
            if (Directory.Exists(pDirPath))
            {
                dirpath = pDirPath;
                return true;
            }
            else return false;
        }

        public void Start()
        {
            PrepareWatcher();
            fsw.EnableRaisingEvents = true;
            eOnMessage?.Invoke("FsWatcher started");
        }

        public void Stop()
        {
            fsw.EnableRaisingEvents = false;
            eOnMessage?.Invoke("FsWatcher stopped");
        }

        void PrepareWatcher()
        {
            if (da == null)
                da = new DataAccess();

            if (fsw == null)
            {
                fsw = new FileSystemWatcher(dirpath);
                fsw.InternalBufferSize = 1024 * 16; // 16KB
               
                fsw.Path = dirpath;
                fsw.Changed += Fsw_Changed;
                fsw.Created += Fsw_Created;
                fsw.Deleted += Fsw_Deleted;
                fsw.Renamed += Fsw_Renamed;
                fsw.Error += Fsw_Error;
            }

        }

        public void SetBufferSize(int kbs)
        {
            fsw.InternalBufferSize = 1024 * kbs;
            eOnMessage?.Invoke(string.Format("Watcher buffer size set to {0} KB",kbs));
        }

        private void Fsw_Error(object sender, ErrorEventArgs e)
        {
            eOnError?.Invoke(string.Format("FsWatcher Error: {0}", e.GetException().Message));
        }

        private void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            fsArgument farg = new fsArgument();
            farg.ChangeType = (int)e.ChangeType;
            farg.Name = e.Name;
            farg.FullName = e.FullPath;
            farg.OldName = e.OldName;
            farg.OldFullName = e.OldFullPath;
            farg.WhenHappened = DateTime.Now;
            Globals.AddFsArg(farg);
            eOnEvet?.Invoke(farg);
            da.InsertFsValue(farg);
        }

        private void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            fsArgument farg = new fsArgument();
            farg.ChangeType = (int)e.ChangeType;
            farg.Name = e.Name;
            farg.FullName = e.FullPath;
            farg.WhenHappened = DateTime.Now;
            Globals.AddFsArg(farg);
            eOnEvet?.Invoke(farg);
            da.InsertFsValue(farg);
        }

        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            fsArgument farg = new fsArgument();
            farg.ChangeType = (int)e.ChangeType;
            farg.Name = e.Name;
            farg.FullName = e.FullPath;
            farg.WhenHappened = DateTime.Now;
            Globals.AddFsArg(farg);
            eOnEvet?.Invoke(farg);
            da.InsertFsValue(farg);

        }

        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            fsArgument farg = new fsArgument();
            farg.ChangeType = (int)e.ChangeType;
            farg.Name = e.Name;
            farg.FullName = e.FullPath;
            farg.WhenHappened = DateTime.Now;
            Globals.AddFsArg(farg);
            eOnEvet?.Invoke(farg);
            da.InsertFsValue(farg);
        }
    }
}
