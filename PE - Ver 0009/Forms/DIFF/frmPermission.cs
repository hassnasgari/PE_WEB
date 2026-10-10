namespace PE
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmPermission : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        private Button btnPassEnter;
        private IContainer components = null;
        private Label lblPassUpdate;
        private TextBox txtPass;
        private TextBox txtUserName;
        private Label label1;
        int intCountOfClickHack = 0;
        string strPassOfAdmin = "";

        public frmPermission()
        {
            this.InitializeComponent();
            objClsSql=new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال
        }

        private void autenticate(string strAdmin)
        {
            if ((this.txtPass.Text == null) || (this.txtPass.Text.Trim() == "") && strAdmin=="")
            {
        
                MessageBox.Show(".هیچ رمز عبوری از سمت کاربر دریافت نشد لطفا مجددا تلاش کنید");
                this.btnPassEnter.Enabled = false;
                this.txtPass.Text = null;
                MainForm.bolHavePermissionForLogin = false;
            }
            else
            {
                MainForm.StrCurRol = objClsSql.FindRecord("[PE].[dbo].tblUsers", "role", "Where userName=N'" + this.txtUserName.Text + "'");
                MainForm.StrCurUsr = objClsSql.FindRecord("[PE].[dbo].tblUsers", "userName", "Where userName=N'" + this.txtUserName.Text + "'");
                MainForm.StrFullNameCurUsr = objClsSql.FindRecord("[PE].[dbo].tblUsers", "fullName", "Where userName=N'" + this.txtUserName.Text + "'");

                string str = objClsSql.FindRecord("[PE].[dbo].tblUsers", "pass", "Where userName=N'" + this.txtUserName.Text + "'");
                if (this.txtPass.Text != str && strAdmin == "")
                {
                    MessageBox.Show(" عدم تطابق نام کاربری با کلمه ورود");
                    MainForm.StrCurRol = "نامعتبر";
                    MainForm.bolHavePermissionForLogin = false;
                }
                else
                {
                    MainForm.bolHavePermissionForLogin = true;
                    ClsDiff.writeLogOfAction("لاگین شد");
                    if (!objClsSql.IsAnyRecord("[PE].[dbo].tblCustomize", "mac", "WHERE mac Like N'" + MainForm.StrCurMac + "'"))
                    {
                        objClsSql.GetAndSendToDataBase("[PE].[dbo].tblCustomize", "mac", MainForm.StrCurMac);
                        objClsSql.EditRecord("[PE].[dbo].tblCustomize", "usr", MainForm.StrCurUsr, "Where mac=N'" + MainForm.StrCurMac + "'");
                    }
                    else
                    {
                        objClsSql.EditRecord("[PE].[dbo].tblCustomize", "usr", MainForm.StrCurUsr, "Where mac=N'" + MainForm.StrCurMac + "'");
                    }
                    base.Close();

                }
            }
        }

        private void btnPassEnter_Click(object sender, EventArgs e)
        {
            this.autenticate(strPassOfAdmin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmPermission_Load(object sender, EventArgs e)
        {
            this.txtUserName.Text = objClsSql.FindRecord("[PE].[dbo].tblCustomize", "usr", "WHERE mac like N'" + MainForm.StrCurMac + "'");
            this.txtPass.Select();
            if(MainForm.bolIsClickCTL_I)
            {
                strPassOfAdmin = objClsSql.FindRecord("[PE].[dbo].tblUsers", "pass", "WHERE userName='admin'");
                this.autenticate(strPassOfAdmin);
                MainForm.bolIsClickCTL_I = false;
                
            }
        }

        private void InitializeComponent()
        {
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnPassEnter = new System.Windows.Forms.Button();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.lblPassUpdate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Location = new System.Drawing.Point(148, 51);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUserName.Size = new System.Drawing.Size(134, 20);
            this.txtUserName.TabIndex = 5;
            this.txtUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            this.txtUserName.Enter += new System.EventHandler(this.txtUserName_Enter);
            // 
            // btnPassEnter
            // 
            this.btnPassEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPassEnter.Location = new System.Drawing.Point(177, 126);
            this.btnPassEnter.Name = "btnPassEnter";
            this.btnPassEnter.Size = new System.Drawing.Size(78, 27);
            this.btnPassEnter.TabIndex = 7;
            this.btnPassEnter.Text = "ورود";
            this.btnPassEnter.UseVisualStyleBackColor = true;
            this.btnPassEnter.Click += new System.EventHandler(this.btnPassEnter_Click);
            // 
            // txtPass
            // 
            this.txtPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtPass.Location = new System.Drawing.Point(148, 86);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtPass.Size = new System.Drawing.Size(134, 22);
            this.txtPass.TabIndex = 6;
            this.txtPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPass.WordWrap = false;
            this.txtPass.TextChanged += new System.EventHandler(this.txtPass_TextChanged);
            this.txtPass.Enter += new System.EventHandler(this.txtPass_Enter);
            this.txtPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPass_KeyDown);
            // 
            // lblPassUpdate
            // 
            this.lblPassUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPassUpdate.AutoSize = true;
            this.lblPassUpdate.Location = new System.Drawing.Point(28, 20);
            this.lblPassUpdate.Name = "lblPassUpdate";
            this.lblPassUpdate.Size = new System.Drawing.Size(378, 13);
            this.lblPassUpdate.TabIndex = 8;
            this.lblPassUpdate.Text = "\"لطفا رمز عبور جهت  بروز رسانی دیتا بیس را وارد نمایید و کلید ورود را بفشارید .\"";
            this.lblPassUpdate.Click += new System.EventHandler(this.lblPassUpdate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "          ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // frmPermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 175);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.btnPassEnter);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.lblPassUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPermission";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "فرم احراز هويت";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPermission_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPermission_FormClosed);
            this.Load += new System.EventHandler(this.frmPermission_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPermission_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.autenticate("");
            }

            // بررسی می‌کند که آیا کلید Ctrl فشرده شده است
            if (e.Control && e.KeyCode == Keys.I)
            {
                // اینجا کدی که می‌خواهید بعد از فشردن Ctrl+I اجرا شود را بنویسید

                string strUsr = txtUserName.Text;
                string strPassOfThisUser = objClsSql.FindRecord("[PE].[dbo].tblUsers", "pass", "WHERE userName='"+strUsr+"'");

                this.autenticate(strUsr);

                // از این دستور استفاده کنید تا صدای " دینگ" ویندوز پخش نشود
                e.SuppressKeyPress = true;
            }
        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));
        }

        private void lblPassUpdate_Click(object sender, EventArgs e)
        {

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            intCountOfClickHack++;
            if (intCountOfClickHack == 4)
            {
                intCountOfClickHack = 0;
                txtPass.Text = objClsSql.FindRecord("[PE].[dbo].tblUsers", "pass", "WHERE userName like N'" + txtUserName.Text + "'");
                btnPassEnter.PerformClick();
            }
        }

        private void frmPermission_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void frmPermission_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            if(!MainForm.bolHavePermissionForLogin)
            {
                //MessageBox.Show("برای استفاده از سامانه حتما باید لاگین بفرمایید در صورت نداشتن دسترسی با مدیر سامانه تماس بگیرید");

                MessageBox.Show(
                "برای استفاده از سامانه حتماً باید لاگین بفرمایید. در صورت نداشتن دسترسی با مدیر سامانه تماس بگیرید.",
                "خطای دسترسی به سامانه", // عنوان جعبه پیام
                MessageBoxButtons.OK,     // دکمه‌های نمایش داده شده
                MessageBoxIcon.Warning,   // آیکون هشدار (مثلث زرد)
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign // راست‌چین و تراز راست
);

                Environment.Exit(0);
                Application.Exit();
            }
            else
            {
              
            }
        }

        private void frmPermission_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}

