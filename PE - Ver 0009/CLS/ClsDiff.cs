using DevExpress.XtraGrid.Views.Grid;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress;

namespace PE
{
   
    class ClsDiff
    {
        //ClsMySql objClsMySql  ;
        static ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public ClsDiff()
        {
            ///objClsMySql = new ClsMySql();
        }
        

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
   
        public static object Player { get; private set; }

        public static bool ContainColumn(string columnName, DataTable table)
        {
            DataColumnCollection columns = table.Columns;
            if (columns.Contains(columnName))
            {
                return true;
            }
            else
                return false;
        }
        #region PesrshianMessageBox
        public static void PershianMessagebox(IWin32Window parent, string title, string message)
        {
            MessageBox.Show(parent, message, title, MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign & MessageBoxOptions.RtlReading);
        }

        public static void MyMessageboxErr(string title, string message)
        {

            MessageBox.Show(message, title, MessageBoxButtons.OK,

                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
               MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
        public static void MyMessageboxInf(string title, string message)
        {

            MessageBox.Show(message, title, MessageBoxButtons.OK,

                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
               MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
        public static void MyMessageboxWarning(string title, string message)
        {

            MessageBox.Show(message, title, MessageBoxButtons.OK,

                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
               MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        } 
        #endregion



        public static InputLanguage GetInputLanguageByName(string inputName)
        {
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                if (lang.Culture.EnglishName.ToLower().StartsWith(inputName))
                {
                    return lang;
                }
            }
            return null;
        }
        //public static string CreateSuitPartitionForLog(string strPeymankar)
        //{
        //    //string strLastPartition = "D";

        //    //string strPath = strLastPartition + ":\\SelectedPeymankarAlmView.txt";

        //    //#region ایجاد فایل در مسیر در صورت عدم وجود
        //    //if (!File.Exists(strPath))
        //    //{
        //    //    // Create a file to write to. 
        //    //    using (StreamWriter sw = File.CreateText(strPath))
        //    //    {
        //    //        sw.Write(strPeymankar);
        //    //        sw.Close();
        //    //    }
        //    //}

        //    //#endregion

        //    //return strPath;
        //}

        public static void SetKeyboardLayout(InputLanguage layout)
        {
            InputLanguage.CurrentInputLanguage = layout;
        }

        public static string DirSearchForXclDb(string sDir)
        {
            int[] intArr = new int[100];
            string strLastUpdateFileName = "نامشخص";
            string[,] arrayOfDiff = new string[100, 2];

            for (int x = 0; x < 100; x++)
            {
                intArr[x] = 5000;
            }

            List<String> files = new List<String>();

            int i = 0;

            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                    if (f.Contains("Database_") && f.Contains(".xlsx"))
                    {
                        int diff = (MainForm.DateTimeSqlServer - System.IO.File.GetLastWriteTime(f)).Days;

                        arrayOfDiff[i, 0] = f;
                        arrayOfDiff[i, 1] = diff.ToString();
                        intArr[i] = diff;

                        i++;
                    }
                }

            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            Array.Sort(intArr);
            int intMin = intArr[0];


            for (int k = 0; k < 100; k++)
            {
                if (arrayOfDiff[k, 1] == intMin.ToString())
                {
                    strLastUpdateFileName = arrayOfDiff[k, 0];
                }
            }

            return strLastUpdateFileName;
        }

       

        public static string[] ReadAlmFileSendToTblBscUpdate(string strPathAlmFile, string Al_No)
        {

            #region  تعریف متغییرها
            string strAlmFile = "";
            int intUnSucsIns = 0;
            int intSucs = 0;
            int contSpareLine = 0;
            string strOneLine = "";
            #endregion

            #region تعریف آرایه ای از فیلدها
            string[] strFields = new string[] {
                "AL_NO",
                "BSC No",
                "BCF No",
                "BTS No",
                "ET",
                "Time",
                "Date",
                "Trx No",
                "Alarm No",
                "Alarm",
                "O&M State",
                "BTS CODE",
                "BTS Name",
                "GPRS State",
                "Traffic",
                "2M Status",
                "BL TRXs",
                "Address",
                "SPARE"
            };
            #endregion

            #region تبدیل فایل به رشته
            try
            {
                //خواندن فایل آقای خلیلی و تبدیل آن به رشته
                strAlmFile = ClsFile.ReadFileConvertToString(strPathAlmFile);

            }
            catch
            {
                ClsFile.WriteStrToLogFile("عدم موفیت در خواندن فایل آلارم آقای  حسین خلیلی", false);
            }
            #endregion

            #region Read File And Send TO DB

            try
            {
                using (StringReader reader = new StringReader(strAlmFile))
                {
                    while ((strOneLine = reader.ReadLine()) != null)//تا انتهای رشته ای او ال کامل بخواند
                    {
                        if (strOneLine.Contains("Last Update of"))
                        {
                            string[] arrContents;
                            int intContSlash = strOneLine.Count(f => f == '/') + 1;
                            for (int i = intContSlash; i < 19; i++)
                            {
                                strOneLine += "/";
                            }

                            arrContents = strOneLine.Split('/');

                            arrContents[0] = Al_No;

                            bool bolSuc =objClsSql.Insert("tblBscUpdate", strFields, arrContents);
                            if (!bolSuc)
                            {
                                intUnSucsIns++;
                            }
                            else
                            {
                                intSucs++;
                            }


                        }
                        else
                        {
                            contSpareLine++;
                        }

                    }
                }
            }
            catch
            {
                ClsFile.WriteStrToLogFile("عدم موفقیت در آنالیز فایل آلارم خوانده شده", false);
            }
            #endregion

            #region تعریف و پر کردن آرایه خروجی
            string[] strOut = new string[3];
            strOut[0] = intUnSucsIns.ToString();
            strOut[1] = intSucs.ToString().ToString();
            strOut[2] = contSpareLine.ToString();
            #endregion

            return strOut;
        }

        public static void SendGroupsArraySmsAndCreateLog(string strSms, string strDest)
        {

            #region دریافت تاریخ و زمان فعلی به شمسی

            string strSendDateTime = ClsConvertDateHassan.convertDateTimeToPersion(DateTime.Now);

            #endregion

            #region ارسال گروهی 

            objClsSql.GetAllAndSendToDataBase("[SMS].[dbo].tblMsgForSend", "msg,isSingle,destNumbers,[user]", "N'" + strSms + "',N'false',N'" + strDest + "',N'" + MainForm.StrFullNameCurUsr + "'");

            #endregion

        }

        public static void SendSmsToOnNumAndLog(string strSms, string strDestNum)
        {
            #region دریافت تاریخ و زمان فعلی به شمسی

            string strSendDateTime = ClsConvertDateHassan.convertDateTimeToPersion(DateTime.Now);

            #endregion

            #region ارسال فردی 

            objClsSql.GetAllAndSendToDataBase("[SMS].[dbo].tblMsgForSend", "msg,isSingle,destNumbers,[user]", "N'" + strSms + "',N'true',N'" + strDestNum + "',N'" + MainForm.StrFullNameCurUsr + "'");

            #endregion
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #region Grid
        public static void AdjustGridsColumns(DataGridView DgvName)
        {
            int colCount = DgvName.ColumnCount;
            for (int j = 0; j < colCount; j++)
            {
                DgvName.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                int widthCol = DgvName.Columns[j].Width;
                DgvName.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                DgvName.Columns[j].Width = widthCol;

                DgvName.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvName.Columns[j].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        public static void GetREDColorIfAlmBigger180(DataGridView dgvName)
        {
            foreach (DataGridViewRow row in dgvName.Rows)
                if (int.Parse(row.Cells["قطعی دقیقه"].Value.ToString()) > 180)
                {
                    row.DefaultCellStyle.BackColor = Color.Pink;
                }
        }



        #endregion

        public static string EditDate(string strDate)
        {

            if (strDate.Length < 10)
            {
                int idxOfFirstSlash = strDate.IndexOf("/");
                int idxOfLastSlash = strDate.LastIndexOf("/");
                int intContOfCharInMont = idxOfLastSlash - idxOfFirstSlash;
                if (intContOfCharInMont == 2)
                {
                    strDate = strDate.Substring(0, 5) + "0" + strDate.Substring(5, 1) + strDate.Substring(6);
                }
                idxOfLastSlash = strDate.LastIndexOf("/");
                string strDay = strDate.Substring(idxOfLastSlash + 1);
                if (strDay.Length == 1)
                {
                    strDate = strDate.Substring(0, 8) + "0" + strDate.Substring(8, 1);
                }

            }

            return strDate;
        }

        public static void SetDateOfUpdate(string strDb, string strTable, string strField)
        {
            string strSqlForUpdate = @" USE " + strDb +
            @" UPDATE " + strTable +
            " SET " + strField + "=GETDATE()"
            ;

           
               objClsSql.SendCommand(strSqlForUpdate);

        }


        public  void DeleteDtRowFromDbAndmyDb(string strRowForDelete)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(" برای حذف ردیف   " + strRowForDelete + "مطمئن هستید  ؟  ", "تاییدیه حذف  ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    objClsSql.SendCommand(@" Delete From DT.dbo.tblDownTimeShort Where row=N'" + strRowForDelete + "'");

                    #region اگر دان تایم در جدول تکراری ها هم هست رکورد مربوط حذف گردد

                    objClsSql.SendCommand(@" Delete From  [DT].[dbo].[tblRepetedDt] Where row=N'" + strRowForDelete + "'");

                    #endregion


                   // objClsMySql.SendCommand(@"Delete From pm.tblod Where row=N'" + strRowForDelete + "'");

                    

                    MessageBox.Show(" ردیف   " + strRowForDelete + "از دیتابیس دان تایم حذف گردید ");
                     ClsDiff.writeLogOfAction(" ردیف   " + strRowForDelete + "از دیتابیس دان تایم حذف گردید ");
                   
                }
                else
                {

                }


            }
            catch
            {

                MessageBox.Show("حذف دان تایم از دیتابیس موفقیت آمیز نبود");
            }
        }

       
        #region STRING
        public static int countStringInString(string text, string searchTerm)
        {

            //Convert the string into an array of words  
            string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',','-' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query.  Use ToLowerInvariant to match "data" and "Data"   
            var matchQuery = from word in source
                             where word.ToLowerInvariant()== searchTerm.ToLowerInvariant()
                             select word;

            // Count the matches, which executes the query.  
            int wordCount = matchQuery.Count();
            //Console.WriteLine("{0} occurrences(s) of the search term \"{1}\" were found.", wordCount, searchTerm);

            return (wordCount);

        }
        #endregion

        public static void writeLogOfAction(string strValue)
        {
            try
            {


                string strDate = ClsConvertDateHassan.convertDateAndDayToPersion(MainForm.DateTimeSqlServer);
                string strTime = MainForm.DateTimeSqlServer.Hour + ":" + MainForm.DateTimeSqlServer.Minute + ":" + MainForm.DateTimeSqlServer.Second;

                string strFields = "[userName], [action],[date],[time],[mac],[computerName],[SelectedNet],[Ip]";
                string strValues = string.Concat("N'", MainForm.StrCurUsr, "',N'", strValue, "',N'", strDate, "',N'", strTime, "',N'", MainForm.StrCurMac, "',N'", MainForm.StrCumputerName, "',N'", MainForm.StrConnectedNetWork, "',N'", MainForm.StrCurIp, "'");

                objClsSql = new ClsSql();
                objClsSql.GetAllAndSendToDataBase("PE.dbo.tblLog", strFields, strValues);
            }
            catch(Exception ex)
            {
                string strExceptionMsg = "ناموفق در کلاس اس کیو ال - به مدیر سامانه اطلاع دهید \n writeLogOfAction()\n" + ex;
                MessageBox.Show(strExceptionMsg);
            }
        }

        public static void CreateUpdatedTblSite()
        {
            #region strSqlForCreateMiniDb
            string strSqlForCrateMiniDb = @"SELECT     لوکیشن AS location, MIN([نام سایت]) AS [نام سایت], MIN(پیمانکار) AS پیمانکار, MIN(منطقه) AS منطقه, [طول جغرافیایی], [عرض جغرافیایی]
FROM         PE.dbo.vwDB
GROUP BY لوکیشن, [طول جغرافیایی], [عرض جغرافیایی]";
            #endregion

            #region ساخت دیتا تیبل از پرسنل

            DataTable dtSite = objClsSql.FillDataTableFromQuery(strSqlForCrateMiniDb);

            #endregion

            #region ساخت  تیبل سایت از روی دیتابیس

            objClsSql.DeleteAllRecords("SA.dbo.tblSite");

           objClsSql.BulkInsertDataTable("SA.dbo.tblSite", dtSite);

            #endregion

           // #region اضافه کردن سایت 0000 جهت توسعه به جدول سایت

           // string[] strField1 = { "location],[نام سایت],[پیمانکار سایت],[منطقه" };
           // string[] strValue1 = { "ES0000","مراجعه شده", "بدون پیمانکار", "----" };

           //objClsSql.Insert("[SA].[dbo].[tblSite]", strField1, strValue1);

           // #endregion
        }

        public static void insertCountInFooterGrid(DevExpress.XtraGrid.Views.Grid.GridView grvName, int column, string strFooterName)
        {
            try
            {
                #region نمایش سرجمع تعداد
                //Show the GridView's summary footer
                grvName.OptionsView.ShowFooter = true;

                //Set up a summary on the "username" column
                grvName.Columns[column].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;

                grvName.Columns[column].SummaryItem.DisplayFormat = "{0:n0} " + strFooterName;
                #endregion
            }
            catch
            {

            }
        }

        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                using (_timeoutTimer)
                    MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }
        public static string chechVerOfDotNetFrameWork()
        {
            string strVerOfDotNet = "Not Found";

            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    strVerOfDotNet = $".NET Framework Version: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}";
                }
                else
                {
                    strVerOfDotNet = ".NET Framework Version 4.5 or later is not detected.";
                }
            }

            return strVerOfDotNet;
        }

        public static string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 528040)
                return "4.8 or later";
            if (releaseKey >= 461808)
                return "4.7.2";
            if (releaseKey >= 461308)
                return "4.7.1";
            if (releaseKey >= 460798)
                return "4.7";
            if (releaseKey >= 394802) 
                return "4.6.2";
            if (releaseKey >= 394254)
                return "4.6.1";
            if (releaseKey >= 393295)
                return "4.6";
            if (releaseKey >= 379893)
                return "4.5.2";
            if (releaseKey >= 378675)
                return "4.5.1";
            if (releaseKey >= 378389)
                return "4.5";
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }


