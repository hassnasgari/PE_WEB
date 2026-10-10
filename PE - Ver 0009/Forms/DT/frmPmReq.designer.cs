namespace PE.Forms.DT
{
    partial class frmPmReq
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
            this.groupBoxTotal = new System.Windows.Forms.GroupBox();
            this.btnPmFlowChart = new System.Windows.Forms.Button();
            this.gbxregistered = new System.Windows.Forms.GroupBox();
            this.gridControlDtRegiterDtByThisUser = new DevExpress.XtraGrid.GridControl();
            this.gridViewDtRegiterDtByThisUser = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnRegPmDt = new DevExpress.XtraEditors.SimpleButton();
            this.gbxDtPerformer = new System.Windows.Forms.GroupBox();
            this.txtSelectedPerformer = new System.Windows.Forms.TextBox();
            this.gcTel = new DevExpress.XtraGrid.GridControl();
            this.gvTel = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.gbxDtSelectTime = new System.Windows.Forms.GroupBox();
            this.nudDtEndHour = new System.Windows.Forms.NumericUpDown();
            this.lblDtEndTime = new System.Windows.Forms.Label();
            this.nudDtEndMin = new System.Windows.Forms.NumericUpDown();
            this.lblDb15 = new System.Windows.Forms.Label();
            this.nudDtStHour = new System.Windows.Forms.NumericUpDown();
            this.lblDb14 = new System.Windows.Forms.Label();
            this.nudDtStMin = new System.Windows.Forms.NumericUpDown();
            this.lblDtStTime = new System.Windows.Forms.Label();
            this.gbxDtSelectDate = new System.Windows.Forms.GroupBox();
            this.txtDtFromDate = new System.Windows.Forms.TextBox();
            this.btnDtFromDate = new System.Windows.Forms.Button();
            this.gbxDtAllSubject = new System.Windows.Forms.GroupBox();
            this.rdbPmSte = new System.Windows.Forms.RadioButton();
            this.rdbPmTwr = new System.Windows.Forms.RadioButton();
            this.rdbPmCln = new System.Windows.Forms.RadioButton();
            this.gbxDtRegCode = new System.Windows.Forms.GroupBox();
            this.lblReqDtRegion = new System.Windows.Forms.Label();
            this.lblDtRegion = new System.Windows.Forms.Label();
            this.txtDtRegSiteName = new System.Windows.Forms.TextBox();
            this.lblDtSiteName = new System.Windows.Forms.Label();
            this.lblDtStDate = new System.Windows.Forms.Label();
            this.txtBoxDtRegCodeSite = new System.Windows.Forms.TextBox();
            this.groupBoxTotal.SuspendLayout();
            this.gbxregistered.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDtRegiterDtByThisUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDtRegiterDtByThisUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.gbxDtPerformer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            this.gbxDtSelectTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtEndHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtEndMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtStHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtStMin)).BeginInit();
            this.gbxDtSelectDate.SuspendLayout();
            this.gbxDtAllSubject.SuspendLayout();
            this.gbxDtRegCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTotal
            // 
            this.groupBoxTotal.Controls.Add(this.btnPmFlowChart);
            this.groupBoxTotal.Controls.Add(this.gbxregistered);
            this.groupBoxTotal.Controls.Add(this.btnRegPmDt);
            this.groupBoxTotal.Controls.Add(this.gbxDtPerformer);
            this.groupBoxTotal.Controls.Add(this.gbxDtSelectTime);
            this.groupBoxTotal.Controls.Add(this.gbxDtSelectDate);
            this.groupBoxTotal.Controls.Add(this.gbxDtAllSubject);
            this.groupBoxTotal.Controls.Add(this.gbxDtRegCode);
            this.groupBoxTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTotal.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTotal.Name = "groupBoxTotal";
            this.groupBoxTotal.Size = new System.Drawing.Size(793, 790);
            this.groupBoxTotal.TabIndex = 0;
            this.groupBoxTotal.TabStop = false;
            // 
            // btnPmFlowChart
            // 
            this.btnPmFlowChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPmFlowChart.Location = new System.Drawing.Point(12, 216);
            this.btnPmFlowChart.Name = "btnPmFlowChart";
            this.btnPmFlowChart.Size = new System.Drawing.Size(211, 291);
            this.btnPmFlowChart.TabIndex = 601;
            this.btnPmFlowChart.Text = "بررسی بر اساس فلوچارت";
            this.btnPmFlowChart.UseVisualStyleBackColor = true;
            this.btnPmFlowChart.Click += new System.EventHandler(this.btnPmFlowChart_Click);
            // 
            // gbxregistered
            // 
            this.gbxregistered.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxregistered.Controls.Add(this.gridControlDtRegiterDtByThisUser);
            this.gbxregistered.Location = new System.Drawing.Point(6, 561);
            this.gbxregistered.Name = "gbxregistered";
            this.gbxregistered.Size = new System.Drawing.Size(781, 217);
            this.gbxregistered.TabIndex = 9;
            this.gbxregistered.TabStop = false;
            // 
            // gridControlDtRegiterDtByThisUser
            // 
            this.gridControlDtRegiterDtByThisUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlDtRegiterDtByThisUser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.gridControlDtRegiterDtByThisUser.Location = new System.Drawing.Point(3, 17);
            this.gridControlDtRegiterDtByThisUser.MainView = this.gridViewDtRegiterDtByThisUser;
            this.gridControlDtRegiterDtByThisUser.Name = "gridControlDtRegiterDtByThisUser";
            this.gridControlDtRegiterDtByThisUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridControlDtRegiterDtByThisUser.Size = new System.Drawing.Size(775, 197);
            this.gridControlDtRegiterDtByThisUser.TabIndex = 3;
            this.gridControlDtRegiterDtByThisUser.TabStop = false;
            this.gridControlDtRegiterDtByThisUser.UseEmbeddedNavigator = true;
            this.gridControlDtRegiterDtByThisUser.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDtRegiterDtByThisUser,
            this.gridView1});
            // 
            // gridViewDtRegiterDtByThisUser
            // 
            this.gridViewDtRegiterDtByThisUser.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewDtRegiterDtByThisUser.Appearance.GroupPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.gridViewDtRegiterDtByThisUser.Appearance.GroupPanel.Options.UseFont = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.GroupPanel.Options.UseTextOptions = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.GroupPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewDtRegiterDtByThisUser.Appearance.GroupRow.Options.UseTextOptions = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.GroupRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtRegiterDtByThisUser.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewDtRegiterDtByThisUser.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Purple;
            this.gridViewDtRegiterDtByThisUser.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtRegiterDtByThisUser.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtRegiterDtByThisUser.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewDtRegiterDtByThisUser.Appearance.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridViewDtRegiterDtByThisUser.Appearance.ViewCaption.BackColor = System.Drawing.Color.AliceBlue;
            this.gridViewDtRegiterDtByThisUser.Appearance.ViewCaption.Options.UseBackColor = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.gridViewDtRegiterDtByThisUser.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtRegiterDtByThisUser.GridControl = this.gridControlDtRegiterDtByThisUser;
            this.gridViewDtRegiterDtByThisUser.Name = "gridViewDtRegiterDtByThisUser";
            this.gridViewDtRegiterDtByThisUser.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewDtRegiterDtByThisUser.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewDtRegiterDtByThisUser.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewDtRegiterDtByThisUser.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewDtRegiterDtByThisUser.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewDtRegiterDtByThisUser.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewDtRegiterDtByThisUser.OptionsView.ShowGroupPanel = false;
            this.gridViewDtRegiterDtByThisUser.OptionsView.ShowIndicator = false;
            this.gridViewDtRegiterDtByThisUser.OptionsView.ShowViewCaption = true;
            this.gridViewDtRegiterDtByThisUser.PaintStyleName = "Office2003";
            this.gridViewDtRegiterDtByThisUser.ViewCaption = "دان تایمهای درخواست شده";
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControlDtRegiterDtByThisUser;
            this.gridView1.Name = "gridView1";
            // 
            // btnRegPmDt
            // 
            this.btnRegPmDt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegPmDt.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnRegPmDt.Appearance.Options.UseFont = true;
            this.btnRegPmDt.Location = new System.Drawing.Point(598, 518);
            this.btnRegPmDt.Name = "btnRegPmDt";
            this.btnRegPmDt.Size = new System.Drawing.Size(192, 37);
            this.btnRegPmDt.TabIndex = 8;
            this.btnRegPmDt.Text = "ثبت درخواست دان تایم";
            this.btnRegPmDt.Click += new System.EventHandler(this.btnRegPmDt_Click);
            // 
            // gbxDtPerformer
            // 
            this.gbxDtPerformer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDtPerformer.Controls.Add(this.txtSelectedPerformer);
            this.gbxDtPerformer.Controls.Add(this.gcTel);
            this.gbxDtPerformer.Location = new System.Drawing.Point(271, 252);
            this.gbxDtPerformer.Name = "gbxDtPerformer";
            this.gbxDtPerformer.Size = new System.Drawing.Size(513, 260);
            this.gbxDtPerformer.TabIndex = 7;
            this.gbxDtPerformer.TabStop = false;
            // 
            // txtSelectedPerformer
            // 
            this.txtSelectedPerformer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelectedPerformer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtSelectedPerformer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtSelectedPerformer.Location = new System.Drawing.Point(6, 234);
            this.txtSelectedPerformer.Name = "txtSelectedPerformer";
            this.txtSelectedPerformer.Size = new System.Drawing.Size(501, 21);
            this.txtSelectedPerformer.TabIndex = 49;
            this.txtSelectedPerformer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSelectedPerformer.TextChanged += new System.EventHandler(this.txtSelectedPerformer_TextChanged);
            // 
            // gcTel
            // 
            this.gcTel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcTel.Location = new System.Drawing.Point(6, 34);
            this.gcTel.MainView = this.gvTel;
            this.gcTel.Name = "gcTel";
            this.gcTel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gcTel.Size = new System.Drawing.Size(504, 184);
            this.gcTel.TabIndex = 48;
            this.gcTel.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTel,
            this.cardView1});
            this.gcTel.Load += new System.EventHandler(this.gcTel_Load);
            this.gcTel.DoubleClick += new System.EventHandler(this.gcTel_DoubleClick);
            this.gcTel.Enter += new System.EventHandler(this.gcTel_Enter);
            this.gcTel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gcTel_KeyDown);
            // 
            // gvTel
            // 
            this.gvTel.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvTel.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvTel.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvTel.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvTel.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.gvTel.Appearance.FocusedRow.ForeColor = System.Drawing.Color.DarkGreen;
            this.gvTel.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTel.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTel.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvTel.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvTel.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvTel.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.gvTel.Appearance.Row.Options.UseFont = true;
            this.gvTel.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gvTel.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.gvTel.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTel.GridControl = this.gcTel;
            this.gvTel.Name = "gvTel";
            this.gvTel.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvTel.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvTel.OptionsBehavior.Editable = false;
            this.gvTel.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditForm;
            this.gvTel.OptionsCustomization.AllowGroup = false;
            this.gvTel.OptionsFind.AlwaysVisible = true;
            this.gvTel.OptionsFind.Condition = DevExpress.Data.Filtering.FilterCondition.Contains;
            this.gvTel.OptionsFind.FindNullPrompt = "لطفا بخشی از نام نام خانوادگی شماره تلفن یا محل کار را جهت جستجو وارد کنید";
            this.gvTel.OptionsFind.SearchInPreview = true;
            this.gvTel.OptionsNavigation.AutoMoveRowFocus = false;
            this.gvTel.OptionsScrollAnnotations.ShowFocusedRow = DevExpress.Utils.DefaultBoolean.True;
            this.gvTel.OptionsScrollAnnotations.ShowSelectedRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvTel.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvTel.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.gvTel.OptionsView.ShowColumnHeaders = false;
            this.gvTel.OptionsView.ShowDetailButtons = false;
            this.gvTel.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvTel.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvTel.OptionsView.ShowGroupPanel = false;
            this.gvTel.OptionsView.ShowIndicator = false;
            this.gvTel.OptionsView.ShowViewCaption = true;
            this.gvTel.ViewCaption = "انتخاب انجام دهنده";
            this.gvTel.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvTel_FocusedRowChanged);
            // 
            // cardView1
            // 
            this.cardView1.GridControl = this.gcTel;
            this.cardView1.Name = "cardView1";
            this.cardView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            this.cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            // 
            // gbxDtSelectTime
            // 
            this.gbxDtSelectTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDtSelectTime.Controls.Add(this.nudDtEndHour);
            this.gbxDtSelectTime.Controls.Add(this.lblDtEndTime);
            this.gbxDtSelectTime.Controls.Add(this.nudDtEndMin);
            this.gbxDtSelectTime.Controls.Add(this.lblDb15);
            this.gbxDtSelectTime.Controls.Add(this.nudDtStHour);
            this.gbxDtSelectTime.Controls.Add(this.lblDb14);
            this.gbxDtSelectTime.Controls.Add(this.nudDtStMin);
            this.gbxDtSelectTime.Controls.Add(this.lblDtStTime);
            this.gbxDtSelectTime.Enabled = false;
            this.gbxDtSelectTime.Location = new System.Drawing.Point(274, 158);
            this.gbxDtSelectTime.Name = "gbxDtSelectTime";
            this.gbxDtSelectTime.Size = new System.Drawing.Size(233, 88);
            this.gbxDtSelectTime.TabIndex = 3;
            this.gbxDtSelectTime.TabStop = false;
            this.gbxDtSelectTime.Text = "زمان انجام فعالیت ";
            // 
            // nudDtEndHour
            // 
            this.nudDtEndHour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudDtEndHour.ForeColor = System.Drawing.Color.Navy;
            this.nudDtEndHour.Location = new System.Drawing.Point(43, 58);
            this.nudDtEndHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudDtEndHour.Name = "nudDtEndHour";
            this.nudDtEndHour.Size = new System.Drawing.Size(44, 21);
            this.nudDtEndHour.TabIndex = 109;
            this.nudDtEndHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDtEndHour.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // lblDtEndTime
            // 
            this.lblDtEndTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDtEndTime.AutoSize = true;
            this.lblDtEndTime.ForeColor = System.Drawing.Color.Navy;
            this.lblDtEndTime.Location = new System.Drawing.Point(159, 61);
            this.lblDtEndTime.Name = "lblDtEndTime";
            this.lblDtEndTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDtEndTime.Size = new System.Drawing.Size(60, 13);
            this.lblDtEndTime.TabIndex = 112;
            this.lblDtEndTime.Text = ": زمان اتمام";
            // 
            // nudDtEndMin
            // 
            this.nudDtEndMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudDtEndMin.ForeColor = System.Drawing.Color.Navy;
            this.nudDtEndMin.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudDtEndMin.Location = new System.Drawing.Point(106, 58);
            this.nudDtEndMin.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudDtEndMin.Name = "nudDtEndMin";
            this.nudDtEndMin.Size = new System.Drawing.Size(44, 21);
            this.nudDtEndMin.TabIndex = 110;
            this.nudDtEndMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDb15
            // 
            this.lblDb15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDb15.AutoSize = true;
            this.lblDb15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblDb15.Location = new System.Drawing.Point(91, 62);
            this.lblDb15.Name = "lblDb15";
            this.lblDb15.Size = new System.Drawing.Size(11, 13);
            this.lblDb15.TabIndex = 111;
            this.lblDb15.Text = ":";
            // 
            // nudDtStHour
            // 
            this.nudDtStHour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudDtStHour.ForeColor = System.Drawing.Color.Teal;
            this.nudDtStHour.Location = new System.Drawing.Point(43, 23);
            this.nudDtStHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudDtStHour.Name = "nudDtStHour";
            this.nudDtStHour.Size = new System.Drawing.Size(45, 21);
            this.nudDtStHour.TabIndex = 105;
            this.nudDtStHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDtStHour.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // lblDb14
            // 
            this.lblDb14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDb14.AutoSize = true;
            this.lblDb14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblDb14.Location = new System.Drawing.Point(94, 25);
            this.lblDb14.Name = "lblDb14";
            this.lblDb14.Size = new System.Drawing.Size(11, 13);
            this.lblDb14.TabIndex = 107;
            this.lblDb14.Text = ":";
            // 
            // nudDtStMin
            // 
            this.nudDtStMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudDtStMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudDtStMin.ForeColor = System.Drawing.Color.Teal;
            this.nudDtStMin.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudDtStMin.Location = new System.Drawing.Point(108, 23);
            this.nudDtStMin.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudDtStMin.Name = "nudDtStMin";
            this.nudDtStMin.Size = new System.Drawing.Size(44, 21);
            this.nudDtStMin.TabIndex = 108;
            this.nudDtStMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDtStTime
            // 
            this.lblDtStTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDtStTime.AutoSize = true;
            this.lblDtStTime.ForeColor = System.Drawing.Color.Green;
            this.lblDtStTime.Location = new System.Drawing.Point(157, 27);
            this.lblDtStTime.Name = "lblDtStTime";
            this.lblDtStTime.Size = new System.Drawing.Size(68, 13);
            this.lblDtStTime.TabIndex = 106;
            this.lblDtStTime.Text = "زمان شروع  :";
            // 
            // gbxDtSelectDate
            // 
            this.gbxDtSelectDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDtSelectDate.Controls.Add(this.txtDtFromDate);
            this.gbxDtSelectDate.Controls.Add(this.btnDtFromDate);
            this.gbxDtSelectDate.Enabled = false;
            this.gbxDtSelectDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.gbxDtSelectDate.Location = new System.Drawing.Point(525, 158);
            this.gbxDtSelectDate.Name = "gbxDtSelectDate";
            this.gbxDtSelectDate.Size = new System.Drawing.Size(262, 88);
            this.gbxDtSelectDate.TabIndex = 2;
            this.gbxDtSelectDate.TabStop = false;
            this.gbxDtSelectDate.Text = "تاریخ انجام فعالیت ";
            // 
            // txtDtFromDate
            // 
            this.txtDtFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtFromDate.Enabled = false;
            this.txtDtFromDate.ForeColor = System.Drawing.Color.Teal;
            this.txtDtFromDate.Location = new System.Drawing.Point(15, 35);
            this.txtDtFromDate.Name = "txtDtFromDate";
            this.txtDtFromDate.ReadOnly = true;
            this.txtDtFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDtFromDate.Size = new System.Drawing.Size(128, 20);
            this.txtDtFromDate.TabIndex = 41;
            this.txtDtFromDate.TabStop = false;
            this.txtDtFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDtFromDate.Leave += new System.EventHandler(this.txtDtFromDate_Leave);
            // 
            // btnDtFromDate
            // 
            this.btnDtFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDtFromDate.ForeColor = System.Drawing.Color.Green;
            this.btnDtFromDate.Location = new System.Drawing.Point(153, 33);
            this.btnDtFromDate.Name = "btnDtFromDate";
            this.btnDtFromDate.Size = new System.Drawing.Size(92, 23);
            this.btnDtFromDate.TabIndex = 0;
            this.btnDtFromDate.Text = "تاریخ فعالیت";
            this.btnDtFromDate.UseVisualStyleBackColor = true;
            this.btnDtFromDate.Click += new System.EventHandler(this.btnDtFromDate_Click);
            // 
            // gbxDtAllSubject
            // 
            this.gbxDtAllSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDtAllSubject.Controls.Add(this.rdbPmSte);
            this.gbxDtAllSubject.Controls.Add(this.rdbPmTwr);
            this.gbxDtAllSubject.Controls.Add(this.rdbPmCln);
            this.gbxDtAllSubject.Enabled = false;
            this.gbxDtAllSubject.Location = new System.Drawing.Point(644, 67);
            this.gbxDtAllSubject.Name = "gbxDtAllSubject";
            this.gbxDtAllSubject.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbxDtAllSubject.Size = new System.Drawing.Size(140, 85);
            this.gbxDtAllSubject.TabIndex = 1;
            this.gbxDtAllSubject.TabStop = false;
            this.gbxDtAllSubject.Text = "موضوع";
            // 
            // rdbPmSte
            // 
            this.rdbPmSte.AutoSize = true;
            this.rdbPmSte.Location = new System.Drawing.Point(13, 39);
            this.rdbPmSte.Name = "rdbPmSte";
            this.rdbPmSte.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rdbPmSte.Size = new System.Drawing.Size(61, 17);
            this.rdbPmSte.TabIndex = 0;
            this.rdbPmSte.Text = "PM-Site";
            this.rdbPmSte.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.rdbPmSte.UseVisualStyleBackColor = true;
            this.rdbPmSte.CheckedChanged += new System.EventHandler(this.rdbPmSte_CheckedChanged);
            // 
            // rdbPmTwr
            // 
            this.rdbPmTwr.AutoSize = true;
            this.rdbPmTwr.Location = new System.Drawing.Point(13, 59);
            this.rdbPmTwr.Name = "rdbPmTwr";
            this.rdbPmTwr.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rdbPmTwr.Size = new System.Drawing.Size(73, 17);
            this.rdbPmTwr.TabIndex = 0;
            this.rdbPmTwr.Text = "PM-Tower";
            this.rdbPmTwr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbPmTwr.UseVisualStyleBackColor = true;
            this.rdbPmTwr.CheckedChanged += new System.EventHandler(this.rdbPmTwr_CheckedChanged);
            // 
            // rdbPmCln
            // 
            this.rdbPmCln.AutoSize = true;
            this.rdbPmCln.Location = new System.Drawing.Point(12, 19);
            this.rdbPmCln.Name = "rdbPmCln";
            this.rdbPmCln.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rdbPmCln.Size = new System.Drawing.Size(84, 17);
            this.rdbPmCln.TabIndex = 0;
            this.rdbPmCln.Text = "PM-Cleaning";
            this.rdbPmCln.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbPmCln.UseVisualStyleBackColor = true;
            this.rdbPmCln.CheckedChanged += new System.EventHandler(this.rdbPmCln_CheckedChanged);
            // 
            // gbxDtRegCode
            // 
            this.gbxDtRegCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDtRegCode.Controls.Add(this.lblReqDtRegion);
            this.gbxDtRegCode.Controls.Add(this.lblDtRegion);
            this.gbxDtRegCode.Controls.Add(this.txtDtRegSiteName);
            this.gbxDtRegCode.Controls.Add(this.lblDtSiteName);
            this.gbxDtRegCode.Controls.Add(this.lblDtStDate);
            this.gbxDtRegCode.Controls.Add(this.txtBoxDtRegCodeSite);
            this.gbxDtRegCode.Location = new System.Drawing.Point(271, 10);
            this.gbxDtRegCode.Name = "gbxDtRegCode";
            this.gbxDtRegCode.Size = new System.Drawing.Size(513, 54);
            this.gbxDtRegCode.TabIndex = 600;
            this.gbxDtRegCode.TabStop = false;
            this.gbxDtRegCode.Text = "کد سایت";
            // 
            // lblReqDtRegion
            // 
            this.lblReqDtRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReqDtRegion.AutoSize = true;
            this.lblReqDtRegion.Location = new System.Drawing.Point(60, 25);
            this.lblReqDtRegion.Name = "lblReqDtRegion";
            this.lblReqDtRegion.Size = new System.Drawing.Size(44, 13);
            this.lblReqDtRegion.TabIndex = 700;
            this.lblReqDtRegion.Text = "منطقه :";
            // 
            // lblDtRegion
            // 
            this.lblDtRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDtRegion.AutoSize = true;
            this.lblDtRegion.Enabled = false;
            this.lblDtRegion.Location = new System.Drawing.Point(32, 25);
            this.lblDtRegion.Name = "lblDtRegion";
            this.lblDtRegion.Size = new System.Drawing.Size(27, 13);
            this.lblDtRegion.TabIndex = 800;
            this.lblDtRegion.Text = "-----";
            // 
            // txtDtRegSiteName
            // 
            this.txtDtRegSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtRegSiteName.Enabled = false;
            this.txtDtRegSiteName.ForeColor = System.Drawing.Color.Teal;
            this.txtDtRegSiteName.Location = new System.Drawing.Point(107, 22);
            this.txtDtRegSiteName.Multiline = true;
            this.txtDtRegSiteName.Name = "txtDtRegSiteName";
            this.txtDtRegSiteName.ReadOnly = true;
            this.txtDtRegSiteName.Size = new System.Drawing.Size(244, 20);
            this.txtDtRegSiteName.TabIndex = 1000;
            this.txtDtRegSiteName.TabStop = false;
            // 
            // lblDtSiteName
            // 
            this.lblDtSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDtSiteName.AutoSize = true;
            this.lblDtSiteName.Location = new System.Drawing.Point(356, 26);
            this.lblDtSiteName.Name = "lblDtSiteName";
            this.lblDtSiteName.Size = new System.Drawing.Size(57, 13);
            this.lblDtSiteName.TabIndex = 500;
            this.lblDtSiteName.Text = "نام سایت :";
            // 
            // lblDtStDate
            // 
            this.lblDtStDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDtStDate.AutoSize = true;
            this.lblDtStDate.Enabled = false;
            this.lblDtStDate.Location = new System.Drawing.Point(428, 27);
            this.lblDtStDate.Name = "lblDtStDate";
            this.lblDtStDate.Size = new System.Drawing.Size(19, 13);
            this.lblDtStDate.TabIndex = 1000;
            this.lblDtStDate.Text = "ES";
            // 
            // txtBoxDtRegCodeSite
            // 
            this.txtBoxDtRegCodeSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxDtRegCodeSite.Location = new System.Drawing.Point(454, 23);
            this.txtBoxDtRegCodeSite.Name = "txtBoxDtRegCodeSite";
            this.txtBoxDtRegCodeSite.Size = new System.Drawing.Size(48, 21);
            this.txtBoxDtRegCodeSite.TabIndex = 0;
            this.txtBoxDtRegCodeSite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxDtRegCodeSite.TextChanged += new System.EventHandler(this.textBoxDtRegCodeSite_TextChanged);
            this.txtBoxDtRegCodeSite.Leave += new System.EventHandler(this.textBoxDtRegCodeSite_Leave);
            // 
            // frmPmReq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 790);
            this.Controls.Add(this.groupBoxTotal);
            this.Name = "frmPmReq";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "فرم درخواست دان تایمهای PM";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPmReq_Load);
            this.groupBoxTotal.ResumeLayout(false);
            this.gbxregistered.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDtRegiterDtByThisUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDtRegiterDtByThisUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.gbxDtPerformer.ResumeLayout(false);
            this.gbxDtPerformer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            this.gbxDtSelectTime.ResumeLayout(false);
            this.gbxDtSelectTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtEndHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtEndMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtStHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDtStMin)).EndInit();
            this.gbxDtSelectDate.ResumeLayout(false);
            this.gbxDtSelectDate.PerformLayout();
            this.gbxDtAllSubject.ResumeLayout(false);
            this.gbxDtAllSubject.PerformLayout();
            this.gbxDtRegCode.ResumeLayout(false);
            this.gbxDtRegCode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTotal;
        private System.Windows.Forms.GroupBox gbxDtRegCode;
        private System.Windows.Forms.Label lblDtStDate;
        private System.Windows.Forms.TextBox txtBoxDtRegCodeSite;
        private System.Windows.Forms.Label lblReqDtRegion;
        private System.Windows.Forms.Label lblDtRegion;
        private System.Windows.Forms.TextBox txtDtRegSiteName;
        private System.Windows.Forms.Label lblDtSiteName;
        private System.Windows.Forms.GroupBox gbxDtSelectDate;
        private System.Windows.Forms.TextBox txtDtFromDate;
        private System.Windows.Forms.Button btnDtFromDate;
        private System.Windows.Forms.GroupBox gbxDtSelectTime;
        private System.Windows.Forms.NumericUpDown nudDtEndHour;
        private System.Windows.Forms.Label lblDtEndTime;
        private System.Windows.Forms.NumericUpDown nudDtEndMin;
        private System.Windows.Forms.Label lblDb15;
        private System.Windows.Forms.NumericUpDown nudDtStHour;
        private System.Windows.Forms.Label lblDb14;
        private System.Windows.Forms.NumericUpDown nudDtStMin;
        private System.Windows.Forms.Label lblDtStTime;
        private System.Windows.Forms.GroupBox gbxDtPerformer;
        private DevExpress.XtraGrid.GridControl gcTel;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTel;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private System.Windows.Forms.TextBox txtSelectedPerformer;
        private DevExpress.XtraEditors.SimpleButton btnRegPmDt;
        private System.Windows.Forms.GroupBox gbxDtAllSubject;
        private System.Windows.Forms.RadioButton rdbPmSte;
        private System.Windows.Forms.RadioButton rdbPmTwr;
        private System.Windows.Forms.RadioButton rdbPmCln;
        private System.Windows.Forms.GroupBox gbxregistered;
        private DevExpress.XtraGrid.GridControl gridControlDtRegiterDtByThisUser;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDtRegiterDtByThisUser;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Button btnPmFlowChart;
    }
}