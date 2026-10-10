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
    public partial class Logs : Form
    {
       
         ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        public Logs()
        {
            InitializeComponent();
            objClsSql = new ClsSql();
        }

        private void Logs_Load(object sender, EventArgs e)
        {
            comboBoxLog.SelectedIndex = 0;//انتخاب اولین آیتم همان 50
            
            FillLogGrid(false);
        }

        private void FillLogGrid(bool bolIsDateLog)
        {
            string strCmd = "";

            if (!bolIsDateLog)
            {
                if (comboBoxLog.SelectedItem.ToString() != "همه")
                {
                    strCmd = @"SELECT  Top(" + comboBoxLog.SelectedItem.ToString() + @")[id]
                 ,[action]
                  ,[userName]
                  ,[date]
                  ,[time]
                  ,[mac]
                  ,[computerName]
                  ,[SelectedNet]
                  ,[IP]
              FROM [PE].[dbo].[tblLog]
              order by id DESC; ";
                }
                else
                {
                    strCmd = @"SELECT  [id]
                 ,[action]
                  ,[userName]
                  ,[date]
                  ,[time]
                  ,[mac]
                  ,[computerName]
                  ,[SelectedNet]
                  ,[IP]
              FROM [PE].[dbo].[tblLog]
              order by id DESC; ";
                }
            }
            else
            {
                strCmd = @"SELECT [id]
                 ,[action]
                  ,[userName]
                  ,[date]
                  ,[time]
                  ,[mac]
                  ,[computerName]
                  ,[SelectedNet]
                  ,[IP]
              FROM [PE].[dbo].[tblLog]
               WHERE( LTRIM(RTRIM(SUBSTRING([DATE] ,LEN([DATE])-10,12)))>=N'" + hM_FarsiCalendarLogFrom.Text.Trim()+ @"'
                AND
            LTRIM(RTRIM(SUBSTRING([DATE] ,LEN([DATE])-10,12)))<=N'" + hM_FarsiCalendarLogTo.Text.Trim() + @"')
              order by id DESC; ";
            }
            

            gridControlLogAction.DataSource = objClsSql.FillDataTableFromQuery(strCmd);

            this.gridViewLogAction.Columns["id"].Width = 15;
            this.gridViewLogAction.Columns["userName"].Width = 30;
            this.gridViewLogAction.Columns["action"].Width = 90;
            this.gridViewLogAction.Columns["date"].Width = 60;
            this.gridViewLogAction.Columns["time"].Width = 30;
            this.gridViewLogAction.Columns["mac"].Width = 45;
            this.gridViewLogAction.Columns["computerName"].Width = 35;
            this.gridViewLogAction.Columns["SelectedNet"].Width = 40;
            this.gridViewLogAction.Columns["IP"].Width = 40;
            gridViewLogAction.MoveLast();

            gridViewLogAction.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);
        }

        private void radioButtonByNumbers_CheckedChanged(object sender, EventArgs e)
        {
            ActionOfChangeTypeOfLog();
        }

        private void ActionOfChangeTypeOfLog()
        {
            if (radioButtonByNumbers.Checked)
            {
                panelLogByNumber.Enabled = true;
                panelLogByDate.Enabled = false;
            }
            else
            {
                panelLogByNumber.Enabled = false;
                panelLogByDate.Enabled = true;
            }
        }

        private void comboBoxLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillLogGrid(false);
        }

        private void btnLogDateCreate_Click(object sender, EventArgs e)
        {
            FillLogGrid(true);
        }
    }
}
  