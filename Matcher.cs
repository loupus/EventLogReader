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
            offset = new TimeSpan(0, 0, 0, 0, 500); // 500 ms
            t1 = new System.Timers.Timer();
            t1.Elapsed += T1_Elapsed;
            t1.Interval = 10000;
            t1.AutoReset = true;
            OutFlag = false;
        }

        private void T1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
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
            //t1.Enabled = true;
        }

        public void Stop()
        {
            StopThread();
            eOnMessage?.Invoke("Matcher stopped");
            // t1.Enabled = false;
        }

        /*
         https://docs.microsoft.com/en-us/windows/security/threat-protection/auditing/event-5145#table-of-file-access-codes


%%4416  -   ReadData (or ListDirectory)	0x1
%%4417  -   WriteData (or AddFile)	0x2,
%%4418  -   AppendData (or AddSubdirectory or CreatePipeInstance)	0x4,
%%4419  -   ReadEA	0x8,
%%4420  -   WriteEA	0x10,
%%4421  -   Execute/Traverse	0x20,
%%4422  -   DeleteChild	0x40,
%%4423  -   ReadAttributes	0x80,
%%4424  -   WriteAttributes	0x100,
%%1537  -   DELETE	0x10000,
%%1538  -   READ_CONTROL	0x20000,
%%1539  -   WRITE_DAC	0x40000,
%%1540  -   WRITE_OWNER	0x80000,
%%1541  -   SYNCHRONIZE	0x100000
%%1542  -   ACCESS_SYS_SEC	0x1000000,


"AO"	Account operators	
"RU"	Alias to allow previous Windows 2000	
"AN"	Anonymous logon
"AU"	Authenticated users	
"BA"	Built-in administrators
"BG"	Built-in guests	
"BO"	Backup operators	
"BU"	Built-in users	
"CA"	Certificate server administrators	
"CG"	Creator group	
"CO"	Creator owner	
"DA"	Domain administrators	
"DC"	Domain computers	
"DD"	Domain controllers	
"DG"	Domain guests	
"DU"	Domain users	
"EA"	Enterprise administrators	
"ED"	Enterprise domain controllers	
"WD"	Everyone	
"PA"	Group Policy administrators
"IU"	Interactively logged-on user
"LA"	Local administrator
"LG"	Local guest
"LS"	Local service account
"SY"	Local system
"NU"	Network logon user
"NO"	Network configuration operators
"NS"	Network service account
"PO"	Printer operators
"PS"	Personal self
"PU"	Power users
"RS"	RAS servers group
"RD"	Terminal server users
"RE"	Replicator
"RC"	Restricted code
"SA"	Schema administrators
"SO"	Server operators
"SU"	Service logon user

G: = Primary Group.
D: = DACL Entries.
S: = SACL Entries.

  ace_type:
"A" - ACCESS ALLOWED
"D" - ACCESS DENIED
"OA" - OBJECT ACCESS ALLOWED: only applies to a subset of the object(s).
"OD" - OBJECT ACCESS DENIED: only applies to a subset of the object(s).
"AU" - SYSTEM AUDIT
"A" - SYSTEM ALARM
"OU" - OBJECT SYSTEM AUDIT
"OL" - OBJECT SYSTEM ALARM

            ace_flags:
"CI" - CONTAINER INHERIT: Child objects that are containers, such as directories, inherit the ACE as an explicit ACE.
"OI" - OBJECT INHERIT: Child objects that are not containers inherit the ACE as an explicit ACE.
"NP" - NO PROPAGATE: only immediate children inherit this ace.
"IO" - INHERITANCE ONLY: ace doesn’t apply to this object, but may affect children via inheritance.
"ID" - ACE IS INHERITED
"SA" - SUCCESSFUL ACCESS AUDIT
"FA" - FAILED ACCESS AUDIT

         */


        void StartThread()
        {
            StopThread();
            OutFlag = false;
            thMatch = new Thread(new ThreadStart(Match));
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
                                    OutPut tout = da.SaveFsValue(temp);
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
                                    OutPut tout = da.SaveFsValue(temp);
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
                    if (temp.ChangeType == (int)WatcherChangeTypes.Created)
                    {
                        
                        eOnMessage?.Invoke("Match Scanning Create: " + temp.FullName);
                        tempList = Globals.EwArgs.FindAll(x => x.EventID == 4663 && x.Stat == FStat.None && x.AccessList.Contains("WriteData"));
                       
                        
                        foreach (ewArgument e in tempList)
                        {
                            if (GetOffsetTimeDifference(e.TimeGenerated, temp.WhenHappened))
                            {
                             

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
                                    OutPut tout = da.SaveFsValue(temp);
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
                              //  }
                            }
                        }
                    }

                 
                    if (temp.ScanCount > 12)
                        temp.Stat = FStat.Failed;
                    eOnEvent?.Invoke(temp);
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
