using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE
{
    internal class clsDt
    {
        static ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public static  bool ThisDtExactlyExisted(string strTxtCodeSite,string strSubject, int nudDtStHour,int nudDtStMin)
        {
            string strStartTimeHour = nudDtStHour > 10 ? nudDtStHour.ToString() : "0" + nudDtStHour;
            string strStartTimeMin = nudDtStMin > 10 ? nudDtStMin.ToString() : "0" + nudDtStMin;
            MainForm.strDtReqStartTime = strStartTimeHour + ":" + strStartTimeMin;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND startDate=N'" + MainForm.strDtStartDate + "' AND startTime=N'" + MainForm.strDtReqStartTime + "' AND [subject]=N'" + strSubject + "')  order by row desc";
            objClsSql = new ClsSql();
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);

            bool bolIsExist = false;
            if (dt.Rows.Count > 0)
            {
                bolIsExist = true;
            }
            else
            {
                bolIsExist = false;
            }

            return bolIsExist;
        }

        //به نظرم این تابع نیازی نیست و از همان قبلی بدست می اید
        public static string GetRowOfExactlyRepeatedDowntime(string strtRegCodeSite,string strSubject)
        {
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);


            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'" + strtRegCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "') order by row desc";
            string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            return strFindedRowOfRepeatedDt;
        }
    }
}
