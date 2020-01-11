using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace EventLogReader
{
    public class OutPut
    {
        public Object OutValue { get; set; }
        public String OutStrErr { get; set; }
        public String OriginalStrErr { get; set; }
        public Boolean OutBool { get; set; }
        public OutPut(Object pOutValue, String pOutStrErr, String pOriginalStrErr, Boolean pOutBool)
        {
            OutValue = pOutValue;
            OutStrErr = pOutStrErr;
            OriginalStrErr = pOriginalStrErr;
            OutBool = pOutBool;
        }
        public static Boolean OutValueExists(OutPut outputu)
        {
            Boolean back = false;
            if (outputu == null) return back;
            if (outputu.OutBool == true && outputu.OutValue != null) back = true;
            return back;
        }
    }

    public class SqlWorker
    {
        SqlConnection con;
        SqlCommand cmd;
        string strCon;

        public SqlWorker(string ConString) //default constructor
        {
            strCon = ConString;
            cmd = new SqlCommand();

        }
        public SqlWorker(string ConString, bool a) //default constructor
        {
            strCon = ConString;
            cmd = new SqlCommand();

        }
        public OutPut _LoadData(string sp, CommandType ct, DataTable dt)
        {
            OutPut back = new OutPut(null, "", "", false);
            con = new SqlConnection(strCon);
            cmd.Connection = con;
            cmd.CommandType = ct;
            cmd.CommandText = sp;


            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                back.OutBool = true;
            }
            catch (SqlException ex)
            {
                back.OutStrErr = "Failed to Load Data";
                back.OriginalStrErr = ex.Message;
                if (con.State != ConnectionState.Closed) con.Close();
                return back;
            }
            catch (Exception ex)
            {
                back.OutStrErr = "Failed to Load Data";
                back.OriginalStrErr = ex.Message;
                if (con.State != ConnectionState.Closed) con.Close();
                return back;
            }

            if (con.State != ConnectionState.Closed) con.Close();
            return back;

        }
        public OutPut _Sorgusuz(string sp, CommandType ct, SqlConnection pcon = null)
        {
            OutPut back = new OutPut(null, "", "", false);
            if(pcon!= null)
                con = pcon;
            else
                con = new SqlConnection(strCon);

            cmd.Connection = con;
            cmd.CommandType = ct;
            cmd.CommandText = sp;

            try
            {
                con.Open();
                back.OutValue = cmd.ExecuteNonQuery();
                back.OutBool = true;

            }
            catch (SqlException ex)
            {
                back.OutStrErr = "Failed to Run Sorgusuz";
                back.OriginalStrErr = ex.Message;
                if (con.State != ConnectionState.Closed) con.Close();
                return back;
            }

            if(pcon == null)
             if (con.State != ConnectionState.Closed) con.Close();

            return back;

        }
        public OutPut _GetScalar(string sp, CommandType ct)
        {
            OutPut back = new OutPut(null, "", "", false);

            con = new SqlConnection(strCon);
            cmd.Connection = con;
            cmd.CommandType = ct;
            cmd.CommandText = sp;
            //  cmd.CommandTimeout = 180;

            try
            {
                con.Open();
                back.OutValue = cmd.ExecuteScalar();
                back.OutBool = true;
            }
            catch (Exception ex)
            {
                back.OutStrErr = "Failed to Get Scalar";
                back.OriginalStrErr = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return back;
        }
        public OutPut _BulkInsert(string tblname, DataTable dt)
        {
            OutPut back = new OutPut(null, "", "", false);
            con = new SqlConnection(strCon);
            SqlBulkCopy bulkcpy = new SqlBulkCopy(con);
            bulkcpy.DestinationTableName = tblname;

           
            try
            {
                con.Open();
                bulkcpy.WriteToServer(dt);
                back.OutBool = true;

            }
            catch (SqlException ex)
            {
                back.OutStrErr = "Failed to Run BulkInsert";
                back.OriginalStrErr = ex.Message;
                if (con.State != ConnectionState.Closed) con.Close();
                return back;
            }

            if (con.State != ConnectionState.Closed) con.Close();
            if (bulkcpy != null) bulkcpy = null;
            return back;

        }
        public void _ResetParameters()
        {
            cmd.Parameters.Clear();
        }
        public void _AddParameter(string name, object value, bool ResetParameters = false)
        {
            if (ResetParameters) { cmd.Parameters.Clear(); }
            cmd.Parameters.AddWithValue(name, value == null ? DBNull.Value : value);
        }
        public void _AddParameter(String pName, SqlDbType tipi, ParameterDirection yonu, Object deger, Boolean ResetParameters)
        {
            if (ResetParameters) { cmd.Parameters.Clear(); }

            SqlParameter a = new SqlParameter();
            a.Direction = yonu;
            a.SqlDbType = tipi;
            a.ParameterName = pName;
            a.Value = deger;
            cmd.Parameters.Add(a);
        }
        public void _AddParameters(List<SqlParameter> pPrms)
        {
            // cmd.Parameters.Clear();
            foreach (SqlParameter p in pPrms)
            {
                cmd.Parameters.Add(p);
            }
        }
        public object _ReturnParValue(String pName) { return cmd.Parameters[pName].Value; }
        ~SqlWorker()
        {
            if (con != null) con.Dispose();
            if (cmd != null) cmd.Dispose();
        }
    };

    public class DataAccess
    {
        SqlWorker sq;
        public DataAccess()
        {
            if(string.IsNullOrEmpty(Globals.ConnectionString))
                Globals.SetSqlConStr();
            sq = new SqlWorker(Globals.ConnectionString);
        }

        ~DataAccess()
        {
            sq = null;
        }

        public OutPut InsertFsValue(fsArgument parg)
        {
            OutPut back = null;
            sq._AddParameter("@pWhenHappened", parg.WhenHappened, true);
            sq._AddParameter("@pName",parg.Name);
            sq._AddParameter("@pFullName", parg.FullName);
            sq._AddParameter("@pOldName", parg.OldName);
            sq._AddParameter("@pOldFullName", parg.OldFullName);
            sq._AddParameter("@pChangeType", parg.ChangeType);
            sq._AddParameter("@pUserName", parg.User);
            sq._AddParameter("@pSourceIp", parg.SourceIp);
            sq._AddParameter("@pStat", (int)parg.Stat);
            back = sq._Sorgusuz("SaveFsValue", CommandType.StoredProcedure);
            return back;
        }

        public OutPut InsertEwValue(ewArgument parg)
        {
            OutPut back = null;
            sq._AddParameter("@pEventID", parg.EventID, true);
            sq._AddParameter("@pRecordID", parg.RecordID);
            sq._AddParameter("@pMachineName", parg.MachineName);
            sq._AddParameter("@pName", parg.Name);
            sq._AddParameter("@pUserName ", parg.UserName);
            sq._AddParameter("@pDomainName", parg.DomainName);
            sq._AddParameter("@pIpAddress", parg.IpAddress);
            sq._AddParameter("@pObjectName", parg.ObjectName);
            sq._AddParameter("@pHandleID", parg.HandleID);
            sq._AddParameter("@pAccessList", parg.AccessList);
            sq._AddParameter("@pAccessMask", parg.AccessMask);
            sq._AddParameter("@pProcessName", parg.ProcessName);
            sq._AddParameter("@pTimeGenerated", parg.TimeGenerated);
            sq._AddParameter("@pStat", (int)parg.Stat);
            back = sq._Sorgusuz("SaveEwValue", CommandType.StoredProcedure);
            return back;
        }

        public OutPut InsertEsValues(List<ewArgument> pList)
        {
            OutPut back = null;
            DataTable dt = ConvertToDataTable(pList);
            back = sq._BulkInsert("eWatcher", dt);
            dt.Clear();
            dt = null;
            return back;
        }
        
        DataTable ConvertToDataTable(List<ewArgument> data)
        {
            EwTable etbl = new EwTable();
            foreach (ewArgument arg in data)
            {
                DataRow dw = etbl.NewRow();
                dw["RecordID"] = arg.RecordID;
                dw["EventID"] = arg.EventID;             
                dw["MachineName"] = arg.MachineName;
                dw["Name"] = arg.Name;
                dw["UserName"] = arg.UserName;
                dw["DomainName"] = arg.DomainName;
                dw["IpAddress"] = arg.IpAddress;
                dw["ObjectName"] = arg.ObjectName;
                dw["HandleID"] = arg.HandleID;
                dw["AccessList"] = arg.AccessList;
                dw["AccessMask"] = arg.AccessMask;
                dw["ProcessName"] = arg.ProcessName;
                dw["TimeGenerated"] = arg.TimeGenerated;
                dw["Stat"] = (int)arg.Stat;
                etbl.Rows.Add(dw);
            }
            return etbl;
        }
    }

}
