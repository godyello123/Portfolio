using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class ItemChivalryEnchantRecord : STableRecord<string>
    {
        public int GroupID = 0;
        public int EnchantLv = 0;
        public int CostCount = 0;
        public int EnchantProb = 0;
        public int MaxProb = 10000;
        public _AbilData Abil = new _AbilData();

        public bool Roulette()
        {
            int randVal = SRandom.Instance.Next(0, MaxProb);

            if (randVal < EnchantProb)
                return true;

            return false;
        }
    }

    public class ItemChivalryEnchantTable : SPairKeyTable<ItemChivalryEnchantTable, int,int, ItemChivalryEnchantRecord>
    {
        Dictionary<int, int> m_MaxLevel = new Dictionary<int, int>();

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
                    ItemChivalryEnchantRecord record = new ItemChivalryEnchantRecord();
                    record.ID = reader.GetValue(i, "ID");
                    record.GroupID = reader.GetValue(i, "GroupID", 0);
                    record.EnchantLv = reader.GetValue(i, "EnchantLv", 0);
                    record.CostCount = reader.GetValue(i, "CostCount", 0);
                    record.EnchantProb = reader.GetValue(i, "EnchantProb", 0);

                    record.Abil.type = reader.GetEnum<CDefine.EAbility>(i, "Abil_Type", CDefine.EAbility.Max);
                    record.Abil.val = reader.GetValue(i, "Abil_Value", 0.0);

                    Add(record.GroupID, record.EnchantLv, record);

                    if (m_MaxLevel.ContainsKey(record.GroupID))
                    {
                        int level = m_MaxLevel[record.GroupID];
                        m_MaxLevel[record.GroupID] = level < record.EnchantLv ? record.EnchantLv : level;
                    }
                    else
                    {
                        m_MaxLevel[record.GroupID] = record.EnchantLv;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            }

           
        }

        public int GetMaxLevel(int groupID)
        {
            if (m_MaxLevel.ContainsKey(groupID))
                return m_MaxLevel[groupID];

            return -1;
        }
    }
}
