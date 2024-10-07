namespace OperTool.Panel
{
    partial class Panel_SystemPost
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
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.customGridView_post = new OperTool.Controls.SGridView();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroTextBox_post_detail_expire = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox_post_detail_start = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.customGridView_detail_items = new OperTool.Controls.SGridView();
            this.customGridView_detail_coins = new OperTool.Controls.SGridView();
            this.metroLabel_post_coins = new MetroFramework.Controls.MetroLabel();
            this.metroLabel_post_items = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.sLabel_system_post_start = new OperTool.Controls.SLabel();
            this.sLabel_system_post_end = new OperTool.Controls.SLabel();
            this.sDateTimePicker_system_post_start = new OperTool.Controls.SDateTimePicker();
            this.sDateTimePicker_system_post_end = new OperTool.Controls.SDateTimePicker();
            this.sButton_system_post_search = new OperTool.Controls.SButton();
            this.sLabel_detail_post_id = new OperTool.Controls.SLabel();
            this.sTextBox_detail_system_post_id = new OperTool.Controls.STextBox();
            this.sTextBox_detail_system_post_title = new OperTool.Controls.STextBox();
            this.sLabel_detail_system_post_title = new OperTool.Controls.SLabel();
            this.sLabel_detail_system_post_msg = new OperTool.Controls.SLabel();
            this.sTextBox_detail_system_post_msg = new OperTool.Controls.STextBox();
            this.Panel_Base.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_post)).BeginInit();
            this.metroPanel2.SuspendLayout();
            this.metroPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_detail_items)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_detail_coins)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel_Base
            // 
            this.Panel_Base.Controls.Add(this.sButton_system_post_search);
            this.Panel_Base.Controls.Add(this.sDateTimePicker_system_post_end);
            this.Panel_Base.Controls.Add(this.sDateTimePicker_system_post_start);
            this.Panel_Base.Controls.Add(this.sLabel_system_post_end);
            this.Panel_Base.Controls.Add(this.sLabel_system_post_start);
            this.Panel_Base.Controls.Add(this.metroPanel1);
            this.Panel_Base.Controls.SetChildIndex(this.Panel_Label, 0);
            this.Panel_Base.Controls.SetChildIndex(this.metroPanel1, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sLabel_system_post_start, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sLabel_system_post_end, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sDateTimePicker_system_post_start, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sDateTimePicker_system_post_end, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sButton_system_post_search, 0);
            // 
            // metroPanel1
            // 
            this.metroPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel1.Controls.Add(this.customGridView_post);
            this.metroPanel1.Controls.Add(this.metroPanel2);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(27, 127);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(992, 330);
            this.metroPanel1.TabIndex = 4;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // customGridView_post
            // 
            this.customGridView_post.AllowUserToAddRows = false;
            this.customGridView_post.AllowUserToDeleteRows = false;
            this.customGridView_post.AllowUserToResizeRows = false;
            this.customGridView_post.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_post.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customGridView_post.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customGridView_post.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_post.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.customGridView_post.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customGridView_post.DefaultCellStyle = dataGridViewCellStyle2;
            this.customGridView_post.EnableHeadersVisualStyles = false;
            this.customGridView_post.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.customGridView_post.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_post.Location = new System.Drawing.Point(13, 9);
            this.customGridView_post.Name = "customGridView_post";
            this.customGridView_post.ReadOnly = true;
            this.customGridView_post.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_post.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.customGridView_post.RowHeadersVisible = false;
            this.customGridView_post.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.customGridView_post.RowTemplate.Height = 23;
            this.customGridView_post.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customGridView_post.Size = new System.Drawing.Size(572, 313);
            this.customGridView_post.TabIndex = 4;
            this.customGridView_post.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customGridView1_CellContentDoubleClick);
            // 
            // metroPanel2
            // 
            this.metroPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel2.Controls.Add(this.sTextBox_detail_system_post_msg);
            this.metroPanel2.Controls.Add(this.sLabel_detail_system_post_msg);
            this.metroPanel2.Controls.Add(this.sLabel_detail_system_post_title);
            this.metroPanel2.Controls.Add(this.sTextBox_detail_system_post_title);
            this.metroPanel2.Controls.Add(this.sTextBox_detail_system_post_id);
            this.metroPanel2.Controls.Add(this.sLabel_detail_post_id);
            this.metroPanel2.Controls.Add(this.metroTextBox_post_detail_expire);
            this.metroPanel2.Controls.Add(this.metroTextBox_post_detail_start);
            this.metroPanel2.Controls.Add(this.metroLabel6);
            this.metroPanel2.Controls.Add(this.metroLabel5);
            this.metroPanel2.Controls.Add(this.metroPanel3);
            this.metroPanel2.Controls.Add(this.metroLabel1);
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(591, 5);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(394, 317);
            this.metroPanel2.TabIndex = 3;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // metroTextBox_post_detail_expire
            // 
            // 
            // 
            // 
            this.metroTextBox_post_detail_expire.CustomButton.Image = null;
            this.metroTextBox_post_detail_expire.CustomButton.Location = new System.Drawing.Point(119, 1);
            this.metroTextBox_post_detail_expire.CustomButton.Name = "";
            this.metroTextBox_post_detail_expire.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_post_detail_expire.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_post_detail_expire.CustomButton.TabIndex = 1;
            this.metroTextBox_post_detail_expire.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_post_detail_expire.CustomButton.UseSelectable = true;
            this.metroTextBox_post_detail_expire.CustomButton.Visible = false;
            this.metroTextBox_post_detail_expire.Lines = new string[] {
        "metroTextBox2"};
            this.metroTextBox_post_detail_expire.Location = new System.Drawing.Point(54, 281);
            this.metroTextBox_post_detail_expire.MaxLength = 32767;
            this.metroTextBox_post_detail_expire.Name = "metroTextBox_post_detail_expire";
            this.metroTextBox_post_detail_expire.PasswordChar = '\0';
            this.metroTextBox_post_detail_expire.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_post_detail_expire.SelectedText = "";
            this.metroTextBox_post_detail_expire.SelectionLength = 0;
            this.metroTextBox_post_detail_expire.SelectionStart = 0;
            this.metroTextBox_post_detail_expire.ShortcutsEnabled = true;
            this.metroTextBox_post_detail_expire.Size = new System.Drawing.Size(141, 23);
            this.metroTextBox_post_detail_expire.TabIndex = 17;
            this.metroTextBox_post_detail_expire.Text = "metroTextBox2";
            this.metroTextBox_post_detail_expire.UseSelectable = true;
            this.metroTextBox_post_detail_expire.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_post_detail_expire.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTextBox_post_detail_start
            // 
            // 
            // 
            // 
            this.metroTextBox_post_detail_start.CustomButton.Image = null;
            this.metroTextBox_post_detail_start.CustomButton.Location = new System.Drawing.Point(119, 1);
            this.metroTextBox_post_detail_start.CustomButton.Name = "";
            this.metroTextBox_post_detail_start.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_post_detail_start.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_post_detail_start.CustomButton.TabIndex = 1;
            this.metroTextBox_post_detail_start.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_post_detail_start.CustomButton.UseSelectable = true;
            this.metroTextBox_post_detail_start.CustomButton.Visible = false;
            this.metroTextBox_post_detail_start.Lines = new string[] {
        "metroTextBox1"};
            this.metroTextBox_post_detail_start.Location = new System.Drawing.Point(54, 252);
            this.metroTextBox_post_detail_start.MaxLength = 32767;
            this.metroTextBox_post_detail_start.Name = "metroTextBox_post_detail_start";
            this.metroTextBox_post_detail_start.PasswordChar = '\0';
            this.metroTextBox_post_detail_start.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_post_detail_start.SelectedText = "";
            this.metroTextBox_post_detail_start.SelectionLength = 0;
            this.metroTextBox_post_detail_start.SelectionStart = 0;
            this.metroTextBox_post_detail_start.ShortcutsEnabled = true;
            this.metroTextBox_post_detail_start.Size = new System.Drawing.Size(141, 23);
            this.metroTextBox_post_detail_start.TabIndex = 16;
            this.metroTextBox_post_detail_start.Text = "metroTextBox1";
            this.metroTextBox_post_detail_start.UseSelectable = true;
            this.metroTextBox_post_detail_start.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_post_detail_start.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(-1, 281);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(45, 19);
            this.metroLabel6.TabIndex = 15;
            this.metroLabel6.Text = "Expire";
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(-1, 256);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(37, 19);
            this.metroLabel5.TabIndex = 14;
            this.metroLabel5.Text = "Start";
            // 
            // metroPanel3
            // 
            this.metroPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroPanel3.Controls.Add(this.customGridView_detail_items);
            this.metroPanel3.Controls.Add(this.customGridView_detail_coins);
            this.metroPanel3.Controls.Add(this.metroLabel_post_coins);
            this.metroPanel3.Controls.Add(this.metroLabel_post_items);
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(201, 3);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(188, 309);
            this.metroPanel3.TabIndex = 12;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // customGridView_detail_items
            // 
            this.customGridView_detail_items.AllowUserToAddRows = false;
            this.customGridView_detail_items.AllowUserToDeleteRows = false;
            this.customGridView_detail_items.AllowUserToResizeRows = false;
            this.customGridView_detail_items.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_detail_items.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customGridView_detail_items.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customGridView_detail_items.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_detail_items.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.customGridView_detail_items.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customGridView_detail_items.DefaultCellStyle = dataGridViewCellStyle5;
            this.customGridView_detail_items.EnableHeadersVisualStyles = false;
            this.customGridView_detail_items.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.customGridView_detail_items.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_detail_items.Location = new System.Drawing.Point(3, 186);
            this.customGridView_detail_items.Name = "customGridView_detail_items";
            this.customGridView_detail_items.ReadOnly = true;
            this.customGridView_detail_items.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_detail_items.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.customGridView_detail_items.RowHeadersVisible = false;
            this.customGridView_detail_items.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.customGridView_detail_items.RowTemplate.Height = 23;
            this.customGridView_detail_items.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customGridView_detail_items.Size = new System.Drawing.Size(180, 98);
            this.customGridView_detail_items.TabIndex = 7;
            // 
            // customGridView_detail_coins
            // 
            this.customGridView_detail_coins.AllowUserToAddRows = false;
            this.customGridView_detail_coins.AllowUserToDeleteRows = false;
            this.customGridView_detail_coins.AllowUserToResizeRows = false;
            this.customGridView_detail_coins.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_detail_coins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customGridView_detail_coins.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customGridView_detail_coins.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_detail_coins.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.customGridView_detail_coins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customGridView_detail_coins.DefaultCellStyle = dataGridViewCellStyle8;
            this.customGridView_detail_coins.EnableHeadersVisualStyles = false;
            this.customGridView_detail_coins.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.customGridView_detail_coins.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_detail_coins.Location = new System.Drawing.Point(3, 27);
            this.customGridView_detail_coins.Name = "customGridView_detail_coins";
            this.customGridView_detail_coins.ReadOnly = true;
            this.customGridView_detail_coins.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_detail_coins.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.customGridView_detail_coins.RowHeadersVisible = false;
            this.customGridView_detail_coins.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.customGridView_detail_coins.RowTemplate.Height = 23;
            this.customGridView_detail_coins.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customGridView_detail_coins.Size = new System.Drawing.Size(180, 98);
            this.customGridView_detail_coins.TabIndex = 6;
            // 
            // metroLabel_post_coins
            // 
            this.metroLabel_post_coins.AutoSize = true;
            this.metroLabel_post_coins.Location = new System.Drawing.Point(3, 0);
            this.metroLabel_post_coins.Name = "metroLabel_post_coins";
            this.metroLabel_post_coins.Size = new System.Drawing.Size(41, 19);
            this.metroLabel_post_coins.TabIndex = 3;
            this.metroLabel_post_coins.Text = "Coins";
            // 
            // metroLabel_post_items
            // 
            this.metroLabel_post_items.AutoSize = true;
            this.metroLabel_post_items.Location = new System.Drawing.Point(1, 138);
            this.metroLabel_post_items.Name = "metroLabel_post_items";
            this.metroLabel_post_items.Size = new System.Drawing.Size(40, 19);
            this.metroLabel_post_items.TabIndex = 5;
            this.metroLabel_post_items.Text = "Items";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(3, 0);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(48, 19);
            this.metroLabel1.TabIndex = 6;
            this.metroLabel1.Text = "Detail";
            // 
            // sLabel_system_post_start
            // 
            this.sLabel_system_post_start.AutoSize = true;
            this.sLabel_system_post_start.Location = new System.Drawing.Point(77, 73);
            this.sLabel_system_post_start.Name = "sLabel_system_post_start";
            this.sLabel_system_post_start.Size = new System.Drawing.Size(37, 19);
            this.sLabel_system_post_start.TabIndex = 11;
            this.sLabel_system_post_start.Text = "Start";
            // 
            // sLabel_system_post_end
            // 
            this.sLabel_system_post_end.AutoSize = true;
            this.sLabel_system_post_end.Location = new System.Drawing.Point(268, 73);
            this.sLabel_system_post_end.Name = "sLabel_system_post_end";
            this.sLabel_system_post_end.Size = new System.Drawing.Size(31, 19);
            this.sLabel_system_post_end.TabIndex = 12;
            this.sLabel_system_post_end.Text = "End";
            // 
            // sDateTimePicker_system_post_start
            // 
            this.sDateTimePicker_system_post_start.CustomFormat = "yyyy-MM-dd HH:mm";
            this.sDateTimePicker_system_post_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.sDateTimePicker_system_post_start.Location = new System.Drawing.Point(77, 95);
            this.sDateTimePicker_system_post_start.Name = "sDateTimePicker_system_post_start";
            this.sDateTimePicker_system_post_start.Size = new System.Drawing.Size(163, 21);
            this.sDateTimePicker_system_post_start.TabIndex = 13;
            // 
            // sDateTimePicker_system_post_end
            // 
            this.sDateTimePicker_system_post_end.CustomFormat = "yyyy-MM-dd HH:mm";
            this.sDateTimePicker_system_post_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.sDateTimePicker_system_post_end.Location = new System.Drawing.Point(268, 96);
            this.sDateTimePicker_system_post_end.Name = "sDateTimePicker_system_post_end";
            this.sDateTimePicker_system_post_end.Size = new System.Drawing.Size(156, 21);
            this.sDateTimePicker_system_post_end.TabIndex = 14;
            // 
            // sButton_system_post_search
            // 
            this.sButton_system_post_search.Location = new System.Drawing.Point(458, 93);
            this.sButton_system_post_search.Name = "sButton_system_post_search";
            this.sButton_system_post_search.Size = new System.Drawing.Size(75, 23);
            this.sButton_system_post_search.TabIndex = 15;
            this.sButton_system_post_search.Text = "Search";
            this.sButton_system_post_search.UseSelectable = true;
            this.sButton_system_post_search.Click += new System.EventHandler(this.onClick_system_post_search);
            // 
            // sLabel_detail_post_id
            // 
            this.sLabel_detail_post_id.AutoSize = true;
            this.sLabel_detail_post_id.Location = new System.Drawing.Point(3, 19);
            this.sLabel_detail_post_id.Name = "sLabel_detail_post_id";
            this.sLabel_detail_post_id.Size = new System.Drawing.Size(39, 19);
            this.sLabel_detail_post_id.TabIndex = 18;
            this.sLabel_detail_post_id.Text = "GUID";
            // 
            // sTextBox_detail_system_post_id
            // 
            // 
            // 
            // 
            this.sTextBox_detail_system_post_id.CustomButton.Image = null;
            this.sTextBox_detail_system_post_id.CustomButton.Location = new System.Drawing.Point(169, 1);
            this.sTextBox_detail_system_post_id.CustomButton.Name = "";
            this.sTextBox_detail_system_post_id.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.sTextBox_detail_system_post_id.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.sTextBox_detail_system_post_id.CustomButton.TabIndex = 1;
            this.sTextBox_detail_system_post_id.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.sTextBox_detail_system_post_id.CustomButton.UseSelectable = true;
            this.sTextBox_detail_system_post_id.CustomButton.Visible = false;
            this.sTextBox_detail_system_post_id.Lines = new string[0];
            this.sTextBox_detail_system_post_id.Location = new System.Drawing.Point(4, 42);
            this.sTextBox_detail_system_post_id.MaxLength = 32767;
            this.sTextBox_detail_system_post_id.Name = "sTextBox_detail_system_post_id";
            this.sTextBox_detail_system_post_id.PasswordChar = '\0';
            this.sTextBox_detail_system_post_id.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_detail_system_post_id.SelectedText = "";
            this.sTextBox_detail_system_post_id.SelectionLength = 0;
            this.sTextBox_detail_system_post_id.SelectionStart = 0;
            this.sTextBox_detail_system_post_id.ShortcutsEnabled = true;
            this.sTextBox_detail_system_post_id.Size = new System.Drawing.Size(191, 23);
            this.sTextBox_detail_system_post_id.TabIndex = 19;
            this.sTextBox_detail_system_post_id.UseSelectable = true;
            this.sTextBox_detail_system_post_id.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_detail_system_post_id.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // sTextBox_detail_system_post_title
            // 
            // 
            // 
            // 
            this.sTextBox_detail_system_post_title.CustomButton.Image = null;
            this.sTextBox_detail_system_post_title.CustomButton.Location = new System.Drawing.Point(169, 1);
            this.sTextBox_detail_system_post_title.CustomButton.Name = "";
            this.sTextBox_detail_system_post_title.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.sTextBox_detail_system_post_title.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.sTextBox_detail_system_post_title.CustomButton.TabIndex = 1;
            this.sTextBox_detail_system_post_title.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.sTextBox_detail_system_post_title.CustomButton.UseSelectable = true;
            this.sTextBox_detail_system_post_title.CustomButton.Visible = false;
            this.sTextBox_detail_system_post_title.Lines = new string[0];
            this.sTextBox_detail_system_post_title.Location = new System.Drawing.Point(3, 90);
            this.sTextBox_detail_system_post_title.MaxLength = 32767;
            this.sTextBox_detail_system_post_title.Name = "sTextBox_detail_system_post_title";
            this.sTextBox_detail_system_post_title.PasswordChar = '\0';
            this.sTextBox_detail_system_post_title.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_detail_system_post_title.SelectedText = "";
            this.sTextBox_detail_system_post_title.SelectionLength = 0;
            this.sTextBox_detail_system_post_title.SelectionStart = 0;
            this.sTextBox_detail_system_post_title.ShortcutsEnabled = true;
            this.sTextBox_detail_system_post_title.Size = new System.Drawing.Size(191, 23);
            this.sTextBox_detail_system_post_title.TabIndex = 20;
            this.sTextBox_detail_system_post_title.UseSelectable = true;
            this.sTextBox_detail_system_post_title.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_detail_system_post_title.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // sLabel_detail_system_post_title
            // 
            this.sLabel_detail_system_post_title.AutoSize = true;
            this.sLabel_detail_system_post_title.Location = new System.Drawing.Point(4, 68);
            this.sLabel_detail_system_post_title.Name = "sLabel_detail_system_post_title";
            this.sLabel_detail_system_post_title.Size = new System.Drawing.Size(33, 19);
            this.sLabel_detail_system_post_title.TabIndex = 21;
            this.sLabel_detail_system_post_title.Text = "Title";
            // 
            // sLabel_detail_system_post_msg
            // 
            this.sLabel_detail_system_post_msg.AutoSize = true;
            this.sLabel_detail_system_post_msg.Location = new System.Drawing.Point(4, 128);
            this.sLabel_detail_system_post_msg.Name = "sLabel_detail_system_post_msg";
            this.sLabel_detail_system_post_msg.Size = new System.Drawing.Size(34, 19);
            this.sLabel_detail_system_post_msg.TabIndex = 22;
            this.sLabel_detail_system_post_msg.Text = "Msg";
            // 
            // sTextBox_detail_system_post_msg
            // 
            // 
            // 
            // 
            this.sTextBox_detail_system_post_msg.CustomButton.Image = null;
            this.sTextBox_detail_system_post_msg.CustomButton.Location = new System.Drawing.Point(97, 2);
            this.sTextBox_detail_system_post_msg.CustomButton.Name = "";
            this.sTextBox_detail_system_post_msg.CustomButton.Size = new System.Drawing.Size(91, 91);
            this.sTextBox_detail_system_post_msg.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.sTextBox_detail_system_post_msg.CustomButton.TabIndex = 1;
            this.sTextBox_detail_system_post_msg.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.sTextBox_detail_system_post_msg.CustomButton.UseSelectable = true;
            this.sTextBox_detail_system_post_msg.CustomButton.Visible = false;
            this.sTextBox_detail_system_post_msg.Lines = new string[0];
            this.sTextBox_detail_system_post_msg.Location = new System.Drawing.Point(4, 150);
            this.sTextBox_detail_system_post_msg.MaxLength = 32767;
            this.sTextBox_detail_system_post_msg.Multiline = true;
            this.sTextBox_detail_system_post_msg.Name = "sTextBox_detail_system_post_msg";
            this.sTextBox_detail_system_post_msg.PasswordChar = '\0';
            this.sTextBox_detail_system_post_msg.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_detail_system_post_msg.SelectedText = "";
            this.sTextBox_detail_system_post_msg.SelectionLength = 0;
            this.sTextBox_detail_system_post_msg.SelectionStart = 0;
            this.sTextBox_detail_system_post_msg.ShortcutsEnabled = true;
            this.sTextBox_detail_system_post_msg.Size = new System.Drawing.Size(191, 96);
            this.sTextBox_detail_system_post_msg.TabIndex = 23;
            this.sTextBox_detail_system_post_msg.UseSelectable = true;
            this.sTextBox_detail_system_post_msg.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_detail_system_post_msg.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // Panel_SystemPost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Panel_SystemPost";
            this.Panel_Base.ResumeLayout(false);
            this.Panel_Base.PerformLayout();
            this.metroPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_post)).EndInit();
            this.metroPanel2.ResumeLayout(false);
            this.metroPanel2.PerformLayout();
            this.metroPanel3.ResumeLayout(false);
            this.metroPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_detail_items)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_detail_coins)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroLabel metroLabel_post_items;
        private MetroFramework.Controls.MetroLabel metroLabel_post_coins;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private MetroFramework.Controls.MetroTextBox metroTextBox_post_detail_expire;
        private MetroFramework.Controls.MetroTextBox metroTextBox_post_detail_start;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private Controls.SGridView customGridView_post;
        private Controls.SGridView customGridView_detail_items;
        private Controls.SGridView customGridView_detail_coins;
        private Controls.SLabel sLabel_system_post_start;
        private Controls.SLabel sLabel_system_post_end;
        private Controls.SDateTimePicker sDateTimePicker_system_post_start;
        private Controls.SDateTimePicker sDateTimePicker_system_post_end;
        private Controls.SButton sButton_system_post_search;
        private Controls.SLabel sLabel_detail_post_id;
        private Controls.STextBox sTextBox_detail_system_post_id;
        private Controls.STextBox sTextBox_detail_system_post_title;
        private Controls.SLabel sLabel_detail_system_post_title;
        private Controls.SLabel sLabel_detail_system_post_msg;
        private Controls.STextBox sTextBox_detail_system_post_msg;
    }
}
