using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE.Forms.DB
{
    public partial class frmDbExcleSheets : Form
    {
        public frmDbExcleSheets()
        {
            InitializeComponent();
        }

        private void لیستشیتهایقابلانتظارToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] strArrayOfExceptedSheetsName = { "Location", "2G - 900", "2G - 1800", "3G - 2100", "4G - 2600", "3G - 900", "4G - 1800", "4G - 900", "4G - 2100" };
            lstExceptedSheets.DataSource = strArrayOfExceptedSheetsName;
        }

        private void حذفشیتهایاضافهازاکسلToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
