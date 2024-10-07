using Amazon.Runtime.Internal.Util;
using Global;
using MongoDB.Driver.Linq;
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
    public partial class Panel_User_Infomation : PannelBase
    {
        public Panel_User_Infomation()
        {
            InitializeComponent();
        }

        public eUserSearchType m_SearchType = eUserSearchType.Max;


        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            customGridView_user_coin.SetHeader<CoinView>();
            customGridView_user_receipt.SetHeader<UserIAPReceiptView>();
            customGridView_user_stage.SetHeader<UserStageView>();

            sCombobox_user_search_type.Init(OperHelper.EnumToStringList<eUserSearchType>());
            sCombobox_block_period.Init(OperHelper.EnumToStringList<eBlockPeriod>());


            sCheckBox_user_live.Init("Live");
            sCheckBox_user_block.Init("Block");
            sCheckBox_user_white.Init("White");

            sTextBox_search_str.Init(false);
            sTextBox_user_deviceid.Init(false);
            sTextBox_user_uid.Init(true);
            sTextBox_user_name.Init(false);
            sTextBox_user_lv.Init(true);
            sTextBox_user_lv_point.Init(true);
            sTextBox_user_logintime.Init(false);
            sTextBox_user_logout_time.Init(false);
            sTextBox_block_period.Init(true);

            sLabel_user_deviceid.Init("DeviceID");
            sLabel_user_uid.Init("UID");
            sLabel_user_name.Init("Name");
            sLabel_user_lv.Init("Lv");
            sLabel_user_lv_point.Init("LvPoint");
            sLabel_user_login_time.Init("LoginTime");
            sLabel_user_logouttime.Init("LogoutTime");

            sButton_user_search.Init("Search");
            sButton_user_block_release.Init("BlockRelease");
            sButton_user_white_release.Init("WhiteRelease");
            sButton_user_kick.Init("Kick");
            sButton_user_item.Init("Item");
            sButton_user_growth.Init("Growth");
            sButton_user_post.Init("Post");
            sButton_user_contents.Init("Contents");
            sButton_user_block_register.Init("Register");
            sButton_user_white_register.Init("Register");

            sButton_user_kick.Enabled = false;

        }

        private void onClick_UserSearchButton(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(sTextBox_search_str.Text))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DataCheck);
                return;
            }

            switch (m_SearchType)
            {
                case eUserSearchType.DeviceID:
                    CNetManager.Instance.O2M_RequestSearchUser(sTextBox_search_str.Text, "", -1);
                    break;
                case eUserSearchType.UID:
                    {
                        long uid = -1;
                        if (!long.TryParse(sTextBox_search_str.Text, out uid))
                        {
                            CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DataCheck);
                            return;
                        }

                        CNetManager.Instance.O2M_RequestSearchUser("", "", uid);
                    }
                    break;
                case eUserSearchType.Name:
                    CNetManager.Instance.O2M_RequestSearchUser("", sTextBox_search_str.Text, -1);
                    break;
                default:
                    break;
            }

        }

        //contents panel
        private void onClick_UserContentsButton(object sender, EventArgs e)
        {
            if (PanelManager.Instance.ChangePanel(ePanelType.USER_CONTENTS) is Panel_User_Contents panel)
            {
                if (panel == null)
                    return;

                long user_uid = -1;
                long.TryParse(sTextBox_user_uid.Text, out user_uid);
                panel.SetUserData(user_uid, sTextBox_user_deviceid.Text, sTextBox_user_name.Text);
                panel.m_ActionFunc.Invoke();
            }
        }

        //user item panel
        private void onClick_UserItemButton(object sender, EventArgs e)
        {
            PanelManager.Instance.ChangePanel(ePanelType.USER_ITEM);
        }

        //user post panel
        private void onClick_UserPostButton(object sender, EventArgs e)
        {
            if (PanelManager.Instance.ChangePanel(ePanelType.USER_POST) is Panel_User_Post panel)
            {
                if (panel == null)
                    return;

                long user_uid = -1;
                long.TryParse(sTextBox_user_uid.Text, out user_uid);
                panel.SetUserData(user_uid, sTextBox_user_deviceid.Text, sTextBox_user_name.Text);
                panel.m_ActionFunc.Invoke();
            }
        }

        private void onSelected_UserSearchType(object sender, EventArgs e)
        {
            sTextBox_search_str.Clear();

            string value = sCombobox_user_search_type.GetValue();
            if (!Enum.TryParse(value, out m_SearchType))
                return;
        }

        private void onKeyPress_OnlyNumber(object sender, KeyPressEventArgs e)
        {
            if (m_SearchType == eUserSearchType.UID)
            {
                if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
                {
                    e.Handled = true;
                }
            }
        }

        private void onClick_UserBlocKRegister(object sender, EventArgs e)
        {
            if (sCombobox_block_period.SelectedItem == null)
                return;

            var value = sCombobox_block_period.GetValue();
            if (!Enum.TryParse(value, out eBlockPeriod type))
                return;

            if (string.IsNullOrEmpty(sTextBox_block_period.Text))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            int blockVal = int.Parse(sTextBox_block_period.Text);
            if (blockVal < 1)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            string deviceID = sTextBox_user_deviceid.Text;
            if (string.IsNullOrEmpty(deviceID))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            long expire_time = 0;
            if (type == eBlockPeriod.Day)
                expire_time = SDateManager.Instance.DateTimeToTimeStamp(DateTime.UtcNow.AddDays(blockVal));
            else if (type == eBlockPeriod.Week)
                expire_time = SDateManager.Instance.DateTimeToTimeStamp(DateTime.UtcNow.AddWeeks(blockVal));
            else if (type == eBlockPeriod.Monty)
                expire_time = SDateManager.Instance.DateTimeToTimeStamp(DateTime.UtcNow.AddMonths(blockVal));
            else if (type == eBlockPeriod.Year)
                expire_time = SDateManager.Instance.DateTimeToTimeStamp(DateTime.UtcNow.AddYears(blockVal));
            else if (type == eBlockPeriod.Permanent)
                expire_time = SDateManager.Instance.DateTimeToTimeStamp(CDefine.MaxTime);

            List<_BlockUser> sendList = new List<_BlockUser>() { new _BlockUser(deviceID, 1, expire_time) };

            CNetManager.Instance.O2M_ReprortBlockUserUpsert(sendList);
        }

        private void onClick_BlcokReleaseButton(object sender, EventArgs e)
        {
            string deviceID = sTextBox_user_deviceid.Text;
            if (string.IsNullOrEmpty(deviceID))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DataCheck);
                return;
            }

            if (!sCheckBox_user_block.Checked)
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.IsNotBlock);
                return;
            }

            CNetManager.Instance.O2M_ReportBlockUserDelete(deviceID);

            sCheckBox_user_block.Checked = !sCheckBox_user_block.Checked;
        }

        private void onClick_WhiteReleaseButton(object sender, EventArgs e)
        {
            string deviceID = sTextBox_user_deviceid.Text;
            if (string.IsNullOrEmpty(deviceID))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DataCheck);
                return;
            }

            if (!sCheckBox_user_white.Checked)
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.IsNotWhite);
                return;
            }

            CNetManager.Instance.O2M_ReportWhiteUserDelete(deviceID);

            sCheckBox_user_white.Checked = !sCheckBox_user_white.Checked;
        }

        private void onClick_UserWhiteRegister(object sender, EventArgs e)
        {
            //register white
            string deviceID = sTextBox_user_deviceid.Text;
            if (string.IsNullOrEmpty(deviceID))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DataCheck);
                return;
            }

            if (sCheckBox_user_white.Checked)
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.IsWhite);
                return;
            }

            CNetManager.Instance.O2M_ReportWhiteUserInsert(deviceID);
        }

        private void onClick_UserKickButton(object sender, EventArgs e)
        {
            string deviceID = sTextBox_user_deviceid.Text;
            if (string.IsNullOrEmpty(deviceID))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DataCheck);
                return;
            }

            CNetManager.Instance.O2M_ReportUserKick(deviceID);
        }

        private void onClick_UserGrowthButton(object sender, EventArgs e)
        {
            //todo : user Growth

        }

        public void SetUserInfoData(_UserData userData, List<_GachaData> gachaDatas, List<_AssetData> assetDatas, List<_StageData> stageDatas, List<_ReceiptData> receiptDatas,
            bool isblock, bool iswhite)
        {
            sTextBox_user_deviceid.Text = userData.m_DeviceID;
            sTextBox_user_uid.Text = userData.m_UID.ToString();
            sTextBox_user_name.Text = userData.m_Name;
            sTextBox_user_lv.Text = userData.m_Level.ToString();
            sTextBox_user_lv_point.Text = userData.m_LevelPoint.ToString();
            sTextBox_user_logintime.Text = SDateManager.Instance.TimeStampToLocalTime(userData.m_LoginTime).ToString();
            sTextBox_user_logout_time.Text = SDateManager.Instance.TimeStampToLocalTime(userData.m_LogoutTime).ToString();

            sCheckBox_user_block.Checked = isblock;
            sCheckBox_user_white.Checked = iswhite;

            
            var coinviews = new List<CoinView>();
            foreach (var iter in assetDatas) coinviews.Add(new CoinView(iter));
            customGridView_user_coin.SetData<CoinView>(coinviews);

            var stageviews = new List<UserStageView>();
            foreach (var iter in stageDatas) stageviews.Add(new UserStageView(iter));
            customGridView_user_stage.SetData<UserStageView>(stageviews);


            long total_price = 0;
            var receiptviews = new List<UserIAPReceiptView>();
            foreach (var iter in receiptDatas)
            {
                receiptviews.Add(new UserIAPReceiptView(iter));
                total_price += iter.m_Price;
            }
            customGridView_user_receipt.SetData<UserIAPReceiptView>(receiptviews);

            metroTextBox_iap_total.Text = total_price.ToString();

            if (userData.m_LoginTime >= userData.m_LogoutTime)
            {
                sButton_user_kick.Enabled = true;
                sCheckBox_user_live.Checked = true;
            }
            else
            {
                sButton_user_kick.Enabled = false;
                sCheckBox_user_live.Checked = false;
            }
        }

      
    }
}
