using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class EventRecord : STableRecord<int>
    {
        public CDefine.EventType EventType = CDefine.EventType.Max;
        public string QuestID = string.Empty;
        public long DuringTime = 0;
        public CDefine.CoinType EventDropCoinID = CDefine.CoinType.Max;
        public long EventDropCoinProb = 0;
        public int EventDropCoinValue = 0;
    }

    public class EventTable : STable<EventTable, int, EventRecord>
    {
        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    EventRecord record = new EventRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.EventType = reader.GetEnum<CDefine.EventType>(i, "EventType");

                    if (record.EventType == CDefine.EventType.CoinEvent)
                    {
                        record.EventDropCoinID = reader.GetEnum<CDefine.CoinType>(i, "EventDropCoinID");
                        record.EventDropCoinProb = reader.GetValue<long>(i, "EventDropCoinProb", 0);
                        record.EventDropCoinValue = reader.GetValue<int>(i, "EventDropCoinValue", 0);
                    }
                    else if (record.EventType == CDefine.EventType.QuestEvent)
                    {
                        record.QuestID = reader.GetValue(i, "QuestID");
                    }

                    record.DuringTime = reader.GetValue<long>(i, "DuringTime", 0);

                    Add(record.ID, record);
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            }

        }
    }
}
