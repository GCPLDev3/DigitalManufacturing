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

namespace MFG_DigitalApp
{
    public partial class ParameterDetails : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("ParameterDetails");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
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
                    //lbldate.Text = model.ShiftDate;
                    txtComments.Attributes.Add("maxlength", txtComments.MaxLength.ToString());

                    txtDate.Text = Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd");//lbldate.Text;
                    GetShiftHeader();
                    GetShift();
                    GetDuration();
                    BindParameterType();
                    BindGrid();
                    GetStation();
                }
            }
        }

        protected void GetStation()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "16"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text.Trim())
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            ddlStation.DataSource = Dt;
            ddlStation.DataTextField = "StationDescription";
            ddlStation.DataValueField = "StationCode";
            ddlStation.DataBind();
            ddlStation.Items.Insert(0, new ListItem("Select Station", "0"));
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
                new SqlParameter("@OPR", "1"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Countryid", Session["CountryId"]),
                new SqlParameter("@currentTime", lbldate.Text),
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
                drpShiftHeader.DataSource = Dt;
                drpShiftHeader.DataTextField = "ShiftName";
                drpShiftHeader.DataValueField = "ShiftCode";
                drpShiftHeader.DataBind();
                drpShiftHeader.Items.Insert(0, new ListItem("Select Shift", ""));
                drpShiftHeader.SelectedValue = lblshift.Text;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetShift::", ex.Message), ex);
            }
        }

        protected void drpShiftHeader_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpShiftHeader.SelectedValue != "")
            {
                string Shift = drpShiftHeader.SelectedItem.Text;
                string[] tokens = Shift.Split('/');
                string date = tokens[tokens.Length - 1];
                date = DateTime.ParseExact(date.Trim(), "dd-MM-yyyy", null).ToString("yyyy-MM-dd");
                if (drpShiftHeader.SelectedValue.Equals("S4"))
                {
                    lblshiftnew.Text = "S1";
                }
                else if (drpShiftHeader.SelectedValue.Equals("S5"))
                {
                    lblshiftnew.Text = "S2";
                }
                else if (drpShiftHeader.SelectedValue.Equals("S6"))
                {
                    lblshiftnew.Text = "S3";
                }
                else
                {
                    lblshiftnew.Text = drpShiftHeader.SelectedValue;
                }
                lblshift.Text = drpShiftHeader.SelectedValue;
                lbldate.Text = date;
                txtDate.Text = lbldate.Text;
                GetShift();
                GetDuration();
                BindParameterType();
                BindGrid();
            }
        }

        protected void GetShift()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "13"),
                new SqlParameter("@PlantCode",lblplantcode.Text)
            };
            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            drpShift.DataSource = Dt;
            drpShift.DataTextField = "ShiftName";
            drpShift.DataValueField = "ShiftCode";
            drpShift.DataBind();
            drpShift.Items.Insert(0, new ListItem("Select", "0"));
            drpShift.SelectedValue = Convert.ToString(lblshift.Text);
        }
        protected void GetDuration()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                //new SqlParameter("@OPR", "18")
                new SqlParameter("@OPR", "2"),
                new SqlParameter("@PlantCode", lblplantcode.Text.Trim())
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_TIME");
                ddlHour.DataSource = Dt;
                ddlHour.DataTextField = "Hours";
                ddlHour.DataValueField = "Hours";
                ddlHour.DataBind();
                ddlHour.Items.Insert(0, new ListItem("Select", ""));
                ddlHour.SelectedValue = Convert.ToString(Dt.Rows[0]["CurrentHour"]);


                SqlParameter[] param1 = new SqlParameter[]
                {
                //new SqlParameter("@OPR", "19")
                new SqlParameter("@OPR", "3"),
                new SqlParameter("@PlantCode", lblplantcode.Text.Trim())
                };
                DataTable Dtt = new DataTable();
                Dtt = DBClass.GetDatatable_WithParam(param1, "DM_SP_GET_TIME");
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
        protected void BindParameterType()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "20"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text)
            };
            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            drpParameterType.DataSource = Dt;
            drpParameterType.DataTextField = "ParameterType";
            drpParameterType.DataValueField = "ParameterType";
            drpParameterType.DataBind();
            drpParameterType.Items.Insert(0, new ListItem("Select", ""));
        }

        protected void drpParameterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindParameters();
        }

        protected void BindParameters()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "16"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line", lblline.Text),
                new SqlParameter("@ParameterType", drpParameterType.SelectedValue),
            };
            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            drpParameter.DataSource = Dt;
            drpParameter.DataTextField = "Parameter";
            drpParameter.DataValueField = "Parameter";
            drpParameter.DataBind();
            drpParameter.Items.Insert(0, new ListItem("Select", ""));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try

            {
                if (ddlHour.SelectedValue.Equals("00") && ddlMinute.SelectedValue.Equals("00"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Time cannot be 00,Please select again Time hour/Minute')", true);
                }
                else
                {
                    UserSelectionModel model = new UserSelectionModel();
                    model = (UserSelectionModel)Session["UserSelectionModel"];
                    var date = model.ShiftDate.ToString().Substring(0, model.ShiftDate.IndexOf(' '));
                    DateTime dateTime = DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                    DateTime dateTime1 = DateTime.Parse(lbldate.Text);
                    object obj;
                    SqlParameter[] param = new SqlParameter[]
                    {
                    new SqlParameter("@OPR", 10),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line",lblline.Text),
                    new SqlParameter("@Date", dateTime1),
                    new SqlParameter("@ShiftCode",drpShift.SelectedValue),
                    new SqlParameter("@ParameterTime",ddlHour.SelectedItem.Text +":"+ddlMinute.SelectedItem.Text),
                    new SqlParameter("@ParameterType",drpParameterType.SelectedValue),
                    new SqlParameter("@ParameterDesc",drpParameter.SelectedValue),
                    new SqlParameter("@Value",txtValue.Text.Trim()),
                    new SqlParameter("@Targ",txtTargValue.Text.Trim()),
                    new SqlParameter("@UOM",txtUOM.Text.Trim()),
                    new SqlParameter("@Comments",txtComments.Text.Trim()),
                    new SqlParameter("@CreatedBy",HttpContext.Current.Session["username"].ToString()),
                    new SqlParameter("@StationCode",ddlStation.SelectedValue)
                    };

                    obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");

                    BindGrid();

                    if (obj != null && Convert.ToInt64(obj) != 0)
                    {
                        ClearFields();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter added successfully')", true);
                    }
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("btnSubmit_Click::", ex.Message), ex);
            }
        }

        protected void ClearFields()
        {
            drpParameterType.SelectedIndex = 0;
            drpParameter.SelectedIndex = 0;
            txtValue.Text = string.Empty;
            txtTargValue.Text = string.Empty;
            txtUOM.Text = string.Empty;
            txtComments.Text = string.Empty;
            ddlStation.SelectedIndex = 0;
        }

        protected void BindGrid()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "2"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@Line",lblline.Text),
                new SqlParameter("@Date",Convert.ToDateTime(txtDate.Text.Trim()).ToString("yyyy-MM-dd")),
                new SqlParameter("@ShiftCode",drpShift.SelectedValue)
                };
                DataSet Ds = new DataSet();
                Ds = DBClass.GetDataSet_WithParam(param, "SP_GET_DATA");

                GrdProduction.DataSource = Ds.Tables[0];
                GrdProduction.DataBind();

                GrdQuality.DataSource = Ds.Tables[1];
                GrdQuality.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindGrid::", ex.Message), ex);
            }
        }

        protected void LinkPrdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton LinkDelete = (LinkButton)sender;
                var deleteID = LinkDelete.CommandArgument;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "11"),
                    new SqlParameter("@DeleteID", deleteID)
                };
                DataTable Dt = new DataTable();
                DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
                BindGrid();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Deleted Successfully.')", true);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("LinkPrdDelete_Click::", ex.Message), ex);
            }
        }

        protected void LinkQlyDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton LinkDelete = (LinkButton)sender;
                var deleteID = LinkDelete.CommandArgument;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "11"),
                    new SqlParameter("@DeleteID", deleteID)
                };
                DataTable Dt = new DataTable();
                DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");
                BindGrid();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Deleted Successfully.')", true);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("LinkQlyDelete_Click::", ex.Message), ex);
            }
        }

        protected void btnstoppages_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoppagesReason.aspx", false);
        }

        protected void ddlStation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}