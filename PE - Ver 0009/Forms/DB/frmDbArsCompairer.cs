using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PE.Forms.DB
{
    public partial class frmDbArsCompairer : Form
    {
        public DataTable DataTableNewforAras { get; private set; }
        public DataTable DataTableOldForAras { get; private set; }
        public frmDbArsCompairer()
        {
            InitializeComponent();
        }

        private void frmDbArsCompairer_Load(object sender, EventArgs e)
        {
            // پاک کردن اطلاعات قبلی
            ClearData();

            var fileSelector = new ExcelFileSelector();
            fileSelector.SelectExcelFiles(out string newFilePath, out string oldFilePath);

            // حالا newFilePath و oldFilePath حاوی مسیرهای انتخاب شده هستند
            MessageBox.Show($"فایل جدید: {newFilePath}\nفایل قدیمی: {oldFilePath}", "مسیرهای انتخاب شده", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //// غیرفعال کردن دکمه‌ها
            //EnableButtons(false);

            try
            {
                // خواندن فایل جدید
                labelStatus.Text = "Raeding New File...";
                DataTableNewforAras =  LoadExcelFile(newFilePath);

                // خواندن فایل قدیمی
                labelStatus.Text = "Raeding Old File...";
                DataTableOldForAras =  LoadExcelFile(oldFilePath);



                gridControlNew.DataSource = DataTableNewforAras;
                gridControlOld.DataSource = DataTableOldForAras;
            }
            finally
            {
                // فعال کردن دکمه‌ها و پنهان کردن Progress Bar
               // EnableButtons(true);
                labelStatus.Text = "";
                // نمایش پیام موفقیت
                MessageBox.Show("فایل‌ها با موفقیت بارگذاری شدند در صورت نیاز به تحلیل یکی از منوی های بالا را انتخاب کنید.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // متد LoadExcelFile برای خواندن یک فایل اکسل
        private DataTable LoadExcelFile(string filePath)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("کد سایت", typeof(string));
            dataTable.Columns.Add("نام سایت", typeof(string));
            dataTable.Columns.Add("آدرس", typeof(string));

            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // خواندن اولین شیت (بدون نیاز به نام شیت)
                        if (reader.Read())
                        {
                            // ایجاد دیکشنری برای نگهداری ایندکس ستون‌ها
                            var columnIndexes = new Dictionary<string, int>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetValue(i)?.ToString();
                                if (!string.IsNullOrEmpty(columnName))
                                {
                                    columnIndexes[columnName] = i; // ذخیره ایندکس ستون‌ها
                                }
                            }

                            // خواندن داده‌ها
                            while (reader.Read()) // خواندن هر ردیف
                            {
                                var newRow = dataTable.NewRow();

                                // استفاده از ایندکس برای دسترسی به مقادیر
                                if (columnIndexes.TryGetValue("کد سایت", out int index1))
                                    newRow["کد سایت"] = reader.GetValue(index1)?.ToString();

                                if (columnIndexes.TryGetValue("نام سایت", out int index2))
                                    newRow["نام سایت"] = reader.GetValue(index2)?.ToString();

                                if (columnIndexes.TryGetValue("آدرس", out int index3))
                                    newRow["آدرس"] = reader.GetValue(index3)?.ToString();

                                dataTable.Rows.Add(newRow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // نمایش پیام خطا در صورت بروز مشکل
                MessageBox.Show($"خطا در بارگذاری فایل: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dataTable;
        }

        //private void EnableButtons(bool enable)
        //{
        //    // نمونه: فعال/غیرفعال کردن تمامی دکمه‌های فرم
        //    foreach (Control control in this.Controls)
        //    {
        //        if (control is Button)
        //        {
        //            control.Enabled = enable;
        //        }
        //    }

        //    //// غیرفعال/فعال کردن آیتم‌های منو
        //    //foreach (ToolStripItem item in مقایسهارسجدیدباقدیمToolStripMenuItem.Items)
        //    //{
        //    //    item.Enabled = enable;
        //    //}
        //}

        // متد برای پاک کردن اطلاعات قبلی
        private void ClearData()
        {
            // پاک کردن DataTableNew
            if (DataTableNewforAras != null)
            {
                DataTableNewforAras.Clear();
                DataTableNewforAras.Dispose();
                DataTableNewforAras = null;
            }

            // پاک کردن DataTableOld
            if (DataTableOldForAras != null)
            {
                DataTableOldForAras.Clear();
                DataTableOldForAras.Dispose();
                DataTableOldForAras = null;
            }

            // پاک کردن DataSource گریدها
            gridControlNew.DataSource = null;
            gridControlOld.DataSource = null;
            gridControlDiff.DataSource = null;
            gridControlOnlyNew.DataSource = null;
            gridControlOnlyOld.DataSource = null;

            // اینجا میتونید بقیه متغیرها و کنترلها رو هم پاک کنید
        }

        public class ExcelFileSelector
        {
            public void SelectExcelFiles(out string newFilePath, out string oldFilePath)
            {
                newFilePath = string.Empty;
                oldFilePath = string.Empty;

                // ایجاد دیالوگ انتخاب فایل
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xls;*.xlsx"; // فیلتر برای فایل‌های اکسل
                    openFileDialog.Title = "انتخاب فایل اکسل جدید";

                    // انتخاب فایل جدید
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        newFilePath = openFileDialog.FileName; // ذخیره مسیر فایل جدید
                    }
                    else
                    {
                        MessageBox.Show("فایل جدید انتخاب نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // خروج از متد اگر فایل انتخاب نشد
                    }
                }

                // ایجاد دیالوگ انتخاب فایل برای فایل قدیمی
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files|*.xls;*.xlsx"; // فیلتر برای فایل‌های اکسل
                    openFileDialog.Title = "انتخاب فایل اکسل قدیمی";

                    // انتخاب فایل قدیمی
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        oldFilePath = openFileDialog.FileName; // ذخیره مسیر فایل قدیمی
                    }
                    else
                    {
                        MessageBox.Show("فایل قدیمی انتخاب نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // خروج از متد اگر فایل انتخاب نشد
                    }
                }
            }
        }

        private void compairForNameOrAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dtDiff = CompareDataTables(DataTableNewforAras, DataTableOldForAras);
            gridControlDiff.DataSource = dtDiff;
            compairForNameOrAddToolStripMenuItem.Enabled = false;   
        }
        public DataTable CompareDataTables(DataTable newTable, DataTable oldTable)
        {
            // ایجاد DataTableDiff
            DataTable dataTableDiff = new DataTable();
            dataTableDiff.Columns.Add("کد سایت", typeof(string));
            dataTableDiff.Columns.Add("نام سایت جدید", typeof(string));
            dataTableDiff.Columns.Add("نام سایت قدیمی", typeof(string));
            dataTableDiff.Columns.Add("آدرس جدید", typeof(string));
            dataTableDiff.Columns.Add("آدرس قدیمی", typeof(string));

            // مقایسه رکوردها
            foreach (DataRow newRow in newTable.Rows)
            {
                string codeSite = newRow["کد سایت"].ToString();
                // جستجوی کد سایت در DataTableOld
                var oldRow = oldTable.AsEnumerable()
                    .FirstOrDefault(r => r.Field<string>("کد سایت") == codeSite);

                if (oldRow != null) // اگر کد سایت در DataTableOld وجود دارد
                {
                    string newSiteName = newRow["نام سایت"].ToString();
                    string oldSiteName = oldRow["نام سایت"].ToString();
                    string newAddress = newRow["آدرس"].ToString();
                    string oldAddress = oldRow["آدرس"].ToString();

                    // بررسی مغایرت‌ها
                    if (newSiteName != oldSiteName || newAddress != oldAddress)
                    {
                        // اضافه کردن رکورد به DataTableDiff
                        var diffRow = dataTableDiff.NewRow();
                        diffRow["کد سایت"] = codeSite;
                        diffRow["نام سایت جدید"] = newSiteName;
                        diffRow["نام سایت قدیمی"] = oldSiteName;
                        diffRow["آدرس جدید"] = newAddress;
                        diffRow["آدرس قدیمی"] = oldAddress;

                        dataTableDiff.Rows.Add(diffRow);
                    }
                }
            }

            return dataTableDiff; // برگرداندن DataTableDiff
        }

        private void compairForCodeSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var (newOnly, oldOnly) = CompareSiteCodes(DataTableNewforAras, DataTableOldForAras);


            gridControlOnlyNew.DataSource = newOnly;
            gridControlOnlyOld.DataSource = oldOnly;

            compairForCodeSiteToolStripMenuItem.Enabled = false;

        }
        public (DataTable newOnly, DataTable oldOnly) CompareSiteCodes(DataTable newTable, DataTable oldTable)
        {

            // ایجاد DataTable برای کد سایت‌هایی که فقط در DataTableNew هستند
            DataTable newOnly = new DataTable();
            newOnly.Columns.Add("کد سایت", typeof(string));
            newOnly.Columns.Add("نام سایت", typeof(string)); // اضافه کردن ستون نام سایت

            // ایجاد DataTable برای کد سایت‌هایی که فقط در DataTableOld هستند
            DataTable oldOnly = new DataTable();
            oldOnly.Columns.Add("کد سایت", typeof(string));
            oldOnly.Columns.Add("نام سایت", typeof(string)); // اضافه کردن ستون نام سایت

            // پیدا کردن کد سایت‌هایی که فقط در DataTableNew هستند
            foreach (DataRow newRow in newTable.Rows)
            {
                string codeSite = newRow["کد سایت"].ToString();
                string siteName = newRow["نام سایت"].ToString(); // استخراج نام سایت

                // بررسی اینکه آیا کد سایت در DataTableOld وجود دارد یا نه
                var existsInOld = oldTable.AsEnumerable()
                    .Any(r => r.Field<string>("کد سایت") == codeSite);

                if (!existsInOld) // اگر در old نیست
                {
                    var newOnlyRow = newOnly.NewRow();
                    newOnlyRow["کد سایت"] = codeSite;
                    newOnlyRow["نام سایت"] = siteName; // اضافه کردن نام سایت
                    newOnly.Rows.Add(newOnlyRow);
                }
            }

            // پیدا کردن کد سایت‌هایی که فقط در DataTableOld هستند
            foreach (DataRow oldRow in oldTable.Rows)
            {
                string codeSite = oldRow["کد سایت"].ToString();
                string siteName = oldRow["نام سایت"].ToString(); // استخراج نام سایت

                // بررسی اینکه آیا کد سایت در DataTableNew وجود دارد یا نه
                var existsInNew = newTable.AsEnumerable()
                    .Any(r => r.Field<string>("کد سایت") == codeSite);

                if (!existsInNew) // اگر در new نیست
                {
                    var oldOnlyRow = oldOnly.NewRow();
                    oldOnlyRow["کد سایت"] = codeSite;
                    oldOnlyRow["نام سایت"] = siteName; // اضافه کردن نام سایت
                    oldOnly.Rows.Add(oldOnlyRow);
                }
            }

            return (newOnly, oldOnly); // برگرداندن دو دیتاتیبل
        }
    }
}
