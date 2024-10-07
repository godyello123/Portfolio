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
    public partial class Panel_SystemPost : PannelBase
    {
        private STimer m_Timer = new STimer(30 * 1000);

        public Panel_SystemPost()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            customGridView_post.SetHeader<SystemPostView>();
            customGridView_detail_items.SetHeader<ItemView>();
            customGridView_detail_coins.SetHeader<CoinView>();

            this.VisibleChanged += new System.EventHandler(this.OnVisible);

            DateTime now = DateTime.Now;
            
            sLabel_system_post_start.Init("Start");
            sLabel_system_post_end.Init("End");
            sLabel_detail_post_id.Init("GUID");
            sLabel_detail_system_post_title.Init("Title");
            sLabel_detail_system_post_msg.Init("Msg");

            sDateTimePicker_system_post_start.Init(OperDefine.MaxTime, OperDefine.MinTime);
            sDateTimePicker_system_post_start.Value = DateTime.Now.AddDays(-7);
            sDateTimePicker_system_post_end.Init(OperDefine.MaxTime, OperDefine.MinTime);
            sDateTimePicker_system_post_end.Value = DateTime.Now;

            sTextBox_detail_system_post_id.Init(false);
            sTextBox_detail_system_post_id.ReadOnly = true;
            sTextBox_detail_system_post_title.Init(false);
            sTextBox_detail_system_post_title.ReadOnly = true;
            sTextBox_detail_system_post_msg.Init(false);
            sTextBox_detail_system_post_msg.ReadOnly = true;

        }

        private void SystemPostLoad()
        {
            var beginDate = sDateTimePicker_system_post_start.Value;
            var endDate = sDateTimePicker_system_post_end.Value;

            if (beginDate >= endDate)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DateCheck);
                return;
            }

            ClearPostDetailData();
            CNetManager.Instance.O2M_RequestSystemPostLoad(SDateManager.Instance.DateTimeToTimeStamp(beginDate), SDateManager.Instance.DateTimeToTimeStamp(endDate));
        }

        private void metroTile_system_post_send_Click(object sender, EventArgs e)
        {
            //todo sendpannel
        }

        private void onClick_system_post_search(object sender, EventArgs e)
        {
            if(!m_Timer.Check())
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.Retry);
                return;
            }

            SystemPostLoad();
        }

        private void OnVisible(object sender, EventArgs e)
        {
            if (!m_Timer.Check())
                return;

            if(this.Visible == true)
                SystemPostLoad();
        }

        public void SetSystemPostData(List<_PostData> systemPosts)
        {
            var viewList = new List<SystemPostView>();
            foreach(var iter in systemPosts)
                viewList.Add(new SystemPostView(iter));

            customGridView_post.SetHeader<SystemPostView>();
        }

        private void metroGrid_post_DoubleClick(object sender, EventArgs e)
        {
            sTextBox_detail_system_post_id.Text = customGridView_post.GetData("SystemPostView_PostID");
            sTextBox_detail_system_post_title.Text = customGridView_post.GetData("SystemPostView_PostTitle");
            sTextBox_detail_system_post_msg.Text = customGridView_post.GetData("SystemPostView_PostMsg");
            metroTextBox_post_detail_start.Text = customGridView_post.GetData("SystemPostView_PostBeginTime");
            metroTextBox_post_detail_expire.Text = customGridView_post.GetData("SystemPostView_PostExpireTime");

            string rewardJson = customGridView_post.GetData("SystemPostView_PostRewards");
            List<_AssetData> rewards = SJson.JsonToObject<List<_AssetData>>(rewardJson);

            List<CoinView> coinviews = new List<CoinView>();
            List<ItemView> itemviews = new List<ItemView>();

            foreach(var iter in rewards)
            {
                if (iter.Type == CDefine.AssetType.Coin)
                    coinviews.Add(new CoinView(iter));
                else
                    itemviews.Add(new ItemView(iter.TableID, iter.Count));
            }

            customGridView_detail_coins.SetData<CoinView>(coinviews);
            customGridView_detail_items.SetData<ItemView>(itemviews);
        }

        private void ClearPostDetailData()
        {
            sTextBox_detail_system_post_id.Text = string.Empty;
            sTextBox_detail_system_post_title.Text = string.Empty;
            sTextBox_detail_system_post_msg.Text = string.Empty;
            metroTextBox_post_detail_start.Text = string.Empty;
            metroTextBox_post_detail_expire.Text = string.Empty;

            customGridView_detail_coins.SetHeader<CoinView>();
            customGridView_detail_items.SetHeader<ItemView>();
        }

        private void customGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}
