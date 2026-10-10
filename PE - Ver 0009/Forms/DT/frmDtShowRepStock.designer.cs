namespace PE.Forms.DT
{
    partial class frmDtShowRepStock
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.gridControlDtShowStoksRep = new DevExpress.XtraGrid.GridControl();
            this.gvDtShowStoksRep = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.تغییرسهمیهToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDtShowStoksRep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDtShowStoksRep)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControlDtShowStoksRep
            // 
            this.gridControlDtShowStoksRep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlDtShowStoksRep.Location = new System.Drawing.Point(52, 41);
            this.gridControlDtShowStoksRep.MainView = this.gvDtShowStoksRep;
            this.gridControlDtShowStoksRep.Name = "gridControlDtShowStoksRep";
            this.gridControlDtShowStoksRep.Size = new System.Drawing.Size(471, 343);
            this.gridControlDtShowStoksRep.TabIndex = 0;
            this.gridControlDtShowStoksRep.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDtShowStoksRep});
            // 
            // gvDtShowStoksRep
            // 
            this.gvDtShowStoksRep.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDtShowStoksRep.Appearance.Empty.Options.UseBackColor = true;
            this.gvDtShowStoksRep.Appearance.FocusedCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDtShowStoksRep.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDtShowStoksRep.Appearance.FocusedCell.Options.UseTextOptions = true;
            this.gvDtShowStoksRep.Appearance.FocusedCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDtShowStoksRep.Appearance.FocusedRow.BackColor = System.Drawing.Color.Yellow;
            this.gvDtShowStoksRep.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDtShowStoksRep.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDtShowStoksRep.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDtShowStoksRep.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDtShowStoksRep.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDtShowStoksRep.Appearance.Row.Options.UseTextOptions = true;
            this.gvDtShowStoksRep.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDtShowStoksRep.Appearance.SelectedRow.BackColor = System.Drawing.Color.White;
            this.gvDtShowStoksRep.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDtShowStoksRep.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDtShowStoksRep.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDtShowStoksRep.GridControl = this.gridControlDtShowStoksRep;
            this.gvDtShowStoksRep.Name = "gvDtShowStoksRep";
            this.gvDtShowStoksRep.OptionsView.ShowGroupPanel = false;
            this.gvDtShowStoksRep.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvDtShowStoksRep_RowCellStyle);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.تغییرسهمیهToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(583, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // تغییرسهمیهToolStripMenuItem
            // 
            this.تغییرسهمیهToolStripMenuItem.Name = "تغییرسهمیهToolStripMenuItem";
            this.تغییرسهمیهToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.تغییرسهمیهToolStripMenuItem.Text = "تغییر سهمیه";
            this.تغییرسهمیهToolStripMenuItem.Click += new System.EventHandler(this.تغییرسهمیهToolStripMenuItem_Click);
            // 
            // frmDtShowRepStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 407);
            this.Controls.Add(this.gridControlDtShowStoksRep);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmDtShowRepStock";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "فرم نمایش سهمیه دان تایم تکراری پیمانکاران";
            this.Load += new System.EventHandler(this.FrmDtShowRepStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDtShowStoksRep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDtShowStoksRep)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DevExpress.XtraGrid.GridControl gridControlDtShowStoksRep;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDtShowStoksRep;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem تغییرسهمیهToolStripMenuItem;
    }
}