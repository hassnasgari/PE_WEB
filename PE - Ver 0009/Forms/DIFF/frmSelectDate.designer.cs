namespace PE.Forms
{
    partial class frmSelectDate
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
            this.gbxTchCalendar = new System.Windows.Forms.GroupBox();
            this.btnSetDateOk = new System.Windows.Forms.Button();
            this.farsiCalendar = new BehComponents.MonthCalendarX();
            this.btnNextDay = new System.Windows.Forms.Button();
            this.txtMiladiDate = new System.Windows.Forms.TextBox();
            this.txtShamsiDate = new System.Windows.Forms.TextBox();
            this.btnLastDay = new System.Windows.Forms.Button();
            this.gbxTchCalendar.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxTchCalendar
            // 
            this.gbxTchCalendar.Controls.Add(this.btnSetDateOk);
            this.gbxTchCalendar.Controls.Add(this.farsiCalendar);
            this.gbxTchCalendar.Controls.Add(this.btnNextDay);
            this.gbxTchCalendar.Controls.Add(this.txtMiladiDate);
            this.gbxTchCalendar.Controls.Add(this.txtShamsiDate);
            this.gbxTchCalendar.Controls.Add(this.btnLastDay);
            this.gbxTchCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxTchCalendar.Location = new System.Drawing.Point(0, 0);
            this.gbxTchCalendar.Name = "gbxTchCalendar";
            this.gbxTchCalendar.Size = new System.Drawing.Size(357, 342);
            this.gbxTchCalendar.TabIndex = 2;
            this.gbxTchCalendar.TabStop = false;
            this.gbxTchCalendar.Enter += new System.EventHandler(this.gbxTchCalendar_Enter);
            // 
            // btnSetDateOk
            // 
            this.btnSetDateOk.Location = new System.Drawing.Point(134, 309);
            this.btnSetDateOk.Name = "btnSetDateOk";
            this.btnSetDateOk.Size = new System.Drawing.Size(75, 23);
            this.btnSetDateOk.TabIndex = 2;
            this.btnSetDateOk.Text = "OK";
            this.btnSetDateOk.UseVisualStyleBackColor = true;
            this.btnSetDateOk.Click += new System.EventHandler(this.btnSetDateOk_Click);
            // 
            // farsiCalendar
            // 
            this.farsiCalendar.BackColor = System.Drawing.Color.Azure;
            this.farsiCalendar.BoldedDayForeColor = System.Drawing.Color.Blue;
            this.farsiCalendar.BorderColor = System.Drawing.Color.CadetBlue;
            this.farsiCalendar.CalendarType = BehComponents.CalendarTypes.Persian;
            this.farsiCalendar.DayRectTickness = 2F;
            this.farsiCalendar.DaysBackColor = System.Drawing.SystemColors.Menu;
            this.farsiCalendar.DaysFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.farsiCalendar.DaysForeColor = System.Drawing.Color.DodgerBlue;
            this.farsiCalendar.EnglishAnnuallyBoldedDates = new System.DateTime[0];
            this.farsiCalendar.EnglishBoldedDates = new System.DateTime[0];
            this.farsiCalendar.EnglishHolidayDates = new System.DateTime[0];
            this.farsiCalendar.EnglishMonthlyBoldedDates = new System.DateTime[0];
            this.farsiCalendar.HolidayForeColor = System.Drawing.Color.Red;
            this.farsiCalendar.HolidayWeekly = BehComponents.MonthCalendarX.DayOfWeekForHoliday.Friday;
            this.farsiCalendar.LineWeekColor = System.Drawing.Color.Black;
            this.farsiCalendar.Location = new System.Drawing.Point(6, 66);
            this.farsiCalendar.Name = "farsiCalendar";
            this.farsiCalendar.PersianAnnuallyBoldedDates = new BehComponents.PersianDateTime[0];
            this.farsiCalendar.PersianBoldedDates = new BehComponents.PersianDateTime[0];
            this.farsiCalendar.PersianHolidayDates = new BehComponents.PersianDateTime[0];
            this.farsiCalendar.PersianMonthlyBoldedDates = new BehComponents.PersianDateTime[0];
            this.farsiCalendar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.farsiCalendar.ShowToday = true;
            this.farsiCalendar.ShowTodayRect = true;
            this.farsiCalendar.ShowToolTips = true;
            this.farsiCalendar.ShowTrailing = true;
            this.farsiCalendar.Size = new System.Drawing.Size(345, 227);
            this.farsiCalendar.Style_DaysButton = BehComponents.ButtonX.ButtonStyles.Simple;
            this.farsiCalendar.Style_GotoTodayButton = BehComponents.ButtonX.ButtonStyles.Green;
            this.farsiCalendar.Style_MonthButton = BehComponents.ButtonX.ButtonStyles.Blue;
            this.farsiCalendar.Style_NextMonthButton = BehComponents.ButtonX.ButtonStyles.Green;
            this.farsiCalendar.Style_PreviousMonthButton = BehComponents.ButtonX.ButtonStyles.Green;
            this.farsiCalendar.Style_YearButton = BehComponents.ButtonX.ButtonStyles.Blue;
            this.farsiCalendar.TabIndex = 0;
            this.farsiCalendar.TitleBackColor = System.Drawing.Color.YellowGreen;
            this.farsiCalendar.TitleFont = new System.Drawing.Font("Tahoma", 8.25F);
            this.farsiCalendar.TitleForeColor = System.Drawing.Color.Black;
            this.farsiCalendar.TodayBackColor = System.Drawing.Color.Wheat;
            this.farsiCalendar.TodayFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.farsiCalendar.TodayForeColor = System.Drawing.Color.Black;
            this.farsiCalendar.TodayRectColor = System.Drawing.Color.Fuchsia;
            this.farsiCalendar.TodayRectTickness = 2F;
            this.farsiCalendar.TrailingForeColor = System.Drawing.Color.DarkGray;
            this.farsiCalendar.WeekDaysBackColor = System.Drawing.Color.Wheat;
            this.farsiCalendar.WeekDaysFont = new System.Drawing.Font("Tahoma", 8.25F);
            this.farsiCalendar.WeekDaysForeColor = System.Drawing.Color.OrangeRed;
            this.farsiCalendar.WeekStartsOn = BehComponents.MonthCalendarX.WeekDays.Saturday;
            this.farsiCalendar.SelectedDateChanged += new BehComponents.MonthCalendarX.OnSelectedDateChanged(this.farsiCalendar_SelectedDateChanged);
            this.farsiCalendar.Enter += new System.EventHandler(this.farsiCalendar_Enter);
            // 
            // btnNextDay
            // 
            this.btnNextDay.BackColor = System.Drawing.Color.LightYellow;
            this.btnNextDay.Location = new System.Drawing.Point(6, 14);
            this.btnNextDay.Name = "btnNextDay";
            this.btnNextDay.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNextDay.Size = new System.Drawing.Size(61, 46);
            this.btnNextDay.TabIndex = 1;
            this.btnNextDay.Text = "< روز بعد";
            this.btnNextDay.UseVisualStyleBackColor = false;
            this.btnNextDay.Click += new System.EventHandler(this.btnNextDay_Click);
            // 
            // txtMiladiDate
            // 
            this.txtMiladiDate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtMiladiDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtMiladiDate.Location = new System.Drawing.Point(73, 40);
            this.txtMiladiDate.Name = "txtMiladiDate";
            this.txtMiladiDate.ReadOnly = true;
            this.txtMiladiDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtMiladiDate.Size = new System.Drawing.Size(203, 23);
            this.txtMiladiDate.TabIndex = 9;
            this.txtMiladiDate.TabStop = false;
            this.txtMiladiDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtShamsiDate
            // 
            this.txtShamsiDate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtShamsiDate.ForeColor = System.Drawing.Color.Peru;
            this.txtShamsiDate.Location = new System.Drawing.Point(73, 14);
            this.txtShamsiDate.Name = "txtShamsiDate";
            this.txtShamsiDate.ReadOnly = true;
            this.txtShamsiDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtShamsiDate.Size = new System.Drawing.Size(203, 23);
            this.txtShamsiDate.TabIndex = 10;
            this.txtShamsiDate.TabStop = false;
            this.txtShamsiDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLastDay
            // 
            this.btnLastDay.BackColor = System.Drawing.Color.LightYellow;
            this.btnLastDay.Location = new System.Drawing.Point(282, 14);
            this.btnLastDay.Name = "btnLastDay";
            this.btnLastDay.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnLastDay.Size = new System.Drawing.Size(69, 48);
            this.btnLastDay.TabIndex = 8;
            this.btnLastDay.Text = " روز قبل >";
            this.btnLastDay.UseVisualStyleBackColor = false;
            this.btnLastDay.Click += new System.EventHandler(this.btnLastDay_Click);
            // 
            // frmSelectDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(357, 342);
            this.Controls.Add(this.gbxTchCalendar);
            this.Name = "frmSelectDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تقویم";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSelectDate_FormClosed);
            this.Load += new System.EventHandler(this.frmSelectDate_Load);
            this.Enter += new System.EventHandler(this.frmSelectDate_Enter);
            this.gbxTchCalendar.ResumeLayout(false);
            this.gbxTchCalendar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxTchCalendar;
        private BehComponents.MonthCalendarX farsiCalendar;
        private System.Windows.Forms.Button btnNextDay;
        private System.Windows.Forms.TextBox txtMiladiDate;
        private System.Windows.Forms.TextBox txtShamsiDate;
        private System.Windows.Forms.Button btnLastDay;
        private System.Windows.Forms.Button btnSetDateOk;
    }
}