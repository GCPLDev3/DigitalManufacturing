using System;
using MFG_DigitalApp.Log;
using MFG_DigitalApp.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MFG_DigitalApp
{
    public partial class ShiftDetails : System.Web.UI.Page
    {

        private static readonly ILogger _logger = Logger.GetLogger("RunDetails");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        string ShiftDate = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //var dat = Request.Cookies["ClientDateTime"].Value;
            //var Date = hdnfldVariable.Value;
            var Date = Session["Currenttime"];
            
            if (Session["CountryId"] != null && Session["CountryId"].Equals("2"))
            {
                string dateTime = DateTime.ParseExact(Date.ToString().Substring(0, Date.ToString().IndexOf(' ')).Trim().Replace("-", "/"), "dd/MM/yyyy", null).ToString("dd-MM-yyyy");
                ShiftDate = dateTime;
            }
            else if (Session["CountryId"] != null && Session["CountryId"].Equals("1"))
            {
                ShiftDate = DateTime.Now.ToString("dd-MM-yyyy");
            }
            if (!IsPostBack)
            {

                GetPlant();
                GetShift(ShiftDate);
                GetData();
            }
        }
        #region Get Shift details with current shift
        protected void GetShift(string Date)
        {
            try
            {
                var PlantCode = drpPlant.SelectedValue;
                var currentTime = "";
                if (Date == "")
                {
                    currentTime = "";
                }
                else
                {
                    //var date = Date.ToString().Substring(0, Date.IndexOf(' '));
                    DateTime datetime = DateTime.ParseExact(Date.Replace("-", "/"), "dd/MM/yyyy", null);
                    var FormatedDate = datetime.ToString("yyyy-MM-dd");
                    currentTime = FormatedDate;
                }
                var x = HttpContext.Current.Session["USERID"].ToString();
                var y = Session["CountryId"];
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "1"),
                new SqlParameter("@PlantCode", PlantCode),
                new SqlParameter("@Countryid", Session["CountryId"]),
                new SqlParameter("@currentTime", currentTime),
                new SqlParameter("@UserId", HttpContext.Current.Session["USERID"].ToString()),

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
                drpShift.SelectedValue = Convert.ToString(Dt.Rows[0]["CurShiftCode"]);


            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetShift::", ex.Message), ex);
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
        protected void drpShift_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GetData();
        }

        protected void drpPlant_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var Date = hdnfldVariable.Value;
            GetShift(ShiftDate);
            GetData();
        }
        protected void GetData()
        {
            try
            {
                UserSelectionModel model = new UserSelectionModel();
                model = (UserSelectionModel)Session["UserSelectionModel"];
                var x = Session["UserID"];
                string Shift = drpShift.SelectedItem.Text;
                string[] tokens = Shift.Split('/');
                string date = tokens[tokens.Length - 1];
                string dateTime = DateTime.ParseExact(date.Trim().Replace("-", "/"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                //DateTime datetime = DateTime.Parse(date);
                //var FormatedDate = datetime.ToString("yyyy-MM-dd");
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "3"),
                new SqlParameter("@PlantCode",drpPlant.SelectedValue),
                new SqlParameter("@Shift", drpShift.SelectedValue),
                new SqlParameter("@Date", dateTime),
                new SqlParameter("@UserId", HttpContext.Current.Session["USERID"].ToString()),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
                GrdStoppageReason.DataSource = Dt;
                GrdStoppageReason.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetData::", ex.Message), ex);
            }
        }

        protected void lnkLine_Click(object sender, EventArgs e)
        {
            var Date = hdnfldVariable.Value;
            UserSelectionModel modeldata = new UserSelectionModel();
            modeldata = (UserSelectionModel)Session["UserSelectionModel"];
            LinkButton lnkLinee = (LinkButton)sender;
            string PlantCode = drpPlant.SelectedValue;
            string Shift = drpShift.SelectedItem.Text;
            string[] tokens = Date.Split(' ');
            string date = tokens[0];
            UserSelectionModel model = new UserSelectionModel();
            model.PlantCode = PlantCode;
            model.Line = lnkLinee.Text;
            model.ShiftCode = drpShift.SelectedValue;
            model.ShiftDate = Date;
            Session["UserSelectionModel"] = model;
            if (GetAssignedOperatorType() == 0)
            {
                Response.Redirect("QualityCheck.aspx");
            }
            else
            {
                Response.Redirect("RunDetails.aspx");
            }
        }

        #region Get AO Type
        protected int GetAssignedOperatorType()
        {
            int assignedOperatorType = 1;
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "9"),
                 new SqlParameter("@UserId", HttpContext.Current.Session["USERID"].ToString())
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    Session["RoleId"] = Dt.Rows[0]["RoleId"].ToString();
                    if (Convert.ToInt16(Dt.Rows[0]["RoleId"].ToString()) == 1)
                    {
                        assignedOperatorType = 1;
                    }

                    else
                    {
                        try
                        {
                            SqlParameter[] param1 = new SqlParameter[]
                            {
                                new SqlParameter("@OPR", "10"),
                                new SqlParameter("@UserId", HttpContext.Current.Session["USERID"].ToString())
                            };
                            DataTable Dt1 = new DataTable();
                            Dt1 = DBClass.GetDatatable_WithParam(param1, "DM_SP_GET_STOPPAGE_DATA");
                            if (Dt1 != null && Dt1.Rows.Count > 0)
                            {
                                if (string.Compare(Dt1.Rows[0]["OperatorType"].ToString(), "QC Supervisor") == 0)
                                {
                                    assignedOperatorType = 0;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(string.Concat("GetPlant::", ex.Message), ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetPlant::", ex.Message), ex);
            }
            return assignedOperatorType;
        }
        #endregion

        protected void GrdStoppageReason_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(drpPlant.SelectedValue=="M016")
            {
                GrdStoppageReason.Columns[6].Visible = true;
                GrdStoppageReason.Columns[7].Visible = false;
            }
            else if (drpPlant.SelectedValue == "M024")
            {
                GrdStoppageReason.Columns[6].Visible = false;
                GrdStoppageReason.Columns[7].Visible = true;
            }
            else
            {
                GrdStoppageReason.Columns[6].Visible = false;
                GrdStoppageReason.Columns[7].Visible = false;
            }
        }
    }
}