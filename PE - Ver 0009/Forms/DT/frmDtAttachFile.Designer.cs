namespace PE.Forms.DT
{
    partial class frmDtAttachFile
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
            this.DtSendNameOfAttachFile = new System.Windows.Forms.Button();
            this.txtDtAttachFile = new System.Windows.Forms.TextBox();
            this.lblDtAttachFile = new System.Windows.Forms.Label();
            this.openFileDialogAttachDtFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // DtSendNameOfAttachFile
            // 
            this.DtSendNameOfAttachFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DtSendNameOfAttachFile.Location = new System.Drawing.Point(7, 12);
            this.DtSendNameOfAttachFile.Name = "DtSendNameOfAttachFile";
            this.DtSendNameOfAttachFile.Size = new System.Drawing.Size(119, 23);
            this.DtSendNameOfAttachFile.TabIndex = 1;
            this.DtSendNameOfAttachFile.Text = "جستجو و ارسال فایل";
            this.DtSendNameOfAttachFile.UseVisualStyleBackColor = true;
            this.DtSendNameOfAttachFile.Click += new System.EventHandler(this.DtSendNameOfAttachFile_Click);
            // 
            // txtDtAttachFile
            // 
            this.txtDtAttachFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtAttachFile.Location = new System.Drawing.Point(143, 14);
            this.txtDtAttachFile.Name = "txtDtAttachFile";
            this.txtDtAttachFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDtAttachFile.Size = new System.Drawing.Size(65, 20);
            this.txtDtAttachFile.TabIndex = 0;
            this.txtDtAttachFile.TabStop = false;
            this.txtDtAttachFile.Text = "ردیف";
            this.txtDtAttachFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDtAttachFile.Enter += new System.EventHandler(this.txtDtAttachFile_Enter);
            this.txtDtAttachFile.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDtAttachFile_KeyPress);
            this.txtDtAttachFile.Leave += new System.EventHandler(this.txtDtAttachFile_Leave);
            // 
            // lblDtAttachFile
            // 
            this.lblDtAttachFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDtAttachFile.AutoSize = true;
            this.lblDtAttachFile.Location = new System.Drawing.Point(224, 17);
            this.lblDtAttachFile.Name = "lblDtAttachFile";
            this.lblDtAttachFile.Size = new System.Drawing.Size(387, 13);
            this.lblDtAttachFile.TabIndex = 2;
            this.lblDtAttachFile.Text = "لطفا شماره ردیف دان تایمی را که می خواهید برای آن فایل پیوست کنید را وارد کنید";
            // 
            // frmDtAttachFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 43);
            this.Controls.Add(this.lblDtAttachFile);
            this.Controls.Add(this.txtDtAttachFile);
            this.Controls.Add(this.DtSendNameOfAttachFile);
            this.Name = "frmDtAttachFile";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = " فرم پیوست کردن فایل به دان تایم درخواستی";
            this.Load += new System.EventHandler(this.frmDtAttachFile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DtSendNameOfAttachFile;
        private System.Windows.Forms.TextBox txtDtAttachFile;
        private System.Windows.Forms.Label lblDtAttachFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogAttachDtFile;
    }
}