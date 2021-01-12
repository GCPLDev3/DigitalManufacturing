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
using static System.Net.Mime.MediaTypeNames;

namespace MFG_DigitalApp
{
    public partial class Process : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("Process");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExport);
            if (!IsPostBack)
            {
                Session["ScheduleId"] = null;
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
                    else
                    {
                        lblshiftnew.Text = model.ShiftCode;
                    }
                    lblshift.Text = model.ShiftCode;
                    //lbldate.Text = model.ShiftDate;
                    if (lblshift.Text.Equals("S3"))
                    {
                        string dateTime = DateTime.ParseExact(model.ShiftDate.Substring(0, model.ShiftDate.IndexOf(" ")).Replace("-", "/"), "dd/MM/yyyy", null).AddDays(-1).ToString("yyyy-MM-dd");                     
                       /// DateTime shiftDay = Convert.ToDateTime(model.ShiftDate);
                        lbldate.Text = dateTime;
                    }
                    else
                    {
                        string dateTime = DateTime.ParseExact(model.ShiftDate.Substring(0, model.ShiftDate.IndexOf(" ")).Replace("-", "/"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                       // DateTime shiftDay = Convert.ToDateTime(model.ShiftDate);
                        lbldate.Text = dateTime;
                    }
                    drpStatus_SelectedIndexChanged(null, null);
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
        }

        protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpStatus.SelectedValue == "Saved Records")
            {
                GrdProcessScheduleStatus.Visible = false;
                GrdProcessCompletedScheduleStatus.Visible = true;
                BindGrdProcessCompletedScheduleStatus();
                ActionButton.Visible = false;
                btnSaveResult.Visible = false;
                btnSaveResult1.Visible = false;
                ResubmitButton.Visible = true;
                ExportButton.Visible = true;
            }
            else
            {
                GrdProcessScheduleStatus.Visible = true;
                GrdProcessCompletedScheduleStatus.Visible = false;
                BindGrdProcessScheduleStatus();
                ActionButton.Visible = true;
                btnSaveResult.Visible = true;
                btnSaveResult1.Visible = true;
                ResubmitButton.Visible = false;
                ExportButton.Visible = false;
            }
            GetGrammage();
        }
        protected void GetGrammage()
        {
            if (GrdProcessScheduleStatus != null && GrdProcessScheduleStatus.Rows.Count > 0)
            {
                SqlParameter[] parame = new SqlParameter[]
                 {
                    new SqlParameter("@OPR","73"),
                    new SqlParameter("@ScheduleId",GrdProcessScheduleStatus.DataKeys[0].Values[1])

                 };
                DataTable data = new DataTable();
                data = DBClass.GetDatatable_WithParam(parame, "DM_SP_GET_DATA");
                foreach (DataRow dataRow in data.Rows)
                {
                    Session["ProcessScheduleId"] = dataRow["Grammage"];
                }
            }
            if (GrdProcessCompletedScheduleStatus != null && GrdProcessCompletedScheduleStatus.Rows.Count > 0)
            {
                SqlParameter[] parame = new SqlParameter[]
                 {
                    new SqlParameter("@OPR","73"),
                     new SqlParameter("@ScheduleId",GrdProcessCompletedScheduleStatus.DataKeys[0].Values[1])
                 };
                DataTable data = new DataTable();
                data = DBClass.GetDatatable_WithParam(parame, "DM_SP_GET_DATA");
                foreach (DataRow dataRow in data.Rows)
                {
                    Session["ProcessScheduleId"] = dataRow["Grammage"];
                }
            }
            if (Session["ProcessScheduleId"] != null && !String.IsNullOrEmpty(Session["ProcessScheduleId"].ToString()))
            {
                txtGrammage.Text = "Grammage=" + Session["ProcessScheduleId"];
            }
            else
            {
                txtGrammage.Text = "Grammage=" + "0";
            }
        }

        protected void BindGrdProcessCompletedScheduleStatus(bool filter = false)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    //new SqlParameter("@OPR","44"),
                    new SqlParameter("@OPR","71"),
                    new SqlParameter("@ScheduleTypeId","1"),
                    new SqlParameter("@ShiftCode",lblshift.Text.Trim()),
                    new SqlParameter("@PlantCode",lblplantcode.Text),
                    new SqlParameter("@Line",lblline.Text)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdProcessCompletedScheduleStatus.DataSource = Dt;
                GrdProcessCompletedScheduleStatus.DataBind();
                foreach (GridViewRow row in GrdProcessCompletedScheduleStatus.Rows)
                {
                    (GrdProcessCompletedScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("Select") as CheckBox).Checked = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindGrdProcessCompletedScheduleStatus::", ex.Message), ex);
            }
        }

        protected void BindGrdProcessScheduleStatus()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    //new SqlParameter("@OPR","40"),
                    new SqlParameter("@OPR","70"),
                    new SqlParameter("@ScheduleTypeId","1"),
                    new SqlParameter("@ShiftCode",lblshift.Text.Trim()),
                    new SqlParameter("@PlantCode",lblplantcode.Text),
                    new SqlParameter("@Line",lblline.Text)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                //Dt.Columns.Add("DueTime", typeof(System.String));
                //for (int i = 0; i < Dt.Rows.Count; i++)
                //{
                //    DateTime createdon = Convert.ToDateTime(Dt.Rows[i]["CreatedOn"].ToString());
                //    string Repeat = Convert.ToDateTime(Dt.Rows[i]["Repeat"].ToString()).ToString("HH:mm");
                //    TimeSpan repeatTime = TimeSpan.Parse(Repeat);
                //    DateTime date = createdon.AddDays(repeatTime.Days).AddHours(repeatTime.Hours).AddMinutes(repeatTime.Minutes);
                //    //TimeSpan newTime = TimeSpan.Parse(createdon).Add(TimeSpan.Parse(Repeat));
                //    Dt.Rows[i]["DueTime"] = date.ToShortTimeString();
                //}
                GrdProcessScheduleStatus.DataSource = Dt;
                GrdProcessScheduleStatus.DataBind();
              
                //foreach (GridViewRow row in GrdProcessScheduleStatus.Rows)
                //{
                //    var ImageName = (GrdProcessScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("Image_Status") as System.Web.UI.WebControls.Image).ImageUrl;
                //    if (!string.IsNullOrEmpty(ImageName))
                //    {
                //        (GrdProcessScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("Select") as CheckBox).Visible = false;
                //        (GrdProcessScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("Result") as TextBox).ReadOnly = true;
                //        (GrdProcessScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("ExpectedResult") as TextBox).ReadOnly = true;

                //    }
                //}

            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindGrdInprocessScheduleStatus::", ex.Message), ex);
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
            Response.AddHeader("content-disposition", "attachment;filename = Process.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);
            GrdProcessCompletedScheduleStatus.AllowPaging = false;
            BindGrdProcessCompletedScheduleStatus();
            foreach (GridViewRow row in GrdProcessCompletedScheduleStatus.Rows)
            {
                GrdProcessCompletedScheduleStatus.Columns[0].Visible = false;
            }
            GrdProcessCompletedScheduleStatus.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Process.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                //To Export all pages
                GrdProcessCompletedScheduleStatus.AllowPaging = false;

                GrdProcessCompletedScheduleStatus.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GrdProcessCompletedScheduleStatus.HeaderRow.Cells)
                {
                    cell.BackColor = GrdProcessCompletedScheduleStatus.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GrdProcessCompletedScheduleStatus.Rows)
                {
                    GrdProcessCompletedScheduleStatus.Columns[0].Visible = false;
                    GrdProcessCompletedScheduleStatus.Columns[13].Visible = false;
                    GrdProcessCompletedScheduleStatus.Columns[14].Visible = true;
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GrdProcessCompletedScheduleStatus.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GrdProcessCompletedScheduleStatus.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                        List<Control> controls = new List<Control>();
                        //Add controls to be removed to Generic List
                        foreach (Control control in cell.Controls)
                        {
                            controls.Add(control);
                        }
                        //Loop through the controls to be removed and replace then with Literal
                        foreach (Control control in controls)
                        {
                            switch (control.GetType().Name)
                            {
                                case "HyperLink":
                                    cell.Controls.Add(new Literal { Text = (control as HyperLink).Text });
                                    break;
                                case "TextBox":
                                    cell.Controls.Add(new Literal { Text = (control as TextBox).Text });
                                    break;
                                case "LinkButton":
                                    cell.Controls.Add(new Literal { Text = (control as LinkButton).Text });
                                    break;
                                case "CheckBox":
                                    cell.Controls.Add(new Literal { Text = (control as CheckBox).Text });
                                    break;
                                case "RadioButton":
                                    cell.Controls.Add(new Literal { Text = (control as RadioButton).Text });
                                    break;
                                case "Label":
                                    cell.Controls.Add(new Literal { Text = (control as Label).Text });
                                    break;
                            }
                            cell.Controls.Remove(control);
                        }
                    }
                }

                GrdProcessCompletedScheduleStatus.RenderControl(hw);
                Response.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("QualityCheck.aspx", false);
        }

        protected void btnInProcessParameter_Click(object sender, EventArgs e)
        {
            Response.Redirect("In_Process.aspx", false);
        }

        protected void btnWeight_Click(object sender, EventArgs e)
        {
            Response.Redirect("Weight.aspx", false);
        }

        protected void btnSQC_Click(object sender, EventArgs e)
        {
            Response.Redirect("SQC.aspx", false);
        }
        protected void Test(object sender, EventArgs e)
        {
            Response.Redirect("SQC.aspx", false);
        }

        protected void btnAddParameter_Click(object sender, EventArgs e)
        {
            if (GrdProcessScheduleStatus != null && GrdProcessScheduleStatus.Rows.Count > 0)
            {
                Session["ScheduleId"] = GrdProcessScheduleStatus.DataKeys[0].Values[1];
            }

            GetParameter();
            GetSeverity();
            GetUOM();
            GetDuration();
            GetStation();
            QualitativeGroup.Visible = false;
            QuantitativeGroup.Visible = false;
            txtLCL.Text = string.Empty;
            txtNoOfSamples.Text = string.Empty;
            txtUCL.Text = string.Empty;

            MPAddParameter.Show();
            lblStartMessage.Text = string.Empty;
            lblStartMessage.ForeColor = Color.White;
        }

        #region Get Parameter
        protected void GetParameter()
        {
            try
            {
                //  SqlParameter[] ScheduleTypeId = new SqlParameter[]
                //{
                //  new SqlParameter("@ScheduleTypeId","1"),

                //};
                //  DataTable dataTable = new DataTable();
                //  dataTable = DBClass.GetDatatable_WithParam(ScheduleTypeId, "DM_SP_GET_ScheduleId");
                //  Session["ScheduleId"] = dataTable.Rows[0]["ScheduleId"];
                //  var ScheduleId = Session["ScheduleId"];
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "27"),
                //new SqlParameter("@ScheduleId",ScheduleId)
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
        protected void GetQUOM()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "29"),
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpQUOM.DataSource = Dt;
                drpQUOM.DataTextField = "UOM";
                drpQUOM.DataValueField = "Id";
                drpQUOM.DataBind();
                drpQUOM.Items.Insert(0, new ListItem("Select UOM", "0"));
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
                //ddlHour.DataSource = Dt;
                //ddlHour.DataTextField = "Hours";
                //ddlHour.DataValueField = "Hours";
                //ddlHour.DataBind();
                //ddlHour.Items.Insert(0, new ListItem("Select", ""));
                //ddlHour.SelectedValue = Convert.ToString(Dt.Rows[0]["CurrentHour"]);

                SqlParameter[] param1 = new SqlParameter[]
                {
                new SqlParameter("@OPR", "19")
                };
                DataTable Dtt = new DataTable();
                Dtt = DBClass.GetDatatable_WithParam(param1, "DM_SP_GET_DATA");
                //ddlMinute.DataSource = Dtt;
                //ddlMinute.DataTextField = "Minutes";
                //ddlMinute.DataValueField = "Minutes";
                //ddlMinute.DataBind();
                //ddlMinute.Items.Insert(0, new ListItem("Select", ""));
                //ddlMinute.SelectedValue = Convert.ToString(Dtt.Rows[0]["CurrentMinute"]);
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
                new SqlParameter("@PlantCode", lblplantcode.Text.Trim()),
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

        protected void OnFieldChange(Object sender, EventArgs e)
        {
            try
            {
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
                else if (QuantitativeGroup.Visible)
                {
                    if (string.IsNullOrEmpty(txtLCL.Text))
                    {
                        lblStartMessage.Text = "Please enter LCL value !";
                        lblStartMessage.ForeColor = Color.Red;
                        // MPAddParameter.Show();
                    }
                    else if (string.IsNullOrEmpty(txtUCL.Text))
                    {
                        lblStartMessage.Text = "Please enter UCL value !";
                        lblStartMessage.ForeColor = Color.Red;
                        //MPAddParameter.Show();
                    }
                    else if (drpQUOM.SelectedIndex == 0)
                    {
                        lblStartMessage.Text = "Please enter QUOM value !";
                        lblStartMessage.ForeColor = Color.Red;

                    }
                    else if (Convert.ToDecimal(txtUCL.Text.ToString()) <= Convert.ToDecimal(txtLCL.Text.ToString()))
                    {
                        lblStartMessage.Text = "LCL should be lesser than UCL!";
                        lblStartMessage.ForeColor = Color.Red;
                        // MPAddParameter.Show();
                    }
                    MPAddParameter.Show();
                }
                else if (QualitativeGroup.Visible)
                {
                    if (string.IsNullOrEmpty(txtEResult.Text))
                    {
                        lblStartMessage.Text = "Please enter Expected Result !";
                        lblStartMessage.ForeColor = Color.Red;
                    }
                    MPAddParameter.Show();
                }
                //else if (string.IsNullOrEmpty(txtEResult.Text) && (string.IsNullOrEmpty(txtLCL.Text) && string.IsNullOrEmpty(txtUCL.Text) && string.IsNullOrEmpty(drpQUOM.Text)))
                //{
                //    if (QuantitativeGroup.Visible)
                //    {
                //        lblStartMessage.Text = "Please enter either qualitative or quantitative parameter !";
                //        lblStartMessage.ForeColor = Color.Red;
                //    }
                //    MPAddParameter.Show();
                //}
                else
                {
                    MPAddParameter.Show();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("btnSubmit_Click::", ex.Message), ex);
            }
        }

        protected void OnParameterChange(Object sender, EventArgs e)
        {
            try
            {
                OnFieldChange(sender, e);
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
                        drpQUOM.Text = string.Empty;
                        MPAddParameter.Show();
                    }
                    else
                    {
                        QualitativeGroup.Visible = false;
                        QuantitativeGroup.Visible = true;
                        txtEResult.Text = string.Empty;
                        GetQUOM();
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
            Object obj = new Object();
            try
            {
                int isChecked = 0;
                for (int i = 0; i < GrdProcessScheduleStatus.Rows.Count; i++)
                {
                    if ((GrdProcessScheduleStatus.Rows[i].Cells[0].FindControl("Select") as CheckBox).Checked)
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
                    for (int i = 0; i < GrdProcessScheduleStatus.Rows.Count; i++)
                    {
                        CheckBox cb = (CheckBox)GrdProcessScheduleStatus.Rows[i].Cells[0].FindControl("Select"); //find the CheckBox
                        if (cb != null)
                        {
                            if (cb.Checked)
                            {
                                parameters.Add(new SqlParameter("OPR", "17"));
                                parameters.Add(new SqlParameter("TestScheduleReportId", GrdProcessScheduleStatus.DataKeys[i].Values[0]));
                                parameters.Add(new SqlParameter("Result", (GrdProcessScheduleStatus.Rows[i].Cells[11].FindControl("Result") as TextBox).Text.Trim()));
                                parameters.Add(new SqlParameter("Status", "0"));
                                parameters.Add(new SqlParameter("@CreatedBy", Convert.ToString(Session["username"])));
                                parameters.Add(new SqlParameter("@ExpectedResult", (GrdProcessScheduleStatus.Rows[i].Cells[12].FindControl("ExpectedResult") as TextBox).Text.Trim()));
                                obj = DBClass.ExecuteNonQuery_WithParam(parameters.ToArray(), "DM_SP_DML_DATA");
                                parameters.Clear();
                            }
                        }
                    }
                    if (obj != null && Convert.ToInt64(obj) != 0)
                    {
                        BindGrdProcessScheduleStatus();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Test Result for selected parameters are skipped successfully')", true);
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

            Object obj = new Object();
            try
            {
                int isChecked = 0;
                int isResult = 0;
                int isExpectedResult = 0;
                for (int i = 0; i < GrdProcessScheduleStatus.Rows.Count; i++)
                {
                    var status = (GrdProcessScheduleStatus.Rows[i].Cells[12].FindControl("Image_Status") as System.Web.UI.WebControls.Image).ImageUrl;
                    if ((GrdProcessScheduleStatus.Rows[i].Cells[0].FindControl("Select") as CheckBox).Checked)
                    {
                        isChecked++;
                        // break;
                    }
                    if (string.IsNullOrEmpty(status))
                    {
                        if (!string.IsNullOrEmpty((GrdProcessScheduleStatus.Rows[i].Cells[0].FindControl("Result") as TextBox).Text))
                        {
                            isResult++;
                            //break;
                        }
                        if (!string.IsNullOrEmpty((GrdProcessScheduleStatus.Rows[i].Cells[0].FindControl("ExpectedResult") as TextBox).Text))
                        {
                            isExpectedResult++;
                            //break;
                        }
                    }

                }
                if (isChecked == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select atleast one record')", true);
                }
                else if (isResult == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Result')", true);
                }
                else if (isExpectedResult == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Expected result')", true);
                }
                else
                {

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    for (int i = 0; i < GrdProcessScheduleStatus.Rows.Count; i++)
                    {
                        CheckBox cb = (CheckBox)GrdProcessScheduleStatus.Rows[i].Cells[0].FindControl("Select"); //find the CheckBox
                        if (cb != null)
                        {
                            if (cb.Checked)
                            {
                                var result = (GrdProcessScheduleStatus.Rows[i].Cells[9].FindControl("Result") as TextBox).Text.Trim().ToString();
                                if (string.IsNullOrEmpty((GrdProcessScheduleStatus.Rows[i].Cells[9].FindControl("Result") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Result')", true);
                                }
                                else if (string.IsNullOrEmpty((GrdProcessScheduleStatus.Rows[i].Cells[10].FindControl("ExpectedResult") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Expected result')", true);
                                }
                                //else if (!(result.Contains(".")))
                                //{
                                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Result should be decimal value')", true);
                                //}
                                else
                                {
                                    parameters.Add(new SqlParameter("OPR", "17"));
                                    parameters.Add(new SqlParameter("@TestScheduleReportId", GrdProcessScheduleStatus.DataKeys[i].Values[0]));
                                    parameters.Add(new SqlParameter("@Result", (GrdProcessScheduleStatus.Rows[i].Cells[11].FindControl("Result") as TextBox).Text.Trim()));
                                    parameters.Add(new SqlParameter("@ExpectedResult", (GrdProcessScheduleStatus.Rows[i].Cells[12].FindControl("ExpectedResult") as TextBox).Text.Trim()));
                                    parameters.Add(new SqlParameter("@Status", "1"));
                                    parameters.Add(new SqlParameter("@CreatedBy", Convert.ToString(Session["username"])));
                                    obj = DBClass.ExecuteNonQuery_WithParam(parameters.ToArray(), "DM_SP_DML_DATA");
                                    parameters.Clear();
                                }
                            }
                        }
                    }
                    if (obj != null && Convert.ToInt64(obj) != 0)
                    {
                        BindGrdProcessScheduleStatus();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Test Result for selected parameters are saved successfully')", true);
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
                object obj = null;
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
                else if (drpUOM.SelectedValue == "0")
                {
                    lblStartMessage.Text = "Please Select UOM !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (QuantitativeGroup.Visible && string.IsNullOrEmpty(txtLCL.Text))
                {

                    lblStartMessage.Text = "Please enter LCL value !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (QuantitativeGroup.Visible && string.IsNullOrEmpty(txtUCL.Text))
                {
                    lblStartMessage.Text = "Please enter UCL value !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (QuantitativeGroup.Visible && drpQUOM.SelectedIndex == 0)
                {
                    lblStartMessage.Text = "Please enter QUOM value !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (QuantitativeGroup.Visible && Convert.ToDecimal(txtUCL.Text.ToString()) <= Convert.ToDecimal(txtLCL.Text.ToString()))
                {
                    lblStartMessage.Text = "LCL should be lesser than UCL!";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else if (QualitativeGroup.Visible && string.IsNullOrEmpty(txtEResult.Text))
                {
                    lblStartMessage.Text = "Please enter Expected Result !";
                    lblStartMessage.ForeColor = Color.Red;
                    MPAddParameter.Show();
                }
                else
                {
                    var x = Session["ScheduleId"];
                    TimeSpan time = new TimeSpan(Int16.Parse("00"), Int16.Parse("00"), 0);
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
                                new SqlParameter("@CreatedBy",Convert.ToString(Session["username"])),
                                new SqlParameter("@PlantCode",lblplantcode.Text),
                                new SqlParameter("@Line",lblline.Text),
                                new SqlParameter("@OnTimeParameterActive","1")

                            };
                        SqlParameter repeatParameter = new SqlParameter("@Repeat", SqlDbType.Time);
                        repeatParameter.Value = time;
                        saveSchedule.Add(repeatParameter);
                        if (!string.IsNullOrEmpty(txtEResult.Text))
                            saveSchedule.Add(new SqlParameter("@EResult", txtEResult.Text));
                        else if (!string.IsNullOrEmpty(txtLCL.Text) && !string.IsNullOrEmpty(txtUCL.Text) && !string.IsNullOrEmpty(drpQUOM.Text))
                        {
                            saveSchedule.Add(new SqlParameter("@LCL", txtLCL.Text));
                            saveSchedule.Add(new SqlParameter("@UCL", txtUCL.Text));
                            saveSchedule.Add(new SqlParameter("@UOM", drpQUOM.Text));
                        }
                        DataTable Dta = new DataTable();
                        //var x = Int32.Parse(Session["ScheduleId"].ToString());
                        SqlParameter[] param = new SqlParameter[]
                       {
                        new SqlParameter("@OPR", "59"),
                        new SqlParameter("@ScheduleId",Int32.Parse(Session["ScheduleId"].ToString())),
                        new SqlParameter("@ParameterId",drpParameter.SelectedValue)
                       };
                        object IsExist = DBClass.ExecuteNonQuery_WithParam(param.ToArray(), "DM_SP_GET_DATA");
                        if (Convert.ToInt64(IsExist) == -1)
                        {
                            
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter alredy Exsist')", true);
                        }
                        else { 
                        Dta = DBClass.GetDatatable_WithParam(saveSchedule.ToArray(), "DM_SP_DML_DATA");
                        if (Dta != null)
                        {
                            obj = Dta.Rows[0]["value"];
                        }
                        if (obj != null && Convert.ToInt64(obj) >= 0)
                        {
                            SqlParameter[] paramEter = new SqlParameter[]
                               {
                                new SqlParameter("@OPR", "48"),
                                new SqlParameter("@ScheduleTypeId", 1),
                                new SqlParameter("@ParameterUniqueId",Convert.ToInt64(obj))
                               };
                            var val = Convert.ToInt64(obj);
                            DataTable Dt = new DataTable();
                            Dt = DBClass.GetDatatable_WithParam(paramEter, "DM_SP_GET_DATA");
                            if (Dt != null && Dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < Dt.Rows.Count; i++)
                                {
                                    List<SqlParameter> saveParam = new List<SqlParameter>
                                    {
                                        new SqlParameter("@OPR", "21"),
                                        new SqlParameter("@ScheduleId",Int32.Parse(Dt.Rows[i]["ScheduleId"].ToString())),
                                        new SqlParameter("@ParameterId",Dt.Rows[i]["ParameterId"].ToString()),
                                        new SqlParameter("@QcScheduleId",Dt.Rows[i]["QC_TestingSchedule_Detail_Id"].ToString()),
                                        new SqlParameter("@ParameterUniqueId",Dt.Rows[i]["ParameterUniqueId"].ToString()),
                                        //new SqlParameter("@DueTime","00:00:00"),
                                        new SqlParameter("@DueTime",DateTime.Now.ToString()),
                                        new SqlParameter("@ScheduleTypeId",Dt.Rows[i]["ScheduleTypeId"].ToString())
                                    };
                                    obj = DBClass.ExecuteNonQuery_WithParam(saveParam.ToArray(), "DM_SP_DML_DATA");
                                }
                            }
                            BindGrdProcessScheduleStatus();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter added successfully')", true);
                        }
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
            GetParameter();
            drpParameter.SelectedIndex = 0;
            lblStartMessage.Text = string.Empty;
            lblStartMessage.ForeColor = Color.White;
            drpStation.SelectedIndex = 0;
            drpSeverity.SelectedIndex = 0;
            txtNoOfSamples.Text = string.Empty;
            drpUOM.SelectedIndex = 0;
            //ddlHour.SelectedIndex = 0;
            //ddlMinute.SelectedIndex = 0;
            txtLCL.Text = string.Empty;
            txtUCL.Text = string.Empty;
            drpQUOM.Text = string.Empty;
            txtEResult.Text = string.Empty;
            lblStartMessage.Text = String.Empty;
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
            drpStatus_SelectedIndexChanged(null, null);
        }
        protected void btnEditResults_Click(object sender, EventArgs e)
        {
            Object obj = new Object();
            try
            {
                int isChecked = 0;
                int isResult = 0;
                int isExpectedResult = 0;
                for (int i = 0; i < GrdProcessCompletedScheduleStatus.Rows.Count; i++)
                {
                    //var status = (GrdProcessCompletedScheduleStatus.Rows[i].Cells[i].FindControl("Image_Status") as System.Web.UI.WebControls.Image).ImageUrl;
                    if ((GrdProcessCompletedScheduleStatus.Rows[i].Cells[0].FindControl("Select") as CheckBox).Checked)
                    {
                        isChecked++;
                        // break;
                    }
                    //if (string.IsNullOrEmpty(status))
                    //{
                    if (!string.IsNullOrEmpty((GrdProcessCompletedScheduleStatus.Rows[i].Cells[0].FindControl("Result") as TextBox).Text))
                    {
                        isResult++;
                        //break;
                    }
                    if (!string.IsNullOrEmpty((GrdProcessCompletedScheduleStatus.Rows[i].Cells[0].FindControl("ExpectedResult") as TextBox).Text))
                    {
                        isExpectedResult++;
                        //break;
                    }
                    //}

                }
                if (isChecked == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select atleast one record')", true);
                }
                else if (isResult == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Result')", true);
                }
                else if (isExpectedResult == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Expected result')", true);
                }
                else
                {

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    for (int i = 0; i < GrdProcessCompletedScheduleStatus.Rows.Count; i++)
                    {
                        CheckBox cb = (CheckBox)GrdProcessCompletedScheduleStatus.Rows[i].Cells[0].FindControl("Select"); //find the CheckBox
                        if (cb != null)
                        {
                            if (cb.Checked)
                            {
                                var result = (GrdProcessCompletedScheduleStatus.Rows[i].Cells[9].FindControl("Result") as TextBox).Text.Trim().ToString();
                                if (string.IsNullOrEmpty((GrdProcessCompletedScheduleStatus.Rows[i].Cells[9].FindControl("Result") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Result')", true);
                                }
                                else if (string.IsNullOrEmpty((GrdProcessCompletedScheduleStatus.Rows[i].Cells[10].FindControl("ExpectedResult") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Expected result')", true);
                                }
                                //else if (!(result.Contains(".")))
                                //{
                                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Result should be decimal ponit')", true);
                                //}
                                else
                                {
                                    parameters.Add(new SqlParameter("@OPR", "17"));
                                    parameters.Add(new SqlParameter("@TestScheduleReportId", GrdProcessCompletedScheduleStatus.DataKeys[i].Values[0]));
                                    parameters.Add(new SqlParameter("@Result", (GrdProcessCompletedScheduleStatus.Rows[i].Cells[11].FindControl("Result") as TextBox).Text.Trim()));
                                    parameters.Add(new SqlParameter("@ExpectedResult", (GrdProcessCompletedScheduleStatus.Rows[i].Cells[12].FindControl("ExpectedResult") as TextBox).Text.Trim()));
                                    parameters.Add(new SqlParameter("@Status", "1"));
                                    parameters.Add(new SqlParameter("@CreatedBy", Convert.ToString(Session["username"])));
                                    obj = DBClass.ExecuteNonQuery_WithParam(parameters.ToArray(), "DM_SP_DML_DATA");
                                    parameters.Clear();
                                }
                            }
                        }
                    }
                    if (obj != null && Convert.ToInt64(obj) != 0)
                    {
                        BindGrdProcessCompletedScheduleStatus();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Test Result for selected parameters are saved successfully')", true);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("SaveResult:", ex.Message), ex);
            }
        }
        protected void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtScheduleFromDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select the From date')", true);
                }
                else if (string.IsNullOrEmpty(txtScheduleToDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please enter the To date')", true);
                }
                else
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                    new SqlParameter("@OPR", "52"),
                    new SqlParameter("@ScheduleTypeId","1"),
                    new SqlParameter("@ShiftCode",lblshift.Text.Trim()),
                    new SqlParameter("@PlantCode",lblplantcode.Text),
                    new SqlParameter("@Line",lblline.Text),
                    new SqlParameter("@FromDate", txtScheduleFromDate.Text),
                    new SqlParameter("@ToDate", txtScheduleToDate.Text)
                    };

                    DataTable Dt = new DataTable();
                    Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                    GrdProcessCompletedScheduleStatus.DataSource = Dt;
                    GrdProcessCompletedScheduleStatus.DataBind();
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindCompletedScheduleStatusGrid::", ex.Message), ex);
            }
        }

    }
}