using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE.Forms.DT
{
    public partial class frmDtShowRepStock : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        String strSqlForShowRemaidedRep = "";
        public frmDtShowRepStock()
        {
            InitializeComponent();
             objClsSql=new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال
        }

        private void FrmDtShowRepStock_Load(object sender, EventArgs e)
        {
            refreshGvDtShowStoksRep();

        }

        private void refreshGvDtShowStoksRep()
        {
            strSqlForShowRemaidedRep = @"USE DT;
             SELECT TOP 1000 [YearMonth]
              ,[North]
              ,[South]
              ,[West]
              ,[East]
               FROM [DT].[dbo].[tblRepRemaindedRegions]
               ORDER BY YearMonth
            ";


            #region نمایش سهمیه
           
            gridControlDtShowStoksRep.DataSource = null;
            gridControlDtShowStoksRep.Refresh();
            gridControlDtShowStoksRep.DataSource = objClsSql.FillDataTableFromQuery(strSqlForShowRemaidedRep);
            gridControlDtShowStoksRep.Refresh();
            #endregion

            #region انتخاب ماه سال فعلی
            string str = ClsConvertDateHassan.ConvertDateToPersian(MainForm.DateTimeSqlServer);
            string strCurYearMonth = str.Substring(0, 4) + "_" + str.Substring(5, 2);
            gvDtShowStoksRep.FocusedRowHandle = gvDtShowStoksRep.LocateByValue("YearMonth", strCurYearMonth);
            #endregion
        }

        //public virtual int LocateByValue(int startRowHandle GridColumn column, object val, params OperationCompleted[] completed)

        private void gvDtShowStoksRep_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "North":
                    e.Appearance.BackColor = Color.LightGreen;
                    break;
                case "South":
                    e.Appearance.BackColor = Color.Yellow;
                    break;
               
                case "West":
                    e.Appearance.BackColor = Color.Pink;
                    break;
                case "East":
                    e.Appearance.BackColor = Color.Beige;
                    break;

           
            }

           
               
           
        }

        private void تغییرسهمیهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDtChgRepStock objFrmDtChgRepStock = new frmDtChgRepStock();
            objFrmDtChgRepStock.ShowDialog();
            refreshGvDtShowStoksRep();

        }
    }
}
