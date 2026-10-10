//using Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static DevExpress.Xpo.Helpers.CommandChannelHelper;

namespace PE.Forms.DT
{
    public partial class frmAmar : Form
    {
        string strDateTimeNow;
        int intDateTimeNow;
        ClsSql objClsSql;
        public frmAmar()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void frmAmar_Load(object sender, EventArgs e)
        {
            #region  استخراج تاریخ اس کیو ال و تنظیم

            strDateTimeNow = ClsConvertDateHassan.convertDateTimeToPersionWithSlash(MainForm.DateTimeSqlServer);

            lblStatisticFromNew.Text = lblStatisticToNew.Text = strDateTimeNow.Substring(0, 10);


            string strYearNow = strDateTimeNow.Substring(0, 4);
            string strMonthNow = strDateTimeNow.Substring(5, 2);
            string strDayNow = strDateTimeNow.Substring(8, 2);

            #region صفر دار کردن ماه روزهای تک رقمی

            string strMonthWithZero = (strMonthNow.Count() < 2) ? "0" + strMonthNow : strMonthNow;

            string strDayWithZero = (strDayNow.Count() < 2) ? "0" + strDayNow : strDayNow;
            //string strDayWithZero = (strDayNow.Substring(0, 1) == "0") ? strDayNow.Substring(0) : strDayNow;
            #endregion

            intDateTimeNow = int.Parse(strYearNow + strMonthWithZero + strDayWithZero);

            #region تنظیم نامریکها با مقادیر امروز

            numericUpDownُStaticFromYearNew.Value = numericUpDownُStaticToYearNew.Value = int.Parse(strYearNow);
            numericUpDownُStaticFromMonthNew.Value = numericUpDownُStaticToMonthNew.Value = int.Parse(strMonthNow);
            numericUpDownُStaticFromDayNew.Value = numericUpDownُStaticToDayNew.Value = int.Parse(strDayNow);
            #endregion

            #endregion


            #region ایجاد آمار ماه اخیر


            string strCurYearMonth = strYearNow + "/" + strMonthNow;

            string strQueryTotalInCurrentMonth = @"
                SELECT     approver, COUNT(Distinct dtNo) AS tedad
                FROM         DT.dbo.tblDownTimeShort
                WHERE     (registerDateInSdm Like N'" + strCurYearMonth + @"%' AND dtNo!=N'') 
                GROUP BY approver
                ";
            DataTable dtTotalInCurrentMonth = objClsSql.FillDataTableFromQuery(strQueryTotalInCurrentMonth);

            chartTotalCurrentMonth.DataSource = dtTotalInCurrentMonth;
            chartTotalCurrentMonth.ChartAreas[0].AxisX.Interval = 1;
            chartTotalCurrentMonth.Series.Clear();
            chartTotalCurrentMonth.ChartAreas[0].AxisY.Maximum = 400;

            Series series1 = new Series();

            for (int j = 1; j < dtTotalInCurrentMonth.Columns.Count; j++)
            {
               

                series1.XValueMember = dtTotalInCurrentMonth.Columns[0].ColumnName;
                series1.YValueMembers = dtTotalInCurrentMonth.Columns[j].ColumnName;

                series1.ChartType = SeriesChartType.Bar;

                series1.IsVisibleInLegend = true;
                series1.IsValueShownAsLabel = true;
                series1.BorderWidth = 3;

                chartTotalCurrentMonth.Series.Add(series1);
            }

            


            #endregion


            btnStatisticCalculateNew.PerformClick();
        }

