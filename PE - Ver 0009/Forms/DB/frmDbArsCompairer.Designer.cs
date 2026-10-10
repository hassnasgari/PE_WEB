namespace PE.Forms.DB
{
    partial class frmDbArsCompairer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDbArsCompairer));
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridControlOnlyNew = new DevExpress.XtraGrid.GridControl();
            this.gridViewOnlyNew = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControlNew = new DevExpress.XtraGrid.GridControl();
            this.gridViewNew = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridControlOnlyOld = new DevExpress.XtraGrid.GridControl();
            this.gridViewOnlyOld = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControlOld = new DevExpress.XtraGrid.GridControl();
            this.gridViewOld = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControlDiff = new DevExpress.XtraGrid.GridControl();
            this.gridViewDiff = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuStripCompDbAras = new System.Windows.Forms.MenuStrip();
            this.compairForNameOrAddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compairForCodeSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOnlyNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOnlyNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewNew)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOnlyOld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOnlyOld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiff)).BeginInit();
            this.menuStripCompDbAras.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(17, 34);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(35, 13);
            this.labelStatus.TabIndex = 1;
            this.labelStatus.Text = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridControlOnlyNew);
            this.groupBox1.Controls.Add(this.gridControlNew);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(15, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(618, 348);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New";
            // 
            // gridControlOnlyNew
            // 
            this.gridControlOnlyNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlOnlyNew.Location = new System.Drawing.Point(365, 22);
            this.gridControlOnlyNew.MainView = this.gridViewOnlyNew;
            this.gridControlOnlyNew.Name = "gridControlOnlyNew";
            this.gridControlOnlyNew.Size = new System.Drawing.Size(242, 320);
            this.gridControlOnlyNew.TabIndex = 8;
            this.gridControlOnlyNew.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewOnlyNew});
            // 
            // gridViewOnlyNew
            // 
            this.gridViewOnlyNew.Appearance.ViewCaption.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewOnlyNew.Appearance.ViewCaption.Options.UseFont = true;
            this.gridViewOnlyNew.GridControl = this.gridControlOnlyNew;
            this.gridViewOnlyNew.Name = "gridViewOnlyNew";
            this.gridViewOnlyNew.OptionsView.ShowFooter = true;
            this.gridViewOnlyNew.OptionsView.ShowGroupPanel = false;
            this.gridViewOnlyNew.OptionsView.ShowViewCaption = true;
            this.gridViewOnlyNew.ViewCaption = "کدهایی که فقط در ارس جدید هست";
            // 
            // gridControlNew
            // 
            this.gridControlNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridControlNew.Location = new System.Drawing.Point(5, 22);
            this.gridControlNew.MainView = this.gridViewNew;
            this.gridControlNew.Name = "gridControlNew";
            this.gridControlNew.Size = new System.Drawing.Size(354, 320);
            this.gridControlNew.TabIndex = 7;
            this.gridControlNew.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewNew});
            // 
            // gridViewNew
            // 
            this.gridViewNew.Appearance.ViewCaption.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewNew.Appearance.ViewCaption.Options.UseFont = true;
            this.gridViewNew.GridControl = this.gridControlNew;
            this.gridViewNew.Name = "gridViewNew";
            this.gridViewNew.OptionsView.ShowFooter = true;
            this.gridViewNew.OptionsView.ShowGroupPanel = false;
            this.gridViewNew.OptionsView.ShowViewCaption = true;
            this.gridViewNew.ViewCaption = "ارس جدید";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.gridControlOnlyOld);
            this.groupBox2.Controls.Add(this.gridControlOld);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(651, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(637, 348);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Old";
            // 
            // gridControlOnlyOld
            // 
            this.gridControlOnlyOld.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlOnlyOld.Location = new System.Drawing.Point(387, 22);
            this.gridControlOnlyOld.MainView = this.gridViewOnlyOld;
            this.gridControlOnlyOld.Name = "gridControlOnlyOld";
            this.gridControlOnlyOld.Size = new System.Drawing.Size(244, 320);
            this.gridControlOnlyOld.TabIndex = 9;
            this.gridControlOnlyOld.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewOnlyOld});
            // 
            // gridViewOnlyOld
            // 
            this.gridViewOnlyOld.Appearance.ViewCaption.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewOnlyOld.Appearance.ViewCaption.Options.UseFont = true;
            this.gridViewOnlyOld.GridControl = this.gridControlOnlyOld;
            this.gridViewOnlyOld.Name = "gridViewOnlyOld";
            this.gridViewOnlyOld.OptionsView.ShowFooter = true;
            this.gridViewOnlyOld.OptionsView.ShowGroupPanel = false;
            this.gridViewOnlyOld.OptionsView.ShowViewCaption = true;
            this.gridViewOnlyOld.ViewCaption = "کدهایی که فقط در ارس قدیمی هست";
            // 
            // gridControlOld
            // 
            this.gridControlOld.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridControlOld.Location = new System.Drawing.Point(6, 22);
            this.gridControlOld.MainView = this.gridViewOld;
            this.gridControlOld.Name = "gridControlOld";
            this.gridControlOld.Size = new System.Drawing.Size(375, 320);
            this.gridControlOld.TabIndex = 8;
            this.gridControlOld.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewOld});
            // 
            // gridViewOld
            // 
            this.gridViewOld.Appearance.ViewCaption.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewOld.Appearance.ViewCaption.Options.UseFont = true;
            this.gridViewOld.GridControl = this.gridControlOld;
            this.gridViewOld.Name = "gridViewOld";
            this.gridViewOld.OptionsView.ShowFooter = true;
            this.gridViewOld.OptionsView.ShowGroupPanel = false;
            this.gridViewOld.OptionsView.ShowViewCaption = true;
            this.gridViewOld.ViewCaption = "ارس قدیمی ";
            // 
            // gridControlDiff
            // 
            this.gridControlDiff.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlDiff.Location = new System.Drawing.Point(20, 439);
            this.gridControlDiff.MainView = this.gridViewDiff;
            this.gridControlDiff.Name = "gridControlDiff";
            this.gridControlDiff.Size = new System.Drawing.Size(1277, 175);
            this.gridControlDiff.TabIndex = 11;
            this.gridControlDiff.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDiff});
            // 
            // gridViewDiff
            // 
            this.gridViewDiff.GridControl = this.gridControlDiff;
            this.gridViewDiff.Name = "gridViewDiff";
            this.gridViewDiff.OptionsView.ShowFooter = true;
            this.gridViewDiff.OptionsView.ShowGroupPanel = false;
            this.gridViewDiff.OptionsView.ShowViewCaption = true;
            this.gridViewDiff.ViewCaption = "مغایرت نام یا آدرس";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 24);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1318, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuStripCompDbAras
            // 
            this.menuStripCompDbAras.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compairForNameOrAddToolStripMenuItem,
            this.compairForCodeSiteToolStripMenuItem});
            this.menuStripCompDbAras.Location = new System.Drawing.Point(0, 0);
            this.menuStripCompDbAras.Name = "menuStripCompDbAras";
            this.menuStripCompDbAras.Size = new System.Drawing.Size(1318, 24);
            this.menuStripCompDbAras.TabIndex = 13;
            this.menuStripCompDbAras.Text = "menuStrip2";
            // 
            // compairForNameOrAddToolStripMenuItem
            // 
            this.compairForNameOrAddToolStripMenuItem.Name = "compairForNameOrAddToolStripMenuItem";
            this.compairForNameOrAddToolStripMenuItem.Size = new System.Drawing.Size(190, 20);
            this.compairForNameOrAddToolStripMenuItem.Text = "Compair For Name And Address";
            this.compairForNameOrAddToolStripMenuItem.Click += new System.EventHandler(this.compairForNameOrAddToolStripMenuItem_Click);
            // 
            // compairForCodeSiteToolStripMenuItem
            // 
            this.compairForCodeSiteToolStripMenuItem.Name = "compairForCodeSiteToolStripMenuItem";
            this.compairForCodeSiteToolStripMenuItem.Size = new System.Drawing.Size(138, 20);
            this.compairForCodeSiteToolStripMenuItem.Text = "Compair For Code Site";
            this.compairForCodeSiteToolStripMenuItem.Click += new System.EventHandler(this.compairForCodeSiteToolStripMenuItem_Click);
            // 
            // frmDbArsCompairer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1318, 626);
            this.Controls.Add(this.gridControlDiff);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.menuStripCompDbAras);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmDbArsCompairer";
            this.Text = "Compait Two Aras Files";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmDbArsCompairer_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOnlyNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOnlyNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewNew)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOnlyOld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOnlyOld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiff)).EndInit();
            this.menuStripCompDbAras.ResumeLayout(false);
            this.menuStripCompDbAras.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gridControlOnlyNew;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewOnlyNew;
        private DevExpress.XtraGrid.GridControl gridControlNew;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewNew;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gridControlOnlyOld;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewOnlyOld;
        private DevExpress.XtraGrid.GridControl gridControlOld;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewOld;
        private DevExpress.XtraGrid.GridControl gridControlDiff;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDiff;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.MenuStrip menuStripCompDbAras;
        private System.Windows.Forms.ToolStripMenuItem compairForNameOrAddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compairForCodeSiteToolStripMenuItem;
    }
}