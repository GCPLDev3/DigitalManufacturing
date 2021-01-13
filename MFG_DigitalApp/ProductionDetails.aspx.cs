using MFG_DigitalApp.Log;
using MFG_DigitalApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.IO;

namespace MFG_DigitalApp
{
    public partial class ProductionDetails : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("RunDetails");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        clsSensorQty objclsSensorQty = new clsSensorQty();
        DataSet ds_CartonPacking = new DataSet();
        public static string labeldate = "";
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
                    var date = model.ShiftDate.ToString().Substring(0, model.ShiftDate.IndexOf(' '));
                    DateTime dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                    //lbldate.Text = dateTime.ToString("yyyy-MM-dd"); ;
                    labeldate = dateTime.ToString("yyyy-MM-dd");
                    if (lblshift.Text.Equals("S3") || lblshift.Text.Equals("S6"))
                    {
                        if (Session["CountryId"].ToString().Trim() == "1" && DateTime.Now.Hour <= 6)
                        {
                            lbldate.Text = dateTime.AddDays(-1).ToShortDateString();
                        }
                        else if (Session["CountryId"].ToString().Trim() == "2" && ((DateTime.Now.Hour <= 7 && DateTime.Now.Minute <= 30)||(DateTime.Now.Hour==23) || (DateTime.Now.Hour >= 22 && DateTime.Now.Minute >= 30)))
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
                    //lbldate.Text = FormatedDate;
                    //lbldate.Text = model.ShiftDate;
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
                GetShift();
                BindProductionEntryGrid();
            }
        }

        protected void btnstoppages_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoppagesReason.aspx", false);
        }

        protected void btnRunDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("RunDetails.aspx", false);
        }

        public void BindProductionEntryGrid()
        {
            try
            {
                ds_CartonPacking = new DataSet();
                GetSensorQty();

                //var date = lbldate.Text.ToString();
                try
                {
                    var date = labeldate;
                    DateTime datetime = DateTime.Parse(date);
                    //var FormatedDate = datetime.ToString("yyyy-MM-dd");
                    var FormatedDate = Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd");
                    SqlParameter[] param = new SqlParameter[]
                    {
                    new SqlParameter("@OPR", "1"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@Date", FormatedDate),
                    new SqlParameter("@PONumber", ""),
                    };
                    DataSet Dt = new DataSet();
                    
                    Dt = DBClass.GetDataSet_WithParam(param, "SP_GET_DATA");
                    if (Dt.Tables[1].Rows.Count > 0)
                    {
                        if (Dt.Tables[1].Rows[0]["IsEditable"].ToString().ToLower() == "false")
                        {
                            btnSaveProdEntry.Enabled = false;
                            btnSaveProdEntry.Visible = false;
                            btnSaveProdEntry1.Enabled = false;
                            btnSaveProdEntry1.Visible = false;
                            lblMsgWRP1.Text = "Production entry already sent to SAP.";
                            lblMsgWRP2.Text = "Production entry already sent to SAP.";
                            lblMsgWRP3.Text = "Production entry already sent to SAP.";

                        }
                        else
                        {
                            btnSaveProdEntry.Enabled = true;
                            btnSaveProdEntry.Visible = true;
                            btnSaveProdEntry1.Enabled = true;
                            btnSaveProdEntry1.Visible = true;
                            lblMsgWRP1.Text = string.Empty;
                            lblMsgWRP2.Text = string.Empty;
                            lblMsgWRP3.Text = string.Empty;
                        }
                    }
                    DivHeaderEntry1.Visible = false;
                    DivGridEntry1.Visible = false;
                    DivHeaderEntry2.Visible = false;
                    DivGridEntry2.Visible = false;
                    DivHeaderEntry3.Visible = false;
                    DivGridEntry3.Visible = false;
                    GrdProdEntry1.DataSource = null;
                    GrdProdEntry1.DataBind();
                    GrdProdEntry2.DataSource = null;
                    GrdProdEntry2.DataBind();
                    GrdProdEntry3.DataSource = null;
                    GrdProdEntry3.DataBind();

                    GetWIPDetails(); // get WIP details

                    if (Dt != null & Dt.Tables.Count > 0)
                    {
                        DataTable dtMRP = new DataTable();
                        dtMRP = Dt.Tables[0];
                        DataTable dtEntry = new DataTable();
                        dtEntry = Dt.Tables[1];

                        int count = 1;

                        foreach (DataRow row in Dt.Tables[0].Rows)
                        {
                            if (count == 1)
                            {

                                DivHeaderEntry1.Visible = true;
                                DivGridEntry1.Visible = true;
                                lblWRP1.Text = Convert.ToString(row["WRPLine"]);
                                DataTable dt1 = new DataTable();
                                dt1 = dtEntry.Select("WRPLine = '" + lblWRP1.Text + "'").CopyToDataTable();
                                if (ds_CartonPacking.Tables.Count > 0)
                                {
                                    if (ds_CartonPacking.Tables[0] != null)
                                    {
                                        if (ds_CartonPacking.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt1.Rows.Count; i++)
                                            {
                                                dt1.Rows[i]["SensorQty"] = ds_CartonPacking.Tables[0].Rows[i]["SensorQty"];
                                            }
                                            dt1.AcceptChanges();
                                        }
                                    }
                                }
                                GrdProdEntry1.DataSource = dt1;
                                GrdProdEntry1.DataBind();
                                
                                //Calculate Sum and display in Footer Row
                                Int32 totalSensorQty = dt1.AsEnumerable().Sum(r => r.Field<Int32>("SensorQty"));
                                Int32 totalActualQty = dt1.AsEnumerable().Sum(r => r.Field<Int32>("ActualQty"));
                                Int32 totalQltyRejection = dt1.AsEnumerable().Sum(r => r.Field<Int32>("QltyRejection"));
                                Int32 totalProcessRejection = dt1.AsEnumerable().Sum(r => r.Field<Int32>("ProcessRejection"));
                                GrdProdEntry1.FooterRow.Cells[3].Text = "Total";
                                GrdProdEntry1.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry1.FooterRow.Cells[3].Font.Bold = true;
                                GrdProdEntry1.FooterRow.Cells[4].Text = totalSensorQty.ToString();
                                GrdProdEntry1.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry1.FooterRow.Cells[4].Font.Bold = true;
                                GrdProdEntry1.FooterRow.Cells[5].Text = totalActualQty.ToString();
                                GrdProdEntry1.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry1.FooterRow.Cells[5].Width = 50;
                                GrdProdEntry1.FooterRow.Cells[5].Font.Bold = true;
                                GrdProdEntry1.FooterRow.Cells[7].Text = totalQltyRejection.ToString();
                                GrdProdEntry1.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry1.FooterRow.Cells[7].Width = 50;
                                GrdProdEntry1.FooterRow.Cells[7].Font.Bold = true;
                                GrdProdEntry1.FooterRow.Cells[8].Text = totalProcessRejection.ToString();
                                GrdProdEntry1.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry1.FooterRow.Cells[8].Width = 50;
                                GrdProdEntry1.FooterRow.Cells[8].Font.Bold = true;
                            }
                            if (count == 2)
                            {
                                DivHeaderEntry2.Visible = true;
                                DivGridEntry2.Visible = true;
                                lblWRP2.Text = Convert.ToString(row["WRPLine"]);
                                DataTable dt2 = new DataTable();
                                dt2 = dtEntry.Select("WRPLine = '" + lblWRP2.Text + "'").CopyToDataTable();
                                if (ds_CartonPacking.Tables.Count > 0)
                                {
                                    if (ds_CartonPacking.Tables[1] != null)
                                    {
                                        if (ds_CartonPacking.Tables[1].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt2.Rows.Count; i++)
                                            {
                                                dt2.Rows[i]["SensorQty"] = ds_CartonPacking.Tables[1].Rows[i]["SensorQty"];
                                            }
                                            dt2.AcceptChanges();
                                        }
                                    }
                                }
                                GrdProdEntry2.DataSource = dt2;
                                GrdProdEntry2.DataBind();

                                //Calculate Sum and display in Footer Row
                                Int32 totalSensorQty = dt2.AsEnumerable().Sum(r => r.Field<Int32>("SensorQty"));
                                Int32 totalActualQty = dt2.AsEnumerable().Sum(r => r.Field<Int32>("ActualQty"));
                                Int32 totalQltyRejection = dt2.AsEnumerable().Sum(r => r.Field<Int32>("QltyRejection"));
                                Int32 totalProcessRejection = dt2.AsEnumerable().Sum(r => r.Field<Int32>("ProcessRejection"));
                                GrdProdEntry2.FooterRow.Cells[3].Text = "Total";
                                GrdProdEntry2.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry2.FooterRow.Cells[3].Font.Bold = true;
                                GrdProdEntry2.FooterRow.Cells[4].Text = totalSensorQty.ToString();
                                GrdProdEntry2.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry2.FooterRow.Cells[4].Font.Bold = true;
                                GrdProdEntry2.FooterRow.Cells[5].Text = totalActualQty.ToString();
                                GrdProdEntry2.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry2.FooterRow.Cells[5].Width = 50;
                                GrdProdEntry2.FooterRow.Cells[5].Font.Bold = true;
                                GrdProdEntry2.FooterRow.Cells[7].Text = totalQltyRejection.ToString();
                                GrdProdEntry2.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry2.FooterRow.Cells[7].Width = 50;
                                GrdProdEntry2.FooterRow.Cells[7].Font.Bold = true;
                                GrdProdEntry2.FooterRow.Cells[8].Text = totalProcessRejection.ToString();
                                GrdProdEntry2.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry2.FooterRow.Cells[8].Width = 50;
                                GrdProdEntry2.FooterRow.Cells[8].Font.Bold = true;
                            }
                            if (count == 3)
                            {
                                DivHeaderEntry3.Visible = true;
                                DivGridEntry3.Visible = true;
                                lblWRP3.Text = Convert.ToString(row["WRPLine"]);
                                DataTable dt3 = new DataTable();
                                dt3 = dtEntry.Select("WRPLine = '" + lblWRP3.Text + "'").CopyToDataTable();
                                if (ds_CartonPacking.Tables.Count > 0)
                                {
                                    if (ds_CartonPacking.Tables[2] != null)
                                    {
                                        if (ds_CartonPacking.Tables[2].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt3.Rows.Count; i++)
                                            {
                                                dt3.Rows[i]["SensorQty"] = ds_CartonPacking.Tables[2].Rows[i]["SensorQty"];
                                            }
                                            dt3.AcceptChanges();
                                        }
                                    }
                                }
                                GrdProdEntry3.DataSource = dt3;
                                GrdProdEntry3.DataBind();

                                //Calculate Sum and display in Footer Row
                                Int32 totalSensorQty = dt3.AsEnumerable().Sum(r => r.Field<Int32>("SensorQty"));
                                Int32 totalActualQty = dt3.AsEnumerable().Sum(r => r.Field<Int32>("ActualQty"));
                                Int32 totalQltyRejection = dt3.AsEnumerable().Sum(r => r.Field<Int32>("QltyRejection"));
                                Int32 totalProcessRejection = dt3.AsEnumerable().Sum(r => r.Field<Int32>("ProcessRejection"));
                                GrdProdEntry3.FooterRow.Cells[3].Text = "Total";
                                GrdProdEntry3.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry3.FooterRow.Cells[3].Font.Bold = true;
                                GrdProdEntry3.FooterRow.Cells[4].Text = totalSensorQty.ToString();
                                GrdProdEntry3.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry3.FooterRow.Cells[4].Font.Bold = true;
                                GrdProdEntry3.FooterRow.Cells[5].Text = totalActualQty.ToString();
                                GrdProdEntry3.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry3.FooterRow.Cells[5].Width = 50;
                                GrdProdEntry3.FooterRow.Cells[5].Font.Bold = true;
                                GrdProdEntry3.FooterRow.Cells[7].Text = totalQltyRejection.ToString();
                                GrdProdEntry3.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry3.FooterRow.Cells[7].Width = 50;
                                GrdProdEntry3.FooterRow.Cells[7].Font.Bold = true;
                                GrdProdEntry3.FooterRow.Cells[8].Text = totalProcessRejection.ToString();
                                GrdProdEntry3.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                                GrdProdEntry3.FooterRow.Cells[8].Width = 50;
                                GrdProdEntry3.FooterRow.Cells[8].Font.Bold = true;
                            }

                            count = count + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.Message + ex.StackTrace);
                    _logger.Error(string.Concat("BindProductionEntryGrid::", ex.Message), ex);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
                _logger.Error(string.Concat("BindProductionEntryGrid::", ex.Message), ex);
            }
        }

        protected void btnSaveProdEntry_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateWIPDetails();

                bool DataSaved = true;
                bool IsIOTLines = false;
                IsIOTLines = GetIsIOTLine();
                StringBuilder strErrorMsg = new StringBuilder();
                double SystemPercentage = Convert.ToDouble(ConfigurationManager.AppSettings["Percentage"]);
                bool IsReasonRequired = false;
                int entry1 = 0;
                foreach (GridViewRow row in GrdProdEntry1.Rows)
                {
                    //string WRP = (row.Cells[0].Text);
                    string Time = (row.Cells[1].Text);
                    Int32 SensorQty = Convert.ToInt32(row.Cells[4].Text == "" ? "0" : row.Cells[4].Text);
                    TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                    Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                    DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                    TextBox txtQltyRejection= (TextBox)row.FindControl("txtQltyRejection");
                    Int32 QltyRejection = Convert.ToInt32(txtQltyRejection.Text.Trim() == "" ? "0" : txtQltyRejection.Text.Trim());
                    TextBox txtProcessRejection = (TextBox)row.FindControl("txtProcessRejection");
                    Int32 ProcessRejection = Convert.ToInt32(txtProcessRejection.Text.Trim() == "" ? "0" : txtProcessRejection.Text.Trim());
                    if (IsIOTLines && drpReasons.SelectedValue.Trim() == "" && (ActualQty > (SensorQty + (SensorQty * SystemPercentage)) || ActualQty < (SensorQty - (SensorQty * SystemPercentage))))
                    {
                        if (entry1 == 0)
                        {
                            strErrorMsg.AppendLine("" + lblWRP1.Text + "");
                            entry1 = entry1 + 1;
                        }
                        IsReasonRequired = true;
                        strErrorMsg.AppendLine("Kindly Select Reason for " + Time + "");
                    }
                }

                int entry2 = 0;
                foreach (GridViewRow row in GrdProdEntry2.Rows)
                {
                    //string WRP = (row.Cells[0].Text);
                    string Time = (row.Cells[1].Text);
                    Int32 SensorQty = Convert.ToInt32(row.Cells[4].Text == "" ? "0" : row.Cells[4].Text);
                    TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                    Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                    DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                    TextBox txtQltyRejection = (TextBox)row.FindControl("txtQltyRejection");
                    Int32 QltyRejection = Convert.ToInt32(txtQltyRejection.Text.Trim() == "" ? "0" : txtQltyRejection.Text.Trim());
                    TextBox txtProcessRejection = (TextBox)row.FindControl("txtProcessRejection");
                    Int32 ProcessRejection = Convert.ToInt32(txtProcessRejection.Text.Trim() == "" ? "0" : txtProcessRejection.Text.Trim());
                    if (IsIOTLines && drpReasons.SelectedValue == "" && (ActualQty > (SensorQty + (SensorQty * SystemPercentage)) || ActualQty < (SensorQty - (SensorQty * SystemPercentage))))
                    {
                        if (entry2 == 0)
                        {
                            strErrorMsg.AppendLine("" + lblWRP2.Text + "");
                            entry2 = entry2 + 1;
                        }
                        IsReasonRequired = true;
                        strErrorMsg.AppendLine("Kindly Select Reason for " + Time + "");
                    }
                }

                int entry3 = 0;
                foreach (GridViewRow row in GrdProdEntry3.Rows)
                {
                    //string WRP = (row.Cells[0].Text);
                    string Time = (row.Cells[1].Text);
                    Int32 SensorQty = Convert.ToInt32(row.Cells[4].Text == "" ? "0" : row.Cells[4].Text);
                    TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                    Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                    DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                    TextBox txtQltyRejection = (TextBox)row.FindControl("txtQltyRejection");
                    Int32 QltyRejection = Convert.ToInt32(txtQltyRejection.Text.Trim() == "" ? "0" : txtQltyRejection.Text.Trim());
                    TextBox txtProcessRejection = (TextBox)row.FindControl("txtProcessRejection");
                    Int32 ProcessRejection = Convert.ToInt32(txtProcessRejection.Text.Trim() == "" ? "0" : txtProcessRejection.Text.Trim());
                    if (IsIOTLines && drpReasons.SelectedValue == "" && (ActualQty > (SensorQty + (SensorQty * SystemPercentage)) || ActualQty < (SensorQty - (SensorQty * SystemPercentage))))
                    {
                        if (entry3 == 0)
                        {
                            strErrorMsg.AppendLine("" + lblWRP3.Text + "");
                            entry3 = entry3 + 1;
                        }
                        IsReasonRequired = true;
                        strErrorMsg.AppendLine("Kindly Enter Reason for " + Time + "");
                    }
                }

                // validation faile then show alert
                if (IsReasonRequired)
                {
                    string str = strErrorMsg.ToString().Replace("\r\n", "\\n");
                    if (Session["CountryId"].ToString().Trim() == "1" && Session["RoleId"].ToString().Trim() != "3")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('" + str + "')", true);
                        return;
                    }
                }
                // Save process start
                foreach (GridViewRow row in GrdProdEntry1.Rows)
                {
                    try
                    {
                        UserSelectionModel model = new UserSelectionModel();
                        model = (UserSelectionModel)Session["UserSelectionModel"];
                        var date = model.ShiftDate.ToString().Substring(0, model.ShiftDate.IndexOf(' '));
                        DateTime dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                        //labeldate = dateTime.ToString("yyyy-MM-dd");
                        labeldate = Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd");
                        TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                        Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                        TextBox txtQltyRejection = (TextBox)row.FindControl("txtQltyRejection");
                        Int32 QltyRejection = Convert.ToInt32(txtQltyRejection.Text.Trim() == "" ? "0" : txtQltyRejection.Text.Trim());
                        TextBox txtProcessRejection = (TextBox)row.FindControl("txtProcessRejection");
                        Int32 ProcessRejection = Convert.ToInt32(txtProcessRejection.Text.Trim() == "" ? "0" : txtProcessRejection.Text.Trim());
                        DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                        string Time = (row.Cells[1].Text);
                        string PONumber = (row.Cells[2].Text.Replace("&nbsp;", ""));
                        Int32 result;
                        SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@OPR", 5),
                            new SqlParameter("@PlantCode", lblplantcode.Text),
                            new SqlParameter("@Line", lblline.Text),
                            new SqlParameter("@Date", labeldate),
                            new SqlParameter("@ShiftCode", lblshift.Text),
                            new SqlParameter("@WRPLine", lblWRP1.Text), // WRP
                            new SqlParameter("@StartTime", Time.Split(' ')[0]),
                            new SqlParameter("@EndTime",Time.Split(' ')[2]),
                            new SqlParameter("@PONumber", PONumber),
                            new SqlParameter("@ActualQty",ActualQty),
                            new SqlParameter("@ReasonCode",drpReasons.SelectedValue),
                            new SqlParameter("@CreatedBy",Convert.ToString(Session["username"])),
                            new SqlParameter("@QltyRejection",QltyRejection),
                            new SqlParameter("@procRejection",ProcessRejection)
                        };

                        result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");

                    }
                    catch (Exception ex)
                    {
                        DataSaved = false;
                        WriteLog(ex.Message + ex.StackTrace);
                        _logger.Error(string.Concat("GrdProdEntry1::", ex.Message), ex);
                    }
                }


                foreach (GridViewRow row in GrdProdEntry2.Rows)
                {
                    try
                    {
                        TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                        Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                        TextBox txtQltyRejection = (TextBox)row.FindControl("txtQltyRejection");
                        Int32 QltyRejection = Convert.ToInt32(txtQltyRejection.Text.Trim() == "" ? "0" : txtQltyRejection.Text.Trim());
                        TextBox txtProcessRejection = (TextBox)row.FindControl("txtProcessRejection");
                        Int32 ProcessRejection = Convert.ToInt32(txtProcessRejection.Text.Trim() == "" ? "0" : txtProcessRejection.Text.Trim());
                        DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                        string Time = (row.Cells[1].Text);
                        string PONumber = (row.Cells[2].Text.Replace("&nbsp;", ""));
                        Int32 result;
                        SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@OPR", 5),
                            new SqlParameter("@PlantCode", lblplantcode.Text),
                            new SqlParameter("@Line", lblline.Text),
                            new SqlParameter("@Date", labeldate),
                            new SqlParameter("@ShiftCode", lblshift.Text),
                            new SqlParameter("@WRPLine", lblWRP2.Text), // WRP
                            new SqlParameter("@StartTime", Time.Split(' ')[0]),
                            new SqlParameter("@EndTime",Time.Split(' ')[2]),
                            new SqlParameter("@PONumber", PONumber),
                            new SqlParameter("@ActualQty",ActualQty),
                            new SqlParameter("@ReasonCode",drpReasons.SelectedValue),
                            new SqlParameter("@CreatedBy",Convert.ToString(Session["username"])),
                            new SqlParameter("@QltyRejection",QltyRejection),
                            new SqlParameter("@procRejection",ProcessRejection)
                        };

                        result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
                    }
                    catch (Exception ex)
                    {
                        DataSaved = false;
                        WriteLog(ex.Message + ex.StackTrace);
                        _logger.Error(string.Concat("GrdProdEntry2::", ex.Message), ex);
                    }
                }

                foreach (GridViewRow row in GrdProdEntry3.Rows)
                {
                    try
                    {
                        TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                        Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                        TextBox txtQltyRejection = (TextBox)row.FindControl("txtQltyRejection");
                        Int32 QltyRejection = Convert.ToInt32(txtQltyRejection.Text.Trim() == "" ? "0" : txtQltyRejection.Text.Trim());
                        TextBox txtProcessRejection = (TextBox)row.FindControl("txtProcessRejection");
                        Int32 ProcessRejection = Convert.ToInt32(txtProcessRejection.Text.Trim() == "" ? "0" : txtProcessRejection.Text.Trim());
                        DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                        string Time = (row.Cells[1].Text);
                        string PONumber = (row.Cells[2].Text.Replace("&nbsp;", ""));
                        Int32 result;
                        SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@OPR", 5),
                            new SqlParameter("@PlantCode", lblplantcode.Text),
                            new SqlParameter("@Line", lblline.Text),
                            new SqlParameter("@Date",labeldate),
                            new SqlParameter("@ShiftCode", lblshift.Text),
                            new SqlParameter("@WRPLine", lblWRP3.Text), // WRP
                            new SqlParameter("@StartTime", Time.Split(' ')[0]),
                            new SqlParameter("@EndTime",Time.Split(' ')[2]),
                            new SqlParameter("@PONumber", PONumber),
                            new SqlParameter("@ActualQty",ActualQty),
                            new SqlParameter("@ReasonCode",drpReasons.SelectedValue),
                            new SqlParameter("@CreatedBy",Convert.ToString(Session["username"])),
                            new SqlParameter("@QltyRejection",QltyRejection),
                            new SqlParameter("@procRejection",ProcessRejection)
                        };
                        result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");

                    }
                    catch (Exception ex)
                    {
                        DataSaved = false;
                        WriteLog(ex.Message + ex.StackTrace);
                        _logger.Error(string.Concat("GrdProdEntry2::", ex.Message), ex);
                    }
                }
                SqlParameter[] param_Active = new SqlParameter[]
                                           {
                                            new SqlParameter("@OPR", "6"),
                                            new SqlParameter("@PlantCode", lblplantcode.Text),
                                            new SqlParameter("@Line", lblline.Text),
                                           };
                DataTable dtlineActive = DBClass.GetDatatable_WithParam(param_Active, "DM_SP_InsertProductionDetails");
                if (dtlineActive.Rows.Count > 0)
                {
                    DataTable dtlineBatch = new DataTable();
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
                            dtlineBatch = DBClass.GetDatatable_WithParamInGDRJ(param_Batch, "DM_SP_GET_IOT_DATA");
                            //dtlineBatch = DBClass.GetDatatable_WithParam(param_Batch, "DM_SP_GET_IOT_DATA");
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

                    if (DataSaved)
                    {
                        SqlParameter[] param_BatchUpd = new SqlParameter[]
                                        {
                                                            new SqlParameter("@OPR", "4"),
                                                            new SqlParameter("@PlantCode",lblplantcode.Text),
                                                            new SqlParameter("@PONumber",dtlineActive.Rows[0]["PONumber"].ToString())
                                        };
                        DataTable dt = DBClass.GetDatatable_WithParam(param_BatchUpd, "SP_GET_DATA");
                        int orderqty = Convert.ToInt16(dt.Rows[0]["OrderQuantity"].ToString());
                        int producedqty = Convert.ToInt16(dt.Rows[0]["ProducedQuanity"].ToString());
                        decimal percentage = (dt.Rows[0]["Percentage"].ToString()==null|| dt.Rows[0]["Percentage"].ToString() =="")? 0:Convert.ToDecimal(dt.Rows[0]["Percentage"].ToString());
                        int AdditionalQuantity = Convert.ToInt16(dt.Rows[0]["AdditionalQuantity"].ToString());
                        if (producedqty > Math.Round(orderqty + AdditionalQuantity + (orderqty * (percentage / 100))))
                        {
                            autostop(dtlineActive.Rows[0]["PONumber"].ToString(), dtlineBatch.Rows[0]["BatchNo"].ToString());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Produced quantity fulfills the ordered quantity count and so the order is autostopped.')", true);
                            Response.Redirect("~/RunDetails.aspx");
                        }
                        else
                        {
                            BindProductionEntryGrid();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Production Details updated Successfully.')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Error, Something went Wrong! datasave')", true);
                    }
                }
                else
                {
                    BindProductionEntryGrid();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Production Details updated Successfully.')", true);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
                _logger.Error(string.Concat("btnSaveProdEntry_Click::", ex.Message), ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Error, Something went Wrong! 1 '"+ex.Message+"','" +ex.StackTrace+"')", true);

            }
        }

        public static bool WriteLog(string strMessage)
        {
            try
            {
                FileStream objFilestream = new FileStream("D:\\Test\\Log_DM.txt", FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage + DateTime.Now.ToString());
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        protected void autostop(string ProcessOrder,string BatchNumber)
        {
            string indotime = "", Curtime = "";string date=string.Empty;
            if (Session["CountryId"] != null && Session["CountryId"].Equals("2"))
            {
                indotime = hdnfldVariableprod.Value;
                date = indotime.ToString().Substring(0, indotime.IndexOf(' '));
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
                date = DateTime.Now.ToString("yyyy-MM-dd");
                string hr = Convert.ToString(DateTime.Now.Hour);
                string mm = Convert.ToString(DateTime.Now.Minute);
                Curtime = hr + ":" + mm;
                //DateTime dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                //labeldate = dateTime.ToString("yyyy-MM-dd");
                labeldate = DateTime.Now.ToString("yyyy-MM-dd");

            }

            object result;
            SqlParameter[] param = new SqlParameter[]
            {
                    new SqlParameter("@OPR", 24),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@PONumber", ProcessOrder),
                    new SqlParameter("@batch", BatchNumber),
                    new SqlParameter("@StoppageDate",labeldate),
                    new SqlParameter("@StoppageTime",Convert.ToString(Curtime)),
                    new SqlParameter("@StoppageBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@CurrentStatus","Completed"),
                    new SqlParameter("@Currentdate",Convert.ToString(labeldate)),
                    new SqlParameter("@Currenttime",Convert.ToString(Curtime))

        };

            result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_DML_DATA");
            if (Convert.ToInt64(result) > 0)
            {
                CheckandSplitProductionEntry(ProcessOrder, labeldate, Convert.ToString(Curtime));
            }
        }

        private void CheckandSplitProductionEntry(string PONumber,string date,string time)
        {
            try
            {
                int result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR",1),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", date),
                    new SqlParameter("@ShiftCode", drpShift.SelectedItem.Text),
                    new SqlParameter("@PONumber",PONumber),
                    new SqlParameter("@StoppageTime",time),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                };

                result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_SPLITENTRY");
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
                _logger.Error(string.Concat("CheckandSplitProductionEntry::", ex.Message), ex);
            }
        }
        public bool GetIsIOTLine()
        {
            bool result = false;
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "12"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text)
                };
                var output = DBClass.ExecuteScalar_WithParam(param, "DM_SP_GET_DATA");
                result = Convert.ToBoolean(output);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
                _logger.Error(string.Concat("GetIsIOTLine::", ex.Message), ex);
            }
            return result;
        }

        // common for all the 3 entry grid.
        protected void GrdProdEntry1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList reasonList = (DropDownList)e.Row.FindControl("drpReasons");
                HiddenField hdnReasonCode = (HiddenField)e.Row.FindControl("hdnReasonCode");
                if (reasonList != null)
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@OPR", "58"),
                        new SqlParameter("@PlantCode", lblplantcode.Text),
                        new SqlParameter("@Line",lblline.Text.Trim())
                    };
                    DataTable Dt = new DataTable();
                    Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                    reasonList.DataSource = Dt;
                    reasonList.DataValueField = "ReasonCode";
                    reasonList.DataTextField = "ReasonDescription";
                    reasonList.DataBind();
                    reasonList.Items.Insert(0, new ListItem("Select Reason", ""));

                    if (hdnReasonCode.Value != "")
                    {
                        reasonList.SelectedValue = hdnReasonCode.Value;
                    }
                }
            }
        }

        public void GetWIPDetails()
        {
            try
            {
                txtWIP.Text = "0";
                txtOffLoadingsNos.Text = "0";
                txtQualityRejection.Text = "0";
                txtProcRejection.Text = "0";

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "13"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", labeldate),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    txtWIP.Text = Convert.ToString(Dt.Rows[0]["WIP"]);
                    txtOffLoadingsNos.Text = Convert.ToString(Dt.Rows[0]["OffLoadingsNos"]);
                    txtQualityRejection.Text = Convert.ToString(Dt.Rows[0]["QualityRejection"]);
                    txtProcRejection.Text = Convert.ToString(Dt.Rows[0]["ProcessRejection"]);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
                _logger.Error(string.Concat("GetWIPDetails::", ex.Message), ex);
            }
        }

        public void UpdateWIPDetails()
        {
            try
            {
                string Shift = drpShift.SelectedItem.Text;
                string[] tokens = Shift.Split('/');
                string date = tokens[tokens.Length - 1].Trim();
                //string dateTime = Convert.ToDateTime(date).ToString("yyyy-dd-MM");
                string dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null).ToShortDateString();
                //var dateTime = Convert.ToDateTime(lbldate.Text).ToString("dd/MM/yyyy");
                //DateTime dateTime1 = DateTime.Parse(dateTime);
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "7"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", dateTime),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@WIP", txtWIP.Text.ToString().Trim() == ""?"0":txtWIP.Text.ToString().Trim()),
                    new SqlParameter("@OffLoadingsNos", txtOffLoadingsNos.Text.Trim().ToString() == ""?"0":txtOffLoadingsNos.Text.Trim().ToString()),
                    new SqlParameter("@QualityRejection", txtQualityRejection.Text.Trim().ToString() == ""?"0":txtQualityRejection.Text.Trim().ToString()),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"])),
                    new SqlParameter("@processrejection",txtProcRejection.Text.ToString().Trim() == ""?"0":txtProcRejection.Text.ToString().Trim())
                };
                DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
                _logger.Error(string.Concat("UpdateWIPDetails::", ex.Message), ex);
            }
        }

        protected void GetShift()
        {
            try
            {
                var date = lbldate.Text.ToString();
                DateTime datetime = DateTime.Parse(date);
                var FormatedDate = datetime.ToString("yyyy-MM-dd");

                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "1"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Countryid", Session["CountryId"]),
                new SqlParameter("@currentTime", FormatedDate),
                new SqlParameter("@UserId", HttpContext.Current.Session["USERID"].ToString())
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_SHIFTDATA");
                foreach (DataRow data in Dt.Rows)
                {
                    if (data["ShiftName"] != null)
                    {
                        var shiftDate = data.ItemArray[1].ToString();
                        string[] splitDate = shiftDate.Split('/');
                        DateTime dateTime = Convert.ToDateTime(splitDate[1]);
                        var dateFormat = dateTime.ToString("dd-MM-yyyy");
                        data["ShiftName"] = splitDate[0] + " / " + dateFormat;
                    }
                }
                drpShift.DataSource = Dt;
                drpShift.DataTextField = "ShiftName";
                drpShift.DataValueField = "ShiftCode";
                drpShift.DataBind();
                drpShift.Items.Insert(0, new ListItem("Select Shift", ""));
                drpShift.SelectedValue = lblshift.Text;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
                _logger.Error(string.Concat("GetShift::", ex.Message), ex);
            }
        }
        protected void drpShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpShift.SelectedValue != "")
            {
                string Shift = drpShift.SelectedItem.Text;
                string[] tokens = Shift.Split('/');
                string date = tokens[tokens.Length - 1].Trim();
                //string dateTime = Convert.ToDateTime(date).ToString("yyyy-dd-MM");
                string dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null).ToShortDateString();
                if (drpShift.SelectedValue.Equals("S4"))
                {
                    lblshiftnew.Text = "S1";
                }
                else if (drpShift.SelectedValue.Equals("S5"))
                {
                    lblshiftnew.Text = "S2";
                }
                else if (drpShift.SelectedValue.Equals("S6"))
                {
                    lblshiftnew.Text = "S3";
                }
                else
                {
                    lblshiftnew.Text = drpShift.SelectedValue;
                }
                lblshift.Text = drpShift.SelectedValue;
                lbldate.Text = dateTime.Trim();
                labeldate = dateTime.Trim();
                /// labeldate = dateTime;

                BindProductionEntryGrid();
            }
        }

        protected void btnParameter_Click(object sender, EventArgs e)
        {
            Response.Redirect("ParameterDetails.aspx", false);
        }

        public void GetSensorQty()
        {
            try
            {
                string QueryString = "";
                string Plant = "", LineNo = "", Shift = "", StrDate = "";
                Plant = lblplantcode.Text;
                LineNo = lblline.Text;
                Shift = lblshift.Text;
                StrDate = lbldate.Text.ToString().Trim();
                if (StrDate != null && StrDate != "")
                {
                    _logger.Error(StrDate);
                    StrDate = Convert.ToDateTime(StrDate).ToString("yyyy-MM-dd");
                }

                DateTime ShiftFrom = new DateTime();
                DateTime ShiftTo = new DateTime();
                string PC_Column_01 = "", PC_Column_02 = "", PC_Column_03 = "";

                QueryString = objclsSensorQty.GetShiftTableDataBy(Plant, LineNo, Shift, StrDate);

                List<Dictionary<String, String>> items = new List<Dictionary<String, String>>();
                items = clsAthenaCode.runQuery(QueryString);

                DataTable MainDataTable = new DataTable();
                DataTable NewDataTable = new DataTable();
                objclsSensorQty.AddColumnsAndPullData(MainDataTable, NewDataTable, items);

                ShiftFrom = Convert.ToDateTime(objclsSensorQty.ShiftStartTime.ToString());
                ShiftTo = Convert.ToDateTime(objclsSensorQty.ShiftEndTime.ToString());

                objclsSensorQty.getHoursData(MainDataTable, NewDataTable, ShiftFrom, ShiftTo);

                PC_Column_01 = objclsSensorQty.PC_Column_01.ToString();
                if (PC_Column_01 != "")
                {
                    objclsSensorQty.GetCartonPackingShiftDataWithHoursBy("CartonPacking01", NewDataTable, ds_CartonPacking, PC_Column_01);
                }
                PC_Column_02 = objclsSensorQty.PC_Column_02.ToString();
                if (PC_Column_02 != "")
                {
                    objclsSensorQty.GetCartonPackingShiftDataWithHoursBy("CartonPacking02", NewDataTable, ds_CartonPacking, PC_Column_02);
                }
                PC_Column_03 = objclsSensorQty.PC_Column_03.ToString();
                if (PC_Column_03 != "")
                {
                    objclsSensorQty.GetCartonPackingShiftDataWithHoursBy("CartonPacking03", NewDataTable, ds_CartonPacking, PC_Column_03);
                }
            }catch(Exception ex) { }
        }
    }
}