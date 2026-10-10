using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PE.Forms.DB
{
    public partial class frmDbSearch : Form
    {
        ClsSql objClsSql = new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال
        string strSiteCode = string.Empty;
        public frmDbSearch()
        {
            InitializeComponent();
        }

        private void frmDbSearch_Load(object sender, EventArgs e)
        {
            radioGroupTypeOfSearch_SelectedIndexChanged(sender, e);
        }

        private void radioGroupTypeOfSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroupTypeOfSearch.SelectedIndex == 0)
            {
                txtCodeSiteForSearch.Enabled = true;
                ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));
                txtCodeSiteForSearch.Focus();
                txtSiteNameForSearch.Clear();
                txtSiteNameForSearch.Enabled = false;
                lblSiteName.Enabled=false;
                lblEs.Enabled = true;

                this.txtCodeSiteForSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            }
            else
            {
                txtSiteNameForSearch.Enabled = true;
                txtCodeSiteForSearch.Enabled = false;
                txtSiteNameForSearch.Focus();
                txtCodeSiteForSearch.Clear();
                ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));

                this.txtSiteNameForSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                lblSiteName.Enabled = true;
                lblEs.Enabled = false;
            }
        }

        private void txtCodeSiteForSearch_TextChanged(object sender, EventArgs e)
       {
            txtCodeSiteForSearch.Focus();

            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));

            if (txtCodeSiteForSearch.TextLength == 4)
            {
                strSiteCode = "ES" + txtCodeSiteForSearch.Text;

                btnDbSearch.PerformClick();

            }
            else
            {
                if (txtCodeSiteForSearch.Text.Length > 4)
                {
                    MessageBox.Show("کد سایت باید یک عدد 4 رقمی باشد");
                    txtCodeSiteForSearch.Text = txtCodeSiteForSearch.Text.Remove(txtCodeSiteForSearch.Text.Length - 1, 1);
                }
                if(txtCodeSiteForSearch.Text.Length ==2)
                {
                    #region پاک کردن ساختار گرید یرای بارگذاری جدید
                    clearDbSearch();
                    #endregion
                }
            }
        }

        private void btnDbSearch_Click(object sender, EventArgs e)
        {
            if (txtCodeSiteForSearch.Text.Length == 4 || txtSiteNameForSearch.Text != string.Empty)
            {
                //search
                SearchInDb();
            }
            else
            {
                if (radioGroupTypeOfSearch.SelectedIndex == 0 && txtCodeSiteForSearch.TextLength < 4)
                {
                    string message = "  لطفا کد سایت را به صورت یک عدد چهار رقمی وارد کنید";
                    MessageBox.Show(message, " اخطار کد سایت ", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }
                else if (txtSiteNameForSearch.Text == string.Empty)
                {

                    string message1 = "  لطفا نام سایت مورد جستجو را وارد کنید";
                    MessageBox.Show(message1, " اخطار نام سایت ", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }

            }
        }

        private void txtSiteNameForSearch_TextChanged(object sender, EventArgs e)
        {
            // Calculate the required width based on the text
            int textWidth = TextRenderer.MeasureText(txtSiteNameForSearch.Text, txtSiteNameForSearch.Font).Width;

            // Set the width of the text box
            txtSiteNameForSearch.Width = textWidth;

            if (txtSiteNameForSearch.Text.Length == 16)
            {
                #region پاک کردن ساختار گرید یرای بارگذاری جدید
                clearDbSearch();
                #endregion

                
            }
            else if(textWidth < 16)
            {
                txtSiteNameForSearch.Width = 16;
            }

        }

        private void frmDbSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        private void SearchInDb()
        {
            #region پاک کردن ساختار گرید یرای بارگذاری جدید
            clearDbSearch();
            #endregion

            #region واکشی
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();

            string strQuery = "";
            if (radioGroupTypeOfSearch.SelectedIndex == 0)

            {
                strQuery = @"select * From [DB].[dbo].[tblDb] Where [کد سایت]=N'ES" + txtCodeSiteForSearch.Text + "';";
            }
            else
            {
                strQuery = @"select * From [DB].[dbo].[tblDb] Where [نام سایت]=N'" + txtSiteNameForSearch.Text + "';"; ;
            }

            DataTable dtResaultSearch = objClsSql.FillDataTableFromQuery(strQuery);
            gridControlDbSearch.DataSource = dtResaultSearch;

            //if(txtSiteNameForSearch.Text=="" && radioGroupTypeOfSearch.SelectedIndex == 0 && dtResaultSearch.Rows.Count!=0)
            //{
            //}
            #endregion

            #region فریز 5 ستون راست
            gridViewDbSearch.Columns["کد سایت"].Fixed = FixedStyle.Left;
            gridViewDbSearch.Columns["نام سایت"].Fixed = FixedStyle.Left;
            gridViewDbSearch.Columns["پیمانکار"].Fixed = FixedStyle.Left;
            gridViewDbSearch.Columns["BSC/RNC"].Fixed = FixedStyle.Left;
            gridViewDbSearch.Columns["BSC&RNC"].Fixed = FixedStyle.Left;
            gridViewDbSearch.Columns["منطقه"].Fixed = FixedStyle.Left;
            gridViewDbSearch.Columns["پیمانکار"].Fixed = FixedStyle.Left;
            #endregion

            #region تنظیم دستی بعضی از ستون ها
            gridViewDbSearch.Columns["آدرس"].Width = 400;
            gridViewDbSearch.Columns["نام سایت"].Width = 150;
            gridViewDbSearch.Columns["پیمانکار"].Width = 150;
            #endregion

            int intCntOfFind = dtResaultSearch.Rows.Count;
            if (intCntOfFind > 0) 
            {
                toolStripStatusLabelSearch.Text = intCntOfFind + " مورد یافت شد  " ;
            }
            else
            {
                toolStripStatusLabelSearch.Text = "  موردی یافت نشد";
            }
        }

        private void clearDbSearch()
        {
            gridViewDbSearch.Columns.Clear();
            gridViewDbSearch.PopulateColumns();
            gridViewDbSearch.RefreshData();

            gridControlDbSearch.DataSource = null;
            gridControlDbSearch.RefreshDataSource();
            gridControlDbSearch.Refresh();
        }

        private void txtSiteNameForSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchInDb();
            }
        }

        private void gridViewDbSearch_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridViewDbSearch.GetFocusedDataRow() == null)
            {
                txtSiteNameForSearch.Clear();
                return;
            }
            if (gridViewDbSearch.SelectedRowsCount == 0)
            {
                txtSiteNameForSearch.Clear();
                return;
            }
            BeginInvoke(new MethodInvoker(delegate
            {

                //txtSiteNameForSearch.Text = gridViewDbSearch.GetFocusedDataRow()["نام سایت"].ToString();

            }
                ));
        }

        private void btnHowOnMap_Click(object sender, EventArgs e)
        {
            DataRow dataRow = this.gridViewDbSearch.GetDataRow(this.gridViewDbSearch.FocusedRowHandle);
            MainForm.StrDbLat = MainForm.StrDbLong = string.Empty;

            if (dataRow != null)
            {

                MainForm.StrDbLat = dataRow["عرض جغرافیایی"].ToString();
                MainForm.StrDbLong = dataRow["طول جغرافیایی"].ToString();

            }

            if (MainForm.StrDbLat == string.Empty || MainForm.StrDbLong == string.Empty)
            {
                MessageBox.Show(" هیچ سایتی از دیتابیس انتخاب نشده . لطفا یکی از سایتها را انتخاب و از خالی نبودن مقدار طول و عرض جغرافیایی آن مطمئن شوید ");
            }
            else
            {
                frmDbMaps frmMap = new frmDbMaps();
                frmMap.ShowDialog();
            }
        }
    }
}
