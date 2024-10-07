using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class DefaultPlayerRecord : STableRecord<int>
    {
        public int ActorID = 0;
        public string SkinID { get; set; }
        public List<int> AttackSkillID = new List<int>();
        public int Level { get; set; }

        public string PrologueID = string.Empty;

        public Dictionary<CDefine.EAbility, _AbilData> DefaultAbils = new Dictionary<CDefine.EAbility, _AbilData>();

        public int ProfileID = 0;
    }

    public class DefaultPlayerTable : STable<DefaultPlayerTable, int, DefaultPlayerRecord>
    {
        private DefaultPlayerRecord m_DefulatPlayerRecord = new DefaultPlayerRecord();

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
                    DefaultPlayerRecord record = new DefaultPlayerRecord();
                    record.ID = reader.GetValue<int>(i, "ID", 0);
                    record.ActorID = reader.GetValue<int>(i, "ActorID", 0);
                    record.SkinID = reader.GetValue(i, "SkinID", "");
                    record.PrologueID = reader.GetValue(i, "PrologueID", "");

                    string defaultSkill = string.Empty;
                    defaultSkill = reader.GetValue(i, "AttackSkillID", "");
                    var strArray = defaultSkill.Split(',');
                    foreach (var iter in strArray)
                    {
                        if (int.TryParse(iter.Trim(), out var ret))
                            record.AttackSkillID.Add(ret);
                    }

                    record.Level = reader.GetValue<int>(i, "Level", 1);

                    for (CDefine.EAbility abilType = CDefine.EAbility.Damage; abilType < CDefine.EAbility.Max; abilType++)
                    {
                        _AbilData abil = new _AbilData();
                        abil.type = abilType;
                        abil.val = reader.GetValue<double>(i, abilType.ToString(), 0);

                        record.DefaultAbils[abil.type] = abil;
                    }

                    record.ProfileID = reader.GetValue(i, "ProfileID", 0);

                    Add(record.ID, record);

                    m_DefulatPlayerRecord = record;
                }

            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            }


            return true;
        }

        public DefaultPlayerRecord GetDefaultRecord()
        {
            return m_DefulatPlayerRecord;
        }

        public Dictionary<CDefine.EAbility, _AbilData> CopyDefaultAbil()
        {
            return new Dictionary<CDefine.EAbility, _AbilData>(m_DefulatPlayerRecord.DefaultAbils);
        }

        public List<int> CopyDefaultSkills()
        {
            return new List<int>();
        }

        public string SkinID()
        {
            return m_DefulatPlayerRecord.SkinID;
        }

        public int ActorID()
        {
            return m_DefulatPlayerRecord.ActorID;
        }

        public int ProfileID()
        {
            return m_DefulatPlayerRecord.ProfileID;
        }
    }
}
