namespace PE.Forms
{
    using DevExpress.Data;
    using DevExpress.Utils;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using PE;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmUpdateDb : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        private Button btnUpDb;
        private IContainer components = null;
        private GridControl GcEEI;
        private GridView gridViewEEI;
        private Label lblInf;
        private OpenFileDialog openFileDialogDbWithSwappedSite;
        private OpenFileDialog openFileDialogXclDb;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        DataTable tableOfDB;

        public frmUpdateDb()
        {
            this.InitializeComponent();
        }
        private void btnUpDb_Click(object sender, EventArgs e)
        {

            this.openFileDialogXclDb.ShowDialog();

            string fileName = this.openFileDialogXclDb.FileName;
            tableOfDB = clsExcel.ReadExcelFileSendToDt("DB", fileName);

            this.GcEEI.DataSource = tableOfDB;

            objClsSql.DeleteAllRecords("[DB].[dbo].[tblCurrentDataBase]");

           objClsSql.BulkInsertDataTable("[DB].[dbo].[tblCurrentDataBase]", tableOfDB);

            

            string strSqlForDAteOfUpdateAras = @"
                    Update [PE].[dbo].[tblUpdateDate]
                    set [dateOfTblOldDb]=N'" + DateTime.Now + "'";

            
            objClsSql.SendCommand(strSqlForDAteOfUpdateAras);

            #region ثبت تاریخ بروز رسانی
            ClsDiff.SetDateOfUpdate("PE", "tblUpdateDate", "dateOfTblOldDb");
            #endregion


            ClsDiff.writeLogOfAction("دیتا بیس سایتهابا فایل اکسل "+ fileName + "بروز شد");

            MessageBox.Show(". دیتا بیس بروز شد ", "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

        } 

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmUpdateDb_Load(object sender, EventArgs e)
        {

            ClsDiff.writeLogOfAction("ورود به صفحه بروز رسانی دیتابیس");
        }

        private void InitializeComponent()
        {
            this.lblInf = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnUpDb = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.GcEEI = new DevExpress.XtraGrid.GridControl();
            this.gridViewEEI = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.openFileDialogXclDb = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogDbWithSwappedSite = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcEEI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEEI)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInf
            // 
            this.lblInf.AutoSize = true;
            this.lblInf.Location = new System.Drawing.Point(630, 0);
            this.lblInf.Name = "lblInf";
            this.lblInf.Size = new System.Drawing.Size(59, 13);
            this.lblInf.TabIndex = 14;
            this.lblInf.Text = "Information";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 307F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 518F));
            this.tableLayoutPanel3.Controls.Add(this.lblInf, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnUpDb, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1112, 34);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // btnUpDb
            // 
            this.btnUpDb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpDb.Location = new System.Drawing.Point(323, 3);
            this.btnUpDb.Name = "btnUpDb";
            this.btnUpDb.Size = new System.Drawing.Size(301, 28);
            this.btnUpDb.TabIndex = 0;
            this.btnUpDb.Text = "Browse Source Xcl File";
            this.btnUpDb.UseVisualStyleBackColor = true;
            this.btnUpDb.Click += new System.EventHandler(this.btnUpDb_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.GcEEI, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.841924F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.21839F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.147776F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.878049F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1118, 718);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // GcEEI
            // 
            this.GcEEI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GcEEI.Location = new System.Drawing.Point(3, 43);
            this.GcEEI.MainView = this.gridViewEEI;
            this.GcEEI.Name = "GcEEI";
            this.GcEEI.Size = new System.Drawing.Size(1112, 609);
            this.GcEEI.TabIndex = 6;
            this.GcEEI.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewEEI});
            // 
            // gridViewEEI
            // 
            this.gridViewEEI.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewEEI.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewEEI.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewEEI.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewEEI.AppearancePrint.EvenRow.Options.UseTextOptions = true;
            this.gridViewEEI.AppearancePrint.EvenRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewEEI.AppearancePrint.EvenRow.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewEEI.AppearancePrint.OddRow.Options.UseTextOptions = true;
            this.gridViewEEI.AppearancePrint.OddRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewEEI.AppearancePrint.OddRow.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewEEI.GridControl = this.GcEEI;
            this.gridViewEEI.GroupPanelText = "لطفا ستون یا ستونهایی را که می خواهید دسته بندی بر اساس آنها صورت گیرد به این قسم" +
    "ت بکشید.";
            this.gridViewEEI.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, null, null, "")});
            this.gridViewEEI.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridViewEEI.Name = "gridViewEEI";
            this.gridViewEEI.OptionsBehavior.SmartVertScrollBar = false;
            this.gridViewEEI.OptionsPrint.EnableAppearanceEvenRow = true;
            this.gridViewEEI.OptionsPrint.EnableAppearanceOddRow = true;
            this.gridViewEEI.OptionsView.ColumnAutoWidth = false;
            this.gridViewEEI.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewEEI.OptionsView.ShowAutoFilterRow = true;
            this.gridViewEEI.OptionsView.ShowFooter = true;
            this.gridViewEEI.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridViewEEI.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            // 
            // openFileDialogXclDb
            // 
            this.openFileDialogXclDb.FileName = "openFileDialogXclDb";
            // 
            // openFileDialogDbWithSwappedSite
            // 
            this.openFileDialogDbWithSwappedSite.FileName = "openFileDialog1";
            // 
            // frmUpdateDb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 721);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmUpdateDb";
            this.Text = "فرم بروز رسانی دیتابیس";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmUpdateDb_Load);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcEEI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEEI)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

