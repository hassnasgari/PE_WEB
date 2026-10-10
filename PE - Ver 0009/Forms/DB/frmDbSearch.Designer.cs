namespace PE.Forms.DB
{
    partial class frmDbSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDbSearch));
            this.txtSiteNameForSearch = new System.Windows.Forms.TextBox();
            this.lblSiteName = new System.Windows.Forms.Label();
            this.lblEs = new System.Windows.Forms.Label();
            this.btnDbSearch = new System.Windows.Forms.Button();
            this.txtCodeSiteForSearch = new System.Windows.Forms.TextBox();
            this.radioGroupTypeOfSearch = new DevExpress.XtraEditors.RadioGroup();
            this.gridControlDbSearch = new DevExpress.XtraGrid.GridControl();
            this.gridViewDbSearch = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.statusStripDb2 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelSearch = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnHowOnMap = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupTypeOfSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDbSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDbSearch)).BeginInit();
            this.statusStripDb2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSiteNameForSearch
            // 
            this.txtSiteNameForSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSiteNameForSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtSiteNameForSearch.Location = new System.Drawing.Point(397, 70);
            this.txtSiteNameForSearch.Name = "txtSiteNameForSearch";
            this.txtSiteNameForSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSiteNameForSearch.Size = new System.Drawing.Size(61, 29);
            this.txtSiteNameForSearch.TabIndex = 8;
            this.txtSiteNameForSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSiteNameForSearch.TextChanged += new System.EventHandler(this.txtSiteNameForSearch_TextChanged);
            this.txtSiteNameForSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSiteNameForSearch_KeyDown);
            // 
            // lblSiteName
            // 
            this.lblSiteName.AutoSize = true;
            this.lblSiteName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblSiteName.Location = new System.Drawing.Point(140, 70);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSiteName.Size = new System.Drawing.Size(251, 24);
            this.lblSiteName.TabIndex = 12;
            this.lblSiteName.Text = "لطفا نام سایت را بطور کامل وارد کنید :";
            // 
            // lblEs
            // 
            this.lblEs.AutoSize = true;
            this.lblEs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEs.ForeColor = System.Drawing.Color.Coral;
            this.lblEs.Location = new System.Drawing.Point(72, 72);
            this.lblEs.Name = "lblEs";
            this.lblEs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblEs.Size = new System.Drawing.Size(35, 24);
            this.lblEs.TabIndex = 11;
            this.lblEs.Text = "ES";
            // 
            // btnDbSearch
            // 
            this.btnDbSearch.Location = new System.Drawing.Point(18, 117);
            this.btnDbSearch.Name = "btnDbSearch";
            this.btnDbSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnDbSearch.Size = new System.Drawing.Size(193, 37);
            this.btnDbSearch.TabIndex = 10;
            this.btnDbSearch.Text = "جستجو";
            this.btnDbSearch.UseVisualStyleBackColor = true;
            this.btnDbSearch.Click += new System.EventHandler(this.btnDbSearch_Click);
            // 
            // txtCodeSiteForSearch
            // 
            this.txtCodeSiteForSearch.AcceptsReturn = true;
            this.txtCodeSiteForSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodeSiteForSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtCodeSiteForSearch.Location = new System.Drawing.Point(18, 69);
            this.txtCodeSiteForSearch.MaxLength = 4;
            this.txtCodeSiteForSearch.Name = "txtCodeSiteForSearch";
            this.txtCodeSiteForSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCodeSiteForSearch.Size = new System.Drawing.Size(54, 29);
            this.txtCodeSiteForSearch.TabIndex = 7;
            this.txtCodeSiteForSearch.TextChanged += new System.EventHandler(this.txtCodeSiteForSearch_TextChanged);
            // 
            // radioGroupTypeOfSearch
            // 
            this.radioGroupTypeOfSearch.Location = new System.Drawing.Point(12, 12);
            this.radioGroupTypeOfSearch.Name = "radioGroupTypeOfSearch";
            this.radioGroupTypeOfSearch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.radioGroupTypeOfSearch.Properties.Appearance.Options.UseFont = true;
            this.radioGroupTypeOfSearch.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "کد سایت"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "نام سایت")});
            this.radioGroupTypeOfSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioGroupTypeOfSearch.Size = new System.Drawing.Size(228, 39);
            this.radioGroupTypeOfSearch.TabIndex = 9;
            this.radioGroupTypeOfSearch.SelectedIndexChanged += new System.EventHandler(this.radioGroupTypeOfSearch_SelectedIndexChanged);
            // 
            // gridControlDbSearch
            // 
            this.gridControlDbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlDbSearch.Location = new System.Drawing.Point(12, 243);
            this.gridControlDbSearch.MainView = this.gridViewDbSearch;
            this.gridControlDbSearch.Name = "gridControlDbSearch";
            this.gridControlDbSearch.Size = new System.Drawing.Size(1240, 115);
            this.gridControlDbSearch.TabIndex = 13;
            this.gridControlDbSearch.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDbSearch});
            // 
            // gridViewDbSearch
            // 
            this.gridViewDbSearch.ActiveFilterEnabled = false;
            this.gridViewDbSearch.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridViewDbSearch.Appearance.FixedLine.Options.UseTextOptions = true;
            this.gridViewDbSearch.Appearance.FixedLine.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridViewDbSearch.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridViewDbSearch.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridViewDbSearch.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewDbSearch.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDbSearch.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewDbSearch.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridViewDbSearch.Appearance.HotTrackedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.HotTrackedRow.Options.UseBackColor = true;
            this.gridViewDbSearch.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.OddRow.Options.UseBackColor = true;
            this.gridViewDbSearch.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.Row.Options.UseBackColor = true;
            this.gridViewDbSearch.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewDbSearch.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDbSearch.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gridViewDbSearch.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridViewDbSearch.Appearance.TopNewRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridViewDbSearch.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gridViewDbSearch.GridControl = this.gridControlDbSearch;
            this.gridViewDbSearch.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridViewDbSearch.Name = "gridViewDbSearch";
            this.gridViewDbSearch.OptionsBehavior.Editable = false;
            this.gridViewDbSearch.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewDbSearch.OptionsFilter.FilterEditorAllowCustomExpressions = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewDbSearch.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewDbSearch.OptionsMenu.ShowSummaryItemMode = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewDbSearch.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewDbSearch.OptionsView.ColumnAutoWidth = false;
            this.gridViewDbSearch.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewDbSearch.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewDbSearch.OptionsView.ShowGroupPanel = false;
            this.gridViewDbSearch.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridViewDbSearch.ViewCaption = "نتیجه ی جستجو";
            this.gridViewDbSearch.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewDbSearch_FocusedRowChanged);
            // 
            // statusStripDb2
            // 
            this.statusStripDb2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelSearch});
            this.statusStripDb2.Location = new System.Drawing.Point(0, 361);
            this.statusStripDb2.Name = "statusStripDb2";
            this.statusStripDb2.Size = new System.Drawing.Size(1265, 22);
            this.statusStripDb2.TabIndex = 14;
            this.statusStripDb2.Text = "statusStrip1";
            // 
            // toolStripStatusLabelSearch
            // 
            this.toolStripStatusLabelSearch.Name = "toolStripStatusLabelSearch";
            this.toolStripStatusLabelSearch.Size = new System.Drawing.Size(0, 17);
            // 
            // btnHowOnMap
            // 
            this.btnHowOnMap.Image = ((System.Drawing.Image)(resources.GetObject("btnHowOnMap.Image")));
            this.btnHowOnMap.Location = new System.Drawing.Point(1130, 12);
            this.btnHowOnMap.Name = "btnHowOnMap";
            this.btnHowOnMap.Size = new System.Drawing.Size(123, 114);
            this.btnHowOnMap.TabIndex = 15;
            this.btnHowOnMap.Text = "نمایش روی نقشه";
            this.btnHowOnMap.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHowOnMap.UseVisualStyleBackColor = true;
            this.btnHowOnMap.Click += new System.EventHandler(this.btnHowOnMap_Click);
            // 
            // frmDbSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 383);
            this.Controls.Add(this.btnHowOnMap);
            this.Controls.Add(this.statusStripDb2);
            this.Controls.Add(this.gridControlDbSearch);
            this.Controls.Add(this.txtSiteNameForSearch);
            this.Controls.Add(this.lblSiteName);
            this.Controls.Add(this.lblEs);
            this.Controls.Add(this.btnDbSearch);
            this.Controls.Add(this.txtCodeSiteForSearch);
            this.Controls.Add(this.radioGroupTypeOfSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDbSearch";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "فرم دیتابیس سایتهای موبایل";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDbSearch_FormClosed);
            this.Load += new System.EventHandler(this.frmDbSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupTypeOfSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDbSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDbSearch)).EndInit();
            this.statusStripDb2.ResumeLayout(false);
            this.statusStripDb2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSiteNameForSearch;
        private System.Windows.Forms.Label lblSiteName;
        private System.Windows.Forms.Label lblEs;
        private System.Windows.Forms.Button btnDbSearch;
        private System.Windows.Forms.TextBox txtCodeSiteForSearch;
        private DevExpress.XtraEditors.RadioGroup radioGroupTypeOfSearch;
        private DevExpress.XtraGrid.GridControl gridControlDbSearch;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDbSearch;
        private System.Windows.Forms.StatusStrip statusStripDb2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSearch;
        private System.Windows.Forms.Button btnHowOnMap;
    }
}