
namespace PE.Forms.DT
{
    partial class frmDtCteateAttachment
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
            this.menuStripDtAttach = new System.Windows.Forms.MenuStrip();
            this.خواندنفایلضمیمهاصلیToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialogDtAttach = new System.Windows.Forms.OpenFileDialog();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gcDtAttach = new DevExpress.XtraGrid.GridControl();
            this.gvDtAttach = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtCodeSite = new System.Windows.Forms.TextBox();
            this.lblAttach = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblCnt = new System.Windows.Forms.Label();
            this.menuStripDtAttach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDtAttach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDtAttach)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripDtAttach
            // 
            this.menuStripDtAttach.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.خواندنفایلضمیمهاصلیToolStripMenuItem});
            this.menuStripDtAttach.Location = new System.Drawing.Point(0, 0);
            this.menuStripDtAttach.Name = "menuStripDtAttach";
            this.menuStripDtAttach.Size = new System.Drawing.Size(770, 24);
            this.menuStripDtAttach.TabIndex = 0;
            this.menuStripDtAttach.Text = "menuStrip1";
            // 
            // خواندنفایلضمیمهاصلیToolStripMenuItem
            // 
            this.خواندنفایلضمیمهاصلیToolStripMenuItem.Name = "خواندنفایلضمیمهاصلیToolStripMenuItem";
            this.خواندنفایلضمیمهاصلیToolStripMenuItem.Size = new System.Drawing.Size(148, 20);
            this.خواندنفایلضمیمهاصلیToolStripMenuItem.Text = "خواندن فایل ضمیمه اصلی";
            this.خواندنفایلضمیمهاصلیToolStripMenuItem.Click += new System.EventHandler(this.خواندنفایلضمیمهاصلیToolStripMenuItem_Click);
            // 
            // openFileDialogDtAttach
            // 
            this.openFileDialogDtAttach.DefaultExt = "Xls,Xlsx";
            this.openFileDialogDtAttach.FileName = "Database For CR";
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportToExcel.Location = new System.Drawing.Point(280, 913);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(181, 35);
            this.btnExportToExcel.TabIndex = 8;
            this.btnExportToExcel.Text = "Export To Excel";
            this.btnExportToExcel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, -126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "paste Here For filter";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, -147);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(666, 51);
            this.textBox1.TabIndex = 5;
            // 
            // gcDtAttach
            // 
            this.gcDtAttach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDtAttach.Location = new System.Drawing.Point(17, 141);
            this.gcDtAttach.MainView = this.gvDtAttach;
            this.gcDtAttach.Name = "gcDtAttach";
            this.gcDtAttach.Size = new System.Drawing.Size(737, 601);
            this.gcDtAttach.TabIndex = 11;
            this.gcDtAttach.UseEmbeddedNavigator = true;
            this.gcDtAttach.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDtAttach});
            // 
            // gvDtAttach
            // 
            this.gvDtAttach.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDtAttach.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDtAttach.Appearance.Row.Options.UseTextOptions = true;
            this.gvDtAttach.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDtAttach.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDtAttach.GridControl = this.gcDtAttach;
            this.gvDtAttach.Name = "gvDtAttach";
            this.gvDtAttach.OptionsView.ShowErrorPanel = DevExpress.Utils.DefaultBoolean.False;
            // 
            // txtCodeSite
            // 
            this.txtCodeSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCodeSite.Enabled = false;
            this.txtCodeSite.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtCodeSite.Location = new System.Drawing.Point(17, 70);
            this.txtCodeSite.Multiline = true;
            this.txtCodeSite.Name = "txtCodeSite";
            this.txtCodeSite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCodeSite.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCodeSite.Size = new System.Drawing.Size(737, 51);
            this.txtCodeSite.TabIndex = 9;
            this.txtCodeSite.TextChanged += new System.EventHandler(this.txtCodeSite_TextChanged);
            // 
            // lblAttach
            // 
            this.lblAttach.AutoSize = true;
            this.lblAttach.Location = new System.Drawing.Point(19, 45);
            this.lblAttach.Name = "lblAttach";
            this.lblAttach.Size = new System.Drawing.Size(108, 13);
            this.lblAttach.TabIndex = 10;
            this.lblAttach.Text = "کد سایت های منتخب :";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(281, 753);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 35);
            this.button1.TabIndex = 12;
            this.button1.Text = "Export To Excel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblCnt
            // 
            this.lblCnt.AutoSize = true;
            this.lblCnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.lblCnt.ForeColor = System.Drawing.Color.Violet;
            this.lblCnt.Location = new System.Drawing.Point(123, 45);
            this.lblCnt.Name = "lblCnt";
            this.lblCnt.Size = new System.Drawing.Size(25, 13);
            this.lblCnt.TabIndex = 13;
            this.lblCnt.Text = "------";
            // 
            // frmDtCteateAttachment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 800);
            this.Controls.Add(this.lblCnt);
            this.Controls.Add(this.gcDtAttach);
            this.Controls.Add(this.txtCodeSite);
            this.Controls.Add(this.lblAttach);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnExportToExcel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStripDtAttach);
            this.MainMenuStrip = this.menuStripDtAttach;
            this.Name = "frmDtCteateAttachment";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "فرم ایجاد ضمیمه دان تایم";
            this.Load += new System.EventHandler(this.frmDtCteateAttachment_Load);
            this.menuStripDtAttach.ResumeLayout(false);
            this.menuStripDtAttach.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDtAttach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDtAttach)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripDtAttach;
        private System.Windows.Forms.ToolStripMenuItem خواندنفایلضمیمهاصلیToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialogDtAttach;
        private System.Windows.Forms.Button btnExportToExcel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private DevExpress.XtraGrid.GridControl gcDtAttach;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDtAttach;
        private System.Windows.Forms.TextBox txtCodeSite;
        private System.Windows.Forms.Label lblAttach;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblCnt;
    }
}