namespace PE.Forms
{
    partial class frmDtCreateSimilar
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
            this.groupBoxDtCreSim = new System.Windows.Forms.GroupBox();
            this.gridControlDtSelForCreSim = new DevExpress.XtraGrid.GridControl();
            this.gridViewDtSelForCreSim = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBoxDtCreSimPowerUser = new System.Windows.Forms.GroupBox();
            this.gbxDtChgGrpPower3 = new System.Windows.Forms.GroupBox();
            this.btnDtDelSelDts = new System.Windows.Forms.Button();
            this.btnDtCreSimDtInDb = new System.Windows.Forms.Button();
            this.gridControlDtFinalSelForReCreate = new DevExpress.XtraGrid.GridControl();
            this.gridViewDtFinalSelForReCreate = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gbxDtCreSim2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDtCreSimReg = new System.Windows.Forms.Button();
            this.txtDtCreSimApplicant = new System.Windows.Forms.TextBox();
            this.cbxDtCreSimSubject = new System.Windows.Forms.ComboBox();
            this.cbxDtCreSimPerformer = new System.Windows.Forms.ComboBox();
            this.gbxDtCreSim1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDtCreSimStartDate = new System.Windows.Forms.TextBox();
            this.txtDtCreSimStartTime = new System.Windows.Forms.TextBox();
            this.txtDtCreSimEndDate = new System.Windows.Forms.TextBox();
            this.txtDtCreSimEndTime = new System.Windows.Forms.TextBox();
            this.groupBoxDtCreSim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDtSelForCreSim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDtSelForCreSim)).BeginInit();
            this.groupBoxDtCreSimPowerUser.SuspendLayout();
            this.gbxDtChgGrpPower3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDtFinalSelForReCreate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDtFinalSelForReCreate)).BeginInit();
            this.gbxDtCreSim2.SuspendLayout();
            this.gbxDtCreSim1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDtCreSim
            // 
            this.groupBoxDtCreSim.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDtCreSim.Controls.Add(this.gridControlDtSelForCreSim);
            this.groupBoxDtCreSim.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDtCreSim.Name = "groupBoxDtCreSim";
            this.groupBoxDtCreSim.Size = new System.Drawing.Size(1150, 107);
            this.groupBoxDtCreSim.TabIndex = 1;
            this.groupBoxDtCreSim.TabStop = false;
            this.groupBoxDtCreSim.Text = "رکورد نمونه انتخابی جهت ایجاد گروهی";
            // 
            // gridControlDtSelForCreSim
            // 
            this.gridControlDtSelForCreSim.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlDtSelForCreSim.Location = new System.Drawing.Point(12, 19);
            this.gridControlDtSelForCreSim.MainView = this.gridViewDtSelForCreSim;
            this.gridControlDtSelForCreSim.Name = "gridControlDtSelForCreSim";
            this.gridControlDtSelForCreSim.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridControlDtSelForCreSim.Size = new System.Drawing.Size(1136, 82);
            this.gridControlDtSelForCreSim.TabIndex = 1;
            this.gridControlDtSelForCreSim.UseEmbeddedNavigator = true;
            this.gridControlDtSelForCreSim.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDtSelForCreSim});
            // 
            // gridViewDtSelForCreSim
            // 
            this.gridViewDtSelForCreSim.Appearance.FilterPanel.Options.UseTextOptions = true;
            this.gridViewDtSelForCreSim.Appearance.FilterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtSelForCreSim.Appearance.GroupRow.Options.UseTextOptions = true;
            this.gridViewDtSelForCreSim.Appearance.GroupRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtSelForCreSim.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewDtSelForCreSim.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtSelForCreSim.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewDtSelForCreSim.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtSelForCreSim.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewDtSelForCreSim.Appearance.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridViewDtSelForCreSim.GridControl = this.gridControlDtSelForCreSim;
            this.gridViewDtSelForCreSim.Name = "gridViewDtSelForCreSim";
            this.gridViewDtSelForCreSim.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewDtSelForCreSim.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewDtSelForCreSim.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewDtSelForCreSim.OptionsCustomization.AllowFilter = false;
            this.gridViewDtSelForCreSim.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewDtSelForCreSim.OptionsView.ColumnAutoWidth = false;
            this.gridViewDtSelForCreSim.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewDtSelForCreSim.OptionsView.RowAutoHeight = true;
            this.gridViewDtSelForCreSim.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewDtSelForCreSim.OptionsView.ShowGroupPanel = false;
            this.gridViewDtSelForCreSim.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gridViewDtSelForCreSim_SelectionChanged);
            // 
            // groupBoxDtCreSimPowerUser
            // 
            this.groupBoxDtCreSimPowerUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDtCreSimPowerUser.Controls.Add(this.gbxDtChgGrpPower3);
            this.groupBoxDtCreSimPowerUser.Controls.Add(this.gbxDtCreSim2);
            this.groupBoxDtCreSimPowerUser.Controls.Add(this.gbxDtCreSim1);
            this.groupBoxDtCreSimPowerUser.Location = new System.Drawing.Point(12, 125);
            this.groupBoxDtCreSimPowerUser.Name = "groupBoxDtCreSimPowerUser";
            this.groupBoxDtCreSimPowerUser.Size = new System.Drawing.Size(1161, 638);
            this.groupBoxDtCreSimPowerUser.TabIndex = 3;
            this.groupBoxDtCreSimPowerUser.TabStop = false;
            // 
            // gbxDtChgGrpPower3
            // 
            this.gbxDtChgGrpPower3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDtChgGrpPower3.Controls.Add(this.btnDtDelSelDts);
            this.gbxDtChgGrpPower3.Controls.Add(this.btnDtCreSimDtInDb);
            this.gbxDtChgGrpPower3.Controls.Add(this.gridControlDtFinalSelForReCreate);
            this.gbxDtChgGrpPower3.Location = new System.Drawing.Point(6, 119);
            this.gbxDtChgGrpPower3.Name = "gbxDtChgGrpPower3";
            this.gbxDtChgGrpPower3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbxDtChgGrpPower3.Size = new System.Drawing.Size(1143, 513);
            this.gbxDtChgGrpPower3.TabIndex = 6;
            this.gbxDtChgGrpPower3.TabStop = false;
            // 
            // btnDtDelSelDts
            // 
            this.btnDtDelSelDts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDtDelSelDts.Location = new System.Drawing.Point(901, 471);
            this.btnDtDelSelDts.Name = "btnDtDelSelDts";
            this.btnDtDelSelDts.Size = new System.Drawing.Size(224, 29);
            this.btnDtDelSelDts.TabIndex = 2;
            this.btnDtDelSelDts.Text = "حذف دان تایمهای انتخابی از لیست نهایی";
            this.btnDtDelSelDts.UseVisualStyleBackColor = true;
            this.btnDtDelSelDts.Click += new System.EventHandler(this.btnDtDelSelDts_Click);
            // 
            // btnDtCreSimDtInDb
            // 
            this.btnDtCreSimDtInDb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDtCreSimDtInDb.Location = new System.Drawing.Point(374, 471);
            this.btnDtCreSimDtInDb.Name = "btnDtCreSimDtInDb";
            this.btnDtCreSimDtInDb.Size = new System.Drawing.Size(187, 30);
            this.btnDtCreSimDtInDb.TabIndex = 1;
            this.btnDtCreSimDtInDb.Text = "اعمال نهایی لیست فوق";
            this.btnDtCreSimDtInDb.UseVisualStyleBackColor = true;
            this.btnDtCreSimDtInDb.Click += new System.EventHandler(this.btnDtCreSimDtInDb_Click);
            // 
            // gridControlDtFinalSelForReCreate
            // 
            this.gridControlDtFinalSelForReCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlDtFinalSelForReCreate.Location = new System.Drawing.Point(13, 19);
            this.gridControlDtFinalSelForReCreate.MainView = this.gridViewDtFinalSelForReCreate;
            this.gridControlDtFinalSelForReCreate.Name = "gridControlDtFinalSelForReCreate";
            this.gridControlDtFinalSelForReCreate.Size = new System.Drawing.Size(1121, 434);
            this.gridControlDtFinalSelForReCreate.TabIndex = 0;
            this.gridControlDtFinalSelForReCreate.UseEmbeddedNavigator = true;
            this.gridControlDtFinalSelForReCreate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDtFinalSelForReCreate});
            // 
            // gridViewDtFinalSelForReCreate
            // 
            this.gridViewDtFinalSelForReCreate.Appearance.GroupPanel.Options.UseTextOptions = true;
            this.gridViewDtFinalSelForReCreate.Appearance.GroupPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtFinalSelForReCreate.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewDtFinalSelForReCreate.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtFinalSelForReCreate.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewDtFinalSelForReCreate.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDtFinalSelForReCreate.GridControl = this.gridControlDtFinalSelForReCreate;
            this.gridViewDtFinalSelForReCreate.Name = "gridViewDtFinalSelForReCreate";
            this.gridViewDtFinalSelForReCreate.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridViewDtFinalSelForReCreate.OptionsSelection.MultiSelect = true;
            // 
            // gbxDtCreSim2
            // 
            this.gbxDtCreSim2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDtCreSim2.Controls.Add(this.label7);
            this.gbxDtCreSim2.Controls.Add(this.label6);
            this.gbxDtCreSim2.Controls.Add(this.label5);
            this.gbxDtCreSim2.Controls.Add(this.btnDtCreSimReg);
            this.gbxDtCreSim2.Controls.Add(this.txtDtCreSimApplicant);
            this.gbxDtCreSim2.Controls.Add(this.cbxDtCreSimSubject);
            this.gbxDtCreSim2.Controls.Add(this.cbxDtCreSimPerformer);
            this.gbxDtCreSim2.Location = new System.Drawing.Point(8, 70);
            this.gbxDtCreSim2.Name = "gbxDtCreSim2";
            this.gbxDtCreSim2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbxDtCreSim2.Size = new System.Drawing.Size(1141, 43);
            this.gbxDtCreSim2.TabIndex = 6;
            this.gbxDtCreSim2.TabStop = false;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(348, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "انجام دهنده :";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(809, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "موضوع :";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1041, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "درخواست کننده :";
            // 
            // btnDtCreSimReg
            // 
            this.btnDtCreSimReg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDtCreSimReg.Location = new System.Drawing.Point(16, 12);
            this.btnDtCreSimReg.Name = "btnDtCreSimReg";
            this.btnDtCreSimReg.Size = new System.Drawing.Size(168, 25);
            this.btnDtCreSimReg.TabIndex = 15;
            this.btnDtCreSimReg.Text = "تشکیل لیست دان تایمهای مشابه";
            this.btnDtCreSimReg.UseVisualStyleBackColor = true;
            this.btnDtCreSimReg.Click += new System.EventHandler(this.btnDtCreSimReg_Click);
            // 
            // txtDtCreSimApplicant
            // 
            this.txtDtCreSimApplicant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtCreSimApplicant.Location = new System.Drawing.Point(880, 14);
            this.txtDtCreSimApplicant.Name = "txtDtCreSimApplicant";
            this.txtDtCreSimApplicant.Size = new System.Drawing.Size(155, 20);
            this.txtDtCreSimApplicant.TabIndex = 10;
            this.txtDtCreSimApplicant.TabStop = false;
            this.txtDtCreSimApplicant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbxDtCreSimSubject
            // 
            this.cbxDtCreSimSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxDtCreSimSubject.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxDtCreSimSubject.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxDtCreSimSubject.FormattingEnabled = true;
            this.cbxDtCreSimSubject.Location = new System.Drawing.Point(420, 15);
            this.cbxDtCreSimSubject.Name = "cbxDtCreSimSubject";
            this.cbxDtCreSimSubject.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxDtCreSimSubject.Size = new System.Drawing.Size(383, 21);
            this.cbxDtCreSimSubject.TabIndex = 9;
            // 
            // cbxDtCreSimPerformer
            // 
            this.cbxDtCreSimPerformer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxDtCreSimPerformer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxDtCreSimPerformer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxDtCreSimPerformer.FormattingEnabled = true;
            this.cbxDtCreSimPerformer.Location = new System.Drawing.Point(190, 14);
            this.cbxDtCreSimPerformer.Name = "cbxDtCreSimPerformer";
            this.cbxDtCreSimPerformer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxDtCreSimPerformer.Size = new System.Drawing.Size(152, 21);
            this.cbxDtCreSimPerformer.Sorted = true;
            this.cbxDtCreSimPerformer.TabIndex = 11;
            // 
            // gbxDtCreSim1
            // 
            this.gbxDtCreSim1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDtCreSim1.Controls.Add(this.label4);
            this.gbxDtCreSim1.Controls.Add(this.label3);
            this.gbxDtCreSim1.Controls.Add(this.label2);
            this.gbxDtCreSim1.Controls.Add(this.label1);
            this.gbxDtCreSim1.Controls.Add(this.txtDtCreSimStartDate);
            this.gbxDtCreSim1.Controls.Add(this.txtDtCreSimStartTime);
            this.gbxDtCreSim1.Controls.Add(this.txtDtCreSimEndDate);
            this.gbxDtCreSim1.Controls.Add(this.txtDtCreSimEndTime);
            this.gbxDtCreSim1.Location = new System.Drawing.Point(8, 19);
            this.gbxDtCreSim1.Name = "gbxDtCreSim1";
            this.gbxDtCreSim1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbxDtCreSim1.Size = new System.Drawing.Size(1141, 45);
            this.gbxDtCreSim1.TabIndex = 6;
            this.gbxDtCreSim1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(377, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "زمان پایان :";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(586, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "تاریخ پایان :";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(819, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "زمان شروع :";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1067, 16);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "تاریخ شروع :";
            // 
            // txtDtCreSimStartDate
            // 
            this.txtDtCreSimStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtCreSimStartDate.Location = new System.Drawing.Point(973, 13);
            this.txtDtCreSimStartDate.Name = "txtDtCreSimStartDate";
            this.txtDtCreSimStartDate.Size = new System.Drawing.Size(89, 20);
            this.txtDtCreSimStartDate.TabIndex = 31;
            this.txtDtCreSimStartDate.TabStop = false;
            this.txtDtCreSimStartDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDtCreSimStartDate.Leave += new System.EventHandler(this.txtDtCreSimStartDate_Leave);
            // 
            // txtDtCreSimStartTime
            // 
            this.txtDtCreSimStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtCreSimStartTime.Location = new System.Drawing.Point(713, 12);
            this.txtDtCreSimStartTime.Name = "txtDtCreSimStartTime";
            this.txtDtCreSimStartTime.Size = new System.Drawing.Size(100, 20);
            this.txtDtCreSimStartTime.TabIndex = 29;
            this.txtDtCreSimStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtDtCreSimEndDate
            // 
            this.txtDtCreSimEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtCreSimEndDate.Location = new System.Drawing.Point(491, 13);
            this.txtDtCreSimEndDate.Name = "txtDtCreSimEndDate";
            this.txtDtCreSimEndDate.Size = new System.Drawing.Size(89, 20);
            this.txtDtCreSimEndDate.TabIndex = 32;
            this.txtDtCreSimEndDate.TabStop = false;
            this.txtDtCreSimEndDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDtCreSimEndDate.Leave += new System.EventHandler(this.txtDtCreSimEndDate_Leave);
            // 
            // txtDtCreSimEndTime
            // 
            this.txtDtCreSimEndTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtCreSimEndTime.Location = new System.Drawing.Point(271, 15);
            this.txtDtCreSimEndTime.Name = "txtDtCreSimEndTime";
            this.txtDtCreSimEndTime.Size = new System.Drawing.Size(100, 20);
            this.txtDtCreSimEndTime.TabIndex = 30;
            this.txtDtCreSimEndTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmDtCreateSimilar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 775);
            this.Controls.Add(this.groupBoxDtCreSimPowerUser);
            this.Controls.Add(this.groupBoxDtCreSim);
            this.Name = "frmDtCreateSimilar";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "فرم ایجاد دان تایمهای مشابه";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmDtCreateSimilar_FormClosed);
            this.Load += new System.EventHandler(this.frmDtCreateSimilar_Load);
            this.groupBoxDtCreSim.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDtSelForCreSim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDtSelForCreSim)).EndInit();
            this.groupBoxDtCreSimPowerUser.ResumeLayout(false);
            this.gbxDtChgGrpPower3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDtFinalSelForReCreate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDtFinalSelForReCreate)).EndInit();
            this.gbxDtCreSim2.ResumeLayout(false);
            this.gbxDtCreSim2.PerformLayout();
            this.gbxDtCreSim1.ResumeLayout(false);
            this.gbxDtCreSim1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDtCreSim;
        private DevExpress.XtraGrid.GridControl gridControlDtSelForCreSim;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDtSelForCreSim;
        
        private System.Windows.Forms.GroupBox groupBoxDtCreSimPowerUser;
        private System.Windows.Forms.GroupBox gbxDtChgGrpPower3;
        private System.Windows.Forms.Button btnDtCreSimReg;
        private System.Windows.Forms.GroupBox gbxDtCreSim2;
        private System.Windows.Forms.TextBox txtDtCreSimApplicant;
        private System.Windows.Forms.ComboBox cbxDtCreSimSubject;
        private System.Windows.Forms.ComboBox cbxDtCreSimPerformer;
        private System.Windows.Forms.GroupBox gbxDtCreSim1;
        private System.Windows.Forms.TextBox txtDtCreSimStartDate;
        private System.Windows.Forms.TextBox txtDtCreSimStartTime;
        private System.Windows.Forms.TextBox txtDtCreSimEndDate;
        private System.Windows.Forms.TextBox txtDtCreSimEndTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gridControlDtFinalSelForReCreate;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDtFinalSelForReCreate;
        private System.Windows.Forms.Button btnDtCreSimDtInDb;
        private System.Windows.Forms.Button btnDtDelSelDts;
    }
}