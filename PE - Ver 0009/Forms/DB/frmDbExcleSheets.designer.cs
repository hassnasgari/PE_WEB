namespace PE.Forms.DB
{
    partial class frmDbExcleSheets
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.لیستشیتهایقابلانتظارToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.تغییرلیستشیتهایقابلانتظارToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.حذفشیتهایاضافهازاکسلToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lstExceptedSheets = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.لیستشیتهایقابلانتظارToolStripMenuItem,
            this.تغییرلیستشیتهایقابلانتظارToolStripMenuItem,
            this.حذفشیتهایاضافهازاکسلToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1128, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // لیستشیتهایقابلانتظارToolStripMenuItem
            // 
            this.لیستشیتهایقابلانتظارToolStripMenuItem.Name = "لیستشیتهایقابلانتظارToolStripMenuItem";
            this.لیستشیتهایقابلانتظارToolStripMenuItem.Size = new System.Drawing.Size(143, 20);
            this.لیستشیتهایقابلانتظارToolStripMenuItem.Text = "لیست شیتهای قابل انتظار";
            this.لیستشیتهایقابلانتظارToolStripMenuItem.Click += new System.EventHandler(this.لیستشیتهایقابلانتظارToolStripMenuItem_Click);
            // 
            // تغییرلیستشیتهایقابلانتظارToolStripMenuItem
            // 
            this.تغییرلیستشیتهایقابلانتظارToolStripMenuItem.Name = "تغییرلیستشیتهایقابلانتظارToolStripMenuItem";
            this.تغییرلیستشیتهایقابلانتظارToolStripMenuItem.Size = new System.Drawing.Size(170, 20);
            this.تغییرلیستشیتهایقابلانتظارToolStripMenuItem.Text = "تغییر لیست شیتهای قابل انتظار";
            // 
            // حذفشیتهایاضافهازاکسلToolStripMenuItem
            // 
            this.حذفشیتهایاضافهازاکسلToolStripMenuItem.Name = "حذفشیتهایاضافهازاکسلToolStripMenuItem";
            this.حذفشیتهایاضافهازاکسلToolStripMenuItem.Size = new System.Drawing.Size(210, 20);
            this.حذفشیتهایاضافهازاکسلToolStripMenuItem.Text = "حذف شیتهای غیر از انتظار از فایل اکسل";
            this.حذفشیتهایاضافهازاکسلToolStripMenuItem.Click += new System.EventHandler(this.حذفشیتهایاضافهازاکسلToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // lstExceptedSheets
            // 
            this.lstExceptedSheets.FormattingEnabled = true;
            this.lstExceptedSheets.Location = new System.Drawing.Point(12, 46);
            this.lstExceptedSheets.Name = "lstExceptedSheets";
            this.lstExceptedSheets.Size = new System.Drawing.Size(120, 550);
            this.lstExceptedSheets.TabIndex = 2;
            // 
            // frbDbExcleSheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 719);
            this.Controls.Add(this.lstExceptedSheets);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frbDbExcleSheets";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "frbDbExcleSheets";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem لیستشیتهایقابلانتظارToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem تغییرلیستشیتهایقابلانتظارToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem حذفشیتهایاضافهازاکسلToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ListBox lstExceptedSheets;
    }
}