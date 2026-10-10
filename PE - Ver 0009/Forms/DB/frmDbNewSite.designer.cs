namespace PE.Forms.DB
{
    partial class frmDbNewSite
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.groupBoxDbAddNewSite = new System.Windows.Forms.GroupBox();
            this.txtDbNewAddress = new System.Windows.Forms.TextBox();
            this.lblDbNewSiteAddress = new System.Windows.Forms.Label();
            this.cbxDbNewRegion = new System.Windows.Forms.ComboBox();
            this.cbxContractor = new System.Windows.Forms.ComboBox();
            this.txtDbNewSiteName = new System.Windows.Forms.TextBox();
            this.txtDbNewLocation = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblDbNewRegion = new System.Windows.Forms.Label();
            this.lblDbNewContractor = new System.Windows.Forms.Label();
            this.lblDbNewSite = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.btnDbAddNewSite = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBoxDbDelNewSite = new System.Windows.Forms.GroupBox();
            this.btnDbDelAllNewSite = new System.Windows.Forms.Button();
            this.btnDbDelSelectedNewSite = new System.Windows.Forms.Button();
            this.btnDbExportNewPlan = new System.Windows.Forms.Button();
            this.saveFileDialogDbExportNewPln = new System.Windows.Forms.SaveFileDialog();
            this.groupBoxDbNewPln = new System.Windows.Forms.GroupBox();
            this.gridControlDbNewSite = new DevExpress.XtraGrid.GridControl();
            this.gridViewDbNewSite = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBoxDbAddNewSite.SuspendLayout();
            this.groupBoxDbDelNewSite.SuspendLayout();
            this.groupBoxDbNewPln.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDbNewSite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDbNewSite)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxDbAddNewSite
            // 
            this.groupBoxDbAddNewSite.Controls.Add(this.txtDbNewAddress);
            this.groupBoxDbAddNewSite.Controls.Add(this.lblDbNewSiteAddress);
            this.groupBoxDbAddNewSite.Controls.Add(this.cbxDbNewRegion);
            this.groupBoxDbAddNewSite.Controls.Add(this.cbxContractor);
            this.groupBoxDbAddNewSite.Controls.Add(this.txtDbNewSiteName);
            this.groupBoxDbAddNewSite.Controls.Add(this.txtDbNewLocation);
            this.groupBoxDbAddNewSite.Controls.Add(this.textBox1);
            this.groupBoxDbAddNewSite.Controls.Add(this.lblDbNewRegion);
            this.groupBoxDbAddNewSite.Controls.Add(this.lblDbNewContractor);
            this.groupBoxDbAddNewSite.Controls.Add(this.lblDbNewSite);
            this.groupBoxDbAddNewSite.Controls.Add(this.lblLocation);
            this.groupBoxDbAddNewSite.Controls.Add(this.btnDbAddNewSite);
            this.groupBoxDbAddNewSite.Location = new System.Drawing.Point(12, 9);
            this.groupBoxDbAddNewSite.Name = "groupBoxDbAddNewSite";
            this.groupBoxDbAddNewSite.Size = new System.Drawing.Size(460, 279);
            this.groupBoxDbAddNewSite.TabIndex = 2;
            this.groupBoxDbAddNewSite.TabStop = false;
            this.groupBoxDbAddNewSite.Text = "ADD NEW SITE";
            // 
            // txtDbNewAddress
            // 
            this.txtDbNewAddress.Location = new System.Drawing.Point(68, 170);
            this.txtDbNewAddress.Name = "txtDbNewAddress";
            this.txtDbNewAddress.Size = new System.Drawing.Size(216, 20);
            this.txtDbNewAddress.TabIndex = 4;
            this.txtDbNewAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDbNewAddress.MouseEnter += new System.EventHandler(this.TextBox2_MouseEnter);
            // 
            // lblDbNewSiteAddress
            // 
            this.lblDbNewSiteAddress.AutoSize = true;
            this.lblDbNewSiteAddress.Location = new System.Drawing.Point(8, 176);
            this.lblDbNewSiteAddress.Name = "lblDbNewSiteAddress";
            this.lblDbNewSiteAddress.Size = new System.Drawing.Size(51, 13);
            this.lblDbNewSiteAddress.TabIndex = 8;
            this.lblDbNewSiteAddress.Text = "Address :";
            // 
            // cbxDbNewRegion
            // 
            this.cbxDbNewRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDbNewRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.cbxDbNewRegion.FormattingEnabled = true;
            this.cbxDbNewRegion.Location = new System.Drawing.Point(69, 138);
            this.cbxDbNewRegion.Name = "cbxDbNewRegion";
            this.cbxDbNewRegion.Size = new System.Drawing.Size(138, 21);
            this.cbxDbNewRegion.Sorted = true;
            this.cbxDbNewRegion.TabIndex = 3;
            // 
            // cbxContractor
            // 
            this.cbxContractor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxContractor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.cbxContractor.FormattingEnabled = true;
            this.cbxContractor.Location = new System.Drawing.Point(69, 109);
            this.cbxContractor.Name = "cbxContractor";
            this.cbxContractor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxContractor.Size = new System.Drawing.Size(180, 21);
            this.cbxContractor.Sorted = true;
            this.cbxContractor.TabIndex = 2;
            // 
            // txtDbNewSiteName
            // 
            this.txtDbNewSiteName.Location = new System.Drawing.Point(69, 76);
            this.txtDbNewSiteName.Name = "txtDbNewSiteName";
            this.txtDbNewSiteName.Size = new System.Drawing.Size(290, 20);
            this.txtDbNewSiteName.TabIndex = 1;
            this.txtDbNewSiteName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDbNewSiteName.Enter += new System.EventHandler(this.TxtDbNewSiteName_Enter);
            // 
            // txtDbNewLocation
            // 
            this.txtDbNewLocation.Location = new System.Drawing.Point(91, 47);
            this.txtDbNewLocation.MaxLength = 5;
            this.txtDbNewLocation.Name = "txtDbNewLocation";
            this.txtDbNewLocation.Size = new System.Drawing.Size(45, 20);
            this.txtDbNewLocation.TabIndex = 0;
            this.txtDbNewLocation.TextChanged += new System.EventHandler(this.TxtDbNewLocation_TextChanged);
            this.txtDbNewLocation.Enter += new System.EventHandler(this.TxtDbNewLocation_Enter);
            this.txtDbNewLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox2_KeyPress);
            this.txtDbNewLocation.Leave += new System.EventHandler(this.TxtDbNewLocation_Leave);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(69, 47);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(22, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "ES";
            // 
            // lblDbNewRegion
            // 
            this.lblDbNewRegion.AutoSize = true;
            this.lblDbNewRegion.Location = new System.Drawing.Point(8, 140);
            this.lblDbNewRegion.Name = "lblDbNewRegion";
            this.lblDbNewRegion.Size = new System.Drawing.Size(47, 13);
            this.lblDbNewRegion.TabIndex = 4;
            this.lblDbNewRegion.Text = "Region :";
            // 
            // lblDbNewContractor
            // 
            this.lblDbNewContractor.AutoSize = true;
            this.lblDbNewContractor.Location = new System.Drawing.Point(5, 112);
            this.lblDbNewContractor.Name = "lblDbNewContractor";
            this.lblDbNewContractor.Size = new System.Drawing.Size(62, 13);
            this.lblDbNewContractor.TabIndex = 4;
            this.lblDbNewContractor.Text = "Contractor :";
            // 
            // lblDbNewSite
            // 
            this.lblDbNewSite.AutoSize = true;
            this.lblDbNewSite.Location = new System.Drawing.Point(5, 81);
            this.lblDbNewSite.Name = "lblDbNewSite";
            this.lblDbNewSite.Size = new System.Drawing.Size(62, 13);
            this.lblDbNewSite.TabIndex = 4;
            this.lblDbNewSite.Text = "Site Name :";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(8, 50);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(50, 13);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "location :";
            // 
            // btnDbAddNewSite
            // 
            this.btnDbAddNewSite.Location = new System.Drawing.Point(155, 227);
            this.btnDbAddNewSite.Name = "btnDbAddNewSite";
            this.btnDbAddNewSite.Size = new System.Drawing.Size(118, 23);
            this.btnDbAddNewSite.TabIndex = 5;
            this.btnDbAddNewSite.Text = "ADD New Site";
            this.btnDbAddNewSite.UseVisualStyleBackColor = true;
            this.btnDbAddNewSite.Click += new System.EventHandler(this.BtnDbAddNewSite_Click);
            // 
            // groupBoxDbDelNewSite
            // 
            this.groupBoxDbDelNewSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxDbDelNewSite.Controls.Add(this.btnDbDelAllNewSite);
            this.groupBoxDbDelNewSite.Controls.Add(this.btnDbDelSelectedNewSite);
            this.groupBoxDbDelNewSite.Location = new System.Drawing.Point(12, 305);
            this.groupBoxDbDelNewSite.Name = "groupBoxDbDelNewSite";
            this.groupBoxDbDelNewSite.Size = new System.Drawing.Size(460, 444);
            this.groupBoxDbDelNewSite.TabIndex = 3;
            this.groupBoxDbDelNewSite.TabStop = false;
            this.groupBoxDbDelNewSite.Text = "Delete New Site";
            // 
            // btnDbDelAllNewSite
            // 
            this.btnDbDelAllNewSite.Location = new System.Drawing.Point(144, 57);
            this.btnDbDelAllNewSite.Name = "btnDbDelAllNewSite";
            this.btnDbDelAllNewSite.Size = new System.Drawing.Size(175, 23);
            this.btnDbDelAllNewSite.TabIndex = 0;
            this.btnDbDelAllNewSite.Text = "Delete All New Site";
            this.btnDbDelAllNewSite.UseVisualStyleBackColor = true;
            this.btnDbDelAllNewSite.Click += new System.EventHandler(this.BtnDbDelAllNewSite_Click);
            // 
            // btnDbDelSelectedNewSite
            // 
            this.btnDbDelSelectedNewSite.Enabled = false;
            this.btnDbDelSelectedNewSite.Location = new System.Drawing.Point(143, 19);
            this.btnDbDelSelectedNewSite.Name = "btnDbDelSelectedNewSite";
            this.btnDbDelSelectedNewSite.Size = new System.Drawing.Size(176, 23);
            this.btnDbDelSelectedNewSite.TabIndex = 0;
            this.btnDbDelSelectedNewSite.Text = "Delete Selected NewSite";
            this.btnDbDelSelectedNewSite.UseVisualStyleBackColor = true;
            this.btnDbDelSelectedNewSite.Click += new System.EventHandler(this.BtnDbDelSelectedNewSite_Click);
            // 
            // btnDbExportNewPlan
            // 
            this.btnDbExportNewPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDbExportNewPlan.Location = new System.Drawing.Point(509, 19);
            this.btnDbExportNewPlan.Name = "btnDbExportNewPlan";
            this.btnDbExportNewPlan.Size = new System.Drawing.Size(102, 23);
            this.btnDbExportNewPlan.TabIndex = 4;
            this.btnDbExportNewPlan.Text = "خروجی به اکسل";
            this.btnDbExportNewPlan.UseVisualStyleBackColor = true;
            this.btnDbExportNewPlan.Click += new System.EventHandler(this.btnDbExportNewPlan_Click);
            // 
            // saveFileDialogDbExportNewPln
            // 
            this.saveFileDialogDbExportNewPln.DefaultExt = "xlsx";
            this.saveFileDialogDbExportNewPln.FileName = "New Site Plan";
            // 
            // groupBoxDbNewPln
            // 
            this.groupBoxDbNewPln.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDbNewPln.Controls.Add(this.gridControlDbNewSite);
            this.groupBoxDbNewPln.Controls.Add(this.btnDbExportNewPlan);
            this.groupBoxDbNewPln.Location = new System.Drawing.Point(501, 21);
            this.groupBoxDbNewPln.Name = "groupBoxDbNewPln";
            this.groupBoxDbNewPln.Size = new System.Drawing.Size(619, 731);
            this.groupBoxDbNewPln.TabIndex = 5;
            this.groupBoxDbNewPln.TabStop = false;
            // 
            // gridControlDbNewSite
            // 
            this.gridControlDbNewSite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gridControlDbNewSite.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControlDbNewSite.Location = new System.Drawing.Point(22, 48);
            this.gridControlDbNewSite.MainView = this.gridViewDbNewSite;
            this.gridControlDbNewSite.Name = "gridControlDbNewSite";
            this.gridControlDbNewSite.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridControlDbNewSite.Size = new System.Drawing.Size(590, 663);
            this.gridControlDbNewSite.TabIndex = 5;
            this.gridControlDbNewSite.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDbNewSite});
            this.gridControlDbNewSite.Click += new System.EventHandler(this.gridControlDbNewSite_Click);
            this.gridControlDbNewSite.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControlDbNewSite_KeyDown);
            // 
            // gridViewDbNewSite
            // 
            this.gridViewDbNewSite.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewDbNewSite.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDbNewSite.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewDbNewSite.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewDbNewSite.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDbNewSite.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewDbNewSite.GridControl = this.gridControlDbNewSite;
            this.gridViewDbNewSite.Name = "gridViewDbNewSite";
            this.gridViewDbNewSite.OptionsView.ColumnAutoWidth = false;
            this.gridViewDbNewSite.OptionsView.ShowAutoFilterRow = true;
            this.gridViewDbNewSite.OptionsView.ShowGroupPanel = false;
            // 
            // frmDbNewSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 764);
            this.Controls.Add(this.groupBoxDbNewPln);
            this.Controls.Add(this.groupBoxDbDelNewSite);
            this.Controls.Add(this.groupBoxDbAddNewSite);
            this.Name = "frmDbNewSite";
            this.Text = "New Plans";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmDbNewSite_Load);
            this.groupBoxDbAddNewSite.ResumeLayout(false);
            this.groupBoxDbAddNewSite.PerformLayout();
            this.groupBoxDbDelNewSite.ResumeLayout(false);
            this.groupBoxDbNewPln.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDbNewSite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDbNewSite)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxDbAddNewSite;
        private System.Windows.Forms.Button btnDbAddNewSite;
        private System.Windows.Forms.TextBox txtDbNewLocation;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtDbNewSiteName;
        private System.Windows.Forms.Label lblDbNewSite;
        private System.Windows.Forms.Label lblDbNewContractor;
        private System.Windows.Forms.ComboBox cbxContractor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ComboBox cbxDbNewRegion;
        private System.Windows.Forms.Label lblDbNewRegion;
        private System.Windows.Forms.TextBox txtDbNewAddress;
        private System.Windows.Forms.Label lblDbNewSiteAddress;
        private System.Windows.Forms.GroupBox groupBoxDbDelNewSite;
        private System.Windows.Forms.Button btnDbDelAllNewSite;
        private System.Windows.Forms.Button btnDbDelSelectedNewSite;
        private System.Windows.Forms.Button btnDbExportNewPlan;
        private System.Windows.Forms.SaveFileDialog saveFileDialogDbExportNewPln;
        private System.Windows.Forms.GroupBox groupBoxDbNewPln;
        private DevExpress.XtraGrid.GridControl gridControlDbNewSite;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDbNewSite;
    }
}