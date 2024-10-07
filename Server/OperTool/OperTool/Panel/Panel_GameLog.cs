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
using SDB;
using SCommon;
using OperTool.Controls;

namespace OperTool.Panel
{
    public partial class Panel_GameLog : PannelBase
    {
        private HashSet<eLogType> m_FilterLogType = new HashSet<eLogType>();
        private HashSet<string> m_FilterDeviceID = new HashSet<string>();
        private HashSet<long> m_FilterUID = new HashSet<long>();

        public Panel_GameLog()
        {
            InitializeComponent();
        }

        public override void Init(ePanelType panel_type, eLanguageType type = eLanguageType.Eng)
        {
            base.Init(panel_type);

            //combobox
            sCombobox_log_type.Init(OperHelper.EnumToStringList<eLogType>());

            //label
            sLabel_device_id.Init("DeviceID");
            sLabel_uid.Init("UID");
            sLabel_start.Init("Start");
            sLabel_end.Init("End");
            sLabel_log_type.Init("LogType");

            //button
            sButton_log_type_add.Init("Add");
            sButton_uid_add.Init("Add");
            sButton_device_id_add.Init("Add");
            sButton_log_search.Init("Search");

            //datetime
            sDateTimePicker_start.Init(OperDefine.MaxTime, OperDefine.MinTime);
            sDateTimePicker_end.Init(OperDefine.MaxTime, OperDefine.MinTime);

            //gridview
            sGridView_device_id.SetHeader("DeviceID");
            sGridView_log_type.SetHeader("LogType");
            sGridView_uid.SetHeader("UID");
            sGridView_log.SetHeader<GameLogView>();

            sTextBox_filter_deviceId.Init(false);
            sTextBox_filter_uid.Init(true);

            FilterListClear();
        }


        private void FilterListClear()
        {
            m_FilterLogType.Clear();
            m_FilterDeviceID.Clear();
            m_FilterUID.Clear();
        }

        private void onGridViewDoubleClick_filter_list_type(object sender, DataGridViewCellEventArgs e)
        {
            //filter type delete
            eLogType logtype = sGridView_log_type.GetDataEnum<eLogType>("LogType", eLogType.Max);
            m_FilterLogType.Remove(logtype);
        }

        private void onGridViewDoubleClick_filter_list_deviceid(object sender, DataGridViewCellEventArgs e)
        {
            //filter user deviceid delete
            string deviceid = sGridView_device_id.GetData("DeviceID");
            m_FilterDeviceID.Remove(deviceid);
        }

        private void onGridViewDoubleClick_filter_list_uid(object sender, DataGridViewCellEventArgs e)
        {
            //fitler user uid delete
            long uid = sGridView_uid.GetData<long>("UID", -1);
            m_FilterUID.Remove(uid);
        }

        private void onClick_filter_type_add(object sender, EventArgs e)
        {
            var selectValue = sCombobox_log_type.GetValue();
            if(selectValue == null)
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.NoneSelectedItem);
                return;
            }

            if(!Enum.TryParse(selectValue, out eLogType type))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.NoneSelectedItem);
                return;
            }

            if(m_FilterLogType.Add(type))
                sGridView_log_type.AddData("LogType", type.ToString());
        }

        private void onClick_filter_deviceid_add(object sender, EventArgs e)
        {
            //filter deviceid add
            string selectValue = sTextBox_filter_deviceId.GetValue();
            if (selectValue == null || string.IsNullOrEmpty(selectValue))
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.NoneSelectedItem);
                return;
            }

            if (m_FilterDeviceID.Add(selectValue))
                sGridView_device_id.AddData("DeviceID", selectValue.ToString());
        }

        private void onClick_filter_uid_add(object sender, EventArgs e)
        {
            long selectValue = sTextBox_filter_uid.GetValue<long>();

            if (m_FilterUID.Add(selectValue))
                sGridView_uid.AddData("UID", selectValue.ToString());
        }

        private void onClick_logbuttonsearch(object sender, EventArgs e)
        {
            DateTime begin = sDateTimePicker_start.Value;
            DateTime end = sDateTimePicker_end.Value;
            if (begin > end)
            {
                CMessageBoxManager.Instance.Warning(this, eMessageBoxMessage.DateCheck);
                return;
            }

            MongoFilterBuilder filter = new MongoFilterBuilder();
            foreach (var iter in m_FilterDeviceID)
                filter.Equal("deviceid", iter);

            foreach (var iter in m_FilterUID)
                filter.Equal("uid", iter);

            foreach (var iter in m_FilterLogType)
                filter.Equal("type", (int)iter);

            filter.Between("time", begin, end);

            string collectionName = $"log_{begin:yyyyMM}";

            string dbname = FormManager.Instance.ConnectServer.ToString();
            OperLogConnectRecord record = OperLogConnectTable.Instance.Find(dbname);
            if (record == null)
                return;

            MongoDBManager.Instance.Insert(new FindQuery(record.DBName, collectionName, filter.Build()));
        }


        public void SetGameLogData(List<LogBson> datas)
        {
            var viewDatas = new List<GameLogView>();
            foreach (var iter in datas)
                viewDatas.Add(new GameLogView(iter));

            sGridView_log.SetData<GameLogView>(viewDatas);
        }
    }
}
