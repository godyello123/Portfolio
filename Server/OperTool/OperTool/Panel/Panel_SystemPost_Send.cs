using Global;
using MetroFramework.Controls;
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
    public partial class Panel_SystemPost_Send : PannelBase
    {
        public Panel_SystemPost_Send()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            sCombobox_system_post_coin_type.Init(OperHelper.EnumToStringList<CDefine.CoinType>());
            sCombobox_system_post_type.Init(OperHelper.EnumToStringList<CDefine.PostType>());
            sCombobox_system_post_coin_count.Init(OperDefine.RewardMultipleValueStr);
            sCombobox_system_item_count.Init(OperDefine.RewardMultipleValueStr);
            sCombobox_system_item_detail_type.Init(OperHelper.EnumToStringList<CDefine.EItemDetailType>());
            
            sLabel_system_post_type.Init("Type");
            sLabel_system_post_title.Init("Title");
            sLabel_system_post_msg.Init("Msg");
            sLabel_system_post_start.Init("Start");
            sLabel_system_post_expire.Init("Expire");

            sTextBox_system_post_title.Init(false);
            sTextBox_system_post_msg.Init(false);
            sTextBox_system_coin_count.Init(true);

            sDateTimePicker_system_post_start.Init(OperDefine.MaxTime, OperDefine.MinTime);
            sDateTimePicker_system_post_expire.Init(OperDefine.MaxTime, OperDefine.MinTime);

            sButton_system_coin_add.Init("Add");
            sButton_system_post_send.Init("Send");
            sButton_system_post_scripts_clear.Init("Clear");
            sButton_system_item_add.Init("Add");

            sGridView_system_coin.SetHeader<CoinView>();
            sGridView_system_item.SetHeader<ItemView>();


            SystemPostSendButtonEnable();
            ClearPostSendScript();
        }

        private void onClick_system_post_click(object sender, EventArgs e)
        {
            string title = sTextBox_system_post_title.Text;
            string msg = sTextBox_system_post_msg.Text;
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(msg))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            long beginTime = SDateManager.Instance.DateTimeToTimeStamp(sDateTimePicker_system_post_start.Value);
            long expireTime = SDateManager.Instance.DateTimeToTimeStamp(sDateTimePicker_system_post_expire.Value);
            if (beginTime >= expireTime)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            var typestr = sCombobox_system_post_type.GetValue();
            if (!Enum.TryParse(typestr, out CDefine.PostType postType))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            Dictionary<string, _AssetData> merge = new Dictionary<string, _AssetData>();
            int sendCoinRowCount = sGridView_system_coin.RowCount;
            for (int i = 0; i < sendCoinRowCount; i++)
            {
                _AssetData data = new _AssetData();
                data.Type = CDefine.AssetType.Coin;
                data.TableID = sGridView_system_coin.GetData("UserCoinView_Type", i);
                data.Count = sGridView_system_coin.GetData<long>("UserCoinView_Value", i);

                if(merge.TryGetValue(data.TableID, out var mergeData))
                    mergeData.Count += data.Count;
                else
                    merge.Add(data.TableID, data);
            }

            int sendItemRowCount = sGridView_system_item.RowCount;
            for(int i = 0; i < sendItemRowCount; i++)
            {
                _AssetData data = new _AssetData();
                data.Type = CDefine.AssetType.Item;
                data.TableID = sGridView_system_item.GetData("ItemView_ItemTID", i);
                data.Count = sGridView_system_item.GetData<long>("ItemView_Count", i);

                if (merge.TryGetValue(data.TableID, out var mergeData))
                    mergeData.Count += data.Count;
                else
                    merge.Add(data.TableID, data);
            }

            List<_AssetData> rewardList = new List<_AssetData>();
            foreach (var iter in merge)
                rewardList.Add(iter.Value);

            //packet

            if(!OperAccountTable.Instance.IsEditor(FormManager.Instance.MainForm.USER_ID))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.AuthorityError);
                return;
            }

            CNetManager.Instance.O2M_RequestSystemPostSend(-1, postType, title, msg, beginTime, expireTime, rewardList);
        }

        private void SetItemTableIDComboBox(CDefine.EItemDetailType type)
        {
            //todo table
            var itemlist = ItemTable.Instance.GetItemTableIDEntrybyDetailType(type);
            if (itemlist == null)
                return;

            sCombobox_system_item_tableid.SetData(itemlist, itemlist);
        }

        private void onChanged_system_item_detailtype(object sender, EventArgs e)
        {
            string typestr = sCombobox_system_item_detail_type.GetValue();
            if (!Enum.TryParse(typestr, out CDefine.EItemDetailType detailType))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            SetItemTableIDComboBox(detailType);
        }

        private void onClick_system_coin_add(object sender, EventArgs e)
        {
            var value = sCombobox_system_post_coin_type.GetValue();
            if (!Enum.TryParse(value, out CDefine.CoinType correctType))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            int multipleval = sCombobox_system_post_coin_count.GetValue<int>();
            if (multipleval < 1)
                return;

            long coinCount = sTextBox_system_coin_count.GetValue<long>();
            if (coinCount < 0)
                return;

            //add
            sGridView_system_coin.AddData<CoinView>(new CoinView(correctType.ToString(), coinCount * multipleval));
        }

        private void onClick_system_item_add(object sender, EventArgs e)
        {
            string itemTID = sCombobox_system_item_tableid.GetValue();
            var itemRecoord = ItemTable.Instance.Find(itemTID);
            if (itemRecoord == null)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            string multiplevalue = sCombobox_system_item_count.GetValue();
            if (!long.TryParse(multiplevalue, out var multiple))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            if (multiple < 1)
                return;

            long count = sTextBox_system_item_count.GetValue<long>();
            if (count < 0)
                return;

            //add
            sGridView_system_item.AddData<ItemView>(new ItemView(itemTID, count * multiple));
        }

        private void onGridViewDoubleClick_system_coin(object sender, DataGridViewCellEventArgs e)
        {
            sGridView_system_coin.RemoveRow();
        }

        private void onGridViewDoubleClick_system_item(object sender, DataGridViewCellEventArgs e)
        {
            sGridView_system_item.RemoveRow();
        }

        public void AfterExcutePostSend(_PostData postData)
        {
            CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.Success);

            ClearPostSendScript();
            AddSendPostList(postData);
        }

        public void AddSendPostList(_PostData postData)
        {
            customGridView_send_post_list.AddData<SystemPostView>(new SystemPostView(postData));
        }

        private void ClearPostSendScript()
        {
            sCombobox_system_post_type.SelectedIndex = 0;
            sCombobox_system_post_coin_type.SelectedIndex = 0;
            sCombobox_system_item_detail_type.SelectedIndex = 0;
            sCombobox_system_post_coin_count.SelectedIndex = 0;
            sCombobox_system_item_count.SelectedIndex = 0;

            sGridView_system_coin.SetHeader<CoinView>();
            sGridView_system_item.SetHeader<ItemView>();
            customGridView_send_post_list.SetHeader<SystemPostView>();

            sTextBox_system_coin_count.Text = OperDefine.Zero;
            sTextBox_system_item_count.Text = OperDefine.Zero;
            sTextBox_system_post_msg.Text = string.Empty;
            sTextBox_system_post_title.Text = string.Empty;

            sDateTimePicker_system_post_start.Value = DateTime.Now;
            sDateTimePicker_system_post_expire.Value = DateTime.Now.AddDays(7);
        }

        private void SystemPostSendButtonEnable()
        {
            if (!OperAccountTable.Instance.IsEditor(FormManager.Instance.MainForm.USER_ID))
            {
                sButton_system_post_send.Enabled = false;
            }
            else
            {
                sButton_system_post_send.Enabled = true;
            }
        }

        private void metroTextBox_item_count_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void onClick_system_post_scripts_clear(object sender, EventArgs e)
        {
            ClearPostSendScript();
        }

    }
}
