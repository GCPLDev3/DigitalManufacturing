using MFG_DigitalApp.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;

namespace MFG_DigitalApp
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        private static readonly ILogger _logger = Logger.GetLogger("MasterPage");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["main_userCode"] != null)
            {
                Label1.Text = Convert.ToString(Session["FullName"]);
                Label4.Text = Convert.ToString(Session["country"]);
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
            if (Convert.ToInt16(Session["RoleID"]) ==2)
            {
                mySidenav.Visible = false;
                mySideNavIcon.Visible = false;
            }
            else
            {
                mySidenav.Visible = true;
                mySideNavIcon.Visible = true;
            }
            // RUN DETAILS
            MenuItem itemRunDetails = new MenuItem();
            itemRunDetails.Text = "Run Details";
            itemRunDetails.NavigateUrl = "#";
            MenuItem FinditemRun = NavigationMenu.FindItem("Run Details");
            if (Request.Url.AbsoluteUri.ToLower().Contains("rundetails") && FinditemRun == null)
            {
                NavigationMenu.Items.Add(itemRunDetails);
            }

            // PRODUCTION ENTRY
            MenuItem itemProdEntry = new MenuItem();
            itemProdEntry.Text = "Hourly Production Entry";
            itemProdEntry.NavigateUrl = "#";
            MenuItem FinditemProd = NavigationMenu.FindItem("Hourly Production Entry");
            if (Request.Url.AbsoluteUri.ToLower().Contains("productiondetails") && FinditemProd == null)
            {
                NavigationMenu.Items.Add(itemProdEntry);
            }

            // STOPPAGES ENTRY
            MenuItem itemStoppageEntry = new MenuItem();
            itemStoppageEntry.Text = "Stoppages Reason Entry";
            itemStoppageEntry.NavigateUrl = "#";
            MenuItem FinditemStoppage = NavigationMenu.FindItem("Stoppages Reason Entry");
            if (Request.Url.AbsoluteUri.ToLower().Contains("stoppagesreason") && FinditemStoppage == null)
            {
                NavigationMenu.Items.Add(itemStoppageEntry);
            }

            // PARAMETER DETAILS
            MenuItem itemParameterEntry = new MenuItem();
            itemParameterEntry.Text = "Shift Parameters";
            itemParameterEntry.NavigateUrl = "#";
            MenuItem FinditemParameter = NavigationMenu.FindItem("Shift Parameters");
            if (Request.Url.AbsoluteUri.ToLower().Contains("parameterdetails") && FinditemParameter == null)
            {
                NavigationMenu.Items.Add(itemParameterEntry);
            }

            //Quality Check

            if (Session["CountryId"] != null&&(!Session["CountryId"].Equals("2")))
            {

                MenuItem itemQualityCheckEntry = new MenuItem();
                itemQualityCheckEntry.Text = "Quality";
                itemQualityCheckEntry.NavigateUrl = "#";
                MenuItem FinditemQualityCheck = NavigationMenu.FindItem("Quality");
                if (Request.Url.AbsoluteUri.ToLower().Contains("qualitycheck") && FinditemQualityCheck == null)
                {
                    NavigationMenu.Items.Add(itemQualityCheckEntry);
                }



                if (Page.TemplateControl.AppRelativeVirtualPath.Equals("~/RunDetails.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    // if (!Convert.ToInt16(Session["RoleID"]) == 1)
                    // {
                    MenuItem itemQualityEntry = new MenuItem();
                    itemQualityEntry.Text = "Quality";
                    itemQualityEntry.NavigateUrl = "QualityCheck.aspx";
                    MenuItem FinditemQuality = NavigationMenu.FindItem("Quality");
                    if (Request.Url.AbsoluteUri.ToLower().Contains("rundetails") && FinditemQuality == null)
                    {
                        NavigationMenu.Items.Add(itemQualityEntry);
                    }
                    //}
                }

            }
            else if (Session["CountryId"] != null&&(Session["CountryId"].Equals("2")&& Session["RoleId"].Equals("3")))
            {
                MenuItem itemQualityCheckEntry = new MenuItem();
                itemQualityCheckEntry.Text = "Quality";
                itemQualityCheckEntry.NavigateUrl = "#";
                MenuItem FinditemQualityCheck = NavigationMenu.FindItem("Quality");
                if (Request.Url.AbsoluteUri.ToLower().Contains("qualitycheck") && FinditemQualityCheck == null)
                {
                    NavigationMenu.Items.Add(itemQualityCheckEntry);
                }



                if (Page.TemplateControl.AppRelativeVirtualPath.Equals("~/RunDetails.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    // if (!Convert.ToInt16(Session["RoleID"]) == 1)
                    // {
                    MenuItem itemQualityEntry = new MenuItem();
                    itemQualityEntry.Text = "Quality";
                    itemQualityEntry.NavigateUrl = "QualityCheck.aspx";
                    MenuItem FinditemQuality = NavigationMenu.FindItem("Quality");
                    if (Request.Url.AbsoluteUri.ToLower().Contains("rundetails") && FinditemQuality == null)
                    {
                        NavigationMenu.Items.Add(itemQualityEntry);
                    }
                    //}
                }
            }
            
            if (Page.TemplateControl.AppRelativeVirtualPath.Equals("~/QualityCheck.aspx", StringComparison.OrdinalIgnoreCase))
            {
                // if (Convert.ToInt16(Session["RoleID"]) == 1)
                //{
                MenuItem itemProductionEntry = new MenuItem();
                itemProductionEntry.Text = "Production";
                itemProductionEntry.NavigateUrl = "RunDetails.aspx";
                MenuItem FinditemProduction = NavigationMenu.FindItem("Production");
                if (Request.Url.AbsoluteUri.ToLower().Contains("qualitycheck") && FinditemProduction == null)
                {
                    NavigationMenu.Items.Add(itemProductionEntry);
                }
                // }
            }

            foreach (MenuItem item in ((Menu)this.FindControl("NavigationMenu")).Items)
            {
                if (Request.Url.AbsoluteUri.ToLower().Contains("shiftdetails") && item.NavigateUrl.ToLower().Contains("shiftdetails"))
                {
                    item.Selected = true;
                }
                else if (Request.Url.AbsoluteUri.ToLower().Contains("rundetails") && item.NavigateUrl.ToLower().Contains("#"))
                {
                    item.Selected = true;
                }
                else if (Request.Url.AbsoluteUri.ToLower().Contains("productiondetails") && item.NavigateUrl.ToLower().Contains("#"))
                {
                    item.Selected = true;
                }
                else if (Request.Url.AbsoluteUri.ToLower().Contains("stoppagesreason") && item.NavigateUrl.ToLower().Contains("#"))
                {
                    item.Selected = true;
                }
                else if (Request.Url.AbsoluteUri.ToLower().Contains("parameterdetails") && item.NavigateUrl.ToLower().Contains("#"))
                {
                    item.Selected = true;
                }
                else if (Request.Url.AbsoluteUri.ToLower().Contains("qualitycheck") && item.NavigateUrl.ToLower().Contains("#"))
                {
                    item.Selected = true;
                }
            }
        }

        void AddChildNodes(DataTable menuData, MenuItem parentMenuItem)
        {
            try
            {
                DataView view = new DataView(menuData);
                view.RowFilter = "ParentID=" + parentMenuItem.Value;
                foreach (DataRowView row in view)
                {
                    MenuItem newMenuItem = new MenuItem(row["MenuName"].ToString(), row["MenuID"].ToString(), "", row["MenuPath"].ToString());
                    if (Convert.ToString(row["MenuPath"]) == "")
                        newMenuItem.Selectable = false;

                    parentMenuItem.ChildItems.Add(newMenuItem);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("AddChildNodes::", ex.Message), ex);
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx", false);
        }
    }
}