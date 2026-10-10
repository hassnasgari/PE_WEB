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
    public partial class frmDtCteateAttachment : Form
    {
        DataTable dt = new DataTable();
        string strExcelPath = "";
        public frmDtCteateAttachment()
        {
            InitializeComponent();
        }

        private void خواندنفایلضمیمهاصلیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //txtCodeSite.Enabled = false;
            dt.Clear();
            gcDtAttach.DataSource = dt;

            openFileDialogDtAttach.ShowDialog();

          if(openFileDialogDtAttach.FileName!="")
            {
                strExcelPath = openFileDialogDtAttach.FileName;
                #region تبدیل فرمت اکسل به تکست

                clsExcel.ReadExcelRewriteItTextFormat(strExcelPath);

                #endregion
                

                dt = clsExcel.ReadExcelFileSendToDt("Database for CR", strExcelPath);

                gcDtAttach.DataSource = dt;

                txtCodeSite.Text = Clipboard.GetText();

                lblCnt.Text= (txtCodeSite.Text.Split(',').Length).ToString() + " عدد";

            }
          else
            {
                MessageBox.Show("هیچ فایلی انتخاب نشده", "اخطار", MessageBoxButtons.OK,MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            }

        }

        private void frmDtCteateAttachment_Load(object sender, EventArgs e)
        {
            
        }

        private void txtCodeSite_TextChanged(object sender, EventArgs e)
        {
            string strOr = "Location = '" + txtCodeSite.Text.Replace(",", "' OR  Location ='") + "'";

            DataView dv = new DataView(dt);
            dv.RowFilter = strOr;
            gcDtAttach.DataSource = dv;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] words = txtCodeSite.Text.Split(',');

            string strExportedFileName = words[0];


            gcDtAttach.ExportToXlsx(strExcelPath.Replace("Database For CR", strExportedFileName));

            MessageBox.Show("Exported To " + strExportedFileName );

            this.Close();
        }
    }
}
