
namespace PE.Forms.DT
{
    partial class frmDtChgRepStock
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
            this.cbxRegion = new System.Windows.Forms.ComboBox();
            this.lblRegion = new System.Windows.Forms.Label();
            this.numRepStockUpDn = new System.Windows.Forms.NumericUpDown();
            this.lblStock = new System.Windows.Forms.Label();
            this.btnChgRepStock = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numRepStockUpDn)).BeginInit();
            this.SuspendLayout();
            // 
            // cbxRegion
            // 
            this.cbxRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxRegion.FormattingEnabled = true;
            this.cbxRegion.Location = new System.Drawing.Point(178, 44);
            this.cbxRegion.Name = "cbxRegion";
            this.cbxRegion.Size = new System.Drawing.Size(121, 21);
            this.cbxRegion.TabIndex = 0;
            this.cbxRegion.SelectedIndexChanged += new System.EventHandler(this.cbxRegion_SelectedIndexChanged);
            // 
            // lblRegion
            // 
            this.lblRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(133, 47);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(39, 13);
            this.lblRegion.TabIndex = 2;
            this.lblRegion.Text = "منطقه :";
            // 
            // numRepStockUpDn
            // 
            this.numRepStockUpDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numRepStockUpDn.Location = new System.Drawing.Point(424, 45);
            this.numRepStockUpDn.Name = "numRepStockUpDn";
            this.numRepStockUpDn.Size = new System.Drawing.Size(120, 20);
            this.numRepStockUpDn.TabIndex = 3;
            this.numRepStockUpDn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numRepStockUpDn.ValueChanged += new System.EventHandler(this.numRepStockUpDn_ValueChanged);
            // 
            // lblStock
            // 
            this.lblStock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStock.AutoSize = true;
            this.lblStock.Location = new System.Drawing.Point(364, 47);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(42, 13);
            this.lblStock.TabIndex = 4;
            this.lblStock.Text = "سهمیه :";
            // 
            // btnChgRepStock
            // 
            this.btnChgRepStock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChgRepStock.AutoSize = true;
            this.btnChgRepStock.Location = new System.Drawing.Point(605, 42);
            this.btnChgRepStock.Name = "btnChgRepStock";
            this.btnChgRepStock.Size = new System.Drawing.Size(159, 23);
            this.btnChgRepStock.TabIndex = 5;
            this.btnChgRepStock.Text = "اعمال تغییرات";
            this.btnChgRepStock.UseVisualStyleBackColor = true;
            this.btnChgRepStock.Click += new System.EventHandler(this.btnChgRepStock_Click);
            // 
            // frmDtChgRepStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 107);
            this.Controls.Add(this.btnChgRepStock);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.numRepStockUpDn);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.cbxRegion);
            this.Name = "frmDtChgRepStock";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "فرم تغییر سهمیه دان تایم تکراری ناحیه ها";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDtChgRepStock_FormClosed);
            this.Load += new System.EventHandler(this.frmDtChgRepStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numRepStockUpDn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxRegion;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.NumericUpDown numRepStockUpDn;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.Button btnChgRepStock;
    }
}