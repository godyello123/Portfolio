namespace OperTool
{
    partial class Form2
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
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroTextBox_id = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox_pw = new MetroFramework.Controls.MetroTextBox();
            this.metroComboBox_serverType = new MetroFramework.Controls.MetroComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.metroTextBox_custom_ip = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBox_custom_port = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroButton1
            // 
            this.metroButton1.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.metroButton1.Location = new System.Drawing.Point(95, 340);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(201, 35);
            this.metroButton1.TabIndex = 3;
            this.metroButton1.Text = "Login";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroTextBox_id
            // 
            // 
            // 
            // 
            this.metroTextBox_id.CustomButton.Image = null;
            this.metroTextBox_id.CustomButton.Location = new System.Drawing.Point(179, 1);
            this.metroTextBox_id.CustomButton.Name = "";
            this.metroTextBox_id.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_id.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_id.CustomButton.TabIndex = 1;
            this.metroTextBox_id.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_id.CustomButton.UseSelectable = true;
            this.metroTextBox_id.CustomButton.Visible = false;
            this.metroTextBox_id.Lines = new string[] {
        "ID"};
            this.metroTextBox_id.Location = new System.Drawing.Point(95, 282);
            this.metroTextBox_id.MaxLength = 32767;
            this.metroTextBox_id.Name = "metroTextBox_id";
            this.metroTextBox_id.PasswordChar = '\0';
            this.metroTextBox_id.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_id.SelectedText = "";
            this.metroTextBox_id.SelectionLength = 0;
            this.metroTextBox_id.SelectionStart = 0;
            this.metroTextBox_id.ShortcutsEnabled = true;
            this.metroTextBox_id.Size = new System.Drawing.Size(201, 23);
            this.metroTextBox_id.TabIndex = 1;
            this.metroTextBox_id.Text = "ID";
            this.metroTextBox_id.UseSelectable = true;
            this.metroTextBox_id.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_id.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTextBox_pw
            // 
            // 
            // 
            // 
            this.metroTextBox_pw.CustomButton.Image = null;
            this.metroTextBox_pw.CustomButton.Location = new System.Drawing.Point(179, 1);
            this.metroTextBox_pw.CustomButton.Name = "";
            this.metroTextBox_pw.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_pw.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_pw.CustomButton.TabIndex = 1;
            this.metroTextBox_pw.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_pw.CustomButton.UseSelectable = true;
            this.metroTextBox_pw.CustomButton.Visible = false;
            this.metroTextBox_pw.Lines = new string[] {
        "Password"};
            this.metroTextBox_pw.Location = new System.Drawing.Point(95, 311);
            this.metroTextBox_pw.MaxLength = 32767;
            this.metroTextBox_pw.Name = "metroTextBox_pw";
            this.metroTextBox_pw.PasswordChar = '●';
            this.metroTextBox_pw.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_pw.SelectedText = "";
            this.metroTextBox_pw.SelectionLength = 0;
            this.metroTextBox_pw.SelectionStart = 0;
            this.metroTextBox_pw.ShortcutsEnabled = true;
            this.metroTextBox_pw.Size = new System.Drawing.Size(201, 23);
            this.metroTextBox_pw.TabIndex = 2;
            this.metroTextBox_pw.Text = "Password";
            this.metroTextBox_pw.UseSelectable = true;
            this.metroTextBox_pw.UseSystemPasswordChar = true;
            this.metroTextBox_pw.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_pw.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroComboBox_serverType
            // 
            this.metroComboBox_serverType.FormattingEnabled = true;
            this.metroComboBox_serverType.ItemHeight = 23;
            this.metroComboBox_serverType.Location = new System.Drawing.Point(95, 247);
            this.metroComboBox_serverType.Name = "metroComboBox_serverType";
            this.metroComboBox_serverType.Size = new System.Drawing.Size(201, 29);
            this.metroComboBox_serverType.TabIndex = 4;
            this.metroComboBox_serverType.UseSelectable = true;
            this.metroComboBox_serverType.SelectedIndexChanged += new System.EventHandler(this.metroComboBox_serverType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(155, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 39);
            this.label1.TabIndex = 5;
            this.label1.Text = "Login";
            // 
            // metroTextBox_custom_ip
            // 
            // 
            // 
            // 
            this.metroTextBox_custom_ip.CustomButton.Image = null;
            this.metroTextBox_custom_ip.CustomButton.Location = new System.Drawing.Point(135, 1);
            this.metroTextBox_custom_ip.CustomButton.Name = "";
            this.metroTextBox_custom_ip.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_custom_ip.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_custom_ip.CustomButton.TabIndex = 1;
            this.metroTextBox_custom_ip.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_custom_ip.CustomButton.UseSelectable = true;
            this.metroTextBox_custom_ip.CustomButton.Visible = false;
            this.metroTextBox_custom_ip.Lines = new string[0];
            this.metroTextBox_custom_ip.Location = new System.Drawing.Point(139, 218);
            this.metroTextBox_custom_ip.MaxLength = 32767;
            this.metroTextBox_custom_ip.Name = "metroTextBox_custom_ip";
            this.metroTextBox_custom_ip.PasswordChar = '\0';
            this.metroTextBox_custom_ip.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_custom_ip.SelectedText = "";
            this.metroTextBox_custom_ip.SelectionLength = 0;
            this.metroTextBox_custom_ip.SelectionStart = 0;
            this.metroTextBox_custom_ip.ShortcutsEnabled = true;
            this.metroTextBox_custom_ip.Size = new System.Drawing.Size(157, 23);
            this.metroTextBox_custom_ip.TabIndex = 6;
            this.metroTextBox_custom_ip.UseSelectable = true;
            this.metroTextBox_custom_ip.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_custom_ip.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTextBox_custom_port
            // 
            // 
            // 
            // 
            this.metroTextBox_custom_port.CustomButton.Image = null;
            this.metroTextBox_custom_port.CustomButton.Location = new System.Drawing.Point(135, 1);
            this.metroTextBox_custom_port.CustomButton.Name = "";
            this.metroTextBox_custom_port.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox_custom_port.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_custom_port.CustomButton.TabIndex = 1;
            this.metroTextBox_custom_port.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_custom_port.CustomButton.UseSelectable = true;
            this.metroTextBox_custom_port.CustomButton.Visible = false;
            this.metroTextBox_custom_port.Lines = new string[0];
            this.metroTextBox_custom_port.Location = new System.Drawing.Point(139, 189);
            this.metroTextBox_custom_port.MaxLength = 32767;
            this.metroTextBox_custom_port.Name = "metroTextBox_custom_port";
            this.metroTextBox_custom_port.PasswordChar = '\0';
            this.metroTextBox_custom_port.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_custom_port.SelectedText = "";
            this.metroTextBox_custom_port.SelectionLength = 0;
            this.metroTextBox_custom_port.SelectionStart = 0;
            this.metroTextBox_custom_port.ShortcutsEnabled = true;
            this.metroTextBox_custom_port.Size = new System.Drawing.Size(157, 23);
            this.metroTextBox_custom_port.TabIndex = 7;
            this.metroTextBox_custom_port.UseSelectable = true;
            this.metroTextBox_custom_port.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_custom_port.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(105, 189);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(20, 19);
            this.metroLabel1.TabIndex = 8;
            this.metroLabel1.Text = "IP";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(97, 218);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(34, 19);
            this.metroLabel2.TabIndex = 9;
            this.metroLabel2.Text = "Port";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 546);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.metroTextBox_custom_port);
            this.Controls.Add(this.metroTextBox_custom_ip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.metroComboBox_serverType);
            this.Controls.Add(this.metroTextBox_pw);
            this.Controls.Add(this.metroTextBox_id);
            this.Controls.Add(this.metroButton1);
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.OnVisibie);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroTextBox metroTextBox_id;
        private MetroFramework.Controls.MetroTextBox metroTextBox_pw;
        private MetroFramework.Controls.MetroComboBox metroComboBox_serverType;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroTextBox metroTextBox_custom_ip;
        private MetroFramework.Controls.MetroTextBox metroTextBox_custom_port;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
    }
}