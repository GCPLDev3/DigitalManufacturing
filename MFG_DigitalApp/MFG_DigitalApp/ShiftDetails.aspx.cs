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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetShift();
                GetPlant();
                GetData();
            }
        }
        #region Get Shift details with current shift
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
                drpShift.Items.Insert(0, new ListItem("Select Shift", "0"));
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
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "2"),
                new SqlParameter("@UserName", Convert.ToString(HttpContext.Current.Session["username"])),
                 new SqlParameter("@UserId", HttpContext.Current.Session["USERID"].ToString())
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
                drpPlant.DataSource = Dt;
                drpPlant.DataTextField = "PlantName";
                drpPlant.DataValueField = "PlantCode";
                drpPlant.DataBind();
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
            GetData();
        }
        protected void GetData()
        {
            try
            {
                UserSelectionModel model = new UserSelectionModel();
                model = (UserSelectionModel)Session["UserSelectionModel"];
                string Shift = drpShift.SelectedItem.Text;
                string[] tokens = Shift.Split('/');
                string date = tokens[tokens.Length - 1];
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "3"),
                new SqlParameter("@PlantCode",drpPlant.SelectedValue),
                new SqlParameter("@Shift", drpShift.SelectedValue),
                new SqlParameter("@Date", date),
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
            UserSelectionModel modeldata = new UserSelectionModel();
            modeldata = (UserSelectionModel)Session["UserSelectionModel"];
            LinkButton lnkLinee = (LinkButton)sender;
            string PlantCode = drpPlant.SelectedValue;
            string Shift = drpShift.SelectedItem.Text;
            string[] tokens = Shift.Split('/');
            string date = tokens[tokens.Length - 1];
            UserSelectionModel model = new UserSelectionModel();
            model.PlantCode = PlantCode;
            model.Line = lnkLinee.Text;
            model.ShiftCode = drpShift.SelectedValue;
            model.ShiftDate = date.Trim();
            Session["UserSelectionModel"] = model;
            if(GetAssignedOperatorType() == 0)
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

    }
}