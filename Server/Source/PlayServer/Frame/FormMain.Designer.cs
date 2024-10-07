namespace PlayServer
{
    partial class FormMain
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_PacketCount = new System.Windows.Forms.TextBox();
            this.listView_Thread = new System.Windows.Forms.ListView();
            this.columnHeader_ThreadID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_ThreadFPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_ThreadInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox_GCCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_UserCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Open = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_MainFPS = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Host = new System.Windows.Forms.TextBox();
            this.richTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.checkBox_ShowLog = new System.Windows.Forms.CheckBox();
            this.textBox_UserID = new System.Windows.Forms.TextBox();
            this.button_Search = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_SelectedUnit = new System.Windows.Forms.TextBox();
            this.textBox_Matcher = new System.Windows.Forms.TextBox();
            this.button_ReloadTable = new System.Windows.Forms.Button();
            this.richTextBox_LogUser = new System.Windows.Forms.RichTextBox();
            this.LogUser_Clear = new System.Windows.Forms.Button();
            this.label_serverversion = new System.Windows.Forms.Label();
            this.LogPacketCount = new System.Windows.Forms.Label();
            this.textBox_logpacket = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_PacketCount
            // 
            this.textBox_PacketCount.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_PacketCount.Location = new System.Drawing.Point(100, 93);
            this.textBox_PacketCount.Name = "textBox_PacketCount";
            this.textBox_PacketCount.Size = new System.Drawing.Size(142, 21);
            this.textBox_PacketCount.TabIndex = 29;
            // 
            // listView_Thread
            // 
            this.listView_Thread.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_ThreadID,
            this.columnHeader_ThreadFPS,
            this.columnHeader_ThreadInput});
            this.listView_Thread.HideSelection = false;
            this.listView_Thread.Location = new System.Drawing.Point(14, 152);
            this.listView_Thread.Name = "listView_Thread";
            this.listView_Thread.Size = new System.Drawing.Size(387, 166);
            this.listView_Thread.TabIndex = 28;
            this.listView_Thread.UseCompatibleStateImageBehavior = false;
            this.listView_Thread.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_ThreadID
            // 
            this.columnHeader_ThreadID.Text = "Thread ID";
            this.columnHeader_ThreadID.Width = 150;
            // 
            // columnHeader_ThreadFPS
            // 
            this.columnHeader_ThreadFPS.Text = "FPS";
            this.columnHeader_ThreadFPS.Width = 100;
            // 
            // columnHeader_ThreadInput
            // 
            this.columnHeader_ThreadInput.Text = "Input Count";
            this.columnHeader_ThreadInput.Width = 100;
            // 
            // textBox_GCCount
            // 
            this.textBox_GCCount.Location = new System.Drawing.Point(100, 66);
            this.textBox_GCCount.Name = "textBox_GCCount";
            this.textBox_GCCount.ReadOnly = true;
            this.textBox_GCCount.Size = new System.Drawing.Size(301, 21);
            this.textBox_GCCount.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "Packet Count";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 12);
            this.label7.TabIndex = 26;
            this.label7.Text = "GC Count";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 12);
            this.label6.TabIndex = 24;
            this.label6.Text = "User Count";
            // 
            // textBox_UserCount
            // 
            this.textBox_UserCount.Location = new System.Drawing.Point(100, 39);
            this.textBox_UserCount.Name = "textBox_UserCount";
            this.textBox_UserCount.ReadOnly = true;
            this.textBox_UserCount.Size = new System.Drawing.Size(142, 21);
            this.textBox_UserCount.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(258, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "Open";
            // 
            // textBox_Open
            // 
            this.textBox_Open.Location = new System.Drawing.Point(299, 13);
            this.textBox_Open.Name = "textBox_Open";
            this.textBox_Open.ReadOnly = true;
            this.textBox_Open.Size = new System.Drawing.Size(102, 21);
            this.textBox_Open.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(258, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "FPS";
            // 
            // textBox_MainFPS
            // 
            this.textBox_MainFPS.Location = new System.Drawing.Point(299, 39);
            this.textBox_MainFPS.Name = "textBox_MainFPS";
            this.textBox_MainFPS.ReadOnly = true;
            this.textBox_MainFPS.Size = new System.Drawing.Size(102, 21);
            this.textBox_MainFPS.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "Host";
            // 
            // textBox_Host
            // 
            this.textBox_Host.Location = new System.Drawing.Point(100, 13);
            this.textBox_Host.Name = "textBox_Host";
            this.textBox_Host.ReadOnly = true;
            this.textBox_Host.Size = new System.Drawing.Size(142, 21);
            this.textBox_Host.TabIndex = 17;
            // 
            // richTextBox_Log
            // 
            this.richTextBox_Log.Location = new System.Drawing.Point(14, 324);
            this.richTextBox_Log.Name = "richTextBox_Log";
            this.richTextBox_Log.Size = new System.Drawing.Size(387, 138);
            this.richTextBox_Log.TabIndex = 30;
            this.richTextBox_Log.Text = "";
            this.richTextBox_Log.WordWrap = false;
            // 
            // checkBox_ShowLog
            // 
            this.checkBox_ShowLog.AutoSize = true;
            this.checkBox_ShowLog.Location = new System.Drawing.Point(320, 127);
            this.checkBox_ShowLog.Name = "checkBox_ShowLog";
            this.checkBox_ShowLog.Size = new System.Drawing.Size(81, 16);
            this.checkBox_ShowLog.TabIndex = 31;
            this.checkBox_ShowLog.Text = "Show Log";
            this.checkBox_ShowLog.UseVisualStyleBackColor = true;
            this.checkBox_ShowLog.Click += new System.EventHandler(this.checkBox_ShowLog_CheckedChanged);
            // 
            // textBox_UserID
            // 
            this.textBox_UserID.Location = new System.Drawing.Point(506, 33);
            this.textBox_UserID.Name = "textBox_UserID";
            this.textBox_UserID.Size = new System.Drawing.Size(103, 21);
            this.textBox_UserID.TabIndex = 33;
            // 
            // button_Search
            // 
            this.button_Search.Location = new System.Drawing.Point(615, 31);
            this.button_Search.Name = "button_Search";
            this.button_Search.Size = new System.Drawing.Size(197, 23);
            this.button_Search.TabIndex = 34;
            this.button_Search.Text = "Search";
            this.button_Search.UseVisualStyleBackColor = true;
            this.button_Search.Click += new System.EventHandler(this.button_Search_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(407, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 12);
            this.label5.TabIndex = 35;
            this.label5.Text = "Search User ID";
            // 
            // textBox_SelectedUnit
            // 
            this.textBox_SelectedUnit.Location = new System.Drawing.Point(615, 350);
            this.textBox_SelectedUnit.Multiline = true;
            this.textBox_SelectedUnit.Name = "textBox_SelectedUnit";
            this.textBox_SelectedUnit.ReadOnly = true;
            this.textBox_SelectedUnit.Size = new System.Drawing.Size(300, 112);
            this.textBox_SelectedUnit.TabIndex = 36;
            // 
            // textBox_Matcher
            // 
            this.textBox_Matcher.Location = new System.Drawing.Point(407, 350);
            this.textBox_Matcher.Multiline = true;
            this.textBox_Matcher.Name = "textBox_Matcher";
            this.textBox_Matcher.ReadOnly = true;
            this.textBox_Matcher.Size = new System.Drawing.Size(202, 112);
            this.textBox_Matcher.TabIndex = 37;
            // 
            // button_ReloadTable
            // 
            this.button_ReloadTable.Location = new System.Drawing.Point(320, 98);
            this.button_ReloadTable.Name = "button_ReloadTable";
            this.button_ReloadTable.Size = new System.Drawing.Size(80, 23);
            this.button_ReloadTable.TabIndex = 46;
            this.button_ReloadTable.Text = "Reload";
            this.button_ReloadTable.UseVisualStyleBackColor = true;
            this.button_ReloadTable.Click += new System.EventHandler(this.button_ReloadTable_Click);
            // 
            // richTextBox_LogUser
            // 
            this.richTextBox_LogUser.Location = new System.Drawing.Point(409, 65);
            this.richTextBox_LogUser.Name = "richTextBox_LogUser";
            this.richTextBox_LogUser.Size = new System.Drawing.Size(506, 279);
            this.richTextBox_LogUser.TabIndex = 47;
            this.richTextBox_LogUser.Text = "";
            // 
            // LogUser_Clear
            // 
            this.LogUser_Clear.Location = new System.Drawing.Point(840, 31);
            this.LogUser_Clear.Name = "LogUser_Clear";
            this.LogUser_Clear.Size = new System.Drawing.Size(75, 23);
            this.LogUser_Clear.TabIndex = 48;
            this.LogUser_Clear.Text = "Clear";
            this.LogUser_Clear.UseVisualStyleBackColor = true;
            this.LogUser_Clear.Click += new System.EventHandler(this.LogUser_Clear_Click);
            // 
            // label_serverversion
            // 
            this.label_serverversion.AutoSize = true;
            this.label_serverversion.Location = new System.Drawing.Point(877, 9);
            this.label_serverversion.Name = "label_serverversion";
            this.label_serverversion.Size = new System.Drawing.Size(48, 12);
            this.label_serverversion.TabIndex = 49;
            this.label_serverversion.Text = "ver 1.31";
            // 
            // LogPacketCount
            // 
            this.LogPacketCount.AutoSize = true;
            this.LogPacketCount.Location = new System.Drawing.Point(15, 125);
            this.LogPacketCount.Name = "LogPacketCount";
            this.LogPacketCount.Size = new System.Drawing.Size(59, 12);
            this.LogPacketCount.TabIndex = 50;
            this.LogPacketCount.Text = "LogCount";
            // 
            // textBox_logpacket
            // 
            this.textBox_logpacket.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_logpacket.Location = new System.Drawing.Point(100, 120);
            this.textBox_logpacket.Name = "textBox_logpacket";
            this.textBox_logpacket.Size = new System.Drawing.Size(142, 21);
            this.textBox_logpacket.TabIndex = 51;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 474);
            this.Controls.Add(this.textBox_logpacket);
            this.Controls.Add(this.LogPacketCount);
            this.Controls.Add(this.label_serverversion);
            this.Controls.Add(this.LogUser_Clear);
            this.Controls.Add(this.richTextBox_LogUser);
            this.Controls.Add(this.button_ReloadTable);
            this.Controls.Add(this.textBox_Matcher);
            this.Controls.Add(this.textBox_SelectedUnit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_Search);
            this.Controls.Add(this.textBox_UserID);
            this.Controls.Add(this.checkBox_ShowLog);
            this.Controls.Add(this.richTextBox_Log);
            this.Controls.Add(this.textBox_PacketCount);
            this.Controls.Add(this.listView_Thread);
            this.Controls.Add(this.textBox_GCCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_UserCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_Open);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_MainFPS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Host);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PlayServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_PacketCount;
        private System.Windows.Forms.ListView listView_Thread;
        private System.Windows.Forms.ColumnHeader columnHeader_ThreadID;
        private System.Windows.Forms.ColumnHeader columnHeader_ThreadFPS;
        private System.Windows.Forms.ColumnHeader columnHeader_ThreadInput;
        private System.Windows.Forms.TextBox textBox_GCCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_UserCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Open;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_MainFPS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Host;
        private System.Windows.Forms.RichTextBox richTextBox_Log;
        private System.Windows.Forms.CheckBox checkBox_ShowLog;
		private System.Windows.Forms.TextBox textBox_UserID;
		private System.Windows.Forms.Button button_Search;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox_SelectedUnit;
		private System.Windows.Forms.TextBox textBox_Matcher;
		private System.Windows.Forms.Button button_ReloadTable;
        private System.Windows.Forms.RichTextBox richTextBox_LogUser;
        private System.Windows.Forms.Button LogUser_Clear;
        private System.Windows.Forms.Label label_serverversion;
        private System.Windows.Forms.Label LogPacketCount;
        private System.Windows.Forms.TextBox textBox_logpacket;
    }
}

