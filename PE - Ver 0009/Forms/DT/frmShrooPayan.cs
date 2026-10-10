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
    public partial class frmShrooPayan : Form
    {
        public frmShrooPayan()
        {
            InitializeComponent();
        }

        private void btnOkShrooAz_Click(object sender, EventArgs e)
        {

            MainForm.strDtNoteShoroo= ClsDiff.EzafeyeSefreAvval(numericUpDownShorooazHour.Text) + ":" +  ClsDiff.EzafeyeSefreAvval(numericUpDownShorooazMinute.Text);
            MainForm.strDtNotePayan = ClsDiff.EzafeyeSefreAvval(numericUpDownPayanHour.Text) + ":" + ClsDiff.EzafeyeSefreAvval(numericUpDownPayanMinute.Text);

            MainForm.strDtPershianNoteShoroo = ClsDiff.EzafeyeSefreAvval(numericUpDownShorooazMinute.Text) + ":" + ClsDiff.EzafeyeSefreAvval(numericUpDownShorooazHour.Text);
            MainForm.strDtPershianNotePayan = ClsDiff.EzafeyeSefreAvval(numericUpDownPayanMinute.Text)  + ":" + ClsDiff.EzafeyeSefreAvval(numericUpDownPayanHour.Text);


            this.Close();
        }
    }
}
