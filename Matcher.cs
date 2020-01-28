using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace EventLogReader
{
    public class Matcher
    {
        DateTime LastEvent;
        TimeSpan offset; // fsWatcher ile EventWatcher timing arası
        System.Timers.Timer t1;
        DataAccess da;
        Thread thMatch;
        static volatile bool OutFlag;
        public event FsErrorEventHandler eOnError;
        public event FsMessageEventHandler eOnMessage;
        public event FsEventHandler eOnEvent;
        public Matcher()
        {
            LastEvent = new DateTime();
            da = new DataAccess();
            offset = new TimeSpan(0, 0, 0, Globals.Config.OffsetSeconds, 0);
            t1 = new System.Timers.Timer();
            t1.Elapsed += T1_Elapsed;
            t1.Interval = 1000*60*10;
            t1.AutoReset = true;
            OutFlag = false;
        }

        private void T1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            eOnMessage?.Invoke("Matcher Timer loop");
            Globals.ClearFsList();
        }

        ~Matcher()
        {
            da = null;
            t1.Enabled = false;
            t1 = null;
        }

        public void Start()
        {
            StartThread();
            eOnMessage?.Invoke("Matcher started");
            t1.Enabled = true;
        }

        public void Stop()
        {
            StopThread();
            eOnMessage?.Invoke("Matcher stopped");
            t1.Enabled = false;
        }
       
        void StartThread()
        {
            StopThread();
            OutFlag = false;
            thMatch = new Thread(new ThreadStart(GoMatch));
            thMatch.Start();
        }

        void StopThread()
        {
            if(thMatch != null)
            {
                OutFlag = true;
                thMatch.Join(3000);
                thMatch = null;
            }
        }

        public void Match()
        {
            List<ewArgument> tempList = null;
            TimeSpan waitTime = new TimeSpan(0, 0, 2);
            OutPut tout = null;
            while (true)
            {
              //  eOnMessage?.Invoke("Match Loop");
                fsArgument temp = Globals.GetNextFs();
                if (temp != null)
                {
                    temp.ScanCount++;
                    // delete match
                    if (temp.ChangeType == (int)WatcherChangeTypes.Deleted)
                    {
                      
                        tempList = Globals.EwArgs.FindAll(x => (x.EventID == 4663 || x.EventID == 5145) && x.Stat == FStat.None);
                      //  eOnMessage?.Invoke("Event Log Count for Match: " + tempList.Count.ToString());
                        foreach (ewArgument e in tempList)
                        {
                            if ( e.EventID == 5145 && e.AccessList.Contains("DELETE") && GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened))
                            {
                                eOnMessage?.Invoke("Match Scanning Shared Delete: " + temp.Name);
                                if (temp.Name == e.RelativeTargetName)
                                {
                                    eOnMessage?.Invoke("Match found");
                                
                                    temp.SourceIp = e.IpAddress;
                                    temp.User = e.DomainName + @"\" + e.UserName;
                                    temp.Stat = FStat.Matched;
                                    e.Stat = FStat.Matched;
                                    tout = da.SaveFsValue(temp);
                                    if (!tout.OutBool)
                                    {
                                        eOnError?.Invoke(string.Format("Matcher Error: {0}", tout.OriginalStrErr));
                                        temp.Stat = FStat.Failed;
                                    }
                                    else
                                    {
                                        temp.Stat = FStat.Completed;
                                        e.Stat = FStat.Completed;
                                    }                                      
                                    LastEvent = e.TimeGenerated;
                                    break;
                                }
                            }
                            if (e.EventID == 4663 && e.AccessList.Contains("DELETE") && GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened))
                            {
                                eOnMessage?.Invoke("Match Scanning Delete: " + temp.Name);

                                if (temp.FullName == e.ObjectName)
                                {
                                    eOnMessage?.Invoke("Match found");
                                    temp.SourceIp = "127.0.0.1";
                                    temp.User = e.DomainName + @"\" + e.UserName;
                                    temp.Stat = FStat.Matched;
                                    e.Stat = FStat.Matched;
                                    tout = da.SaveFsValue(temp);
                                    if (!tout.OutBool)
                                    {
                                        eOnError?.Invoke(string.Format("Matcher Error: {0}", tout.OriginalStrErr));
                                        temp.Stat = FStat.Failed;
                                    }
                                    else
                                    {
                                        temp.Stat = FStat.Completed;
                                        e.Stat = FStat.Completed;                                      
                                    }
                                    LastEvent = e.TimeGenerated;
                                    break;
                                }
                            }
                            //   e.Stat = FStat.NotUsed;
                          
                            if (OutFlag)
                                break;
                        }                    
                    }

                    //todo önce write data bul, sonra aynı time'da bir tane objectname al// object name tutmuyor
                    if (temp.ChangeType == (int)WatcherChangeTypes.Created
                        || temp.ChangeType == (int)WatcherChangeTypes.Changed
                        || temp.ChangeType == (int)WatcherChangeTypes.Renamed
                        )
                    {                        
                        eOnMessage?.Invoke("Match Scanning Create: " + temp.FullName);
                        tempList = Globals.EwArgs.FindAll(x => (x.EventID == 4663 || x.EventID == 5145) && x.Stat == FStat.None && x.AccessList.Contains("WriteData"));                     
                        
                        foreach (ewArgument e in tempList)
                        {
                            if (GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened))
                            {

                                if(e.EventID == 5145 && temp.Name == e.RelativeTargetName)
                                {
                                        eOnMessage?.Invoke("Match found");
                                        temp.SourceIp = e.IpAddress;
                                        temp.User = e.DomainName + @"\" + e.UserName;
                                        temp.Stat = FStat.Matched;
                                        e.Stat = FStat.Matched;
                                        tout = da.SaveFsValue(temp);
                                        if (!tout.OutBool)
                                        {
                                            eOnError?.Invoke(string.Format("Matcher Error: {0}", tout.OriginalStrErr));
                                            temp.Stat = FStat.Failed;
                                        }
                                        else
                                        {
                                            temp.Stat = FStat.Completed;
                                            e.Stat = FStat.Completed;
                                        }
                                        LastEvent = e.TimeGenerated;
                                        break;
                                   
                                }
                                else
                                {
                                    //todo sağlamasını object name ile yap

                                    // bu olmuyor
                                    //int aaa = tempList.FindAll(x => x.TimeGenerated.ToString("MM/dd/yyyy hh:mm:ss.fff") == ee.TimeGenerated.ToString("MM/dd/yyyy hh:mm:ss.fff") && x.ObjectName == temp.FullName).Count;
                                    // eOnMessage?.Invoke("Aday Sayısı:" + aaa.ToString());
                                    // ewArgument t = tempList.Find(x => x.TimeGenerated.ToString("MM/dd/yyyy hh:mm:ss.fff") == ee.TimeGenerated.ToString("MM/dd/yyyy hh:mm:ss.fff") && x.ObjectName == temp.FullName);
                                    //  ewArgument t = tempList.Find(x => x.TimeGenerated.ToString("MM/dd/yyyy hh:mm:ss.fff") == ee.TimeGenerated.ToString("MM/dd/yyyy hh:mm:ss.fff"));
                                    //if (t != null)
                                    //{


                                    eOnMessage?.Invoke("Match found");
                                    temp.SourceIp = "127.0.0.1";
                                    temp.User = e.DomainName + @"\" + e.UserName;
                                    temp.Stat = FStat.Matched;
                                    e.Stat = FStat.Matched;
                                    tout = da.SaveFsValue(temp);
                                    if (!tout.OutBool)
                                    {
                                        eOnError?.Invoke(string.Format("Matcher Error: {0}", tout.OriginalStrErr));
                                        temp.Stat = FStat.Failed;
                                    }
                                    else
                                    {
                                        temp.Stat = FStat.Completed;
                                        e.Stat = FStat.Completed;
                                    }

                                    LastEvent = e.TimeGenerated;
                                    break;
                                }
                            }
                        }
                    }



                 
                    if (temp.ScanCount > 12)
                        temp.Stat = FStat.Failed;
                    eOnEvent?.Invoke(temp);
                    if(tempList != null)
                        tempList.Clear();
                }
                if (OutFlag)
                    break;
                Thread.Sleep(waitTime);
               

                
            }
           
           
        }

        public void GoMatch()  // todo add time conditions
        {
            List<ewArgument> tempList = null;
            TimeSpan waitTime = new TimeSpan(0, 0, 2);
            OutPut tout = null;
            while(true)
            {
                fsArgument temp = Globals.GetNextFs();
                if (temp != null)
                {
                    temp.ScanCount++;

                    if (temp.ChangeType == (int)WatcherChangeTypes.Deleted)
                    {
                        tempList = Globals.EwArgs.FindAll(x => (x.EventID == 4663 || x.EventID == 5145) && x.AccessList.Contains("DELETE")  && x.Stat == FStat.None );
                        foreach (ewArgument e in tempList)
                        {
                            if(e.EventID == 5145 && e.RelativeTargetName == temp.Name && GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened))
                            {
                                eOnMessage?.Invoke("Match found");

                                temp.SourceIp = e.IpAddress;
                                temp.User = e.DomainName + @"\" + e.UserName;
                                temp.Stat = FStat.Matched;
                                temp.RefRecordID = e.RecordID;
                                e.Stat = FStat.Matched;
                                tout = da.SaveFsValue(temp);
                                if (!tout.OutBool)
                                {
                                    eOnError?.Invoke(string.Format("Matcher Error: {0}", tout.OriginalStrErr));
                                    temp.Stat = FStat.Failed;
                                }
                                else
                                {
                                    temp.Stat = FStat.Completed;
                                    e.Stat = FStat.Completed;
                                }
                                LastEvent = e.TimeGenerated;
                                break;
                            }

                            if (e.EventID == 4663 && e.ObjectName == temp.FullName && GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened))
                            {
                                eOnMessage?.Invoke("Match found");
                                temp.SourceIp = "127.0.0.1";
                                temp.User = e.DomainName + @"\" + e.UserName;
                                temp.RefRecordID = e.RecordID;
                                tout = da.SaveFsValue(temp);
                                if (!tout.OutBool)
                                {
                                    eOnError?.Invoke(string.Format("Matcher Error: {0}", tout.OriginalStrErr));
                                    temp.Stat = FStat.Failed;
                                }
                                else
                                {
                                    temp.Stat = FStat.Matched;
                                    e.Stat = FStat.Matched;
                                }
                                LastEvent = e.TimeGenerated;
                                break;
                            }

                        }
                    }

                    if (temp.ChangeType == (int)WatcherChangeTypes.Created || temp.ChangeType == (int)WatcherChangeTypes.Changed || temp.ChangeType == (int)WatcherChangeTypes.Renamed)
                    {
                      
                        tempList = Globals.EwArgs.FindAll(x => (x.EventID == 4663 || x.EventID == 5145) && x.AccessList.Contains("WriteData") && (x.Stat == FStat.None || x.Stat == FStat.Matched));
                        foreach (ewArgument e in tempList)
                        {
                            if (e.EventID == 5145 && e.RelativeTargetName == temp.Name && GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened))
                            {
                                eOnMessage?.Invoke("Match found");

                                temp.SourceIp = e.IpAddress;
                                temp.User = e.DomainName + @"\" + e.UserName;
                                temp.RefRecordID = e.RecordID;
                                tout = da.SaveFsValue(temp);
                                if (!tout.OutBool)
                                {
                                    eOnError?.Invoke(string.Format("Matcher Error: {0}", tout.OriginalStrErr));
                                    temp.Stat = FStat.Failed;
                                }
                                else
                                {
                                    temp.Stat = FStat.Matched;
                                    e.Stat = FStat.Matched;
                                }
                                LastEvent = e.TimeGenerated;
                                break;
                            }
                            // todo object name
                            if (e.EventID == 4663 && GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened))
                            {
                                if (Globals.EwArgs.Exists(x => x.EventID == 4663 && x.ObjectName == temp.FullName && (x.Stat == FStat.None || x.Stat == FStat.Matched) && GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened)))
                                {
                                    eOnMessage?.Invoke("Match found");
                                    temp.SourceIp = "127.0.0.1";
                                    temp.User = e.DomainName + @"\" + e.UserName;
                                    temp.RefRecordID = e.RecordID;
                                    tout = da.SaveFsValue(temp);
                                    if (!tout.OutBool)
                                    {
                                        eOnError?.Invoke(string.Format("Matcher Error: {0}", tout.OriginalStrErr));
                                        temp.Stat = FStat.Failed;
                                    }
                                    else
                                    {
                                        temp.Stat = FStat.Matched;
                                        e.Stat = FStat.Matched;
                                    }
                                    LastEvent = e.TimeGenerated;
                                    break;
                                }
                            }

                           // e.Stat = FStat.NotUsed;
                        }
                    }

                    if (temp.ScanCount > 12)
                        temp.Stat = FStat.Failed;

                    eOnEvent?.Invoke(temp);

                    if (tempList != null)
                        tempList.Clear();

                }
                    if (OutFlag)
                    break;
                Thread.Sleep(waitTime);
            }
        }

        bool GetOffsetTimeDifference(DateTime dt1, DateTime dt2)
        {
            TimeSpan fark;
            if (dt1 < dt2)
                fark = dt2 - dt1;
            else
                fark = dt1 - dt2;
            if (offset > fark)
            {
                eOnMessage?.Invoke("TimeDifference Accept " );
                return true;
            }
              
            else
            {
                eOnMessage?.Invoke("TimeDifference Reject ");
                return false;
            }
               
        }
    }
}
