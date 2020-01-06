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
        
        public Form1()
        {
            InitializeComponent();
           
        }    
        private void Form1_Load(object sender, EventArgs e)
        {
            Prepare();
        }

        private void Prepare()
        {
            fs = new fsWatcher();
            em = new EventMonitor();

            fs.SetDir(@"C:\Users\hakansoyalp\Desktop\watch\");
            fs.eOnError += Fs_eOnError;
            fs.eOnEvet += Fs_eOnEvet;
            fs.eOnMessage += Fs_eOnMessage;

            em.eOnEvet += Em_eOnEvet;
            em.eOnError += Em_eOnError;

            dtFs = new DataTable();
            dtFs.Columns.Add("Name", Type.GetType("System.String"));
            dtFs.Columns.Add("Fullname", Type.GetType("System.String"));
            dtFs.Columns.Add("Oldname", Type.GetType("System.String"));
            dtFs.Columns.Add("OldFullname", Type.GetType("System.String"));
            dtFs.Columns.Add("ChangeType", Type.GetType("System.Int32"));
            dtFs.Columns.Add("User", Type.GetType("System.String"));
            dtFs.Columns.Add("SourceIp", Type.GetType("System.String"));
            dtFs.Columns.Add("WhenHappened", Type.GetType("System.DateTime"));
            gvFw.DataSource = dtFs;

            dtEs = new DataTable();
            dtEs.Columns.Add("EventID", Type.GetType("System.Int32"));
            dtEs.Columns.Add("RecordID", Type.GetType("System.Int64"));
            dtEs.Columns.Add("MachineName", Type.GetType("System.String"));
            dtEs.Columns.Add("Name", Type.GetType("System.String"));
            dtEs.Columns.Add("UserName", Type.GetType("System.String"));
            dtEs.Columns.Add("DomainName", Type.GetType("System.String"));
            dtEs.Columns.Add("IpAddress", Type.GetType("System.String"));
            dtEs.Columns.Add("ObjectName", Type.GetType("System.String"));
            dtEs.Columns.Add("HandleID", Type.GetType("System.String"));
            dtEs.Columns.Add("AccessList", Type.GetType("System.String"));
            dtEs.Columns.Add("AccessMask", Type.GetType("System.String"));
            dtEs.Columns.Add("ProcessName", Type.GetType("System.String"));
            dtEs.Columns.Add("TimeGenerated", Type.GetType("System.DateTime"));
            gvEw.DataSource = dtEs;


        }

        private void Em_eOnEvet(ewArgument arg)
        {
            AddEwArg(arg);
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
                DataRow dw = dtFs.NewRow();
                dw["Name"] = arg.Name;
                dw["Fullname"] = arg.FullName;
                dw["Oldname"] = arg.OldName;
                dw["OldFullname"] = arg.OldFullName;
                dw["ChangeType"] = arg.ChangeType;
                dw["User"] = arg.User;
                dw["SourceIp"] = arg.SourceIp;
                dw["WhenHappened"] = arg.WhenHappened;
                dtFs.Rows.Add(dw);
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
                DataRow dw = dtEs.NewRow();
                dw["EventID"] = arg.EventID;
                dw["RecordID"] = arg.RecordID;
                dw["MachineName"] = arg.MachineName;
                dw["Name"] = arg.Name;
                dw["UserName"] = arg.UserName;
                dw["IpAddress"] = arg.IpAddress;
                dw["DomainName"] = arg.DomainName;
                dw["ObjectName"] = arg.ObjectName;
                dw["HandleID"] = arg.HandleID;
                dw["AccessList"] = arg.AccessList;
                dw["AccessMask"] = arg.AccessMask;
                dw["ProcessName"] = arg.ProcessName;
                dw["TimeGenerated"] = arg.TimeGenerated;
                dtEs.Rows.Add(dw);
                dtEs.AcceptChanges();
                gvEw.Refresh();
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            fs.Start();
            em.StartMonitor();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            fs.Stop();
            em.StopMonitor();
        }
    }
}
