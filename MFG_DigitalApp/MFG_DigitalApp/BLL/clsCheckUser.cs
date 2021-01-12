#region Import Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

#endregion

/// <summary>
/// Contents User Authentication related functions
/// </summary>


public class clsCheckUser
{
    #region SQL variable Declaration
    SqlConnection sqlConn;
    SqlCommand sqlCmd;
    SqlDataAdapter sqlAdp;
    DataSet sqlDS;
    #endregion

    #region Class Variable
    clsAuthentication objAuthentication;
    #endregion

    #region Constructor
    public clsCheckUser()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Check valid Username and Password
    public bool checkUser(string username, string password, bool needADAuth = true)
    {
        try
        {
            sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DB_MFG"].ConnectionString);
            sqlConn.Open();

            sqlCmd = new SqlCommand("DM_SP_uservalidate", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@username", username);

            sqlAdp = new SqlDataAdapter(sqlCmd);
            sqlDS = new DataSet();

            sqlAdp.Fill(sqlDS);

            if (sqlDS.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["USERID"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["USERID"]);
                HttpContext.Current.Session["username"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["Username"]);
                HttpContext.Current.Session["FullName"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["Full_Name"]);
                HttpContext.Current.Session["Useremailid"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["Email_Address"]);
                objAuthentication = new clsAuthentication();
                if (!needADAuth)
                    return true;
                else
                {
                   //return objAuthentication.IsAuthenticated("", username, password);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            sqlDS.Dispose();
            sqlAdp.Dispose();
            sqlCmd.Dispose();
            sqlConn.Dispose();

            objAuthentication = null;
        }
    }
    #endregion

    #region Check valid Username and Password
    public DataTable checkUser_authenticate(string useremailID, string userareaCode)
    {
        try
        {
            sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBCONN"].ConnectionString);
            sqlConn.Open();

            sqlCmd = new SqlCommand("[uservalidate_authentication]", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.AddWithValue("@useremailID", useremailID);
            sqlCmd.Parameters.AddWithValue("@userareaCode", userareaCode);

            sqlAdp = new SqlDataAdapter(sqlCmd);
            sqlDS = new DataSet();

            sqlAdp.Fill(sqlDS);

            return sqlDS.Tables[0];
        }
        finally
        {
            sqlDS.Dispose();
            sqlAdp.Dispose();
            sqlCmd.Dispose();
            sqlConn.Dispose();

            objAuthentication = null;
        }
    }
    #endregion

    #region Check valid Username and Password
    public bool check_Authenticate_login(string useremailID, string userareaCode, string userareatype)
    {
        try
        {
            sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBCONN"].ConnectionString);
            sqlConn.Open();

            sqlCmd = new SqlCommand("[uservalidate_Login]", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.AddWithValue("@useremailID", useremailID);
            sqlCmd.Parameters.AddWithValue("@userareaCode", userareaCode);
            sqlCmd.Parameters.AddWithValue("@userareatype", userareatype);
            sqlAdp = new SqlDataAdapter(sqlCmd);
            sqlDS = new DataSet();

            sqlAdp.Fill(sqlDS);

            if (sqlDS.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["usercode"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["usercode"]);
                HttpContext.Current.Session["userfullname"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["username"]);
                HttpContext.Current.Session["usermobile"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["usermobile"]);
                HttpContext.Current.Session["useremailid"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["useremailid"]);

                HttpContext.Current.Session["userid"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["userid"]);
                HttpContext.Current.Session["userareacode"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["userareacode"]);
                HttpContext.Current.Session["usertype"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["usertype"]);
                HttpContext.Current.Session["userareatype"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["userareatype"]);
                HttpContext.Current.Session["dpassword"] = Convert.ToString(sqlDS.Tables[0].Rows[0]["userpassword"]);

                //if (Convert.ToString(sqlDS.Tables[0].Rows[0]["userareatype"]).ToLower() != "dist" && Convert.ToString(sqlDS.Tables[0].Rows[0]["userareatype"]).ToLower() != "isr")
                //{
                //    objAuthentication = new clsAuthentication();
                //    return objAuthentication.IsAuthenticated(System.Configuration.ConfigurationManager.AppSettings["DomainName"], username, password);
                //    //return true;
                //}
                //else
                //{
                //    return true;
                //}
                return true;
            }
            else
            {
                return false;
            }
        }
        finally
        {
            sqlDS.Dispose();
            sqlAdp.Dispose();
            sqlCmd.Dispose();
            sqlConn.Dispose();

            objAuthentication = null;
        }
    }
    #endregion
    public DataTable fillUserareacode(string useremailID, string userareaCode)
    {
        try
        {
            sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBCONN"].ConnectionString);
            sqlConn.Open();

            sqlCmd = new SqlCommand("[user_addtional_mapping]", sqlConn);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.AddWithValue("@useremailID", useremailID);
            sqlCmd.Parameters.AddWithValue("@userareaCode", userareaCode);

            sqlAdp = new SqlDataAdapter(sqlCmd);
            sqlDS = new DataSet();

            sqlAdp.Fill(sqlDS);

            return sqlDS.Tables[0];
        }
        finally
        {
            sqlAdp.Dispose();
            sqlConn.Dispose();
        }
    }
}
