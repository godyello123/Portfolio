using Global;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class AdsBuffRecord : STableRecord<int>
    {
        public int MaxWatchAdsDailyCount = 0;
        public int MaxLv = -1;
        public _AbilData Abil = new _AbilData();
        public long Duration = 0;
    }

    public class AdsBuffTable : STable<AdsBuffTable, int, AdsBuffRecord>
    {
        public override void Prepare() { }

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    AdsBuffRecord record = new AdsBuffRecord();
                    record.ID = reader.GetValue(i, "ID", 0);
                    record.MaxLv = reader.GetValue(i, "MaxLv", 0);
                    record.MaxWatchAdsDailyCount = reader.GetValue(i, "MaxWatchAdsDailyCount", 0);
                    record.Abil.type = reader.GetEnum<CDefine.EAbility>(i, "Abil_Type", CDefine.EAbility.Max);
                    record.Abil.val = reader.GetValue(i, "Abil_Value", 0.0);
                    record.Duration = reader.GetValue(i, "DurationTime", 0);

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

        public Dictionary<int, _AdsBUffData> CopyDefault()
        {
            Dictionary<int, _AdsBUffData> ret = new Dictionary<int, _AdsBUffData>();

            foreach(var it in Table)
            {
                var record = it.Value;
                _AdsBUffData data = new _AdsBUffData();
                data.m_BuffID = record.ID;
                data.m_BuffLv = 1;
                data.m_BuffExp = 0;
                data.m_BuffExpTime = -1;
                data.m_WatchAdsDailyExpTime = SDateManager.Instance.TomorrowTimestamp() + 1;
                data.m_WatchAdsDailyCount = 0;

                ret.Add(data.m_BuffID, data);
            }

            return ret;
        }

        public int GetCount()
        {
            return m_Table.Count;
        }

    }
}
