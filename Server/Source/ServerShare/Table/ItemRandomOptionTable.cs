using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class AbilProb
    {
        public CDefine.EAbility Type;
        public double Value;
        public int Prob = 0;
    }

    public class ItemRandomOptionRecord : STableRecord<int>
    {
        public int GroupID = 0;
        public int StatProb = 0;
        public int AbilMaxProb = 0;
        public CDefine.EItemGrade Grade = CDefine.EItemGrade.Max;
        public List<AbilProb> Abils = new List<AbilProb>();
        
        public Tuple<int,int> Roulette()
        {
            int randVal = SRandom.Instance.Next(0, AbilMaxProb);

            int idx = 0;
            foreach (var prob in Abils)
            {
                if (randVal > prob.Prob)
                {
                    idx++;
                    continue;
                }

                return Tuple.Create(ID, idx);
            }

            return null;
        }

        public void Roulette(ref int abli_key, ref int abil_idx)
        {
            int randVal = SRandom.Instance.Next(0, AbilMaxProb);

            int idx = 0;
            foreach(var prob in Abils)
            {
                if (randVal > prob.Prob)
                {
                    idx++;
                    continue;
                }

                abli_key = this.ID;
                abil_idx = idx;
                break;
            }
        }
    }

    public class _GroupProbInfo
    {
        public int MaxProb = 0;
        public List<ItemRandomOptionRecord> m_Records = new List<ItemRandomOptionRecord>();

        public ItemRandomOptionRecord Roulette()
        {
            int randVal = SRandom.Instance.Next(0, MaxProb);

            ItemRandomOptionRecord optionRecord = new ItemRandomOptionRecord();

            foreach (var iter in m_Records)
            {
                if (randVal > iter.StatProb)
                    continue;

                optionRecord = iter;
                return optionRecord;
            }

            return null;
        }
    }

    public class ItemRandomOptionTable : STable<ItemRandomOptionTable, int, ItemRandomOptionRecord>
    {
        private Dictionary<int, _GroupProbInfo> m_GropuProbs = new Dictionary<int, _GroupProbInfo>();

        public ItemRandomOptionRecord GroupRoulette(int groupID)
        {
            if (m_GropuProbs.ContainsKey(groupID))
                return m_GropuProbs[groupID].Roulette();

            return null;
        }

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
                    ItemRandomOptionRecord record = new ItemRandomOptionRecord();

                    record.ID = reader.GetValue<int>(i, "ID", 0);
                    record.GroupID = reader.GetValue<int>(i, "GroupID", 0);
                    record.StatProb = reader.GetValue<int>(i, "StatsProb", 0); ;
                    record.Grade = reader.GetEnum<CDefine.EItemGrade>(i, "Grade", CDefine.EItemGrade.Max);

                    int total_abil_prob = 0;
                    for (int colidx = 1; true; colidx++)
                    {
                        string strAbilType = $"abil_type_{colidx}";
                        string strAbilval = $"abil_value_{colidx}";
                        string strAbilProb = $"abil_prob_{colidx}";

                        if (!reader.DoesColumnExist(strAbilType) || !reader.DoesColumnExist(strAbilval) || !reader.DoesColumnExist(strAbilProb))
                            break;

                        AbilProb abilProbData = new AbilProb();

                        string strAbilType_val = reader.GetValue(i, strAbilType);
                        if (string.IsNullOrEmpty(strAbilType_val))
                            break;

                        string strAbilval_val = reader.GetValue(i, strAbilval);
                        if (string.IsNullOrEmpty(strAbilval_val))
                            break;

                        string strAbilProb_val = reader.GetValue(i, strAbilProb);
                        if (string.IsNullOrEmpty(strAbilProb_val))
                            break;

                        abilProbData.Type = reader.GetEnum<CDefine.EAbility>(i, strAbilType, CDefine.EAbility.Max);
                        abilProbData.Value = reader.GetValue<double>(i, strAbilval, 0.0);

                        int abilProb = reader.GetValue<int>(i, strAbilProb, 0);
                        if (abilProb > 0)
                        {
                            total_abil_prob += abilProb;
                            abilProbData.Prob = total_abil_prob;

                            record.Abils.Add(abilProbData);
                        }
                    }

                    record.AbilMaxProb = total_abil_prob;

                    MakeGroupProb(record);
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

        private void MakeGroupProb(ItemRandomOptionRecord record)
        {
            if (m_GropuProbs.ContainsKey(record.GroupID))
            {
                record.StatProb += m_GropuProbs[record.GroupID].MaxProb;
                m_GropuProbs[record.GroupID].MaxProb = record.StatProb;
                m_GropuProbs[record.GroupID].m_Records.Add(record);
            }
            else
            {
                _GroupProbInfo info = new _GroupProbInfo();
                info.MaxProb = record.StatProb;
                info.m_Records.Add(record);
                m_GropuProbs.Add(record.GroupID, info);
            }
        }
    }
}
