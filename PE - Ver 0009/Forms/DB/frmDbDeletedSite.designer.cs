namespace PE.Forms.DB
{
    partial class frmDbDeletedSite
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.btnDbDelSelectedDeletedSite = new System.Windows.Forms.Button();
            this.groupBoxDbDelDeletedSite = new System.Windows.Forms.GroupBox();
            this.btnDbDelAllDeletedSite = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.txtDbDeletedSiteName = new System.Windows.Forms.TextBox();
            this.txtDbDeletedLocation = new System.Windows.Forms.TextBox();
            this.txtEs = new System.Windows.Forms.TextBox();
            this.lblDbDeletedSite = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.btnDbAddDeletedSite = new System.Windows.Forms.Button();
            this.groupBoxDbAddDeletedSite = new System.Windows.Forms.GroupBox();
            this.txtDeletedDate = new System.Windows.Forms.TextBox();
            this.btnDeletedDate = new System.Windows.Forms.Button();
            this.lblDbDeletedDate = new System.Windows.Forms.Label();
            this.saveFileDialogDbDelSitesPlan = new System.Windows.Forms.SaveFileDialog();
            this.groupBoxDelSitesPlan = new System.Windows.Forms.GroupBox();
            this.gridControlDbDeletedSite = new DevExpress.XtraGrid.GridControl();
            this.gridViewDbDeletedSite = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnDbExpoprtDelSitesPlan = new System.Windows.Forms.Button();
            this.groupBoxDbDelDeletedSite.SuspendLayout();
            this.groupBoxDbAddDeletedSite.SuspendLayout();
            this.groupBoxDelSitesPlan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDbDeletedSite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDbDeletedSite)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDbDelSelectedDeletedSite
            // 
            this.btnDbDelSelectedDeletedSite.Enabled = false;
            this.btnDbDelSelectedDeletedSite.Location = new System.Drawing.Point(127, 49);
            this.btnDbDelSelectedDeletedSite.Name = "btnDbDelSelectedDeletedSite";
            this.btnDbDelSelectedDeletedSite.Size = new System.Drawing.Size(176, 23);
            this.btnDbDelSelectedDeletedSite.TabIndex = 4;
            this.btnDbDelSelectedDeletedSite.Text = "Delete Selected NewSite";
            this.btnDbDelSelectedDeletedSite.UseVisualStyleBackColor = true;
            this.btnDbDelSelectedDeletedSite.Click += new System.EventHandler(this.BtnDbDelSelectedDeletedSite_Click);
            // 
            // groupBoxDbDelDeletedSite
            // 
            this.groupBoxDbDelDeletedSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxDbDelDeletedSite.Controls.Add(this.btnDbDelAllDeletedSite);
            this.groupBoxDbDelDeletedSite.Controls.Add(this.btnDbDelSelectedDeletedSite);
            this.groupBoxDbDelDeletedSite.Location = new System.Drawing.Point(32, 304);
            this.groupBoxDbDelDeletedSite.Name = "groupBoxDbDelDeletedSite";
            this.groupBoxDbDelDeletedSite.Size = new System.Drawing.Size(460, 444);
            this.groupBoxDbDelDeletedSite.TabIndex = 6;
            this.groupBoxDbDelDeletedSite.TabStop = false;
            this.groupBoxDbDelDeletedSite.Text = "Delete New Site";
            // 
            // btnDbDelAllDeletedSite
            // 
            this.btnDbDelAllDeletedSite.Location = new System.Drawing.Point(128, 87);
            this.btnDbDelAllDeletedSite.Name = "btnDbDelAllDeletedSite";
            this.btnDbDelAllDeletedSite.Size = new System.Drawing.Size(175, 23);
            this.btnDbDelAllDeletedSite.TabIndex = 5;
            this.btnDbDelAllDeletedSite.Text = "Delete All Deleted Site";
            this.btnDbDelAllDeletedSite.UseVisualStyleBackColor = true;
            this.btnDbDelAllDeletedSite.Click += new System.EventHandler(this.BtnDbDelAllDeletedSite_Click);
            // 
            // txtDbDeletedSiteName
            // 
            this.txtDbDeletedSiteName.Location = new System.Drawing.Point(69, 114);
            this.txtDbDeletedSiteName.Name = "txtDbDeletedSiteName";
            this.txtDbDeletedSiteName.Size = new System.Drawing.Size(290, 20);
            this.txtDbDeletedSiteName.TabIndex = 1;
            this.txtDbDeletedSiteName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDbDeletedSiteName.Enter += new System.EventHandler(this.TxtDbDeletedSiteName_Enter);
            // 
            // txtDbDeletedLocation
            // 
            this.txtDbDeletedLocation.Location = new System.Drawing.Point(91, 85);
            this.txtDbDeletedLocation.MaxLength = 5;
            this.txtDbDeletedLocation.Name = "txtDbDeletedLocation";
            this.txtDbDeletedLocation.Size = new System.Drawing.Size(45, 20);
            this.txtDbDeletedLocation.TabIndex = 0;
            this.txtDbDeletedLocation.TextChanged += new System.EventHandler(this.TxtDbDeletedLocation_TextChanged);
            this.txtDbDeletedLocation.Enter += new System.EventHandler(this.TxtDbDeletedLocation_Enter);
            this.txtDbDeletedLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtDbDeletedLocation_KeyPress);
            this.txtDbDeletedLocation.Leave += new System.EventHandler(this.TxtDbDeletedLocation_Leave);
            // 
            // txtEs
            // 
            this.txtEs.Enabled = false;
            this.txtEs.Location = new System.Drawing.Point(69, 85);
            this.txtEs.Name = "txtEs";
            this.txtEs.Size = new System.Drawing.Size(22, 20);
            this.txtEs.TabIndex = 5;
            this.txtEs.Text = "ES";
            // 
            // lblDbDeletedSite
            // 
            this.lblDbDeletedSite.AutoSize = true;
            this.lblDbDeletedSite.Location = new System.Drawing.Point(5, 119);
            this.lblDbDeletedSite.Name = "lblDbDeletedSite";
            this.lblDbDeletedSite.Size = new System.Drawing.Size(62, 13);
            this.lblDbDeletedSite.TabIndex = 4;
            this.lblDbDeletedSite.Text = "Site Name :";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(8, 88);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(50, 13);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "location :";
            // 
            // btnDbAddDeletedSite
            // 
            this.btnDbAddDeletedSite.Location = new System.Drawing.Point(144, 223);
            this.btnDbAddDeletedSite.Name = "btnDbAddDeletedSite";
            this.btnDbAddDeletedSite.Size = new System.Drawing.Size(118, 27);
            this.btnDbAddDeletedSite.TabIndex = 3;
            this.btnDbAddDeletedSite.Text = "ADD Deleted Site";
            this.btnDbAddDeletedSite.UseVisualStyleBackColor = true;
            this.btnDbAddDeletedSite.Click += new System.EventHandler(this.BtnDbAddDeletedSite_Click);
            // 
            // groupBoxDbAddDeletedSite
            // 
            this.groupBoxDbAddDeletedSite.Controls.Add(this.txtDeletedDate);
            this.groupBoxDbAddDeletedSite.Controls.Add(this.btnDeletedDate);
            this.groupBoxDbAddDeletedSite.Controls.Add(this.txtDbDeletedSiteName);
            this.groupBoxDbAddDeletedSite.Controls.Add(this.txtDbDeletedLocation);
            this.groupBoxDbAddDeletedSite.Controls.Add(this.txtEs);
            this.groupBoxDbAddDeletedSite.Controls.Add(this.lblDbDeletedDate);
            this.groupBoxDbAddDeletedSite.Controls.Add(this.lblDbDeletedSite);
            this.groupBoxDbAddDeletedSite.Controls.Add(this.lblLocation);
            this.groupBoxDbAddDeletedSite.Controls.Add(this.btnDbAddDeletedSite);
            this.groupBoxDbAddDeletedSite.Location = new System.Drawing.Point(33, 8);
            this.groupBoxDbAddDeletedSite.Name = "groupBoxDbAddDeletedSite";
            this.groupBoxDbAddDeletedSite.Size = new System.Drawing.Size(460, 279);
            this.groupBoxDbAddDeletedSite.TabIndex = 5;
            this.groupBoxDbAddDeletedSite.TabStop = false;
            this.groupBoxDbAddDeletedSite.Text = "ADD Deleted SITE";
            this.groupBoxDbAddDeletedSite.Enter += new System.EventHandler(this.GroupBoxDbAddDeletedSite_Enter);
            // 
            // txtDeletedDate
            // 
            this.txtDeletedDate.BackColor = System.Drawing.Color.MintCream;
            this.txtDeletedDate.Enabled = false;
            this.txtDeletedDate.Location = new System.Drawing.Point(232, 147);
            this.txtDeletedDate.Name = "txtDeletedDate";
            this.txtDeletedDate.Size = new System.Drawing.Size(96, 20);
            this.txtDeletedDate.TabIndex = 7;
            this.txtDeletedDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnDeletedDate
            // 
            this.btnDeletedDate.Location = new System.Drawing.Point(91, 145);
            this.btnDeletedDate.Name = "btnDeletedDate";
            this.btnDeletedDate.Size = new System.Drawing.Size(129, 23);
            this.btnDeletedDate.TabIndex = 2;
            this.btnDeletedDate.Text = "Deleted Date";
            this.btnDeletedDate.UseVisualStyleBackColor = true;
            this.btnDeletedDate.Click += new System.EventHandler(this.BtnDeletedDate_Click);
            // 
            // lblDbDeletedDate
            // 
            this.lblDbDeletedDate.AutoSize = true;
            this.lblDbDeletedDate.Location = new System.Drawing.Point(5, 150);
            this.lblDbDeletedDate.Name = "lblDbDeletedDate";
            this.lblDbDeletedDate.Size = new System.Drawing.Size(76, 13);
            this.lblDbDeletedDate.TabIndex = 4;
            this.lblDbDeletedDate.Text = "Deleted Date :";
            // 
            // saveFileDialogDbDelSitesPlan
            // 
            this.saveFileDialogDbDelSitesPlan.DefaultExt = "Xlsx";
            this.saveFileDialogDbDelSitesPlan.FileName = "Deleted Sites Plan";
            // 
            // groupBoxDelSitesPlan
            // 
            this.groupBoxDelSitesPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDelSitesPlan.Controls.Add(this.btnDbExpoprtDelSitesPlan);
            this.groupBoxDelSitesPlan.Controls.Add(this.gridControlDbDeletedSite);
            this.groupBoxDelSitesPlan.Location = new System.Drawing.Point(621, 12);
            this.groupBoxDelSitesPlan.Name = "groupBoxDelSitesPlan";
            this.groupBoxDelSitesPlan.Size = new System.Drawing.Size(480, 736);
            this.groupBoxDelSitesPlan.TabIndex = 7;
            this.groupBoxDelSitesPlan.TabStop = false;
            // 
            // gridControlDbDeletedSite
            // 
            this.gridControlDbDeletedSite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode2.RelationName = "Level1";
            this.gridControlDbDeletedSite.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gridControlDbDeletedSite.Location = new System.Drawing.Point(27, 63);
            this.gridControlDbDeletedSite.MainView = this.gridViewDbDeletedSite;
            this.gridControlDbDeletedSite.Name = "gridControlDbDeletedSite";
            this.gridControlDbDeletedSite.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridControlDbDeletedSite.Size = new System.Drawing.Size(426, 655);
            this.gridControlDbDeletedSite.TabIndex = 1;
            this.gridControlDbDeletedSite.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDbDeletedSite});
            // 
            // gridViewDbDeletedSite
            // 
            this.gridViewDbDeletedSite.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewDbDeletedSite.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDbDeletedSite.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewDbDeletedSite.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewDbDeletedSite.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewDbDeletedSite.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewDbDeletedSite.GridControl = this.gridControlDbDeletedSite;
            this.gridViewDbDeletedSite.Name = "gridViewDbDeletedSite";
            this.gridViewDbDeletedSite.OptionsView.ColumnAutoWidth = false;
            this.gridViewDbDeletedSite.OptionsView.ShowAutoFilterRow = true;
            this.gridViewDbDeletedSite.OptionsView.ShowGroupPanel = false;
            // 
            // btnDbExpoprtDelSitesPlan
            // 
            this.btnDbExpoprtDelSitesPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDbExpoprtDelSitesPlan.Location = new System.Drawing.Point(358, 19);
            this.btnDbExpoprtDelSitesPlan.Name = "btnDbExpoprtDelSitesPlan";
            this.btnDbExpoprtDelSitesPlan.Size = new System.Drawing.Size(95, 23);
            this.btnDbExpoprtDelSitesPlan.TabIndex = 2;
            this.btnDbExpoprtDelSitesPlan.Text = "خروجی به اکسل";
            this.btnDbExpoprtDelSitesPlan.UseVisualStyleBackColor = true;
            this.btnDbExpoprtDelSitesPlan.Click += new System.EventHandler(this.btnDbExpoprtDelSitesPlan_Click);
            // 
            // frmDbDeletedSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 774);
            this.Controls.Add(this.groupBoxDelSitesPlan);
            this.Controls.Add(this.groupBoxDbDelDeletedSite);
            this.Controls.Add(this.groupBoxDbAddDeletedSite);
            this.Name = "frmDbDeletedSite";
            this.Text = "objFrmDbDeletedSite";
            this.Load += new System.EventHandler(this.ObjFrmDbDeletedSite_Load);
            this.groupBoxDbDelDeletedSite.ResumeLayout(false);
            this.groupBoxDbAddDeletedSite.ResumeLayout(false);
            this.groupBoxDbAddDeletedSite.PerformLayout();
            this.groupBoxDelSitesPlan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDbDeletedSite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDbDeletedSite)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDbDelSelectedDeletedSite;
        private System.Windows.Forms.GroupBox groupBoxDbDelDeletedSite;
        private System.Windows.Forms.Button btnDbDelAllDeletedSite;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TextBox txtDbDeletedSiteName;
        private System.Windows.Forms.TextBox txtDbDeletedLocation;
        private System.Windows.Forms.TextBox txtEs;
        private System.Windows.Forms.Label lblDbDeletedSite;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button btnDbAddDeletedSite;
        private System.Windows.Forms.GroupBox groupBoxDbAddDeletedSite;
        private System.Windows.Forms.Label lblDbDeletedDate;
        private System.Windows.Forms.Button btnDeletedDate;
        private System.Windows.Forms.TextBox txtDeletedDate;
        private System.Windows.Forms.SaveFileDialog saveFileDialogDbDelSitesPlan;
        private System.Windows.Forms.GroupBox groupBoxDelSitesPlan;
        private DevExpress.XtraGrid.GridControl gridControlDbDeletedSite;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDbDeletedSite;
        private System.Windows.Forms.Button btnDbExpoprtDelSitesPlan;
    }
}