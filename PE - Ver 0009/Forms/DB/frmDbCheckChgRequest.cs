namespace PE.Forms
{
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Views.Grid;
    using PE;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmDbCheckChgRequest : Form
    {
        ClsSql objClsSql;//ایجاد شیی از روی کلاس  اس کیو ال
        private IContainer components = null;
        private GridControl gridControlCheckChgReq;
        private GridView gridViewCheckChgReq;
        private GroupBox groupBoxDbTelChange;
        private Label lblReqSubject;
        private Label lblReqTel;
        private TextBox txtReqSubject;
        private TextBox txtReqRequsterTel;
        private TextBox txtReqRequesterName;
        private Label lblRequester;
        private Button btnReqSendRequest;
        private TableLayoutPanel tableLayoutPanelCheckChgReq;
        private GroupBox grpBxReqAdmin;
        private Label lblReqStatus;
        private Label lblReqTime;
        private Label lblReqUser;
        private Label lblReqNo;
        private TextBox txtReqTime;
        private TextBox txtReqUser;
        private TextBox txtReqNo;
        private Button btnReqUpdate;
        private ComboBox cbxReqStatus;
        private TextBox txtReqChgComments;
        private Label lblReqChgComment;
        private Button btnReqChgDel;
        int intSelectedRows;
        string strCurRowInReqChgGrid;

        public frmDbCheckChgRequest()
        {
            this.InitializeComponent();
            objClsSql = new ClsSql();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmDbCheckChgRequest_Load(object sender, EventArgs e)
        {
            ClsDiff.writeLogOfAction("ورود به صفحه بررسی درخواست تغییرات دیتابیس");
            fillRequestGridAndForm();
            gridViewCheckChgReq.MoveLast();
        }

        private void fillRequestGridAndForm()
        {
            string strQuery = "";
            if (MainForm.StrCurRol == "administrator" || MainForm.StrCurRol == "PowerUser")
            {
                btnReqSendRequest.Enabled = true;
                btnReqChgDel.Visible = true;
                //txtReqNo.Enabled = false;
                //txtReqRequesterName.Enabled = false;
                //txtReqRequsterTel.Enabled = false;
               // txtReqSubject.Enabled = false;
                //txtReqTime.Enabled = false;
                //txtReqUser.Enabled = false;
                grpBxReqAdmin.Visible = true;

                strQuery = @"SELECT       reqNo AS [شماره درخواست] , [user] AS کاربر, requester AS [درخواست کننده], tel AS [تلفن مستقیم], request AS [درخواست تغییرات], requestDateTime AS [زمان تغییر], status AS [آخرین وضعیت],comments AS [ توضیحات واحد بررسی عملکرد]
                    FROM [PE].[dbo].tblRequestChange ORDER BY reqNo";
                
                this.gridControlCheckChgReq.DataSource = objClsSql.FillDataTableFromQuery(strQuery);
                gridViewCheckChgReq.MoveLast();

                try
                {
                    gridViewCheckChgReq.Columns["تلفن مستقیم"].Width = 100;
                }
                catch
                {
                    
                }
            }
            else
            {
                grpBxReqAdmin.Visible = false;
                btnReqChgDel.Visible = false;
                txtReqRequesterName.Focus();
                strQuery = @"SELECT         reqNo AS [شماره درخواست] , [user] AS کاربر, requester AS [درخواست کننده], request AS [درخواست تغییرات], requestDateTime AS [زمان تغییر], status AS [آخرین وضعیت],comments AS [توضیحات واحد بررسی عملکرد]
                    FROM [PE].[dbo].tblRequestChange ORDER BY reqNo";
                this.gridControlCheckChgReq.DataSource = objClsSql.FillDataTableFromQuery(strQuery);
                gridViewCheckChgReq.MoveLast();
            }

            try
            {
                gridViewCheckChgReq.Columns["شماره درخواست"].Width = 100;
                gridViewCheckChgReq.Columns["کاربر"].Width = 70;
                gridViewCheckChgReq.Columns["درخواست کننده"].Width = 90;
                gridViewCheckChgReq.Columns["درخواست تغییرات"].Width = 450;
                gridViewCheckChgReq.Columns["زمان تغییر"].Width = 120;
                gridViewCheckChgReq.Columns["آخرین وضعیت"].Width = 150;
                if (gridViewCheckChgReq.GetFocusedRowCellValue("توضیحات واحد بر رسی عملکرد") != null)
                    gridViewCheckChgReq.Columns["توضیحات واحد بررسی عملکرد"].Width = 300;
            }
            catch
            {

            }
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanelCheckChgReq = new System.Windows.Forms.TableLayoutPanel();
            this.gridControlCheckChgReq = new DevExpress.XtraGrid.GridControl();
            this.gridViewCheckChgReq = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBoxDbTelChange = new System.Windows.Forms.GroupBox();
            this.btnReqChgDel = new System.Windows.Forms.Button();
            this.btnReqUpdate = new System.Windows.Forms.Button();
            this.grpBxReqAdmin = new System.Windows.Forms.GroupBox();
            this.txtReqChgComments = new System.Windows.Forms.TextBox();
            this.lblReqChgComment = new System.Windows.Forms.Label();
            this.lblReqStatus = new System.Windows.Forms.Label();
            this.lblReqTime = new System.Windows.Forms.Label();
            this.cbxReqStatus = new System.Windows.Forms.ComboBox();
            this.lblReqUser = new System.Windows.Forms.Label();
            this.lblReqNo = new System.Windows.Forms.Label();
            this.txtReqTime = new System.Windows.Forms.TextBox();
            this.txtReqUser = new System.Windows.Forms.TextBox();
            this.txtReqNo = new System.Windows.Forms.TextBox();
            this.btnReqSendRequest = new System.Windows.Forms.Button();
            this.lblReqSubject = new System.Windows.Forms.Label();
            this.lblReqTel = new System.Windows.Forms.Label();
            this.txtReqSubject = new System.Windows.Forms.TextBox();
            this.txtReqRequsterTel = new System.Windows.Forms.TextBox();
            this.txtReqRequesterName = new System.Windows.Forms.TextBox();
            this.lblRequester = new System.Windows.Forms.Label();
            this.tableLayoutPanelCheckChgReq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCheckChgReq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCheckChgReq)).BeginInit();
            this.groupBoxDbTelChange.SuspendLayout();
            this.grpBxReqAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelCheckChgReq
            // 
            this.tableLayoutPanelCheckChgReq.ColumnCount = 1;
            this.tableLayoutPanelCheckChgReq.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCheckChgReq.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCheckChgReq.Controls.Add(this.gridControlCheckChgReq, 0, 0);
            this.tableLayoutPanelCheckChgReq.Controls.Add(this.groupBoxDbTelChange, 0, 1);
            this.tableLayoutPanelCheckChgReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCheckChgReq.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelCheckChgReq.Name = "tableLayoutPanelCheckChgReq";
            this.tableLayoutPanelCheckChgReq.RowCount = 3;
            this.tableLayoutPanelCheckChgReq.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCheckChgReq.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCheckChgReq.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCheckChgReq.Size = new System.Drawing.Size(1169, 766);
            this.tableLayoutPanelCheckChgReq.TabIndex = 0;
            // 
            // gridControlCheckChgReq
            // 
            this.gridControlCheckChgReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlCheckChgReq.Location = new System.Drawing.Point(3, 3);
            this.gridControlCheckChgReq.MainView = this.gridViewCheckChgReq;
            this.gridControlCheckChgReq.Name = "gridControlCheckChgReq";
            this.gridControlCheckChgReq.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridControlCheckChgReq.Size = new System.Drawing.Size(1163, 367);
            this.gridControlCheckChgReq.TabIndex = 0;
            this.gridControlCheckChgReq.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewCheckChgReq});
            // 
            // gridViewCheckChgReq
            // 
            this.gridViewCheckChgReq.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridViewCheckChgReq.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewCheckChgReq.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewCheckChgReq.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewCheckChgReq.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewCheckChgReq.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridViewCheckChgReq.GridControl = this.gridControlCheckChgReq;
            this.gridViewCheckChgReq.Name = "gridViewCheckChgReq";
            this.gridViewCheckChgReq.OptionsView.ShowGroupPanel = false;
            this.gridViewCheckChgReq.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridViewCheckChgReq_RowStyle);
            this.gridViewCheckChgReq.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewCheckChgReq_FocusedRowChanged);
            // 
            // groupBoxDbTelChange
            // 
            this.groupBoxDbTelChange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDbTelChange.Controls.Add(this.grpBxReqAdmin);
            this.groupBoxDbTelChange.Controls.Add(this.btnReqSendRequest);
            this.groupBoxDbTelChange.Controls.Add(this.lblReqSubject);
            this.groupBoxDbTelChange.Controls.Add(this.lblReqTel);
            this.groupBoxDbTelChange.Controls.Add(this.txtReqSubject);
            this.groupBoxDbTelChange.Controls.Add(this.txtReqRequsterTel);
            this.groupBoxDbTelChange.Controls.Add(this.txtReqRequesterName);
            this.groupBoxDbTelChange.Controls.Add(this.lblRequester);
            this.groupBoxDbTelChange.Location = new System.Drawing.Point(3, 376);
            this.groupBoxDbTelChange.Name = "groupBoxDbTelChange";
            this.groupBoxDbTelChange.Size = new System.Drawing.Size(1163, 367);
            this.groupBoxDbTelChange.TabIndex = 1;
            this.groupBoxDbTelChange.TabStop = false;
            this.groupBoxDbTelChange.Text = "درخواست تغییرات";
            // 
            // btnReqChgDel
            // 
            this.btnReqChgDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReqChgDel.Location = new System.Drawing.Point(809, 92);
            this.btnReqChgDel.Name = "btnReqChgDel";
            this.btnReqChgDel.Size = new System.Drawing.Size(131, 23);
            this.btnReqChgDel.TabIndex = 7;
            this.btnReqChgDel.Text = "حذف درخواست";
            this.btnReqChgDel.UseVisualStyleBackColor = true;
            this.btnReqChgDel.Click += new System.EventHandler(this.btnReqChgDel_Click);
            // 
            // btnReqUpdate
            // 
            this.btnReqUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReqUpdate.Location = new System.Drawing.Point(980, 92);
            this.btnReqUpdate.Name = "btnReqUpdate";
            this.btnReqUpdate.Size = new System.Drawing.Size(141, 23);
            this.btnReqUpdate.TabIndex = 6;
            this.btnReqUpdate.Text = "بروز رسانی درخواست";
            this.btnReqUpdate.UseVisualStyleBackColor = true;
            this.btnReqUpdate.Click += new System.EventHandler(this.btnReqUpdate_Click);
            // 
            // grpBxReqAdmin
            // 
            this.grpBxReqAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBxReqAdmin.Controls.Add(this.btnReqChgDel);
            this.grpBxReqAdmin.Controls.Add(this.btnReqUpdate);
            this.grpBxReqAdmin.Controls.Add(this.txtReqChgComments);
            this.grpBxReqAdmin.Controls.Add(this.lblReqChgComment);
            this.grpBxReqAdmin.Controls.Add(this.lblReqStatus);
            this.grpBxReqAdmin.Controls.Add(this.lblReqTime);
            this.grpBxReqAdmin.Controls.Add(this.cbxReqStatus);
            this.grpBxReqAdmin.Controls.Add(this.lblReqUser);
            this.grpBxReqAdmin.Controls.Add(this.lblReqNo);
            this.grpBxReqAdmin.Controls.Add(this.txtReqTime);
            this.grpBxReqAdmin.Controls.Add(this.txtReqUser);
            this.grpBxReqAdmin.Controls.Add(this.txtReqNo);
            this.grpBxReqAdmin.Location = new System.Drawing.Point(9, 240);
            this.grpBxReqAdmin.Name = "grpBxReqAdmin";
            this.grpBxReqAdmin.Size = new System.Drawing.Size(1148, 121);
            this.grpBxReqAdmin.TabIndex = 5;
            this.grpBxReqAdmin.TabStop = false;
            // 
            // txtReqChgComments
            // 
            this.txtReqChgComments.Location = new System.Drawing.Point(107, 52);
            this.txtReqChgComments.Multiline = true;
            this.txtReqChgComments.Name = "txtReqChgComments";
            this.txtReqChgComments.Size = new System.Drawing.Size(866, 20);
            this.txtReqChgComments.TabIndex = 4;
            this.txtReqChgComments.Enter += new System.EventHandler(this.txtReqChgComments_Enter);
            // 
            // lblReqChgComment
            // 
            this.lblReqChgComment.AutoSize = true;
            this.lblReqChgComment.Location = new System.Drawing.Point(980, 55);
            this.lblReqChgComment.Name = "lblReqChgComment";
            this.lblReqChgComment.Size = new System.Drawing.Size(150, 13);
            this.lblReqChgComment.TabIndex = 3;
            this.lblReqChgComment.Text = "توضیحات واحد بررسی عملکرد :";
            // 
            // lblReqStatus
            // 
            this.lblReqStatus.AutoSize = true;
            this.lblReqStatus.Location = new System.Drawing.Point(507, 22);
            this.lblReqStatus.Name = "lblReqStatus";
            this.lblReqStatus.Size = new System.Drawing.Size(76, 13);
            this.lblReqStatus.TabIndex = 1;
            this.lblReqStatus.Text = "آخرین وضعیت :";
            // 
            // lblReqTime
            // 
            this.lblReqTime.AutoSize = true;
            this.lblReqTime.Location = new System.Drawing.Point(681, 26);
            this.lblReqTime.Name = "lblReqTime";
            this.lblReqTime.Size = new System.Drawing.Size(60, 13);
            this.lblReqTime.TabIndex = 1;
            this.lblReqTime.Text = "زمان تغییر :";
            // 
            // cbxReqStatus
            // 
            this.cbxReqStatus.FormattingEnabled = true;
            this.cbxReqStatus.Items.AddRange(new object[] {
            "در حال بررسی",
            "در انتظار موافقت اداره طراحی",
            "با درخواست تغییرات موافقت و در دیتابیس اعمال گردید",
            "اطلاعات درخواست شده معتبر نمی باشد",
            "با درخواست موافقت نگردید"});
            this.cbxReqStatus.Location = new System.Drawing.Point(188, 19);
            this.cbxReqStatus.Name = "cbxReqStatus";
            this.cbxReqStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxReqStatus.Size = new System.Drawing.Size(300, 21);
            this.cbxReqStatus.TabIndex = 2;
            this.cbxReqStatus.Text = "در حال بررسی";
            // 
            // lblReqUser
            // 
            this.lblReqUser.AutoSize = true;
            this.lblReqUser.Location = new System.Drawing.Point(863, 22);
            this.lblReqUser.Name = "lblReqUser";
            this.lblReqUser.Size = new System.Drawing.Size(38, 13);
            this.lblReqUser.TabIndex = 1;
            this.lblReqUser.Text = "کاربر :";
            // 
            // lblReqNo
            // 
            this.lblReqNo.AutoSize = true;
            this.lblReqNo.Location = new System.Drawing.Point(1042, 22);
            this.lblReqNo.Name = "lblReqNo";
            this.lblReqNo.Size = new System.Drawing.Size(88, 13);
            this.lblReqNo.TabIndex = 1;
            this.lblReqNo.Text = "شماره درخواست :";
            // 
            // txtReqTime
            // 
            this.txtReqTime.Location = new System.Drawing.Point(589, 19);
            this.txtReqTime.Name = "txtReqTime";
            this.txtReqTime.Size = new System.Drawing.Size(92, 20);
            this.txtReqTime.TabIndex = 0;
            // 
            // txtReqUser
            // 
            this.txtReqUser.Location = new System.Drawing.Point(757, 19);
            this.txtReqUser.Name = "txtReqUser";
            this.txtReqUser.Size = new System.Drawing.Size(100, 20);
            this.txtReqUser.TabIndex = 0;
            // 
            // txtReqNo
            // 
            this.txtReqNo.Location = new System.Drawing.Point(930, 19);
            this.txtReqNo.Name = "txtReqNo";
            this.txtReqNo.Size = new System.Drawing.Size(100, 20);
            this.txtReqNo.TabIndex = 0;
            // 
            // btnReqSendRequest
            // 
            this.btnReqSendRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReqSendRequest.Location = new System.Drawing.Point(761, 211);
            this.btnReqSendRequest.Name = "btnReqSendRequest";
            this.btnReqSendRequest.Size = new System.Drawing.Size(171, 23);
            this.btnReqSendRequest.TabIndex = 4;
            this.btnReqSendRequest.Text = "ارسال درخواست تغییرات";
            this.btnReqSendRequest.UseVisualStyleBackColor = true;
            this.btnReqSendRequest.Click += new System.EventHandler(this.btnReqSendRequest_Click);
            // 
            // lblReqSubject
            // 
            this.lblReqSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReqSubject.AutoSize = true;
            this.lblReqSubject.Location = new System.Drawing.Point(945, 98);
            this.lblReqSubject.Name = "lblReqSubject";
            this.lblReqSubject.Size = new System.Drawing.Size(210, 13);
            this.lblReqSubject.TabIndex = 3;
            this.lblReqSubject.Text = "موضوع درخواست در دیتابیس یا دفترچه تلفن :";
            // 
            // lblReqTel
            // 
            this.lblReqTel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReqTel.AutoSize = true;
            this.lblReqTel.Location = new System.Drawing.Point(945, 52);
            this.lblReqTel.Name = "lblReqTel";
            this.lblReqTel.Size = new System.Drawing.Size(215, 13);
            this.lblReqTel.TabIndex = 3;
            this.lblReqTel.Text = "تلفن مستقیم (ترجیحا موبایل)درخواست کننده  :";
            // 
            // txtReqSubject
            // 
            this.txtReqSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReqSubject.Location = new System.Drawing.Point(21, 82);
            this.txtReqSubject.Multiline = true;
            this.txtReqSubject.Name = "txtReqSubject";
            this.txtReqSubject.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReqSubject.Size = new System.Drawing.Size(918, 107);
            this.txtReqSubject.TabIndex = 3;
            this.txtReqSubject.Enter += new System.EventHandler(this.txtReqSubject_Enter);
            // 
            // txtReqRequsterTel
            // 
            this.txtReqRequsterTel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReqRequsterTel.Location = new System.Drawing.Point(758, 49);
            this.txtReqRequsterTel.Name = "txtReqRequsterTel";
            this.txtReqRequsterTel.Size = new System.Drawing.Size(181, 20);
            this.txtReqRequsterTel.TabIndex = 2;
            this.txtReqRequsterTel.Enter += new System.EventHandler(this.txtReqRequsterTel_Enter);
            this.txtReqRequsterTel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtReqRequsterTel_KeyPress);
            // 
            // txtReqRequesterName
            // 
            this.txtReqRequesterName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReqRequesterName.BackColor = System.Drawing.Color.White;
            this.txtReqRequesterName.Location = new System.Drawing.Point(758, 19);
            this.txtReqRequesterName.Name = "txtReqRequesterName";
            this.txtReqRequesterName.Size = new System.Drawing.Size(181, 20);
            this.txtReqRequesterName.TabIndex = 1;
            this.txtReqRequesterName.Enter += new System.EventHandler(this.txtReqRequesterName_Enter);
            // 
            // lblRequester
            // 
            this.lblRequester.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRequester.AutoSize = true;
            this.lblRequester.Location = new System.Drawing.Point(983, 24);
            this.lblRequester.Name = "lblRequester";
            this.lblRequester.Size = new System.Drawing.Size(175, 13);
            this.lblRequester.TabIndex = 0;
            this.lblRequester.Text = "نام و نام خانوادگی درخواست کننده :";
            // 
            // frmDbCheckChgRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 766);
            this.Controls.Add(this.tableLayoutPanelCheckChgReq);
            this.Name = "frmDbCheckChgRequest";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "فرم  تغییرات دیتابیس و دفترچه تلفن";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmDbCheckChgRequest_Load);
            this.tableLayoutPanelCheckChgReq.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCheckChgReq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCheckChgReq)).EndInit();
            this.groupBoxDbTelChange.ResumeLayout(false);
            this.groupBoxDbTelChange.PerformLayout();
            this.grpBxReqAdmin.ResumeLayout(false);
            this.grpBxReqAdmin.PerformLayout();
            this.ResumeLayout(false);

        }

        private void btnReqSendRequest_Click(object sender, EventArgs e)
        {
            ClsDiff.writeLogOfAction("ارسال درخواست تغییرات");
            #region تعریف متغییر بودن همه فیلدها
            //متغییر پر بودن همه فیلدها
            bool bolIsCompAllDtField =
                (this.txtReqRequesterName.Text == "") ||
                (this.txtReqRequsterTel.Text == "") ||
                (this.txtReqSubject.Text == "")
               
                ;
            #endregion

            if (bolIsCompAllDtField)
            {
                #region چک تکمیل بودن همه فیلدها برای پیدا کردن مورد یا موارد ناکامل
                string str = "";
                if (this.txtReqRequesterName.Text == "")
                {
                    str = str + "نام درخواست کننده و ";
                }
                if (this.txtReqRequsterTel.Text == "")
                {
                    str = str + "شماره تلفن درخواست کننده و ";
                }
                if (this.txtReqSubject.Text == "")
                {
                    str = str + "موضوع تغییرات و ";
                }
                
                str = str.Substring(0, str.Length - 2);
                MessageBox.Show($"  کاربر گرامی فیلد  {str} تکمیل نشده است لطفا درخواست خود را تکمیل بفرمایید", "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                #endregion
            }
            else
            {
                #region تولید شماره درخواست جدید
                int reqNo;
               

                string s =objClsSql.FindFieldInLastRecord("[PE].[dbo].tblRequestChange", "reqNo", "reqNo");

                if (s == "نامشخص")
                {
                    reqNo = 0;
                }
                else
                {
                    reqNo = int.Parse(s);
                }

                reqNo = reqNo + 1;

                #endregion

                string strDateTime = ClsConvertDateHassan.convertDateTimeToPersion(DateTime.Now);
                string strFieldsName = "[reqNo],[user],[requester],[tel],[request],[requestDateTime],[status]";
                string strValues = "N'" + reqNo + "',N'" + MainForm.StrCurUsr + "',N'" + this.txtReqRequesterName.Text + "',N'" + txtReqRequsterTel.Text + "',N'" + txtReqSubject.Text + "',N'" + strDateTime + "',N'در حال بررسی'";
                objClsSql.GetAllAndSendToDataBase("[PE].[dbo].[tblRequestChange]", strFieldsName, strValues);
                MessageBox.Show(".درخواست شما به واحد بررسی عملکرد ارسال شد تا در صورت تایید اعمال گردد");
                this.txtReqRequesterName.Clear();
                this.txtReqRequsterTel.Clear();
                this.txtReqSubject.Clear();

                fillRequestGridAndForm();
            }
           
        }

        private void gridViewCheckChgReq_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
          
            initialGridViewReq(e);

        }

        private void initialGridViewReq(DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int intCountReq = gridViewCheckChgReq.DataRowCount;
            if(intCountReq==0)
            {
                return;
            }
            if (e.FocusedRowHandle >= 0)
            {
                strCurRowInReqChgGrid = this.gridViewCheckChgReq.GetDataRow(e.FocusedRowHandle)["شماره درخواست"].ToString();
            }
            else
            {
                MessageBox.Show("هیچ ردیفی مشخص نشد");
            }

            if (MainForm.StrCurRol == "administrator" || MainForm.StrCurRol == "PowerUser")
            {
                intSelectedRows = int.Parse(gridViewCheckChgReq.GetFocusedRowCellValue("شماره درخواست").ToString());

                txtReqSubject.Text = gridViewCheckChgReq.GetFocusedRowCellValue("درخواست تغییرات").ToString();
                txtReqUser.Text = gridViewCheckChgReq.GetFocusedRowCellValue("کاربر").ToString();
                txtReqRequesterName.Text = gridViewCheckChgReq.GetFocusedRowCellValue("درخواست کننده").ToString();

               // btnReqSendRequest.Enabled = false;
                btnReqUpdate.Visible = true;

                grpBxReqAdmin.Enabled = true;
                grpBxReqAdmin.Visible = true;
                txtReqTime.Text = gridViewCheckChgReq.GetFocusedRowCellValue("زمان تغییر").ToString();
                cbxReqStatus.Text = gridViewCheckChgReq.GetFocusedRowCellValue("آخرین وضعیت").ToString();
                txtReqRequsterTel.Text = gridViewCheckChgReq.GetFocusedRowCellValue("تلفن مستقیم").ToString();
                txtReqNo.Text = gridViewCheckChgReq.GetFocusedRowCellValue("شماره درخواست").ToString();
                if (gridViewCheckChgReq.GetFocusedRowCellValue("توضیحات واحد بررسی عملکرد") != null)
                {
                    txtReqChgComments.Text = gridViewCheckChgReq.GetFocusedRowCellValue("توضیحات واحد بررسی عملکرد").ToString();
                }

            }
            else
            {
                btnReqSendRequest.Enabled = true;
                btnReqUpdate.Visible = false;

                grpBxReqAdmin.Enabled = false;
                grpBxReqAdmin.Visible = false;
            }
        }

        private void btnReqUpdate_Click(object sender, EventArgs e)
        {
            string strSql = @"Update [PE].[dbo].[tblRequestChange]
             set [status]=N'" + cbxReqStatus.Text+ "',[comments]=N'"+txtReqChgComments.Text+@"'
              where reqNo=N'"+txtReqNo.Text+"'";

           
            objClsSql.SendCommand(strSql);

            fillRequestGridAndForm();

        }

        private void txtReqRequsterTel_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("en"));
        }

        private void txtReqSubject_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void txtReqRequesterName_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void txtReqRequsterTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar)))
            {
                MessageBox.Show(". همکار عزیز تلفن فقط می تواند عدد باشد");
                e.Handled = true;
            }
        }

        private void btnReqChgDel_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("  برای حذف درخواست شماره  " + strCurRowInReqChgGrid + " مطمئن هستید  ؟  ", " تاییدیه اعمال  تغییرات  ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                this.btnReqChgDel.Enabled = false;

                int countTblReq = objClsSql.Count("[PE].[dbo].tblRequestChange","");

                int reqNoForDel = int.Parse(strCurRowInReqChgGrid);

                
                objClsSql.SendCommand(@"Delete TOP(1) [PE].[dbo].tblRequestChange Where reqNo=N'" + strCurRowInReqChgGrid + "'");
                strCurRowInReqChgGrid = gridViewCheckChgReq.GetSelectedRows().ToString();

                for (int i= reqNoForDel+1; i<= countTblReq; i++)
                {
                   
                    objClsSql.SendCommand(@"Update [PE].[dbo].tblRequestChange set   reqNo=N'" + (i - 1) + "' Where reqNo=N'" + i + "'");
                }

                fillRequestGridAndForm();

                string text = $".کاربر گرامی درخواست شما برای حذف درخواست شماره {strCurRowInReqChgGrid} انجام گردید";
                MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                ClsDiff.writeLogOfAction("حذف درخواست تغییرات انجام گرفت");

                this.btnReqChgDel.Enabled = true;
                gridViewCheckChgReq.MoveLast();

            }

            

        }

        private void txtReqChgComments_Enter(object sender, EventArgs e)
        {
            ClsDiff.SetKeyboardLayout(ClsDiff.GetInputLanguageByName("pe"));
        }

        private void gridViewCheckChgReq_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
               
                string strStatus = view.GetRowCellDisplayText(e.RowHandle, view.Columns["آخرین وضعیت"]);


                switch (strStatus)
                {
                    case "در حال بررسی":

                        e.Appearance.BackColor = Color.Yellow;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "در انتظار موافقت اداره طراحی":
                        e.Appearance.BackColor = Color.LightGreen;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "با درخواست تغییرات موافقت و در دیتابیس اعمال گردید":
                        e.Appearance.BackColor = Color.SeaGreen;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "اطلاعات درخواست شده معتبر نمی باشد":
                        e.Appearance.BackColor = Color.LightPink;
                        e.Appearance.BackColor2 = Color.White;
                        return;

                    case "با درخواست موافقت نگردید":
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.BackColor2 = Color.White;
                        return;
                }
           }
        }
    }
}

