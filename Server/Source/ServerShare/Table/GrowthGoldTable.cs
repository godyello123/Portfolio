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
    public static class ConvertTableValue
    {
        public static void ConvertValue(ref Int64 refVal, string tableStr)
        {
            if (string.IsNullOrEmpty(tableStr))
                refVal = -1;

            if (!Int64.TryParse(tableStr, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out refVal))
                refVal = -1;
        }

        public static void ConvertValue(ref int refVal, string tableStr)
        {
            if (string.IsNullOrEmpty(tableStr))
                refVal = -1;

            if (!int.TryParse(tableStr, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out refVal))
                refVal = -1;
        }

        public static void ConvertValue(ref double refVal, string tableStr)
        {
            if (string.IsNullOrEmpty(tableStr))
                refVal = -1;

            if (!double.TryParse(tableStr, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out refVal))
                refVal = -1;
        }
    }

    public class GrowthGoldRecord : STableRecord<int>
    {
        public int GroupID { get; set; }
        public CDefine.EAbility abil_type { get; set; }
        public double up_value { get; set; }
        public int Level { get; set; }
        public string AssetType { get; set; }
        public long AssetValue { get; set; }
        public int ConditionID { get; set; }
        public double total_value { get; set; }
    }

    public class GrowthGoldTable : SPairKeyTable<GrowthGoldTable, int, int, GrowthGoldRecord>
    {
        public override void Prepare()
        {
        }

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                Dictionary<int, GrowthGoldRecord> temp_value = new Dictionary<int, GrowthGoldRecord>();

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    GrowthGoldRecord record = new GrowthGoldRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.GroupID = reader.GetValue<int>(i, "GroupID", 0);
                    record.abil_type = reader.GetEnum<CDefine.EAbility>(i, "abil_type", CDefine.EAbility.Max);
                    record.up_value = reader.GetValue<double>(i, "up_value", 0);
                    record.Level = reader.GetValue<int>(i, "Level", 0);
                    record.AssetType = reader.GetValue(i, "AssetType", "");
                    record.AssetValue = reader.GetValue(i, "AssetValue", 0);

                    if (true == temp_value.ContainsKey(record.GroupID))
                    {
                        record.AssetValue += temp_value[record.GroupID].AssetValue;
                        temp_value[record.GroupID].AssetValue = record.AssetValue;

                        record.total_value += temp_value[record.GroupID].total_value + record.up_value;
                        temp_value[record.GroupID].total_value = record.total_value;
                    }
                    else
                    {
                        GrowthGoldRecord temp_record = new GrowthGoldRecord();
                        temp_record.up_value = record.up_value;
                        temp_record.AssetValue = record.AssetValue;
                        temp_record.total_value = record.up_value;
                        temp_value.Add(record.GroupID, temp_record);
                    }

                    Add(record.GroupID, record.Level, record);
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
