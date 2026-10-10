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
    public partial class frmDtChangeAnnounceForDownTime : Form
    {
        public frmDtChangeAnnounceForDownTime()
        {
            InitializeComponent();
        }

        private void FrmDtChangeAnnounceForDownTime_Load(object sender, EventArgs e)
        {
            lblCurText.Text= MainForm.strDtTextForNewDownTime;

        }

        private void BtnChangeannounce_Click(object sender, EventArgs e)
        {
            MainForm.strDtTextForNewDownTime = txtNewStrForAnnounce.Text;


            MainForm.bolDtHaveChangeOnDtAnnounce= false;

            

            MessageBox.Show("پیام جدید اعمال گردید");
            this.Close() ;

        }
    }
}
