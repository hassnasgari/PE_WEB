
namespace PE.Forms.DB
{
    partial class frmCompairAndUpdateDbSummary
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompairAndUpdateDbSummary));
            this.groupBoxStageOfCompair = new System.Windows.Forms.GroupBox();
            this.stepProgressBar1 = new DevExpress.XtraEditors.StepProgressBar();
            this.stepProgressBarItem1 = new DevExpress.XtraEditors.StepProgressBarItem();
            this.stepProgressBarItem2 = new DevExpress.XtraEditors.StepProgressBarItem();
            this.stepProgressBarItem3 = new DevExpress.XtraEditors.StepProgressBarItem();
            this.stepProgressBarItem4 = new DevExpress.XtraEditors.StepProgressBarItem();
            this.stepProgressBarItem5 = new DevExpress.XtraEditors.StepProgressBarItem();
            this.stepProgressBarItem6 = new DevExpress.XtraEditors.StepProgressBarItem();
            this.openFileDialogXclDb = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxStageOfCompair.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stepProgressBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxStageOfCompair
            // 
            this.groupBoxStageOfCompair.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxStageOfCompair.Controls.Add(this.stepProgressBar1);
            this.groupBoxStageOfCompair.Location = new System.Drawing.Point(12, 27);
            this.groupBoxStageOfCompair.Name = "groupBoxStageOfCompair";
            this.groupBoxStageOfCompair.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxStageOfCompair.Size = new System.Drawing.Size(1045, 484);
            this.groupBoxStageOfCompair.TabIndex = 1;
            this.groupBoxStageOfCompair.TabStop = false;
            this.groupBoxStageOfCompair.Text = "مراحل بررسی اکسل خوانده شده با موارد مورد انتظار";
            // 
            // stepProgressBar1
            // 
            this.stepProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stepProgressBar1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.stepProgressBar1.Appearance.Options.UseForeColor = true;
            this.stepProgressBar1.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.stepProgressBar1.Items.Add(this.stepProgressBarItem1);
            this.stepProgressBar1.Items.Add(this.stepProgressBarItem2);
            this.stepProgressBar1.Items.Add(this.stepProgressBarItem3);
            this.stepProgressBar1.Items.Add(this.stepProgressBarItem4);
            this.stepProgressBar1.Items.Add(this.stepProgressBarItem5);
            this.stepProgressBar1.Items.Add(this.stepProgressBarItem6);
            this.stepProgressBar1.Location = new System.Drawing.Point(97, 19);
            this.stepProgressBar1.Name = "stepProgressBar1";
            this.stepProgressBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.stepProgressBar1.Size = new System.Drawing.Size(918, 437);
            this.stepProgressBar1.TabIndex = 1;
            // 
            // stepProgressBarItem1
            // 
            this.stepProgressBarItem1.ContentBlock2.Caption = "خواندن فایل اکسل";
            this.stepProgressBarItem1.Name = "stepProgressBarItem1";
            // 
            // stepProgressBarItem2
            // 
            this.stepProgressBarItem2.ContentBlock2.Caption = "بررسی وجود شیتهای مورد نیاز";
            this.stepProgressBarItem2.Name = "stepProgressBarItem2";
            // 
            // stepProgressBarItem3
            // 
            this.stepProgressBarItem3.ContentBlock2.Caption = "وجود ستونهای مورد انتظار در شیتها";
            this.stepProgressBarItem3.Name = "stepProgressBarItem3";
            // 
            // stepProgressBarItem4
            // 
            this.stepProgressBarItem4.ContentBlock2.Caption = "مقایسه تکمیل شد";
            this.stepProgressBarItem4.Name = "stepProgressBarItem4";
            // 
            // stepProgressBarItem5
            // 
            this.stepProgressBarItem5.ContentBlock2.Caption = "بروز رسانی SQL تکمیل شد";
            this.stepProgressBarItem5.Name = "stepProgressBarItem5";
            // 
            // stepProgressBarItem6
            // 
            this.stepProgressBarItem6.ContentBlock2.Caption = "بروز رسانی همه دیتابیسها SQL , mySql) انجام شد";
            this.stepProgressBarItem6.Name = "stepProgressBarItem6";
            // 
            // frmCompairAndUpdateDbSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 663);
            this.Controls.Add(this.groupBoxStageOfCompair);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCompairAndUpdateDbSummary";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "فرم مقایسه و بروز رسانی دیتابیس  مختصر";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCompairAndUpdateDbSummary_Load);
            this.groupBoxStageOfCompair.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stepProgressBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxStageOfCompair;
        private DevExpress.XtraEditors.StepProgressBar stepProgressBar1;
        private DevExpress.XtraEditors.StepProgressBarItem stepProgressBarItem1;
        private DevExpress.XtraEditors.StepProgressBarItem stepProgressBarItem2;
        private DevExpress.XtraEditors.StepProgressBarItem stepProgressBarItem3;
        private DevExpress.XtraEditors.StepProgressBarItem stepProgressBarItem4;
        private DevExpress.XtraEditors.StepProgressBarItem stepProgressBarItem5;
        private System.Windows.Forms.OpenFileDialog openFileDialogXclDb;
        private DevExpress.XtraEditors.StepProgressBarItem stepProgressBarItem6;
    }
}