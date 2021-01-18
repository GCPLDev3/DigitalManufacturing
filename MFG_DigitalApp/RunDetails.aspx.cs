using MFG_DigitalApp.Log;
using MFG_DigitalApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
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
        public static string labeldate = "";
        public static string plant = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetOperatorByEDP(string.Empty,0);
                if (Session["UserSelectionModel"] != null)
                {
                    UserSelectionModel model = new UserSelectionModel();
                    model = (UserSelectionModel)Session["UserSelectionModel"];
                    lblplantcode.Text = model.PlantCode;
                    lblline.Text = model.Line;
                    if (model.ShiftCode.Equals("S4"))
                    {
                        lblshiftnew.Text = "S1";
                    }
                    else if (model.ShiftCode.Equals("S5"))
                    {
                        lblshiftnew.Text = "S2";
                    }
                    else if (model.ShiftCode.Equals("S6"))
                    {
                        lblshiftnew.Text = "S3";
                    }
                    else
                    {
                        lblshiftnew.Text = model.ShiftCode;
                    }
                    lblshift.Text = model.ShiftCode;
                    Session["Line"] = model.Line;
                    plant = lblplantcode.Text;
                    //lbldate.Text = model.ShiftDate;
                    //var date1 = DateTime.Now.ToString("dd-MM-yyyy");
                    var date = model.ShiftDate.ToString().Substring(0, model.ShiftDate.IndexOf(' '));
                    DateTime dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                    //var dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //DateTime datetime = DateTime.Parse(date);
                    //var FormatedDate = dateTime.ToString("yyyy-MM-dd");
                    labeldate = dateTime.ToString("yyyy-MM-dd");

                    if (lblshift.Text.Equals("S3") || lblshift.Text.Equals("S6"))
                    {
                        if (Session["CountryId"].ToString().Trim() == "1" && DateTime.Now.Hour <= 6)
                        {
                            lbldate.Text = dateTime.AddDays(-1).ToShortDateString();
                        }
                        else if (Session["CountryId"].ToString().Trim() == "2" && ((DateTime.Now.Hour <= 7 && DateTime.Now.Minute <= 30) || (DateTime.Now.Hour == 23) || (DateTime.Now.Hour >= 22 && DateTime.Now.Minute >= 30)))
                        {
                            lbldate.Text = dateTime.AddDays(-1).ToShortDateString();
                        }
                        else
                        {
                            lbldate.Text = dateTime.ToShortDateString();
                        }
                        //lbldate.Text = dateTime.AddDays(-1).ToShortDateString();
                    }
                    else
                    {
                        // DateTime shiftDay = Convert.ToDateTime(model.ShiftDate);
                        lbldate.Text = dateTime.ToShortDateString();
                    }
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
            UserSelectionModel model = new UserSelectionModel();
            model = (UserSelectionModel)Session["UserSelectionModel"];
            var date = model.ShiftDate;
            //DateTime datetime = DateTime.Parse(date);
            //var FormatedDate = datetime.ToString("yyyy-MM-dd");
            DateTime dateTime = DateTime.ParseExact(date.Substring(0, date.IndexOf(" ")).Replace("-", "/"), "dd/MM/yyyy", null);
            var FormatedDate = dateTime.ToString("yyyy-MM-dd");
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "2"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@Date", FormatedDate),
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

        //protected void GetShift(string date)
        //{
        //    SqlParameter[] param = new SqlParameter[]
        //    {
        //       new SqlParameter("@OPR", "13"),
        //        new SqlParameter("@PlantCode",lblplantcode.Text),
        //        new SqlParameter("@Date",date)
        //    };

        //    DataTable Dt = new DataTable();
        //    Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
        //    ddlShift.DataSource = Dt;
        //    ddlShift.DataTextField = "ShiftName";
        //    ddlShift.DataValueField = "ShiftCode";
        //    ddlShift.DataBind();
        //}
            protected void GrdActiveAndScheduled_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (lblplantcode.Text == "M016")
            {
                GrdActiveAndScheduled.Columns[9].Visible = true;
                GrdActiveAndScheduled.Columns[10].Visible = false;
            }
            else if (lblplantcode.Text == "M024" && (lblline.Text == "MT Cream 1" || lblline.Text == "MT Cream 2" || lblline.Text == "MT Devlop 1" || lblline.Text == "MT Devlop 2"))
            {
                GrdActiveAndScheduled.Columns[9].Visible = false;
                GrdActiveAndScheduled.Columns[10].Visible = true;
            }
            else
            {
                GrdActiveAndScheduled.Columns[9].Visible = false;
                GrdActiveAndScheduled.Columns[10].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                LinkButton lnkDeletePO = (LinkButton)e.Row.FindControl("lnkDeletePO");
                LinkButton lnkRestartPO = (LinkButton)e.Row.FindControl("lnkRestartPO");
                if (e.Row.Cells[0].Text.Equals("Active"))
                {
                    ViewState["Active"] = "Active";
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                    if (lnkDeletePO != null)
                        lnkDeletePO.Visible = false;
                    lnkRestartPO.Visible = false;
                }
                else if (e.Row.Cells[0].Text.Equals("Completed"))
                {
                    ViewState["Completed"] = "Completed";
                    e.Row.BackColor = System.Drawing.Color.LightCyan;
                    lnkDeletePO.Visible = false;
                    lnkRestartPO.Visible = true;
                }
                else if (e.Row.Cells[0].Text.Equals("Next"))
                {
                    ViewState["Next"] = "Next";
                    lnkDeletePO.Visible = true;
                    lnkRestartPO.Visible = false;
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
            txtStartDate.Text = labeldate;
            txtStartDate.Attributes.Add("readonly", "true");
            BindTimeControl(drpTimeStartPO);
            BindTimeControl(ddlRestartTime);

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

                DataRow[] results = DtProcessOrders.Select("ProcessOrder = '" + drpStartPO.SelectedValue + "'");

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
            string indotime = hdnfldVariable.Value;
            lblSchMessage.Text = string.Empty;
            indotime = indotime.ToString().Substring(0, indotime.IndexOf(' '));
            indotime = DateTime.ParseExact(indotime.Replace("-", "/"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
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
                    new SqlParameter("@Date", labeldate),
                    new SqlParameter("@PONumber", Convert.ToString(drpStartPO.SelectedValue)),
                    new SqlParameter("@StartDate",txtStartDate.Text.Trim()),
                    new SqlParameter("@StartTime",Convert.ToString(drpTimeStartPO.SelectedValue)),
                    new SqlParameter("@StartedBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@CurrentStatus","Active"),
                    new SqlParameter("@Gram",txtGramStart.Text.Trim()),
                    new SqlParameter("@CaseSize",txtCaseSizeStart.Text.Trim()),
                    new SqlParameter("@additionalqty","0")
                };

                result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_DML_DATA");

                if (Convert.ToInt64(result) > 0)
                {
                    SqlParameter[] param_Active = new SqlParameter[]
                                                 {
                                            new SqlParameter("@OPR", "6"),
                                            new SqlParameter("@PlantCode", lblplantcode.Text),
                                            new SqlParameter("@Line", lblline.Text),
                                                 };
                    DataTable dtlineActive = DBClass.GetDatatable_WithParam(param_Active, "DM_SP_InsertProductionDetails");
                    if (dtlineActive != null)
                    {
                        if (dtlineActive.Rows.Count > 0)
                        {
                            string ProcessOrder = dtlineActive.Rows[0]["PONumber"].ToString();
                            SqlParameter[] param_Batch = new SqlParameter[]
                               {
                                                    //new SqlParameter("@OPR", "2"),
                                                    new SqlParameter("@OPR", "2"),
                                                    new SqlParameter("@ProcessOrder", ProcessOrder),
                                                    new SqlParameter("@PlantCode",lblplantcode.Text)
                               };
                            DataTable dtlineBatch = DBClass.GetDatatable_WithParamInGDRJ(param_Batch, "DM_SP_GET_IOT_DATA");
                            //DataTable dtlineBatch = DBClass.GetDatatable_WithParam(param_Batch, "DM_SP_GET_IOT_DATA");
                            if (dtlineBatch != null)
                            {
                                if (dtlineBatch.Rows.Count > 0)
                                {
                                    string BatchNumber = dtlineBatch.Rows[0]["BatchNo"].ToString();
                                    SqlParameter[] param_BatchUpd = new SqlParameter[]
                                        {
                                                            new SqlParameter("@OPR", "7"),
                                                            new SqlParameter("@ProcessOrder", ProcessOrder),
                                                            new SqlParameter("@BatchNumber", BatchNumber),
                                        };
                                    int main = DBClass.ExecuteNonQuery_WithParam(param_BatchUpd, "DM_SP_InsertProductionDetails");
                                }
                            }
                        }
                    }
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
            txtStartDate.Text = labeldate;
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
                    new SqlParameter("@Date", Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd")),
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

        private void CheckandAssignPoforrestant(string PONumber, string date)
        {
            try
            {
                int result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR",2),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", Convert.ToDateTime(date).ToString("yyyy-MM-dd")),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@PONumber",PONumber),
                    new SqlParameter("@StartTime",DateTime.Now.ToString("HH:mm")),
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
            //if (!IsPostBack)
            //{
            //    BindOperators();
            //}
            lblOperatorMessage.Text = string.Empty;
            BindOperators();
            //GetShift(labeldate);
            BindStations();
            //BindEDP();
            ddlShift.SelectedValue = lblshift.Text;
            BindOperatorsGrd();
            MPAssignOperator.Show();
        }

        public void BindOperatorsGrd()
        {
            var date = lbldate.Text.ToString();
            DateTime datetime = DateTime.Parse(date);
            string formatteddate = datetime.ToString("yyyy-MM-dd");

            //DateTime dateTime = DateTime.ParseExact(lbldate.Text.Replace("-", "/"), "MM/dd/yyyy", null);
            //var date = dateTime.ToString().Substring(0, dateTime.ToString().IndexOf(' '));
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "8"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@Date", formatteddate),
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
                new SqlParameter("@OPR", "57"),
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

        //public void BindEDP()
        //{
        //    SqlParameter[] param = new SqlParameter[]
        //    {
        //        new SqlParameter("@OPR", "10"),
        //        new SqlParameter("@plant", lblplantcode.Text)
        //    };

        //    DataTable DtOperators = new DataTable();
        //    DtOperators = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
        //    drpEDP.DataSource = DtOperators;
        //    drpEDP.DataTextField = "EDPNumber";
        //    drpEDP.DataValueField = "EDPNumber";
        //    drpEDP.DataBind();
        //    drpEDP.Items.Insert(0, new ListItem("Select EDP Number", ""));
        //}

        protected void btnAddOperator_Click(object sender, EventArgs e)
        {
            lblOperatorMessage.Text = string.Empty;
            if (drpStation.SelectedValue == "")
            {
                lblOperatorMessage.Text = "Please select Station";
                lblOperatorMessage.ForeColor = Color.Red;
            }
            else if (txtEDP.Text== ""|| txtEDP.Text==null)
            {
                lblOperatorMessage.Text = "Please select EDP number";
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

                //DateTime dateTime = DateTime.ParseExact(lbldate.Text.ToString().Replace("-", "/"), "dd/MM/yyyy", null);
                //DateTime dateTime = DateTime.ParseExact(lbldate.Text.Replace("-", "/"), "MM/dd/yyyy", null);
                //var date = dateTime.ToString().Substring(0, dateTime.ToString().IndexOf(' '));

                //lbldate.Text = dateTime.ToString("yyyy-MM-dd"); ;
                //labeldate = Convert.ToDateTime(date).ToString("yyyy-MM-dd");

                var date = lbldate.Text.ToString();
                DateTime datetime = DateTime.Parse(date);
                string formatteddate = datetime.ToString("yyyy-MM-dd");
                object result;
                string shift = string.Empty;
                if(Session["countryId"].ToString()=="2")
                {
                    if(ddlShift.SelectedValue=="S1")
                    {
                        shift = "S4";
                    }
                    else if (ddlShift.SelectedValue == "S2")
                    {
                        shift = "S5";
                    }
                    else if (ddlShift.SelectedValue == "S3")
                    {
                        shift = "S6";
                    }
                    else
                    {
                        shift = lblshift.Text;
                    }
                }
                else
                {
                    if(ddlShift.SelectedValue=="Select")
                    {
                        shift = lblshift.Text;
                    }
                    else
                    {
                        shift = ddlShift.SelectedValue;
                    }
                }
                
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 4),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", formatteddate),
                    //new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@ShiftCode", shift),
                    new SqlParameter("@StationCode",Convert.ToString(drpStation.SelectedValue)),
                    new SqlParameter("@OperatorName",drpOperator.SelectedValue),
                    new SqlParameter("@Type",Convert.ToString(drpType.SelectedValue)),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@edp",txtEDP.Text)
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
            ddlShift.SelectedValue = lblshift.Text;
            drpOperator.SelectedIndex = -1;
            txtEDP.Text = string.Empty;
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
            //BindOperators();
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
            drpBatch.Enabled = true;
            drpSchedulePO.Enabled = true;
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "1"),
                //new SqlParameter("@Date", lbldate.Text),
                new SqlParameter("@Date", labeldate),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text)
                };

                DataTable DtSchedulePO = new DataTable();
                //DtSchedulePO = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_IOT_DATA");
                DtSchedulePO = DBClass.GetDatatable_WithParamInGDRJ(param, "DM_SP_GET_IOT_DATA");
                DataTable newDt = new DataTable();
                newDt = DtSchedulePO.Clone();
                if (DtSchedulePO != null)
                {
                    foreach (DataRow row in DtSchedulePO.Rows)
                    {
                        SqlParameter[] parameter = new SqlParameter[]
                        {
                            new SqlParameter("@OPR", "4"),
                            new SqlParameter("@ProcessOrder", row["ProcessOrder"].ToString()),
                            new SqlParameter("@BatchNumber", row["BatchNumber"].ToString()),
                            new SqlParameter("@Line", Session["Line"].ToString())
                        };
                        DataTable obj = DBClass.GetDatatable_WithParam(parameter, "DM_SP_GET_IOT_DATA");
                        if (obj != null)
                        {
                            if (Convert.ToInt32(obj.Rows[0][0].ToString()) == 0)
                                newDt.Rows.Add(row.ItemArray);
                        }
                    }
                }
                Session["DtSchedulePO"] = newDt;
                drpSchedulePO.Items.Clear();
                drpSchedulePO.DataSource = newDt;
                drpSchedulePO.DataTextField = "ProcessOrderWithMatDesc";
                //drpSchedulePO.DataValueField = "ID";
                drpSchedulePO.DataValueField = "ProcessOrder";
                drpSchedulePO.DataBind();
                drpSchedulePO.Items.Insert(0, new ListItem("Select Process Order", ""));
                drpBatch.Items.Clear();
                drpBatch.DataSource = newDt;
                drpBatch.DataTextField = "BatchNumber";
                drpBatch.DataValueField = "BatchNumber";
                drpBatch.DataBind();
                drpBatch.Items.Insert(0, new ListItem("Select Batch Number", ""));
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

                //DataRow[] results = DtProcessOrders.Select("ID =" + drpSchedulePO.SelectedValue);
                DataRow[] results = DtProcessOrders.Select("ProcessOrder =" + drpSchedulePO.SelectedValue);

                if (results != null && results.Count() > 0)
                {
                    txtMaterialSchedulePO.Text = Convert.ToString(results[0]["MaterialCode"]);
                    lblMaterialDescSchedulePO.Text = Convert.ToString(results[0]["MaterialDescription"]);
                    lblOrderQuantitySchedulePO.Text = Convert.ToString(results[0]["OrderQuantity"]);
                    txtGramSch.Text = Convert.ToString(results[0]["Gram"]);
                    txtCaseSizeSch.Text = Convert.ToString(results[0]["CaseSize"]);
                    drpBatch.SelectedValue = Convert.ToString(results[0]["BatchNumber"]);
                    drpBatch.Enabled = false;
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
            string indotime = hdnfldVariable.Value;
            lblSchMessage.Text = string.Empty;
            indotime = indotime.ToString().Substring(0, indotime.IndexOf(' '));
            indotime = DateTime.ParseExact(indotime.Replace("-", "/"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
            //indotime = indotime.ToString("dd-MM-yyyy");
            if (drpSchedulePO.SelectedValue == "")
            {
                //lblSchMessage.Text = "Process Order Empty !";
                //lblSchMessage.ForeColor = Color.Red;
                if (drpBatch.SelectedValue == "")
                {
                    lblSchMessage.Text += "Batch Number Empty !";
                    lblSchMessage.ForeColor = Color.Red;

                    lblSchMessage.Text = "   Process Order Empty !";
                    lblSchMessage.ForeColor = Color.Red;
                }
            }
            //else if (drpBatch.SelectedValue == "")
            //{
            //    lblSchMessage.Text += "Batch Number Empty !";
            //    lblSchMessage.ForeColor = Color.Red;
            //}
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
                new SqlParameter("@OPR", "1"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text),
                new SqlParameter("@PONumber", Convert.ToString(drpSchedulePO.SelectedValue)),
                new SqlParameter("@batch",Convert.ToString(drpBatch.SelectedValue)),
                new SqlParameter("@MaterialCode", Convert.ToString(txtMaterialSchedulePO.Text)),
                new SqlParameter("@MaterialDescrip", Convert.ToString(lblMaterialDescSchedulePO.Text)),
                new SqlParameter("@OrderQuantity", Convert.ToString(lblOrderQuantitySchedulePO.Text)),
                //new SqlParameter("@ScheduledDate",indotime),
                new SqlParameter("@ScheduledDate",Convert.ToDateTime(Convert.ToString(txtScheduleStart.Text)).ToString("yyyy-MM-dd")),
                new SqlParameter("@ScheduledTime",Convert.ToString(drpTimeSchedulePO.SelectedValue)),
                new SqlParameter("@ScheduledBy",Convert.ToString(Session["username"])),
                new SqlParameter("@CurrentStatus","Next"),
                new SqlParameter("@Gram",txtGramSch.Text),
                new SqlParameter("@CaseSize",txtCaseSizeSch.Text.Trim())

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
            txtStopDate.Text = labeldate;
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
            string indotime = "", Curtime = "";
            if (Session["CountryId"] != null && Session["CountryId"].Equals("2"))
            {
                indotime = hdnfldVariable.Value;
                var date = indotime.ToString().Substring(0, indotime.IndexOf(' '));
                TimeSpan time = TimeSpan.Parse(indotime.Substring(indotime.IndexOf(' '), indotime.Length - indotime.IndexOf(' ')));
                //Curtime = indotime.Substring(12, 17 - 12);
                //DateTime currentTime = Convert.ToDateTime(indotime);
                //string hr = Convert.ToString(currentTime.Hour);
                //string mm = Convert.ToString(currentTime.Minute);
                string hr = time.Hours.ToString();
                string mm = time.Minutes.ToString();
                Curtime = hr + ":" + mm;
                DateTime dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                labeldate = dateTime.ToString("yyyy-MM-dd");
            }
            else if (Session["CountryId"] != null && Session["CountryId"].Equals("1"))
            {
                //indotime = Convert.ToString(DateTime.Now);
                //var date = indotime.ToString().Substring(0, indotime.IndexOf(' '));
                //DateTime currentTime = Convert.ToDateTime(indotime);
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string hr = Convert.ToString(DateTime.Now.Hour);
                string mm = Convert.ToString(DateTime.Now.Minute);
                Curtime = hr + ":" + mm;
                //DateTime dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                //labeldate = dateTime.ToString("yyyy-MM-dd");
                labeldate = DateTime.Now.ToString("yyyy-MM-dd");

            }
            lblSchMessage.Text = string.Empty;
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
                    new SqlParameter("@OPR", 22),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@PONumber", Convert.ToString(drpStopPO.SelectedValue)),
                    new SqlParameter("@batch", Convert.ToString(drpBatch.SelectedValue)),
                    new SqlParameter("@StoppageDate",Convert.ToString(txtStopDate.Text)),
                    new SqlParameter("@StoppageTime",Convert.ToString(drpTimeStopPO.SelectedValue)),
                    new SqlParameter("@StoppageBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@CurrentStatus","Completed"),
                    new SqlParameter("@Currentdate",Convert.ToString(labeldate)),
                    new SqlParameter("@Currenttime",Convert.ToString(Curtime))

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
        protected void drpBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSchMessage.Text = string.Empty;
            if (drpBatch.SelectedValue != "" && Session["DtSchedulePO"] != null)
            {
                DataTable DtProcessOrders = new DataTable();
                DtProcessOrders = (DataTable)(Session["DtSchedulePO"]);

                DataRow[] results = DtProcessOrders.Select("BatchNumber like '%" + drpBatch.SelectedValue + "%'");

                if (results != null && results.Count() > 0)
                {
                    txtMaterialSchedulePO.Text = Convert.ToString(results[0]["MaterialCode"]);
                    lblMaterialDescSchedulePO.Text = Convert.ToString(results[0]["MaterialDescription"]);
                    lblOrderQuantitySchedulePO.Text = Convert.ToString(results[0]["OrderQuantity"]);
                    txtGramSch.Text = Convert.ToString(results[0]["Gram"]);
                    txtCaseSizeSch.Text = Convert.ToString(results[0]["CaseSize"]);
                    drpSchedulePO.SelectedValue = Convert.ToString(results[0]["ProcessOrder"]);
                    drpSchedulePO.Enabled = false;
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

        public void BindTimeControl(DropDownList drp)
        {
            string indotime = "";
            if (Session["CountryId"] != null && Session["CountryId"].Equals("2"))
            {
                indotime = hdnfldVariable.Value;
                indotime = indotime.ToString().Substring(0, indotime.IndexOf(' '));
                indotime = DateTime.ParseExact(indotime.Replace("-", "/"), "dd/MM/yyyy", null).ToString();
            }
            else if (Session["CountryId"] != null && Session["CountryId"].Equals("1"))
            {
                indotime = DateTime.Now.Date.ToString("yyyy-MM-dd");
                //indotime = indotime.ToString().Substring(0, indotime.IndexOf(' '));
                //indotime = DateTime.ParseExact(indotime.Replace("-", "/"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
            }

            lblSchMessage.Text = string.Empty;
            //indotime = indotime.ToString().Substring(0, indotime.IndexOf(' '));
            SqlParameter[] param = new SqlParameter[]
            {
                //new SqlParameter("@OPR", "56")
                new SqlParameter("@OPR", "1"),
                new SqlParameter("@PlantCode", lblplantcode.Text.Trim())
            };

            drp.Items.Clear();
            DataTable DtTimes = new DataTable();
            DtTimes = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_TIME");
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

        protected void lnkRestartPO_Click(object sender, EventArgs e)
        {
            LinkButton lnkDeletePO = (LinkButton)sender;
            var PO = lnkDeletePO.CommandArgument;
            BindTimeControl(ddlRestartTime);
            SqlParameter[] param1 = new SqlParameter[]
            {
                    new SqlParameter("@OPR", "25"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@PONumber", PO)
            };
            DataTable Dt1 = DBClass.GetDatatable_WithParam(param1, "DM_SP_DML_DATA");
            Session["Autostop"] = Dt1;
            UserSelectionModel model = new UserSelectionModel();
            model = (UserSelectionModel)Session["UserSelectionModel"];
            var date = model.ShiftDate;
            DateTime dateTime = DateTime.ParseExact(date.Substring(0, date.IndexOf(" ")).Replace("-", "/"), "dd/MM/yyyy", null);
            var FormatedDate = dateTime.ToString("yyyy-MM-dd");
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "75"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Convert.ToInt16(Dt.Rows[0]["Activecount"].ToString()) == 0)
                {
                    SqlParameter[] paramlast = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "76"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                };

                    DataTable Dtlast = new DataTable();
                    Dtlast = DBClass.GetDatatable_WithParam(paramlast, "DM_SP_GET_DATA");
                    txtRestartDate.Text = Convert.ToDateTime(Dtlast.Rows[0]["StoppageDate"].ToString()).ToString("yyyy-MM-dd");
                    ddlRestartTime.SelectedValue = Convert.ToDateTime(Dtlast.Rows[0]["StoppageTime"].ToString()).ToString("HH:mm");
                    Session["LastTime"] = Dtlast.Rows[0]["StoppageTime"].ToString();
                    MPRestartPO.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('PO is already active in line, you can not restart this PO')", true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindActiveAndScheduledGrid::", ex.Message), ex);
            }
        }

        protected void btnRestart_Click(object sender, EventArgs e)
        {
            lblReStartMessage.Text = string.Empty;
            Session["CurTime"] = ddlRestartTime.SelectedValue;
            Session["date"] = txtRestartDate.Text;
            Session["qty"] = txtPOQuantity.Text;
            if (Convert.ToDateTime(ddlRestartTime.SelectedValue) > Convert.ToDateTime(Convert.ToDateTime(Session["LastTime"].ToString()).ToString("HH:mm")))
            {
                string addquantity = txtPOQuantity.Text;
                DataTable dt = (DataTable)Session["Autostop"];
                string quantity = Convert.ToString(Convert.ToInt16(dt.Rows[0]["AdditionalQuantity"].ToString()) + Convert.ToInt16(txtPOQuantity.Text));

                if (dt.Rows.Count > 0)
                {
                    object result;
                    SqlParameter[] param = new SqlParameter[]
                    {
                    new SqlParameter("@OPR", 26),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@PONumber", dt.Rows[0]["PONumber"].ToString()),
                    new SqlParameter("@StartDate",txtRestartDate.Text),
                    new SqlParameter("@StartTime",ddlRestartTime.SelectedValue),
                    new SqlParameter("@StartedBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@additionalqty",quantity),
                    new SqlParameter("@addqty",addquantity)
                    };

                    result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_DML_DATA");

                    if (Convert.ToInt64(result) > 0)
                    {
                        SqlParameter[] param_Active = new SqlParameter[]
                                                     {
                                            new SqlParameter("@OPR", "6"),
                                            new SqlParameter("@PlantCode", lblplantcode.Text),
                                            new SqlParameter("@Line", lblline.Text),
                                                     };
                        DataTable dtlineActive = DBClass.GetDatatable_WithParam(param_Active, "DM_SP_InsertProductionDetails");
                        if (dtlineActive != null)
                        {
                            if (dtlineActive.Rows.Count > 0)
                            {
                                string ProcessOrder = dtlineActive.Rows[0]["PONumber"].ToString();
                                SqlParameter[] param_Batch = new SqlParameter[]
                                   {
                                                    //new SqlParameter("@OPR", "2"),
                                                    new SqlParameter("@OPR", "2"),
                                                    new SqlParameter("@PlantCode",lblplantcode.Text),
                                                    new SqlParameter("@ProcessOrder", ProcessOrder)
                                   };
                                DataTable dtlineBatch = DBClass.GetDatatable_WithParamInGDRJ(param_Batch, "DM_SP_GET_IOT_DATA");
                                //DataTable dtlineBatch = DBClass.GetDatatable_WithParam(param_Batch, "DM_SP_GET_IOT_DATA");
                                if (dtlineBatch != null)
                                {
                                    if (dtlineBatch.Rows.Count > 0)
                                    {
                                        string BatchNumber = dtlineBatch.Rows[0]["BatchNo"].ToString();
                                        SqlParameter[] param_BatchUpd = new SqlParameter[]
                                            {
                                                            new SqlParameter("@OPR", "7"),
                                                            new SqlParameter("@ProcessOrder", ProcessOrder),
                                                            new SqlParameter("@BatchNumber", BatchNumber),
                                            };
                                        int main = DBClass.ExecuteNonQuery_WithParam(param_BatchUpd, "DM_SP_InsertProductionDetails");
                                    }
                                }
                            }
                        }
                        lblStartMessage.Text = "PO Start Run Success";
                        lblStartMessage.ForeColor = Color.Green;
                        CheckandAssignPoforrestant(dt.Rows[0]["PONumber"].ToString(), labeldate);
                        ClearStartPO();
                        BindActiveAndScheduledGrid();
                        //MPRestartPO.Show();
                        Response.Redirect("~/RunDetails.aspx");
                    }
                    else
                    {
                        lblReStartMessage.Text = "Error, Something went wrong!";
                        lblReStartMessage.ForeColor = Color.Red;
                        MPRestartPO.Show();
                    }
                }
            }
            else
            {
                lblReStartMessage.Text = "Time must be greated than selected time";
                lblReStartMessage.ForeColor = Color.Red;
                ddlRestartTime.SelectedValue = Session["CurTime"].ToString();
                txtRestartDate.Text = Session["date"].ToString();
                txtPOQuantity.Text = Session["qty"].ToString();
                MPRestartPO.Show();
            }
        }

        protected void lnkRestartPOCompleted_Click(object sender, EventArgs e)
        {
            //LinkButton lnkDeletePO = (LinkButton)sender;
            //var PO = lnkDeletePO.CommandArgument;
            //SqlParameter[] param1 = new SqlParameter[]
            //{
            //        new SqlParameter("@OPR", "25"),
            //        new SqlParameter("@PlantCode", lblplantcode.Text),
            //        new SqlParameter("@Line", lblline.Text),
            //        new SqlParameter("@PONumber", PO)
            //};
            //DataTable Dt1 = DBClass.GetDatatable_WithParam(param1, "DM_SP_DML_DATA");
            //Session["Autostop"] = Dt1;
            //UserSelectionModel model = new UserSelectionModel();
            //model = (UserSelectionModel)Session["UserSelectionModel"];
            //var date = model.ShiftDate;
            //DateTime dateTime = DateTime.ParseExact(date.Substring(0, date.IndexOf(" ")).Replace("-", "/"), "dd/MM/yyyy", null);
            //var FormatedDate = dateTime.ToString("yyyy-MM-dd");
            //try
            //{
            //    SqlParameter[] param = new SqlParameter[]
            //    {
            //        new SqlParameter("@OPR", "75"),
            //        new SqlParameter("@PlantCode", lblplantcode.Text),
            //        new SqlParameter("@Line", lblline.Text),
            //    };

            //    DataTable Dt = new DataTable();
            //    Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            //    if (Convert.ToInt16(Dt.Rows[0]["Activecount"].ToString()) == 0)
            //    {
            //        MPRestartPO.Show();
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('PO is already active in line, you can not restart this PO')", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(string.Concat("BindActiveAndScheduledGrid::", ex.Message), ex);
            //}
            LinkButton lnkDeletePO = (LinkButton)sender;
            var PO = lnkDeletePO.CommandArgument;
            BindTimeControl(ddlRestartTime);
            SqlParameter[] param1 = new SqlParameter[]
            {
                    new SqlParameter("@OPR", "25"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@PONumber", PO)
            };
            DataTable Dt1 = DBClass.GetDatatable_WithParam(param1, "DM_SP_DML_DATA");
            Session["Autostop"] = Dt1;
            UserSelectionModel model = new UserSelectionModel();
            model = (UserSelectionModel)Session["UserSelectionModel"];
            var date = model.ShiftDate;
            DateTime dateTime = DateTime.ParseExact(date.Substring(0, date.IndexOf(" ")).Replace("-", "/"), "dd/MM/yyyy", null);
            var FormatedDate = dateTime.ToString("yyyy-MM-dd");
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "75"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Convert.ToInt16(Dt.Rows[0]["Activecount"].ToString()) == 0)
                {
                    SqlParameter[] paramlast = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "76"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                };

                    DataTable Dtlast = new DataTable();
                    Dtlast = DBClass.GetDatatable_WithParam(paramlast, "DM_SP_GET_DATA");
                    txtRestartDate.Text = Convert.ToDateTime(Dtlast.Rows[0]["StoppageDate"].ToString()).ToString("yyyy-MM-dd");
                    ddlRestartTime.SelectedValue = Convert.ToDateTime(Dtlast.Rows[0]["StoppageTime"].ToString()).ToString("HH:mm");
                    Session["LastTime"] = Dtlast.Rows[0]["StoppageTime"].ToString();
                    MPRestartPO.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('PO is already active in line, you can not restart this PO')", true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindActiveAndScheduledGrid::", ex.Message), ex);
            }
        }

        protected void GrdCompleted_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        //protected void drpEDP_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    SqlParameter[] param = new SqlParameter[]
        //        {
        //            new SqlParameter("@OPR", "8"),
        //            new SqlParameter("@edp",drpEDP.SelectedValue),
        //            new SqlParameter("@plant",lblplantcode.Text)
        //        };

        //    DataTable Dt = new DataTable();
        //    Dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
        //    drpOperator.SelectedValue = Convert.ToString(Dt.Rows[0]["OperatorName"]);
        //    MPAssignOperator.Show();
        //}

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetOperatorByEDP(string prefixText,int count)
        {
            DataBaseConnectClass DBClass = new DataBaseConnectClass();
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "8"),
                    new SqlParameter("@query",prefixText),
                    new SqlParameter("@plant",plant)
                };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
            List<string> operators = new List<string>();

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                operators.Add(Dt.Rows[i]["OperatorName"].ToString());
            }
            return operators;
        }

        protected void txtEDP_TextChanged(object sender, EventArgs e)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "8"),
                    new SqlParameter("@query",txtEDP.Text),
                    new SqlParameter("@plant",lblplantcode.Text)
                };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
            if (Dt.Rows.Count > 0)
            {
                BindOperators();
                drpOperator.SelectedValue = Convert.ToString(Dt.Rows[0]["OperatorName1"]);
                drpType.SelectedValue = Convert.ToString(Dt.Rows[0]["Type"]);
            }
            else
            {
                
                drpOperator.DataSource = null;
                drpOperator.DataBind();
                drpOperator.Items.Insert(0, new ListItem("Operator does not exists", ""));
            }
            MPAssignOperator.Show();
        }

        protected void drpOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "9"),
                    new SqlParameter("@query",drpOperator.SelectedValue),
                    new SqlParameter("@plant",lblplantcode.Text)
                };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
            //drpEDP.SelectedValue = Convert.ToString(Dt.Rows[0]["EDPNumber"]);
            txtEDP.Text = Convert.ToString(Dt.Rows[0]["EDPNumber"]);
            drpType.SelectedValue = Convert.ToString(Dt.Rows[0]["Type"]);
            MPAssignOperator.Show();
        }
    }
}