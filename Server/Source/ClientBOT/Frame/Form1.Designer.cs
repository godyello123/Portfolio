
namespace ClientBOT
{
    partial class Form1
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Start = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_botSetting = new System.Windows.Forms.Button();
            this.label_testerCount = new System.Windows.Forms.Label();
            this.textBox_testerCount = new System.Windows.Forms.TextBox();
            this.label_testername = new System.Windows.Forms.Label();
            this.textBox_testername = new System.Windows.Forms.TextBox();
            this.label_host = new System.Windows.Forms.Label();
            this.textBox_host = new System.Windows.Forms.TextBox();
            this.label_port = new System.Windows.Forms.Label();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label_connectdelay = new System.Windows.Forms.Label();
            this.textBox_connectdelayMin = new System.Windows.Forms.TextBox();
            this.textBox_connectdelayMax = new System.Windows.Forms.TextBox();
            this.label_connectdelaymin_max = new System.Windows.Forms.Label();
            this.label_playtime_min_max = new System.Windows.Forms.Label();
            this.textBox_playtimeMax = new System.Windows.Forms.TextBox();
            this.textBox_playtimeMIn = new System.Windows.Forms.TextBox();
            this.label_playTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(108, 170);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(89, 45);
            this.button_Start.TabIndex = 17;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(203, 170);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(89, 45);
            this.button_stop.TabIndex = 18;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            // 
            // button_botSetting
            // 
            this.button_botSetting.Location = new System.Drawing.Point(10, 170);
            this.button_botSetting.Name = "button_botSetting";
            this.button_botSetting.Size = new System.Drawing.Size(92, 45);
            this.button_botSetting.TabIndex = 19;
            this.button_botSetting.Text = "BOTSetting";
            this.button_botSetting.UseVisualStyleBackColor = true;
            this.button_botSetting.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_testerCount
            // 
            this.label_testerCount.AutoSize = true;
            this.label_testerCount.Location = new System.Drawing.Point(203, 117);
            this.label_testerCount.Name = "label_testerCount";
            this.label_testerCount.Size = new System.Drawing.Size(74, 12);
            this.label_testerCount.TabIndex = 20;
            this.label_testerCount.Text = "TesterCount";
            // 
            // textBox_testerCount
            // 
            this.textBox_testerCount.Location = new System.Drawing.Point(205, 132);
            this.textBox_testerCount.Name = "textBox_testerCount";
            this.textBox_testerCount.ReadOnly = true;
            this.textBox_testerCount.Size = new System.Drawing.Size(87, 21);
            this.textBox_testerCount.TabIndex = 21;
            // 
            // label_testername
            // 
            this.label_testername.AutoSize = true;
            this.label_testername.Location = new System.Drawing.Point(12, 117);
            this.label_testername.Name = "label_testername";
            this.label_testername.Size = new System.Drawing.Size(75, 12);
            this.label_testername.TabIndex = 22;
            this.label_testername.Text = "TesterName";
            // 
            // textBox_testername
            // 
            this.textBox_testername.Location = new System.Drawing.Point(12, 132);
            this.textBox_testername.Name = "textBox_testername";
            this.textBox_testername.ReadOnly = true;
            this.textBox_testername.Size = new System.Drawing.Size(187, 21);
            this.textBox_testername.TabIndex = 23;
            // 
            // label_host
            // 
            this.label_host.AutoSize = true;
            this.label_host.Location = new System.Drawing.Point(12, 9);
            this.label_host.Name = "label_host";
            this.label_host.Size = new System.Drawing.Size(30, 12);
            this.label_host.TabIndex = 24;
            this.label_host.Text = "Host";
            // 
            // textBox_host
            // 
            this.textBox_host.Location = new System.Drawing.Point(12, 24);
            this.textBox_host.Name = "textBox_host";
            this.textBox_host.ReadOnly = true;
            this.textBox_host.Size = new System.Drawing.Size(187, 21);
            this.textBox_host.TabIndex = 25;
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.Location = new System.Drawing.Point(205, 9);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(27, 12);
            this.label_port.TabIndex = 26;
            this.label_port.Text = "Port";
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(207, 24);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.ReadOnly = true;
            this.textBox_port.Size = new System.Drawing.Size(87, 21);
            this.textBox_port.TabIndex = 27;
            // 
            // label_connectdelay
            // 
            this.label_connectdelay.AutoSize = true;
            this.label_connectdelay.Location = new System.Drawing.Point(12, 56);
            this.label_connectdelay.Name = "label_connectdelay";
            this.label_connectdelay.Size = new System.Drawing.Size(84, 12);
            this.label_connectdelay.TabIndex = 28;
            this.label_connectdelay.Text = "ConnectDelay";
            // 
            // textBox_connectdelayMin
            // 
            this.textBox_connectdelayMin.Location = new System.Drawing.Point(130, 53);
            this.textBox_connectdelayMin.Name = "textBox_connectdelayMin";
            this.textBox_connectdelayMin.ReadOnly = true;
            this.textBox_connectdelayMin.Size = new System.Drawing.Size(62, 21);
            this.textBox_connectdelayMin.TabIndex = 29;
            // 
            // textBox_connectdelayMax
            // 
            this.textBox_connectdelayMax.Location = new System.Drawing.Point(232, 53);
            this.textBox_connectdelayMax.Name = "textBox_connectdelayMax";
            this.textBox_connectdelayMax.ReadOnly = true;
            this.textBox_connectdelayMax.Size = new System.Drawing.Size(62, 21);
            this.textBox_connectdelayMax.TabIndex = 30;
            // 
            // label_connectdelaymin_max
            // 
            this.label_connectdelaymin_max.AutoSize = true;
            this.label_connectdelaymin_max.Location = new System.Drawing.Point(204, 59);
            this.label_connectdelaymin_max.Name = "label_connectdelaymin_max";
            this.label_connectdelaymin_max.Size = new System.Drawing.Size(14, 12);
            this.label_connectdelaymin_max.TabIndex = 31;
            this.label_connectdelaymin_max.Text = "~";
            // 
            // label_playtime_min_max
            // 
            this.label_playtime_min_max.AutoSize = true;
            this.label_playtime_min_max.Location = new System.Drawing.Point(204, 86);
            this.label_playtime_min_max.Name = "label_playtime_min_max";
            this.label_playtime_min_max.Size = new System.Drawing.Size(14, 12);
            this.label_playtime_min_max.TabIndex = 35;
            this.label_playtime_min_max.Text = "~";
            // 
            // textBox_playtimeMax
            // 
            this.textBox_playtimeMax.Location = new System.Drawing.Point(232, 80);
            this.textBox_playtimeMax.Name = "textBox_playtimeMax";
            this.textBox_playtimeMax.ReadOnly = true;
            this.textBox_playtimeMax.Size = new System.Drawing.Size(62, 21);
            this.textBox_playtimeMax.TabIndex = 34;
            // 
            // textBox_playtimeMIn
            // 
            this.textBox_playtimeMIn.Location = new System.Drawing.Point(130, 80);
            this.textBox_playtimeMIn.Name = "textBox_playtimeMIn";
            this.textBox_playtimeMIn.ReadOnly = true;
            this.textBox_playtimeMIn.Size = new System.Drawing.Size(62, 21);
            this.textBox_playtimeMIn.TabIndex = 33;
            // 
            // label_playTime
            // 
            this.label_playTime.AutoSize = true;
            this.label_playTime.Location = new System.Drawing.Point(12, 83);
            this.label_playTime.Name = "label_playTime";
            this.label_playTime.Size = new System.Drawing.Size(59, 12);
            this.label_playTime.TabIndex = 32;
            this.label_playTime.Text = "PlayTime";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 450);
            this.Controls.Add(this.label_playtime_min_max);
            this.Controls.Add(this.textBox_playtimeMax);
            this.Controls.Add(this.textBox_playtimeMIn);
            this.Controls.Add(this.label_playTime);
            this.Controls.Add(this.label_connectdelaymin_max);
            this.Controls.Add(this.textBox_connectdelayMax);
            this.Controls.Add(this.textBox_connectdelayMin);
            this.Controls.Add(this.label_connectdelay);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.label_port);
            this.Controls.Add(this.textBox_host);
            this.Controls.Add(this.label_host);
            this.Controls.Add(this.textBox_testername);
            this.Controls.Add(this.label_testername);
            this.Controls.Add(this.textBox_testerCount);
            this.Controls.Add(this.label_testerCount);
            this.Controls.Add(this.button_botSetting);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_Start);
            this.Name = "Form1";
            this.Text = "BOT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_botSetting;
        private System.Windows.Forms.Label label_testerCount;
        private System.Windows.Forms.TextBox textBox_testerCount;
        private System.Windows.Forms.Label label_testername;
        private System.Windows.Forms.TextBox textBox_testername;
        private System.Windows.Forms.Label label_host;
        private System.Windows.Forms.TextBox textBox_host;
        private System.Windows.Forms.Label label_port;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label_connectdelay;
        private System.Windows.Forms.TextBox textBox_connectdelayMin;
        private System.Windows.Forms.TextBox textBox_connectdelayMax;
        private System.Windows.Forms.Label label_connectdelaymin_max;
        private System.Windows.Forms.Label label_playtime_min_max;
        private System.Windows.Forms.TextBox textBox_playtimeMax;
        private System.Windows.Forms.TextBox textBox_playtimeMIn;
        private System.Windows.Forms.Label label_playTime;
    }
}

