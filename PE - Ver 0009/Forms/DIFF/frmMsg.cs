using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE.Forms
{
    public partial class frmMsg : Form
    {
        public frmMsg()
        {
            InitializeComponent();
        }

        private void frmMsg_Load(object sender, EventArgs e)
        {
            lblFrmMessage.Text = MainForm.strMsg;
            this.Width = lblFrmMessage.Width + 10;
            this.Height = lblFrmMessage.Height + 60;
        }
    }
}
