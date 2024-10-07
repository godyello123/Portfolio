using DnsClient;
using Global;
using Packet_P2M;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class EventRouletteRecord : STableRecord<int>
    {
        public int EventID = -1;
        public CDefine.CoinType CostCoinType = CDefine.CoinType.Max;
        public int Index = -1;
        public long CostValue = 0;
        public int RewardID = 0;
        public int Prob = 0;
    }

    public class EventRouletteTable : STable<EventRouletteTable, int, EventRouletteRecord>
    {
        Dictionary<int, List<EventRouletteRecord>> m_RoulettEntry = new Dictionary<int, List<EventRouletteRecord>>();

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    EventRouletteRecord record = new EventRouletteRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.EventID = reader.GetValue(i, "EventID", 0);
                    record.Index = reader.GetValue(i, "Index", 0);
                    record.CostCoinType = reader.GetEnum<CDefine.CoinType>(i, "CostCoinType", CDefine.CoinType.Max);
                    record.CostValue = reader.GetValue(i, "CostValue", 0);
                    record.RewardID = reader.GetValue(i, "RewardID", 0);
                    record.Prob = reader.GetValue(i, "Prob", 0);

                    Add(record.ID, record);

                    if(m_RoulettEntry.ContainsKey(record.EventID))
                        m_RoulettEntry[record.EventID].Add(record);
                    else
                        m_RoulettEntry[record.EventID] = new List<EventRouletteRecord> { record };
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            }
        }

        private List<EventRouletteRecord> FindEntry(int eventID)
        {
            if (m_RoulettEntry.TryGetValue(eventID, out var retval))
                return retval;

            return null;
        }

        public EventRouletteRecord Findfirst(int eventID)
        {
            List<EventRouletteRecord> entry = new List<EventRouletteRecord>();
            if (!m_RoulettEntry.TryGetValue(eventID, out entry))
                return null;

            if (entry.Count < 1) return null;

            return entry[0];
        }

        public EventRouletteRecord Roullet(int eventID, List<int> exclude)
        {
            var entry = FindEntry(eventID);
            if (entry == null)
                return null;

            List<EventRouletteRecord> copyEntry = SCommon.SCopy<List<EventRouletteRecord>>.DeepCopy(entry);

            int total_prob = 0;
            for (int i = copyEntry.Count - 1; i >= 0; --i)
            {
                var entries = copyEntry[i];
                if (exclude.Contains(entries.Index))
                {
                    copyEntry.RemoveAt(i);
                }
                else
                {
                    total_prob += entries.Prob;
                    entries.Prob = total_prob;
                }
            }

            int randValue = SRandom.Instance.Next(0, total_prob);

            for (int j = copyEntry.Count - 1; j >= 0; --j)
            {
                var roulletEntrys = copyEntry[j];
                if (randValue > roulletEntrys.Prob)
                    continue;

                return roulletEntrys;
            }

            return null;
        }
    }
}
