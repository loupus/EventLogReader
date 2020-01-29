using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

namespace EventLogReader
{
    class fsWatcher  // todo kaç defa tarayacaz?
    {
        string dirpath = "";     
        FileSystemWatcher fsw;
        DataAccess da;
      
        public event FsMessageEventHandler eOnMessage;
        public event FsErrorEventHandler eOnError;
        public event FsEventHandler eOnEvet;
        System.Timers.Timer t1;

        public fsWatcher()
        {
        }

        ~fsWatcher()
        {
            if (fsw != null)
                fsw = null;

            if(t1 != null)
            {
                t1.Enabled = false;
                t1 = null;
            }

            //Created = 1,
            //Deleted = 2,
            //Changed = 4,
            //Renamed = 8, 
            //All = 15

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
            t1.Enabled = true;
            eOnMessage?.Invoke("FsWatcher started");
        }

        public void Stop()
        {
            fsw.EnableRaisingEvents = false;
            t1.Enabled = false;
            eOnMessage?.Invoke("FsWatcher stopped");
        }

        void PrepareWatcher()
        {
            if (da == null)
                da = new DataAccess();

            if (fsw == null)
            {
                fsw = new FileSystemWatcher(dirpath);
                fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.FileName;
                fsw.InternalBufferSize = 1024 * 16; // 16KB
               
                fsw.Path = dirpath;
                fsw.IncludeSubdirectories = true;
                fsw.Changed += Fsw_Changed;
                fsw.Created += Fsw_Created;
                fsw.Deleted += Fsw_Deleted;
                fsw.Renamed += Fsw_Renamed;
                fsw.Error += Fsw_Error;
            }
            SetTimer();
        }

        private void SetTimer()
        {
            if(t1 == null)
            {
                t1 = new System.Timers.Timer(1000 * 60 * 5);
                t1.Elapsed += T1_Elapsed;
                t1.AutoReset = true;
            }

        }
        public void SetBufferSize(int kbs)
        {
            fsw.InternalBufferSize = 1024 * kbs;
            eOnMessage?.Invoke(string.Format("Watcher buffer size set to {0} KB",kbs));
        }


        private void T1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            eOnMessage?.Invoke("Matcher Timer loop");
            Globals.ClearFsList();
        }
        private void Fsw_Error(object sender, ErrorEventArgs e)
        {
            eOnError?.Invoke(string.Format("FsWatcher Error: {0}", e.GetException().Message));
        }

        private void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            Task.Run(() =>
            {
                fsArgument farg = new fsArgument();
                farg.ChangeType = (int)e.ChangeType;
                farg.Name = e.Name;
                farg.FullName = e.FullPath;
                farg.OldName = e.OldName;
                farg.OldFullName = e.OldFullPath;
            //    farg.WhenHappened = DateTime.Now;
                farg.WhenHappened = File.GetLastAccessTime(e.FullPath); // todo last write olmuyor
                Globals.AddFsArg(farg);
                eOnEvet?.Invoke(farg);
                da.SaveFsValue(farg);              
            }
            );
        }
        private void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            Task.Run(() =>
            {
                fsArgument farg = new fsArgument();
                farg.ChangeType = (int)e.ChangeType;
                farg.Name = e.Name;
                farg.FullName = e.FullPath;
                farg.WhenHappened = DateTime.Now;
                Globals.AddFsArg(farg);
                eOnEvet?.Invoke(farg);
                da.SaveFsValue(farg);
            }
            );
        }
        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {

            try
            {
                if (File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
                    return;
            }
            catch (Exception ex)
            {

                eOnError?.Invoke(string.Format("FsWatcher Error: {0}", ex.Message));
            }

            Task.Run(() =>
            {
                fsArgument farg = new fsArgument();
                farg.ChangeType = (int)e.ChangeType;
                farg.Name = e.Name;
                farg.FullName = e.FullPath;
                //farg.WhenHappened = DateTime.Now;
                farg.WhenHappened = File.GetCreationTime(e.FullPath);            
                Globals.AddFsArg(farg);
                eOnEvet?.Invoke(farg);
                da.SaveFsValue(farg);
            }
            );
        }
        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
                    return;
            }
            catch (Exception ex)
            {

                eOnError?.Invoke(string.Format("FsWatcher Error: {0}", ex.Message));
            }
            Task.Run(() =>
            {
                fsArgument farg = new fsArgument();
                farg.ChangeType = (int)e.ChangeType;
                farg.Name = e.Name;
                farg.FullName = e.FullPath;
                farg.WhenHappened = File.GetLastWriteTime(e.FullPath);
                //   farg.WhenHappened = DateTime.Now;
                Globals.AddFsArg(farg);
                eOnEvet?.Invoke(farg);
                da.SaveFsValue(farg);
            }
    );
        }

    }
}
