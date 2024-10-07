
namespace MessageServer
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Host = new System.Windows.Forms.TextBox();
            this.textBox_GCCount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_FPS = new System.Windows.Forms.TextBox();
            this.listView_GameServer = new System.Windows.Forms.ListView();
            this.columnHeader_PlayServer_SessionKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PlayServer_IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PlayServer_Port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PlayServer_Open = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PlayServer_ClientCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PlayServer_MainFPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PlayServer_DBFPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PlayServer_DBInputCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox_userCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox_Log
            // 
            this.richTextBox_Log.Location = new System.Drawing.Point(21, 246);
            this.richTextBox_Log.Name = "richTextBox_Log";
            this.richTextBox_Log.Size = new System.Drawing.Size(573, 200);
            this.richTextBox_Log.TabIndex = 48;
            this.richTextBox_Log.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 12);
            this.label1.TabIndex = 50;
            this.label1.Text = "Host";
            // 
            // textBox_Host
            // 
            this.textBox_Host.Location = new System.Drawing.Point(57, 12);
            this.textBox_Host.Name = "textBox_Host";
            this.textBox_Host.ReadOnly = true;
            this.textBox_Host.Size = new System.Drawing.Size(142, 21);
            this.textBox_Host.TabIndex = 49;
            // 
            // textBox_GCCount
            // 
            this.textBox_GCCount.Location = new System.Drawing.Point(346, 39);
            this.textBox_GCCount.Name = "textBox_GCCount";
            this.textBox_GCCount.ReadOnly = true;
            this.textBox_GCCount.Size = new System.Drawing.Size(248, 21);
            this.textBox_GCCount.TabIndex = 57;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(266, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 12);
            this.label7.TabIndex = 56;
            this.label7.Text = "GC Count";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 12);
            this.label2.TabIndex = 55;
            this.label2.Text = "FPS";
            // 
            // textBox_FPS
            // 
            this.textBox_FPS.Location = new System.Drawing.Point(57, 39);
            this.textBox_FPS.Name = "textBox_FPS";
            this.textBox_FPS.ReadOnly = true;
            this.textBox_FPS.Size = new System.Drawing.Size(142, 21);
            this.textBox_FPS.TabIndex = 54;
            // 
            // listView_GameServer
            // 
            this.listView_GameServer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_PlayServer_SessionKey,
            this.columnHeader_PlayServer_IP,
            this.columnHeader_PlayServer_Port,
            this.columnHeader_PlayServer_Open,
            this.columnHeader_PlayServer_ClientCount,
            this.columnHeader_PlayServer_MainFPS,
            this.columnHeader_PlayServer_DBFPS,
            this.columnHeader_PlayServer_DBInputCount});
            this.listView_GameServer.HideSelection = false;
            this.listView_GameServer.Location = new System.Drawing.Point(21, 77);
            this.listView_GameServer.Name = "listView_GameServer";
            this.listView_GameServer.Size = new System.Drawing.Size(573, 150);
            this.listView_GameServer.TabIndex = 58;
            this.listView_GameServer.UseCompatibleStateImageBehavior = false;
            this.listView_GameServer.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_PlayServer_SessionKey
            // 
            this.columnHeader_PlayServer_SessionKey.Text = "Key";
            // 
            // columnHeader_PlayServer_IP
            // 
            this.columnHeader_PlayServer_IP.Text = "IP";
            this.columnHeader_PlayServer_IP.Width = 100;
            // 
            // columnHeader_PlayServer_Port
            // 
            this.columnHeader_PlayServer_Port.Text = "Port";
            // 
            // columnHeader_PlayServer_Open
            // 
            this.columnHeader_PlayServer_Open.Text = "Open";
            this.columnHeader_PlayServer_Open.Width = 70;
            // 
            // columnHeader_PlayServer_ClientCount
            // 
            this.columnHeader_PlayServer_ClientCount.Text = "Client";
            // 
            // columnHeader_PlayServer_MainFPS
            // 
            this.columnHeader_PlayServer_MainFPS.Text = "Main";
            // 
            // columnHeader_PlayServer_DBFPS
            // 
            this.columnHeader_PlayServer_DBFPS.Text = "DB";
            // 
            // columnHeader_PlayServer_DBInputCount
            // 
            this.columnHeader_PlayServer_DBInputCount.Text = "DBInputCount";
            // 
            // textBox_userCount
            // 
            this.textBox_userCount.Location = new System.Drawing.Point(346, 12);
            this.textBox_userCount.Name = "textBox_userCount";
            this.textBox_userCount.ReadOnly = true;
            this.textBox_userCount.Size = new System.Drawing.Size(248, 21);
            this.textBox_userCount.TabIndex = 59;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 12);
            this.label3.TabIndex = 60;
            this.label3.Text = "UserCount";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 466);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_userCount);
            this.Controls.Add(this.listView_GameServer);
            this.Controls.Add(this.textBox_GCCount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_FPS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Host);
            this.Controls.Add(this.richTextBox_Log);
            this.Name = "FormMain";
            this.Text = "MessageServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_Log;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Host;
        private System.Windows.Forms.TextBox textBox_GCCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_FPS;
        private System.Windows.Forms.ListView listView_GameServer;
        private System.Windows.Forms.ColumnHeader columnHeader_PlayServer_SessionKey;
        private System.Windows.Forms.ColumnHeader columnHeader_PlayServer_IP;
        private System.Windows.Forms.ColumnHeader columnHeader_PlayServer_Port;
        private System.Windows.Forms.ColumnHeader columnHeader_PlayServer_Open;
        private System.Windows.Forms.ColumnHeader columnHeader_PlayServer_ClientCount;
        private System.Windows.Forms.ColumnHeader columnHeader_PlayServer_MainFPS;
        private System.Windows.Forms.ColumnHeader columnHeader_PlayServer_DBFPS;
        private System.Windows.Forms.ColumnHeader columnHeader_PlayServer_DBInputCount;
        private System.Windows.Forms.TextBox textBox_userCount;
        private System.Windows.Forms.Label label3;
    }
}

