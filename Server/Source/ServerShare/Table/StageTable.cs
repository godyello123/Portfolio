using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class StageRecord : STableRecord<int>
    {
        public CDefine.eStageType Type = CDefine.eStageType.Max;
        public int Chapter = 0;
        public int Stage = 0;
        public int PrevStage = -1;
        public int NextStage = -1;
        public int GotoFailStage = -1;
        public int ConditionID = 0;
        public int ClearTime_Min = 0;
        public int ClearTime = 0;
        public CDefine.CoinType CoinType = CDefine.CoinType.Max;
        public int CoinCount = 0;
        public int RewardID = 0;
        public int Sweep_RewardID = 0;
        public int Fruition_RewardID = 0;
    }

    public class StageTable : STable<StageTable, int, StageRecord>
    {
        private Dictionary<CDefine.eStageType, Dictionary<int, StageRecord>> m_StageDictionary = new Dictionary<CDefine.eStageType, Dictionary<int, StageRecord>>();
        private List<_StageData> m_DefaultStageDatas = new List<_StageData>();
        
        private Dictionary<CDefine.eStageType, StageRecord> m_EndStages = new Dictionary<CDefine.eStageType, StageRecord>();

        public override void Prepare()
        {
            foreach (var iter in m_StageDictionary)
            {
                var records = iter.Value;
                for (CDefine.eStageType type = CDefine.eStageType.Main_Stage; type < CDefine.eStageType.Max; ++type)
                {
                    var findit = records.FirstOrDefault(x => x.Value.Type == type);
                    var findrecord = findit.Value;
                    if (findrecord == null)
                        continue;

                    _StageData stageData = new _StageData();
                    stageData.type = findrecord.Type;
                    stageData.curTID = findrecord.ID;
                    stageData.maxTID = 0;
                    stageData.totalCnt = 0;

                    if (stageData.type != CDefine.eStageType.Main_Stage)
                        stageData.loop = true;
                    else
                        stageData.loop = false;

                    m_DefaultStageDatas.Add(stageData);
                }
            }
        }

        public StageRecord GetNextStage(StageRecord record)
        {
            var nextStage = Find(record.NextStage);
            if (nextStage == null)
                return record;

            return nextStage;
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
                    StageRecord record = new StageRecord();
                    record.ID = reader.GetValue(i, "ID", -1);
                    record.Type = reader.GetEnum(i, "Type", Global.CDefine.eStageType.Max);
                    record.Chapter = reader.GetValue(i, "Chapter", 0);
                    record.Stage = reader.GetValue(i, "Stage", 0);
                    record.ConditionID = reader.GetValue(i, "ConditionID", 0);
                    record.PrevStage = reader.GetValue(i, "PrevStage", -1);
                    record.NextStage = reader.GetValue(i, "NextStage", -1);
                    record.GotoFailStage = reader.GetValue(i, "GotoFailStage", -1);
                    record.ClearTime_Min = reader.GetValue(i, "ClearTime_Min", 0);
                    record.ClearTime = reader.GetValue<int>(i, "ClearTime", 0);
                    record.CoinType = reader.GetEnum<CDefine.CoinType>(i, "CoinType", CDefine.CoinType.Max);
                    record.CoinCount = reader.GetValue<int>(i, "CoinCount", 0);
                    record.RewardID = reader.GetValue<int>(i, "RewardID", 0);
                    record.Fruition_RewardID = reader.GetValue<int>(i, "Fruition_RewardID", 0);
                    record.Sweep_RewardID = reader.GetValue<int>(i, "Sweep_RewardID", 0);

                    Add(record.ID, record);

                    if (m_StageDictionary.TryGetValue(record.Type, out var dics))
                    {
                        if (dics.TryGetValue(record.ID, out var data))
                            data = record;
                        else
                            dics.Add(record.ID, record);
                    }
                    else
                    {
                        Dictionary<int, StageRecord> dic = new Dictionary<int, StageRecord>();
                        dic.Add(record.ID, record);
                        m_StageDictionary.Add(record.Type, dic);
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
        
        
        public List<_StageData> GetDefaultStageData()
        {
            List<_StageData> retList = new List<_StageData>();
            foreach (var iter in m_DefaultStageDatas)
                retList.Add(new _StageData(iter));

            return retList;
        }

        public StageRecord FindRecord(CDefine.eStageType type, int id)
        {
            if(m_StageDictionary.TryGetValue(type, out var records))
            {
                if (records.TryGetValue(id, out var ret))
                    return ret;
            }

            return null;
        }
    }
}
