using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Global
{
    public class SkillRecord : STableRecord<int>
    {
        public int GroupID = 0;
        public CDefine.SkillCategory GroupType = CDefine.SkillCategory.Max;
        public float DamageValue = 0;
        public int CoolTime = 0;
        public int DurationTime = 0;
        public int ConditionID = 0;
        public List<_AssetData> ConsumeAssets = new List<_AssetData>();
        public int Level = 0;
    }

    public class SkillTable : STable<SkillTable, int, SkillRecord>
    {
        private Dictionary<int, List<SkillRecord>> m_Sub = new Dictionary<int, List<SkillRecord>>();
        private List<_SkillData> m_NormalSkill = new List<_SkillData>();
        

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    SkillRecord record = new SkillRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.GroupID = reader.GetValue<int>(i, "GroupID", 0);
                    record.GroupType = reader.GetEnum<CDefine.SkillCategory>(i, "GroupType");
                    record.DamageValue = reader.GetValue<float>(i, "DamageValue", 0);
                    record.CoolTime = reader.GetValue<int>(i, "CoolTime", 0);
                    record.DurationTime = reader.GetValue<int>(i, "DurationTime", 0);
                    record.ConditionID = reader.GetValue<int>(i, "ConditionID", -1);
                    record.Level = reader.GetValue<int>(i, "Level", 0);

                    for (int colidx = 1; true; ++colidx)
                    {
                        string strCosttype = $"Consume_AssetType_{colidx}";
                        string strCostid = $"Consume_AssetTableID_{colidx}";
                        string strCostval = $"Consume_AssetValue_{colidx}";

                        string sct_val = reader.GetValue(i, strCosttype);
                        if (string.IsNullOrEmpty(sct_val))
                            break;

                        string sci_val = reader.GetValue(i, strCostid);
                        if (string.IsNullOrEmpty(sci_val))
                            break;

                        string scv_val = reader.GetValue(i, strCostval);
                        if (string.IsNullOrEmpty(scv_val))
                            break;

                        _AssetData cost = new _AssetData();
                        cost.Type = reader.GetEnum<CDefine.AssetType>(i, strCosttype, CDefine.AssetType.Max);
                        cost.TableID = reader.GetValue(i, strCostid);
                        cost.Count = reader.GetValue(i, strCostval, 0);

                        record.ConsumeAssets.Add(cost);
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

        public override void Prepare()
        {
            foreach (var it in Table)
            {
                var itRecord = it.Value;

                if (itRecord.GroupID < 1)
                    continue;

                if (itRecord.GroupType == CDefine.SkillCategory.Attack)
                    continue;

                if (m_Sub.ContainsKey(itRecord.GroupID))
                    m_Sub[itRecord.GroupID].Add(itRecord);
                else
                    m_Sub[itRecord.GroupID] = new List<SkillRecord> { itRecord };
            }

            var defaultSkills = DefaultPlayerTable.Instance.CopyDefaultSkills();
            foreach(var iter in defaultSkills)
            {
                var record = Find(iter);
                if (record == null)
                    continue;

                _SkillData skill = new _SkillData();
                skill.TID = record.ID;
                skill.GroupID = record.GroupID;

                m_NormalSkill.Add(skill);
            }



        }

        public List<SkillRecord> FindGroup(int groupID)
        {
            if (m_Sub.TryGetValue(groupID, out var retVal))
                return retVal;

            return null;
        }

        public SkillRecord FindGroupFirst(int groupID)
        {
            var records = FindGroup(groupID);
            if (records == null || records.Count < 1)
                return null;

            return records[0];
        }

        public SkillRecord Find(int groupID, int skillLv)
        {
            var records = FindGroup(groupID);
            if (records == null || records.Count < 1)
                return null;

            var ret = records.Find(x => x.Level == skillLv);
            if (ret == null)
                return null;

            return ret;
        }

        public List<_SkillData> CopyDefalut()
        {
            List<_SkillData> ret = new List<_SkillData>();
            
            foreach(var iter in m_Sub)
            {
                var records = iter.Value;
                var firstrecord = FindGroupFirst(iter.Key);

                if (firstrecord.GroupType == CDefine.SkillCategory.Attack)
                    continue;

                _SkillData skill = new _SkillData();
                skill.TID = firstrecord.ID;
                skill.GroupID = firstrecord.GroupID;
                skill.IsLearend = false;
                skill.Dmg = 0;
                skill.Lv = 1;

                ret.Add(skill);
            }

            return ret;
        }

        public List<_SkillData> CopyNormalSkills()
        {
            return SCopy<List<_SkillData>>.DeepCopy(m_NormalSkill);
        }

    }
}
