using Global;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Global
{
    public class KnightUpgradeRecord : STableRecord<int>
    {
        public int GroupID = -1;
        public int Star = -1;
        public int NextID = -1;
        public int SpecialAbilID = -1;
        public int SlotTableID = -1;
        public int SlotMaxCount = -1;
        public int AccessoryMaterial = 0;
        public int AccessoryMaterial_Count = 0;
        public List<_AbilData> EquipAbils = new List<_AbilData>();
        public _AbilData SpecialAbil = new _AbilData();
    }

    public class KnightUpgradeTable : STable<KnightUpgradeTable, int, KnightUpgradeRecord>
    {
        private Dictionary<int, Dictionary<int, KnightUpgradeRecord>> m_GroupStars = new Dictionary<int, Dictionary<int, KnightUpgradeRecord>>();

        public override void Prepare() 
        {
            foreach (var iter in m_Table)
            {
                var record = iter.Value;
                
                if(m_GroupStars.ContainsKey(record.GroupID))
                {
                    var entryStars = m_GroupStars[record.GroupID];
                    entryStars[record.Star] = record;
                }
                else
                {
                    var entry = new Dictionary<int, KnightUpgradeRecord>();
                    entry[record.Star] = record;
                    m_GroupStars[record.GroupID] = entry;
                }
            }
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
                    KnightUpgradeRecord record = new KnightUpgradeRecord();
                    record.ID = reader.GetValue(i, "ID", -1);
                    record.GroupID = reader.GetValue(i, "GroupID", -1);
                    record.Star = reader.GetValue(i, "Star", -1);
                    record.NextID = reader.GetValue(i, "NextID", -1);
                    record.SpecialAbilID = reader.GetValue(i, "SpecialAbilID", -1);
                    record.SlotTableID = reader.GetValue(i, "SlotTableID", -1);
                    record.SlotMaxCount = reader.GetValue(i, "SlotMaxCount", -1);
                    record.AccessoryMaterial = reader.GetValue(i, "AccessoryMaterial", -1);
                    record.AccessoryMaterial_Count = reader.GetValue(i, "AccessoryMaterial_Count", 0);

                    record.SpecialAbil.type = reader.GetEnum<CDefine.EAbility>(i, "SpecialAbil", CDefine.EAbility.Max);
                    record.SpecialAbil.val = reader.GetValue<double>(i, "SpecialAbil_Value", 0.0);

                    int idx = 1;
                    while (true)
                    {
                        string strEquipAbilType = "equip_abil_" + idx.ToString();
                        if (!reader.DoesColumnExist(strEquipAbilType))
                            break;


                        string strEquipAbilValue = "equip_abil_value_" + idx.ToString();
                        if (!reader.DoesColumnExist(strEquipAbilValue))
                            break;

                        string strEquipAbilType_check = reader.GetValue(i, strEquipAbilType);
                        string strEquipAbilValue_check = reader.GetValue(i, strEquipAbilValue);
                        if (string.IsNullOrEmpty(strEquipAbilType_check) || string.IsNullOrEmpty(strEquipAbilValue_check))
                            break;

                        _AbilData equipabil = new _AbilData();

                        equipabil.type = reader.GetEnum(i, strEquipAbilType, CDefine.EAbility.Max);
                        equipabil.val = reader.GetValue(i, strEquipAbilValue, 0.0);

                        if (equipabil.type != CDefine.EAbility.Max)
                            record.EquipAbils.Add(equipabil);

                        ++idx;
                    }

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

        public KnightUpgradeRecord FindGroupID(int groupID, int star)
        {
            if (m_GroupStars.TryGetValue(groupID, out var entry))
            {
                if (entry.TryGetValue(star, out var retVal))
                    return retVal;

                return null;
            }

            return null;
        }
    }
}
