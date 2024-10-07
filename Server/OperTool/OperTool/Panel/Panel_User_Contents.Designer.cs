namespace OperTool.Panel
{
    partial class Panel_User_Contents
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contents_tab = new MetroFramework.Controls.MetroTabControl();
            this.tab_Skill = new MetroFramework.Controls.MetroTabPage();
            this.tab_GrowthGold = new MetroFramework.Controls.MetroTabPage();
            this.customGridView_user_growthgold = new OperTool.Controls.SGridView();
            this.tab_GrowthLevel = new MetroFramework.Controls.MetroTabPage();
            this.customGridView_user_growth_level = new OperTool.Controls.SGridView();
            this.tab_Quest = new MetroFramework.Controls.MetroTabPage();
            this.tab_Pass = new MetroFramework.Controls.MetroTabPage();
            this.tab_Shop = new MetroFramework.Controls.MetroTabPage();
            this.tab_Relic = new MetroFramework.Controls.MetroTabPage();
            this.tab_Gacha = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage9 = new MetroFramework.Controls.MetroTabPage();
            this.sTextBox_user_name = new OperTool.Controls.STextBox();
            this.sTextBox_user_deviceid = new OperTool.Controls.STextBox();
            this.sTextBox_user_uid = new OperTool.Controls.STextBox();
            this.sLabel_user_name = new OperTool.Controls.SLabel();
            this.sLabel_user_deviceID = new OperTool.Controls.SLabel();
            this.sLabel_user_uid = new OperTool.Controls.SLabel();
            this.Panel_Base.SuspendLayout();
            this.contents_tab.SuspendLayout();
            this.tab_GrowthGold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_user_growthgold)).BeginInit();
            this.tab_GrowthLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_user_growth_level)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel_Base
            // 
            this.Panel_Base.Controls.Add(this.sTextBox_user_name);
            this.Panel_Base.Controls.Add(this.sTextBox_user_deviceid);
            this.Panel_Base.Controls.Add(this.sTextBox_user_uid);
            this.Panel_Base.Controls.Add(this.sLabel_user_name);
            this.Panel_Base.Controls.Add(this.sLabel_user_deviceID);
            this.Panel_Base.Controls.Add(this.sLabel_user_uid);
            this.Panel_Base.Controls.Add(this.contents_tab);
            this.Panel_Base.VisibleChanged += new System.EventHandler(this.OnVisible);
            this.Panel_Base.Controls.SetChildIndex(this.contents_tab, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sLabel_user_uid, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sLabel_user_deviceID, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sLabel_user_name, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sTextBox_user_uid, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sTextBox_user_deviceid, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sTextBox_user_name, 0);
            this.Panel_Base.Controls.SetChildIndex(this.Panel_Label, 0);
            // 
            // contents_tab
            // 
            this.contents_tab.Controls.Add(this.tab_Skill);
            this.contents_tab.Controls.Add(this.tab_GrowthGold);
            this.contents_tab.Controls.Add(this.tab_GrowthLevel);
            this.contents_tab.Controls.Add(this.tab_Quest);
            this.contents_tab.Controls.Add(this.tab_Pass);
            this.contents_tab.Controls.Add(this.tab_Shop);
            this.contents_tab.Controls.Add(this.tab_Relic);
            this.contents_tab.Controls.Add(this.tab_Gacha);
            this.contents_tab.Controls.Add(this.metroTabPage9);
            this.contents_tab.Location = new System.Drawing.Point(33, 115);
            this.contents_tab.Name = "contents_tab";
            this.contents_tab.SelectedIndex = 0;
            this.contents_tab.Size = new System.Drawing.Size(978, 404);
            this.contents_tab.TabIndex = 15;
            this.contents_tab.UseSelectable = true;
            this.contents_tab.SelectedIndexChanged += new System.EventHandler(this.contents_tab_SelectedIndexChanged);
            // 
            // tab_Skill
            // 
            this.tab_Skill.HorizontalScrollbarBarColor = true;
            this.tab_Skill.HorizontalScrollbarHighlightOnWheel = false;
            this.tab_Skill.HorizontalScrollbarSize = 10;
            this.tab_Skill.Location = new System.Drawing.Point(4, 38);
            this.tab_Skill.Name = "tab_Skill";
            this.tab_Skill.Size = new System.Drawing.Size(970, 362);
            this.tab_Skill.TabIndex = 1;
            this.tab_Skill.Text = "Skill";
            this.tab_Skill.VerticalScrollbarBarColor = true;
            this.tab_Skill.VerticalScrollbarHighlightOnWheel = false;
            this.tab_Skill.VerticalScrollbarSize = 10;
            // 
            // tab_GrowthGold
            // 
            this.tab_GrowthGold.Controls.Add(this.customGridView_user_growthgold);
            this.tab_GrowthGold.HorizontalScrollbarBarColor = true;
            this.tab_GrowthGold.HorizontalScrollbarHighlightOnWheel = false;
            this.tab_GrowthGold.HorizontalScrollbarSize = 10;
            this.tab_GrowthGold.Location = new System.Drawing.Point(4, 38);
            this.tab_GrowthGold.Name = "tab_GrowthGold";
            this.tab_GrowthGold.Size = new System.Drawing.Size(970, 385);
            this.tab_GrowthGold.TabIndex = 0;
            this.tab_GrowthGold.Text = "GrowthGold";
            this.tab_GrowthGold.VerticalScrollbarBarColor = true;
            this.tab_GrowthGold.VerticalScrollbarHighlightOnWheel = false;
            this.tab_GrowthGold.VerticalScrollbarSize = 10;
            // 
            // customGridView_user_growthgold
            // 
            this.customGridView_user_growthgold.AllowUserToAddRows = false;
            this.customGridView_user_growthgold.AllowUserToDeleteRows = false;
            this.customGridView_user_growthgold.AllowUserToResizeRows = false;
            this.customGridView_user_growthgold.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_user_growthgold.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customGridView_user_growthgold.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customGridView_user_growthgold.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_user_growthgold.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.customGridView_user_growthgold.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customGridView_user_growthgold.DefaultCellStyle = dataGridViewCellStyle20;
            this.customGridView_user_growthgold.EnableHeadersVisualStyles = false;
            this.customGridView_user_growthgold.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.customGridView_user_growthgold.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_user_growthgold.Location = new System.Drawing.Point(32, 19);
            this.customGridView_user_growthgold.Name = "customGridView_user_growthgold";
            this.customGridView_user_growthgold.ReadOnly = true;
            this.customGridView_user_growthgold.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_user_growthgold.RowHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.customGridView_user_growthgold.RowHeadersVisible = false;
            this.customGridView_user_growthgold.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.customGridView_user_growthgold.RowTemplate.Height = 23;
            this.customGridView_user_growthgold.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customGridView_user_growthgold.Size = new System.Drawing.Size(268, 351);
            this.customGridView_user_growthgold.TabIndex = 2;
            // 
            // tab_GrowthLevel
            // 
            this.tab_GrowthLevel.Controls.Add(this.customGridView_user_growth_level);
            this.tab_GrowthLevel.HorizontalScrollbarBarColor = true;
            this.tab_GrowthLevel.HorizontalScrollbarHighlightOnWheel = false;
            this.tab_GrowthLevel.HorizontalScrollbarSize = 10;
            this.tab_GrowthLevel.Location = new System.Drawing.Point(4, 38);
            this.tab_GrowthLevel.Name = "tab_GrowthLevel";
            this.tab_GrowthLevel.Size = new System.Drawing.Size(970, 385);
            this.tab_GrowthLevel.TabIndex = 4;
            this.tab_GrowthLevel.Text = "GrowthLevel";
            this.tab_GrowthLevel.VerticalScrollbarBarColor = true;
            this.tab_GrowthLevel.VerticalScrollbarHighlightOnWheel = false;
            this.tab_GrowthLevel.VerticalScrollbarSize = 10;
            // 
            // customGridView_user_growth_level
            // 
            this.customGridView_user_growth_level.AllowUserToAddRows = false;
            this.customGridView_user_growth_level.AllowUserToDeleteRows = false;
            this.customGridView_user_growth_level.AllowUserToResizeRows = false;
            this.customGridView_user_growth_level.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_user_growth_level.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.customGridView_user_growth_level.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.customGridView_user_growth_level.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_user_growth_level.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.customGridView_user_growth_level.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customGridView_user_growth_level.DefaultCellStyle = dataGridViewCellStyle23;
            this.customGridView_user_growth_level.EnableHeadersVisualStyles = false;
            this.customGridView_user_growth_level.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.customGridView_user_growth_level.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customGridView_user_growth_level.Location = new System.Drawing.Point(32, 20);
            this.customGridView_user_growth_level.Name = "customGridView_user_growth_level";
            this.customGridView_user_growth_level.ReadOnly = true;
            this.customGridView_user_growth_level.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customGridView_user_growth_level.RowHeadersDefaultCellStyle = dataGridViewCellStyle24;
            this.customGridView_user_growth_level.RowHeadersVisible = false;
            this.customGridView_user_growth_level.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.customGridView_user_growth_level.RowTemplate.Height = 23;
            this.customGridView_user_growth_level.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customGridView_user_growth_level.Size = new System.Drawing.Size(268, 351);
            this.customGridView_user_growth_level.TabIndex = 3;
            // 
            // tab_Quest
            // 
            this.tab_Quest.HorizontalScrollbarBarColor = true;
            this.tab_Quest.HorizontalScrollbarHighlightOnWheel = false;
            this.tab_Quest.HorizontalScrollbarSize = 10;
            this.tab_Quest.Location = new System.Drawing.Point(4, 38);
            this.tab_Quest.Name = "tab_Quest";
            this.tab_Quest.Size = new System.Drawing.Size(970, 385);
            this.tab_Quest.TabIndex = 2;
            this.tab_Quest.Text = "Quest";
            this.tab_Quest.VerticalScrollbarBarColor = true;
            this.tab_Quest.VerticalScrollbarHighlightOnWheel = false;
            this.tab_Quest.VerticalScrollbarSize = 10;
            // 
            // tab_Pass
            // 
            this.tab_Pass.HorizontalScrollbarBarColor = true;
            this.tab_Pass.HorizontalScrollbarHighlightOnWheel = false;
            this.tab_Pass.HorizontalScrollbarSize = 10;
            this.tab_Pass.Location = new System.Drawing.Point(4, 38);
            this.tab_Pass.Name = "tab_Pass";
            this.tab_Pass.Size = new System.Drawing.Size(970, 385);
            this.tab_Pass.TabIndex = 3;
            this.tab_Pass.Text = "Pass";
            this.tab_Pass.VerticalScrollbarBarColor = true;
            this.tab_Pass.VerticalScrollbarHighlightOnWheel = false;
            this.tab_Pass.VerticalScrollbarSize = 10;
            // 
            // tab_Shop
            // 
            this.tab_Shop.HorizontalScrollbarBarColor = true;
            this.tab_Shop.HorizontalScrollbarHighlightOnWheel = false;
            this.tab_Shop.HorizontalScrollbarSize = 10;
            this.tab_Shop.Location = new System.Drawing.Point(4, 38);
            this.tab_Shop.Name = "tab_Shop";
            this.tab_Shop.Size = new System.Drawing.Size(970, 385);
            this.tab_Shop.TabIndex = 5;
            this.tab_Shop.Text = "Shop";
            this.tab_Shop.VerticalScrollbarBarColor = true;
            this.tab_Shop.VerticalScrollbarHighlightOnWheel = false;
            this.tab_Shop.VerticalScrollbarSize = 10;
            // 
            // tab_Relic
            // 
            this.tab_Relic.HorizontalScrollbarBarColor = true;
            this.tab_Relic.HorizontalScrollbarHighlightOnWheel = false;
            this.tab_Relic.HorizontalScrollbarSize = 10;
            this.tab_Relic.Location = new System.Drawing.Point(4, 38);
            this.tab_Relic.Name = "tab_Relic";
            this.tab_Relic.Size = new System.Drawing.Size(970, 385);
            this.tab_Relic.TabIndex = 6;
            this.tab_Relic.Text = "Relic";
            this.tab_Relic.VerticalScrollbarBarColor = true;
            this.tab_Relic.VerticalScrollbarHighlightOnWheel = false;
            this.tab_Relic.VerticalScrollbarSize = 10;
            // 
            // tab_Gacha
            // 
            this.tab_Gacha.HorizontalScrollbarBarColor = true;
            this.tab_Gacha.HorizontalScrollbarHighlightOnWheel = false;
            this.tab_Gacha.HorizontalScrollbarSize = 10;
            this.tab_Gacha.Location = new System.Drawing.Point(4, 38);
            this.tab_Gacha.Name = "tab_Gacha";
            this.tab_Gacha.Size = new System.Drawing.Size(970, 385);
            this.tab_Gacha.TabIndex = 7;
            this.tab_Gacha.Text = "Gacha";
            this.tab_Gacha.VerticalScrollbarBarColor = true;
            this.tab_Gacha.VerticalScrollbarHighlightOnWheel = false;
            this.tab_Gacha.VerticalScrollbarSize = 10;
            // 
            // metroTabPage9
            // 
            this.metroTabPage9.HorizontalScrollbarBarColor = true;
            this.metroTabPage9.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage9.HorizontalScrollbarSize = 10;
            this.metroTabPage9.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage9.Name = "metroTabPage9";
            this.metroTabPage9.Size = new System.Drawing.Size(970, 385);
            this.metroTabPage9.TabIndex = 8;
            this.metroTabPage9.Text = "metroTabPage9";
            this.metroTabPage9.VerticalScrollbarBarColor = true;
            this.metroTabPage9.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage9.VerticalScrollbarSize = 10;
            // 
            // sTextBox_user_name
            // 
            // 
            // 
            // 
            this.sTextBox_user_name.CustomButton.Image = null;
            this.sTextBox_user_name.CustomButton.Location = new System.Drawing.Point(148, 1);
            this.sTextBox_user_name.CustomButton.Name = "";
            this.sTextBox_user_name.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.sTextBox_user_name.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.sTextBox_user_name.CustomButton.TabIndex = 1;
            this.sTextBox_user_name.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.sTextBox_user_name.CustomButton.UseSelectable = true;
            this.sTextBox_user_name.CustomButton.Visible = false;
            this.sTextBox_user_name.Lines = new string[0];
            this.sTextBox_user_name.Location = new System.Drawing.Point(541, 86);
            this.sTextBox_user_name.MaxLength = 32767;
            this.sTextBox_user_name.Name = "sTextBox_user_name";
            this.sTextBox_user_name.PasswordChar = '\0';
            this.sTextBox_user_name.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_user_name.SelectedText = "";
            this.sTextBox_user_name.SelectionLength = 0;
            this.sTextBox_user_name.SelectionStart = 0;
            this.sTextBox_user_name.ShortcutsEnabled = true;
            this.sTextBox_user_name.Size = new System.Drawing.Size(170, 23);
            this.sTextBox_user_name.TabIndex = 26;
            this.sTextBox_user_name.UseSelectable = true;
            this.sTextBox_user_name.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_user_name.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // sTextBox_user_deviceid
            // 
            // 
            // 
            // 
            this.sTextBox_user_deviceid.CustomButton.Image = null;
            this.sTextBox_user_deviceid.CustomButton.Location = new System.Drawing.Point(183, 1);
            this.sTextBox_user_deviceid.CustomButton.Name = "";
            this.sTextBox_user_deviceid.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.sTextBox_user_deviceid.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.sTextBox_user_deviceid.CustomButton.TabIndex = 1;
            this.sTextBox_user_deviceid.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.sTextBox_user_deviceid.CustomButton.UseSelectable = true;
            this.sTextBox_user_deviceid.CustomButton.Visible = false;
            this.sTextBox_user_deviceid.Lines = new string[0];
            this.sTextBox_user_deviceid.Location = new System.Drawing.Point(262, 86);
            this.sTextBox_user_deviceid.MaxLength = 32767;
            this.sTextBox_user_deviceid.Name = "sTextBox_user_deviceid";
            this.sTextBox_user_deviceid.PasswordChar = '\0';
            this.sTextBox_user_deviceid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_user_deviceid.SelectedText = "";
            this.sTextBox_user_deviceid.SelectionLength = 0;
            this.sTextBox_user_deviceid.SelectionStart = 0;
            this.sTextBox_user_deviceid.ShortcutsEnabled = true;
            this.sTextBox_user_deviceid.Size = new System.Drawing.Size(205, 23);
            this.sTextBox_user_deviceid.TabIndex = 25;
            this.sTextBox_user_deviceid.UseSelectable = true;
            this.sTextBox_user_deviceid.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_user_deviceid.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // sTextBox_user_uid
            // 
            // 
            // 
            // 
            this.sTextBox_user_uid.CustomButton.Image = null;
            this.sTextBox_user_uid.CustomButton.Location = new System.Drawing.Point(174, 1);
            this.sTextBox_user_uid.CustomButton.Name = "";
            this.sTextBox_user_uid.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.sTextBox_user_uid.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.sTextBox_user_uid.CustomButton.TabIndex = 1;
            this.sTextBox_user_uid.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.sTextBox_user_uid.CustomButton.UseSelectable = true;
            this.sTextBox_user_uid.CustomButton.Visible = false;
            this.sTextBox_user_uid.Lines = new string[0];
            this.sTextBox_user_uid.Location = new System.Drawing.Point(35, 86);
            this.sTextBox_user_uid.MaxLength = 32767;
            this.sTextBox_user_uid.Name = "sTextBox_user_uid";
            this.sTextBox_user_uid.PasswordChar = '\0';
            this.sTextBox_user_uid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_user_uid.SelectedText = "";
            this.sTextBox_user_uid.SelectionLength = 0;
            this.sTextBox_user_uid.SelectionStart = 0;
            this.sTextBox_user_uid.ShortcutsEnabled = true;
            this.sTextBox_user_uid.Size = new System.Drawing.Size(196, 23);
            this.sTextBox_user_uid.TabIndex = 24;
            this.sTextBox_user_uid.UseSelectable = true;
            this.sTextBox_user_uid.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_user_uid.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // sLabel_user_name
            // 
            this.sLabel_user_name.AutoSize = true;
            this.sLabel_user_name.Location = new System.Drawing.Point(541, 64);
            this.sLabel_user_name.Name = "sLabel_user_name";
            this.sLabel_user_name.Size = new System.Drawing.Size(57, 19);
            this.sLabel_user_name.TabIndex = 23;
            this.sLabel_user_name.Text = "!!!Name";
            // 
            // sLabel_user_deviceID
            // 
            this.sLabel_user_deviceID.AutoSize = true;
            this.sLabel_user_deviceID.Location = new System.Drawing.Point(262, 64);
            this.sLabel_user_deviceID.Name = "sLabel_user_deviceID";
            this.sLabel_user_deviceID.Size = new System.Drawing.Size(71, 19);
            this.sLabel_user_deviceID.TabIndex = 22;
            this.sLabel_user_deviceID.Text = "!!!DeviceID";
            // 
            // sLabel_user_uid
            // 
            this.sLabel_user_uid.AutoSize = true;
            this.sLabel_user_uid.Location = new System.Drawing.Point(35, 61);
            this.sLabel_user_uid.Name = "sLabel_user_uid";
            this.sLabel_user_uid.Size = new System.Drawing.Size(42, 19);
            this.sLabel_user_uid.TabIndex = 21;
            this.sLabel_user_uid.Text = "!!!UID";
            // 
            // Panel_User_Contents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Panel_User_Contents";
            this.Panel_Base.ResumeLayout(false);
            this.Panel_Base.PerformLayout();
            this.contents_tab.ResumeLayout(false);
            this.tab_GrowthGold.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_user_growthgold)).EndInit();
            this.tab_GrowthLevel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customGridView_user_growth_level)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroTabControl contents_tab;
        private MetroFramework.Controls.MetroTabPage tab_GrowthGold;
        private MetroFramework.Controls.MetroTabPage tab_Skill;
        private MetroFramework.Controls.MetroTabPage tab_Quest;
        private MetroFramework.Controls.MetroTabPage tab_Pass;
        private MetroFramework.Controls.MetroTabPage tab_GrowthLevel;
        private MetroFramework.Controls.MetroTabPage tab_Shop;
        private MetroFramework.Controls.MetroTabPage tab_Relic;
        private MetroFramework.Controls.MetroTabPage tab_Gacha;
        private MetroFramework.Controls.MetroTabPage metroTabPage9;
        private Controls.SGridView customGridView_user_growthgold;
        private Controls.SGridView customGridView_user_growth_level;
        private Controls.STextBox sTextBox_user_name;
        private Controls.STextBox sTextBox_user_deviceid;
        private Controls.STextBox sTextBox_user_uid;
        private Controls.SLabel sLabel_user_name;
        private Controls.SLabel sLabel_user_deviceID;
        private Controls.SLabel sLabel_user_uid;
    }
}
