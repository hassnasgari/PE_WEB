using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE.Forms.DB
{
    public partial class frmDbDeletedSite : Form
    {
        DataTable dtDeletedSite;
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال

        public frmDbDeletedSite()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void ObjFrmDbDeletedSite_Load(object sender, EventArgs e)
        {
            refreshDbDeletedSiteGrid();

            if (MainForm.StrCurRol == "administrator" || MainForm.StrCurRol == "PowerUser")
            {
                groupBoxDbAddDeletedSite.Width = 460;
                groupBoxDbDelDeletedSite.Width = 460;

                groupBoxDbAddDeletedSite.Enabled = true;
                groupBoxDbDelDeletedSite.Enabled = true;

                refreshComboBoxContarctor();
                refreshComboBoxRegion();
            }
            else
            {
                groupBoxDbAddDeletedSite.Enabled = false;
                groupBoxDbDelDeletedSite.Enabled = false;

                groupBoxDbAddDeletedSite.Width = 1;
                groupBoxDbDelDeletedSite.Width = 1;
            }
        }

        private void TxtDbDeletedLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {

                MessageBox.Show("لطفا فقط عدد وارد کنید");
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TxtDbDeletedLocation_TextChanged(object sender, EventArgs e)
        {
            txtDbDeletedLocation.CharacterCasing = CharacterCasing.Upper;

            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));

            if (txtDbDeletedLocation.TextLength == 4)
            {

                string strLocation = "ES" + txtDbDeletedLocation.Text;

                DataRow[] dr = null;
                try
                {
                    dr = MainForm.DtMiniTempDb.Select("[لوکیشن]='" + strLocation + "'");
                    txtDbDeletedSiteName.Text = dr[0].ItemArray[2].ToString();

                    if(dr[0].ItemArray[2].ToString()!="" && dr[0].ItemArray[2].ToString()!=null)
                    {
                    }
                    else
                    {
                        MessageBox.Show("  همانطور که مشاهده می کنید این  لوکیشن در دیتابیس وجود ندارد و طبیعتا قابل حذف نیست ");
                    }
                    
                }
                catch
                {
                   // txtDbDeletedSiteName.Text = "NEW";
                }


            }
            else
            {
                txtDbDeletedSiteName.Text = "-----------------------------------------------";

                if (txtDbDeletedLocation.Text.Length > 4)
                {
                    MessageBox.Show("کد سایت باید یک عدد 4 رقمی باشد");
                    txtDbDeletedLocation.Text = txtDbDeletedLocation.Text.Remove(txtDbDeletedLocation.Text.Length - 1, 1);

                }
            }
        }

        private void TxtDbDeletedLocation_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));
            txtDbDeletedLocation.CharacterCasing = CharacterCasing.Upper;
            txtDbDeletedLocation.SelectionStart = 4;
        }

        private void refreshComboBoxContarctor()
        {
            DataTable table = objClsSql.FillDataTableFromQuery("SELECT [پیمانکار] FROM [DbAras].[dbo].tblDB  GROUP BY [پیمانکار]  HAVING [پیمانکار]!=N''");
            
        }

        private void refreshComboBoxRegion()
        {
            DataTable table = objClsSql.FillDataTableFromQuery("SELECT [منطقه] FROM [DbAras].[dbo].tblDB  GROUP BY [منطقه]  HAVING [منطقه]!=N''");
       
        }

        private void TxtDbDeletedLocation_Leave(object sender, EventArgs e)
        {
            if (txtDbDeletedLocation.TextLength < 4 && txtDbDeletedLocation.TextLength != 0)
            {
                MessageBox.Show("دقت کن ! لوکیشن چهار رقمی است");
                txtDbDeletedLocation.Focus();
            }

            if (txtDbDeletedLocation.TextLength == 4)
            {
                DataRow[] foundAuthors = dtDeletedSite.Select("لوکیشن ='ES" + txtDbDeletedLocation.Text + "'");
                if (foundAuthors.Length == 1)
                {
                    MessageBox.Show("چیو میخوای اضافه کنی این که در جدول مقابل وجود داره");
                    txtDbDeletedLocation.Focus();
                    return;
                }
            }

        }

        private void TxtDbDeletedSiteName_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void BtnDbAddDeletedSite_Click(object sender, EventArgs e)
        {
            if (txtDbDeletedLocation.TextLength != 4)
            {
                MessageBox.Show("لطفا تعداد ارقام لوکیشن را چک کن");
                txtDbDeletedLocation.Focus();
                return;
            }


            bool bolOkInsert = AddDeletedTempSite("ES" + txtDbDeletedLocation.Text, txtDbDeletedSiteName.Text, txtDeletedDate.Text);

            if (bolOkInsert)
            {
                MessageBox.Show("موفق شدی سایت حذف شده رو اضافه کنی .اسم سایت " + txtDbDeletedSiteName.Text + " دمت گرم");

                txtDbDeletedLocation.Clear();
                txtDbDeletedSiteName.Clear();
                refreshDbDeletedSiteGrid();
                ClsDiff.writeLogOfAction(" یه سایت جدید" + txtDbDeletedSiteName.Text + " به سامانه اضافه شد  " + txtDbDeletedSiteName.Text);
            }
            else
            {
                MessageBox.Show("نشد که نشد . به مدیر سیستم خبر بده");

                ClsDiff.writeLogOfAction(" عدم موفقیت در اضافه کردن سایت جدید " + txtDbDeletedSiteName.Text);
            }


        }

        private bool AddDeletedTempSite(string strLocation, string strSiteName, string strDeletedDate)
        {

            string[] strValue = { strLocation, strSiteName, strDeletedDate};
            string[] strParam = { "location", "siteName", "DeletedDate" };
            bool bolInsertOk =objClsSql.Insert("PE.dbo.tblTempDeletedSite", strParam, strValue);
            return bolInsertOk;
        }

        private void refreshDbDeletedSiteGrid()
        {

            try
            {
                 gridControlDbDeletedSite.DataSource = dtDeletedSite = objClsSql.FillDataTableFromQuery(@"SELECT  [location] AS لوکیشن
                  ,[siteName] AS [نام سایت]
                  ,[DeletedDate] AS [تاریخ حذف]
               
	                FROM [PE].[dbo].[tblTempDeletedSite] ");

                this.gridViewDbDeletedSite.Columns["لوکیشن"].Width = 70;
                this.gridViewDbDeletedSite.Columns["نام سایت"].Width = 110;
                this.gridViewDbDeletedSite.Columns["تاریخ حذف"].Width = 130;
               

            }
            catch (Exception)
            {
                MessageBox.Show(" اطلاعات  سایتهای حذف شده از دیتا بیس واکشی نشد  مراتب را به مدیر سامانه اطلاع دهید  ");
                return;
            }
        }

        private void GridControlDbDeletedSite_KeyDown(object sender, KeyEventArgs e)
        {
            EnabledOrDisableDelBtn();
        }

        private void EnabledOrDisableDelBtn()
        {
            if (gridViewDbDeletedSite.SelectedRowsCount == 1)
            {
                btnDbDelSelectedDeletedSite.Enabled = true;
            }
            else
            {
                btnDbDelSelectedDeletedSite.Enabled = false;
            }
        }

        private void BtnDbDelSelectedDeletedSite_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" سطر انتخابی حذف شود ؟ ", "آلارم",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)

            {

                string strLocation = gridViewDbDeletedSite.GetFocusedRowCellValue("لوکیشن").ToString();

                int intContRecordBeforDel = objClsSql.Count("PE.dbo.tblTempDeletedSite","");
               objClsSql.DeleteRecords("PE.dbo.tblTempDeletedSite", "WHERE location=N'" + strLocation + "'");
                int intContRecordAfterDel = objClsSql.Count("PE.dbo.tblTempDeletedSite","");

                if (intContRecordBeforDel == intContRecordAfterDel + 1)

                {
                    MessageBox.Show(  " حذف شد"+ strLocation + "سطر جدول سایتهای جدید با لوکیشن ");

                    ClsDiff.writeLogOfAction(  " حذف شد" + strLocation +  "سطر جدول سایتهای جدید با لوکیشن ");
                }
                else
                {
                    MessageBox.Show("سطر انتخابی حذف نشده ببین چی شده ! با مدیر سامانه بتماس");

                    ClsDiff.writeLogOfAction("عدم موفقیت در سطر انتخابی Removed SITE");

                }

                refreshDbDeletedSiteGrid();
            }
        }

        private void BtnDbDelAllDeletedSite_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(" همه حذف ؟ ", "آلارم",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.Yes)

            {
                objClsSql.DeleteAllRecords("PE.dbo.tblTempDeletedSite");

                if (objClsSql.Count("PE.dbo.tblTempDeletedSite","") == 0)
                {
                    MessageBox.Show("همش Deleted Site حذف شد");
                    ClsDiff.writeLogOfAction("  کل جدول سایتهای حذف شده در لیست حذف شد");
                }
                else
                {
                    MessageBox.Show("همش حذف نشده ببین چی شده ! با مدیر سامانه بتماس");
                    ClsDiff.writeLogOfAction("عدم موفقیت در حذف کل جدول Deleted SITE");

                }

                refreshDbDeletedSiteGrid();
            }

        }

        private void GridControlDbDeletedSite_Click(object sender, EventArgs e)
        {
            EnabledOrDisableDelBtn();
        }

        private void GroupBoxDbAddDeletedSite_Enter(object sender, EventArgs e)
        {

        }

        private void BtnDeletedDate_Click(object sender, EventArgs e)
        {
            frmSelectDate objFrmselectDate = new frmSelectDate();
            objFrmselectDate.ShowDialog();
            txtDeletedDate.Text = MainForm.StrSelectedPersianDateWithDotSeperator;
        }

        private void btnDbExpoprtDelSitesPlan_Click(object sender, EventArgs e)
        {
            this.saveFileDialogDbDelSitesPlan.ShowDialog();

            if (this.saveFileDialogDbDelSitesPlan.FileName != string.Empty)
            {
                this.gridViewDbDeletedSite.ExportToXlsx(this.saveFileDialogDbDelSitesPlan.FileName);
            }
            else
            {
                MessageBox.Show("مقصد جهت ذخیره انتخاب نشده");
            }
        }
    }
}
