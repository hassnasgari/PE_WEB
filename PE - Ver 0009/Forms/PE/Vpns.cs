using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE.Forms.PE
{
    public partial class Vpns : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public Vpns()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void Vpns_Load(object sender, EventArgs e)
        {
            UpdateVpnList();
        }

        private void UpdateVpnList()
        {
            #region  پر کردن دیتا تیبل وی پی ان  
            DataTable dtExistVpn = objClsSql.FillDataTableFromQuery("PE.dbo.tblExsitVpn");
            #endregion

            #region چک می کند آیا اصلا شبکه وصل داریم و اگر داریم کدام وی پی ان وصله آنرا در دیتاتیبل ویرایش می کند
            if (NetworkInterface.GetIsNetworkAvailable())//first If
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface Interface in interfaces)
                {
                    for (int i = 0; i < dtExistVpn.Rows.Count; i++)
                    {
                        DataRow row = dtExistVpn.Rows[i];

                        if (row.ItemArray[0].ToString() == Interface.Name)
                        {
                            dtExistVpn.Rows[i]["isConnected"] = true;
                        }
                    }
                }
            }//fi 
            #endregion

            #region نمایش جدول
            gridControlVpnExistList.DataSource = dtExistVpn;
            #endregion
        }

        private void اضافهکردنبهلیستموردنظارتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVpnsAdd objFrmVpnsAdd = new frmVpnsAdd();
            objFrmVpnsAdd.ShowDialog();
            UpdateVpnList();
        }

        private void بهروزرسانیجدولToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateVpnList();
        }

        private void حذفموردانتخابشدهازلیستنظارتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strVpnName = "";
            if (gridViewVpnExistList.SelectedRowsCount>0)
            {
                DialogResult dialogResultDelPersonal;

                DataRow row = gridViewVpnExistList.GetDataRow(gridViewVpnExistList.GetSelectedRows()[0]);
                strVpnName = row.ItemArray[0].ToString();

                dialogResultDelPersonal = MessageBox.Show("   مطمئن هستید ؟ " + strVpnName + "   برای حذف وی پی ان به نام  " , " تاییدیه حذف وی پی ان ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                if (dialogResultDelPersonal == DialogResult.Yes)
                {
                    try
                    {
                       objClsSql.DeleteRecords("[PE].[dbo].tblExsitVpn", "Where VpnName=N'" + strVpnName + "'");//حذف وی پی ان
                        ClsDiff.writeLogOfAction("حذف   وی پی ان به نام  "+  strVpnName);
                        UpdateVpnList();
                    }
                    catch
                    {
                        MessageBox.Show("اشکال در حذف وی پی ان ");
                        ClsDiff.writeLogOfAction("اشکال در حذف وی پی ان " + strVpnName);

                    }
                }
                else if (dialogResultDelPersonal == DialogResult.No)
                {

                }

            }
            else
            {           
                MessageBox.Show("هیچ سطری جهت حذف انتخاب نشده", "آلارم", MessageBoxButtons.OK,
               MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

            }
        }
    }
}
