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

namespace OperTool.Panel
{
    public partial class Panel_User_Contents : PannelBase
    {
        public Panel_User_Contents()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            customGridView_user_growthgold.SetHeader<GrowthLevelView>();
            customGridView_user_growth_level.SetHeader<GrowthLevelView>();

            sLabel_user_uid.Init("UID");
            sLabel_user_deviceID.Init("DeviceID");
            sLabel_user_name.Init("Name");

            sTextBox_user_uid.Init(true);
            sTextBox_user_deviceid.Init(false);
            sTextBox_user_name.Init(false);
        }

        private void OnVisible(object sender, EventArgs e) 
        {
            if (this.Visible)
                contents_tab.SelectedIndex = 0;
        }

        public void SetUserData(long uid, string deviceID, string name)
        {
            sTextBox_user_deviceid.Text = deviceID;
            sTextBox_user_uid.Text = uid.ToString();
            sTextBox_user_name.Text = name;
        }

        private void ContentsDataLoad()
        {
            if (contents_tab.SelectedTab == null)
                return;

            long uid = -1;
            if(string.IsNullOrEmpty(sTextBox_user_uid.Text) || long.TryParse(sTextBox_user_uid.Text, out uid))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.UserIDCheck);
                return;
            }

            string tapName = contents_tab.SelectedTab.Name;
            switch (tapName) 
            {
                case "tab_GrowthGold": CNetManager.Instance.O2M_RequestUserGrowthGoldLoad(uid); break;
                case "tab_GrowthLevel": CNetManager.Instance.O2M_RequestUserGrowthLevelLoad(uid); break;
                case "tab_Quest": CNetManager.Instance.O2M_RequestUserQuestLoad(uid); break;
                case "tab_Pass": CNetManager.Instance.O2M_RequestUserPassLoad(uid); break;
                case "tab_Skill": CNetManager.Instance.O2M_RequestUserSkillLoad(uid); break;
                case "tab_Shop": CNetManager.Instance.O2M_RequestUserShopLoad(uid); break;
                case "tab_Relic": CNetManager.Instance.O2M_RequestUserRelicLoad(uid); break;
                case "tab_Gacha": CNetManager.Instance.O2M_RequestUserGachaLoad(uid); break;
            }
        }

        private void contents_tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContentsDataLoad();
        }

        public void AfterExcuteGrowthLevelLoad(List<_LevelData> datas)
        {

        }

        public void AfterExcuteGrowthGoldLoad(List<_LevelData> datas)
        {

        }
    }
}
