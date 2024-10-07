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
    public partial class Panel_System_Notice : PannelBase
    {
        public Panel_System_Notice()
        {
            InitializeComponent();

            customGridView_notice_list.SetHeader<NoticeView>();
            ClearNoticeAddScripts();
            ClearNoticeDetailScripts();
        }

        private void ClearNoticeAddScripts()
        {
            metroTextBox_add_notice_loop.Text = string.Empty;
            metroTextBox_add_notice_msg.Text = string.Empty;
            metroTextBox_add_notice_term.Text = string.Empty;
            dateTimePicker_add_notice_start.Value = DateTime.Now;
            dateTimePicker_add_notice_end.Value = DateTime.Now.AddMinutes(30);
        }

        private void ClearNoticeDetailScripts()
        {
            metroTextBox_detail_notice_id.Text = string.Empty;
            metroTextBox_detail_notice_loop.Text = string.Empty;
            metroTextBox_detail_notice_msg.Text = string.Empty;
            metroTextBox_detail_notice_term.Text = string.Empty;
            dateTimePicker_detail_notice_start.Value = DateTime.Now;
            dateTimePicker_detail_notice_end.Value = DateTime.Now.AddMinutes(30);
        }

        private void metroTextBox_add_notice_loop_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void metroButton_notice_add_Click(object sender, EventArgs e)
        {
            string msg = metroTextBox_add_notice_msg.Text;
            if(string.IsNullOrEmpty(msg))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            int term = int.Parse(metroTextBox_add_notice_loop.Text);
            int loop = int.Parse(metroTextBox_add_notice_term.Text);
            DateTime start = dateTimePicker_add_notice_start.Value.ToUniversalTime();
            DateTime end = dateTimePicker_add_notice_end.Value.ToUniversalTime();

            if(start >= end)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DateCheck);
                return;
            }

            var notice = new _NoticeData();
            notice.m_ID = -1;
            notice.m_Msg = msg;
            notice.m_Term = term;
            notice.m_Loop = loop;
            notice.m_StartDate = start;
            notice.m_EndDate = end;

            CNetManager.Instance.O2M_RequestNoticeUpdate(notice);
        }

        private void button_system_notice_search_Click(object sender, EventArgs e)
        {
            DateTime start = dateTimePicker_system_notice_start.Value.ToUniversalTime();
            DateTime end = dateTimePicker_system_notice_end.Value.ToUniversalTime();

            if(start >= end)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DateCheck);
                return;
            }

            CNetManager.Instance.O2M_RequestNoticeLoad(start, end);
        }

        public void AfterExcuteNoticeLoad(List<_NoticeData> notices)
        {
            var views = new List<NoticeView>();
            foreach (var iter in notices)
            {
                iter.m_StartDate = iter.m_StartDate.ToLocalTime();
                iter.m_EndDate = iter.m_EndDate.ToLocalTime();
                views.Add(new NoticeView(iter));
            }

            customGridView_notice_list.SetData<NoticeView>(views);
        }

        public void AfterExcuteNoticeUpdate(_NoticeData notice)
        {
            CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.Success);

            ClearNoticeAddScripts();

            notice.m_StartDate = notice.m_StartDate.ToLocalTime();
            notice.m_EndDate = notice.m_EndDate.ToLocalTime();

            customGridView_notice_list.AddData<NoticeView>(new NoticeView(notice));
        }

        private void customGridView_notice_list_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            metroTextBox_detail_notice_id.Text = customGridView_notice_list.GetData("NoticeView_UID");
            metroTextBox_detail_notice_msg.Text = customGridView_notice_list.GetData("NoticeView_Msg");
            dateTimePicker_detail_notice_start.Value = customGridView_notice_list.GetData<DateTime>("NoticeView_Start", DateTime.MinValue); ;

            dateTimePicker_detail_notice_end.Value = customGridView_notice_list.GetData<DateTime>("NoticeView_End", DateTime.MinValue);
            metroTextBox_detail_notice_loop.Text = customGridView_notice_list.GetData("NoticeView_Loop");
            metroTextBox_detail_notice_term.Text = customGridView_notice_list.GetData("NoticeView_Term");
        }

        private void metroButton_notice_erase_Click(object sender, EventArgs e)
        {
            long uid = long.Parse(metroTextBox_detail_notice_id.Text);

            customGridView_notice_list.RemoveRow();

            CNetManager.Instance.O2M_RequestNoticeErase(uid);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            long uid = long.Parse(metroTextBox_detail_notice_id.Text);
            string msg = metroTextBox_detail_notice_msg.Text;
            if (string.IsNullOrEmpty(msg))
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DataCheck);
                return;
            }

            int term = int.Parse(metroTextBox_detail_notice_loop.Text);
            int loop = int.Parse(metroTextBox_detail_notice_term.Text);
            DateTime start = dateTimePicker_detail_notice_start.Value.ToUniversalTime();
            DateTime end = dateTimePicker_detail_notice_end.Value.ToUniversalTime();

            if (start >= end)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.DateCheck);
                return;
            }

            if(start <= DateTime.UtcNow)
            {
                CMessageBoxManager.Instance.Info(this, eMessageBoxMessage.AlreadyStart);
                return;
            }

            var notice = new _NoticeData();
            notice.m_ID = uid;
            notice.m_Msg = msg;
            notice.m_Term = term;
            notice.m_Loop = loop;
            notice.m_StartDate = start;
            notice.m_EndDate = end;

            customGridView_notice_list.RemoveRow();
            CNetManager.Instance.O2M_RequestNoticeUpdate(notice);
        }
    }
}
