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
    public partial class frmDbNewSite : Form
    {
        DataTable dtNewSite;
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال

        public frmDbNewSite()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void FrmDbNewSite_Load(object sender, EventArgs e)
        {
            refreshDbNewSiteGrid();

            if (MainForm.StrCurRol == "administrator" || MainForm.StrCurRol == "PowerUser")
            {
                groupBoxDbAddNewSite.Width = 460;
                groupBoxDbDelNewSite.Width = 460;

                groupBoxDbAddNewSite.Enabled = true;
                groupBoxDbDelNewSite.Enabled = true;

                refreshComboBoxContarctor();
                refreshComboBoxRegion();
            }
            else
            {
                groupBoxDbAddNewSite.Enabled = false;
                groupBoxDbDelNewSite.Enabled = false;

                groupBoxDbAddNewSite.Width = 1;
                groupBoxDbDelNewSite.Width = 1;
            }

        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
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

        private void TxtDbNewLocation_TextChanged(object sender, EventArgs e)
        {

            txtDbNewLocation.CharacterCasing = CharacterCasing.Upper;

            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));

            if (txtDbNewLocation.TextLength == 4)
            {

                string strLocation = "ES" + txtDbNewLocation.Text;

                DataRow[] dr = null;
                try
                {
                    dr = MainForm.DtMiniTempDb.Select("[لوکیشن]='" + strLocation + "'");
                    txtDbNewSiteName.Text = dr[0].ItemArray[2].ToString();
                    MessageBox.Show("  همانطور که مشاهده می کنید این  لوکیشن در دیتابیس وجود دارد و جدید محسوب نمیشه ");
                }
                catch
                {
                    txtDbNewSiteName.Text = "NEW";
                }


            }
            else
            {
                txtDbNewSiteName.Text = "-----------------------------------------------";

                if (txtDbNewLocation.Text.Length > 4)
                {
                    MessageBox.Show("کد سایت باید یک عدد 4 رقمی باشد");
                    txtDbNewLocation.Text = txtDbNewLocation.Text.Remove(txtDbNewLocation.Text.Length - 1, 1);

                }
            }

        }

        private void TxtDbNewLocation_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));
            txtDbNewLocation.CharacterCasing = CharacterCasing.Upper;
            txtDbNewLocation.SelectionStart = 4;
        }

        private void refreshComboBoxContarctor()
        {
            DataTable table = objClsSql.FillDataTableFromQuery("SELECT [پیمانکار] FROM [DB].[dbo].tblDbSummary  GROUP BY [پیمانکار]  HAVING [پیمانکار]!=N''");
            this.cbxContractor.DataSource = table;
            this.cbxContractor.DisplayMember = "پیمانکار";
            this.cbxContractor.SelectedIndex = -1;
        }

        private void refreshComboBoxRegion()
        {
            DataTable table = objClsSql.FillDataTableFromQuery("SELECT [منطقه] FROM [DB].[dbo].tblDBSummary  GROUP BY [منطقه]  HAVING [منطقه]!=N''");
            this.cbxDbNewRegion.DataSource = table;
            this.cbxDbNewRegion.DisplayMember = "منطقه";
            this.cbxDbNewRegion.SelectedIndex = -1;
        }

        private void TxtDbNewLocation_Leave(object sender, EventArgs e)
        {
            if (txtDbNewLocation.TextLength < 4 && txtDbNewLocation.TextLength!=0)
            {
                MessageBox.Show("دقت کن ! لوکیشن چهار رقمی است");
                txtDbNewLocation.Focus();
            }

            if (txtDbNewLocation.TextLength == 4)
            {
                DataRow[] foundAuthors = dtNewSite.Select("لوکیشن ='ES" + txtDbNewLocation.Text + "'");
                if (foundAuthors.Length == 1)
                {
                    MessageBox.Show("چیو میخوای اضافه کنی این که در جدول مقابل وجود داره");
                    txtDbNewLocation.Focus();
                    return;
                }
            }


        }
                              
        private void TxtDbNewSiteName_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void TextBox2_MouseEnter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void BtnDbAddNewSite_Click(object sender, EventArgs e)
        {
            if (txtDbNewLocation.TextLength != 4)
            {
                MessageBox.Show("لطفا تعداد ارقام لوکیشن را چک کن");
                txtDbNewLocation.Focus();
                return;
            }
           

            if (txtDbNewSiteName.TextLength < 3)
            {
                MessageBox.Show("لطفا تعداد حروف نام سایت را چک کن");
                txtDbNewSiteName.Focus();
                return;
            }

            if (txtDbNewSiteName.Text =="NEW")
            {
                MessageBox.Show("لطفا یک نام صحیح برای سایت انتخاب کن");
                txtDbNewSiteName.Focus();
                return;
            }

            bool bolOkInsert = AddNewTempSite("ES"+txtDbNewLocation.Text, txtDbNewSiteName.Text, cbxContractor.Text, cbxDbNewRegion.Text, txtDbNewAddress.Text);

            if (bolOkInsert)
            {
                MessageBox.Show("موفق شدی سایت جدید رو اضافه کنی .اسم سایت "+ txtDbNewSiteName.Text+" دمت گرم");
                
                txtDbNewLocation.Clear();
                txtDbNewSiteName.Clear();
                cbxContractor.Refresh();
                cbxDbNewRegion.Refresh();
                txtDbNewAddress.Clear();
                refreshDbNewSiteGrid();
                ClsDiff.writeLogOfAction(" یه سایت جدید"+txtDbNewSiteName.Text+" به سامانه اضافه شد  " + txtDbNewSiteName.Text);
            }
            else
            {
                MessageBox.Show("نشد که نشد . به مدیر سیستم خبر بده");

                ClsDiff.writeLogOfAction(" عدم موفقیت در اضافه کردن سایت جدید " + txtDbNewSiteName.Text);
            }
          
        }
               
        private bool AddNewTempSite(string strLocation, string strSiteName, string strContractor, string strRegion, string strAddress)
        {

            string[] strValue = { strLocation, strSiteName, strContractor, strRegion, strAddress };
            string[] strParam = { "location", "siteName", "contractor", "region", "address" };
            bool bolInsertOk =objClsSql.Insert("PE.dbo.tblTempNewSite", strParam, strValue);
            return bolInsertOk;
        }

        private void refreshDbNewSiteGrid()
        {

            try
            {
                gridControlDbNewSite.DataSource =dtNewSite= objClsSql.FillDataTableFromQuery(@"SELECT  [location] AS لوکیشن
      ,[siteName] AS [نام سایت]
      ,[contractor]AS پیمانکار
      ,[region] AS [منطقه]
      ,[address] AS [آدرس]
	    FROM [PE].[dbo].[tblTempNewSite] ");
                this.gridViewDbNewSite.Columns["لوکیشن"].Width = 70;
                this.gridViewDbNewSite.Columns["نام سایت"].Width = 110;
                this.gridViewDbNewSite.Columns["پیمانکار"].Width = 130;
                this.gridViewDbNewSite.Columns["منطقه"].Width = 75;;
                this.gridViewDbNewSite.Columns["آدرس"].Width = 260;

            }
            catch (Exception)
            {
                MessageBox.Show(" اطلاعات  سایتهای جدید از دیتا بیس واکشی نشد  مراتب را به مدیر سامانه اطلاع دهید  ");
                return;
            }
        }

        private void EnabledOrDisableDelBtn()
        {
            if (gridViewDbNewSite.SelectedRowsCount == 1)
            {
                btnDbDelSelectedNewSite.Enabled = true;
            }
            else
            {
                btnDbDelSelectedNewSite.Enabled = false;
            }
        }

        private void BtnDbDelSelectedNewSite_Click(object sender, EventArgs e)
        {



            if (MessageBox.Show(" سطر انتخابی حذف ؟ ", "آلارم",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)

            {

                string strLocation = gridViewDbNewSite.GetFocusedRowCellValue("لوکیشن").ToString();

                int intContRecordBeforDel = objClsSql.Count("PE.dbo.tblTempNewSite","");
               objClsSql.DeleteRecords("PE.dbo.tblTempNewSite", "WHERE location=N'" + strLocation + "'");
                int intContRecordAfterDel = objClsSql.Count("PE.dbo.tblTempNewSite","");

                if (intContRecordBeforDel== intContRecordAfterDel+1)
                
                {
                    MessageBox.Show("سطر جدول سایتهای جدید با لوکیشن " + strLocation+" حذف شد");

                    ClsDiff.writeLogOfAction("سطر جدول سایتهای جدید با لوکیشن " + strLocation + " حذف شد");
                }
                else
                {
                    MessageBox.Show("سطر انتخابی حذف نشده ببین چی شده ! با مدیر سامانه بتماس");
                    ClsDiff.writeLogOfAction("عدم موفقیت در سطر انتخابی NEW SITE");

                }

                refreshDbNewSiteGrid();
            }

        }

        private void BtnDbDelAllNewSite_Click(object sender, EventArgs e)
        {


            if (MessageBox.Show(" همه حذف ؟ ","آلارم",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.Yes)
         
            {
                objClsSql.DeleteAllRecords("PE.dbo.tblTempNewSite");

                if (objClsSql.Count("PE.dbo.tblTempNewSite","") == 0)
                {
                    MessageBox.Show("همش New Site حذف شد");
                    ClsDiff.writeLogOfAction("  کل جدول سایتهای جدید حذف شد");
                }
                else
                {
                    MessageBox.Show("همش حذف نشده ببین چی شده ! با مدیر سامانه بتماس");
                    ClsDiff.writeLogOfAction("عدم موفقیت در حذف کل جدول NEW SITE");

                }

                refreshDbNewSiteGrid();
            }

        }

        private void btnDbExportNewPlan_Click(object sender, EventArgs e)
        {
            this.saveFileDialogDbExportNewPln.ShowDialog();

            if (this.saveFileDialogDbExportNewPln.FileName != string.Empty)
            {
                this.gridViewDbNewSite.ExportToXlsx(this.saveFileDialogDbExportNewPln.FileName);
            }
            else
            {
                MessageBox.Show("مقصد جهت ذخیره انتخاب نشده");
            }
        }

        private void gridControlDbNewSite_KeyDown(object sender, KeyEventArgs e)
        {
            EnabledOrDisableDelBtn();
        }

        private void gridControlDbNewSite_Click(object sender, EventArgs e)
        {
            EnabledOrDisableDelBtn();
        }
        
    }
}