        private void btnStatisticCalculateNew_Click(object sender, EventArgs e)
        {
            groupBoxFromTo.Visible = true;

            #region استخراج تاریخ درخواستی و ولیدیشن آن
            string strFromMonth = numericUpDownُStaticFromMonthNew.Value.ToString();
            string strFromDay = numericUpDownُStaticFromDayNew.Value.ToString();

            strFromMonth = (strFromMonth.Count() < 2) ? "0" + strFromMonth : strFromMonth;
            strFromDay = (strFromDay.Count() < 2) ? "0" + strFromDay : strFromDay;

            lblStatisticFromNew.Text = numericUpDownُStaticFromYearNew.Value.ToString() + "/" + strFromMonth + "/" + strFromDay;

            string strToMonth = numericUpDownُStaticToMonthNew.Value.ToString();
            string strToDay = numericUpDownُStaticToDayNew.Value.ToString();

            strToMonth = (strToMonth.Count() < 2) ? "0" + strToMonth : strToMonth;
            strToDay = (strToDay.Count() < 2) ? "0" + strToDay : strToDay;

            lblStatisticToNew.Text = numericUpDownُStaticToYearNew.Value.ToString() + "/" + strToMonth + "/" + strToDay;

            int intFrom = int.Parse(numericUpDownُStaticFromYearNew.Value.ToString() + strFromMonth + strFromDay);
            int intTo = int.Parse(numericUpDownُStaticToYearNew.Value.ToString() + strToMonth + strToDay);

            if (intFrom > intDateTimeNow)
            {
                MessageBox.Show("خطا", "تاریخ شروع نمی تواند از تاریخ جاری بزرگتر باشد لطفا اصلاح بفرمایید", MessageBoxButtons.OK,

                                  MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                                 MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;

            }


            if (intTo < intFrom)
            {
                MessageBox.Show("خطا", "تاریخ پایان نمی تواند از تاریخ شروع کوچکتر باشد لطفا اصلاح بفرمایید", MessageBoxButtons.OK,

                                   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                                  MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }
            #endregion


            string strQuery = @"
                SELECT     approver, COUNT(*) AS tedad
                FROM         DT.dbo.tblDownTimeShort
                WHERE     (registerDateInSdm >=N'" + lblStatisticFromNew.Text + @"' AND registerDateInSdm <= N'" + lblStatisticToNew.Text + @"')
                GROUP BY approver
                ";

            DataTable dt = objClsSql.FillDataTableFromQuery(strQuery);

            chartRowsNew.DataSource = dt;
            chartRowsNew.ChartAreas[0].AxisX.Interval = 1;
            chartRowsNew.Series.Clear();

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                Series series = new Series();

                series.XValueMember = dt.Columns[0].ColumnName;
                series.YValueMembers = dt.Columns[i].ColumnName;

                series.ChartType = SeriesChartType.Pie;

                series.IsVisibleInLegend = true;
                series.IsValueShownAsLabel = true;
                series.BorderWidth = 3;


                chartRowsNew.Series.Add(series);

                strQuery = @"
                             SELECT   COUNT(*) AS tedad
                    FROM         DT.dbo.tblDownTimeShort
                    WHERE     (registerDateInSdm >=N'" + lblStatisticFromNew.Text + @"' AND registerDateInSdm <= N'" + lblStatisticToNew.Text + @"')

                    ";
                dt = objClsSql.FillDataTableFromQuery(strQuery);

            }

            lblTotalDtNew.Text = dt.Rows[0].ItemArray[0].ToString();


            string strQuery2 = @"
                SELECT     approver, COUNT(Distinct dtNo) AS tedad
                FROM         DT.dbo.tblDownTimeShort
                WHERE     (registerDateInSdm >=N'" + lblStatisticFromNew.Text + @"' AND registerDateInSdm <= N'" + lblStatisticToNew.Text + @"' AND dtNo!=N'') 
                GROUP BY approver
                ";
            DataTable dt2 = objClsSql.FillDataTableFromQuery(strQuery2);

            chartCrsNew.DataSource = dt2;
            chartCrsNew.ChartAreas[0].AxisX.Interval = 1;
            chartCrsNew.Series.Clear();


            for (int j = 1; j < dt2.Columns.Count; j++)
            {
                Series series1 = new Series();

                series1.XValueMember = dt2.Columns[0].ColumnName;
                series1.YValueMembers = dt2.Columns[j].ColumnName;

                series1.ChartType = SeriesChartType.Pie;

                series1.IsVisibleInLegend = true;
                series1.IsValueShownAsLabel = true;
                series1.BorderWidth = 3;


                chartCrsNew.Series.Add(series1);

                strQuery = @"
                             SELECT  COUNT(Distinct dtNo) AS tedad
                    FROM         DT.dbo.tblDownTimeShort
                    WHERE     (registerDateInSdm >=N'" + lblStatisticFromNew.Text + @"' AND registerDateInSdm <= N'" + lblStatisticToNew.Text + @"')

                    ";
                dt2 = objClsSql.FillDataTableFromQuery(strQuery);
            }

            lblTotalCrsNew.Text = dt2.Rows[0].ItemArray[0].ToString();
        }

        private void numericUpDownُStaticFromDayNew_ValueChanged(object sender, EventArgs e)
        {
            groupBoxFromTo.Visible = false;
        }

        private void numericUpDownُStaticToDayNew_ValueChanged(object sender, EventArgs e)
        {
            groupBoxFromTo.Visible = false;
        }

        private void numericUpDownُStaticFromMonthNew_ValueChanged(object sender, EventArgs e)
        {
            groupBoxFromTo.Visible = false;
        }

        private void numericUpDownُStaticToMonthNew_ValueChanged(object sender, EventArgs e)
        {
            groupBoxFromTo.Visible = false;
        }

        private void numericUpDownُStaticFromYearNew_ValueChanged(object sender, EventArgs e)
        {
            groupBoxFromTo.Visible = false;
        }

        private void numericUpDownُStaticToYearNew_ValueChanged(object sender, EventArgs e)
        {
            groupBoxFromTo.Visible = false;
        }
    }
}
