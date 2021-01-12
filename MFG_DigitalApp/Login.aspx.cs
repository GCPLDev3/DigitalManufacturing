using MFG_DigitalApp.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MFG_DigitalApp
{
    public partial class Login : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("Login");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        //clsException objException;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strIdUser = (HttpContext.Current.User.Identity.Name.ToString());
            //strIdUser = "GODREJINDS\\impiger.satish";
            if (strIdUser != "")
            {
                string[] parts = strIdUser.Split('\\');
                string StrWindowsUser = parts[parts.Length - 2];
                string StrWindowsUsername = parts[parts.Length - 1];

                if (StrWindowsUser.ToUpper() == "GODREJINDS")
                {
                    clsCheckUser objCheckuser;
                    try
                    {
                        Session.Clear();
                        objCheckuser = new clsCheckUser();
                        _logger.Info("login:" + StrWindowsUsername);
                        if (objCheckuser.checkUser(StrWindowsUsername, "", false) == true)
                        {
                            FormsAuthentication.SetAuthCookie(StrWindowsUsername, false);

                            //modify the Domain attribute of the cookie to the second level of domain
                            System.Web.HttpCookie MyCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(User.Identity.Name.ToString(), false);
                            MyCookie.Domain = "godrejcp.biz";//like miscrosoft.com, dell.com...
                            Response.AppendCookie(MyCookie);

                            Session["main_userCode"] = StrWindowsUsername;
                            Response.Cookies["sso_username"].Value = StrWindowsUsername;

                            Response.Redirect("Home.aspx", false);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(string.Concat("Page_Load::", ex.Message), ex);
                    }
                    finally { objCheckuser = null; }
                }
            }
        }

        protected void CmdLogin_Click(object sender, EventArgs e)
        {
            clsCheckUser objCheckuser;
           Session["Currenttime"] = hdnfldVariable.Value;
            

            try
            {
                txtUserName.Text = txtUserName.Text.Replace("@godrejcp.com", "").Replace("@godrejinds.com", "");

                objCheckuser = new clsCheckUser();

                if (objCheckuser.checkUser(txtUserName.Text.Trim(), txtPassword.Text) == true)
                {
                    _logger.Info("login:" + txtUserName.Text.Trim());

                    FormsAuthentication.SetAuthCookie(txtUserName.Text.Trim(), false);
                    //modify the Domain attribute of the cookie to the second level of domain
                    System.Web.HttpCookie MyCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(User.Identity.Name.ToString(), false);
                    MyCookie.Domain = "godrejcp.biz";//like miscrosoft.com, dell.com...
                    Response.AppendCookie(MyCookie);

                    Session["main_userCode"] = txtUserName.Text.Trim();
                    Response.Cookies["sso_username"].Value = txtUserName.Text.Trim();
                    Response.Cookies["sso_password"].Value = txtPassword.Text;

                    Response.Redirect("ShiftDetails.aspx", false);
                    GetUserRole();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Invalid Username or Password')", true);
                }
            }

            catch (Exception ex)
            {
                _logger.Error(string.Concat("CmdLogin_Click::", ex.Message), ex);
            }

            finally { objCheckuser = null; }

        }
        protected void GetUserRole()
        {
            try
            {

                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "9"),
                 new SqlParameter("@UserId", HttpContext.Current.Session["USERID"].ToString())
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    Session["RoleId"] = Dt.Rows[0]["RoleId"].ToString();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}