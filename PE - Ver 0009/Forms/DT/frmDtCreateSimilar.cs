using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
namespace PE.Forms
{
    public partial class frmDtCreateSimilar : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        string strDtRegDateSelForcreSim = "";
        string strDtNoSelForcreSim = "";
        string strForFilter = "";

        bool bolValidateStartDate = true;
        bool bolValidateEndDate = true;
        public frmDtCreateSimilar()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void frmDtCreateSimilar_Load(object sender, EventArgs e)
        {
            btnDtCreSimDtInDb.Enabled = true;
            ClsDiff.writeLogOfAction("ورود به صفحه ایجاد دان تایمهای مشابه");

            #region پر کردن کومبوباکسها
            refreshComboBoxCreSimSubject();
            refreshComboBoxCreSimPerformer(); 
            #endregion

            gridControlDtSelForCreSim.DataSource = MainForm.DtSelectedDtForCreSim;

            this.gridViewDtSelForCreSim.OptionsView.ColumnAutoWidth = false;

            this.gridViewDtSelForCreSim.Columns["ردیف"].Width =70;
            this.gridViewDtSelForCreSim.Columns["تاریخ ثبت"].Width = 70;
            this.gridViewDtSelForCreSim.Columns["ثبت"].Width = 40;
            this.gridViewDtSelForCreSim.Columns["کد سایت"].Width = 60;
            this.gridViewDtSelForCreSim.Columns["نام سایت"].Width = 120;
            this.gridViewDtSelForCreSim.Columns["منطقه"].Width = 60;
            this.gridViewDtSelForCreSim.Columns["تاریخ شروع"].Width = 70;
            this.gridViewDtSelForCreSim.Columns["زمان شروع"].Width = 0x2f;
            this.gridViewDtSelForCreSim.Columns["تاریخ پایان"].Width = 70;
            this.gridViewDtSelForCreSim.Columns["زمان پایان"].Width = 40;
            this.gridViewDtSelForCreSim.Columns["درخواست کننده"].Width = 90;
            this.gridViewDtSelForCreSim.Columns["موضوع"].Width = 150;
            this.gridViewDtSelForCreSim.Columns["انجام دهنده"].Width = 60;
            this.gridViewDtSelForCreSim.Columns["ثبت کننده"].Width = 90;
            this.gridViewDtSelForCreSim.Columns["شماره"].Width = 50;
            this.gridViewDtSelForCreSim.Columns["وضعیت"].Width = 100;
            this.gridViewDtSelForCreSim.Columns["یادداشت"].Width = 60;


            foreach (GridColumn column in this.gridViewDtSelForCreSim.Columns)//فراخوانی تک تک ستونها و
                {
                    string strColumn = gridViewDtSelForCreSim.GetRowCellValue(this.gridViewDtSelForCreSim.GetSelectedRows()[0], column).ToString();//پیدا کردن مقدار فیلد جدید
                    switch (column.ToString())
                    {
                    #region بررسی ستونها برای مقایسه و تغییر

                        case "تاریخ ثبت":
                            strDtRegDateSelForcreSim = strColumn;
                            break;

                        case "شماره":
                            strDtNoSelForcreSim = strColumn;
                            break;

                        case "تاریخ شروع":
                            txtDtCreSimStartDate.Text = strColumn;
                            break;
                        case "زمان شروع":
                            txtDtCreSimStartTime.Text = strColumn;
                            break;
                        case "تاریخ پایان":
                            txtDtCreSimEndDate.Text = strColumn;
                            break;
                 
                        case "زمان پایان":
                            txtDtCreSimEndTime.Text = strColumn;
                            break;
                        case "درخواست کننده":
                            txtDtCreSimApplicant.Text = strColumn;
                            break;
                        case "موضوع":
                            cbxDtCreSimSubject.Text = strColumn;
                            break;
                        case "انجام دهنده":
                            cbxDtCreSimPerformer.Text = strColumn;
                            break;

                        #endregion
                    }
                }
           
        }
        private void gridViewDtSelForCreSim_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
           
        }

        private void btnDtCreSimReg_Click(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();

            if (!(bolValidateStartDate && bolValidateEndDate))
            {
                MessageBox.Show("حداقل در یکی از تاریخها فرمت صحیح وارد نشده است لطفا اصلاح و مجددا تلاش کنید");
                return;
            }

            if (MainForm.DtDtForCreSim.Columns.Count == 0)
            {
                MainForm.DtDtForCreSim.Columns.Add("ردیف", typeof(int));
                MainForm.DtDtForCreSim.Columns.Add("تاریخ ثبت", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("ثبت", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("کد سایت", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("نام سایت", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("منطقه", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("BSC", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("تاریخ شروع", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("شروع", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("تاریخ پایان", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("پایان", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("درخواست کننده", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("موضوع", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("انجام دهنده", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("ثبت کننده", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("شماره", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("وضعیت", typeof(string));
                MainForm.DtDtForCreSim.Columns.Add("یادداشت", typeof(string));
            }
            else
            {
                MainForm.DtDtForCreSim.Clear();
            }

            MainForm.DtDtForCreSim=objClsSql.FillDataTableFromQuery(@"
                       USE DT ;
                       SELECT   
                        row AS ردیف,
                        registerDate AS [تاریخ ثبت],
	                     registerTime AS [ثبت],
	                      siteCode AS [کد سایت],
	                       siteName AS [نام سایت],
                            region AS [منطقه],
                                curBSC AS [فعلی BSC],
		                         startDate AS [تاریخ شروع],
		                             startTime AS [شروع],
			                            endDate AS [تاریخ پایان],
			                               endTime AS [پایان],
			                                 applicant AS [درخواست کننده],
			                                    subject AS موضوع,
			                                     performer AS [انجام دهنده]
                                     
                                      from [DT].[dbo].tblDownTimeShort Where registerDate=N'" + strDtRegDateSelForcreSim + "' AND dtNo=N'"+strDtNoSelForcreSim+"'"+strForFilter);

            int i = 0;
            foreach (DataRow row in MainForm.DtDtForCreSim.Rows)
            {
                MainForm.DtDtForCreSim.Rows[i]["تاریخ شروع"] = txtDtCreSimStartDate.Text;
                MainForm.DtDtForCreSim.Rows[i]["شروع"] = txtDtCreSimStartTime.Text;
                MainForm.DtDtForCreSim.Rows[i]["تاریخ پایان"] = txtDtCreSimEndDate.Text;
                MainForm.DtDtForCreSim.Rows[i]["پایان"] = txtDtCreSimEndTime.Text;
                
                MainForm.DtDtForCreSim.Rows[i]["درخواست کننده"] = txtDtCreSimApplicant.Text;
                MainForm.DtDtForCreSim.Rows[i]["موضوع"] = cbxDtCreSimSubject.Text;
                MainForm.DtDtForCreSim.Rows[i]["انجام دهنده"] = cbxDtCreSimPerformer.Text;

                i++;
            }

            gridControlDtFinalSelForReCreate.DataSource = MainForm.DtDtForCreSim;

            this.gridViewDtFinalSelForReCreate.OptionsView.ColumnAutoWidth = false;

            this.gridViewDtFinalSelForReCreate.Columns["ردیف"].Width = 40;
            this.gridViewDtFinalSelForReCreate.Columns["تاریخ ثبت"].Width = 70;
            this.gridViewDtFinalSelForReCreate.Columns["ثبت"].Width = 70;
            this.gridViewDtFinalSelForReCreate.Columns["کد سایت"].Width = 60;
            this.gridViewDtFinalSelForReCreate.Columns["نام سایت"].Width = 120;
            //this.gridViewDtFinalSelForReCreate.Columns["BSC"].Width = 60;
            this.gridViewDtFinalSelForReCreate.Columns["تاریخ شروع"].Width = 70;
            this.gridViewDtFinalSelForReCreate.Columns["شروع"].Width = 70;
            this.gridViewDtFinalSelForReCreate.Columns["تاریخ پایان"].Width = 70;
            this.gridViewDtFinalSelForReCreate.Columns["پایان"].Width = 70;
            this.gridViewDtFinalSelForReCreate.Columns["درخواست کننده"].Width = 140;
            this.gridViewDtFinalSelForReCreate.Columns["موضوع"].Width = 160;
            this.gridViewDtFinalSelForReCreate.Columns["انجام دهنده"].Width = 80;

        }

        private void refreshComboBoxCreSimSubject()
        {
            DataTable table = objClsSql.FillDataTableFromQuery("SELECT subject FROM [DT].[dbo].tblDownTimeShort GROUP BY subject");
            this.cbxDtCreSimSubject.DataSource = table;
            this.cbxDtCreSimSubject.DisplayMember = "subject";
            this.cbxDtCreSimSubject.Text = "QA PLAN";
        }

        private void refreshComboBoxCreSimPerformer()
        {
            DataTable table = objClsSql.FillDataTableFromQuery("SELECT performer FROM [DT].[dbo].tblDownTimeShort GROUP BY performer ORDER BY Performer");
            this.cbxDtCreSimPerformer.DataSource = table;
            this.cbxDtCreSimPerformer.DisplayMember = "performer";
        }

        private void btnDtCreSimDtInDb_Click(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            switch (MessageBox.Show("برای ایجاد دان تایمهای گروهی جدول فوق مطمئن هستید",
          "تائیدیه",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question))
            {
               
                case DialogResult.Yes:
                    btnDtCreSimDtInDb.Enabled = false;
                    CreateGroupDt();
                    ClsDiff.writeLogOfAction("ایجاد دان تایمهای مشابه");
                    break;
                case DialogResult.No:
                    return;
            }


          
        }

        private void CreateGroupDt()
        {
            #region پیدا کردن شماره بعدی برای ثبت دان تایم جدید
            int num;
            string s =objClsSql.FindFieldInLastRecord("[DT].[dbo].tblDownTimeShort", "row", "row");

            if (s == "نامشخص")
            {
                num = 0;
            }
            else
            {
                num = int.Parse(s);
            }

            int intNewRow = num + 1;

            #endregion

            string strFieldsName = "[row],[registerDate],[registerTime],[siteCode],[siteName],[region],[curBSC],[startDate],[startTime],[endDate],[endTime],[applicant],[subject],[performer],[approver],[Status]";


            string strRegisterDate = ClsConvertDateHassan.ConvertDateToPersian(MainForm.DateTimeSqlServer);//استخراج زمان ثبت از اس کیو ال سرور
            string strRegisterTime = MainForm.DateTimeSqlServer.ToString("HH:mm");

            int i = 0;
            foreach (DataRow row in MainForm.DtDtForCreSim.Rows)
            {
              
                 string strValues = string.Concat(new object[] {
                "N'", intNewRow.ToString(),
                "',N'", strRegisterDate,
                "',N'", strRegisterTime,
                "',N'", MainForm.DtDtForCreSim.Rows[i]["کد سایت"],
                "',N'", MainForm.DtDtForCreSim.Rows[i]["نام سایت"],
                 "',N'", MainForm.DtDtForCreSim.Rows[i]["منطقه"],
                "',N'", MainForm.DtDtForCreSim.Rows[i]["فعلی BSC"],
                "',N'",  MainForm.DtDtForCreSim.Rows[i]["تاریخ شروع"],
                "',N'", MainForm.DtDtForCreSim.Rows[i]["شروع"],
                 "',N'",  MainForm.DtDtForCreSim.Rows[i]["تاریخ پایان"],
                "',N'", MainForm.DtDtForCreSim.Rows[i]["پایان"],
                "',N'",  MainForm.DtDtForCreSim.Rows[i]["درخواست کننده"],
                "',N'",  MainForm.DtDtForCreSim.Rows[i]["موضوع"],
                "',N'",  MainForm.DtDtForCreSim.Rows[i]["انجام دهنده"],
                 "',N'", MainForm.StrCurUsr,
                "',N'ثبت توسط کاربر'"
                    });

                #region ثبت در  اس کیو ال
                 objClsSql.GetAllAndSendToDataBase("[DT].[dbo].tblDownTimeShort", strFieldsName, strValues);//ثبت
                #endregion

                //#region ثبت در مای اس کیو ال

                //ClsDiff objClsDiff = new ClsDiff();
                //objClsDiff.RegisterDownTimeInMySql(strFieldsName.Replace("[", "").Replace("]", ""), strValues.Replace("[", "").Replace("]", ""));
                //#endregion
                intNewRow = intNewRow + 1;
               
                i++;
            }

         

            ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfTblDt");


        }

        private void btnDtDelSelDts_Click(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            for (int i = 0; i < gridViewDtFinalSelForReCreate.SelectedRowsCount; i++)
            {
                if (gridViewDtFinalSelForReCreate.GetSelectedRows()[i] >= 0)
                {
                    DataRow dr= gridViewDtFinalSelForReCreate.GetDataRow(gridViewDtFinalSelForReCreate.GetSelectedRows()[i]);
                    string str = dr.ItemArray[0].ToString();
                     strForFilter += " AND [row] !=N'" + str + "' ";
                }

            }

            switch (MessageBox.Show("برای حذف تایمهای از جدول مشابه مطمئن هستید",
       "تائیدیه",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    btnDtCreSimReg.PerformClick();
                    break;
                case DialogResult.No:
                    return;
            }


           

        }

        private void txtDtCreSimStartDate_Leave(object sender, EventArgs e)
        {
            if(txtDtCreSimStartDate.Text.Length<10)
            {
                txtDtCreSimStartDate.BackColor = Color.Pink;
                MessageBox.Show("فرمت تاریخ باید به صورت 1398/05/09 باشد");
                bolValidateStartDate = false;

            }
            else
            {
                txtDtCreSimStartDate.BackColor = Color.White;
                bolValidateStartDate = true;
            }
        }

        private void txtDtCreSimEndDate_Leave(object sender, EventArgs e)
        {
            if (txtDtCreSimEndDate.Text.Length < 10)
            {
                txtDtCreSimEndDate.BackColor = Color.Pink;
                MessageBox.Show("فرمت تاریخ باید به صورت 1398/05/09 باشد");
                bolValidateEndDate = false;

            }
            else
            {
                txtDtCreSimEndDate.BackColor = Color.White;
                bolValidateEndDate = true;
            }
        }

        private void FrmDtCreateSimilar_FormClosed(object sender, FormClosedEventArgs e)
        {
            
              
           
        }
    }
}