        public DataTable selectDataTable(DataTable dt,string strSelect)
        {
            DataTable dtOut = dt.Clone();
            DataRow[] dr = dt.Select(strSelect);
            foreach (DataRow row in dr)
            {
                dtOut.ImportRow(row);
            }
            return dtOut;
        }

        public static DataTable SelectColumnWithArray(DataTable dtForSelectColumn, string[] arrColForSel)
        {
            string[] columnNames = dtForSelectColumn.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();


            foreach (string strCol in columnNames)
            {
                if (!arrColForSel.Contains(strCol.ToString()))
                {
                    dtForSelectColumn.Columns.Remove(strCol);
                }
            }

            dtForSelectColumn.AcceptChanges();

            return dtForSelectColumn;
        }

        public static DataTable GroupingOdAndCreateId(DataTable dtForCalculatAndGroup)
        {
            var result = from rows in dtForCalculatAndGroup.AsEnumerable()
                         group rows by new { ID = rows["ID"] } into grp
                         select grp;

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Id", typeof(string));
            dt1.Columns.Add("Fisrt OccUr", typeof(string));
            dt1.Columns.Add("Last Clear", typeof(string));
            dt1.Columns.Add("Interval", typeof(double));


            List<DataTable> dts = new List<DataTable>();
            List<DateTime> LstStartDateOfOpenDoor = new List<DateTime>();
            List<DateTime> LstEndDateOfOpenDoor = new List<DateTime>();

            foreach (var item in result)//واکشی یک گروه آی دی مشترگ
            {

                string strId = "";

                double dblMaxOpenDoor = 0;

                foreach (DataRow dr in item)//واکشی رکوردهای داخلی گروه آی دی
                {
                    strId = dr.Field<String>("ID");
                    try
                    {

                        if (dr.Field<String>("شروع") != null)
                        {
                            if (DateTime.Parse(dr.Field<String>("شروع")).Hour >= 7 && DateTime.Parse(dr.Field<String>("شروع")).Hour < 19)
                            {
                                LstStartDateOfOpenDoor.Add(DateTime.Parse(dr.Field<String>("شروع")));
                            }

                        }

                        if (dr.Field<String>("پایان") != null)
                        {
                            if (DateTime.Parse(dr.Field<String>("پایان")).Hour >= 7 && DateTime.Parse(dr.Field<String>("پایان")).Hour <= 19)
                            {
                                if (DateTime.Parse(dr.Field<String>("پایان")).Hour == 19)
                                {
                                    if (DateTime.Parse(dr.Field<String>("پایان")).Minute > 0)
                                        continue;
                                }
                                LstEndDateOfOpenDoor.Add(DateTime.Parse(dr.Field<String>("پایان")));
                            }

                        }

                    }
                    catch
                    {

                    }

                }



                dt1.Rows.Add(strId, LstEndDateOfOpenDoor.Max(), LstStartDateOfOpenDoor.Min(), dblMaxOpenDoor);


                LstStartDateOfOpenDoor = new List<DateTime>();
                LstEndDateOfOpenDoor = new List<DateTime>();

            }

            return dt1;


        }


