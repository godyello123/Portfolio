using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using Global;


namespace Global
{
    public class MissionRecord : STableRecord<int>
    {
        public CDefine.QuestType Quest_Type = CDefine.QuestType.Max;
        public string Quest_Id = "";
        public int Next = 0;
        public int Order = 0;
        public int ConditionID = 0;
        public int Unlock_Condition_ID = 0;
        public int Reward_Id = 0;
        public int Reward_Pass_Id = 0;
    }

    public class MissionTable : STable<MissionTable, int, MissionRecord>
    {
        public Dictionary<CDefine.QuestType, MissionRecord> m_DefaultMissions = new Dictionary<CDefine.QuestType, MissionRecord>();

        private Dictionary<KeyValuePair<CDefine.QuestType, string>, MissionRecord> m_FirstMisison = new Dictionary<KeyValuePair<CDefine.QuestType, string>, MissionRecord>();

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
                    MissionRecord record = new MissionRecord();
                    record.ID = reader.GetValue(i, "ID", 0);
                    record.Quest_Type = reader.GetEnum<CDefine.QuestType>(i, "Quest_Type", CDefine.QuestType.Main);
                    record.Quest_Id = reader.GetValue(i, "Quest_Id");
                    record.Next = reader.GetValue(i, "Next", 0);
                    record.Order = reader.GetValue<int>(i, "Order", 0);
                    record.ConditionID = reader.GetValue<int>(i, "Condition_Id", 0);
                    record.Unlock_Condition_ID = reader.GetValue<int>(i, "Unlock_Condition_Id", 0);
                    record.Reward_Id = reader.GetValue<int>(i, "Reward_Id", 0);
                    record.Reward_Pass_Id = reader.GetValue<int>(i, "Reward_Pass_Id", 0);

                    Add(record.ID, record);

                    var key = new KeyValuePair<CDefine.QuestType, string>(record.Quest_Type, record.Quest_Id);
                    if (m_FirstMisison.TryGetValue(key, out var missionRecord))
                    {
                        if (m_FirstMisison[key].Order > record.Order)
                            m_FirstMisison[key] = record;
                    }
                    else
                    {
                        m_FirstMisison[key] = record;
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

        public MissionRecord CopyFirstMission(CDefine.QuestType type, string questID)
        {
            var key = new KeyValuePair<CDefine.QuestType, string>(type, questID);
            if (m_FirstMisison.TryGetValue(key, out var retVal))
                return SCopy<MissionRecord>.DeepCopy(retVal);

            return null;
        }

    }

}