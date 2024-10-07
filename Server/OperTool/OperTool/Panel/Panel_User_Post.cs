using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperTool.Panel
{
    public partial class Panel_User_Post : PannelBase
    {
        STimer m_Timer = new STimer(30 * 1000);
        
        public Panel_User_Post()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            sCombobox_send_coin_count.Init(OperDefine.RewardMultipleValueStr);
            sCombobox_send_item_count.Init(OperDefine.RewardMultipleValueStr);

            m_ActionFunc += UsrePosdLoad;

            sCombobox_send_type.Init(OperHelper.EnumToStringList<CDefine.PostType>());
            sCombobox_send_coin_type.Init(OperHelper.EnumToStringList<CDefine.CoinType>());
            sCombobox_send_item_detail_type.Init(OperHelper.EnumToStringList<CDefine.EItemDetailType>());

            sTextBox_device_id.Init(false);
            sTextBox_name.Init(false);
            sTextBox_uid.Init(true);
            sTextBox_send_title.Init(false);
            sTextBox_send_msg.Init(false);
            sTextBox_send_coin_count.Init(true);
            sTextBox_send_item_count.Init(true);

            sLabel_device_id.Init("DeviceID");
            sLabel_name.Init("Name");
            sLabel_uid.Init("UID");
            sLabel_view_end.Init("End");
            sLabel_view_start.Init("Start");
            sLabel_post_detail_type.Init("Type");
            sLabel_detail_post_id.Init("PostID");
            sLabel_detail_title.Init("Title");
            sLabel_detail_msg.Init("Msg");
            sLabel_detail_coins.Init("Coins");
            sLabel_detail_items.Init("Items");
            sLabel_detail_start.Init("Start");
            sLabel_detail_end.Init("End");
            sLabel_sended_list.Init("SendedList");
            sLabel_send_type.Init("Type");
            sLabel_send_title.Init("Title");
            sLabel_send_expire.Init("Expire");
            sLabel_send_items.Init("Items");
            sLabel_send_coins.Init("Coins");
            sLabel_send_coin_count.Init("Count");
            sLabel_send_msg.Init("Msg");
            sLabel_send_item_detail_type.Init("DetailType");
            sLabel_send_item_tableid.Init("TableID");
            sLabel_send_item_count.Init("Count");


            sButton_post_search.Init("Search");
            sButton_send_item_add.Init("Add");
            sButton_send_coin_add.Init("Add");
            sButton_send.Init("Send");
            sButton_send_clear.Init("Clear");

            sCheckBox_read.Init("Read");
            sCheckBox_reward.Init("Reward");

            sDateTimePicker_view_start.Init(OperDefine.MaxTime, OperDefine.MinTime);
            sDateTimePicker_view_end.Init(OperDefine.MaxTime, OperDefine.MinTime);
            sDateTimePicker_detail_start.Init(OperDefine.MaxTime, OperDefine.MinTime);
            sDateTimePicker_detail_end.Init(OperDefine.MaxTime, OperDefine.MinTime);

            sGridView_post_view.Init<UserPostView>();
            sGridView_detail_coins.Init<CoinView>();
            sGridView_detail_items.Init<ItemView>();

            ClearPostSendScript();
        }

        public void SetUserData(long uid, string deviceID, string name)
        {
            sTextBox_uid.SetText(uid.ToString());
            sTextBox_name.SetText(name);
            sTextBox_device_id.SetText(deviceID);

            sDateTimePicker_view_start.Value = DateTime.Now.AddDays(-7);
            sDateTimePicker_view_end.Value = DateTime.Now;


            ClearPostSendScript();
            sGridView_sended_list.SetHeader<UserPostView>();
        }

        private void ClearPostSendScript()
        {
            sCombobox_send_type.SelectedIndex = 0;
            sCombobox_send_coin_type.SelectedIndex = 0;
            sCombobox_send_item_detail_type.SelectedIndex = 0;
            sCombobox_send_item_count.SelectedIndex = 0;
            sCombobox_send_coin_count.SelectedIndex = 0;

            sGridView_send_coins.SetHeader<CoinView>();
            sGridView_send_items.SetHeader<ItemView>();
            sGridView_sended_list.SetHeader<UserPostView>();

            sTextBox_send_coin_count.Text = string.Empty;
            sTextBox_send_item_count.Text = string.Empty;
            sTextBox_send_title.Text = string.Empty;
            sTextBox_send_msg.Text = string.Empty;

            sDateTimePicker_send_start.Value = DateTime.Now;
            sDateTimePicker_send_expire.Value = DateTime.Now.AddDays(7);
        }

        public void AfterExcutePostLoad(List<_PostData> posts)
        {
            var viewlist = new List<UserPostView>();
            foreach(var iter in posts)
                viewlist.Add(new UserPostView(iter));

            sGridView_post_view.AddData<UserPostView>(viewlist);
        }

        private void UsrePosdLoad()
        {
            long uid = sTextBox_uid.GetValue<long>();
            string deviceID = sTextBox_device_id.GetValue();
            string name = sTextBox_name.GetValue();

            if (string.IsNullOrEmpty(deviceID) || string.IsNullOrEmpty(name))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DataCheck);
                return;
            }

            long start = SDateManager.Instance.DateTimeToTimeStamp(sDateTimePicker_view_start.Value);
            long end = SDateManager.Instance.DateTimeToTimeStamp(sDateTimePicker_view_end.Value);

            if (start >= end)
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DateCheck);
                return;
            }

            sGridView_post_view.SetHeader<UserPostView>();

            CNetManager.Instance.O2M_RequestUserPostLoad(uid, start, end);
        }

        public void AfterExcutePostSend(_PostData post)
        {
            CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.Success);

            sGridView_sended_list.AddData<UserPostView>(new UserPostView(post));
        }

        private void customGridView_user_post_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            sTextBox_detail_type.Text = sGridView_post_view.GetData("UserPostView_Type");
            sTextBox_detail_post_id.Text = sGridView_post_view.GetData("UserPostView_ID");
            sTextBox_detail_title.Text = sGridView_post_view.GetData("UserPostView_Title");
            sTextBox_detail_msg.Text = sGridView_post_view.GetData("UserPostView_Msg");
            
            sCheckBox_read.Checked = sGridView_post_view.GetData<bool>("UserPostView_IsRead", false);
            sCheckBox_reward.Checked = sGridView_post_view.GetData<bool>("UserPostView_IsRewarded", false);

            sDateTimePicker_detail_start.Value = sGridView_post_view.GetData<DateTime>("UserPostView_BeginTime" , DateTime.MinValue);
            sDateTimePicker_detail_end.Value = sGridView_post_view.GetData<DateTime>("UserPostView_ExpireTime", DateTime.MinValue);

            var rewards = new List<_AssetData>();
            string jsonStr = sGridView_post_view.GetData("UserPostView_Reward");
            if (!SJson.IsValidJson(jsonStr))
                return;

            rewards = SJson.JsonToObject<List<_AssetData>>(jsonStr);

            List<CoinView> coinviews = new List<CoinView>();
            List<ItemView> itmeviews = new List<ItemView>();
            foreach(var iter in rewards)
            {
                if (iter.Type == CDefine.AssetType.Coin)
                    coinviews.Add(new CoinView(iter));
                else if (iter.Type == CDefine.AssetType.Item)
                    itmeviews.Add(new ItemView(iter.TableID, iter.Count));
            }

            sGridView_detail_coins.SetData<CoinView>(coinviews);
            sGridView_detail_items.SetData<ItemView>(itmeviews);
        }

        private void metroButton_send_coin_add_Click(object sender, EventArgs e)
        {
            CDefine.CoinType correctType = sCombobox_send_coin_type.GetValue<CDefine.CoinType>();
            if (correctType == CDefine.CoinType.Max)
                return;

            long coinCount = sTextBox_send_coin_count.GetValue<long>();
            if (coinCount < 1)
                return;

            //add
            sGridView_send_coins.AddData<CoinView>(new CoinView(correctType.ToString(), coinCount));
        }

        private void metroButton_send_item_add_Click(object sender, EventArgs e)
        {
            string itemTID = sCombobox_item_table_id.GetValue();;
            var itemRecoord = ItemTable.Instance.Find(itemTID);
            if (itemRecoord == null)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            long itemCount = sTextBox_send_item_count.GetValue<int>();
            if (itemCount < 1)
                return;

            //add
            sGridView_send_items.AddData<ItemView>(new ItemView(itemTID, itemCount));
        }

        private void metroButton_send_Click(object sender, EventArgs e)
        {
            long userUID = sTextBox_uid.GetValue<long>();
            if(userUID == default(long))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.UserIDCheck);
                return;
            }
            
            string title = sTextBox_send_title.GetValue();
            string msg = sTextBox_send_msg.GetValue();
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(msg))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            long beginTime = SDateManager.Instance.DateTimeToTimeStamp(sDateTimePicker_send_start.Value);
            long expireTime = SDateManager.Instance.DateTimeToTimeStamp(sDateTimePicker_send_expire.Value);
            if (beginTime >= expireTime)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            CDefine.PostType posttype = sCombobox_send_type.GetValue<CDefine.PostType>();
            if(posttype == CDefine.PostType.Max)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            Dictionary<string, _AssetData> merge = new Dictionary<string, _AssetData>();
            int sendCoinRowCount = sGridView_send_coins.RowCount;
            for (int i = 0; i < sendCoinRowCount; i++)
            {
                _AssetData data = new _AssetData();
                data.Type = CDefine.AssetType.Coin;
                data.TableID = sGridView_send_coins.GetData("UserCoinView_Type", i);
                data.Count = sGridView_send_coins.GetData<long>("UserCoinView_Value", i);

                if (merge.TryGetValue(data.TableID, out var mergeData))
                    mergeData.Count += data.Count;
                else
                    merge.Add(data.TableID, data);
            }

            int sendItemRowCount = sGridView_send_items.RowCount;
            for (int i = 0; i < sendItemRowCount; i++)
            {
                _AssetData data = new _AssetData();
                data.Type = CDefine.AssetType.Item;
                data.TableID = sGridView_send_items.GetData("ItemView_ItemTID", i);
                data.Count = sGridView_send_items.GetData<long>("ItemView_Count", i);

                if (merge.TryGetValue(data.TableID, out var mergeData))
                    mergeData.Count += data.Count;
                else
                    merge.Add(data.TableID, data);
            }

            List<_AssetData> rewardList = new List<_AssetData>();
            foreach (var iter in merge)
                rewardList.Add(iter.Value);

            if (!OperAccountTable.Instance.IsEditor(FormManager.Instance.MainForm.USER_ID))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.AuthorityError);
                return;
            }

            if (!CMessageBoxManager.Instance.Question(this, eMessageBoxMessage.PostCheck))
                return;

            CNetManager.Instance.O2M_RequestUserPostSend(userUID, OperDefine.GeneraterGUID(), posttype, title, msg, beginTime, expireTime, rewardList);
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            ClearPostSendScript();
        }

        private void OnUserPostKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void metroComboBox_send_item_detail_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            CDefine.EItemDetailType detailType = sCombobox_send_item_detail_type.GetValue<CDefine.EItemDetailType>();
            if (detailType == CDefine.EItemDetailType.Max)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }
            
            SetItemTableIDComboBox(detailType);
        }

        private void SetItemTableIDComboBox(CDefine.EItemDetailType type)
        {
            //todo table
            var itemlist = ItemTable.Instance.GetItemTableIDEntrybyDetailType(type);
            if (itemlist == null)
                return;

            //ombobox_item_table_id.Init()
        }

        private void OnVisibie(object sender, EventArgs e)
        {
            if (this.Visible)
                metroTabControl_user_pos.SelectedIndex = 0;
        }

        private void sButton_post_search_Click(object sender, EventArgs e)
        {
            if (!m_Timer.Check())
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.Retry);
                return;
            }

            UsrePosdLoad();
        }
    }
}
