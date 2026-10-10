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
    public partial class frmDtChgRepStock : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public frmDtChgRepStock()
        {
            InitializeComponent();
        }

        private void frmDtChgRepStock_Load(object sender, EventArgs e)
        {
            objClsSql=new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال
            DataTable dtRegion= objClsSql.FillDataTableFromQuery("Use DT; Select regName From tblRegion");
            cbxRegion.DataSource = dtRegion;
            cbxRegion.DisplayMember = "regName";
            btnChgRepStock.Enabled = false;
        }

        private void cbxRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strRegion = cbxRegion.Text;

                #region انتخاب ماه سال فعلی
                string str = ClsConvertDateHassan.ConvertDateToPersian(MainForm.DateTimeSqlServer);
                string strCurYearMonth = str.Substring(0, 4) + "_" + str.Substring(5, 2);
                #endregion
                string strCmd = "Select " + strRegion + "  From [DT].[dbo].tblRepRemaindedRegions Where YearMonth=N'" + strCurYearMonth + "'";

                string  strRemaindedStock = objClsSql.SendCommandGetStringResult(strCmd);
                numRepStockUpDn.Value = int.Parse(strRemaindedStock);
            }
            catch(Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
        }

        private void numRepStockUpDn_ValueChanged(object sender, EventArgs e)
        {
            btnChgRepStock.Enabled = true;
            
            
            
                   
            

        }

        private void btnChgRepStock_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("میزان سهمیه به مقدار جدید تغییر یابد", "تایدیه", MessageBoxButtons.YesNo,

                  MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                 MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (result == DialogResult.Yes)
            {
                string strRegion = cbxRegion.Text;

                #region انتخاب ماه سال فعلی
                string str = ClsConvertDateHassan.ConvertDateToPersian(MainForm.DateTimeSqlServer);
                string strCurYearMonth = str.Substring(0, 4) + "_" + str.Substring(5, 2);
                #endregion


                bool bolIsUpdate =objClsSql.EditRecord("[DT].[dbo].tblRepRemaindedRegions", strRegion, numRepStockUpDn.Value.ToString(), " Where YearMonth=N'" + strCurYearMonth + "'");
                if (bolIsUpdate)
                {
                    
                    ClsDiff.AutoClosingMessageBox.Show("تغییرات خواسته شده در سهمیه اعمال شد", "Note", 4000);
                    
                    this.Close();
                }
                else
                    ClsDiff.AutoClosingMessageBox.Show("اشکال در اعمال سهمیه جدید", "Alarm", 4000);



              
                
            }
        }

        private void frmDtChgRepStock_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}

