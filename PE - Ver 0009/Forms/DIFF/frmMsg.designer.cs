namespace PE.Forms
{
    partial class frmMsg
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
            this.lblFrmMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFrmMessage
            // 
            this.lblFrmMessage.AutoSize = true;
            this.lblFrmMessage.Location = new System.Drawing.Point(3, 9);
            this.lblFrmMessage.Name = "lblFrmMessage";
            this.lblFrmMessage.Size = new System.Drawing.Size(126, 13);
            this.lblFrmMessage.TabIndex = 1;
            this.lblFrmMessage.Text = "این جا محل پیام شماهاست";
            // 
            // frmMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 75);
            this.Controls.Add(this.lblFrmMessage);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMsg";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.Text = "پیام";
            this.Load += new System.EventHandler(this.frmMsg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFrmMessage;
    }
}