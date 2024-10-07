namespace OperTool.Panel
{
    partial class Panel_GameLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.sTextBox_filter_uid = new OperTool.Controls.STextBox();
            this.sTextBox_filter_deviceId = new OperTool.Controls.STextBox();
            this.sButton_log_search = new OperTool.Controls.SButton();
            this.sGridView_uid = new OperTool.Controls.SGridView();
            this.sGridView_device_id = new OperTool.Controls.SGridView();
            this.sGridView_log_type = new OperTool.Controls.SGridView();
            this.sDateTimePicker_end = new OperTool.Controls.SDateTimePicker();
            this.sDateTimePicker_start = new OperTool.Controls.SDateTimePicker();
            this.sButton_uid_add = new OperTool.Controls.SButton();
            this.sButton_device_id_add = new OperTool.Controls.SButton();
            this.sButton_log_type_add = new OperTool.Controls.SButton();
            this.sLabel_end = new OperTool.Controls.SLabel();
            this.sLabel_start = new OperTool.Controls.SLabel();
            this.sLabel_uid = new OperTool.Controls.SLabel();
            this.sLabel_device_id = new OperTool.Controls.SLabel();
            this.sLabel_log_type = new OperTool.Controls.SLabel();
            this.sCombobox_log_type = new OperTool.Controls.SCombobox();
            this.sGridView_log = new OperTool.Controls.SGridView();
            this.sButton_log_export = new OperTool.Controls.SButton();
            this.sButton_log_clear = new OperTool.Controls.SButton();
            this.Panel_Base.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            this.metroPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sGridView_uid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sGridView_device_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sGridView_log_type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sGridView_log)).BeginInit();
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
            this.metroPanel1.Controls.Add(this.sButton_log_clear);
            this.metroPanel1.Controls.Add(this.sButton_log_export);
            this.metroPanel1.Controls.Add(this.sGridView_log);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(379, 53);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(629, 476);
            this.metroPanel1.TabIndex = 10;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // metroPanel2
            // 
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel2.Controls.Add(this.sTextBox_filter_uid);
            this.metroPanel2.Controls.Add(this.sTextBox_filter_deviceId);
            this.metroPanel2.Controls.Add(this.sButton_log_search);
            this.metroPanel2.Controls.Add(this.sGridView_uid);
            this.metroPanel2.Controls.Add(this.sGridView_device_id);
            this.metroPanel2.Controls.Add(this.sGridView_log_type);
            this.metroPanel2.Controls.Add(this.sDateTimePicker_end);
            this.metroPanel2.Controls.Add(this.sDateTimePicker_start);
            this.metroPanel2.Controls.Add(this.sButton_uid_add);
            this.metroPanel2.Controls.Add(this.sButton_device_id_add);
            this.metroPanel2.Controls.Add(this.sButton_log_type_add);
            this.metroPanel2.Controls.Add(this.sLabel_end);
            this.metroPanel2.Controls.Add(this.sLabel_start);
            this.metroPanel2.Controls.Add(this.sLabel_uid);
            this.metroPanel2.Controls.Add(this.sLabel_device_id);
            this.metroPanel2.Controls.Add(this.sLabel_log_type);
            this.metroPanel2.Controls.Add(this.sCombobox_log_type);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(29, 53);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(327, 480);
            this.metroPanel2.TabIndex = 15;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // sTextBox_filter_uid
            // 
            // 
            // 
            // 
            this.sTextBox_filter_uid.CustomButton.Image = null;
            this.sTextBox_filter_uid.CustomButton.Location = new System.Drawing.Point(144, 1);
            this.sTextBox_filter_uid.CustomButton.Name = "";
            this.sTextBox_filter_uid.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.sTextBox_filter_uid.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.sTextBox_filter_uid.CustomButton.TabIndex = 1;
            this.sTextBox_filter_uid.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.sTextBox_filter_uid.CustomButton.UseSelectable = true;
            this.sTextBox_filter_uid.CustomButton.Visible = false;
            this.sTextBox_filter_uid.Lines = new string[0];
            this.sTextBox_filter_uid.Location = new System.Drawing.Point(14, 285);
            this.sTextBox_filter_uid.MaxLength = 32767;
            this.sTextBox_filter_uid.Name = "sTextBox_filter_uid";
            this.sTextBox_filter_uid.PasswordChar = '\0';
            this.sTextBox_filter_uid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_filter_uid.SelectedText = "";
            this.sTextBox_filter_uid.SelectionLength = 0;
            this.sTextBox_filter_uid.SelectionStart = 0;
            this.sTextBox_filter_uid.ShortcutsEnabled = true;
            this.sTextBox_filter_uid.Size = new System.Drawing.Size(166, 23);
            this.sTextBox_filter_uid.TabIndex = 53;
            this.sTextBox_filter_uid.UseSelectable = true;
            this.sTextBox_filter_uid.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_filter_uid.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // sTextBox_filter_deviceId
            // 
            // 
            // 
            // 
            this.sTextBox_filter_deviceId.CustomButton.Image = null;
            this.sTextBox_filter_deviceId.CustomButton.Location = new System.Drawing.Point(144, 1);
            this.sTextBox_filter_deviceId.CustomButton.Name = "";
            this.sTextBox_filter_deviceId.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.sTextBox_filter_deviceId.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.sTextBox_filter_deviceId.CustomButton.TabIndex = 1;
            this.sTextBox_filter_deviceId.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.sTextBox_filter_deviceId.CustomButton.UseSelectable = true;
            this.sTextBox_filter_deviceId.CustomButton.Visible = false;
            this.sTextBox_filter_deviceId.Lines = new string[0];
            this.sTextBox_filter_deviceId.Location = new System.Drawing.Point(14, 159);
            this.sTextBox_filter_deviceId.MaxLength = 32767;
            this.sTextBox_filter_deviceId.Name = "sTextBox_filter_deviceId";
            this.sTextBox_filter_deviceId.PasswordChar = '\0';
            this.sTextBox_filter_deviceId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_filter_deviceId.SelectedText = "";
            this.sTextBox_filter_deviceId.SelectionLength = 0;
            this.sTextBox_filter_deviceId.SelectionStart = 0;
            this.sTextBox_filter_deviceId.ShortcutsEnabled = true;
            this.sTextBox_filter_deviceId.Size = new System.Drawing.Size(166, 23);
            this.sTextBox_filter_deviceId.TabIndex = 52;
            this.sTextBox_filter_deviceId.UseSelectable = true;
            this.sTextBox_filter_deviceId.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_filter_deviceId.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // sButton_log_search
            // 
            this.sButton_log_search.Location = new System.Drawing.Point(233, 443);
            this.sButton_log_search.Name = "sButton_log_search";
            this.sButton_log_search.Size = new System.Drawing.Size(89, 32);
            this.sButton_log_search.TabIndex = 48;
            this.sButton_log_search.Text = "Search";
            this.sButton_log_search.UseSelectable = true;
            this.sButton_log_search.Click += new System.EventHandler(this.onClick_logbuttonsearch);
            // 
            // sGridView_uid
            // 
            this.sGridView_uid.AllowUserToAddRows = false;
            this.sGridView_uid.AllowUserToDeleteRows = false;
            this.sGridView_uid.AllowUserToResizeRows = false;
            this.sGridView_uid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sGridView_uid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sGridView_uid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sGridView_uid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sGridView_uid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sGridView_uid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sGridView_uid.DefaultCellStyle = dataGridViewCellStyle2;
            this.sGridView_uid.EnableHeadersVisualStyles = false;
            this.sGridView_uid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.sGridView_uid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sGridView_uid.Location = new System.Drawing.Point(14, 315);
            this.sGridView_uid.Name = "sGridView_uid";
            this.sGridView_uid.ReadOnly = true;
            this.sGridView_uid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sGridView_uid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.sGridView_uid.RowHeadersVisible = false;
            this.sGridView_uid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.sGridView_uid.RowTemplate.Height = 23;
            this.sGridView_uid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sGridView_uid.Size = new System.Drawing.Size(166, 64);
            this.sGridView_uid.TabIndex = 51;
            this.sGridView_uid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.onGridViewDoubleClick_filter_list_uid);
            // 
            // sGridView_device_id
            // 
            this.sGridView_device_id.AllowUserToAddRows = false;
            this.sGridView_device_id.AllowUserToDeleteRows = false;
            this.sGridView_device_id.AllowUserToResizeRows = false;
            this.sGridView_device_id.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sGridView_device_id.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sGridView_device_id.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sGridView_device_id.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sGridView_device_id.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.sGridView_device_id.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sGridView_device_id.DefaultCellStyle = dataGridViewCellStyle5;
            this.sGridView_device_id.EnableHeadersVisualStyles = false;
            this.sGridView_device_id.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.sGridView_device_id.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sGridView_device_id.Location = new System.Drawing.Point(14, 188);
            this.sGridView_device_id.Name = "sGridView_device_id";
            this.sGridView_device_id.ReadOnly = true;
            this.sGridView_device_id.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sGridView_device_id.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.sGridView_device_id.RowHeadersVisible = false;
            this.sGridView_device_id.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.sGridView_device_id.RowTemplate.Height = 23;
            this.sGridView_device_id.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sGridView_device_id.Size = new System.Drawing.Size(166, 63);
            this.sGridView_device_id.TabIndex = 50;
            this.sGridView_device_id.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.onGridViewDoubleClick_filter_list_deviceid);
            // 
            // sGridView_log_type
            // 
            this.sGridView_log_type.AllowUserToAddRows = false;
            this.sGridView_log_type.AllowUserToDeleteRows = false;
            this.sGridView_log_type.AllowUserToResizeRows = false;
            this.sGridView_log_type.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sGridView_log_type.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sGridView_log_type.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sGridView_log_type.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sGridView_log_type.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.sGridView_log_type.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sGridView_log_type.DefaultCellStyle = dataGridViewCellStyle8;
            this.sGridView_log_type.EnableHeadersVisualStyles = false;
            this.sGridView_log_type.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.sGridView_log_type.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sGridView_log_type.Location = new System.Drawing.Point(14, 57);
            this.sGridView_log_type.Name = "sGridView_log_type";
            this.sGridView_log_type.ReadOnly = true;
            this.sGridView_log_type.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sGridView_log_type.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.sGridView_log_type.RowHeadersVisible = false;
            this.sGridView_log_type.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.sGridView_log_type.RowTemplate.Height = 23;
            this.sGridView_log_type.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sGridView_log_type.Size = new System.Drawing.Size(166, 73);
            this.sGridView_log_type.TabIndex = 49;
            this.sGridView_log_type.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.onGridViewDoubleClick_filter_list_type);
            // 
            // sDateTimePicker_end
            // 
            this.sDateTimePicker_end.CustomFormat = "yyyy-MM-dd HH:mm";
            this.sDateTimePicker_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.sDateTimePicker_end.Location = new System.Drawing.Point(10, 454);
            this.sDateTimePicker_end.MaxDate = new System.DateTime(2100, 12, 30, 0, 0, 0, 0);
            this.sDateTimePicker_end.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.sDateTimePicker_end.Name = "sDateTimePicker_end";
            this.sDateTimePicker_end.Size = new System.Drawing.Size(146, 21);
            this.sDateTimePicker_end.TabIndex = 47;
            // 
            // sDateTimePicker_start
            // 
            this.sDateTimePicker_start.CustomFormat = "yyyy-MM-dd HH:mm";
            this.sDateTimePicker_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.sDateTimePicker_start.Location = new System.Drawing.Point(10, 404);
            this.sDateTimePicker_start.MaxDate = new System.DateTime(2100, 12, 30, 0, 0, 0, 0);
            this.sDateTimePicker_start.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.sDateTimePicker_start.Name = "sDateTimePicker_start";
            this.sDateTimePicker_start.Size = new System.Drawing.Size(141, 21);
            this.sDateTimePicker_start.TabIndex = 46;
            // 
            // sButton_uid_add
            // 
            this.sButton_uid_add.Location = new System.Drawing.Point(186, 286);
            this.sButton_uid_add.Name = "sButton_uid_add";
            this.sButton_uid_add.Size = new System.Drawing.Size(63, 23);
            this.sButton_uid_add.TabIndex = 45;
            this.sButton_uid_add.Text = "Add";
            this.sButton_uid_add.UseSelectable = true;
            this.sButton_uid_add.Click += new System.EventHandler(this.onClick_filter_uid_add);
            // 
            // sButton_device_id_add
            // 
            this.sButton_device_id_add.Location = new System.Drawing.Point(186, 159);
            this.sButton_device_id_add.Name = "sButton_device_id_add";
            this.sButton_device_id_add.Size = new System.Drawing.Size(63, 23);
            this.sButton_device_id_add.TabIndex = 44;
            this.sButton_device_id_add.Text = "Add";
            this.sButton_device_id_add.UseSelectable = true;
            this.sButton_device_id_add.Click += new System.EventHandler(this.onClick_filter_deviceid_add);
            // 
            // sButton_log_type_add
            // 
            this.sButton_log_type_add.Location = new System.Drawing.Point(186, 28);
            this.sButton_log_type_add.Name = "sButton_log_type_add";
            this.sButton_log_type_add.Size = new System.Drawing.Size(63, 23);
            this.sButton_log_type_add.TabIndex = 43;
            this.sButton_log_type_add.Text = "Add";
            this.sButton_log_type_add.UseSelectable = true;
            this.sButton_log_type_add.Click += new System.EventHandler(this.onClick_filter_type_add);
            // 
            // sLabel_end
            // 
            this.sLabel_end.AutoSize = true;
            this.sLabel_end.Location = new System.Drawing.Point(10, 432);
            this.sLabel_end.Name = "sLabel_end";
            this.sLabel_end.Size = new System.Drawing.Size(43, 19);
            this.sLabel_end.TabIndex = 42;
            this.sLabel_end.Text = "!!!End";
            // 
            // sLabel_start
            // 
            this.sLabel_start.AutoSize = true;
            this.sLabel_start.Location = new System.Drawing.Point(10, 382);
            this.sLabel_start.Name = "sLabel_start";
            this.sLabel_start.Size = new System.Drawing.Size(49, 19);
            this.sLabel_start.TabIndex = 41;
            this.sLabel_start.Text = "!!!Start";
            // 
            // sLabel_uid
            // 
            this.sLabel_uid.AutoSize = true;
            this.sLabel_uid.Location = new System.Drawing.Point(14, 258);
            this.sLabel_uid.Name = "sLabel_uid";
            this.sLabel_uid.Size = new System.Drawing.Size(42, 19);
            this.sLabel_uid.TabIndex = 40;
            this.sLabel_uid.Text = "!!!UID";
            // 
            // sLabel_device_id
            // 
            this.sLabel_device_id.AutoSize = true;
            this.sLabel_device_id.Location = new System.Drawing.Point(10, 137);
            this.sLabel_device_id.Name = "sLabel_device_id";
            this.sLabel_device_id.Size = new System.Drawing.Size(71, 19);
            this.sLabel_device_id.TabIndex = 39;
            this.sLabel_device_id.Text = "!!!DeviceID";
            // 
            // sLabel_log_type
            // 
            this.sLabel_log_type.AutoSize = true;
            this.sLabel_log_type.Location = new System.Drawing.Point(14, 0);
            this.sLabel_log_type.Name = "sLabel_log_type";
            this.sLabel_log_type.Size = new System.Drawing.Size(58, 19);
            this.sLabel_log_type.TabIndex = 38;
            this.sLabel_log_type.Text = "LogType";
            // 
            // sCombobox_log_type
            // 
            this.sCombobox_log_type.FormattingEnabled = true;
            this.sCombobox_log_type.ItemHeight = 23;
            this.sCombobox_log_type.Location = new System.Drawing.Point(14, 22);
            this.sCombobox_log_type.Name = "sCombobox_log_type";
            this.sCombobox_log_type.Size = new System.Drawing.Size(166, 29);
            this.sCombobox_log_type.TabIndex = 37;
            this.sCombobox_log_type.UseSelectable = true;
            // 
            // sGridView_log
            // 
            this.sGridView_log.AllowUserToAddRows = false;
            this.sGridView_log.AllowUserToDeleteRows = false;
            this.sGridView_log.AllowUserToResizeRows = false;
            this.sGridView_log.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sGridView_log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sGridView_log.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sGridView_log.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sGridView_log.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.sGridView_log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sGridView_log.DefaultCellStyle = dataGridViewCellStyle11;
            this.sGridView_log.EnableHeadersVisualStyles = false;
            this.sGridView_log.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.sGridView_log.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.sGridView_log.Location = new System.Drawing.Point(22, 28);
            this.sGridView_log.Name = "sGridView_log";
            this.sGridView_log.ReadOnly = true;
            this.sGridView_log.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sGridView_log.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.sGridView_log.RowHeadersVisible = false;
            this.sGridView_log.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.sGridView_log.RowTemplate.Height = 23;
            this.sGridView_log.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sGridView_log.Size = new System.Drawing.Size(586, 379);
            this.sGridView_log.TabIndex = 2;
            // 
            // sButton_log_export
            // 
            this.sButton_log_export.Location = new System.Drawing.Point(533, 413);
            this.sButton_log_export.Name = "sButton_log_export";
            this.sButton_log_export.Size = new System.Drawing.Size(75, 38);
            this.sButton_log_export.TabIndex = 3;
            this.sButton_log_export.Text = "Export";
            this.sButton_log_export.UseSelectable = true;
            // 
            // sButton_log_clear
            // 
            this.sButton_log_clear.Location = new System.Drawing.Point(452, 413);
            this.sButton_log_clear.Name = "sButton_log_clear";
            this.sButton_log_clear.Size = new System.Drawing.Size(75, 38);
            this.sButton_log_clear.TabIndex = 4;
            this.sButton_log_clear.Text = "Clear";
            this.sButton_log_clear.UseSelectable = true;
            // 
            // Panel_GameLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Panel_GameLog";
            this.Panel_Base.ResumeLayout(false);
            this.Panel_Base.PerformLayout();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sGridView_uid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sGridView_device_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sGridView_log_type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sGridView_log)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private Controls.SCombobox sCombobox_log_type;
        private Controls.SLabel sLabel_device_id;
        private Controls.SLabel sLabel_log_type;
        private Controls.SButton sButton_log_type_add;
        private Controls.SLabel sLabel_end;
        private Controls.SLabel sLabel_start;
        private Controls.SLabel sLabel_uid;
        private Controls.SButton sButton_uid_add;
        private Controls.SButton sButton_device_id_add;
        private Controls.SDateTimePicker sDateTimePicker_end;
        private Controls.SDateTimePicker sDateTimePicker_start;
        private Controls.SButton sButton_log_search;
        private Controls.SGridView sGridView_device_id;
        private Controls.SGridView sGridView_log_type;
        private Controls.SGridView sGridView_uid;
        private Controls.STextBox sTextBox_filter_deviceId;
        private Controls.STextBox sTextBox_filter_uid;
        private Controls.SGridView sGridView_log;
        private Controls.SButton sButton_log_clear;
        private Controls.SButton sButton_log_export;
    }
}
