
namespace PE.Forms.DT
{
    partial class frmDtSetTextAboveGrid
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtDtUnfreez = new System.Windows.Forms.TextBox();
            this.menuSetAboveGrid = new System.Windows.Forms.MenuStrip();
            this.تنظیماتپیامToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.رنگپیامToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialogSetTextAboveGrid = new System.Windows.Forms.ColorDialog();
            this.menuSetAboveGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(569, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "ثبت";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtDtUnfreez
            // 
            this.txtDtUnfreez.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtUnfreez.BackColor = System.Drawing.Color.White;
            this.txtDtUnfreez.Location = new System.Drawing.Point(12, 28);
            this.txtDtUnfreez.Multiline = true;
            this.txtDtUnfreez.Name = "txtDtUnfreez";
            this.txtDtUnfreez.Size = new System.Drawing.Size(1152, 95);
            this.txtDtUnfreez.TabIndex = 2;
            // 
            // menuSetAboveGrid
            // 
            this.menuSetAboveGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.تنظیماتپیامToolStripMenuItem});
            this.menuSetAboveGrid.Location = new System.Drawing.Point(0, 0);
            this.menuSetAboveGrid.Name = "menuSetAboveGrid";
            this.menuSetAboveGrid.Size = new System.Drawing.Size(1176, 24);
            this.menuSetAboveGrid.TabIndex = 3;
            this.menuSetAboveGrid.Text = "menuStrip1";
            // 
            // تنظیماتپیامToolStripMenuItem
            // 
            this.تنظیماتپیامToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.رنگپیامToolStripMenuItem});
            this.تنظیماتپیامToolStripMenuItem.Name = "تنظیماتپیامToolStripMenuItem";
            this.تنظیماتپیامToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.تنظیماتپیامToolStripMenuItem.Text = "تنظیمات پیام";
            this.تنظیماتپیامToolStripMenuItem.Click += new System.EventHandler(this.تنظیماتپیامToolStripMenuItem_Click);
            // 
            // رنگپیامToolStripMenuItem
            // 
            this.رنگپیامToolStripMenuItem.Name = "رنگپیامToolStripMenuItem";
            this.رنگپیامToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.رنگپیامToolStripMenuItem.Text = "رنگ پیام";
            this.رنگپیامToolStripMenuItem.Click += new System.EventHandler(this.رنگپیامToolStripMenuItem_Click);
            // 
            // frmDtSetTextAboveGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 222);
            this.Controls.Add(this.txtDtUnfreez);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuSetAboveGrid);
            this.MainMenuStrip = this.menuSetAboveGrid;
            this.Name = "frmDtSetTextAboveGrid";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "فرم تنظیم متن بالای جدول دان تایمها برای حالتی که شبکه فریز نیست";
            this.Load += new System.EventHandler(this.frmDtSetTextAboveGrid_Load);
            this.menuSetAboveGrid.ResumeLayout(false);
            this.menuSetAboveGrid.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtDtUnfreez;
        private System.Windows.Forms.MenuStrip menuSetAboveGrid;
        private System.Windows.Forms.ToolStripMenuItem تنظیماتپیامToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem رنگپیامToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialogSetTextAboveGrid;
    }
}