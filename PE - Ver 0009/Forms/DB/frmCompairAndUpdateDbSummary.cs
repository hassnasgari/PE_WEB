
using DevExpress.CodeParser;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PE.Forms.DB
{
    public partial class frmCompairAndUpdateDbSummary : Form
    {
        ClsSql objClsSql ;//ایجاد شیی از روی کلاس  اس کیو ال
        #region متغییرها

        #region تعریف یک آرایه با نام تمام شیتها مورد انتظار برای دیتابیس 

        string[] arrExceptedSeets = new string[] {
            "Worksheet"
        };

        #endregion

        bool bolHasEqualSheet = false;
        bool bolHasEqualColumnInAllSheets = false;
        Boolean bolRsltOfCompairIsOk = false;

        #endregion

        string StrDbXclFileNameWithPathForUpdateDb;

        public frmCompairAndUpdateDbSummary()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void frmCompairAndUpdateDbSummary_Load(object sender, EventArgs e) 
        {
            MainForm.bolIsCompleteUpdateDb = false;

            stepProgressBar1.SelectNext();

            #region خواندن فایل اکسل دیتابیس جدید
            openFileDialogXclDb.DefaultExt = "*.Xlsx";

            this.openFileDialogXclDb.ShowDialog();

            StrDbXclFileNameWithPathForUpdateDb = this.openFileDialogXclDb.FileName;

            if (StrDbXclFileNameWithPathForUpdateDb == string.Empty)
            {
                MessageBox.Show("هیچ فایلی انتخاب نشده لطفا برنامه را بسته و مجددا اجرا نمایید");
                this.Close();
            }
            else
            {

                #region تبدیل فرمت اکسل به تکست

                clsExcel.ReadExcelRewriteItTextFormat(StrDbXclFileNameWithPathForUpdateDb);

                #endregion

                stepProgressBar1.SelectNext();//رفتن به مرحله بررسی وجود شیتهای مورد نیاز

                #region بررسی وجود شیتهای مورد نیاز
                bolHasEqualSheet = CompairNewSheetsWithExceptedSheets();
                #endregion

                if (bolHasEqualSheet)
                {
                    stepProgressBar1.SelectNext();//رفتن برای مقایسه ستونها

                    #region چک تک تک ستون های مورد انتظار در تک تک شیتها 
                    bolHasEqualColumnInAllSheets = CompairColumnInDesierdNewSheetsInNewXcl(); //چک تک تک ستون های مورد انتظار در تک تک شیتها 
                    #endregion

                    #region اگر ستونهای تمام شیتها هم مساویند
                    if (bolHasEqualColumnInAllSheets)
                    {
                       // MessageBox.Show(" ستون های تمامی شیت های مورد نظر برابرند ", "بررسی ستون های  مورد نیاز یافت نشده ");
                        bolRsltOfCompairIsOk = true;

                        stepProgressBar1.SelectNext();//رفتن به مرحله بهد به معنای مغایرتی نیست

                        UpdateDb();//بروز رسانی مختصر

                        MainForm.bolIsCompleteUpdateDb= true;

                       
                        
                        stepProgressBar1.SelectNext();//بروز رسانی با اس کیو موفقیت انجام شد
                        
                        #region تاخیر 4 ثانیه ای و بستن صفحه
                        Thread.Sleep(4000);
                        this.Close(); 
                        #endregion
                    }
                    #endregion

                    #region اگر ستونهای  شیتها  مساوی نیستند
                    else
                    {
                        MessageBox.Show(" ستون های تمامی شیت های مورد نظر برابرنیستند لطفا مغایرتها را بررسی نمایید ", "بررسی ستون های  مورد نیاز یافت نشده ");
                        bolRsltOfCompairIsOk = false;
                    }
                    #endregion
                }
                else
                {
                    string strMsg = " لطفا مغایرت شیتها را برطرف کنید و مجددا فایل اکسل دیتابیس را بارگذاری نمایید ";
                    MessageBox.Show(strMsg, "گزارش شیت های مورد انتظار و ناموجود", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                }

            }

            #endregion
        }

        private bool CompairNewSheetsWithExceptedSheets()
        {
            bool bolIsEqualSheetsAndColumns = false;

            #region استخراج آرایه شیتهای فایل اکسل خوانده شده  جدید
            List<string> lstOfReadedXclFileName = clsExcel.ExtractSheeName(StrDbXclFileNameWithPathForUpdateDb);//استخراج لیست شیتهای فایل اکسل جدید

            string[] arrOfReadedXclFileName = lstOfReadedXclFileName.ToArray();
            #endregion

            #region استخراج شیتهای مورد نیاز در جدید و در صورت وجود همه رفتن به مرحله مقایسه ستونها
            string[] arrDeletedSheet = arrExceptedSeets.Except(arrOfReadedXclFileName).ToArray();

            if (arrDeletedSheet.Length > 0)
            {
                string strDeletedSheet = "";
                string strTemp = "";
                foreach (string str in arrDeletedSheet)
                {
                    strDeletedSheet += str + "\r\n";
                }
                if (arrDeletedSheet.Length == 1)
                {
                    strTemp = "  :  شیت مورد نیاز یافت نشده ";
                }
                else
                {
                    strTemp = "  : لیست شیت های مورد نیاز یافت نشده ";
                }
                MessageBox.Show(strTemp + "\r\n\n" + strDeletedSheet);
            }
            else
            {
                bolIsEqualSheetsAndColumns = true;
                //MessageBox.Show(" شیتی کم نداریم ", "بررسی شیت های  مورد نیاز یافت نشده ");


            }

            #endregion

            return bolIsEqualSheetsAndColumns;
        }

        private bool CompairColumnInDesierdNewSheetsInNewXcl()
        {
            #region تعریف متغییرها

            bool bolAreEqualColumnInAllGSheets = false;
            bool bolAreEqualColumnInLocationSheet = false;

            #region تعریف آرایه ای از ستونها برای تک تک شیت ها
            string[] arrExceptedCulumnInGSheets = new string[] {
            "کد سایت",
            "نام سایت",
            "باند فرکانسی فعال",
            "طول جغرافیایی",
            "عرض جغرافیایی",
            "BSC/RNC"
             };
            #endregion

            #endregion

            #region مقایسه ستون های جداول هشت گانه ی جی

            foreach (var itemArr in arrExceptedSeets)
            {
               

                #region استخراج ستونهای شیت انتخابی فایل دیتابیس اکسل توسط حلقه  

                string[] arrColumnsOfSelectedSheet = clsExcel.ExtractColumns(itemArr, StrDbXclFileNameWithPathForUpdateDb);

                #endregion

                #region مقایسه ستونهای شیت انتخابی استخراج شده با ستونهای مورد انتظار

                string[] arrOfLostColumnInOfXclFile = arrExceptedCulumnInGSheets.Except(arrColumnsOfSelectedSheet).ToArray();


                if (arrOfLostColumnInOfXclFile.Length == 1)//اگه یکی کم شده
                {
                    string strMsg = "ستون مورد انتظار" + "\r\n\n " + arrOfLostColumnInOfXclFile[0] + "\r\n\n " + "  از شیت" + "\r\n\n " + itemArr + "\r\n\n " + "در فایل خوانده شده وجود ندارد";
                    MessageBox.Show(strMsg, "گزارش ستون حذف شده در شیت ", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    bolAreEqualColumnInAllGSheets = false;
                    return (false);
                }
                else if (arrOfLostColumnInOfXclFile.Length > 1)//اگه چند تا کم شده
                {
                    string strMsg = "ستون های مورد انتظار" + "\r\n\n " + string.Join("\n", arrOfLostColumnInOfXclFile) + "\r\n\n " + "  از شیت" + "\r\n\n " + itemArr + "\r\n\n " + "در فایل خوانده شده وجود ندارد";
                    MessageBox.Show(strMsg, "گزارش ستون های حذف شده", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                    bolAreEqualColumnInAllGSheets = false;
                    return (false);
                }
                else if (arrOfLostColumnInOfXclFile.Length == 0)
                {
                    bolAreEqualColumnInAllGSheets = true;
                }

                #endregion
            }

            #endregion

            if ( bolAreEqualColumnInAllGSheets)
                return true;
            else
                return false;
        }

        private bool UpdateDb()
        {

            #region بروز رسانی

                this.Cursor = Cursors.WaitCursor;

                #region حذف دیتابیس  قبلی
                string strSqlForDelLastDbSummarty = @"USE master
                                                            alter database " + "DB" + @" set single_user with rollback immediate;
                                                    DROP DATABASE " + "DB" + @";";
                objClsSql.SendCommand(strSqlForDelLastDbSummarty);
                #endregion

                #region ایجاد یک دیتابیس   
                string strSqlForCreateDb = @"Create DATABASE " + "DB" + " COLLATE Arabic_CI_AS";

                bool bolIsCreateDataBase = objClsSql.SendCommand(strSqlForCreateDb);

                if (!bolIsCreateDataBase)
                {
                    MessageBox.Show("دیتابیس  قبلی حذف و ایجاد دیتابیس  خالی با مشکل مواجه شد لطفا بررسی نمایید");
                    return false;
                }
                #endregion

                #region تغییر فرمت تمام فایل به تکست
                clsExcel.ReadExcelRewriteItTextFormat(StrDbXclFileNameWithPathForUpdateDb);
                    #endregion

                #region پر کردن دیتابیس خالی با فایل اکسل
                MainForm.dtDb = clsExcel.ReadExcelFileSendToDt("Worksheet", StrDbXclFileNameWithPathForUpdateDb);

                foreach (DataRow row in MainForm.dtDb.Rows)
                {
                    row["منطقه"] = DBNull.Value; 
                }
            #endregion

                #region حذف فیلدهای اضافه دیتاتیبل
                //MainForm.dtDb.Columns.Remove("منطقه");//چون منطقه تهران به درد ما نمی خوره
                MainForm.dtDb.Columns.Remove("استان");
                MainForm.dtDb.Columns.Remove("شهر مخابراتی");
                #endregion

                #region اضافه کردن فیلدهای تکنولوژی مورد نیاز دیتاتیبل
                MainForm.dtDb.Columns.Add("2G900", typeof(String));
                MainForm.dtDb.Columns.Add("2G1800", typeof(String));
                MainForm.dtDb.Columns.Add("3G900", typeof(String));
                MainForm.dtDb.Columns.Add("3G2100", typeof(String));
                MainForm.dtDb.Columns.Add("4G900", typeof(String));
                MainForm.dtDb.Columns.Add("4G1800", typeof(String));
                MainForm.dtDb.Columns.Add("4G2100", typeof(String));
                MainForm.dtDb.Columns.Add("4G2600", typeof(String));
                MainForm.dtDb.Columns.Add("پیمانکار", typeof(String));
                MainForm.dtDb.Columns.Add("مالک", typeof(String));
                MainForm.dtDb.Columns.Add("شماره تماس مالک", typeof(String));
                MainForm.dtDb.Columns.Add("اولویت", typeof(String));
            #endregion

            #region اضافه کردن فیلدهای  مورد نیاز دیتابیس باطری کولر
            //dtBatteryCooler.Columns.Add("کد سایت", typeof(String));
           MainForm.dtDb.Columns.Add("تعداد راک رکتی فایر", typeof(String));
           MainForm.dtDb.Columns.Add("تعداد ماژول رکتی فایر", typeof(String));
           MainForm.dtDb.Columns.Add("Rectifier - نوع رکتی فایر", typeof(String));
           MainForm.dtDb.Columns.Add("Rectifier - تاریخ تعویض رکتی فایر", typeof(String));
           MainForm.dtDb.Columns.Add("Rectifier - Description", typeof(String));
           MainForm.dtDb.Columns.Add("تعداد راک باتری", typeof(String));
           MainForm.dtDb.Columns.Add("نوع باتری ها", typeof(String));
           MainForm.dtDb.Columns.Add("Batteries Bank - تعداد باتری", typeof(String));
           MainForm.dtDb.Columns.Add("Batteries Bank - تاریخ تعویض باتری", typeof(String));
           MainForm.dtDb.Columns.Add("Batteries Bank - کارخانه سازنده باتری", typeof(String));
           MainForm.dtDb.Columns.Add("Batteries Bank - مشخصات باتری ها", typeof(String));
           MainForm.dtDb.Columns.Add("Batteries Bank - طول عمر باتری (ماه)", typeof(String));
           MainForm.dtDb.Columns.Add("تعداد کولر", typeof(String));
           MainForm.dtDb.Columns.Add("اطلاعات سرمایش - کارخانه سازنده کولر", typeof(String));
           MainForm.dtDb.Columns.Add("اطلاعات سرمایش - نوع کولر", typeof(String));
           MainForm.dtDb.Columns.Add("اطلاعات سرمایش - تاریخ تعویض کولر", typeof(String));
           MainForm.dtDb.Columns.Add("اطلاعات سرمایش - Description", typeof(String));
            
            #endregion

            #region تحلیل فیلد شلوغ پلوغ و پر کردن فیلدهای جدید
            foreach (DataRow row in MainForm.dtDb.Rows)
                 {

                    row.SetField("منطقه", DBNull.Value); // خالی کردن اطلاعات فیلد منطقه برای پر کردنش با اطلاعات خودمون

                    string strBand = row["باند فرکانسی فعال"].ToString();

                    if (strBand.Contains("2G900"))
                    {
                        row["2G900"] = "ES1G" + row["کد سایت"].ToString().Replace("ES", "");
                    }
                    if (strBand.Contains("2G1800"))
                    {
                        row["2G1800"] = "ES2G" + row["کد سایت"].ToString().Replace("ES", "");
                    }

                    if (strBand.Contains("3G900"))
                    {
                        row["3G900"] = "ES3G" + row["کد سایت"].ToString().Replace("ES", "");
                    }

                    if (strBand.Contains("3G2100"))
                    {
                        row["3G2100"] = "ES3G" + row["کد سایت"].ToString().Replace("ES", "");
                    }

                    if (strBand.Contains("4G900"))
                    {
                        row["4G900"] = "ES4G" + row["کد سایت"].ToString().Replace("ES", "");
                    }

                    if (strBand.Contains("4G1800"))
                    {
                        row["4G1800"] = "ES4G" + row["کد سایت"].ToString().Replace("ES", "");
                    }

                    if (strBand.Contains("4G2100"))
                    {
                        row["4G2100"] = "ES4G" + row["کد سایت"].ToString().Replace("ES", "");
                    }

                    if (strBand.Contains("4G2600"))
                    {
                        row["4G2600"] = "ES4G" + row["کد سایت"].ToString().Replace("ES", "");
                    }
                }
                 #endregion

                //#region تغییر نام ستونهای دیتاتیبل
                //MainForm.dtDb.Columns["کد سایت"].ColumnName = "لوکیشن";
                //#endregion

                #region تغییر ترتیب ستونهای دیتاتیبل
                MainForm.dtDb.Columns["کد سایت"].SetOrdinal(0);
                MainForm.dtDb.Columns["نام سایت"].SetOrdinal(1);
                MainForm.dtDb.Columns["پیمانکار"].SetOrdinal(2);
                MainForm.dtDb.Columns["BSC/RNC"].SetOrdinal(3);
                //MainForm.dtDb.Columns["BSC&RNC"].SetOrdinal(4);
                MainForm.dtDb.Columns["منطقه"].SetOrdinal(5);
                MainForm.dtDb.Columns["آدرس"].SetOrdinal(6);
                MainForm.dtDb.Columns["اولویت"].SetOrdinal(7);
                MainForm.dtDb.Columns["2G900"].SetOrdinal(8);
                MainForm.dtDb.Columns["2G1800"].SetOrdinal(9);
                MainForm.dtDb.Columns["3G900"].SetOrdinal(10);
                MainForm.dtDb.Columns["3G2100"].SetOrdinal(11);
                MainForm.dtDb.Columns["4G900"].SetOrdinal(12);
                MainForm.dtDb.Columns["4G1800"].SetOrdinal(13);
                MainForm.dtDb.Columns["4G2100"].SetOrdinal(14);
                MainForm.dtDb.Columns["4G2600"].SetOrdinal(15);
                if (MainForm.dtDb.Columns.Contains("نوع سایت"))
                {
                    MainForm.dtDb.Columns["نوع سایت"].SetOrdinal(15);
                }
                MainForm.dtDb.Columns["طول جغرافیایی"].SetOrdinal(16);
                MainForm.dtDb.Columns["عرض جغرافیایی"].SetOrdinal(17);
                MainForm.dtDb.Columns["وضعیت نود"].SetOrdinal(18);
                MainForm.dtDb.Columns["مالک"].SetOrdinal(19);
                MainForm.dtDb.Columns["شماره تماس مالک"].SetOrdinal(20);
            
               
                //if (MainForm.dtDb.Columns.Contains("نوع نصب BTS"))
                //{
                //    MainForm.dtDb.Columns["نوع نصب BTS"].SetOrdinal(16);
                //}
                //if (MainForm.dtDb.Columns.Contains("تاریخ راه اندازی اولیه"))
                //{
                //    MainForm.dtDb.Columns["تاریخ راه اندازی اولیه"].SetOrdinal(16);
                //}
                //MainForm.dtDb.Columns["نوع سایت"].SetOrdinal(16);
                //MainForm.dtDb.Columns["نوع نصب BTS"].SetOrdinal(13);
                //MainForm.dtDb.Columns["تاریخ راه اندازی اولیه"].SetOrdinal(17);
               
               
                
                
                #endregion

                #region ثبت تاریخ بروز رسانی دیتابیس 

                ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfLastDbUp");

                ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfLastDbUp");
                #endregion

                #region محاسبه و اعلام آخرین بروز رسانی ها

                string strSqlForDetectLastUpdateDb = @"
                            USE PE;
                            SELECT [dateOfLastDbUp]
                            FROM tblUpdateDate

                                ";

                //string strDateLastUpdate = objClsSql.SendCommandGetStringResult(strSqlForDetectLastUpdateDb);
                string strDateLastUpdate = objClsSql.SendCommandGetStringResult(strSqlForDetectLastUpdateDb);

                MainForm.StrDbDateLastUpdatePersian = "نا مشخص";

                if (strDateLastUpdate != null && strDateLastUpdate != string.Empty && strDateLastUpdate != "نا مشخص")
                {
                    MainForm.StrDbDateLastUpdatePersian = ClsConvertDateHassan.convertDateTimeToPersionWithSlash(DateTime.Parse(strDateLastUpdate));
                }

                string strLastUpdate = " آخرین زمان بروز رسانی دیتابیس   " + MainForm.StrDbDateLastUpdatePersian + " می باشد  ";



                #endregion

                #region حذف جدول قدیم در صورت وجود

                bool bolIsDeletedTable=objClsSql.DeleteWhereExistTableInDataBase("[DB].[dbo].[tblDb]");

                #endregion

                #region ساخت جدول جدید در دیتابیس جدید
                string strSqlForCreateTblAras = clsExcel.CreateTableFromDt("[DB].[dbo].[tblDb]", MainForm.dtDb);

                objClsSql.SendCommand(strSqlForCreateTblAras);
            #endregion

                //#region یک جوری دو تا جدول پیمانکار و ارس رو  جوین کن و یک دیتا تیبل بده

                ////حواسم باشد با شکل جدید لوکیشن را کدسایت کنم
                //string strSqlForCreateViewJoin=@"SELECT DB.dbo.tblDbAras.*,PE.dbo.tblContractor.*
                //    FROM DB.dbo.tblDbAras
                //    JOIN PE.dbo.tblContractor ON DB.dbo.tblDbAras.[لوکیشن] = PE.dbo.tblContractor.[کد سایت]";

                //#endregion

                #region جایگذاری اطلاعات خوانده شده از اکسل در جدول دیتابیس جدید

                objClsSql.BulkInsertDataTable("[DB].[dbo].[tblDb]", MainForm.dtDb);
                    #endregion

                #region حل مشکل یا و کاف در دیتابیس ارس

                string strSqlForSolveYaAndKaf = @"
                            USE DB;  

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


                this.Cursor = Cursors.Default;

             
            #endregion

            return false;
        }

    }
}
