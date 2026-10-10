namespace PE
{
    partial class frmSeEmReprotBeetweenTwoDate
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
            this.groupBoxSeEmRepBetWeenTwoDate = new System.Windows.Forms.GroupBox();
            this.txtToDateSeEm = new System.Windows.Forms.TextBox();
            this.btnTo = new System.Windows.Forms.Button();
            this.txtFromDateSeEm = new System.Windows.Forms.TextBox();
            this.btnFrom = new System.Windows.Forms.Button();
            this.btnCreateSeEmReport = new System.Windows.Forms.Button();
            this.groupBoxSeEmRepBetWeenTwoDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSeEmRepBetWeenTwoDate
            // 
            this.groupBoxSeEmRepBetWeenTwoDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSeEmRepBetWeenTwoDate.Controls.Add(this.txtToDateSeEm);
            this.groupBoxSeEmRepBetWeenTwoDate.Controls.Add(this.btnTo);
            this.groupBoxSeEmRepBetWeenTwoDate.Controls.Add(this.txtFromDateSeEm);
            this.groupBoxSeEmRepBetWeenTwoDate.Controls.Add(this.btnFrom);
            this.groupBoxSeEmRepBetWeenTwoDate.Location = new System.Drawing.Point(12, 11);
            this.groupBoxSeEmRepBetWeenTwoDate.Name = "groupBoxSeEmRepBetWeenTwoDate";
            this.groupBoxSeEmRepBetWeenTwoDate.Size = new System.Drawing.Size(500, 75);
            this.groupBoxSeEmRepBetWeenTwoDate.TabIndex = 0;
            this.groupBoxSeEmRepBetWeenTwoDate.TabStop = false;
            this.groupBoxSeEmRepBetWeenTwoDate.Text = "لطفا بازه زمانی جهت گزارش ها را مشخص کنید";
            // 
            // txtToDateSeEm
            // 
            this.txtToDateSeEm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToDateSeEm.Enabled = false;
            this.txtToDateSeEm.Location = new System.Drawing.Point(16, 31);
            this.txtToDateSeEm.Name = "txtToDateSeEm";
            this.txtToDateSeEm.Size = new System.Drawing.Size(89, 20);
            this.txtToDateSeEm.TabIndex = 3;
            this.txtToDateSeEm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtToDateSeEm.Leave += new System.EventHandler(this.TxtToDateSeEm_Leave);
            // 
            // btnTo
            // 
            this.btnTo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTo.Location = new System.Drawing.Point(120, 29);
            this.btnTo.Name = "btnTo";
            this.btnTo.Size = new System.Drawing.Size(75, 23);
            this.btnTo.TabIndex = 2;
            this.btnTo.Text = "تا تاریخ";
            this.btnTo.UseVisualStyleBackColor = true;
            this.btnTo.Click += new System.EventHandler(this.BtnTo_Click);
            // 
            // txtFromDateSeEm
            // 
            this.txtFromDateSeEm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFromDateSeEm.Enabled = false;
            this.txtFromDateSeEm.Location = new System.Drawing.Point(315, 32);
            this.txtFromDateSeEm.Name = "txtFromDateSeEm";
            this.txtFromDateSeEm.Size = new System.Drawing.Size(84, 20);
            this.txtFromDateSeEm.TabIndex = 1;
            this.txtFromDateSeEm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFromDateSeEm.TextChanged += new System.EventHandler(this.txtFromDateSeEm_TextChanged);
            this.txtFromDateSeEm.Leave += new System.EventHandler(this.TxtFromDateSeEm_Leave);
            // 
            // btnFrom
            // 
            this.btnFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFrom.Location = new System.Drawing.Point(405, 30);
            this.btnFrom.Name = "btnFrom";
            this.btnFrom.Size = new System.Drawing.Size(75, 23);
            this.btnFrom.TabIndex = 0;
            this.btnFrom.Text = "از تاریخ";
            this.btnFrom.UseVisualStyleBackColor = true;
            this.btnFrom.Click += new System.EventHandler(this.BtnFrom_Click);
            // 
            // btnCreateSeEmReport
            // 
            this.btnCreateSeEmReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateSeEmReport.Enabled = false;
            this.btnCreateSeEmReport.Location = new System.Drawing.Point(204, 92);
            this.btnCreateSeEmReport.Name = "btnCreateSeEmReport";
            this.btnCreateSeEmReport.Size = new System.Drawing.Size(103, 34);
            this.btnCreateSeEmReport.TabIndex = 1;
            this.btnCreateSeEmReport.Text = "ایجاد گزارش";
            this.btnCreateSeEmReport.UseVisualStyleBackColor = true;
            this.btnCreateSeEmReport.Click += new System.EventHandler(this.BtnCreateSeEmReport_Click);
            // 
            // frmSeEmReprotBeetweenTwoDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 130);
            this.Controls.Add(this.btnCreateSeEmReport);
            this.Controls.Add(this.groupBoxSeEmRepBetWeenTwoDate);
            this.Name = "frmSeEmReprotBeetweenTwoDate";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "گزارش بین دو بازه زمانی";
            this.Load += new System.EventHandler(this.FrmSeEmReprotBeetweenTwoDate_Load);
            this.groupBoxSeEmRepBetWeenTwoDate.ResumeLayout(false);
            this.groupBoxSeEmRepBetWeenTwoDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSeEmRepBetWeenTwoDate;
        private System.Windows.Forms.Button btnFrom;
        private System.Windows.Forms.TextBox txtFromDateSeEm;
        private System.Windows.Forms.TextBox txtToDateSeEm;
        private System.Windows.Forms.Button btnTo;
        private System.Windows.Forms.Button btnCreateSeEmReport;
    }
}