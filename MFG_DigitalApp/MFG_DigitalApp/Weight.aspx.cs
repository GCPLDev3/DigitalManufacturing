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
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;

namespace MFG_DigitalApp
{
    public partial class Weight : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("Weight");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            List<string> keys = Request.Form.AllKeys.Where(key =>key!=null && key.Contains("txtDynamic")).ToList();
            int i = 1;
            foreach (string key in keys)
            {
                this.CreateTextBox();
                i++;
            }
        }

    

        private void CreateTextBox()
        {           
            int Sample= Convert.ToInt16(Session["Sample"]);
            System.Web.UI.HtmlControls.HtmlGenericControl rowDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
            rowDiv.Attributes.Add("class", "row form-group");
            for (int i = 1; i <= Sample; i++)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl newDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                newDiv.Attributes.Add("class", "txtboxes col-md-4");
                Label label = new Label();
                label.ID = "lbl" + i;
                label.Text = "Sample" + i;
                TextBox textBox = new TextBox();
                textBox.ID = "txtDynamic" + i;
                textBox.Attributes.Add("name", textBox.ID);
                textBox.Attributes.Add("style", "width:100%;");
                textBox.Attributes.Add("type", "number");
                textBox.Attributes.Add("min", "0");
                textBox.Attributes.Add("max", "999");
                textBox.Attributes.Add("value", "0");
                textBox.Attributes.Add("runat", "server");
                //textBox.Text = "6";
                newDiv.Controls.Add(label);
                newDiv.Controls.Add(textBox);
                rowDiv.Controls.Add(newDiv);
            }
            itemPlaceHolder1.Controls.Add(rowDiv);


        }


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

              

        protected void AddSampleInput(object sender,EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            int Sample = Convert.ToInt16(linkButton.CommandArgument);
            Session["Sample"] = Sample;
            myhiddenField.Value = ((GridViewRow)((Control)sender).NamingContainer).RowIndex.ToString();
            System.Web.UI.HtmlControls.HtmlGenericControl rowDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
            rowDiv.Attributes.Add("class", "row form-group");
            for (int i = 1; i <= Sample; i++)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl newDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                newDiv.Attributes.Add("class", "txtboxes col-md-4");
                Label label = new Label();
                label.ID = "lbl" + i;
                label.Text = "Sample" + i;
                TextBox textBox = new TextBox();
                textBox.ID = "txtDynamic" + i;
                textBox.Attributes.Add("name", textBox.ID);
                textBox.Attributes.Add("AutoPostback", "true");
                textBox.Attributes.Add("style", "width:100%;");
                textBox.Attributes.Add("type", "number");
                textBox.Attributes.Add("min", "0");
                textBox.Attributes.Add("max", "999");
                textBox.Attributes.Add("value", "0");
                textBox.Attributes.Add("runat", "server");
                newDiv.Controls.Add(label);
                newDiv.Controls.Add(textBox);
                rowDiv.Controls.Add(newDiv);
            }
            itemPlaceHolder1.Controls.Add(rowDiv);
            MPSampleInput.Show();
        }

        protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpStatus.SelectedValue == "Saved Records")
            {
                GrdWeightScheduleStatus.Visible = false;
                GrdWeightCompletedScheduleStatus.Visible = true;
                BindWeightCompletedScheduleStatusGrd();
                ActionButton.Visible = false;
                SubmitButton.Visible = false;
                ExportButton.Visible = true;
            }
            else
            {
                GrdWeightScheduleStatus.Visible = true;
                GrdWeightCompletedScheduleStatus.Visible = false;
                BindGrdWeightScheduleStatus();
                ActionButton.Visible = true;
                SubmitButton.Visible = true;
                ExportButton.Visible = false;
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
            Response.AddHeader("content-disposition", "attachment;filename = Weight.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);
            GrdWeightCompletedScheduleStatus.AllowPaging = false;
            GrdWeightCompletedScheduleStatus.RenderControl(htmlWrite);
            BindWeightCompletedScheduleStatusGrd();
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        protected void BindGrdWeightScheduleStatus()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR","40"),
                    new SqlParameter("@ScheduleTypeId","4"),
                    new SqlParameter("@ShiftCode",lblshift.Text.Trim())
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdWeightScheduleStatus.DataSource = Dt;

                GrdWeightScheduleStatus.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindGrdWeightScheduleStatus::", ex.Message), ex);
            }
        }

        protected void BindWeightCompletedScheduleStatusGrd()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR","44"),
                    new SqlParameter("@ScheduleTypeId","4"),
                    new SqlParameter("@ShiftCode",lblshift.Text.Trim())
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdWeightCompletedScheduleStatus.DataSource = Dt;

                GrdWeightCompletedScheduleStatus.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindWeightCompletedScheduleStatusGrd::", ex.Message), ex);
            }
        }

        protected void btnProcessParameter_Click(object sender, EventArgs e)
        {
            Response.Redirect("Process.aspx", false);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("QualityCheck.aspx", false);
        }

        protected void btnInProcess_Click(object sender, EventArgs e)
        {
            Response.Redirect("In_Process.aspx", false);
        }

        protected void btnSQC_Click(object sender, EventArgs e)
        {
            Response.Redirect("SQC.aspx", false);
        }

        protected void btnAddParameter_Click(object sender, EventArgs e)
        {
            GetParameter();
            GetSeverity();
            GetUOM();
            GetDuration();
            GetStation();
            QualitativeGroup.Visible = false;
            QuantitativeGroup.Visible = false;
            MPAddParameter.Show();
            if (GrdWeightScheduleStatus != null && GrdWeightScheduleStatus.Rows.Count > 0)
            {
                Session["ScheduleId"] = GrdWeightScheduleStatus.DataKeys[0].Values[1];
            }
        }

        #region Get Parameter
        protected void GetParameter()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "27"),
                    new SqlParameter("@ScheduleId",Convert.ToInt16(Session["ScheduleId"].ToString()))
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpParameter.DataSource = Dt;
                drpParameter.DataTextField = "ParameterName";
                drpParameter.DataValueField = "Id";
                drpParameter.DataBind();
                drpParameter.Items.Insert(0, new ListItem("Select Parameter", "0"));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetParameter::", ex.Message), ex);
            }
        }
        #endregion
        #region Get Severity
        protected void GetSeverity()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "28"),
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpSeverity.DataSource = Dt;
                drpSeverity.DataTextField = "SeverityName";
                drpSeverity.DataValueField = "Id";
                drpSeverity.DataBind();
                drpSeverity.Items.Insert(0, new ListItem("Select Severity", "0"));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetSeverity::", ex.Message), ex);
            }
        }
        #endregion

        #region Get UOM
        protected void GetUOM()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "29"),
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpUOM.DataSource = Dt;
                drpUOM.DataTextField = "UOM";
                drpUOM.DataValueField = "Id";
                drpUOM.DataBind();
                drpUOM.Items.Insert(0, new ListItem("Select UOM", "0"));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetUOM::", ex.Message), ex);
            }
        }
        #endregion

        protected void GetDuration()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "18")
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                ddlHour.DataSource = Dt;
                ddlHour.DataTextField = "Hours";
                ddlHour.DataValueField = "Hours";
                ddlHour.DataBind();
                ddlHour.Items.Insert(0, new ListItem("Select", ""));
                ddlHour.SelectedValue = Convert.ToString(Dt.Rows[0]["CurrentHour"]);

                SqlParameter[] param1 = new SqlParameter[]
                {
                new SqlParameter("@OPR", "19")
                };
                DataTable Dtt = new DataTable();
                Dtt = DBClass.GetDatatable_WithParam(param1, "DM_SP_GET_DATA");
                ddlMinute.DataSource = Dtt;
                ddlMinute.DataTextField = "Minutes";
                ddlMinute.DataValueField = "Minutes";
                ddlMinute.DataBind();
                ddlMinute.Items.Insert(0, new ListItem("Select", ""));
                ddlMinute.SelectedValue = Convert.ToString(Dtt.Rows[0]["CurrentMinute"]);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetDuration::", ex.Message), ex);
            }
        }

        #region Get Station
        protected void GetStation()
        {
            try
            {

                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "7"),
                new SqlParameter("@PlantCode", lblplantcode.Text.Trim())
                };

                DataTable DtStations = new DataTable();
                DtStations = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpStation.DataSource = DtStations;
                drpStation.DataTextField = "StationDescription";
                drpStation.DataValueField = "StationCode";
                drpStation.DataBind();
                drpStation.Items.Insert(0, new ListItem("Select Station", ""));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetStation::", ex.Message), ex);
            }
        }
        #endregion

        protected void OnParameterChange(Object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "39"),
                new SqlParameter("@ParameterId", drpParameter.SelectedValue),
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    if (Convert.ToInt16(Dt.Rows[0]["ParameterTypeId"].ToString()) == 1)
                    {
                        QualitativeGroup.Visible = true;
                        QuantitativeGroup.Visible = false;
                        txtLCL.Text = string.Empty;
                        txtUCL.Text = string.Empty;
                        txtQUOM.Text = string.Empty;
                        MPAddParameter.Show();
                    }
                    else
                    {
                        QualitativeGroup.Visible = false;
                        QuantitativeGroup.Visible = true;
                        txtEResult.Text = string.Empty;
                        MPAddParameter.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetParameterType::", ex.Message), ex);
            }
        }

        protected void btnSkipResult_Click(object sender, EventArgs e)
        {
            Object obj;
            try
            {
                int isChecked = 0;
                for (int i = 0; i < GrdWeightScheduleStatus.Rows.Count; i++)
                {
                    if ((GrdWeightScheduleStatus.Rows[i].Cells[0].FindControl("Select") as CheckBox).Checked)
                    {
                        isChecked++;
                        break;
                    }
                }
                if (isChecked == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select atleast one record')", true);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    for (int i = 0; i < GrdWeightScheduleStatus.Rows.Count; i++)
                    {
                        CheckBox cb = (CheckBox)GrdWeightScheduleStatus.Rows[i].Cells[0].FindControl("Select"); //find the CheckBox
                        if (cb != null)
                        {
                            if (cb.Checked)
                            {
                                parameters.Add(new SqlParameter("OPR", "17"));
                                parameters.Add(new SqlParameter("TestScheduleReportId", GrdWeightScheduleStatus.DataKeys[i].Values[0]));
                                parameters.Add(new SqlParameter("Result", (GrdWeightScheduleStatus.Rows[i].Cells[11].FindControl("lblResult") as TextBox).Text.Trim().ToString()));
                                parameters.Add(new SqlParameter("Status", "0"));
                                parameters.Add(new SqlParameter("@CreatedBy", Convert.ToString(Session["username"])));


                                obj = DBClass.ExecuteNonQuery_WithParam(parameters.ToArray(), "DM_SP_DML_DATA");

                                if (obj != null && Convert.ToInt64(obj) != 0)
                                {
                                    BindGrdWeightScheduleStatus();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Test Result for selected parameters are skipped successfully')", true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("SkipResult:", ex.Message), ex);
            }
        }

        protected void btnSaveResult_Click(object sender, EventArgs e)
        {
            Object obj;
            try
            {
                int isChecked = 0;
                for (int i = 0; i < GrdWeightScheduleStatus.Rows.Count; i++)
                {
                    if ((GrdWeightScheduleStatus.Rows[i].Cells[0].FindControl("Select") as CheckBox).Checked)
                    {
                        isChecked++;
                        break;
                    }
                }
                if (isChecked == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select atleast one record')", true);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    for (int i = 0; i < GrdWeightScheduleStatus.Rows.Count; i++)
                    {
                        CheckBox cb = (CheckBox)GrdWeightScheduleStatus.Rows[i].Cells[0].FindControl("Select"); //find the CheckBox
                        if (cb != null)
                        {
                            if (cb.Checked)
                            {
                                if (string.IsNullOrEmpty((GrdWeightScheduleStatus.Rows[i].Cells[11].FindControl("lblResult") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Result is missing')", true);
                                }
                                else
                                {
                                    parameters.Add(new SqlParameter("OPR", "17"));
                                    parameters.Add(new SqlParameter("TestScheduleReportId", GrdWeightScheduleStatus.DataKeys[i].Values[0]));
                                    parameters.Add(new SqlParameter("Result", (GrdWeightScheduleStatus.Rows[i].Cells[11].FindControl("lblResult") as TextBox).Text.Trim().ToString()));
                                    parameters.Add(new SqlParameter("Status", "1"));
                                    parameters.Add(new SqlParameter("@CreatedBy", Convert.ToString(Session["username"])));


                                    obj = DBClass.ExecuteNonQuery_WithParam(parameters.ToArray(), "DM_SP_DML_DATA");

                                    if (obj != null && Convert.ToInt64(obj) != 0)
                                    {
                                        BindGrdWeightScheduleStatus();
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Test Result for selected parameters are saved successfully')", true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("SaveResult:", ex.Message), ex);
            }
        }

        protected void btnPreviousResult_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddParam_Click(object sender, EventArgs e)
        {
            try
            {
                object obj;
                lblStartMessage.Text = string.Empty;
                if (drpParameter.SelectedValue == "0")
                {
                    lblStartMessage.Text = "Please Select Parameter !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (drpStation.SelectedValue == "")
                {
                    lblStartMessage.Text = "Please Select Station !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (drpSeverity.SelectedValue == "0")
                {
                    lblStartMessage.Text = "Please Select Severity !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (string.IsNullOrEmpty(txtNoOfSamples.Text))
                {
                    lblStartMessage.Text = "Please enter No.of Sample !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (drpUOM.SelectedValue == "0")
                {
                    lblStartMessage.Text = "Please Select UOM !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (string.IsNullOrEmpty(txtEResult.Text) && (string.IsNullOrEmpty(txtLCL.Text) && string.IsNullOrEmpty(txtUCL.Text) && string.IsNullOrEmpty(txtQUOM.Text)))
                {
                    lblStartMessage.Text = "Please enter either qualitative or quantitative parameter !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }

                else
                {
                    TimeSpan time = new TimeSpan(Int16.Parse(ddlHour.SelectedValue), Int16.Parse(ddlMinute.SelectedValue), 0);
                    if (Session["ScheduleId"] == null && Convert.ToInt16(Session["ScheduleId"]) <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please activate schedule')", true);
                    }
                    else
                    {
                        List<SqlParameter> saveSchedule = new List<SqlParameter>
                            {
                                new SqlParameter("@OPR", "14"),
                                new SqlParameter("@ScheduleId",Int32.Parse(Session["ScheduleId"].ToString())),
                                new SqlParameter("@ParameterId",drpParameter.SelectedValue),
                                new SqlParameter("@StationCode",drpStation.SelectedValue),
                                new SqlParameter("@SeverityId",drpSeverity.SelectedValue),
                                new SqlParameter("@NoOfSamplesId",txtNoOfSamples.Text.Trim()),
                                new SqlParameter("@UOMId",drpUOM.SelectedValue),
                                new SqlParameter("@ShiftCode",lblshift.Text.Trim()),
                                new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                            };
                        SqlParameter repeatParameter = new SqlParameter("@Repeat", SqlDbType.Time);
                        repeatParameter.Value = time;
                        saveSchedule.Add(repeatParameter);
                        if (!string.IsNullOrEmpty(txtEResult.Text))
                            saveSchedule.Add(new SqlParameter("@EResult", txtEResult.Text));
                        else if (!string.IsNullOrEmpty(txtLCL.Text) && !string.IsNullOrEmpty(txtUCL.Text) && !string.IsNullOrEmpty(txtQUOM.Text))
                        {
                            saveSchedule.Add(new SqlParameter("@LCL", txtLCL.Text));
                            saveSchedule.Add(new SqlParameter("@UCL", txtUCL.Text));
                            saveSchedule.Add(new SqlParameter("@UOM", txtQUOM.Text));
                        }

                        obj = DBClass.ExecuteNonQuery_WithParam(saveSchedule.ToArray(), "DM_SP_DML_DATA");

                        if (obj != null && Convert.ToInt64(obj) != 0)
                        {
                            BindGrdWeightScheduleStatus();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter added successfully')", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("btnSubmit_Click::", ex.Message), ex);
            }
        }

        protected void btnClearParam_Click(object sender, EventArgs e)
        {
            drpParameter.SelectedIndex = 0;
            drpStation.SelectedIndex = 0;
            drpSeverity.SelectedIndex = 0;
            txtNoOfSamples.Text = string.Empty;
            drpUOM.SelectedIndex = 0;
            ddlHour.SelectedIndex = 0;
            ddlMinute.SelectedIndex = 0;
            txtLCL.Text = string.Empty;
            txtUCL.Text = string.Empty;
            txtQUOM.Text = string.Empty;
            txtEResult.Text = string.Empty;
            lblStartMessage.Text = String.Empty;
        }
    }
}