using MFG_DigitalApp.Log;
using MFG_DigitalApp.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
namespace MFG_DigitalApp
{
    public partial class UploadOperator : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("UploadOperator");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        private static string uploadfilename;
        int count = 0;
        #region OleDb variable declaration
        OleDbConnection OledbConn;
        OleDbCommand OledbCmd;
        OleDbDataAdapter OledbAdp;
        DataTable OledbDT;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lnkDownload.Attributes.Add("href", "./Format/UploadOperatorFormatEDP.xlsx");
                BindOperator();
                BindAssignedOperator();
            }
        }
        protected void BindOperator()
        {
            SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", "6"),
                new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                };

            DataSet ds = new DataSet();
            ds = DBClass.GetDataSet_WithParam(param, "DM_SP_ADD_ASSIGNED_OPERATOR_DATA");
            drpPlantCode.DataSource = ds.Tables[0];
            drpPlantCode.DataTextField = "PlantCode";
            drpPlantCode.DataValueField = "ID";
            drpPlantCode.DataBind();
            drpPlantCode.Items.Insert(0, new ListItem("Select Plant Code", ""));

            ddlPlantCode.DataSource = ds.Tables[0];
            ddlPlantCode.DataTextField = "PlantCode";
            ddlPlantCode.DataValueField = "ID";
            ddlPlantCode.DataBind();
            ddlPlantCode.Items.Insert(0, new ListItem("Select Plant Code", ""));

            drpLine.DataSource = ds.Tables[1];
            drpLine.DataTextField = "Line";
            drpLine.DataValueField = "Id";
            drpLine.DataBind();
            drpLine.Items.Insert(0, new ListItem("Select Line", ""));

            ddlLine.DataSource = ds.Tables[1];
            ddlLine.DataTextField = "Line";
            ddlLine.DataValueField = "Id";
            ddlLine.DataBind();
            ddlLine.Items.Insert(0, new ListItem("Select Line", ""));

            drpStationCode.DataSource = ds.Tables[2];
            drpStationCode.DataTextField = "StationCode";
            drpStationCode.DataValueField = "StationCode1";
            drpStationCode.DataBind();
            drpStationCode.Items.Insert(0, new ListItem("Select Station Code", ""));

            ddlStationCode.DataSource = ds.Tables[2];
            ddlStationCode.DataTextField = "StationCode";
            ddlStationCode.DataValueField = "StationCode1";
            ddlStationCode.DataBind();
            ddlStationCode.Items.Insert(0, new ListItem("Select Station Code", ""));
        }
        protected void BindAssignedOperator()
        {
            SqlParameter[] param = new SqlParameter[]
                  {
                new SqlParameter("@OPR", "2"),
                 new SqlParameter("@PlantCode",  drpPlantCode.SelectedIndex == 0 ? "" : drpPlantCode.SelectedItem.Text.Trim()),
                    new SqlParameter("@Line",   drpLine.SelectedIndex == 0 ? "" : drpLine.SelectedItem.Text.Trim()),
                    new SqlParameter("@ShiftCode", drpShift.SelectedIndex == 0 ? "" : drpShift.SelectedItem.Text.Trim()),
                    new SqlParameter("@StationCode",  drpStationCode.SelectedIndex == 0 ? "" : drpStationCode.SelectedValue.Trim()),
                    new SqlParameter("@OperatorName", txtOperator.Text.Trim()),
                    new SqlParameter("@Type",  drpAOType.SelectedIndex == 0 ? "" : drpAOType.SelectedItem.Text.Trim()),
                    new SqlParameter("@ShiftFromDate", txtShiftFromDate.Text.Trim()),
                    new SqlParameter("@ShiftToDate", txtShiftToDate.Text.Trim()),
                    new SqlParameter("@CreatedFromDate", txtCreatedFromDate.Text.Trim()),
                    new SqlParameter("@CreatedToDate", txtCreatedToDate.Text.Trim()),
                    new SqlParameter("@CreatedBy",CreatedBy.Text.Trim()),
                    new SqlParameter("@edp",txtedp1.Text.Trim())

                  };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_ADD_ASSIGNED_OPERATOR_DATA");
            GVAssignedOperator.DataSource = Dt;
            GVAssignedOperator.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {

                uploadfilename = DateTime.Now.Date.ToString("ddMMyyyy") + "_" + DateTime.Now.TimeOfDay.ToString("hhmmss") + "_" + Session["USERID"] + "_" + Path.GetFileName(FileUpload1.PostedFile.FileName);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(Server.MapPath("UploadFile") + "\\" + uploadfilename);

                FileUpload1.PostedFile.SaveAs(System.Configuration.ConfigurationManager.AppSettings["path_uploadexcel"] + uploadfilename);

                BindExcel(System.Configuration.ConfigurationManager.AppSettings["path_uploadexcel"] + uploadfilename, Extension);


            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("uploadOperator::", ex.Message), ex);
            }
        }
        #region Bind Gridview
        protected void BindExcel(string filepath, string Extension)
        {
            //DataTable dtExcel;
            //  DataTable dtFinal;
            string conStr = "";
            string isHDR = "Yes";
            try
            {
                switch (Extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = ConfigurationManager.ConnectionStrings["Excel03"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07
                        conStr = ConfigurationManager.ConnectionStrings["Excel07"].ConnectionString;
                        break;
                }
                conStr = String.Format(conStr, filepath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                DataTable dt = new DataTable();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();
                ////////////////
                bool flag = false;
                DataTable newdt = new DataTable();
                newdt.Columns.Add("PlantCode", typeof(string));
                newdt.Columns.Add("Line", typeof(string));
                newdt.Columns.Add("StationCode", typeof(string));
                newdt.Columns.Add("EDP", typeof(string));
                newdt.Columns.Add("Name", typeof(string));
                newdt.Columns.Add("Type", typeof(string));
                newdt.Columns.Add("ShiftCode", typeof(string));
                newdt.Columns.Add("Date", typeof(string));
                newdt.Columns.Add("Status", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PlantCode"].ToString() != null && dr["PlantCode"].ToString() != string.Empty)
                    {
                        if (dr["Line"].ToString() != null && dr["Line"].ToString() != "")
                        {
                            if (dr["StationCode"].ToString() != null && dr["StationCode"].ToString() != "")
                            {
                                if (dr["EDP"].ToString() != null && dr["EDP"].ToString() != "")
                                {
                                    if (dr["Name"].ToString() != null && dr["Name"].ToString() != "")
                                    {
                                        if (dr["Type"].ToString() != null && dr["Type"].ToString() != "")
                                        {
                                            if (dr["ShiftCode"].ToString() != null && dr["ShiftCode"].ToString() != "")
                                            {
                                                if (dr["Date"].ToString() != null && dr["Date"].ToString() != "")
                                                {
                                                    flag = true;
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Date is empty!')", true);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Shift code is empty!')", true);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Type is empty!')", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Name is empty!')", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('EDP is empty!')", true);
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('StationCode is empty!')", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Line is empty!')", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('PlantCode is empty!')", true);
                        return;
                    }

                    if (flag == true)
                    {
                        DataRow dr1 = newdt.NewRow();
                        string result;
                        SqlParameter[] param = new SqlParameter[]
                        {
                                                        new SqlParameter("@PlantCode", Convert.ToString(dr["PlantCode"])),
                                                        new SqlParameter("@Line",  Convert.ToString(dr["Line"])),
                                                        new SqlParameter("@StationCode", Convert.ToString(dr["StationCode"])),
                                                        new SqlParameter("@edp", Convert.ToString(dr["EDP"])),
                                                        new SqlParameter("@Date",Convert.ToDateTime(Convert.ToString(dr["Date"])).ToString("yyyy-MM-dd")),
                                                        new SqlParameter("@ShiftCode", Convert.ToString(dr["ShiftCode"])),
                                                        new SqlParameter("@OperatorName", Convert.ToString(dr["Name"])),
                        };
                        result = DBClass.ExecuteScalar_WithParam(param, "SP_OperatorDetails").ToString();
                        dr1["PlantCode"] = dr["PlantCode"];
                        dr1["Line"] = dr["Line"];
                        dr1["StationCode"] = dr["StationCode"];
                        dr1["EDP"] = dr["EDP"];
                        dr1["Name"] = dr["Name"];
                        dr1["Type"] = dr["Type"];
                        dr1["ShiftCode"] = dr["ShiftCode"];
                        dr1["Date"] = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd");
                        dr1["Status"] = result;

                        newdt.Rows.Add(dr1);

                        if (result != "OK")
                        {
                            count++;
                        }

                        if (count == 0)
                        {
                            btnSave.Visible = true;
                            ViewState["Operator"] = dt;
                        }
                    }
                    //////////////////
                }
                GrdOperators.DataSource = newdt;
                //GrdOperators.DataSource = dt;
                GrdOperators.DataBind();
                //btnSave.Visible = true;
                //ViewState["Operator"] = dt;
                //    DataTable DTRequest = new DataTable();

                //    SqlParameter[] param = new SqlParameter[]
                //{
                //     new SqlParameter("@CreatedBy",Convert.ToString(Session["main_userCode"]) ),
                //    new SqlParameter("@t",dtExcel)
                //};

                //    DTRequest = DBclass.GetDatatable_WithParam(param, "SP_APP_Upload_Validation");

                //    GrdOperators.DataSource = DTRequest;
                //    GrdOperators.DataBind();
            }
            catch (Exception ex)
            {

                _logger.Error(string.Concat("uploadOperatorFile::", ex.Message), ex);
                if (ex.Message.Contains("Sheet1") == true)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Sheet1 Not Found !')", true);

                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('" + ex.Message + "')", true);

                btnSave.Visible = false;
                //  pnlMsgUploadIndex.Visible = true;

            }
            finally
            {
                // dtExcel.Dispose();

            }
        }
        #endregion
        #region Bind Excel Index Norm data
        public DataTable bindExcelIndexData(string filepath)
        {
            try
            {
                string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                OledbConn = new OleDbConnection(connstr);
                OledbConn.Open();


                DataTable dtExcelSchema = new DataTable();

                dtExcelSchema = OledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();


                // OledbCmd = new OleDbCommand("SELECT * FROM [" + SheetName + "]", OledbConn);
                OledbCmd = new OleDbCommand("SELECT * FROM [Sheet1$]", OledbConn);
                OledbAdp = new OleDbDataAdapter(OledbCmd);

                OledbDT = new DataTable();

                OledbAdp.Fill(OledbDT);

                return OledbDT;

            }

            finally
            {
                OledbDT.Dispose();
                OledbAdp.Dispose();
                OledbCmd.Dispose();
                OledbConn.Dispose();
            }
        }
        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["Operator"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime datetime = DateTime.Parse(Convert.ToString(dt.Rows[i]["Date"]));
                string formatteddate = datetime.ToString("yyyy-MM-dd");
                object result;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 1),
                    new SqlParameter("@PlantCode", Convert.ToString(dt.Rows[i]["PlantCode"])),
                    new SqlParameter("@Line",  Convert.ToString(dt.Rows[i]["Line"])),
                    new SqlParameter("@Date", formatteddate),
                    new SqlParameter("@ShiftCode",  Convert.ToString(dt.Rows[i]["ShiftCode"])),
                    new SqlParameter("@StationCode", Convert.ToString(dt.Rows[i]["StationCode"])),
                    new SqlParameter("@OperatorName", Convert.ToString(dt.Rows[i]["Name"])),
                    new SqlParameter("@Type", Convert.ToString(dt.Rows[i]["Type"])),
                    new SqlParameter("@EDP", Convert.ToString(dt.Rows[i]["EDP"])),
                    new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                };
                result = DBClass.ExecuteScalar_WithParam(param, "DM_SP_ADD_ASSIGNED_OPERATOR_DATA");
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                       "alert3", "alert('Operator data submitted successfully');", true);
            GrdOperators.DataSource = null;
            GrdOperators.DataBind();
            btnSave.Visible = false;
            BindAssignedOperator();

        }

        protected void GVAssignedOperator_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GVAssignedOperator.EditIndex)
            {
                //(e.Row.Cells[0].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        protected void GVAssignedOperator_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GVAssignedOperator.EditIndex = e.NewEditIndex;
            this.BindAssignedOperator();
        }

        protected void GVAssignedOperator_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GVAssignedOperator.EditIndex = -1;
            this.BindAssignedOperator();
        }



        protected void GVAssignedOperator_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ID = string.Empty;
            ID = Convert.ToString(e.CommandArgument);
            if (e.CommandName.Equals("EditOperator"))
            {

                hdID.Value = ID;
                MPAssignOperator.Show();
                getOperatorDetails(ID);

            }
            if (e.CommandName.Equals("DeleteOperator"))
            {
                object res = null;
                SqlParameter[] Existsparam = new SqlParameter[]
                                        {
                             new SqlParameter("@OPR", 3),
                            new SqlParameter("@ID",ID)
                                        };
                res = DBClass.ExecuteScalar_WithParam(Existsparam, "DM_SP_ADD_ASSIGNED_OPERATOR_DATA");
                if (res.ToString() == "SUCCESS")
                {
                    Response.Write("<script> alert('Operator details deleted successfully!'); </script>");
                }
                BindAssignedOperator();
            }
        }
        protected void getOperatorDetails(string ID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                    {
                new SqlParameter("@OPR", "4"),
                new SqlParameter("@ID",ID)
                    };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_ADD_ASSIGNED_OPERATOR_DATA");
                ddlPlantCode.SelectedIndex = ddlPlantCode.Items.IndexOf(ddlPlantCode.Items.FindByText(Convert.ToString(Dt.Rows[0]["PlantCode"])));
                // txtPlantCode.Text = Convert.ToString(Dt.Rows[0]["PlantCode"]);
                ddlLine.SelectedIndex = ddlLine.Items.IndexOf(ddlLine.Items.FindByText(Convert.ToString(Dt.Rows[0]["Line"])));
                //txtLine.Text = Convert.ToString(Dt.Rows[0]["Line"]);
                ddlStationCode.SelectedIndex = ddlStationCode.Items.IndexOf(ddlStationCode.Items.FindByText(Convert.ToString(Dt.Rows[0]["StationCode"])));
                // txtStationCode.Text = Convert.ToString(Dt.Rows[0]["StationCode"]);
                txtName.Text = Convert.ToString(Dt.Rows[0]["AssignedOperator"]);
                ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByText(Convert.ToString(Dt.Rows[0]["AOType"])));
                // txtType.Text = Convert.ToString(Dt.Rows[0]["AOType"]);
                ddlShiftCode.SelectedIndex = ddlShiftCode.Items.IndexOf(ddlShiftCode.Items.FindByText(Convert.ToString(Dt.Rows[0]["Shift"])));
                // txtShiftCode.Text = Convert.ToString(Dt.Rows[0]["Shift"]);
                DateTime datetime = DateTime.Parse(Convert.ToString(Dt.Rows[0]["Date"]));
                string formatteddate = datetime.ToString("yyyy-MM-dd");
                txtDate.Text = formatteddate;
                DateTime datetime2 = DateTime.Parse(Convert.ToString(Dt.Rows[0]["CreatedOn"]));
                string formatteddate2 = datetime2.ToString("yyyy-MM-dd");
                txtCreatedOn.Text = formatteddate2;
                txtCreatedBy.Text = Convert.ToString(Dt.Rows[0]["CreatedBy"]);
                txtEDP.Text = Convert.ToString(Dt.Rows[0]["EDP"]);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnUpdateOperator_Click(object sender, EventArgs e)
        {
            object res;
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 5),
                    new SqlParameter("@PlantCode", ddlPlantCode.SelectedItem.Text),
                    new SqlParameter("@Line",  ddlLine.SelectedItem.Text),
                    new SqlParameter("@Date", txtDate.Text),
                    new SqlParameter("@ShiftCode", ddlShiftCode.SelectedItem.Text),
                    new SqlParameter("@StationCode", ddlStationCode.SelectedItem.Text),
                    new SqlParameter("@OperatorName", txtName.Text),
                    new SqlParameter("@Type", ddlType.SelectedItem.Text),
                    new SqlParameter("@CreatedBy",txtCreatedBy.Text),
                     new SqlParameter("@Id",hdID.Value),
                     new SqlParameter("@EDP",txtEDP.Text)
                };
            res = DBClass.ExecuteScalar_WithParam(param, "DM_SP_ADD_ASSIGNED_OPERATOR_DATA");
            if (res.ToString() == "SUCCESS")
            {
                Response.Write("<script> alert('Operator details updated successfully!'); </script>");
            }
            BindAssignedOperator();
        }

        protected void GVAssignedOperator_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVAssignedOperator.PageIndex = e.NewPageIndex;
            BindAssignedOperator();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindAssignedOperator();
        }

        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            GVAssignedOperator.AllowPaging = false;
            //gvInvoiceDetails.Columns[10].Visible = false;
            //gvInvoiceDetails.Columns[11].Visible = false;
            //gvInvoiceDetails.Columns[12].Visible = false;
            // FillGrid();
            BindAssignedOperator();
            string FileName = "AssignedOperatorDetails_" + DateTime.Now + ".xlsx";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            GVAssignedOperator.Columns[0].Visible = false;
            GVAssignedOperator.Columns[11].Visible = false;
            GVAssignedOperator.Columns[12].Visible = false;
            GVAssignedOperator.GridLines = GridLines.Both;
            GVAssignedOperator.HeaderStyle.Font.Bold = true;
            GVAssignedOperator.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

            drpPlantCode.SelectedIndex = 0;
            drpLine.SelectedIndex = 0;
            drpShift.SelectedIndex = 0;
            drpStationCode.SelectedIndex = 0;
            txtOperator.Text = "";
            drpAOType.SelectedIndex = 0;
            txtShiftFromDate.Text = "";
            txtShiftToDate.Text = "";
            txtCreatedFromDate.Text = "";
            txtCreatedToDate.Text = "";
            CreatedBy.Text = "";
        }
        protected void btnExporttoExcel_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }
        //protected void btnDeleteRecords_Click(object sender, EventArgs e)
        //{


        //    foreach (GridViewRow row in GVAssignedOperator.Rows)
        //    {
        //        if ((row.FindControl("chkSelect") as CheckBox).Checked)
        //        {
        //            int ID = Convert.ToInt32(GVAssignedOperator.DataKeys[row.RowIndex].Value);
        //            object res = null;
        //            SqlParameter[] Existsparam = new SqlParameter[]
        //                                {
        //                     new SqlParameter("@OPR", 3),                           
        //                    new SqlParameter("@ID",ID)   
        //                                };
        //            res = DBClass.ExecuteScalar_WithParam(Existsparam, "DM_SP_ADD_ASSIGNED_OPERATOR_DATA");
        //        }
        //    }

        //    BindAssignedOperator();
        //    Response.Write("<script> alert('Operator details deleted successfully!'); </script>");
        //}  
        protected void btnDeleteRecords_Click(object sender, EventArgs e)
        {

            object res = null;
            foreach (GridViewRow row in GVAssignedOperator.Rows)
            {
                if ((row.FindControl("chkSelect") as CheckBox).Checked)
                {
                    int ID = Convert.ToInt32(GVAssignedOperator.DataKeys[row.RowIndex].Value);

                    SqlParameter[] Existsparam = new SqlParameter[]
                                        {
                             new SqlParameter("@OPR", 3),
                            new SqlParameter("@ID",ID)
                                        };
                    res = DBClass.ExecuteScalar_WithParam(Existsparam, "DM_SP_ADD_ASSIGNED_OPERATOR_DATA");
                }
            }

            BindAssignedOperator();
            if (res != null)
            {
                Response.Write("<script> alert('Operator details deleted successfully!'); </script>");
            }
            else
            {
                Response.Write("<script> alert('Please select atleast one row of operator details!'); </script>");
            }
        }
    }
}