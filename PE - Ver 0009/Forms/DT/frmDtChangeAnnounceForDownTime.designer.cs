namespace PE.Forms.DT
{
    partial class frmDtChangeAnnounceForDownTime
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
            this.lbldtCurrentTextOfAnnounce = new System.Windows.Forms.Label();
            this.lblCurText = new System.Windows.Forms.Label();
            this.lbldtNewTextOfAnnounce = new System.Windows.Forms.Label();
            this.txtNewStrForAnnounce = new System.Windows.Forms.TextBox();
            this.btnChangeannounce = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbldtCurrentTextOfAnnounce
            // 
            this.lbldtCurrentTextOfAnnounce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbldtCurrentTextOfAnnounce.AutoSize = true;
            this.lbldtCurrentTextOfAnnounce.Location = new System.Drawing.Point(117, 51);
            this.lbldtCurrentTextOfAnnounce.Name = "lbldtCurrentTextOfAnnounce";
            this.lbldtCurrentTextOfAnnounce.Size = new System.Drawing.Size(146, 13);
            this.lbldtCurrentTextOfAnnounce.TabIndex = 0;
            this.lbldtCurrentTextOfAnnounce.Text = "متن فعلی اعلان دان تایم جدید :";
            // 
            // lblCurText
            // 
            this.lblCurText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurText.AutoSize = true;
            this.lblCurText.Location = new System.Drawing.Point(290, 50);
            this.lblCurText.Name = "lblCurText";
            this.lblCurText.Size = new System.Drawing.Size(22, 13);
            this.lblCurText.TabIndex = 1;
            this.lblCurText.Text = "-----";
            // 
            // lbldtNewTextOfAnnounce
            // 
            this.lbldtNewTextOfAnnounce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbldtNewTextOfAnnounce.AutoSize = true;
            this.lbldtNewTextOfAnnounce.Location = new System.Drawing.Point(117, 86);
            this.lbldtNewTextOfAnnounce.Name = "lbldtNewTextOfAnnounce";
            this.lbldtNewTextOfAnnounce.Size = new System.Drawing.Size(208, 13);
            this.lbldtNewTextOfAnnounce.TabIndex = 2;
            this.lbldtNewTextOfAnnounce.Text = "متن جدید پیام دان تایم جدید برای جایگزینی :";
            // 
            // txtNewStrForAnnounce
            // 
            this.txtNewStrForAnnounce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewStrForAnnounce.Location = new System.Drawing.Point(334, 83);
            this.txtNewStrForAnnounce.Name = "txtNewStrForAnnounce";
            this.txtNewStrForAnnounce.Size = new System.Drawing.Size(285, 20);
            this.txtNewStrForAnnounce.TabIndex = 3;
            // 
            // btnChangeannounce
            // 
            this.btnChangeannounce.Location = new System.Drawing.Point(346, 135);
            this.btnChangeannounce.Name = "btnChangeannounce";
            this.btnChangeannounce.Size = new System.Drawing.Size(138, 23);
            this.btnChangeannounce.TabIndex = 4;
            this.btnChangeannounce.Text = "اعمال تغییرات";
            this.btnChangeannounce.UseVisualStyleBackColor = true;
            this.btnChangeannounce.Click += new System.EventHandler(this.BtnChangeannounce_Click);
            // 
            // frmDtChangeAnnounceForDownTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 177);
            this.Controls.Add(this.btnChangeannounce);
            this.Controls.Add(this.txtNewStrForAnnounce);
            this.Controls.Add(this.lbldtNewTextOfAnnounce);
            this.Controls.Add(this.lblCurText);
            this.Controls.Add(this.lbldtCurrentTextOfAnnounce);
            this.Name = "frmDtChangeAnnounceForDownTime";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "فرم تغییر پیام دان تایم";
            this.Load += new System.EventHandler(this.FrmDtChangeAnnounceForDownTime_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbldtCurrentTextOfAnnounce;
        private System.Windows.Forms.Label lblCurText;
        private System.Windows.Forms.Label lbldtNewTextOfAnnounce;
        private System.Windows.Forms.TextBox txtNewStrForAnnounce;
        private System.Windows.Forms.Button btnChangeannounce;
    }
}