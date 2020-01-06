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

namespace EventLogReader
{
  
    class EventMonitor
    {
        public event EwEventHandler eOnEvet;
        public event FsErrorEventHandler eOnError;
        EventLogWatcher ewatch;
        
       
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
            StringBuilder sb = new StringBuilder();
            sb.Append("<QueryList>");
            sb.Append(@"<Query Id=""0"">");
            sb.Append(@"<Select Path=""Security"">");
            sb.Append(@"*[System[(EventID=4663 or EventID=5145 or EventID=5140 or EventID=4660)");
            sb.Append(@" and TimeCreated[@SystemTime &gt;= '" + DateTime.Now.AddMinutes(-1).ToUniversalTime().ToString("o") + "']]]");
            //  sb.Append(@" and *[EventData[Data[@Name =""ObjectName""] and (Data ='" + arg.Fullname + "')]]");
            sb.Append(@"</Select>");
            sb.Append(@"</Query>");
            sb.Append(@"</QueryList>");

            EventLogQuery query = new EventLogQuery("Security", PathType.LogName, sb.ToString());
            ewatch = new EventLogWatcher(query);
            ewatch.EventRecordWritten += Ewatch_EventRecordWritten;
        }



        public void StartMonitor()
        {
            if (ewatch == null)
                Prepare();
            try
            {
                ewatch.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

        public void StopMonitor()
        {
            ewatch.Enabled = false;
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
            ew.Name = er.LogName;
            ew.EventID = er.Id;
            ew.RecordID = er.RecordId == null ? 0 : (long)er.RecordId;
            ew.TimeGenerated = er.TimeCreated == null ? DateTime.MinValue : (DateTime) er.TimeCreated;

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

            }
            catch (Exception ex)
            {

                eOnError?.Invoke(string.Format("EwWatcher Error: {0}", ex.Message));
            }

            tnode = null;
            manager = null;
            xdoc = null;

            eOnEvet?.Invoke(ew);
        }
      
    }
}
