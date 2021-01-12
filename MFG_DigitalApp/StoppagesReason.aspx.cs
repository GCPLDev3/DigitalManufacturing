using MFG_DigitalApp.Log;
using MFG_DigitalApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MFG_DigitalApp
{
    public partial class StoppagesReason : System.Web.UI.Page
    {
        private static readonly ILogger _logger = Logger.GetLogger("StoppagesReason");
        DataBaseConnectClass DBClass = new DataBaseConnectClass();
        private static string labeldate = "";


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
                    //lbldate.Text = dateTime.ToString("dd-MM-yyyy"); 

                    //lbldate.Text = FormatedDate;
                    //string date = model.ShiftDate;
                    //DateTime dateTime = DateTime.Parse(date);
                    //var Formatteddate = dateTime.ToString("yyyy-MM-dd");
                    //lbldate.Text = Formatteddate;

                    txtDate.Text = Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd"); ;// lbldate.Text;// labeldate;
                    divplant.Visible = lblplantcode.Text=="M016"?true:false;
                    GetShiftHeader();
                    GetShift(Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd"));
                    GetStation();
                    GetDuration();
                    GrdStoppageReason.EditIndex = -1;
                    BindStoppagesReason();
                    getCategory();
                    getDowntype();
                    getTPMLosses();
                    bindtimeslot(Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd"));
                    //GetReason();
                    //BindTimeControl(drpTime);
                }
            }
        }

        public void bindtimeslot(string date1)
        {
            if (ddlWrapping.SelectedValue == "0")
            {
                timeslot.Visible = true;
                timeslot2.Visible = true;
            }
            else if (ddlWrapping.SelectedValue == "1")
            {
                timeslot.Visible = true;
                timeslot2.Visible = false;
            }
            else if (ddlWrapping.SelectedValue == "2")
            {
                timeslot.Visible = false;
                timeslot2.Visible = true;
            }
            string shift = lblshiftnew.Text;
            int count = 0;
            int count1 = 0;
            int slotcount = 0;
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@opr", 1),
                  new SqlParameter("@countryid", Convert.ToString(Session["CountryId"])),
                    new SqlParameter("@shiftcode", shift),
                    new SqlParameter("@plant", lblplantcode.Text),
                    new SqlParameter("@line", lblline.Text),
                    new SqlParameter("@date", date1),
                    new SqlParameter("@wrp",ddlWrapping.SelectedValue=="0"? "1":ddlWrapping.SelectedValue)
            };
            string date = DateTime.Now.ToString("HH:mm");
            //drp.Items.Clear();
            DataSet DtTimes = new DataSet();
            DtTimes = DBClass.GetDataSet_WithParam(param, "sp_getTimeSlot");
            if (DtTimes.Tables[2].Rows.Count > 0)
            {
                lblstoppagecount.Text = Convert.ToString(DtTimes.Tables[2].Rows[0]["openstoppage"]) + " of " + Convert.ToString(DtTimes.Tables[2].Rows[0]["totalstoppage"]);
                lblminutescount.Text = Convert.ToString(DtTimes.Tables[2].Rows[0]["openmin"]) + " of " + Convert.ToString(DtTimes.Tables[2].Rows[0]["totalmin"]);
            }
            else
            {
                lblstoppagecount.Text = "0 of 0";
                lblminutescount.Text = "0 of 0";
            }
            if (DtTimes.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table style='width:100%;'><tr>");
                for (int k = 0; k < 8; k++)
                {
                    for (int l = 0; l < 12; l++)
                    {
                        //Int32 starttime = 0;
                        //string[] time = new string[10];
                        //if (DtTimes.Tables[2].Rows.Count > 0)
                        //{ 
                        // time = Convert.ToString(DtTimes.Tables[2].Rows[count1]["Time"]).Split(':');
                        // starttime = Convert.ToInt32(Convert.ToString(time[0] + time[1]));
                        //}

                        if (Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]) > Convert.ToDateTime(date) && Convert.ToDateTime(date1).ToString("yyyy-MM-dd") == DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        {
                            sb.Append("<td style='background-color:gray;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                        }
                        //else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Minute) ->&& ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute < Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Minute))) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "" && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != null))
                        //  else if((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Minute) || (Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute < Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Minute)) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "" && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != null && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "00"))
                       
                        else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":","")) >= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["Time"]).Replace(":", "")) && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", ""))<= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["final"]).Replace(":", "")) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "" && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != null && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "00"))
                        {
                            sb.Append("<td style='background-color:gold;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>"); 
                            //if ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).AddMinutes(-5).Minute)|| (Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour > Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).AddMinutes(-5).Minute))
                            //{
                            //    count1++;
                            //}
                            slotcount++;
                            if (Convert.ToInt16(DtTimes.Tables[2].Rows[count1]["slot"]) == slotcount)
                            {
                                count1++;
                                slotcount = 0;
                            }
                        }
                        //else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Minute) && ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Minute)) || (Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour > Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour)) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == "" || Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == null))
                        else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) >= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["Time"]).Replace(":", "")) && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) <= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["final"]).Replace(":", "")) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == "" || Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == null || Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == "00"))
                        {
                            sb.Append("<td style='background-color:red;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                            //if ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).AddMinutes(-5).Minute))
                            //{
                            //    count1++;
                            //}
                            slotcount++;
                            if (Convert.ToInt16(DtTimes.Tables[2].Rows[count1]["slot"]) == slotcount)
                            {
                                count1++;
                                slotcount = 0;

                            }
                        }
                        else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "False" && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) >= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["Time"]).Replace(":", "")) && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) <= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["final"]).Replace(":", "")))
                        {
                            sb.Append("<td style='background-color:black;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                            //if ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).AddMinutes(-5).Minute))
                            //{
                            //    count1++;
                            //}
                            slotcount++;
                            if (Convert.ToInt16(DtTimes.Tables[2].Rows[count1]["slot"]) == slotcount)
                            {
                                count1++;
                                slotcount = 0;

                            }
                        }
                        else
                        {
                            sb.Append("<td style='background-color:green;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                        }
                            count++;
                        }
                }
                sb.Append("</tr><tr>");
                for (int i = 0; i < 8; i++)
                {
                    for (int l = 0; l < 12; l++)
                    {
                        if (l == 6)
                        {
                            sb.Append("<td colspan=12>" + DtTimes.Tables[0].Rows[i]["StartTime"] + "</td>");
                        }
                    }
                }
                sb.Append("</tr></table>");
                timeslot.Text = sb.ToString();
            }

            if (ddlWrapping.SelectedValue == "0")
            {
                 count = 0;
                 count1 = 0;
                slotcount = 0;
                SqlParameter[] param2 = new SqlParameter[]
               {
                    new SqlParameter("@opr", 1),
                      new SqlParameter("@countryid", Convert.ToString(Session["CountryId"])),
                        new SqlParameter("@shiftcode", shift),
                        new SqlParameter("@plant", lblplantcode.Text),
                        new SqlParameter("@line", lblline.Text),
                        new SqlParameter("@date", date1),
                        new SqlParameter("@wrp","2")
               };
                DtTimes = DBClass.GetDataSet_WithParam(param2, "sp_getTimeSlot");
                if (DtTimes.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table style='width:100%;'><tr>");
                    for (int k = 0; k < 8; k++)
                    {
                        for (int l = 0; l < 12; l++)
                        {
                            //Int32 starttime = 0;
                            //string[] time = new string[10];
                            //if (DtTimes.Tables[2].Rows.Count > 0)
                            //{ 
                            // time = Convert.ToString(DtTimes.Tables[2].Rows[count1]["Time"]).Split(':');
                            // starttime = Convert.ToInt32(Convert.ToString(time[0] + time[1]));
                            //}

                            if (Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]) > Convert.ToDateTime(date) && Convert.ToDateTime(date1).ToString("yyyy-MM-dd") == DateTime.Now.Date.ToString("yyyy-MM-dd"))
                            {
                                sb.Append("<td style='background-color:gray;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                            }
                            //else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Minute) ->&& ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute < Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Minute))) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "" && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != null))
                            //  else if((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Minute) || (Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute < Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Minute)) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "" && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != null && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "00"))

                            else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) >= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["Time"]).Replace(":", "")) && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) <= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["final"]).Replace(":", "")) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "" && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != null && Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) != "00"))
                            {
                                sb.Append("<td style='background-color:gold;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                                //if ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).AddMinutes(-5).Minute)|| (Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour > Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).AddMinutes(-5).Minute))
                                //{
                                //    count1++;
                                //}
                                slotcount++;
                                if (Convert.ToInt16(DtTimes.Tables[2].Rows[count1]["slot"]) == slotcount)
                                {
                                    count1++;
                                    slotcount = 0;
                                }
                            }
                            //else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute >= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["Time"]).Minute) && ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Minute)) || (Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour > Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour)) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == "" || Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == null))
                            else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "True" && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) >= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["Time"]).Replace(":", "")) && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) <= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["final"]).Replace(":", "")) && (Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == "" || Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == null || Convert.ToString(DtTimes.Tables[2].Rows[count1]["ReasonCode"]) == "00"))
                            {
                                sb.Append("<td style='background-color:red;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                                //if ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).AddMinutes(-5).Minute))
                                //{
                                //    count1++;
                                //}
                                slotcount++;
                                if (Convert.ToInt16(DtTimes.Tables[2].Rows[count1]["slot"]) == slotcount)
                                {
                                    count1++;
                                    slotcount = 0;

                                }
                            }
                            else if ((DtTimes.Tables[2].Rows.Count > count1) && Convert.ToString(DtTimes.Tables[2].Rows[count1]["isactive"]) == "False" && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) >= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["Time"]).Replace(":", "")) && Convert.ToInt32(Convert.ToString(DtTimes.Tables[1].Rows[count]["TimeValue"]).Replace(":", "")) <= Convert.ToInt32(Convert.ToString(DtTimes.Tables[2].Rows[count1]["final"]).Replace(":", "")))
                            {
                                sb.Append("<td style='background-color:black;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                                //if ((Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Hour <= Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).Hour && Convert.ToDateTime(DtTimes.Tables[1].Rows[count]["TimeValue"]).Minute == Convert.ToDateTime(DtTimes.Tables[2].Rows[count1]["final"]).AddMinutes(-5).Minute))
                                //{
                                //    count1++;
                                //}
                                slotcount++;
                                if (Convert.ToInt16(DtTimes.Tables[2].Rows[count1]["slot"]) == slotcount)
                                {
                                    count1++;
                                    slotcount = 0;

                                }
                            }
                            else
                            {
                                sb.Append("<td style='background-color:green;height:35px;' id=" + DtTimes.Tables[1].Rows[count]["TimeValue"] + ">&nbsp;</td>");
                            }
                            count++;
                        }
                    }
                    sb.Append("</tr><tr>");
                    for (int i = 0; i < 8; i++)
                    {
                        for (int l = 0; l < 12; l++)
                        {
                            if (l == 6)
                            {
                                sb.Append("<td colspan=12>" + DtTimes.Tables[0].Rows[i]["StartTime"] + "</td>");
                            }
                        }
                    }
                    sb.Append("</tr></table>");
                    timeslot2.Text = sb.ToString();
                }
            }
        }

        public void BindTimeControl(DropDownList drp)
        {
            //SqlParameter[] param = new SqlParameter[]
            //{
            //    new SqlParameter("@OPR", "14"),
            //    new SqlParameter("@ShiftCode", drpShift.SelectedValue),
            //    new SqlParameter("@PlantCode", lblplantcode.Text),
            //    new SqlParameter("@ShiftDate",Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd"))
            //};

            //drp.Items.Clear();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@opr", 1),
                  new SqlParameter("@countryid", Convert.ToString(Session["CountryId"])),
                    new SqlParameter("@shiftcode", drpShift.SelectedValue)
            };
            DataSet DtTimes = new DataSet();
            //DtTimes = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            DtTimes = DBClass.GetDataSet_WithParam(param, "sp_getTimeSlot");
            drp.DataSource = DtTimes.Tables[1];
            drp.DataValueField = "TimeValue";
            drp.DataTextField = "TimeValue";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("HH:MM", ""));
            //GrdStoppageReason.EditIndex = -1;
            //drp.SelectedValue = Convert.ToString(DtTimes.Rows[0]["CurrentTime"]);
        }
        protected void GetShift(string date)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@OPR", "13"),
                new SqlParameter("@PlantCode",lblplantcode.Text),
                new SqlParameter("@Date",date)
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            drpShift.DataSource = Dt;
            drpShift.DataTextField = "ShiftName";
            drpShift.DataValueField = "ShiftCode";
            drpShift.DataBind();
            drpShift.Items.Insert(0, new ListItem("Select Shift", "0"));
            if (lblshift.Text == "PS1")
            { lblshift.Text = "S1"; }
            else if (lblshift.Text == "PS2")
            { lblshift.Text = "S2"; }
            else if (lblshift.Text == "PS3") { lblshift.Text = "S3"; }
            else if (lblshift.Text == "PS4")
            { lblshift.Text = "S4"; }
            else if (lblshift.Text == "PS5")
            { lblshift.Text = "S5"; }
            else if (lblshift.Text == "PS6") { lblshift.Text = "S6"; }

            drpShift.SelectedValue = Convert.ToString(lblshift.Text);
            //drpShift.Enabled = false;
            drpShift_SelectedIndexChanged(null, null);
        }
        protected void GetStation()
        {

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "16"),
                new SqlParameter("@PlantCode", lblplantcode.Text),
               new SqlParameter("@Line",lblline.Text.Trim())
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            drpStation.DataSource = Dt;
            drpStation.DataTextField = "StationDescription";
            drpStation.DataValueField = "StationCode";
            drpStation.DataBind();
            drpStation.Items.Insert(0, new ListItem("Select Station", "0"));
        }
        protected void GetReason()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@OPR", "15"),
                 new SqlParameter("@PlantCode",lblplantcode.Text.Trim()),
                 new SqlParameter("@Line",lblline.Text.Trim()),
                  new SqlParameter("@StationCode",drpStation.SelectedValue)
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            drpReason.DataSource = Dt;
            drpReason.DataTextField = "ReasonDescription";
            drpReason.DataValueField = "ReasonCode";
            drpReason.DataBind();
            drpReason.Items.Insert(0, new ListItem("Select Reason", "0"));
        }
        protected void GetDuration()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "7")
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            ddlHour.DataSource = Dt;
            ddlHour.DataTextField = "Hours";
            ddlHour.DataValueField = "Hours";
            ddlHour.DataBind();
            ddlHour.Items.Insert(0, new ListItem("Select Hour", "0"));

            SqlParameter[] param1 = new SqlParameter[]
            {
                new SqlParameter("@OPR", "8")
            };
            DataTable Dtt = new DataTable();
            Dtt = DBClass.GetDatatable_WithParam(param1, "DM_SP_GET_STOPPAGE_DATA");
            ddlMinute.DataSource = Dtt;
            ddlMinute.DataTextField = "Minutes";
            ddlMinute.DataValueField = "Minutes";
            ddlMinute.DataBind();
            ddlMinute.Items.Insert(0, new ListItem("Select Minute", "0"));
        }
        protected void getCategory()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@opr", "1"),
                new SqlParameter("@PlantCode", lblplantcode.Text)
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "sp_GetCategory");
            ddlCategory.DataSource = Dt;
            ddlCategory.DataTextField = "Category_Name";
            ddlCategory.DataValueField = "Category_Id";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("Select Category", "0"));
            //drpShift.Enabled = false;
            //drpShift_SelectedIndexChanged(null, null);
        }
        protected void getDowntype()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@opr", "1")
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "sp_GetDowntimetype");
            ddlActivity.DataSource = Dt;
            ddlActivity.DataTextField = "DTDescription";
            ddlActivity.DataValueField = "DTID";
            ddlActivity.DataBind();


            ddlActivity.Items.Insert(0, new ListItem("Select Downtime Type", "0"));
            //drpShift.Enabled = false;
            //drpShift_SelectedIndexChanged(null, null);
        }

        protected void getTPMLosses()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@opr", "3")
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "sp_GetDowntimetype");
            ddlTPMLoss.DataSource = Dt;
            ddlTPMLoss.DataTextField = "TPMLossDescription";
            ddlTPMLoss.DataValueField = "TPMID";
            ddlTPMLoss.DataBind();
            ddlTPMLoss.Items.Insert(0, new ListItem("Select TPM loss", "0"));
        }
        protected void GetReasonStationWise()
        {
            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@OPR", "1"),
                 new SqlParameter("@StationCode", drpStation.SelectedValue),
                 new SqlParameter("@plantCode", lblplantcode.Text)
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "sp_getReaasons");
            if (Dt.Rows.Count > 0)
            {
                drpReason.DataSource = Dt;
                drpReason.DataTextField = "ReasonDescription";
                drpReason.DataValueField = "ReasonCode";
                drpReason.DataBind();

                drpReason.Items.Insert(0, new ListItem("Select Reason", "0"));
                ddlCategory.SelectedIndex = -1;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('No reasons present for this Station!!')", true);
            }
        }
        protected void GetReasonCatWise()
        {
            if (drpStation.SelectedValue != "0")
            {
                SqlParameter[] param = new SqlParameter[]
                {
                 new SqlParameter("@OPR", "2"),
                 new SqlParameter("@StationCode", drpStation.SelectedValue),
                 new SqlParameter("@catid", ddlCategory.SelectedValue),
                 new SqlParameter("@plantCode", lblplantcode.Text)
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "sp_getReaasons");
                if (Dt.Rows.Count > 0)
                {
                    drpReason.DataSource = Dt;
                    drpReason.DataTextField = "ReasonDescription";
                    drpReason.DataValueField = "ReasonCode";
                    drpReason.DataBind();
                    drpReason.Items.Insert(0, new ListItem("Select Reason", "0"));
                }
                else
                {
                    drpReason.SelectedIndex = -1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('No reason list available for this category!!')", true);
                }
                //drpReason.Items.Insert(0, new ListItem("Select Reason", "0"));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select Station!!')", true);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (btnSubmit.Text == "Add")
            {
                if (ddlHour.SelectedValue.Equals("00") && ddlMinute.SelectedValue.Equals("00"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Duration cannot be 00,Please select again Duration hour/Minute')", true);
                }
                else
                {
                    UserSelectionModel model = new UserSelectionModel();
                    model = (UserSelectionModel)Session["UserSelectionModel"];
                    var date = model.ShiftDate.ToString().Substring(0, model.ShiftDate.IndexOf(' '));
                    var date1 = model.ShiftDate.ToString();
                    // DateTime dateTime1 = DateTime.Parse(date1);
                    //date = Convert.ToDateTime(lbldate.Text).ToString("dd/MM/yyyy");
                    //DateTime dateTime =Convert.ToDateTime(Convert.ToDateTime(lbldate.Text).ToString("dd/MM/yyyy"));// DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                    object obj;
                    SqlParameter[] param = new SqlParameter[]
                    {
                new SqlParameter("@OPR", 1),
                new SqlParameter("@AutoId", null),
                new SqlParameter("@Date", Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd")),
                new SqlParameter("@Time", drpTime.SelectedValue),
                new SqlParameter("@ShiftCode",drpShift.SelectedValue),
                new SqlParameter("@StationCode",drpStation.SelectedValue),
                new SqlParameter("@ReasonCode",drpReason.SelectedValue),
                new SqlParameter("@DownTime",ddlHour.SelectedItem.Text +":"+ddlMinute.SelectedItem.Text),
                new SqlParameter("@Line",lblline.Text),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@CreatedBy",HttpContext.Current.Session["username"].ToString()),
                new SqlParameter("@EntryTime",date1),
                new SqlParameter("@comments",txtComment.Text),
                new SqlParameter("@category",ddlCategory.SelectedItem.Text),
                new SqlParameter("@downtimetype",ddlActivity.SelectedItem.Text),
                new SqlParameter("@tpmloss",ddlTPMLoss.SelectedItem.Text)
                    };

                    obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
                    BindStoppagesReason();
                    clear();
                    if (obj != null)
                    {

                        //GrdStoppageReason.EditIndex = -1;
                        //drpStation.SelectedIndex = 0;
                        //drpReason.SelectedIndex = 0;
                        //ddlHour.SelectedIndex = 0;
                        //ddlMinute.SelectedIndex = 0;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Stoppage Reason Entry added successfully')", true);
                    }
                }
            }
            else
            {
                if (ddlHour.SelectedValue.Equals("00") && ddlMinute.SelectedValue.Equals("00"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Duration cannot be 00,Please select again Duration hour/Minute')", true);
                }
                else
                {
                    UserSelectionModel model = new UserSelectionModel();
                    model = (UserSelectionModel)Session["UserSelectionModel"];
                    var date = model.ShiftDate.ToString().Substring(0, model.ShiftDate.IndexOf(' '));
                    var date1 = model.ShiftDate.ToString();
                    // DateTime dateTime1 = DateTime.Parse(date1);
                    //date = Convert.ToDateTime(lbldate.Text).ToString("dd/MM/yyyy");
                    //DateTime dateTime =Convert.ToDateTime(Convert.ToDateTime(lbldate.Text).ToString("dd/MM/yyyy"));// DateTime.ParseExact(date.Replace("-", "/"), "dd/MM/yyyy", null);
                    object obj;
                    SqlParameter[] param = new SqlParameter[]
                    {
                new SqlParameter("@OPR", 1),
                new SqlParameter("@AutoId", Convert.ToString(Session["Autoid"])),
                new SqlParameter("@Date", Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd")),
                new SqlParameter("@Time", drpTime.SelectedValue),
                new SqlParameter("@ShiftCode",drpShift.SelectedValue),
                new SqlParameter("@StationCode",drpStation.SelectedValue),
                new SqlParameter("@ReasonCode",drpReason.SelectedValue),
                new SqlParameter("@DownTime",ddlHour.SelectedItem.Text +":"+ddlMinute.SelectedItem.Text),
                new SqlParameter("@Line",lblline.Text),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@CreatedBy",HttpContext.Current.Session["username"].ToString()),
                new SqlParameter("@EntryTime",date1),
                new SqlParameter("@comments",txtComment.Text),
                new SqlParameter("@category",ddlCategory.SelectedItem.Text),
                new SqlParameter("@downtimetype",ddlActivity.SelectedItem.Text),
                new SqlParameter("@tpmloss",ddlTPMLoss.SelectedItem.Text)
                    };

                    obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
                    GrdStoppageReason.EditIndex = -1;
                    BindStoppagesReason();
                    clear();
                    if (obj != null)
                    {

                        //drpStation.SelectedIndex = 0;
                        //drpReason.SelectedIndex = 0;
                        //ddlHour.SelectedIndex = 0;
                        //ddlMinute.SelectedIndex = 0;
                        //ddlActivity.SelectedIndex = 0;
                        //ddlTPMLoss.SelectedIndex = 0;
                        //txtComment.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Stoppage Reason Entry added successfully')", true);
                    }

                    //BindStoppagesReason();
                }
            }
            bindtimeslot(Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd"));
        }

        protected void clear()
        {
            drpStation.SelectedIndex = 0;
            drpReason.SelectedIndex = 0;
            ddlHour.SelectedIndex = 0;
            ddlMinute.SelectedIndex = 0;
            ddlActivity.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            ddlTPMLoss.SelectedIndex = 0;
            btnSubmit.Text = "Add";
            btnCancel.Visible = false;
            txtComment.Text = string.Empty;
        }

        protected void drpStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetReason();
            //getCategory();
            //getDowntype();
            GetReasonStationWise();
        }

        public void BindStoppagesReason()
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@OPR", 3),
                 new SqlParameter("@Line",lblline.Text),
                 new SqlParameter("@PlantCode", lblplantcode.Text),
                 new SqlParameter("@Date",Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd")),
                 new SqlParameter("@ShiftCode", lblshift.Text),
             };
            dt = DBClass.GetDatatable_WithParam(param, "SP_GET_DATA");
            GrdStoppageReason.DataSource = dt;
            GrdStoppageReason.DataBind();
            //foreach (GridViewRow dataRow in GrdStoppageReason.Rows)
            //{
            //    var AutoId = GrdStoppageReason.DataKeys[dataRow.RowIndex].Value.ToString();
            //    if (AutoId.Equals("0"))
            //    {
            //        Button button = (Button)dataRow.Cells[11].Controls[0];
            //        if (button.Text == "Delete")
            //        {
            //            button.Visible = false;
            //        }

            //        Button buttonedit = (Button)dataRow.Cells[10].Controls[0];
            //        if (buttonedit.Text == "Edit")
            //        {
            //            buttonedit.Visible = false;
            //        }
            //    }
            //}
        }

        protected void GrdStoppageReason_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdReasoncode = (e.Row.FindControl("hdReasoncode") as HiddenField);
                if (hdReasoncode.Value.ToUpper() == "TOTAL")
                {
                    e.Row.Attributes["style"] = "background-color: #00B0F0";
                    //e.Row.Attributes["style"] = "color: #FFFFFF";


                    e.Row.Font.Bold = true;
                }
                foreach (Button button in e.Row.Cells[7].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete this record?')){ return false; };";
                    }
                }
            }
            if (e.Row.RowIndex != GrdStoppageReason.EditIndex)
            {
                //(e.Row.Cells[6].Controls[1] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
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
                     new SqlParameter("@currentTime",labeldate),
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
                _logger.Error(string.Concat("GetShiftHeader::", ex.Message), ex);
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
                if (drpShiftHeader.SelectedValue.Equals("S4") || drpShiftHeader.SelectedValue.Equals("PS4") || drpShiftHeader.SelectedValue.Equals("PS1"))
                {
                    lblshiftnew.Text = "S1";
                }
                else if (drpShiftHeader.SelectedValue.Equals("S5") || drpShiftHeader.SelectedValue.Equals("PS5") || drpShiftHeader.SelectedValue.Equals("PS2"))
                {
                    lblshiftnew.Text = "S2";
                }
                else if (drpShiftHeader.SelectedValue.Equals("S6") || drpShiftHeader.SelectedValue.Equals("PS6") || drpShiftHeader.SelectedValue.Equals("PS3"))
                {
                    lblshiftnew.Text = "S3";
                }
                else
                {
                    lblshiftnew.Text = drpShiftHeader.SelectedValue;
                }
                lblshift.Text = drpShiftHeader.SelectedValue;
                lbldate.Text = date;
                labeldate = date;
                txtDate.Text = lbldate.Text;
                GetShift(date);
                GetStation();
                GetDuration();
                BindStoppagesReason();
                //clear();
                bindtimeslot(date);
            }
        }

        protected void drpShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTimeControl(drpTime);
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedValue != "0")
            {
                GetReasonCatWise();
            }
            else
            {
                GetReasonStationWise();
            }
        }

        protected void drpReason_SelectedIndexChanged(object sender, EventArgs e)
        {

            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@OPR", "3"),
                 new SqlParameter("@StationCode", drpStation.SelectedValue),
                 new SqlParameter("@reasoncode", drpReason.SelectedValue),
                 new SqlParameter("@plantCode", lblplantcode.Text)
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "sp_getReaasons");
            if (Dt.Rows.Count > 0)
            {
                //drpReason.DataSource = Dt;
                //drpReason.DataTextField = "ReasonDescription";
                //drpReason.DataValueField = "ReasonCode";
                //drpReason.DataBind();
                //drpReason.Items.Insert(0, new ListItem("Select Reason", "0"));
                ddlCategory.ClearSelection();
                ddlCategory.Items.FindByValue(Convert.ToInt16(Dt.Rows[0]["Category_ID"]).ToString()).Selected = true;
                //ddlCategory.SelectedIndex = Convert.ToInt16(Dt.Rows[0]["Category_ID"]);
                ddlActivity.SelectedIndex = Convert.ToInt16(Dt.Rows[0]["DTID"]);
                ddlTPMLoss.SelectedIndex = Convert.ToInt16(Dt.Rows[0]["TPMLoss"]);
            }
        }

        protected void GrdStoppageReason_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)GrdStoppageReason.Rows[e.RowIndex];
            Int64 AutoId = Convert.ToInt32(GrdStoppageReason.DataKeys[e.RowIndex].Value.ToString());
            object obj;
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", 3),
                new SqlParameter("@AutoId", AutoId),
            };

            obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
            GrdStoppageReason.EditIndex = -1;
            BindStoppagesReason();
            if (obj != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Stoppage Reason Entry deleted successfully')", true);
            }
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            txtComment.Attributes.Add("maxlength", txtComment.MaxLength.ToString());
            lblCommentsMessage.Text = string.Empty;
            GetCommentsDetails();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:count()", true);
            MPComments.Show();

        }

        public void GetCommentsDetails()
        {
            try
            {
                txtComment.Text = string.Empty;
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@OPR", "15"),
                    new SqlParameter("@PlantCode", lblplantcode.Text),
                    new SqlParameter("@Line", lblline.Text),
                    new SqlParameter("@Date", labeldate),
                    new SqlParameter("@ShiftCode", lblshift.Text),
                };
                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    txtComment.Text = Convert.ToString(Dt.Rows[0]["Comments"]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Concat("GetCommentsDetails::", ex.Message), ex);
            }
        }

        protected void btnAddComments_Click(object sender, EventArgs e)
        {
            try
            {
                lblCommentsMessage.Text = string.Empty;
                if (txtComment.Text.Trim() == "")
                {
                    lblCommentsMessage.Text = "Please Enter Comments";
                    lblCommentsMessage.ForeColor = Color.Red;
                }
                else
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@OPR", "9"),
                        new SqlParameter("@PlantCode", lblplantcode.Text),
                        new SqlParameter("@Line", lblline.Text),
                        new SqlParameter("@Date",labeldate),
                        new SqlParameter("@ShiftCode", drpShift.SelectedValue),
                        new SqlParameter("@Comments", txtComment.Text.ToString().Trim()),
                        new SqlParameter("@CreatedBy",Convert.ToString(Session["username"]))
                    };
                    DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_DATA");

                    lblCommentsMessage.Text = "Comments Saved Successfully.";
                    lblCommentsMessage.ForeColor = Color.Green;
                    BindStoppagesReason();
                }
            }
            catch (Exception ex)
            {
                lblCommentsMessage.Text = ex.Message.ToString();
                lblCommentsMessage.ForeColor = Color.Red;
                _logger.Error(string.Concat("btnAddComments_Click::", ex.Message), ex);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:count()", true);
            MPComments.Show();
        }

        protected void btnClearComments_Click(object sender, EventArgs e)
        {
            lblCommentsMessage.Text = string.Empty;
            txtComment.Text = string.Empty;
            MPComments.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:count()", true);
        }

        protected void btnParameter_Click(object sender, EventArgs e)
        {
            Response.Redirect("ParameterDetails.aspx", false);
        }

        protected void btnOkComments_Click(object sender, EventArgs e)
        {
            MPComments.Hide();
            BindStoppagesReason();
        }



        protected void GrdStoppageReason_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewRow row = (GridViewRow)GrdStoppageReason.Rows[e.NewEditIndex];
            Int64 AutoId = Convert.ToInt32(GrdStoppageReason.DataKeys[e.NewEditIndex].Value.ToString());
            Session["Autoid"] = AutoId;
            showdata(Convert.ToString(AutoId));
            row.Cells[10].Visible = false;
            row.Cells[11].Visible = false;
            btnSubmit.Text = "Update";
            btnCancel.Visible = true;
        }

        protected void showdata(string autoid)
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@OPR", 5),
                 new SqlParameter("@autoid",autoid)
             };
            dt = DBClass.GetDatatable_WithParam(param, "SP_GET_DATA");
            if (dt.Rows.Count > 0)
            {

                txtDate.Text = Convert.ToString(dt.Rows[0]["Date"]);
                drpShift.SelectedValue = Convert.ToString(dt.Rows[0]["ShiftCode"]);
                BindTimeeditControl(drpTime, Convert.ToString(dt.Rows[0]["Time"]));
                drpStation.SelectedValue = Convert.ToString(dt.Rows[0]["StationCode"])==""?"0": Convert.ToString(dt.Rows[0]["StationCode"]);
                ddlCategory.SelectedValue = Convert.ToString(dt.Rows[0]["Category"]) == "" ? "0" : Convert.ToString(dt.Rows[0]["Category"]);
                GetReasonCatWiseEdit(Convert.ToString(dt.Rows[0]["ReasonCode"]) == "" ? "0" : Convert.ToString(dt.Rows[0]["Reasoncode"]));
                GetDurationedit(Convert.ToString(dt.Rows[0]["hh"]), Convert.ToString(dt.Rows[0]["mm"]));
                //drpReason.SelectedValue = Convert.ToString(dt.Rows[0]["ReasonCode"]);
                ddlTPMLoss.SelectedValue = Convert.ToString(dt.Rows[0]["TPMLossID"]) == "" ? "0" : Convert.ToString(dt.Rows[0]["TPMLossID"]);
                ddlActivity.SelectedValue = Convert.ToString(dt.Rows[0]["DowntimeType"]) == "" ? "0" : Convert.ToString(dt.Rows[0]["DownTimeType"]);
                txtComment.Text = Convert.ToString(dt.Rows[0]["CommentId"]);
            }
        }

        protected void GetDurationedit(string hh, string mm)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "7")
            };

            DataTable Dt = new DataTable();
            Dt = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_STOPPAGE_DATA");
            ddlHour.DataSource = Dt;
            ddlHour.DataTextField = "Hours";
            ddlHour.DataValueField = "Hours";
            ddlHour.DataBind();
            ddlHour.Items.Insert(0, new ListItem("Select Hour", "0"));
            ddlHour.SelectedValue = hh;

            SqlParameter[] param1 = new SqlParameter[]
            {
                new SqlParameter("@OPR", "8")
            };
            DataTable Dtt = new DataTable();
            Dtt = DBClass.GetDatatable_WithParam(param1, "DM_SP_GET_STOPPAGE_DATA");
            ddlMinute.DataSource = Dtt;
            ddlMinute.DataTextField = "Minutes";
            ddlMinute.DataValueField = "Minutes";
            ddlMinute.DataBind();
            ddlMinute.Items.Insert(0, new ListItem("Select Minute", "0"));
            ddlMinute.SelectedValue = mm;
        }

        protected void GetReasonCatWiseEdit(string reasoncode)
        {
            if (drpStation.SelectedValue != "0")
            {
                SqlParameter[] param = new SqlParameter[]
                {
                 new SqlParameter("@OPR", "2"),
                 new SqlParameter("@StationCode", drpStation.SelectedValue),
                 new SqlParameter("@catid", ddlCategory.SelectedValue),
                 new SqlParameter("@plantCode", lblplantcode.Text)
                };

                DataTable Dt = new DataTable();
                Dt = DBClass.GetDatatable_WithParam(param, "sp_getReaasons");
                if (Dt.Rows.Count > 0)
                {
                    drpReason.DataSource = Dt;
                    drpReason.DataTextField = "ReasonDescription";
                    drpReason.DataValueField = "ReasonCode";
                    drpReason.DataBind();
                    drpReason.Items.Insert(0, new ListItem("Select Reason", "0"));
                    drpReason.SelectedValue = reasoncode;
                }
                else
                {
                    drpReason.SelectedIndex = -1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('No reason list available for this category!!')", true);
                }
                //drpReason.Items.Insert(0, new ListItem("Select Reason", "0"));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Please select Station!!')", true);
            }
        }

        public void BindTimeeditControl(DropDownList drp, string time)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@OPR", "14"),
                new SqlParameter("@ShiftCode", drpShift.SelectedValue),
                new SqlParameter("@PlantCode", lblplantcode.Text),
                new SqlParameter("@ShiftDate",Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd"))
            };

            //drp.Items.Clear();
            DataTable DtTimes = new DataTable();
            DtTimes = DBClass.GetDatatable_WithParam(param, "DM_SP_GET_DATA");
            drp.DataSource = DtTimes;
            drp.DataValueField = "TimeValue";
            drp.DataTextField = "TimeValue";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("HH:MM", ""));
            drp.SelectedValue = time;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            btnSubmit.Text = "Add";
            btnCancel.Visible = false;
            GrdStoppageReason.EditIndex = -1;
            BindStoppagesReason();
        }

        protected void GrdStoppageReason_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditStoppage")
            {
                Int64 AutoId = Convert.ToInt32(e.CommandArgument);
                Session["Autoid"] = AutoId;
                showdata(Convert.ToString(AutoId));
                //row.Cells[10].Visible = false;
                //row.Cells[11].Visible = false;
                GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;
                row.Cells[10].Visible = false;
                row.Cells[11].Visible = false;
                btnSubmit.Text = "Update";
                btnCancel.Visible = true;
            }
            else if (e.CommandName == "DeleteStoppage")
            {
                Int64 AutoId = Convert.ToInt32(e.CommandArgument);
                object obj;
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@OPR", 3),
                new SqlParameter("@AutoId", AutoId),
                };

                obj = DBClass.ExecuteNonQuery_WithParam(param, "DM_SP_DML_STOPPAGE_DATA");
                GrdStoppageReason.EditIndex = -1;
                BindStoppagesReason();
                if (obj != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "javascript:alert('Stoppage Reason Entry deleted successfully')", true);
                }
                bindtimeslot(Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd"));
            }
        }

        protected void ddlWrapping_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindtimeslot(Convert.ToDateTime(lbldate.Text).ToString("yyyy-MM-dd"));
        }

        //protected void GrdStoppageReason_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //}

        //protected void GrdStoppageReason_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        //{
        //}
    }
}