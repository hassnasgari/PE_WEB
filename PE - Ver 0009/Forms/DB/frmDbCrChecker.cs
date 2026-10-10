using DevExpress.XtraGrid;
using OfficeOpenXml;
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

namespace PE.Forms.DB
{
    public partial class frmDbCrChecker : Form
    {
        ClsSql objClsSql = new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال
        DataTable addableDataTable = new DataTable();
        public frmDbCrChecker()
        {
            InitializeComponent();
        }

        private void frmDbCrChecker_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void readFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addableDataTable = new DataTable();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    using (ExcelPackage package = new ExcelPackage(fileInfo))
                    {
                        // خواندن نام شیت از TextBox
                        string sheetName = txtSheetName.Text.Trim();

                        // بررسی وجود شیت با نام مشخص شده
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                        if (worksheet == null)
                        {
                            MessageBox.Show($"Worksheet with name '{sheetName}' does not exist.");
                            return;
                        }


                        // افزودن ستون‌ها به DataTable
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            string columnName = worksheet.Cells[1, col].Text;
                            addableDataTable.Columns.Add(columnName); //
                        }

                        // افزودن ردیف‌ها به DataTable
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            DataRow dataRow = addableDataTable.NewRow();
                            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                            {
                                string cellValue = worksheet.Cells[row, col].Text;


                                dataRow[col - 1] = cellValue; // اگر موفق بود، مقدار را ذخیره کن

                            }
                            addableDataTable.Rows.Add(dataRow);
                        }


                        //حذف ستون هشتگ یا خالی
                        delHashtagAndColumn1(addableDataTable);


                        //مقایسه با قبلی ها و فرستادن به دیتا بیس

                        CompairAndInsertToDb();

                        // نمایش داده‌ها در GridControl DevExpress

                        RefreshGrid();
                    }
                }
            }
        }
        private DataTable delHashtagAndColumn1(DataTable dt)
        {
            // حذف ستون‌ها بر اساس شرایط
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                string columnName = dt.Columns[i].ColumnName;

                // بررسی اینکه آیا عنوان خالی است یا شامل علامت #
                if (columnName.Contains("Column1") || columnName.Contains("#"))
                {
                    dt.Columns.RemoveAt(i);
                }
            }

            return dt;
        }


            private void CompairAndInsertToDb()
        {
            string strSql = @"SELECT 
              [Ticket ID]
              ,[Title]
              ,[Workflow Type]
              ,[Status]
              ,[State]
              ,[Is overtime]
              ,[Dispatched by]
              ,[Dispatch Time]
              ,[Last Modification Time]
               FROM [CRC].[dbo].[tblTotalCr]";

            DataTable TotalDataTable = objClsSql.FillDataTableFromQuery(strSql);
            TotalDataTable = MergeDataTables(TotalDataTable, addableDataTable);


            //حذف کل جدول

            bool bolDelOk = objClsSql.DeleteAllRecords("[CRC].[dbo].[tblTotalCr]");

            // نمایش نتیجه با MessageBox
            // string message = bolDelOk ? "عملیات حذف با موفقیت انجام شد." : "عملیات حذف ناموفق بود.";

            // MessageBox.Show(message, "نتیجه عملیات", MessageBoxButtons.OK, MessageBoxIcon.Information);



            //جایگذاری اطلاعات جدید
            bool bolInsertCrsToDB = objClsSql.BulkInsertDataTable("CRC.dbo.tblTotalCr", TotalDataTable);

            // نمایش نتیجه با MessageBox
            //string message1 = bolInsertCrsToDB ? "عملیات درج با موفقیت انجام شد." : "عملیات درج ناموفق بود.";

            RefreshGrid();

            // MessageBox.Show(message1, "نتیجه عملیات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshGrid()
        {
            string strSql = @"
            SELECT [Ticket ID]
          ,[Title]
          ,[Workflow Type]
          ,[Status]
          ,[State]
          ,[Is overtime]
          ,[Dispatched by]
          ,[Dispatch Time]
          ,[Last Modification Time]
            FROM [CRC].[dbo].[tblTotalCr]
              ORDER BY [Last Modification Time] DESC;
            ";
            gridControl1.DataSource = objClsSql.FillDataTableFromQuery(strSql);
        }
        private DataTable MergeDataTables(DataTable TotalDataTable, DataTable addableData)
        {

            foreach (DataRow addRow in addableData.Rows)
            {
                string ticketId = addRow["Ticket ID"].ToString(); // تبدیل به رشته
                DataRow existingRow = TotalDataTable.Select($"[Ticket ID] = '{ticketId}'").FirstOrDefault(); // مقایسه به عنوان رشته

                if (existingRow == null)
                {
                    // اگر رکورد وجود ندارد، آن را اضافه کنید
                    TotalDataTable.ImportRow(addRow);

                    //  string messages = $"رکورد جدید با Ticket ID: {ticketId} به totalData اضافه شد.";
                    // MessageBox.Show(messages, "رکورد جدید", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    // اگر رکورد وجود دارد، آن را به‌روزرسانی کنید
                    existingRow.ItemArray = addRow.ItemArray; // به‌روزرسانی تمام ستون‌ها
                                                              // MessageBox.Show($"رکورد با Ticket ID: {ticketId} به‌روزرسانی شد.", "به‌روزرسانی رکورد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            return TotalDataTable; // برگرداندن DataTable نهایی
        }


    }
}
