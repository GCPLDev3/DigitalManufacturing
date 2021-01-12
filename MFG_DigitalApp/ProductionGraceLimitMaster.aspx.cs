using MFG_DigitalApp.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MFG_DigitalApp
{
    public partial class ProductionGraceLimitMaster : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("QI_ParameterDetails");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Bindgrid();
                Bindplant();
            }
        }

        protected void Bindgrid()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", 1),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "sp_getpercentageplantlinewise");
                grdPlantDetails.DataSource = Dt;
                grdPlantDetails.DataBind();

            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetData::", ex.Message), ex);
            }
        }

        protected void Bindplant()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", 3),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "sp_getpercentageplantlinewise");
                ddlPlant.DataSource = Dt;
                ddlPlant.DataTextField ="PlantName";
                ddlPlant.DataValueField ="PlantCode";
                ddlPlant.DataBind();
                ddlPlant.Items.Insert(0, new ListItem("Select Plant", ""));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetData::", ex.Message), ex);
            }
        }

        protected void clear()
        {
            ddlPlant.SelectedIndex = -1;
            grdPlantDetails.EditIndex = -1;
            txtPercentage.Text = string.Empty;
            txtSubmit.Text = "Submit";
            Bindgrid();
            Bindplant();
            grdPlantDetails.Columns[3].Visible = true ;
        }

        protected void txtSubmit_Click(object sender, EventArgs e)
        {
            if (txtSubmit.Text == "Submit")
            {
                string plantcode = ddlPlant.SelectedValue;
                decimal percentage = Convert.ToDecimal(txtPercentage.Text);
                SqlParameter[] param = new SqlParameter[]
                    { new SqlParameter("@opr",4),
                new SqlParameter("@plantcode",plantcode),
                new SqlParameter("@percentage",percentage),
                new SqlParameter("@user",Session["USERID"].ToString())
                    };

                int result = Convert.ToInt16(DBClass.ExecuteScalar_WithParam(param, "sp_getpercentageplantlinewise"));
                if (result == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Grace limit added successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Issue in adding grace percentage. Please contact Administrator.')", true);
                }
                clear();
            }
            else if(txtSubmit.Text=="Update")
            {
                string plantcode = ddlPlant.SelectedValue;
                decimal percentage = Convert.ToDecimal(txtPercentage.Text);
                SqlParameter[] param = new SqlParameter[]
                    { new SqlParameter("@opr",6),
                new SqlParameter("@plantcode",plantcode),
                new SqlParameter("@percentage",percentage),
                new SqlParameter("@user",Session["USERID"].ToString())
                    };

                int result = Convert.ToInt16(DBClass.ExecuteScalar_WithParam(param, "sp_getpercentageplantlinewise"));
                if (result == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Grace limit updated successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Issue in updating grace percentage. Please contact Administrator.')", true);
                }
                grdPlantDetails.Columns[3].Visible = true;
                clear();
            }
        }

        protected void txtClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void grdPlantDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grdPlantDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void getInvoiceByID_Click(object sender, EventArgs e)
        {
            ImageButton Imgbtn = sender as ImageButton;
            GridViewRow row = Imgbtn.NamingContainer as GridViewRow;
            string pk = grdPlantDetails.DataKeys[row.RowIndex].Values[0].ToString();
            grdPlantDetails.Columns[3].Visible = false;
            SqlParameter[] plant = new SqlParameter[]
            {
                new SqlParameter("@opr",5),
            };
            DataTable dt = DBClass.GetDatatable_WithParam(plant, "sp_getpercentageplantlinewise");
            ddlPlant.DataSource = dt;
            ddlPlant.DataBind();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@opr",2),
                new SqlParameter("@plantcode",pk)
            };
            DataTable dt1= DBClass.GetDatatable_WithParam(param, "sp_getpercentageplantlinewise");
            ddlPlant.SelectedValue = dt1.Rows[0]["PlantCode"].ToString();
            txtPercentage.Text= dt1.Rows[0]["Percentage"].ToString();
            txtSubmit.Text = "Update";
        }


        protected void ImageButton1_Click(object sender, EventArgs e)
        {
            ImageButton Imgbtn = sender as ImageButton;
            GridViewRow row = Imgbtn.NamingContainer as GridViewRow;
            string pk = grdPlantDetails.DataKeys[row.RowIndex].Values[0].ToString();
            grdPlantDetails.Columns[3].Visible = false;
            SqlParameter[] plant = new SqlParameter[]
            {
                new SqlParameter("@opr",7),
                new SqlParameter("@plantcode",pk),
                new SqlParameter("@user",Session["USERID"].ToString())
            };
            string dt = DBClass.ExecuteScalar_WithParam(plant, "sp_getpercentageplantlinewise").ToString();

            if (dt== "Delete")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Record deleted successfully.')", true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vendor deleted successfully.!!!')", true);
            }
            clear();
            
        }

        protected void grdPlantDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}