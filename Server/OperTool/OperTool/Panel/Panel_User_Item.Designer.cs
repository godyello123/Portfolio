namespace OperTool.Panel
{
    partial class Panel_User_Item
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
            this.metroTabControl_item = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.metroGrid_item_weapon = new MetroFramework.Controls.MetroGrid();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage3 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage4 = new MetroFramework.Controls.MetroTabPage();
            this.sLabel_user_uid = new OperTool.Controls.SLabel();
            this.sLabel_user_deviceID = new OperTool.Controls.SLabel();
            this.sLabel_user_name = new OperTool.Controls.SLabel();
            this.sTextBox_user_uid = new OperTool.Controls.STextBox();
            this.sTextBox_user_deviceid = new OperTool.Controls.STextBox();
            this.sTextBox_user_name = new OperTool.Controls.STextBox();
            this.Panel_Base.SuspendLayout();
            this.metroTabControl_item.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroGrid_item_weapon)).BeginInit();
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
            this.Panel_Base.Controls.Add(this.metroTabControl_item);
            this.Panel_Base.Controls.SetChildIndex(this.Panel_Label, 0);
            this.Panel_Base.Controls.SetChildIndex(this.metroTabControl_item, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sLabel_user_uid, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sLabel_user_deviceID, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sLabel_user_name, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sTextBox_user_uid, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sTextBox_user_deviceid, 0);
            this.Panel_Base.Controls.SetChildIndex(this.sTextBox_user_name, 0);
            // 
            // metroTabControl_item
            // 
            this.metroTabControl_item.Controls.Add(this.metroTabPage1);
            this.metroTabControl_item.Controls.Add(this.metroTabPage2);
            this.metroTabControl_item.Controls.Add(this.metroTabPage3);
            this.metroTabControl_item.Controls.Add(this.metroTabPage4);
            this.metroTabControl_item.Location = new System.Drawing.Point(57, 111);
            this.metroTabControl_item.Name = "metroTabControl_item";
            this.metroTabControl_item.SelectedIndex = 0;
            this.metroTabControl_item.Size = new System.Drawing.Size(919, 385);
            this.metroTabControl_item.TabIndex = 3;
            this.metroTabControl_item.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.Controls.Add(this.metroGrid_item_weapon);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(911, 343);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "Weapon";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
            // 
            // metroGrid_item_weapon
            // 
            this.metroGrid_item_weapon.AllowUserToResizeRows = false;
            this.metroGrid_item_weapon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.metroGrid_item_weapon.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.metroGrid_item_weapon.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGrid_item_weapon.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metroGrid_item_weapon.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.metroGrid_item_weapon.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGrid_item_weapon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.metroGrid_item_weapon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metroGrid_item_weapon.DefaultCellStyle = dataGridViewCellStyle2;
            this.metroGrid_item_weapon.EnableHeadersVisualStyles = false;
            this.metroGrid_item_weapon.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroGrid_item_weapon.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGrid_item_weapon.Location = new System.Drawing.Point(29, 33);
            this.metroGrid_item_weapon.Name = "metroGrid_item_weapon";
            this.metroGrid_item_weapon.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGrid_item_weapon.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.metroGrid_item_weapon.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.metroGrid_item_weapon.RowTemplate.Height = 23;
            this.metroGrid_item_weapon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metroGrid_item_weapon.Size = new System.Drawing.Size(392, 324);
            this.metroGrid_item_weapon.TabIndex = 2;
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 10;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(911, 343);
            this.metroTabPage2.TabIndex = 1;
            this.metroTabPage2.Text = "Defence";
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 10;
            // 
            // metroTabPage3
            // 
            this.metroTabPage3.HorizontalScrollbarBarColor = true;
            this.metroTabPage3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage3.HorizontalScrollbarSize = 10;
            this.metroTabPage3.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage3.Name = "metroTabPage3";
            this.metroTabPage3.Size = new System.Drawing.Size(911, 343);
            this.metroTabPage3.TabIndex = 2;
            this.metroTabPage3.Text = "Knight";
            this.metroTabPage3.VerticalScrollbarBarColor = true;
            this.metroTabPage3.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage3.VerticalScrollbarSize = 10;
            // 
            // metroTabPage4
            // 
            this.metroTabPage4.HorizontalScrollbarBarColor = true;
            this.metroTabPage4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage4.HorizontalScrollbarSize = 10;
            this.metroTabPage4.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage4.Name = "metroTabPage4";
            this.metroTabPage4.Size = new System.Drawing.Size(911, 343);
            this.metroTabPage4.TabIndex = 3;
            this.metroTabPage4.Text = "Etc";
            this.metroTabPage4.VerticalScrollbarBarColor = true;
            this.metroTabPage4.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage4.VerticalScrollbarSize = 10;
            // 
            // sLabel_user_uid
            // 
            this.sLabel_user_uid.AutoSize = true;
            this.sLabel_user_uid.Location = new System.Drawing.Point(61, 57);
            this.sLabel_user_uid.Name = "sLabel_user_uid";
            this.sLabel_user_uid.Size = new System.Drawing.Size(34, 19);
            this.sLabel_user_uid.TabIndex = 15;
            this.sLabel_user_uid.Text = "!UID";
            // 
            // sLabel_user_deviceID
            // 
            this.sLabel_user_deviceID.AutoSize = true;
            this.sLabel_user_deviceID.Location = new System.Drawing.Point(288, 60);
            this.sLabel_user_deviceID.Name = "sLabel_user_deviceID";
            this.sLabel_user_deviceID.Size = new System.Drawing.Size(63, 19);
            this.sLabel_user_deviceID.TabIndex = 16;
            this.sLabel_user_deviceID.Text = "!DeviceID";
            // 
            // sLabel_user_name
            // 
            this.sLabel_user_name.AutoSize = true;
            this.sLabel_user_name.Location = new System.Drawing.Point(567, 60);
            this.sLabel_user_name.Name = "sLabel_user_name";
            this.sLabel_user_name.Size = new System.Drawing.Size(49, 19);
            this.sLabel_user_name.TabIndex = 17;
            this.sLabel_user_name.Text = "!Name";
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
            this.sTextBox_user_uid.Location = new System.Drawing.Point(61, 82);
            this.sTextBox_user_uid.MaxLength = 32767;
            this.sTextBox_user_uid.Name = "sTextBox_user_uid";
            this.sTextBox_user_uid.PasswordChar = '\0';
            this.sTextBox_user_uid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_user_uid.SelectedText = "";
            this.sTextBox_user_uid.SelectionLength = 0;
            this.sTextBox_user_uid.SelectionStart = 0;
            this.sTextBox_user_uid.ShortcutsEnabled = true;
            this.sTextBox_user_uid.Size = new System.Drawing.Size(196, 23);
            this.sTextBox_user_uid.TabIndex = 18;
            this.sTextBox_user_uid.UseSelectable = true;
            this.sTextBox_user_uid.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_user_uid.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
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
            this.sTextBox_user_deviceid.Location = new System.Drawing.Point(288, 82);
            this.sTextBox_user_deviceid.MaxLength = 32767;
            this.sTextBox_user_deviceid.Name = "sTextBox_user_deviceid";
            this.sTextBox_user_deviceid.PasswordChar = '\0';
            this.sTextBox_user_deviceid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_user_deviceid.SelectedText = "";
            this.sTextBox_user_deviceid.SelectionLength = 0;
            this.sTextBox_user_deviceid.SelectionStart = 0;
            this.sTextBox_user_deviceid.ShortcutsEnabled = true;
            this.sTextBox_user_deviceid.Size = new System.Drawing.Size(205, 23);
            this.sTextBox_user_deviceid.TabIndex = 19;
            this.sTextBox_user_deviceid.UseSelectable = true;
            this.sTextBox_user_deviceid.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_user_deviceid.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
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
            this.sTextBox_user_name.Location = new System.Drawing.Point(567, 82);
            this.sTextBox_user_name.MaxLength = 32767;
            this.sTextBox_user_name.Name = "sTextBox_user_name";
            this.sTextBox_user_name.PasswordChar = '\0';
            this.sTextBox_user_name.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.sTextBox_user_name.SelectedText = "";
            this.sTextBox_user_name.SelectionLength = 0;
            this.sTextBox_user_name.SelectionStart = 0;
            this.sTextBox_user_name.ShortcutsEnabled = true;
            this.sTextBox_user_name.Size = new System.Drawing.Size(170, 23);
            this.sTextBox_user_name.TabIndex = 20;
            this.sTextBox_user_name.UseSelectable = true;
            this.sTextBox_user_name.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.sTextBox_user_name.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // Panel_User_Item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Panel_User_Item";
            this.Panel_Base.ResumeLayout(false);
            this.Panel_Base.PerformLayout();
            this.metroTabControl_item.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metroGrid_item_weapon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl metroTabControl_item;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroGrid metroGrid_item_weapon;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
        private MetroFramework.Controls.MetroTabPage metroTabPage3;
        private MetroFramework.Controls.MetroTabPage metroTabPage4;
        private Controls.SLabel sLabel_user_deviceID;
        private Controls.SLabel sLabel_user_uid;
        private Controls.STextBox sTextBox_user_uid;
        private Controls.SLabel sLabel_user_name;
        private Controls.STextBox sTextBox_user_name;
        private Controls.STextBox sTextBox_user_deviceid;
    }
}
