using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;
using System.Timers;

namespace EventLogReader
{
  
    class EventMonitor
    {
        public event EwEventHandler eOnEvet;
        public event FsErrorEventHandler eOnError;
        public event FsMessageEventHandler eOnMessage;
        EventLogWatcher ewatch;
        DataAccess da;
        System.Timers.Timer t1;
       
        public EventMonitor()
        {          
          
            
        }

        ~EventMonitor()
        {
            if(ewatch != null)
                ewatch = null;
        }

        public void Prepare()
        {
            if (da == null)
                da = new DataAccess();

            StringBuilder sb = new StringBuilder();
            sb.Append("<QueryList>");
            sb.Append(@"<Query Id=""0"">");
            sb.Append(@"<Select Path=""Security"">");
            sb.Append(@"*[System[(EventID=4663 or EventID=5145 or EventID=5140 or EventID=4660)");
            sb.Append(@"]]");
          //  sb.Append(@" and TimeCreated[@SystemTime &gt;= '" + DateTime.Now.AddMinutes(-1).ToUniversalTime().ToString("o") + "']]]");
            //  sb.Append(@" and *[EventData[Data[@Name =""ObjectName""] and (Data ='" + arg.Fullname + "')]]");
            sb.Append(@"</Select>");
            sb.Append(@"</Query>");
            sb.Append(@"</QueryList>");

            EventLogQuery query = new EventLogQuery("Security", PathType.LogName, sb.ToString());
            ewatch = new EventLogWatcher(query);
            ewatch.EventRecordWritten += Ewatch_EventRecordWritten;

           // SetTimer();
        }
        public void StartMonitor()
        {
            if (ewatch == null)
                Prepare();
            try
            {
                ewatch.Enabled = true;
                eOnMessage?.Invoke("EwWatcher started");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

        public void StopMonitor()
        {
            ewatch.Enabled = false;
         //   t1.Enabled = false;
            eOnMessage?.Invoke("EwWatcher stopped");
        }

        private void Ewatch_EventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {

            EvalEventData(e.EventRecord);
            //MessageBox.Show(e.EventRecord.ToXml());
        }

        private void EvalEventData(EventRecord er)
        {
            ewArgument ew = new ewArgument();
            ew.MachineName = er.MachineName;
            ew.Name = er.ProviderName;   // todo ??          
            ew.EventID = er.Id;
            ew.RecordID = er.RecordId == null ? 0 : (long)er.RecordId;
            ew.TimeGenerated = er.TimeCreated == null ? DateTime.MinValue : (DateTime) er.TimeCreated;
            ew.ActivityID = er.ActivityId.ToString();
            ew.RelatedActivityID = er.RelatedActivityId.ToString();
            er.

            string[] xpaths =
            {

                 "/bk:Event/bk:EventData/bk:Data[@Name='ObjectName']"
                ,"/bk:Event/bk:EventData/bk:Data[@Name='SubjectUserName']"
                ,"/bk:Event/bk:EventData/bk:Data[@Name='SubjectDomainName']"              
                ,"/bk:Event/bk:EventData/bk:Data[@Name='HandleId']"
                ,"/bk:Event/bk:EventData/bk:Data[@Name='AccessList']"
                ,"/bk:Event/bk:EventData/bk:Data[@Name='AccessMask']"
                ,"/bk:Event/bk:EventData/bk:Data[@Name='ProcessName']"
                ,"/bk:Event/bk:EventData/bk:Data[@Name='IpAddress']"
                ,"/bk:Event/bk:EventData/bk:Data[@Name='RelativeTargetName']"

                //"/Event/EventData/Data[@Name = 'ObjectName']"
                //,"//Event/EventData/Data/@Name=SubjectUserName"
                //,"//Event/EventData/Data/@Name=SubjectDomainName"
                //,"//Event/EventData/Data/@Name=ObjectName"
                //,"//Event/EventData/Data/@Name=HandleId"
                //,"//Event/EventData/Data/@Name=AccessList"
                //,"//Event/EventData/Data/@Name=AccessMask"
                //,"//Event/EventData/Data/@Name=ProcessName"
            };

            string strxml = er.ToXml();


            XmlDocument xdoc = new XmlDocument();
            XmlNamespaceManager manager = null;
            XmlNode tnode = null;
            try
            {            
                xdoc.LoadXml(strxml);
                manager = new XmlNamespaceManager(xdoc.NameTable);
                manager.AddNamespace("bk", "http://schemas.microsoft.com/win/2004/08/events/event");        
                
                tnode = xdoc.SelectSingleNode(xpaths[0], manager);
                if (tnode != null)
                {
                    ew.ObjectName = tnode.InnerText;
                }
                tnode = xdoc.SelectSingleNode(xpaths[1], manager);
                if (tnode != null)
                {
                    ew.UserName = tnode.InnerText;
                }
                tnode = xdoc.SelectSingleNode(xpaths[2], manager);
                if (tnode != null)
                {
                    ew.DomainName = tnode.InnerText;
                }
                tnode = xdoc.SelectSingleNode(xpaths[3], manager);
                if (tnode != null)
                {
                    ew.HandleID = tnode.InnerText;
                }
                tnode = xdoc.SelectSingleNode(xpaths[4], manager);
                if (tnode != null)
                {
                    ew.AccessList = tnode.InnerText;
                }
                tnode = xdoc.SelectSingleNode(xpaths[5], manager);
                if (tnode != null)
                {
                    ew.AccessMask = tnode.InnerText;
                }
                tnode = xdoc.SelectSingleNode(xpaths[6], manager);
                if (tnode != null)
                {
                    ew.ProcessName = tnode.InnerText;
                }
                tnode = xdoc.SelectSingleNode(xpaths[7], manager);
                if (tnode != null)
                {
                    ew.IpAddress = tnode.InnerText;
                }
                tnode = xdoc.SelectSingleNode(xpaths[8], manager);
                if (tnode != null)
                {
                    ew.RelativeTargetName = tnode.InnerText;
                }

            }
            catch (Exception ex)
            {

                eOnError?.Invoke(string.Format("EwWatcher Error: {0}", ex.Message));
            }

            tnode = null;
            manager = null;
            xdoc = null;

            Globals.AddEwArg(ew);
            eOnEvet?.Invoke(ew);
           OutPut tout =  da.InsertEwValue(ew);
            if(!tout.OutBool)
                eOnError?.Invoke(string.Format("EwWatcher Error: {0}", tout.OriginalStrErr));
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            t1 = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            t1.Elapsed += T1_Elapsed;
            t1.AutoReset = true;
            t1.Enabled = true;
        }

        private void T1_Elapsed(object sender, ElapsedEventArgs e)
        {

            ////eOnMessage?.Invoke("Timer loop");
            //if (Globals.EwArgs.Count>0)
            //{
            //    OutPut tout = null;
            //    lock (Globals._EwBufferLock)
            //    {
            //        tout =  da.InsertEsValues(Globals.EwArgs);
            //        Globals.EwArgs.Clear();
            //    }
            //    if(tout.OutBool == false)
            //        eOnError?.Invoke(string.Format("EwWatcher Error: {0}", tout.OriginalStrErr));
            //}

           
        }
    }
}
