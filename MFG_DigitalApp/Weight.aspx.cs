using MFG_DigitalApp.Log;
using MFG_DigitalApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MFG_DigitalApp
{
    public partial class Weight : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("Weight");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        private static List<String> SampleInputs = new List<String>();
        private static bool gridValue=false;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            List<string> keys = Request.Form.AllKeys.Where(key => key != null && key.Contains("txtDynamic")).ToList();
            int i = 1;
            foreach (string key in keys)
            {
                //this.CreateTextBox();
                i++;
            }
        }



        private void CreateTextBox()
        {
            int Sample = Convert.ToInt16(Session["Sample"]);
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
                        //DateTime shiftDay = Convert.ToDateTime(model.ShiftDate);
                        lbldate.Text = dateTime;
                    }
                    else
                    {
                        string dateTime = DateTime.ParseExact(model.ShiftDate.Substring(0, model.ShiftDate.IndexOf(" ")).Replace("-", "/"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                        //DateTime shiftDay = Convert.ToDateTime(model.ShiftDate);
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



        protected void AddSampleInput(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            int Sample = Convert.ToInt16(linkButton.CommandArgument);
            Session["Sample"] = Sample;
            myhiddenField.Value = ((GridViewRow)((Control)sender).NamingContainer).RowIndex.ToString();
            System.Web.UI.HtmlControls.HtmlGenericControl rowDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
            rowDiv.Attributes.Add("class", "row form-group");
            if (drpStatus.SelectedValue == "Saved Records")
            {
                if (gridValue)
                {
                    int sampleId = Convert.ToInt32(myhiddenField.Value);
                    SampleInputs = new List<string>();
                    HiddenField sample = GrdWeightCompletedScheduleStatus.Rows[sampleId].FindControl("HiddenField1") as HiddenField;
                    if (!string.IsNullOrEmpty(sample.Value))
                    {
                        SampleInputs = JsonConvert.DeserializeObject<List<string>>(sample.Value);
                    }
                }
            }
            else
            {
                int sampleId = Convert.ToInt32(myhiddenField.Value);
                SampleInputs = new List<string>();
                var result= (GrdWeightScheduleStatus.Rows[sampleId].FindControl("lblResult") as TextBox).Text;
                HiddenField sample = GrdWeightScheduleStatus.Rows[sampleId].FindControl("HiddenField1") as HiddenField;
                if (!string.IsNullOrEmpty(sample.Value)&& !string.IsNullOrEmpty(result))
                {
                    SampleInputs = JsonConvert.DeserializeObject<List<string>>(sample.Value);
                }
            }
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
                textBox.Attributes.Add("type", "decimal");
                textBox.Attributes.Add("min", "0");
                textBox.Attributes.Add("max", "999");
                textBox.Attributes.Add("value", "0");
                textBox.Attributes.Add("runat", "server");
                textBox.Attributes.Add("onkeypress", "return isNumeric(event,this)");
                if (drpStatus.SelectedValue == "Saved Records"&&gridValue)
                {
                    if(SampleInputs!=null && SampleInputs.Count > 0)
                    {
                        textBox.Text = SampleInputs[i - 1];
                    }
                   
                }
                else if(drpStatus.SelectedValue == "Pending Records")
                {
                    if (SampleInputs != null&& SampleInputs.Count>0)
                    {
                        textBox.Text = SampleInputs[i - 1];
                    }
                }
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
                btnSaveResult.Visible = false;
                btnSaveResult1.Visible = false;
                ResubmitButton.Visible = true;
                ExportButton.Visible = true;
                gridValue = true;
            }
            else
            {
                GrdWeightScheduleStatus.Visible = true;
                GrdWeightCompletedScheduleStatus.Visible = false;
                BindGrdWeightScheduleStatus();
                ActionButton.Visible = true;
                btnSaveResult.Visible = true;
                btnSaveResult1.Visible = true;
                ResubmitButton.Visible = false;
                ExportButton.Visible = false;
                gridValue = false;
            }
            GetGrammage();
        }

        protected void GetGrammage()
        {
            if (GrdWeightScheduleStatus != null && GrdWeightScheduleStatus.Rows.Count > 0)
            {
                SqlParameter[] parame = new SqlParameter[]
                 {
                    new SqlParameter("@OPR","73"),
                    new SqlParameter("@ScheduleId",GrdWeightScheduleStatus.DataKeys[0].Values[1])

                 };
                DataTable data = new DataTable();
                data = DBClass.GetDatatable_WithParam(parame, "DM_SP_GET_DATA");
                foreach (DataRow dataRow in data.Rows)
                {
                    Session["WeightScheduleId"] = dataRow["Grammage"];
                }
            }
            if (GrdWeightCompletedScheduleStatus != null && GrdWeightCompletedScheduleStatus.Rows.Count > 0)
            {
                SqlParameter[] parame = new SqlParameter[]
                 {
                    new SqlParameter("@OPR","73"),
                     new SqlParameter("@ScheduleId",GrdWeightCompletedScheduleStatus.DataKeys[0].Values[1])
                 };
                DataTable data = new DataTable();
                data = DBClass.GetDatatable_WithParam(parame, "DM_SP_GET_DATA");
                foreach (DataRow dataRow in data.Rows)
                {
                    Session["WeightScheduleId"] = dataRow["Grammage"];
                }
            }
            if (Session["WeightScheduleId"] != null && !String.IsNullOrEmpty(Session["WeightScheduleId"].ToString()))
            {
                txtGrammage.Text = "Grammage=" + Session["WeightScheduleId"];
            }
            else
            {
                txtGrammage.Text = "Grammage=" + "0";
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
            BindWeightCompletedScheduleStatusGrd();
            foreach (GridViewRow row in GrdWeightCompletedScheduleStatus.Rows)
            {
                GrdWeightCompletedScheduleStatus.Columns[0].Visible = false;
            }
            GrdWeightCompletedScheduleStatus.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
        protected void ExportToExcel(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Weight.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                //To Export all pages
                GrdWeightCompletedScheduleStatus.AllowPaging = false;

                GrdWeightCompletedScheduleStatus.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GrdWeightCompletedScheduleStatus.HeaderRow.Cells)
                {
                    cell.BackColor = GrdWeightCompletedScheduleStatus.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GrdWeightCompletedScheduleStatus.Rows)
                {
                    LinkButton lnk = (LinkButton)row.FindControl("lnkAddSampleInput");
                    lnk.Visible = false;
                    GrdWeightCompletedScheduleStatus.Columns[0].Visible = false;
                    GrdWeightCompletedScheduleStatus.Columns[9].Visible = false;
                    GrdWeightCompletedScheduleStatus.Columns[10].Visible = true;
                    GrdWeightCompletedScheduleStatus.Columns[14].Visible = false;
                    GrdWeightCompletedScheduleStatus.Columns[15].Visible = true;
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GrdWeightCompletedScheduleStatus.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GrdWeightCompletedScheduleStatus.RowStyle.BackColor;
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

                GrdWeightCompletedScheduleStatus.RenderControl(hw);
                Response.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        protected void BindGrdWeightScheduleStatus()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    //new SqlParameter("@OPR","40"),
                    new SqlParameter("@OPR","70"),
                    new SqlParameter("@ScheduleTypeId","4"),
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
                GrdWeightScheduleStatus.DataSource = Dt;

                GrdWeightScheduleStatus.DataBind();
                foreach (GridViewRow row in GrdWeightScheduleStatus.Rows)
                {
                    //var ImageName = (GrdWeightScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("Image_Status") as System.Web.UI.WebControls.Image).ImageUrl;
                    //if (!string.IsNullOrEmpty(ImageName))
                    //{
                        //(GrdWeightScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("Select") as CheckBox).Visible = false;
                        //(GrdWeightScheduleStatus.Rows[row.RowIndex].Cells[9].FindControl("lblResult") as TextBox).ReadOnly = true;
                       // (GrdWeightScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("ExpectedResult") as TextBox).ReadOnly = true;

                   // }
                }
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
                    //new SqlParameter("@OPR","44"),
                    new SqlParameter("@OPR","71"),
                    new SqlParameter("@ScheduleTypeId","4"),
                    new SqlParameter("@ShiftCode",lblshift.Text.Trim()),
                    new SqlParameter("@PlantCode",lblplantcode.Text),
                    new SqlParameter("@Line",lblline.Text)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdWeightCompletedScheduleStatus.DataSource = Dt;

                GrdWeightCompletedScheduleStatus.DataBind();
                foreach (GridViewRow row in GrdWeightCompletedScheduleStatus.Rows)
                {
                    (GrdWeightCompletedScheduleStatus.Rows[row.RowIndex].Cells[row.RowIndex].FindControl("Select") as CheckBox).Checked = false;
                }
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
            //Session["ScheduleId"] = GrdWeightScheduleStatus.DataKeys[0].Values[1];
            if (GrdWeightScheduleStatus != null && GrdWeightScheduleStatus.Rows.Count > 0)
            {
                Session["ScheduleId"] = GrdWeightScheduleStatus.DataKeys[0].Values[1];
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
                // var ScheduleId = Session["ScheduleId"];
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "27"),
                    //new SqlParameter("@ScheduleId",Session["ScheduleId"] )
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
                //Dtt = DBClass.GetDatatable_WithParam(param1, "DM_SP_GET_DATA");
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
                //else if (string.IsNullOrEmpty(txtEResult.Text) && (string.IsNullOrEmpty(txtLCL.Text) && string.IsNullOrEmpty(txtUCL.Text) && string.IsNullOrEmpty(drpQUOM.Text)))
                //{
                //    if (QuantitativeGroup.Visible)
                //    {
                //        lblStartMessage.Text = "Please enter either qualitative or quantitative parameter !";
                //        lblStartMessage.ForeColor = Color.Red;
                //    }
                //    MPAddParameter.Show();
                //}
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
                                parameters.Add(new SqlParameter("ExpectedResult", (GrdWeightScheduleStatus.Rows[i].Cells[11].FindControl("ExpectedResult") as TextBox).Text.Trim()));


                                obj = DBClass.ExecuteNonQuery_WithParam(parameters.ToArray(), "DM_SP_DML_DATA");
                                parameters.Clear();
                            }
                        }
                    }
                    if (obj != null && Convert.ToInt64(obj) != 0)
                    {
                        BindGrdWeightScheduleStatus();
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
                //var x = (GrdWeightScheduleStatus.Rows[0].Cells[11].FindControl("ExpectedResult") as TextBox).Text.Trim();
                int isChecked = 0;
                int isResult = 0;
                int isExpectedResult = 0;
                for (int i = 0; i < GrdWeightScheduleStatus.Rows.Count; i++)
                {
                    var status = (GrdWeightScheduleStatus.Rows[i].Cells[11].FindControl("Image_Status") as System.Web.UI.WebControls.Image).ImageUrl;
                    if ((GrdWeightScheduleStatus.Rows[i].Cells[0].FindControl("Select") as CheckBox).Checked)
                    {
                        isChecked++;
                        // break;
                    }
                    if (string.IsNullOrEmpty(status))
                    {
                        if (!string.IsNullOrEmpty((GrdWeightScheduleStatus.Rows[i].Cells[0].FindControl("lblResult") as TextBox).Text))
                        {
                            isResult++;
                            //break;
                        }
                        if (!string.IsNullOrEmpty((GrdWeightScheduleStatus.Rows[i].Cells[0].FindControl("ExpectedResult") as TextBox).Text))
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
                    for (int i = 0; i < GrdWeightScheduleStatus.Rows.Count; i++)
                    {

                        CheckBox cb = (CheckBox)GrdWeightScheduleStatus.Rows[i].Cells[0].FindControl("Select"); //find the CheckBox
                        if (cb != null)
                        {
                            if (cb.Checked)
                            {
                                var result = (GrdWeightScheduleStatus.Rows[i].Cells[9].FindControl("lblResult") as TextBox).Text.Trim().ToString();
                                if (string.IsNullOrEmpty((GrdWeightScheduleStatus.Rows[i].Cells[9].FindControl("lblResult") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Result')", true);
                                }
                                else if (string.IsNullOrEmpty((GrdWeightScheduleStatus.Rows[i].Cells[10].FindControl("ExpectedResult") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Expected result')", true);
                                }
                                //else if (!(result.Contains(".")))
                                //{
                                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Result should be decimal value')", true);
                                //}
                                else
                                {
                                    HiddenField hiddenField=(HiddenField)GrdWeightScheduleStatus.Rows[i].Cells[10].FindControl("HiddenField1");
                                    parameters.Add(new SqlParameter("OPR", "17"));
                                    parameters.Add(new SqlParameter("TestScheduleReportId", GrdWeightScheduleStatus.DataKeys[i].Values[0]));
                                    parameters.Add(new SqlParameter("Result", (GrdWeightScheduleStatus.Rows[i].Cells[11].FindControl("lblResult") as TextBox).Text.Trim().ToString()));
                                    parameters.Add(new SqlParameter("ExpectedResult", (GrdWeightScheduleStatus.Rows[i].Cells[11].FindControl("ExpectedResult") as TextBox).Text.Trim()));
                                    parameters.Add(new SqlParameter("Status", "1"));
                                    parameters.Add(new SqlParameter("@CreatedBy", Convert.ToString(Session["username"])));
                                    if(!string.IsNullOrEmpty(hiddenField.Value))
                                    {
                                        parameters.Add(new SqlParameter("@SampleInputs", hiddenField.Value));
                                    }

                                    obj = DBClass.ExecuteNonQuery_WithParam(parameters.ToArray(), "DM_SP_DML_DATA");
                                    parameters.Clear();
                                }
                            }
                        }
                    }
                    if (obj != null && Convert.ToInt64(obj) != 0)
                    {
                        BindGrdWeightScheduleStatus();
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
                    try
                    {
                        SqlParameter[] param = new SqlParameter[]
                        {

                    new SqlParameter("@ScheduleTypeId","4"),
                    new SqlParameter("@PlantCode",lblplantcode.Text),
                    new SqlParameter("@Line",lblline.Text)

                        };
                        DataTable Dt = new DataTable();
                        Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_ScheduleId");
                        //GrdWeightScheduleStatus.DataSource = Dt.Rows[0]["ScheduleId"];
                        Session["ScheduleId"] = Dt.Rows[0]["ScheduleId"];

                        GrdWeightScheduleStatus.DataBind();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(string.Concat("BindGrdWeightScheduleStatus::", ex.Message), ex);
                    }
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
                                new SqlParameter("@ScheduleId",Session["ScheduleId"]),
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
                        new SqlParameter("@ScheduleId",Session["ScheduleId"]),
                        new SqlParameter("@ParameterId",drpParameter.SelectedValue)
                       };
                        object IsExist = DBClass.ExecuteNonQuery_WithParam(param.ToArray(), "DM_SP_GET_DATA");
                        if (Convert.ToInt64(IsExist) == -1)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter alredy Exsist')", true);
                        }
                        else
                        {
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
                                new SqlParameter("@ScheduleTypeId", 4),
                                new SqlParameter("@ParameterUniqueId",Convert.ToInt64(obj))
                                      };
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
                                BindGrdWeightScheduleStatus();
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
            drpParameter.SelectedIndex = 0;
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

        protected void btnEditResult_Click(object sender, EventArgs e)
        {
            Object obj = new Object();
            try
            {
                var x = (GrdWeightCompletedScheduleStatus.Rows[0].Cells[11].FindControl("ExpectedResult") as TextBox).Text.Trim();
                int isChecked = 0;
                int isResult = 0;
                int isExpectedResult = 0;
                for (int i = 0; i < GrdWeightCompletedScheduleStatus.Rows.Count; i++)
                {
                    // var status = (GrdWeightCompletedScheduleStatus.Rows[i].Cells[i].FindControl("Image_Status") as System.Web.UI.WebControls.Image).ImageUrl;
                    if ((GrdWeightCompletedScheduleStatus.Rows[i].Cells[0].FindControl("Select") as CheckBox).Checked)
                    {
                        isChecked++;
                        // break;
                    }
                    //if (string.IsNullOrEmpty(status))
                    //{
                    if (!string.IsNullOrEmpty((GrdWeightCompletedScheduleStatus.Rows[i].Cells[0].FindControl("lblResult") as TextBox).Text))
                    {
                        isResult++;
                        //break;
                    }
                    if (!string.IsNullOrEmpty((GrdWeightCompletedScheduleStatus.Rows[i].Cells[0].FindControl("ExpectedResult") as TextBox).Text))
                    {
                        isExpectedResult++;
                        //break;
                    }
                    // }
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
                    for (int i = 0; i < GrdWeightCompletedScheduleStatus.Rows.Count; i++)
                    {

                        CheckBox cb = (CheckBox)GrdWeightCompletedScheduleStatus.Rows[i].Cells[0].FindControl("Select"); //find the CheckBox
                        if (cb != null)
                        {
                            if (cb.Checked)
                            {
                                var result = (GrdWeightCompletedScheduleStatus.Rows[i].Cells[9].FindControl("lblResult") as TextBox).Text.Trim().ToString();
                                if (string.IsNullOrEmpty((GrdWeightCompletedScheduleStatus.Rows[i].Cells[9].FindControl("lblResult") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Result')", true);
                                }
                                else if (string.IsNullOrEmpty((GrdWeightCompletedScheduleStatus.Rows[i].Cells[10].FindControl("ExpectedResult") as TextBox).Text.Trim().ToString()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please Enter the Expected result')", true);
                                }
                                //else if (!(result.Contains(".")))
                                //{
                                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Result should be decimal value')", true);
                                //}
                                else
                                {
                                    HiddenField hiddenField = (HiddenField)GrdWeightCompletedScheduleStatus.Rows[i].Cells[10].FindControl("HiddenField1");
                                    parameters.Add(new SqlParameter("OPR", "17"));
                                    parameters.Add(new SqlParameter("TestScheduleReportId", GrdWeightCompletedScheduleStatus.DataKeys[i].Values[0]));
                                    parameters.Add(new SqlParameter("Result", (GrdWeightCompletedScheduleStatus.Rows[i].Cells[11].FindControl("lblResult") as TextBox).Text.Trim().ToString()));
                                    parameters.Add(new SqlParameter("ExpectedResult", (GrdWeightCompletedScheduleStatus.Rows[i].Cells[11].FindControl("ExpectedResult") as TextBox).Text.Trim()));
                                    parameters.Add(new SqlParameter("Status", "1"));
                                    parameters.Add(new SqlParameter("@CreatedBy", Convert.ToString(Session["username"])));
                                    if (!string.IsNullOrEmpty(hiddenField.Value))
                                    {
                                        parameters.Add(new SqlParameter("@SampleInputs", hiddenField.Value));
                                    }

                                    obj = DBClass.ExecuteNonQuery_WithParam(parameters.ToArray(), "DM_SP_DML_DATA");
                                    parameters.Clear();
                                }
                            }
                        }
                    }
                    if (obj != null && Convert.ToInt64(obj) != 0)
                    {
                        BindWeightCompletedScheduleStatusGrd();
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
                         new SqlParameter("@ScheduleTypeId","4"),
                    new SqlParameter("@ShiftCode",lblshift.Text.Trim()),
                    new SqlParameter("@PlantCode",lblplantcode.Text),
                    new SqlParameter("@Line",lblline.Text),
                    new SqlParameter("@FromDate", txtScheduleFromDate.Text),
                    new SqlParameter("@ToDate", txtScheduleToDate.Text)
                    };

                    DataTable Dt = new DataTable();
                    Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                    GrdWeightCompletedScheduleStatus.DataSource = Dt;
                    GrdWeightCompletedScheduleStatus.DataBind();
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindCompletedScheduleStatusGrid::", ex.Message), ex);
            }
        }

        protected void GrdWeightScheduleStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                foreach (GridViewRow row in GrdWeightScheduleStatus.Rows)
                {
                    (GrdWeightScheduleStatus.Rows[row.RowIndex].Cells[9].FindControl("lblResult") as TextBox).Enabled = false;
                }
            }

        }

        protected void GrdWeightCompletedScheduleStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                foreach (GridViewRow row in GrdWeightCompletedScheduleStatus.Rows)
                {
                    (GrdWeightCompletedScheduleStatus.Rows[row.RowIndex].Cells[9].FindControl("lblResult") as TextBox).Enabled = false;
                }
            }
        }
    }
}