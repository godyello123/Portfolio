
namespace LogServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_send = new System.Windows.Forms.TextBox();
            this.textBox_recv = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Host = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_servercount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "RecvCount";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "SendCount";
            // 
            // textBox_send
            // 
            this.textBox_send.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_send.Location = new System.Drawing.Point(116, 69);
            this.textBox_send.Name = "textBox_send";
            this.textBox_send.Size = new System.Drawing.Size(142, 21);
            this.textBox_send.TabIndex = 52;
            // 
            // textBox_recv
            // 
            this.textBox_recv.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_recv.Location = new System.Drawing.Point(116, 40);
            this.textBox_recv.Name = "textBox_recv";
            this.textBox_recv.Size = new System.Drawing.Size(142, 21);
            this.textBox_recv.TabIndex = 53;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 12);
            this.label4.TabIndex = 55;
            this.label4.Text = "Host";
            // 
            // textBox_Host
            // 
            this.textBox_Host.Location = new System.Drawing.Point(116, 12);
            this.textBox_Host.Name = "textBox_Host";
            this.textBox_Host.ReadOnly = true;
            this.textBox_Host.Size = new System.Drawing.Size(142, 21);
            this.textBox_Host.TabIndex = 54;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 12);
            this.label3.TabIndex = 56;
            this.label3.Text = "ServerCount";
            // 
            // textBox_servercount
            // 
            this.textBox_servercount.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_servercount.Location = new System.Drawing.Point(116, 98);
            this.textBox_servercount.Name = "textBox_servercount";
            this.textBox_servercount.Size = new System.Drawing.Size(142, 21);
            this.textBox_servercount.TabIndex = 57;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 303);
            this.Controls.Add(this.textBox_servercount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_Host);
            this.Controls.Add(this.textBox_recv);
            this.Controls.Add(this.textBox_send);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "LogServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_send;
        private System.Windows.Forms.TextBox textBox_recv;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Host;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_servercount;
    }
}

