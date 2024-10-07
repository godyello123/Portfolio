using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class ActorRecord : STableRecord<int>
    {
        public CDefine.ActorType ActorType { get; set; }
        public long ModelID { get; set; }
        public int SkillID { get; set; }
        public int ActiveSkillID { get; set; }
    }

    public class ActorTable : STable<ActorTable, int, ActorRecord>
    {
        private _ActorData m_DefaultActor = new _ActorData();

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    ActorRecord record = new ActorRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.ActorType = reader.GetEnum<CDefine.ActorType>(i, "ActorType");
                    record.ModelID = reader.GetValue<int>(i, "ModelID", 0);
                    record.SkillID = reader.GetValue<int>(i, "SkillID", 0);
                    record.ActiveSkillID = reader.GetValue<int>(i, "ActiveSkillID", 0);

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
            var findIt = Table.FirstOrDefault(x => x.Value.ActorType == CDefine.ActorType.Player);
            ActorRecord record = findIt.Value;
            if (record == null)
                return;

            m_DefaultActor.m_TableID = record.ID.ToString();
            m_DefaultActor.m_SkinID = record.ModelID.ToString();
            m_DefaultActor.m_DefaultSkill.Add(record.SkillID);
            m_DefaultActor.m_DefaultSkill.Add(record.ActiveSkillID);
        }
    }
}
