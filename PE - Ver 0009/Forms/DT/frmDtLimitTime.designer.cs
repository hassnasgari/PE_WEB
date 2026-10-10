
namespace PE.Forms.DT
{
    partial class frmDtLimitTime
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
            this.lblDtLimit = new System.Windows.Forms.Label();
            this.comboBoxDtLimitHour = new System.Windows.Forms.ComboBox();
            this.lblDtLimitHour = new System.Windows.Forms.Label();
            this.lblDtLimitMin = new System.Windows.Forms.Label();
            this.comboBoxDtLimitMin = new System.Windows.Forms.ComboBox();
            this.btnDtLimitReg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDtLimit
            // 
            this.lblDtLimit.AutoSize = true;
            this.lblDtLimit.Location = new System.Drawing.Point(99, 49);
            this.lblDtLimit.Name = "lblDtLimit";
            this.lblDtLimit.Size = new System.Drawing.Size(254, 13);
            this.lblDtLimit.TabIndex = 0;
            this.lblDtLimit.Text = "آخرین زمان برای گرفتن دان تایم در روز را مشخص کنید";
            // 
            // comboBoxDtLimitHour
            // 
            this.comboBoxDtLimitHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDtLimitHour.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.comboBoxDtLimitHour.Location = new System.Drawing.Point(165, 85);
            this.comboBoxDtLimitHour.Name = "comboBoxDtLimitHour";
            this.comboBoxDtLimitHour.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDtLimitHour.TabIndex = 1;
            // 
            // lblDtLimitHour
            // 
            this.lblDtLimitHour.AutoSize = true;
            this.lblDtLimitHour.Location = new System.Drawing.Point(115, 88);
            this.lblDtLimitHour.Name = "lblDtLimitHour";
            this.lblDtLimitHour.Size = new System.Drawing.Size(35, 13);
            this.lblDtLimitHour.TabIndex = 2;
            this.lblDtLimitHour.Text = "ساعت";
            // 
            // lblDtLimitMin
            // 
            this.lblDtLimitMin.AutoSize = true;
            this.lblDtLimitMin.Location = new System.Drawing.Point(115, 135);
            this.lblDtLimitMin.Name = "lblDtLimitMin";
            this.lblDtLimitMin.Size = new System.Drawing.Size(32, 13);
            this.lblDtLimitMin.TabIndex = 4;
            this.lblDtLimitMin.Text = "دقیقه";
            // 
            // comboBoxDtLimitMin
            // 
            this.comboBoxDtLimitMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDtLimitMin.FormattingEnabled = true;
            this.comboBoxDtLimitMin.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.comboBoxDtLimitMin.Location = new System.Drawing.Point(165, 132);
            this.comboBoxDtLimitMin.Name = "comboBoxDtLimitMin";
            this.comboBoxDtLimitMin.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDtLimitMin.TabIndex = 3;
            // 
            // btnDtLimitReg
            // 
            this.btnDtLimitReg.Location = new System.Drawing.Point(184, 216);
            this.btnDtLimitReg.Name = "btnDtLimitReg";
            this.btnDtLimitReg.Size = new System.Drawing.Size(102, 23);
            this.btnDtLimitReg.TabIndex = 5;
            this.btnDtLimitReg.Text = "تایید و ثبت";
            this.btnDtLimitReg.UseVisualStyleBackColor = true;
            this.btnDtLimitReg.Click += new System.EventHandler(this.btnDtLimitReg_Click);
            // 
            // frmDtLimitTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 301);
            this.Controls.Add(this.btnDtLimitReg);
            this.Controls.Add(this.lblDtLimitMin);
            this.Controls.Add(this.comboBoxDtLimitMin);
            this.Controls.Add(this.lblDtLimitHour);
            this.Controls.Add(this.comboBoxDtLimitHour);
            this.Controls.Add(this.lblDtLimit);
            this.Name = "frmDtLimitTime";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = " تنظیم حداکثر ساعت مجاز گرفتن دان تایم";
            this.Load += new System.EventHandler(this.frmDtLimitTime_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDtLimit;
        private System.Windows.Forms.ComboBox comboBoxDtLimitHour;
        private System.Windows.Forms.Label lblDtLimitHour;
        private System.Windows.Forms.Label lblDtLimitMin;
        private System.Windows.Forms.ComboBox comboBoxDtLimitMin;
        private System.Windows.Forms.Button btnDtLimitReg;
    }
}