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
    public partial class QualityCheck : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("Quality Checking");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExport);
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
                    drpStatus_SelectedIndexChanged(null, null);
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
        }
        #region Active Grid
        protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpStatus.SelectedValue == "Completed")
            {
                HeaderButtons.Visible = false;
                GrdActiveAndScheduled.Visible = false;
                GrdCompleted.Visible = true;
                GrdScheduleStatus.Visible = false;
                GrdCompletedStatus.Visible = true;
                BindCompletedGrid();
                BindCompletedScheduleStatusGrid();
            }
            else
            {
                HeaderButtons.Visible = true;
                GrdActiveAndScheduled.Visible = true;
                GrdCompleted.Visible = false;
                GrdScheduleStatus.Visible = true;
                GrdCompletedStatus.Visible = false;
                BindActiveAndScheduledGrid();
                BindScheduleStatusGrid();
            }
        }

        protected void BindActiveAndScheduledGrid()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "2"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@Date", lbldate.Text),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdActiveAndScheduled.DataSource = Dt;
                GrdActiveAndScheduled.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindActiveAndScheduledGrid::", ex.Message), ex);
            }
        }


        protected void GrdActiveAndScheduled_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkDeletePO = (LinkButton)e.Row.FindControl("lnkDeletePO");

                if (e.Row.Cells[0].Text.Equals("Active"))
                {
                    ViewState["Active"] = "Active";
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                    if (lnkDeletePO != null)
                        lnkDeletePO.Visible = false;
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = SQC.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);
            if (Session["ScheduleTypeId"] != null && Convert.ToInt16(Session["ScheduleTypeId"].ToString()) != 3)
            {
                GrdCompletedScheduleStatus.AllowPaging = false;
                BindCompletedScheduleParameterGrid(Convert.ToInt16(Session["TestScheduleDetailId"].ToString()));
                GrdCompletedScheduleStatus.RenderControl(htmlWrite);
            }
            else
            {
                GrdSQCCompletedScheduleStatus.AllowPaging = false;
                BindSQCCompletedScheduleParameterGrid(Convert.ToInt16(Session["TestScheduleDetailId"].ToString()));
                GrdSQCCompletedScheduleStatus.RenderControl(htmlWrite);
            }
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        protected void lnkDeletePO_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region Completed Grid
        protected void BindCompletedGrid()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "5"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdCompleted.DataSource = Dt;
                GrdCompleted.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindCompletedGrid::", ex.Message), ex);
            }
        }

        protected void GrdCompleted_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdCompleted.PageIndex = e.NewPageIndex;
            BindCompletedGrid();
        }

        protected void GrdCompletedSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdCompletedScheduleStatus.PageIndex = e.NewPageIndex;
            if(Session["TestScheduleDetailId"] !=null && Convert.ToInt16(Session["TestScheduleDetailId"]) > 0)
            {
                    BindCompletedScheduleParameterGrid(Convert.ToInt16(Session["TestScheduleDetailId"]));
            }
                        
            MPCompletedSchedule.Show();
        }

        protected void GrdSQCCompletedScheduleStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdSQCCompletedScheduleStatus.PageIndex = e.NewPageIndex;
            if (Session["TestScheduleDetailId"] != null && Convert.ToInt16(Session["TestScheduleDetailId"]) > 0)
            {
                BindSQCCompletedScheduleParameterGrid(Convert.ToInt16(Session["TestScheduleDetailId"]));
            }
            MPCompletedSchedule.Show();
        }

        protected void lnkPONumber_Click(object sender, EventArgs e)
        {
            
        }
        protected void btnActivateSchedule_Click(Object Sender,EventArgs e)
        {
            GetScheduleType(drpScheduleType);
            BindTimeControl(drpTimeStartSchedule);
            MPActiveSchedule.Show();
        }
        protected void LoadSchedule(Object Sender,EventArgs e)
        {
            if (drpScheduleType.SelectedValue != "")
            {
                GetSchedule(drpScheduleType,drpSchedule, lblplantcode.Text);
                txtGramStart.Text = String.Empty;
                GetGrammage();
                drpSchedule.Enabled = true;
            }
            else
            {
                drpSchedule.Enabled = false;
            }
            MPActiveSchedule.Show();
        }

        public void BindTimeControl(DropDownList drp)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "1")
            };

            drp.Items.Clear();
            DataTable DtTimes = new DataTable();
            DtTimes = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            drp.DataSource = DtTimes;
            drp.DataValueField = "TimeValue";
            drp.DataTextField = "TimeValue";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("HH:MM", ""));
            drp.SelectedValue = Convert.ToString(DtTimes.Rows[0]["CurrentTime"]);
        }
        protected void GetSchedule(DropDownList drpScheduleType,DropDownList drpSchedule, String PlantCode)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "35"),
                new SqlParameter("@ScheduleTypeId",drpScheduleType.SelectedValue),
                new SqlParameter("@PlantCode", PlantCode)
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpSchedule.DataSource = Dt;
                drpSchedule.DataTextField = "ScheduleNumber";
                drpSchedule.DataValueField = "Id";
                drpSchedule.DataBind();
                drpSchedule.Items.Insert(0, new ListItem("Select Schedule", "0"));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetScheduleType::", ex.Message), ex);
            }
        }

        protected void GetGrammage()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "36"),
                    new SqlParameter("@ScheduleTypeId",drpScheduleType.SelectedValue)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    txtGramStart.Text = Dt.Rows[0]["Grammage"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetGrammage::", ex.Message), ex);
            }
        }

        #endregion
        #region Get Schedule Type
        protected void GetScheduleType(DropDownList drp)
        {
            int Type = 0;
            if(string.Compare(drp.ID, "drpScheduleType") == 0)
            {
                Type = 1;
            }
            else
            {
                Type = 2;
            }
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "38"),
                new SqlParameter("@Type",Type)
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drp.DataSource = Dt;
                drp.DataTextField = "ScheduleTypeName";
                drp.DataValueField = "Id";
                drp.DataBind();
                drp.Items.Insert(0, new ListItem("Select Schedule Type", "0"));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetScheduleType::", ex.Message), ex);
            }
        }
        #endregion

        protected void btnStopSchedule_Click(Object Sender, EventArgs e)
        {
            lblStopMessage.Text = String.Empty;
            GetScheduleType(drpStopScheduleType);
            txtStopDate.Text = lbldate.Text;
            //txtStopDate.Attributes.Add("readonly", "true");
            BindTimeControl(drpTimeStopSchedule);
            MPStopSchedule.Show();
        }

        protected void btnProcessParameter_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("Process.aspx", false);
        }
        protected void btnWeight_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("Weight.aspx", false);
        }
        protected void btnInProcessParameter_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("In_Process.aspx", false);
        }
        protected void btnSQC_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("SQC.aspx", false);
        }
        protected void btnActivate_Click(Object Sender, EventArgs e)
        {
            lblStartMessage.Text = string.Empty;
            if (drpScheduleType.SelectedValue == "0")
            {
                lblStartMessage.Text = "Schedule Type Empty !";
                lblStartMessage.ForeColor = Color.Red;
                MPActiveSchedule.Show();
            }
            else if (drpSchedule.SelectedValue == "0")
            {
                lblStartMessage.Text = "Schedule Empty !";
                lblStartMessage.ForeColor = Color.Red;
                MPActiveSchedule.Show();
            }
            else if (txtStartDate.Text == "")
            {
                lblStartMessage.Text = "Start Date empty !";
                lblStartMessage.ForeColor = Color.Red;
                MPActiveSchedule.Show();
            }
            
            
            else
            {
                object result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 15),
                    new SqlParameter("@ScheduleTypeId", drpScheduleType.SelectedValue),
                    new SqlParameter("@ScheduleId", drpSchedule.SelectedValue),
                    new SqlParameter("@StartDate",txtStartDate.Text.Trim()),
                    new SqlParameter("@StartTime",Convert.ToString(drpTimeStartSchedule.SelectedValue)),
                    new SqlParameter("@StartedBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@Status",1),
                    new SqlParameter("@Gram",txtGramStart.Text.Trim()),
                };


                result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");

                if (Convert.ToInt64(result) > 0)
                {
                    lblStartMessage.Text = "Schedule Activate Success";
                    lblStartMessage.ForeColor = Color.Green;
                    ClearActivateSchedule();
                    BindScheduleStatusGrid();
                    MPActiveSchedule.Hide();
                }
                else if (Convert.ToInt16(result) == -1)
                {
                    lblStartMessage.Text = "Please Stop the current Active Schedule and then Start Activate new Schedule";
                    lblStartMessage.ForeColor = Color.Red;
                    MPActiveSchedule.Show();
                }
                else
                {
                    lblStartMessage.Text = "Error, Something went wrong!";
                    lblStartMessage.ForeColor = Color.Red;
                    MPActiveSchedule.Show();
                }
                
            }
        }

        protected void BindScheduleStatusGrid()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "37")
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdScheduleStatus.DataSource = Dt;
                GrdScheduleStatus.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindScheduleStatusGrid::", ex.Message), ex);
            }
        }

        protected void BindCompletedScheduleStatusGrid()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "42")
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdCompletedStatus.DataSource = Dt;
                GrdCompletedStatus.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindCompletedScheduleStatusGrid::", ex.Message), ex);
            }
        }

        protected void btnClearSchedule_Click(Object sender,EventArgs e)
        {
            ClearActivateSchedule();
        }

        private void ClearActivateSchedule()
        {
            GetScheduleType(drpScheduleType);
            drpTimeStartSchedule.SelectedIndex = 0;
            txtGramStart.Text = string.Empty;
            txtStartDate.Text = lbldate.Text;
        }

        protected void lnkScheduleName_Click(Object sender,EventArgs e)
        {
            int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
            int id = Convert.ToInt32(GrdCompletedStatus.DataKeys[rowIndex].Values[0]);
            Session["TestScheduleDetailId"] = id;
            Session["ScheduleTypeId"] = Convert.ToInt32(GrdCompletedStatus.DataKeys[rowIndex].Values[1]);
            if (Convert.ToInt32(GrdCompletedStatus.DataKeys[rowIndex].Values[1]) == 3)
            {
                divGrdSQCCompletedScheduleStatus.Visible = true;
                dvGrdCompletedScheduleStatus.Visible = false;
                BindSQCCompletedScheduleParameterGrid(id);
            }
            else
            {
                divGrdSQCCompletedScheduleStatus.Visible = false;
                dvGrdCompletedScheduleStatus.Visible = true;
                BindCompletedScheduleParameterGrid(id);
            }
            MPCompletedSchedule.Show();
        }

        protected void BindSQCCompletedScheduleParameterGrid(int id)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "43"),
                    new SqlParameter("@TestScheduleDetailId", id)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdSQCCompletedScheduleStatus.DataSource = Dt;
                GrdSQCCompletedScheduleStatus.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindSQCCompletedScheduleParameterGrid::", ex.Message), ex);
            }
        }

        protected void BindCompletedScheduleParameterGrid(int id)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "43"),
                    new SqlParameter("@TestScheduleDetailId", id)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdCompletedScheduleStatus.DataSource = Dt;
                GrdCompletedScheduleStatus.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindCompletedGrid::", ex.Message), ex);
            }
        }


        #region ASSIGN OPERATOR
        protected void btnAssignOperator_Click(object sender, EventArgs e)
        {
            lblOperatorMessage.Text = string.Empty;
            BindStations();
            BindOperators();
            BindOperatorsGrd();
            MPAssignOperator.Show();
        }

        public void BindOperatorsGrd()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "8"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@Date", lbldate.Text),
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdOperators.DataSource = Dt;
                GrdOperators.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindCompletedGrid::", ex.Message), ex);
            }
        }


        public void BindStations()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "7"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text),
            };

            DataTable DtStations = new DataTable();
            DtStations = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            drpStation.DataSource = DtStations;
            drpStation.DataTextField = "StationDescription";
            drpStation.DataValueField = "StationCode";
            drpStation.DataBind();
            drpStation.Items.Insert(0, new ListItem("Select Station", ""));
        }

        public void BindOperators()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "6"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text),
            };

            DataTable DtOperators = new DataTable();
            DtOperators = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            drpOperator.DataSource = DtOperators;
            drpOperator.DataTextField = "OperatorName";
            drpOperator.DataValueField = "OperatorName";
            drpOperator.DataBind();
            drpOperator.Items.Insert(0, new ListItem("Select Name", ""));
        }

        protected void btnAddOperator_Click(object sender, EventArgs e)
        {
            lblOperatorMessage.Text = string.Empty;
            if (drpStation.SelectedValue == "")
            {
                lblOperatorMessage.Text = "Please select Station";
                lblOperatorMessage.ForeColor = Color.Red;
            }
            else if (drpOperator.SelectedValue == "")
            {
                lblOperatorMessage.Text = "Please select Name";
                lblOperatorMessage.ForeColor = Color.Red;
            }
            
            else
            {
                object result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 4),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", lbldate.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@StationCode",Convert.ToString(drpStation.SelectedValue)),
                    new SqlParameter("@OperatorName",Convert.ToString(drpOperator.SelectedValue)),
                    new SqlParameter("@Type","QC Supervisor"),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                };

                result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_DML_DATA");

                if (Convert.ToInt64(result) > 0)
                {
                    lblOperatorMessage.Text = "Operator added successfully.";
                    lblOperatorMessage.ForeColor = Color.Green;
                    ClearOperator();
                    BindOperatorsGrd();
                }
                else if (Convert.ToInt64(result) == -1)
                {
                    lblOperatorMessage.Text = "Station & Name already added.";
                    lblOperatorMessage.ForeColor = Color.Red;
                }
                else
                {
                    lblOperatorMessage.Text = "Error, Something went wrong!";
                    lblOperatorMessage.ForeColor = Color.Red;
                }
            }

            MPAssignOperator.Show();
        }

        protected void lnkDeleteAO_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton lnkDeleteAO = (ImageButton)sender;
                var deleteID = lnkDeleteAO.CommandArgument;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "8"),
                    new SqlParameter("@DeleteID", deleteID)
                };
                DataTable Dt = new DataTable();
                DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
                BindOperatorsGrd();
                lblOperatorMessage.Text = "Deleted Successfully.";
                lblOperatorMessage.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("lnkDeleteAO_Click::", ex.Message), ex);
            }

            MPAssignOperator.Show();
        }

        public void ClearOperator()
        {
            BindStations();
            BindOperators();
        }

        private void ClearStopSchedule()
        {
            txtStopDate.Text = string.Empty;
            drpTimeStopSchedule.SelectedIndex = 0;
            GetScheduleType(drpStopScheduleType);
        }



        #endregion

        protected void btnStopScheduleType_Click(object sender, EventArgs e)
        {
            lblStopMessage.Text = string.Empty;
            if (drpStopScheduleType.SelectedValue == "0")
            {
                lblStopMessage.Text = "Schedule Type Empty !";
                lblStopMessage.ForeColor = Color.Red;
                MPStopSchedule.Show();
            }
            else if (txtStopDate.Text == "")
            {
                lblStopMessage.Text = "Stop Date empty!";
                lblStopMessage.ForeColor = Color.Red;
                MPStopSchedule.Show();
            }
            else if (drpTimeStopSchedule.SelectedValue == "")
            {
                lblStopMessage.Text = "Stop Time empty!";
                lblStopMessage.ForeColor = Color.Red;
                MPStopSchedule.Show();
            }
            else
            {
                object result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 16),
                    new SqlParameter("@ScheduleTypeId", drpStopScheduleType.SelectedValue),
                    new SqlParameter("@StoppageDate",Convert.ToString(txtStopDate.Text)),
                    new SqlParameter("@StoppageTime",Convert.ToString(drpTimeStopSchedule.SelectedValue)),
                    new SqlParameter("@StoppageBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@CurrentStatus",0)
                };

                result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_DML_DATA");

                if (Convert.ToInt64(result) > 0)
                {
                    ViewState["Active"] = null;
                    lblStopMessage.Text = "Schedule Stop Run Success";
                    lblStopMessage.ForeColor = Color.Green;
                    ClearStopSchedule();
                    BindScheduleStatusGrid();
                    MPStopSchedule.Hide();
                }
                else if (Convert.ToInt64(result) == -1)
                {
                    lblStopMessage.Text = "Error, Future Date time not allowed!";
                    lblStopMessage.ForeColor = Color.Red;
                    MPStopSchedule.Show();
                }
                else
                {
                    lblStopMessage.Text = "Error, Something went wrong!";
                    lblStopMessage.ForeColor = Color.Red;
                    MPStopSchedule.Show();
                }
            }
        }

    }
}