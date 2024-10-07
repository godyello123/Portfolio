using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global;
using SCommon;
using SNetwork;

namespace MessageServer
{
    public partial class FormMain : Form
    {
        public class CUIData
        {
            public int UserCount { get; set; }
            public int FPS { get; set; }
            private Dictionary<long, CServerInfo> m_ServerTable = new Dictionary<long, CServerInfo>();
            public Dictionary<long, CServerInfo> ServerTable
            {
                get { return m_ServerTable; }
                set { m_ServerTable = SCopy<Dictionary<long, CServerInfo>>.DeepCopy(value); }
            }
        }

        private Timer m_UITimer;
        private Timer m_LogTimer;
        public bool Stop { get; set; }
        private CUIData m_UIData = new CUIData();
        private Queue<string> m_Log = new Queue<string>();

        public FormMain()
        {
            InitializeComponent();

            CLogger.Instance.FormMain = this;
            CLogger.Instance.Start("Log/MessageServer/MessageServer", false);
            CNetManager.Instance.FormMain = this;
            CNetManager.Instance.Start(CConfig.Instance.Port);

            textBox_Host.Text = string.Format("{0} : {1}", SNetSystem.GetLocalIP(), CConfig.Instance.Port.ToString());

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

        public void WriteLog(string log)
        {
            lock (m_Log) { m_Log.Enqueue(log); }
        }

        public void OnUITimer(object sender, EventArgs e)
        {
            if (Stop) Close();

            textBox_GCCount.Text = string.Format("Gen0 : {0}, Gen1 : {1}, Gen2 : {2}",
                GC.CollectionCount(0).ToString(), GC.CollectionCount(1).ToString(), GC.CollectionCount(2).ToString());

            lock (m_UIData)
            {
                textBox_FPS.Text = m_UIData.FPS.ToString();

                int userCnt = 0;
                listView_GameServer.Items.Clear();
                foreach (var iter in m_UIData.ServerTable)
                {
                    var server = iter.Value;
                    if (server.m_Type != "PlayServer") continue;

                    int i = listView_GameServer.Items.Count;
                    listView_GameServer.Items.Add(server.m_ServerSessionKey.ToString());
                    listView_GameServer.Items[i].SubItems.Add(server.m_DN);
                    listView_GameServer.Items[i].SubItems.Add(server.m_Port.ToString());
                    listView_GameServer.Items[i].SubItems.Add(((CDefine.eServiceType)server.m_ServiceType).ToString());
                    listView_GameServer.Items[i].SubItems.Add(server.m_ClientCount.ToString());
                    listView_GameServer.Items[i].SubItems.Add(server.m_MainFPS.ToString());
                    listView_GameServer.Items[i].SubItems.Add(server.m_DBFPS.ToString());
                    listView_GameServer.Items[i].SubItems.Add(server.m_DBInputCount.ToString());

                    userCnt += server.m_ClientCount;
                }

                textBox_userCount.Text = userCnt.ToString();
            }
        }

        public void OnLogTimer(object sender, EventArgs e)
        {
            lock (m_Log)
            {
                while (m_Log.Count > 0)
                {
                    richTextBox_Log.SelectionStart = richTextBox_Log.Text.Length;
                    richTextBox_Log.ScrollToCaret();
                    richTextBox_Log.AppendText(m_Log.Dequeue());
                    richTextBox_Log.AppendText("\n");
                }
            }
        }

        public void SetUIData(int fps, Dictionary<long, CServerInfo> servers)
        {
            m_UIData.FPS = fps;
            m_UIData.ServerTable = servers;
        }
    }
}
