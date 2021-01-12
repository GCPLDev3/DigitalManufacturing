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
    public partial class QI_CreateSchedule : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("CreateSchedule");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetBrand();
                GetPlant();
                GetGrammage();
                GetScheduleType();
                GetSeverity();
                GetUOM();
                GetDuration();
                //GetShiftHeader();
                if (Session["ScheduleId"] != null && Convert.ToInt32(Session["ScheduleId"]) > 0)
                {
                    LoadData();
                    GetParameter();
                }
                QualitativeGroup.Visible = false;
                QuantitativeGroup.Visible = false;
            }

        }

        protected void LoadData()
        {
            try
            {
                int scheduleId = Convert.ToInt16(Session["ScheduleId"]);
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "31"),
                new SqlParameter("@ScheduleId",scheduleId)
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    txtScheduleNumber.Text = Dt.Rows[0]["ScheduleNumber"].ToString().Trim();
                    txtScheduleNumber.ReadOnly = true;
                    drpBrand.SelectedIndex = Int16.Parse(Dt.Rows[0]["BrandId"].ToString());
                    if (drpBrand.SelectedIndex != -1)
                        GetVariant();
                    drpVariant.SelectedIndex = Int16.Parse(Dt.Rows[0]["VariantId"].ToString());
                    drpScheduleType.SelectedIndex = Int16.Parse(Dt.Rows[0]["ScheduleTypeId"].ToString());
                    drpPlant.SelectedValue = Dt.Rows[0]["PlantCode"].ToString().Trim();
                    if (drpPlant.SelectedIndex != -1)
                        GetStation();
                    txtScheduleDesc.Text = Dt.Rows[0]["ScheduleDescription"].ToString().Trim();
                    drpGrammage.SelectedIndex = Int16.Parse(Dt.Rows[0]["GrammageId"].ToString().Trim());
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetUOM::", ex.Message), ex);
            }
        }

        #region Get Brand details
        protected void GetBrand()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "23")
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");

                drpBrand.DataSource = Dt;
                drpBrand.DataTextField = "BrandName";
                drpBrand.DataValueField = "Id";
                drpBrand.DataBind();
                drpBrand.Items.Insert(0, new ListItem("Select Brand", "0"));
                //drpBrand.SelectedValue = Convert.ToString(Dt.Rows[0]["BrandName"]);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetBrand::", ex.Message), ex);
            }
        }
        #endregion
        #region Get plantdetails of User
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
                drpPlant.DataSource = Dt;
                drpPlant.DataTextField = "PlantName";
                drpPlant.DataValueField = "PlantCode";
                drpPlant.DataBind();
                //drpPlant.Items.Insert(0, new ListItem("Select plant"));
                drpPlant.SelectedValue = Convert.ToString(Dt.Rows[0]["PlantName"]);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetPlant::", ex.Message), ex);
            }
        }
        #endregion

        //protected void GetShiftHeader()
        //{
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[]
        //        {
        //            new SqlParameter("@OPR", "10")
        //        };
        //        DataTable Dt = new DataTable();
        //        Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
        //        drpShiftHeader.DataSource = Dt;
        //        drpShiftHeader.DataTextField = "ShiftName";
        //        drpShiftHeader.DataValueField = "ShiftCode";
        //        drpShiftHeader.DataBind();
        //        drpShiftHeader.Items.Insert(0, new ListItem("Select Shift", ""));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(string.Concat("GetShiftHeader::", ex.Message), ex);
        //    }
        //}

        #region Get Variantdetails of brand
        protected void GetVariant()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "24"),
                new SqlParameter("@BrandId",drpBrand.SelectedValue),
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpVariant.DataSource = Dt;
                drpVariant.DataTextField = "VariantName";
                drpVariant.DataValueField = "Id";
                drpVariant.DataBind();
                drpVariant.Items.Insert(0, new ListItem("Select Variant", "0"));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetVariant::", ex.Message), ex);
            }
        }
        #endregion

        #region Get Grammagedetails
        protected void GetGrammage()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "25"),
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpGrammage.DataSource = Dt;
                drpGrammage.DataTextField = "Grammage";
                drpGrammage.DataValueField = "Id";
                drpGrammage.DataBind();
                drpGrammage.Items.Insert(0, new ListItem("Select Grammage", "0"));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetGrammage::", ex.Message), ex);
            }
        }
        #endregion

        #region Get Schedule Type
        protected void GetScheduleType()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "26"),
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpScheduleType.DataSource = Dt;
                drpScheduleType.DataTextField = "ScheduleTypeName";
                drpScheduleType.DataValueField = "Id";
                drpScheduleType.DataBind();
                drpScheduleType.Items.Insert(0, new ListItem("Select Schedule Type", "0"));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetScheduleType::", ex.Message), ex);
            }
        }
        #endregion

        #region Get Parameter
        protected void GetParameter()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "27"),
                new SqlParameter("@ScheduleId",Int32.Parse(Session["ScheduleId"].ToString()))
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

        protected void LoadStation(Object s, EventArgs e)
        {
            if (!string.IsNullOrEmpty(drpPlant.SelectedValue))
                GetStation();
        }
        #region Get Station
        protected void GetStation()
        {
            try
            {

                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "7"),
                new SqlParameter("@PlantCode", drpPlant.Text),
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtScheduleNumber.Text) || (GrdAddedParameterList != null && GrdAddedParameterList.Rows.Count > 0))
                Response.Redirect("QI_Schedules.aspx", false);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please add parameters to schedule')", true);
        }
        protected void LoadVariant(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(drpBrand.SelectedValue))
                GetVariant();
        }
        protected void clear_All()
        {
            drpBrand.SelectedIndex = 0;
            txtScheduleNumber.Text = string.Empty;
            drpPlant.SelectedIndex = 0;
            drpVariant.SelectedIndex = 0;
            drpGrammage.SelectedIndex = 0;
            drpScheduleType.SelectedIndex = 0;
            txtScheduleDesc.Text = string.Empty;
            drpUOM.SelectedIndex = 0;
            drpQUOM.SelectedIndex = 0;
            txtNoOfSamples.Text = string.Empty;
            drpParameter.SelectedIndex = 0;
            drpStation.SelectedIndex = 0;
            drpSeverity.SelectedIndex = 0;
            txtLCL.Text = string.Empty;
            txtUCL.Text = string.Empty;
            ddlHour.SelectedIndex = 0;
            ddlMinute.SelectedIndex = 0;
        }
        protected void clear_All_Parametes()
        {
            //drpBrand.SelectedIndex = 0;
            //txtScheduleNumber.Text = string.Empty;
            //drpPlant.SelectedIndex = 0;
            //drpVariant.SelectedIndex = 0;
            //drpGrammage.SelectedIndex = 0;
            //IdDrpParaGrmge.SelectedIndex = 0;
            //drpScheduleType.SelectedIndex = 0;
            //txtScheduleDesc.Text = string.Empty;
            drpUOM.SelectedIndex = 0;
            drpQUOM.SelectedIndex = 0;
            txtNoOfSamples.Text = string.Empty;
            drpParameter.SelectedIndex = 0;
            drpStation.SelectedIndex = 0;
            drpSeverity.SelectedIndex = 0;
            txtLCL.Text = string.Empty;
            txtUCL.Text = string.Empty;
            ddlHour.SelectedIndex = 0;
            ddlMinute.SelectedIndex = 0;
            //IdDrpParaGrmge.SelectedIndex = 0;
        }
        protected void btnCreateSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                object obj;
                System.Threading.Thread.Sleep(2000);
                List<SqlParameter> saveSchedule = new List<SqlParameter>
                {
                    new SqlParameter("@OPR", "13"),
                    new SqlParameter("@BrandId",drpBrand.SelectedValue),
                    new SqlParameter("@PlantCode",drpPlant.SelectedValue),
                    new SqlParameter("@VariantId",drpVariant.SelectedValue),
                    new SqlParameter("@GrammageId",drpGrammage.SelectedValue),
                    new SqlParameter("@ScheduleTypeId",drpScheduleType.SelectedValue),
                    new SqlParameter("@ScheduleDescription", txtScheduleDesc.Text.Trim()),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"])),
                };
                if (Session["CScheduleId"] != null && Convert.ToInt16(Session["CScheduleId"]) > 0)
                    saveSchedule.Add(new SqlParameter("@CScheduleId", Convert.ToInt16(Session["CScheduleId"].ToString())));

                obj = DBClass.ExecuteNonQuery_WithParam(saveSchedule.ToArray(), "DM_SP_DML_DATA");



                if (obj != null && Convert.ToInt64(obj) != 0)
                {
                    Session["ScheduleId"] = Convert.ToInt64(obj);
                    BindGrid();
                    txtScheduleNumber.Text = GetScheduleNumber(Convert.ToInt16(obj));
                    btnSubmit.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Schedule created successfully')", true);
                    GetParameter();
                    GetStation();
                }
                else
                {
                    clear_All();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("btnSubmit_Click::", ex.Message), ex);
            }
        }

        public string GetScheduleNumber(int Id)
        {
            string ScheduleNumber = string.Empty;
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "31"),
                    new SqlParameter("@ScheduleId", Id),
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    ScheduleNumber = Dt.Rows[0]["ScheduleNumber"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetIsIOTLine::", ex.Message), ex);
            }
            return ScheduleNumber;
        }

        protected void btnAddParam_Click(object sender, EventArgs e)
        {
            try
            {
                decimal UclValue = String.IsNullOrEmpty(txtUCL.Text.ToString()) ? 0M : Convert.ToDecimal(txtUCL.Text.ToString());
                decimal LclValue = String.IsNullOrEmpty(txtLCL.Text.ToString()) ? 0M : Convert.ToDecimal(txtLCL.Text.ToString());
                object obj;
                System.Threading.Thread.Sleep(2000);
                TimeSpan time = new TimeSpan(Int16.Parse(ddlHour.SelectedValue), Int16.Parse(ddlMinute.SelectedValue), 0);
                if (Session["ScheduleId"] == null && Convert.ToInt16(Session["ScheduleId"]) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please create schedule')", true);
                }
                //else if (UclValue <= LclValue && QualitativeGroup.Visible)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('LCL should be lesser than UCL')", true);
                //    // MPAddParameter.Show();
                //}
                else if (ddlHour.SelectedValue.Equals("00") && ddlMinute.SelectedValue.Equals("00"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Repeat time cannot be 00,Please select again hour/Minute')", true);
                }
               
                else
                {
                    List<SqlParameter> saveParam = new List<SqlParameter>
                {
                    new SqlParameter("@OPR", "14"),
                    new SqlParameter("@ScheduleId",Int32.Parse(Session["ScheduleId"].ToString())),
                    new SqlParameter("@ParameterId",drpParameter.SelectedValue),
                    new SqlParameter("@StationCode",drpStation.SelectedValue),
                    new SqlParameter("@SeverityId",drpSeverity.SelectedValue),
                    new SqlParameter("@NoOfSamplesId",txtNoOfSamples.Text.Trim()),
                    new SqlParameter("@UOMId",drpUOM.SelectedValue),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"])),
                     new SqlParameter("@OnTimeParameterActive","0")
                };
                    SqlParameter repeatParameter = new SqlParameter("@Repeat", SqlDbType.Time);
                    repeatParameter.Value = time;
                    saveParam.Add(repeatParameter);
                    if (!string.IsNullOrEmpty(txtEResult.Text))
                        saveParam.Add(new SqlParameter("@EResult", txtEResult.Text));
                    else if (!string.IsNullOrEmpty(txtLCL.Text) && !string.IsNullOrEmpty(txtUCL.Text) && !string.IsNullOrEmpty(drpUOM.Text))
                    {
                        saveParam.Add(new SqlParameter("@LCL", txtLCL.Text));
                        saveParam.Add(new SqlParameter("@UCL", txtUCL.Text));
                        saveParam.Add(new SqlParameter("@UOM", drpUOM.Text));
                    }
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please enter either qualitative or quantitative parameter')", true);
                    //}
                    var x = Int32.Parse(Session["ScheduleId"].ToString());
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
                    else
                    { 
                    obj = DBClass.ExecuteNonQuery_WithParam(saveParam.ToArray(), "DM_SP_DML_DATA");
                        if (obj != null && Convert.ToInt64(obj) != 0)
                        {
                            BindGrid();
                            GetParameter();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter added successfully')", true);
                            clear_All_Parametes();
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("btnSubmit_Click::", ex.Message), ex);
            }
        }

        protected void BindGrid()
        {
            try
            {
                int sessionId = 0;
                if (Session["ScheduleId"] != null && Convert.ToInt32(Session["ScheduleId"]) != 0)
                {
                    sessionId = Convert.ToInt32(Session["ScheduleId"]);
                }
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "32"),
                new SqlParameter("@ScheduleId", sessionId),
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdAddedParameterList.DataSource = Dt;
                GrdAddedParameterList.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetData::", ex.Message), ex);
            }
        }

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
                        drpQUOM.Text = string.Empty;
                    }
                    else
                    {
                        QualitativeGroup.Visible = false;
                        QuantitativeGroup.Visible = true;
                        txtEResult.Text = string.Empty;
                        GetQUOM();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetParameterType::", ex.Message), ex);
            }
        }
        protected void btnClearParam_Click(Object sender, EventArgs e)
        {
            //drpParameter.SelectedIndex = 0;
            //drpStation.SelectedIndex = 0;
            //drpSeverity.SelectedIndex = 0;
            //txtNoOfSamples.Text = string.Empty;
            //drpUOM.SelectedIndex = 0;
            //ddlHour.SelectedIndex = 0;
            //ddlMinute.SelectedIndex = 0;
            //txtLCL.Text = string.Empty;
            //txtUCL.Text = string.Empty;
            //drpQUOM.Text = string.Empty;
            //txtEResult.Text = string.Empty;
            //drpBrand.SelectedIndex = 0;
            // txtScheduleNumber.Text = string.Empty;
            //drpPlant.SelectedIndex = 0;
            // drpVariant.SelectedIndex = 0;
            //drpGrammage.SelectedIndex = 0;
            //IdDrpParaGrmge.SelectedIndex = 0;
            // drpScheduleType.SelectedIndex = 0;
            //txtScheduleDesc.Text = string.Empty;
            drpUOM.SelectedIndex = 0;
            if (drpQUOM.Visible)
            {
                drpQUOM.SelectedIndex = 0;
            }

            txtNoOfSamples.Text = string.Empty;
            drpParameter.SelectedIndex = 0;
            drpStation.SelectedIndex = 0;
            drpSeverity.SelectedIndex = 0;
            txtLCL.Text = string.Empty;
            txtUCL.Text = string.Empty;
            ddlHour.SelectedIndex = 0;
            ddlMinute.SelectedIndex = 0;
        }

        protected void GrdAddedParameterList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                foreach (Button button in e.Row.Cells[10].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete this record?')){ return false; };";
                    }
                }
            }

        }

        protected void GrdAddedParameterList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int64 AutoId = Convert.ToInt32(GrdAddedParameterList.DataKeys[e.RowIndex].Value.ToString());
            int ParameterId = Convert.ToInt32(GrdAddedParameterList.DataKeys[e.RowIndex].Values[1].ToString());
            //int ParameterId = Convert.ToInt32(GrdAddedParameterList.Rows[e.RowIndex].FindControl("ParameterId"));
            int ScheduleId = Convert.ToInt32(Session["ScheduleId"]);
            object obj;
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", 6),
                new SqlParameter("@ScheduleId",ScheduleId),
                new SqlParameter("@ParameterId", ParameterId),
               // new SqlParameter("@AutoId", AutoId),
            };

            obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
            BindGrid();
            GetParameter();
            if (obj != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter deleted successfully')", true);
            }
        }

    }
}