using Global;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
   
    public class GrowthLevelRecord : STableRecord<int>
    {
        public CDefine.EAbility abil_type { get; set; }
        public double up_value { get; set; }
        public int max_point { get; set; }
        public int ConditionID { get; set; }
    }

    public class GrowthLevelTable : STable<GrowthLevelTable, int, GrowthLevelRecord>
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
                    GrowthLevelRecord record = new GrowthLevelRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.abil_type = reader.GetEnum<CDefine.EAbility>(i, "abil_type", CDefine.EAbility.Max);
                    record.up_value = reader.GetValue<double>(i, "up_value", 0);
                    record.max_point = reader.GetValue<int>(i, "max_point", int.MaxValue);
                    record.ConditionID = reader.GetValue<int>(i, "ConditionID", 0);

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

        public Dictionary<int, _LevelData> CopyDefault()
        {
            Dictionary<int, _LevelData> retList = new Dictionary<int, _LevelData>();

            foreach (var data in m_Table)
            {
                _LevelData level_data = new _LevelData();
                level_data.m_TableID = data.Key;
                level_data.m_UseCount = 0;
                retList.Add(level_data.m_TableID, level_data);
            }

            return retList;
        }
    }
}
