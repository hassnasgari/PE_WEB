namespace PE.Forms.PE
{
    partial class Vpns
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
            this.menuStripVpns = new System.Windows.Forms.MenuStrip();
            this.مدیریتVpnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.اضافهکردنبهلیستموردنظارتToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.حذفموردانتخابشدهازلیستنظارتToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.بهروزرسانیجدولToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridControlVpnExistList = new DevExpress.XtraGrid.GridControl();
            this.gridViewVpnExistList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.menuStripVpns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlVpnExistList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewVpnExistList)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripVpns
            // 
            this.menuStripVpns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.مدیریتVpnsToolStripMenuItem});
            this.menuStripVpns.Location = new System.Drawing.Point(0, 0);
            this.menuStripVpns.Name = "menuStripVpns";
            this.menuStripVpns.Size = new System.Drawing.Size(338, 24);
            this.menuStripVpns.TabIndex = 1;
            this.menuStripVpns.Text = "menuStrip1";
            // 
            // مدیریتVpnsToolStripMenuItem
            // 
            this.مدیریتVpnsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.اضافهکردنبهلیستموردنظارتToolStripMenuItem,
            this.حذفموردانتخابشدهازلیستنظارتToolStripMenuItem,
            this.بهروزرسانیجدولToolStripMenuItem});
            this.مدیریتVpnsToolStripMenuItem.Name = "مدیریتVpnsToolStripMenuItem";
            this.مدیریتVpnsToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.مدیریتVpnsToolStripMenuItem.Text = "مدیریت Vpns";
            // 
            // اضافهکردنبهلیستموردنظارتToolStripMenuItem
            // 
            this.اضافهکردنبهلیستموردنظارتToolStripMenuItem.Name = "اضافهکردنبهلیستموردنظارتToolStripMenuItem";
            this.اضافهکردنبهلیستموردنظارتToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.اضافهکردنبهلیستموردنظارتToolStripMenuItem.Text = "اضافه کردن به لیست مورد نظارت ";
            this.اضافهکردنبهلیستموردنظارتToolStripMenuItem.Click += new System.EventHandler(this.اضافهکردنبهلیستموردنظارتToolStripMenuItem_Click);
            // 
            // حذفموردانتخابشدهازلیستنظارتToolStripMenuItem
            // 
            this.حذفموردانتخابشدهازلیستنظارتToolStripMenuItem.Name = "حذفموردانتخابشدهازلیستنظارتToolStripMenuItem";
            this.حذفموردانتخابشدهازلیستنظارتToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.حذفموردانتخابشدهازلیستنظارتToolStripMenuItem.Text = "حذف مورد انتخاب شده از لیست نظارت";
            this.حذفموردانتخابشدهازلیستنظارتToolStripMenuItem.Click += new System.EventHandler(this.حذفموردانتخابشدهازلیستنظارتToolStripMenuItem_Click);
            // 
            // بهروزرسانیجدولToolStripMenuItem
            // 
            this.بهروزرسانیجدولToolStripMenuItem.Name = "بهروزرسانیجدولToolStripMenuItem";
            this.بهروزرسانیجدولToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.بهروزرسانیجدولToolStripMenuItem.Text = "بروز رسانی جدول";
            this.بهروزرسانیجدولToolStripMenuItem.Click += new System.EventHandler(this.بهروزرسانیجدولToolStripMenuItem_Click);
            // 
            // gridControlVpnExistList
            // 
            this.gridControlVpnExistList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlVpnExistList.Location = new System.Drawing.Point(12, 53);
            this.gridControlVpnExistList.MainView = this.gridViewVpnExistList;
            this.gridControlVpnExistList.Name = "gridControlVpnExistList";
            this.gridControlVpnExistList.Size = new System.Drawing.Size(316, 379);
            this.gridControlVpnExistList.TabIndex = 3;
            this.gridControlVpnExistList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewVpnExistList});
            // 
            // gridViewVpnExistList
            // 
            this.gridViewVpnExistList.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewVpnExistList.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewVpnExistList.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewVpnExistList.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewVpnExistList.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewVpnExistList.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewVpnExistList.GridControl = this.gridControlVpnExistList;
            this.gridViewVpnExistList.Name = "gridViewVpnExistList";
            this.gridViewVpnExistList.OptionsBehavior.Editable = false;
            this.gridViewVpnExistList.OptionsView.ShowGroupPanel = false;
            // 
            // Vpns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 444);
            this.Controls.Add(this.gridControlVpnExistList);
            this.Controls.Add(this.menuStripVpns);
            this.MainMenuStrip = this.menuStripVpns;
            this.Name = "Vpns";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vpns";
            this.Load += new System.EventHandler(this.Vpns_Load);
            this.menuStripVpns.ResumeLayout(false);
            this.menuStripVpns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlVpnExistList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewVpnExistList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStripVpns;
        private System.Windows.Forms.ToolStripMenuItem مدیریتVpnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem اضافهکردنبهلیستموردنظارتToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem حذفموردانتخابشدهازلیستنظارتToolStripMenuItem;
        private DevExpress.XtraGrid.GridControl gridControlVpnExistList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewVpnExistList;
        private System.Windows.Forms.ToolStripMenuItem بهروزرسانیجدولToolStripMenuItem;
    }
}