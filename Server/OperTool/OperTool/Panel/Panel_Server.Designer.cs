namespace OperTool.Panel
{
    partial class Panel_Server
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customGridView_serverinfo = new OperTool.Controls.SGridView();
            this.metroButton_server_refresh = new MetroFramework.Controls.MetroButton();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroLabel_detail_dbfps = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox_detail_dbfps = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox_detail_fps = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel_detail_dn = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox_detail_dn = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox_detail_ip = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox_detail_user_cnt = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel_detail_client_count = new MetroFramework.Controls.MetroLabel();
            this.metroCheckBox_detail_live = new MetroFramework.Controls.MetroCheckBox();
            this.metroTextBox_detail_type = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox_detail_port = new MetroFramework.Controls.MetroTextBox();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroButton_service_type = new MetroFramework.Controls.MetroButton();
            this.metroComboBox_service_type = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.metroCheckBox_live = new MetroFramework.Controls.MetroCheckBox();
            this.metroPanel4 = new MetroFramework.Controls.MetroPanel();
            this.Panel_Base.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_serverinfo)).BeginInit();
            this.metroPanel1.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.metroPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_Base
            // 
            this.Panel_Base.Controls.Add(this.metroPanel4);
            this.Panel_Base.Controls.Add(this.metroPanel2);
            this.Panel_Base.Controls.Add(this.metroTextBox_detail_port);
            this.Panel_Base.Controls.Add(this.metroPanel1);
            this.Panel_Base.Controls.Add(this.metroButton_server_refresh);
            this.Panel_Base.Controls.SetChildIndex(this.metroButton_server_refresh, 0);
            this.Panel_Base.Controls.SetChildIndex(this.metroPanel1, 0);
            this.Panel_Base.Controls.SetChildIndex(this.metroTextBox_detail_port, 0);
            this.Panel_Base.Controls.SetChildIndex(this.Panel_Label, 0);
            this.Panel_Base.Controls.SetChildIndex(this.metroPanel2, 0);
            this.Panel_Base.Controls.SetChildIndex(this.metroPanel4, 0);
            // 
            // customGridView_serverinfo
            // 
            this.customGridView_serverinfo.AllowUserToAddRows = false;
            this.customGridView_serverinfo.AllowUserToDeleteRows = false;
            this.customGridView_serverinfo.AllowUserToResizeRows = false;
            this.customGridView_serverinfo.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_serverinfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customGridView_serverinfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_serverinfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.customGridView_serverinfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customGridView_serverinfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.customGridView_serverinfo.EnableHeadersVisualStyles = false;
            this.customGridView_serverinfo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.customGridView_serverinfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_serverinfo.Location = new System.Drawing.Point(10, 31);
            this.customGridView_serverinfo.Name = "customGridView_serverinfo";
            this.customGridView_serverinfo.ReadOnly = true;
            this.customGridView_serverinfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_serverinfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.customGridView_serverinfo.RowHeadersVisible = false;
            this.customGridView_serverinfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.customGridView_serverinfo.RowTemplate.Height = 23;
            this.customGridView_serverinfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customGridView_serverinfo.Size = new System.Drawing.Size(782, 151);
            this.customGridView_serverinfo.TabIndex = 3;
            this.customGridView_serverinfo.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customGridView_serverinfo_CellContentDoubleClick);
            // 
            // metroButton_server_refresh
            // 
            this.metroButton_server_refresh.Location = new System.Drawing.Point(866, 117);
            this.metroButton_server_refresh.Name = "metroButton_server_refresh";
            this.metroButton_server_refresh.Size = new System.Drawing.Size(137, 32);
            this.metroButton_server_refresh.TabIndex = 4;
            this.metroButton_server_refresh.Text = "Refresh";
            this.metroButton_server_refresh.UseSelectable = true;
            this.metroButton_server_refresh.Click += new System.EventHandler(this.metroButton_server_refresh_Click);
            // 
            // metroPanel1
            // 
            this.metroPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel1.Controls.Add(this.metroLabel_detail_dbfps);
            this.metroPanel1.Controls.Add(this.metroTextBox_detail_dbfps);
            this.metroPanel1.Controls.Add(this.metroLabel3);
            this.metroPanel1.Controls.Add(this.metroTextBox_detail_fps);
            this.metroPanel1.Controls.Add(this.metroLabel2);
            this.metroPanel1.Controls.Add(this.metroLabel_detail_dn);
            this.metroPanel1.Controls.Add(this.metroTextBox_detail_dn);
            this.metroPanel1.Controls.Add(this.metroTextBox_detail_ip);
            this.metroPanel1.Controls.Add(this.metroLabel);
            this.metroPanel1.Controls.Add(this.metroTextBox_detail_user_cnt);
            this.metroPanel1.Controls.Add(this.metroLabel_detail_client_count);
            this.metroPanel1.Controls.Add(this.metroCheckBox_detail_live);
            this.metroPanel1.Controls.Add(this.metroTextBox_detail_type);
            this.metroPanel1.Controls.Add(this.metroLabel1);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(37, 313);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(472, 196);
            this.metroPanel1.TabIndex = 5;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // metroLabel_detail_dbfps
            // 
            this.metroLabel_detail_dbfps.AutoSize = true;
            this.metroLabel_detail_dbfps.Location = new System.Drawing.Point(161, 114);
            this.metroLabel_detail_dbfps.Name = "metroLabel_detail_dbfps";
            this.metroLabel_detail_dbfps.Size = new System.Drawing.Size(48, 19);
            this.metroLabel_detail_dbfps.TabIndex = 14;
            this.metroLabel_detail_dbfps.Text = "DBFPS";
            // 
            // metroTextBox_detail_dbfps
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_dbfps.CustomButton.Image = null;
            this.metroTextBox_detail_dbfps.CustomButton.Location = new System.Drawing.Point(57, 1);
            this.metroTextBox_detail_dbfps.CustomButton.Name = "";
            this.metroTextBox_detail_dbfps.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_dbfps.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_dbfps.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_dbfps.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_dbfps.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_dbfps.CustomButton.Visible = false;
            this.metroTextBox_detail_dbfps.Lines = new string[0];
            this.metroTextBox_detail_dbfps.Location = new System.Drawing.Point(215, 114);
            this.metroTextBox_detail_dbfps.MaxLength = 32767;
            this.metroTextBox_detail_dbfps.Name = "metroTextBox_detail_dbfps";
            this.metroTextBox_detail_dbfps.PasswordChar = '\0';
            this.metroTextBox_detail_dbfps.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_dbfps.SelectedText = "";
            this.metroTextBox_detail_dbfps.SelectionLength = 0;
            this.metroTextBox_detail_dbfps.SelectionStart = 0;
            this.metroTextBox_detail_dbfps.ShortcutsEnabled = true;
            this.metroTextBox_detail_dbfps.Size = new System.Drawing.Size(79, 23);
            this.metroTextBox_detail_dbfps.TabIndex = 15;
            this.metroTextBox_detail_dbfps.UseSelectable = true;
            this.metroTextBox_detail_dbfps.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_dbfps.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(17, 114);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(31, 19);
            this.metroLabel3.TabIndex = 12;
            this.metroLabel3.Text = "FPS";
            // 
            // metroTextBox_detail_fps
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_fps.CustomButton.Image = null;
            this.metroTextBox_detail_fps.CustomButton.Location = new System.Drawing.Point(57, 1);
            this.metroTextBox_detail_fps.CustomButton.Name = "";
            this.metroTextBox_detail_fps.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_fps.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_fps.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_fps.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_fps.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_fps.CustomButton.Visible = false;
            this.metroTextBox_detail_fps.Lines = new string[0];
            this.metroTextBox_detail_fps.Location = new System.Drawing.Point(56, 113);
            this.metroTextBox_detail_fps.MaxLength = 32767;
            this.metroTextBox_detail_fps.Name = "metroTextBox_detail_fps";
            this.metroTextBox_detail_fps.PasswordChar = '\0';
            this.metroTextBox_detail_fps.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_fps.SelectedText = "";
            this.metroTextBox_detail_fps.SelectionLength = 0;
            this.metroTextBox_detail_fps.SelectionStart = 0;
            this.metroTextBox_detail_fps.ShortcutsEnabled = true;
            this.metroTextBox_detail_fps.Size = new System.Drawing.Size(79, 23);
            this.metroTextBox_detail_fps.TabIndex = 13;
            this.metroTextBox_detail_fps.UseSelectable = true;
            this.metroTextBox_detail_fps.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_fps.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(325, 55);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(34, 19);
            this.metroLabel2.TabIndex = 11;
            this.metroLabel2.Text = "Port";
            // 
            // metroLabel_detail_dn
            // 
            this.metroLabel_detail_dn.AutoSize = true;
            this.metroLabel_detail_dn.Location = new System.Drawing.Point(22, 82);
            this.metroLabel_detail_dn.Name = "metroLabel_detail_dn";
            this.metroLabel_detail_dn.Size = new System.Drawing.Size(28, 19);
            this.metroLabel_detail_dn.TabIndex = 10;
            this.metroLabel_detail_dn.Text = "DN";
            // 
            // metroTextBox_detail_dn
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_dn.CustomButton.Image = null;
            this.metroTextBox_detail_dn.CustomButton.Location = new System.Drawing.Point(216, 1);
            this.metroTextBox_detail_dn.CustomButton.Name = "";
            this.metroTextBox_detail_dn.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_dn.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_dn.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_dn.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_dn.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_dn.CustomButton.Visible = false;
            this.metroTextBox_detail_dn.Lines = new string[0];
            this.metroTextBox_detail_dn.Location = new System.Drawing.Point(56, 81);
            this.metroTextBox_detail_dn.MaxLength = 32767;
            this.metroTextBox_detail_dn.Name = "metroTextBox_detail_dn";
            this.metroTextBox_detail_dn.PasswordChar = '\0';
            this.metroTextBox_detail_dn.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_dn.SelectedText = "";
            this.metroTextBox_detail_dn.SelectionLength = 0;
            this.metroTextBox_detail_dn.SelectionStart = 0;
            this.metroTextBox_detail_dn.ShortcutsEnabled = true;
            this.metroTextBox_detail_dn.Size = new System.Drawing.Size(238, 23);
            this.metroTextBox_detail_dn.TabIndex = 9;
            this.metroTextBox_detail_dn.UseSelectable = true;
            this.metroTextBox_detail_dn.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_dn.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTextBox_detail_ip
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_ip.CustomButton.Image = null;
            this.metroTextBox_detail_ip.CustomButton.Location = new System.Drawing.Point(216, 1);
            this.metroTextBox_detail_ip.CustomButton.Name = "";
            this.metroTextBox_detail_ip.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_ip.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_ip.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_ip.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_ip.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_ip.CustomButton.Visible = false;
            this.metroTextBox_detail_ip.Lines = new string[0];
            this.metroTextBox_detail_ip.Location = new System.Drawing.Point(56, 52);
            this.metroTextBox_detail_ip.MaxLength = 32767;
            this.metroTextBox_detail_ip.Name = "metroTextBox_detail_ip";
            this.metroTextBox_detail_ip.PasswordChar = '\0';
            this.metroTextBox_detail_ip.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_ip.SelectedText = "";
            this.metroTextBox_detail_ip.SelectionLength = 0;
            this.metroTextBox_detail_ip.SelectionStart = 0;
            this.metroTextBox_detail_ip.ShortcutsEnabled = true;
            this.metroTextBox_detail_ip.Size = new System.Drawing.Size(238, 23);
            this.metroTextBox_detail_ip.TabIndex = 8;
            this.metroTextBox_detail_ip.UseSelectable = true;
            this.metroTextBox_detail_ip.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_ip.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel
            // 
            this.metroLabel.AutoSize = true;
            this.metroLabel.Location = new System.Drawing.Point(22, 53);
            this.metroLabel.Name = "metroLabel";
            this.metroLabel.Size = new System.Drawing.Size(20, 19);
            this.metroLabel.TabIndex = 7;
            this.metroLabel.Text = "IP";
            // 
            // metroTextBox_detail_user_cnt
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_user_cnt.CustomButton.Image = null;
            this.metroTextBox_detail_user_cnt.CustomButton.Location = new System.Drawing.Point(82, 1);
            this.metroTextBox_detail_user_cnt.CustomButton.Name = "";
            this.metroTextBox_detail_user_cnt.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_user_cnt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_user_cnt.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_user_cnt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_user_cnt.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_user_cnt.CustomButton.Visible = false;
            this.metroTextBox_detail_user_cnt.Lines = new string[0];
            this.metroTextBox_detail_user_cnt.Location = new System.Drawing.Point(258, 13);
            this.metroTextBox_detail_user_cnt.MaxLength = 32767;
            this.metroTextBox_detail_user_cnt.Name = "metroTextBox_detail_user_cnt";
            this.metroTextBox_detail_user_cnt.PasswordChar = '\0';
            this.metroTextBox_detail_user_cnt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_user_cnt.SelectedText = "";
            this.metroTextBox_detail_user_cnt.SelectionLength = 0;
            this.metroTextBox_detail_user_cnt.SelectionStart = 0;
            this.metroTextBox_detail_user_cnt.ShortcutsEnabled = true;
            this.metroTextBox_detail_user_cnt.Size = new System.Drawing.Size(104, 23);
            this.metroTextBox_detail_user_cnt.TabIndex = 6;
            this.metroTextBox_detail_user_cnt.UseSelectable = true;
            this.metroTextBox_detail_user_cnt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_user_cnt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel_detail_client_count
            // 
            this.metroLabel_detail_client_count.AutoSize = true;
            this.metroLabel_detail_client_count.Location = new System.Drawing.Point(182, 14);
            this.metroLabel_detail_client_count.Name = "metroLabel_detail_client_count";
            this.metroLabel_detail_client_count.Size = new System.Drawing.Size(70, 19);
            this.metroLabel_detail_client_count.TabIndex = 5;
            this.metroLabel_detail_client_count.Text = "UserCount";
            // 
            // metroCheckBox_detail_live
            // 
            this.metroCheckBox_detail_live.AutoSize = true;
            this.metroCheckBox_detail_live.Location = new System.Drawing.Point(398, 18);
            this.metroCheckBox_detail_live.Name = "metroCheckBox_detail_live";
            this.metroCheckBox_detail_live.Size = new System.Drawing.Size(44, 15);
            this.metroCheckBox_detail_live.TabIndex = 4;
            this.metroCheckBox_detail_live.Text = "Live";
            this.metroCheckBox_detail_live.UseSelectable = true;
            // 
            // metroTextBox_detail_type
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_type.CustomButton.Image = null;
            this.metroTextBox_detail_type.CustomButton.Location = new System.Drawing.Point(82, 1);
            this.metroTextBox_detail_type.CustomButton.Name = "";
            this.metroTextBox_detail_type.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_type.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_type.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_type.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_type.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_type.CustomButton.Visible = false;
            this.metroTextBox_detail_type.Lines = new string[0];
            this.metroTextBox_detail_type.Location = new System.Drawing.Point(56, 13);
            this.metroTextBox_detail_type.MaxLength = 32767;
            this.metroTextBox_detail_type.Name = "metroTextBox_detail_type";
            this.metroTextBox_detail_type.PasswordChar = '\0';
            this.metroTextBox_detail_type.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_type.SelectedText = "";
            this.metroTextBox_detail_type.SelectionLength = 0;
            this.metroTextBox_detail_type.SelectionStart = 0;
            this.metroTextBox_detail_type.ShortcutsEnabled = true;
            this.metroTextBox_detail_type.Size = new System.Drawing.Size(104, 23);
            this.metroTextBox_detail_type.TabIndex = 3;
            this.metroTextBox_detail_type.UseSelectable = true;
            this.metroTextBox_detail_type.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_type.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(14, 13);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(36, 19);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Type";
            // 
            // metroTextBox_detail_port
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_port.CustomButton.Image = null;
            this.metroTextBox_detail_port.CustomButton.Location = new System.Drawing.Point(57, 1);
            this.metroTextBox_detail_port.CustomButton.Name = "";
            this.metroTextBox_detail_port.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_port.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_port.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_port.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_port.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_port.CustomButton.Visible = false;
            this.metroTextBox_detail_port.Lines = new string[0];
            this.metroTextBox_detail_port.Location = new System.Drawing.Point(401, 367);
            this.metroTextBox_detail_port.MaxLength = 32767;
            this.metroTextBox_detail_port.Name = "metroTextBox_detail_port";
            this.metroTextBox_detail_port.PasswordChar = '\0';
            this.metroTextBox_detail_port.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_port.SelectedText = "";
            this.metroTextBox_detail_port.SelectionLength = 0;
            this.metroTextBox_detail_port.SelectionStart = 0;
            this.metroTextBox_detail_port.ShortcutsEnabled = true;
            this.metroTextBox_detail_port.Size = new System.Drawing.Size(79, 23);
            this.metroTextBox_detail_port.TabIndex = 11;
            this.metroTextBox_detail_port.UseSelectable = true;
            this.metroTextBox_detail_port.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_port.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroPanel2
            // 
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel2.Controls.Add(this.metroButton_service_type);
            this.metroPanel2.Controls.Add(this.metroComboBox_service_type);
            this.metroPanel2.Controls.Add(this.metroLabel4);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(848, 170);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(175, 121);
            this.metroPanel2.TabIndex = 12;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // metroButton_service_type
            // 
            this.metroButton_service_type.Location = new System.Drawing.Point(18, 78);
            this.metroButton_service_type.Name = "metroButton_service_type";
            this.metroButton_service_type.Size = new System.Drawing.Size(142, 23);
            this.metroButton_service_type.TabIndex = 4;
            this.metroButton_service_type.Text = "Apply";
            this.metroButton_service_type.UseSelectable = true;
            this.metroButton_service_type.Click += new System.EventHandler(this.metroButton_service_type_Click);
            // 
            // metroComboBox_service_type
            // 
            this.metroComboBox_service_type.FormattingEnabled = true;
            this.metroComboBox_service_type.ItemHeight = 23;
            this.metroComboBox_service_type.Location = new System.Drawing.Point(18, 35);
            this.metroComboBox_service_type.Name = "metroComboBox_service_type";
            this.metroComboBox_service_type.Size = new System.Drawing.Size(142, 29);
            this.metroComboBox_service_type.TabIndex = 3;
            this.metroComboBox_service_type.UseSelectable = true;
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(18, 13);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(78, 19);
            this.metroLabel4.TabIndex = 2;
            this.metroLabel4.Text = "ServiceType";
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = null;
            // 
            // metroCheckBox_live
            // 
            this.metroCheckBox_live.AutoSize = true;
            this.metroCheckBox_live.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.metroCheckBox_live.FontWeight = MetroFramework.MetroCheckBoxWeight.Bold;
            this.metroCheckBox_live.Location = new System.Drawing.Point(740, 6);
            this.metroCheckBox_live.Name = "metroCheckBox_live";
            this.metroCheckBox_live.Size = new System.Drawing.Size(52, 19);
            this.metroCheckBox_live.TabIndex = 3;
            this.metroCheckBox_live.Text = "Live";
            this.metroCheckBox_live.UseSelectable = true;
            // 
            // metroPanel4
            // 
            this.metroPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel4.Controls.Add(this.metroCheckBox_live);
            this.metroPanel4.Controls.Add(this.customGridView_serverinfo);
            this.metroPanel4.HorizontalScrollbarBarColor = true;
            this.metroPanel4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel4.HorizontalScrollbarSize = 10;
            this.metroPanel4.Location = new System.Drawing.Point(37, 102);
            this.metroPanel4.Name = "metroPanel4";
            this.metroPanel4.Size = new System.Drawing.Size(805, 189);
            this.metroPanel4.TabIndex = 14;
            this.metroPanel4.VerticalScrollbarBarColor = true;
            this.metroPanel4.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel4.VerticalScrollbarSize = 10;
            // 
            // Panel_Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Panel_Server";
            this.Panel_Base.ResumeLayout(false);
            this.Panel_Base.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_serverinfo)).EndInit();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.metroPanel4.ResumeLayout(false);
            this.metroPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SGridView customGridView_serverinfo;
        private MetroFramework.Controls.MetroButton metroButton_server_refresh;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_port;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_fps;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel_detail_dn;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_dn;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_ip;
        private MetroFramework.Controls.MetroLabel metroLabel;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_user_cnt;
        private MetroFramework.Controls.MetroLabel metroLabel_detail_client_count;
        private MetroFramework.Controls.MetroCheckBox metroCheckBox_detail_live;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_type;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel_detail_dbfps;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_dbfps;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroButton metroButton_service_type;
        private MetroFramework.Controls.MetroComboBox metroComboBox_service_type;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private MetroFramework.Controls.MetroCheckBox metroCheckBox_live;
        private MetroFramework.Controls.MetroPanel metroPanel4;
    }
}
