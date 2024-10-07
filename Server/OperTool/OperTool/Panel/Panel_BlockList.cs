using Global;
using Packet_O2M;
using SCommon;
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
    public partial class Panel_BlockList : PannelBase
    {
        private STimer m_Timer = new STimer(30 * 1000);

        public Panel_BlockList()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            metroComboBox_search_block_user_type.Items.Clear();
            for (eUserSearchType i = eUserSearchType.DeviceID; i < eUserSearchType.Max; ++i)
                metroComboBox_search_block_user_type.Items.Add(i.ToString());

            metroComboBox_block_period.Items.Clear();
            for (eBlockPeriod i = eBlockPeriod.Day; i < eBlockPeriod.Max; i++)
                metroComboBox_block_period.Items.Add(i.ToString());

            customGridView_block_user.SetHeader<BlockView>();
            
            customGridView_add_block.SetHeader<UserAccountView>();
            customGridView_add_block.AllowUserToAddRows = true;

            metroComboBox_search_block_user_type.SelectedIndex = 0;
            metroComboBox_block_period.SelectedIndex = 0;

            SetLabelText();
        }

        

        private void SetLabelText()
        {
            sLabel_total.Text = "Total";
            sLabel_period.Text = "Period";
        }

        private void metroButton_block_refresh_Click(object sender, EventArgs e)
        {
            if(!m_Timer.Check())
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.Retry);
                return;
            }

            metroTextBox_block_list_total.Text = string.Empty;
            customGridView_block_user.SetHeader<BlockView>();

            CNetManager.Instance.O2M_RequestBlockUserLoad();
        }

        public void AfterExcuteBlockUserLoad(List<_BlockUser> blocks)
        {
            var views = new List<BlockView>();
            foreach (var iter in blocks)
                views.Add(new BlockView(iter));

            customGridView_block_user.SetData<BlockView>(views);

            int cnt = blocks.Count;
            metroTextBox_block_list_total.Text = cnt.ToString();
        }

        //find
        private void metroButton_block_user_search_Click(object sender, EventArgs e)
        {
            if (metroComboBox_search_block_user_type.SelectedItem == null)
                return;

            var value = metroComboBox_search_block_user_type.SelectedItem.ToString();
            if (!Enum.TryParse(value, out eUserSearchType type))
                return;

            if (string.IsNullOrEmpty(metroTextBox_search_block_user.Text))
            {
                return;
            }

            int findRow = -1;
            if(type == eUserSearchType.UID)
            {
                long uid = 0;
                if (!long.TryParse(metroTextBox_search_block_user.Text, out uid))
                    return;

                //todo : uid add
            }
            else if(type == eUserSearchType.DeviceID)
            {
                findRow = customGridView_block_user.FindRow("BlockView_DeviceID", metroTextBox_search_block_user.Text);
            }
            else if(type == eUserSearchType.Name)
            {
                //todo : name add
            }

            if(findRow != -1)
            {
                metroTextBox_detail_deviceid.Text = customGridView_block_user.GetData("BlockView_DeviceID", string.Empty);
                metroTextBox_detail_block_cnt.Text = customGridView_block_user.GetData("BlockView_Count", 0);
                metroTextBox_detail_expire_time.Text = customGridView_block_user.GetData("BlockView_ExpTime", string.Empty);
                metroCheckBox_detail_blocked.Checked = customGridView_block_user.GetData<bool>("BlockView_IsBlocked", false);
            }
        }

        private void metroButton_add_block_clear_Click(object sender, EventArgs e)
        {
            customGridView_add_block.SetHeader<UserAccountView>();
        }

        private void metroButton_add_block_Click(object sender, EventArgs e)
        {
            //todo : export
            //customGridView_add_block
        }

        private void metroButton_block_user_export_Click(object sender, EventArgs e)
        {
            //todo : export
            //customGridView_block_user
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            //apply

        }
    }
}
