﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

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
        String strCon;

        public SqlWorker(String ConString) //default constructor
        {
            strCon = ConString;
            cmd = new SqlCommand();

        }
        public SqlWorker(String ConString, Boolean a) //default constructor
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
        public OutPut _Sorgusuz(string sp, CommandType ct)
        {
            OutPut back = new OutPut(null, "", "", false);
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
        public void _ResetParameters()
        {
            cmd.Parameters.Clear();
        }
        public void _AddParameter(String name, Object value, Boolean ResetParameters = false)
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
    class DataAccess
    {
    }
}
