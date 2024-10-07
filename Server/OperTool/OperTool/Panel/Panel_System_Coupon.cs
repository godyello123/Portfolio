using Global;
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
    public partial class Panel_System_Coupon : PannelBase
    {
        STimer m_Timer = new STimer(30 * 1000);

        public Panel_System_Coupon()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            customGridView_coupon_list.SetHeader<CouponView>();
            dateTimePicker_coupon_start.Value = DateTime.Now.AddDays(-7);
            dateTimePicker_coupon_end.Value = DateTime.Now;

            for (CDefine.CoinType i = CDefine.CoinType.BlueDia; i < CDefine.CoinType.Max; ++i)
                metroComboBox_coin_type.Items.Add(i.ToString());

            for (CDefine.EItemDetailType i = CDefine.EItemDetailType.Weapon; i < CDefine.EItemDetailType.Max; ++i)
                metroComboBox_send_item_detail_type.Items.Add(i.ToString());

            foreach (var iter in OperDefine.RewardMultipleValue)
            {
                metroComboBox_coin_count.Items.Add(iter.ToString());
                metroComboBox_item_count.Items.Add(iter.ToString());
            }

            ClearCouponCreateScripts();

            customGridView_send_coins.SetHeader<CoinView>();
            customGridView_send_items.SetHeader<ItemView>();
        }

        private void ClearCouponCreateScripts()
        {
            metroComboBox_coin_type.SelectedIndex = 0;
            metroComboBox_send_item_detail_type.SelectedIndex = 0;

            metroTextBox_send_couponId.Text = string.Empty;
            metroTextBox_send_coupon_cnt.Text = string.Empty;
            metroTextBox_send_coupon_lv.Text = string.Empty;
            metroTextBox_send_item_count.Text = string.Empty;
            metroTextBox_send_coin_count.Text = string.Empty;

            //SetItemTableIDComboBox();
        }

        private void button_coupon_search_Click(object sender, EventArgs e)
        {
            if(!m_Timer.Check())
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.Retry);
                return;
            }

            SystemCouponLoad();
        }

        public void AfterExcuteCouponLoad(List<_CouponData> coupons)
        {
            var views = new List<CouponView>();
            foreach (var iter in coupons)
                views.Add(new CouponView(iter));

            customGridView_coupon_list.SetData<CouponView>(views);
        }

        public void AfterExcuteCouponCreate(_CouponData coupon)
        {
            ClearCouponCreateScripts();

            CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.Success);

            customGridView_coupon_list.AddData<CouponView>(new CouponView(coupon));
        }

        private void metroButton_detail_delete_Click(object sender, EventArgs e)
        {
            //todo : coupon remove
        }

        private void metroLabel10_Click(object sender, EventArgs e)
        {

        }

        private void metroButton_send_coin_add_Click(object sender, EventArgs e)
        {
            if (!Enum.TryParse(metroComboBox_coin_type.SelectedItem.ToString(), out CDefine.CoinType correctType))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            if (!long.TryParse(metroTextBox_send_coin_count.Text, out var coinCount))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            if (coinCount < 1)
                return;

            //add
            customGridView_send_coins.AddData<CoinView>(new CoinView(correctType.ToString(), coinCount));
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(metroTextBox_send_couponId.Text) || 
                string.IsNullOrEmpty(metroTextBox_send_coupon_cnt.Text) ||
                string.IsNullOrEmpty(metroTextBox_send_coupon_lv.Text))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DataCheck);
                return;
            }

            string couponID = metroTextBox_send_couponId.Text;
            int count = int.Parse(metroTextBox_send_coupon_cnt.Text);
            int lv = int.Parse(metroTextBox_send_coupon_lv.Text);

            long beginTime = SDateManager.Instance.DateTimeToTimeStamp(dateTimePicker_send_coupon_begin.Value);
            long expireTime = SDateManager.Instance.DateTimeToTimeStamp(dateTimePicker_send_coupon_expire.Value);
            if (beginTime >= expireTime)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            Dictionary<string, _AssetData> merge = new Dictionary<string, _AssetData>();
            int sendCoinRowCount = customGridView_send_coins.RowCount;
            for (int i = 0; i < sendCoinRowCount; i++)
            {
                _AssetData data = new _AssetData();
                data.Type = CDefine.AssetType.Coin;
                data.TableID = customGridView_send_coins.GetData("UserCoinView_Type", i);
                data.Count = customGridView_send_coins.GetData<long>("UserCoinView_Value", i);

                if (merge.TryGetValue(data.TableID, out var mergeData))
                    mergeData.Count += data.Count;
                else
                    merge.Add(data.TableID, data);
            }

            int sendItemRowCount = customGridView_send_items.RowCount;
            for (int i = 0; i < sendItemRowCount; i++)
            {
                _AssetData data = new _AssetData();
                data.Type = CDefine.AssetType.Item;
                data.TableID = customGridView_send_items.GetData("ItemView_ItemTID", i);
                data.Count = customGridView_send_items.GetData<long>("ItemView_Count", i);

                if (merge.TryGetValue(data.TableID, out var mergeData))
                    mergeData.Count += data.Count;
                else
                    merge.Add(data.TableID, data);
            }

            List<_AssetData> rewardList = new List<_AssetData>();
            foreach (var iter in merge)
                rewardList.Add(iter.Value);


            CNetManager.Instance.O2M_RequestCouponCreate(couponID, count, lv, beginTime, expireTime, rewardList);
        }

        private void metroButton_send_item_add_Click(object sender, EventArgs e)
        {
            //todo list
            if (metroComboBox_send_item_table_id.SelectedItem == null)
                return;

            string itemTID = metroComboBox_send_item_table_id.SelectedItem.ToString();
            var itemRecoord = ItemTable.Instance.Find(itemTID);
            if (itemRecoord == null)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            if (!long.TryParse(metroTextBox_send_item_count.Text, out var itemCount))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            if (itemCount < 1)
                return;

            customGridView_send_items.AddData<ItemView>(new ItemView(itemTID, itemCount));
        }

        private void onKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void customGridView_coupon_list_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            metroTextBox_detail_couponID.Text = customGridView_coupon_list.GetData("CouponView_ID");
            metroTextBox_detail_count.Text = customGridView_coupon_list.GetData("CouponView_Count");
            metroTextBox_detail_user_level.Text = customGridView_coupon_list.GetData("CouponView_UseLevel");
            dateTimePicker_detail_begin.Value = customGridView_coupon_list.GetData<DateTime>("CouponView_Begin", DateTime.MinValue);
            dateTimePicker_detail_expire.Value = customGridView_coupon_list.GetData<DateTime>("CouponView_Expire", DateTime.MinValue);

            string json = customGridView_coupon_list.GetData("CouponView_Rewards");
            var rewards = new List<_AssetData>();
            if (SJson.IsValidJson(json))
                rewards = SJson.JsonToObject<List<_AssetData>>(json);

            var coinViews = new List<CoinView>();
            var itemViews = new List<ItemView>();
            foreach(var iter in rewards)
            {
                if(iter.Type == CDefine.AssetType.Coin)
                    coinViews.Add(new CoinView(iter));
                else
                    itemViews.Add(new ItemView(iter.TableID, iter.Count));
            }

            customGridView_detail_coin.SetData<CoinView>(coinViews);
            customGridView_detail_item.SetData<ItemView>(itemViews);
        }

        private void metroComboBox_send_item_detail_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            var typestr = metroComboBox_send_item_detail_type.SelectedItem;
            if (!Enum.TryParse(typestr.ToString(), out CDefine.EItemDetailType detailType))
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

            foreach (var iter in itemlist)
                metroComboBox_send_item_table_id.Items.Add(iter);
        }

        private void SystemCouponLoad()
        {
            var beginDate = dateTimePicker_coupon_start.Value;
            var endDate = dateTimePicker_coupon_end.Value;

            if (beginDate >= endDate)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DateCheck);
                return;
            }

            ClearCouponDetailData();
            CNetManager.Instance.O2M_RequestSystemPostLoad(SDateManager.Instance.DateTimeToTimeStamp(beginDate), SDateManager.Instance.DateTimeToTimeStamp(endDate));
        }

        private void ClearCouponDetailData()
        {
            metroTextBox_detail_count.Text = string.Empty;
            metroTextBox_detail_couponID.Text = string.Empty;
            metroTextBox_detail_user_level.Text = string.Empty;
            dateTimePicker_detail_begin.Value = DateTime.Now;
            dateTimePicker_detail_expire.Value = DateTime.Now;

            customGridView_detail_coin.SetHeader<CoinView>();
            customGridView_detail_item.SetHeader<ItemView>();
        }

        private void OnVisible(object sender, EventArgs e)
        {
            if (!m_Timer.Check())
                return;

            if (this.Visible == true)
                SystemCouponLoad();
        }
    }
}
