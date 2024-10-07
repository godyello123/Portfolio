using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class StageSkillRecord : STableRecord<CDefine.eStageType>
    {
        public HashSet<int> BanSkills = new HashSet<int>();
        public int MaxEquipSlot = 0;


        public bool IsBanSkill(int skillID)
        {
            if (BanSkills.Count < 1)
                return false;

            return BanSkills.Contains(skillID);
        }
    }

    public class StageSkillTable : STable<StageSkillTable, CDefine.eStageType, StageSkillRecord>
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
                    StageSkillRecord record = new StageSkillRecord();
                    record.ID = reader.GetEnum<CDefine.eStageType>(i, "ID", CDefine.eStageType.Max);
                    record.BanSkills = reader.GetSet<int>(i, "BanSkillID");
                    record.MaxEquipSlot = reader.GetValue(i, "MaxEqipSlot", 0);

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
    }
}
