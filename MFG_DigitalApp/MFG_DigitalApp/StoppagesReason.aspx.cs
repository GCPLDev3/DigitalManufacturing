using MFG_DigitalApp.Log;
using MFG_DigitalApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MFG_DigitalApp
{
    public partial class StoppagesReason : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("StoppagesReason");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserSelectionModel"] != null)
                {
                    UserSelectionModel model = new UserSelectionModel();
                    model = (UserSelectionModel)Session["UserSelectionModel"];
                    lblplantcode.Text = model.PlantCode;
                    lblline.Text = model.Line;
                    lblshift.Text = model.ShiftCode;
                    lbldate.Text = model.ShiftDate;

                    txtDate.Text = lbldate.Text;
                    GetShiftHeader();
                    GetShift();
                    GetStation();
                    GetDuration();
                    BindStoppagesReason();
                    //BindTimeControl(drpTime);
                }
            }
        }
        public void BindTimeControl(DropDownList drp)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "14"),
                new SqlParameter("@ShiftCode", drpShift.SelectedValue),
            };

            drp.Items.Clear();
            DataTable DtTimes = new DataTable();
            DtTimes = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            drp.DataSource = DtTimes;
            drp.DataValueField = "TimeValue";
            drp.DataTextField = "TimeValue";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("HH:MM", ""));
            //drp.SelectedValue = Convert.ToString(DtTimes.Rows[0]["CurrentTime"]);
        }
        protected void GetShift()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "4")
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");

            drpShift.DataSource = Dt;
            drpShift.DataTextField = "ShiftName";
            drpShift.DataValueField = "ShiftCode";
            drpShift.DataBind();
            drpShift.Items.Insert(0, new ListItem("Select Shift", "0"));
            drpShift.SelectedValue = Convert.ToString(lblshift.Text);
            //drpShift.Enabled = false;
            drpShift_SelectedIndexChanged(null, null);
        }
        protected void GetStation()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "5"),
                new SqlParameter("@PlantCode", lblplantcode.Text)
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            drpStation.DataSource = Dt;
            drpStation.DataTextField = "StationDescription";
            drpStation.DataValueField = "StationCode";
            drpStation.DataBind();
            drpStation.Items.Insert(0, new ListItem("Select Station", "0"));
        }
        protected void GetReason()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@OPR", "6"),
                 new SqlParameter("@StationCode", drpStation.SelectedValue)
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            drpReason.DataSource = Dt;
            drpReason.DataTextField = "ReasonDescription";
            drpReason.DataValueField = "ReasonCode";
            drpReason.DataBind();
            drpReason.Items.Insert(0, new ListItem("Select Reason", "0"));
        }
        protected void GetDuration()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "7")
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            ddlHour.DataSource = Dt;
            ddlHour.DataTextField = "Hours";
            ddlHour.DataValueField = "Hours";
            ddlHour.DataBind();
            ddlHour.Items.Insert(0, new ListItem("Select Hour", "0"));

            SqlParameter[] param1 = new SqlParameter[]
            {
                new SqlParameter("@OPR", "8")
            };
            DataTable Dtt = new DataTable();
            Dtt = DBClass.GetDatatable_WithParam(param1, "DM_SP_GET_STOPPAGE_DATA");
            ddlMinute.DataSource = Dtt;
            ddlMinute.DataTextField = "Minutes";
            ddlMinute.DataValueField = "Minutes";
            ddlMinute.DataBind();
            ddlMinute.Items.Insert(0, new ListItem("Select Minute", "0"));
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            object obj;
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", 1),
                new SqlParameter("@AutoId", null),
                new SqlParameter("@Date", txtDate.Text.Trim()),
                new SqlParameter("@Time", drpTime.SelectedValue),
                new SqlParameter("@ShiftCode",drpShift.SelectedValue),
                new SqlParameter("@StationCode",drpStation.SelectedValue),
                new SqlParameter("@ReasonCode",drpReason.SelectedValue),
                new SqlParameter("@DownTime",ddlHour.SelectedItem.Text +":"+ddlMinute.SelectedItem.Text),
                new SqlParameter("@Line",lblline.Text),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@CreatedBy",HttpContext.Current.Session["username"].ToString())
            };

            obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
            BindStoppagesReason();
            if (obj != null)
            {
                drpStation.SelectedIndex = 0;
                drpReason.SelectedIndex = 0;
                ddlHour.SelectedIndex = 0;
                ddlMinute.SelectedIndex = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Stoppage Reason Entry added successfully')", true);
            }
        }

        protected void drpStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetReason();
        }

        public void BindStoppagesReason()
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@OPR", 2),
                 new SqlParameter("@Line",lblline.Text),
                 new SqlParameter("@PlantCode", lblplantcode.Text),
                 new SqlParameter("@Date", lbldate.Text),
                 new SqlParameter("@ShiftCode", lblshift.Text),
             };
            dt = DBClass.GetDatatable_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
            GrdStoppageReason.DataSource = dt;
            GrdStoppageReason.DataBind();
        }

        protected void GrdStoppageReason_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdReasoncode = (e.Row.FindControl("hdReasoncode") as HiddenField);
                if (hdReasoncode.Value.ToUpper() == "TOTAL")
                {
                    e.Row.Attributes["style"] = "background-color: #00B0F0";
                    //e.Row.Attributes["style"] = "color: #FFFFFF";
                    e.Row.Font.Bold = true;

                    foreach (Button button in e.Row.Cells[7].Controls.OfType<Button>())
                    {
                        if (button.CommandName == "Delete")
                        {
                            button.Visible = false;
                        }
                    }
                }
                foreach (Button button in e.Row.Cells[7].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete this record?')){ return false; };";
                    }
                }
            }
            if (e.Row.RowIndex != GrdStoppageReason.EditIndex)
            {
                //(e.Row.Cells[6].Controls[1] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        protected void btnProductionEntry_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductionDetails.aspx", false);
        }

        protected void btnRunDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("RunDetails.aspx", false);
        }

        protected void GetShiftHeader()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "1")
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
                drpShiftHeader.DataSource = Dt;
                drpShiftHeader.DataTextField = "ShiftName";
                drpShiftHeader.DataValueField = "ShiftCode";
                drpShiftHeader.DataBind();
                drpShiftHeader.Items.Insert(0, new ListItem("Select Shift", ""));
                drpShiftHeader.SelectedValue = lblshift.Text;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetShiftHeader::", ex.Message), ex);
            }
        }

        protected void drpShiftHeader_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpShiftHeader.SelectedValue != "")
            {
                string Shift = drpShiftHeader.SelectedItem.Text;
                string[] tokens = Shift.Split('/');
                string date = tokens[tokens.Length - 1];

                lblshift.Text = drpShiftHeader.SelectedValue;
                lbldate.Text = date;
                txtDate.Text = lbldate.Text;
                GetShift();
                GetStation();
                GetDuration();
                BindStoppagesReason();
            }
        }

        protected void drpShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTimeControl(drpTime);
        }

        protected void GrdStoppageReason_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)GrdStoppageReason.Rows[e.RowIndex];
            Int64 AutoId = Convert.ToInt32(GrdStoppageReason.DataKeys[e.RowIndex].Value.ToString());
            object obj;
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", 3),
                new SqlParameter("@AutoId", AutoId),
            };

            obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
            BindStoppagesReason();
            if (obj != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Stoppage Reason Entry deleted successfully')", true);
            }
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            txtComment.Attributes.Add("maxlength", txtComment.MaxLength.ToString());
            lblCommentsMessage.Text = string.Empty;
            GetCommentsDetails();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:count()", true);
            MPComments.Show();

        }

        public void GetCommentsDetails()
        {
            try
            {
                txtComment.Text = string.Empty;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "15"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", lbldate.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    txtComment.Text = Convert.ToString(Dt.Rows[0]["Comments"]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetCommentsDetails::", ex.Message), ex);
            }
        }

        protected void btnAddComments_Click(object sender, EventArgs e)
        {
            try
            {
                lblCommentsMessage.Text = string.Empty;
                if (txtComment.Text.Trim() == "")
                {
                    lblCommentsMessage.Text = "Please Enter Comments";
                    lblCommentsMessage.ForeColor = Color.Red;
                }
                else
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@OPR", "9"),
                        new SqlParameter("@PlantCode", lblplantcode.Text),
                        new SqlParameter("@Line", lblline.Text),
                        new SqlParameter("@Date", lbldate.Text),
                        new SqlParameter("@ShiftCode", lblshift.Text),
                        new SqlParameter("@Comments", txtComment.Text.ToString().Trim()),
                        new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                    };
                    DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");

                    lblCommentsMessage.Text = "Comments Saved Successfully.";
                    lblCommentsMessage.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                lblCommentsMessage.Text = ex.Message.ToString();
                lblCommentsMessage.ForeColor = Color.Red;
                _logger.Error(string.Concat("btnAddComments_Click::", ex.Message), ex);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:count()", true);
            MPComments.Show();
        }

        protected void btnClearComments_Click(object sender, EventArgs e)
        {
            lblCommentsMessage.Text = string.Empty;
            txtComment.Text = string.Empty;
            MPComments.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:count()", true);
        }

        protected void btnParameter_Click(object sender, EventArgs e)
        {
            Response.Redirect("ParameterDetails.aspx", false);
        }
    }
}