using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PE.Forms.DT
{
    public partial class frmTadakholBa : Form
    {
        bool bolIsCorrectDtNoFormat=false;
        public frmTadakholBa()
        {
            InitializeComponent();
        }

        private void frmTadakholBa_Load(object sender, EventArgs e)
        {
            txtRowCr.Select();
        }

        private void btnTadakholBa_Click(object sender, EventArgs e)
        {
            if(txtRowCr.Text!="")
            {
                MainForm.strDtNoteTadakholBa=txtRowCr.Text.ToString();
            }
            else
            {
                MessageBox.Show("اخطار", "هیچ شماره ای برای تداخل  مشخص نشده", MessageBoxButtons.OK,

                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            this.Close();
          
        }


        private void txtRow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
                btnTadakholBa.PerformClick();
            }
         
        }

        private void radioGroup1_Properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRowCr.Clear();
            txtRowCr.Focus();
        }

        private void txtRowCr_TextChanged(object sender, EventArgs e)
        {
            if(radioGroupTypeOfInterfernce.SelectedIndex==0)
            {
                string userInput = txtRowCr.Text; // Replace with your textbox value
                string pattern = @"^\d{6}$"; // Matches exactly six digits

                // Check if the input matches the pattern
                bool isMatch = Regex.IsMatch(userInput, pattern);

                // Output the result
                if (isMatch)
                {
                    txtRowCr.ForeColor = Color.Green;
                    bolIsCorrectDtNoFormat = true;
                    return;
                }
                else
                {
                    txtRowCr.ForeColor = Color.Red;
                    bolIsCorrectDtNoFormat = false;

                    MessageBox.Show("اخطار", "    تعداد کاراکتر شماره یا فرمت ردیف صحیح نمی باشد. ردیف باید شش رقم باشد ", MessageBoxButtons.OK,

                                      MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                                     MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                }

            }
            else if (txtRowCr.Text.Contains("MCI-CR-"))
            {
                txtRowCr.Text = txtRowCr.Text.Replace("MCI-CR-", string.Empty);
            }

            Match match = Regex.Match(txtRowCr.Text.Trim(), @"202\d\d\d\d\d-\d\d\d\d\d");
            if ((match.Success && txtRowCr.Text.Count() == 14) || txtRowCr.Text.Count() == 0)
            {
                txtRowCr.ForeColor = Color.Green;
                bolIsCorrectDtNoFormat = true;
                // chbDtChgGrpDtNo.Checked = true;
            }
            else
            {
                bolIsCorrectDtNoFormat = false;
                txtRowCr.ForeColor = Color.Red;
                // chbDtChgGrpDtNo.Checked = false;


                MessageBox.Show("اخطار", "  تعداد کاراکتر شماره یا فرمت شماره دان تایم صحیح نمی باشد ", MessageBoxButtons.OK,

                                       MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                                      MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            }
        }
    }
}
