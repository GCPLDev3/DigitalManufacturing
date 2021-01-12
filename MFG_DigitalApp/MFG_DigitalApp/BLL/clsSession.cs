#region Import Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

/// <summary>
/// Content Session related Function
/// </summary>


public class clsSession
{

    #region Constructor
    public clsSession()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Check Valid Session
    public void checkSession()
    {
        if (HttpContext.Current.Session["main_userCode"] == null)
        {
            distroySession();
        }
    }
    #endregion

    #region Distroy Session
    public void distroySession()
    {
        HttpContext.Current.Response.Cookies["sso_username"].Expires = DateTime.Now.AddDays(-1d);
        HttpContext.Current.Response.Cookies["sso_password"].Expires = DateTime.Now.AddDays(-1d);

        //HttpContext.Current.Response.Cookies["userareacode"].Expires = DateTime.Now.AddDays(-1d);
        //HttpContext.Current.Response.Cookies["usertype"].Expires = DateTime.Now.AddDays(-1d);
        //HttpContext.Current.Response.Cookies["userareatype"].Expires = DateTime.Now.AddDays(-1d);
        HttpContext.Current.Response.Cookies["usercode"].Expires = DateTime.Now.AddDays(-1d);
        //HttpContext.Current.Response.Cookies["IsMappedUser"].Expires = DateTime.Now.AddDays(-1d);
        HttpContext.Current.Session.Abandon();
        // HttpContext.Current.Response.Cookies.Clear();
        HttpContext.Current.Response.Redirect("~\\Login.aspx");
    }
    #endregion

    #region Display Welcome name
    public string welcomeuser()
    {
        Page page = HttpContext.Current.Handler as Page;
        string welcomestr = string.Empty;

        if (HttpContext.Current.Session["userfullname"].ToString() != "")
            welcomestr += HttpContext.Current.Session["userfullname"].ToString() + " [" + HttpContext.Current.Session["usercode"].ToString() + "]";
        else
            welcomestr += HttpContext.Current.Session["usercode"].ToString();

        if (HttpContext.Current.Session["usermobile"].ToString() != "")
            welcomestr += "<br /> Mob No : " + HttpContext.Current.Session["usermobile"].ToString();

        if (HttpContext.Current.Session["useremailid"].ToString() != "")
        {
            var arr = HttpContext.Current.Session["useremailid"].ToString().Split(new char[] { '@' });
            if (arr.Length > 1)
                welcomestr += "<br /> Email Id : " + Convert.ToString(arr[0]) + "@" + Convert.ToString(arr[1]);
            else
                welcomestr += "<br /> Email Id : " + HttpContext.Current.Session["useremailid"].ToString();
        }
        return ((Label)page.Master.FindControl("lblMasterUsername")).Text = welcomestr;
    }
    #endregion
}
