using DevExpress.CodeParser;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PE.Forms;
using PE.Forms.DB;
using PE.Forms.DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using DevExpress.Spreadsheet;
using DataTable = System.Data.DataTable;
using Action = System.Action;
using Workbook = DevExpress.Spreadsheet.Workbook;

namespace PE
{
    public partial class MainForm : Form
    {
        #region VARS

        ClsSql objClsSql = new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال

        #region Public
        //sstring exeFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        //static public string strPath = StrSelectedRootPathInPeServer;
        //static public string strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static public string strSelectedTab = "";
        static public bool bolIsClickCTL_I = false;
        static public string strMsg = "";

        #region strSelectedRootPath
        private static string strSelectedRootPathInPeServer = string.Empty;
        #endregion

        #region media_player
        private const short cHNL = 1;
        private const int sMPL_RATE = 8000;
        private const int bIT_PER_SMPL = 8;
        private const short fILTER_FREQ_LOW = -10000;
        private const short fILTER_FREQ_HIGH = 10000;
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        #endregion

        #region connect_Db
        private static string strConnectedNetWork = string.Empty;
        private static string strAccessNetIp = "در جستجوی شبکه فعال";
        #endregion

        #region connect_MyDb

        private bool bolIsConnectedMyDb;
        #endregion



        #region strCur
        public static string strCurMac = string.Empty;
        public static string strCurIp = string.Empty;
        public static string strCumputerName = string.Empty;
        private static string strCurUsr = string.Empty;
        private static string strFullNameCurUsr = string.Empty;
        private static string strCurRol = string.Empty;

        private static string strCurVer = string.Empty;
        public static string strCurCompany = string.Empty;
        //public static string strCurNet = string.Empty;
        #endregion

        public static bool bolHavePermissionForLogin=false;

        public static Color ClrMessageForUsr = Color.Red;

        ToolTip tip = new ToolTip();
        private static DateTime dateTimeSqlServer;

        #endregion

        #region DT
        public static int intTajmieePms = 1;


        #region LIST
        List<string> lstSelectedCodeSitesForClipBoard;
        List<string> lstSelectedSubjectesForClipBoard;
        #endregion

        #region DataTables
        private static DataTable dtDtTemp;
        private static DataTable dtDbArasTemp;
        private static DataTable dtSelectedDt = new DataTable();
        private static DataTable dtSelectedDtForCreSim = new DataTable();
        private static DataTable dtDtForCreSim = new DataTable();
        private static DataTable dtDbSerch = new DataTable();
        private DataTable dtAdRecAlmFromCont = new DataTable();
        private DataTable dtSerchTel = new DataTable();
        private static DataTable dtMiniTempDb = null;

        #endregion

        #region strings

        public static string strDtNoteTadakholBa;

        public static string strDtNotePayan;
        public static string strDtNoteShoroo;

        public static string strDtPershianNotePayan;
        public static string strDtPershianNoteShoroo;

        public static string strDtRepeatedRow;
        public static string strDtChgGrpSelRows;
        public static string strDtState = string.Empty;
        public static string strDTCurDtNo = string.Empty;
        public static string strCurApplicant = string.Empty;
        public static string strCurApprover = string.Empty;
        public static string strCurDtNo = string.Empty;
        public static string strDtEndDate = string.Empty;
        public static string strCurEndTime = string.Empty;
        public static string strCurPerformer = string.Empty;
        public static string strCurRowInDtGrid = string.Empty;
        public static string strCurSiteName = string.Empty;
        public static string strDtStartDate = string.Empty;
        public static string strDtReqStartTime = string.Empty;
        public static string strCurSubject = string.Empty;
        public static string strPreviousLastDtUpdateTimeInMyPc = string.Empty;
        public string strDtTempForSubjectCombo = string.Empty;
        public string strDtTempForPerformerCombo = string.Empty;
        public string strDtLastRowOfDownTime = string.Empty;
        public static string strDtTextForNewDownTime = string.Empty;
        public static string strDtAboveGridWhenNotFreez = string.Empty;
        public static string strDtMaxPermitTimeForReg = string.Empty;

        #endregion

        #region Ints

        private static int intDtLastSelectedRow;
        public static int intDtTopRowInGrid;
        public static int intDtFocusedRow;

        private static int intDtPahseOfDownTime;
        private static int intDtCountTmrForNewDt = 120;
        private int intDtCountTmrForOldDt = 7200;

        private static string strDtLastCHECKSUM_AGG;

        public static int intDtRepeatedRow = 0;


        #endregion

        #region Bools
        public static bool bolDtIsRepeatedDt = false;

        public static bool bolDtForceToRefreshAndGoToEnd = true;
        public static bool bolDtForceToRefreshAndDontGoToEnd = false;

        public static bool bolDtIsCreateDownTime = false;
        public static bool bolDtIsNeddedToRefresh = false;

        public static bool bolDtUpdateRowWithForm = false;

        public static bool bolDtHaveRunTimeFilter = false;
        public static bool bolDtHaveWaitForClarityFilter = false;
        public static bool bolDtHaveWaitForSessionFilter = false;
        public static bool bolDtHaveChangeOnDtAnnounce = true;
        public static bool bolDtRegInClarity = false;
        public static bool bolDtRegByUser = false;
        public static bool bolDtPakage = false;
        public static bool bolDtInProcessingRequest = false;
        public static bool bolDtRepeated = false;
        public static bool bolDtShowCurrentMonth = true;
        public static bool bolDtHaveNewDateForFreez = false;
        public static bool BolIsNewMessageForAboveDtGrid = false;

        public static bool BolIsEmptyCrNo = true;
        public static bool BolIsCurUserPowerOrAdmin = false;
        public static bool BolIsDblClickedColCrNo = false;
        public static bool BolHavePermitRefresh = true;
        public static bool BolHavePermitToRegDt = true;
        public static bool bolDtNeedToCheck = false;
        public static bool bolDtFinalReviw = false;

        #endregion

        #region data_Rows
        private static DataRow[] drRowNumberAttachFile = null;
        private static DataRow[] drRowNumberRepetedDt = null;

        #endregion




        public static Color ClrMessageForTextAboveDtGrid = Color.GreenYellow;
        #endregion

        #region DB
        public static DataTable dtDb;
        public static DataTable dtNewDb;
        public static string StrdateOfLastDbUpdate = null;
        public static string StrdateOfDbUpdate = null;
        // تنظیمات اتصال به دیتابیس طبق درخواست شما
        private string connectionString = "Data Source=172.16.3.116;Initial Catalog=DB;User ID=asg;Password=Asg_8560@200";


        private static string strDbXclFileNameWithPathForUpdateDb = string.Empty;
        private static bool bolDbIsEqualExcelFileForUpdate;
        private static string strDbLat = string.Empty;
        private static string strDbLong = string.Empty;
        private static string strDbDateLastUpdatePershian = "نامشخص";
        private static string strDbNewLastSelectedLocation = string.Empty;
        private static string strDbNewCurSelectedLocation = string.Empty;
        private static string strDbPreviousLastDbArasUpdateTime = string.Empty;
        private static bool bolDbSelectedSum = true;
        public static bool bolIsCompleteUpdateDb = false;

        public static DataTable dtDbSummary = null;
        public static DataTable dtDbTotal = null;
        public static DataTable dtShenasehBargh = null;

        public static string strDateOfLastDbUpInStart = null;




        #endregion

        #region PB
        private static string strLastNameTelBook = string.Empty;
        private static string strLastTelTelBook = string.Empty;

        #endregion

        #region PE

        #region VPN
        private static bool bolIsConnectedVpn = false;
        int intCounterForCheckVpn = 1200;
        #endregion
        #endregion

        #region 3M

        public static string strSeEmStartSeEmReportDate = string.Empty;
        public static string strSeEmEndSeEmReportDate = string.Empty;
        DataTable dataTableDownTimeFilteredBySelected3M = new DataTable();
        DataTable dataTable3M;
        DataTable dataTableFiltered3MForSearchDt;
        bool bolIsAll3MReport;
        bool bolIsAll3MPmReport;
        string strViewReportTitle = string.Empty;
        enum reportType
        {
            all,
            allPm,
            interval,
            intervalPm
        }

        reportType typeOf3MRep;


        public static string strSqlForReprotAfterRegHaveDtInMySql = string.Empty;



        #endregion

        #region Selected DateTime

        #region چه تبی تنظیم تاریخ کرده

        public static string strWhitchTabSetDatTime = string.Empty;

        #endregion

        #region شمسی
        private static string strSelectedPershianDateWithDotSeperator = string.Empty;//شمسی اسلش دار
        private static string strSelectedPershianDateWithSlashSeperator = string.Empty;//شمسی نقطه دار

        private static string strSelectedPershianDateNameOfWeek = string.Empty;//نام روز هفته به شمسی
        private static string strSelectedPershianDateWithNameOfDay = string.Empty;//شمسی و روز هفته
        #endregion

        #region میلادی
        private static string strSelectedMiladiDateWithDotSeperator = string.Empty;//میلادی اسلش دار
        private static string strSelectedMiladiDateWithSlashSeperator = string.Empty;//میلادی نقطه دار

        private static string strSelectedMiladiDateNameOfWeek = string.Empty;//نام روز هفته به میلادی
        private static string strSelectedMiladiDateWithNameOfDay = string.Empty;//میلادی  و روز هفته

        #endregion

        #region DT
        public static DateTime dteDtReg;//تاریخ وزمان ثبت دان تایم میلادی
        public static DateTime dteDtStart;//تاریخ وزمان شروع دان تایم میلادی
        public static DateTime dteDtEnd;//تاریخ وزمان پایان دان تایم میلادی

        #endregion

        #endregion

        #region SA
        public static string strConnectedNetWorkSa = string.Empty;

        public static int intLastCountOfRecordInTblSaLog = 0;

        public static DataTable dtLastConnections = new DataTable();
        #endregion

        #region PM
        public string strDefaultYearMonth = "";
        #endregion

        #endregion

        #region چوم
        public static string StrSelectedRootPathInPeServer { get => strSelectedRootPathInPeServer; set => strSelectedRootPathInPeServer = value; }
        public static short CHNL => cHNL;
        public static int SMPL_RATE => sMPL_RATE;
        public static int BIT_PER_SMPL => bIT_PER_SMPL;
        public static short FILTER_FREQ_LOW => fILTER_FREQ_LOW;
        public static short FILTER_FREQ_HIGH => fILTER_FREQ_HIGH;
        public SoundPlayer Player { get => player; set => player = value; }
        public static string StrConnectedNetWork { get => strConnectedNetWork; set => strConnectedNetWork = value; }
        public static string StrAccessNetIp { get => strAccessNetIp; set => strAccessNetIp = value; }
        public bool BolIsConnectedMyDb { get => bolIsConnectedMyDb; set => bolIsConnectedMyDb = value; }

        public static string StrCurMac { get => strCurMac; set => strCurMac = value; }
        public static string StrCurIp { get => strCurIp; set => strCurIp = value; }
        public static string StrCumputerName { get => strCumputerName; set => strCumputerName = value; }
        public static string StrCurUsr { get => strCurUsr; set => strCurUsr = value; }
        public static string StrFullNameCurUsr { get => strFullNameCurUsr; set => strFullNameCurUsr = value; }
        public static string StrCurRol { get => strCurRol; set => strCurRol = value; }
        // public static CColor ClrMessageForUsr { get => clrMessageForUsr; set => clrMessageForUsr = value; }
        public ToolTip Tip { get => tip; set => tip = value; }
        public static DateTime DateTimeSqlServer { get => dateTimeSqlServer; set => dateTimeSqlServer = value; }
        public static DataTable DtDtTemp { get => dtDtTemp; set => dtDtTemp = value; }
        public static DataTable DtDbArasTemp { get => dtDbArasTemp; set => dtDbArasTemp = value; }
        public static DataTable DtSelectedDt { get => dtSelectedDt; set => dtSelectedDt = value; }
        public static DataTable DtSelectedDtForCreSim { get => dtSelectedDtForCreSim; set => dtSelectedDtForCreSim = value; }
        public static DataTable DtDtForCreSim { get => dtDtForCreSim; set => dtDtForCreSim = value; }
        public static DataTable DtDbSerch { get => dtDbSerch; set => dtDbSerch = value; }
        public DataTable DtAdRecAlmFromCont { get => dtAdRecAlmFromCont; set => dtAdRecAlmFromCont = value; }
        public DataTable DtSerchTel { get => dtSerchTel; set => dtSerchTel = value; }
        public static DataTable DtMiniTempDb { get => dtMiniTempDb; set => dtMiniTempDb = value; }
        public static string StrDtChgGrpSelRows { get => strDtChgGrpSelRows; set => strDtChgGrpSelRows = value; }
        public static string StrDtState { get => strDtState; set => strDtState = value; }
        public static string StrDTCurDtNo { get => strDTCurDtNo; set => strDTCurDtNo = value; }
        public static string StrCurApplicant { get => strCurApplicant; set => strCurApplicant = value; }
        public static string StrCurApprover { get => strCurApprover; set => strCurApprover = value; }
        public static string StrCurDtNo { get => strCurDtNo; set => strCurDtNo = value; }
        public static string StrDtEndDate { get => strDtEndDate; set => strDtEndDate = value; }
        public static string StrCurEndTime { get => strCurEndTime; set => strCurEndTime = value; }
        public static string StrCurPerformer { get => strCurPerformer; set => strCurPerformer = value; }
        public static string StrCurRowInDtGrid { get => strCurRowInDtGrid; set => strCurRowInDtGrid = value; }
        public static string StrCurSiteName { get => strCurSiteName; set => strCurSiteName = value; }
        public static string StrDtStartDate { get => strDtStartDate; set => strDtStartDate = value; }
        public static string StrCurStartTime { get => strDtReqStartTime; set => strDtReqStartTime = value; }
        public static string StrCurSubject { get => strCurSubject; set => strCurSubject = value; }
        public static string StrPreviousLastDtUpdateTimeInMyPc { get => strPreviousLastDtUpdateTimeInMyPc; set => strPreviousLastDtUpdateTimeInMyPc = value; }
        public string StrDtTempForSubjectCombo { get => strDtTempForSubjectCombo; set => strDtTempForSubjectCombo = value; }
        public string StrDtTempForPerformerCombo { get => strDtTempForPerformerCombo; set => strDtTempForPerformerCombo = value; }
        public static int IntDtPhaseOfDownTime { get => intDtPahseOfDownTime; set => intDtPahseOfDownTime = value; }
        public static int IntDtCountTmrForNewDt { get => intDtCountTmrForNewDt; set => intDtCountTmrForNewDt = value; }
        public int IntDtCountTmrForOldDt { get => intDtCountTmrForOldDt; set => intDtCountTmrForOldDt = value; }

        //  public static bool BolDtForceToRefresh { get => bolDtForceToRefresh; set => bolDtForceToRefresh = value; }
        public static bool BolDtIsCreateDownTime { get => bolDtIsCreateDownTime; set => bolDtIsCreateDownTime = value; }
        public static DataRow[] DrRowNumberAttachFile { get => drRowNumberAttachFile; set => drRowNumberAttachFile = value; }
        public static string StrDbXclFileNameWithPathForUpdateDb { get => strDbXclFileNameWithPathForUpdateDb; set => strDbXclFileNameWithPathForUpdateDb = value; }
        public static bool BolDbIsEqualExcelFileForUpdate { get => bolDbIsEqualExcelFileForUpdate; set => bolDbIsEqualExcelFileForUpdate = value; }
        public static string StrDbLat { get => strDbLat; set => strDbLat = value; }
        public static string StrDbLong { get => strDbLong; set => strDbLong = value; }
        //  public static string StrDbDateLastUpdatePershianSummary { get => strDbDateLastUpdatePershian; set => strDbDateLastUpdatePershian = value; }
        public static string StrDbDateLastUpdatePersian = "نامشخص";
        public static string StrDbNewLastSelectedLocation { get => strDbNewLastSelectedLocation; set => strDbNewLastSelectedLocation = value; }
        public static string StrDbNewCurSelectedLocation { get => strDbNewCurSelectedLocation; set => strDbNewCurSelectedLocation = value; }
        public static string StrDbPreviousLastDbSummaryArasUpdateTime { get => strDbPreviousLastDbArasUpdateTime; set => strDbPreviousLastDbArasUpdateTime = value; }
        public static string StrDbPreviousLastDbArasUpdateTime { get => strDbPreviousLastDbArasUpdateTime; set => strDbPreviousLastDbArasUpdateTime = value; }
        public static string StrLastNameTelBook { get => strLastNameTelBook; set => strLastNameTelBook = value; }
        public static string StrLastTelTelBook { get => strLastTelTelBook; set => strLastTelTelBook = value; }

        public static bool BolIsConnectedVpn { get => bolIsConnectedVpn; set => bolIsConnectedVpn = value; }
        public int IntCounterForCheckVpn { get => intCounterForCheckVpn; set => intCounterForCheckVpn = value; }

        public static string StrSelectedPersianDateWithDotSeperator { get => strSelectedPershianDateWithDotSeperator; set => strSelectedPershianDateWithDotSeperator = value; }
        public static string StrSelectedPershianDateWithSlashSeperator { get => strSelectedPershianDateWithSlashSeperator; set => strSelectedPershianDateWithSlashSeperator = value; }
        public static string StrSelectedPershianDateNameOfWeek { get => strSelectedPershianDateNameOfWeek; set => strSelectedPershianDateNameOfWeek = value; }
        public static string StrSelectedPershianDateWithNameOfDay { get => strSelectedPershianDateWithNameOfDay; set => strSelectedPershianDateWithNameOfDay = value; }
        public static string StrSelectedMiladiDateWithDotSeperator { get => strSelectedMiladiDateWithDotSeperator; set => strSelectedMiladiDateWithDotSeperator = value; }
        public static string StrSelectedMiladiDateWithSlashSeperator { get => strSelectedMiladiDateWithSlashSeperator; set => strSelectedMiladiDateWithSlashSeperator = value; }
        public static string StrSelectedMiladiDateNameOfWeek { get => strSelectedMiladiDateNameOfWeek; set => strSelectedMiladiDateNameOfWeek = value; }
        public static string StrSelectedMiladiDateWithNameOfDay { get => strSelectedMiladiDateWithNameOfDay; set => strSelectedMiladiDateWithNameOfDay = value; }
        #endregion

        #region frmMain

        public MainForm()
        {
            InitializeComponent();

            
            using (var stream = File.OpenRead(MainForm.StrSelectedRootPathInPeServer + @"\PE.ico"))
            {
                this.Icon = new System.Drawing.Icon(stream);
            }
            //موقت
           // MessageBox.Show(StrSelectedRootPathInPeServer);

            #region init dtSqlDependency
            _connectionDependencyDt = new SqlConnection(ConnectionStringDependencyDt);
            _commandDependencyDt = new SqlCommand(SelectQueryDependencyDt, _connectionDependencyDt);
            #endregion
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckTab();
        }