        //private static string SetDateForStatusBar()
        //{
        //    if (!MainForm.BolIsConnectedDb)
        //    {
        //        this.BolIsConnectedDb = this.CheckAndConnectToDB("master");
        //    }

        //    if (BolIsConnectedDb)
        //    {

        //        try
        //        {
        //            MainForm.DateTimeSqlServer = objClsSql.GetDatTimeMyqlServer();

        //            //this.lblStatus.Text =  string.Concat(new object[] { " امروز ", ClsConvertDateHassan.convertDateAndDayToPersion(DateTimeSqlServer), " ساعت  ", DateTimeSqlServer.TimeOfDay , "   ----------------  مطابق با  ", DateTimeSqlServer.ToString("dd-MM-yyyy   روز  dd  از ماه  MMM  سال  yyyy  میلادی ")  +"                 "+ MainForm.StrConnectedNetWork });
        //            lblStatusBar.Text = string.Concat(new object[] { " امروز ", ClsConvertDateHassan.convertDateAndDayToPersion(DateTimeSqlServer), "   ----------------  مطابق با  ", DateTimeSqlServer.ToString("dd-MM-yyyy   روز  dd  از ماه  MMM  سال  yyyy  میلادی ") + "                 " + MainForm.StrConnectedNetWork });
        //        }
        //        catch
        //        {
        //            this.lblStatusBar.Text = " اشکالی در تطابق تاریخها وجود دارد . لطفا مورد را به مدیر سیستم اطلاع دهید";
        //        }

