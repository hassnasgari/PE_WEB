using DevExpress.XtraGrid.Columns;
using PE.Forms.DT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PE.Forms
{
    public partial class frmDtMultiChange : Form
    {
        ClsSql objClsSql;
        bool bolIsCorrectDtNoFormat = false;

        public frmDtMultiChange()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void frmDtMultiChange_Load(object sender, EventArgs e)
        {
            //// تنظیمات اولیه کامبوباکس‌ها
            //cbxDtChgGrpStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            //cbxDtChgGrpActivity.DropDownStyle = ComboBoxStyle.DropDownList;

            // مخفی کردن کنترل‌های اضافی
            txtEditedShroo.Visible = false;
            lblTo.Visible = false;
            txtEditedShroo.Text = string.Empty;
            txtRowOfTadakhol.Visible = false;
            txtEditedPayan.Visible = false;

            // لاگ ورود
            ClsDiff.writeLogOfAction("ورود به صفحه فرم تغییرات گروهی دان تایم");

            // دسترسی ادمین
            gbxDtChgGrpAdmin.Enabled = (MainForm.StrCurRol == "administrator");

            // رفرش کردن کامبوباکس‌ها
            refreshCbxDtChgGrpPerformer();
            refreshCbxDtChgGrpSubject();
            refreshCbxtxtOutage();

            // خالی کردن شماره دان تایم
            cbxDtChgGrpDtNo.Text = string.Empty;

            // تنظیم دیتاسورس گرید
            gridControlDtSelected.DataSource = MainForm.DtSelectedDt;

            // تنظیم عرض ستون‌ها
            SetColumnWidths();

            // انتخاب اولین ردیف
            if (gridViewDtSelected.RowCount > 0)
            {
                gridViewDtSelected.FocusedRowHandle = 0;
                gridViewDtSelected.SelectRow(0);
            }

            // ذخیره وضعیت ویرایش
            SaveStatusToEditting();
        }

        private void SetColumnWidths()
        {
            var columns = new Dictionary<string, int>
            {
                {"ردیف", 55}, {"تاریخ ثبت", 70}, {"ثبت", 50}, {"کد سایت", 60},
                {"نام سایت", 120}, {"منطقه", 60}, {"تاریخ شروع", 70}, {"زمان شروع", 50},
                {"تاریخ پایان", 70}, {"زمان پایان", 50}, {"درخواست کننده", 90}, {"موضوع", 100},
                {"انجام دهنده", 70}, {"ثبت کننده", 70}, {"شماره", 110}, {"وضعیت", 80},
                {"فعالیت", 80}, {"یادداشت", 120}
            };

            foreach (var col in columns)
            {
                if (this.gridViewDtSelected.Columns[col.Key] != null)
                    this.gridViewDtSelected.Columns[col.Key].Width = col.Value;
            }
        }

        private void SaveStatusToEditting()
        {
            foreach (DataRow row in MainForm.DtSelectedDt.Rows)
            {
                string strCurStatus = row["وضعیت"].ToString();
                if (!strCurStatus.Contains(" و در حال ویرایش توسط "))
                {
                    string strSqlForUpdateStatus = $@"
                        UPDATE [DT].[dbo].[tblDownTimeShort]
                        SET status += N' و در حال ویرایش توسط ' + N' {MainForm.StrFullNameCurUsr} '
                        WHERE row = N'{row["ردیف"]}'";
                    objClsSql.SendCommand(strSqlForUpdateStatus);
                }
            }
        }

        private void refreshCbxDtChgGrpSubject()
        {
            DataTable table = objClsSql.FillDataTableFromQuery("SELECT subject FROM [DT].[dbo].tblDownTimeShort GROUP BY subject");
            cbxDtChgGrpSubject.DataSource = table;
            cbxDtChgGrpSubject.DisplayMember = "subject";
        }

        private void refreshCbxDtChgGrpPerformer()
        {
            DataTable table = objClsSql.FillDataTableFromQuery("SELECT Name FROM [Tel].[dbo].tblTel");
            cbxDtChgGrpPerformer.DataSource = table;
            cbxDtChgGrpPerformer.DisplayMember = "Name";
        }

        private void refreshCbxtxtOutage()
        {
            var Interval = new List<int> { 0, 15, 30, 45, 60, 75, 90, 105, 120, 150, 180, 210, 240, 270, 300, 330, 360, 390, 420, 450, 480, 510, 540, 570, 600, 630, 660, 690, 720, 780, 840, 900, 960, 1020, 1080, 1140, 1200, 1260, 1320, 1380, 1440, 1500, 1560, 1620, 1680, 1740, 1800, 1860, 1920, 1980, 2040, 2100, 2160, 2220, 2280, 2340, 2400, 2460, 2520, 2580, 2640, 2700, 2760, 2820, 2880, 2940, 3000, 3060, 3120, 3180, 3240, 3300, 3360, 3420, 3480, 3540, 3600, 3660, 3720, 3780, 3840, 3900, 3960, 4020, 4080, 4140, 4200, 4260, 4320, 4380, 4440, 4500, 4560, 4620, 4680, 4740, 4800, 4860, 4920, 4980, 5040, 5100, 5160, 5220, 5280, 5340, 5400, 5460, 5520, 5580, 5640, 5700, 5760, 5820, 5880, 5940, 6000, 6060, 6120, 6180, 6240, 6300, 6360, 6420, 6480, 6540, 6600, 6660, 6720, 6780, 6840, 6900, 6960, 7020, 7080, 7140, 7200, 7260, 7320, 7380, 7440, 7500, 7560, 7620, 7680, 7740, 7800, 7860, 7920, 7980, 8040, 8100, 8160, 8220, 8280, 8340, 8400, 8460, 8520, 8580, 8640, 8700, 8760, 8820, 8880, 8940, 9000, 9060, 9120, 9180, 9240, 9300, 9360, 9420, 9480, 9540, 9600, 9660, 9720, 9780, 9840, 9900, 9960, 10020, 10080 };
            List<string> lst = Interval.Select(item => item.ToString()).ToList();
            listBoxDtOutageMin.DataSource = null;
            listBoxDtOutageMin.Items.Clear();
            listBoxDtOutageMin.DataSource = lst;
        }

        private void gridViewDtSelected_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int[] selectedRows = gridViewDtSelected.GetSelectedRows();
                if (selectedRows == null || selectedRows.Length == 0) return;

                int selectedRowHandle = selectedRows[0];

                foreach (GridColumn column in gridViewDtSelected.Columns)
                {
                    string columnName = column.ToString();
                    string strColumn = gridViewDtSelected.GetRowCellValue(selectedRowHandle, column)?.ToString() ?? string.Empty;

                    // برای دیباگ: می‌توانید لاگ بگیرید تا ببینید چه ستون‌هایی وجود دارد
                    // System.Diagnostics.Debug.WriteLine($"Column: '{columnName}' = '{strColumn}'");

                    switch (columnName)
                    {
                        case "ردیف":
                            txtDtChgGrpRow.Text = strColumn;
                            break;
                        case "منطقه":
                            txtDtChgGrpRegion.Text = strColumn;
                            break;
                        case "تاریخ ثبت":
                            txtDtChgGrpRegDate.Text = strColumn;
                            break;
                        case "ثبت":
                            txtDtChgGrpRegTime.Text = strColumn;
                            break;
                        case "کد سایت":
                            txtDtChgGrpSiteCode.Text = strColumn;
                            break;
                        case "نام سایت":
                            txtDtChgGrpSiteName.Text = strColumn;
                            break;
                        case "تاریخ شروع":
                            txtDtChgGrpStartDate.Text = strColumn;
                            break;
                        case "زمان شروع":
                            txtDtChgGrpStartTime.Text = strColumn;
                            if (!string.IsNullOrEmpty(txtDtChgGrpRow.Text))
                            {
                                string strOutage = objClsSql.SendCommandGetStringResult($"USE DT; SELECT [outageMin] FROM tblDownTimeShort WHERE row = N'{txtDtChgGrpRow.Text}'");
                                if (strOutage.Contains("_YES"))
                                {
                                    radioButtonIsInBusyHour.Checked = true;
                                    strOutage = strOutage.Replace("_YES", "");
                                }
                                else if (strOutage.Contains("_NO"))
                                {
                                    radioButtonIsInBusyHour.Checked = false;
                                    strOutage = strOutage.Replace("_NO", "");
                                }
                                if (listBoxDtOutageMin.Items.Contains(strOutage))
                                    listBoxDtOutageMin.Text = strOutage;
                            }
                            break;
                        case "تاریخ پایان":
                            txtDtChgGrpEndDate.Text = strColumn;
                            break;
                        case "زمان پایان":
                            txtDtChgGrpEndTime.Text = strColumn;
                            break;
                        case "درخواست کننده":
                            txtDtChgGrpApplicant.Text = strColumn;
                            break;
                        case "موضوع":
                            cbxDtChgGrpSubject.Text = strColumn;
                            break;
                        case "انجام دهنده":
                            cbxDtChgGrpPerformer.Text = strColumn;
                            break;
                        case "ثبت کننده":
                            if (string.IsNullOrEmpty(strColumn))
                            {
                                cbxDtChgGrpApprover.Text = MainForm.StrFullNameCurUsr;
                                cbxDtChgGrpApprover.ForeColor = Color.DarkGreen;
                            }
                            else
                            {
                                cbxDtChgGrpApprover.Text = strColumn;
                                cbxDtChgGrpApprover.ForeColor = Color.Black;
                            }
                            break;
                        case "شماره":
                            cbxDtChgGrpDtNo.Text = strColumn;
                            break;

                        case "وضعیت":
                            if (strColumn == "ثبت توسط کاربر")
                            {
                                cbxDtChgGrpStatus.Text = "ثبت در (SDM)";
                                cbxDtChgGrpStatus.ForeColor = Color.BlueViolet;
                            }
                            else
                            {
                                cbxDtChgGrpStatus.Text = strColumn;
                                cbxDtChgGrpStatus.ForeColor = Color.Black;
                            }
                            break;

                        case "یادداشت":
                            // تنظیم یادداشت در کامبوباکس یادداشت
                            cbxDtChgGrpNote.Text = strColumn;
                            break;

                        case "فعالیت":
                            cbxDtChgGrpActivity.Text = strColumn;

                            //todo موقتا این بخش رو فعال و با خطوز بالا جایگزین کردم
                            // تنظیم فعالیت در کامبوباکس (با بررسی وجود آیتم)
                            //if (!string.IsNullOrEmpty(strColumn))
                            //{
                            //    bool itemExists = false;
                            //    foreach (var item in cbxDtChgGrpActivity.Items)
                            //    {
                            //        if (item.ToString() == strColumn)
                            //        {
                            //            itemExists = true;
                            //            break;
                            //        }
                            //    }
                            //    if (itemExists)
                            //        cbxDtChgGrpActivity.SelectedItem = strColumn;
                            //    else
                            //        cbxDtChgGrpActivity.SelectedIndex = -1;
                            //}
                            //else
                            //{
                            //    cbxDtChgGrpActivity.SelectedIndex = -1;
                            //}
                            break;



                      
                        default:
                            // در صورت وجود ستون‌های دیگر، کاری نمی‌کند
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ClsDiff.writeLogOfAction($"خطا در FocusedRowChanged: {ex.Message}");
            }
        }

        private void btnDtChgGrpReg_Click(object sender, EventArgs e)
        {
            string strDateTimeNow = ClsConvertDateHassan.convertDateTimeToPersionWithSlash(MainForm.DateTimeSqlServer);
            string strYearNow = strDateTimeNow.Substring(0, 4);
            string strMonthNow = strDateTimeNow.Substring(5, 2);
            string strDayNow = strDateTimeNow.Substring(8, 2);
            string strDateNow = $"{strYearNow}/{strMonthNow}/{strDayNow}";

            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            DialogResult result = MessageBox.Show($"برای تغییر گروهی به تعداد {gridViewDtSelected.RowCount} موافق هستید؟", "تایید", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                // ذخیره ردیف انتخاب شده
                int currentFocusedRowHandle = gridViewDtSelected.FocusedRowHandle;
                object currentRowId = currentFocusedRowHandle >= 0 ? gridViewDtSelected.GetRowCellValue(currentFocusedRowHandle, "ردیف") : null;

                foreach (DataRow row in MainForm.DtSelectedDt.Rows)
                {
                    string sRrow = row.ItemArray[0].ToString();
                    List<object> ListValues = new List<object>();
                    List<string> listEditableFild = new List<string>();

                    // بررسی چک باکس‌ها و اضافه کردن فیلدها
                    if (chbDtChgGrpRow.Checked) { listEditableFild.Add("[row]"); ListValues.Add(txtDtChgGrpRow.Text.Trim()); }
                    if (chbDtChgGrpRegDate.Checked) { listEditableFild.Add("[registerDate]"); ListValues.Add(txtDtChgGrpRegDate.Text.Trim()); }
                    if (chbDtChgGrpRegTime.Checked) { listEditableFild.Add("[registerTime]"); ListValues.Add(txtDtChgGrpRegTime.Text.Trim()); }
                    if (chbDtChgGrpSiteCode.Checked) { listEditableFild.Add("[siteCode]"); ListValues.Add(txtDtChgGrpSiteCode.Text.Trim()); }
                    if (chbDtChgGrpSiteName.Checked) { listEditableFild.Add("[siteName]"); ListValues.Add(txtDtChgGrpSiteName.Text.Trim()); }
                    if (chbDtChgGrpStartDate.Checked) { listEditableFild.Add("[startDate]"); ListValues.Add(txtDtChgGrpStartDate.Text.Trim()); }
                    if (chbDtChgGrpStartTime.Checked) { listEditableFild.Add("[startTime]"); ListValues.Add(txtDtChgGrpStartTime.Text.Trim()); }
                    if (chbDtChgGrpEndDate.Checked) { listEditableFild.Add("[endDate]"); ListValues.Add(txtDtChgGrpEndDate.Text.Trim()); }
                    if (chbDtChgGrpEndTime.Checked) { listEditableFild.Add("[endTime]"); ListValues.Add(txtDtChgGrpEndTime.Text.Trim()); }
                    if (chbDtChgGrpApplicant.Checked) { listEditableFild.Add("[applicant]"); ListValues.Add(txtDtChgGrpApplicant.Text.Trim()); }
                    if (chbDtChgGrpSubject.Checked) { listEditableFild.Add("[subject]"); ListValues.Add(cbxDtChgGrpSubject.Text.Trim()); }
                    if (chbDtChgGrpREgion.Checked) { listEditableFild.Add("[region]"); ListValues.Add(txtDtChgGrpRegion.Text.Trim()); }

                    if (chbDtChgGrpPerformer.Checked)
                    {
                        string strPerformer = cbxDtChgGrpPerformer.Text.Trim();
                        string strPerformerWithoutDashAndDigit = Regex.Replace(strPerformer, @"[-0-9]", "");
                        string strTelOfPerformer = objClsSql.SendCommandGetStringResult($"SELECT Tel FROM [TEL].[dbo].tblTel WHERE Name = N'{strPerformerWithoutDashAndDigit}'");
                        listEditableFild.Add("[performer]");
                        ListValues.Add($"{cbxDtChgGrpPerformer.Text.Trim()}-{strTelOfPerformer}");
                    }

                    if (chbDtChgGrpApprover.Checked)
                    {
                        listEditableFild.Add("[approver]");
                        ListValues.Add(cbxDtChgGrpApprover.Text.Trim());
                    }

                    if (chbDtChgGrpDtNo.Checked)
                    {
                        Match match = Regex.Match(cbxDtChgGrpDtNo.Text.Trim(), @"202\d\d\d\d\d-\d\d\d\d\d");
                        if (match.Success || string.IsNullOrEmpty(cbxDtChgGrpDtNo.Text))
                        {
                            listEditableFild.Add("[dtNo]");
                            ListValues.Add(cbxDtChgGrpDtNo.Text.Trim());
                            listEditableFild.Add("[registerDateInSdm]");
                            ListValues.Add(strDateNow);
                        }
                        else
                        {
                            ClsDiff.MyMessageboxErr("اخطار", "فرمت شماره دان تایم صحیح نمی باشد");
                            return;
                        }
                    }

                    if (chbDtChgGrpStatus.Checked)
                    {
                        listEditableFild.Add("[status]");
                        ListValues.Add(cbxDtChgGrpStatus.Text.Trim());
                    }
                    else
                    {
                        // todo موقتا غیر فعال کردم حس می کنم لزوم ندارد
                        //listEditableFild.Add("[status]");
                        //ListValues.Add(row["وضعیت"].ToString());
                    }

                    if (chbDtChgGrpNote.Checked)
                    {
                        string strNote = string.Empty;
                        if (txtEditedPayan.Visible)
                            strNote = $"{MainForm.strDtNoteShoroo} تا {MainForm.strDtNotePayan}";
                        else if (txtRowOfTadakhol.Visible)
                            strNote = MainForm.strDtNoteTadakholBa.Contains("-") ? $" تداخل با CR-{MainForm.strDtNoteTadakholBa}" : $" تداخل با ردیف {MainForm.strDtNoteTadakholBa}";
                        else
                            strNote = cbxDtChgGrpNote.Text?.Trim() ?? "";

                        listEditableFild.Add("[note]");
                        ListValues.Add(strNote);
                    }

                    if (chbDtChgGrpActivity.Checked)
                    {
                        listEditableFild.Add("[Activity]");
                        string activityValue = cbxDtChgGrpActivity.SelectedItem?.ToString() ?? "";
                        ListValues.Add(activityValue);
                    }
                    else
                    {
                        listEditableFild.Add("[Activity]");
                        ListValues.Add(row["فعالیت"].ToString());
                    }

                    

                    if (chbDtChgGrpOutageTime.Checked)
                    {
                        string strOutage = objClsSql.SendCommandGetStringResult($"USE DT; SELECT outageMin FROM tblDownTimeShort WHERE row = N'{sRrow}'");
                        string selectedOutage = listBoxDtOutageMin.Text?.Trim() ?? "0";
                        if (strOutage.Contains("_YES"))
                            strOutage = selectedOutage + "_YES";
                        else if (strOutage.Contains("_NO"))
                            strOutage = selectedOutage + "_NO";
                        else
                            strOutage = selectedOutage;
                        if(!listEditableFild.Contains("[outageMin]"))
                           listEditableFild.Add("[outageMin]");
                        ListValues.Add(strOutage);
                    }

                    if (checkBoxBusyOver.Checked)
                    {
                        string strOutage = objClsSql.SendCommandGetStringResult($"USE DT; SELECT outageMin FROM tblDownTimeShort WHERE row = N'{sRrow}'");
                        strOutage = strOutage.Replace("_YES", "").Replace("_NO", "");
                        strOutage += radioButtonIsInBusyHour.Checked ? "_YES" : "_NO";

                        if (!listEditableFild.Contains("[outageMin]"))
                            listEditableFild.Add("[outageMin]");
                        ListValues.Add(strOutage);
                    }

                    // ثبت تغییرات
                    if (listEditableFild.Count > 0)
                    {
                        objClsSql.UpDate("[DT].[dbo].tblDownTimeShort", listEditableFild.ToArray(), ListValues.Cast<object>().ToArray(), $" row = N'{sRrow}'");
                    }
                }

                // رفرش جدول
                //string rowsCondition = string.Join(",", MainForm.DtSelectedDt.AsEnumerable()
                // .Select(r => r.Field<int>("ردیف").ToString().Trim()));  // استفاده از Field<int>

                string rowsCondition = string.Join(",", MainForm.DtSelectedDt.AsEnumerable()
    .Select(r => Convert.ToString(r["ردیف"]).Trim())
    .Where(s => !string.IsNullOrEmpty(s))); // حذف ردیف‌های احتمالی خالی


                // در کوئری نباید دور عددها نقل قول بگذارید (چون ردیف عددی است)
                DataTable dt = objClsSql.FillDataTableFromQuery($@"
    SELECT row AS ردیف, registerDate AS [تاریخ ثبت], registerTime AS [ثبت],
           siteCode AS [کد سایت], siteName AS [نام سایت], region AS [منطقه],
           startDate AS [تاریخ شروع], startTime AS [زمان شروع],
           endDate AS [تاریخ پایان], endTime AS [زمان پایان],
           applicant AS [درخواست کننده], subject AS موضوع,
           performer AS [انجام دهنده], approver AS [ثبت کننده],
           dtNo AS شماره, status AS [وضعیت], Note AS [یادداشت], Activity AS [فعالیت]
    FROM [DT].[dbo].tblDownTimeShort WHERE row IN ({rowsCondition})");

                MainForm.DtSelectedDt = dt;
                gridControlDtSelected.DataSource = null;
                gridControlDtSelected.DataSource = dt;

                // بازیابی ردیف انتخاب شده
                if (currentRowId != null)
                {
                    for (int i = 0; i < gridViewDtSelected.RowCount; i++)
                    {
                        object rowId = gridViewDtSelected.GetRowCellValue(i, "ردیف");
                        if (rowId?.ToString() == currentRowId.ToString())
                        {
                            gridViewDtSelected.FocusedRowHandle = i;
                            gridViewDtSelected.SelectRow(i);
                            break;
                        }
                    }
                }

                ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfTblDt");
                ClsDiff.AutoClosingMessageBox.Show("تغییرات گروهی انجام شد", "موفق", 500);
                ClsDiff.writeLogOfAction("تغییرات گروهی روی دان تایمها صورت گرفت");
            }

            SaveStatusToEditting();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            DialogResult result = MessageBox.Show($"برای حذف به تعداد {gridViewDtSelected.RowCount} موافق هستید؟", "تایید", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                DialogResult result1 = MessageBox.Show($"آیا واقعاً می‌خواهید تعداد {gridViewDtSelected.RowCount} دان تایم را حذف کنید؟", "تایید نهایی", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    int currentFocusedRowHandle = gridViewDtSelected.FocusedRowHandle;
                    object currentRowId = currentFocusedRowHandle >= 0 ? gridViewDtSelected.GetRowCellValue(currentFocusedRowHandle, "ردیف") : null;

                    MainForm.DtSelectedDt.DefaultView.Sort = "ردیف desc";
                    MainForm.DtSelectedDt = MainForm.DtSelectedDt.DefaultView.ToTable();

                    foreach (DataRow row in MainForm.DtSelectedDt.Rows)
                    {
                        string StrCurRowInDtGrid = row.ItemArray[0].ToString();
                        string strLastRowInDtTable = objClsSql.FindFieldInLastRecord("[DT].[dbo].tblDownTimeShort", "row", "row");

                        if (StrCurRowInDtGrid == strLastRowInDtTable)
                        {
                            ClsDiff objClsDiff = new ClsDiff();
                            objClsDiff.DeleteDtRowFromDbAndmyDb(strLastRowInDtTable);
                        }
                        else
                        {
                            objClsSql.SendCommand($"UPDATE [DT].[dbo].[tblDownTimeShort] SET status = N'حذف', Note = N'{MainForm.StrCurUsr}' WHERE row = N'{StrCurRowInDtGrid}'");
                        }
                    }

                    string rowsCondition = string.Join(",", MainForm.DtSelectedDt.AsEnumerable().Select(r => $"'{r.Field<string>("ردیف")}'"));
                    DataTable dt = objClsSql.FillDataTableFromQuery($@"
                        SELECT row AS ردیف, registerDate AS [تاریخ ثبت], registerTime AS [زمان],
                               siteCode AS [کد سایت], siteName AS [نام سایت], region AS [منطقه],
                               startDate AS [تاریخ شروع], startTime AS [شروع],
                               endDate AS [تاریخ پایان], endTime AS [زمان پایان],
                               applicant AS [درخواست کننده], subject AS موضوع,
                               performer AS [انجام دهنده], approver AS [ثبت],
                               dtNo AS شماره, status AS [وضعیت], Note AS [یادداشت], Activity AS [فعالیت]
                        FROM [DT].[dbo].tblDownTimeShort WHERE row IN ({rowsCondition})");

                    MainForm.DtSelectedDt = dt;
                    gridControlDtSelected.DataSource = null;
                    gridControlDtSelected.DataSource = dt;

                    if (currentRowId != null)
                    {
                        for (int i = 0; i < gridViewDtSelected.RowCount; i++)
                        {
                            if (gridViewDtSelected.GetRowCellValue(i, "ردیف")?.ToString() == currentRowId.ToString())
                            {
                                gridViewDtSelected.FocusedRowHandle = i;
                                gridViewDtSelected.SelectRow(i);
                                break;
                            }
                        }
                    }

                    ClsDiff.writeLogOfAction("حذف گروهی روی دان تایمها صورت گرفت");
                    MainForm.bolDtForceToRefreshAndDontGoToEnd = true;
                }
            }
        }

        private void frmDtMultiChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (DataRow row in MainForm.DtSelectedDt.Rows)
            {
                string strCurStatus = objClsSql.FindRecord("[DT].[dbo].[tblDownTimeShort]", "status", $" WHERE row = {row.ItemArray[0]}");
                if (strCurStatus.Contains(" و در حال ویرایش توسط "))
                {
                    string newStatus = strCurStatus.Replace(" و در حال ویرایش توسط ", "").Replace(MainForm.StrFullNameCurUsr, "").Trim();
                    objClsSql.SendCommand($"UPDATE [DT].[dbo].[tblDownTimeShort] SET status = N'{newStatus}' WHERE row = {row["ردیف"]}");
                }
            }
        }

        private void frmDtMultiChange_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.BolHavePermitRefresh = true;
            gridControlDtSelected.DataSource = null;
        }

        private void cbxDtChgGrpDtNo_TextChanged(object sender, EventArgs e)
        {
            if (cbxDtChgGrpDtNo.Text.Contains("MCI-CR-"))
            {
                cbxDtChgGrpDtNo.Text = cbxDtChgGrpDtNo.Text.Replace("MCI-CR-", string.Empty);
            }

            Match match = Regex.Match(cbxDtChgGrpDtNo.Text.Trim(), @"202\d\d\d\d\d-\d\d\d\d\d");
            if ((match.Success && cbxDtChgGrpDtNo.Text.Length == 14) || cbxDtChgGrpDtNo.Text.Length == 0)
            {
                cbxDtChgGrpDtNo.ForeColor = Color.Green;
                bolIsCorrectDtNoFormat = true;
            }
            else
            {
                bolIsCorrectDtNoFormat = false;
                cbxDtChgGrpDtNo.ForeColor = Color.Red;

                // تصحیح شده - ترتیب پارامترها درست است
                MessageBox.Show(
                    "تعداد کاراکتر شماره یا فرمت شماره دان تایم صحیح نمی باشد",  // متن
                    "اخطار",                                                      // عنوان
                    MessageBoxButtons.OK,                                         // دکمه‌ها
                    MessageBoxIcon.Warning,                                       // آیکون
                    MessageBoxDefaultButton.Button1,                              // دکمه پیش‌فرض
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading   // گزینه‌ها
                );
            }
        }

        private void cbxDtChgGrpNote_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDtChgGrpNote.SelectedItem != null)
            {
                string selectedNote = cbxDtChgGrpNote.SelectedItem.ToString();
                if (selectedNote.Contains("تصحیح زمان فعالیت") && chbDtChgGrpNote.Checked)
                {
                    frmShrooPayan objFrmShrooAz = new frmShrooPayan();
                    objFrmShrooAz.ShowDialog();
                    if (!string.IsNullOrEmpty(MainForm.strDtNoteShoroo))
                    {
                        txtEditedShroo.Visible = true;
                        txtEditedPayan.Visible = true;
                        lblTo.Visible = true;
                        txtRowOfTadakhol.Visible = false;
                        txtEditedShroo.Text = MainForm.strDtNoteShoroo;
                        txtEditedPayan.Text = MainForm.strDtNotePayan;
                        MainForm.strDtNoteTadakholBa = string.Empty;
                    }
                }
                else if (selectedNote.Contains("تداخل با ردیف") && chbDtChgGrpNote.Checked)
                {
                    frmTadakholBa objFrmTadakholBa = new frmTadakholBa();
                    objFrmTadakholBa.ShowDialog();
                    if (!string.IsNullOrEmpty(MainForm.strDtNoteTadakholBa))
                    {
                        txtRowOfTadakhol.Visible = true;
                        txtRowOfTadakhol.Text = MainForm.strDtNoteTadakholBa;
                        lblTo.Visible = false;
                        txtEditedShroo.Visible = false;
                        txtEditedPayan.Visible = false;
                        MainForm.strDtNoteShoroo = MainForm.strDtNotePayan = string.Empty;
                    }
                }
                else
                {
                    ResetNoteControls();
                }
            }
        }

        private void ResetNoteControls()
        {
            MainForm.strDtNoteShoroo = MainForm.strDtNotePayan = string.Empty;
            txtEditedShroo.Visible = false;
            txtEditedPayan.Visible = false;
            lblTo.Visible = false;
            txtRowOfTadakhol.Visible = false;
            txtRowOfTadakhol.Text = string.Empty;
            txtEditedShroo.Text = string.Empty;
        }

        private void chbDtChgGrpDtNo_CheckedChanged(object sender, EventArgs e) { }
    }
}