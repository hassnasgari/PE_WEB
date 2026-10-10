
namespace PE.Forms.DT
{
    partial class frmDtFreezMessage
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
            this.btnFreezFrom = new System.Windows.Forms.Button();
            this.btnFreezTo = new System.Windows.Forms.Button();
            this.txtFreezStart = new System.Windows.Forms.TextBox();
            this.txtFreezEnd = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtText = new System.Windows.Forms.TextBox();
            this.rdoBtnSetText = new System.Windows.Forms.RadioButton();
            this.rdoBtnSelectDate = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFreezFrom
            // 
            this.btnFreezFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFreezFrom.Location = new System.Drawing.Point(204, 63);
            this.btnFreezFrom.Name = "btnFreezFrom";
            this.btnFreezFrom.Size = new System.Drawing.Size(142, 23);
            this.btnFreezFrom.TabIndex = 0;
            this.btnFreezFrom.Text = "تاریخ شروع فریز شبکه";
            this.btnFreezFrom.UseVisualStyleBackColor = true;
            this.btnFreezFrom.Click += new System.EventHandler(this.btnFreezFrom_Click);
            // 
            // btnFreezTo
            // 
            this.btnFreezTo.Enabled = false;
            this.btnFreezTo.Location = new System.Drawing.Point(328, 51);
            this.btnFreezTo.Name = "btnFreezTo";
            this.btnFreezTo.Size = new System.Drawing.Size(142, 23);
            this.btnFreezTo.TabIndex = 1;
            this.btnFreezTo.Text = "تاریخ پایان فریز شبکه";
            this.btnFreezTo.UseVisualStyleBackColor = true;
            this.btnFreezTo.Click += new System.EventHandler(this.btnFreezTo_Click);
            // 
            // txtFreezStart
            // 
            this.txtFreezStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFreezStart.BackColor = System.Drawing.SystemColors.Control;
            this.txtFreezStart.Enabled = false;
            this.txtFreezStart.Location = new System.Drawing.Point(34, 66);
            this.txtFreezStart.Name = "txtFreezStart";
            this.txtFreezStart.Size = new System.Drawing.Size(132, 20);
            this.txtFreezStart.TabIndex = 1;
            this.txtFreezStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFreezEnd
            // 
            this.txtFreezEnd.BackColor = System.Drawing.SystemColors.Control;
            this.txtFreezEnd.Enabled = false;
            this.txtFreezEnd.Location = new System.Drawing.Point(143, 51);
            this.txtFreezEnd.Name = "txtFreezEnd";
            this.txtFreezEnd.Size = new System.Drawing.Size(170, 20);
            this.txtFreezEnd.TabIndex = 1;
            this.txtFreezEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(370, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(207, 33);
            this.button1.TabIndex = 1;
            this.button1.Text = "ثبت";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtText);
            this.groupBox1.Controls.Add(this.rdoBtnSetText);
            this.groupBox1.Controls.Add(this.txtFreezEnd);
            this.groupBox1.Controls.Add(this.rdoBtnSelectDate);
            this.groupBox1.Controls.Add(this.btnFreezTo);
            this.groupBox1.Location = new System.Drawing.Point(387, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(658, 159);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "تاریخ یا متن پایان";
            // 
            // txtText
            // 
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.Enabled = false;
            this.txtText.Location = new System.Drawing.Point(6, 88);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(463, 20);
            this.txtText.TabIndex = 2;
            this.txtText.Text = " اطلاع ثانوی ";
            // 
            // rdoBtnSetText
            // 
            this.rdoBtnSetText.AutoSize = true;
            this.rdoBtnSetText.Location = new System.Drawing.Point(481, 88);
            this.rdoBtnSetText.Name = "rdoBtnSetText";
            this.rdoBtnSetText.Size = new System.Drawing.Size(135, 17);
            this.rdoBtnSetText.TabIndex = 1;
            this.rdoBtnSetText.TabStop = true;
            this.rdoBtnSetText.Text = "انتخاب پایان بصورت متن";
            this.rdoBtnSetText.UseVisualStyleBackColor = true;
            // 
            // rdoBtnSelectDate
            // 
            this.rdoBtnSelectDate.AutoSize = true;
            this.rdoBtnSelectDate.BackColor = System.Drawing.SystemColors.Control;
            this.rdoBtnSelectDate.Location = new System.Drawing.Point(474, 54);
            this.rdoBtnSelectDate.Name = "rdoBtnSelectDate";
            this.rdoBtnSelectDate.Size = new System.Drawing.Size(144, 17);
            this.rdoBtnSelectDate.TabIndex = 0;
            this.rdoBtnSelectDate.TabStop = true;
            this.rdoBtnSelectDate.Text = "انتخاب پایان بصورت تاریخ";
            this.rdoBtnSelectDate.UseVisualStyleBackColor = false;
            this.rdoBtnSelectDate.CheckedChanged += new System.EventHandler(this.rdoBtnSelectDate_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnFreezFrom);
            this.groupBox2.Controls.Add(this.txtFreezStart);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(369, 159);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "تاریخ شروع";
            // 
            // frmDtFreezMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 222);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "frmDtFreezMessage";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "پیام فریز";
            this.Load += new System.EventHandler(this.frmDtFreezMessage_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFreezFrom;
        private System.Windows.Forms.Button btnFreezTo;
        private System.Windows.Forms.TextBox txtFreezStart;
        private System.Windows.Forms.TextBox txtFreezEnd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.RadioButton rdoBtnSetText;
        private System.Windows.Forms.RadioButton rdoBtnSelectDate;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}