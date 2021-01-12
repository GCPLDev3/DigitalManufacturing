using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MFG_DigitalApp
{
    public class clsSensorQty
    {
        public String QueryString = "";
        public string Plant = "", LineNo = "", Shift = "", StrDate = "";
        public string ShiftStartTime = DateTime.Now.ToString();
        public string ShiftEndTime = DateTime.Now.ToString();
        DataSet ds_CartonPackings = new DataSet();
        public string PC_Column_01 = "", PC_Column_02 = "", PC_Column_03 = "";

        public void GetCartonPackingShiftDataWithHoursBy(string ShiftTableName, DataTable FullShiftDataTable, DataSet ds_CartonPacking, string Column)
        {
            if (FullShiftDataTable != null)
            {
                if (FullShiftDataTable.Rows.Count > 0)
                {
                    DataTable ShiftDataTable = new DataTable(ShiftTableName);
                    MakeDataTable(ShiftDataTable, FullShiftDataTable, Column);
                    DataRow NewRow = ShiftDataTable.NewRow();
                    NewRow["DateTimeCol"] = "Total";
                    NewRow["SensorQty"] = ShiftDataTable.AsEnumerable().Sum(x => Convert.ToDecimal(x["SensorQty"]));
                    ShiftDataTable.Rows.Add(NewRow);
                    //GridViewShiftData.DataSource = ShiftDataTable;  //Gridview
                    //GridViewShiftData.DataBind(); //binding data to Gridview
                    ds_CartonPacking.Tables.Add(ShiftDataTable);
                }
            }
        }

        public void getHoursData(DataTable MainDataTable, DataTable NewDataTable, DateTime dt_from, DateTime dt_to)
        {
            dt_from = Convert.ToDateTime(ShiftStartTime.ToString());
            dt_to = Convert.ToDateTime(ShiftEndTime.ToString());

            if (MainDataTable != null)
            {
                if (MainDataTable.Rows.Count > 0)
                {
                    DataRow[] rows = MainDataTable.Select("ist_time >= #" + dt_from.ToString("yyyy-MM-dd HH:mm:ss") + "# AND ist_time <= #" + dt_to.ToString("yyyy-MM-dd HH:mm:ss") + "#");
                    foreach (DataRow dr in rows)
                    {
                        NewDataTable.ImportRow(dr);
                    }
                }
            }
        }

        public void MakeDataTable(DataTable myTable, DataTable NewDataTable, string ColumnName)
        {
            DataRow myNewRow;
            //ADDING COLUMN
            myTable.Columns.Add("DateTimeCol");
            myTable.Columns.Add("SensorQty", System.Type.GetType("System.Decimal"));

            //ShiftTiming();
            DateTime dt_from = Convert.ToDateTime(ShiftStartTime);
            for (int h = 0; h < 8; h++)
            {
                //ADDING ROW TO DATA-Table
                myNewRow = myTable.NewRow();
                //TimeSpan timespan = new TimeSpan(06, 00, 00);
                DateTime time = Convert.ToDateTime(ShiftStartTime);
                string starthour = time.AddHours(h).ToString("HH:mm");
                string endhour = time.AddHours(h + 1).ToString("HH:mm");
                //dt_from.AddHours(h);
                //DateTime dt_to = dt_from.AddHours(h + 1);
                dt_from = time.AddHours(h);
                DateTime dt_to = time.AddHours(h + 1);

                myNewRow["DateTimeCol"] = starthour + " to " + endhour;

                //decimal sum = Convert.ToDecimal(NewDataTable.Compute("Sum(" + ColumnName + ")", "ist_time >= #" + dt_from.ToString("yyyy-MM-dd HH:mm:ss") + "# AND ist_time <= #" + dt_to.ToString("yyyy-MM-dd HH:mm:ss") + "#"));
                //myNewRow["SensorQty"] = NewDataTable.Compute("Sum(" + ColumnName + ")", "ist_time >= #" + dt_from.ToString("yyyy-MM-dd HH:mm:ss") + "# AND ist_time <= #" + dt_to.ToString("yyyy-MM-dd HH:mm:ss") + "#");

                //myNewRow["SensorQty"] = 0;
                myNewRow["SensorQty"] = NewDataTable.Compute("Sum(" + ColumnName + ")", "ist_time >= #" + dt_from.ToString("yyyy-MM-dd HH:mm:ss") + "# AND ist_time <= #" + dt_to.ToString("yyyy-MM-dd HH:mm:ss") + "#");
                decimal sum = 0;
                if (!Convert.IsDBNull(myNewRow["SensorQty"]))
                {
                    sum = Convert.ToDecimal(myNewRow["SensorQty"]);
                }

                if (LineNo == "MT Devlop 1" || LineNo == "MT Devlop 2")
                {
                    sum = sum * 5;
                }
                else if (LineNo == "MT Cream 1" || LineNo == "MT Cream 2")
                {
                    sum = sum * 7;
                }
                myNewRow["SensorQty"] = sum;
                myTable.Rows.Add(myNewRow);
            }
        }

        public void AddColumnsAndPullData(DataTable MainDataTable, DataTable NewDataTable, List<Dictionary<String, String>> items)
        {
            foreach (var item in items)
            {
                foreach (KeyValuePair<String, String> pair in item)
                {
                    if (pair.Key == pair.Value)
                    {
                        if (pair.Key == "ist_time")
                        {
                            MainDataTable.Columns.Add(pair.Key, System.Type.GetType("System.DateTime"));
                            NewDataTable.Columns.Add(pair.Key, System.Type.GetType("System.DateTime"));
                        }
                        else
                        if (pair.Key == "noc_1" || pair.Key == "noc_2" || pair.Key == "cp_1" || pair.Key == "cp_2" || pair.Key == "cp_3" || pair.Key == "fgp" || pair.Key == "sm3_cp" || pair.Key == "mc_1_fgp" || pair.Key == "mc_2_fgp" || pair.Key == "mc_3_fgp" || pair.Key == "mc_4_fgp" || pair.Key == "mc_5_fgp" || pair.Key == "mc_6_fgp" || pair.Key == "mtrack1_cut" || pair.Key == "mtrack2_cut" || pair.Key == "mtrack3_cut" || pair.Key == "mtrack4_cut")
                        {
                            MainDataTable.Columns.Add(pair.Key, System.Type.GetType("System.Decimal"));
                            NewDataTable.Columns.Add(pair.Key, System.Type.GetType("System.Decimal"));
                            MainDataTable.Columns[pair.Key].DefaultValue = 0.0;
                            NewDataTable.Columns[pair.Key].DefaultValue = 0.0;
                        }
                        else
                        {
                            MainDataTable.Columns.Add(pair.Key);
                            NewDataTable.Columns.Add(pair.Key);
                        }
                    }
                }

                DataRow row;
                row = MainDataTable.NewRow();
                foreach (KeyValuePair<String, String> pair in item)
                {
                    if (pair.Key != pair.Value)
                    {
                        row[pair.Key] = pair.Value;
                    }
                }
                MainDataTable.Rows.Add(row);
            }
        }

        public void ShiftTiming(string Shift)
        {
            if (Shift == "S1")
            {
                ShiftStartTime = StrDate + " 06:00:00";
                ShiftEndTime = StrDate + " 13:59:59";
            }
            else if (Shift == "S2")
            {
                ShiftStartTime = StrDate + " 14:00:00";
                ShiftEndTime = StrDate + " 21:59:59";
            }
            else if (Shift == "S3")
            {
                ShiftStartTime = StrDate + " 22:00:00";
                DateTime enteredDate = DateTime.Parse(StrDate);
                //DateTime enteredDate = DateTime.ParseExact(StrDate.Replace("-", "/"), "dd/MM/yyyy", null);
                StrDate = enteredDate.AddDays(1).ToString("yyyy-MM-dd");
                ShiftEndTime = StrDate + " 05:59:59";
            }
            this.ShiftStartTime = ShiftStartTime;
            this.ShiftEndTime = ShiftEndTime;
        }

        public string GetShiftTableDataBy(string Plant, string LineNo, string Shift, string StrDate)
        {
            string table = "", QueryString = "";
            string column = "";
            ShiftStartTime = DateTime.Now.ToString();
            ShiftEndTime = DateTime.Now.ToString();
            this.Plant = Plant;
            this.LineNo = LineNo;
            this.Shift = Shift;
            this.StrDate = StrDate;

            ShiftTiming(Shift);

            if (Plant == "M016" && LineNo == "LINE 2")
            {
                column = "ist_time, noc_1, noc_2";
                table = "katha_soap_line2_a1_data_view";
                PC_Column_01 = "noc_1";
                PC_Column_02 = "noc_2";
            }
            else if (Plant == "M015" && LineNo == "LINE 1")
            {
                column = "ist_time, cp_1, cp_2, cp_3";
                table = "thana_soap_line1_a1_data_view";
                PC_Column_01 = "cp_1";
                PC_Column_02 = "cp_2";
                PC_Column_03 = "cp_3";
            }
            else if (Plant == "M020" && LineNo == "LINE 5")
            {
                column = "ist_time, fgp";
                table = "N_Conso_Refill_Line5_a1_data_view";
                PC_Column_01 = "fgp";
            }
            else if (Plant == "M020" && LineNo == "LINE 6")
            {
                column = "ist_time, fgp";
                table = "N_Conso_Refill_Line6_a1_data_view";
                PC_Column_01 = "fgp";
            }
            else if (Plant == "M020" && LineNo == "LINE 7")
            {
                column = "ist_time, fgp";
                table = "N_Conso_Refill_Line7_a1_data_view";
                PC_Column_01 = "fgp";
            }
            else if (Plant == "M020" && LineNo == "LINE 8")
            {
                column = "ist_time, fgp";
                table = "N_Conso_Refill_Line8_A1_data_view";
                PC_Column_01 = "fgp";
            }
            else if (Plant == "M002" && LineNo == "LINE 3")
            {
                column = "ist_time, sm3_cp";
                table = "pondy_coil_line3_a1_data_view";
                PC_Column_01 = "sm3_cp";
            }
            else if (Plant == "M024" && LineNo == "Autopack 1")
            {
                column = "ist_time, mc_1_fgp";
                table = "Lokhra_Creme_Line_AP_A1_data_view";
                PC_Column_01 = "mc_1_fgp";
            }
            else if (Plant == "M024" && LineNo == "Autopack 1")
            {
                column = "ist_time, mc_1_fgp";
                table = "Lokhra_Creme_Line_AP_A1_data_view";
                PC_Column_01 = "mc_1_fgp";
            }
            else if (Plant == "M024" && LineNo == "Autopack 2")
            {
                column = "ist_time, mc_2_fgp";
                table = "Lokhra_Creme_Line_AP_A1_data_view";
                PC_Column_01 = "mc_2_fgp";
            }
            else if (Plant == "M024" && LineNo == "Autopack 3")
            {
                column = "ist_time, mc_3_fgp";
                table = "Lokhra_Creme_Line_AP_A1_data_view";
                PC_Column_01 = "mc_3_fgp";
            }
            else if (Plant == "M024" && LineNo == "Autopack 4")
            {
                column = "ist_time, mc_4_fgp";
                table = "Lokhra_Creme_Line_AP_A1_data_view";
                PC_Column_01 = "mc_4_fgp";
            }
            else if (Plant == "M024" && LineNo == "Autopack 5")
            {
                column = "ist_time, mc_5_fgp";
                table = "Lokhra_Creme_Line_AP_A1_data_view";
                PC_Column_01 = "mc_5_fgp";
            }
            else if (Plant == "M024" && LineNo == "Autopack 6")
            {
                column = "ist_time, mc_6_fgp";
                table = "Lokhra_Creme_Line_AP_A1_data_view";
                PC_Column_01 = "mc_6_fgp";
            }

            else if (Plant == "M024" && LineNo == "MT Devlop 1")
            {
                column = "ist_time, mtrack1_cut";
                table = "Lokhra_Creme_Line_MT_A1_data_view";
                PC_Column_01 = "mtrack1_cut";
            }
            else if (Plant == "M024" && LineNo == "MT Cream 1")
            {
                column = "ist_time, mtrack2_cut";
                table = "Lokhra_Creme_Line_MT_A1_data_view";
                PC_Column_01 = "mtrack2_cut";
            }
            else if (Plant == "M024" && LineNo == "MT Devlop 2")
            {
                column = "ist_time, mtrack3_cut";
                table = "Lokhra_Creme_Line_MT_A1_data_view";
                PC_Column_01 = "mtrack3_cut";
            }
            else if (Plant == "M024" && LineNo == "MT Cream 2")
            {
                column = "ist_time, mtrack4_cut";
                table = "Lokhra_Creme_Line_MT_A1_data_view";
                PC_Column_01 = "mtrack4_cut";
            }
            else
            {
                return QueryString;
            }

            QueryString = "select " + column + " from " + table + " WHERE ist_time BETWEEN TIMESTAMP '" + ShiftStartTime + "' and TIMESTAMP '" + ShiftEndTime + "'";
            return QueryString;
        }
    }
}