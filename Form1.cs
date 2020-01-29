using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;

namespace EventLogReader
{
    public partial class Form1 : Form
    {
        fsWatcher fs;
        EventMonitor em;
        DataTable dtFs;
        DataTable dtEs;
        Matcher match;
        
        public Form1()
        {
            InitializeComponent();
           
        }    
        private void Form1_Load(object sender, EventArgs e)
        {
           
            Prepare();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtFs.Clear();
            dtEs.Clear();
            dtFs = null;
            dtEs = null;
            Environment.Exit(0);
        }
        private void Prepare()
        {
            Config.GetConfig();

            fs = new fsWatcher();
            em = new EventMonitor();
            match = new Matcher();

            cbShowEvents.Checked = Globals.ShowEvents;
            cbSaveEwDb.Checked = Globals.SaveEwDb;
            cbSaveFsDb.Checked = Globals.SaveFsDb;

            //  fs.SetDir(@"C:\Users\hakansoyalp\Desktop\watch\");
            fs.SetDir(Globals.Config.Directory);
            fs.eOnError += Fs_eOnError;
            fs.eOnEvet += Fs_eOnEvet;
            fs.eOnMessage += Fs_eOnMessage;

            em.eOnEvet += Em_eOnEvet;
            em.eOnError += Em_eOnError;
            em.eOnMessage += Em_eOnMessage;

            match.eOnError += Match_eOnError;
            match.eOnMessage += Match_eOnMessage;
            match.eOnEvent += Match_eOnEvent;
           // match.eOnUpdate += Match_eOnUpdate;

            dtFs = new FsTable();
          
            gvFw.DataSource = dtFs;

            dtEs = new EwTable();           
            gvEw.DataSource = dtEs;
        
        }

        private void Match_eOnEvent(fsArgument arg)
        {
            UpdateFsTable(arg);
        }
        private void Match_eOnMessage(string arg)
        {
            AddLog(arg);
        }
        private void Match_eOnError(string arg)
        {
            AddLog(arg);
        }
        private void Em_eOnMessage(string arg)
        {
            AddLog(arg);
        }
        private void Em_eOnEvet(ewArgument arg)
        {
            AddEwArg(arg);
           // Task.Run(() => AddEwArg(arg));
        }
        private void Em_eOnError(string arg)
        {
            AddLog(arg);
        }
        private void Fs_eOnMessage(string arg)
        {
            AddLog(arg);
        }
        private void Fs_eOnEvet(fsArgument arg)
        {
            AddFsArg(arg);              
        }
        private void Fs_eOnError(string arg)
        {
            AddLog(arg);
        }
        private void AddLog(string log)
        {
            if (txtLog.InvokeRequired)
            {
                var d = new FsMessageEventHandler(AddLog);
                txtLog.Invoke(d, new object[] { log });
            }
            else
                txtLog.Text += log + Environment.NewLine;
        }
        private void AddFsArg(fsArgument arg)
        {

            if (gvFw.InvokeRequired)
            {
                var d = new FsEventHandler(AddFsArg);
                gvFw.Invoke(d, new object[] { arg });
            }
            else
            {
                if (dtFs.Rows.Count > 1000)
                    dtFs.Rows.Clear();

                DataRow dw = dtFs.NewRow();
                dw["ID"] = arg.ID;
                dw["Name"] = arg.Name;
                dw["Fullname"] = arg.FullName;
                dw["Oldname"] = arg.OldName;
                dw["OldFullname"] = arg.OldFullName;
                dw["ChangeType"] = arg.ChangeType;
                dw["User"] = arg.User;
                dw["SourceIp"] = arg.SourceIp;
                dw["WhenHappened"] = arg.WhenHappened.ToString("MM/dd/yyyy hh:mm:ss.fff");
                dw["RefRecordID"] = arg.RefRecordID;
                dw["ScanCount"] = arg.ScanCount;
                dw["Stat"] = (int)arg.Stat;              
                dtFs.Rows.Add(dw);
                dtFs.AcceptChanges();
                gvFw.Refresh();
            }
        }
        private void UpdateFsTable(fsArgument arg)
        {
            if (dtFs.Rows.Count == 0) return;
            foreach(DataRow dw in dtFs.Rows)
            {
                if(dw["ID"].ToString() == arg.ID)
                {
                    dw["Name"] = arg.Name;
                    dw["Fullname"] = arg.FullName;
                    dw["Oldname"] = arg.OldName;
                    dw["OldFullname"] = arg.OldFullName;
                    dw["ChangeType"] = arg.ChangeType;
                    dw["User"] = arg.User;
                    dw["SourceIp"] = arg.SourceIp;
                    dw["WhenHappened"] = arg.WhenHappened.ToString("MM/dd/yyyy hh:mm:ss.fff");
                    dw["RefRecordID"] = arg.RefRecordID;
                    dw["ScanCount"] = arg.ScanCount;
                    dw["Stat"] = (int)arg.Stat;
                    break;
                }
            }

            if (gvFw.InvokeRequired)
            {
                var d = new FsEventHandler(UpdateFsTable);
                gvFw.Invoke(d, new object[] { arg });
            }
            else
            {
                
                dtFs.AcceptChanges();
                gvFw.Refresh();
            }
        }
        private void AddEwArg(ewArgument arg)
        {
            if (gvEw.InvokeRequired)
            {
                var d = new EwEventHandler(AddEwArg);
                gvEw.Invoke(d, new object[] { arg });
            }
            else
            {
                if (dtEs.Rows.Count > 1000)
                    dtEs.Rows.Clear();

                DataRow dw = dtEs.NewRow();
                dw["EventID"] = arg.EventID;
                dw["RecordID"] = arg.RecordID;
                //dw["ActivityID"] = arg.ActivityID;
                dw["MachineName"] = arg.MachineName;
                dw["Name"] = arg.Name;
                dw["UserName"] = arg.UserName;
                dw["IpAddress"] = arg.IpAddress;
                dw["DomainName"] = arg.DomainName;
                dw["ObjectName"] = arg.ObjectName;
                dw["RelativeTargetName"] = arg.RelativeTargetName;
                dw["HandleID"] = arg.HandleID;
                dw["AccessList"] = arg.AccessList;
                dw["AccessMask"] = arg.AccessMask;
                dw["ProcessName"] = arg.ProcessName;
                dw["TimeGenerated"] = arg.TimeGenerated.ToString("MM/dd/yyyy hh:mm:ss.fff");
                dw["Stat"] = (int)arg.Stat;
                dtEs.Rows.Add(dw);
                dtEs.AcceptChanges();
                gvEw.Refresh();
            }

            Application.DoEvents();
           

        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = false;
            fs.Start();
            em.StartMonitor();
            match.Start();
            btnStop.Enabled = true;
            btnStart.Enabled = true;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = false;
            fs.Stop();
            em.StopMonitor();
            match.Stop();
            btnStop.Enabled = true;
            btnStart.Enabled = true;
        }       
        private void button1_Click(object sender, EventArgs e)
        {
          //  match.GoMatch();
        }
        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfig fc = new frmConfig();
            fc.ShowDialog();
        }

        private void cbShowEvents_CheckedChanged(object sender, EventArgs e)
        {
            Globals.ShowEvents = cbShowEvents.Checked;          
        }

        private void cbUpdateDb_CheckedChanged(object sender, EventArgs e)
        {
            Globals.SaveEwDb = cbSaveEwDb.Checked;
           
        }

        private void cbSaveFsDb_CheckedChanged(object sender, EventArgs e)
        {
            Globals.SaveFsDb = cbSaveFsDb.Checked;
        }
    }
}