        private void CheckTab()
        {
            objClsSql = new ClsSql();
            #region بروز رسانی نوار وضعیت 

            try
            {
                lblStatusBar.Text = objClsSql.SetDateForStatusBar();
            }
            catch
            {
                MessageBox.Show("خطا", "در هنگام چک تب مشکلی در تطابق فرمت تاریخ وجود دارد لطفا به مدیر سیستم گزارش دهید", MessageBoxButtons.OK,

                   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                  MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            }


            #endregion

            strSelectedTab = this.tabControlMain.SelectedTab.Text;
            switch (strSelectedTab)
            {

                case "DT":

                    ClsDiff.writeLogOfAction("ورود به تب دان تایم");
                    this.DtTabActive();

                    break;

                case "PB":


                    ClsDiff.writeLogOfAction("ورود به تب دفترچه تلفن");
                    this.TelTabActive();

                    break;


                case "DB":
                    ClsDiff.writeLogOfAction("ورود به تب دیتابیس");
                    this.DbNewTabActive();
                    break;


                case "PE":

                    ClsDiff.writeLogOfAction("ورود به تب پی ای");
                    PeTabActive();
                    break;


                case "Req":

                    ClsDiff.writeLogOfAction("ورود به رک");
                    ReqTabActive();
                    break;



            }


        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();

            #region تنظیم ورژن
            strCurVer = this.Text;

            //this.Text = strCurVer + "    " + "< به سامانه بررسی عملکرد مخابرات اصفهان خوش آمدید  >";
            #endregion

            tabControlMain.Visible = false;

            #region تشخیص آی پی شبکه کاندید برای ارتباط با سرور ترجیحا 10.84 و سپس 172.16
            StrCurIp = ClsNet.GetIpAddress();
            #endregion

            #region تشخیص مک آدرس آی پی کاندید شده
            strCurMac = ClsNet.GetMacFromIp(StrCurIp);
            #endregion

            #region تشخیص نام کامپیوتر 
            StrCumputerName = ClsNet.GetName();
            #endregion

            #region استخراج ساعت اس کیو ال و ثبت اطلاعات شبکه و ساعت اتصال در دیتابیس

            try
            {

                


                objClsSql.GetAllAndSendToDataBase("[PE].dbo.tblClient", "hostName,ip,mac,dateTime ", "N'" + StrCumputerName + "',N'" + StrCurIp + "',N'" + StrCurMac + "',N'" + DateTimeSqlServer + "'");


                ClsDiff.writeLogOfAction("ارتباط با دیتا بیس برقرار شد و سامانه لود شد");
            }
            catch
            {


                MessageBox.Show("خطا", "عدم اتصال 1", MessageBoxButtons.OK,

                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            #endregion

            #region پرکردن دیتاتیبل موقت از مینی دیتابیس

            DtMiniTempDb = objClsSql.FillDataTableFromQuery(@"SELECT 
                [کد سایت]
                ,[نام سایت]
                ,[BSC/RNC]
                ,[پیمانکار]
                ,[منطقه]
            FROM [DB].[dbo].[tblDb]");

            #endregion

            openLoginForm();

            this.CheckTab();

            #region واکشی تاریخ آخرین بروز رسانی دیتابیس

            try
            {
                strDateOfLastDbUpInStart = objClsSql.SendCommandGetStringResult("Select dateOfLastDbUp From PE.dbo.tblUpdateDate");
            }


            catch
            {
                MessageBox.Show(" به دلیل عدم اتصال به سرور اس کیو ال " + "\r\n" + strDateOfLastDbUpInStart + "\r\n" + " واکشی نشد لطفا مراتب را به مدیر سامانه اطلاع دهید" + "\r\n", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign, false);
            }

            #endregion

            #region راه اندازی اس کیو ال دپندنسی دان تایم


            // بارگذاری داده های موجود
            if (tabControlMain.SelectedTab.Name == "tabPageDT")
            {

                RefreshDtAndGoToEnd();
                //راه اندازی SqlDependency برای بررسی تغییرات
                StartMonitoringDt();

            }

            #endregion


            lblStatusBar.Text = objClsSql.SetDateForStatusBar();

            tabControlMain.Visible = true;


        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(" از سامانه خارج می شوید ؟ ",
                     "P.E",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information) == DialogResult.No)
            {
                //openLoginForm();
                e.Cancel = true;
            }

            // برای توقف SqlDependency استفاده می شود
            System.Data.SqlClient.SqlDependency.Stop(ConnectionStringDependencyDt);
            base.OnClosing(e);
        }


        #endregion

        #region LogIn_LogOut

        private void btnLogin_Click(object sender, EventArgs e)
        {
            openLoginForm();
        }

        private void openLoginForm()
        {
            new frmPermission().ShowDialog();

            if ((StrCurRol != "نامعتبر") && (StrCurRol != string.Empty))
            {
                ClsDiff.writeLogOfAction("با نام کاربری معتبر لاگین شد");

                this.btnLogin.Enabled = false;

                //FilldateTimeNumericUpDown();

                this.PermissionToObjectWithRoleForDt(StrCurRol);

                // this.lblLoginName.Text = "< " + StrF+ullNameCurUsr + " خوش آمدید  >";
                this.Text = string.Empty;


                this.Text = strCurVer + "    " + " < " + StrFullNameCurUsr + " به سامانه بررسی عملکرد مخابرات استان اصفهان خوش آمدید >";

                #region تشخیص نام کامپیوتر 

                strCurCompany = objClsSql.SendCommandGetStringResult("USE PE ;Select company From tblUsers Where UserName=N'" + StrCurUsr + "'");
                #endregion

                this.btnLogin.Enabled = false;
                this.btnLogOut.Enabled = true;
                //this.gridViewDt.OptionsBehavior.Editable = true;

                viewChangeRequest();

                this.CheckTab();
            }
            else
            {
                ClsDiff.writeLogOfAction("تلاش برای لاگین موفقیت آمیز نبود");

                
                // دستور صحیح برای بستن کامل برنامه
               // Application.Exit();

                this.resetDtToDefaultMode();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Text = "";
            strCurCompany = "";

            ClsDiff.writeLogOfAction("لاگ اوت انجام شد");
            this.btnLogOut.Enabled = false;
            this.btnLogin.Enabled = true;
            bolIsClickCTL_I = false;

            StrCurRol = string.Empty;
            StrCurUsr = string.Empty;
            StrFullNameCurUsr = string.Empty;
            
            
            openLoginForm();

            this.resetTelToDefaultMode();

            this.CheckTab();
        }

        #endregion

        #region DIFF
        private void SetContainsFilter(GridView view)
        {
            foreach (GridColumn col in view.Columns)
            {
                col.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
            }
        }

        private void CenterControlInParent(Control ctrlToCenter, bool bolVerticalAlign, bool bolHorizantalAlign)
        {
            if (bolHorizantalAlign)
                ctrlToCenter.Left = (ctrlToCenter.Parent.Width - ctrlToCenter.Width) / 2;
            if (bolVerticalAlign)
                ctrlToCenter.Top = (ctrlToCenter.Parent.Height - ctrlToCenter.Height) / 2;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.I))
            {
                bolIsClickCTL_I = true;
                btnLogin.PerformClick();


                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region SqlDependencyDt

        #region Vars Of sqlDependency

        /// برای اتصال SqlDependency با دیتابیس
        private readonly SqlConnection _connectionDependencyDt;

        /// برای اجرای دستور با SqlDependency
        private readonly SqlCommand _commandDependencyDt;

        /// شیء SqlDependency
        private System.Data.SqlClient.SqlDependency _dependencyDependencyDt;

        /// رشته اتصال به دیتابیس Sql Server
        private string ConnectionStringDependencyDt = "Data Source=" +"172.16.3.116" + ";Initial Catalog=DT;Persist Security Info=True;User ID=asg;Password=Asg_8560@200";

        /// دستور انتخاب فیلد های جدول
        private const string SelectQueryDependencyDt = "Select subject,performer,approver,dtNo,status,Note,outageMin,InBsyHour,haveAttachedFile FROM dbo.[tblDownTimeShort]";
        //private const string SelectQueryDependencyDt = "Select row,siteCode,startDate,startTime,endDate,endTime,applicant,subject,performer,approver,dtNo,status,Note,region,outageMin,InBsyHour,haveAttachedFile FROM dbo.[tblDownTimeShort]";
        //SELECT row,registerDate,registerTime,siteCode,domain,siteName,curBSC,startDate,startTime,endDate,endTime,applicant,subject,performer,approver,dtNo,status,Note,region,outageMin,mysqlFlag,registerDateInSdm,lastModifyStatusUsrDate,InBsyHour,haveAttachedFile FROM DT.dbo.tblDownTimeShort


        #endregion

        private void StartMonitoringDt()
        {
            try
            {
                //// قبل از شروع یک بار آن متوقف کنید
                System.Data.SqlClient.SqlDependency.Stop(ConnectionStringDependencyDt);
                System.Data.SqlClient.SqlDependency.Start(ConnectionStringDependencyDt);

                RegisterSqlDependencyDt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RegisterSqlDependencyDt()
        {
            // بررسی دسترسی کاربر
            if (!DoesUserHavePermissionDt())
                return;

            // بایند کردن command با شیء SqlDependency
            _commandDependencyDt.Notification = null;
            _dependencyDependencyDt = new System.Data.SqlClient.SqlDependency(_commandDependencyDt);

            // ایجاد یک handler برای اعمال تغییرات بر روی دیتا گرید
            _dependencyDependencyDt.OnChange += DependencyOnChangeDt;

            // باز کردن اتصال دیتابیس در صورت نیاز
            if (_connectionDependencyDt.State != ConnectionState.Open)
                _connectionDependencyDt.Open();

            // اجرای Command با یکی از متدهای Execute موجود در شیء SqlCommand
            _commandDependencyDt.ExecuteNonQuery();

            // بستن اتصال بعد از اجرای دستور
            _connectionDependencyDt.Close();
        }
        private void DependencyOnChangeDt(object sender, SqlNotificationEventArgs e)
        {

            bolDtIsNeddedToRefresh = true;

            var dependencyDt = (SqlDependency)sender;
            dependencyDt.OnChange -= DependencyOnChangeDt;
            Invoke(new System.Action(coloring));
            RegisterSqlDependencyDt();
        }


        public void coloring()
        {

            pictureBoxUpDt.ImageLocation = MainForm.StrSelectedRootPathInPeServer + @"\Not update.png";//
            pictureBoxUpDt.Load();


        }

        private bool DoesUserHavePermissionDt()
        {
            try
            {
                var clientPermission = new SqlClientPermission(PermissionState.Unrestricted);
                clientPermission.Demand();
                return true;
            }
            catch
            {
                MessageBox.Show("دسترسی کاربر به اس کیو ال دپندنسی بررسی گردد");
                return false;
            }
        }
        #endregion

        #endregion

        #region DT

        private void دانتایمپیامToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPmReq objFrmPmReq = new frmPmReq();
            objFrmPmReq.ShowDialog();
        }
        private void فعالToolStripMenuItem_Click(object sender, EventArgs e)
        {
            غیرفعالToolStripMenuItem.Checked = false;
            فعالToolStripMenuItem.Checked = true;
            BolHavePermitToRegDt = true;

            MessageBox.Show(" ثبت دان تایم توسط ادمین  فعال شد", "اطلاع رسانی", MessageBoxButtons.OK,
            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void غیرفعالToolStripMenuItem_Click(object sender, EventArgs e)
        {

            فعالToolStripMenuItem.Checked = false;
            غیرفعالToolStripMenuItem.Checked = true;
            BolHavePermitToRegDt = false;

            MessageBox.Show("بطور موقت ثبت دان تایم توسط ادمین غیر فعال شد", "اطلاع رسانی", MessageBoxButtons.OK,
            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

        }

        private void ماهاخیرToolStripMenuItem_Click(object sender, EventArgs e)
        {
            پانزدهروزاخیرToolStripMenuItem.Checked = false;
            ماهاخیرToolStripMenuItem.Checked = true;
            دوماهاخیرToolStripMenuItem.Checked = false;
            همهToolStripMenuItem.Checked = false;

            bolDtShowCurrentMonth = true;
            bolDtForceToRefreshAndGoToEnd = true;
            bolDtForceToRefreshAndDontGoToEnd = false;

            RefreshDtAndGoToEnd();
        }

        private void دوماهاخیرToolStripMenuItem_Click(object sender, EventArgs e)
        {
            پانزدهروزاخیرToolStripMenuItem.Checked = false;
            دوماهاخیرToolStripMenuItem.Checked = true;
            ماهاخیرToolStripMenuItem.Checked = false;
            همهToolStripMenuItem.Checked = false;

            bolDtShowCurrentMonth = true;
            bolDtForceToRefreshAndGoToEnd = true;
            bolDtForceToRefreshAndDontGoToEnd = false;

            RefreshDtAndGoToEnd();
        }

        private void toolStripMenuItemStatistics_Click(object sender, EventArgs e)
        {
            frmAmar objFrmAmar = new frmAmar();
            objFrmAmar.ShowDialog();
        }

        #region DT Tab Active
        public void DtTabActive()
        {


            RefreshDtAndGoToEnd();

            this.tabPageDT.ToolTipText = "Down Time";

            #region پنهان سازی صفحه هنگام لود
            tabControlMain.Visible = false;
            #endregion

            #region ایجاد یک آرایه از ردیفهایی که فایل پیوست دارند
            DrRowNumberAttachFile = objClsSql.FillDataTableFromQuery("Select rowWithAttachedFileInDt From DT.dbo.tblAttachedFile").Select();
            #endregion

            #region ایجاد یک آرایه از ردیفهایی دان تایم تکراری دارند
            drRowNumberRepetedDt = objClsSql.FillDataTableFromQuery("Select repeatedRow From DT.dbo.tblRepetedDt").Select();


            #endregion

            manageDtAuthentication(StrCurRol);

            #region تنظیم فیلتر جزیی برای ستونهای گرید
            SetContainsFilter(gridViewDt);//فیلتر جزیی 
            #endregion

            #region آشکار سازی صفحه بعد از لود
            tabControlMain.Visible = true;
            #endregion

        }

        #endregion

        #region GridViewDt
        private void gridViewDt_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (tabControlMain.SelectedTab.Text != "DT")
                return;

            GridView view = sender as GridView;

            if (e.RowHandle >= 0)
            {
                string strStatus = view.GetRowCellDisplayText(e.RowHandle, view.Columns["وضعیت"]);
                string strActivity = view.GetRowCellDisplayText(e.RowHandle, view.Columns["فعالیت"]);
                string strNote = view.GetRowCellDisplayText(e.RowHandle, view.Columns["یادداشت"]);

                // تنظیمات پایه
                e.Appearance.BackColor2 = Color.White;

                // ساخت رشته ترکیبی وضعیت + فعالیت
                string strCombinedStatus;
                if (string.IsNullOrEmpty(strActivity) || strActivity == "خالی" || strActivity == "")
                {
                    strCombinedStatus = $"{strStatus} - بدون فعالیت";
                }
                else
                {
                    strCombinedStatus = $"{strStatus} - {strActivity}";
                }

                if (strNote.Contains("کنسل"))
                {
                    e.Appearance.ForeColor = Color.Orange;
                }

                if (strStatus.Contains("در حال ویرایش"))
                {
                    e.Appearance.ForeColor = Color.Red;
                }

                // ================================================
                // ۱. بررسی حالت‌های ترکیبی (۲۳ حالت بر اساس CSV جدید)
                // ================================================

                // === گروه تائید ===
                if (strCombinedStatus == "تائید - انجام")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 128, 128);  // #008080
                    e.Appearance.ForeColor = Color.FromArgb(0, 0, 160);    // #0000A0
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "تائید - لغو اضطراری")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 128, 128);  // #008080
                    e.Appearance.ForeColor = Color.FromArgb(255, 28, 255); // #FF1CFF
                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    return;
                }
                if (strCombinedStatus == "تائید - لغو توسط سیستم")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 128, 128);  // #008080
                    e.Appearance.ForeColor = Color.FromArgb(255, 255, 0);  // #FFFF00
                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    return;
                }
                if (strCombinedStatus == "تائید - لغو توسط کاربر")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 128, 128);  // #008080
                    e.Appearance.ForeColor = Color.FromArgb(255, 128, 0);  // #FF8000
                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    return;
                }
                if (strCombinedStatus == "تائید - لغو - شبکه فریز")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 128, 128);  // #008080
                    e.Appearance.ForeColor = Color.White;                   // #FFFFFF
                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    return;
                }
                if (strCombinedStatus == "تائید - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 128, 128);  // #008080
                    e.Appearance.ForeColor = Color.Black;                   // #000000
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }

