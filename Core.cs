using System;
using System.Data;


namespace EventLogReader
{

    public enum FStat
    {
        None = 0
       ,OnInvestigation = 1
       ,Failed = 2
       ,NotUsed = 3
       ,NotFound = 4
       ,Completed = 9
    }
    public class fsArgument
    {
        public string ID;
        public string Name;
        public string FullName;
        public string OldName;
        public string OldFullName;
        public int ChangeType;
        public string User;
        public string SourceIp;
        public DateTime WhenHappened;
        public FStat Stat;
        public fsArgument()
        {
            ID = Guid.NewGuid().ToString();
            Stat = FStat.None;
        }
    };

    public class ewArgument
    {
        public int EventID;
        public long RecordID;
        public string ActivityID;
        public string RelatedActivityID;
        public string MachineName;
        public string Name;
        public string UserName;
        public string DomainName;
        public string IpAddress;
        public string ObjectName;
        public string HandleID;
        public string AccessList;
        public string AccessMask;
        public string ProcessName;
        public string RelativeTargetName;
        public DateTime TimeGenerated;
        public FStat Stat;
        public ewArgument()
        {
            Stat = FStat.None;
        }
    };

    public class EwTable : DataTable
    {
        public EwTable()
        {
            Columns.Add("EventID", Type.GetType("System.Int32"));
            Columns.Add("RecordID", Type.GetType("System.Int64"));
            Columns.Add("ActivityID", Type.GetType("System.String"));
            Columns.Add("RelatedActivityID", Type.GetType("System.String"));
            Columns.Add("MachineName", Type.GetType("System.String"));
            Columns.Add("Name", Type.GetType("System.String"));
            Columns.Add("UserName", Type.GetType("System.String"));
            Columns.Add("DomainName", Type.GetType("System.String"));
            Columns.Add("IpAddress", Type.GetType("System.String"));
            Columns.Add("ObjectName", Type.GetType("System.String"));
            Columns.Add("RelativeTargetName", Type.GetType("System.String"));
            Columns.Add("HandleID", Type.GetType("System.String"));
            Columns.Add("AccessList", Type.GetType("System.String"));
            Columns.Add("AccessMask", Type.GetType("System.String"));
            Columns.Add("ProcessName", Type.GetType("System.String"));
            Columns.Add("TimeGenerated", Type.GetType("System.DateTime"));
            Columns.Add("Stat", Type.GetType("System.Int32"));
        }

        ~EwTable()
        {
            Rows.Clear();
            Columns.Clear();
        }
    }

    public class FsTable : DataTable
    {
        public FsTable()
        {
            Columns.Add("ID", Type.GetType("System.String"));
            Columns.Add("Name", Type.GetType("System.String"));
            Columns.Add("Fullname", Type.GetType("System.String"));
            Columns.Add("Oldname", Type.GetType("System.String"));
            Columns.Add("OldFullname", Type.GetType("System.String"));
            Columns.Add("ChangeType", Type.GetType("System.Int32"));
            Columns.Add("User", Type.GetType("System.String"));
            Columns.Add("SourceIp", Type.GetType("System.String"));
            Columns.Add("WhenHappened", Type.GetType("System.DateTime"));
            Columns.Add("Stat", Type.GetType("System.Int32"));
        }
        ~FsTable()
        {
            Rows.Clear();
            Columns.Clear();
        }
    }
}
