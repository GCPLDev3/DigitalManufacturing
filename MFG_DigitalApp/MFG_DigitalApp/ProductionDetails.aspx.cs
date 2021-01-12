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

namespace MFG_DigitalApp
{
    public partial class ProductionDetails : System.Web.UI.Page
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
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "9"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@Date", lbldate.Text),
                    new SqlParameter("@PONumber", ""),
                };
                DataSet Dt = new DataSet();
                Dt = DBClass.GetDataSet_WithParam(param, "DM_SP_GET_DATA");

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
                            GrdProdEntry1.DataSource = dt1;
                            GrdProdEntry1.DataBind();

                            //Calculate Sum and display in Footer Row
                            Int32 totalSensorQty = dt1.AsEnumerable().Sum(r => r.Field<Int32>("SensorQty"));
                            Int32 totalActualQty = dt1.AsEnumerable().Sum(r => r.Field<Int32>("ActualQty"));
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

                        }
                        if (count == 2)
                        {
                            DivHeaderEntry2.Visible = true;
                            DivGridEntry2.Visible = true;
                            lblWRP2.Text = Convert.ToString(row["WRPLine"]);
                            DataTable dt2 = new DataTable();
                            dt2 = dtEntry.Select("WRPLine = '" + lblWRP2.Text + "'", "ID asc").CopyToDataTable();
                            GrdProdEntry2.DataSource = dt2;
                            GrdProdEntry2.DataBind();

                            //Calculate Sum and display in Footer Row
                            Int32 totalSensorQty = dt2.AsEnumerable().Sum(r => r.Field<Int32>("SensorQty"));
                            Int32 totalActualQty = dt2.AsEnumerable().Sum(r => r.Field<Int32>("ActualQty"));
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

                        }
                        if (count == 3)
                        {
                            DivHeaderEntry3.Visible = true;
                            DivGridEntry3.Visible = true;
                            lblWRP3.Text = Convert.ToString(row["WRPLine"]);
                            DataTable dt3 = new DataTable();
                            dt3 = dtEntry.Select("WRPLine = '" + lblWRP3.Text + "'", "ID asc").CopyToDataTable();
                            GrdProdEntry3.DataSource = dt3;
                            GrdProdEntry3.DataBind();

                            //Calculate Sum and display in Footer Row
                            Int32 totalSensorQty = dt3.AsEnumerable().Sum(r => r.Field<Int32>("SensorQty"));
                            Int32 totalActualQty = dt3.AsEnumerable().Sum(r => r.Field<Int32>("ActualQty"));
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
                        }

                        count = count + 1;
                    }
                }
            }
            catch (Exception ex)
            {
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('" + str + "')", true);
                    return;
                }
                // Save process start
                foreach (GridViewRow row in GrdProdEntry1.Rows)
                {
                    try
                    {
                        TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                        Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                        DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                        string Time = (row.Cells[1].Text);
                        string PONumber = (row.Cells[2].Text.Replace("&nbsp;", ""));
                        Int32 result;
                        SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@OPR", 5),
                            new SqlParameter("@PlantCode", lblplantcode.Text),
                            new SqlParameter("@Line", lblline.Text),
                            new SqlParameter("@Date", lbldate.Text),
                            new SqlParameter("@ShiftCode", lblshift.Text),
                            new SqlParameter("@WRPLine", lblWRP1.Text), // WRP
                            new SqlParameter("@StartTime", Time.Split(' ')[0]),
                            new SqlParameter("@EndTime",Time.Split(' ')[2]),
                            new SqlParameter("@PONumber", PONumber),
                            new SqlParameter("@ActualQty",ActualQty),
                            new SqlParameter("@ReasonCode",drpReasons.SelectedValue),
                            new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                        };

                        result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");

                    }
                    catch (Exception ex)
                    {
                        DataSaved = false;
                        _logger.Error(string.Concat("GrdProdEntry1::", ex.Message), ex);
                    }
                }


                foreach (GridViewRow row in GrdProdEntry2.Rows)
                {
                    try
                    {
                        TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                        Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                        DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                        string Time = (row.Cells[1].Text);
                        string PONumber = (row.Cells[2].Text.Replace("&nbsp;", ""));
                        Int32 result;
                        SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@OPR", 5),
                            new SqlParameter("@PlantCode", lblplantcode.Text),
                            new SqlParameter("@Line", lblline.Text),
                            new SqlParameter("@Date", lbldate.Text),
                            new SqlParameter("@ShiftCode", lblshift.Text),
                            new SqlParameter("@WRPLine", lblWRP2.Text), // WRP
                            new SqlParameter("@StartTime", Time.Split(' ')[0]),
                            new SqlParameter("@EndTime",Time.Split(' ')[2]),
                            new SqlParameter("@PONumber", PONumber),
                            new SqlParameter("@ActualQty",ActualQty),
                            new SqlParameter("@ReasonCode",drpReasons.SelectedValue),
                            new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                        };

                        result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
                    }
                    catch (Exception ex)
                    {
                        DataSaved = false;
                        _logger.Error(string.Concat("GrdProdEntry2::", ex.Message), ex);
                    }
                }

                foreach (GridViewRow row in GrdProdEntry3.Rows)
                {
                    try
                    {
                        TextBox txtActualQty = (TextBox)row.FindControl("txtActualQty");
                        Int32 ActualQty = Convert.ToInt32(txtActualQty.Text.Trim() == "" ? "0" : txtActualQty.Text.Trim());
                        DropDownList drpReasons = (DropDownList)row.FindControl("drpReasons");
                        string Time = (row.Cells[1].Text);
                        string PONumber = (row.Cells[2].Text.Replace("&nbsp;", ""));
                        Int32 result;
                        SqlParameter[] param = new SqlParameter[]
                        {
                            new SqlParameter("@OPR", 5),
                            new SqlParameter("@PlantCode", lblplantcode.Text),
                            new SqlParameter("@Line", lblline.Text),
                            new SqlParameter("@Date", lbldate.Text),
                            new SqlParameter("@ShiftCode", lblshift.Text),
                            new SqlParameter("@WRPLine", lblWRP3.Text), // WRP
                            new SqlParameter("@StartTime", Time.Split(' ')[0]),
                            new SqlParameter("@EndTime",Time.Split(' ')[2]),
                            new SqlParameter("@PONumber", PONumber),
                            new SqlParameter("@ActualQty",ActualQty),
                            new SqlParameter("@ReasonCode",drpReasons.SelectedValue),
                            new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                        };
                        result = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
                        
                    }
                    catch (Exception ex)
                    {
                        DataSaved = false;
                        _logger.Error(string.Concat("GrdProdEntry2::", ex.Message), ex);
                    }
                }

                if (DataSaved)
                {
                    BindProductionEntryGrid();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Production Details updated Successfully.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Error, Something went Wrong!')", true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("btnSaveProdEntry_Click::", ex.Message), ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Error, Something went Wrong!')", true);
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
                    new SqlParameter("@Line", lblline.Text),
                };
                var output = DBClass.ExecuteScalar_WithParam(param, "DM_SP_GET_DATA");
                result = Convert.ToBoolean(output);
            }
            catch (Exception ex)
            {
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
                        new SqlParameter("@OPR", "11"),
                        new SqlParameter("@PlantCode", lblplantcode.Text)
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

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "13"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", lbldate.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    txtWIP.Text = Convert.ToString(Dt.Rows[0]["WIP"]);
                    txtOffLoadingsNos.Text = Convert.ToString(Dt.Rows[0]["OffLoadingsNos"]);
                    txtQualityRejection.Text = Convert.ToString(Dt.Rows[0]["QualityRejection"]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetWIPDetails::", ex.Message), ex);
            }
        }

        public void UpdateWIPDetails()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "7"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", lbldate.Text),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                    new SqlParameter("@WIP", txtWIP.Text.ToString().Trim() == ""?"0":txtWIP.Text.ToString().Trim()),
                    new SqlParameter("@OffLoadingsNos", txtOffLoadingsNos.Text.Trim().ToString() == ""?"0":txtOffLoadingsNos.Text.Trim().ToString()),
                    new SqlParameter("@QualityRejection", txtQualityRejection.Text.Trim().ToString() == ""?"0":txtQualityRejection.Text.Trim().ToString()),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                };
                DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("UpdateWIPDetails::", ex.Message), ex);
            }
        }

        protected void GetShift()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "1")
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
                drpShift.DataSource = Dt;
                drpShift.DataTextField = "ShiftName";
                drpShift.DataValueField = "ShiftCode";
                drpShift.DataBind();
                drpShift.Items.Insert(0, new ListItem("Select Shift", ""));
                drpShift.SelectedValue = lblshift.Text;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetShift::", ex.Message), ex);
            }
        }
        protected void drpShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpShift.SelectedValue != "")
            {
                string Shift = drpShift.SelectedItem.Text;
                string[] tokens = Shift.Split('/');
                string date = tokens[tokens.Length - 1];

                lblshift.Text = drpShift.SelectedValue;
                lbldate.Text = date;

                BindProductionEntryGrid();
            }
        }

        protected void btnParameter_Click(object sender, EventArgs e)
        {
            Response.Redirect("ParameterDetails.aspx", false);
        }
    }
}