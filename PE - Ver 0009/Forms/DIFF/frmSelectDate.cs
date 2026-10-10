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

namespace PE.Forms
{
   
    public partial class frmSelectDate : Form
    {
        public frmSelectDate()
        {
            InitializeComponent();
        }

        private bool EnableDisableNextDays()//چک تاریخ 
        {
            bool bolEanable = false;

            int result = DateTime.Compare(farsiCalendar.GetSelectedDateInDateTime().Date, MainForm.DateTimeSqlServer);

            if (result < 0 || MainForm.StrCurUsr.Contains("reg"))
            {
                btnNextDay.Enabled = true;
                bolEanable = true;

            }
            else
            {
                btnNextDay.Enabled = false;
                bolEanable = false;
            }

            return bolEanable;
        }

        private bool EnableDisableLastDays()//چک تاریخ 
        {
            bool bolEanable = false;

            int result = DateTime.Compare(farsiCalendar.GetSelectedDateInDateTime().Date, MainForm.DateTimeSqlServer);

            if (result < 0 || MainForm.StrCurUsr.Contains("reg"))
            {
                btnNextDay.Enabled = true;
                bolEanable = true;

            }
            else
            {
                btnNextDay.Enabled = false;
                bolEanable = false;
            }

            return bolEanable;
        }

        private void gbxTchCalendar_Enter(object sender, EventArgs e)
        {
            FillAllTxtDateInForm();
        }

        private void FillAllTxtDateInForm()
        {

            #region پر کردن تکس باکس نام فارسی روز شمسی در صفحه انتخاب تاریخ و مقدار دهی به همین متغییر توتال در فرم اصلی

            txtShamsiDate.Text = farsiCalendar.GetSelectedDateInPersianDateTime().ToLongDateString();

            #endregion

            #region پر کردن تکس باکس نام  روز میلادی در صفحه انتخاب تاریخ 
             txtMiladiDate.Text = farsiCalendar.GetSelectedDateInDateTime().Year + "   " +
                       CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(farsiCalendar.GetSelectedDateInDateTime().Month) + "   " +
                       farsiCalendar.GetSelectedDateInDateTime().Day
                      ;
            #endregion

           

        }

        private void frmSelectDate_Load(object sender, EventArgs e)
        {
            #region پاک کردن تمام تاریخها

            #region شمسی
            MainForm.StrSelectedPershianDateWithNameOfDay = "";
            MainForm.StrSelectedPershianDateWithSlashSeperator = "";

            MainForm.StrSelectedPersianDateWithDotSeperator = "";
            MainForm.StrSelectedPershianDateNameOfWeek = "";

            #endregion

            #region میلادی

            MainForm.StrSelectedPershianDateWithNameOfDay = "";
            MainForm.StrSelectedPershianDateWithSlashSeperator = "";
            MainForm.StrSelectedPersianDateWithDotSeperator = "";
            MainForm.StrSelectedPershianDateNameOfWeek = "";

            #endregion

            #endregion

            #region تنظیم تاریخ شمسی امروز برای نمایش متنی روز جاری  از روی سرور
            farsiCalendar.SetTodayDate(MainForm.DateTimeSqlServer);
            #endregion

            #region تنظیم تاریخ  شمسی امروز برای روز انتخاب شده در جدول تقویم از روی سرور
            farsiCalendar.SetSelectedDate(MainForm.DateTimeSqlServer);
            #endregion

            #region اگر اکانت کاربر از نوع رجیستر نیست تاریخ و ماه بروز باشد
            if (!MainForm.StrCurUsr.Contains("reg"))
            {
                #region نمایش تکس باکس میلادی
                txtMiladiDate.Visible = true;
                #endregion

                #region تکمیل تکس باکسها با تاریخ شمسی و میلادی
                 txtShamsiDate.Text = farsiCalendar.GetSelectedDateInPersianDateTime().ToLongDateString();

                 txtMiladiDate.Text = farsiCalendar.GetSelectedDateInDateTime().Year + "   " +
                     CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(farsiCalendar.GetSelectedDateInDateTime().Month) + "   " +
                     farsiCalendar.GetSelectedDateInDateTime().Day;
                #endregion
            }
            #endregion

            #region اگر اکانت کاربر از نوع رجیستر هست تاریخ و ماه بروز باشد
            else
            {
                #region عدم نمایش میلادی چونکه برای پی ام نیازی نیست
                txtMiladiDate.Visible = false;
                #endregion

                #region نمایش تکس بالا بر اساس اولین روز ماه بعد

                int intDayOfMonth = farsiCalendar.GetSelectedDateInPersianDateTime().Day;

                txtShamsiDate.Text = farsiCalendar.GetSelectedDateInPersianDateTime().AddMonths(1).AddDays(-(intDayOfMonth - 1)).ToLongDateString();

                #endregion

                #region تنظیم تاریخ اولین روز شمسی ماه بعد  از روی سرور

                farsiCalendar.SetSelectedDate(MainForm.DateTimeSqlServer.AddMonths(1).AddDays(-(intDayOfMonth - 1)));

                #endregion


            }

            #endregion

        }

