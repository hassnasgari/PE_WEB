namespace PE.Forms
{
    partial class frmVpnsAdd
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
            this.txtVpnForAdd = new System.Windows.Forms.TextBox();
            this.btnAddVpn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVpnOwner = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtVpnForAdd
            // 
            this.txtVpnForAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVpnForAdd.Location = new System.Drawing.Point(243, 67);
            this.txtVpnForAdd.Name = "txtVpnForAdd";
            this.txtVpnForAdd.Size = new System.Drawing.Size(59, 20);
            this.txtVpnForAdd.TabIndex = 0;
            this.txtVpnForAdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAddVpn
            // 
            this.btnAddVpn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddVpn.Location = new System.Drawing.Point(139, 144);
            this.btnAddVpn.Name = "btnAddVpn";
            this.btnAddVpn.Size = new System.Drawing.Size(140, 23);
            this.btnAddVpn.TabIndex = 1;
            this.btnAddVpn.Text = "اضافه کردن به فرم نظارت";
            this.btnAddVpn.UseVisualStyleBackColor = true;
            this.btnAddVpn.Click += new System.EventHandler(this.btnAddVpn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "لطفا اطلاعات مورد نظر را وارد کنید و کلید اضافه کردن به فرم نظارت را بفشارید";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "نام VPN :";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(325, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "نام مالک :";
            // 
            // txtVpnOwner
            // 
            this.txtVpnOwner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVpnOwner.Location = new System.Drawing.Point(130, 93);
            this.txtVpnOwner.Name = "txtVpnOwner";
            this.txtVpnOwner.Size = new System.Drawing.Size(172, 20);
            this.txtVpnOwner.TabIndex = 1;
            this.txtVpnOwner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmVpnsAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 187);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtVpnOwner);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddVpn);
            this.Controls.Add(this.txtVpnForAdd);
            this.Name = "frmVpnsAdd";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = " فرم اضافه کردن مورد جدید به لیست نظارتی VPN";
            this.Load += new System.EventHandler(this.frmVpnsAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtVpnForAdd;
        private System.Windows.Forms.Button btnAddVpn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVpnOwner;
    }
}