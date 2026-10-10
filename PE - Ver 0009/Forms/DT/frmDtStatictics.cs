using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PE.Forms.DT
{
    public partial class frmDtStatictics : Form
    {
        string strDateTimeNow;
        int intDateTimeNow;
        ClsSql objClsSql;
        public frmDtStatictics()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void frmDtStatictics_Load(object sender, EventArgs e)
        {
            #region  استخراج تاریخ اس کیو ال و تنظیم

            strDateTimeNow = ClsConvertDateHassan.convertDateTimeToPersionWithSlash(MainForm.DateTimeSqlServer);

            lblStatisticFrom.Text = lblStatisticTo.Text = strDateTimeNow.Substring(0, 10);


            string strYearNow = strDateTimeNow.Substring(0, 4);
            string strMonthNow = strDateTimeNow.Substring(5, 2);
            string strDayNow = strDateTimeNow.Substring(8, 2);

            #region صفر دار کردن ماه روزهای تک رقمی
            string strMonthWithZero = (strMonthNow.Substring(1) == "0") ? strMonthNow.Substring(1, 1) : strMonthNow;
            string strDayWithZero = (strDayNow.Substring(0, 1) == "0") ? strDayNow.Substring(0) : strDayNow;
            #endregion

            intDateTimeNow = int.Parse(strYearNow + strMonthWithZero + strDayWithZero);

            #region تنظیم نامریکها با مقادیر امروز

            numericUpDownُStaticFromYear.Value = numericUpDownُStaticToYear.Value = int.Parse(strYearNow);
            numericUpDownُStaticFromMonth.Value = numericUpDownُStaticToMonth.Value = int.Parse(strMonthNow);
            numericUpDownُStaticFromDay.Value = numericUpDownُStaticToDay.Value = int.Parse(strDayNow);
            #endregion

            #endregion

            btnStatisticCalculate.PerformClick();
        }


        private void btnStatisticCalculate_Click_1(object sender, EventArgs e)
        {
            #region استخراج تاریخ درخواستی و ولیدیشن آن
            string strFromMonth = numericUpDownُStaticFromMonth.Value.ToString();
            string strFromDay = numericUpDownُStaticFromDay.Value.ToString();

            strFromMonth = (strFromMonth.Count() < 2) ? "0" + strFromMonth : strFromMonth;
            strFromDay = (strFromDay.Count() < 2) ? "0" + strFromDay : strFromDay;

            lblStatisticFrom.Text = numericUpDownُStaticFromYear.Value.ToString() + "/" + strFromMonth + "/" + strFromDay;

            string strToMonth = numericUpDownُStaticToMonth.Value.ToString();
            string strToDay = numericUpDownُStaticToDay.Value.ToString();

            strToMonth = (strToMonth.Count() < 2) ? "0" + strToMonth : strToMonth;
            strToDay = (strToDay.Count() < 2) ? "0" + strToDay : strToDay;

            lblStatisticTo.Text = numericUpDownُStaticToYear.Value.ToString() + "/" + strToMonth + "/" + strToDay;

            int intFrom = int.Parse(numericUpDownُStaticFromYear.Value.ToString() + strFromMonth + strFromDay);
            int intTo = int.Parse(numericUpDownُStaticToYear.Value.ToString() + strToMonth + strToDay);

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
                WHERE     (registerDate >=N'" + lblStatisticFrom.Text + @"' AND registerDate <= N'" + lblStatisticTo.Text + @"')
                GROUP BY approver
                ";
            DataTable dt = objClsSql.FillDataTableFromQuery(strQuery);

            chartRows.DataSource = dt;
            chartRows.ChartAreas[0].AxisX.Interval = 1;
            chartRows.Series.Clear();

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                Series series = new Series();

                series.XValueMember = dt.Columns[0].ColumnName;
                series.YValueMembers = dt.Columns[i].ColumnName;

                series.ChartType = SeriesChartType.Pie;

                series.IsVisibleInLegend = true;
                series.IsValueShownAsLabel = true;
                series.BorderWidth = 3;


                chartRows.Series.Add(series);

                strQuery = @"
                             SELECT   COUNT(*) AS tedad
                    FROM         DT.dbo.tblDownTimeShort
                    WHERE     (registerDate >=N'" + lblStatisticFrom.Text + @"' AND registerDate <= N'" + lblStatisticTo.Text + @"')

                    ";
                dt = objClsSql.FillDataTableFromQuery(strQuery);

            }

            lblTotalDt.Text = dt.Rows[0].ItemArray[0].ToString();


            string strQuery2 = @"
                SELECT     approver, COUNT(Distinct dtNo) AS tedad
                FROM         DT.dbo.tblDownTimeShort
                WHERE     (registerDate >=N'" + lblStatisticFrom.Text + @"' AND registerDate <= N'" + lblStatisticTo.Text + @"' AND dtNo!=N'') 
                GROUP BY approver
                ";
            DataTable dt2 = objClsSql.FillDataTableFromQuery(strQuery2);

            chartCrs.DataSource = dt2;
            chartCrs.ChartAreas[0].AxisX.Interval = 1;
            chartCrs.Series.Clear();

            for (int j = 1; j < dt2.Columns.Count; j++)
            {
                Series series1 = new Series();

                series1.XValueMember = dt2.Columns[0].ColumnName;
                series1.YValueMembers = dt2.Columns[j].ColumnName;

                series1.ChartType = SeriesChartType.Pie;

                series1.IsVisibleInLegend = true;
                series1.IsValueShownAsLabel = true;
                series1.BorderWidth = 3;


                chartCrs.Series.Add(series1);

                strQuery = @"
                             SELECT  COUNT(Distinct dtNo) AS tedad
                    FROM         DT.dbo.tblDownTimeShort
                    WHERE     (registerDate >=N'" + lblStatisticFrom.Text + @"' AND registerDate <= N'" + lblStatisticTo.Text + @"')

                    ";
                dt2 = objClsSql.FillDataTableFromQuery(strQuery);
            }

            lblTotalCrs.Text = dt2.Rows[0].ItemArray[0].ToString();

        }

        
    }
}
