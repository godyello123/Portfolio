using Global;
using Packet_O2M;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperTool.Panel
{
    public partial class Panel_Server : PannelBase
    {
        public Panel_Server()
        {
            InitializeComponent();
            m_ActionFunc += ServerDataLoad;
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            metroComboBox_service_type.Items.Clear();
            for (eServerOpenType i = eServerOpenType.Open; i < eServerOpenType.Max; i++)
                metroComboBox_service_type.Items.Add(i.ToString());

            metroComboBox_service_type.SelectedIndex = 0;

            customGridView_serverinfo.SetHeader<ServerInfoView>();
        }

        private void metroButton_server_refresh_Click(object sender, EventArgs e)
        {
            ServerDataLoad();
        }

        public void ServerDataLoad()
        {
            CNetManager.Instance.O2M_RequestServerStateLoad();
        }

        public void AfterExcuteServerStateLoad(List<CServerInfo> datas, bool Open)
        {
            var views = new List<ServerInfoView>();
            foreach (var iter in datas)
                views.Add(new ServerInfoView(iter));

            customGridView_serverinfo.SetData<ServerInfoView>(views);

            metroCheckBox_live.Checked = Open;

            CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.Success);
        }

        private void customGridView_serverinfo_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            metroTextBox_detail_type.Text = customGridView_serverinfo.GetData("ServerInfoView_Type");
            metroTextBox_detail_user_cnt.Text = customGridView_serverinfo.GetData("ServerInfoView_ClinetCount");
            metroTextBox_detail_ip.Text = customGridView_serverinfo.GetData("ServerInfoView_IP");
            metroTextBox_detail_dn.Text = customGridView_serverinfo.GetData("ServerInfoView_DN");
            metroTextBox_detail_port.Text = customGridView_serverinfo.GetData("ServerInfoView_Port");
            metroTextBox_detail_fps.Text = customGridView_serverinfo.GetData("ServerInfoView_MainFPS");
            metroTextBox_detail_dbfps.Text = customGridView_serverinfo.GetData("ServerInfoView_DBFPS");
            metroCheckBox_detail_live.Checked = customGridView_serverinfo.GetData<bool>("ServerInfoView_Open", false);
        }

        private void metroButton_service_type_Click(object sender, EventArgs e)
        {
            if (metroComboBox_service_type.SelectedItem == null)
                return;

            var type = metroComboBox_service_type.SelectedItem.ToString();
            if (!System.Enum.TryParse(type, out eServerOpenType serviceType))
                return;

            if(CMessageBoxManager.Instance.Question(this, eMessageBoxMessage.ServiceTypeChange, serviceType.ToString()))
                CNetManager.Instance.O2M_ReportChangeServerServiceType(serviceType == eServerOpenType.Open);
        }
    }
}
