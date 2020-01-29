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
        Dictionary<string, string> Accesses;

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

            SetDictionary();

            StringBuilder sb = new StringBuilder();
            sb.Append("<QueryList>");
            sb.Append(@"<Query Id=""0"">");
            sb.Append(@"<Select Path=""Security"">");
            sb.Append(@"*[System[(EventID=4663 or EventID=5145)");
           // sb.Append(@"*[System[(EventID=4663 or EventID=5145 or EventID=5140 or EventID=4660 or EventID=4656 or EventID=4658)");
            sb.Append(@"]]");
          //  sb.Append(@" and TimeCreated[@SystemTime &gt;= '" + DateTime.Now.AddMinutes(-1).ToUniversalTime().ToString("o") + "']]]");
            //  sb.Append(@" and *[EventData[Data[@Name =""ObjectName""] and (Data ='" + arg.Fullname + "')]]");
            sb.Append(@"</Select>");
            sb.Append(@"</Query>");
            sb.Append(@"</QueryList>");

            EventLogQuery query = new EventLogQuery("Security", PathType.LogName, sb.ToString());
            ewatch = new EventLogWatcher(query);
            ewatch.EventRecordWritten += Ewatch_EventRecordWritten;

            SetTimer();
        }
        public void StartMonitor()
        {
         
            if (ewatch == null)
                Prepare();
            try
            {
                ewatch.Enabled = true;
                t1.Enabled = true;
                eOnMessage?.Invoke("EwWatcher started");
            }
            catch (Exception ex)
            {
                eOnError?.Invoke(string.Format("EwWatcher Error: {0}", ex.Message));
               
            }
        
        }

        public void StopMonitor()
        {
          
            ewatch.Enabled = false;
            t1.Enabled = false;
            eOnMessage?.Invoke("EwWatcher stopped");
        }

        private void Ewatch_EventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
            // e.EventException.Message
            if(e.EventException != null)
                eOnError?.Invoke(string.Format("EwWatcher Error: {0}", e.EventException.Message));

            //if (OutFlag)
            //    ewatch.Enabled = false;
            //   else
            Task.Run(() => EvalEventData(e.EventRecord));
            // EvalEventData(e.EventRecord);
           
        }

        private void EvalEventData(EventRecord er)
        {
            
            ewArgument ew = new ewArgument();        
            ew.MachineName = er.MachineName;
            ew.Name = er.ProviderName;   // todo ??          
            ew.EventID = er.Id;
            ew.RecordID = er.RecordId == null ? 0 : (long)er.RecordId;
            ew.TimeGenerated = er.TimeCreated == null ? DateTime.MinValue : (DateTime) er.TimeCreated;

          //  eOnMessage?.Invoke(string.Format("RecordId:{0} TimeGenerated:{1}",ew.RecordID,ew.TimeGenerated.ToString("dd/MM/yyyy hh:mm:ss.fff")));
            //ew.ActivityID = er.ActivityId.ToString();
            //ew.RelatedActivityID = er.RelatedActivityId.ToString();


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
                   // ew.AccessList = tnode.InnerText;
                    ew.AccessList = TranslateAccessCodes(tnode.InnerText);
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
            if(Globals.ShowEvents)
              eOnEvet?.Invoke(ew);
            lock (Globals._DBLock)
            {
                OutPut tout = da.InsertEwValue(ew);          
                if (!tout.OutBool)
                    eOnError?.Invoke(string.Format("EwWatcher Error: {0}", tout.OriginalStrErr));
            }

        }

        private void SetTimer()
        {
            t1 = new System.Timers.Timer(1000*60*5);
            t1.Elapsed += T1_Elapsed;
            t1.AutoReset = true;
            t1.Enabled = true;
        }

        private void T1_Elapsed(object sender, ElapsedEventArgs e)
        {

            eOnMessage?.Invoke("EW Timer loop");
            Globals.ClearEwList();
           
        }

        void SetDictionary()
        {
            if (Accesses == null)
                Accesses = new Dictionary<string, string>();
            else
                Accesses.Clear();

            Accesses.Add("4416", "ReadData");
            Accesses.Add("4417", "WriteData");
            Accesses.Add("4418", "AppendData");
            Accesses.Add("4419", "ReadEA");
            Accesses.Add("4420", "WriteEA");
            Accesses.Add("4421", "Execute/Traverse");
            Accesses.Add("4422", "DeleteChild");
            Accesses.Add("4423", "ReadAttributes");
            Accesses.Add("4424", "WriteAttributes");
            Accesses.Add("1537", "DELETE");
            Accesses.Add("1538", "READ_CONTROL");
            Accesses.Add("1539", "WRITE_DAC");
            Accesses.Add("1540", "WRITE_OWNER");
            Accesses.Add("1541", "SYNCHRONIZE");
            Accesses.Add("1542", "ACCESS_SYS_SEC");
        }

        string TranslateAccessCodes(string pstraccess)
        {
            string back = "";
            string[] spearator = { "%%", " " };
            string[] ss = pstraccess.Split(spearator, StringSplitOptions.RemoveEmptyEntries);
            foreach(string s in ss)
            {
                back += ConvertACodeStr(s.Trim()) + " | ";
            }
            if (back.EndsWith(" | "))
                back = back.Remove(back.Length - 3);
            ss = null;
            return back;
        }

        string ConvertACodeStr(string strcode)
        {
            string back = "";
            Accesses.TryGetValue(strcode, out back);
            return back;
        }
    }
}
