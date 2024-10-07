namespace OperTool.Panel
{
    partial class Panel_BlockList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.sLabel_total = new OperTool.Controls.SLabel();
            this.metroButton_block_user_export = new MetroFramework.Controls.MetroButton();
            this.metroButton_block_user_search = new MetroFramework.Controls.MetroButton();
            this.metroTextBox_search_block_user = new MetroFramework.Controls.MetroTextBox();
            this.metroComboBox_search_block_user_type = new MetroFramework.Controls.MetroComboBox();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.metroCheckBox_detail_blocked = new MetroFramework.Controls.MetroCheckBox();
            this.metroTextBox_detail_expire_time = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox_detail_block_cnt = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox_detail_deviceid = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox_block_list_total = new MetroFramework.Controls.MetroTextBox();
            this.metroButton_block_refresh = new MetroFramework.Controls.MetroButton();
            this.customGridView_block_user = new OperTool.Controls.SGridView();
            this.metroComboBox_block_period = new MetroFramework.Controls.MetroComboBox();
            this.metroTextBox_block_period = new MetroFramework.Controls.MetroTextBox();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroTextBox_add_block_total_cnt = new MetroFramework.Controls.MetroTextBox();
            this.metroButton_add_block = new MetroFramework.Controls.MetroButton();
            this.metroButton_add_block_clear = new MetroFramework.Controls.MetroButton();
            this.metroButton_add_block_bulk = new MetroFramework.Controls.MetroButton();
            this.customGridView_add_block = new OperTool.Controls.SGridView();
            this.sLabel_period = new OperTool.Controls.SLabel();
            this.Panel_Base.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            this.metroPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_block_user)).BeginInit();
            this.metroPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_add_block)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel_Base
            // 
            this.Panel_Base.Controls.Add(this.metroPanel2);
            this.Panel_Base.Controls.Add(this.metroPanel1);
            this.Panel_Base.Controls.SetChildIndex(this.Panel_Label, 0);
            this.Panel_Base.Controls.SetChildIndex(this.metroPanel1, 0);
            this.Panel_Base.Controls.SetChildIndex(this.metroPanel2, 0);
            // 
            // metroPanel1
            // 
            this.metroPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel1.Controls.Add(this.sLabel_total);
            this.metroPanel1.Controls.Add(this.metroButton_block_user_export);
            this.metroPanel1.Controls.Add(this.metroButton_block_user_search);
            this.metroPanel1.Controls.Add(this.metroTextBox_search_block_user);
            this.metroPanel1.Controls.Add(this.metroComboBox_search_block_user_type);
            this.metroPanel1.Controls.Add(this.metroPanel3);
            this.metroPanel1.Controls.Add(this.metroTextBox_block_list_total);
            this.metroPanel1.Controls.Add(this.metroButton_block_refresh);
            this.metroPanel1.Controls.Add(this.customGridView_block_user);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(21, 67);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(597, 450);
            this.metroPanel1.TabIndex = 3;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // sLabel_total
            // 
            this.sLabel_total.AutoSize = true;
            this.sLabel_total.Location = new System.Drawing.Point(17, 13);
            this.sLabel_total.Name = "sLabel_total";
            this.sLabel_total.Size = new System.Drawing.Size(40, 19);
            this.sLabel_total.TabIndex = 11;
            this.sLabel_total.Text = "!Total";
            // 
            // metroButton_block_user_export
            // 
            this.metroButton_block_user_export.Location = new System.Drawing.Point(323, 412);
            this.metroButton_block_user_export.Name = "metroButton_block_user_export";
            this.metroButton_block_user_export.Size = new System.Drawing.Size(92, 23);
            this.metroButton_block_user_export.TabIndex = 10;
            this.metroButton_block_user_export.Text = "Export";
            this.metroButton_block_user_export.UseSelectable = true;
            this.metroButton_block_user_export.Click += new System.EventHandler(this.metroButton_block_user_export_Click);
            // 
            // metroButton_block_user_search
            // 
            this.metroButton_block_user_search.Location = new System.Drawing.Point(505, 73);
            this.metroButton_block_user_search.Name = "metroButton_block_user_search";
            this.metroButton_block_user_search.Size = new System.Drawing.Size(75, 23);
            this.metroButton_block_user_search.TabIndex = 9;
            this.metroButton_block_user_search.Text = "Search";
            this.metroButton_block_user_search.UseSelectable = true;
            this.metroButton_block_user_search.Click += new System.EventHandler(this.metroButton_block_user_search_Click);
            // 
            // metroTextBox_search_block_user
            // 
            // 
            // 
            // 
            this.metroTextBox_search_block_user.CustomButton.Image = null;
            this.metroTextBox_search_block_user.CustomButton.Location = new System.Drawing.Point(128, 1);
            this.metroTextBox_search_block_user.CustomButton.Name = "";
            this.metroTextBox_search_block_user.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_search_block_user.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_search_block_user.CustomButton.TabIndex = 1;
            this.metroTextBox_search_block_user.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_search_block_user.CustomButton.UseSelectable = true;
            this.metroTextBox_search_block_user.CustomButton.Visible = false;
            this.metroTextBox_search_block_user.Lines = new string[0];
            this.metroTextBox_search_block_user.Location = new System.Drawing.Point(430, 44);
            this.metroTextBox_search_block_user.MaxLength = 32767;
            this.metroTextBox_search_block_user.Name = "metroTextBox_search_block_user";
            this.metroTextBox_search_block_user.PasswordChar = '\0';
            this.metroTextBox_search_block_user.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_search_block_user.SelectedText = "";
            this.metroTextBox_search_block_user.SelectionLength = 0;
            this.metroTextBox_search_block_user.SelectionStart = 0;
            this.metroTextBox_search_block_user.ShortcutsEnabled = true;
            this.metroTextBox_search_block_user.Size = new System.Drawing.Size(150, 23);
            this.metroTextBox_search_block_user.TabIndex = 8;
            this.metroTextBox_search_block_user.UseSelectable = true;
            this.metroTextBox_search_block_user.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_search_block_user.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroComboBox_search_block_user_type
            // 
            this.metroComboBox_search_block_user_type.FormattingEnabled = true;
            this.metroComboBox_search_block_user_type.ItemHeight = 23;
            this.metroComboBox_search_block_user_type.Location = new System.Drawing.Point(323, 41);
            this.metroComboBox_search_block_user_type.Name = "metroComboBox_search_block_user_type";
            this.metroComboBox_search_block_user_type.Size = new System.Drawing.Size(101, 29);
            this.metroComboBox_search_block_user_type.TabIndex = 7;
            this.metroComboBox_search_block_user_type.UseSelectable = true;
            // 
            // metroPanel3
            // 
            this.metroPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel3.Controls.Add(this.metroCheckBox_detail_blocked);
            this.metroPanel3.Controls.Add(this.metroTextBox_detail_expire_time);
            this.metroPanel3.Controls.Add(this.metroLabel5);
            this.metroPanel3.Controls.Add(this.metroTextBox_detail_block_cnt);
            this.metroPanel3.Controls.Add(this.metroLabel4);
            this.metroPanel3.Controls.Add(this.metroTextBox_detail_deviceid);
            this.metroPanel3.Controls.Add(this.metroLabel3);
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(323, 102);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(257, 128);
            this.metroPanel3.TabIndex = 6;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // metroCheckBox_detail_blocked
            // 
            this.metroCheckBox_detail_blocked.AutoSize = true;
            this.metroCheckBox_detail_blocked.Location = new System.Drawing.Point(176, 99);
            this.metroCheckBox_detail_blocked.Name = "metroCheckBox_detail_blocked";
            this.metroCheckBox_detail_blocked.Size = new System.Drawing.Size(65, 15);
            this.metroCheckBox_detail_blocked.TabIndex = 8;
            this.metroCheckBox_detail_blocked.Text = "Blocked";
            this.metroCheckBox_detail_blocked.UseSelectable = true;
            // 
            // metroTextBox_detail_expire_time
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_expire_time.CustomButton.Image = null;
            this.metroTextBox_detail_expire_time.CustomButton.Location = new System.Drawing.Point(134, 1);
            this.metroTextBox_detail_expire_time.CustomButton.Name = "";
            this.metroTextBox_detail_expire_time.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_expire_time.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_expire_time.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_expire_time.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_expire_time.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_expire_time.CustomButton.Visible = false;
            this.metroTextBox_detail_expire_time.Lines = new string[] {
        "metroTextBox3"};
            this.metroTextBox_detail_expire_time.Location = new System.Drawing.Point(85, 70);
            this.metroTextBox_detail_expire_time.MaxLength = 32767;
            this.metroTextBox_detail_expire_time.Name = "metroTextBox_detail_expire_time";
            this.metroTextBox_detail_expire_time.PasswordChar = '\0';
            this.metroTextBox_detail_expire_time.ReadOnly = true;
            this.metroTextBox_detail_expire_time.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_expire_time.SelectedText = "";
            this.metroTextBox_detail_expire_time.SelectionLength = 0;
            this.metroTextBox_detail_expire_time.SelectionStart = 0;
            this.metroTextBox_detail_expire_time.ShortcutsEnabled = true;
            this.metroTextBox_detail_expire_time.Size = new System.Drawing.Size(156, 23);
            this.metroTextBox_detail_expire_time.TabIndex = 7;
            this.metroTextBox_detail_expire_time.Text = "metroTextBox3";
            this.metroTextBox_detail_expire_time.UseSelectable = true;
            this.metroTextBox_detail_expire_time.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_expire_time.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(8, 70);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(74, 19);
            this.metroLabel5.TabIndex = 6;
            this.metroLabel5.Text = "ExpireTime";
            // 
            // metroTextBox_detail_block_cnt
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_block_cnt.CustomButton.Image = null;
            this.metroTextBox_detail_block_cnt.CustomButton.Location = new System.Drawing.Point(134, 1);
            this.metroTextBox_detail_block_cnt.CustomButton.Name = "";
            this.metroTextBox_detail_block_cnt.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_block_cnt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_block_cnt.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_block_cnt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_block_cnt.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_block_cnt.CustomButton.Visible = false;
            this.metroTextBox_detail_block_cnt.Lines = new string[] {
        "metroTextBox2"};
            this.metroTextBox_detail_block_cnt.Location = new System.Drawing.Point(85, 39);
            this.metroTextBox_detail_block_cnt.MaxLength = 32767;
            this.metroTextBox_detail_block_cnt.Name = "metroTextBox_detail_block_cnt";
            this.metroTextBox_detail_block_cnt.PasswordChar = '\0';
            this.metroTextBox_detail_block_cnt.ReadOnly = true;
            this.metroTextBox_detail_block_cnt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_block_cnt.SelectedText = "";
            this.metroTextBox_detail_block_cnt.SelectionLength = 0;
            this.metroTextBox_detail_block_cnt.SelectionStart = 0;
            this.metroTextBox_detail_block_cnt.ShortcutsEnabled = true;
            this.metroTextBox_detail_block_cnt.Size = new System.Drawing.Size(156, 23);
            this.metroTextBox_detail_block_cnt.TabIndex = 5;
            this.metroTextBox_detail_block_cnt.Text = "metroTextBox2";
            this.metroTextBox_detail_block_cnt.UseSelectable = true;
            this.metroTextBox_detail_block_cnt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_block_cnt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(8, 39);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(75, 19);
            this.metroLabel4.TabIndex = 4;
            this.metroLabel4.Text = "BlockCount";
            // 
            // metroTextBox_detail_deviceid
            // 
            // 
            // 
            // 
            this.metroTextBox_detail_deviceid.CustomButton.Image = null;
            this.metroTextBox_detail_deviceid.CustomButton.Location = new System.Drawing.Point(134, 1);
            this.metroTextBox_detail_deviceid.CustomButton.Name = "";
            this.metroTextBox_detail_deviceid.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_detail_deviceid.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_detail_deviceid.CustomButton.TabIndex = 1;
            this.metroTextBox_detail_deviceid.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_detail_deviceid.CustomButton.UseSelectable = true;
            this.metroTextBox_detail_deviceid.CustomButton.Visible = false;
            this.metroTextBox_detail_deviceid.Lines = new string[] {
        "metroTextBox1"};
            this.metroTextBox_detail_deviceid.Location = new System.Drawing.Point(85, 10);
            this.metroTextBox_detail_deviceid.MaxLength = 32767;
            this.metroTextBox_detail_deviceid.Name = "metroTextBox_detail_deviceid";
            this.metroTextBox_detail_deviceid.PasswordChar = '\0';
            this.metroTextBox_detail_deviceid.ReadOnly = true;
            this.metroTextBox_detail_deviceid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_detail_deviceid.SelectedText = "";
            this.metroTextBox_detail_deviceid.SelectionLength = 0;
            this.metroTextBox_detail_deviceid.SelectionStart = 0;
            this.metroTextBox_detail_deviceid.ShortcutsEnabled = true;
            this.metroTextBox_detail_deviceid.Size = new System.Drawing.Size(156, 23);
            this.metroTextBox_detail_deviceid.TabIndex = 3;
            this.metroTextBox_detail_deviceid.Text = "metroTextBox1";
            this.metroTextBox_detail_deviceid.UseSelectable = true;
            this.metroTextBox_detail_deviceid.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_detail_deviceid.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(8, 12);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(59, 19);
            this.metroLabel3.TabIndex = 2;
            this.metroLabel3.Text = "DeviceID";
            // 
            // metroTextBox_block_list_total
            // 
            // 
            // 
            // 
            this.metroTextBox_block_list_total.CustomButton.Image = null;
            this.metroTextBox_block_list_total.CustomButton.Location = new System.Drawing.Point(71, 1);
            this.metroTextBox_block_list_total.CustomButton.Name = "";
            this.metroTextBox_block_list_total.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_block_list_total.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_block_list_total.CustomButton.TabIndex = 1;
            this.metroTextBox_block_list_total.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_block_list_total.CustomButton.UseSelectable = true;
            this.metroTextBox_block_list_total.CustomButton.Visible = false;
            this.metroTextBox_block_list_total.Lines = new string[0];
            this.metroTextBox_block_list_total.Location = new System.Drawing.Point(67, 12);
            this.metroTextBox_block_list_total.MaxLength = 32767;
            this.metroTextBox_block_list_total.Name = "metroTextBox_block_list_total";
            this.metroTextBox_block_list_total.PasswordChar = '\0';
            this.metroTextBox_block_list_total.ReadOnly = true;
            this.metroTextBox_block_list_total.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_block_list_total.SelectedText = "";
            this.metroTextBox_block_list_total.SelectionLength = 0;
            this.metroTextBox_block_list_total.SelectionStart = 0;
            this.metroTextBox_block_list_total.ShortcutsEnabled = true;
            this.metroTextBox_block_list_total.Size = new System.Drawing.Size(93, 23);
            this.metroTextBox_block_list_total.TabIndex = 5;
            this.metroTextBox_block_list_total.UseSelectable = true;
            this.metroTextBox_block_list_total.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_block_list_total.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroButton_block_refresh
            // 
            this.metroButton_block_refresh.Location = new System.Drawing.Point(232, 12);
            this.metroButton_block_refresh.Name = "metroButton_block_refresh";
            this.metroButton_block_refresh.Size = new System.Drawing.Size(75, 23);
            this.metroButton_block_refresh.TabIndex = 3;
            this.metroButton_block_refresh.Text = "Refresh";
            this.metroButton_block_refresh.UseSelectable = true;
            this.metroButton_block_refresh.Click += new System.EventHandler(this.metroButton_block_refresh_Click);
            // 
            // customGridView_block_user
            // 
            this.customGridView_block_user.AllowUserToAddRows = false;
            this.customGridView_block_user.AllowUserToDeleteRows = false;
            this.customGridView_block_user.AllowUserToResizeRows = false;
            this.customGridView_block_user.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_block_user.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customGridView_block_user.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customGridView_block_user.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_block_user.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.customGridView_block_user.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customGridView_block_user.DefaultCellStyle = dataGridViewCellStyle11;
            this.customGridView_block_user.EnableHeadersVisualStyles = false;
            this.customGridView_block_user.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.customGridView_block_user.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_block_user.Location = new System.Drawing.Point(17, 41);
            this.customGridView_block_user.Name = "customGridView_block_user";
            this.customGridView_block_user.ReadOnly = true;
            this.customGridView_block_user.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_block_user.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.customGridView_block_user.RowHeadersVisible = false;
            this.customGridView_block_user.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.customGridView_block_user.RowTemplate.Height = 23;
            this.customGridView_block_user.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customGridView_block_user.Size = new System.Drawing.Size(290, 394);
            this.customGridView_block_user.TabIndex = 2;
            // 
            // metroComboBox_block_period
            // 
            this.metroComboBox_block_period.FormattingEnabled = true;
            this.metroComboBox_block_period.ItemHeight = 23;
            this.metroComboBox_block_period.Location = new System.Drawing.Point(69, 9);
            this.metroComboBox_block_period.Name = "metroComboBox_block_period";
            this.metroComboBox_block_period.Size = new System.Drawing.Size(115, 29);
            this.metroComboBox_block_period.TabIndex = 5;
            this.metroComboBox_block_period.UseSelectable = true;
            // 
            // metroTextBox_block_period
            // 
            // 
            // 
            // 
            this.metroTextBox_block_period.CustomButton.Image = null;
            this.metroTextBox_block_period.CustomButton.Location = new System.Drawing.Point(142, 1);
            this.metroTextBox_block_period.CustomButton.Name = "";
            this.metroTextBox_block_period.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_block_period.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_block_period.CustomButton.TabIndex = 1;
            this.metroTextBox_block_period.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_block_period.CustomButton.UseSelectable = true;
            this.metroTextBox_block_period.CustomButton.Visible = false;
            this.metroTextBox_block_period.Lines = new string[0];
            this.metroTextBox_block_period.Location = new System.Drawing.Point(190, 12);
            this.metroTextBox_block_period.MaxLength = 32767;
            this.metroTextBox_block_period.Name = "metroTextBox_block_period";
            this.metroTextBox_block_period.PasswordChar = '\0';
            this.metroTextBox_block_period.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_block_period.SelectedText = "";
            this.metroTextBox_block_period.SelectionLength = 0;
            this.metroTextBox_block_period.SelectionStart = 0;
            this.metroTextBox_block_period.ShortcutsEnabled = true;
            this.metroTextBox_block_period.Size = new System.Drawing.Size(164, 23);
            this.metroTextBox_block_period.TabIndex = 6;
            this.metroTextBox_block_period.UseSelectable = true;
            this.metroTextBox_block_period.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_block_period.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroPanel2
            // 
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel2.Controls.Add(this.sLabel_period);
            this.metroPanel2.Controls.Add(this.metroButton1);
            this.metroPanel2.Controls.Add(this.metroTextBox_add_block_total_cnt);
            this.metroPanel2.Controls.Add(this.metroButton_add_block);
            this.metroPanel2.Controls.Add(this.metroButton_add_block_clear);
            this.metroPanel2.Controls.Add(this.metroButton_add_block_bulk);
            this.metroPanel2.Controls.Add(this.customGridView_add_block);
            this.metroPanel2.Controls.Add(this.metroTextBox_block_period);
            this.metroPanel2.Controls.Add(this.metroComboBox_block_period);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(635, 67);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(370, 450);
            this.metroPanel2.TabIndex = 4;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(271, 297);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(75, 32);
            this.metroButton1.TabIndex = 12;
            this.metroButton1.Text = "Apply";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroTextBox_add_block_total_cnt
            // 
            // 
            // 
            // 
            this.metroTextBox_add_block_total_cnt.CustomButton.Image = null;
            this.metroTextBox_add_block_total_cnt.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.metroTextBox_add_block_total_cnt.CustomButton.Name = "";
            this.metroTextBox_add_block_total_cnt.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_add_block_total_cnt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_add_block_total_cnt.CustomButton.TabIndex = 1;
            this.metroTextBox_add_block_total_cnt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_add_block_total_cnt.CustomButton.UseSelectable = true;
            this.metroTextBox_add_block_total_cnt.CustomButton.Visible = false;
            this.metroTextBox_add_block_total_cnt.Lines = new string[0];
            this.metroTextBox_add_block_total_cnt.Location = new System.Drawing.Point(279, 41);
            this.metroTextBox_add_block_total_cnt.MaxLength = 32767;
            this.metroTextBox_add_block_total_cnt.Name = "metroTextBox_add_block_total_cnt";
            this.metroTextBox_add_block_total_cnt.PasswordChar = '\0';
            this.metroTextBox_add_block_total_cnt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_add_block_total_cnt.SelectedText = "";
            this.metroTextBox_add_block_total_cnt.SelectionLength = 0;
            this.metroTextBox_add_block_total_cnt.SelectionStart = 0;
            this.metroTextBox_add_block_total_cnt.ShortcutsEnabled = true;
            this.metroTextBox_add_block_total_cnt.Size = new System.Drawing.Size(75, 23);
            this.metroTextBox_add_block_total_cnt.TabIndex = 11;
            this.metroTextBox_add_block_total_cnt.UseSelectable = true;
            this.metroTextBox_add_block_total_cnt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_add_block_total_cnt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroButton_add_block
            // 
            this.metroButton_add_block.Location = new System.Drawing.Point(190, 297);
            this.metroButton_add_block.Name = "metroButton_add_block";
            this.metroButton_add_block.Size = new System.Drawing.Size(75, 32);
            this.metroButton_add_block.TabIndex = 10;
            this.metroButton_add_block.Text = "Export";
            this.metroButton_add_block.UseSelectable = true;
            this.metroButton_add_block.Click += new System.EventHandler(this.metroButton_add_block_Click);
            // 
            // metroButton_add_block_clear
            // 
            this.metroButton_add_block_clear.Location = new System.Drawing.Point(109, 297);
            this.metroButton_add_block_clear.Name = "metroButton_add_block_clear";
            this.metroButton_add_block_clear.Size = new System.Drawing.Size(75, 32);
            this.metroButton_add_block_clear.TabIndex = 9;
            this.metroButton_add_block_clear.Text = "Clear";
            this.metroButton_add_block_clear.UseSelectable = true;
            this.metroButton_add_block_clear.Click += new System.EventHandler(this.metroButton_add_block_clear_Click);
            // 
            // metroButton_add_block_bulk
            // 
            this.metroButton_add_block_bulk.Location = new System.Drawing.Point(26, 297);
            this.metroButton_add_block_bulk.Name = "metroButton_add_block_bulk";
            this.metroButton_add_block_bulk.Size = new System.Drawing.Size(75, 32);
            this.metroButton_add_block_bulk.TabIndex = 8;
            this.metroButton_add_block_bulk.Text = "Bulk";
            this.metroButton_add_block_bulk.UseSelectable = true;
            // 
            // customGridView_add_block
            // 
            this.customGridView_add_block.AllowUserToAddRows = false;
            this.customGridView_add_block.AllowUserToDeleteRows = false;
            this.customGridView_add_block.AllowUserToResizeRows = false;
            this.customGridView_add_block.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_add_block.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customGridView_add_block.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customGridView_add_block.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_add_block.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.customGridView_add_block.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customGridView_add_block.DefaultCellStyle = dataGridViewCellStyle8;
            this.customGridView_add_block.EnableHeadersVisualStyles = false;
            this.customGridView_add_block.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.customGridView_add_block.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_add_block.Location = new System.Drawing.Point(16, 73);
            this.customGridView_add_block.Name = "customGridView_add_block";
            this.customGridView_add_block.ReadOnly = true;
            this.customGridView_add_block.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_add_block.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.customGridView_add_block.RowHeadersVisible = false;
            this.customGridView_add_block.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.customGridView_add_block.RowTemplate.Height = 23;
            this.customGridView_add_block.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customGridView_add_block.Size = new System.Drawing.Size(338, 211);
            this.customGridView_add_block.TabIndex = 7;
            // 
            // sLabel_period
            // 
            this.sLabel_period.AutoSize = true;
            this.sLabel_period.Location = new System.Drawing.Point(8, 13);
            this.sLabel_period.Name = "sLabel_period";
            this.sLabel_period.Size = new System.Drawing.Size(52, 19);
            this.sLabel_period.TabIndex = 13;
            this.sLabel_period.Text = "!period";
            // 
            // Panel_BlockList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Panel_BlockList";
            this.Panel_Base.ResumeLayout(false);
            this.Panel_Base.PerformLayout();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.metroPanel3.ResumeLayout(false);
            this.metroPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_block_user)).EndInit();
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_add_block)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel1;
        private Controls.SGridView customGridView_block_user;
        private MetroFramework.Controls.MetroButton metroButton_block_refresh;
        private MetroFramework.Controls.MetroTextBox metroTextBox_block_period;
        private MetroFramework.Controls.MetroComboBox metroComboBox_block_period;
        private MetroFramework.Controls.MetroTextBox metroTextBox_block_list_total;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private Controls.SGridView customGridView_add_block;
        private MetroFramework.Controls.MetroButton metroButton_add_block_bulk;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private MetroFramework.Controls.MetroButton metroButton_add_block;
        private MetroFramework.Controls.MetroButton metroButton_add_block_clear;
        private MetroFramework.Controls.MetroTextBox metroTextBox_add_block_total_cnt;
        private MetroFramework.Controls.MetroButton metroButton_block_user_export;
        private MetroFramework.Controls.MetroButton metroButton_block_user_search;
        private MetroFramework.Controls.MetroTextBox metroTextBox_search_block_user;
        private MetroFramework.Controls.MetroComboBox metroComboBox_search_block_user_type;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_expire_time;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_block_cnt;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroTextBox metroTextBox_detail_deviceid;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroCheckBox metroCheckBox_detail_blocked;
        private MetroFramework.Controls.MetroButton metroButton1;
        private Controls.SLabel sLabel_total;
        private Controls.SLabel sLabel_period;
    }
}
