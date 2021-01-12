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
    public partial class QI_Schedules : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("QI_ScheduleDetails");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        public bool isDeleteButtonTriggred = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExport);
            if (!IsPostBack)
            {
                GetData();
                GetPlant();
                GetScheduleTypes();
                txtCreatedBy.Text = "";
            }
        }

        protected void OnSelectedRecord(Object sender, EventArgs e)
        {
            for (int i = 0; i < GrdScheduleList.Rows.Count; i++)
            {
                RadioButton rb = (RadioButton)GrdScheduleList.Rows[i]
                                .Cells[0].FindControl("rbCreate");
                if (rb != null)
                {
                    if (rb.Checked)
                    {
                        int rowIndex = ((sender as RadioButton).NamingContainer as GridViewRow).RowIndex;
                        int id = Convert.ToInt32(GrdScheduleList.DataKeys[rowIndex].Values[0]);
                        if (id > 0)
                            Session["CScheduleId"] = id;
                        break;
                    }
                }
            }
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            GrdScheduleList.PageIndex = e.NewPageIndex;
            GetData();
        }

        protected void GetData()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "33"),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdScheduleList.DataSource = Dt;
                GrdScheduleList.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetData::", ex.Message), ex);
            }
        }

        protected void btnCreateSchedule_Click(object sender, EventArgs e)
        {
            Session["ScheduleId"] = null;
            Response.Redirect("QI_CreateSchedule.aspx", false);
        }
        protected void lnkScheduleNumber_Click(object sender, EventArgs e)
        {
            int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
            int id = Convert.ToInt32(GrdScheduleList.DataKeys[rowIndex].Values[0]);
            if (id > 0)
                Session["ScheduleId"] = id;
            Response.Redirect("QI_EditSchedule.aspx", false);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = QC_Schedule.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);
            GrdScheduleList.AllowPaging = false;
            GetData();
            foreach (GridViewRow row in GrdScheduleList.Rows)
            {
                GrdScheduleList.Columns[0].Visible = false;
                (GrdScheduleList.Rows[row.RowIndex].Cells[1].FindControl("lnkScheduleNumber") as LinkButton).Font.Underline = false;
                (GrdScheduleList.Rows[row.RowIndex].Cells[1].FindControl("lnkScheduleNumber") as LinkButton).Enabled = false;
            }
            GrdScheduleList.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        protected void GrdScheduleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                foreach (Button button in e.Row.Cells[9].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete this record?')){ return false; };";
                    }
                }
            }

        }

        protected void GrdScheduleList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


        }
        public void GetScheduleTypes()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "38"),
                new SqlParameter("@Type",3)
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpScheduleTypeId.DataSource = Dt;
                drpScheduleTypeId.DataTextField = "ScheduleTypeName";
                drpScheduleTypeId.DataValueField = "Id";
                drpScheduleTypeId.DataBind();
                drpScheduleTypeId.Items.Insert(0, new ListItem("Select Schedule Type", "0"));

            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetScheduleType::", ex.Message), ex);
            }
        }

        protected void drpScheduleTypeId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void drpPlant_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GetPlant()
        {
            try
            {
                var countryId = Session["CountryId"];
                var x = HttpContext.Current.Session["USERID"].ToString();
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "2"),
                 new SqlParameter("@UserId", HttpContext.Current.Session["USERID"].ToString()),
                new SqlParameter("@CountryId", countryId),
                new SqlParameter("@currentTime", "")
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_SHIFTDATA");
                //drpPlant.ClearSelection();
                drpPlant.DataSource = Dt;
                drpPlant.DataTextField = "PlantName";
                drpPlant.DataValueField = "PlantCode";
                drpPlant.DataBind();
                drpPlant.Items.Insert(0, new ListItem("Select plant", "0"));
                // drpPlant.SelectedValue = Convert.ToString(Dt.Rows[0]["PlantName"]);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetPlant::", ex.Message), ex);
            }
        }

        protected void BtnShow_Click(object sender, EventArgs e)
        {
            bool plantIsNull = false;
            bool scheduleTypeIsNull = false;
            try
            {
                if (drpPlant.SelectedIndex == 0 && drpScheduleTypeId.SelectedIndex == 0 && string.IsNullOrEmpty(txtCreatedBy.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select/Enter atleast one field')", true);
                }
                else
                {
                    if (drpPlant.SelectedIndex == 0)
                    {
                        plantIsNull = true;
                        drpPlant.Items.RemoveAt(0);
                        drpPlant.Items.Insert(0, new ListItem("0"));
                        drpPlant.SelectedValue = "0";
                    }
                    if (drpScheduleTypeId.SelectedIndex == 0)
                    {
                        scheduleTypeIsNull = true;
                        drpScheduleTypeId.Items.RemoveAt(0);
                        drpScheduleTypeId.Items.Insert(0, new ListItem("0"));
                        drpScheduleTypeId.SelectedValue = "0";
                    }
                    SqlParameter[] param = new SqlParameter[]
                    {
                    new SqlParameter("@OPR", "55"),
                       new SqlParameter("@PlantCode",drpPlant.SelectedValue),
                    new SqlParameter("@ScheduleTypeId", drpScheduleTypeId.SelectedValue),
                    new SqlParameter("@CreatedBy",txtCreatedBy.Text)
                    };

                    DataTable Dt = new DataTable();
                    Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                    GrdScheduleList.DataSource = Dt;
                    GrdScheduleList.DataBind();
                    if (Dt.Rows.Count > 0)
                    {
                        if (plantIsNull)
                        {
                            drpPlant.Items.RemoveAt(0);
                            drpPlant.Items.Insert(0, new ListItem("Select Plant"));
                        }
                        if (scheduleTypeIsNull)
                        {
                            drpScheduleTypeId.Items.RemoveAt(0);
                            drpScheduleTypeId.Items.Insert(0, new ListItem("Select Schedule Type"));
                        }
                        //drpScheduleTypeId.SelectedIndex = 0;
                        //drpPlant.SelectedIndex = 0;
                        //txtCreatedBy.Text = "";
                    }
                    else if (Dt.Rows.Count == 0)
                    {
                        if (plantIsNull)
                        {
                            drpPlant.Items.RemoveAt(0);
                            drpPlant.Items.Insert(0, new ListItem("Select Plant"));
                        }
                        if (scheduleTypeIsNull)
                        {
                            drpScheduleTypeId.Items.RemoveAt(0);
                            drpScheduleTypeId.Items.Insert(0, new ListItem("Select Schedule Type"));
                        }
                        ///drpScheduleTypeId.SelectedIndex = 0;
                        //drpPlant.SelectedIndex = 0;
                        //txtCreatedBy.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindCompletedScheduleStatusGrid::", ex.Message), ex);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            int isChecked = 0;
            for (int i = 0; i < GrdScheduleList.Rows.Count; i++)
            {
                if ((GrdScheduleList.Rows[i].Cells[0].FindControl("Select") as CheckBox).Checked)
                {
                    isChecked++;
                    //break;
                }
            }
            if (isChecked == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select atleast one record')", true);
            }
            else
            {
                Object obj = new Object();
                bool isDeletion = false;
                for (int i = 0; i < GrdScheduleList.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)GrdScheduleList.Rows[i].Cells[0].FindControl("Select"); //find the CheckBox                      
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {

                            SqlParameter[] para = new SqlParameter[]
                             {
                            new SqlParameter("@OPR", 51),
                            new SqlParameter("@ScheduleId", GrdScheduleList.DataKeys[i].Values[0])
                             };
                            DataTable DtCount = DBClass.GetDatatable_WithParam(para, "DM_SP_GET_DATA");

                            if (DtCount != null && DtCount.Rows.Count > 0)
                            {
                                isDeletion = false; 
                                LinkButton  SchaduleNumber = (LinkButton)GrdScheduleList.Rows[i].Cells[1].FindControl("ScheduleNumber");
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('" + SchaduleNumber + " Schedule already activated,Please stop and delete / Un select the schedule!!!')", true);
                                //GetData();
                            }
                            else
                            {
                                isDeletion = true;
                                SqlParameter[] param = new SqlParameter[]
                               {
                                    new SqlParameter("@OPR", 5),
                                    new SqlParameter("@AutoId", GrdScheduleList.DataKeys[i].Values[0])
                               };

                                 obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
                            }
                        }
                    }
                }
                if(isDeletion)
                {
                    if (obj != null && Convert.ToInt64(obj) == 1)
                    {
                        GetData();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Schedule deleted successfully')", true);

                    }
                }
                else if(!isDeletion)
                {
                    GetData();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('selected Schedule already activated,Please stop and delete / Un select the schedule!!!')", true);
                  
                }
               

            }
        }

        protected void chckchanged(object sender, EventArgs e)
        {
            CheckBox chckheader = (CheckBox)GrdScheduleList.HeaderRow.FindControl("CheckAll");
            foreach (GridViewRow row in GrdScheduleList.Rows)
            {
                CheckBox chckrw = (CheckBox)row.FindControl("Select");
                if (chckheader.Checked == true)
                {
                    chckrw.Checked = true;
                }
                else
                {
                    chckrw.Checked = false;
                }
            }
        }
    }
}