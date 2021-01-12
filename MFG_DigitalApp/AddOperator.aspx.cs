using MFG_DigitalApp.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MFG_DigitalApp
{
    public partial class AddOperator : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("UploadOperator");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        private static string uploadfilename;
        int count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lnkDownload.Attributes.Add("href", "./Format/UploadOperatorwithEDP.xlsx");
                bindgrid();
                bindplant();
                bindoperatortype();
            }
        }

        public void bindoperatortype()
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@OPR", 13)
             };
            dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
            drpType.DataSource = dt;
            drpType.DataTextField = "OperatorType";
            drpType.DataValueField = "OperatorType";
            drpType.DataBind();
            drpType.Items.Insert(0, new ListItem("Select Type", ""));

            ddlType.DataSource = dt;
            ddlType.DataTextField = "OperatorType";
            ddlType.DataValueField = "OperatorType";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("Select Type", ""));
        }

        public void bindplant()
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@OPR", 2)
             };
            dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
            ddlPlant.DataSource = dt;
            ddlPlant.DataTextField = "PlantName";
            ddlPlant.DataValueField = "PlantCode";
            ddlPlant.DataBind();
            ddlPlant.Items.Insert(0, new ListItem("Select Plant", ""));

            ddlPlantOperator.DataSource = dt;
            ddlPlantOperator.DataTextField = "PlantName";
            ddlPlantOperator.DataValueField = "PlantCode";
            ddlPlantOperator.DataBind();
            ddlPlantOperator.Items.Insert(0, new ListItem("Select Plant", ""));
        }
        public void bindgrid()
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@OPR", 1)
             };
            dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
            grdOperator.DataSource = dt;
            grdOperator.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string plant = ddlPlantOperator.SelectedValue;
            string edpno = txtEdp.Text;
            string operatorname = txtOperatorname.Text;
            string type = drpType.SelectedValue;
            if (btnSubmit.Text == "Add")
            {
                SqlParameter[] param = new SqlParameter[]
                 { new SqlParameter("@opr",4),
                   new SqlParameter("@plant",plant),
                   new SqlParameter("@edp",edpno),
                   new SqlParameter("@query",operatorname),
                   new SqlParameter("@type",type),
                   new SqlParameter("@user",Session["username"].ToString())
                 };

                int result = Convert.ToInt16(DBClass.ExecuteScalar_WithParam(param, "SP_OperatorMaster"));
                if (result > -1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Operator added successfully')", true);
                }
                else if (result == -1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Operator already exist.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Issue in adding operator details. Please contact Administrator.')", true);
                }
                Clear();
            }
            else if (btnSubmit.Text == "Update")
            {
                SqlParameter[] param = new SqlParameter[]
                 { new SqlParameter("@opr",5),
                   new SqlParameter("@plant",plant),
                   new SqlParameter("@edp",edpno),
                   new SqlParameter("@query",operatorname),
                   new SqlParameter("@type",type),
                   new SqlParameter("@user",Session["username"].ToString()),
                   new SqlParameter("@id",Session["Id"].ToString())

                 };

                int result = Convert.ToInt16(DBClass.ExecuteScalar_WithParam(param, "SP_OperatorMaster"));
                if (result > -1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Operator data updated successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Issue in updating operator details. Please contact Administrator.')", true);
                }
                btnSubmit.Text = "Add";
                Clear();
            }
            bindgrid();
        }

        public void Clear()
        {
            txtEdp.Text = string.Empty;
            txtOperatorname.Text = string.Empty;
            ddlPlant.SelectedIndex = -1;
            ddlPlantOperator.SelectedIndex = -1;
            txtsearch.Text = string.Empty;
            grdOperator.Columns[3].Visible = true;
            grdOperator.Columns[4].Visible = true;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlPlantOperator.SelectedIndex = -1;
            txtOperatorname.Text = string.Empty;
            txtEdp.Text = string.Empty;
            bindgrid();
            grdOperator.Columns[3].Visible = true;
            grdOperator.Columns[4].Visible = true;
            btnSubmit.Text = "Add";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@OPR", 3),
                 new SqlParameter("@plant", ddlPlant.SelectedValue==""?null:ddlPlant.SelectedValue),
                 new SqlParameter("@query", txtsearch.Text),
                 new SqlParameter("@type", ddlType.SelectedValue==""?null:ddlType.SelectedValue)
             };
            dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
            grdOperator.DataSource = dt;
            grdOperator.DataBind();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlPlantOperator.SelectedIndex = -1;
            txtsearch.Text = string.Empty;
            bindgrid();
        }

        protected void grdOperator_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditOperator")
            {
                string id = Convert.ToString(e.CommandArgument);
                Session["Id"] = id;
                DataTable dt = new DataTable();
                SqlParameter[] param = new SqlParameter[]
                 {
                 new SqlParameter("@OPR", 6),
                 new SqlParameter("@id", id)
                 };
                dt = DBClass.GetDatatable_WithParam(param, "SP_OperatorMaster");
                ddlPlantOperator.SelectedValue = dt.Rows[0]["PlantCode"].ToString();
                txtEdp.Text = dt.Rows[0]["EDPNumber"].ToString();
                txtOperatorname.Text = dt.Rows[0]["OperatorName"].ToString();
                drpType.SelectedValue = dt.Rows[0]["Type"].ToString();
                grdOperator.Columns[3].Visible = false;
                grdOperator.Columns[4].Visible = false;
                //GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;
                //row.Cells[3].Visible = false;
                //row.Cells[4].Visible = false;
                btnSubmit.Text = "Update";
            }
            else if (e.CommandName == "DeleteOperator")
            {
                string AutoId = Convert.ToString(e.CommandArgument);
                object obj;
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", 7),
                new SqlParameter("@id", AutoId),
                new SqlParameter("@user", Session["username"].ToString())
                };

                obj = DBClass.ExecuteNonQuery_WithParam(param, "SP_OperatorMaster");
                grdOperator.EditIndex = -1;
                bindgrid();
                if (obj != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Operator deleted successfully')", true);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["Operator"];
            //object obj;
            //SqlParameter[] param = new SqlParameter[]
            //    {
            //    new SqlParameter("@OPR", 12),
            //    new SqlParameter("@plant", dt.Rows[0]["PlantCode"]),
            //    new SqlParameter("@user", Session["username"].ToString())
            //    };
            //obj = DBClass.ExecuteNonQuery_WithParam(param, "SP_OperatorMaster");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object result;
                SqlParameter[] param1 = new SqlParameter[]
                {
                    new SqlParameter("@OPR", 4),
                    new SqlParameter("@plant", Convert.ToString(dt.Rows[i]["PlantCode"])),
                    new SqlParameter("@query", Convert.ToString(dt.Rows[i]["OperatorName"])),
                    new SqlParameter("@edp", Convert.ToString(dt.Rows[i]["EDPNumber"])),
                    new SqlParameter("@user",Convert.ToString(Session["username"])),
                    new SqlParameter("@type",Convert.ToString(dt.Rows[i]["Type"]))
                };
                result = DBClass.ExecuteScalar_WithParam(param1, "SP_OperatorMaster");
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                       "alert3", "alert('Operator data submitted successfully');", true);
            GrdOperators.DataSource = null;
            GrdOperators.DataBind();
            btnSave.Visible = false;
            GrdOperators.Visible = false;
            bindgrid();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                uploadfilename = DateTime.Now.Date.ToString("ddMMyyyy") + "_" + DateTime.Now.TimeOfDay.ToString("hhmmss") + "_" + Session["USERID"] + "_" + Path.GetFileName(FileUpload1.PostedFile.FileName);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(Server.MapPath("UploadFile\\Operator") + "\\" + uploadfilename);
                FileUpload1.PostedFile.SaveAs(System.Configuration.ConfigurationManager.AppSettings["path_uploadexcel"] + uploadfilename);
                BindExcel(System.Configuration.ConfigurationManager.AppSettings["path_uploadexceloperator"] + uploadfilename, Extension);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("uploadOperator::", ex.Message), ex);
            }
        }

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
                newdt.Columns.Add("EDPNumber", typeof(string));
                newdt.Columns.Add("OperatorName", typeof(string));
                newdt.Columns.Add("Type", typeof(string));
                newdt.Columns.Add("Status", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PlantCode"].ToString() != null && dr["PlantCode"].ToString() != string.Empty)
                    {
                        if (Convert.ToString(dr["EDPNumber"]) != null && Convert.ToString(dr["EDPNumber"].ToString()) != "")
                        {
                            if (dr["OperatorName"].ToString() != null && dr["OperatorName"].ToString() != "")
                            {
                                if (dr["Type"].ToString() != null && dr["Type"].ToString() != "")
                                {
                                    flag = true;
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Type is empty!')", true);
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Operator Name is empty!')", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('EDP Number is empty!')", true);
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
                            new SqlParameter("@opr", 11),
                            new SqlParameter("@plant", Convert.ToString(dr["PlantCode"])),
                            new SqlParameter("@edp", Convert.ToString(dr["EDPNumber"])),
                            new SqlParameter("@query", Convert.ToString(dr["OperatorName"])),
                            new SqlParameter("@type", Convert.ToString(dr["Type"]))
                        };
                        result = DBClass.ExecuteScalar_WithParam(param, "SP_OperatorMaster").ToString();
                        dr1["PlantCode"] = dr["PlantCode"];
                        dr1["EDPNumber"] = dr["EDPNumber"];
                        dr1["OperatorName"] = dr["OperatorName"];
                        dr1["Type"] = dr["Type"];
                        dr1["Status"] = result;
                        newdt.Rows.Add(dr1);

                        if (result != "OK")
                        {
                            count++;
                            //btnSave.Visible = false;
                        }

                        if (count == 0)
                        {
                            //btnSave.Visible = true;
                            ViewState["Operator"] = dt;
                        }
                    }
                    //////////////////
                }
                if (count > 0)
                {
                    btnSave.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string userID = Convert.ToString(Session["username"]);
                for (int count = 0; count < grdOperator.Rows.Count; count++)
                {
                    CheckBox chkCheck = (CheckBox)grdOperator.Rows[count].FindControl("chkCheck");

                    if (chkCheck.Checked)
                    {
                        Label lblForId = (Label)grdOperator.Rows[count].FindControl("lblID");
                        sb.Append(lblForId.Text);
                        sb.Append(",");


                        Label lblID = (Label)grdOperator.Rows[count].FindControl("lblID");
                        Label lblPlant = (Label)grdOperator.Rows[count].FindControl("lblPlant");
                        Label lblEDP = (Label)grdOperator.Rows[count].FindControl("lblEDP");
                        Label lblOperator = (Label)grdOperator.Rows[count].FindControl("lblOperator");
                        Label lblType = (Label)grdOperator.Rows[count].FindControl("lblType");

                        SqlParameter[] param_W = new SqlParameter[]
                                            {
                                                new SqlParameter("@opr", 14),
                                                new SqlParameter("@id", lblID.Text),
                                                new SqlParameter("@plant", lblPlant.Text),
                                                new SqlParameter("@edp", lblEDP.Text),
                                                new SqlParameter("@query", lblOperator.Text),
                                                new SqlParameter("@user", userID)
                    };
                        int main = DBClass.ExecuteNonQuery_WithParam(param_W, "SP_OperatorMaster");
                    }
                }
                //Add update code
                string IdString = sb.ToString();
                //string IdsFromHiddenValue = hdfIDs.Value;
                if (string.IsNullOrEmpty(IdString))
                {
                    //lblMessage.Text = "Please select atleast one record";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select atleast one record !')", true);
                }
                else
                {
                    //lblMessage.Text = "";
                    //string IdsFromHiddenField = hdfIDs.Value.ToString();
                    //if (!string.IsNullOrEmpty(IdsFromHiddenField))
                    //{
                    //    IdString = IdsFromHiddenField + IdString;
                    //}
                    IdString = IdString.Substring(0, IdString.LastIndexOf(","));
                    //string userID = "1";//now without login

                    //pass this string as a parameter to stored procedure
                    //DAL objDAL = new DAL();
                    SqlParameter[] param_W = new SqlParameter[]
                                            {
                                                new SqlParameter("@opr", 15),
                                                new SqlParameter("@user", userID),
                                                new SqlParameter("@list", IdString)
                    };
                    int main = DBClass.ExecuteNonQuery_WithParam(param_W, "SP_OperatorMaster");
                    //objDAL.DeleteFromGCPL(userID, IdString);
                    //lblMessage.Text = "Invoices are sent to respective transporters";
                    string message = "Selected Operators are deleted from list.";
                    string script = "window.onload = function(){ alert('";
                    script += message;
                    script += "')};";
                    ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                    //BindData(ddlTransportMethod.SelectedValue.ToString());
                    //BindData(Session["TransportMode"].ToString());
                    bindgrid();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("uploadOperator::", ex.Message), ex);
            }
        }
    }
}