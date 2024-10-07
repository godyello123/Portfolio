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

namespace GateServer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            CNetManager.Instance.FormMain = this;
            CNetManager.Instance.Start(CConfig.Instance.Port);
            CLogger.Instance.Start("Log/GateServer/GateServer", false);
            textBox1.Text = $"{CConfig.Instance.IP} : {CConfig.Instance.Port}";
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //m_UITimer.Stop();
            //m_LogTimer.Stop();
            CNetManager.Instance.Stop();
        }
    }
}