        private void farsiCalendar_Enter(object sender, EventArgs e)
        {
            FillAllTxtDateInForm();
        }

        private void farsiCalendar_SelectedDateChanged(DateTime selectedDateTime, BehComponents.PersianDateTime selectedPersianDateTime)
        {
            FillAllTxtDateInForm();
            
        }

        private void frmSelectDate_Enter(object sender, EventArgs e)
        {
            FillAllTxtDateInForm();
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            farsiCalendar.SetSelectedDate(farsiCalendar.GetSelectedDateInPersianDateTime().AddDays(1));

            FillAllTxtDateInForm();
        }

        private void btnLastDay_Click(object sender, EventArgs e)
        {

            farsiCalendar.SetSelectedDate(farsiCalendar.GetSelectedDateInPersianDateTime().AddDays(-1));

            FillAllTxtDateInForm();
        }

        private void btnSetDateOk_Click(object sender, EventArgs e)
        {
            


            #region پر کردن تکس باکسهای فرم تاریخ
            FillAllTxtDateInForm(); 
            #endregion

            #region پر کردن دیت تایمهای فرم اصلی
            FillAllVarDateTimeInMainForm();
            #endregion

            MainForm.strWhitchTabSetDatTime = "";//ریست نوع تاریخ و آمادگی برای هر تب دیگر

            if (MainForm.StrCurRol == "pm_requester")
            {
                if (MainForm.StrSelectedMiladiDateWithDotSeperator == "")
                {

                }
            }


            this.Close();
        }

        private void frmSelectDate_FormClosed(object sender, FormClosedEventArgs e)
        {
            #region پر کردن دیت تایمهای فرم اصلی
            FillAllVarDateTimeInMainForm(); 
            #endregion

            
        }

        private void FillAllVarDateTimeInMainForm()
        {
            #region تبدیل تاریخ شمسی دریافت شده به میلادی
            string strSelecteDateTimeInPershian = farsiCalendar.GetSelectedDateInPersianDateTime().ToShortDateString();
            DateTime selectedDateTimeInMiladi = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(strSelecteDateTimeInPershian);
            #endregion

            #region برگرداندن تاریخ شمسی به فرم اصلی با فرمت مختلف

            MainForm.StrSelectedPershianDateWithNameOfDay = farsiCalendar.GetSelectedDateInPersianDateTime().ToLongDateString();
            MainForm.StrSelectedPershianDateWithSlashSeperator = MainForm.StrSelectedPershianDateWithSlashSeperator = ClsDiff.EditDate(strSelecteDateTimeInPershian);
            MainForm.StrSelectedPersianDateWithDotSeperator = ClsDiff.EditDate(strSelecteDateTimeInPershian).Replace("/", ".");
            MainForm.StrSelectedPershianDateNameOfWeek = ClsConvertDateHassan.GetDayOfWeekName(selectedDateTimeInMiladi);

            #endregion

            #region برگردان تاریخ انتخاب شده ی شمسی به میلادی و تعیین تاریخ میلادی در فرم اصلی با فرمتهای مختلف

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("");
            MainForm.StrSelectedMiladiDateWithNameOfDay = selectedDateTimeInMiladi.ToString("dddd , dd  MMM  yyyy", culture);

            MainForm.StrSelectedMiladiDateWithSlashSeperator = selectedDateTimeInMiladi.ToString("yyyy/MM/dd");
            MainForm.StrSelectedMiladiDateWithDotSeperator = MainForm.StrSelectedMiladiDateWithSlashSeperator.Replace("/", ".");
            MainForm.StrSelectedMiladiDateNameOfWeek = selectedDateTimeInMiladi.DayOfWeek.ToString();

            #endregion

            #region پرکردن متغییرهای تاریخی میلادی دان تایم

            MainForm.dteDtStart = selectedDateTimeInMiladi;

            #endregion

        }
    }
}
