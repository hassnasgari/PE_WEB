namespace PE.Forms
{
    partial class Logs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Logs));
            this.panelLogTypeSelect = new System.Windows.Forms.Panel();
            this.radioButtonByNumbers = new System.Windows.Forms.RadioButton();
            this.radioButtonByDate = new System.Windows.Forms.RadioButton();
            this.gridViewLogAction = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControlLogAction = new DevExpress.XtraGrid.GridControl();
            this.panelLogByNumber = new System.Windows.Forms.Panel();
            this.lblLog = new System.Windows.Forms.Label();
            this.comboBoxLog = new System.Windows.Forms.ComboBox();
            this.panelLogByDate = new System.Windows.Forms.Panel();
            this.btnLogDateCreate = new System.Windows.Forms.Button();
            this.hM_FarsiCalendarLogTo = new HM_FarsiCalendar.HM_FarsiCalendar();
            this.lblLogTo = new System.Windows.Forms.Label();
            this.lblLog2 = new System.Windows.Forms.Label();
            this.hM_FarsiCalendarLogFrom = new HM_FarsiCalendar.HM_FarsiCalendar();
            this.lblLogFrom = new System.Windows.Forms.Label();
            this.panelLogTypeSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewLogAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLogAction)).BeginInit();
            this.panelLogByNumber.SuspendLayout();
            this.panelLogByDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLogTypeSelect
            // 
            this.panelLogTypeSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLogTypeSelect.Controls.Add(this.radioButtonByNumbers);
            this.panelLogTypeSelect.Controls.Add(this.radioButtonByDate);
            this.panelLogTypeSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogTypeSelect.Location = new System.Drawing.Point(0, 0);
            this.panelLogTypeSelect.Name = "panelLogTypeSelect";
            this.panelLogTypeSelect.Size = new System.Drawing.Size(1169, 30);
            this.panelLogTypeSelect.TabIndex = 4;
            // 
            // radioButtonByNumbers
            // 
            this.radioButtonByNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonByNumbers.AutoSize = true;
            this.radioButtonByNumbers.Checked = true;
            this.radioButtonByNumbers.Location = new System.Drawing.Point(966, 6);
            this.radioButtonByNumbers.Name = "radioButtonByNumbers";
            this.radioButtonByNumbers.Size = new System.Drawing.Size(198, 17);
            this.radioButtonByNumbers.TabIndex = 6;
            this.radioButtonByNumbers.TabStop = true;
            this.radioButtonByNumbers.Text = "گزارش بر اساس تعدادی از آخرین موارد";
            this.radioButtonByNumbers.UseVisualStyleBackColor = true;
            this.radioButtonByNumbers.CheckedChanged += new System.EventHandler(this.radioButtonByNumbers_CheckedChanged);
            // 
            // radioButtonByDate
            // 
            this.radioButtonByDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonByDate.AutoSize = true;
            this.radioButtonByDate.Location = new System.Drawing.Point(773, 6);
            this.radioButtonByDate.Name = "radioButtonByDate";
            this.radioButtonByDate.Size = new System.Drawing.Size(147, 17);
            this.radioButtonByDate.TabIndex = 2;
            this.radioButtonByDate.Text = "گزارش بر اساس بازه زمانی";
            this.radioButtonByDate.UseVisualStyleBackColor = true;
            // 
            // gridViewLogAction
            // 
            this.gridViewLogAction.Appearance.GroupButton.Options.UseTextOptions = true;
            this.gridViewLogAction.Appearance.GroupButton.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewLogAction.Appearance.GroupFooter.Options.UseTextOptions = true;
            this.gridViewLogAction.Appearance.GroupFooter.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewLogAction.Appearance.GroupPanel.Options.UseTextOptions = true;
            this.gridViewLogAction.Appearance.GroupPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewLogAction.Appearance.GroupRow.Options.UseTextOptions = true;
            this.gridViewLogAction.Appearance.GroupRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewLogAction.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewLogAction.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewLogAction.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewLogAction.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewLogAction.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewLogAction.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewLogAction.AppearancePrint.FilterPanel.Options.UseTextOptions = true;
            this.gridViewLogAction.AppearancePrint.FilterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewLogAction.AppearancePrint.FilterPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewLogAction.GridControl = this.gridControlLogAction;
            this.gridViewLogAction.Name = "gridViewLogAction";
            this.gridViewLogAction.OptionsView.ShowAutoFilterRow = true;
            this.gridViewLogAction.OptionsView.ShowFooter = true;
            this.gridViewLogAction.OptionsView.ShowGroupedColumns = true;
            // 
            // gridControlLogAction
            // 
            this.gridControlLogAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlLogAction.Location = new System.Drawing.Point(0, 100);
            this.gridControlLogAction.MainView = this.gridViewLogAction;
            this.gridControlLogAction.Name = "gridControlLogAction";
            this.gridControlLogAction.Size = new System.Drawing.Size(1169, 665);
            this.gridControlLogAction.TabIndex = 1;
            this.gridControlLogAction.UseEmbeddedNavigator = true;
            this.gridControlLogAction.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewLogAction});
            // 
            // panelLogByNumber
            // 
            this.panelLogByNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLogByNumber.Controls.Add(this.lblLog);
            this.panelLogByNumber.Controls.Add(this.comboBoxLog);
            this.panelLogByNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogByNumber.Location = new System.Drawing.Point(0, 30);
            this.panelLogByNumber.Name = "panelLogByNumber";
            this.panelLogByNumber.Size = new System.Drawing.Size(1169, 39);
            this.panelLogByNumber.TabIndex = 5;
            // 
            // lblLog
            // 
            this.lblLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(1029, 14);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(126, 13);
            this.lblLog.TabIndex = 1;
            this.lblLog.Text = "نمایش تعداد گزارش آخری :";
            // 
            // comboBoxLog
            // 
            this.comboBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLog.FormattingEnabled = true;
            this.comboBoxLog.Items.AddRange(new object[] {
            "50",
            "100",
            "300",
            "500",
            "1000",
            "5000",
            "همه"});
            this.comboBoxLog.Location = new System.Drawing.Point(902, 9);
            this.comboBoxLog.Name = "comboBoxLog";
            this.comboBoxLog.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLog.TabIndex = 0;
            this.comboBoxLog.SelectedIndexChanged += new System.EventHandler(this.comboBoxLog_SelectedIndexChanged);
            // 
            // panelLogByDate
            // 
            this.panelLogByDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLogByDate.Controls.Add(this.btnLogDateCreate);
            this.panelLogByDate.Controls.Add(this.hM_FarsiCalendarLogTo);
            this.panelLogByDate.Controls.Add(this.lblLogTo);
            this.panelLogByDate.Controls.Add(this.lblLog2);
            this.panelLogByDate.Controls.Add(this.hM_FarsiCalendarLogFrom);
            this.panelLogByDate.Controls.Add(this.lblLogFrom);
            this.panelLogByDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogByDate.Enabled = false;
            this.panelLogByDate.Location = new System.Drawing.Point(0, 69);
            this.panelLogByDate.Name = "panelLogByDate";
            this.panelLogByDate.Size = new System.Drawing.Size(1169, 28);
            this.panelLogByDate.TabIndex = 6;
            // 
            // btnLogDateCreate
            // 
            this.btnLogDateCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogDateCreate.Location = new System.Drawing.Point(563, 1);
            this.btnLogDateCreate.Name = "btnLogDateCreate";
            this.btnLogDateCreate.Size = new System.Drawing.Size(93, 23);
            this.btnLogDateCreate.TabIndex = 6;
            this.btnLogDateCreate.Text = "ایجاد گزارش";
            this.btnLogDateCreate.UseVisualStyleBackColor = true;
            this.btnLogDateCreate.Click += new System.EventHandler(this.btnLogDateCreate_Click);
            // 
            // hM_FarsiCalendarLogTo
            // 
            this.hM_FarsiCalendarLogTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hM_FarsiCalendarLogTo.BodyBackColor = System.Drawing.Color.White;
            this.hM_FarsiCalendarLogTo.BodyForeColor = System.Drawing.Color.Black;
            this.hM_FarsiCalendarLogTo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.hM_FarsiCalendarLogTo.HeaderBackColor = System.Drawing.Color.Gray;
            this.hM_FarsiCalendarLogTo.HeaderFont = new System.Drawing.Font("Tahoma", 8.25F);
            this.hM_FarsiCalendarLogTo.HeaderForeColor = System.Drawing.Color.White;
            this.hM_FarsiCalendarLogTo.Location = new System.Drawing.Point(678, 3);
            this.hM_FarsiCalendarLogTo.Max_Date = "1400/12/30";
            this.hM_FarsiCalendarLogTo.Min_Date = "1350/01/01";
            this.hM_FarsiCalendarLogTo.Name = "hM_FarsiCalendarLogTo";
            this.hM_FarsiCalendarLogTo.SelectedBackColor = System.Drawing.Color.Blue;
            this.hM_FarsiCalendarLogTo.SelectedForeColor = System.Drawing.Color.White;
            this.hM_FarsiCalendarLogTo.Size = new System.Drawing.Size(125, 21);
            this.hM_FarsiCalendarLogTo.TabIndex = 5;
            this.hM_FarsiCalendarLogTo.TitleFont = new System.Drawing.Font("Tahoma", 8.25F);
            // 
            // lblLogTo
            // 
            this.lblLogTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogTo.AutoSize = true;
            this.lblLogTo.Location = new System.Drawing.Point(823, 7);
            this.lblLogTo.Name = "lblLogTo";
            this.lblLogTo.Size = new System.Drawing.Size(15, 13);
            this.lblLogTo.TabIndex = 4;
            this.lblLogTo.Text = "تا";
            // 
            // lblLog2
            // 
            this.lblLog2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLog2.AutoSize = true;
            this.lblLog2.Location = new System.Drawing.Point(1010, 7);
            this.lblLog2.Name = "lblLog2";
            this.lblLog2.Size = new System.Drawing.Size(156, 13);
            this.lblLog2.TabIndex = 2;
            this.lblLog2.Text = "   محدوده تاریخ را مشخص کنید : ";
            // 
            // hM_FarsiCalendarLogFrom
            // 
            this.hM_FarsiCalendarLogFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hM_FarsiCalendarLogFrom.BodyBackColor = System.Drawing.Color.White;
            this.hM_FarsiCalendarLogFrom.BodyForeColor = System.Drawing.Color.Black;
            this.hM_FarsiCalendarLogFrom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.hM_FarsiCalendarLogFrom.HeaderBackColor = System.Drawing.Color.Gray;
            this.hM_FarsiCalendarLogFrom.HeaderFont = new System.Drawing.Font("Tahoma", 8.25F);
            this.hM_FarsiCalendarLogFrom.HeaderForeColor = System.Drawing.Color.White;
            this.hM_FarsiCalendarLogFrom.Location = new System.Drawing.Point(858, 3);
            this.hM_FarsiCalendarLogFrom.Max_Date = "1400/12/30";
            this.hM_FarsiCalendarLogFrom.Min_Date = "1350/01/01";
            this.hM_FarsiCalendarLogFrom.Name = "hM_FarsiCalendarLogFrom";
            this.hM_FarsiCalendarLogFrom.SelectedBackColor = System.Drawing.Color.Blue;
            this.hM_FarsiCalendarLogFrom.SelectedForeColor = System.Drawing.Color.White;
            this.hM_FarsiCalendarLogFrom.Size = new System.Drawing.Size(125, 21);
            this.hM_FarsiCalendarLogFrom.TabIndex = 3;
            this.hM_FarsiCalendarLogFrom.TitleFont = new System.Drawing.Font("Tahoma", 8.25F);
            // 
            // lblLogFrom
            // 
            this.lblLogFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogFrom.AutoSize = true;
            this.lblLogFrom.Location = new System.Drawing.Point(989, 7);
            this.lblLogFrom.Name = "lblLogFrom";
            this.lblLogFrom.Size = new System.Drawing.Size(15, 13);
            this.lblLogFrom.TabIndex = 2;
            this.lblLogFrom.Text = "از";
            // 
            // Logs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 766);
            this.Controls.Add(this.panelLogByDate);
            this.Controls.Add(this.panelLogByNumber);
            this.Controls.Add(this.gridControlLogAction);
            this.Controls.Add(this.panelLogTypeSelect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Logs";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "لاگ عملکرد سامانه و کاربران";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Logs_Load);
            this.panelLogTypeSelect.ResumeLayout(false);
            this.panelLogTypeSelect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewLogAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLogAction)).EndInit();
            this.panelLogByNumber.ResumeLayout(false);
            this.panelLogByNumber.PerformLayout();
            this.panelLogByDate.ResumeLayout(false);
            this.panelLogByDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelLogTypeSelect;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewLogAction;
        private DevExpress.XtraGrid.GridControl gridControlLogAction;
        private System.Windows.Forms.Panel panelLogByNumber;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.ComboBox comboBoxLog;
        private System.Windows.Forms.Panel panelLogByDate;
        private HM_FarsiCalendar.HM_FarsiCalendar hM_FarsiCalendarLogTo;
        private System.Windows.Forms.Label lblLogTo;
        private System.Windows.Forms.Label lblLog2;
        private HM_FarsiCalendar.HM_FarsiCalendar hM_FarsiCalendarLogFrom;
        private System.Windows.Forms.Label lblLogFrom;
        private System.Windows.Forms.RadioButton radioButtonByNumbers;
        private System.Windows.Forms.RadioButton radioButtonByDate;
        private System.Windows.Forms.Button btnLogDateCreate;
    }
}