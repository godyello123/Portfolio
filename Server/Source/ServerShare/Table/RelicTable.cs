using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Global
{
    public class RelicRecord : STableRecord<int>
    {
        public int GroupID = 0;
        public int Level = 0;
        public int Prob = 0;
        public int BonusProb = 0;
        public _AssetData CostAsset = new _AssetData();
        public List<_AbilData> Abils = new List<_AbilData>();
    }

    public class RelicTable : SPairKeyTable<RelicTable, int, int, RelicRecord>
    {
        Dictionary<int, RelicRecord> m_DefaultRelics = new Dictionary<int, RelicRecord>();

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

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    RelicRecord record = new RelicRecord();
                    record.ID = reader.GetValue<int>(i, "ID", 0);
                    record.GroupID = reader.GetValue(i, "GroupID", 0);
                    record.Prob = reader.GetValue(i, "Prob", int.MaxValue);
                    record.Level = reader.GetValue(i, "Lv", 0);
                    record.BonusProb = reader.GetValue(i, "BonusProb", 0);
                    record.CostAsset.Type = CDefine.AssetType.Coin;
                    record.CostAsset.TableID = reader.GetValue(i, "Coin_Type");
                    record.CostAsset.Count = reader.GetValue(i, "Coin_Value", long.MaxValue);

                    int idx = 1;
                    while (true)
                    {
                        string strAbilType = "Abil_Type_" + idx.ToString();
                        if (!reader.DoesColumnExist(strAbilType))
                            break;

                        string strAbilValue = "Abil_Value_" + idx.ToString();
                        if (!reader.DoesColumnExist(strAbilValue))
                            break;

                        string strabilType_check = reader.GetValue(i, strAbilType);
                        string straiblvalue_check = reader.GetValue(i, strAbilValue);
                        if (string.IsNullOrEmpty(strabilType_check) || string.IsNullOrEmpty(straiblvalue_check))
                            break;

                        _AbilData abil = new _AbilData();
                        abil.type = reader.GetEnum(i, strAbilType, CDefine.EAbility.Max);
                        abil.val = reader.GetValue(i, strAbilValue, 0.0);

                        if (abil.type != CDefine.EAbility.Max)
                            record.Abils.Add(abil);

                        ++idx;
                    }

                    if (m_DefaultRelics.TryGetValue(record.GroupID, out var relic))
                    {
                        if (relic.Level > record.Level)
                            m_DefaultRelics[record.GroupID] = record;
                    }
                    else
                    {
                        m_DefaultRelics[record.GroupID] = record;
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

        public Dictionary<int, _RelicData> CopyDefaults()
        {
            Dictionary<int, _RelicData> retVal = new Dictionary<int, _RelicData>();
            foreach (var iter in m_DefaultRelics)
            {
                var record = iter.Value;
                _RelicData relic = new _RelicData();
                relic.m_GroupID = record.GroupID;
                relic.m_Level = record.Level;
                relic.m_BonusProb = 0;

                retVal[relic.m_GroupID] = relic;
            }

            return retVal;
        }
    }
}
