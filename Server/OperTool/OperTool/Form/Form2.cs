using Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperTool
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {
        private eServerType m_ServerType = eServerType.Max;

        public Form2()
        {
            InitializeComponent();
        }

        public void Init()
        {
            metroComboBox_serverType.Items.Clear();
            for (eServerType i = eServerType.Dev; i < eServerType.Max; ++i)
                metroComboBox_serverType.Items.Add(i.ToString());

            ClearLoginData();
        }

        public eServerType GetServerType()
        {
            return m_ServerType;
        }

        public string GetLoginID()
        {
            return metroTextBox_id.Text;
        }

        public string GetLoginPW()
        {
            return metroTextBox_pw.Text;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(metroTextBox_id.Text) || string.IsNullOrEmpty(metroTextBox_pw.Text))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.IDorPwCheck);
                return;
            }

            if (!OperAccountTable.Instance.IsVaildAccount(metroTextBox_id.Text, metroTextBox_pw.Text))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.IDorPwCheck);
                return;
            }

            if (m_ServerType == eServerType.Custom)
            {
                ushort custom_port = 0;
                if(!ushort.TryParse(metroTextBox_custom_port.Text, out custom_port) || string.IsNullOrEmpty(metroTextBox_custom_ip.Text))
                {
                    CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.IPorPortCheck);
                    return;
                }

                CNetManager.Instance.ConneectServer(metroTextBox_custom_ip.Text, custom_port);
            }
            else
            {
                var connectRecord = OperConnectTable.Instance.Find(m_ServerType);
                if (connectRecord == null)
                {
                    CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.ServerSelectError);
                    return;
                }

                CNetManager.Instance.ConneectServer(connectRecord.DN, connectRecord.Port);
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormManager.Instance.Stop();
        }

        private void metroComboBox_serverType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = metroComboBox_serverType.SelectedItem;
            if(!Enum.TryParse(type.ToString(), out m_ServerType))
                CMessageBoxManager.Instance.Error(this, eMessageBoxMessage.ServerSelectError);

            if (m_ServerType == eServerType.Custom)
            {
                metroTextBox_custom_ip.Enabled = true;
                metroTextBox_custom_port.Enabled = true;
            }
        }

        private void OnVisibie(object sender, EventArgs e)
        {
            if (this.Visible == true)
                ClearLoginData();
        }

        private void ClearLoginData()
        {
            metroComboBox_serverType.SelectedIndex = 0;
            metroTextBox_custom_ip.Enabled = false;
            metroTextBox_custom_port.Enabled = false;
            metroTextBox_custom_ip.Text = string.Empty;
            metroTextBox_custom_port.Text = string.Empty;
            metroTextBox_id.Text = string.Empty;
            metroTextBox_pw.Text = string.Empty;

        }
    }
}
