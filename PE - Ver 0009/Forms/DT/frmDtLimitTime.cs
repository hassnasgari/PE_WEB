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
    public partial class frmDtLimitTime : Form
    {
        ClsSql objClsSql;
        public frmDtLimitTime()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void frmDtLimitTime_Load(object sender, EventArgs e)
        {
            string strTime= objClsSql.SendCommandGetStringResult("Select lastPermitTimeForDownTime from DT.dbo.tblLastPermitTimeForDt where id='1'");
            if (strTime.Length == 4 && strTime!= "نامشخص")
            {
                comboBoxDtLimitHour.Text = strTime.Substring(0, 2);
          
                comboBoxDtLimitMin.Text = strTime.Substring(2, 2);
            }
        }

        private void btnDtLimitReg_Click(object sender, EventArgs e)
        {
            if(comboBoxDtLimitHour.Text.Length==2 && comboBoxDtLimitMin.Text.Length == 2)
            {
                bool bolIsEditedLimitDtTime=objClsSql.EditRecord("DT.dbo.tblLastPermitTimeForDt", "lastPermitTimeForDownTime",comboBoxDtLimitHour.Text+comboBoxDtLimitMin.Text, "where id='1'");
                if (bolIsEditedLimitDtTime)
                {
                    MessageBox.Show("تغییرا ت با موفقیت انجام شد");
                }
                else
                {
                   
                    MessageBox.Show("متاسفانه تغییرات انجام نشد لطفا با مدیر سیستم تماس بگیرید");
                }
            }
            else
            {
                MessageBox.Show("فرمت ساعت یا دقیقه دستکاری شده لطفا از لیست انتخاب نمایید");
            }
        }
    }
}