        //    }
        //    else
        //    {
        //        this.lblStatusBar.Text = "متاسفانه اشکالی در شبکه رایانه شما وجود دارد آنرا اصلاح بفرمایید.";
        //    }

        //}

        //public   bool RegisterDownTimeInMySql(string strFieldsName, string strValues)
        //{
        //    strValues = strValues.Replace("[", "").Replace("]", "");
            
        //    #region 

        //    //bool bolIsRegInMySql = objClsMySql.Insert("mobilechat.tblDownTime1", strFieldsName, strValues);
        //    //if (!bolIsRegInMySql)
        //    //{
        //    //    MessageBox.Show("ایجاد دان تایم در مای اسکیول انجام نشد لطفا به مدیر سیستم اطلاع دهید");
        //    //}

        //    #endregion
        //    //return bolIsRegInMySql;
        //}

        public static string[] extractFieldFromDataTable(DataTable dt)
        {
            string[] arrFields = new string[dt.Columns.Count];

            int columnCount = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                arrFields[columnCount] = dc.ToString().Trim();
                columnCount++;
            }

            return arrFields;
        }


        public static string EzafeyeSefreAvval(string strNumber)
        {
            string strOut = "";

            if (int.Parse(strNumber) < 10)
            {
                strOut = "0" + strNumber;
            }
            else
            {
                strOut = strNumber;
            }
           
             return strOut;
        }

    }
}



