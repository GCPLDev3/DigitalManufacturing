using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFG_DigitalApp.Models;
namespace MFG_DigitalApp
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnclick_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                UserSelectionModel model = new UserSelectionModel();
                model.PlantCode = "M016";
                model.Line = "LINE 2";
                model.ShiftCode = "S2";
                model.ShiftDate = "2019-08-09";
                Session["UserSelectionModel"] = model;

                Response.Redirect("RunDetails.aspx", false);
            }

        }
    }
}