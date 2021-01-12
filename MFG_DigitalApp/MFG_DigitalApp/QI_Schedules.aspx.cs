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
    public partial class QI_Schedules : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("QI_ScheduleDetails");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        protected void OnSelectedRecord(Object sender,EventArgs e)
        {
            for (int i = 0; i < GrdScheduleList.Rows.Count; i++)
            {
                RadioButton rb = (RadioButton)GrdScheduleList.Rows[i]
                                .Cells[0].FindControl("rbCreate");
                if (rb != null)
                {
                    if (rb.Checked)
                    {
                        int rowIndex = ((sender as RadioButton).NamingContainer as GridViewRow).RowIndex;
                        int id = Convert.ToInt32(GrdScheduleList.DataKeys[rowIndex].Values[0]);
                        if (id > 0)
                            Session["CScheduleId"] = id;
                        break;
                    }
                }
            }
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            GrdScheduleList.PageIndex = e.NewPageIndex;
            GetData();
        }

        protected void GetData()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "33"),
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                GrdScheduleList.DataSource = Dt;
                GrdScheduleList.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetData::", ex.Message), ex);
            }
        }

        protected void btnCreateSchedule_Click(object sender, EventArgs e)
        {

            Response.Redirect("QI_CreateSchedule.aspx",false);
        }
        protected void lnkScheduleNumber_Click(object sender, EventArgs e)
        {
            int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
            int id = Convert.ToInt32(GrdScheduleList.DataKeys[rowIndex].Values[0]);
            if(id > 0)
                Session["ScheduleId"] = id;
            Response.Redirect("QI_EditSchedule.aspx", false);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename = QC_Schedule.xls");
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);
            GrdScheduleList.AllowPaging = false;
            GetData();
            GrdScheduleList.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }

        protected void GrdScheduleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                foreach (Button button in e.Row.Cells[9].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete this record?')){ return false; };";
                    }
                }
            }

        }

        protected void GrdScheduleList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int64 AutoId = Convert.ToInt32(GrdScheduleList.DataKeys[e.RowIndex].Value.ToString());
            object obj;
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", 5),
                new SqlParameter("@AutoId", AutoId),
            };

            obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
            GetData();
            if (obj != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Schedule deleted successfully')", true);
            }
        }
    }
}