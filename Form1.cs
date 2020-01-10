﻿using System;
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

        private void Prepare()
        {
            fs = new fsWatcher();
            em = new EventMonitor();
            match = new Matcher();

            fs.SetDir(@"C:\Users\hakansoyalp\Desktop\watch\");
            fs.eOnError += Fs_eOnError;
            fs.eOnEvet += Fs_eOnEvet;
            fs.eOnMessage += Fs_eOnMessage;

            em.eOnEvet += Em_eOnEvet;
            em.eOnError += Em_eOnError;
            em.eOnMessage += Em_eOnMessage;

            dtFs = new FsTable();
          
            gvFw.DataSource = dtFs;

            dtEs = new EwTable();           
            gvEw.DataSource = dtEs;

        
        }

        private void Em_eOnMessage(string arg)
        {
            AddLog(arg);
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
                dw["ID"] = arg.ID;
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
                dw["ActivityID"] = arg.ActivityID;
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
          //  match.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            fs.Stop();
            em.StopMonitor();
            match.Stop();
        }
    }
}
