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
    public partial class QI_Parameters : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("QI_ParameterDetails");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
                GetParameterTypes();
            }
        }
        protected void GetData()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", 22),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdParameterList.DataSource = Dt;
                GrdParameterList.DataBind();

            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetData::", ex.Message), ex);
            }
        }
        protected void chckchanged(object sender, EventArgs e)
        {
            CheckBox chckheader = (CheckBox)GrdParameterList.HeaderRow.FindControl("CheckAll");
            foreach (GridViewRow row in GrdParameterList.Rows)
            {
                CheckBox chckrw = (CheckBox)row.FindControl("Select");
                if (chckheader.Checked == true)
                {
                    chckrw.Checked = true;
                }
                else
                {
                    chckrw.Checked = false;
                }
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            GrdParameterList.PageIndex = e.NewPageIndex;
            GetData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = QC_Parameter.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);
            GrdParameterList.AllowPaging = false;
            GetData();
            foreach (GridViewRow row in GrdParameterList.Rows)
            {
                (GrdParameterList.Rows[row.RowIndex].Cells[0].FindControl("lnkParameterName") as LinkButton).Enabled = false;
            }

            GrdParameterList.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        protected void lnkParameterName_Click(object sender, EventArgs e)
        {

            int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
            int id = Convert.ToInt32(GrdParameterList.DataKeys[rowIndex].Values[0]);
            SqlParameter[] param = new SqlParameter[]
             {
                new SqlParameter("@OPR", 30),
                new SqlParameter("ParameterId",id)
             };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            if (Dt != null && Dt.Rows.Count > 0)
            {
                GetParameterType();
                MPStartAddParam.Show();
                txtPName.Text = Dt.Rows[0]["ParameterName"].ToString().Trim();
                drpPType.SelectedIndex = Int16.Parse(Dt.Rows[0]["ParameterTypeId"].ToString());
                txtTool.Text = Dt.Rows[0]["ToolsRequired"].ToString().Trim();
                hdnParameterId.Value = Dt.Rows[0]["Id"].ToString();
            }
        }

        protected void btnCreateParameter_Click(object sender, EventArgs e)
        {
            GetParameterType();
            MPStartAddParam.Show();
            ClearFields();
        }
        protected void btnSaveCreateParameter_Click(object sender, EventArgs e)
        {
            try
            {
                object obj;
                List<SqlParameter> saveParam = new List<SqlParameter>
                {
                    new SqlParameter("@OPR", 12),
                    new SqlParameter("@ParameterName", txtPName.Text.Trim()),
                    new SqlParameter("@QI_ParameterType",drpPType.SelectedValue),
                    new SqlParameter("@ToolRequired", txtTool.Text.Trim()),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]).Trim())
                };
                if (!string.IsNullOrEmpty(hdnParameterId.Value))
                    saveParam.Add(new SqlParameter("@ParameterId", hdnParameterId.Value));
                obj = DBClass.ExecuteNonQuery_WithParam(saveParam.ToArray(), "DM_SP_DML_DATA");

                if (obj != null && Convert.ToInt64(obj) != 0)
                {
                    ClearFields();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter added successfully')", true);
                    Response.Redirect("QI_Parameters.aspx", false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("btnSubmit_Click::", ex.Message), ex);
            }
        }
        protected void GetParameterType()
        {
            try
            {

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "21")
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpPType.DataSource = Dt;
                drpPType.DataTextField = "ParameterTypeName";
                drpPType.DataValueField = "Id";
                drpPType.DataBind();
                drpPType.Items.Insert(0, new ListItem("Select Parameter Type", ""));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetParameterType::", ex.Message), ex);
            }
        }
        protected void ClearFields()
        {
            drpPType.SelectedIndex = 0;
            txtTool.Text = string.Empty;
            txtPName.Text = string.Empty;
        }

        protected void GrdParameterList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                foreach (Button button in e.Row.Cells[5].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete this record?')){ return false; };";
                    }
                }
            }

        }

        protected void GrdParameterList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Int64 AutoId = Convert.ToInt32(GrdParameterList.DataKeys[e.RowIndex].Value.ToString());
                object obj;
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", 4),
                new SqlParameter("@AutoId", AutoId),
                };

                obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
                GetData();
                if (obj != null && Convert.ToInt16(obj.ToString()) == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Sorry, you cannot delete this Parameter because it is associated with Schedule.');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Parameter deleted successfully')", true);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GrdParameterList_RowDeleting::", ex.Message), ex);
            }
        }

        public void GetParameterTypes()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "21")
            };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                drpParameterType.DataSource = Dt;
                drpParameterType.DataTextField = "ParameterTypeName";
                drpParameterType.DataValueField = "Id";
                drpParameterType.DataBind();
                drpParameterType.Items.Insert(0, new ListItem("Select Parameter Type", "0"));

            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("Get parameter type::", ex.Message), ex);
            }
        }

        protected void drpParameterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@OPR", "74"),
                        new SqlParameter("@ParameterName",txtParameterName.Text),
                        new SqlParameter("@ParameterType", drpParameterType.SelectedValue),
                        new SqlParameter("@CreatedBy",txtCreatedBy.Text)
                    };
            if(drpParameterType.SelectedIndex == 0)
            {
               param = new SqlParameter[]
                        {
                        new SqlParameter("@OPR", "74"),
                        new SqlParameter("@ParameterName",txtParameterName.Text),
                        new SqlParameter("@CreatedBy",txtCreatedBy.Text)
                        };
            }

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            GrdParameterList.DataSource = Dt;
            GrdParameterList.DataBind();
        }

        protected void BtnShow_Click(object sender, EventArgs e)
        {
            bool parameterTypeIsNull = false;
            try
            {
                if (string.IsNullOrEmpty(txtParameterName.Text) && drpParameterType.SelectedIndex == 0 && string.IsNullOrEmpty(txtCreatedBy.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select/Enter atleast one field')", true);
                }
                else
                {
                    if (drpParameterType.SelectedIndex == 0)
                    {
                        parameterTypeIsNull = true;
                        drpParameterType.Items.RemoveAt(0);
                        drpParameterType.Items.Insert(0, new ListItem("0"));
                        drpParameterType.SelectedValue = "0";
                    }
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@OPR", "74"),
                        new SqlParameter("@ParameterName",txtParameterName.Text),
                        new SqlParameter("@ParameterType", drpParameterType.SelectedValue),
                        new SqlParameter("@CreatedBy",txtCreatedBy.Text)
                    };

                    DataTable Dt = new DataTable();
                    Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                    GrdParameterList.DataSource = Dt;
                    GrdParameterList.DataBind();
                    if (Dt.Rows.Count > 0)
                    {
                        if (parameterTypeIsNull)
                        {
                            drpParameterType.Items.RemoveAt(0);
                            drpParameterType.Items.Insert(0, new ListItem("Select Parameter Type"));
                        }
                    }
                    else if (Dt.Rows.Count == 0)
                    {
                        if (parameterTypeIsNull)
                        {
                            drpParameterType.Items.RemoveAt(0);
                            drpParameterType.Items.Insert(0, new ListItem("Select Parameter Type"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("BindCompletedParametersGrid::", ex.Message), ex);
            }
        }
    }
}