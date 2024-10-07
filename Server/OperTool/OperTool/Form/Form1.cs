using OperTool.Panel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCommon;
using OperTool;
using MongoDB.Driver.Core.Servers;
using Global;
using System.Collections.Concurrent;

namespace OperTool
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        private eServerType m_ServerType = eServerType.Max;
        private string m_UserID = string.Empty;
        private ConcurrentQueue<IWorker> m_WorkQueue = new ConcurrentQueue<IWorker>();

        private Timer m_FormTimer;
        private Timer m_LogTimer;

        public string USER_ID { get { return m_UserID; } }

        public Form1()
        {
            InitializeComponent();

            m_FormTimer = new Timer();
            m_FormTimer.Interval = 500;
            m_FormTimer.Tick += new EventHandler(OnFormTimer);
            m_FormTimer.Start();

            m_LogTimer = new Timer();
            m_LogTimer.Interval = 500;
            m_LogTimer.Tick += new EventHandler(OnLogTimer);
            m_LogTimer.Start();
        }

        public void InsertWork(IWorker work)
        {
            m_WorkQueue.Enqueue(work);
        }

        public void OnFormTimer(object sender, EventArgs e)
        {
            IWorker work = null;
            if(m_WorkQueue.TryDequeue(out work))
                work.Excute();
        }

        public void OnLogTimer(object sender, EventArgs e)
        {
            MongoDBManager.Instance.Update();
        }

        public void Init()
        {
            PanelManager.Instance.Init(this);
        }

        public void SetMainFormServerState(eServerType type, string ID, bool isConnected)
        {
            m_ServerType = type;
            m_UserID = ID;
            metroTextBox_connect_server.Text = m_ServerType.ToString();
            metroTextBox_connecting.Text = isConnected.ToString();

            var operRecord = OperAccountTable.Instance.Find(ID);
            if (operRecord != null)
                metroTextBox_oper_role.Text = operRecord.Role.ToString();
        }

        private void metroLink_user_infomation_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.USER_INFOMATION);
        }

        private void metroLink_home_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.HOME);
        }

        private void metroLink_system_post_view_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SYSTEM_POST_VIEW);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormManager.Instance.Stop();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            CNetManager.Instance.DisConnectServer();
            FormManager.Instance.ShowLoginForm();

            MongoDBManager.Instance.Stop();
            this.Hide();
        }

        public void DisConnectServer()
        {
            CMessageBoxManager.Instance.Error(this, eMessageBoxMessage.DisConnectServer);
            CNetManager.Instance.DisConnectServer();
            FormManager.Instance.ShowLoginForm();

            MongoDBManager.Instance.Stop();
            this.Hide();
        }

        private void metroLink_user_item_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.USER_ITEM);
        }

        private void metroLink_user_contents_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.USER_CONTENTS);
        }

        private void metroLink_user_post_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.USER_POST);
        }

        private void metroLink_user_receipt_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.USER_RECEIPT);
        }

        private void metroLink_send_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SYSTEM_POST_SEND);
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SYSTEM_NOTICE);
        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SYSTEM_BLOCKUSER);
        }

        private void metroLink3_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SYSTEM_WHITEUSER);
        }

        private void metroLink4_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SYSTEM_COUPON);
        }
      
        private void metroLink6_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SYSTEM_POST_VIEW);
        }

        private void metroLink7_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SYSTEM_POST_SEND);
        }

        private void metroLink8_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.SERVER);
        }

        private void OnVisible(object sender, EventArgs e)
        {
            if (this.Visible)
                OperTabControl_main.SelectedIndex = 0;
        }

        private void metroLink9_Click(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.GAME_LOG);
        }
    }
}
