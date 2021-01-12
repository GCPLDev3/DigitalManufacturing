#region Import Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Diagnostics;
#endregion


    public class clsException
    {
        #region Write Error Log File
        public void writeErrorLog(Exception ex)
        {
           FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["path_errorlog"], FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);

            StackTrace st = new StackTrace(ex, true);

            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                sw.WriteLine(DateTime.Now.ToString() + " \t " + ex.Source + " \t " + Convert.ToString(HttpContext.Current.Session["userareacode"]) + " \t " + Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]) + " \t " + ex.Message + " \t " + sf.GetMethod().Name + " \t " + sf.GetFileLineNumber() + " \t " + sf.GetFileName());
                sw.Flush();
            }

            sw.Dispose();
            fs.Dispose();

            ErrMsg(ex);
        }
        #endregion

       

        #region Write Upload Log File Status
        public void writeUploadLogstatus(string str)
        {
            FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["path_errorlog"], FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(DateTime.Now.Date.ToString("dd-MMM-yyyy") + " \t " + DateTime.Now.TimeOfDay.ToString("hhmmss") + " \t " + (Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]) == null ? "NULL" : Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"])) + " \t " + (Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) == null ? "NULL" : Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])) + " \t " + HttpContext.Current.Session["usercode"] + " \t " + str );
            sw.Flush();

            sw.Dispose();
            fs.Dispose();
        }
        #endregion

        #region Display Error Message to User
        public void ErrMsg(Exception ex)
        {
            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "MessagePopUp", "alert('" + ex.Message.ToString() + "');", true);
        }
        #endregion
    }
