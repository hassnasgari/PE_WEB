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
    public partial class frmVpnsAdd : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public frmVpnsAdd()
        {
            InitializeComponent();
            objClsSql=new ClsSql();//ایجاد شیی از روی کلاس  اس کیو ال
        }

        private void frmVpnsAdd_Load(object sender, EventArgs e)
        {
            txtVpnForAdd.Focus();
        }

        private void btnAddVpn_Click(object sender, EventArgs e)
        {

            #region چک کامل بودن هر دو تکس باکس
           if( txtVpnForAdd.Text=="" || txtVpnOwner.Text == "")
            {
                if (txtVpnForAdd.Text == "")
                {
                    MessageBox.Show("لطفا نام وی پی ان را کامل نمایید", "آلارم", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                    txtVpnForAdd.Focus();
                    return;
                }
                if (txtVpnOwner.Text == "")
                {
                    MessageBox.Show("لطفا مالک وی پی ان را کامل نمایید", "آلارم", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                    txtVpnOwner.Focus();
                    return;
                }
            }

            #endregion


            #region اضافه کردن به دیتابیس

            objClsSql.GetAllAndSendToDataBase("PE.dbo.tblExsitVpn", "VpnName,VpnOwner,isConnected","N'"+ txtVpnForAdd.Text+"',N'"+ txtVpnOwner.Text+"',"+ 0);

            MessageBox.Show("وی پی ان جدید اضافه گردید", "آلارم", MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

            this.Close();


            #endregion


        }
    }
}