                // === گروه تائید مشروط ===
                if (strCombinedStatus == "تائید مشروط - انجام")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 179, 179);  // #00B3B3
                    e.Appearance.ForeColor = Color.FromArgb(0, 0, 160);    // #0000A0
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "تائید مشروط - لغو اضطراری")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 179, 179);  // #00B3B3
                    e.Appearance.ForeColor = Color.FromArgb(191, 0, 191);  // #BF00BF
                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    return;
                }
                if (strCombinedStatus == "تائید مشروط - لغو توسط سیستم")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 179, 179);  // #00B3B3
                    e.Appearance.ForeColor = Color.FromArgb(225, 225, 0);  // ##E1E100

                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    return;
                }
                if (strCombinedStatus == "تائید مشروط - لغو توسط کاربر")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 179, 179);  // #00B3B3
                    e.Appearance.ForeColor = Color.FromArgb(149, 74, 0);   // #954A00
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "تائید مشروط - لغو - شبکه فریز")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 179, 179);  // #00B3B3
                    e.Appearance.ForeColor = Color.White;                   // #FFFFFF
                    e.Appearance.BackColor2 = e.Appearance.BackColor;
                    return;
                }
                if (strCombinedStatus == "تائید مشروط - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(0, 179, 179);  // #00B3B3
                    e.Appearance.ForeColor = Color.Black;                   // #000000
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }

                // === سایر حالت‌ها ===
                if (strCombinedStatus == "ثبت در (SDM) - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(128, 255, 128); // #80FF80
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "حذف - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(128, 128, 128); // #808080
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "در حال پردازش درخواست - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 255, 0);   // #FFFF00
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "درخواست منقضی - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(210, 180, 140); // #D2B48C
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "شبکه فریز - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(176, 168, 224); // #B0A8E0
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "عدم تائید - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 0, 0);     // #FF0000
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "عدم تائید(موقت) - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 45, 150);  // #FF2D96
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "لغو - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(224, 144, 192); // #E090C0
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "مغایرت با دستورالعمل - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(160, 82, 45);   // #A0522D
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "منتظر ارائه در جلسه - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 179, 71);  // #FFB347
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }
                if (strCombinedStatus == "نیاز به اصلاح درخواست - بدون فعالیت")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 222, 173); // #FFDEAD
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.BackColor2 = Color.White;
                    return;
                }

                // ================================================
                // ۲. اگر حالت ترکیبی جدید نبود، برو سراغ حالت‌های قدیمی
                // ================================================
                switch (strStatus)
                {
                    case "پردازش فنی":
                        e.Appearance.BackColor = Color.LawnGreen;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "لغو":
                        e.Appearance.BackColor = Color.MediumVioletRed;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "ثبت توسط کاربر":
                        e.Appearance.BackColor = Color.White;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "در حال پردازش درخواست":
                        e.Appearance.BackColor = Color.Yellow;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "مغایرت با دستورالعمل":
                        e.Appearance.BackColor = Color.SaddleBrown;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "شبکه فریز":
                        e.Appearance.BackColor = Color.FromArgb(154, 147, 236);
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "شبکه فریز و مغایر دستور العمل":
                        e.Appearance.BackColor = Color.FromArgb(154, 147, 236);
                        e.Appearance.BackColor2 = Color.SaddleBrown;
                        return;

                    case "حذف":
                        e.Appearance.BackColor = Color.Gray;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "ثبت در (SDM)":
                        e.Appearance.BackColor = Color.LightGreen;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "نیاز به اصلاح درخواست":
                        e.Appearance.BackColor = Color.Bisque;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "درخواست منقضی":
                        e.Appearance.BackColor = Color.RosyBrown;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "درخواست اصلاح و مجددا ثبت شد":
                        e.Appearance.BackColor = Color.FromArgb(170, 130, 170);
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "منتظر ارائه در جلسه":
                        e.Appearance.BackColor = Color.Orange;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "تائید":
                        e.Appearance.BackColor = Color.FromArgb(0, 128, 128);
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "تائید مشروط":
                        e.Appearance.BackColor = Color.FromArgb(0, 179, 179);
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "عدم تائید نهایی":
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "عدم تائید(موقت)":
                        e.Appearance.BackColor = Color.MediumPurple;
                        e.Appearance.ForeColor = Color.Black;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "تائید ناموفق":
                        e.Appearance.BackColor = Color.FromArgb(0, 128, 128);
                        e.Appearance.BackColor2 = Color.White;
                        e.Appearance.ForeColor = Color.Brown;
                        return;

                    case "تائید موفق":
                        e.Appearance.BackColor = Color.FromArgb(0, 128, 128);
                        e.Appearance.BackColor2 = Color.White;
                        e.Appearance.ForeColor = Color.DarkBlue;
                        return;

                    case "در انتظار تایید درخواست مرتبط":
                        e.Appearance.BackColor = Color.FromArgb(200, 180, 90);
                        e.Appearance.BackColor2 = Color.White;
                        return;
                }
            }
        }
        private void gridViewDt_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (StrCurRol != "PowerUser" && StrCurRol != "administrator")
            {
                return;
            }
            GridView view = sender as GridView;
            // Check whether a row is right-clicked.
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
            {
                int rowHandle = e.HitInfo.RowHandle;
                // Delete existing menu items, if any.
                e.Menu.Items.Clear();
                // Add a submenu with a single menu item.
                e.Menu.Items.Add(CreateRowSubMenu(view, rowHandle));

            }
        }
        private void gridViewDt_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (((e.Column.ToString() == "شماره") && ((StrCurRol == "PowerUser") || (StrCurRol == "administrator"))) && (e.Column.ToString().Length >= 4))
            {
                this.gridViewDt.FocusedRowHandle = e.RowHandle;
                this.applyApprover();
            }
            //test
            string cellValue = e.Value.ToString();
            string cellValue1 = gridViewDt.GetRowCellValue(e.RowHandle, gridViewDt.Columns[e.Column.ToString()]).ToString();
        }
        private void gridViewDt_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    GridView view = sender as GridView;

                    StrCurRowInDtGrid = this.gridViewDt.GetDataRow(e.FocusedRowHandle)["ردیف"].ToString();

                    StrDTCurDtNo = view.GetRowCellDisplayText(e.FocusedRowHandle, view.Columns["شماره"]);

                    StrDtState = view.GetRowCellDisplayText(e.FocusedRowHandle, view.Columns["وضعیت"]);

                    switch (StrDtState)
                    {
                        case "":
                            IntDtPhaseOfDownTime = 0;
                            return;
                        case "ثبت توسط کاربر":
                            IntDtPhaseOfDownTime = 1;
                            return;
                        case "ثبت در (SDM)":
                            IntDtPhaseOfDownTime = 2;
                            return;

                        case "تائید":
                            IntDtPhaseOfDownTime = 3;
                            return;
                    }
                }
            }
            catch
            {
            }
        }
        private void gridViewDt_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            string row = gridViewDt.GetRowCellDisplayText(e.RowHandle, gridViewDt.Columns["ردیف"]);

            if (e.Column.FieldName == "ردیف" && DrRowNumberAttachFile != null)
            {
                for (int i = 0; i < DrRowNumberAttachFile.Length; i++)
                {
                    if (DrRowNumberAttachFile[i].ItemArray[0].ToString() == row)
                    {
                        e.Appearance.ForeColor = Color.Blue;
                    }

                }

                for (int j = 0; j < drRowNumberRepetedDt.Length; j++)
                {
                    if (drRowNumberRepetedDt[j].ItemArray[0].ToString() == row)
                    {
                        e.Appearance.BackColor = Color.CornflowerBlue;
                        //e.RowHandle 
                    }

                }

            }
        }
        public void RefreshDtGrid(bool bolGoToEnd)
        {
            //پیکچر
            pictureBoxUpDt.ImageLocation = MainForm.StrSelectedRootPathInPeServer + @"\update.png";//
            pictureBoxUpDt.Load();

            bolDtIsNeddedToRefresh = false;

            //if (gridViewDt.SelectedRowsCount > 1 || !BolHavePermitRefresh)
            if (gridViewDt.SelectedRowsCount > 1)
                return;
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            if (tabControlMain.SelectedTab.Name == "tabPageDT")
            {
                //StartMonitoringDt();موقتا به خاطر سرعت حذف کردم

                #region ثبت اینکس اولین سط و سطر فوکوس شده برای برگرداندن به حالت قبل در صورت لزوم 
                intDtTopRowInGrid = gridViewDt.TopRowIndex;
                intDtFocusedRow = gridViewDt.FocusedRowHandle;

                #endregion

                #region ایجاد یک آرایه از ردیفهایی که فایل پیوست دارند
                DrRowNumberAttachFile = objClsSql.FillDataTableFromQuery("Select rowWithAttachedFileInDt From DT.dbo.tblAttachedFile order by [rowWithAttachedFileInDt]").Select();
                #endregion

                #region ایجاد یک آرایه از ردیفهایی دان تایم تکراری دارند
                drRowNumberRepetedDt = objClsSql.FillDataTableFromQuery("Select repeatedRow From DT.dbo.tblRepetedDt").Select();
                #endregion

                #region بررسی اینکه شبکه فریز است یا نه
                string strIsFreezeNet = objClsSql.SendCommandGetStringResult("SELECT  [isFreezeNetwork]  FROM [DT].[dbo].[tblFreeze]");

                if (strIsFreezeNet == "True")
                {

                    gridViewDt.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    gridViewDt.Appearance.GroupPanel.ForeColor = System.Drawing.Color.BlueViolet;

                    اعلانفریزبودنشبکهToolStripMenuItem.Checked = true;


                    string strStartFreezDate = objClsSql.SendCommandGetStringResult("Select startFreezeDate From DT.dbo.tblFreezDate Where row=1");
                    string strEndFreezDate = objClsSql.SendCommandGetStringResult("Select endFreezeDate From DT.dbo.tblFreezDate Where row=1");
                    //    string strXtraString = @" و هیچگونه فعالیت برنامه ریزی شده ای تایید نخواهد شد ." + "\r\n" + @"
                    //توجه داشته باشید که موارد *رفع خرابی* بر اساس پروسه سی آر های Emergency  و صرفا با *تایید مدیریت محترم اداره کل عملیات شبکه* قابل اجرا خواهند بود .
                    //";

                    string strXtraString = @" و هیچگونه فعالیت برنامه ریزی شده ای تایید نخواهد شد" + "\r\n" + " (به جز فعالیتهای اضطراری با مجوز عملیات شبکه)";


                    // 
                    gridViewDt.GroupPanelText = string.Format("  کاربر گرامی شبکه از ( {0} ) تا ( {1} ) فریز می باشد", strStartFreezDate, strEndFreezDate) + strXtraString;


                }
                else
                {
                    gridViewDt.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    gridViewDt.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Green;

                    اعلانفریزبودنشبکهToolStripMenuItem.Checked = false;
                    gridViewDt.GroupPanelText = MainForm.strDtAboveGridWhenNotFreez = objClsSql.SendCommandGetStringResult("SELECT messageForDtGrid FROM DT.dbo.tblUnFreezeMessage Where row=1");
                    gridViewDt.Appearance.GroupPanel.ForeColor = Color.FromArgb(int.Parse(objClsSql.SendCommandGetStringResult("SELECT messageColor FROM DT.dbo.tblUnFreezeMessage Where row=1")));
                }
                #endregion

                #region تشخیص تعداد قابل نمایش دان تایم

                string strLimitForShowableInQuery = string.Empty;

                if (پانزدهروزاخیرToolStripMenuItem.Checked)
                {
                    DateTime dteFifteenDayAgo = DateTimeSqlServer.AddDays(-15);
                    string strDate15DaysAgo = ClsConvertDateHassan.ConvertDateToPersian(dteFifteenDayAgo).Substring(0, 10);
                    strLimitForShowableInQuery = " where registerDate> N'" + strDate15DaysAgo + "'";
                }
                else if (ماهاخیرToolStripMenuItem.Checked)
                {
                    DateTime dteThirtyDayAgo = DateTimeSqlServer.AddDays(-30);
                    string strDate30DaysAgo = ClsConvertDateHassan.ConvertDateToPersian(dteThirtyDayAgo).Substring(0, 10);
                    strLimitForShowableInQuery = " where registerDate> N'" + strDate30DaysAgo + "'";
                }
                else if (دوماهاخیرToolStripMenuItem.Checked)
                {
                    DateTime dteSixtyDayAgo = DateTimeSqlServer.AddDays(-60);
                    string strDate60DaysAgo = ClsConvertDateHassan.ConvertDateToPersian(dteSixtyDayAgo).Substring(0, 10);
                    strLimitForShowableInQuery = " where registerDate> N'" + strDate60DaysAgo + "'";
                }
                else
                {
                    strLimitForShowableInQuery = " where 1=1";
                    //strLimitForShowableInQuery = "";
                }

                #endregion

                #region بررسی نوع فیلتر

                string strLimitForQuery = string.Empty;

                if (!HaveAnyFilterInDownTimeGrid())//معمولیها یعنی بدون فیلتر
                {
                    strLimitForQuery = string.Empty;
                }
                else // فیلتری ها 
                {
                    if (درحالاجراToolStripMenuItem1.Checked)
                    {
                        #region نمایش دان تایمهای در حال اجرا

                        string strCurTime = MainForm.DateTimeSqlServer.ToString("HH:mm");

                        strLimitForQuery = @"startDate=N'" + ClsConvertDateHassan.ConvertDateToPersian(DateTimeSqlServer) +
                            @"' AND N'" + strCurTime +
                            @"'>=startTime " +
                            @" AND N'" + strCurTime +
                            @"'<=endTime AND [status]=N'تائید'";

                        #endregion
                    }

                    if (ثبتتوسطکاربرToolStripMenuItem.Checked)
                    {
                        strLimitForQuery = " status=N'" + "ثبت توسط کاربر" + "'";
                    }

                    if (منتظرارائهدرجلسهToolStripMenuItem.Checked)
                    {
                        strLimitForQuery = " status=N'" + "منتظر ارائه در جلسه" + "'";
                    }

                    if (ثبتدرکلاریتیToolStripMenuItem.Checked)
                    {
                        strLimitForQuery = " status=N'" + "ثبت در (SDM)" + "'";
                    }

                    if (درحالپردازشدرخواستToolStripMenuItem.Checked)
                    {
                        strLimitForQuery = " status=N'" + "در حال پردازش درخواست" + "'";
                    }
                    if (پکیجToolStripMenuItem.Checked)
                    {
                        //strSubjectForPm == "PM-Cleaning" || strSubjectForPm == "PM-Site" || strSubjectForPm == "PM-Tower"
                        strLimitForQuery = " status=N'" + "ثبت توسط کاربر" + "' AND  (subject = N'PM-Cleaning' OR subject = N'PM-Site' OR subject = N'PM-Tower')  ";
                    }
                    if (نیازبهبررسیToolStripMenuItem.Checked)
                    {
                        strLimitForQuery = " status=N'" + "ثبت در (SDM)" + "' OR" +
                        " status=N'" + "ثبت توسط کاربر" + "' OR" +
                        " status=N'" + "پردازش فنی" + "' OR"+ 
                        " status=N'" + "در حال پردازش درخواست" + "' OR" +
                         " status=N'" + "عدم تائید(موقت)" + "' OR" +
                        " status=N'" + "در انتظار تایید درخواست مرتبط" + "'" ;
                    }
                    
                    if (بررسینهاییToolStripMenuItem.Checked)
                    {
                        strLimitForQuery = " status=N'" + "ثبت در (SDM)" + "' OR" +
                        " status=N'" + "ثبت توسط کاربر" + "' OR" +
                        " status=N'" + "پردازش فنی" + "' OR" +
                        " status=N'" + "در حال پردازش درخواست" + "' OR" +
                         " status=N'" + "عدم تائید(موقت)" + "' OR" +
                        " status=N'" + "در انتظار تایید درخواست مرتبط" + "' OR" +
                        " status=N'" + "منتظر ارائه در جلسه" + "'";
                    }
                }

                string strQuery = @"USE DT;   
    SELECT   row AS ردیف ,
    registerDate AS [تاریخ ثبت],
    registerTime AS [ثبت],
    siteCode AS [کد سایت],
    siteName AS [نام سایت],
    region AS [منطقه],
    startDate AS [تاریخ شروع],
    startTime AS [زمان شروع],
    endDate AS [تاریخ پایان],
    endTime AS [زمان پایان],
    applicant AS [درخواست کننده],
    subject AS موضوع,
    performer AS [انجام دهنده],
    approver AS [ثبت کننده],
     dtNo AS شماره ,
    status AS [وضعیت],
    Note AS [یادداشت],
    Activity AS [فعالیت]
     FROM   [DT].[dbo].tblDownTimeShort 
    ";

                if (تکراریToolStripMenuItem.Checked)
                {
                    strQuery += " INNER JOIN dbo.tblRepetedDt ON dbo.tblDownTimeShort.row = dbo.tblRepetedDt.repeatedRow ";
                }

                if (strLimitForQuery != string.Empty)
                {
                    strQuery += strLimitForShowableInQuery + " AND " + strLimitForQuery;
                }
                else
                {
                    strQuery += strLimitForShowableInQuery;
                }

                strQuery += " Order By row ";

                #endregion 

                #region نمایش دان تایمها با یا بدون فیلتر 

                this.gridControlDt.DataSource = null;

                this.gridControlDt.DataSource = objClsSql.FillDataTableFromQuery(strQuery);


                this.gridViewDt.OptionsView.ColumnAutoWidth = false;


                try
                {

                    this.gridViewDt.Columns["ردیف"].Width = 55;
                    this.gridViewDt.Columns["تاریخ ثبت"].Width = 55;
                    this.gridViewDt.Columns["ثبت"].Width = 40;
                    this.gridViewDt.Columns["کد سایت"].Width = 60;
                    this.gridViewDt.Columns["نام سایت"].Width = 95;
                    this.gridViewDt.Columns["منطقه"].Width = 40;
                    this.gridViewDt.Columns["تاریخ شروع"].Width = 55;
                    this.gridViewDt.Columns["زمان شروع"].Width = 45;
                    this.gridViewDt.Columns["تاریخ پایان"].Width = 55;
                    this.gridViewDt.Columns["زمان پایان"].Width = 40;
                    this.gridViewDt.Columns["درخواست کننده"].Width = 90;
                    this.gridViewDt.Columns["موضوع"].Width = 100;
                    this.gridViewDt.Columns["انجام دهنده"].Width = 120;
                    this.gridViewDt.Columns["ثبت کننده"].Width = 80;
                    this.gridViewDt.Columns["شماره"].Width = 123;
                    this.gridViewDt.Columns["وضعیت"].Width = 75;
                    this.gridViewDt.Columns["یادداشت"].Width = 120;
                }
                catch (Exception)
                {
                    MessageBox.Show(" اطلاعات دان تایم از دیتا بیس واکشی نشد در صورتی که بعد از ریفرش جدول تکمیل نگردید مراتب را به مدیر سامانه اطلاع دهید  ");
                    return;
                }
                #endregion

                #region بررسی رفتن به سطر آخر یا نه

                #region اگر نیاز به رفتن به سطر آخره برو و دوباره مجوز رفتن به آخر را فالس کن

                if (bolGoToEnd)
                {
                    gridViewDt.MoveLast();//رفتن به سطر آخر

                    bolGoToEnd = false;//ریست 
                }
                else
                {
                    #region برگشت به سطر مورد تغییر
                    try
                    {
                        #region برگرداندن گرید دان تایم به حالت شروع تغییر مثل بالی جدول و سطر انتخاب شده
                        gridViewDt.TopRowIndex = intDtTopRowInGrid;
                        gridViewDt.FocusedRowHandle = intDtFocusedRow;

                        intDtTopRowInGrid = 0;
                        intDtLastSelectedRow = 0;

                        #endregion



                    }
                    catch
                    {

                    }
                    #endregion
                }

                #endregion

                #endregion


            }

        }

        #endregion

        #region Reset To Default
        private void resetDtToDefaultMode()
        {
            if ((StrCurMac == "00-24-1D-29-B4-67") || (StrCurMac == "FC-AA-14-36-5B-B6"))
            {
                MainForm.BolDtIsCreateDownTime = true;
            }

            this.gridViewDt.OptionsBehavior.Editable = false;
            StrCurPerformer = string.Empty;
            StrCurSiteName = string.Empty;
            StrDtStartDate = ClsConvertDateHassan.convertDateToPersion(MainForm.DateTimeSqlServer.AddDays(2));
            StrCurStartTime = string.Empty;
            StrDtEndDate = StrDtStartDate;
            StrCurEndTime = string.Empty;
            StrCurApplicant = string.Empty;
            StrCurDtNo = string.Empty;
            StrCurSubject = string.Empty;
            StrCurApprover = string.Empty;



            this.enableDisableObject(false, false, false, true, true, false, false, false, true, true, false, false);
            StrSelectedPershianDateWithSlashSeperator = StrSelectedPershianDateWithSlashSeperator = ClsConvertDateHassan.convertDateTimeToPersionWithSlash(DateTimeSqlServer.AddDays(2)).Substring(0, 10);
        }

        #endregion

        #region functions


        private void PerformChanges()
        {
            if (IntDtPhaseOfDownTime == 3)
            {
                switch (MessageBox.Show(" دان تایم ردیف  " + MainForm.StrCurRowInDtGrid + " قبلا تکمیل شده آیا واقعا می خواهید آنرا تغییر دهید ؟    ", "  تاییدیه اعمال  تغییرات  ", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes://با علم به اینکه دان تایم تکمیل است می خوام تغییر دهم
                        checkOneByOneFileAndEdit();
                        ClsDiff.writeLogOfAction("تغییر در فیلدی از دان تایم");
                        break;
                    case DialogResult.No:
                        break;

                }
            }
            else
            {
                checkOneByOneFileAndEdit();
            }

            ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfTblDt");


            //     MainForm.bolDtIsChangingInLastRegisterDt = true;//برای اینکه ریفرش گرید دانتایم آنرا به آخر نبرد

            //DtTabActive();
        }

        private void checkOneByOneFileAndEdit()//چک تک تک فیلدها و اعمال تغییرات در دیتا بیس
        {

            bool bolIsCompChange = false;

            if (MainForm.StrCurRowInDtGrid != string.Empty)//اگر سطر جهت اعمال تغییرات انتخاب شده
            {
                string strCurRowInDtGrid = MainForm.StrCurRowInDtGrid;//استخراج سطر قابل تغییر

                gridViewDt.ClearSelection();

                intDtLastSelectedRow = int.Parse(strCurRowInDtGrid);
                intDtTopRowInGrid = gridViewDt.TopRowIndex;

                switch (MessageBox.Show(" تغییرات روی ردیف  " + MainForm.StrCurRowInDtGrid + " اعمال گردد   ؟    ", "  تاییدیه اعمال  تغییرات  ", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        #region YES

                        string strDtCurRowBeforChange = StrCurRowInDtGrid;

                        foreach (GridColumn column in this.gridViewDt.Columns)//فراخوانی تک تک ستونها و
                        {
                            string strFieldName = string.Empty;

                            switch (column.ToString())// پیدا کردن نام دیتابیسی
                            {
                                #region پیدا کردن نام دیتابیسی ستون
                                case "ردیف":
                                    strFieldName = "row";
                                    break;

                                case "تاریخ ثبت":
                                    strFieldName = "registerDate";
                                    break;

                                case "ثبت":
                                    strFieldName = "registerTime";
                                    break;

                                case "کد سایت":
                                    strFieldName = "siteCode";
                                    break;

                                case "نام سایت":
                                    strFieldName = "siteName";
                                    break;

                                case "منطقه":
                                    strFieldName = "region";
                                    break;

                                case "تاریخ شروع":
                                    strFieldName = "startDate";
                                    break;

                                case "زمان":
                                    strFieldName = "startTime";
                                    break;

                                case "تاریخ پایان":
                                    strFieldName = "endDate";
                                    break;

                                case "زمان پایان":
                                    strFieldName = "endTime";
                                    break;

                                case "درخواست کننده":
                                    strFieldName = "applicant";
                                    break;

                                case "موضوع":
                                    strFieldName = "subject";
                                    break;

                                case "انجام دهنده":
                                    strFieldName = "performer";
                                    break;

                                case "ثبت کننده":
                                    strFieldName = "approver";
                                    break;

                                case "شماره":
                                    strFieldName = "dtNo";
                                    break;

                                case "وضعیت":
                                    strFieldName = "status";
                                    break;

                                case "یادداشت":
                                    strFieldName = "Note";
                                    break;
                                    #endregion
                            }

                            try
                            {

                                #region انتخاب واقعی سطر کلیک شده

                                this.gridViewDt.FocusedRowHandle = (int.Parse(MainForm.StrCurRowInDtGrid)) - 1;
                                this.gridViewDt.SelectRow(int.Parse(MainForm.StrCurRowInDtGrid) - 1);

                                #endregion

                                string strNewValue = this.gridViewDt.GetRowCellValue(this.gridViewDt.GetSelectedRows()[0], column).ToString();//پیدا کردن مقدار فیلد جدید

                                #region بررسی اینکه فیلد مورد تغییر شماره دان تایم هست یا نه
                                if (strFieldName != "dtNo")
                                {
                                    #region اگر فیلد مورد تغییر شماره دان تایم نیست فقط ثبت تغییرات

                                    string strSqlForEditDt = @"
                                        Update  top(1) [DT].[dbo].[tblDownTimeShort]
                                         set " + strFieldName + @"=N'" + strNewValue + @"'
	                                     where row= N'" + MainForm.StrCurRowInDtGrid + @"'
	                                     ";
                                    bolIsCompChange = objClsSql.SendCommand(strSqlForEditDt);


                                    if (strFieldName == "status" && strNewValue == "تکراری")
                                    {
                                        objClsSql.DeleteRecords("[DT].[dbo].[tblRepetedDt]", "WHERE repeatedRow =N'" + MainForm.StrCurRowInDtGrid + "'");

                                    }


                                    #endregion
                                }
                                else
                                {
                                    #region اگر فیلد مورد تغییر شماره دان تایم است  ثبت تغییرات و ثبت وضعیت بعه عنوان ثبت در (SDM)

                                    string strSqlForEditDt = @"
                                        Update  top(1) [DT].[dbo].[tblDownTimeShort]
                                         set " + strFieldName + @"=N'" + strNewValue + @"'
	                                     where row= N'" + MainForm.StrCurRowInDtGrid + @"'
	                                     ";
                                    bolIsCompChange = objClsSql.SendCommand(strSqlForEditDt);
                                    if (IntDtPhaseOfDownTime == 1 && strNewValue != string.Empty)
                                    {
                                        bolIsCompChange = objClsSql.EditRecord("[DT].[dbo].tblDownTimeShort", "status", "ثبت در (SDM)", "Where row=N'" + MainForm.StrCurRowInDtGrid + "'");
                                        IntDtPhaseOfDownTime = 2;

                                    }


                                    #endregion
                                }
                                #endregion
                            }
                            catch
                            {

                            }

                        }

                        RefreshDtAndDontGoToEnd();

                        if (bolIsCompChange)
                        {
                            // MessageBox.Show(". تغییرات روی ردیف " + strCurRowInDtGrid + " اعمال گردید ", "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                            bolIsCompChange = false;

                        }
                        else
                        {
                            MessageBox.Show(" حداقل یکی از تغییرات انجام نشد ", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                        }

                        break;
                    #endregion
                    case DialogResult.No:

                        #region NO
                        break;
                        #endregion
                }
            }
            else
            {
                MessageBox.Show("هیچ ردیفی انتخاب نشده !", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            }


        }

        private void PermissionToObjectWithRoleForDt(string strRole)
        {
            switch (strRole)
            {
                case "administrator":
                    this.enableDisableObject(true, true, true, true, true, true, true, true, true, true, true, true);
                    break;

                case "PowerUser":
                    this.enableDisableObject(true, true, true, true, true, true, true, true, true, true, true, true);
                    break;

                case "register":
                    this.enableDisableObject(true, false, true, true, true, true, false, false, true, true, true, true);
                    break;
                case "pm_requester":
                    this.enableDisableObject(true, false, true, true, true, true, false, false, true, true, true, true);
                    break;

                case "contractor":
                    this.enableDisableObject(false, false, false, true, true, true, false, false, true, true, true, false);
                    break;
                case "k_masool":
                    this.enableDisableObject(true, false, true, true, true, true, false, false, true, true, true, true);
                    break;
                case "ts":
                    this.enableDisableObject(true, false, true, true, true, true, false, false, true, true, true, true);
                    break;
                default:
                    this.enableDisableObject(false, false, false, true, false, true, false, false, false, true, false, false);
                    break;
            }
            //this.txtDtApplicant.Text = objClsSql.FindRecord("[PE].[dbo].tblUsers", "fullName", "Where username=N'" + StrCurUsr + "'");
            //this.txtDtApplicant.Enabled = false;
        }

        private void applyApprover()
        {
            DataRow dataRow = this.gridViewDt.GetDataRow(this.gridViewDt.FocusedRowHandle);
            string str = dataRow[0].ToString();
            if (StrCurUsr != string.Empty)
            {
                string str2 = dataRow["ثبت کننده"].ToString();

                if (dataRow["شماره"].ToString() != string.Empty)
                {
                    dataRow["ثبت کننده"] = StrFullNameCurUsr;
                    dataRow["وضعیت"] = "ثبت در (SDM)";
                }

                this.PerformChanges();
            }
            else
            {
                MessageBox.Show(".برای ثبت باید با نام کاربری مشخص وارد شده باشید");
            }
        }

        private void enableDisableObject(bool bolBtnReg, bool bolBtnFinalReg, bool bolBtnDel, bool bolBtnExpXcl, bool bolGrdConDt, bool bolAnnounce, bool bolBtnMessage, bool bolCheckedAnnoun, bool bolCheckedUpdat, bool bolBtnUpdate, bool bolBtnExport, bool bolBtnDtFinalChange)
        {

            if (bolGrdConDt)
            {
                this.gridControlDt.Enabled = true;
            }
            else
            {
                this.gridControlDt.Enabled = false;
            }



        }



        private void manageDtAuthentication(string strRol)
        {

            DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();

            exportToExcelToolStripMenuItem.Visible = false;

            #region فعال سازی یا غیر فعال کردن صفحه دان تام بر اساس سطح دسترسی
            if (StrCurRol == string.Empty)
            {
                #region  غیرفعال کردن منوهای مدیریتی
                exportToExcelToolStripMenuItem.Visible = false;
                آپلودفایلپیوستToolStripMenuItem.Visible = false;
                toolStripMenuItemCreateAttach.Visible = false;

                //انتخابToolStripMenuItem.Visible = false;
                ویرایشوحذفToolStripMenuItem.Visible = false;
                ایجادمشابهToolStripMenuItem.Visible = false;
                ایجادمشابهToolStripMenuItem.Visible = false;
                اعلامصوتیToolStripMenuItem.Visible = false;
                backUpToolStripMenuItem.Visible = false;
                درخواستدانتایمToolStripMenuItem.Visible = false;
                کپیToolStripMenuItem.Visible=false;
                مدیریتToolStripMenuItem.Visible = false;
                // آپلودفایلپیوستToolStripMenuItem.Visible = false;
                //toolStripMenuItem5.Visible = false;
                toolStripMenuItemNew.Visible = false;
                toolStripMenuItemStatistics.Visible = false;
                #endregion

                resetDtToDefaultMode();

            }
            else if (StrCurRol == "register" || StrCurRol == "k_masool" || StrCurRol == "ts")
            {
                #region  فعال کردن منوهای مدیریتی
                exportToExcelToolStripMenuItem.Visible = false;
                آپلودفایلپیوستToolStripMenuItem.Visible = true;
                toolStripMenuItemCreateAttach.Visible = false;
                ////انتخابToolStripMenuItem.Visible = false;
                ویرایشوحذفToolStripMenuItem.Visible = false;
                ایجادمشابهToolStripMenuItem.Visible = false;
                ایجادمشابهToolStripMenuItem.Visible = false;
                اعلامصوتیToolStripMenuItem.Visible = false;
                backUpToolStripMenuItem.Visible = false;
                درخواستدانتایمToolStripMenuItem.Visible = true;
                مدیریتToolStripMenuItem.Visible = true;
                toolStripMenuItemNew.Visible = false;
                //toolStripMenuItem5.Visible = false;
                toolStripMenuItemStatistics.Visible = false;
                #endregion


            }
            else if (StrCurRol == "pm_requester")
            {
                #region  فعال کردن منوهای مدیریتی
                exportToExcelToolStripMenuItem.Visible = false;
                آپلودفایلپیوستToolStripMenuItem.Visible = false;
                toolStripMenuItemCreateAttach.Visible = false;
                ////انتخابToolStripMenuItem.Visible = false;
                ویرایشوحذفToolStripMenuItem.Visible = false;
                ایجادمشابهToolStripMenuItem.Visible = false;
                ایجادمشابهToolStripMenuItem.Visible = false;
                اعلامصوتیToolStripMenuItem.Visible = false;
                backUpToolStripMenuItem.Visible = false;
                درخواستدانتایمToolStripMenuItem.Visible = true;
                مدیریتToolStripMenuItem.Visible = true;
                toolStripMenuItemNew.Visible = false;
                // toolStripMenuItem5.Visible = false;
                toolStripMenuItemStatistics.Visible = false;

                #endregion
            }

            else if (StrCurRol == "administrator" || StrCurRol == "PowerUser")
            {


                #region  فعال کردن منوهای مدیریتی
                exportToExcelToolStripMenuItem.Visible = true;
                آپلودفایلپیوستToolStripMenuItem.Visible = true;
                toolStripMenuItemCreateAttach.Visible = true;
                //انتخابToolStripMenuItem.Visible = true;
                ویرایشوحذفToolStripMenuItem.Visible = true;
                ایجادمشابهToolStripMenuItem.Visible = true;
                ایجادمشابهToolStripMenuItem.Visible = true;
                اعلامصوتیToolStripMenuItem.Visible = true;
                backUpToolStripMenuItem.Visible = true;
                درخواستدانتایمToolStripMenuItem.Visible = true;
                مدیریتToolStripMenuItem.Visible = true;
                toolStripMenuItemNew.Visible = true;
                // toolStripMenuItem5.Visible = true;
                toolStripMenuItemStatistics.Visible = true;
                کپیToolStripMenuItem.Visible=true;


                #endregion

            }
            else
            {
                exportToExcelToolStripMenuItem.Visible = true;
                toolStripMenuItemStatistics.Visible = false;
            }


            #endregion

        }

        private void FilterBycodeAndSubjectInCurrentMonth(string codeSite, string subject)//همه دان تایمها در ماه اخیر که یادداشت کنسل را نداشته باشند با این موضوع و کد را نشان می دهذد
        {
            string strCurreentMonth = StrSelectedPershianDateWithSlashSeperator.Substring(0, 7);
            gridViewDt.Columns["تاریخ شروع"].FilterInfo = new ColumnFilterInfo("[تاریخ شروع] LIKE '" + strCurreentMonth + "%'");
            gridViewDt.Columns["کد سایت"].FilterInfo = new ColumnFilterInfo("[کد سایت] LIKE '" + codeSite + "'");
            gridViewDt.Columns["موضوع"].FilterInfo = new ColumnFilterInfo("[موضوع] LIKE '" + subject + "'");
            gridViewDt.Columns["یادداشت"].FilterInfo = new ColumnFilterInfo("[یادداشت] Not LIKE '%کنسل%'");
        }

        private bool HaveAnyFilterInDownTimeGrid()
        {
            if (bolDtHaveRunTimeFilter ||
                bolDtHaveWaitForClarityFilter ||
                bolDtHaveWaitForSessionFilter ||
                bolDtRegInClarity ||
                bolDtRegByUser ||
                bolDtInProcessingRequest ||
                bolDtShowCurrentMonth ||
                bolDtFinalReviw ||
                bolDtNeedToCheck
              )
            {

                return (true);
            }
            else
                return (false);
        }

        #endregion

        #region  Sub Menu

        DXMenuItem CreateRowSubMenu(GridView view, int rowHandle)
        {
            DXSubMenuItem subMenu = new DXSubMenuItem(" لطفا مورد درخواستی را کلیک کنید");
            subMenu.GetIsRightToLeft();
            subMenu.Items.IsRightToLeft = true;

           



            DXMenuItem menuItemEditRow = new DXMenuItem("&ویرایش و حذف ردیف یا ردیفهای انتخابی", new EventHandler(OnEditRowClick), imageCollection1.Images[1]);

            subMenu.Items.Add(menuItemEditRow);

            #region  کپی کردن سلول با راست کلیک

            DXMenuItem menuItemCopyThisCell = new DXMenuItem("&کپی محتوای این خانه به حافظه موقت", new EventHandler(OnCopyCellClick), imageCollection1.Images[2]);
            subMenu.Items.Add(menuItemCopyThisCell);




            void OnCopyCellClick(object sender, EventArgs e)
            {
                if (view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn) != null && view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString() != String.Empty)
                    Clipboard.SetText(view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString());
                else
                    MessageBox.Show("! خانه انتخاب شده خالی است");
            }

            #endregion

            #region  کپی سری کدسایتها به حافظه موقت 

            DXMenuItem menuItemCopyArrayOfCodeSite = new DXMenuItem("&کپی کد سایتهای منتخب به حافظه موقت", new EventHandler(OnCopycODEsITEcLICKClick), imageCollection1.Images[5]);
            subMenu.Items.Add(menuItemCopyArrayOfCodeSite);

            lstSelectedCodeSitesForClipBoard = new List<string>();

            void OnCopycODEsITEcLICKClick(object sender, EventArgs e)
            {
                if (view.SelectedRowsCount > 1)
                {


                    for (int i = 0; i < gridViewDt.SelectedRowsCount; i++)
                    {
                        lstSelectedCodeSitesForClipBoard.Add(gridViewDt.GetRowCellValue(gridViewDt.GetSelectedRows()[i], "کد سایت").ToString().Replace("XX", ""));
                    }
                    List<string> lstDistinctSelectedCodeSitesForClipBoard = lstSelectedCodeSitesForClipBoard.Distinct().ToList();
                    string strJoinedLst = string.Join(",", lstDistinctSelectedCodeSitesForClipBoard);


                    Clipboard.SetText(strJoinedLst);
                    string strMsg = lstDistinctSelectedCodeSitesForClipBoard.Count + " عدد کدسایت بدون تکرار در کلیپ برد کپی شد  " + Environment.NewLine + strJoinedLst;
                    MessageBox.Show(strMsg, "کپی کدسایتهای انتخابی بدون تکرار", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                }
                else
                {
                    MessageBox.Show("!فقط یک سطر انتخاب شده و سری قابل ساخت نیست لطفا حداقل دو سطر انتخاب کنید", "کپی کدسایتهای انتخابی بدون تکرار", MessageBoxButtons.OK,
                   MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }
            }


            #endregion


            #region  کپی سری موضوعات به حافظه موقت 

            DXMenuItem menuItemCopyArrayOfSubjects = new DXMenuItem("&کپی موضوعات منتخب به حافظه موقت", new EventHandler(OnCopySubjectsClick), imageCollection1.Images[7]);
            subMenu.Items.Add(menuItemCopyArrayOfSubjects);

            lstSelectedSubjectesForClipBoard = new List<string>();

            void OnCopySubjectsClick(object sender, EventArgs e)
            {
                if (view.SelectedRowsCount >= 1)
                {


                    for (int i = 0; i < gridViewDt.SelectedRowsCount; i++)
                    {
                        lstSelectedSubjectesForClipBoard.Add(gridViewDt.GetRowCellValue(gridViewDt.GetSelectedRows()[i], "موضوع").ToString());
                    }
                    //List<string> lstDistinctSelectedSubjectsForClipBoard = lstSelectedSubjectesForClipBoard.Distinct().ToList();
                    string strJoinedLst = string.Join("\r\n", lstSelectedSubjectesForClipBoard);


                    Clipboard.SetText(strJoinedLst);
                    string strMsg = lstSelectedSubjectesForClipBoard.Count + " عدد موضوع  تکراری و غیر تکراری در کلیپ برد کپی شد . " + Environment.NewLine + Environment.NewLine + strJoinedLst;
                    MessageBox.Show(strMsg, "کپی موضوعات انتخابی ", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                }
                else
                {
                   // MessageBox.Show("!فقط یک سطر انتخاب شده و سری قابل ساخت نیست لطفا حداقل دو سطر انتخاب کنید", "کپی موضوعات انتخابی ", MessageBoxButtons.OK,
                   //MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                   //MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }
            }


            #endregion

            #region پیدا کردن شماره ردیف آخر
            string strLastRowInDtTable = objClsSql.FindFieldInLastRecord("[DT].[dbo].tblDownTimeShort", "row", "row");
            if (StrCurRowInDtGrid == strLastRowInDtTable)
            {
                DXMenuItem menuItemDeleteFromDataBase = new DXMenuItem("&حذف از دیتا بیس", new EventHandler(OnDeleteFromDb), imageCollection1.Images[0]);
                subMenu.Items.Add(menuItemDeleteFromDataBase);
            }

            #endregion

            bool bolIsRowWithAttachedFile = false;

            for (int i = 0; i < DrRowNumberAttachFile.Length; i++)
            {
                if (DrRowNumberAttachFile[i].ItemArray[0].ToString() == StrCurRowInDtGrid)
                {
                    bolIsRowWithAttachedFile = true;
                }
            }


            if (bolIsRowWithAttachedFile)
            {
                DXMenuItem menuItemViewAttachedFile = new DXMenuItem("&دانلود فایل پیوست", new EventHandler(OnViewAttchedFile), imageCollection1.Images[2]);
                subMenu.Items.Add(menuItemViewAttachedFile);
                menuItemViewAttachedFile.IsRightToLeft=false;
            }

            bool bolIsThisRowRepeatedRow = objClsSql.IsAnyRecord("[DT].[dbo].[tblRepetedDt]", "WHERE [repeatedRow]=N'" + StrCurRowInDtGrid + "'");
            if (bolIsThisRowRepeatedRow)
            {
                DXMenuItem menuItemRepeatedRow = new DXMenuItem("&نمایش  موارد مشابه کنسل نشده", new EventHandler(OnRepeatedClick), imageCollection1.Images[6]);

                subMenu.Items.Add(menuItemRepeatedRow);
            }

            return subMenu;
        }




        void OnEditRowClick(object sender, EventArgs e)
        {
            ویرایشوحذفToolStripMenuItem.PerformClick();
        }

        void OnViewAttchedFile(object sender, EventArgs e)
        {

          

            try
            {
                string strShareDirctoryNameFor = @"\\" + "172.16.3.116" + @"\PE_Share\DtAttachFile\";//ساخت نام جدید فایل با استفاده از ردیف
                string[] arrFilePaths = Directory.GetFiles(strShareDirctoryNameFor);

                var results = Array.FindAll(arrFilePaths, s => s.Contains(StrCurRowInDtGrid));

                //System.Diagnostics.Process.Start(results[0]); پیش تر با این خط فایل رو باز می کردیم
                //اینجا فایل باید به دسکتاپ کپی بشه

                //پیدا کردن کدسایت

                DataRow dr = gridViewDt.GetDataRow(gridViewDt.FocusedRowHandle);
                string strSelectedCodeSite = dr["کد سایت"].ToString();

                string strPathAttachedFile = results[0];
                
                DownloadToDesktop(strPathAttachedFile,  strSelectedCodeSite);


            }
            catch
            {
                MessageBox.Show("را بررسی نمایید DtAttachFile  فایل قابل اجرا با این شماره ردیف پیدا نشد . لطفا شاخه به اشتراک گذاشته شده در سرور به نام ");
            }

        }



        public static void DownloadToDesktop(string fileUrl, string fileName)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string extension = Path.GetExtension(fileUrl);
                string fullPath = System.IO.Path.Combine(desktopPath, fileName);
             

                using (WebClient client = new WebClient())
                {
                    // نمایش پیشرفت دانلود در MessageBox
                    client.DownloadProgressChanged += (s, e) =>
                    {
                        // برای نمایش پیشرفت در MessageBox (اختیاری)
                        // MessageBox.Show($"دانلود شده: {e.ProgressPercentage}%", "وضعیت دانلود", 
                        //     MessageBoxButtons.OK, MessageBoxIcon.Information);
                    };

                    client.DownloadFileCompleted += (s, e) =>
                    {
                        if (e.Error == null)
                        {
                            MessageBox.Show(
                                $"فایل با موفقیت در آدرس زیر ذخیره شد:\n{fullPath+extension}",
                                "دانلود کامل",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        else
                        {
                            MessageBox.Show(
                                $"خطا در دانلود:\n{e.Error.Message}",
                                "خطا",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    };

                    client.DownloadFileAsync(new Uri(fileUrl), fullPath+extension);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"خطای غیرمنتظره:\n{ex.Message}",
                    "خطای سیستم",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        void OnDeleteFromDb(object sender, EventArgs e)
        {
            intDtTopRowInGrid = gridViewDt.TopRowIndex;
            DeleteDtFromDb();
            RefreshDtAndDontGoToEnd();

        }

        public void DeleteDtFromDb()
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(" برای حذف ردیف   " + StrCurRowInDtGrid + "مطمئن هستید  ؟  ", "تاییدیه حذف  ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    bool bolDelRslt = objClsSql.SendCommand(@" Delete From DT.dbo.tblDownTimeShort Where row=N'" + StrCurRowInDtGrid + "'");


                    MessageBox.Show(" ردیف آخر با موفقیت از دیتابیس حذف گردید ");
                    ClsDiff.writeLogOfAction(" ردیف   " + StrCurRowInDtGrid + "از دیتابیس دان تایم حذف گردید ");


                    bolDtForceToRefreshAndGoToEnd = true;
                    bolDtForceToRefreshAndDontGoToEnd = false;

                }
                else
                {

                }


            }
            catch
            {

                MessageBox.Show("حذف دان تایم از دیتابیس  اس کیو ال یا مای اس کیو ال موفقیت آمیز نبود موفقیت آمیز نبود");
            }
        }

        private void OnRepeatedClick(object sender, EventArgs e)
        {
            DataRow dr = gridViewDt.GetDataRow(gridViewDt.FocusedRowHandle);

            string strSelectedSubject = dr["موضوع"].ToString();
            string strSelectedCodeSite = dr["کد سایت"].ToString();

            FilterBycodeAndSubjectInCurrentMonth(strSelectedCodeSite, strSelectedSubject);
        }
        #endregion

        #region ToolTip

        private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {

            bool validColumnForStartTime = false;

            bool validColumnFordate = false;
            bool validColumnForRowColomn = false;

            if (e.SelectedControl != gridControlDt)
                return;

            GridHitInfo hitInfo = gridViewDt.CalcHitInfo(e.ControlMousePosition);

            if (hitInfo.InRow == false)
                return;

            if (hitInfo.Column == null)
                return;

            //concern only the following fields
            if (hitInfo.Column.FieldName == "تاریخ شروع" || hitInfo.Column.FieldName == "تاریخ پایان")
                validColumnFordate = true;
            if (hitInfo.Column.FieldName == "زمان شروع" || hitInfo.Column.FieldName == "زمان پایان")
            {
                validColumnForStartTime = true;

                SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                toolTipArgs.Title.Text = string.Empty;

                //Get the data from this row

                int row = hitInfo.RowHandle;
                if (row < 0)
                    return;


                string strRow = gridViewDt.GetRowCellValue(row, "ردیف").ToString();

                string strOutage = objClsSql.SendCommandGetStringResult("Select outageMin from Dt.dbo.tblDownTimeShort Where row=N'" + strRow + "'");
                toolTipArgs.Contents.Text = strOutage;
                e.Info = new ToolTipControlInfo();
                e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
                e.Info.ToolTipType = ToolTipType.SuperTip;
                e.Info.SuperTip = new SuperToolTip();
                e.Info.SuperTip.Setup(toolTipArgs);

            }

            if (gridViewDt.SelectedRowsCount > 1)
                validColumnForRowColomn = true;

            if (!validColumnFordate && !validColumnForRowColomn)
                return;

            string strFieldName = hitInfo.Column.FieldName;

            if (validColumnFordate)
            {
                SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                toolTipArgs.Title.Text = string.Empty;

                //Get the data from this row

                int row = hitInfo.RowHandle;
                if (row < 0)
                    return;

                string strDateValue = gridViewDt.GetRowCellValue(row, strFieldName).ToString();

                if (strDateValue == "0000/00/00")
                    return;
                //convert to miladi

                DateTime dtMiladi = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(strDateValue);
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(string.Empty);
                string FormatDate = dtMiladi.ToString("dddd , dd  MMM  yyyy", culture);

                // Add Notes &description for the tool-tip

                toolTipArgs.Contents.Text = FormatDate;

                e.Info = new ToolTipControlInfo();
                e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
                e.Info.ToolTipType = ToolTipType.SuperTip;
                e.Info.SuperTip = new SuperToolTip();
                e.Info.SuperTip.Setup(toolTipArgs);
            }
            else
            if (validColumnForRowColomn && strFieldName == "ردیف")
            {
                SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                toolTipArgs.Title.Text = string.Empty;

                toolTipArgs.Contents.Text = " تعداد دان تایم انتخاب شده " + gridViewDt.SelectedRowsCount.ToString() + " عدد می باشد ";

                e.Info = new ToolTipControlInfo();
                e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
                e.Info.ToolTipType = ToolTipType.SuperTip;
                e.Info.SuperTip = new SuperToolTip();
                e.Info.SuperTip.Setup(toolTipArgs);

            }

            if (validColumnForStartTime)
            {
                SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
                toolTipArgs.Title.Text = string.Empty;
                e.Info = new ToolTipControlInfo();
                e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();
                e.Info.ToolTipType = ToolTipType.SuperTip;
                e.Info.SuperTip = new SuperToolTip();
                e.Info.SuperTip.Setup(toolTipArgs);
            }

        }

        #endregion

        #region menuStrip
        #region menuStripDtPublic

        private void درخواستدانتایمToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ClsSql clsSql = new ClsSql();
            lblStatusBar.Text = objClsSql.SetDateForStatusBar();

            string strSql = "SELECT TOP 1 [row] FROM [DT].[dbo].[tblDownTimeShort] order By row desc";
            strDtLastRowOfDownTime = objClsSql.SendCommandGetStringResult(strSql);

            if (MainForm.StrCurRol != "administrator")
            {
               
                string strSetedTime = clsSql.SendCommandGetStringResult("Select [lastPermitTimeForDownTime] from DT.dbo.tblLastPermitTimeForDt  where id='1'").Substring(0,4);

                int intSetedLinitTime = int.Parse(strSetedTime.Replace(":", string.Empty));

                int intCurTime = int.Parse(MainForm.DateTimeSqlServer.ToShortTimeString().Replace(":", string.Empty));

                if (intCurTime > intSetedLinitTime)
                {

                    string strMsg = @" کاربر گرامی به منظور تکمیل پروسه دان تایمهای درخواستی امروز پس از ساعت  " + "\r\n" + strSetedTime + "\r\n" + " سیستم قادر به پذیرش درخواست جدید دان تایم  نمی باشد  " + "\r\n" + " لطفا در روز اداری بعدی اقدام به ثبت درخواست کنید ";
                    MessageBox.Show(strMsg, "توجه", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    return;

                }

            }
            new frmDtRegDownTime().ShowDialog();

            RefreshDtAndGoToEnd();

        }


        [Obsolete]
        private void gridViewDt_CustomDrawFooter(object sender, RowObjectCustomDrawEventArgs e)
        {
            string strMessageForTypeOfFilter = string.Empty;

            if (bolDtHaveRunTimeFilter)
            {
                strMessageForTypeOfFilter = "فیلتر : در حال اجرا";
            }
            if (bolDtRegByUser)
            {
                strMessageForTypeOfFilter = "فیلتر : ثبت توسط کاربر";
            }
            if (bolDtHaveWaitForSessionFilter)
            {
                strMessageForTypeOfFilter = "فیلتر : منتظر ارائه در جلسه";
            }
            if (bolDtRegInClarity)
            {
                strMessageForTypeOfFilter = "فیلتر : ثبت در (SDM)";
            }
            if (bolDtInProcessingRequest)
            {
                strMessageForTypeOfFilter = "فیلتر : در حال پردازش درخواست";
            }
            if (bolDtRepeated)
            {
                strMessageForTypeOfFilter = "فیلتر : تکراری ها";
            }
            if (bolDtNeedToCheck)
            {
                strMessageForTypeOfFilter = "فیلتر : نیاز به بررسی";
            }
            if (bolDtFinalReviw)
            {
                strMessageForTypeOfFilter = "فیلتر : بررسی نهایی";
            }

            #region اعلام تعداد فیلتر ستونی
            int intCountOfcolumnFiltered = gridViewDt.FilteredColumnsCount;

            switch (intCountOfcolumnFiltered)
            {
                case 1:
                    strMessageForTypeOfFilter += "\r\n" + intCountOfcolumnFiltered + "  filtered column" + "  ";

                    break;
                default:
                    if (intCountOfcolumnFiltered > 1)
                    {
                        strMessageForTypeOfFilter += "\r\n" + intCountOfcolumnFiltered + "  filtered columns" + "  ";

                    }

                    break;
            }
            #endregion

            writeStringUnderGridviw(e, strMessageForTypeOfFilter);
        }

        private static void writeStringUnderGridviw(RowObjectCustomDrawEventArgs e, string strMessageForTypeOfFilter)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            var rect = e.Bounds;
            rect.X += 10;
            e.DefaultDraw();
            using (System.Drawing.Font f = new System.Drawing.Font(e.Appearance.GetFont(), FontStyle.Bold))
                e.Cache.DrawString(strMessageForTypeOfFilter, f, Brushes.Red, rect, stringFormat);
            e.Handled = true;
        }
        #endregion

        #region منو استریپ فیلترها

        private void آپلودفایلپیوستToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            frmDtAttachFile objFrmDtAttachFile = new frmDtAttachFile();
            objFrmDtAttachFile.Show();

        }

        private void ویرایشوحذفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            intDtLastSelectedRow = int.Parse(strCurRowInDtGrid);
            intDtTopRowInGrid = gridViewDt.TopRowIndex;

            if (StrCurRol != "administrator" && StrCurRol != "PowerUser")
            {
                return;
            }
            if (gridViewDt.SelectedRowsCount == 0)
            {
                MessageBox.Show("هیچ دان تایمی جهت ویرایش  انتخاب نشده. لطفا مجددا تلاش کنید");
                return;
            }

            intDtLastSelectedRow = int.Parse(strCurRowInDtGrid);
            intDtTopRowInGrid = gridViewDt.TopRowIndex;

            if (DtSelectedDt.Columns.Count == 0)
            {
                DtSelectedDt.Columns.Add("ردیف", typeof(int));
                DtSelectedDt.Columns.Add("تاریخ ثبت", typeof(string));
                DtSelectedDt.Columns.Add("ثبت", typeof(string));
                DtSelectedDt.Columns.Add("کد سایت", typeof(string));
                DtSelectedDt.Columns.Add("نام سایت", typeof(string));
                DtSelectedDt.Columns.Add("منطقه", typeof(string));
                DtSelectedDt.Columns.Add("تاریخ شروع", typeof(string));
                DtSelectedDt.Columns.Add("زمان شروع", typeof(string));
                DtSelectedDt.Columns.Add("تاریخ پایان", typeof(string));
                DtSelectedDt.Columns.Add("زمان پایان", typeof(string));
                DtSelectedDt.Columns.Add("درخواست کننده", typeof(string));
                DtSelectedDt.Columns.Add("موضوع", typeof(string));
                DtSelectedDt.Columns.Add("انجام دهنده", typeof(string));
                DtSelectedDt.Columns.Add("ثبت کننده", typeof(string));
                DtSelectedDt.Columns.Add("شماره", typeof(string));
                DtSelectedDt.Columns.Add("وضعیت", typeof(string));
                DtSelectedDt.Columns.Add("یادداشت", typeof(string));
                DtSelectedDt.Columns.Add("فعالیت", typeof(string));
               



            }
            else
            {
                DtSelectedDt.Clear();
            }


            StrDtChgGrpSelRows = string.Empty;

            for (int i = 0; i < gridViewDt.SelectedRowsCount; i++)
            {

                //if (gridViewDt.GetSelectedRows()[i] >= 0)
                //{
                DtSelectedDt.Rows.Add(gridViewDt.GetDataRow(gridViewDt.GetSelectedRows()[i]).ItemArray);

                if (gridViewDt.SelectedRowsCount > 1 && i < gridViewDt.SelectedRowsCount - 1)
                    StrDtChgGrpSelRows += gridViewDt.GetRowCellValue(gridViewDt.GetSelectedRows()[i], "ردیف") + "' OR row=N'";
                else
                    StrDtChgGrpSelRows += gridViewDt.GetRowCellValue(gridViewDt.GetSelectedRows()[i], "ردیف");


                //}
            }


            frmDtMultiChange frmMultiChange = new frmDtMultiChange();

            frmMultiChange.ShowDialog();

            gridViewDt.ClearSelection();

            RefreshDtAndDontGoToEnd();

        }

        private void پانزدهروزاخیرToolStripMenuItem_Click(object sender, EventArgs e)
        {
            پانزدهروزاخیرToolStripMenuItem.Checked = true;
            ماهاخیرToolStripMenuItem.Checked = false;
            دوماهاخیرToolStripMenuItem.Checked = false;
            همهToolStripMenuItem.Checked = false;

            bolDtShowCurrentMonth = true;
            bolDtForceToRefreshAndGoToEnd = true;
            bolDtForceToRefreshAndDontGoToEnd = false;

            RefreshDtAndGoToEnd();

        }

        private void همهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            پانزدهروزاخیرToolStripMenuItem.Checked = false;
            ماهاخیرToolStripMenuItem.Checked = false;
            دوماهاخیرToolStripMenuItem.Checked = false;
            همهToolStripMenuItem.Checked = true;

            bolDtForceToRefreshAndGoToEnd = true;
            bolDtForceToRefreshAndDontGoToEnd = false;
            bolDtShowCurrentMonth = false;

            RefreshDtAndGoToEnd();

        }

        private void فیلترهاToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BolHavePermitRefresh = true;
            gridViewDt.ClearSelection();
        }

        private void حذفهمهفیلترهاToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridViewDt.ActiveFilter.Clear();

            bolDtHaveRunTimeFilter = false;
            bolDtRegByUser = false;
            bolDtHaveWaitForSessionFilter = false;
            bolDtRegInClarity = false;
            bolDtInProcessingRequest = false;
            bolDtRepeated = false;
            bolDtPakage = false;
            bolDtNeedToCheck = false;
            bolDtFinalReviw = false;

            درحالاجراToolStripMenuItem1.Checked = false;
            ثبتتوسطکاربرToolStripMenuItem.Checked = false;
            منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
            ثبتدرکلاریتیToolStripMenuItem.Checked = false;
            درحالپردازشدرخواستToolStripMenuItem.Checked = false;
            تکراریToolStripMenuItem.Checked = false;
            پکیجToolStripMenuItem.Checked = false;
            نیازبهبررسیToolStripMenuItem.Checked = false;
            بررسینهاییToolStripMenuItem.Checked = false;

            gridViewDt.OptionsView.ShowFooter = true;


            RefreshDtAndGoToEnd();

        }

        private void درحالاجراToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (درحالاجراToolStripMenuItem1.Checked)
            {
                درحالاجراToolStripMenuItem1.Checked = false;
                bolDtHaveRunTimeFilter = false;
            }
            else
            {
                gridViewDt.OptionsView.ShowFooter = true;
              

                bolDtHaveRunTimeFilter = true;
                bolDtRegByUser = false;
                bolDtHaveWaitForSessionFilter = false;
                bolDtRegInClarity = false;
                bolDtInProcessingRequest = false;
                bolDtRepeated = false;
                bolDtPakage = false;
                bolDtNeedToCheck = false;
                bolDtFinalReviw = false;

                درحالاجراToolStripMenuItem1.Checked = true;
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                تکراریToolStripMenuItem.Checked = false;
                پکیجToolStripMenuItem.Checked = false;
                نیازبهبررسیToolStripMenuItem.Checked = false;
                بررسینهاییToolStripMenuItem.Checked = false;

            }

            RefreshDtAndGoToEnd();

        }

        private void ثبتتوسطکاربرToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ثبتتوسطکاربرToolStripMenuItem.Checked)
            {
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                bolDtRegByUser = false;
            }
            else
            {
                gridViewDt.OptionsView.ShowFooter = true;


                bolDtHaveRunTimeFilter = false;
                bolDtRegByUser = true;
                bolDtHaveWaitForSessionFilter = false;
                bolDtRegInClarity = false;
                bolDtInProcessingRequest = false;
                bolDtRepeated = false;
                bolDtPakage = false;
                bolDtNeedToCheck = false;
                bolDtFinalReviw = false;

                درحالاجراToolStripMenuItem1.Checked = false;
                ثبتتوسطکاربرToolStripMenuItem.Checked = true;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                تکراریToolStripMenuItem.Checked = false;
                پکیجToolStripMenuItem.Checked = false;
                نیازبهبررسیToolStripMenuItem.Checked = false;
                بررسینهاییToolStripMenuItem.Checked = false;
            }

            RefreshDtAndGoToEnd();


        }

        private void منتظرارائهدرجلسهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (منتظرارائهدرجلسهToolStripMenuItem.Checked)
            {
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                bolDtHaveWaitForSessionFilter = false;
            }
            else
            {
                gridViewDt.OptionsView.ShowFooter = true;


                bolDtHaveRunTimeFilter = false;
                bolDtRegByUser = false;
                bolDtHaveWaitForSessionFilter = true;
                bolDtRegInClarity = false;
                bolDtInProcessingRequest = false;
                bolDtRepeated = false;
                bolDtPakage = false;
                bolDtNeedToCheck = false;
                bolDtFinalReviw = false;

                درحالاجراToolStripMenuItem1.Checked = false;
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = true;
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                تکراریToolStripMenuItem.Checked = false;
                پکیجToolStripMenuItem.Checked = false;
                نیازبهبررسیToolStripMenuItem.Checked = false;
                بررسینهاییToolStripMenuItem.Checked = false;


            }
            RefreshDtAndGoToEnd();
        }

        private void ثبتدرکلاریتیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ثبتدرکلاریتیToolStripMenuItem.Checked)
            {
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;           
                string s = gridViewDt.ActiveFilterString;                
            }
            else
            {

                gridViewDt.OptionsView.ShowFooter = true;


                bolDtHaveRunTimeFilter = false;
                bolDtRegByUser = false;
                bolDtHaveWaitForSessionFilter = false;
                bolDtRegInClarity = true;
                bolDtInProcessingRequest = false;
                bolDtRepeated = false;
                bolDtPakage = false;
                bolDtNeedToCheck = false;
                bolDtFinalReviw = false;

                درحالاجراToolStripMenuItem1.Checked = false;
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                ثبتدرکلاریتیToolStripMenuItem.Checked = true;
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                تکراریToolStripMenuItem.Checked = false;
                پکیجToolStripMenuItem.Checked = false;
                نیازبهبررسیToolStripMenuItem.Checked = false;
                بررسینهاییToolStripMenuItem.Checked = false;
            }
            RefreshDtAndGoToEnd();
        }

        private void درحالپردازشدرخواستToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (درحالپردازشدرخواستToolStripMenuItem.Checked)
            {
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                bolDtInProcessingRequest = false;
            }
            else
            {
                gridViewDt.OptionsView.ShowFooter = true;


                bolDtHaveRunTimeFilter = false;
                bolDtRegByUser = false;
                bolDtHaveWaitForSessionFilter = false;
                bolDtRegInClarity = false;
                bolDtInProcessingRequest = true;
                bolDtRepeated = false;
                bolDtPakage = false;
                bolDtNeedToCheck = false;
                bolDtFinalReviw = false;

                درحالاجراToolStripMenuItem1.Checked = false;
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;
                درحالپردازشدرخواستToolStripMenuItem.Checked = true;
                تکراریToolStripMenuItem.Checked = false;
                پکیجToolStripMenuItem.Checked = false;
                نیازبهبررسیToolStripMenuItem.Checked = false;
                بررسینهاییToolStripMenuItem.Checked = false;
            }
            RefreshDtAndGoToEnd();
        }

        private void تکراریToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (تکراریToolStripMenuItem.Checked)
            {
                تکراریToolStripMenuItem.Checked = false;
                bolDtInProcessingRequest = false;
            }
            else
            {
                gridViewDt.OptionsView.ShowFooter = true;


                bolDtHaveRunTimeFilter = false;
                bolDtRegByUser = false;
                bolDtHaveWaitForSessionFilter = false;
                bolDtRegInClarity = false;
                bolDtInProcessingRequest = false;
                bolDtRepeated = true;
                bolDtPakage = false;
                bolDtNeedToCheck = false;
                bolDtFinalReviw = false;

                درحالاجراToolStripMenuItem1.Checked = false;
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                تکراریToolStripMenuItem.Checked = true;
                پکیجToolStripMenuItem.Checked = false;
                نیازبهبررسیToolStripMenuItem.Checked = false;
                بررسینهاییToolStripMenuItem.Checked = false;
            }
            RefreshDtAndGoToEnd();
        }

        #endregion

        #region menuStripDtPowerUsers

        private void تنظیمپیامحالتغیرفریزشبکهToolStripMenuItem_Click_1(object sender, EventArgs e)
        {


            frmDtSetTextAboveGrid objFrmDtSetTextAboveGrid = new frmDtSetTextAboveGrid();
            objFrmDtSetTextAboveGrid.ShowDialog();

            RefreshDtAndGoToEnd();

        }


        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            frmDtCteateAttachment objFrmDtCteateAttachment = new frmDtCteateAttachment();
            objFrmDtCteateAttachment.Show();
        }
        #endregion

        #endregion

        #region نحوه رفرش




        private void همهToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExportAllOrOnlySelectedRows(false);
        }
        #endregion

        private void btnDtOkDeselect_MouseHover(object sender, EventArgs e)
        {
            BolHavePermitRefresh = true;
            RefreshDtAndDontGoToEnd();

        }

        private void سطرهایانتخابیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportAllOrOnlySelectedRows(true);
        }
        private void ExportAllOrOnlySelectedRows(bool bolJustSelectedRows)
        {

            if (bolJustSelectedRows)
            {
                gridViewDt.OptionsPrint.PrintSelectedRowsOnly = true;
            }
            else
            {
                gridViewDt.OptionsPrint.PrintSelectedRowsOnly = false;
            }

            #region بررسی اینکه تعداد نمایش فیلتر نشده باشد

            if (پانزدهروزاخیرToolStripMenuItem.Checked && !bolJustSelectedRows)
            {
                string strMessage = "کاربر گرامی در نظر داشته باشید که با توجه به فیلتر فعلی فقط دان تایمهای ماه اخیر اکسپورت می گردد";
                DialogResult result = MessageBox.Show(strMessage, "توجه", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign);

                if (result == DialogResult.Yes)
                {
                    #region Export To Xcl
                    exportToExcelToolStripMenuItem.Enabled = false;
                    this.saveFileDialogExpToXcl.ShowDialog();



                    if (this.saveFileDialogExpToXcl.FileName != string.Empty)
                    {

                        this.gridControlDt.ExportToXlsx(this.saveFileDialogExpToXcl.FileName);
                    }
                    else
                    {
                        MessageBox.Show("هیچ فایل انتخاب نشده");

                    }
                    exportToExcelToolStripMenuItem.Enabled = true;
                    #endregion
                }
                else if (result == DialogResult.No)
                {
                    return;
                }


            }
            else
            {
                #region Export To Xcl
                exportToExcelToolStripMenuItem.Enabled = false;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ES";
                //String StrSavableFileName = ;
                int[] selectedRows = gridViewDt.GetSelectedRows();
                int rowHandle = selectedRows[0];

                var cellValue = gridViewDt.GetRowCellValue(rowHandle, "کد سایت");
                path += cellValue.ToString().Substring(2, 4);

                saveFileDialogExpToXcl.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialogExpToXcl.FilterIndex = 0;
                saveFileDialogExpToXcl.RestoreDirectory = true;
                saveFileDialogExpToXcl.Title = "Export Excel File To";

                this.saveFileDialogExpToXcl.FileName = path + ".xlsx";
                this.saveFileDialogExpToXcl.ShowDialog();

                this.saveFileDialogExpToXcl.FileName = this.saveFileDialogExpToXcl.FileName;
                if (this.saveFileDialogExpToXcl.FileName != string.Empty)
                {
                    this.gridControlDt.ExportToXlsx(this.saveFileDialogExpToXcl.FileName);
                }
                else
                {
                    MessageBox.Show("هیچ فایل انتخاب نشده");

                }
                exportToExcelToolStripMenuItem.Enabled = true;
                #endregion
            }
            #endregion


        }

        private void حداکثرزمانمجازبرایدانتایمToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDtLimitTime objFrmDtLimitTime = new frmDtLimitTime();
            objFrmDtLimitTime.ShowDialog();
        }

        private void gridControlDt_ProcessGridKey(object sender, KeyEventArgs e)
        {//موقتا با دابل کلیک ثبت شماره دانم تایم به درخواست خانم محمدی حذف گردید

            //#region بررسی اینکه آیا این یوزر پاور یا ادمین هست که بتونه شماره دان تایم وارد کنه
            //if (strCurRol == "administrator" || strCurRol == "PowerUser")
            //{
            //    BolIsCurUserPowerOrAdmin = true;
            //}
            //else
            //{
            //    return;
            //}
            //#endregion

            //#region بررسی اینکه آیا شماره دان تایم خالیه اصلا که میخواهیم پرش کنیم
            //if (gridViewDt.SelectedRowsCount == 0)
            //{
            //    return;
            //}
            //string strCrNo = gridViewDt.GetFocusedDataRow()["شماره"].ToString();
            //if (strCrNo == "")
            //{
            //    BolIsEmptyCrNo = true;
            //}
            //else
            //{
            //    BolIsEmptyCrNo = false;
            //    return;
            //}
            //#endregion

            //#region اگه یکی از سه شرط  درسته 
            //if (BolIsDblClickedColCrNo || e.KeyCode == Keys.Enter || (e.KeyCode == Keys.V && e.Control))
            //{
            //    if (BolIsCurUserPowerOrAdmin && BolIsEmptyCrNo)//اگه یوزر قدرتمنده و فیلد شماره دان تایم خالیه

            //        InsertDtNo();//شماره دان تایم و نام ثبت کننده و وضعیت رو با توجه به کلیپ برد تکمیل کن
            //}
            //#endregion

        }


        private void gridViewDt_DoubleClick(object sender, EventArgs e)
        {
            //#region بررسی اینکه آیا سطر دابل کلیک شده شماره دان تام هست یا نه
            //DXMouseEventArgs ea = e as DXMouseEventArgs;
            //GridView view = sender as GridView;
            //GridHitInfo info = view.CalcHitInfo(ea.Location);
            //if (info.Column.ToString() == "شماره")
            //{
            //    BolIsDblClickedColCrNo = true;

            //    SendKeys.Send("~");
            //}
            //else
            //{
            //    BolIsDblClickedColCrNo = false;
            //}
            //#endregion
        }

        private void نمایشسهمیهتکراریشرکتهاToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDtShowRepStock().ShowDialog();
        }

        private void ایجادمشابهToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridViewDt.SelectedRowsCount == 0)
            {
                MessageBox.Show(" هیچ ردیفی برای ایجاد مشابه انتخاب نشده ");
                return;
            }

            if (DtSelectedDtForCreSim.Columns.Count == 0)
            {
                DtSelectedDtForCreSim.Columns.Add("ردیف", typeof(int));
                DtSelectedDtForCreSim.Columns.Add("تاریخ ثبت", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("ثبت", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("کد سایت", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("نام سایت", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("منطقه", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("تاریخ شروع", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("زمان شروع", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("تاریخ پایان", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("زمان پایان", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("درخواست کننده", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("موضوع", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("انجام دهنده", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("ثبت کننده", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("شماره", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("وضعیت", typeof(string));
                DtSelectedDtForCreSim.Columns.Add("یادداشت", typeof(string));
            }
            else
            {
                DtSelectedDtForCreSim.Clear();
            }

            ArrayList arrList = new ArrayList();

            if (gridViewDt.GetSelectedRows()[0] >= 0)
            {
                ایجادمشابهToolStripMenuItem.Enabled = false;

                DataRow selectedRow = gridViewDt.GetDataRow(gridViewDt.GetSelectedRows()[0]);
                object[] sourceValues = selectedRow.ItemArray;

                // اگر تعداد ستون‌های مبدأ بیشتر از مقصد است
                if (sourceValues.Length > DtSelectedDtForCreSim.Columns.Count)
                {
                    // روش 1: فقط ستون‌های مورد نیاز را کپی کنید
                    object[] destValues = new object[DtSelectedDtForCreSim.Columns.Count];
                    Array.Copy(sourceValues, destValues, DtSelectedDtForCreSim.Columns.Count);
                    DtSelectedDtForCreSim.Rows.Add(destValues);
                }
                else if (sourceValues.Length == DtSelectedDtForCreSim.Columns.Count)
                {
                    // تعداد ستون‌ها برابر است
                    DtSelectedDtForCreSim.Rows.Add(sourceValues);
                }
                else
                {
                    // اگر ستون‌های مقصد بیشتر است
                    DtSelectedDtForCreSim.Rows.Add(sourceValues);
                }
            }



            frmDtCreateSimilar objFrmCreSim = new frmDtCreateSimilar();
            objFrmCreSim.Show();

            ایجادمشابهToolStripMenuItem.Enabled = true;


        }

        private void اعلانفریزبودنشبکهToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (!اعلانفریزبودنشبکهToolStripMenuItem.Checked)
            {
                اعلانفریزبودنشبکهToolStripMenuItem.Checked = true;
                if (اعلانفریزبودنشبکهToolStripMenuItem.Checked)
                {
                    DialogResult dialogResult = MessageBox.Show("برای اعلام فریز بودن شبکه مطمئن هستید", " فریز بودن یا نبودن . مسئله اینست  ", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        frmDtFreezMessage objFrmDtFreezMessage = new frmDtFreezMessage();
                        objFrmDtFreezMessage.ShowDialog();

                        ClsDiff.writeLogOfAction("فریز بودن شبکه را اعلام کرد");

                        objClsSql.EditRecord("[DT].[dbo].[tblFreeze]", "isFreezeNetwork", true, string.Empty);

                        RefreshDtAndGoToEnd();

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        اعلانفریزبودنشبکهToolStripMenuItem.Checked = false;
                    }
                }

            }
            else if (اعلانفریزبودنشبکهToolStripMenuItem.Checked)
            {

                DialogResult dialogResult = MessageBox.Show("برای لغو اعلام فریز بودن شبکه مطمئن هستید", " فریز بودن یا نبودن . مسئله اینست  ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    اعلانفریزبودنشبکهToolStripMenuItem.Checked = false;
                    ClsDiff.writeLogOfAction(" لغو فریز  شبکه را اعلام کرد");
                    objClsSql.EditRecord("[DT].[dbo].[tblFreeze]", "isFreezeNetwork", false, string.Empty);

                    gridViewDt.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    gridViewDt.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Green;

                    RefreshDtAndGoToEnd();
                }
                else if (dialogResult == DialogResult.No)
                {

                }

            }


        }

        private void backUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();

            string strDateTimeSqlserve = DateTimeSqlServer.ToString().Replace("/", "-").Replace(":", "_");

            string strCommandForDtBackUp = @"USE DT 
                    BACKUP DATABASE DT
                    TO DISK ='C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER2008\MSSQL\Backup\DT\DT_BackUp_ByPrg_" + strDateTimeSqlserve + @".BAK'";

            objClsSql.SendCommand(strCommandForDtBackUp);


            ClsDiff.writeLogOfAction("از دان تایمها بک آپ دستی گرفته شد");

            MessageBox.Show("Created BackUp DownTime Data Base in 10.84.77.58 _ " + @"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER2008\MSSQL\Backup\DT\\\DT_BackUp_ByPrg_" + strDateTimeSqlserve + @".BAK'");

        }



        private void RefreshDtAndGoToEnd()
        {
            RefreshDtGrid(true);
        }
        private void RefreshDtAndDontGoToEnd()
        {
            RefreshDtGrid(true);
        }

        private void gridViewDt_MouseWheel(object sender, MouseEventArgs e)
        {
            BolHavePermitRefresh = false;
        }

        private void gridViewDt_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            BolHavePermitRefresh = true;
        }

        #endregion

        #region DB
        private void excelToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.saveFileDialogExpToXcl.ShowDialog();

            if (this.saveFileDialogExpToXcl.FileName != string.Empty)
            {
                this.gridViewDbNew.ExportToXlsx(this.saveFileDialogExpToXcl.FileName);
            }
            else
            {
                MessageBox.Show("مقصد جهت ذخیره انتخاب نشده");
            }
        }

        private void sendRequestToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            new frmDbCheckChgRequest().ShowDialog();
        }

        private void viewRequestesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            new frmDbCheckChgRequest().ShowDialog();
        }

        private void newPlansToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmDbNewSite objFrmDbNewSite = new frmDbNewSite();
            objFrmDbNewSite.ShowDialog();
        }

        private void removedPlansToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmDbDeletedSite objFrmDbNDeletedSite = new frmDbDeletedSite();
            objFrmDbNDeletedSite.ShowDialog();
        }
        private void showOnMapsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DataRow dataRow = this.gridViewDbNew.GetDataRow(this.gridViewDbNew.FocusedRowHandle);
            StrDbLat = StrDbLong = string.Empty;

            if (dataRow != null)
            {

                StrDbLat = dataRow["عرض جغرافیایی"].ToString();
                StrDbLong = dataRow["طول جغرافیایی"].ToString();

            }

            if (StrDbLat == string.Empty || StrDbLong == string.Empty)
            {
                MessageBox.Show(" هیچ سایتی از دیتابیس انتخاب نشده . لطفا یکی از سایتها را انتخاب و از خالی نبودن مقدار طول و عرض جغرافیایی آن مطمئن شوید ");
            }
            else
            {
                frmDbMaps frmMap = new frmDbMaps();
                frmMap.ShowDialog();
            }
        }
        private void countToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            gridViewDbNew.CollapseAllGroups();
            int intCountOfRows = gridViewDbNew.RowCount;
            string strMsg = "CountGroupedRowsToolStripMenuItem Of Grouped Rows Is : " + intCountOfRows.ToString();
            MessageBox.Show(strMsg);
        }

        private void بهروزرسانیدیتابیسجدیدToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();

            #region باز کردن فرم مقایسه رسانی دیتا بیس برای بروز رسانی دیتابیس

            frmCompairAndUpdateDbSummary frmCompUpDbSumm = new frmCompairAndUpdateDbSummary();

            frmCompUpDbSumm.ShowDialog();
            #endregion

            #region نمایش در جدول دیتابیس
            if (IsNeedRefreshDbGrids())
            {
                RefreshNewDbGrid();
            }

            #endregion

            if (bolIsCompleteUpdateDb)
            {
                #region محاسبه و اعلام آخرین بروز رسانی ها
                toolStripLblDbNew.Text = CalculateLastUpDbTimeSummary();
                #endregion
            }
            else
            {

            }
        }

        private void cotractorCompaniToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            this.openFileDialog.ShowDialog();

            string fileName = this.openFileDialog.FileName;

            if (fileName == string.Empty)
            {
                MessageBox.Show("هیچ فایلی جهت بروز رسانی انتخاب نشده");
                return;
            }

            this.toolStripLblDbNew.Text = fileName + " با موفقیت خوانده شد  ";

            string strSqlForDelLastTblContractorOwner = @" Use PE ;
            Delete From tblContractor;
            ";


            objClsSql.SendCommand(strSqlForDelLastTblContractorOwner);

            DataTable dtContractorOwner = clsExcel.ReadExcelFileSendToDt("Sheet1", fileName);

            #region حذف فیلدهای اضافه دیتاتیبل
            dtContractorOwner.Columns.Remove("نام سایت");
            dtContractorOwner.Columns.Remove("آدرس");
            dtContractorOwner.Columns.Remove("تاریخ تحویل به پیمانکار");
            #endregion

            foreach (DataRow dr1 in dtNewDb.Rows)
            {
                foreach (DataRow dr2 in dtContractorOwner.Rows)
                {
                    if (dr1[0].ToString() == dr2[0].ToString())
                    {
                        dr1["کد سایت"] = dr2["کد سایت"];
                        dr1["منطقه"] = dr2["منطقه"];
                        dr1["پیمانکار"] = dr2["پیمانکار"];
                        dr1["اولویت"] = dr2["اولویت"];
                        dr1["مالک"] = dr2["مالک"];
                        dr1["شماره تماس مالک"] = dr2["شماره تماس مالک"];
                    }
                }

            }

            objClsSql.DeleteAllRecords("[DB].dbo.[tblDb]");
            objClsSql.BulkInsertDataTable("[DB].dbo.[tblDb]", dtNewDb);

            // objClsSql.BulkInsertDataTable("[PE].dbo.[tblContractor]", dtContractorOwner);

            #region حل مشکل یا و کاف در دیتابیس ارس

            string strSqlForSolveYaAndKaf = @"
            USE PE;  
   
            DECLARE @Table NVARCHAR(MAX),  
            @Col NVARCHAR(MAX)  
   
            DECLARE Table_Cursor CURSOR   
            FOR  
            --پيدا كردن تمام فيلدهاي متني تمام جداول ديتابيس جاري  
            SELECT a.name, --table  
            b.name --col  
            FROM   sysobjects a,  
            syscolumns b  
            WHERE  a.id = b.id  
            AND a.xtype = 'u' --User table  
            AND (  
            b.xtype = 99 --ntext  
            OR b.xtype = 35 -- text  
            OR b.xtype = 231 --nvarchar  
            OR b.xtype = 167 --varchar  
            OR b.xtype = 175 --char  
            OR b.xtype = 239 --nchar  
            )  
   
            OPEN Table_Cursor FETCH NEXT FROM  Table_Cursor INTO @Table,@Col  
            WHILE (@@FETCH_STATUS = 0)  
            BEGIN  
            EXEC (  
            'update [' + @Table + '] set [' + @Col +  
            ']= REPLACE(REPLACE(CAST([' + @Col +  
            '] as nvarchar(max)) , NCHAR(1610), NCHAR(1740)),NCHAR(1603),NCHAR(1705)) WHERE CHARINDEX(NCHAR(1603),[' + @Col +  '])>0 OR CHARINDEX(NCHAR(1610),[' + @Col +  '])>0'  
            )  
            --PRINT 'Table: ' + @Table +' Col: '+ @Col;
            FETCH NEXT FROM Table_Cursor INTO @Table,@Col  
            END CLOSE Table_Cursor DEALLOCATE Table_Cursor 
            ";


            objClsSql.SendCommand(strSqlForSolveYaAndKaf);
            #endregion

            #region ثبت تاریخ بروز رسانی
            ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfTblContractorOwner");
            #endregion


            RefreshNewDbGrid();//رفرش جدول
        }

        private void بهروزرسانیباطریکولرToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            this.openFileDialog.ShowDialog();

            string fileName = this.openFileDialog.FileName;

            if (fileName == string.Empty)
            {
                MessageBox.Show("هیچ فایلی جهت بروز رسانی انتخاب نشده");
                return;
            }

            this.toolStripLblDbNew.Text = fileName + " با موفقیت خوانده شد  ";

            DataTable dtBatteryCooler = clsExcel.ReadExcelFileSendToDt("Worksheet", fileName);

            #region حذف فیلدهای اضافه دیتاتیبل
            dtBatteryCooler.Columns.Remove("استان");
            #endregion

            foreach (DataRow dr1 in dtNewDb.Rows)
            {
                foreach (DataRow dr2 in dtBatteryCooler.Rows)
                {
                    if (dr1[0].ToString() == dr2[0].ToString())
                    {
                        dr1["تعداد راک رکتی فایر"] = dr2["تعداد راک رکتی فایر"];
                        dr1["تعداد ماژول رکتی فایر"] = dr2["تعداد ماژول رکتی فایر"];
                        dr1["Rectifier - نوع رکتی فایر"] = dr2["Rectifier - نوع رکتی فایر"];
                        dr1["Rectifier - تاریخ تعویض رکتی فایر"] = dr2["Rectifier - تاریخ تعویض رکتی فایر"];
                        dr1["Rectifier - Description"] = dr2["Rectifier - Description"];
                        dr1["تعداد راک باتری"] = dr2["تعداد راک باتری"];
                        dr1["نوع باتری ها"] = dr2["نوع باتری ها"];
                        dr1["Batteries Bank - تعداد باتری"] = dr2["Batteries Bank - تعداد باتری"];
                        dr1["Batteries Bank - تاریخ تعویض باتری"] = dr2["Batteries Bank - تاریخ تعویض باتری"];
                        dr1["Batteries Bank - کارخانه سازنده باتری"] = dr2["Batteries Bank - کارخانه سازنده باتری"];
                        dr1["Batteries Bank - مشخصات باتری ها"] = dr2["Batteries Bank - مشخصات باتری ها"];
                        dr1["Batteries Bank - طول عمر باتری (ماه)"] = dr2["Batteries Bank - طول عمر باتری (ماه)"];
                        dr1["تعداد کولر"] = dr2["تعداد کولر"];
                        dr1["اطلاعات سرمایش - کارخانه سازنده کولر"] = dr2["اطلاعات سرمایش - کارخانه سازنده کولر"];
                        dr1["اطلاعات سرمایش - نوع کولر"] = dr2["اطلاعات سرمایش - نوع کولر"];
                        dr1["اطلاعات سرمایش - تاریخ تعویض کولر"] = dr2["اطلاعات سرمایش - تاریخ تعویض کولر"];
                        dr1["اطلاعات سرمایش - Description"] = dr2["اطلاعات سرمایش - Description"];
                    }
                }

            }

            objClsSql.DeleteAllRecords("[DB].dbo.[tblDb]");
            objClsSql.BulkInsertDataTable("[DB].dbo.[tblDb]", dtNewDb);

            // objClsSql.BulkInsertDataTable("[PE].dbo.[tblContractor]", dtContractorOwner);

            #region حل مشکل یا و کاف در دیتابیس ارس

            string strSqlForSolveYaAndKaf = @"
            USE PE;  
   
            DECLARE @Table NVARCHAR(MAX),  
            @Col NVARCHAR(MAX)  
   
            DECLARE Table_Cursor CURSOR   
            FOR  
            --پيدا كردن تمام فيلدهاي متني تمام جداول ديتابيس جاري  
            SELECT a.name, --table  
            b.name --col  
            FROM   sysobjects a,  
            syscolumns b  
            WHERE  a.id = b.id  
            AND a.xtype = 'u' --User table  
            AND (  
            b.xtype = 99 --ntext  
            OR b.xtype = 35 -- text  
            OR b.xtype = 231 --nvarchar  
            OR b.xtype = 167 --varchar  
            OR b.xtype = 175 --char  
            OR b.xtype = 239 --nchar  
            )  
   
            OPEN Table_Cursor FETCH NEXT FROM  Table_Cursor INTO @Table,@Col  
            WHILE (@@FETCH_STATUS = 0)  
            BEGIN  
            EXEC (  
            'update [' + @Table + '] set [' + @Col +  
            ']= REPLACE(REPLACE(CAST([' + @Col +  
            '] as nvarchar(max)) , NCHAR(1610), NCHAR(1740)),NCHAR(1603),NCHAR(1705)) WHERE CHARINDEX(NCHAR(1603),[' + @Col +  '])>0 OR CHARINDEX(NCHAR(1610),[' + @Col +  '])>0'  
            )  
            --PRINT 'Table: ' + @Table +' Col: '+ @Col;
            FETCH NEXT FROM Table_Cursor INTO @Table,@Col  
            END CLOSE Table_Cursor DEALLOCATE Table_Cursor 
            ";


            objClsSql.SendCommand(strSqlForSolveYaAndKaf);
            #endregion

            #region ثبت تاریخ بروز رسانی
            ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfTblContractorOwner");
            #endregion


            RefreshNewDbGrid();//رفرش جدول
        }

       

        #region عملیات مربوط به اکتیو شدن تب دیتابیس
        private void DbNewTabActive()
        {


            if (strCurRol == "")
            {
                groupControlDb.Visible = false;

                MessageBox.Show("لطفا لاگین بفرمایید", "اطلاع رسانی", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }

            if (strCurRol == "PowerUser" || strCurRol == "administrator")
            {
                exportToolStripMenuItem.Visible = true;
                پشتیبانToolStripMenuItem.Visible = true;
                updateToolStripMenuItem.Visible = true;

                groupControlDb.Visible = true;
                مقایسهارسجدیدباقدیمToolStripMenuItem.Visible = true;
                #region در صورت نیاز رفرش
                try
                {
                    #region بررسی نیاز به رفرش
                    StrdateOfDbUpdate = objClsSql.SendCommandGetStringResult("USE PE; Select dateOfLastDbUp From tblUpdateDate ");

                    bool bolEqualLastAndCurrentUpdate = StrdateOfDbUpdate.Equals(StrdateOfLastDbUpdate);

                    if (!bolEqualLastAndCurrentUpdate)
                    {
                        StrdateOfLastDbUpdate = StrdateOfDbUpdate;
                        #region رفرش
                        RefreshNewDbGrid();
                        StrdateOfLastDbUpdate = StrdateOfDbUpdate;
                    }
                    #endregion



                    #endregion

                    #region محاسبه و اعلام آخرین بروز رسانی ها
                    string strLastUpdate = CalculateLastUpDbTimeSummary();
                    toolStripLblDbNew.Text = strLastUpdate;
                    #endregion

                }
                catch (IOException e)
                {
                    if (e.Source != null)
                        MessageBox.Show("IOException source: {0}", e.Source);
                    throw;
                }
                #endregion
            }
            else
            {

                groupControlDb.Visible = false;
                مقایسهارسجدیدباقدیمToolStripMenuItem.Visible = false;

                frmDbSearch objFrmDSearch = new frmDbSearch();
                objFrmDSearch.ShowDialog();

            }

        }
        #endregion

        #region محاسبه و ثبت تاریخ و زمان آخرین بروز رسانی
        private string CalculateLastUpDbTimeSummary()
        {
            if (StrDbDateLastUpdatePersian == "نامشخص")
            {
                string strSqlForDetectLastUpdateDbS = @"
                USE PE;
                SELECT dateOfLastDbUp
                FROM tblUpdateDate
                
                 ";

                string strDateLastUpdate = objClsSql.SendCommandGetStringResult(strSqlForDetectLastUpdateDbS);

                StrDbDateLastUpdatePersian = "نا مشخص";

                if (strDateLastUpdate != null && strDateLastUpdate != string.Empty && strDateLastUpdate != "نا مشخص")
                {
                    try
                    {
                        StrDbDateLastUpdatePersian = ClsConvertDateHassan.convertDateTimeToPersionWithSlash(DateTime.Parse(strDateLastUpdate));
                    }
                    catch
                    {
                        MessageBox.Show("Update Date Is Not Valid!!!!");
                    }
                }

            }

            string strLastUpdate = " آخرین زمان بروز رسانی دیتابیس   " + StrDbDateLastUpdatePersian + " می باشد  ";
            return strLastUpdate;
        }

        #endregion

        #region بررسی نیاز به رفرش
        private bool IsNeedRefreshDbGrids()
        {
            bool bolNeedRefresh = false;
            string strCurDateOfUpdateDbSummaryInSql = "";

            #region واکشی تاریخ آخرین بروز رسانی دیتابیس

            try
            {
                strCurDateOfUpdateDbSummaryInSql = objClsSql.SendCommandGetStringResult("Select dateOfLastDbUp From PE.dbo.tblUpdateDate");

            }
            catch
            {
                MessageBox.Show(" به دلیل عدم اتصال به سرور اس کیو ال " + "\r\n" + strDateOfLastDbUpInStart + "\r\n" + " واکشی نشد لطفا مراتب را به مدیر سامانه اطلاع دهید" + "\r\n", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign, false);
            }

            #endregion

            #region بررسی نیاز به رفرش 
            if (dtDbSummary == null || strDateOfLastDbUpInStart != strCurDateOfUpdateDbSummaryInSql)
            {
                bolNeedRefresh = true;
            }
            else
            {
                bolNeedRefresh = false;
            }
            return bolNeedRefresh;
            #endregion;

        }

        #endregion

        #region اکسپورت به اکسل

        #endregion

        #region آپدیت

        #region بروز رسانی دیتابیس 



        #endregion

        #region بروز رسانی پیمانکار و مالک



        #endregion

        #region دیدن وتغییر دیتابیس

        #region رفتن به فرم تغییرات دیتابیس و دفتر تلفن

        #endregion


        #endregion

        #region مشاهده درخواستهای تغییرات در دیتابیس

        private void viewChangeRequest()
        {
            if (StrCurRol == "PowerUser" || StrCurRol == "administrator")
            {

                int intCountOfChangRequest = int.Parse(objClsSql.SendCommandGetStringResult("Select Count(*) From [PE].[dbo].[tblRequestChange] Where status=N'در حال بررسی' "));

                if (intCountOfChangRequest > 0)
                {

                    pictureBoxAdUp.Visible = true;
                    this.pictureBoxAdUp.ImageLocation = MainForm.StrSelectedRootPathInPeServer + @"\NeededFiles\ICO\notification1.ico";//
                    Tip.SetToolTip(this.pictureBoxAdUp, intCountOfChangRequest + " مورد درخواست تغییرات داریم   ");
                }
                else
                {
                    pictureBoxAdUp.Visible = false;
                }
            }
        }
        #endregion

        #region عملیات مربوط به آیکون درخواست تغییرات
        private void pictureBoxAdUp_MouseClick(object sender, MouseEventArgs e)
        {
            if (StrCurRol == "PowerUser" || StrCurRol == "administrator")
            {
                int intCountOfChangRequest = int.Parse(objClsSql.SendCommandGetStringResult("select Count(*) from [PE].[dbo].[tblRequestChange] where status=N'در حال بررسی' "));

                if (intCountOfChangRequest > 0)
                {
                    frmDbCheckChgRequest frmDbChg = new frmDbCheckChgRequest();
                    frmDbChg.ShowDialog();
                }
                else
                {


                }
            }
        }

        private void pictureBoxAdUp_MouseHover(object sender, EventArgs e)
        {
            viewChangeRequest();
        }
        #endregion

        #region نمایش روی نقشه سایت

        #endregion

        #region شمارش تعداد فیلتر شده
        private void countToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridViewDbNew.CollapseAllGroups();
            int intCountOfRows = gridViewDbNew.RowCount;
            string strMsg = "CountGroupedRowsToolStripMenuItem Of Grouped Rows Is : " + intCountOfRows.ToString();
            MessageBox.Show(strMsg);
        }
        #endregion

        #region Plans

        #endregion

        #region New_Db

        private void RefreshNewDbGrid()
        {
            lblDbNewOpgBarText.Visible = true;

            #region پاک کردن ساختار گرید یرای بارگذاری جدید
            gridViewDbNew.Columns.Clear();
            gridViewDbNew.PopulateColumns();
            gridViewDbNew.RefreshData();
            gridControlDbNew.DataSource = null;
            gridControlDbNew.RefreshDataSource();
            gridControlDbNew.Refresh();
            #endregion

            #region مرتب کردن و وسط چین و ...
            this.gridControlDbNew.Visible = false;

            lblDbNewOpgBarText.Visible = true;

            CenterControlInParent(progressBarControlDbNew, false, true);//وسط چین کردن لیبل

            CenterControlInParent(lblDbNewOpgBarText, true, true); ;//وسط چین کردن نوار پیشرفت البته یکم بالاتر

            progressBarControlDbNew.Top = lblDbNewOpgBarText.Parent.Height / 2 - 50;

            lblDbNewOpgBarText.Text = "اطلاعات دیتابیس در حال بارگذاری اولیه می باشد . لطفا منتظر باشید";
            this.toolStripLblDbNew.Text = " اطلاعات دیتابیس در حال بارگذاری اولیه می باشد . لطفا منتظر باشید";

            //lblDbNewOpgBarText.Text += "  ضمناً  " + CalculateLastUpDbTimeSummary();

            progressBarControlDbNew.Visible = true;
            lblDbNewOpgBarText.Visible = true;

            progressBarControlDbNew.EditValue = 1;

            System.Windows.Forms.Application.DoEvents();
            #endregion

            #region واکشی
            MainForm.DateTimeSqlServer = objClsSql.GetDateTimeSqlServer();
            string strQuery = @"select * From [DB].[dbo].[tblDb]  ORDER BY  [کد سایت];
                    ";

            dtNewDb = objClsSql.FillDataTableFromQuery(strQuery);
            gridControlDbNew.DataSource = dtNewDb;
            #endregion

            #region فریز 5 ستون راست
            gridViewDbNew.Columns["کد سایت"].Fixed = FixedStyle.Left;
            gridViewDbNew.Columns["نام سایت"].Fixed = FixedStyle.Left;
            gridViewDbNew.Columns["پیمانکار"].Fixed = FixedStyle.Left;
            gridViewDbNew.Columns["BSC/RNC"].Fixed = FixedStyle.Left;
            //gridViewDbNew.Columns["BSC&RNC"].Fixed = FixedStyle.Left;
            gridViewDbNew.Columns["منطقه"].Fixed = FixedStyle.Left;
            gridViewDbNew.Columns["پیمانکار"].Fixed = FixedStyle.Left;
            #endregion

            gridViewDbNew.Appearance.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            gridViewDbNew.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

            #region انتخاب سطر فیلتر پنل
            gridViewDbNew.FocusedRowHandle = GridControl.AutoFilterRowHandle;
            gridViewDbNew.ShowEditor();
            #endregion

            progressBarControlDbNew.Visible = false;

            lblDbNewOpgBarText.Visible = false;
            gridViewDbNew.Columns["آدرس"].Width = 400;
            gridViewDbNew.Columns["نام سایت"].Width = 150;
            gridViewDbNew.Columns["پیمانکار"].Width = 150;
           // gridViewDbNew.Columns["BSC&RNC"].Width = 90;
            this.gridControlDbNew.Visible = true;

        }

        #endregion


        #endregion

        #endregion 

        #region PB
        private void TelTabActive()
        {
            if (strCurUsr == "")
                ارتباطبامدیرToolStripMenuItem.Enabled = false;
            else
                ارتباطبامدیرToolStripMenuItem.Enabled = true;


            if (((StrCurRol == "administrator") || (StrCurRol == "PowerUser")) || (StrCurRol == "Support"))
            {
                بررسیدرخواستهاToolStripMenuItem.Enabled = true;
                gbxEditTel.Visible = true;
                gbxAddTel.Visible = true;
                exportToXclToolStripMenuItem.Enabled = true;
            }
            else
            {
                gvTel.OptionsBehavior.Editable = false;
                بررسیدرخواستهاToolStripMenuItem.Enabled = false;
                gbxEditTel.Visible = false;
                gbxAddTel.Visible = false;
                exportToXclToolStripMenuItem.Enabled = false;
            }

            gcTel.Visible = true;
            gcTel.DataSource = objClsSql.FillDataTableFromQuery("Select telId AS [ردیف], Name As [نام و نام خانوادگی],tel AS [شماره تلفن ها],WorkPlace AS [محل کار]  From [TEL].[dbo].[tblTel] Order By Name");
            gvTel.Columns["ردیف"].Visible = false;
        }

        private void resetTelToDefaultMode()
        {
            this.gcTel.DataSource = null;
            this.gcTel.Visible = false;
        }

        private void gcTel_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {

                int intTelId = int.Parse(gvTel.GetFocusedDataRow()["ردیف"].ToString());
                string strName = gvTel.GetFocusedDataRow()["نام و نام خانوادگی"].ToString();
                string strTel = gvTel.GetFocusedDataRow()["شماره تلفن ها"].ToString();

                string strMessage = "کاربر گرامی تلفن انتخاب شده  حذف گردد ؟";
                DialogResult result = MessageBox.Show(strMessage, "توجه", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign);

                if (result == DialogResult.Yes)
                {
                    string strWhere = " WHERE telId=" + intTelId;
                    string[] strField = { "Name", "Tel" };
                    string[] strValue = { strName, strTel };
                    bool bolIsDelRecord = objClsSql.DeleteRecords("[TEL].[dbo].[tblTel]", strWhere);
                    if (bolIsDelRecord)
                    {
                        ClsDiff.AutoClosingMessageBox.Show("مشخصات تلفن حذف شد", "Note", 4000);
                        this.CheckTab();
                    }
                    else
                        ClsDiff.AutoClosingMessageBox.Show("اشکال در حذف", "Alarm", 4000);
                }
                else if (result == DialogResult.No)
                {

                }

            }
        }

        private void gvTel_RowUpdated(object sender, RowObjectEventArgs e)
        {
            if (gvTel.IsNewItemRow(e.RowHandle))
            {


                //int intTelId = int.Parse(gvtel.GetFocusedDataRow()["ردیف"].ToString());
                string strName = gvTel.GetFocusedDataRow()["نام و نام خانوادگی"].ToString();
                string strTel = gvTel.GetFocusedDataRow()["شماره تلفن ها"].ToString();

                string strMessage = "کاربر گرامی تغییرات در دفتر تلفن اعمال گردد ؟";
                DialogResult result = MessageBox.Show(strMessage, "توجه", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign);

                if (result == DialogResult.Yes)
                {
                    // string strWhere = "telId=" + intTelId;
                    string[] strField = { "Name", "Tel" };
                    string[] strValue = { strName, strTel };
                    bool bolIsUpdate = objClsSql.Insert("[TEL].[dbo].[tblTel]", strField, strValue);
                    if (bolIsUpdate)
                        ClsDiff.AutoClosingMessageBox.Show("تغییرات خواسته شده در دفتر تلفن اعمال شد", "Note", 4000);
                    else
                        ClsDiff.AutoClosingMessageBox.Show("تغییرات خواسته شده در دفتر تلفن اعمال نشد", "Alarm", 4000);
                }
                else if (result == DialogResult.No)
                {

                }
            }
        }

        private void gvTel_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "شماره تلفن ها")
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.DarkGreen;
            }
        }

        private void gcTel_Load(object sender, EventArgs e)
        {
            GridControl gridControl = sender as GridControl;
            GridView gridView = gridControl.MainView as GridView;
            if (gridView != null)
                gridControl.BeginInvoke(new Action(gridView.ShowFindPanel));
        }

        private void gcTel_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void درخواستToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDbCheckChgRequest objFrmChgTelDb = new frmDbCheckChgRequest();
            objFrmChgTelDb.Show();

        }
        private void بررسیدرخواستهاToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDbCheckChgRequest objFrmChgTelDb = new frmDbCheckChgRequest();
            objFrmChgTelDb.Show();
        }
        private void gvTel_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (gvTel.GetFocusedDataRow() == null)
            {
                return;
            }
            if (gvTel.SelectedRowsCount == 0)
            {
                return;
            }
            BeginInvoke(new MethodInvoker(delegate
            {
                txtName.Text = gvTel.GetFocusedDataRow()["نام و نام خانوادگی"].ToString();
                txtTel.Text = gvTel.GetFocusedDataRow()["شماره تلفن ها"].ToString();
                txtWorkPlace.Text = gvTel.GetFocusedDataRow()["محل کار"].ToString();

            }
                ));
        }
        private void btnTelChange_Click(object sender, EventArgs e)
        {
            int intTelId = int.Parse(gvTel.GetFocusedDataRow().ItemArray[0].ToString());

            string strMessage = "کاربر گرامی تغییرات در دفتر تلفن اعمال گردد ؟";
            DialogResult result = MessageBox.Show(strMessage, "توجه", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);

            if (result == DialogResult.Yes)
            {
                string strWhere = "telId=" + intTelId;
                string[] strField = { "Name", "Tel", "WorkPlace" };
                string[] strValue = { txtName.Text.Trim(), txtTel.Text.Trim(), txtWorkPlace.Text.Trim() };
                bool bolIsUpdate = objClsSql.UpDate("[TEL].[dbo].[tblTel]", strField, strValue, strWhere);
                if (bolIsUpdate)
                {
                    ClsDiff.AutoClosingMessageBox.Show("تغییرات خواسته شده در دفتر تلفن اعمال شد", "Note", 4000);
                    TelTabActive();
                    int row = gvTel.LocateByValue("نام و نام خانوادگی", txtName.Text, null);
                    gvTel.FocusedRowHandle = row;
                }
                else
                    ClsDiff.AutoClosingMessageBox.Show("تغییرات خواسته شده در دفتر تلفن اعمال نشد", "Alarm", 4000);
            }
            else if (result == DialogResult.No)
            {

            }
        }
        private void btnTelDelete_Click(object sender, EventArgs e)
        {
            int intTelId = int.Parse(gvTel.GetFocusedDataRow()["ردیف"].ToString());
            string strName = txtName.Text;
            string strTel = txtTel.Text;

            string strMessage = "کاربر گرامی تلفن انتخاب شده  حذف گردد ؟";
            DialogResult result = MessageBox.Show(strMessage, "توجه", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);

            if (result == DialogResult.Yes)
            {
                string strWhere = " WHERE telId=" + intTelId;
                string[] strField = { "Name", "Tel" };
                string[] strValue = { strName, strTel };
                bool bolIsDelRecord = objClsSql.DeleteRecords("[TEL].[dbo].[tblTel]", strWhere);
                if (bolIsDelRecord)
                {
                    ClsDiff.AutoClosingMessageBox.Show("مشخصات تلفن حذف شد", "Note", 4000);
                    this.CheckTab();
                    txtName.Clear();
                    txtTel.Clear();
                }
                else
                    ClsDiff.AutoClosingMessageBox.Show("اشکال در حذف", "Alarm", 4000);
            }
            else if (result == DialogResult.No)
            {

            }
        }
        private void btnAddTelNumber_Click(object sender, EventArgs e)
        {
            string strName = txtAddName.Text.Trim();
            if (strName == null || strName == "")
            {
                ClsDiff.AutoClosingMessageBox.Show("لطفا نام مورد نظر را وارد کنید", "Note", 7000);
                return;
            }
            string strTel = txtAddTel.Text.Trim();
            if (strTel == null || strTel == "")
            {
                ClsDiff.AutoClosingMessageBox.Show("لطفا تلفن مورد نظر را وارد کنید", "Note", 7000);
                return;
            }

            string strWorkPlace = txtAddNewWorkPlace.Text.Trim();
            if (strWorkPlace == null || strWorkPlace == "")
            {
                ClsDiff.AutoClosingMessageBox.Show("لطفا محل کار مورد نظر را وارد کنید", "Note", 7000);
                return;
            }

            string strMessage = "کاربر گرامی شماره درخواستی اضافه گردد ؟";
            DialogResult result = MessageBox.Show(strMessage, "توجه", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);

            if (result == DialogResult.Yes)
            {

                string[] strField = { "Name", "Tel", "WorkPlace" };
                string[] strValue = { strName, strTel, strWorkPlace };

                int row = gvTel.LocateByValue("نام و نام خانوادگی", strName, null);
                if (row > 0)
                {
                    ClsDiff.AutoClosingMessageBox.Show("کاربر گرامی این نام در دفترچه تلفن موجود است در صورت نیاز آنرا ویرایش نمایید", "Alarm", 4000);
                    gvTel.FocusedRowHandle = row;
                    return;
                }
                bool bolIsUpdate = objClsSql.Insert("[TEL].[dbo].[tblTel]", strField, strValue);
                if (bolIsUpdate)
                {
                    txtAddName.Clear();
                    txtAddTel.Clear();
                    txtAddNewWorkPlace.Clear();

                    ClsDiff.AutoClosingMessageBox.Show("شماره تلفن مورد نظر شما به دفترچه تلفن اضافه شد", "Note", 4000);
                    this.CheckTab();
                    string strAddedIdTel = objClsSql.SendCommandGetStringResult("select telId from  [TEL].[dbo].[tblTel] Where Name=N'" + strName + "' AND Tel=N'" + strTel + "'");
                    row = gvTel.LocateByValue("نام و نام خانوادگی", strName, null);
                    gvTel.FocusedRowHandle = row;
                }
                else
                    ClsDiff.AutoClosingMessageBox.Show("اشکالی در اضافه کردن شماره جدید پیش آمد لطفا به مدیر سیستم اطلاع دهید", "Alarm", 4000);
            }
            else if (result == DialogResult.No)
            {

            }
        }
        private void txtAddName_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTelChange.PerformClick();
            }
        }
        private void txtTel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTelChange.PerformClick();
            }
        }
        private void txtAddName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddTelNumber.PerformClick();
            }
        }
        private void txtAddTel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddTelNumber.PerformClick();
            }

        }


        #endregion

        #region REQ
        private void ReqTabActive()
        {
            if (MainForm.StrCurRol != string.Empty)
            {
                new frmDbCheckChgRequest().ShowDialog();
                //درخواستتغییراتToolStripMenuItem.Enabled = true;
            }
            else
            {
                MessageBox.Show("برای مشاهده و ایجاد درخواست لطفا لاگین بفرمایید", "لاگین", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }


        }



        #endregion

        #region PE

        #region LOG


        private void PeTabActive()
        {
            if (StrCurRol == "administrator")
            {
                menuStripPe.Enabled = true;
            }
            else
            {
                menuStripPe.Enabled = false;

            }
        }


        #endregion

        #region VPN


        private void مشاهدهلاگهاToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Logs objLogs = new Logs();
            objLogs.ShowDialog();
        }

        private void مدیریتVpnsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PE.Forms.PE.Vpns objVpns = new Forms.PE.Vpns();
            objVpns.ShowDialog();
        }

        #endregion

        #endregion

        private void آمارجدیدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAmar objFrmAmar = new frmAmar();
            objFrmAmar.ShowDialog();
        }

        private void gridViewDt_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {

            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;

            string caption = info.Column.Caption;

            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();

            info.GroupText = string.Format("{0} : {1} (count= {2})", caption, info.GroupValueText, view.GetChildRowCount(e.RowHandle));

        }

        private void پکیجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (پکیجToolStripMenuItem.Checked)
            {
                پکیجToolStripMenuItem.Checked = false;
                bolDtPakage = false;
            }
            else
            {
                gridViewDt.OptionsView.ShowFooter = true;


                bolDtHaveRunTimeFilter = false;
                bolDtRegByUser = false;
                bolDtHaveWaitForSessionFilter = false;
                bolDtRegInClarity = false;
                bolDtInProcessingRequest = false;
                bolDtRepeated = false;
                bolDtPakage = true;
                bolDtNeedToCheck = false;
                bolDtFinalReviw = false;

                درحالاجراToolStripMenuItem1.Checked = false;
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                تکراریToolStripMenuItem.Checked = false;
                پکیجToolStripMenuItem.Checked = true;
                نیازبهبررسیToolStripMenuItem.Checked = false;
                بررسینهاییToolStripMenuItem.Checked = false;
            }

            RefreshDtAndGoToEnd();
        }


        private void کپیفایلاجراییبهسرورToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string applicationName = System.AppDomain.CurrentDomain.FriendlyName;
            string strDestIp = "172.16.3.116";

            string sourceFile = StrSelectedRootPathInPeServer + "\\" + applicationName;
            string destinationPath = @"\\" + strDestIp + @"\P.E Data\PE\" + applicationName;

            try
            {

                // Copy the file to the destination path
                File.Copy(sourceFile, destinationPath, true);

                MessageBox.Show("File copied successfully.");
            }
            catch (IOException e1)
            {
                MessageBox.Show("An error occurred while copying the file: " + e1.Message);
            }

        }

        private void حذففایلاجراییقبلیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strDestIp = "172.16.3.116";
            // مسیر فایل بچ
            string batchFilePath = @"\\" + strDestIp + @"\P.E Data\PE\deletePeFile.bat";

            // اجرای فایل بچ
            System.Diagnostics.Process.Start(batchFilePath);
        }

        private void خواندنفایلهایجدیدوقدیمToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // متد LoadExcelFile برای خواندن یک فایل اکسل
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

        private void مقایسهیToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void مقایسهارسجدیدباقدیمToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmDbArsCompairer objFrmDbArasCompairer = new frmDbArsCompairer();

            objFrmDbArasCompairer.ShowDialog();
        }

        private void چککردنCRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDbCrChecker objFrmDbCrChecker = new frmDbCrChecker();

            objFrmDbCrChecker.ShowDialog();
        }

        private void کپیToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // دریافت تمام پروسس‌های اکسل
            Process[] excelProcesses = Process.GetProcessesByName("EXCEL");

            // بستن هر پروسس اکسل
            foreach (Process process in excelProcesses)
            {
                process.Kill(); // بستن پروسس
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // اگر استفاده غیرتجاری دارید

            DataTable dtCopyCrToDSK = CreateDatatTable();//ایجاد دیتا تیبل برای ساخت فایل اکسل

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            string filePath = Path.Combine(desktopPath, "PE.xlsx");

            if (!IsFileLocked(filePath))
            {
              
                // اگر فایل در حال استفاده نباشد، می‌توانید عملیات خود را انجام دهید
                DeleteFile(filePath);

                // یا ایجاد فایل جدید
                CreateExcelFile(filePath,dtCopyCrToDSK);

                // ایجاد یک نمونه از اکسل
                Excel.Application excelApp = new Excel.Application();

                // نمایش اکسل به کاربر
                excelApp.Visible = true;

                // باز کردن فایل اکسل
                Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);

                txtLog.Visible=true;
                // فراخوانی تابع برای کپی فایل‌ها
                CopyFilesFromDataTable(dtCopyCrToDSK, message => txtLog.AppendText(message + Environment.NewLine));

                // توقف برنامه به مدت 3 ثانیه
                Thread.Sleep(3000);

                if (txtLog.Text!="error")
                {
                    txtLog.Visible = false;
                }
            }
            else
            {
                MessageBox.Show($" باز است. لطفاً آن را ببندید  {Path.GetFileName(filePath)} فایل ", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }





        private static void CreateExcelFile(string filePatch, DataTable dtCopyCrToDSK)
        {
            // ایجاد یک فایل اکسل
            using (var package = new ExcelPackage(new FileInfo(filePatch)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // کپی کردن داده‌ها از DataTable به اکسل
                worksheet.Cells.LoadFromDataTable(dtCopyCrToDSK, true);

                // فرمت دادن به جدول
                int rows = dtCopyCrToDSK.Rows.Count + 1;
                int columns = dtCopyCrToDSK.Columns.Count;

                using (ExcelRange range = worksheet.Cells[1, 1, rows, columns])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // فرمت دادن به ردیف اول
                using (ExcelRange headerRange = worksheet.Cells[1, 1, 1, dtCopyCrToDSK.Columns.Count])
                {
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#007bff")); // آبی
                    headerRange.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffffff")); // سفید
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // وسط چین
                }

                // پیدا کردن ستون "ضمیمه"
                int attachmentColumn = 0;
                for (int i = 1; i <= dtCopyCrToDSK.Columns.Count; i++)
                {
                    if (worksheet.Cells[1, i].Value.ToString() == "ضمیمه")
                    {
                        attachmentColumn = i;
                        break;
                    }
                }

                // رنگ کردن متن سلول‌هایی که در ستون "ضمیمه" کلمه "دارد" وجود دارد
                for (int i = 2; i <= dtCopyCrToDSK.Rows.Count + 1; i++)
                {
                    string cellValue = worksheet.Cells[i, attachmentColumn].Value.ToString();
                    if (cellValue.Trim() == "دارد")
                    {
                        worksheet.Cells[i, attachmentColumn].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#008000")); // سبز
                        worksheet.Cells[i, attachmentColumn].Style.Font.Bold = true; // بولد
                    }
                }

                // رنگ زرد برای ردیف‌های متناوب
                for (int i = 2; i <= dtCopyCrToDSK.Rows.Count + 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        using (ExcelRange rowRange = worksheet.Cells[i, 1, i, dtCopyCrToDSK.Columns.Count])
                        {
                            rowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rowRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#ffffcc")); // زرد کم رنگ
                        }
                    }
                }

                // تنظیم خودکار عرض ستون‌ها
                for (int i = 1; i <= dtCopyCrToDSK.Columns.Count; i++)
                {
                    worksheet.Column(i).AutoFit();
                }

                // راست به چپ کردن فایل اکسل با EPPlus
                worksheet.View.RightToLeft = true;

                // ذخیره فایل اکسل
                package.Save();
            }

           // MessageBox.Show("در دسکتاپ شما ایجاد شد PE.xlsx  فایل اکسل با نام");
        }


        private DataTable  CreateDatatTable()
        {
            string strSql = @"SELECT   row AS ردیف ,
              haveAttachedFile AS [ضمیمه],
              registerDate AS [تاریخ ثبت],
              registerTime AS [ثبت],
              siteCode AS [کد سایت],
              siteName AS [نام سایت],
              region AS [منطقه],
              startDate AS [تاریخ شروع],
              startTime AS [زمان شروع],
              endDate AS [تاریخ پایان],
              endTime AS [زمان پایان],
              outageMin AS [زمان قطعی],
              applicant AS [درخواست کننده],
              subject AS موضوع,
              performer AS [انجام دهنده],
              approver AS [ثبت کننده],
              dtNo AS شماره ,
              status AS [وضعیت],
              Note AS [یادداشت]
 
  
               FROM   [DT].[dbo].tblDownTimeShort 
                       where
                        status=N'پردازش فنی'  Or 
                         status=N'عدم تائید(موقت)'  Or 
                          status=N'ثبت در (SDM)' Or 
                           status=N'ثبت توسط کاربر' Or 
                            status=N'در حال پردازش درخواست' Or
                              status=N'نیاز به اصلاح درخواست' Or
                                status=N'منتظر ارائه در جلسه' ; ";

            DataTable dtCopyCrToDSK = new DataTable();
            dtCopyCrToDSK = objClsSql.FillDataTableFromQuery(strSql); //دیتاتیبل حاوی دان تایم های جاری مورد نظر
                                                                      // مرتب‌سازی DataTable بر اساس فیلد "ردیف"
            dtCopyCrToDSK.DefaultView.Sort = "ردیف ASC"; // برای مرتب‌سازی صعودی
            DataTable sortedTable = dtCopyCrToDSK.DefaultView.ToTable();

            return (sortedTable);
        }

        static void DeleteFile(string filePath)
        {
           
            // بررسی وجود فایل و حذف آن
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    //MessageBox.Show($"   .با موفقیت حذف شد {filePath} فایل ", "پیغام", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex is IOException)
                    {
                        CloseFileProcesses(filePath);
                        try
                        {
                            File.Delete(filePath);
                           // MessageBox.Show($"فایل {filePath} با موفقیت حذف شد.", "پیغام", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex2)
                        {
                            MessageBox.Show($"رخ داد   {filePath}  خطایی در حذف فایل {ex2.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"رخ داد {filePath} خطایی در حذف فایل: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
               // MessageBox.Show($" وجود ندارد {filePath} فایل", "پیغام", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        static void CloseFileProcesses(string filePath)
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    foreach (ProcessModule module in process.Modules)
                    {
                        if (module.FileName.ToLower() == filePath.ToLower())
                        {
                            process.Kill();
                            MessageBox.Show($"فرآیند {process.ProcessName} بسته شد.", "پیغام", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // برخی فرآیندها ممکن است دسترسی نداشته باشند
                }
            }

            // اگر هنوز فایل در حال استفاده است، ممکن است توسط یک فرآیند دیگر باز شده باشد
            // در این صورت، باید از کاربر بخواهید که فایل را ببندد
            if (IsFileInUse(filePath))
            {
                MessageBox.Show($" باز است. لطفاً آن را ببندید  {Path.GetFileName(filePath)} فایل ", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
        }

        static bool IsFileInUse(string filePath)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    stream.Close();
                }
                return false;
            }
            catch
            {
                return true;
            }
        }

        static bool IsFileLocked(string fileName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, fileName);

            if (!File.Exists(filePath))
            {
                return false; // فایل وجود ندارد
            }

            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    stream.Close();
                }
                return false; // فایل در حال استفاده نیست
            }
            catch
            {
                return true; // فایل در حال استفاده است
            }
        }

        public static void CopyFilesFromDataTable(DataTable dataTable, Action<string> logAction)
        {

            // مسیر شبکه و پوشه مقصد
            string networkPath = null;

            if (StrSelectedRootPathInPeServer.Contains("172.16.3.116"))
            {
                networkPath = @"\\172.16.3.116\P.E Data\DtAttachFile";
            }
            else
            {
                networkPath = @"\\10.84.224.74\P.E Data\DtAttachFile";
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destFolder = Path.Combine(desktopPath, "PE");

            try
            {
                // بررسی دسترسی به مسیر شبکه
                if (!NetworkAccessCheck(networkPath))
                {
                    logAction?.Invoke("خطا: دسترسی به مسیر شبکه امکان‌پذیر نیست!");
                    return;
                }

                // ایجاد پوشه مقصد در صورت عدم وجود
                CreateDestinationFolder(destFolder, logAction);

                // پاکسازی محتوای پوشه مقصد (در صورت وجود)
                CleanDestinationFolder(destFolder, logAction);

                // دریافت شماره ردیف‌هایی که فیلد Attachment آن‌ها مقدار "دارد" است و کپی فایل‌ها
                var rowsWithAttachments = dataTable.AsEnumerable()
                                                    .Where(row => row.Field<string>("ضمیمه") == "دارد")
                                                    .Select(row => new
                                                    {
                                                        RowNumber = row.Field<Int64>("ردیف"),
                                                        SiteCode = row.Field<string>("کد سایت")
                                                    });

                foreach (var row in rowsWithAttachments)
                {
                    string sourceFilePattern = Path.Combine(networkPath, $"{row.RowNumber}.*"); // الگوی جستجو

                    // جستجوی فایل مطابق الگو در مسیر شبکه
                    string[] foundFiles = Directory.GetFiles(networkPath, $"{row.RowNumber}.*");

                    if (foundFiles.Length == 0)
                    {
                        logAction?.Invoke($"فایلی با نام {row.RowNumber}.* در مسیر {networkPath} یافت نشد!");
                        continue; // ادامه حلقه برای بررسی فایل‌های بعدی
                    }

                    // کپی تمام فایل‌های منطبق به پوشه مقصد
                    foreach (var foundFile in foundFiles)
                    {
                        string fileExtension = Path.GetExtension(foundFile); // استخراج پسوند فایل
                        string destFileName = $"{row.RowNumber}-{row.SiteCode}{fileExtension}"; // نام جدید فایل
                        string destFilePath = Path.Combine(destFolder, destFileName);

                        File.Copy(foundFile, destFilePath, true);
                        logAction?.Invoke($"فایل {Path.GetFileName(foundFile)} با موفقیت کپی شد به {destFileName}.");
                    }
                }
            }
            catch (Exception ex)
            {
                logAction?.Invoke($"خطای سیستمی: {ex.Message}");
            }
            
        }

        /// <summary>
        /// بررسی دسترسی به مسیر شبکه
        /// </summary>
        /// <param name="path">مسیر شبکه</param>
        /// <returns>True اگر دسترسی وجود داشته باشد، False در غیر این صورت</returns>
        private static bool NetworkAccessCheck(string path)
        {
            try
            {
                return Directory.Exists(path);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ایجاد پوشه مقصد در صورت عدم وجود
        /// </summary>
        /// <param name="path">مسیر پوشه مقصد</param>
        /// <param name="logAction">اکشنی برای ثبت لاگ</param>
        private static void CreateDestinationFolder(string path, Action<string> logAction)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                logAction?.Invoke("پوشه PE در دسکتاپ ایجاد شد.");
            }
        }

        /// <summary>
        /// پاکسازی محتوای پوشه مقصد (در صورت وجود)
        /// </summary>
        /// <param name="path">مسیر پوشه مقصد</param>
        /// <param name="logAction">اکشنی برای ثبت لاگ</param>
        private static void CleanDestinationFolder(string path, Action<string> logAction)
        {
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }

                foreach (string dir in Directory.GetDirectories(path))
                {
                    Directory.Delete(dir, true);
                }

                logAction?.Invoke("محتوای پوشه PE پاکسازی شد.");
            }
        }

        private void exportToXclToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialogExpToXcl.ShowDialog();

            if (this.saveFileDialogExpToXcl.FileName != string.Empty)
            {
                this.gvTel.ExportToXlsx(this.saveFileDialogExpToXcl.FileName);
            }
            else
            {
                MessageBox.Show("مقصد جهت ذخیره انتخاب نشده");
            }
        }

       

        private void نیازبهبررسیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (نیازبهبررسیToolStripMenuItem.Checked)
            {
                نیازبهبررسیToolStripMenuItem.Checked = false;
                bolDtNeedToCheck=false;
            }
            else
            {
                نیازبهبررسیToolStripMenuItem.Checked = true;
                bolDtNeedToCheck = true;
                gridViewDt.OptionsView.ShowFooter = true;

                bolDtHaveRunTimeFilter = false;//در حال اجرا
                bolDtRegByUser = false;//ثبت توسط کاربر
                bolDtHaveWaitForSessionFilter = false;//منتظ ارائه در جلسه
                bolDtRegInClarity = false;//ثبت در اس دی ام
                bolDtInProcessingRequest = false;//در حال پردازش درخواست
                bolDtRepeated = false;//تکراری
                bolDtPakage = false;//پکیج
                bolDtFinalReviw = false;//بررسی نهایی

                درحالاجراToolStripMenuItem1.Checked = false;
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                تکراریToolStripMenuItem.Checked = false;
                پکیجToolStripMenuItem.Checked = false;
                بررسینهاییToolStripMenuItem.Checked = false;
            }

            RefreshDtAndGoToEnd();
        }

        private void بررسینهاییToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (بررسینهاییToolStripMenuItem.Checked)
            {
                بررسینهاییToolStripMenuItem.Checked = false;
                bolDtFinalReviw=false;
            }
            else
            {
                بررسینهاییToolStripMenuItem.Checked = true;
                bolDtFinalReviw = true;

                gridViewDt.OptionsView.ShowFooter = true;

                bolDtHaveRunTimeFilter = false;
                bolDtRegByUser = false;
                bolDtHaveWaitForSessionFilter = false;
                bolDtRegInClarity = false;
                bolDtInProcessingRequest = false;
                bolDtRepeated = false;
                bolDtPakage = false;
                bolDtNeedToCheck = false;
                

                درحالاجراToolStripMenuItem1.Checked = false;
                ثبتتوسطکاربرToolStripMenuItem.Checked = false;
                منتظرارائهدرجلسهToolStripMenuItem.Checked = false;
                ثبتدرکلاریتیToolStripMenuItem.Checked = false;
                درحالپردازشدرخواستToolStripMenuItem.Checked = false;
                تکراریToolStripMenuItem.Checked = false;
                پکیجToolStripMenuItem.Checked = false;
                نیازبهبررسیToolStripMenuItem.Checked = false;
               
            }

            RefreshDtAndGoToEnd();
        }

        private void تعدادقابلنمایشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDtAndGoToEnd();
        }

        private void بهروزرسانیBSCRNCToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ProcessExcelAndDatabase(string filePath)
        {
            // 1. ایجاد آبجکت Workbook دو اکسپرس و بارگذاری فایل
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filePath);

            // 2. چک کردن وجود شیت‌های BSC و RNC
            var sheetBSC = workbook.Worksheets.FirstOrDefault(s => s.Name.Equals("BSC", StringComparison.OrdinalIgnoreCase));
            var sheetRNC = workbook.Worksheets.FirstOrDefault(s => s.Name.Equals("RNC", StringComparison.OrdinalIgnoreCase));

            if (sheetBSC == null || sheetRNC == null)
            {
                throw new Exception("فایل اکسل انتخاب شده فاقد شیت‌های 'BSC' یا 'RNC' می‌باشد.");
            }

            // 3. اتصال به دیتابیس
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // ---------------------------------------------------------
                // *** شروع قسمت اصلاح شده: بررسی و ایجاد ستون در صورت نبودن ***
                // ---------------------------------------------------------
                string checkColQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblDb' AND COLUMN_NAME = 'BSC&RNC'";
                using (SqlCommand checkCmd = new SqlCommand(checkColQuery, conn))
                {
                    int colCount = (int)checkCmd.ExecuteScalar();
                    if (colCount == 0)
                    {
                        // اگر ستون نبود، آن را ایجاد کن
                        string addColQuery = "ALTER TABLE [DB].[dbo].[tblDb] ADD [BSC&RNC] NVARCHAR(MAX)";
                        using (SqlCommand addCmd = new SqlCommand(addColQuery, conn))
                        {
                            addCmd.ExecuteNonQuery();
                        }

                        // نمایش پیام به کاربر
                        MessageBox.Show("فیلد [BSC&RNC] در دیتابیس وجود نداشت. سیستم آن را ایجاد کرد و عملیات ادامه می‌یابد.",
                                        "ایجاد خودکار فیلد",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
                // ---------------------------------------------------------
                // *** پایان قسمت اصلاح شده ***
                // ---------------------------------------------------------

                // خواندن تمام کدهای سایت برای پردازش
                string selectQuery = "SELECT [کد سایت] FROM [DB].[dbo].[tblDb]";

                DataTable dtSites = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                {
                    adapter.Fill(dtSites);
                }

                // آماده‌سازی دستور Update
                string updateQuery = "UPDATE [DB].[dbo].[tblDb] SET [BSC&RNC] = @ResultValue WHERE [کد سایت] = @SiteCode";

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    // تعریف پارامترها
                    updateCmd.Parameters.Add("@ResultValue", SqlDbType.NVarChar);
                    updateCmd.Parameters.Add("@SiteCode", SqlDbType.NVarChar);

                    // 4. حلقه روی تمام ردیف‌های دیتابیس
                    foreach (DataRow row in dtSites.Rows)
                    {
                        string siteCode = row["کد سایت"].ToString();
                        string finalValue = "";
                        bool foundAny = false;

                        // --- پردازش شیت BSC ---
                        string bscValue = FindValueInSheet(sheetBSC, siteCode, "SITE", "BSC");
                        if (!string.IsNullOrEmpty(bscValue))
                        {
                            finalValue = bscValue;
                            foundAny = true;
                        }

                        // --- پردازش شیت RNC ---
                        string rncValue = FindValueInSheet(sheetRNC, siteCode, "SITE", "RNC");
                        if (!string.IsNullOrEmpty(rncValue))
                        {
                            if (foundAny)
                            {
                                finalValue += "," + rncValue;
                            }
                            else
                            {
                                finalValue = rncValue;
                            }
                        }

                        // 5. آپدیت دیتابیس
                        updateCmd.Parameters["@ResultValue"].Value = (object)finalValue ?? DBNull.Value;
                        updateCmd.Parameters["@SiteCode"].Value = siteCode;

                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private string FindValueInSheet(DevExpress.Spreadsheet.Worksheet sheet, string searchKey, string keyColumnHeader, string valueColumnHeader)
        {
            // تغییر Range به CellRange
            DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();

            int keyColIndex = -1;
            int valColIndex = -1;

            // جستجوی هدرها در ردیف اول
            for (int col = usedRange.LeftColumnIndex; col <= usedRange.RightColumnIndex; col++)
            {
                string headerText = sheet.Cells[usedRange.TopRowIndex, col].Value.ToString();

                if (headerText.Equals(keyColumnHeader, StringComparison.OrdinalIgnoreCase))
                    keyColIndex = col;

                if (headerText.Equals(valueColumnHeader, StringComparison.OrdinalIgnoreCase) ||
                   (valueColumnHeader == "BSC" && headerText.Contains("BSC")) ||
                   (valueColumnHeader == "RNC" && headerText.Contains("RNC")))
                {
                    valColIndex = col;
                }
            }

            if (keyColIndex == -1 || valColIndex == -1) return null;

            // متد Search مجموعه‌ای از Cell برمی‌گرداند
            IEnumerable<DevExpress.Spreadsheet.Cell> foundCells = sheet.Search(searchKey);

            foreach (DevExpress.Spreadsheet.Cell cell in foundCells)
            {
                // مطمئن می‌شویم که مقدار پیدا شده در ستون کلید (SITE) است
                if (cell.ColumnIndex == keyColIndex)
                {
                    // مقدار سلول هم‌ردیف در ستون مورد نظر را برمی‌گرداند
                    return sheet.Cells[cell.RowIndex, valColIndex].Value.ToString();
                }
            }

            return null;
        }

        private void بهروزرسانیBSCRNCToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            #region خواندن فایل اکسل
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Select the BSC/RNC Excel File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ProcessExcelAndDatabase(openFileDialog.FileName);
                        RefreshNewDbGrid();
                        MessageBox.Show("عملیات با موفقیت انجام شد", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("خطا در اجرا: \n" + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            #endregion

        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshNewDbGrid();
        }
    }
}










