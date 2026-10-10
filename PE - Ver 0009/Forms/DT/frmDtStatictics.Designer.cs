
namespace PE.Forms.DT
{
    partial class frmDtStatictics
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDtStatictics));
            this.vwDtStaticTodayBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblStatistics = new System.Windows.Forms.Label();
            this.numericUpDownُStaticFromMonth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownُStaticFromDay = new System.Windows.Forms.NumericUpDown();
            this.lblStaticFrom = new System.Windows.Forms.Label();
            this.lblStaticFromDay = new System.Windows.Forms.Label();
            this.lblStaticFromMonth = new System.Windows.Forms.Label();
            this.lblStaticToMonth = new System.Windows.Forms.Label();
            this.lblStaticToDay = new System.Windows.Forms.Label();
            this.lblStaticTo = new System.Windows.Forms.Label();
            this.numericUpDownُStaticToDay = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownُStaticToMonth = new System.Windows.Forms.NumericUpDown();
            this.lblStatisticFrom = new System.Windows.Forms.Label();
            this.lblStatisticTo = new System.Windows.Forms.Label();
            this.lblStaticToYear = new System.Windows.Forms.Label();
            this.numericUpDownُStaticToYear = new System.Windows.Forms.NumericUpDown();
            this.lblStaticFromYear = new System.Windows.Forms.Label();
            this.numericUpDownُStaticFromYear = new System.Windows.Forms.NumericUpDown();
            this.lblAz = new System.Windows.Forms.Label();
            this.lblTa = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTotalDt = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.chartRows = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnStatisticCalculate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chartCrs = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTotalCrs = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.vwDtStaticTodayBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticFromMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticFromDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticToDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticToMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticToYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticFromYear)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRows)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCrs)).BeginInit();
            this.SuspendLayout();
            // 
            // vwDtStaticTodayBindingSource
            // 
            this.vwDtStaticTodayBindingSource.DataMember = "vwDtStaticToday";
            // 
            // lblStatistics
            // 
            this.lblStatistics.AutoSize = true;
            this.lblStatistics.Location = new System.Drawing.Point(31, 22);
            this.lblStatistics.Name = "lblStatistics";
            this.lblStatistics.Size = new System.Drawing.Size(225, 13);
            this.lblStatistics.TabIndex = 1;
            this.lblStatistics.Text = "لطفا بازه زمانی جهت رسم نمودار را مشخص کنید";
            // 
            // numericUpDownُStaticFromMonth
            // 
            this.numericUpDownُStaticFromMonth.Location = new System.Drawing.Point(357, 55);
            this.numericUpDownُStaticFromMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDownُStaticFromMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownُStaticFromMonth.Name = "numericUpDownُStaticFromMonth";
            this.numericUpDownُStaticFromMonth.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownُStaticFromMonth.TabIndex = 2;
            this.numericUpDownُStaticFromMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownُStaticFromDay
            // 
            this.numericUpDownُStaticFromDay.Location = new System.Drawing.Point(197, 56);
            this.numericUpDownُStaticFromDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDownُStaticFromDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownُStaticFromDay.Name = "numericUpDownُStaticFromDay";
            this.numericUpDownُStaticFromDay.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownُStaticFromDay.TabIndex = 3;
            this.numericUpDownُStaticFromDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblStaticFrom
            // 
            this.lblStaticFrom.AutoSize = true;
            this.lblStaticFrom.BackColor = System.Drawing.Color.PaleGreen;
            this.lblStaticFrom.Location = new System.Drawing.Point(31, 58);
            this.lblStaticFrom.Name = "lblStaticFrom";
            this.lblStaticFrom.Size = new System.Drawing.Size(43, 13);
            this.lblStaticFrom.TabIndex = 4;
            this.lblStaticFrom.Text = "از تاریخ";
            // 
            // lblStaticFromDay
            // 
            this.lblStaticFromDay.AutoSize = true;
            this.lblStaticFromDay.Location = new System.Drawing.Point(99, 57);
            this.lblStaticFromDay.Name = "lblStaticFromDay";
            this.lblStaticFromDay.Size = new System.Drawing.Size(91, 13);
            this.lblStaticFromDay.TabIndex = 5;
            this.lblStaticFromDay.Text = "روز شروع گزارش :";
            // 
            // lblStaticFromMonth
            // 
            this.lblStaticFromMonth.AutoSize = true;
            this.lblStaticFromMonth.Location = new System.Drawing.Point(264, 56);
            this.lblStaticFromMonth.Name = "lblStaticFromMonth";
            this.lblStaticFromMonth.Size = new System.Drawing.Size(90, 13);
            this.lblStaticFromMonth.TabIndex = 6;
            this.lblStaticFromMonth.Text = "ماه شروع گزارش :";
            // 
            // lblStaticToMonth
            // 
            this.lblStaticToMonth.AutoSize = true;
            this.lblStaticToMonth.Location = new System.Drawing.Point(264, 94);
            this.lblStaticToMonth.Name = "lblStaticToMonth";
            this.lblStaticToMonth.Size = new System.Drawing.Size(86, 13);
            this.lblStaticToMonth.TabIndex = 11;
            this.lblStaticToMonth.Text = "ماه پایان گزارش :";
            // 
            // lblStaticToDay
            // 
            this.lblStaticToDay.AutoSize = true;
            this.lblStaticToDay.Location = new System.Drawing.Point(110, 94);
            this.lblStaticToDay.Name = "lblStaticToDay";
            this.lblStaticToDay.Size = new System.Drawing.Size(87, 13);
            this.lblStaticToDay.TabIndex = 10;
            this.lblStaticToDay.Text = "روز پایان گزارش :";
            // 
            // lblStaticTo
            // 
            this.lblStaticTo.AutoSize = true;
            this.lblStaticTo.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.lblStaticTo.Location = new System.Drawing.Point(31, 96);
            this.lblStaticTo.Name = "lblStaticTo";
            this.lblStaticTo.Size = new System.Drawing.Size(43, 13);
            this.lblStaticTo.TabIndex = 9;
            this.lblStaticTo.Text = "تا تاریخ";
            // 
            // numericUpDownُStaticToDay
            // 
            this.numericUpDownُStaticToDay.Location = new System.Drawing.Point(201, 92);
            this.numericUpDownُStaticToDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDownُStaticToDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownُStaticToDay.Name = "numericUpDownُStaticToDay";
            this.numericUpDownُStaticToDay.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownُStaticToDay.TabIndex = 8;
            this.numericUpDownُStaticToDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownُStaticToMonth
            // 
            this.numericUpDownُStaticToMonth.Location = new System.Drawing.Point(356, 91);
            this.numericUpDownُStaticToMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDownُStaticToMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownُStaticToMonth.Name = "numericUpDownُStaticToMonth";
            this.numericUpDownُStaticToMonth.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownُStaticToMonth.TabIndex = 7;
            this.numericUpDownُStaticToMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblStatisticFrom
            // 
            this.lblStatisticFrom.AutoSize = true;
            this.lblStatisticFrom.Location = new System.Drawing.Point(697, 62);
            this.lblStatisticFrom.Name = "lblStatisticFrom";
            this.lblStatisticFrom.Size = new System.Drawing.Size(19, 13);
            this.lblStatisticFrom.TabIndex = 12;
            this.lblStatisticFrom.Text = "----";
            // 
            // lblStatisticTo
            // 
            this.lblStatisticTo.AutoSize = true;
            this.lblStatisticTo.Location = new System.Drawing.Point(697, 100);
            this.lblStatisticTo.Name = "lblStatisticTo";
            this.lblStatisticTo.Size = new System.Drawing.Size(19, 13);
            this.lblStatisticTo.TabIndex = 13;
            this.lblStatisticTo.Text = "----";
            // 
            // lblStaticToYear
            // 
            this.lblStaticToYear.AutoSize = true;
            this.lblStaticToYear.Location = new System.Drawing.Point(485, 96);
            this.lblStaticToYear.Name = "lblStaticToYear";
            this.lblStaticToYear.Size = new System.Drawing.Size(91, 13);
            this.lblStaticToYear.TabIndex = 17;
            this.lblStaticToYear.Text = "سال پایان گزارش :";
            // 
            // numericUpDownُStaticToYear
            // 
            this.numericUpDownُStaticToYear.Location = new System.Drawing.Point(582, 93);
            this.numericUpDownُStaticToYear.Maximum = new decimal(new int[] {
            1405,
            0,
            0,
            0});
            this.numericUpDownُStaticToYear.Minimum = new decimal(new int[] {
            1395,
            0,
            0,
            0});
            this.numericUpDownُStaticToYear.Name = "numericUpDownُStaticToYear";
            this.numericUpDownُStaticToYear.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownُStaticToYear.TabIndex = 16;
            this.numericUpDownُStaticToYear.Value = new decimal(new int[] {
            1401,
            0,
            0,
            0});
            // 
            // lblStaticFromYear
            // 
            this.lblStaticFromYear.AutoSize = true;
            this.lblStaticFromYear.Location = new System.Drawing.Point(485, 58);
            this.lblStaticFromYear.Name = "lblStaticFromYear";
            this.lblStaticFromYear.Size = new System.Drawing.Size(95, 13);
            this.lblStaticFromYear.TabIndex = 15;
            this.lblStaticFromYear.Text = "سال شروع گزارش :";
            // 
            // numericUpDownُStaticFromYear
            // 
            this.numericUpDownُStaticFromYear.Location = new System.Drawing.Point(583, 57);
            this.numericUpDownُStaticFromYear.Maximum = new decimal(new int[] {
            1405,
            0,
            0,
            0});
            this.numericUpDownُStaticFromYear.Minimum = new decimal(new int[] {
            1395,
            0,
            0,
            0});
            this.numericUpDownُStaticFromYear.Name = "numericUpDownُStaticFromYear";
            this.numericUpDownُStaticFromYear.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownُStaticFromYear.TabIndex = 14;
            this.numericUpDownُStaticFromYear.Value = new decimal(new int[] {
            1401,
            0,
            0,
            0});
            // 
            // lblAz
            // 
            this.lblAz.AutoSize = true;
            this.lblAz.Location = new System.Drawing.Point(660, 62);
            this.lblAz.Name = "lblAz";
            this.lblAz.Size = new System.Drawing.Size(21, 13);
            this.lblAz.TabIndex = 19;
            this.lblAz.Text = "از :";
            // 
            // lblTa
            // 
            this.lblTa.AutoSize = true;
            this.lblTa.Location = new System.Drawing.Point(660, 100);
            this.lblTa.Name = "lblTa";
            this.lblTa.Size = new System.Drawing.Size(21, 13);
            this.lblTa.TabIndex = 20;
            this.lblTa.Text = "تا :";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblTotalDt);
            this.groupBox1.Controls.Add(this.lblTotal);
            this.groupBox1.Controls.Add(this.chartRows);
            this.groupBox1.Location = new System.Drawing.Point(626, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(638, 411);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "آمار تعداد ردیف ثبت شده";
            // 
            // lblTotalDt
            // 
            this.lblTotalDt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalDt.AutoSize = true;
            this.lblTotalDt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblTotalDt.Location = new System.Drawing.Point(431, 32);
            this.lblTotalDt.Name = "lblTotalDt";
            this.lblTotalDt.Size = new System.Drawing.Size(25, 13);
            this.lblTotalDt.TabIndex = 26;
            this.lblTotalDt.Text = "------";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(477, 32);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(140, 13);
            this.lblTotal.TabIndex = 25;
            this.lblTotal.Text = "  تعداد کل  ردیف های ثبتی : ";
            // 
            // chartRows
            // 
            this.chartRows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartRows.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRows.Legends.Add(legend1);
            this.chartRows.Location = new System.Drawing.Point(48, 65);
            this.chartRows.Name = "chartRows";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartRows.Series.Add(series1);
            this.chartRows.Size = new System.Drawing.Size(566, 316);
            this.chartRows.TabIndex = 23;
            this.chartRows.Text = "chart1";
            // 
            // btnStatisticCalculate
            // 
            this.btnStatisticCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStatisticCalculate.BackColor = System.Drawing.Color.Lavender;
            this.btnStatisticCalculate.Location = new System.Drawing.Point(1079, 71);
            this.btnStatisticCalculate.Name = "btnStatisticCalculate";
            this.btnStatisticCalculate.Size = new System.Drawing.Size(178, 40);
            this.btnStatisticCalculate.TabIndex = 24;
            this.btnStatisticCalculate.Text = "آمارگیری";
            this.btnStatisticCalculate.UseVisualStyleBackColor = false;
            this.btnStatisticCalculate.Click += new System.EventHandler(this.btnStatisticCalculate_Click_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.chartCrs);
            this.groupBox2.Controls.Add(this.lblTotalCrs);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(16, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(597, 411);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "آمار تعداد سی آر ثبت شده";
            // 
            // chartCrs
            // 
            this.chartCrs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chartCrs.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartCrs.Legends.Add(legend2);
            this.chartCrs.Location = new System.Drawing.Point(17, 81);
            this.chartCrs.Name = "chartCrs";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartCrs.Series.Add(series2);
            this.chartCrs.Size = new System.Drawing.Size(563, 282);
            this.chartCrs.TabIndex = 29;
            this.chartCrs.Text = "chart1";
            // 
            // lblTotalCrs
            // 
            this.lblTotalCrs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalCrs.AutoSize = true;
            this.lblTotalCrs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblTotalCrs.Location = new System.Drawing.Point(353, 46);
            this.lblTotalCrs.Name = "lblTotalCrs";
            this.lblTotalCrs.Size = new System.Drawing.Size(25, 13);
            this.lblTotalCrs.TabIndex = 28;
            this.lblTotalCrs.Text = "------";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(420, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "   تعداد کل سی آر های ثبت شده :";
            // 
            // frmDtStatictics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1269, 559);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStatisticCalculate);
            this.Controls.Add(this.lblTa);
            this.Controls.Add(this.lblAz);
            this.Controls.Add(this.lblStaticToYear);
            this.Controls.Add(this.numericUpDownُStaticToYear);
            this.Controls.Add(this.lblStaticFromYear);
            this.Controls.Add(this.numericUpDownُStaticFromYear);
            this.Controls.Add(this.lblStatisticTo);
            this.Controls.Add(this.lblStatisticFrom);
            this.Controls.Add(this.lblStaticToMonth);
            this.Controls.Add(this.lblStaticToDay);
            this.Controls.Add(this.lblStaticTo);
            this.Controls.Add(this.numericUpDownُStaticToDay);
            this.Controls.Add(this.numericUpDownُStaticToMonth);
            this.Controls.Add(this.lblStaticFromMonth);
            this.Controls.Add(this.lblStaticFromDay);
            this.Controls.Add(this.lblStaticFrom);
            this.Controls.Add(this.numericUpDownُStaticFromDay);
            this.Controls.Add(this.numericUpDownُStaticFromMonth);
            this.Controls.Add(this.lblStatistics);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDtStatictics";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "اطلاعات آماری دان تایمهای گرفته شده";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmDtStatictics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vwDtStaticTodayBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticFromMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticFromDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticToDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticToMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticToYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownُStaticFromYear)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRows)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCrs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
       
        private System.Windows.Forms.BindingSource vwDtStaticTodayBindingSource;
        private System.Windows.Forms.Label lblStatistics;
        private System.Windows.Forms.NumericUpDown numericUpDownُStaticFromMonth;
        private System.Windows.Forms.NumericUpDown numericUpDownُStaticFromDay;
        private System.Windows.Forms.Label lblStaticFrom;
        private System.Windows.Forms.Label lblStaticFromDay;
        private System.Windows.Forms.Label lblStaticFromMonth;
        private System.Windows.Forms.Label lblStaticToMonth;
        private System.Windows.Forms.Label lblStaticToDay;
        private System.Windows.Forms.Label lblStaticTo;
        private System.Windows.Forms.NumericUpDown numericUpDownُStaticToDay;
        private System.Windows.Forms.NumericUpDown numericUpDownُStaticToMonth;
        private System.Windows.Forms.Label lblStatisticFrom;
        private System.Windows.Forms.Label lblStatisticTo;
        private System.Windows.Forms.Label lblStaticToYear;
        private System.Windows.Forms.NumericUpDown numericUpDownُStaticToYear;
        private System.Windows.Forms.Label lblStaticFromYear;
        private System.Windows.Forms.NumericUpDown numericUpDownُStaticFromYear;
        private System.Windows.Forms.Label lblAz;
        private System.Windows.Forms.Label lblTa;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTotalDt;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRows;
        private System.Windows.Forms.Button btnStatisticCalculate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTotalCrs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCrs;
    }
}