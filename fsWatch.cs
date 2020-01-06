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
        string user;
        FileSystemWatcher fsw;
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
            eOnMessage?.Invoke("Watcher started");
        }

        public void Stop()
        {
            fsw.EnableRaisingEvents = false;
            eOnMessage?.Invoke("Watcher stopped");
        }

        void PrepareWatcher()
        {
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
           // Console.ForegroundColor = ConsoleColor.Red;
           // Console.WriteLine("Sender:" + sender.ToString() + "-" + e.GetException().Message );
            eOnError?.Invoke(string.Format("FsWatcher Error: {0}", e.GetException().Message));
        }

        private void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            //Console.ForegroundColor = ConsoleColor.Blue;
            //Console.WriteLine("User:" + sender.ToString());
            //Console.WriteLine("ChangeType:" + Enum.GetName(e.ChangeType.GetType(), e.ChangeType));
            //Console.WriteLine("Name:" + e.Name);
            //Console.WriteLine("FullPath:" + e.FullPath);
            //Console.WriteLine("OldName:" + e.OldName);
            //Console.WriteLine("OldFullPath:" + e.OldFullPath);

            fsArgument farg = new fsArgument();
            farg.ChangeType = (int)e.ChangeType;
            farg.Name = e.Name;
            farg.FullName = e.FullPath;
            farg.OldName = e.OldName;
            farg.OldFullName = e.OldFullPath;
            farg.WhenHappened = DateTime.Now;
            eOnEvet?.Invoke(farg);
        }

        private void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Console.WriteLine("User:" + sender.ToString());
            //Console.WriteLine("ChangeType:" + Enum.GetName(e.ChangeType.GetType(), e.ChangeType));
            //Console.WriteLine("Name:" + e.Name);
            //Console.WriteLine("FullPath:" + e.FullPath);

            fsArgument farg = new fsArgument();
            farg.ChangeType = (int)e.ChangeType;
            farg.Name = e.Name;
            farg.FullName = e.FullPath;
            farg.WhenHappened = DateTime.Now;
            eOnEvet?.Invoke(farg);
        }

        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            //Console.ForegroundColor = ConsoleColor.White;
            //user = System.IO.File.GetAccessControl(e.FullPath).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            //Console.WriteLine("User:" + user);
            //Console.WriteLine("ChangeType:" + Enum.GetName(e.ChangeType.GetType(), e.ChangeType));
            //Console.WriteLine("Name:" + e.Name);
            //Console.WriteLine("FullPath:" + e.FullPath);

            fsArgument farg = new fsArgument();
            farg.ChangeType = (int)e.ChangeType;
            farg.Name = e.Name;
            farg.FullName = e.FullPath;
            farg.WhenHappened = DateTime.Now;
            eOnEvet?.Invoke(farg);

        }

        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine("User:" + sender.ToString());
            //Console.WriteLine("ChangeType:" + Enum.GetName(e.ChangeType.GetType(), e.ChangeType));
            //Console.WriteLine("Name:" + e.Name);
            //Console.WriteLine("FullPath:" + e.FullPath);

            fsArgument farg = new fsArgument();
            farg.ChangeType = (int)e.ChangeType;
            farg.Name = e.Name;
            farg.FullName = e.FullPath;
            farg.WhenHappened = DateTime.Now;
            eOnEvet?.Invoke(farg);
        }
    }
}
