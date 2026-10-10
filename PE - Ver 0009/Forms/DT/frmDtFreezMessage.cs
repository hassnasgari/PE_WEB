using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE.Forms.DT
{
    public partial class frmDtFreezMessage : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public frmDtFreezMessage()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void btnFreezFrom_Click(object sender, EventArgs e)
        {
            frmSelectDate frmSelDate = new frmSelectDate();
            frmSelDate.ShowDialog();
            if (MainForm.StrSelectedPershianDateWithSlashSeperator != "")//ساعت صفر را در نظر می گیرد که باید اصلاح گردد چون روز جاری را قبول نمی کند
            {
                MainForm.StrDtStartDate = MainForm.StrSelectedPershianDateWithSlashSeperator;
                DateTime dteDtFreezStartDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrSelectedPershianDateWithSlashSeperator);

                int intCompareDate = DateTime.Compare(dteDtFreezStartDate.Date, MainForm.DateTimeSqlServer.Date);

                if (!(intCompareDate < 0))
                {
                    txtFreezStart.Text = MainForm.StrSelectedPershianDateWithNameOfDay;
                }
                else
                {
                    MessageBox.Show("تذکر لازم اینکه تاریخ شروع فریز قبل از تاریخ امروز تنظیم شد ");
                    txtFreezStart.Text = MainForm.StrSelectedPershianDateWithNameOfDay;
                }
            }
            else
            {
                MessageBox.Show(" هیچ تاریخی تنظیم نشده لطفا مجددا تلاش نمایید ");
                return;
            }
        }

        private void btnFreezTo_Click(object sender, EventArgs e)
        {
            frmSelectDate frmSelDate = new frmSelectDate();
            frmSelDate.ShowDialog();

            if (MainForm.StrSelectedPershianDateWithSlashSeperator != "")
            {
                MainForm.StrDtEndDate = MainForm.StrSelectedPershianDateWithSlashSeperator;
                DateTime dteDtFreezEndDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrSelectedPershianDateWithSlashSeperator);

                int intCompareDate = DateTime.Compare(dteDtFreezEndDate, MainForm.DateTimeSqlServer.Date);

                if (intCompareDate >= 0)
                {
                    txtFreezEnd.Text = MainForm.StrSelectedPershianDateWithNameOfDay;
                    this.txtFreezEnd.ForeColor = System.Drawing.Color.Navy;
                }
                else
                {
                    MessageBox.Show("تاریخ پایان فریز باید مساوی یا بزرگتر از تاریخ امروز باشد");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (txtFreezStart.Text != "" && (txtFreezEnd.Text != "" || txtText.Text!=""))
            {
               

                if (rdoBtnSelectDate.Checked)
                {
                    objClsSql.EditRecord("DT.dbo.tblFreezDate", "startFreezeDate", txtFreezStart.Text, " Where row=1");
                    objClsSql.EditRecord("DT.dbo.tblFreezDate", "endFreezeDate",  " پایان روز "+txtFreezEnd.Text , " Where row=1");

                    MessageBox.Show(" تاریخ شروع و پایان فریز با موفقیت اعمال شد ");
                    MainForm.bolDtHaveNewDateForFreez = true;
                }

               
                else
                {
                    objClsSql.EditRecord("DT.dbo.tblFreezDate", "startFreezeDate", txtFreezStart.Text, " Where row=1");
                    objClsSql.EditRecord("DT.dbo.tblFreezDate", "endFreezeDate", txtText.Text, " Where row=1");
                    MessageBox.Show(" تاریخ شروع و متن پایان فریز با موفقیت اعمال شد ");
                    MainForm.bolDtHaveNewDateForFreez = true;
                }

                
                this.Close();
                
            }
            else
            {
                if (txtFreezStart.Text == "")
                {
                    MessageBox.Show(" تاریخ شروع فریز تنظیم نشده لطفا تنظیم و مجددا تلاش نمایید ");
                    btnFreezFrom.Focus();
                }
                else
                {
                    if (rdoBtnSelectDate.Checked)
                    {
                        MessageBox.Show(" تاریخ پایان فریز تنظیم نشده لطفا تنظیم و مجددا تلاش نمایید ");
                        btnFreezTo.Focus();
                    }
                    else
                    {
                        MessageBox.Show(" متن پایان فریز تنظیم نشده لطفا تنظیم و مجددا تلاش نمایید ");
                        txtText.Focus();
                    }


                    
                }
                               
            }
        }

        private void frmDtFreezMessage_Load(object sender, EventArgs e)
        {
            
            MainForm.bolDtHaveNewDateForFreez = false;
        }

        private void rdoBtnSelectDate_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoBtnSelectDate.Checked)
            {
                btnFreezTo.Enabled = true;
                txtText.Enabled = false;
                btnFreezTo.Focus();


            }
            else
            {
                btnFreezTo.Enabled = false;
                txtText.Enabled = true;
                txtText.Focus();
            }
        }
    }
}
