namespace PE.Forms.DT
{
    partial class frmTadakholBa
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
            this.txtRowCr = new System.Windows.Forms.TextBox();
            this.btnTadakholBa = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioGroupTypeOfInterfernce = new DevExpress.XtraEditors.RadioGroup();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupTypeOfInterfernce.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRowCr
            // 
            this.txtRowCr.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRowCr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRowCr.Location = new System.Drawing.Point(38, 47);
            this.txtRowCr.Name = "txtRowCr";
            this.txtRowCr.Size = new System.Drawing.Size(199, 29);
            this.txtRowCr.TabIndex = 1;
            this.txtRowCr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRowCr.TextChanged += new System.EventHandler(this.txtRowCr_TextChanged);
            this.txtRowCr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRow_KeyDown);
            // 
            // btnTadakholBa
            // 
            this.btnTadakholBa.Location = new System.Drawing.Point(145, 128);
            this.btnTadakholBa.Name = "btnTadakholBa";
            this.btnTadakholBa.Size = new System.Drawing.Size(193, 37);
            this.btnTadakholBa.TabIndex = 2;
            this.btnTadakholBa.Text = "OK";
            this.btnTadakholBa.UseVisualStyleBackColor = true;
            this.btnTadakholBa.Click += new System.EventHandler(this.btnTadakholBa_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioGroupTypeOfInterfernce);
            this.groupBox1.Location = new System.Drawing.Point(252, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(245, 94);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "تداخل با :";
            // 
            // radioGroupTypeOfInterfernce
            // 
            this.radioGroupTypeOfInterfernce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroupTypeOfInterfernce.Location = new System.Drawing.Point(3, 16);
            this.radioGroupTypeOfInterfernce.Name = "radioGroupTypeOfInterfernce";
            this.radioGroupTypeOfInterfernce.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "ردیف"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "CR")});
            this.radioGroupTypeOfInterfernce.Properties.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_Properties_SelectedIndexChanged);
            this.radioGroupTypeOfInterfernce.Size = new System.Drawing.Size(239, 75);
            this.radioGroupTypeOfInterfernce.TabIndex = 0;
            // 
            // frmTadakholBa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 186);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnTadakholBa);
            this.Controls.Add(this.txtRowCr);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTadakholBa";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmTadakholBa_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupTypeOfInterfernce.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtRowCr;
        private System.Windows.Forms.Button btnTadakholBa;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.RadioGroup radioGroupTypeOfInterfernce;
    }
}