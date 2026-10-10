using AGP.ArianaService;
using DevExpress.CodeParser;
using DevExpress.Office.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Import.Rtf;
//using PE.CLS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PE.Forms.DT
{
    public partial class frmPmReq : DevExpress.XtraEditors.XtraForm
    {
        #region VAR

        #region فیلدهای ضروری بران دان تایم ثبت در سرور خودمان
            String StrRow = string.Empty;
            string strRegisterDate = string.Empty;
            string strRegisterTime = string.Empty;
            string strSiteCode = string.Empty;
            string strSiteName = string.Empty;
            string strCurBSC = string.Empty;
            string strStartDate = string.Empty;
            string strStartYear_Month = string.Empty;//برای بررسی سهمیه
            string strStartTime = string.Empty;
            string strEndDate = string.Empty;
            string strEndTime = string.Empty;
            string strApplicant = string.Empty;
            string strSubject = string.Empty;
            string strPerformer = string.Empty;
            string strApprover = string.Empty;
            string strDtNo = string.Empty;
            string strStatus = "ثبت توسط کاربر";
            string strNote = string.Empty;
            string strRegion = string.Empty;
            string strOutageMin = "60";
            string strTelOfPerformer = string.Empty;
            string strCcState = string.Empty;
            string strMiladiStartDate = string.Empty;
            string strStartYear = string.Empty;
            string strStartMont = string.Empty;


        #endregion

        #region متغییرهای متفرقه
        string strSourceRowOfRepeatedRow = string.Empty;

        ClsSql objClsSql = new ClsSql();

        string strLocation = string.Empty;

        string strTempForPerformer = string.Empty;

        bool bolIsCompleteParameterFDorReg = false;

        bool bolIsSucssesRegDownTime = false;

        int intKasreSahmieh = 0;
        #endregion

        #endregion

        public frmPmReq()
        {
            InitializeComponent();
        }

        private void frmPmReq_Load(object sender, EventArgs e)
        {

            #region بروز رسانی جدول درخواست ها

            RefreshRequestDt();

            #endregion

            #region تنظیم تاریخ شروع و پایان برای دو روز بعد
            ResetStartDateForNewDownTime();
            #endregion

            #region آماده سازی اطلاعات انجام دهنده دان تایم و پر کردن جدول مربوطه
            objClsSql = new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال

            gcTel.DataSource = objClsSql.FillDataTableFromQuery("Select telId AS [ردیف], Name As [نام و نام خانوادگی],tel AS [شماره تلفن ها],WorkPlace AS [محل کار]  From [TEL].[dbo].[tblTel] Order By Name");
            gvTel.Columns["ردیف"].Visible = false;
            gvTel.CancelSelection();

            txtSelectedPerformer.Clear();

            gbxDtPerformer.Enabled = false;

            btnRegPmDt.Enabled = false;
            #endregion

        }

        private void textBoxDtRegCodeSite_TextChanged(object sender, EventArgs e)
        {
            this.txtBoxDtRegCodeSite.CharacterCasing = CharacterCasing.Upper;

            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));

            if (txtBoxDtRegCodeSite.TextLength == 4)
            {
                strSiteCode = "ES" + txtBoxDtRegCodeSite.Text;
                // strLocation = "ES" + strSiteCode;

                DataRow[] dr = null;
                try
                {
                    dr = MainForm.DtMiniTempDb.Select("[کد سایت]='" + strSiteCode + "'");

                    strSiteName = txtDtRegSiteName.Text = dr[0].ItemArray[1].ToString();

                    if (txtDtRegSiteName.Text.Length == 0)
                    {
                        strSiteName = txtDtRegSiteName.Text = "نامشخص";
                    }

                    strRegion = lblDtRegion.Text = dr[0].ItemArray[4].ToString();

                    #region بررسی سهمیه پی ام  درخواست کننده برای این منطقه

                    if (!(HaveAnyRemaindedStock()))//اگه سهمیه نداره
                    {
                        string message = "کاربر گرامی سهمیه دان تایم تکراری برای منطقه شما به اتمام رسیده است .  ";
                        MessageBox.Show(message, " اخطار در مورد اتمام سهمیه ", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                        return;

                    }

                    #endregion

                }
                catch
                {
                    txtDtRegSiteName.Text = "نامشخص";

                    lblDtRegion.Text = "نامشخص";
                }
                gbxDtAllSubject.Enabled = true;//آفرین حالا میتونی موضوع انتخاب کنی

                EnableDisableBtnRegDt();

                gbxDtAllSubject.Focus();

            }
            else
            {
                txtDtRegSiteName.Text = "-----------------------------------------------";
                gbxDtAllSubject.Enabled = false;//تا کدسایت تکمیل نشده از انتخاب موضوع خبری نیست
                bolIsCompleteParameterFDorReg = false;//تا کد سایت تکمیل نشده خبری از ثبت نیست

                lblDtRegion.Text = string.Empty;

                if (txtBoxDtRegCodeSite.Text.Length > 4)
                {
                    MessageBox.Show("کد سایت باید یک عدد 4 رقمی باشد");
                    txtBoxDtRegCodeSite.Text = txtBoxDtRegCodeSite.Text.Remove(txtBoxDtRegCodeSite.Text.Length - 1, 1);

                }
            }


        }

        private void rdbPmCln_CheckedChanged(object sender, EventArgs e)
        {
            gbxDtPerformer.Enabled = true;

            if (rdbPmCln.Checked)
            {
                strSubject = rdbPmCln.Text;
                gbxDtSelectDate.Enabled = true;
                gbxDtSelectTime.Enabled = false;

            }
            else
            { gbxDtSelectDate.Enabled = false; }

            EnableDisableBtnRegDt();
        }

        private void rdbPmSte_CheckedChanged(object sender, EventArgs e)
        {
            gbxDtPerformer.Enabled = true;
            if (rdbPmSte.Checked)
            {
                strSubject = rdbPmSte.Text;
                gbxDtSelectDate.Enabled = true;
                gbxDtSelectTime.Enabled = false;
            }
            else
            { gbxDtSelectDate.Enabled = false; }
            EnableDisableBtnRegDt();
        }

        private void rdbPmTwr_CheckedChanged(object sender, EventArgs e)
        {
            gbxDtPerformer.Enabled = true;
            if (rdbPmTwr.Checked)
            {
                strSubject = rdbPmTwr.Text;
                gbxDtSelectDate.Enabled = true;
                gbxDtSelectTime.Enabled = false;
            }
            else
            { gbxDtSelectDate.Enabled = false; }
            EnableDisableBtnRegDt();
        }

        private void gcTel_Load(object sender, EventArgs e)
        {
            GridControl gridControl = sender as GridControl;
            GridView gridView = gridControl.MainView as GridView;
            if (gridView != null)
                gridControl.BeginInvoke(new Action(gridView.ShowFindPanel));
        }

        private void gcTel_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void gvTel_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvTel.GetFocusedDataRow() == null)
            {
                return;
            }
            if (gvTel.SelectedRowsCount == 0)
            {
                return;
            }
            BeginInvoke(new MethodInvoker(delegate
            {
                strTempForPerformer = gvTel.GetFocusedDataRow()["نام و نام خانوادگی"].ToString();//استخراج نام و نام خانوادگی انجام دهنده ی درخواستی
                strTelOfPerformer = gvTel.GetFocusedDataRow()["شماره تلفن ها"].ToString();//استخراج تلفن انجام دهنده ی درخواستی

                txtSelectedPerformer.Text = "  انجام دهنده ی انتخاب شده : " + strTempForPerformer + "    " + strTelOfPerformer + "    " + gvTel.GetFocusedDataRow()["محل کار"].ToString();

            }
              ));

        }

        private void gcTel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EvaluteDtForPmReq();
                //refreshRegisterGrid();
            }
        }

        private string getLastRowNumberOfRepeatedDt()
        {
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strSiteCode + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "') order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);
            //string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            string strReturnRow = dt.Rows[0].ItemArray[0].ToString();

            return strReturnRow;
        }

        private string getLastStateOfRepeatedDt()
        {
            //string strTxtCodeSite = textBoxDtRegCodeSite.Text;
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);
            //string strSubject = comboBoxDtSubject.Text;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strSiteCode + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "') order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);
            //string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            string strReturnRow = dt.Rows[0].ItemArray[2].ToString();

            return strReturnRow;
        }

        private string getNotetStateOfRepeatedDt()
        {
            //string strTxtCodeSite = textBoxDtRegCodeSite.Text;
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);
            //string strSubject = comboBoxDtSubject.Text;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strSiteCode + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "'" + " AND status=N'تائید') order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);
            //string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            string strReturnNote = dt.Rows[0].ItemArray[3].ToString();

            return strReturnNote;
        }

        #region تابع بررسی سهمیه دان تایم تکراری در این ماه 
        private int RemaindedRep(string strRegion, string yearMonth)
        {
            //string strTblName = @"[DT].[dbo].[tblRepRemaindedRegion" + strRegion.Trim() + "]";

            string strSql = "Select " + strRegion + " From [DT].[dbo].tblRepRemaindedRegions Where [yearMonth]=N'" + yearMonth + "'";
            string strCntOfRemaindedRep = objClsSql.SendCommandGetStringResult(strSql);

            return int.Parse(strCntOfRemaindedRep);
        }

        #endregion

        #region تابع کسر یک یا دو  واحد از سهمیه پیمانکار 

        private void DecrementRemainRepForThisRegion(string strRegion, string yearMonth, int intCntOfdecrement)
        {

            int intExistedRep = int.Parse(objClsSql.SendCommandGetStringResult("Select " + strRegion + " From [DT].[dbo]. tblRepRemaindedRegions Where [yearMonth]=N'" + yearMonth + "'"));

            int intRemaindedRep = intExistedRep - intCntOfdecrement;
            objClsSql.EditRecord("[DT].[dbo]. tblRepRemaindedRegions", strRegion, intRemaindedRep, "Where yearMonth=N'" + yearMonth + "'");


        }

        #endregion

        #region ثبت دان تایم
        private void RegisterDownTime(bool bolIsRepeatedPmDownTime)
        {
            #region گرفتن ساعت سرور
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            #endregion

            #region اگه اشتباهی زده و دان تایم نمی خواهد خارج شو در غیر این صورت گرفتن دان تایم رو ادامه بده
            bool bolAcceptThidDt = MessageBox.Show($"آیا برای ایجاد {"دان تایم  "} بر روی سایت یا مورد {this.txtDtRegSiteName.Text} مطمئن هستید؟", "تاییدیه اعمال  تغییرات  ", MessageBoxButtons.YesNo) == DialogResult.Yes;
            if (!bolAcceptThidDt) { return; }

            #endregion

            #region ریست رشته های موقت برای حفظ سابقه ی کومبوها
            string StrDtTempForSubjectCombo = string.Empty;
            string StrDtTempForPerformerCombo = string.Empty;
            #endregion

            #region حفظ سابقه کومبوها !!!!ناقص
            #endregion

            #region اگر پارامتر یا پارامترهایی از موارد خواسته شده دان تایم ناقصه پیام مناسب و خروج
            //متغییر پر بودن همه فیلدها
            bool bolIsUnCompAllDtField =
                (strSiteCode == string.Empty) ||
                (strSiteName == string.Empty) ||
                (strSubject == string.Empty) ||
                (strTelOfPerformer == string.Empty || strTelOfPerformer == "نامشخص")
                ;

            if (bolIsUnCompAllDtField)
            {
                #region چک تکمیل بودن همه فیلدها برای پیدا کردن مورد یا موارد ناکامل
                string str = string.Empty;
                if (strSiteCode == string.Empty)
                {
                    str = str + "کد سایت و ";
                }
                if (strSiteName == string.Empty)
                {
                    str = str + "نام سایت و ";
                }
                if (strTelOfPerformer == string.Empty)
                {
                    str = str + " و outage";
                }
                if (strSubject == string.Empty)
                {
                    str = str + "موضوع و ";
                }
                if (strTelOfPerformer == string.Empty || strTelOfPerformer == "نامشخص")
                {
                    str = str + "انجام دهنده و ";
                }

                str = str.Substring(0, str.Length - 2);
                MessageBox.Show($"  کاربر گرامی فیلد  {str} تکمیل نشده است لطفا درخواست خود را تکمیل بفرمایید", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                return;
                #endregion


            }

            #region اخطار نامشخص بودن نام سایت
            if (strSiteName == "نامشخص")
            {
                DialogResult dialogResultNamoshakhas = MessageBox.Show(" نام سایت نامشخص می باشد  و احتمالا کد سایت نامعتبر وارد کرده اید آیا تمایل دارید این دان دان تایم ثبت گردد؟  ", "  اخطار ثبت دان تایم با نام نامشخص  ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                if (dialogResultNamoshakhas != DialogResult.Yes)
                {
                    return;
                }

            }
            #endregion

            #endregion

            #region پیدا کردن شماره بعدی برای ثبت دان تایم جدید
            int num;
            string s = objClsSql.FindFieldInLastRecord("[DT].[dbo].tblDownTimeShort", "row", "row");

            if (s == "نامشخص")
            {
                num = 0;
            }
            else
            {
                num = int.Parse(s);
            }

            int intNewRow = num + 1;
            #endregion

            #region استخراج زمان سرور برای ثبت زمان رجیستر
            string strCurPershianDate = ClsConvertDateHassan.ConvertDateToPersian(MainForm.DateTimeSqlServer);//استخراج زمان ثبت از اس کیو ال سرور

            string strCurHoureMin = MainForm.DateTimeSqlServer.ToString("HH:mm");
            #endregion

            #region استخراج زمان شروع و پایان دان تایم درخواستی از نامریک ها
            string strStartHour = this.nudDtStHour.Value.ToString("00");
            string strStartMin = this.nudDtStMin.Value.ToString("00");

            strStartTime = strStartHour + ":" + strStartMin;//استخراج زمان شروع

            string strEndHour = this.nudDtEndHour.Value.ToString("00");
            string strEndMin = this.nudDtEndMin.Value.ToString("00");

            strEndTime = strEndHour + ":" + strEndMin;//استخراج زمان پایان
            #endregion

            #region ساخت آرایه فیلدها
            string strFieldsName =
                @"[row],
                [registerDate],
                [registerTime],
                [siteCode],
                [siteName],
                [curBSC],
                [startDate],
                [startTime],
                [endDate],
                [endTime],
                [applicant],
                [subject],
                [performer],
                [approver],
                [dtNo],
                [Status],
                [region],
                [Note],
                [outageMin]
               
               ";
            #endregion

            #region ساخت آرایه مقادیر

            #region خالی کردن آرایه مقادیر
            string strValues = string.Empty;
            #endregion

            #region تکمیل بعضی از مقادیر جامانده
            StrRow = intNewRow.ToString();//ردیف جدید
            strRegisterDate = strCurPershianDate;//تاریخ ثبت
            strRegisterTime = strCurHoureMin;//زمان ثبت
            strApplicant = MainForm.StrFullNameCurUsr;//استخراج درخواست کننده 
            #endregion


            #region آرایه ی مقادیر !! فعلا و موقت
            strValues = string.Concat(new object[] {
                "N'", StrRow                  ,"'," +
                "N'", strRegisterDate         ,"'," +
                "N'", strRegisterTime         ,"'," +
                "N'", strSiteCode             ,"'," +
                "N'", strSiteName             ,"'," +
                "N'", strCurBSC               ,"'," +
                "N'", strStartDate            ,"'," +
                "N'", strStartTime            ,"'," +
                "N'", strEndDate              ,"'," +
                "N'", strEndTime              ,"'," +
                "N'", strApplicant            ,"'," +
                "N'", strSubject              ,"'," +
                "N'", strPerformer            ,"'," +
                "N'", strApprover             ,"'," +
                "N'", strDtNo                 ,"'," +
                "N'", strStatus               ,"'," +
                "N'", strRegion               ,"'," +
                "N'", strNote                 ,"'," +
                "N'", strOutageMin            ,"'," 
                });
            #endregion



            //if (strNoteForPmSameSubAndCodeAndDate != string.Empty && !strNoteForPmSameSubAndCodeAndDate.Contains("row"))
            //{
            //    string strDt = strNoteForPmSameSubAndCodeAndDate.Substring(0, 14);

            //    string strApprover = "Auto";
            //    strValues = string.Concat(new object[] {
            //        "N'", intNewRow.ToString(), "',N'", strCurPershianDate, "',N'", strCurHoureMin, "',N'", "ES"+this.textBoxDtRegCodeSite.Text.Trim(), "',N'", this.txtDtRegSiteName.Text.Trim(), "',N'", string.Empty, "',N'", strDtFromYear, "/", strDtFromMonth,
            //        "/", strDtFromDay, "',N'", str7, ":", strStartMin, "',N'", strDtToYear, "/", strDtToMonth, "/", strDtToDay, "',N'", strEndHour, ":", str10,
            //        "',N'", MainForm.StrFullNameCurUsr, "',N'", StrDtTempForSubjectCombo.Trim(), "',N'",  strTempForPerformer.Trim()+"-"+strTelOfPerformer+"-"+strTelOfPerformer, "',N'",strApprover, "',N'", string.Empty, "',N'ثبت در (SDM)',N'"+strRegion+"',N'"+strNoteForPmSameSubAndCodeAndDate+"',N'"+strDtSetOutage+"',N'',N'"+strDateTimeStartMiladi+"'"
            //    }); ;
            //}
            //else if (strNoteForPmSameSubAndCodeAndDate != string.Empty && strNoteForPmSameSubAndCodeAndDate.Contains("row"))
            //{
            //    strValues = string.Concat(new object[] {
            //        "N'", intNewRow.ToString(), "',N'", strCurPershianDate, "',N'", strCurHoureMin, "',N'", "ES"+this.textBoxDtRegCodeSite.Text.Trim(), "',N'", this.txtDtRegSiteName.Text.Trim(), "',N'", string.Empty, "',N'", strDtFromYear, "/", strDtFromMonth,
            //        "/", strDtFromDay, "',N'", str7, ":", strStartMin, "',N'", strDtToYear, "/", strDtToMonth, "/", strDtToDay, "',N'", strEndHour, ":", str10,
            //        "',N'", MainForm.StrFullNameCurUsr, "',N'", StrDtTempForSubjectCombo.Trim(), "',N'",  strTempForPerformer.Trim()+"-"+strTelOfPerformer, "',N'","',N'", "',N'ثبت توسط کاربر',N'"+strRegion+"',N'"+strNoteForPmSameSubAndCodeAndDate+"',N'"+strDtSetOutage+"',N'',N'"+strDateTimeStartMiladi+"'"
            //    }); ;
            //}
            //else
            //{
            //    string strDt = string.Empty;
            //    string strApprover = string.Empty;
            //    strValues = string.Concat(new object[] {
            //        "N'", intNewRow.ToString(), "',N'", strCurPershianDate, "',N'", strCurHoureMin, "',N'", "ES"+this.textBoxDtRegCodeSite.Text.Trim(), "',N'", this.txtDtRegSiteName.Text.Trim(), "',N'", string.Empty, "',N'", strDtFromYear, "/", strDtFromMonth,
            //        "/", strDtFromDay, "',N'", str7, ":", strStartMin, "',N'", strDtToYear, "/", strDtToMonth, "/", strDtToDay, "',N'", strEndHour, ":", str10,
            //        "',N'", MainForm.StrFullNameCurUsr, "',N'", StrDtTempForSubjectCombo.Trim(), "',N'",  strTempForPerformer.Trim()+"-"+strTelOfPerformer, "',N'",strApprover, "',N'",strDt, "',N'ثبت توسط کاربر',N'"+strRegion+"',N'"+strNoteForPmSameSubAndCodeAndDate+"',N'"+strDtSetOutage+"',N'',N'"+strDateTimeStartMiladi+"'"
            //            }); ;
            //}

            #endregion

            #region ثبت دان تایم در اس کیو ال
            bolIsSucssesRegDownTime = objClsSql.GetAllAndSendToDataBase("[DT].[dbo].tblDownTimeShort", strFieldsName, strValues);//ثبت 
            #region پیام در مورد ثبت دان تایم
            if (!bolIsSucssesRegDownTime)
            {
                MainForm.BolDtIsCreateDownTime = true;//اعلام ثبت
                MessageBox.Show("در ثبت دان تایم مشکلی پیش آمد لطفا بررسی فرمایید", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                txtBoxDtRegCodeSite.Select();
                return;
            }

            else
            {
                ClsDiff.AutoClosingMessageBox.Show("ثبت شد", "موفق", 2000);
            }
            #endregion
            #endregion

            #region ثبت شماره دان تایم تکراری و آخرین دان تایمی مرجعی که مشابه آنرا داریم در دیتابیس
            if (bolIsRepeatedPmDownTime)
            {
                objClsSql.GetAndSendToDataBaseAndReturnResult("[DT].[dbo].[tblRepetedDt]", "[repeatedRow]", intNewRow);
                objClsSql.EditRecord("[DT].[dbo].[tblRepetedDt]", "[sourceOfRepeatedRow]", strSourceRowOfRepeatedRow, "Where [repeatedRow]=N'" + intNewRow + "'");
                bolIsRepeatedPmDownTime = false;
                strSourceRowOfRepeatedRow = string.Empty;
            }
            #endregion

            MainForm.IntDtCountTmrForNewDt = 0;

            #region اگه ثبت موفق بود آماده سازی مجدد کومبوباکسها و ادیت تکست ها
            if (MainForm.BolDtIsCreateDownTime)
            {
                #region خالی کردن تکس باکس ها
                this.txtBoxDtRegCodeSite.Text = string.Empty;
                this.txtDtRegSiteName.Text = "----------------";
                #endregion

                #region اجبار به رفرش و رفتن به سطر آخر
                MainForm.bolDtForceToRefreshAndGoToEnd = true;
                MainForm.bolDtForceToRefreshAndDontGoToEnd = false;
                #endregion

                MainForm.BolDtIsCreateDownTime = false;

                ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfTblDt");
            }
            #endregion

            #region ثبت در لاگ
            ClsDiff.writeLogOfAction(" ایجاد دان تایم جدید ");
            #endregion

            #region تمرکز مجدد بروی تکس باکس کد سایت
            this.txtBoxDtRegCodeSite.Focus();
            this.txtBoxDtRegCodeSite.Select();
            #endregion

        }
        #endregion

        private void RefreshRequestDt()
        {
            DataTable dt = new DataTable();
            string strSql = @"
            SELECT 
            [siteCode] AS [کد سایت] 
            ,[siteName]AS [نام سایت] 
            ,[startDate] + ' _ '+[startTime] AS [شروع]             
            ,[endDate] + ' _ '+[endTime] AS [پایان]             
            ,[subject]AS [موضوع] 
            ,[performer]AS [انجام دهنده] 
            FROM [DT].[dbo].[tblDownTimeShort] 
            Where applicant=N'" + MainForm.StrFullNameCurUsr + "' AND status=N'ثبت توسط کاربر'";

            dt = objClsSql.FillDataTableFromQuery(strSql);

            gridControlDtRegiterDtByThisUser.DataSource = dt;
        }

        private bool HaveAnyRemaindedStock()
        {
            #region ریست شاخص دان تایم تکراری
            MainForm.bolDtIsRepeatedDt = false;
            #endregion

            #region پاک کردن منبع دان تایم تکراری
            strSourceRowOfRepeatedRow = string.Empty;
            #endregion

            return true;

        }

        private bool IsRepeatedDtThisReqInThisMonthOfYear()
        {
            //string strTxtCodeSite = textBoxDtRegCodeSite.Text;//از کدسایت درخواستی استخراج میگردد
            //string strTxtSubject = comboBoxDtSubject.Text;// از موضوع درخواستی استخراج می گردد
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);//از سال درخواستی استخراج می گردد
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);//از ماه درخواستی استخراج می گردد

            string strSqlForFindSourceOfRepeatedRow = @"Select  row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strSiteCode + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "'" + ") order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);

            bool bolIsRepeated = false;
            if (dt.Rows.Count > 0)
            {
                bolIsRepeated = true;
            }
            else
            {
                bolIsRepeated = false;
            }

            return bolIsRepeated;
        }
        private DataTable LastRepeatedDtThisReqInThisMonthOfYear()
        {

            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);//از سال درخواستی استخراج می گردد
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);//از ماه درخواستی استخراج می گردد

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1)  row,region,subject,startDate,status,note 
                FROM [DT].[dbo].[tblDownTimeShort] Where
                (siteCode=N'" + strSiteCode + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "')" +
                " order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);

            return dt;
        }

        private bool IsThisDtRepetitious()//اگه 
        {
            #region بررسی اینکه این دان تایم پی امی تکراری نباشد
            bool bolIsExsistThisPmDt = clsDt.ThisDtExactlyExisted(txtBoxDtRegCodeSite.Text, strSubject, int.Parse(nudDtStHour.Value.ToString()), int.Parse(nudDtStMin.Value.ToString()));
            if (bolIsExsistThisPmDt)
            {
                #region پیدا کردن ردیف دان تایم ثبت شده که دقیقا مشابه دان تایم درخواستی هستش  
                string strRowOfRepeatedRequestedRow = clsDt.GetRowOfExactlyRepeatedDowntime(strSiteCode, strSubject);
                #endregion

                #region بابا اینو در ردیف فلان گرفتی بیخیال 
                string message = " کاربر گرامی نظر به اینکه یک دان تایم با همین کد سایت با همین موضوع در همین تاریخ شروع در ردیف  " + strRowOfRepeatedRequestedRow + "  ثبت شده است شما مجاز به ثبت این دان تایم نیستید ";
                MessageBox.Show(message, " اخطار در مورد درخواست تکراری ", MessageBoxButtons.OK,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                #endregion
            }
            #endregion
            return bolIsExsistThisPmDt;
        }

        private void EvaluteDtForPmReq()//بررسی درخواست صورت 
        {

            // #region اگر دقیقا همین دان تایم قبلا درخواست شده استخراج ردیف مرجع تکرار وپیام مناسب و خروج 0



            //#region در صورتیکه دان تایم درخواستی دقیقا مشابه قبلی ندارد 3

            //#region موضوع دان تایم پی ام هستش 1

            //bool bolIsPmThisDtReq = ThisReqDtIsPm(comboBoxDtSubject.Text);

            //#region اگر درخواست پی امی هستش و با همین کدسایت با همین تاریخ با موضوع متفاوت دان تایمی درخواست شده دوباره نگیریم
            //if (bolIsPmThisDtReq)
            //{
            //    #region تنظیم اتوماتیک 60 دقیقه اوتیج دان تایم های پی امی
            //    listBoxDtOutageMin.SelectedItem = "60";
            //    strDtSetOutage = "60";
            //    txtSerchedOutage.Text = " 60  دقیقه";
            //    listBoxDtOutageMin.SelectedItem = "60";
            //    gbxDtOutage.Enabled = false;
            //    #endregion

            //    #region conflict cr  <= is PM  AND date=date  AND codeSite=CodeSite subject!=Subject
            //    strNoteForPmSameSubAndCodeAndDate = string.Empty;
            //    strNoteForPmSameSubAndCodeAndDate = WhichRowIsPmRequestAndThatHasSameCodeSiteAndDate();
            //    #endregion

            //    #region مشخص کردن اینکه آیا در ماه جاری تکراریه یعنی کد سایت و موضوعش در ماه جاریه این سال جاری وجوداره

            //    #region در ماه جاری تکرایه
            //    if (IsRepeatedDtThisReqInThisMonthOfYear())// 3تکراریه وضعیت و ردیفشو دربیار تا ببینیم چیکارش باید کرد
            //    {
            //        #region ردیف آخرین جایی که تکرار شده رو در بیار

            //        string strRowNumberOfLastRepeatedDt = strSourceRowOfRepeatedRow = getLastRowNumberOfRepeatedDt();


            //        #endregion

            //        #region وضعیت آخرین جایی رو هم که دان تایم تکرار شده رو در بیار ببینم
            //        string strStateOfLastRepeatedDt = getLastStateOfRepeatedDt();

            //        #endregion

            //        #region اگر وضیعت غیر از تایید است بررسی کن وضعیت در حال پردازش یا ثبت در اس دی ام هست یا نه

            //        #region اگر وضعیت تایید نیست بررسی کن درحال پردازشه یا در وضعیت ثبت در اس دی ام ه یا غیر از اون که یوقت کلاه سرمون نره
            //        if (strStateOfLastRepeatedDt != "تائید")
            //        {
            //            #region اگر وضعیت در حال پردازش درخواست می باشد
            //            if (strStateOfLastRepeatedDt == "در حال پردازش درخواست")
            //            {
            //                MessageBox.Show(" کاربر گرامی شما قبلا در ردیف   " + strRowNumberOfLastRepeatedDt + " همین دان تایم را درخواست کرده اید و در حال پردازش درخواست شما هستیم و امکان درخواست همین دان تایم در ماه جاری وجود ندارد ");
            //                return;//خروج
            //            }

            //            #endregion

            //            #region اگر وضعیت ثبت در اس دی ام هست
            //            else if (strStateOfLastRepeatedDt == "ثبت در (SDM)")
            //            {
            //                MessageBox.Show(" کاربر گرامی شما قبلا در ردیف   " + strRowNumberOfLastRepeatedDt + " همین دان تایم را درخواست کرده اید و در وضعیت ثبت در اس دی ام و منتظر تایید تهران هستیم وامکان درخواست همین دان تایم در ماه جاری وجود ندارد ");
            //                return;//خروج
            //            }
            //            #endregion

            //            #region Bاگر تایید نشده و در وضعیت پردازش و ثبت در اس دی ام نیست یک دان تایم معمولی بگیر
            //            else
            //            {
            //                RegisterDownTime(false);//یک دان تایم معمولی بگیر چون تایید نشده که از سهمیه این بدبختا کم کنیم
            //                return;//خروج
            //            }
            //            #endregion
            //        }
            //        #endregion

            //        #region اگه تایید شده برو تو کار بررسی سهمیه تکراری ها و بررسی داشتن یا نداشتن اجازه و اگه اجازه داره بسته به کنسل بودن یا نه میزان یک یا دو واحد از سهمیه کم کن
            //        else
            //        {
            //            #region یادداشت آخرین جایی رو هم که دان تایم تکرار شده رو در بیار ببینم
            //            string strNoteOfLastRepeatedDt = getNotetStateOfRepeatedDt();
            //            #endregion

            //            #region دان تایم تکراری میخواد ولی دمش گرم قبلا کنسل کرده بنابراین اگه سهمیه داره یکی از سهمیش کم کن و براش بگیر
            //            if (strNoteOfLastRepeatedDt.Contains("کنسل"))
            //            {
            //                #region محاسبه  سهمیه
            //                int intRemaindedRep = RemaindedRep(strRegion, strYearMonth);

            //                bool bolHaveRemainRepForDownTime = intRemaindedRep > 1 ? true : false;// یعنی سهمیه باقی مانده 
            //                #endregion

            //                #region داشتن سهمیه و سوال برای ادامه و کسر
            //                if (bolHaveRemainRepForDownTime)//سهمیه دارد
            //                {

            //                    #region اگر موفقیت آمیز بود کسر سهمیه
            //                    DialogResult dialogResult = MessageBox.Show(" همین دان تایم را درخواست کرده و کنسل کرده اید و در صورت ادامه یک واحد از سهمیه دان تایم تکراری شما کسر خواهد شد آیا ادامه می دهید؟ " + strRowNumberOfLastRepeatedDt + "در ردیف ", "توجه", MessageBoxButtons.YesNo);
            //                    #endregion

            //                    #region اگه موافقه که یکی از سهمییش کم بشه بگیر
            //                    if (dialogResult == DialogResult.Yes)
            //                    {

            //                        #region ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
            //                        RegisterDownTime(true);//ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
            //                        #endregion

            //                        #region اگر موفقیت آمیز بود کسر سهمیه
            //                        if (bolIsSucssesRegDownTime)//اگر موفقیت آمیز بود کسر سهمیه
            //                        {
            //                            //کسر سهمیه پیمانکار به اندازه یک واحد

            //                            DecrementRemainRepForThisRegion(strRegion, strYearMonth, 1);
            //                            return;//خروج
            //                        }
            //                        #endregion


            //                    }
            //                    #endregion

            //                    #region اگه موافق نیست خارج شو
            //                    else if (dialogResult == DialogResult.No)
            //                    {
            //                        return;//خروج
            //                    }
            //                    #endregion


            //                }
            //                #endregion

            //                #region نداشتن سهمیه و پیام مناسب و خروج
            //                else
            //                {
            //                    MessageBox.Show("سهمیه دان تایم تکراری شما در این ماه تمام شده و امکان ثبت دان تایم تکراری برای شما وجود ندارد");
            //                    return;
            //                }
            //                #endregion

            //            }
            //            #endregion

            //            #region دان تایم تکراری میخواد و دفعه قبلی هم کنسل نکرده ببین اگه سهمیه داره دمدش  نرم دو تا ازش کم کن و بگیر
            //            else
            //            {
            //                #region محاسبه  سهمیه

            //                //محاسبه سهمیه
            //                int intRemaindedRep = RemaindedRep(strRegion, strYearMonth);

            //                #region اگه سهمیه ی باقیمانده یک هستش یه ارفاقی بکن 2 کن بتونه یکی بگیره
            //                if (intRemaindedRep == 1)//اگه سهمیه ی باقیمانده یک هستش یه ارفاقی بکن 2 کن بتونه یکی بگیره
            //                {
            //                    intRemaindedRep++;
            //                    //سهمیه رو دو کن
            //                }
            //                #endregion

            //                bool bolHaveRemainRepForDownTime = intRemaindedRep > 1 ? true : false;// سهمیه باقی مانده 

            //                #endregion

            //                #region داشتن سهمیه و سوال برای ادامه و کسر
            //                if (bolHaveRemainRepForDownTime)//سهمیه دارد
            //                {

            //                    #region اگر موفقیت آمیز بود کسر سهمیه
            //                    DialogResult dialogResult = MessageBox.Show(" همین دان تایم را درخواست کرده و کنسل نکرده اید و در صورت ادامه دو واحد از سهمیه دان تایم تکراری شما کسر خواهد شد آیا ادامه می دهید؟ " + strRowNumberOfLastRepeatedDt + "در ردیف ", "توجه", MessageBoxButtons.YesNo);
            //                    #endregion

            //                    #region اگه موافقه که یکی از سهمییش کم بشه بگیر
            //                    if (dialogResult == DialogResult.Yes)
            //                    {

            //                        #region ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
            //                        RegisterDownTime(true);//ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
            //                        #endregion

            //                        #region اگر موفقیت آمیز بود کسر سهمیه
            //                        if (bolIsSucssesRegDownTime)//اگر موفقیت آمیز بود کسر سهمیه
            //                        {
            //                            //کسر سهمیه پیمانکار به اندازه دو واحد

            //                            DecrementRemainRepForThisRegion(strRegion, strYearMonth, 2);
            //                            return;//خروج
            //                        }
            //                        #endregion


            //                    }
            //                    #endregion

            //                    #region اگه موافق نیست خارج شو
            //                    else if (dialogResult == DialogResult.No)
            //                    {
            //                        return;//خروج
            //                    }
            //                    #endregion


            //                }
            //                #endregion

            //                #region نداشتن سهمیه و پیام مناسب و خروج
            //                else
            //                {
            //                    MessageBox.Show("سهمیه دان تایم تکراری شما در این ماه تمام شده و امکان ثبت دان تایم تکراری برای شما وجود ندارد");
            //                    return;
            //                }
            //                #endregion



            //            }
            //            #endregion

            //        }
            //        #endregion

            //        #endregion

            //    }
            //    #endregion

            //    #region در ماه جاری تکراری نیست پس یه دان تایم معمولی بگیر
            //    else//4 تکراری نیست یه دان تایم معمولی بگیر
            //    {
            //        RegisterDownTime(false);//تکراری نیست یه دان تایم معمولی بگیر
            //        return;
            //    }
            //    #endregion

            //    #endregion

            //}
            //#endregion



            //#endregion

            //#region موضوع دان تایم پی امی نیست 2
            //#region  Bیک دان تایم معمولی بگیره بره 
            //else //2 موضوع دان تایم پی امی نیست 
            //{
            //    //یک دان تایم معمولی بگیر بره
            //    RegisterDownTime(false);//فالس یعنی تکراری نیست
            //}
            //#endregion
            //#endregion

            //#endregion
        }

        private void textBoxDtRegCodeSite_Leave(object sender, EventArgs e)
        {
            EnableDisableBtnRegDt();

        }

        private void EnableDisableBtnRegDt()
        {
            if (txtBoxDtRegCodeSite.Text.Length == 4 && txtDtFromDate.Text != "" && (rdbPmCln.Checked || rdbPmSte.Checked || rdbPmTwr.Checked) && txtSelectedPerformer.Text != "")
            {
                btnRegPmDt.Enabled = true;
            }
            else
            {
                btnRegPmDt.Enabled = false;
            }
        }

        private void txtDtFromDate_Leave(object sender, EventArgs e)
        {
            EnableDisableBtnRegDt();
        }

        private void txtSelectedPerformer_TextChanged(object sender, EventArgs e)
        {
            strPerformer = txtSelectedPerformer.Text.Replace("  انجام دهنده ی انتخاب شده :", "");
            EnableDisableBtnRegDt();
        }

        private void gcTel_DoubleClick(object sender, EventArgs e)
        {
            EnableDisableBtnRegDt();
        }

        private void btnDtFromDate_Click(object sender, EventArgs e)
        {
            frmSelectDate objfrmSelectDate = new frmSelectDate();
            objfrmSelectDate.ShowDialog();

            if (MainForm.StrSelectedPershianDateWithSlashSeperator != string.Empty)//ساعت صفر را در نظر می گیرد که باید اصلاح گردد چون روز جاری را قبول نمی کند
            {
                gbxDtPerformer.Enabled = true;

                MainForm.StrDtStartDate = strEndDate = strStartDate = MainForm.StrDtStartDate = MainForm.StrSelectedPershianDateWithSlashSeperator;

                strStartYear_Month = strStartDate.Substring(0, 7).Replace("/", "_");

                DateTime dteDtRequstedStartDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrSelectedPershianDateWithSlashSeperator);

                int intCompareDate = DateTime.Compare(dteDtRequstedStartDate.Date, MainForm.DateTimeSqlServer.Date);

                if (!(intCompareDate < 0))
                {
                    txtDtFromDate.Text = MainForm.StrSelectedPershianDateWithNameOfDay;
                }
                else
                {
                    MessageBox.Show("تاریخ شروع دان تایم باید مساوی یا بزرگتر از تاریخ امروز باشد");
                    btnRegPmDt.Enabled = false;
                }
            }
            else
            {
                gbxDtPerformer.Enabled = false;
            }

            if (MainForm.dteDtStart < MainForm.DateTimeSqlServer.AddDays(2) && MainForm.StrCurRol != "administrator")
            {

                string message = "کاربر گرامی تاریخ شروع دان تایم پی ام حداقل باید دو روز کاری  با تاریخ درخواست فاصله داشته باشد .  ";
                MessageBox.Show(message, " اخطار در مورد عدم رعایت فاصله زمانی تاریخ شروع ", MessageBoxButtons.OK,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                gbxDtPerformer.Enabled = false;
                btnDtFromDate.PerformClick();

            }
            else
            {
                EnableDisableBtnRegDt();
            }


        }

        private void ResetStartDateForNewDownTime()
        {
            #region Reset For New DownTime
            txtDtFromDate.Text = ClsConvertDateHassan.GetMiladiDateReturnFullFarsiDatewithEnglishNumeric(MainForm.DateTimeSqlServer.AddDays(2));

            strEndDate = strStartDate = MainForm.StrDtStartDate = MainForm.strDtEndDate = ClsConvertDateHassan.convertDateToPersion(MainForm.DateTimeSqlServer.AddDays(2));

            #endregion
        }

        private string WhichRowInThisYearMonthHasSameCodeSubject()
        {
            string strOut = string.Empty;

            strStartYear = MainForm.StrDtStartDate.Substring(0, 4);// سال درخواستی استخراج می گردد
            strStartMont = MainForm.StrDtStartDate.Substring(5, 2);// ماه درخواستی استخراج می گردد

            string strSql = @"Select row FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'" + strSiteCode + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]!=N'" + strSubject + "'" + ") order by row desc";

            DataTable dt = objClsSql.FillDataTableFromQuery(strSql);


            if (dt.Rows.Count > 0)
            {
                strOut = dt.Rows[0].ToString();
            }
            else if (dt.Rows.Count == 0)
            {
                strOut = "Not Found";
            }


            return strOut;
        }

        private void btnRegPmDt_Click(object sender, EventArgs e)
        {
            #region بررسی تکمیل بودن چهار عنصر اساسی پی ام که کاربر مشخص می کند- که فکر کنم چون راهی برای اشتباه کاربر نذاشتم و فکر کنم بزودی این بخش رو حذف کنم
            if (strSiteCode.Length != 6)
            {
                string message = "لطفا کد سایت وارد شده را چک کنید ";
                MessageBox.Show(message, " اخطار در مورد کد سایت ", MessageBoxButtons.OK,

               MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
              MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }

            if (!(rdbPmCln.Checked || rdbPmSte.Checked || rdbPmTwr.Checked))
            {
                string message = "لطفایکی از موضوعات پی ام را انتخاب کنید ";
                MessageBox.Show(message, " اخطار در مورد عدم انتخاب موضوع ", MessageBoxButtons.OK,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }

            if (txtDtFromDate.Text == "")
            {
                string message = "لطفا  روز انجام پی ام را مشخص کنید انتخاب کنید ";
                MessageBox.Show(message, " اخطار در مورد روز انجام پی ام ", MessageBoxButtons.OK,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }

            if (txtSelectedPerformer.Text == "")
            {
                string message = "لطفاانجام دهنده ی پی ام را انتخاب کنید ";
                MessageBox.Show(message, " اخطار عدم انتخاب انجام دهنده ", MessageBoxButtons.OK,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }

            #endregion

            #region بررسی درخواست تکراری ( دقیقا همین درخواست ) و در صورت وجود خروج - همین کد سایت با همین موضوع در همین تاریخ شروع 
            if (IsThisDtRepetitious())
            {
                return;
            }
            #endregion

            #region بررسی اینکه آیا در این سال ماه کسی  دان تایمی با همین کد و موضوع درخواست داده یا نه که بریم فکر کنیم اجازه بدیم و سهمیه کم کنیم یا اصلا اجازه ندیم
            string strFoundedRow = WhichRowInThisYearMonthHasSameCodeSubject();
            #endregion

            #region درخواست پی ام  همین سایت همین موضوع همین ماه که تخلفه و نیاز است ببینیم سهمیه تخلف دارند یا نه

            #region در ماه جاری تکرایه
            if (IsRepeatedDtThisReqInThisMonthOfYear())// 3تکراریه وضعیت و ردیفشو دربیار تا ببینیم چیکارش باید کرد
            {
                #region ردیف آخرین جایی که تکرار شده رو در بیار

                string strRowNumberOfLastRepeatedDt = strSourceRowOfRepeatedRow = getLastRowNumberOfRepeatedDt();

                #endregion

                #region وضعیت آخرین جایی رو هم که دان تایم تکرار شده رو در بیار ببینم
                string strStateOfLastRepeatedDt = getLastStateOfRepeatedDt();

                #endregion

                #region اگر وضیعت غیر از تایید است بررسی کن وضعیت در حال پردازش یا ثبت در اس دی ام هست یا نه

                #region اگر وضعیت تایید نیست بررسی کن درحال پردازشه یا در وضعیت ثبت در اس دی ام ه یا غیر از اون که یوقت کلاه سرمون نره
                if (strStateOfLastRepeatedDt != "تائید")
                {
                    #region اگر وضعیت در حال پردازش درخواست می باشد
                    if (strStateOfLastRepeatedDt == "در حال پردازش درخواست")
                    {
                        MessageBox.Show(" کاربر گرامی شما قبلا در ردیف   " + strRowNumberOfLastRepeatedDt + " همین دان تایم را درخواست کرده اید و در حال پردازش درخواست شما هستیم و امکان درخواست همین دان تایم وجود ندارد ");
                        return;//خروج
                    }

                    #endregion

                    #region اگر وضعیت ثبت در اس دی ام هست
                    else if (strStateOfLastRepeatedDt == "ثبت در (SDM)")
                    {
                        MessageBox.Show(" کاربر گرامی شما قبلا در ردیف   " + strRowNumberOfLastRepeatedDt + " همین دان تایم را درخواست کرده اید و در وضعیت ثبت در اس دی ام و منتظر تایید تهران هستیم وامکان درخواست همین دان تایم وجود ندارد ");
                        return;//خروج
                    }
                    #endregion

                    #region Bاگر تایید نشده و در وضعیت پردازش و ثبت در اس دی ام نیست یک دان تایم معمولی بگیر
                    else
                    {
                        RegisterDownTime(false);//یک دان تایم معمولی بگیر چون تایید نشده که از سهمیه این بدبختا کم کنیم
                        return;//خروج
                    }
                    #endregion
                }
                #endregion

                #region اگه تایید شده برو تو کار بررسی سهمیه تکراری ها و بررسی داشتن یا نداشتن اجازه و اگه اجازه داره بسته به کنسل بودن یا نه میزان یک یا دو واحد از سهمیه کم کن
                else
                {
                    #region یادداشت آخرین جایی رو هم که دان تایم تکرار شده رو در بیار ببینم
                    string strNoteOfLastRepeatedDt = getNotetStateOfRepeatedDt();
                    #endregion

                    #region دان تایم تکراری میخواد ولی دمش گرم قبلا کنسل کرده بنابراین اگه سهمیه داره یکی از سهمیش کم کن و براش بگیر
                    if (strNoteOfLastRepeatedDt == "کنسل")
                    {
                        #region محاسبه  سهمیه
                        int intRemaindedRep = RemaindedRep(strRegion, strStartYear_Month);

                        bool bolHaveRemainRepForDownTime = intRemaindedRep > 1 ? true : false;// یعنی سهمیه باقی مانده 
                        #endregion

                        #region داشتن سهمیه و سوال برای ادامه و کسر
                        if (bolHaveRemainRepForDownTime)//سهمیه دارد
                        {

                            #region اگر موفقیت آمیز بود کسر سهمیه

                            string message = "در ردیف " + strRowNumberOfLastRepeatedDt + " همین دان تایم را درخواست کرده و کنسل کرده اید و در صورت ادامه یک واحد از سهمیه دان تایم تکراری شما کسر خواهد شد آیا ادامه می دهید؟ ";
                            DialogResult dialogResult = MessageBox.Show(message, " اخطار در مورد کسر سهمیه 1 واحدی ", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);


                            #endregion

                            #region اگه موافقه که یکی از سهمییش کم بشه بگیر
                            if (dialogResult == DialogResult.Yes)
                            {

                                #region ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
                                RegisterDownTime(true);//ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
                                #endregion

                                #region اگر موفقیت آمیز بود کسر سهمیه
                                if (bolIsSucssesRegDownTime)//اگر موفقیت آمیز بود کسر سهمیه
                                {
                                    //کسر سهمیه پیمانکار به اندازه یک واحد

                                    DecrementRemainRepForThisRegion(strRegion, strStartDate, 1);
                                    return;//خروج
                                }
                                #endregion


                            }
                            #endregion

                            #region اگه موافق نیست خارج شو
                            else if (dialogResult == DialogResult.No)
                            {
                                return;//خروج
                            }
                            #endregion


                        }
                        #endregion

                        #region نداشتن سهمیه و پیام مناسب و خروج
                        else
                        {
                            MessageBox.Show("سهمیه دان تایم تکراری شما در این ماه تمام شده و امکان ثبت دان تایم تکراری برای شما وجود ندارد");
                            return;
                        }
                        #endregion

                    }
                    #endregion

                    #region دان تایم تکراری میخواد و دفعه قبلی هم کنسل نکرده ببین اگه سهمیه داره دمدش  نرم دو تا ازش کم کن و بگیر
                    else
                    {
                        #region محاسبه  سهمیه

                        //محاسبه سهمیه
                        int intRemaindedRep = RemaindedRep(strRegion, strStartYear_Month);

                        #region اگه سهمیه ی باقیمانده یک هستش یه ارفاقی بکن 2 کن بتونه یکی بگیره
                        if (intRemaindedRep == 1)//اگه سهمیه ی باقیمانده یک هستش یه ارفاقی بکن 2 کن بتونه یکی بگیره
                        {
                            intRemaindedRep++;
                            //سهمیه رو دو کن
                        }
                        #endregion

                        bool bolHaveRemainRepForDownTime = intRemaindedRep > 1 ? true : false;// سهمیه باقی مانده 

                        #endregion

                        #region داشتن سهمیه و سوال برای ادامه و کسر
                        if (bolHaveRemainRepForDownTime)//سهمیه دارد
                        {

                            #region اگر موفقیت آمیز بود کسر سهمیه

                            string message = " در ردیف " + strRowNumberOfLastRepeatedDt + " همین دان تایم را درخواست کرده و کنسل نکرده اید و در صورت ادامه دو واحد از سهمیه دان تایم تکراری شما کسر خواهد شد آیا ادامه می دهید؟ \n";
                            DialogResult dialogResult = MessageBox.Show(message, " اخطار در مورد کسر سهمیه 2 واحدی ", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);


                            #endregion

                            #region اگه موافقه که یکی از سهمییش کم بشه بگیر
                            if (dialogResult == DialogResult.Yes)
                            {

                                #region ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
                                RegisterDownTime(true);//ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
                                #endregion

                                #region اگر موفقیت آمیز بود کسر سهمیه
                                if (bolIsSucssesRegDownTime)//اگر موفقیت آمیز بود کسر سهمیه
                                {
                                    //کسر سهمیه پیمانکار به اندازه دو واحد

                                    DecrementRemainRepForThisRegion(strRegion, strStartDate, 2);
                                    return;//خروج
                                }
                                #endregion


                            }
                            #endregion

                            #region اگه موافق نیست خارج شو
                            else if (dialogResult == DialogResult.No)
                            {
                                return;//خروج
                            }
                            #endregion


                        }
                        #endregion

                        #region نداشتن سهمیه و پیام مناسب و خروج
                        else
                        {
                            MessageBox.Show("سهمیه دان تایم تکراری شما در این ماه تمام شده و امکان ثبت دان تایم تکراری برای شما وجود ندارد");
                            return;
                        }
                        #endregion



                    }
                    #endregion

                }
                #endregion

                #endregion

            }
            #endregion

            #region اگه هیچ تکراری در کار نیست
            else
            {
                #region ثبت دان تایم  
                RegisterDownTime(true);
                #endregion
            }
            #endregion

            #endregion

            #region بروز رسانی جدول درخواست ها

            RefreshRequestDt();

            #endregion
        }

        private void btnPmFlowChart_Click(object sender, EventArgs e)
        {
            bool bolIsPermitDownTime = false;//شاخص مجوز کرفتن دان تایم پی امی
            bool bolHasSimilar = false;//شاخص مجوز کرفتن دان تایم پی امی

            #region بررسی تعداد دان تایم پی امی iهمین ماه همین کد سایت
            DataTable dt = LastRepeatedDtThisReqInThisMonthOfYear();//استخراج آخرین دان تایم در ماه سال که همین کدسایت رو داره

            int intCountOfRepThisCodeInYearMonth = dt.Rows.Count;//تعداد تکرار این کد سایت در ماه جاری چک می گردد 
            #endregion

            
            if (intCountOfRepThisCodeInYearMonth == 0)   //  شاخه 2 // اصلا در این ماه سال این کد سایت وجود ندارد
            #region شاخه 2
            {
                bolIsPermitDownTime = true;
                intKasreSahmieh = 0;
                //ای جان بگیر برو اصن نیست که بخوای تحلیل کنی
            }
            #endregion
           
            else//حداقل یه دان تایم با این کد سایت در سال ماه اخیر وجود داره  // شاخه 1
            #region شاخه 1
            {
                #region استخراج مشخصات آخرین دان تایم پی امی در ماه اخیر که همین کد سایت را دارد
                DataRow row = dt.Rows[0];//استخراج آخرین دان تایم باهمین کدسایت سال ماه

                string strRowOfLastSimilarReq = row["row"].ToString();
                string strStatusOfLastSimilarReq = row["status"].ToString();
                string strNoteOfLastSimilarReq = row["note"].ToString();
                string strStartDateOfLastSimilarReq = row["startDate"].ToString();
                string strSubjectOfLastSimilarReq = row["subject"].ToString();
                #endregion

                if (strSubjectOfLastSimilarReq == strSubject)//اگه موضوع یکسانه//شاخه 3
                #region شاخه 3

                {
                    #region شاخه 5
                    if (strStartDateOfLastSimilarReq == strStartDate)//اگه روزش هم یکسانه//شاخه 5
                    {
                        if (strStatusOfLastSimilarReq == "حذف" ||
                            strStatusOfLastSimilarReq == "نیاز به اصلاح درخواست" ||
                            strStatusOfLastSimilarReq == "عدم تائید" ||
                            strStatusOfLastSimilarReq == "مغایرت با دستورالعمل" ||
                            strStatusOfLastSimilarReq == "فریز شبکه"
                            )//بررسی انجام نشدنی بودن این دان تایم //شاخه 7
                            #region شاخه 7
                            {
                                bolIsPermitDownTime = true;
                                intKasreSahmieh = 0;
                                //معلومه نتونسته بگیره بذاریم بگیره بدبخت 
                            } 
                            #endregion
                        else//شاخه 8
                            #region شاخه 8
                        if (strStatusOfLastSimilarReq == "ثبت توسط کاربر" ||
                                strStatusOfLastSimilarReq == "ثبت در (SDM)" ||
                                strStatusOfLastSimilarReq == "در حال پردازش درخواست"
                                )//بررسی انجام نشدنی بودن این دان تایم //شاخه 9
                            #region شاخه 9
                            {
                                string message = "کاربر گرامی چون در ردیف  "+"\r\n"+ strRowOfLastSimilarReq + "\r\n"+" همین درخواست موجود است تا تعیین تکلیف نهایی منتظر بمانید .  ";
                                MessageBox.Show(message, " یادآوری موجود بودن همین درخواست ", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                                return;//بررسی گردد به کجا باید رفت
                            }
                        else//اگر هیچ کدام از این حالتها نیست لابد تایید است که بررسی می کنیم// شاخه 10
                        if (strStatusOfLastSimilarReq .Contains("تائید"))// شاخه 11
                        #region شاخه 11      1-3-5-8-10-11
                        {
                            string message = "کاربر گرامی چون در ردیف "+"\r\n"+ strRowOfLastSimilarReq + "\r\n"+" همین درخواست موجود است و تایید هم شده امکان ثبت مجدد وجود ندارد .  ";
                            MessageBox.Show(message, " یادآوری موجود بودن همین درخواست ", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                            return;//بررسی گردد به کجا باید رفت
                        }
                        #endregion
                        else// شاخه 12

                        #region شاخه 12     1 - 3 - 5 - 8 - 10 - 11
                        {

                            string message = "  کاربر گرامی خطای شماره 2 را به مدیر سیستم اطلاع دهید" + "\r\n" + strStatusOfLastSimilarReq + "\r\n" + " حالت قبلی دیده نشده در شاخه ی   " +"\r\n"+ "1 - 3 - 5 - 8 - 10 - 11";
                            MessageBox.Show(message, " یادآوری موجود بودن همین درخواست ", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                            //به مدیر سیتم خطای شماره دو را گزارش دهید
                        }
                        #endregion



                        #endregion 
                        #endregion

                    }
                    #endregion

                    else//اگه روزها یکسان نیست// شاخه 6
                    #region شاخه 6

                    //بحث سهمیه که باید کاملا پیاده سازی بشه
                    if (strStatusOfLastSimilarReq == "تائید" ||
                         strStatusOfLastSimilarReq == "تائید مشروط" ||
                           strStatusOfLastSimilarReq == "تائید ناموفق" ||
                             strStatusOfLastSimilarReq == "تائید موفق"
                        )// شاخه 17

                    #region شاخه 17
                    {
                        int intRemaindedRep = RemaindedRep(strRegion, strStartYear_Month);//بررسی سهمیه باقی مانده

                        if (strNoteOfLastSimilarReq.Contains("کنسل"))//اگر کلمه کنسل در یادداشت هست// شاخه 19
                                                                     //بررسی حداقل یک سهمیه برای کم کردن و دادن مجوز
                            if (intRemaindedRep >= 1) // شاخه 21
                            {
                                bolIsPermitDownTime = true;
                                intKasreSahmieh = -1;
                                //پیام مناسب که یکی کم میشه و بعد  اگر انجام شد یکی کم کنم
                                //رجیستر دان تایم با کم کردن یک سهمیه دان تایم تکراری

                            }
                            else// شاخه 22
                                bolIsPermitDownTime = false;
                        //نداری بابا . سهمیه نداری
                        else// شاخه 20
                            if (intRemaindedRep >= 2)// شاخه 23
                        {
                            bolIsPermitDownTime = true;
                            intKasreSahmieh = -2;
                            //پیام مناسب که دو تا کم میشه و بعد  اگر انجام شد یکی کم کنم
                            //رجیستر دان تایم با کم کردن دو سهمیه دان تایم تکراری
                        }
                        else // شاخه 24
                            bolIsPermitDownTime = false;
                        //نداری بابا . سهمیه نداری

                    }
                    #endregion

                    #region شاخه 18
                    else//شاخه 18
                                        if (strStatusOfLastSimilarReq == "عدم تائید" ||
                                        strStatusOfLastSimilarReq == "مغایرت با دستورالعمل" ||
                                        strStatusOfLastSimilarReq == "نیاز به اصلاح درخواست" ||
                                        strStatusOfLastSimilarReq == "شبکه فریز")// شاخه 25
                    {
                        bolIsPermitDownTime = true;//نتونسته بگیره مجوز بده بگیره
                        intKasreSahmieh = 0;
                    }


                    else// شاخه 26
                                            if (strStatusOfLastSimilarReq == "ثبت توسط کاربر" ||
                                                strStatusOfLastSimilarReq == "ثبت در (SDM)" ||
                                                strStatusOfLastSimilarReq == "در حال پردازش درخواست"
                                                )//بررسی انجام نشدنی بودن این دان تایم // شاخه 27
                                                 //کاربر گرامی در ردیف فلان برای یک روز متفاوت این دان تایم رو گرفتین و تعیین وضعیت نشده لطفا تا مشخص شدن وضعیت منتظر بمانید
                        bolIsPermitDownTime = false;
                    else // شاخه 28
                         //وضعیت نامشخص لطفا با مدیر سامانه تماس بگیرید و خطای شماره یک را گزارش دهید 
                        bolIsPermitDownTime = false;

                    #endregion 
                    #endregion
                }
                #endregion

                #region شاخه 4
                //اگه موضوع ها یکسان نیست

                    #region شاخه 13

                        #region شاخه 15

                    else//اگه موضوعش یکسان نیست//شاخه 4
                    if (strStartDateOfLastSimilarReq == strStartDate)//موضوع یکی نیست ولی روزش یکسانه//شاخه 13
                        if (strStatusOfLastSimilarReq == "تائید")//شاخه 15 
                        {
                            //ثبت در قسمت یادداشت برای بچه های دان تایم گیر که این پی امیه و مشابهش با موضوع متفاوت هست اگه میخوای همین شماره رو بزن
                            bolIsPermitDownTime = true;
                            intKasreSahmieh = 0;
                            bolHasSimilar = true;//موقع ثبت دان تایم در قسمت یادداشت بنویسد کانفلیکت با فلان شماره دان تایم
                        }
                        #endregion

                        #region شاخه 16
                        else// شاخه 16
                        {
                            bolIsPermitDownTime = true;//نه موضوع نه تاریخ یکیه تازه تاییدم نشده چیکارش داری بابا 
                            intKasreSahmieh = 0;

                        }  
                    #endregion

                    #endregion

                    #region شاخه 14
                    else//شاخه 14
                    {
                        bolIsPermitDownTime = true;//موضوع و روزش یکسان نیست بذار بگیره بدبخت
                        intKasreSahmieh = 0;
                    }  
                    #endregion

                #endregion


            } 
            #endregion



            #region ثبت دان تایم پی امی
            if (bolIsPermitDownTime)
            {
                bolIsSucssesRegDownTime = RegisterPmDownTime(intKasreSahmieh);
            }
            else
            { 

            }
            #endregion

        }

        private bool RegisterPmDownTime(int intDecrementSahmieh)
        {
            #region استخراج زمان دقیق ثبت از روی اس کیو ال سرور
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            #endregion

            if (MessageBox.Show($"آیا برای ایجاد {"دان تایم  "} بر روی سایت یا مورد {this.txtDtRegSiteName.Text} مطمئن هستید؟", "تاییدیه اعمال  تغییرات  ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string StrDtTempForSubjectCombo = string.Empty;

                if (strSiteName == "نامشخص")
                {
                    DialogResult dialogResultDelPersonal = MessageBox.Show(" نام سایت نامشخص می باشد  و احتمالا کد سایت نامعتبر وارد کرده اید آیا تمایل دارید این دان دان تایم ثبت گردد؟  ", "  اخطار ثبت دان تایم با نام نامشخص  ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                            MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                    if (dialogResultDelPersonal == DialogResult.No)
                    {
                        return false;
                    }

                }
            }

            #region استخراج زمان سرور برای ثبت زمان رجیستر
            strRegisterDate = ClsConvertDateHassan.ConvertDateToPersian(MainForm.DateTimeSqlServer);//استخراج زمان ثبت از اس کیو ال سرور
            
            strRegisterTime = MainForm.DateTimeSqlServer.ToString("HH:mm");
            #endregion

            #region پیدا کردن شماره بعدی برای ثبت دان تایم جدید
            int num=0;
            string strRow = objClsSql.FindFieldInLastRecord("[DT].[dbo].tblDownTimeShort", "row", "row");

            if (strRow == "نامشخص")
            {

                string message = "لطفا خطای شماره 3 را مدیر سیستم اطلاع دهید.  ";
                MessageBox.Show(message, " اخطار در عدم دریافت شماره ردیف جدید ", MessageBoxButtons.OK,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return false;
            }
            else
            {
                num = int.Parse(strRow);
            }

            int intNewRow = num + 1;
            #endregion

            #region ساخت آرایه فیلدها
            string strFieldsName =@"
                                    [row],
                                    [registerDate],
                                    [registerTime],
                                    [siteCode],
                                    [siteName],
                                    [curBSC],
                                    [startDate],
                                    [startTime],
                                    [endDate],
                                    [endTime],
                                    [applicant],
                                    [subject],
                                    [performer],
                                    [approver],
                                    [dtNo],
                                    [Status],
                                    [region],
                                    [Note],
                                    [outageMin]
                                   ";
            #endregion

            #region استخراج آرایه ی مقادیر
            string strValues = string.Empty;

            strValues = string.Concat(new object[] {
                                    "N'", intNewRow.ToString(),
                                    "',N'", strRegisterDate, 
                                    "',N'", strRegisterTime,
                                    "',N'", "ES"+strSiteCode, 
                                    "',N'",strSiteName, 
                                    "',N'", strCurBSC, 
                                    "',N'", strStartDate, 
                                    "',N'", strStartTime,
                                    "',N'", strEndDate,
                                    "',N'", strEndTime,
                                    "',N'", strApplicant,
                                    "',N'", strSubject,
                                    "',N'",  strTempForPerformer.Trim()+"-"+strTelOfPerformer+"-"+strTelOfPerformer, 
                                    "',N'",strApprover,
                                    "',N'", string.Empty, 
                                    "',N'ثبت در (SDM)'",
                                    ",N'",strRegion,
                                     "',N'", string.Empty,
                                    "',N'"+strOutageMin+"'" 
               
            });  
            #endregion;

            #region ثبت در اس کیو ال
            bolIsSucssesRegDownTime = objClsSql.GetAllAndSendToDataBase("[DT].[dbo].tblDownTimeShort", strFieldsName, strValues);//ثبت 
            #endregion

            #region ثبت شماره دان تایم تکراری و آخرین دان تایمی مرجعی که مشابه آنرا داریم در دیتابیس
            //if (bolIsRepeatedPmDownTime)
            //{
            //    objClsSql.GetAndSendToDataBaseAndReturnResult("[DT].[dbo].[tblRepetedDt]", "[repeatedRow]", intNewRow);
            //    objClsSql.EditRecord("[DT].[dbo].[tblRepetedDt]", "[sourceOfRepeatedRow]", strSourceRowOfRepeatedRow, "Where [repeatedRow]=N'" + intNewRow + "'");
            //    bolIsRepeatedPmDownTime = false;
            //    strSourceRowOfRepeatedRow = string.Empty;
            //}
            #endregion

            #region ثبت در لاگ
            if (bolIsSucssesRegDownTime)
            {
                ClsDiff.writeLogOfAction(" ایجاد دان تایم جدید ردیف  " + strRow);
            }
            else
            {
                ClsDiff.writeLogOfAction("تلاش ناموفق برای ثبت دان تایم");
            }
            #endregion

            return true;
        }
    }
}

      
        
    

