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
    public partial class RunDetails : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("RunDetails");
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
                    drpStatus_SelectedIndexChanged(null, null);
                    //BindSchedulePO();
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
        }

        protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpStatus.SelectedValue == "Completed")
            {
                HeaderButtons.Visible = false;
                GrdActiveAndScheduled.Visible = false;
                GrdCompleted.Visible = true;
                BindCompletedGrid();
            }
            else
            {
                HeaderButtons.Visible = true;
                GrdActiveAndScheduled.Visible = true;
                GrdCompleted.Visible = false;
                BindActiveAndScheduledGrid();
            }
        }

        #region Active And ScheduledGrid
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

        protected void lnkDeletePO_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkDeletePO = (LinkButton)sender;
                var PO = lnkDeletePO.CommandArgument;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "6"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@PONumber", PO)
                };
                DataTable Dt = new DataTable();
                DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
                BindActiveAndScheduledGrid();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('PO Deleted Successfully.')", true);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("lnkDeletePO_Click::", ex.Message), ex);
            }
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

        protected void lnkPONumber_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkPONumber = (LinkButton)sender;
                lblPoNumber.Text = lnkPONumber.Text;

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "10"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@PONumber", lnkPONumber.Text)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdPOProductionDetails.DataSource = Dt;
                GrdPOProductionDetails.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("lnkPONumber_Click::", ex.Message), ex);
            }

            MPProdDetail.Show();
        }

        #endregion

        #region START RUN
        protected void btnStartRun_Click(object sender, EventArgs e)
        {
            lblStartMessage.Text = string.Empty;
            btnStartPO.Enabled = true;
            if (ViewState["Active"] != null)
            {
                lblStartMessage.Text = "Please Stop Run the current Active PO and then Start Run new PO.";
                lblStartMessage.ForeColor = Color.Red;
                btnStartPO.Enabled = false;
            }
            BindStartPo();
            txtMaterialStartPO.Text = string.Empty;
            txtGramStart.Text = string.Empty;
            txtCaseSizeStart.Text = string.Empty;
            txtStartDate.Text = lbldate.Text;
            txtStartDate.Attributes.Add("readonly", "true");
            BindTimeControl(drpTimeStartPO);

            MPStartPO.Show();
        }

        public void BindStartPo()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "3"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text),
            };

            DataTable DtStartPO = new DataTable();
            DtStartPO = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");

            Session["DtStartPO"] = DtStartPO;

            drpStartPO.DataSource = DtStartPO;
            drpStartPO.DataTextField = "ProcessOrderWithMatDesc";
            drpStartPO.DataValueField = "ProcessOrder";
            drpStartPO.DataBind();
            drpStartPO.Items.Insert(0, new ListItem("Select Process Order", ""));
        }
        protected void drpStartPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStartMessage.Text = string.Empty;
            if (drpStartPO.SelectedValue != "" && Session["DtStartPO"] != null)
            {
                DataTable DtProcessOrders = (DataTable)(Session["DtStartPO"]);

                DataRow[] results = DtProcessOrders.Select("ProcessOrder =" + drpStartPO.SelectedValue);

                if (results != null && results.Count() > 0)
                {
                    txtMaterialStartPO.Text = Convert.ToString(results[0]["MaterialCode"]);
                    txtGramStart.Text = Convert.ToString(results[0]["Gram"]);
                    txtCaseSizeStart.Text = Convert.ToString(results[0]["CaseSize"]);
                }
                else
                {
                    txtMaterialStartPO.Text = string.Empty;
                    txtGramStart.Text = string.Empty;
                    txtCaseSizeStart.Text = string.Empty;
                }
            }
            else
            {
                txtMaterialStartPO.Text = string.Empty;
                txtGramStart.Text = string.Empty;
                txtCaseSizeStart.Text = string.Empty;
            }

            MPStartPO.Show();
        }

        protected void btnStartPO_Click(object sender, EventArgs e)
        {
            lblStartMessage.Text = string.Empty;

            if (ViewState["Active"] != null)
            {
                lblStartMessage.Text = "Please Stop Run the current Active PO and then Start Run new PO.";
                lblStartMessage.ForeColor = Color.Red;
                MPStartPO.Show();
            }
            else if (drpStartPO.SelectedValue == "")
            {
                lblStartMessage.Text = "Process Order Empty !";
                lblStartMessage.ForeColor = Color.Red;
                MPStartPO.Show();
            }
            else if (txtStartDate.Text == "")
            {
                lblStartMessage.Text = "Start Date empty !";
                lblStartMessage.ForeColor = Color.Red;
                MPStartPO.Show();
            }
            else if (drpTimeStartPO.SelectedValue == "")
            {
                lblStartMessage.Text = "Start Time empty !";
                lblStartMessage.ForeColor = Color.Red;
                MPStartPO.Show();
            }
            else
            {
                object result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 2),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@Date", lbldate.Text),
                    new SqlParameter("@PONumber", Convert.ToString(drpStartPO.SelectedValue)),
                    new SqlParameter("@StartDate",txtStartDate.Text.Trim()),
                    new SqlParameter("@StartTime",Convert.ToString(drpTimeStartPO.SelectedValue)),
                    new SqlParameter("@StartedBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@CurrentStatus","Active"),
                    new SqlParameter("@Gram",txtGramStart.Text.Trim()),
                    new SqlParameter("@CaseSize",txtCaseSizeStart.Text.Trim())
                };

                result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_DML_DATA");

                if (Convert.ToInt64(result) > 0)
                {
                    lblStartMessage.Text = "PO Start Run Success";
                    lblStartMessage.ForeColor = Color.Green;
                    CheckandAssignPo(drpStartPO.SelectedValue);
                    ClearStartPO();
                    BindActiveAndScheduledGrid();
                    MPStartPO.Hide();
                }
                else
                {
                    lblStartMessage.Text = "Error, Something went wrong!";
                    lblStartMessage.ForeColor = Color.Red;
                    MPStartPO.Show();
                }
            }
        }

        private void ClearStartPO()
        {
            BindStartPo();
            txtMaterialStartPO.Text = string.Empty;
            drpTimeStartPO.SelectedIndex = 0;
            txtGramStart.Text = string.Empty;
            txtCaseSizeStart.Text = string.Empty;
            txtStartDate.Text = lbldate.Text;
        }

        private void CheckandAssignPo(string PONumber)
        {
            try
            {
                int result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR",2),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", txtStartDate.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@PONumber",PONumber),
                    new SqlParameter("@StartTime",drpTimeStartPO.SelectedValue),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                };

                result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_SPLITENTRY");
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("CheckandSplitProductionEntry::", ex.Message), ex);
            }
        }
        #endregion

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
            else if (drpType.SelectedValue == "")
            {
                lblOperatorMessage.Text = "Please select Type";
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
                    new SqlParameter("@Type",Convert.ToString(drpType.SelectedValue)),
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
            drpType.SelectedValue = "";
        }


        #endregion

        #region SCHEDULE
        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            lblSchMessage.Text = string.Empty;
            BindSchedulePO();
            txtScheduleStart.Attributes.Add("readonly", "true");
            txtMaterialSchedulePO.Text = string.Empty;
            lblMaterialDescSchedulePO.Text = string.Empty;
            lblOrderQuantitySchedulePO.Text = string.Empty;
            txtScheduleStart.Text = lbldate.Text;
            BindTimeControl(drpTimeSchedulePO);
            txtGramSch.Text = string.Empty;
            txtCaseSizeSch.Text = string.Empty;
            MPSchedulePO.Show();
        }

        public void BindSchedulePO()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "1"),
                new SqlParameter("@Date", lbldate.Text),
                new SqlParameter("@PlantCode", lblplantcode.Text.Trim()),
                new SqlParameter("@Line", lblline.Text),
                };

                DataTable DtSchedulePO = new DataTable();
                DtSchedulePO = DBClass.GetDatatable_WithParamInGDRJ(param, "DM_SP_GET_IOT_DATA");
                Session["DtSchedulePO"] = DtSchedulePO;
                drpSchedulePO.Items.Clear();
                drpSchedulePO.DataSource = DtSchedulePO;
                drpSchedulePO.DataTextField = "ProcessOrderWithMatDesc";
                drpSchedulePO.DataValueField = "ProcessOrder";
                drpSchedulePO.DataBind();
                drpSchedulePO.Items.Insert(0, new ListItem("Select Process Order", ""));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindSchedulePO::", ex.Message), ex);
            }
        }

        protected void drpSchedulePO_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSchMessage.Text = string.Empty;
            if (drpSchedulePO.SelectedValue != "" && Session["DtSchedulePO"] != null)
            {
                DataTable DtProcessOrders = (DataTable)(Session["DtSchedulePO"]);

                DataRow[] results = DtProcessOrders.Select("ProcessOrder =" + drpSchedulePO.SelectedValue);

                if (results != null && results.Count() > 0)
                {
                    txtMaterialSchedulePO.Text = Convert.ToString(results[0]["MaterialCode"]);
                    lblMaterialDescSchedulePO.Text = Convert.ToString(results[0]["MaterialDescription"]);
                    lblOrderQuantitySchedulePO.Text = Convert.ToString(results[0]["OrderQuantity"]);
                    txtGramSch.Text = Convert.ToString(results[0]["Gram"]);
                    txtCaseSizeSch.Text = Convert.ToString(results[0]["CaseSize"]);
                }
                else
                {
                    txtMaterialSchedulePO.Text = string.Empty;
                    lblMaterialDescSchedulePO.Text = string.Empty;
                    lblOrderQuantitySchedulePO.Text = string.Empty;
                    txtGramSch.Text = string.Empty;
                    txtCaseSizeSch.Text = string.Empty;
                }
            }

            MPSchedulePO.Show();
        }

        protected void btnSchedulePo_Click(object sender, EventArgs e)
        {
            lblSchMessage.Text = string.Empty;
            if (drpSchedulePO.SelectedValue == "")
            {
                lblSchMessage.Text = "Process Order Empty !";
                lblSchMessage.ForeColor = Color.Red;
            }
            else if (txtScheduleStart.Text == "")
            {
                lblSchMessage.Text = "Schedule Date is empty!";
                lblSchMessage.ForeColor = Color.Red;
            }
            else if (drpTimeSchedulePO.SelectedValue == "")
            {
                lblSchMessage.Text = "Schedule Time empty!";
                lblSchMessage.ForeColor = Color.Red;
            }
            else if (txtGramSch.Text.Trim() == "")
            {
                lblSchMessage.Text = "Gram is empty!";
                lblSchMessage.ForeColor = Color.Red;
            }
            else if (txtCaseSizeSch.Text.Trim() == "")
            {
                lblSchMessage.Text = "Case Size is empty!";
                lblSchMessage.ForeColor = Color.Red;
            }
            else
            {
                object result;
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", 1),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text),
                new SqlParameter("@PONumber", Convert.ToString(drpSchedulePO.SelectedValue)),
                new SqlParameter("@MaterialCode", Convert.ToString(txtMaterialSchedulePO.Text)),
                new SqlParameter("@MaterialDescrip", Convert.ToString(lblMaterialDescSchedulePO.Text)),
                new SqlParameter("@OrderQuantity", Convert.ToString(lblOrderQuantitySchedulePO.Text)),
                new SqlParameter("@ScheduledDate", Convert.ToString(txtScheduleStart.Text)),
                new SqlParameter("@ScheduledTime",Convert.ToString(drpTimeSchedulePO.SelectedValue)),
                new SqlParameter("@ScheduledBy",Convert.ToString(Session["username"])),
                new SqlParameter("@CurrentStatus","Next"),
                new SqlParameter("@Gram",txtGramSch.Text),
                new SqlParameter("@CaseSize",txtCaseSizeSch.Text.Trim()),

                };

                result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_DML_DATA");

                if (Convert.ToInt64(result) > 0)
                {
                    lblSchMessage.Text = "PO Scheduled Success";
                    lblSchMessage.ForeColor = Color.Green;
                    ClearSchedulerFields();
                    BindActiveAndScheduledGrid();
                }
                else if (Convert.ToInt64(result) == -1)
                {
                    lblSchMessage.Text = "Process order already Scheduled!";
                    lblSchMessage.ForeColor = Color.Red;
                }
                else
                {
                    lblSchMessage.Text = "Error, Something went wrong!";
                    lblSchMessage.ForeColor = Color.Red;
                }
            }

            MPSchedulePO.Show();
        }

        private void ClearSchedulerFields()
        {
            drpSchedulePO.SelectedIndex = 0;
            txtMaterialSchedulePO.Text = string.Empty;
            lblMaterialDescSchedulePO.Text = string.Empty;
            lblOrderQuantitySchedulePO.Text = string.Empty;
            txtScheduleStart.Text = lbldate.Text;
            BindTimeControl(drpTimeSchedulePO);
            txtGramSch.Text = string.Empty;
            txtCaseSizeSch.Text = string.Empty;
        }
        #endregion

        #region STOP RUN
        protected void btnStop_Click(object sender, EventArgs e)
        {
            lblStopMessage.Text = string.Empty;
            BindStopPO();
            txtStopDate.Text = lbldate.Text;
            txtStopDate.Attributes.Add("readonly", "true");
            BindTimeControl(drpTimeStopPO);
            MPStopPO.Show();
        }

        public void BindStopPO()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "4"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text),
            };

            DataTable DtStopPO = new DataTable();
            DtStopPO = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");

            Session["DtStopPO"] = DtStopPO;

            drpStopPO.DataSource = DtStopPO;
            drpStopPO.DataTextField = "ProcessOrderWithMatDesc";
            drpStopPO.DataValueField = "ProcessOrder";
            drpStopPO.DataBind();
            drpStopPO.Items.Insert(0, new ListItem("Select Process Order", ""));

            if (DtStopPO != null && DtStopPO.Rows.Count > 0)
            {
                drpStopPO.SelectedValue = Convert.ToString(DtStopPO.Rows[0]["ProcessOrder"]);
                drpStopPO_SelectedIndexChanged(null, null);
            }
        }

        protected void drpStopPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblStopMessage.Text = string.Empty;
            if (drpStopPO.SelectedValue != "" && Session["DtStopPO"] != null)
            {
                DataTable DtProcessOrders = (DataTable)(Session["DtStopPO"]);

                DataRow[] results = DtProcessOrders.Select("ProcessOrder =" + drpStopPO.SelectedValue);

                if (results != null && results.Count() > 0)
                {
                    txtMaterialStopPO.Text = Convert.ToString(results[0]["MaterialCode"]);
                }
                else
                {
                    txtMaterialStopPO.Text = string.Empty;
                }
            }
            else
            {
                txtMaterialStopPO.Text = string.Empty;
            }

            MPStopPO.Show();
        }

        protected void btnStopPO_Click(object sender, EventArgs e)
        {
            lblStopMessage.Text = string.Empty;
            if (drpStopPO.SelectedValue == "")
            {
                lblStopMessage.Text = "Process Order Empty !";
                lblStopMessage.ForeColor = Color.Red;
                MPStopPO.Show();
            }
            else if (txtStopDate.Text == "")
            {
                lblStopMessage.Text = "Stop Date empty!";
                lblStopMessage.ForeColor = Color.Red;
                MPStopPO.Show();
            }
            else if (drpTimeStopPO.SelectedValue == "")
            {
                lblStopMessage.Text = "Stop Time empty!";
                lblStopMessage.ForeColor = Color.Red;
                MPStopPO.Show();
            }
            else
            {
                object result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 3),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@PONumber", Convert.ToString(drpStopPO.SelectedValue)),
                    new SqlParameter("@StoppageDate",Convert.ToString(txtStopDate.Text)),
                    new SqlParameter("@StoppageTime",Convert.ToString(drpTimeStopPO.SelectedValue)),
                    new SqlParameter("@StoppageBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@CurrentStatus","Completed")
                };

                result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_DML_DATA");

                if (Convert.ToInt64(result) > 0)
                {
                    ViewState["Active"] = null;
                    lblStopMessage.Text = "PO Stop Run Success";
                    lblStopMessage.ForeColor = Color.Green;
                    CheckandSplitProductionEntry(drpStopPO.SelectedValue);
                    ClearStopPO();
                    BindActiveAndScheduledGrid();
                    MPStopPO.Hide();
                }
                else if (Convert.ToInt64(result) == -1)
                {
                    lblStopMessage.Text = "Error, Future Date time not allowed!";
                    lblStopMessage.ForeColor = Color.Red;
                    MPStopPO.Show();
                }
                else
                {
                    lblStopMessage.Text = "Error, Something went wrong!";
                    lblStopMessage.ForeColor = Color.Red;
                    MPStopPO.Show();
                }
            }
        }

        private void ClearStopPO()
        {
            txtMaterialStopPO.Text = string.Empty;
            txtStopDate.Text = string.Empty;
            drpTimeStopPO.SelectedIndex = 0;
            BindStopPO();
        }

        private void CheckandSplitProductionEntry(string PONumber)
        {
            try
            {
                int result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR",1),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", txtStopDate.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@PONumber",PONumber),
                    new SqlParameter("@StoppageTime",drpTimeStopPO.SelectedValue),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                };

                result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_SPLITENTRY");
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("CheckandSplitProductionEntry::", ex.Message), ex);
            }
        }
        #endregion

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

        protected void btnstoppages_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoppagesReason.aspx", false);
        }

        protected void btnProductions_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductionDetails.aspx", false);
        }

        protected void btnParameter_Click(object sender, EventArgs e)
        {
            Response.Redirect("ParameterDetails.aspx", false);
        }
    }
}