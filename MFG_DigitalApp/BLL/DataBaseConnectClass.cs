using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web;
using MFG_DigitalApp.Log;

public class DataBaseConnectClass
{
    public string connectionString = ConfigurationManager.ConnectionStrings["DB_MFG"].ConnectionString;
    public string connectionStringGDRJ = ConfigurationManager.ConnectionStrings["DB_GDRJ"].ConnectionString;
    SqlConnection con = null;
    private static readonly ILogger _logger = Logger.GetLogger("DataBaseConnectClass");
    public int ExecuteNonQuery_WithParam(SqlParameter[] param, string SPName)
    {
        try
        {
            int return_value = 0;
            con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i <= param.Length - 1; i++)
            {
                cmd.Parameters.AddWithValue(param[i].ToString(), param[i].Value);
            }
            con.Open();
            cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
            return_value = cmd.ExecuteNonQuery();
            int retval = (int)cmd.Parameters["@retValue"].Value;
            if (retval > 0)
                return_value = retval;
            con.Close();

            return return_value;
        }
        catch (Exception ex)
        {
            _logger.Error(string.Concat("ExecuteNonQuery_WithParam::", ex.Message), ex);
            return 0;
        }
    }

    public int ExecuteNonQuery_NoParam(string SPName)
    {
        try
        {
            int iCnt = 0;
            con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            iCnt = cmd.ExecuteNonQuery();
            con.Close();
            return iCnt;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public int ExecuteNonQuery_NoParam_Query(string SPName)
    {
        try
        {
            int iCnt = 0;
            con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            iCnt = cmd.ExecuteNonQuery();
            con.Close();
            return iCnt;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public object ExecuteScalar_WithParam(SqlParameter[] param, string SPName)
    {
        try
        {
            object iCnt;
            con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i <= param.Length - 1; i++)
            {
                cmd.Parameters.AddWithValue(param[i].ToString(), param[i].Value);
            }
            con.Open();
            iCnt = cmd.ExecuteScalar();

            con.Close();
            return iCnt;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public object ExecuteScalar_WithParam_Qry(string SPName)
    {
        try
        {
            object iCnt;
            con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            iCnt = cmd.ExecuteScalar();

            con.Close();
            return iCnt;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public DataTable GetDatatable_NoParam(string SPName)
    {
        try
        {
            DataTable DT = new DataTable();
            con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter(SPName, con);
            da.Fill(DT);

            return DT;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public DataSet GetDataSet_NoParam(string SPName)
    {
        try
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter(SPName, con);
            da.Fill(DS);

            return DS;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public DataSet GetDataTable_WithDTParam(int Type, DataTable dt, DataTable DTD, string FromDate, string ToDate, string SPName)
    {
        try
        {
            DataSet ds = new DataSet();
            con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@FromDate", FromDate);
            cmd.Parameters.AddWithValue("@ToDate", ToDate);
            cmd.Parameters.AddWithValue("@P", dt);
            cmd.Parameters.AddWithValue("@t", DTD);
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            con.Open();
            da.Fill(ds);
            con.Close();
            return ds;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    public int Execute_NonQuery(string CreatedBy, DataTable dt, DataTable DTD, string FromDate, string ToDate, string SPName)
    {
        try
        {
            DataSet ds = new DataSet();
            con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("@FromDate", FromDate);
            cmd.Parameters.AddWithValue("@ToDate", ToDate);
            cmd.Parameters.AddWithValue("@P", dt);
            cmd.Parameters.AddWithValue("@t", DTD);


            int i = cmd.ExecuteNonQuery();

            return i;
        }
        catch (Exception ex)
        {
            return 0;
        }

    }

    public int Execute_NonQuery_Shipment(DataTable dt, DataTable DTD, string FromDate, string ToDate, string SPName)
    {
        try
        {
            DataSet ds = new DataSet();
            con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FromDate", FromDate);
            cmd.Parameters.AddWithValue("@ToDate", ToDate);
            cmd.Parameters.AddWithValue("@P", dt);
            cmd.Parameters.AddWithValue("@t", DTD);


            int i = cmd.ExecuteNonQuery();

            return i;
        }
        catch (Exception ex)
        {
            return 0;
        }

    }

    public DataTable GetDatatable_WithParam(SqlParameter[] param, string SPName)
    {
        try
        {
            DataTable DT = new DataTable();
            con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;
            for (int i = 0; i <= param.Length - 1; i++)
            {
                cmd.Parameters.AddWithValue(param[i].ToString(), param[i].Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            con.Open();
            da.Fill(DT);
            con.Close();
            return DT;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public DataSet GetDataSet_WithParam(SqlParameter[] param, string SPName)
    {
        try
        {
            DataSet DT = new DataSet();
            con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 600;
            for (int i = 0; i <= param.Length - 1; i++)
            {
                cmd.Parameters.AddWithValue(param[i].ToString(), param[i].Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(DT);

            return DT;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public int ExecuteNonQuery_WithParam_withConnection(SqlParameter[] param, string SPName, SqlConnection con)
    {
        try
        {
            con.Close();
            int return_value = 0;
            //con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i <= param.Length - 1; i++)
            {
                cmd.Parameters.AddWithValue(param[i].ToString(), param[i].Value);
            }
            con.Open();
            cmd.Parameters.Add("@retValue", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
            return_value = cmd.ExecuteNonQuery();
            int retval = (int)cmd.Parameters["@retValue"].Value;
            if (retval > 0)
                return_value = retval;
            con.Close();

            return return_value;
        }
        catch (Exception ex)
        {
            _logger.Error(string.Concat("ExecuteNonQuery_WithParam::", ex.Message), ex);
            return 0;
        }
    }

    public int ExecuteNonQuery_NoParam_Query_withConnection(string SPName,SqlConnection con)
    {
        try
        {
            con.Close();
            int iCnt = 0;
            //con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            iCnt = cmd.ExecuteNonQuery();
            con.Close();
            return iCnt;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public object ExecuteScalar_WithParam_withConnection(SqlParameter[] param, string SPName,SqlConnection con)
    {
        try
        {
            object iCnt;
            con.Close();
            //con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i <= param.Length - 1; i++)
            {
                cmd.Parameters.AddWithValue(param[i].ToString(), param[i].Value);
            }
            con.Open();
            iCnt = cmd.ExecuteScalar();

            con.Close();
            return iCnt;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    #region 

    public DataTable GetDatatable_WithParamInGDRJ(SqlParameter[] param, string SPName)
    {
        try
        {
            DataTable DT = new DataTable();
            con = new SqlConnection(connectionStringGDRJ);
            SqlCommand cmd = new SqlCommand(SPName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            for (int i = 0; i <= param.Length - 1; i++)
            {
                cmd.Parameters.AddWithValue(param[i].ToString(), param[i].Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(DT);

            return DT;
        }
        catch (Exception ex)
        {
            _logger.Error(string.Concat("GetDatatable_WithParamInGDRJ::", ex.Message), ex);
            return null;
        }
    }


    #endregion
    #region Datetime conversion
    public DateTime ConvertTime(String ConversionDate)
    {
        string date = ConversionDate.Substring(0, ConversionDate.IndexOf(' '));
        string Time = ConversionDate.Substring(ConversionDate.IndexOf(' '), ConversionDate.Length - ConversionDate.IndexOf(' '));
        TimeSpan timeSpan = TimeSpan.Parse(Time);
        try
        {
            DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            var result = dateTime.Add(timeSpan);
            return result;
        }
        catch (Exception ex)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", null);
                var result = dateTime.Add(timeSpan);
                return result;
            }
            catch (Exception)
            {
                DateTime dateTime = DateTime.ParseExact(date, "MM-dd-yyyy", null);
                var result = dateTime.Add(timeSpan);
                return result;
            }
        }
    }
    public string ConvertDate(String ConversionDate)
    {
        try
        {
            DateTime dateTime = DateTime.ParseExact(ConversionDate, "yyyy-MM-dd", null);
            return dateTime.ToShortDateString();
        }
        catch (Exception ex)
        {
            try
            {
                DateTime dateTime = DateTime.ParseExact(ConversionDate, "dd-MM-yyyy", null);
                return dateTime.ToShortDateString();
            }
            catch (Exception)
            {
                DateTime dateTime = DateTime.ParseExact(ConversionDate, "MM-dd-yyyy", null);
                return dateTime.ToShortDateString();
            }
        }
    }
    #endregion
}


