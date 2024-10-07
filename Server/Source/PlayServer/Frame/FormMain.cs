using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace PlayServer
{
	public delegate void QuitMatchViewDelegate();

	public partial class FormMain : Form
	{
		public class CUIData
		{
			public string Open { get; set; }
			public int UserCount { get; set; }
            public int MainFPS { get; set; }
			public List<CThreadState> ThreadStateList { get; set; }
			public int RecvPacketCount { get; set; }
			public int SendPacketCount { get; set; }

			public int LogPacketCount { get; set; }
		}

		private Timer m_UITimer;
		private Timer m_LogTimer;
		public bool Stop { get; set; }
		private CUIData m_UIData = new CUIData();
		private object m_UILock = new object();
		private string m_SearchUserID = "";
		public string SearchUserID { get { lock(m_UILock) { return m_SearchUserID; } } set { lock(m_UILock) { m_SearchUserID = value; } } }
		private Queue<string> m_Log = new Queue<string>();
        private Queue<string> m_UserLog = new Queue<string>();

        public FormMain()
		{
			InitializeComponent();

			CLogger.Instance.FormMain = this;
			CLogger.Instance.Start("Log/PlayServer/PlayServer", false);
			CNetManager.Instance.FormMain = this;
			CNetManager.Instance.Start(CConfig.Instance.PORT);

			textBox_Host.Text = string.Format("{0} : {1}", CConfig.Instance.IP, CConfig.Instance.PORT.ToString());
            label_serverversion.Text = "";//Global.CDefine.VERSION;


            m_UITimer = new Timer();
			m_UITimer.Interval = 1000;
			m_UITimer.Tick += new EventHandler(OnUITimer);
			m_UITimer.Start();

			m_LogTimer = new Timer();
			m_LogTimer.Interval = 100;
			m_LogTimer.Tick += new EventHandler(OnLogTimer);
			m_LogTimer.Start();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			m_UITimer.Stop();
			m_LogTimer.Stop();
            CNetManager.Instance.Stop();
        }

		public void SetUIData(string type, int userCount, int mainFPS, List<CThreadState> threadStateList,
			int recvPacketCount, int sendPacketCount,int logpacketcount)
		{
			lock(m_UIData)
			{
				m_UIData.Open = type;
				m_UIData.UserCount = userCount;
				m_UIData.MainFPS = mainFPS;
				m_UIData.ThreadStateList = threadStateList;
				m_UIData.RecvPacketCount = recvPacketCount;
				m_UIData.SendPacketCount = sendPacketCount;
				m_UIData.LogPacketCount = logpacketcount;
			}
		}

		public void WriteLog(string log)
		{
			lock(m_Log) { m_Log.Enqueue(log); }
		}

        public void WriteUserLog(string log)
        {
            lock (m_Log) { m_UserLog.Enqueue(log); }
        }

        public void OnUITimer(object sender, EventArgs e)
		{
			if(Stop) Close();
			textBox_GCCount.Text = string.Format("Gen0 : {0}, Gen1 : {1}, Gen2 : {2}",
				GC.CollectionCount(0).ToString(), GC.CollectionCount(1).ToString(), GC.CollectionCount(2).ToString());

			lock(m_UIData)
			{
				textBox_Open.Text = m_UIData.Open.ToString();
				textBox_UserCount.Text = m_UIData.UserCount.ToString();
				textBox_MainFPS.Text = m_UIData.MainFPS.ToString();
				textBox_PacketCount.Text = string.Format("{0} / {1}", m_UIData.RecvPacketCount, m_UIData.SendPacketCount);
				textBox_logpacket.Text = string.Format("{0}", m_UIData.LogPacketCount);

				if(m_UIData.ThreadStateList != null)
				{
					if(listView_Thread.Items.Count != m_UIData.ThreadStateList.Count)
					{
						listView_Thread.Items.Clear();
						foreach(var ThreadState in m_UIData.ThreadStateList)
						{
							int i = listView_Thread.Items.Count;
							listView_Thread.Items.Add(ThreadState.ID);
							listView_Thread.Items[i].SubItems.Add(ThreadState.FPS.ToString());
							listView_Thread.Items[i].SubItems.Add(ThreadState.InputCount.ToString());
						}
					}
					else
					{
						for(int i = 0; i < m_UIData.ThreadStateList.Count; i++)
						{
							listView_Thread.Items[i].SubItems[1].Text = m_UIData.ThreadStateList[i].FPS.ToString();
							listView_Thread.Items[i].SubItems[2].Text = m_UIData.ThreadStateList[i].InputCount.ToString();
						}
					}
				}
			}
		}

		public void OnLogTimer(object sender, EventArgs e)
		{
			lock(m_Log)
			{
				while(m_Log.Count > 0)
				{
					richTextBox_Log.SelectionStart = richTextBox_Log.Text.Length;
					richTextBox_Log.ScrollToCaret();
					richTextBox_Log.AppendText(m_Log.Dequeue());
					richTextBox_Log.AppendText("\n");
				}

                while (m_UserLog.Count > 0)
                {
                    richTextBox_LogUser.SelectionStart = richTextBox_LogUser.Text.Length;
                    richTextBox_LogUser.ScrollToCaret();
                    richTextBox_LogUser.AppendText(m_UserLog.Dequeue());
                    richTextBox_LogUser.AppendText("\n");
                }
            }
		}

		private void checkBox_ShowLog_CheckedChanged(object sender, EventArgs e)
		{
			CLogger.Instance.ShowLog = checkBox_ShowLog.Checked;
		}

		private void button_Search_Click(object sender, EventArgs e)
		{
			SearchUserID = textBox_UserID.Text;
		}

		private void pictureBox_Match_Paint(object sender, PaintEventArgs e)
		{
			DrawMatch(e.Graphics);
		}

		private void button_ReloadTable_Click(object sender, EventArgs e)
		{
			CNetManager.Instance.ReloadTable = true;
		}

        private void LogUser_Clear_Click(object sender, EventArgs e)
        {
			richTextBox_LogUser.Clear();
            richTextBox_Log.Clear();
		}
    }
}
