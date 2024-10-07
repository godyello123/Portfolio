using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class EventShopRecord : STableRecord<int>
    {
        public int EventID = 0;
        public int LimitCount = 0;
        public int RewardID = 0;
        public CDefine.CoinType CostCoinType = CDefine.CoinType.Max;
        public int CostCoinValue = 0;
    }

    public class EventShopTable : STable<EventShopTable, int, EventShopRecord>
    {
        Dictionary<int, List<EventShopRecord>> m_DefaultEventShopRecords = new Dictionary<int, List<EventShopRecord>>();

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    EventShopRecord record = new EventShopRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.EventID = reader.GetValue<int>(i, "EventID", 0);
                    record.LimitCount = reader.GetValue<int>(i, "LimitCount", 0);
                    record.RewardID = reader.GetValue<int>(i, "RewardID", 0);
                    record.CostCoinType = reader.GetEnum<CDefine.CoinType>(i, "CostCoinType");
                    record.CostCoinValue = reader.GetValue<int>(i, "CostCoinValue");

                    Add(record.ID, record);

                    if (m_DefaultEventShopRecords.ContainsKey(record.EventID))
                        m_DefaultEventShopRecords[record.EventID].Add(record);
                    else
                        m_DefaultEventShopRecords[record.EventID] = new List<EventShopRecord> { record };
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            }

        }

        public List<_EventShopData> CopyDefaultEventShopDatas(long eventuid, int eventid)
        {
            var retVal = new List<_EventShopData>();
            var records = FindShopRecords(eventid);
            if (records == null)
                return retVal;

            foreach(var iter in records)
            {
                _EventShopData defaultData = new _EventShopData();
                defaultData.m_EventShopID = iter.ID;
                defaultData.m_EventUID = eventuid;
                defaultData.m_BuyCount = 0;

                retVal.Add(defaultData);
            }

            return retVal;
        }

        public List<EventShopRecord> FindShopRecords(int eventid)
        {
            if (m_DefaultEventShopRecords.TryGetValue(eventid, out var retval))
                return retval;

            return null;
        }
    }
}
