using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PE.Forms.DT
{

    public partial class frmDtRegDownTime : Form
    {

        #region VAR

        //private ClsMySql objClsMySql;//ایجاد شیی از روی کلاس مای اس کیو ال
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        bool bolIsSucssesRegDownTime = false;
        string strSourceRowOfRepeatedRow = string.Empty;
        string strRegion = string.Empty;
        string strNoteForPmSameSubAndCodeAndDate = string.Empty;
        string strTelOfPerformer = string.Empty;
        DataTable table = new DataTable();
        DataTable AllNames = new DataTable();
        string strTempForPerformer = string.Empty;
        string strDtSetOutage = string.Empty;
        double dblDtTotalMinuteTime = 0;
        double dblDtTotalMinuteTimeLast = 10;
        DataTable tblCbxSubject;
        string strDomain = string.Empty;
        string strSubject = string.Empty;

        bool bolIsReqPm = false;
        bool bolIsReqVisit = false;

        #endregion

        #region FORM
        private void FrmDtRegDownTime_Load(object sender, EventArgs e)
        {
            rgpDtTime.Enabled = false;
            gbx_bazdidSobh.Enabled = false;
            radioGroupDomain.SelectedIndex = 0;
            objClsSql = new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال

            #region بررسی ساعت مناسب ایجاد دان تایم در صورتیکه کاربر پاور یا ادمین نیست

            if (MainForm.StrCurRol != "administrator" && MainForm.StrCurRol != "ts")
            {
                //string strSetedTime = objClsSql.SendCommandGetStringResult("Select lastPermitTimeForDownTime from DT.dbo.tblLastPermitTimeForDt where row='1'").Insert(2, ":");
                string strSetedTime = objClsSql.SendCommandGetStringResult("Select lastPermitTimeForDownTime from DT.dbo.tblLastPermitTimeForDt  where id='1'").Substring(0, 4);

                int intSetedLimitTime = int.Parse(strSetedTime.Replace(":", string.Empty));

                int intCurTime = int.Parse(MainForm.DateTimeSqlServer.ToShortTimeString().Replace(":", string.Empty));

                if (intCurTime > intSetedLimitTime)
                {
                    string strMsg = @" کاربر گرامی به منظور تکمیل پروسه دان تایمهای درخواستی امروز پس از ساعت  " + strSetedTime.Substring(2, 2) + " : " + strSetedTime.Substring(0, 2) + " سیستم قادر به پذیرش درخواست جدید نمی باشد  ";
                    MessageBox.Show(strMsg, "توجه", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                    this.Close();
                    // MessageBox.Show("  کاربر گرامی به منظور تکمیل پروسه دان تایمهای درخواستی امروز پس از ساعت  " + strSetedTime + " سیستم قادر به پذیرش درخواست جدید نمی باشد  ");
                    return;
                }

            }

            #endregion


            if (!MainForm.BolHavePermitToRegDt)
            {
                MessageBox.Show("بطور موقت ثبت دان تایم توسط ادمین غیر فعال شد", "اطلاع رسانی", MessageBoxButtons.OK,

                  MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                 MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                this.Close();
            }
      

           
            refreshListBoxPerformer();

            textBoxDtRegCodeSite.TabIndex = 0;

            refreshRegisterGrid();

            textBoxDtRegCodeSite.Select();
            if (MainForm.StrCurRol=="ts")
            {
                //disable all unused box
                gbxDtAllSubject.Visible = false;
                gbxDtSelectDateTime.Visible = false;
                gbxDtOutage.Visible = false;
                txtSerchedPerformer.TabIndex = 1;

                gbxDtPerformer.Location = new Point(gbxDtRegCode.Location.X, gbxDtRegCode.Location.Y+ gbxDtRegCode.Height+10);
                btnDtReg.Location = new Point(btnDtReg.Location.X, gbxDtPerformer.Location.Y + gbxDtPerformer.Height + 10);
                gridControlDtRegiterDtByThisUser.Location = new Point(gridControlDtRegiterDtByThisUser.Location.X, btnDtReg.Location.Y + btnDtReg.Height + 10);
                gridControlDtRegiterDtByThisUser.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                return;
            }

            ResetForNewDownTime();
            refreshComboBoxSubject();
            FilldateTimeNumericUpDownForPm();
           
            txtSerchedOutage.Text = string.Empty;
           

           
            comboBoxDtSubject.TabIndex = 1;
            btnDtFromDate.TabIndex = 2;
            nudDtStHour.TabIndex = 3;
            btnDtToDate.TabIndex = 4;
            nudDtEndHour.TabIndex = 5;
            nudDtEndMin.TabIndex = 6;
            listBoxDtOutageMin.TabIndex = 7;
            txtSerchedPerformer.TabIndex = 8;
            btnDtReg.TabIndex = 9;


        }

        private void FrmDtRegDownTime_FormClosed(object sender, FormClosedEventArgs e)
        {
            DateTime dateTimeSqlServer = MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
        }

        public frmDtRegDownTime()
        {
            InitializeComponent();
            //objClsMySql = new ClsMySql();
        }
        #endregion

        #region TextBox
        private void TextBoxDtRegCodeSite_Leave(object sender, EventArgs e)
        {
            if (textBoxDtRegCodeSite.Text.Length < 4)
            {
                MessageBox.Show(" تعداد ارقام کد سایت باید 4 عدد باشد \n و شما فقط  " + textBoxDtRegCodeSite.Text.Length + "   رقم وارد کرده اید لطفا اصلاح فرمایید \n");
                textBoxDtRegCodeSite.Focus();
            }
            
        }
        private void TextBoxDtRegCodeSite_Enter(object sender, EventArgs e)
        {

            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));
            this.textBoxDtRegCodeSite.CharacterCasing = CharacterCasing.Upper;
            this.textBoxDtRegCodeSite.SelectionStart = 4;

        }
        private void TextBoxDtRegCodeSite_TextChanged(object sender, EventArgs e)
        {
            this.textBoxDtRegCodeSite.CharacterCasing = CharacterCasing.Upper;

            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));

            if (textBoxDtRegCodeSite.TextLength == 4)
            {
                string strLocation = "ES" + textBoxDtRegCodeSite.Text;

                DataRow[] dr = null;
                try
                {
                    dr = MainForm.DtMiniTempDb.Select("[کد سایت]='" + strLocation + "'");

                    txtDtRegSiteName.Text = dr[0].ItemArray[1].ToString();
                    if (txtDtRegSiteName.Text.Length == 0)
                    {
                        txtDtRegSiteName.Text = "نامشخص";
                    }

                    lblDtRegion.Text = dr[0].ItemArray[4].ToString();

                }
                catch
                {
                    txtDtRegSiteName.Text = "نامشخص";

                    lblDtRegion.Text = "نامشخص";
                }

            }
            else
            {
                txtDtRegSiteName.Text = "-----------------------------------------------";

                lblDtRegion.Text = string.Empty;

                if (textBoxDtRegCodeSite.Text.Length > 4)
                {
                    MessageBox.Show("کد سایت باید یک عدد 4 رقمی باشد");
                    textBoxDtRegCodeSite.Text = textBoxDtRegCodeSite.Text.Remove(textBoxDtRegCodeSite.Text.Length - 1, 1);

                }
            }
        }
        private void TextBoxDtRegCodeSite_KeyPress(object sender, KeyPressEventArgs e)
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
        private void TextBoxDtRegCodeSite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EvaluteDownTimeAndRegDt();

                refreshRegisterGrid();
            }
        }
        private void txtSerchedPerformer_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }
        private void txtSerchedPerformer_TextChanged(object sender, EventArgs e)
        {
            refreshListBoxPerformer();
        }
        private void txtSerchedPerformer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MainForm.strCurPerformer = strTelOfPerformer = lstSerchedPerformer.Text;
                if (lstSerchedPerformer.SelectedItem.ToString() != string.Empty && lstSerchedPerformer.SelectedItem.ToString() != null)
                {
                    EvaluteDownTimeAndRegDt();
                }

                if (bolIsSucssesRegDownTime)
                {
                    refreshRegisterGrid();
                }



            }
            if (e.KeyCode == Keys.Down)
            {
                lstSerchedPerformer.Select();
            }

        }

        private void txtSerchedOutage_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void txtSerchedOutage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (bolIsSucssesRegDownTime)
                {
                    refreshRegisterGrid();
                }

            }
            if (e.KeyCode == Keys.Down)
            {
                listBoxDtOutageMin.Select();
            }
        }

        private void txtSerchedOutage_MouseUp(object sender, MouseEventArgs e)
        {
            FillListBox();
        }

        private void txtSerchedOutage_MouseHover(object sender, EventArgs e)
        {
            FillListBox();
        }

        private void txtSerchedPerformer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (txtSerchedPerformer.Text != string.Empty && txtSerchedPerformer.Text != null)
            {
                txtSerchedPerformer.SelectAll();
            }
        }
        #endregion

        #region buttons
        private void BtnDtReg_Click(object sender, EventArgs e)
        {
           

            if (listBoxDtOutageMin.SelectedItems.Count > 0)
            {
                strDtSetOutage = listBoxDtOutageMin.SelectedItem.ToString();

            }
            string str = txtDtToDate.Text;
            int intSelPerformerList = lstSerchedPerformer.SelectedIndex;
            strTempForPerformer = lstSerchedPerformer.GetItemText(lstSerchedPerformer.SelectedItem);
            EvaluteDownTimeAndRegDt();
            refreshRegisterGrid();
           // refreshComboBoxSubject();نمی دونم چرا حافظه ی صفحه رو هم پاک می کنه
        }

        private void BtnDtToDate_Click(object sender, EventArgs e)
        {

            MainForm.strWhitchTabSetDatTime = "DT";

            frmSelectDate objfrmSelectDate = new frmSelectDate();
            objfrmSelectDate.ShowDialog();

            if (MainForm.StrSelectedPershianDateWithSlashSeperator != string.Empty)
            {
                MainForm.StrDtEndDate = MainForm.StrSelectedPershianDateWithSlashSeperator;
                DateTime dteDtRequstedEndDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrSelectedPershianDateWithSlashSeperator);

                int intCompareDate = DateTime.Compare(dteDtRequstedEndDate, MainForm.DateTimeSqlServer.Date);

                if (intCompareDate >= 0)
                {
                    txtDtToDate.Text = MainForm.StrSelectedPershianDateWithNameOfDay;
                    this.txtDtToDate.ForeColor = System.Drawing.Color.Navy;
                }
                else
                {
                    MessageBox.Show("تاریخ پایان دان تایم باید مساوی یا بزرگتر از تاریخ امروز باشد");
                }
            }
        }

        private void BtnDtFromDate_Click(object sender, EventArgs e)
        {
            MainForm.strWhitchTabSetDatTime = "DT";
            frmSelectDate objfrmSelectDate = new frmSelectDate();
            objfrmSelectDate.ShowDialog();

            if (MainForm.StrSelectedPershianDateWithSlashSeperator != string.Empty)//ساعت صفر را در نظر می گیرد که باید اصلاح گردد چون روز جاری را قبول نمی کند
            {
                MainForm.StrDtEndDate = MainForm.StrDtStartDate = MainForm.StrSelectedPershianDateWithSlashSeperator;
                DateTime dteDtRequstedStartDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrSelectedPershianDateWithSlashSeperator);

                int intCompareDate = DateTime.Compare(dteDtRequstedStartDate.Date, MainForm.DateTimeSqlServer.Date);

                if (!(intCompareDate < 0))
                {
                    txtDtToDate.Text = txtDtFromDate.Text = MainForm.StrSelectedPershianDateWithNameOfDay;
                }
                else
                {
                    MessageBox.Show("تاریخ شروع دان تایم باید مساوی یا بزرگتر از تاریخ امروز باشد");
                }
            }
            else
            {
                MessageBox.Show(" هیچ تاریخی تنظیم نشده لطفا مجددا تلاش نمایید ");
                return;
            }
        }
        #endregion

        #region Grid
        private void refreshRegisterGrid()
        {
            // gridViewDtRegiterDtByThisUser.GroupPanelText = @"لیست دان تایم های ثبت توسط کاربر شما";
            string strStatus;
            if (MainForm.StrCurRol!="ts")
            {
                 strStatus = "ثبت توسط کاربر";
            }
            else
            {
                strStatus = "تائید";
            }
            
            #region دستور استخراج ده دان تایم آخر این درخواست کننده
            string strSql = @"SELECT
            [row] AS [ردیف]
            ,[registerDate] AS [تاریخ ثبت]
            ,[registerTime] AS [ثبت]
            ,[siteCode]  AS [کد سایت]
            ,[siteName] AS [نام سایت]           
            ,[startDate] AS [تاریخ شروع]
            ,[startTime] AS [زمان شروع]
            ,[endDate] AS [تاریخ پایان]
            ,[endTime] AS [زمان پایان]
            ,[subject] AS [موضوع]
            ,[performer] AS [انجام دهنده]
            --,[approver] AS [ثبت کننده]
           --,[dtNo] AS [شماره]
           --,[status] AS [وضعیت]
      
            FROM [DT].[dbo].[tblDownTimeShort]
            Where applicant=N'" + MainForm.StrFullNameCurUsr + @"' And status=N'"+strStatus+"' Order By row ";
            #endregion

            gridControlDtRegiterDtByThisUser.DataSource = null;

            DataTable dtRegisteredDownTime = objClsSql.FillDataTableFromQuery(strSql);

            gridControlDtRegiterDtByThisUser.DataSource = dtRegisteredDownTime;
            gridViewDtRegiterDtByThisUser.MoveLast();
            //gridViewDtRegiterDtByThisUser.MoveLastVisible();

            try

            {
                this.gridViewDtRegiterDtByThisUser.Columns["ردیف"].Width = 55;
                this.gridViewDtRegiterDtByThisUser.Columns["تاریخ ثبت"].Width = 70;
                this.gridViewDtRegiterDtByThisUser.Columns["ثبت"].Width = 70;
                this.gridViewDtRegiterDtByThisUser.Columns["کد سایت"].Width = 60;
                this.gridViewDtRegiterDtByThisUser.Columns["نام سایت"].Width = 100;
                //this.gridViewDtRegiterDtByThisUser.Columns["منطقه"].Width = 60;
                this.gridViewDtRegiterDtByThisUser.Columns["تاریخ شروع"].Width = 70;
                this.gridViewDtRegiterDtByThisUser.Columns["زمان شروع"].Width = 70;
                this.gridViewDtRegiterDtByThisUser.Columns["تاریخ پایان"].Width = 70;
                this.gridViewDtRegiterDtByThisUser.Columns["زمان پایان"].Width = 70;
                this.gridViewDtRegiterDtByThisUser.Columns["موضوع"].Width = 200;
                this.gridViewDtRegiterDtByThisUser.Columns["انجام دهنده"].Width = 200;
                //this.gridViewDtRegiterDtByThisUser.Columns["ثبت کننده"].Width = 90;
                //this.gridViewDtRegiterDtByThisUser.Columns["شماره"].Width = 50;
                //this.gridViewDtRegiterDtByThisUser.Columns["وضعیت"].Width = 120;
            }
            catch
            {

            }
        }
        #endregion

        #region comboBox
        public void refreshListBoxPerformer()
        {
            try
            {
                string strWhere = " WHERE NAME Like  N'%" + txtSerchedPerformer.Text + "%'";
                table = objClsSql.FillDataTableFromQuery("SELECT Name,telId,[WorkPlace] FROM [TEL].[dbo].tblTel " + strWhere);
                //txtSerchedPerformer.Text= table.Rows[0][0].ToString();
                lstSerchedPerformer.DataSource = table;
                lstSerchedPerformer.DisplayMember = "Name";
                txtRegDtWorkPlace.Text = table.Rows[0][2].ToString();

                lstTelId.DataSource = table;
                lstTelId.DisplayMember = "telId";
            }
            catch
            {

            }

        }

        public void refreshListBoxOutage()
        {
            string strWhere = " WHERE NAME Like  N'%" + txtSerchedOutage.Text + "%'";
            table = objClsSql.FillDataTableFromQuery("SELECT Name FROM [TEL].[dbo].tblTel " + strWhere);

            lstSerchedPerformer.DataSource = table;
            lstSerchedPerformer.DisplayMember = "Name";
        }



        private void refreshComboBoxSubject()
        {
            tblCbxSubject = objClsSql.FillDataTableFromQuery("Select subject from [DT].[dbo].tblDownTimeShort GROUP BY subject order by count(subject) desc");
            this.comboBoxDtSubject.DataSource = tblCbxSubject;
            this.comboBoxDtSubject.DisplayMember = "subject";
            this.comboBoxDtSubject.Text = string.Empty;
        }

        private void FunctionForLeaveSubject()
        {
            //اگر موضوع درخواست پی ام هستش سطح دسترسی کاربر برای ایجاد مجوز دان تایم پی ام روی کدسایتهای منطقه خودشون مشخص بشه

            #region بررسی اینکه ایا این درخواست از نوع پی هست یا نه
            bool bolIsPmThisDtReq = ThisReqDtIsPm(comboBoxDtSubject.Text);
            #endregion

            #region اگر درخواست از نوع پی ام هست بررسی مربوط بودن کد سایت به کارشناس مسیول مربوط منطقه 

            if (bolIsPmThisDtReq)//درخواست پی امی هستش  
            {
                #region تنظیم اتوماتیک 60 دقیقه اوتیج دان تایم های پی امی
                listBoxDtOutageMin.SelectedItem = "60";
                strDtSetOutage = "60";
                txtSerchedOutage.Text = " 60  دقیقه";
                listBoxDtOutageMin.SelectedItem = "60";
                gbxDtOutage.Enabled = false;
                #endregion

                #region    اگه کاربر سطح دسترسی پاور یا ادمین رو داره بذار بگیره
                if (MainForm.StrCurRol == "administrator" || MainForm.StrCurRol == "PowerUser")
                {
                    #region چون کاربر سطح دسترسی بالا دارد به منطقش گیری نده و برو سراغ انجام دهنده و بقیه ماجرا
                    //lstSerchedPerformer.Select();// 
                    #endregion

                    #region اگه نامشخصه نام منطقش رو فرضی بذار یکی از منطقه های همین کارشناس مسوول تا سهمیش کم بشه
                    if (lblDtRegion.Text == "نامشخص")
                    {
                        strRegion = "South";
                    }
                    #endregion
                }
                #endregion

                #region اگه کاربر سطح دسترسی پاور یا ادمین رو نداره ببین کیه آیا میتونه رو این منطقه بگیره
                else
                {
                    #region کاربر سطح دسترسی مدیر یا پاور یوزر نداره بررسی کن ببین اصن جز کارشناس مسوول های دارای مجوز گرفتن دان تایم پی ان هست

                    #region اگه کاربر مجتبی آیته
                    if (MainForm.StrFullNameCurUsr == "مجتبی آیت")
                    {
                        #region  بررسی اینکه سایت درخواستی برای دان تایم پی ام مربوط به منطقه خودش هست یا نامشخصه 

                        #region اگه مربوط به خودشه یا نامشخصه اجازه بده ادامه بده
                        if (lblDtRegion.Text == "Center-N" || lblDtRegion.Text == "West" || lblDtRegion.Text == "نامشخص" || lblDtRegion.Text == "Center-S")
                        {
                            #region منطقه خودشه حالا نامشخص رو هم بهش تخفیف میدیم بذار بره سراغ انجام دهنده و قالو بکنه
                            //lstSerchedPerformer.Select();
                            #endregion



                        }
                        #endregion

                        #region منطقه خودش نیست اجازه نده ادامه بده پیامش بده و برش گردون به کد سایت تا دوباره انتخاب کنه
                        else
                        {
                            MessageBox.Show("کاربر گرامی درخواست پی ام شما مربوط به منطقه تحت پوشش پیمانکاران شما نیست لطفا کد سایت مربوط انتخاب کنید");
                            comboBoxDtSubject.Text = string.Empty;
                            textBoxDtRegCodeSite.Select();
                            textBoxDtRegCodeSite.Focus();//فوکوس روی کد سایت واسه انتخاب صحیح
                            return;
                        }


                        #endregion



                        #endregion
                    }
                    #endregion

                    #region اگه کاربر محسن سلمانیه
                    else if (MainForm.StrFullNameCurUsr == "محسن سلمانی")
                    {
                        #region MyRegion بررسی اینکه سایت درخواستی برای دان تایم پی ام مربوط به منطقه خودش هست یا نامشخصه 

                        #region اگه مربوط به خودشه یا نامشخصه اجازه بده ادامه بده
                        if (lblDtRegion.Text == "North" || lblDtRegion.Text == "East" || lblDtRegion.Text == "نامشخص")
                        {
                            #region منطقه خودشه حالا نامشخص رو هم بهش تخفیف میدیم بذار بره سراغ انجام دهنده و قالو بکنه
                            //lstSerchedPerformer.Select();
                            #endregion

                            #region اگه نامشخصه نام منطقش رو فرضی بذار یکی از منطقه های همین کارشناس مسوول تا سهمیش کم بشه
                            if (lblDtRegion.Text == "نامشخص")
                            {
                                strRegion = "North";
                            }
                            #endregion

                        }
                        #endregion

                        #region منطقه خودش نیست اجازه نده ادامه بده پیامش بده و برش گردون به کد سایت تا دوباره انتخاب کنه
                        else
                        {
                            MessageBox.Show("کاربر گرامی درخواست پی ام شما مربوط به منطقه تحت پوشش پیمانکاران شما نیست لطفا کد سایت مربوط انتخاب کنید");
                            comboBoxDtSubject.Text = string.Empty;
                            textBoxDtRegCodeSite.Select();//فوکوس روی کد سایت واسه انتخاب صحیح
                            textBoxDtRegCodeSite.Focus();
                            return;
                        }


                        #endregion

                        #endregion
                    }

                    #endregion

                    #region کاربر نه آیته نه سلمانی و نه پاور نه ادمین پس نمیتونه دان تایم پی امی بگیره
                    else//کاربر آیت و سلمانی نیست و سطح دسترسی بالا هم ندارد پس بیخود می کنه درخواست دان تایم پی امی بدهد
                    {
                        MessageBox.Show("کاربر گرامی شما مجوز گرفتن دان تایم از نوع پی ام رو ندارید ");
                        comboBoxDtSubject.Text = string.Empty;
                        comboBoxDtSubject.Select();//دوباره موضوع انتخاب کن
                    }
                    #endregion

                    #endregion

                }
                #endregion
            }
            #endregion

            #region اگر درخواست از نوع پی ام نبود فوکوس بر روی انجام دهنده تا پایان ماجرا
            else
            {
                #region لغو تنظیمات اتوماتیک 60 دقیقه اوتیج دان تایم های پی امی
                listBoxDtOutageMin.SelectedItem = string.Empty;
                strDtSetOutage = string.Empty;
                gbxDtOutage.Enabled = true;
                #endregion
            }
            #endregion

            btnDtFromDate.Select();
        }

        private void ComboBoxDtSubject_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));
        }

        private void ComboBoxDtSubject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lstSerchedPerformer.Select();
            }
        }

        private void comboBoxDtSubject_SelectedValueChanged(object sender, EventArgs e)
        {
            bolIsReqPm = ThisReqDtIsPm(comboBoxDtSubject.Text);
            bolIsReqVisit = ThisReqDtIsVisit(comboBoxDtSubject.Text);

            if (comboBoxDtSubject.SelectedValue == null)
                return;

            if (bolIsReqPm)//درخواست پی امیه
            {
               // ResetForNewDownTime();

                gbxVisit.Enabled = false;
                gbxVisit.Visible = false;


                nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = false;
               

                if (textBoxDtRegCodeSite.Text != "")
                {
                    rgpDtTime.Enabled = true;//فعال کردن انتخاب صبح و عصر برای پی ام

                }

                FilldateTimeNumericUpDownForPm();

                //radioButtonIsNotInBusyHour.Checked = true;

                //gbxBusyOver.Enabled = false;

                gbxDtOutage.Enabled = false;

                listBoxDtOutageMin.SelectedItem = "60";
                strDtSetOutage = "60";
                txtSerchedOutage.Text = " 60  دقیقه";
                listBoxDtOutageMin.SelectedItem = "60";
                nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = false;

            }
            else if(bolIsReqVisit)//درخواست بازدیدیه
            {
                gbxVisit.Enabled = true;
                gbxVisit.Visible = true;

                ResetDtRegDateToToday();

                nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = false;

                #region یه جورایی زوری بردن برنامه به سلکتد چنج
                rdbVisitSobh.SelectedIndex = 1;
                rdbVisitSobh.SelectedIndex = 0; 
                #endregion

                rgpDtTime.Enabled = true;

                //radioButtonIsNotInBusyHour.Checked = true;

                


                if (rgpDtTime.SelectedIndex == 0)//صبح
                {
                    gbx_bazdidSobh.Enabled = true;
                    gbx_bazdidAsr.Enabled = false;

                    #region یه تغغیر پلتیکی واسه تشخیص ساعت های نامریک
                    rdbVisitSobh.SelectedIndex = 1;
                    rdbVisitSobh.SelectedIndex = 0; 
                    #endregion

                }
                else if (rgpDtTime.SelectedIndex == 1)//عصر
                {
                    gbx_bazdidSobh.Enabled = false;
                    gbx_bazdidAsr.Enabled = true;

                    #region یه تغغیر پلتیکی واسه تشخیص ساعت های نامریک
                    rdbVisitAsr.SelectedIndex = 1;
                    rdbVisitAsr.SelectedIndex = 0;
                    #endregion
                }


                //gbxBusyOver.Enabled = false;

                gbxDtOutage.Enabled = false;

                listBoxDtOutageMin.SelectedItem = "0";
                strDtSetOutage = "0";
                txtSerchedOutage.Text = " 0  دقیقه";
                listBoxDtOutageMin.SelectedItem = "0";


            }
            else// درخواست غیر پی امی و بازدیده
            {

                ResetForNewDownTime();
                gbxVisit.Enabled = false;
                gbxVisit.Visible = false;

                rgpDtTime.Enabled = false;//غیرفعال کردن انتخاب صبح و عصر برای پی ام
                //radioButtonIsNotInBusyHour.Checked = true;
                //gbxBusyOver.Enabled = true;
                nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = true;
                gbxDtOutage.Enabled = true;

                listBoxDtOutageMin.SelectedItem = string.Empty;
                strDtSetOutage = string.Empty;
                txtSerchedOutage.Text = string.Empty;
                listBoxDtOutageMin.SelectedItem = string.Empty;
            }
        }
        #endregion

        #region txtPerformer
        private void txtDtPerformer_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void txtDtPerformer_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion

        #region NumericUpDown
        private void FilldateTimeNumericUpDownForPm()
        {
            if (rgpDtTime.SelectedIndex == 0 && bolIsReqPm)//انتخاب بازه ی صبح
            {
                #region  نامریک آپ و دان ها را برای پی ام صبح
                nudDtStHour.Value = 7;
                nudDtEndHour.Value = 13;

                nudDtStMin.Value = 0;
                nudDtEndMin.Value = 0;
                #endregion
            }
            else if (rgpDtTime.SelectedIndex == 1 && bolIsReqPm)
            {
                #region  نامریک آپ و دان ها را برای پی ام عصر
                nudDtStHour.Value =13;
                nudDtEndHour.Value = 19;

                nudDtStMin.Value = 0;
                nudDtEndMin.Value = 0;
                #endregion
            }

            nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = false;

            //#region موقت نامریک آپ و دان ها را برای پی ام ها گذاشتم
            //nudDtStHour.Value = 8;
            //nudDtEndHour.Value = 16;

            //nudDtStMin.Value = 0;
            //nudDtEndMin.Value = 0;
            //#endregion
        }

        private void NudDtStMin_ValueChanged(object sender, EventArgs e)
        {
            if (nudDtStMin.Value == 60)
            {
                nudDtStMin.Value = 0;
            }
        }

        private void NudDtEndMin_ValueChanged(object sender, EventArgs e)
        {
            if (nudDtEndMin.Value == 60)
            {
                nudDtEndMin.Value = 0;
            }
        }
        #endregion

        #region Functions

        #region سفارشی کردن موضوع بر اساس نقش کاربر
        private void CustumizeSubjectBoxWithRol()
        {
            if (MainForm.StrCurRol == "pm_requester")
            {
                gbxDtAllSubject.Enabled = false;
            }
            else
            {
                gbxDtAllSubject.Enabled = true;
            }
        }


        #endregion

        private void RegisterDownTime(bool bolIsRepeatedPmDownTime)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();

            if (MessageBox.Show($"آیا برای ایجاد {"دان تایم  "} بر روی سایت یا مورد {this.txtDtRegSiteName.Text} مطمئن هستید؟", "تاییدیه اعمال  تغییرات  ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string StrDtTempForSubjectCombo = string.Empty;

                #region حفظ سابقه کومبوها
                if (MainForm.StrCurRol == "pm_requester")
                {
                    //if(radioGroupReqPm.SelectedIndex==0)
                    //{
                    //    StrDtTempForSubjectCombo = "PM-Cleaning";
                    //}
                    //else if(radioGroupReqPm.SelectedIndex == 1)
                    //{
                    //    StrDtTempForSubjectCombo = "PM-Site";
                    //}
                    //else if(radioGroupReqPm.SelectedIndex == 2)
                    //{
                    //    StrDtTempForSubjectCombo = "PM-Tower";
                    //}

                }
                else//بقیه غیر پی ام چی ها
                {
                    StrDtTempForSubjectCombo = comboBoxDtSubject.Text;
                }


                string StrDtTempForPerformerCombo = strTempForPerformer;
                #endregion

                #region چک بزرگتر بودن تاریخ شروع و پایان

                DateTime dteStDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrDtStartDate);

                DateTime dteMiladiStartDate = dteStDate;
                //DateTime dteMiladiStartDate = DateTime.Parse(MainForm.StrDtStartDate.Substring(0, 10)).Date;
                var dteEndDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrDtEndDate);

                DateTime dteMiladtStartDt = dteStDate.AddHours(double.Parse(nudDtStHour.Value.ToString())).AddMinutes(double.Parse(nudDtStMin.Value.ToString()));
                DateTime dteMiladtEndDt = dteEndDate.AddHours(double.Parse(nudDtEndHour.Value.ToString())).AddMinutes(double.Parse(nudDtEndMin.Value.ToString()));

                dblDtTotalMinuteTime = (dteMiladtEndDt - dteMiladtStartDt).TotalMinutes;

                int intCompareDate = DateTime.Compare(dteEndDate, dteStDate);

                if (intCompareDate < 0)
                {
                    MessageBox.Show(" تاریخ پایان دان تایم باید مساوی یا بزرگتر از تاریخ امروز باشد ");
                    return;
                }
                else if (intCompareDate == 0)
                {
                    if (nudDtStHour == nudDtEndHour && nudDtStMin == nudDtEndMin)
                    {
                        MessageBox.Show(" زمان شروع و پایان دان تایم یکسان تنظیم شده لطفا اصلاح بفرمایید ");
                        return;
                    }
                }

                #endregion

                if(MainForm.StrCurRol=="ts")
                {
                    StrDtTempForSubjectCombo = "رفع خرابی";
                    strDtSetOutage = "0";
                }

                //در صورت عدم تغییر رادیو باتن های دامین  پیش فرض سایت بشود
                if (strDomain == "" || strDomain == null)
                {
                    strDomain = "Site";
                }
                //چسباندن دامین به اول موضوع
                if (strDomain != "Site")
                {
                    strSubject = string.Concat(strDomain.Trim(), " - ", comboBoxDtSubject.Text.Trim());
                }
                else
                {
                    strSubject = comboBoxDtSubject.Text.Trim();
                }

                #region چک تکمیل بودن همه فیلدها
                //متغییر پر بودن همه فیلدها
                bool bolIsUnCompAllDtField =
                    (this.textBoxDtRegCodeSite.Text == string.Empty) ||
                    (this.txtDtRegSiteName.Text == string.Empty) ||
                    (StrDtTempForSubjectCombo == string.Empty) ||
                    (strTelOfPerformer == string.Empty || strTelOfPerformer == "نامشخص") ||
                     (strDtSetOutage == string.Empty) ||
                    (this.textBoxDtRegCodeSite.Text.Length != 4)
                    ;
                #endregion

                #region اگر پارامتر یا پارامترهایی از موارد خواسته شده دان تایم ناقصه پیام مناسب و خروج
                if (bolIsUnCompAllDtField)
                {
                    #region چک تکمیل بودن همه فیلدها برای پیدا کردن مورد یا موارد ناکامل
                    string str = string.Empty;
                    if (this.textBoxDtRegCodeSite.Text == string.Empty)
                    {
                        str = str + "کد سایت و ";
                    }
                    if (this.txtDtRegSiteName.Text == string.Empty)
                    {
                        str = str + "نام سایت و ";
                    }
                    if (strTelOfPerformer == string.Empty)
                    {
                        str = str + " و outage";
                    }
                    if (StrDtTempForSubjectCombo == string.Empty)
                    {
                        str = str + "موضوع و ";
                    }
                    if (strTelOfPerformer == string.Empty || strTelOfPerformer == "نامشخص")
                    {
                        str = str + "انجام دهنده و ";
                    }
                    if (strDtSetOutage == string.Empty)
                    {
                        str = str + "قطعی محتمل و ";
                    }
                    if (this.textBoxDtRegCodeSite.Text.Length != 4)
                    {
                        str = str + " کد سایت باید 4 رقم باشد و ";
                    }
                    str = str.Substring(0, str.Length - 2);
                    MessageBox.Show($"  کاربر گرامی فیلد  {str} تکمیل نشده است لطفا درخواست خود را تکمیل بفرمایید", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    return;
                    #endregion
                }
                #endregion

                #region اگر همه پارامترها کامله برو سراغ کارهای گرفتن دان تایم
                else
                {
                    #region اگرنام سایت نامشخصه پیام مناسب بده

                    if (txtDtRegSiteName.Text == "نامشخص")
                    {

                        DialogResult dialogResultDelPersonal = MessageBox.Show(" نام سایت نامشخص می باشد  و احتمالا کد سایت نامعتبر وارد کرده اید آیا تمایل دارید این دان دان تایم ثبت گردد؟  ", "  اخطار ثبت دان تایم با نام نامشخص  ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                        if (dialogResultDelPersonal == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            return;
                        }

                    }

                    #region استخراج تاریخ شروع و پایان دان تایم درخواستی از صفحه تقویم فارسی یا تاریخ پیش فرض اس کیو ال

                    DateTime dateTimeSqlServer = MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();

                    string strDtFromYear = string.Empty;

                    if (MainForm.StrDtStartDate != string.Empty)
                    {
                        strDtFromYear = MainForm.StrDtStartDate.Substring(0, 4);
                    }
                    else
                    {
                        strDtFromYear = ClsConvertDateHassan.convertDateToPersion(dateTimeSqlServer).Substring(0, 4);
                    }

                    string strDtFromMonth = string.Empty;
                    if (MainForm.StrDtStartDate != string.Empty)
                    {
                        strDtFromMonth = MainForm.StrDtStartDate.Substring(5, 2);
                    }
                    else
                    {
                        strDtFromMonth = ClsConvertDateHassan.convertDateToPersion(dateTimeSqlServer).Substring(5, 2);
                    }

                    string strDtFromDay = string.Empty;
                    if (MainForm.StrDtStartDate != string.Empty)
                    {
                        strDtFromDay = MainForm.StrDtStartDate.Substring(8, 2);
                    }
                    else
                    {
                        strDtFromDay = ClsConvertDateHassan.convertDateToPersion(dateTimeSqlServer).Substring(8, 2);
                    }


                    string strDtToYear = string.Empty;
                    if (MainForm.StrDtEndDate != string.Empty)
                    {
                        strDtToYear = MainForm.StrDtEndDate.Substring(0, 4);
                    }
                    else
                    {
                        strDtToYear = ClsConvertDateHassan.convertDateToPersion(dateTimeSqlServer).Substring(0, 4);
                    }


                    string strDtToMonth = string.Empty;
                    if (MainForm.StrDtEndDate != string.Empty)
                    {
                        strDtToMonth = MainForm.StrDtEndDate.Substring(5, 2);
                    }
                    else
                    {
                        strDtToMonth = ClsConvertDateHassan.convertDateToPersion(dateTimeSqlServer).Substring(5, 2);
                    }

                    string strDtToDay = string.Empty;
                    if (MainForm.StrDtEndDate != string.Empty)
                    {
                        strDtToDay = MainForm.StrDtEndDate.Substring(8, 2);
                    }
                    else
                    {
                        strDtToDay = ClsConvertDateHassan.convertDateToPersion(dateTimeSqlServer).Substring(8, 2);
                    }
                    #endregion

                    #region پیدا کردن ساعت فعلی سرور و زمان شروع و پایان درخواستی

                    PersianCalendar calendar = new PersianCalendar();

                    DateTime time2 = calendar.ToDateTime(int.Parse(strDtFromYear), int.Parse(strDtFromMonth), int.Parse(strDtFromDay), int.Parse(this.nudDtStHour.Value.ToString()), int.Parse(this.nudDtStMin.Value.ToString()), 0, 0);
                    DateTime time3 = calendar.ToDateTime(int.Parse(strDtToYear), int.Parse(strDtToMonth), int.Parse(strDtToDay), int.Parse(this.nudDtEndHour.Value.ToString()), int.Parse(this.nudDtEndMin.Value.ToString()), 0, 0);

                    #endregion

                    if (time2 > time3)//چک بزرگتر نبودن زمان شروع از پایان
                    {
                        MessageBox.Show(".به علت بزرگتر بودن زمان شروع از پایان مجوز شما ثبت نگردید مجددا اصلاح و  ثبت نمایید");
                        MainForm.BolDtIsCreateDownTime = false;
                    }
                    else
                    {
                        #region ثبت دان تایم در صورت درست بودن زمانها

                        #region پیدا کردن شماره بعدی برای ثبت دان تایم جدید
                        int num;
                        string s = objClsSql.FindFieldInLastRecord("[DT].[dbo].tblDownTimeShort", "row", "row");

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

                        #region ثبت دان تام

                        #region استخراج زمان سرور برای ثبت زمان رجیستر
                        string str4 = ClsConvertDateHassan.ConvertDateToPersian(MainForm.DateTimeSqlServer);//استخراج زمان ثبت از اس کیو ال سرور
                        string str5 = MainForm.DateTimeSqlServer.ToString("HH:mm");
                        #endregion

                        #region استخراج منطقه
                        string strRegion = lblDtRegion.Text;
                        #endregion

                        #region ساخت آرایه فیلدها
                        string strFieldsName = "[row],[registerDate],[registerTime],[siteCode],[domain],[siteName],[curBSC],[startDate],[startTime],[endDate],[endTime],[applicant],[subject],[performer],[approver],[dtNo],[Status],[region],[Note],[outageMin]";
                        #endregion

                        #region استخراج زمان شروع و پایان دان تایم درخواستی از نامریک ها
                        string str7 = this.nudDtStHour.Value.ToString("00");
                        string str8 = this.nudDtStMin.Value.ToString("00");
                        string str9 = this.nudDtEndHour.Value.ToString("00");
                        string str10 = this.nudDtEndMin.Value.ToString("00");
                        #endregion

                        string strDateTimeStartMiladi = dteStDate.ToString("MMM") + " " + dteStDate.Day + " " + dteStDate.Year;

                        //#region تشخیص در بیزی آور بودن یا نه
                        //if (radioButtonIsInBusyHour.Checked&& !strDtSetOutage.Contains("_YES"))
                        //{
                        //    strDtSetOutage += "_YES";
                        //}
                        //else if(!strDtSetOutage.Contains("_NO"))
                        //{
                        //    strDtSetOutage += "_NO";
                        //}

                        //#endregion

                        string strValues = string.Empty;
                        if (StrDtTempForSubjectCombo.Contains("pM-"))
                        {
                            StrDtTempForSubjectCombo.Replace("pM-", "PM-");
                        };

                        if (strNoteForPmSameSubAndCodeAndDate != string.Empty && !strNoteForPmSameSubAndCodeAndDate.Contains("row"))
                        {
                            string strDt = strNoteForPmSameSubAndCodeAndDate.Substring(0, 14);

                            string strApprover = "Auto";
                            strValues = string.Concat(new object[] {
                                "N'", intNewRow.ToString(), "',N'", str4, "',N'", str5, "',N'", "ES"+this.textBoxDtRegCodeSite.Text.Trim(), "',N'",strDomain, "',N'", this.txtDtRegSiteName.Text.Trim(), "',N'", string.Empty, "',N'", strDtFromYear, "/", strDtFromMonth,
                                "/", strDtFromDay, "',N'", str7, ":", str8, "',N'", strDtToYear, "/", strDtToMonth, "/", strDtToDay, "',N'", str9, ":", str10,
                                "',N'", MainForm.StrFullNameCurUsr, "',N'", strSubject, "',N'",  strTempForPerformer.Trim()+"-"+strTelOfPerformer+"-"+strTelOfPerformer, "',N'",strApprover, "',N'", string.Empty, "',N'ثبت در (SDM)',N'"+strRegion+"',N'"+strNoteForPmSameSubAndCodeAndDate+"',N'"+strDtSetOutage+"'"
                            }); ;
                        }
                        else if (strNoteForPmSameSubAndCodeAndDate != string.Empty && strNoteForPmSameSubAndCodeAndDate.Contains("row"))
                        {


                            strValues = string.Concat(new object[] {
                                "N'", intNewRow.ToString(), "',N'", str4, "',N'", str5, "',N'", "ES"+this.textBoxDtRegCodeSite.Text.Trim(), "',N'",strDomain, "',N'", this.txtDtRegSiteName.Text.Trim(), "',N'", string.Empty, "',N'", strDtFromYear, "/", strDtFromMonth,
                                "/", strDtFromDay, "',N'", str7, ":", str8, "',N'", strDtToYear, "/", strDtToMonth, "/", strDtToDay, "',N'", str9, ":", str10,
                                "',N'", MainForm.StrFullNameCurUsr, "',N'",strSubject, "',N'",  strTempForPerformer.Trim()+"-"+strTelOfPerformer, "',N'","',N'", "',N'ثبت توسط کاربر',N'"+strRegion+"',N'"+strNoteForPmSameSubAndCodeAndDate+"',N'"+strDtSetOutage+"'"
                            }); ;
                        }
                        else
                        {
                            if(MainForm.StrCurRol!="ts")
                            {
                                string strDt = string.Empty;
                                string strApprover = string.Empty;
                                
                                strValues = string.Concat(new object[] {
                                "N'", intNewRow.ToString(), "',N'", str4, "',N'", str5, "',N'", "ES"+this.textBoxDtRegCodeSite.Text.Trim(), "',N'",strDomain, "',N'", this.txtDtRegSiteName.Text.Trim(), "',N'", string.Empty, "',N'", strDtFromYear, "/", strDtFromMonth,
                                "/", strDtFromDay, "',N'", str7, ":", str8, "',N'", strDtToYear, "/", strDtToMonth, "/", strDtToDay, "',N'", str9, ":", str10,
                                "',N'", MainForm.StrFullNameCurUsr, "',N'", strSubject, "',N'",  strTempForPerformer.Trim()+"-"+strTelOfPerformer, "',N'",strApprover, "',N'",strDt, "',N'ثبت توسط کاربر',N'"+strRegion+"',N'"+strNoteForPmSameSubAndCodeAndDate+"',N'"+strDtSetOutage+"'"
                                 }); ;
                            }
                            else
                            {
                                string strDt = "0";
                                string strApprover = "اتوماتیک";
                                strDtFromYear = "0000";
                                strDtFromMonth = "00";
                                strDtFromDay = "00";
                                strDtToYear = "0000";
                                strDtToMonth = "00";
                                strDtToDay = "00";
                                str7= "0";
                                str9 = "0";

                                strValues = string.Concat(new object[] {
                                "N'", intNewRow.ToString(), "',N'", str4, "',N'", str5, "',N'", "ES"+this.textBoxDtRegCodeSite.Text.Trim(), "',N'",strDomain, "',N'", this.txtDtRegSiteName.Text.Trim(), "',N'", string.Empty, "',N'", strDtFromYear, "/", strDtFromMonth,
                                "/", strDtFromDay, "',N'", str7, ":", str8, "',N'", strDtToYear, "/", strDtToMonth, "/", strDtToDay, "',N'", str9, ":", str10,
                                "',N'", MainForm.StrFullNameCurUsr, "',N'", strSubject, "',N'",  strTempForPerformer.Trim()+"-"+strTelOfPerformer, "',N'",strApprover, "',N'",strDt, "',N'تائید',N'"+strRegion+"',N'"+strNoteForPmSameSubAndCodeAndDate+"',N'"+strDtSetOutage+"'"
                                 }); ;
                            }
                            
                        }

                        //#region اضافه کردن پی ام ای دی در صورت پی امی بودن
                        //if (ThisReqDtIsPm(comboBoxDtSubject.Text))
                        //{
                        //    strFieldsName += ",[pm_id]";
                        //    strValues += ",N'ES" + this.textBoxDtRegCodeSite.Text.Trim() + strDtFromYear + strDtFromMonth + strDtFromDay + "'";
                        //}
                        //#endregion

                        //اینجوس

                        #region ثبت در اس کیو ال
                        bolIsSucssesRegDownTime = objClsSql.GetAllAndSendToDataBase("[DT].[dbo].tblDownTimeShort", strFieldsName, strValues);//ثبت 
                        #endregion

                        //#region ثبت در مای اس کیو ال
                        //ClsDiff objClsDiff = new ClsDiff();
                        //objClsDiff.RegisterDownTimeInMySql(strFieldsName.Replace("[", string.Empty).Replace("]", string.Empty), strValues.Replace("[", string.Empty).Replace("]", string.Empty));
                        //#endregion

                        //نکته اینکه تمام دان تایمها قرار است در مای اسکیو ال هم ثبت بشود ولی فقط پی امی ها پی ام آدی بخورد
                        if (!bolIsSucssesRegDownTime)
                        {
                            MessageBox.Show("در ثبت دان تایم مشکلی پیش آمد لطفا بررسی فرمایید", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                            textBoxDtRegCodeSite.Select();
                            return;
                        }

                        else
                        {
                            ClsDiff.AutoClosingMessageBox.Show("ثبت شد", "موفق", 2000);
                        }
                        #region ثبت شماره دان تایم تکراری و آخرین دان تایمی مرجعی که مشابه آنرا داریم در دیتابیس
                        if (bolIsRepeatedPmDownTime)
                        {
                            objClsSql.GetAndSendToDataBaseAndReturnResult("[DT].[dbo].[tblRepetedDt]", "[repeatedRow]", intNewRow);
                            objClsSql.EditRecord("[DT].[dbo].[tblRepetedDt]", "[sourceOfRepeatedRow]", strSourceRowOfRepeatedRow, "Where [repeatedRow]=N'" + intNewRow + "'");
                            bolIsRepeatedPmDownTime = false;
                            strSourceRowOfRepeatedRow = string.Empty;
                        }
                        #endregion

                        MainForm.BolDtIsCreateDownTime = true;//اعلام ثبت

                        MainForm.IntDtCountTmrForNewDt = 0;
                       // strDomain = "";

                        //MainForm.bolDtForceToRefreshAndGoToEnd = true;
                        //MainForm.bolDtForceToRefreshAndDontGoToEnd = false;

                        // MessageBox.Show(".درخواست شما با موفقیت ثبت و ثبت توسط کاربر می باشد", "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        #endregion

                        #endregion
                    }
                    #endregion
                }

                #endregion

                #region اگه ثبت موفق بود آماده سازی مجدد کومبوباکسها و ادیت تکست ها
                if (MainForm.BolDtIsCreateDownTime)
                {
                    #region خالی کردن تکس باکس ها
                    this.textBoxDtRegCodeSite.Text = string.Empty;
                    this.txtDtRegSiteName.Text = "----------------";
                    #endregion

                    #region اجبار به رفرش و رفتن به سطر آخر
                    MainForm.bolDtForceToRefreshAndGoToEnd = true;
                    MainForm.bolDtForceToRefreshAndDontGoToEnd = false;
                    #endregion

                    #region رفرش کومبوباکسها
                    //this.refreshComboBoxSubject();

                    // this.refreshComboBoxPerformer(); 
                    #endregion

                    MainForm.BolDtIsCreateDownTime = false;

                    ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOftblDt");
                }
                #endregion

                #region ثبت در لاگ
                ClsDiff.writeLogOfAction(" ایجاد دان تایم جدید ");
                #endregion

                #region برگرداندن حافظه به کومبوها
                comboBoxDtSubject.Text = StrDtTempForSubjectCombo;
                lstSerchedPerformer.SelectedItem = strTempForPerformer;
                #endregion

                #region تمرکز مجدد بروی تکس باکس کد سایت
                this.textBoxDtRegCodeSite.Focus();
                this.textBoxDtRegCodeSite.Select();
                #endregion

            }
            else if (bolIsRepeatedPmDownTime)
            {
                
            }

        }

        private void RegisterDownTimeInMySql(string strFieldsName, string strValues)
        {
            strValues = strValues.Replace("[", string.Empty).Replace("]", string.Empty);


            #region 

            //bool bolIsRegInMySql = objClsMySql.Insert("mobilechat.tblDownTime1", strFieldsName, strValues);
            //if (!bolIsRegInMySql)
            //{
            //    MessageBox.Show("ایجاد دان تایم در مای اسکیول انجام نشد لطفا به مدیر سیستم اطلاع دهید");
            //}

            #endregion

        }
        private bool ThisDtExactlyExisted()
        {
            string strTxtCodeSite = textBoxDtRegCodeSite.Text;


            string strSubject = comboBoxDtSubject.Text;
            string strStartTimeHour = nudDtStHour.Value > 10 ? nudDtStHour.Value.ToString() : "0" + nudDtStHour.Value;
            string strStartTimeMin = nudDtStMin.Value > 10 ? nudDtStMin.Value.ToString() : "0" + nudDtStMin.Value;
            MainForm.strDtReqStartTime = strStartTimeHour + ":" + strStartTimeMin;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND startDate=N'" + MainForm.strDtStartDate + "' AND startTime=N'" + MainForm.strDtReqStartTime + "' AND [subject]=N'" + strSubject + "')  order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);

            bool bolIsExist = false;
            if (dt.Rows.Count > 0)
            {
                bolIsExist = true;
            }
            else
            {
                bolIsExist = false;
            }

            return bolIsExist;
        }

        public bool ThisReqDtIsPm(string strSubjectForPm)
        {
            bool bolThisReqDtIsPm = false;


            if (strSubjectForPm == "PM-Cleaning" || strSubjectForPm == "PM-Site" || strSubjectForPm == "PM-Tower")
            {
                bolThisReqDtIsPm = true;
            }
            else
            {
                bolThisReqDtIsPm = false;
            }

            return bolThisReqDtIsPm;
        }

        public bool ThisReqDtIsVisit(string strSubjectForPm)
        {
            bool bolThisReqDtIsVisit = false;


            if (strSubjectForPm == "Visit_TroubleShooting" || strSubjectForPm == "Visit_Aras Documentation" || strSubjectForPm == "Visit_Transmission Issue" || strSubjectForPm == "Visit_Roll Out Issue")
            {
                bolThisReqDtIsVisit = true;
            }
            else
            {
                bolThisReqDtIsVisit = false;
            }

            return bolThisReqDtIsVisit;
        }

        private string GetRowOfExactlyRepeatedDowntime()
        {
            string strTxtCodeSite = textBoxDtRegCodeSite.Text;
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);

            string strSubject = comboBoxDtSubject.Text;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "') order by row desc";
            string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            return strFindedRowOfRepeatedDt;
        }

        private void EvaluteDownTimeAndRegDt()//بررسی درخواست و در صورت عدم اشکال گرفتن دان تایم
        {
            #region استخراج شماره تلفن انجام دهنده و تهیه ترکیب نام و شماره
            string strSqlQuery = "SELECT Tel FROM [TEL].[dbo].tblTel Where Name=N'" + strTempForPerformer.Trim() + "' AND WorkPlace=N'" + txtRegDtWorkPlace.Text + "'";
            strTelOfPerformer = objClsSql.SendCommandGetStringResult(strSqlQuery);
            #endregion

            #region استخراج منطقه بر اساس کد سایت
            if (lblDtRegion.Text != "نامشخص")
            {
                strRegion = lblDtRegion.Text;
            }
            else
            {
                if (MainForm.StrFullNameCurUsr == "محسن سلمانی")
                {

                    strRegion = "North";

                }
                else if (MainForm.StrFullNameCurUsr == "مجتبی آیت")
                {
                    strRegion = "West";
                }

                else if (MainForm.StrFullNameCurUsr == "حمید سبحانی")
                {
                    strRegion = "South";
                }

            }


            #endregion

           

            #region در صورتیکه دان تایم درخواست رفع خرابی هست ترک تشریفات زمانی بکن و بگیر بره

            if (MainForm.StrCurRol == "ts")
            {
                RegisterDownTime(false);
                return;
            } 
            #endregion

            string str = txtDtToDate.Text;

            #region چک بزرگتر بودن تاریخ شروع از پایان

            DateTime dteStDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrDtStartDate);


            var dteEndDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrDtEndDate);

            DateTime dteMiladtStartDt = dteStDate.AddHours(double.Parse(nudDtStHour.Value.ToString())).AddMinutes(double.Parse(nudDtStMin.Value.ToString()));
            DateTime dteMiladtEndDt = dteEndDate.AddHours(double.Parse(nudDtEndHour.Value.ToString())).AddMinutes(double.Parse(nudDtEndMin.Value.ToString()));

            dblDtTotalMinuteTime = (dteMiladtEndDt - dteMiladtStartDt).TotalMinutes;

            int intCompareDate = DateTime.Compare(dteEndDate, dteStDate);

            if (intCompareDate < 0)
            {
                MessageBox.Show(" تاریخ پایان دان تایم باید مساوی یا بزرگتر از تاریخ امروز باشد ");
                return;
            }
            else if (intCompareDate == 0)
            {
                if (nudDtStHour == nudDtEndHour && nudDtStMin == nudDtEndMin)
                {
                    MessageBox.Show(" زمان شروع و پایان دان تایم یکسان تنظیم شده لطفا اصلاح بفرمایید ");
                    return;
                }
            }

            #endregion

            #region چک بزگتر بودن زمان شروع از الان که تایم اس کیو ال سرور هستش

            DateTime dteSqlServer = objClsSql.GetDateTimeSqlServer();

            string strTimeSqlServer = dteSqlServer.Hour.ToString("00") + dteSqlServer.Minute.ToString("00");
            long lngDteTimeSqlServer = ClsConvertDateHassan.ConvertMiladiDateToLong(dteSqlServer);



            string strRequestedTime = nudDtStHour.Value.ToString("00") + nudDtStMin.Value.ToString("00");


            int intStartHour = int.Parse(nudDtStHour.Value.ToString());
            int intStartMin = int.Parse(nudDtStMin.Value.ToString());

            long lngRequestedStartDateMiladi = ClsConvertDateHassan.ConvertMiladiDateToLong(dteStDate.AddHours(intStartHour).AddMinutes(intStartMin));

            if (lngRequestedStartDateMiladi < lngDteTimeSqlServer)
            {
                MessageBox.Show("زمان شروع کوچکتر از زمان فعلی تنظیم شده لطفا اصلاح بفرمایید");
                return;

            }


            #endregion

            //#region بررسی ساعت مناسب ایجاد دان تایم در صورتیکه کاربر پاور یا ادمین نیست

            //if (MainForm.StrCurRol != "administrator" && MainForm.StrCurRol != "ts")
            //{
            //    //string strSetedTime = objClsSql.SendCommandGetStringResult("Select lastPermitTimeForDownTime from DT.dbo.tblLastPermitTimeForDt where row='1'").Insert(2, ":");
            //    string strSetedTime = objClsSql.SendCommandGetStringResult("Select lastPermitTimeForDownTime from DT.dbo.tblLastPermitTimeForDt  where id='1'").Substring(0, 4);

            //    int intSetedLimitTime = int.Parse(strSetedTime.Replace(":", string.Empty));

            //    int intCurTime = int.Parse(MainForm.DateTimeSqlServer.ToShortTimeString().Replace(":", string.Empty));

            //    if (intCurTime > intSetedLimitTime)
            //    {
            //        string strMsg = @" کاربر گرامی به منظور تکمیل پروسه دان تایمهای درخواستی امروز پس از ساعت  " + strSetedTime.Substring(2, 2) + " : " + strSetedTime.Substring(0, 2) + " سیستم قادر به پذیرش درخواست جدید نمی باشد  ";
            //    MessageBox.Show(strMsg, "توجه", MessageBoxButtons.OK,
            //    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
            //    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);


            //     // MessageBox.Show("  کاربر گرامی به منظور تکمیل پروسه دان تایمهای درخواستی امروز پس از ساعت  " + strSetedTime + " سیستم قادر به پذیرش درخواست جدید نمی باشد  ");
            //        return;
            //    }

            //}

            //#endregion

            #region استخراج ماه روز درخواست دان تایم
            string strYearMonth = MainForm.StrDtStartDate.Substring(0, 7).Replace("/", "_");
            #endregion

            //نکته اینکه تکراری با دقیقا همینه در تاریخ و روز فرق دارند

            #region اگر دقیقا همین دان تایم قبلا درخواست شده استخراج ردیف مرجع تکرار وپیام مناسب و خروج 0

            #region ریست شاخص دان تایم تکراری
            MainForm.bolDtIsRepeatedDt = false;
            strSourceRowOfRepeatedRow = string.Empty;
            #endregion

            #region پیدا کردن ردیف  دان تایم دقیقا مثل همین و پیام مناسب و خروج
            if (ThisDtExactlyExisted())
            {
                #region  خود خودشه -اعلام اینکه دان تایم در تاریخ شروع با همین مضمون روی همین کدسایت دان تایم گرفته شده

                #region پیدا کردن ردیف دان تایم ثبت شده که دقیقا مشابه دان تایم درخواستی هستش  
                string strRowOfRepeatedRequestedRow = GetRowOfExactlyRepeatedDowntime();
                #endregion

                #region دادن پیغام بابا اینو گرفتی حالا تایید شده یا نه چرا دوباره میگیری . نمیشه . بیخیال
                if (strRowOfRepeatedRequestedRow != null && strRowOfRepeatedRequestedRow != string.Empty && strRowOfRepeatedRequestedRow != "نامشخص")//اگر ردیفی دان تایم دقیقا مشابه و تکراری پیدا کردی
                {
                    MessageBox.Show(" کاربر گرامی نظر به اینکه یک دان تایم با همین کد سایت با همین موضوع در همین تاریخ شروع در ردیف  " + strRowOfRepeatedRequestedRow + "  ثبت شده است شما مجاز به ثبت این دان تایم نیستید ");
                    return;//خروج
                }
                #endregion

                #endregion
            }
            #endregion

            #endregion

            #region در صورتیکه دان تایم درخواستی دقیقا مشابه قبلی ندارد 3

            #region موضوع دان تایم پی ام هستش 1

            bool bolIsPmThisDtReq = ThisReqDtIsPm(comboBoxDtSubject.Text);

            #region اگر درخواست پی امی هستش و با همین کدسایت با همین تاریخ با موضوع متفاوت دان تایمی درخواست شده دوباره نگیریم
            if (bolIsPmThisDtReq)
            {
                #region تنظیم اتوماتیک 60 دقیقه اوتیج دان تایم های پی امی
                listBoxDtOutageMin.SelectedItem = "60";
                strDtSetOutage = "60";
                txtSerchedOutage.Text = " 60  دقیقه";
                listBoxDtOutageMin.SelectedItem = "60";
                gbxDtOutage.Enabled = false;
                #endregion

                #region conflict cr  <= is PM  AND date=date  AND codeSite=CodeSite subject!=Subject
                strNoteForPmSameSubAndCodeAndDate = string.Empty;
                strNoteForPmSameSubAndCodeAndDate = WhichRowIsPmRequestAndThatHasSameCodeSiteAndDate();
                #endregion

                #region مشخص کردن اینکه آیا در ماه جاری تکراریه یعنی کد سایت و موضوعش در ماه جاریه این سال جاری وجوداره

                #region در ماه جاری تکرایه
                if (IsRepeatedDtThisReqInThisMonthOfYear())// 3تکراریه وضعیت و ردیفشو دربیار تا ببینیم چیکارش باید کرد
                {
                    #region ردیف آخرین جایی که تکرار شده رو در بیار

                    string strRowNumberOfLastRepeatedDt = strSourceRowOfRepeatedRow = getLastRowNumberOfRepeatedDt();


                    #endregion

                    #region وضعیت آخرین جایی رو هم که دان تایم تکرار شده رو در بیار ببینم
                    string strStateOfLastRepeatedDt = getLastStateOfRepeatedDt();

                    #endregion

                    #region اگر وضیعت غیر از تایید است بررسی کن وضعیت در حال پردازش یا ثبت در اس دی ام هست یا نه

                    #region اگر وضعیت تایید نیست بررسی کن درحال پردازشه یا در وضعیت ثبت در اس دی ام ه یا غیر از اون که یوقت کلاه سرمون نره
                    if (strStateOfLastRepeatedDt != "تائید")
                    {
                        #region اگر وضعیت در حال پردازش درخواست می باشد
                        if (strStateOfLastRepeatedDt == "در حال پردازش درخواست")
                        {
                            MessageBox.Show(" کاربر گرامی شما قبلا در ردیف   " + strRowNumberOfLastRepeatedDt + " همین دان تایم را درخواست کرده اید و در حال پردازش درخواست شما هستیم و امکان درخواست همین دان تایم در ماه جاری وجود ندارد ");
                            return;//خروج
                        }

                        #endregion

                        #region اگر وضعیت ثبت در اس دی ام هست
                        else if (strStateOfLastRepeatedDt == "ثبت در (SDM)")
                        {
                            MessageBox.Show(" کاربر گرامی شما قبلا در ردیف   " + strRowNumberOfLastRepeatedDt + " همین دان تایم را درخواست کرده اید و در وضعیت ثبت در اس دی ام و منتظر تایید تهران هستیم وامکان درخواست همین دان تایم در ماه جاری وجود ندارد ");
                            return;//خروج
                        }
                        #endregion

                        #region Bاگر تایید نشده و در وضعیت پردازش و ثبت در اس دی ام نیست یک دان تایم معمولی بگیر
                        else
                        {
                            RegisterDownTime(false);//یک دان تایم معمولی بگیر چون تایید نشده که از سهمیه این بدبختا کم کنیم
                            return;//خروج
                        }
                        #endregion
                    }
                    #endregion

                    #region اگه تایید شده برو تو کار بررسی سهمیه تکراری ها و بررسی داشتن یا نداشتن اجازه و اگه اجازه داره بسته به کنسل بودن یا نه میزان یک یا دو واحد از سهمیه کم کن
                    else
                    {
                        #region یادداشت آخرین جایی رو هم که دان تایم تکرار شده رو در بیار ببینم
                        string strNoteOfLastRepeatedDt = getNotetStateOfRepeatedDt();
                        #endregion

                        #region دان تایم تکراری میخواد ولی دمش گرم قبلا کنسل کرده بنابراین اگه سهمیه داره یکی از سهمیش کم کن و براش بگیر
                        //if (strNoteOfLastRepeatedDt == "کنسل")
                        if (strNoteOfLastRepeatedDt.Contains("کنسل"))
                        {
                            #region محاسبه  سهمیه
                            int intRemaindedRep = RemaindedRep(strRegion, strYearMonth);

                            bool bolHaveRemainRepForDownTime = intRemaindedRep > 1 ? true : false;// یعنی سهمیه باقی مانده 
                            #endregion

                            #region داشتن سهمیه و سوال برای ادامه و کسر
                            if (bolHaveRemainRepForDownTime)//سهمیه دارد
                            {

                                #region اگر موفقیت آمیز بود کسر سهمیه
                                DialogResult dialogResult = MessageBox.Show(" همین دان تایم را درخواست کرده و کنسل کرده اید و در صورت ادامه یک واحد از سهمیه دان تایم تکراری شما کسر خواهد شد آیا ادامه می دهید؟ " + strRowNumberOfLastRepeatedDt + "در ردیف ", "توجه", MessageBoxButtons.YesNo);
                                #endregion

                                #region اگه موافقه که یکی از سهمییش کم بشه بگیر
                                if (dialogResult == DialogResult.Yes)
                                {

                                    #region ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
                                    RegisterDownTime(true);//ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
                                    #endregion

                                    #region اگر موفقیت آمیز بود کسر سهمیه
                                    if (bolIsSucssesRegDownTime)//اگر موفقیت آمیز بود کسر سهمیه
                                    {
                                        //کسر سهمیه پیمانکار به اندازه یک واحد

                                        DecrementRemainRepForThisRegion(strRegion, strYearMonth, 1);
                                        return;//خروج
                                    }
                                    #endregion


                                }
                                #endregion

                                #region اگه موافق نیست خارج شو
                                else if (dialogResult == DialogResult.No)
                                {
                                    return;//خروج
                                }
                                #endregion


                            }
                            #endregion

                            #region نداشتن سهمیه و پیام مناسب و خروج
                            else
                            {
                                MessageBox.Show("سهمیه دان تایم تکراری شما در این ماه تمام شده و امکان ثبت دان تایم تکراری برای شما وجود ندارد");
                                return;
                            }
                            #endregion

                        }
                        #endregion

                        #region دان تایم تکراری میخواد و دفعه قبلی هم کنسل نکرده ببین اگه سهمیه داره دمدش  نرم دو تا ازش کم کن و بگیر
                        else
                        {
                            #region محاسبه  سهمیه

                            //محاسبه سهمیه
                            int intRemaindedRep = RemaindedRep(strRegion, strYearMonth);

                            #region اگه سهمیه ی باقیمانده یک هستش یه ارفاقی بکن 2 کن بتونه یکی بگیره
                            if (intRemaindedRep == 1)//اگه سهمیه ی باقیمانده یک هستش یه ارفاقی بکن 2 کن بتونه یکی بگیره
                            {
                                intRemaindedRep++;
                                //سهمیه رو دو کن
                            }
                            #endregion

                            bool bolHaveRemainRepForDownTime = intRemaindedRep > 1 ? true : false;// سهمیه باقی مانده 

                            #endregion

                            #region داشتن سهمیه و سوال برای ادامه و کسر
                            if (bolHaveRemainRepForDownTime)//سهمیه دارد
                            {

                                #region اگر موفقیت آمیز بود کسر سهمیه
                                DialogResult dialogResult = MessageBox.Show(" همین دان تایم را درخواست کرده و کنسل نکرده اید و در صورت ادامه دو واحد از سهمیه دان تایم تکراری شما کسر خواهد شد آیا ادامه می دهید؟ " + strRowNumberOfLastRepeatedDt + "در ردیف ", "توجه", MessageBoxButtons.YesNo);
                                #endregion

                                #region اگه موافقه که یکی از سهمییش کم بشه بگیر
                                if (dialogResult == DialogResult.Yes)
                                {

                                    #region ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
                                    RegisterDownTime(true);//ثبت دان تایم با ثبت شماره ردیف دان تایم تکراری در دیتابیس 
                                    #endregion

                                    #region اگر موفقیت آمیز بود کسر سهمیه
                                    if (bolIsSucssesRegDownTime)//اگر موفقیت آمیز بود کسر سهمیه
                                    {
                                        //کسر سهمیه پیمانکار به اندازه دو واحد
                                        if (strRegion == "" || strRegion == null)
                                            return;
                                        DecrementRemainRepForThisRegion(strRegion, strYearMonth, 2);
                                        return;//خروج
                                    }
                                    #endregion


                                }
                                #endregion

                                #region اگه موافق نیست خارج شو
                                else if (dialogResult == DialogResult.No)
                                {
                                    return;//خروج
                                }
                                #endregion


                            }
                            #endregion

                            #region نداشتن سهمیه و پیام مناسب و خروج
                            else
                            {
                                MessageBox.Show("سهمیه دان تایم تکراری شما در این ماه تمام شده و امکان ثبت دان تایم تکراری برای شما وجود ندارد");
                                return;
                            }
                            #endregion



                        }
                        #endregion

                    }
                    #endregion

                    #endregion

                }
                #endregion

                #region در ماه جاری تکراری نیست پس یه دان تایم معمولی بگیر
                else//4 تکراری نیست یه دان تایم معمولی بگیر
                {
                    RegisterDownTime(false);//تکراری نیست یه دان تایم معمولی بگیر
                    return;
                }
                #endregion

                #endregion

            }
            #endregion



            #endregion

            #region موضوع دان تایم پی امی نیست 2
            #region  Bیک دان تایم معمولی بگیره بره 
            else //2 موضوع دان تایم پی امی نیست 
            {
                //یک دان تایم معمولی بگیر بره
                RegisterDownTime(false);//فالس یعنی تکراری نیست
            }
            #endregion
            #endregion

            #endregion
        }

        private void ResetForNewDownTime()
        {
            #region Reset For New DownTime
            txtDtToDate.Text = txtDtFromDate.Text = ClsConvertDateHassan.GetMiladiDateReturnFullFarsiDatewithEnglishNumeric(MainForm.DateTimeSqlServer.AddDays(2));

            MainForm.StrDtStartDate = MainForm.strDtEndDate = ClsConvertDateHassan.convertDateToPersion(MainForm.DateTimeSqlServer.AddDays(2));

            lstSerchedPerformer.ResetText();
            txtDtToDate.ForeColor = System.Drawing.Color.Navy;

            #endregion
        }

        private void ResetDtRegDateToToday()
        {
            #region
            txtDtToDate.Text = txtDtFromDate.Text = ClsConvertDateHassan.GetMiladiDateReturnFullFarsiDatewithEnglishNumeric(MainForm.DateTimeSqlServer.AddDays(0));

            MainForm.StrDtStartDate = MainForm.strDtEndDate = ClsConvertDateHassan.convertDateToPersion(MainForm.DateTimeSqlServer.AddDays(0));

            lstSerchedPerformer.ResetText();
            txtDtToDate.ForeColor = System.Drawing.Color.Navy;

            #endregion
        }



        private string getLastRowOfDtTableWithThisParam(string strStartYear, string strStartMonth, string strStartDay, string strCodeSite, string strSubject)
        {
            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMonth + "' AND [subject]=N'" + strSubject + "') order by row desc";
            string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            return strFindedRowOfRepeatedDt;
        }

        private DataTable FoureCellOfRepeatedDowntimeInCurrentMont()
        {
            string strTxtCodeSite = textBoxDtRegCodeSite.Text;
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);
            string strSubject = comboBoxDtSubject.Text;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "'" + " AND status=N'تائید') order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);
            //string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            return dt;
        }

        #region تشخیص اینکه ایا مشابه این دان تایم درخواستی در همین سال و ماه دان تایم مشابه یعنی همین کد و موضوع وجود داره یعنی فقط روز چک نمی شود 
        private bool IsRepeatedDtThisReqInThisMonthOfYear()
        {
            string strTxtCodeSite = textBoxDtRegCodeSite.Text;//از کدسایت درخواستی استخراج میگردد
            string strTxtSubject = comboBoxDtSubject.Text;// از موضوع درخواستی استخراج می گردد
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);//از سال درخواستی استخراج می گردد
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);//از ماه درخواستی استخراج می گردد

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strTxtSubject + "'" + ") order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);

            bool bolIsRepeated = false;
            if (dt.Rows.Count > 0)
            {
                bolIsRepeated = true;
            }
            else
            {
                bolIsRepeated = false;
            }

            return bolIsRepeated;
        }
        #endregion

        #region تشخیص اینکه ایا مشابه این دان تایم درخواستی در همین سال و ماه دان تایم مشابه یعنی همین کد و موضوع وجود داره یعنی فقط روز چک نمی شود 
        private string WhichRowIsPmRequestAndThatHasSameCodeSiteAndDate()
        {
            string strMessageForPmSameSubAndCodeAndDate = string.Empty;

            string strTxtCodeSite = textBoxDtRegCodeSite.Text;//از کدسایت درخواستی استخراج میگردد
            string strTxtSubject = comboBoxDtSubject.Text;// از موضوع درخواستی استخراج می گردد
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);//از سال درخواستی استخراج می گردد
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);//از ماه درخواستی استخراج می گردد
            string strStartDay = MainForm.strDtStartDate.Substring(8, 2);//از ماه درخواستی استخراج می گردد

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,dtNo,StartDate,subject,status FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND SUBSTRING(startDate, 09, 2)=N'" + strStartDay + "' AND [subject]!=N'" + strTxtSubject + "'" + ") order by row desc";

            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);
            string strDtNo = string.Empty;
            try
            {
                strDtNo = dt.Rows[0].ItemArray[1].ToString();
            }
            catch
            {

            }

            if (dt.Rows.Count > 0 && strDtNo != string.Empty)
            {
                strMessageForPmSameSubAndCodeAndDate = dt.Rows[0].ItemArray[1].ToString() + "__" + dt.Rows[0].ItemArray[3].ToString() + "__ احتمال تداخل با";
            }
            else if (dt.Rows.Count > 0 && strDtNo == string.Empty)
            {
                strMessageForPmSameSubAndCodeAndDate = "row : " + dt.Rows[0].ItemArray[0].ToString() + dt.Rows[0].ItemArray[1].ToString() + "__" + dt.Rows[0].ItemArray[3].ToString() + "__Conflict";
            }
            else
            {
                strMessageForPmSameSubAndCodeAndDate = string.Empty;
            }

            return strMessageForPmSameSubAndCodeAndDate;
        }
        #endregion

        private string getLastRowNumberOfRepeatedDt()
        {
            string strTxtCodeSite = textBoxDtRegCodeSite.Text;
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);
            string strSubject = comboBoxDtSubject.Text;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "') order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);
            //string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            string strReturnRow = dt.Rows[0].ItemArray[0].ToString();

            return strReturnRow;
        }

        private string getLastStateOfRepeatedDt()
        {
            string strTxtCodeSite = textBoxDtRegCodeSite.Text;
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);
            string strSubject = comboBoxDtSubject.Text;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "') order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);
            //string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            string strReturnRow = dt.Rows[0].ItemArray[2].ToString();

            return strReturnRow;
        }

        private string getNotetStateOfRepeatedDt()
        {
            string strTxtCodeSite = textBoxDtRegCodeSite.Text;
            string strStartMont = MainForm.strDtStartDate.Substring(5, 2);
            string strStartYear = MainForm.strDtStartDate.Substring(0, 4);
            string strSubject = comboBoxDtSubject.Text;

            string strSqlForFindSourceOfRepeatedRow = @"Select TOP(1) row,region,status,note FROM [DT].[dbo].[tblDownTimeShort] Where (siteCode=N'ES" + strTxtCodeSite + "' AND SUBSTRING(startDate, 1, 4)=N'" + strStartYear + "' AND SUBSTRING(startDate, 6, 2)=N'" + strStartMont + "' AND [subject]=N'" + strSubject + "'" + " AND status=N'تائید') order by row desc";
            DataTable dt = objClsSql.FillDataTableFromQuery(strSqlForFindSourceOfRepeatedRow);
            //string strFindedRowOfRepeatedDt = objClsSql.SendCommandGetStringResult(strSqlForFindSourceOfRepeatedRow);

            string strReturnNote = dt.Rows[0].ItemArray[3].ToString();

            return strReturnNote;
        }

        private void FillListBox()
        {
           

            #region محاسبه کل زمان درخواستی برای دان تایم جهت درست کردن لیست متناسب
            DateTime dteStDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrDtStartDate);
            DateTime dteEndDate = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(MainForm.StrDtEndDate);

            DateTime dteMiladtStartDt = dteStDate.AddHours(double.Parse(nudDtStHour.Value.ToString())).AddMinutes(double.Parse(nudDtStMin.Value.ToString()));
            DateTime dteMiladtEndDt = dteEndDate.AddHours(double.Parse(nudDtEndHour.Value.ToString())).AddMinutes(double.Parse(nudDtEndMin.Value.ToString()));

            dblDtTotalMinuteTime = (dteMiladtEndDt - dteMiladtStartDt).TotalMinutes;
            if (dblDtTotalMinuteTime < 0)
            {
                MessageBox.Show("زمان شروع از ‍پایان کوچکتر است لطفا اصلاح فر مایید");
                nudDtStHour.Select();
            }
            #endregion

            #region اگه قبلا لیست درست شده و تغییری نداشتیم بیخال شو
            if (dblDtTotalMinuteTimeLast == dblDtTotalMinuteTime)
            {
                return;
            }
            #endregion

            #region ساخت دیتاتیبل برای لیست

            DataTable dtMin = new DataTable();
            dtMin.Columns.Add("Minute", typeof(int));

            #endregion

            #region ساخت لیست ساعتی اوتیج 7 روزه
            var Interval = new List<int>();
            Interval.Add(0);
            Interval.Add(15);
            Interval.Add(30);
            Interval.Add(45);
            Interval.Add(60);
            Interval.Add(75);
            Interval.Add(90);
            Interval.Add(105);
            Interval.Add(120);
            Interval.Add(150);
            Interval.Add(180);
            Interval.Add(210);
            Interval.Add(240);
            Interval.Add(270);
            Interval.Add(300);
            Interval.Add(330);
            Interval.Add(360);
            Interval.Add(390);
            Interval.Add(420);
            Interval.Add(450);
            Interval.Add(480);
            Interval.Add(510);
            Interval.Add(540);
            Interval.Add(570);
            Interval.Add(600);
            Interval.Add(630);
            Interval.Add(660);
            Interval.Add(690);
            Interval.Add(720);
            Interval.Add(780);
            Interval.Add(840);
            Interval.Add(900);
            Interval.Add(960);
            Interval.Add(1020);
            Interval.Add(1080);
            Interval.Add(1140);
            Interval.Add(1200);
            Interval.Add(1260);
            Interval.Add(1320);
            Interval.Add(1380);
            Interval.Add(1440);
            Interval.Add(1500);
            Interval.Add(1560);
            Interval.Add(1620);
            Interval.Add(1680);
            Interval.Add(1740);
            Interval.Add(1800);
            Interval.Add(1860);
            Interval.Add(1920);
            Interval.Add(1980);
            Interval.Add(2040);
            Interval.Add(2100);
            Interval.Add(2160);
            Interval.Add(2220);
            Interval.Add(2280);
            Interval.Add(2340);
            Interval.Add(2400);
            Interval.Add(2460);
            Interval.Add(2520);
            Interval.Add(2580);
            Interval.Add(2640);
            Interval.Add(2700);
            Interval.Add(2760);
            Interval.Add(2820);
            Interval.Add(2880);
            Interval.Add(2940);
            Interval.Add(3000);
            Interval.Add(3060);
            Interval.Add(3120);
            Interval.Add(3180);
            Interval.Add(3240);
            Interval.Add(3300);
            Interval.Add(3360);
            Interval.Add(3420);
            Interval.Add(3480);
            Interval.Add(3540);
            Interval.Add(3600);
            Interval.Add(3660);
            Interval.Add(3720);
            Interval.Add(3780);
            Interval.Add(3840);
            Interval.Add(3900);
            Interval.Add(3960);
            Interval.Add(4020);
            Interval.Add(4080);
            Interval.Add(4140);
            Interval.Add(4200);
            Interval.Add(4260);
            Interval.Add(4320);
            Interval.Add(4380);
            Interval.Add(4440);
            Interval.Add(4500);
            Interval.Add(4560);
            Interval.Add(4620);
            Interval.Add(4680);
            Interval.Add(4740);
            Interval.Add(4800);
            Interval.Add(4860);
            Interval.Add(4920);
            Interval.Add(4980);
            Interval.Add(5040);
            Interval.Add(5100);
            Interval.Add(5160);
            Interval.Add(5220);
            Interval.Add(5280);
            Interval.Add(5340);
            Interval.Add(5400);
            Interval.Add(5460);
            Interval.Add(5520);
            Interval.Add(5580);
            Interval.Add(5640);
            Interval.Add(5700);
            Interval.Add(5760);
            Interval.Add(5820);
            Interval.Add(5880);
            Interval.Add(5940);
            Interval.Add(6000);
            Interval.Add(6060);
            Interval.Add(6120);
            Interval.Add(6180);
            Interval.Add(6240);
            Interval.Add(6300);
            Interval.Add(6360);
            Interval.Add(6420);
            Interval.Add(6480);
            Interval.Add(6540);
            Interval.Add(6600);
            Interval.Add(6660);
            Interval.Add(6720);
            Interval.Add(6780);
            Interval.Add(6840);
            Interval.Add(6900);
            Interval.Add(6960);
            Interval.Add(7020);
            Interval.Add(7080);
            Interval.Add(7140);
            Interval.Add(7200);
            Interval.Add(7260);
            Interval.Add(7320);
            Interval.Add(7380);
            Interval.Add(7440);
            Interval.Add(7500);
            Interval.Add(7560);
            Interval.Add(7620);
            Interval.Add(7680);
            Interval.Add(7740);
            Interval.Add(7800);
            Interval.Add(7860);
            Interval.Add(7920);
            Interval.Add(7980);
            Interval.Add(8040);
            Interval.Add(8100);
            Interval.Add(8160);
            Interval.Add(8220);
            Interval.Add(8280);
            Interval.Add(8340);
            Interval.Add(8400);
            Interval.Add(8460);
            Interval.Add(8520);
            Interval.Add(8580);
            Interval.Add(8640);
            Interval.Add(8700);
            Interval.Add(8760);
            Interval.Add(8820);
            Interval.Add(8880);
            Interval.Add(8940);
            Interval.Add(9000);
            Interval.Add(9060);
            Interval.Add(9120);
            Interval.Add(9180);
            Interval.Add(9240);
            Interval.Add(9300);
            Interval.Add(9360);
            Interval.Add(9420);
            Interval.Add(9480);
            Interval.Add(9540);
            Interval.Add(9600);
            Interval.Add(9660);
            Interval.Add(9720);
            Interval.Add(9780);
            Interval.Add(9840);
            Interval.Add(9900);
            Interval.Add(9960);
            Interval.Add(10020);
            Interval.Add(10080);
            #endregion

            #region پرکردن دیتاتیبل از لیست
            foreach (int item in Interval)
            {
                if (item < dblDtTotalMinuteTime)
                    dtMin.Rows.Add(item);
            }
            #endregion

            #region پر کردن لیست با دیتاتیبل
            List<string> lst = new List<string>();
            foreach (DataRow r in dtMin.Rows)
            {
                lst.Add(r["Minute"].ToString());
            }
            listBoxDtOutageMin.DataSource = null;
            listBoxDtOutageMin.Items.Clear();
            listBoxDtOutageMin.DataSource = lst;
            #endregion

            #region مساوی نمودن توتال مینیت قبل و جدید جهت جلوگیری از رفرش لیست مرتب براساس موس اور
            dblDtTotalMinuteTimeLast = dblDtTotalMinuteTime;
            #endregion
        }

        #region توابع سهمیه پیمانکار

        #region تابع بررسی سهمیه دان تایم تکراری در این ماه 
        private int RemaindedRep(string strRegion, string yearMonth)
        {
            //string strTblName = @"[DT].[dbo].[tblRepRemaindedRegion" + strRegion.Trim() + "]";
            if(strRegion=="" || strRegion==null)
            {
                return 100;//موقت
            }
            string strSql = "Select " + strRegion + " From [DT].[dbo].tblRepRemaindedRegions Where [yearMonth]=N'" + yearMonth + "'";
            string strCntOfRemaindedRep = objClsSql.SendCommandGetStringResult(strSql);

            
            return int.Parse(strCntOfRemaindedRep);
        }



        #endregion

        #region تابع بررسی اینکه آیا این منطقه سهمیه دان تایم تکراری در این ماه را دارد؟
        private bool IsAnyRemaindedRep(string strRegion, string yearMonth)
        {
            bool bolIsExist = false;

            if (RemaindedRep(strRegion, yearMonth) > 0)
            {
                bolIsExist = true;
            }
            else
            {
                bolIsExist = false;
            }


            return bolIsExist;
        }
        #endregion

        #region تابع کسر یک یا دو  واحد از سهمیه پیمانکار 

        private void DecrementRemainRepForThisRegion(string strRegion, string yearMonth, int intCntOfdecrement)
        {

            int intExistedRep = int.Parse(objClsSql.SendCommandGetStringResult("Select " + strRegion + " From [DT].[dbo]. tblRepRemaindedRegions Where [yearMonth]=N'" + yearMonth + "'"));

            int intRemaindedRep = intExistedRep - intCntOfdecrement;
            objClsSql.EditRecord("[DT].[dbo]. tblRepRemaindedRegions", strRegion, intRemaindedRep, "Where yearMonth=N'" + yearMonth + "'");


        }

        #endregion

        #endregion

        #endregion

        #region ListBox
        private void lstSerchedPerformer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int intSelPerformerList = lstSerchedPerformer.SelectedIndex;
                txtSerchedPerformer.Text = strTempForPerformer = lstSerchedPerformer.GetItemText(lstSerchedPerformer.SelectedItem);

                EvaluteDownTimeAndRegDt();
                refreshRegisterGrid();
            }
        }

        private void lstSerchedPerformer_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            int intSelPerformerList = lstSerchedPerformer.SelectedIndex;
            txtSerchedPerformer.Text = strTempForPerformer = lstSerchedPerformer.GetItemText(lstSerchedPerformer.SelectedItem);

            if (txtSerchedPerformer.Text == strTempForPerformer)
            {
                EvaluteDownTimeAndRegDt();
                if (bolIsSucssesRegDownTime)
                {
                    refreshRegisterGrid();
                }

            }

        }

        private void listBoxDtOutageMin_MouseHover(object sender, EventArgs e)
        {
            FillListBox();
            FunctionForLeaveSubject();
            strDtSetOutage = listBoxDtOutageMin.SelectedItem.ToString();
        }

        private void listBoxDtOutageMin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                strDtSetOutage = listBoxDtOutageMin.SelectedItem.ToString();

            }
        }

        private void listBoxDtOutageMin_DoubleClick(object sender, EventArgs e)
        {
            strDtSetOutage = listBoxDtOutageMin.SelectedItem.ToString();
        }

        private void listBoxDtOutageMin_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            strDtSetOutage = listBoxDtOutageMin.SelectedItem.ToString();

            // EvaluteDownTime();
            if (bolIsSucssesRegDownTime)
            {
                refreshRegisterGrid();
            }
        }

        private void listBoxDtOutageMin_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBoxDtOutageMin.SelectedItems.Count > 0)
            {
                strDtSetOutage = listBoxDtOutageMin.SelectedItem.ToString();

            }

        }


        #endregion

        #region gbx
        private void gbxDtOutage_MouseHover(object sender, EventArgs e)
        {
            //FillListBox();
            //FunctionForLeaveSubject();
        }
        private void gbxDtSelectDateTime_MouseHover(object sender, EventArgs e)
        {
            if (MainForm.StrCurRol == "administrator" ||comboBoxDtSubject.Text == string.Empty)
            {
                nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = true;
                return;
            }
            //else
            //{
            //    nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = false;
            //}

            if (comboBoxDtSubject.Text == string.Empty || bolIsReqPm || bolIsReqVisit)
            {
                //زمانها غیر فعال
                nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = false;
            }
        }

        #endregion

        #region lst
        private void listBoxDtOutageMin_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBoxDtOutageMin.SelectedItems.Count > 0)
            {
                txtSerchedOutage.Text = listBoxDtOutageMin.SelectedItem.ToString() + "  دقیقه ";
            }

        }

        private void lstSerchedPerformer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intSelPerformerList = lstSerchedPerformer.SelectedIndex;
            string strNameOfPerformer = lstSerchedPerformer.GetItemText(lstSerchedPerformer.SelectedItem);

            int intTelId = lstTelId.SelectedIndex;
            string strTelId = lstTelId.GetItemText(lstTelId.SelectedItem);

            txtRegDtWorkPlace.Text = objClsSql.FindRecord("TEL.dbo.tblTel", "WorkPlace", " WHERE Name=N'" + strNameOfPerformer + "'  AND telId=N'" + strTelId + "'");

        }
        #endregion

        #region pictureBox
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            txtSerchedPerformer.Clear();
        }

        private void pictureBoxDtPerformerSearch_Click(object sender, EventArgs e)
        {
            txtSerchedPerformer.Select();
            txtSerchedPerformer.Focus();
        }

        #endregion

        private void txtSerchedOutage_TextChanged(object sender, EventArgs e)
        {
            if (txtSerchedOutage.Text == "0  دقیقه ")
            {
                txtSerchedOutage.ForeColor = Color.DarkRed;
            }
            else if (txtSerchedOutage.Text == "60  دقیقه ")
            {
                txtSerchedOutage.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtSerchedOutage.ForeColor = Color.DarkBlue;
            }
        }

        private void comboBoxDtSubject_Leave(object sender, EventArgs e)
        {
            enableDisableTiming();
        }

        private void comboBoxDtSubject_MouseLeave(object sender, EventArgs e)
        {
            enableDisableTiming();
        }

        private void enableDisableTiming()
        {
            if (bolIsReqPm ||bolIsReqVisit|| comboBoxDtSubject.Text == string.Empty)
            {
                gbxDtOutage.Enabled = false;
                //gbxBusyOver.Enabled = false;
            }
            else
            {
                gbxDtOutage.Enabled = true;
                //gbxBusyOver.Enabled = true;
            }
        }

        private void comboBoxDtSubject_MouseHover(object sender, EventArgs e)
        {
            enableDisableTiming();
        }

        private void rgpDtTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            
           if (rgpDtTime.SelectedIndex==0 )
            {
                FillNudVisitForSobh();
                gbx_bazdidSobh.Enabled = true;
                gbx_bazdidAsr.Enabled = false;
            }
            else 
            {
                FillNudVisitForAsr();
                gbx_bazdidSobh.Enabled = false;
                gbx_bazdidAsr.Enabled = true;
            }

           FilldateTimeNumericUpDownForPm();

        }

        private void rdbBazdidSobh_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillNudVisitForSobh();
        }

        private void FillNudVisitForSobh()
        {
            if (rdbVisitSobh.SelectedIndex == 0)
            {
                nudDtStHour.Value = 07;
                nudDtStMin.Value = 00;
                nudDtEndHour.Value = 11;
                nudDtEndMin.Value = 00;
            }

            if (rdbVisitSobh.SelectedIndex == 1)
            {
                nudDtStHour.Value = 08;
                nudDtStMin.Value = 00;
                nudDtEndHour.Value = 12;
                nudDtEndMin.Value = 00;
            }

            if (rdbVisitSobh.SelectedIndex == 2)
            {
                nudDtStHour.Value = 09;
                nudDtStMin.Value = 00;
                nudDtEndHour.Value = 13;
                nudDtEndMin.Value = 00;
            }
        }

        private void rdbVisitAsr_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillNudVisitForAsr();

        }

        private void FillNudVisitForAsr()
        {
            if (rdbVisitAsr.SelectedIndex == 0)
            {
                nudDtStHour.Value = 13;
                nudDtStMin.Value = 00;
                nudDtEndHour.Value = 17;
                nudDtEndMin.Value = 00;
            }

            if (rdbVisitAsr.SelectedIndex == 1)
            {
                nudDtStHour.Value = 14;
                nudDtStMin.Value = 00;
                nudDtEndHour.Value = 18;
                nudDtEndMin.Value = 00;
            }

            if (rdbVisitAsr.SelectedIndex == 2)
            {
                nudDtStHour.Value = 15;
                nudDtStMin.Value = 00;
                nudDtEndHour.Value = 19;
                nudDtEndMin.Value = 00;
            }
        }

        private void radioGroupDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroupDomain.SelectedIndex == 0)
            {
                strDomain = "Site";
            }
            else if(radioGroupDomain.SelectedIndex==1)
            {
                strDomain = "LRAN";
            }
            else if (radioGroupDomain.SelectedIndex == 2)
            {
                strDomain = "HRAN";
            }
            else if (radioGroupDomain.SelectedIndex == 3)
            {
                strDomain = "IPBB";
            }
        }

        private void radioGroupDomain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)    
            {
                EvaluteDownTimeAndRegDt();

                refreshRegisterGrid();
            }
        }

        private void radioGroupDomain_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void gbxDtSelectDateTime_Enter(object sender, EventArgs e)
        {
            if (!bolIsReqPm && !bolIsReqVisit)
            {
                nudDtStHour.Enabled = nudDtStMin.Enabled = nudDtEndHour.Enabled = nudDtEndMin.Enabled = true;

            }
        }
    }
}
