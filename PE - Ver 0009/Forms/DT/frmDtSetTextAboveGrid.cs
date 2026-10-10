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
    public partial class frmDtSetTextAboveGrid : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public frmDtSetTextAboveGrid()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void frmDtSetTextAboveGrid_Load(object sender, EventArgs e)
        {
            txtDtUnfreez.Text= MainForm.strDtAboveGridWhenNotFreez = objClsSql.SendCommandGetStringResult("SELECT messageForDtGrid FROM DT.dbo.tblUnFreezeMessage Where row=1");
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
           objClsSql.EditRecord("DT.dbo.tblUnFreezeMessage", "messageForDtGrid", txtDtUnfreez.Text, " Where row=1");
            txtDtUnfreez.Text = MainForm.strDtAboveGridWhenNotFreez = objClsSql.SendCommandGetStringResult("SELECT messageForDtGrid FROM DT.dbo.tblUnFreezeMessage Where row=1");

            
            objClsSql.SendCommand("Update [DT].[dbo].[tblUnFreezeMessage] Set messageColor=" + MainForm.ClrMessageForTextAboveDtGrid.ToArgb() + " WHERE row=1");


            MainForm.BolIsNewMessageForAboveDtGrid = true;
            this.Close();
        }

        private void رنگپیامToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialogSetTextAboveGrid.ShowDialog();

            if (!colorDialogSetTextAboveGrid.Color.IsEmpty)
            {
                MainForm.ClrMessageForTextAboveDtGrid = colorDialogSetTextAboveGrid.Color;
            }

        }

        private void تنظیماتپیامToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
