using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Global
{
    public class ConditionRecord : STableRecord<int>
    {
        public CDefine.MissionCondition Type;
        public string Param;
        public HashSet<string> Target = new HashSet<string>();
        public long Value;
        public string UnLock_String;

        public string Key() { return Key(Type, Param); }
        public static string Key(CDefine.MissionCondition type, string param) { return type.ToString() + param; }
        public bool IsValidTarget(string target)
        {
            return Target.Count < 1 || Target.Contains(target);
        }
        public bool IsSwapValue()
        {
            return IsSelfCheckable();
        }

        public bool IsSelfCheckable()
        {
            switch (Type)
            {
                case CDefine.MissionCondition.Player_Level:
                case CDefine.MissionCondition.Stage_Clear:
                case CDefine.MissionCondition.Growth_Gold:
                case CDefine.MissionCondition.Equip_Item:
                case CDefine.MissionCondition.Equip_Knight:
                case CDefine.MissionCondition.Equip_Skill:
                case CDefine.MissionCondition.SkillLearn:
                case CDefine.MissionCondition.Gacha_Item:
                case CDefine.MissionCondition.Growth_Level:
                    return true;
                default:
                    break;
            }

            return false;
        }
    }


    public class ConditionTable : STable<ConditionTable, int, ConditionRecord>
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

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    var record = new ConditionRecord();
                    record.ID = reader.GetValue<int>(i, "Id", 0);
                    record.Type = reader.GetEnum<CDefine.MissionCondition>(i, "Condition_Type", CDefine.MissionCondition.Max);
                    record.Param = reader.GetValue(i, "Condition_Param", "");
                    record.Target = reader.GetSet<string>(i, "Condition_Target");
                    record.Value = reader.GetValue<long>(i, "Condition_Value", long.MaxValue);
                    record.UnLock_String = reader.GetValue(i, "UnLock_String", "");


                    if (!Add(record.ID, record))
                    {
                        CommonLogger.Error($"{GetType().Name} - ID : {record.ID}");
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
    }

}
