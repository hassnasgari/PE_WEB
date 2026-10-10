using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE.Forms.DT
{
    public partial class frmDtAttachFile : Form
    {
        ClsSql objClsSql = new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال
        string strDefaultSelectedRowNumber = "";
        public frmDtAttachFile()
        {
            InitializeComponent();
        }

        private void frmDtAttachFile_Load(object sender, EventArgs e)
        {
            strDefaultSelectedRowNumber=txtDtAttachFile.Text = MainForm.StrCurRowInDtGrid;

            txtDtAttachFile.Focus();
        }

        private void txtDtAttachFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
             (e.KeyChar != '.'))
            {
                e.Handled = true;
                MessageBox.Show("لطفا فقط عدد رذیف دان تایم وارد کنید");
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtDtAttachFile_Leave(object sender, EventArgs e)
        {
            if (txtDtAttachFile.Text.Length < 5)
            {
                txtDtAttachFile.BackColor = Color.Pink;
                MessageBox.Show("شماره ردیف معمولا یک عدد 5 رقمی باشد تعداد ارقام وارد شده " + txtDtAttachFile.Text.Length + "عدد می باشد ");
            }
        }

        private void txtDtAttachFile_Enter(object sender, EventArgs e)
        {
           InputLanguage.CurrentInputLanguage =
           InputLanguage.FromCulture(System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
        }

        private void DtSendNameOfAttachFile_Click(object sender, EventArgs e)
        {
            string strNetIp ="172.16.3.116";

            bool bolIsAnyDtWithThisRow=objClsSql.IsAnyRecord("DT.dbo.tblDownTimeShort", " WHERE row=N'" + txtDtAttachFile.Text + "'");

            if(bolIsAnyDtWithThisRow)//بررسی وجود این ردیف دان تایم برای ارسال پیوست
            {
                #region اگر چنین ردیف دان تایمی وجود دارد ادامه

                    #region آپلود و ارسال فایل

                    DialogResult result = openFileDialogAttachDtFile.ShowDialog();

                    if (result == DialogResult.OK)//
                    {
                        string uploadedFileName =  openFileDialogAttachDtFile.FileName;//فایل آپلود شده
                        string extension = Path.GetExtension(openFileDialogAttachDtFile.SafeFileName);//استخراج پسوند

                        string destFileName = @"\\" + strNetIp + @"\PE_Share\DtAttachFile\" + txtDtAttachFile.Text + extension;//ساخت نام جدید فایل با استفاده از ردیف

                        #region  بررسی وجود فایل پیوست دان تایم در سرور 

                        bool bolFileExist = File.Exists(destFileName);//بررسی وجود فایل هم نام در سرور

                        if (bolFileExist)
                        {
                            #region در صورت تمایل جایگزینی

                            DialogResult dialogResult = MessageBox.Show("Sure", " هم اکنون یک فایل پیوست دان تایم وجود دارد آیا می خواهید جایگزین گردد", MessageBoxButtons.YesNo);

                            if (dialogResult == DialogResult.Yes)
                            {
                                File.Delete(destFileName);
                                copyAttachedFileToServerAndInsertToDbRowNumber(uploadedFileName, destFileName, true, strDefaultSelectedRowNumber);
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                MessageBox.Show(" انصراف از جایگزینی فایل پیوست دان تایم ");
                                ClsDiff.writeLogOfAction("انصراف از جایگزینی فایل پیوست دان تایم");
                            }

                            #endregion
                        }
                        else
                        {
                        #region انتقال فایل پیوست به سرور بدون جایگزینی
                        copyAttachedFileToServerAndInsertToDbRowNumber(uploadedFileName,  destFileName, false, strDefaultSelectedRowNumber);
                      
                            #endregion
                    }
                        #endregion
                    }

                    this.Close();

                    #endregion 

                #endregion
            }
            else
            {
                #region در صورت نبودن چنین ردیفی در لیست دان تایمها پیام مناسب و اجازه وارد کردن شماره ردیف درست
                MessageBox.Show(" هیچ ردیف دان تایم پیدا نشد لطفا مجددا تلاش کنید ");
                txtDtAttachFile.Focus(); 
                #endregion
            }
        }

        private void copyAttachedFileToServerAndInsertToDbRowNumber(string uploadedFileName, string destFileName,bool bolOverWright,string strRowNumber)
        {
            try
            {
               
                File.Copy(uploadedFileName, destFileName, bolOverWright);

                string strdateTimeForAttachFile = ClsConvertDateHassan.convertDateAndDayToPersion(DateTime.Now);

                
                objClsSql.SendCommand("INSERT INTO DT.dbo.tblAttachedFile (rowWithAttachedFileInDt,dateTime) VALUES ("+  strRowNumber+",N'"+  strdateTimeForAttachFile + "' ) ");
                objClsSql.SendCommand("update [DT].[dbo].[tblDownTimeShort] set haveAttachedFile=N'دارد' Where row=N'" + strRowNumber + @"'");
                //
                MessageBox.Show(" فایل پیوست دان تایم  به سرور منتقل شد ");

                ClsDiff.writeLogOfAction(" انتقال فایل پیوست  به سرور" );

                #region ایجاد یک آرایه از ردیفهایی که فایل پیوست دارند

                MainForm.DrRowNumberAttachFile = objClsSql.FillDataTableFromQuery("Select rowWithAttachedFileInDt From DT.dbo.tblAttachedFile").Select();

                #endregion



            }
            catch (Exception ex)
            {
                MessageBox.Show(" فایل پیوست دان تایم  در سرور جایگزین نشد " +ex.ToString());

                ClsDiff.writeLogOfAction("ناموفق در انتقال فایل پیوست  به سرور");
            }
        }
    }
}
