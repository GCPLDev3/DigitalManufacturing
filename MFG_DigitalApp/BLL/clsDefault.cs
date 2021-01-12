#region Import Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
#endregion


    public class clsDefault
    {
        #region SQL variable declaration
        SqlConnection sqlConn;
        SqlCommand sqlCmd;
        SqlDataAdapter sqlAdp;
        DataTable sqlDT;
        #endregion

        #region Show Home Page Link
        public DataTable showHomePageLink()
        {
            try
            {
                sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBCONN_RecruitmentPortal"].ConnectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand("[sp_get_Drishti_Homepagelink]", sqlConn);
                
                sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlDT = new DataTable();

                sqlAdp.Fill(sqlDT);

                return sqlDT;
            }
            finally
            {
                sqlDT.Dispose();
                sqlAdp.Dispose();
                sqlCmd.Dispose();
                sqlConn.Dispose();
            }
        }
        #endregion
    }
