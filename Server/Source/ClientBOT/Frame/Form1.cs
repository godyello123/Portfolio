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
using Global;

namespace ClientBOT
{
    public partial class Form1 : Form
    {
        private Timer m_Timer;
        private STimer m_STimer = new STimer(1000);
        private _UserData m_UserData = new _UserData();

        public long m_UID;
        public string m_DeviceID;

        public BOTSetting m_BOTSetting = new BOTSetting();
        public bool m_IsBOTSetting = false;

        public Form1()
        {
            InitializeComponent();

            CNetManager.Instance.Start();
            CNetManager.Instance.FormMain = this;

            m_Timer = new Timer();
            m_Timer.Interval = 1;
            m_Timer.Tick += new EventHandler(OnTimerTick);
            m_Timer.Start();
        }

        public void OnTimerTick(object sender, EventArgs e)
        {
            CNetManager.Instance.Update();
            CTesterManager.Instance.Update();
            //if(m_STimer.Check())
            //    CNetManager.Instance.WritePlayServer(new Packet_X2X.X2X_HeartBeat());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CNetManager.Instance.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SCSVReader reader = new SCSVReader();
            reader.Load("BOTSetting.csv");

            int rowCount = reader.GetRowCount();
            for (int i = 0; i < rowCount; ++i)
            {
                m_BOTSetting.m_Host = reader.GetValue(i, "Host");
                m_BOTSetting.m_Port = reader.GetValue<ushort>(i, "Port");
                m_BOTSetting.m_TesterName = reader.GetValue(i, "TesterName");
                m_BOTSetting.m_TesterCount = reader.GetValue<int>(i, "TesterCount");
                m_BOTSetting.m_ConnectDelayMin = reader.GetValue<int>(i, "ConnectDelayMin");
                m_BOTSetting.m_ConenctDelayMax = reader.GetValue<int>(i, "ConenctDelayMax");
                m_BOTSetting.m_PlayTimeMin = reader.GetValue<int>(i, "PlayTimeMin");
                m_BOTSetting.m_PlayTimeMax = reader.GetValue<int>(i, "PlayTimeMax");

                m_IsBOTSetting = true;

                textBox_testerCount.Text = m_BOTSetting.m_TesterCount.ToString();
                textBox_testername.Text = m_BOTSetting.m_TesterName;
                textBox_host.Text = m_BOTSetting.m_Host;
                textBox_port.Text = m_BOTSetting.m_Port.ToString();
                textBox_playtimeMIn.Text = m_BOTSetting.m_PlayTimeMin.ToString();
                textBox_playtimeMax.Text = m_BOTSetting.m_PlayTimeMax.ToString();
                textBox_connectdelayMin.Text = m_BOTSetting.m_ConnectDelayMin.ToString();
                textBox_connectdelayMax.Text = m_BOTSetting.m_ConenctDelayMax.ToString();
            }
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (!m_IsBOTSetting)
            {
                MessageBox.Show(this, "Please BOT Setting!");
                return;
            }

            CTesterManager.Instance.Start();
        }
    }
}
