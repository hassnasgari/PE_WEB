using PE.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE
{
    public partial class frmSeEmReprotBeetweenTwoDate : Form
    {
        
        public frmSeEmReprotBeetweenTwoDate()
        {
            InitializeComponent();
        }

        private void BtnFrom_Click(object sender, EventArgs e)
        {

            frmSelectDate frmTchDate = new frmSelectDate();
            frmTchDate.ShowDialog();

            txtFromDateSeEm.Text = MainForm.StrSelectedPershianDateWithSlashSeperator.Replace("/","-");
            MainForm.strSeEmStartSeEmReportDate = "";
            MainForm.strSeEmStartSeEmReportDate = txtFromDateSeEm.Text;

            if (txtFromDateSeEm.Text.Length == 10)
            {
                btnTo.Enabled = true;
            }
            else
            {
                MessageBox.Show("لطفا تاریخ شروع گزارش را مشخص کنید");
            }
        }

        private void BtnTo_Click(object sender, EventArgs e)
        {
            frmSelectDate frmTchDate = new frmSelectDate();
            frmTchDate.ShowDialog();

            txtToDateSeEm.Text = MainForm.StrSelectedPershianDateWithSlashSeperator.Replace("/", "-");
            MainForm.strSeEmEndSeEmReportDate = txtToDateSeEm.Text;
            btnCreateSeEmReport.Enabled = true;
        }

        private void TxtFromDateSeEm_Leave(object sender, EventArgs e)
        {
           
        }

        private void TxtToDateSeEm_Leave(object sender, EventArgs e)
        {

            MainForm.strSeEmEndSeEmReportDate = txtToDateSeEm.Text;
        }

        private void BtnCreateSeEmReport_Click(object sender, EventArgs e)
        {
            
            if(txtFromDateSeEm.Text.Length!=10)
            {
                MessageBox.Show("تاریخ شروع گزارش را چک کنید");
                return;
            }
            else if (txtToDateSeEm.Text.Length != 10)
            {
                MessageBox.Show("تاریخ پایان گزارش را چک کنید");
                return;
            }
            else
            {
                DateTime dteStart = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(txtFromDateSeEm.Text);
                DateTime dteEnd = ClsConvertDateHassan.convertShamsiToMiladiReturnDate(txtToDateSeEm.Text);

                if(dteStart> dteEnd)
                {
                    MessageBox.Show("تاریخ پایان باید بزرگتر از تاریخ شروع باشد");
                    return;
                }

            }
            
          this.Close();
        }

        private void FrmSeEmReprotBeetweenTwoDate_Load(object sender, EventArgs e)
        {
            btnTo.Enabled = false;
            btnCreateSeEmReport.Enabled = false;
        }

        private void txtFromDateSeEm_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
