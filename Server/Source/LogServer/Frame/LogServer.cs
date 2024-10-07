using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LogServer
{
    public partial class Form1 : Form
    {
        public class CUIData
        {
            public int RecvCount { get; set; }
            public int SendCount { get; set; }

            public int ServerCount { get; set; }
        }

        private CUIData m_UIData = new CUIData();
        private Timer m_UITimer;
		public bool Stop = false;
        private List<string> ServerList = new List<string>();

        public Form1()
        {
            InitializeComponent();
            CLogger.Instance.Form = this;
            CLogger.Instance.Start("Log/LogServer/LogServer", false);
            CNetManager.Instance.Form = this;
            CNetManager.Instance.Start();

            //Global.SAWSS3Manager.Instance.Init(CConfig.Instance.AWS_BucketName, CConfig.Instance.AWS_BucketRegion);
            ///Global.SLogManager.Instance.Init();


            textBox_Host.Text = string.Format("{0} : {1}", CConfig.Instance.PublicIP, CConfig.Instance.Port.ToString());

            m_UITimer = new Timer();
            m_UITimer.Interval = 1000;
            m_UITimer.Tick += new EventHandler(OnUITimer);
            m_UITimer.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CNetManager.Instance.Stop();
            //Global.SLogManager.Instance.Stop();
            //Global.SAWSS3Manager.Instance.Stop();
        }

        public void SetUIData(int recvPacketCount, int sendPacketCount)
        {
            lock (m_UIData)
            {
                m_UIData.RecvCount = recvPacketCount;
                m_UIData.SendCount = sendPacketCount;
            }
        }

        public void OnUITimer(object sender, EventArgs e)
		{
			if (Stop) Close();
			
			lock (m_UIData)
			{
                textBox_recv.Text = m_UIData.RecvCount.ToString();
                textBox_send.Text = m_UIData.SendCount.ToString();
                textBox_servercount.Text = m_UIData.ServerCount.ToString();
			}
		}

        public void AddServerCount()
        {
            m_UIData.ServerCount++;
        }

        public void DeleteServerCount()
        {
            m_UIData.ServerCount--;
            if (m_UIData.ServerCount <= 0)
                m_UIData.ServerCount = 0;
        }
	}
}
