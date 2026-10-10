using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using a=Microsoft.Office.Interop.Excel;
using System.Windows;

namespace PE
{
    class clsExcel
    {
        //ClsMySql objClsMySql;

        static ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال

        #region کشتن همه پروسسهای اکسل
        private static void KillExcelProcess()
        {
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                try
                {
                    p.Kill();
                }
                catch
                { }

            }
        }//;کشتن همه پروسسهای اکسل 
        #endregion

        #region ریلیز حافظه های اشیا
        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "  releaseObject ");
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }




        #endregion


        public clsExcel()
        {
            objClsSql = new ClsSql();
            
        }

        #region شیت

        public static List<string> ExtractSheeName(string Import_FileName)
        {
            KillExcelProcess();

            var lstExtratSheets = new List<String>();

            using (OleDbConnection conn = new OleDbConnection())
            {

                string fileExtension = Path.GetExtension(Import_FileName);

                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=yes;TypeGuessRows=0;ImportMixedType s=Text\"";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=yes;TypeGuessRows=0;ImportMixedType s=Text\"";

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(Import_FileName);

                int intSheetCount = workbook.Sheets.Count;
                string[] strWorkSheet = new string[15];

                //Microsoft.Office.Interop.Excel.Sheets excelSheets = workbook.Worksheets;


                for (int i = 0; i < intSheetCount; i++)
                {


                    lstExtratSheets.Add(workbook.Worksheets[i + 1].Name);


                }
              
                return (lstExtratSheets);
            }


            

        }

        public static string ExtractSheeNameToCommaString(string Import_FileName)
        {
            KillExcelProcess();

            string strExtratSheets = string.Empty;

            using (OleDbConnection conn = new OleDbConnection())
            {

                string fileExtension = Path.GetExtension(Import_FileName);

                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=yes;TypeGuessRows=0;ImportMixedType s=Text\"";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=yes;TypeGuessRows=0;ImportMixedType s=Text\"";

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(Import_FileName);

                int intSheetCount = workbook.Sheets.Count;
                //string[] strWorkSheet = new string[15];



                for (int i = 0; i < intSheetCount; i++)
                {
                    strExtratSheets += (workbook.Worksheets[i + 1].Name) + ",";
                }

                return (strExtratSheets);
            }
        }

        public static int CountSheet(string path)
        {

            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (OleDbConnection conn = new OleDbConnection())
            {
                string Import_FileName = path;

                string fileExtension = Path.GetExtension(Import_FileName);

                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);

                return (workbook.Sheets.Count);
            }




        }

        public static bool CheckSheetCount(string strXclFileName, int intExceptedSheetCount)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            int numSheets = clsExcel.CountSheet(strXclFileName);

            if (numSheets == intExceptedSheetCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<string> CompairExistSheetWithArrayExtractExtraSheet(string path, string[] strArrayOfExceptedSheetsName)
        {
            var lstExtraSheets = new List<String>();

            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (OleDbConnection conn = new OleDbConnection())
            {
                string Import_FileName = path;

                string fileExtension = Path.GetExtension(Import_FileName);

                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=yes;TypeGuessRows=0;ImportMixedType s=Text\"";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=yes;TypeGuessRows=0;ImportMixedType s=Text\"";

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);

                int intSheetCount = workbook.Sheets.Count;
                string[] strWorkSheet = new string[15];


                for (int i = 0; i < intSheetCount; i++)
                {
                    strWorkSheet[i] = workbook.Worksheets[i + 1].Name;
                    if (strArrayOfExceptedSheetsName.Contains(strWorkSheet[i]))
                    {

                    }
                    else
                    {
                        lstExtraSheets.Add(strWorkSheet[i]);
                    }

                }

                return (lstExtraSheets);
            }

        }

     

        public static bool deleteListOfSheets(string path, List<string> sheetsList)
        {
            #region کشتن پروسسهای اکسل

            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }
            #endregion

            using (OleDbConnection conn = new OleDbConnection())
            {

                #region ایجاد کانکشن استرینگ
                string fileExtension = Path.GetExtension(path);

                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=yes;TypeGuessRows=0;ImportMixedType s=Text\"";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=yes;TypeGuessRows=0;ImportMixedType s=Text\"";
                #endregion

                #region تعریف ورک بوک
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();

                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);
                #endregion

                #region حذف آرایه ای از شیتها
                foreach (string SheetsName in sheetsList)
                {

                    app.DisplayAlerts = false;

                    Microsoft.Office.Interop.Excel.Worksheet wkSheet = (Microsoft.Office.Interop.Excel.Worksheet)app.ActiveWorkbook.Worksheets[SheetsName];

                    wkSheet.Delete();

                    app.DisplayAlerts = true;

                }
                #endregion

                #region ذخیره مجدد ورک بوک حذفیات جدید
                workbook.Save();
                #endregion

                return (true);
            }
        }

        #endregion

        #region ستون
        public static string[] ExtractColumns(string sheetName, string filePath)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            string[] array1;

            //var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 16.0;IMEX=1;HDR=Yes;TypeGuessRows=0;ImportMixedTypes=Text\""; ;
            var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=Yes;TypeGuessRows=0;ImportMixedTypes=Text\""; ;
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                DataTable dt = new DataTable();

                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 * FROM [" + sheetName + "$" + "] ";
                    var adapter = new OleDbDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                int intCountOfField = dt.Columns.Count;

                array1 = new string[intCountOfField];
                int i = 0;
                foreach (DataColumn column in dt.Columns)
                {
                    array1[i] = column.ToString();
                    i++;
                    // intCountOfField++;
                    //Do something with your columns
                }

            }
            return array1;
        }

        public static List<string> ExtractSelectedColumns(string sheetName, string filePath, String[] strSelectedColumn)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            List<string> listColumn = new List<string>();

            var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=Yes;TypeGuessRows=0;ImportMixedTypes=Text\""; ;

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                DataTable dt = new DataTable();

                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });





                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT TOP 1 * FROM [" + sheetName + "$" + "] ";
                    var adapter = new OleDbDataAdapter(cmd);
                    adapter.Fill(dt);




                    foreach (string s in strSelectedColumn)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (s == column.ToString().Trim())
                            {
                                listColumn.Add(s);
                                continue;
                            }
                        }
                    }

                  ;
                }



            }
            return listColumn;
        }
        #endregion

        #region سلول
        public static string GetCellAddress(int row, int col)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            StringBuilder sb = new StringBuilder();
            col--;
            do
            {
                sb.Insert(0, (char)('A' + (col % 26)));
                col /= 26;
            } while (col > 0);
            sb.Append(row);
            return sb.ToString();
        }

        public static void ReadExceldeletUntil(string path, string cell)
        {

            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (OleDbConnection conn = new OleDbConnection())
            {
                string Import_FileName = path;

                string fileExtension = Path.GetExtension(Import_FileName);

                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);


            }
        }
        public static string ReadCell(string sheetName, string path, string strCellAddress)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            using (OleDbConnection conn = new OleDbConnection())
            {
                DataTable dt = new DataTable();
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=FALSE;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=FALSE;'";

                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + sheetName + "$" + strCellAddress + ":" + strCellAddress + "]";

                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;


                        da.SelectCommand = comm;

                        da.Fill(dt);
                        return dt.Columns[0].ToString();
                    }

                }
            }
        }

        private void DeleteCells(string strXclFileName, string strCondition)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            // create excel-instance:
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            // open the concrete file:
            Microsoft.Office.Interop.Excel.Workbook excelWorkbook = excel.Workbooks.Open(strXclFileName);
            // select worksheet. NOT zero-based!!:
            Microsoft.Office.Interop.Excel._Worksheet excelWorkbookWorksheet = excelWorkbook.Sheets[1];
            // create a range:
            Microsoft.Office.Interop.Excel.Range usedRange = excelWorkbookWorksheet.UsedRange;

            // iterate range
            foreach (Microsoft.Office.Interop.Excel.Range r in usedRange)
            {
                // check condition:
                if (r.Value2 == strCondition)
                    // if match, delete and shift remaining cells up:
                    r.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
            }

            // save changes (!!):
            excelWorkbook.Save();

            // cleanup:
            if (excel != null)
            {
                Process[] pProcess;
                pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                pProcess[0].Kill();
            }
        }

        #endregion

        #region دیتا تیبل
        public static DataTable ReadExcelFileSendToDt(string sheetName, string path)
        {
            string stShtName = sheetName;
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion

            using (OleDbConnection conn = new OleDbConnection())
            {
                DataTable dt = new DataTable();

                #region ساخت کانکشن استرینگ
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";

                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                                           // "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath +             ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;\"";
                #endregion

                if (sheetName == string.Empty)
                {
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                    sheetName = worksheet.Name;
                }

                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + stShtName + "$]";

                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);

                        try
                        {
                            objClsSql = new ClsSql();
                            dt = objClsSql.RemoveNullColumnFromDataTable(dt);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex + "\n  RemoveNullColumnFromDataTable ");
                        }

                        return dt;
                    }
                }
            }

        }

        public static DataTable ReadExcelFileSendToDt(string sheetName, string path, bool isFirstRowHeader)
        {
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (OleDbConnection conn = new OleDbConnection())
            {

                DataTable dt = new DataTable();
                string strYesNo = isFirstRowHeader ? "Yes" : "No";
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=" + strYesNo + ";'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=" + strYesNo + ";'";

                if (sheetName == string.Empty)
                {
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                    sheetName = worksheet.Name;
                }

                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + sheetName + "$]";

                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        objClsSql = new ClsSql();
                        dt = objClsSql.RemoveNullColumnFromDataTable(dt);


                        return dt;
                    }
                }
            }

        }

        public static DataTable ReadExcelFromStartRangUntilEndFileSendToDt(string sheetName, string strStartRange, string path)
        {
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (OleDbConnection conn = new OleDbConnection())
            {

                DataTable dt = new DataTable();
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";


                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                sheetName = worksheet.Name;




                Microsoft.Office.Interop.Excel.Range xlRange = worksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                //string columnName = xlWorksheet.Columns[c].Address;
                string str = worksheet.Columns[colCount].Address;
                string strLastColumnName = worksheet.Columns[colCount].Address.Substring(0, 2);

                string strRange = strStartRange + ":" + strLastColumnName.Remove(0, 1) + (rowCount).ToString();

                using (OleDbCommand comm = new OleDbCommand())
                {
                    //comm.CommandText = "Select * from [" + sheetName + "$ $" + sheetName + "]";
                    comm.CommandText = @"Select * from [" + sheetName + "$" + strRange + "]";

                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        objClsSql = new ClsSql();
                        dt = objClsSql.RemoveNullColumnFromDataTable(dt);
                        return dt;
                    }
                }
            }

        }

        public static DataTable ReadExcelFromStartRangUntilEndFileSendToDtNew(string sheetName, string strStartRange, string path)
        {
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (OleDbConnection conn = new OleDbConnection())
            {

                DataTable dt = new DataTable();
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";


                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                sheetName = worksheet.Name;




                Microsoft.Office.Interop.Excel.Range xlRange = worksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                //string columnName = xlWorksheet.Columns[c].Address;
                string str = worksheet.Columns[colCount].Address;
                string strLastColumnName = worksheet.Columns[colCount].Address.Substring(0, 2);

                string strRange = strStartRange + ":" + strLastColumnName.Remove(0, 1) + (rowCount).ToString();

                using (OleDbCommand comm = new OleDbCommand())
                {
                    //comm.CommandText = "Select * from [" + sheetName + "$ $" + sheetName + "]";
                    comm.CommandText = @"Select * from [" + sheetName + "$" + strRange + "]";

                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        objClsSql = new ClsSql();
                        dt = objClsSql.RemoveNullColumnFromDataTable(dt);
                        return dt;
                    }
                }
            }

        }

        public static DataTable ReadExcelFileSendToDtFull(string sheetName, string path, string strRang, bool isFirstRowHeader)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            using (OleDbConnection conn = new OleDbConnection())
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);

                if (sheetName == string.Empty)
                {

                    Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                    sheetName = worksheet.Name;

                    Microsoft.Office.Interop.Excel.Range xlRange = worksheet.UsedRange;
                    int rowCount = xlRange.Rows.Count;

                }


                DataTable dt = new DataTable();
                string str = isFirstRowHeader ? "Yes" : "No";
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=" + str + ";'";

                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + sheetName + "$" + strRang + "]";

                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        return dt;
                    }


                }
            }
        }

        public static DataTable ReadRangSendToDt(string filePath, string strRang)
        {
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            DataTable dtExcel = new DataTable();
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";

            string strConn;

            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);

            conn.Open();

            DataTable schemaTable = new DataTable();
            schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            DataRow schemaRow = schemaTable.Rows[0];

            string sheet = schemaRow["TABLE_NAME"].ToString().Replace("'", string.Empty);
            if (sheet.Contains("Report"))
            {
                string query = "Select * from [" + sheet + string.Empty + strRang + "]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                dtExcel.Locale = CultureInfo.CurrentCulture;
                daexcel.Fill(dtExcel);
            }
            else
            {
                schemaRow = schemaTable.Rows[1];
                sheet = schemaRow["TABLE_NAME"].ToString().Replace("'", string.Empty);
                string query = "Select * from [" + sheet + string.Empty + strRang + "]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                dtExcel.Locale = CultureInfo.CurrentCulture;
                daexcel.Fill(dtExcel);

            }


            conn.Close();

            return dtExcel;
        }

        public static string CreateTableFromDt(string tableName, DataTable dt)
        {

            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion

            string sqlsc;
            sqlsc = "CREATE TABLE " + tableName + "(";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqlsc += "\n [" + dt.Columns[i].ColumnName.Trim() + "] ";
                string columnType = dt.Columns[i].DataType.ToString();
                switch (columnType)
                {
                    case "System.Int32":
                        sqlsc += " int ";
                        break;
                    case "System.Int64":
                        sqlsc += " bigint ";
                        break;
                    case "System.Int16":
                        sqlsc += " smallint";
                        break;
                    case "System.Byte":
                        sqlsc += " tinyint";
                        break;
                    case "System.Decimal":
                        sqlsc += " decimal ";
                        break;
                    case "System.DateTime":
                        sqlsc += " datetime ";
                        break;
                    case "System.String":
                    default:

                        sqlsc += string.Format(" nvarchar({0}) ", dt.Columns[i].MaxLength == -1 ? "max" : dt.Columns[i].MaxLength.ToString());

                        break;
                }

                if (dt.Columns[i].AutoIncrement)
                    sqlsc += " IDENTITY(" + dt.Columns[i].AutoIncrementSeed.ToString() + "," + dt.Columns[i].AutoIncrementStep.ToString().Trim() + ") ";
                if (!dt.Columns[i].AllowDBNull)
                    sqlsc += " NOT NULL ";
                sqlsc += ",";
            }
            return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";
        }

        public static string CreateSqlStringForCreateTableFromDt(string tableName, DataTable dt)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            string sqlsc;
            sqlsc = "CREATE TABLE " + tableName + "(";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                
                sqlsc += "\n [" + dt.Columns[i].ColumnName.Trim() + "] ";
                //string columnType = dt.Columns[i].DataType.ToString();

                //if (dt.Columns[i].ColumnName != "شناسه قبض برق AC")
                //{
                    sqlsc += string.Format(" nvarchar({0}) ", dt.Columns[i].MaxLength == -1 ? "max" : dt.Columns[i].MaxLength.ToString());
                //}
                //else
                //{
                //    sqlsc += string.Format(" real ", dt.Columns[i].MaxLength == -1 );
                //}



                if (dt.Columns[i].AutoIncrement)
                    sqlsc += " IDENTITY(" + dt.Columns[i].AutoIncrementSeed.ToString() + "," + dt.Columns[i].AutoIncrementStep.ToString().Trim() + ") ";
                if (!dt.Columns[i].AllowDBNull)
                    sqlsc += " NOT NULL ";
                sqlsc += ",";
            }
            return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";
        }

        public static string CreateSqlStringForCreateTableFromDtWithOneColumnnumberFormat(string tableName, DataTable dt)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            string sqlsc;
            sqlsc = "CREATE TABLE " + tableName + "(";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqlsc += "\n [" + dt.Columns[i].ColumnName.Trim() + "] ";
                //string columnType = dt.Columns[i].DataType.ToString();

                sqlsc += string.Format(" nvarchar({0}) ", dt.Columns[i].MaxLength == -1 ? "max" : dt.Columns[i].MaxLength.ToString());

                if (dt.Columns[i].AutoIncrement)
                    sqlsc += " IDENTITY(" + dt.Columns[i].AutoIncrementSeed.ToString() + "," + dt.Columns[i].AutoIncrementStep.ToString().Trim() + ") ";
                if (!dt.Columns[i].AllowDBNull)
                    sqlsc += " NOT NULL ";
                sqlsc += ",";
            }
            return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";
        }

        public static void DeleteBlankCells(string tableName)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkBook.Application.ScreenUpdating = false;
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            Microsoft.Office.Interop.Excel.Range DR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Columns["A:A"];
            DR.Select();
            DR.Delete();
            Microsoft.Office.Interop.Excel.Range ACol = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Columns["A:A"];
            Microsoft.Office.Interop.Excel.Range Ronge = ACol.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeBlanks);
            Ronge.EntireRow.Delete();
            Microsoft.Office.Interop.Excel.Range A1 = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            A1.Select();
            xlWorkBook.Application.ScreenUpdating = true;
        }

        public static DataTable excelToDataTable(string findedXclFileName)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            #region ساخت یک فایل اکسل از روی فایل قبلی جهت رفع مشکل فایل باز

            //string strTempXcl = frmMain.strPathDbXclFiles + @"\" + "NewestDb.xlsx";
            // File.Copy(frmMain.strPathDbXclFiles + @"\" + findedXclFileName, strTempXcl, true);

            #endregion

            DataTable dtExcel = new DataTable();

            #region بررسی ورژن اکسل و داشتن هدر
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (findedXclFileName.Substring(findedXclFileName.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + findedXclFileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + findedXclFileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            #endregion

            #region ایجاد کانکشن با اکسل
            OleDbConnection conn = new OleDbConnection(strConn);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            #endregion

            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            DataRow schemaRow = schemaTable.Rows[0];

            string sheet = schemaTable.Rows[0].Field<string>("TABLE_NAME");//بدست آوردن اولین شیت
            if (!sheet.EndsWith("_"))
            {
                string query = "SELECT  * FROM [" + sheet + "]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                dtExcel.Locale = System.Globalization.CultureInfo.CurrentCulture;
                daexcel.Fill(dtExcel);
            }

            conn.Close();
            conn.Dispose();
            return dtExcel;
        }

        public static DataTable ColumnOfexcelToDataTable(string findedXclFileName, string ColumnName, string sheetname)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            #region ساخت یک فایل اکسل از روی فایل قبلی جهت رفع مشکل فایل باز

            //string strTempXcl = frmMain.strPathDbXclFiles + @"\" + "NewestDb.xlsx";
            // File.Copy(frmMain.strPathDbXclFiles + @"\" + findedXclFileName, strTempXcl, true);

            #endregion

            DataTable dtColumnOfExcel = new DataTable();

            #region بررسی ورژن اکسل و داشتن هدر
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (findedXclFileName.Substring(findedXclFileName.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + findedXclFileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + findedXclFileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            #endregion

            #region ایجاد کانکشن با اکسل
            OleDbConnection conn = new OleDbConnection(strConn);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            #endregion

            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            DataRow schemaRow = schemaTable.Rows[0];

            //string sheet = schemaTable.Rows[0].Field<string>("TABLE_NAME");//بدست آوردن اولین شیت

          
            
            string query = "SELECT [شناسه قبض برق AC] FROM [" + sheetname+"$]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
            daexcel.Fill(schemaTable);

            //DataTable dt = new DataTable();
            //dt.Columns.Add(ColumnName, typeof(string));
            //    //dtColumnOfExcel.Locale = System.Globalization.CultureInfo.CurrentCulture;
               
                
            //dt = dtColumnOfExcel.DefaultView.ToTable(false, ColumnName);


            conn.Close();
            conn.Dispose();
            return dtColumnOfExcel;
        }
        #endregion

        #region پاک کردن کلی
        public static void ClearExcelFile(string path)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion
            var excel = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excel.Workbooks.Open(path);

            try
            {
                foreach (dynamic worksheet in workbook.Worksheets)
                {
                    worksheet.Cells.ClearContents();
                }

                workbook.Save();
            }
            finally
            {
                workbook.Close();
                excel.Quit();
            }
        }

        public static void ClearContentExcelFile(string path)
        {

            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("sheet");

                var headerRow = new List<string[]>()
                  {
                    new string[] { "personid", "Name", "family", "محل کار", "نام پدر", "Check", "نوع همکاری", "شماره موبایل ", "شماره تلفن محل سکونت ", "تاریخ شروع قرداد", "تاریخ اتمام قرارداد", "ش شناسنامه", "محل صدور", "آدرس محل سکونت", "آدرس محل کار", "تلفن شرکت ", "کد پرسنلی", "voice", "مسیر فایل تصویری", "رزرو1", "رزرو2", "رزرو3", "رزرو4", "پست الکترونیکی", "کدپستی", "توضیحات" }
                  };

                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["sheet"];

                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                FileInfo excelFile = new FileInfo(path);
                excel.SaveAs(excelFile);
            }

        }
        #endregion

        #region تغییر فرمت جنرال به تکست
        public static void ReadExcelRewriteItTextFormat(string path)
        {

            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (OleDbConnection conn = new OleDbConnection())
            {
                string Import_FileName = path;

                string fileExtension = Path.GetExtension(Import_FileName);

                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);


                foreach (dynamic worksheet in workbook.Worksheets)
                {
                    worksheet.Cells.NumberFormat =@"";
                }

                workbook.Save();
            }


        }
        #endregion

        #region تغییر فرمت جنرال به نامبر

        public static void ReadExcelRewriteItToNumberFormat(string path)
        {

            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            using (OleDbConnection conn = new OleDbConnection())
            {
                string Import_FileName = path;

                string fileExtension = Path.GetExtension(Import_FileName);

                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(path);


                foreach (dynamic worksheet in workbook.Worksheets)
                {
                    worksheet.Cells.NumberFormat = "#############";
                }



                workbook.Save();
            }


        }
        #endregion

        public static DataTable ColumnsOfExcelToDataTable(string findedXclFileName, string[] arrColumns, string sheetname)
        {
            #region کشتن پروسسهای اکسل
            Process[] processes = Process.GetProcessesByName("EXCEL");
            foreach (Process p in processes)
            {
                p.Kill();
            }

            #endregion

            DataTable dtColumnOfExcel = new DataTable();

            #region بررسی ورژن اکسل و داشتن هدر
            bool hasHeaders = true;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (findedXclFileName.Substring(findedXclFileName.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + findedXclFileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + findedXclFileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            #endregion

            #region ایجاد کانکشن با اکسل
            OleDbConnection conn = new OleDbConnection(strConn);
            if (conn.State != ConnectionState.Open)
                conn.Open();
            #endregion

            string query = "SELECT *  FROM[" + sheetname + "$]";
            OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);

            daexcel.Fill(dtColumnOfExcel);

            conn.Close();
            conn.Dispose();



            // Create the DataView of the DataTable
            DataView view = new DataView(dtColumnOfExcel);

            // Create a new DataTable from the DataView with just the columns desired - and in the order desired
            DataTable dtOut = view.ToTable("Selected", false, arrColumns);







            return dtOut;
        }


        public static DataTable ReadExcelFileCreateDtByChatGpt(string filePath)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;\"";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("SELECT * FROM [Sheet1$]", connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();

                return dataTable;
            }
        }

    }
}



