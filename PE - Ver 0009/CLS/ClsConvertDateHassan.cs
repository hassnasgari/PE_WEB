namespace PE
{
    using System;
    using System.Globalization;

    public class ClsConvertDateHassan
    {

        public static DateTime convertShamsiToMiladiReturnDate(string strShamsi)//1396/08/23
        {
            string strYear = "";
            string strMonth = "";
            string strDay = "";
            string[] arrStrShamsiDate= {"1398","01","01"};

            if (strShamsi.Contains("/"))
            {
                arrStrShamsiDate = strShamsi.Split('/');
            }

            if(strShamsi.Contains("."))
            {
                arrStrShamsiDate = strShamsi.Split('.');
            }
            if (strShamsi.Contains("-"))
            {
                arrStrShamsiDate = strShamsi.Split('-');
            }

            if (arrStrShamsiDate[0].Length<4)
            {
                strYear = "13"+ arrStrShamsiDate[0];
            }
            else
            {
                strYear =  arrStrShamsiDate[0];
            }

            if (arrStrShamsiDate[1].Length <2)
            {
                strMonth = "0" + arrStrShamsiDate[1];
            }
            else
            {
                strMonth = arrStrShamsiDate[1];
            }

            if (arrStrShamsiDate[2].Length < 2)
            {
                strDay = "0" + arrStrShamsiDate[2];
            }
            else
            {
                strDay = arrStrShamsiDate[2];
            }


            PersianCalendar pc = new PersianCalendar();
            DateTime dte = new DateTime(int.Parse(strYear),int.Parse(strMonth), int.Parse (strDay), pc);


            return dte.Date;
        }

        public static string convertDateAndDayToPersion(DateTime dteMiladiDate)
        {
            PersianCalendar calendar = new PersianCalendar();
            string str = "";
            string str2 = "";
            string str3 = "";
            str = calendar.GetYear(dteMiladiDate).ToString();
            str2 = calendar.GetMonth(dteMiladiDate).ToString();
            str3 = calendar.GetDayOfMonth(dteMiladiDate).ToString();
            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            string str4 = MainForm.DateTimeSqlServer.DayOfWeek.ToString();
            switch (str4)
            {
                case "Saturday":
                    str4 = "شنبه";
                    break;

                case "Sunday":
                    str4 = "یک شنبه";
                    break;

                case "Monday":
                    str4 = "دوشنبه";
                    break;

                case "Tuesday":
                    str4 = "سه شنبه";
                    break;

                case "Wednesday":
                    str4 = "چهارشنبه";
                    break;

                case "Thursday":
                    str4 = "پنج شنبه";
                    break;

                case "Friday":
                    str4 = "جمعه";
                    break;
            }
            return $"{str4}  {str}/{str2}/{str3}";
        }

        public static string ConvertDateTimeToPersianWithDayOfWeek(DateTime dteMiladiDate)
        {
            PersianCalendar calendar = new PersianCalendar();
            string str = "";
            string str2 = "";
            string str3 = "";
            string str4 = dteMiladiDate.ToLongTimeString();
            str = calendar.GetYear(dteMiladiDate).ToString();
            str2 = calendar.GetMonth(dteMiladiDate).ToString();
            str3 = calendar.GetDayOfMonth(dteMiladiDate).ToString();
            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            string str5 = dteMiladiDate.DayOfWeek.ToString();
            switch (str5)
            {
                case "Saturday":
                    str5 = " شنبه ";
                    break;

                case "Sunday":
                    str5 = " یک شنبه ";
                    break;

                case "Monday":
                    str5 = " دوشنبه ";
                    break;

                case "Tuesday":
                    str5 = " سه شنبه ";
                    break;

                case "Wednesday":
                    str5 = " چهارشنبه ";
                    break;

                case "Thursday":
                    str5 = " پنج شنبه ";
                    break;

                case "Friday":
                    str5 = " جمعه ";
                    break;
            }
            return (str5 + "    " + $"  {str}/{str2}/{str3}  ");
        }

        public static string convertDateTimeToPersion(DateTime dteMiladiDate)
        {
            PersianCalendar calendar = new PersianCalendar();
            string str = "";
            string str2 = "";
            string str3 = "";
            string str4 = dteMiladiDate.ToString("HH:mm");
            str = calendar.GetYear(dteMiladiDate).ToString();
            str2 = calendar.GetMonth(dteMiladiDate).ToString();
            str3 = calendar.GetDayOfMonth(dteMiladiDate).ToString();
            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            return $"{str}{str2}{str3}__{str4}";
        }

        public static string convertDateTimeToPersionWithDashWithOutTime(DateTime dteMiladiDate)
        {
            PersianCalendar calendar = new PersianCalendar();
            string str = "";
            string str2 = "";
            string str3 = "";
            str = calendar.GetYear(dteMiladiDate).ToString();
            str2 = calendar.GetMonth(dteMiladiDate).ToString();
            str3 = calendar.GetDayOfMonth(dteMiladiDate).ToString();
            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            return $"{str}-{str2}-{str3}";
        }
        public static string ConvertDateToPersian( DateTime dteMiladiDate)
        {
            PersianCalendar calendar = new PersianCalendar();
            string str = "";
            string str2 = "";
            string str3 = "";
            str = calendar.GetYear(dteMiladiDate).ToString();
            str2 = calendar.GetMonth(dteMiladiDate).ToString();
            str3 = calendar.GetDayOfMonth(dteMiladiDate).ToString();
            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            return $"{str.Substring(0, 4)}/{str2}/{str3}";
        }

        public static string convertDateToPersion(DateTime dteMiladiDate)
        {
            PersianCalendar calendar = new PersianCalendar();
            string str = "";
            string str2 = "";
            string str3 = "";
            str = calendar.GetYear(dteMiladiDate).ToString();
            str2 = calendar.GetMonth(dteMiladiDate).ToString();
            str3 = calendar.GetDayOfMonth(dteMiladiDate).ToString();
            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            return $"{str}/{str2}/{str3}";
        }

        public DateTime convertFormat(string strMiladiDate)
        {
            strMiladiDate = strMiladiDate.Replace("_", " ");
            string str = strMiladiDate.Substring(9);
            return DateTime.Parse("20" + strMiladiDate.Substring(6, 2) + "/" + strMiladiDate.Substring(0, 2) + "/" + strMiladiDate.Substring(3, 2) + " " + str);
        }

        public static string Shamsi2Miladi(string _dateMiladi)
        {
            int year = int.Parse(_dateMiladi.Substring(0, 4));
            int month = int.Parse(_dateMiladi.Substring(5, 2));
            int day = int.Parse(_dateMiladi.Substring(8, 2));
            DateTime time = new PersianCalendar().ToDateTime(year, month, day, 0, 0, 0, 0);
            return (time.ToString("yyyy/MM/dd") + "   " + time.DayOfWeek);
        }

        public static string convertDateTimeToTenCharAndDashFormat(DateTime dteMiladiDate)
        {

            string str = "";
            string str2 = "";
            string str3 = "";

            str = dteMiladiDate.Year.ToString();
            str2 = dteMiladiDate.Month.ToString();
            str3 = dteMiladiDate.Day.ToString();

            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            return $"{str}/{str2}/{str3}";
        }

        public static string convertDateTimeToPersionWithSlash(DateTime dteMiladiDate)
        {
            PersianCalendar calendar = new PersianCalendar();
            string str = "";
            string str2 = "";
            string str3 = "";
            string str4 = dteMiladiDate.ToString("HH:mm");
            str = calendar.GetYear(dteMiladiDate).ToString();
            str2 = calendar.GetMonth(dteMiladiDate).ToString();
            str3 = calendar.GetDayOfMonth(dteMiladiDate).ToString();
            if (str2.Length == 1)
            {
                str2 = "0" + str2;
            }
            if (str3.Length == 1)
            {
                str3 = "0" + str3;
            }
            str2 = "/"+str2;
            str3 = "/"+str3;
            return $"{str}{str2}{str3}__{str4}";
        }

        public static string convertNameDayFromMiladiToShamsi(string strMiladiDate)
        {
            string strShamsiDate = "";
            switch (strMiladiDate)
            {
                case "Saturday":
                    strShamsiDate = "شنبه";
                    break;

                case "Sunday":
                    strShamsiDate = "یک شنبه";
                    break;

                case "Monday":
                    strShamsiDate = "دوشنبه";
                    break;

                case "Tuesday":
                    strShamsiDate = "سه شنبه";
                    break;

                case "Wednesday":
                    strShamsiDate = "چهارشنبه";
                    break;

                case "Thursday":
                    strShamsiDate = "پنج شنبه";
                    break;

                case "Friday":
                    strShamsiDate = "جمعه";
                    break;
            }

            return strShamsiDate;
        }

        public static int WhatsNumberOfTodayInMonth()
        {
            PersianCalendar calendar = new PersianCalendar();

            return calendar.GetDayOfMonth(DateTime.Now);
          
        }

        public static int howLongThisMonth()
        {
            int intCntOfDay = 0;

            PersianCalendar calendar = new PersianCalendar();
            DateTime dte = DateTime.Now;

            int intYear = calendar.GetYear(dte);
            int intMonth = calendar.GetMonth(dte);

            if (intMonth < 7)
                intCntOfDay = 31;
            if (6 <=intMonth && intMonth<=11)
                intCntOfDay = 30;

            bool bolKabiseh = calendar.IsLeapMonth(intYear, intMonth);

            if (intMonth==12 && bolKabiseh)
                intCntOfDay = 30;

            if (intMonth == 12 && !bolKabiseh)
                intCntOfDay = 29;

            return intCntOfDay;
        }

        public static string[] recentlyFiveDay()
        {
            string[] strRecently5day = new string[5];

            PersianCalendar calendar = new PersianCalendar();

            //DateTime dte = MainForm.dateTimeSqlServer;
            DateTime dte = MainForm.DateTimeSqlServer.AddDays(10);//موقت 

            string strPershDate = calendar.GetYear(dte) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetMonth(dte)) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetDayOfMonth(dte));
            strRecently5day[4] = strPershDate;

            DateTime dteMinus1 = dte.AddDays(-1);

            strRecently5day[3] = calendar.GetYear(dteMinus1) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetMonth(dteMinus1)) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetDayOfMonth(dteMinus1));

            DateTime dteMinus2 = dte.AddDays(-2);
            strRecently5day[2] = calendar.GetYear(dteMinus2) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetMonth(dteMinus2)) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetDayOfMonth(dteMinus2));

            DateTime dteMinus3 = dte.AddDays(-3);
            strRecently5day[1] = calendar.GetYear(dteMinus3) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetMonth(dteMinus3)) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetDayOfMonth(dteMinus3));

            DateTime dteMinus4 = dte.AddDays(-4);

            strRecently5day[0] = calendar.GetYear(dteMinus4) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetMonth(dteMinus4)) + "/" + ConvertMonthAndDayToTwoDigFormat(calendar.GetDayOfMonth(dteMinus4));

            return strRecently5day;
        }

        public static string ConvertMonthAndDayToTwoDigFormat(int intMonthOrDay)
        {
            string strTemp = intMonthOrDay.ToString();

            if (strTemp.Length < 2)
                strTemp = "0" + strTemp;
            return strTemp;
        }

        public static bool IsLast4_Or_5_Or_6_DayOfMonth()
        {
            int intNumberOfTodayInMonth = WhatsNumberOfTodayInMonth();
            int inthowLongThisMonth = howLongThisMonth();

            if (intNumberOfTodayInMonth >= 26 && intNumberOfTodayInMonth <= inthowLongThisMonth)
                return true;
            else
            return false;
        }

        public static string stringOfFirstPershianDateOfLastMonth()
        {
            string strLastMont = "";

            PersianCalendar calendar = new PersianCalendar();

            DateTime dte = DateTime.Now;

            int intCurMonth = calendar.GetMonth(dte);
            int intLastMounth = intCurMonth - 1;
            strLastMont = intLastMounth.ToString();

            if (intLastMounth<10)
            {
                strLastMont = "0" + strLastMont;
            }

            string strPershDate = calendar.GetYear(dte) + "/" + strLastMont + "/" + "01";

            return strPershDate;
        }

        public static string GetMonthName( DateTime date)
        {
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');
            int month = Convert.ToInt32(dates[1]);

            switch (month)
            {
                case 1: return "فررودين";
                case 2: return "ارديبهشت";
                case 3: return "خرداد";
                case 4: return "تير‏";
                case 5: return "مرداد";
                case 6: return "شهريور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دي";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return "";
            }

        }

        public static string GetDayOfWeekName( DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday: return "شنبه";
                case DayOfWeek.Sunday: return "يکشنبه";
                case DayOfWeek.Monday: return "دوشنبه";
                case DayOfWeek.Tuesday: return "سه‏شنبه";
                case DayOfWeek.Wednesday: return "چهارشنبه";
                case DayOfWeek.Thursday: return "پنجشنبه";
                case DayOfWeek.Friday: return "جمعه";
                default: return "";
            }
        }

        public static string GetMiladiDateReturnFullFarsiDate( DateTime date)
        {
            string strPershianDayOfWeek = GetDayOfWeekName(date);
            string strPershianMonth = GetMonthName(date);

            PersianCalendar jc = new PersianCalendar();

            string strOut = string.Format("{3} {2} {1} {0:0000}" , strPershianMonth, jc.GetDayOfMonth(date), strPershianDayOfWeek, jc.GetYear(date));

            strOut = strOut.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").
                Replace("5", "۵").Replace("۶", "۱").Replace("v", "۱").Replace("8", "۸").Replace("9", "۹");
            return strOut;
        }
        public static string GetMiladiDateReturnFullFarsiDatewithEnglishNumeric(DateTime date)
        {
            string strPershianDayOfWeek = GetDayOfWeekName(date);
            string strPershianMonth = GetMonthName(date);

            PersianCalendar jc = new PersianCalendar();

            string strOut = string.Format("{3} {2} {1} {0:0000}", strPershianMonth, jc.GetDayOfMonth(date), strPershianDayOfWeek, jc.GetYear(date));

            //strOut = strOut.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").
            //Replace("5", "۵").Replace("۶", "۱").Replace("v", "۱").Replace("8", "۸").Replace("9", "۹");
            return strOut;
        }

        public static string GetMiladiDateReturnDayOfTheWeekInPershian(DateTime date)
        {
            string strPershianDayOfWeek = GetDayOfWeekName(date);

            string strOut =strPershianDayOfWeek;

          
            return strOut;
        }

        public static string setNameOfNextMonthShamsi()
        {
            DateTime date = MainForm.DateTimeSqlServer;
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');

            int month = Convert.ToInt32(dates[1]);
            month++;
            if (month==13)
            {
                month = 1;
            }

            switch (month)
            {
                case 1: return "فررودين";
                case 2: return "ارديبهشت";
                case 3: return "خرداد";
                case 4: return "تیر ماه‏";
                case 5: return "مرداد ماه";
                case 6: return "شهريور ماه";
                case 7: return "مهر ماه";
                case 8: return "آبان ماه";
                case 9: return " آذر ماه";
                case 10: return " دی ماه";
                case 11: return "بهمن ماه";
                case 12: return "اسفند ماه";
                default: return "";
            }


           
        }

        public static int setIntOfNextMonth()
        {
            DateTime date = MainForm.DateTimeSqlServer;
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');
            int month = Convert.ToInt32(dates[1]);
            month++;
            if (month == 13)
            {
                month = 1;
            }

            return month;

        }

        public static string setIntOfNextMonthShmsi()
        {
            DateTime date = MainForm.DateTimeSqlServer;
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');
            int month = Convert.ToInt32(dates[1]);
            month++;
            if (month == 13)
            {
                month = 1;
            }

            return month.ToString();
        }

        public static string setIntOfNextYearShmsi()
        {
           
            DateTime date = MainForm.DateTimeSqlServer;
            PersianCalendar jc = new PersianCalendar();

            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

            string[] dates = pdate.Split('/');

            int year =Convert.ToInt32(dates[0]); ;
            int month = Convert.ToInt32(dates[1]);

            month++;
            if (month == 13)
            {
                year++;
            }

            return year.ToString();
        }

        public static DateTime FindFirstShamsiDateOfNextMonthToMiladi()//اولین روز ماه شمیس آتی را پیدا و به میلادی تبدیل می کند
        {
            DateTime curDate = MainForm.DateTimeSqlServer;//تاریخ میلادی امروز

            #region تاریخ شمسی امروز
            PersianCalendar jc = new PersianCalendar();
            string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(curDate), jc.GetMonth(curDate), jc.GetDayOfMonth(curDate));
            #endregion

            #region جداسازی ماه و سال از تاریخ شمسی
            string[] dates = pdate.Split('/');

            int month = Convert.ToInt32(dates[1]);
            int year = Convert.ToInt32(dates[0]);
            #endregion

            #region اضافه کردن ماه و در صورت نیاز سال
            month++;

            if (month == 13)
            {
                month = 1;
                year++;

            } 
            #endregion

            string strFirstDateOfNextMonth = year.ToString() + "/" + month.ToString() + "/" + "01";//ساخت اولین روز شمسی ماه بعد 

            DateTime dteResult = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(strFirstDateOfNextMonth);//پیدا کردن اولین روز شمسی ماه بعد به میلادی

            return dteResult;

        }

        public static long ConvertMiladiDateToLong(DateTime dteMiladi)
        {


            string strMiladiYear = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(dteMiladi.Year).ToString();
            string intMiladiMonth = dteMiladi.Month.ToString("00");
            string intMiladiDay = dteMiladi.Day.ToString("00");

            string intHour = dteMiladi.Hour.ToString("00");
            string intMinute = dteMiladi.Minute.ToString("00");

            string strDateTimeMiladiSqlServer = strMiladiYear + intMiladiMonth + intMiladiDay + intHour + intMinute;
            return  long.Parse(strDateTimeMiladiSqlServer);


        }
    }
}

