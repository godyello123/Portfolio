using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global;
using SCommon;

namespace PlayServer
{
    public class CStageAgent
    {
        public class CStage
        {
            public _StageData m_Data = new _StageData();

            public CStage(_StageData data)
            {
                m_Data = data;
            }
        }

        class ProgressStage
        {
            public CDefine.eStageType m_Type = CDefine.eStageType.Max;
            public string m_StageTID = string.Empty;
            public bool m_IsStart = false;
        }

        private CUser m_Owner;
        private Dictionary<CDefine.eStageType, CStage> m_Stages = new Dictionary<CDefine.eStageType, CStage>();
        private ProgressStage m_ProgressStage = new ProgressStage();

        public CStageAgent(CUser owner) { m_Owner = owner; }

        public List<_StageData> GetList()
        {
            List<_StageData> retList = new List<_StageData>();
            foreach (var it in m_Stages)
            {
                var stage = it.Value;
                retList.Add(stage.m_Data);
            }

            return retList;
        }

        protected void Prepare()
        {
            var defaultDatas = StageTable.Instance.GetDefaultStageData();
            foreach (var iter in defaultDatas)
                m_Stages[iter.type] = new CStage(iter);

            m_ProgressStage.m_Type = CDefine.eStageType.Main_Stage;
            m_ProgressStage.m_IsStart = false;
        }

        public CStage Find(CDefine.eStageType type)
        {
            if (m_Stages.TryGetValue(type, out var ret))
                return ret;

            return null;
        }

        public void Init(List<_StageData> stagedatas)
        {
            Prepare();

            foreach (var iter in stagedatas)
            {
                if (m_Stages.TryGetValue(iter.type, out var stage))
                    stage.m_Data = iter;
                else
                    m_Stages.Add(iter.type, new CStage(iter));
            }
        }

        public void SaveAtLogout()
        {
        }

        public Packet_Result.Result ReqStageStart(long sessionKey, int stageTID)
        {
            //valid
            var stageRecord = StageTable.Instance.Find(stageTID);
            if (stageRecord == null)
                return Packet_Result.Result.PacketError;

            var stage = Find(stageRecord.Type);
            if (stage == null)
                return Packet_Result.Result.PacketError;

            stage.m_Data.curTID = stageTID;

            //todo : progress stage check
            //m_ProgressStage.m_StageTID = stage.m_Data.curTID;
            //m_ProgressStage.m_Type = stageRecord.Type;
            //m_ProgressStage.m_IsStart = true;
            
            //todo condition
            //if(!m_Owner.IsCondition(stageRecord.ConditionID))
            //{
            //    return Packet_Result.Result.ConditionCheckError;
            //}

            //todo : 던전 입장 재화 확인 필요

            if (stageRecord.Type == CDefine.eStageType.Event_Stage)
            {
                CNetManager.Instance.P2C_ResultStageStart(sessionKey, stage.m_Data, Packet_Result.Result.Success);
                return Packet_Result.Result.Success;
            }

            CLogger.Instance.Debug($"Stage Start : {stageRecord.Type} : {stage.m_Data.curTID}");

            CNetManager.Instance.P2C_ResultStageStart(sessionKey, stage.m_Data, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }
        public Packet_Result.Result ReqUpgradeStageStart(long sessionKey, int stageTID)
        {
            //valid
            var stageRecord = StageTable.Instance.Find(stageTID);
            if (stageRecord == null)
                return Packet_Result.Result.PacketError;

            if (stageRecord.Type != CDefine.eStageType.UpGrade_Stage)
                return Packet_Result.Result.PacketError;

            var stage = Find(stageRecord.Type);
            if (stage == null)
                return Packet_Result.Result.PacketError;

            if (stage.m_Data.maxTID >= stageTID)
                return Packet_Result.Result.PacketError;

            //todo : condition
           
            stage.m_Data.curTID = stageTID;

            //todo : progress stage check
            //m_ProgressStage.m_StageTID = stage.m_Data.curTID;
            //m_ProgressStage.m_Type = stageRecord.Type;
            //m_ProgressStage.m_IsStart = true;

            CLogger.Instance.Debug($"ReqUpgradeStageStart : {stageRecord.Type} : {stage.m_Data.curTID}");

            CNetManager.Instance.P2C_ResultUpgradeStageStart(sessionKey, stage.m_Data, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }
        
        public Packet_Result.Result ReqStageClear(long sessionKey, int stageTID, bool isClear)
        {
            var stageRecord = StageTable.Instance.Find(stageTID);
            if (stageRecord == null)
            {
                CLogger.Instance.Debug($"ReqStageClear stageRecord  NULL");
                return Packet_Result.Result.PacketError;
            }

            var userData = m_Owner.UserData;
            if (userData == null)
                return Packet_Result.Result.PacketError;

            //only prologe
            if (stageRecord.Type == CDefine.eStageType.Event_Stage && userData.m_EventStageID != 0)
                return ReqPrologueStageClear(sessionKey, stageRecord, isClear);
            
            //todo upgrade stage
            switch (stageRecord.Type)
            {
                case CDefine.eStageType.Main_Stage:
                    return ReqMainStageClear(sessionKey, stageRecord, isClear);
                case CDefine.eStageType.Gold_Stage:
                case CDefine.eStageType.Enchant_Stage:
                case CDefine.eStageType.Knights_Stage:
                case CDefine.eStageType.Chivalry_Stage:
                case CDefine.eStageType.Relic_Stage:
                case CDefine.eStageType.Game_Stage:                
                    return ReqDungeonClear(sessionKey, stageRecord, isClear);
                default:
                    break;
            }

            return Packet_Result.Result.PacketError;
        }

        public Packet_Result.Result ReqUpgradeStageClear(long sessionKey, int stageTID, bool isClear)
        {
            //progress stage


            var record = StageTable.Instance.Find(stageTID);
            if (record == null)
                return Packet_Result.Result.InValidRecord;

            if (record.Type != CDefine.eStageType.UpGrade_Stage)
                return Packet_Result.Result.PacketError;

            var stageData = Find(record.Type);
            if (stageData == null)
                return Packet_Result.Result.InValidData;

            var stage = Find(record.Type);
            if (stage == null)
            {
                CLogger.Instance.Debug($"ReqStageClear stageData NULL");
                return Packet_Result.Result.PacketError;
            }

            if (stage.m_Data.curTID != record.ID)
            {
                CLogger.Instance.Debug($"ReqStageClear stage.m_Data.curTID : {stage.m_Data.curTID} / stageTID : {record.ID} ");
                return Packet_Result.Result.PacketError;
            }

            var userData = m_Owner.UserData;
            if (userData == null)
                return Packet_Result.Result.PacketError;

            var rewards = RewardTable.Instance.Find(record.RewardID);
            if (rewards == null)
            {
                CLogger.Instance.Debug($"ReqStageClear rewards NULL");
                return Packet_Result.Result.PacketError;
            }

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            if (isClear)
            {
                var gotoRecord = StageTable.Instance.Find(record.NextStage);
                if (gotoRecord == null)
                    gotoRecord = record; 

                stage.m_Data.totalCnt++;

                if (stage.m_Data.maxTID < stage.m_Data.curTID)
                    stage.m_Data.maxTID = stage.m_Data.curTID;

                stage.m_Data.curTID = gotoRecord.ID;

                foreach (var iter in rewards)
                {
                    var reward = iter.Value;
                    forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                }

                forClient.UpdateRewardData(m_Owner, ref dbtran, true);
                dbtran.Merge();

                //승급 스킨 자동 장착
                foreach (var iter in dbtran.m_UpdateItemList)
                    m_Owner.ItemAgent.AutoItemEquip(iter);

                m_Owner.StatusAgent.RefreshAll();

                m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Stage_Clear, "", record.Type.ToString(), stage.m_Data.maxTID);
            }
            else
            {
                var failStageRecord = StageTable.Instance.Find(record.GotoFailStage);
                if (failStageRecord == null)
                {
                    CLogger.Instance.Debug($"ReqStageClear failStageRecord NULL");
                    return Packet_Result.Result.PacketError;
                }

                stage.m_Data.loop = true;
                stage.m_Data.curTID = record.GotoFailStage;
            }

            CDBManager.Instance.QueryCharacterUpdateUpgradeStage(m_Owner.DBGUID, sessionKey, userData, stage.m_Data, forClient, dbtran, isClear);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.stage_clear, m_Owner.UserData, SJson.ObjectToJson(stage.m_Data), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ReqPrologueStageClear(long sessionKey, StageRecord record, bool isClear)
        {
            var stage = Find(record.Type);
            if (stage == null)
            {
                CLogger.Instance.Debug($"ReqStageClear stageData NULL");
                return Packet_Result.Result.PacketError;
            }

            var userData = m_Owner.UserData;
            if (userData == null)
                return Packet_Result.Result.PacketError;

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            //only prologue

            if (isClear)
            {
                var mainStage = Find(CDefine.eStageType.Main_Stage);
                if (mainStage == null)
                    return Packet_Result.Result.PacketError;

                forClient.UpdateRewardData(m_Owner, ref dbtran, true);
                dbtran.Merge();

                userData.m_EventStageID = 0;
                
                stage.m_Data.maxTID = stage.m_Data.curTID;

                m_Owner.StatusAgent.RefreshAll();

                m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Stage_Clear, "", record.Type.ToString(), record.ID);
                CDBManager.Instance.QueryCharacterStageSave(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, stage.m_Data);
                CDBManager.Instance.QueryCharacterUpdateStage(m_Owner.DBGUID, sessionKey, userData, mainStage.m_Data, forClient, dbtran, isClear);
            }
            else
            {
                var failStageRecord = StageTable.Instance.Find(record.GotoFailStage);
                if (failStageRecord == null)
                {
                    CLogger.Instance.Debug($"ReqStageClear failStageRecord NULL");
                    return Packet_Result.Result.PacketError;
                }

                stage.m_Data.loop = true;
                stage.m_Data.curTID = record.GotoFailStage;
            }

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ReqMainStageClear(long sessionKey, StageRecord record, bool isClear)
        {
            var stage = Find(record.Type);
            if (stage == null)
            {
                CLogger.Instance.Debug($"ReqStageClear stageData NULL");
                return Packet_Result.Result.PacketError;
            }

            var userData = m_Owner.UserData;
            if (userData == null)
                return Packet_Result.Result.PacketError;

            if (stage.m_Data.curTID != record.ID)
            {
                CLogger.Instance.Debug($"ReqStageClear stage.m_Data.curTID : {stage.m_Data.curTID} / stageTID : {record.ID} ");
                return Packet_Result.Result.PacketError;
            }

            CLogger.Instance.Debug($"ReqStageClear IsValid Complete!");

            //process
            var rewards = RewardTable.Instance.Find(record.RewardID);
            if (rewards == null)
            {
                CLogger.Instance.Debug($"ReqStageClear rewards NULL");
                return Packet_Result.Result.PacketError;
            }

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            if (isClear)
            {
                if (stage.m_Data.maxTID < stage.m_Data.curTID)
                    stage.m_Data.maxTID = stage.m_Data.curTID;

                var maxRecord = StageTable.Instance.Find(stage.m_Data.maxTID);
                if (maxRecord == null)
                    return Packet_Result.Result.PacketError;

                var gotoRecord = StageTable.Instance.GetNextStage(maxRecord);
                if (gotoRecord == null)
                    return Packet_Result.Result.PacketError;

                stage.m_Data.totalCnt++;

                if (stage.m_Data.loop)
                {
                    stage.m_Data.curTID = record.ID;
                }
                else
                {
                    stage.m_Data.curTID = gotoRecord.ID;
                }

                foreach (var iter in rewards)
                {
                    var reward = iter.Value;
                    if (reward.AssetType == CDefine.AssetType.Coin)
                    {
                        long rewardValue = m_Owner.CorrectCoinValue(reward.RewardID, reward.Value);
                        forClient.Insert(reward.AssetType, reward.RewardID, rewardValue);
                    }
                    else
                    {
                        forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                    }
                }

                m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Stage_Clear, "", record.Type.ToString(), stage.m_Data.maxTID);
            }
            else
            {
                var failStageRecord = StageTable.Instance.Find(record.GotoFailStage);
                if (failStageRecord == null)
                {
                    CLogger.Instance.Debug($"ReqStageClear failStageRecord NULL");
                    return Packet_Result.Result.PacketError;
                }

                stage.m_Data.loop = true;
                stage.m_Data.curTID = record.GotoFailStage;
            }

            forClient.UpdateRewardData(m_Owner, ref dbtran, true);
            dbtran.Merge();


            //overview & rank update
            m_Owner.RefreshUserOverView();
            CNetManager.Instance.P2M_ReportUserRankUpdate(m_Owner.UID, CDefine.ERankType.Rank_MainStage);

            //db
            CDBManager.Instance.QueryCharacterUpdateStage(m_Owner.DBGUID, sessionKey, userData, stage.m_Data, forClient, dbtran, isClear);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ReqDungeonClear(long sessionKey, StageRecord record, bool isClear)
        {
            var stageData = Find(record.Type);
            if (stageData == null)
                return Packet_Result.Result.InValidData;

            var stage = Find(record.Type);
            if (stage == null)
            {
                CLogger.Instance.Debug($"ReqStageClear stageData NULL");
                return Packet_Result.Result.PacketError;
            }

            if (stage.m_Data.curTID != record.ID)
            {
                CLogger.Instance.Debug($"ReqStageClear stage.m_Data.curTID : {stage.m_Data.curTID} / stageTID : {record.ID} ");
                return Packet_Result.Result.PacketError;
            }

            var userData = m_Owner.UserData;
            if (userData == null)
                return Packet_Result.Result.PacketError;

            var rewards = RewardTable.Instance.Find(record.RewardID);
            if (rewards == null)
            {
                CLogger.Instance.Debug($"ReqStageClear rewards NULL");
                return Packet_Result.Result.PacketError;
            }

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            if (isClear)
            {
                var gotoRecord = StageTable.Instance.GetNextStage(record);
                if (gotoRecord == null)
                {
                    CLogger.Instance.Debug($"ReqStageClear gotoRecord NULL");
                    return Packet_Result.Result.PacketError;
                }

                stage.m_Data.totalCnt++;

                if (stage.m_Data.maxTID < stage.m_Data.curTID)
                    stage.m_Data.maxTID = stage.m_Data.curTID;

                stage.m_Data.curTID = gotoRecord.ID;

                foreach (var iter in rewards)
                {
                    var reward = iter.Value;
                    forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                }

                forClient.UpdateRewardData(m_Owner, ref dbtran, true);
                dbtran.Merge();

                m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Stage_Clear, "", record.Type.ToString(), stage.m_Data.maxTID);
            }
            else
            {
                var failStageRecord = StageTable.Instance.Find(record.GotoFailStage);
                if (failStageRecord == null)
                {
                    CLogger.Instance.Debug($"ReqStageClear failStageRecord NULL");
                    return Packet_Result.Result.PacketError;
                }

                stage.m_Data.loop = true;
                stage.m_Data.curTID = record.GotoFailStage;
            }

            CDBManager.Instance.QueryCharacterUpdateStage(m_Owner.DBGUID, sessionKey, userData, stage.m_Data, forClient, dbtran, isClear);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.stage_clear, m_Owner.UserData, SJson.ObjectToJson(stage.m_Data), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqStageSweep(long sessionKey, CDefine.eStageType type, int stageID, int count)
        {
            //valid
            if(count < 1 || type == CDefine.eStageType.Main_Stage)
            {
                CLogger.Instance.System($"ReqStageSweep count < 1 : {count}");
                return Packet_Result.Result.PacketError;
            }

            var stage = Find(type);
            if (stage == null)
            {
                CLogger.Instance.System($"ReqStageSweep  stage null : {type}");
                return Packet_Result.Result.PacketError;
            }

            var stageData = stage.m_Data;
            if(stageData == null)
            {
                CLogger.Instance.System($"ReqStageSweep  stageData null : {type}");
                return Packet_Result.Result.PacketError;
            }

            var stageRecord = StageTable.Instance.Find(stageID);
            if(stageRecord == null)
            {
                CLogger.Instance.System($"ReqStageSweep stageRecord == null {stageID}");
                return Packet_Result.Result.InValidRecord;
            }

            if(stageData.maxTID < stageID)
            {
                CLogger.Instance.System($"ReqStageSweep if(maxTID < sweepID) : {stageData.maxTID}");
                return Packet_Result.Result.InValidData;
            }

            if (!m_Owner.HasEnoughAsset(CDefine.AssetType.Coin, stageRecord.CoinType.ToString(), stageRecord.CoinCount * count))
                return Packet_Result.Result.LackAssetError;

            var rewards = RewardTable.Instance.Find(stageRecord.Sweep_RewardID); 
            if (rewards == null)
                return Packet_Result.Result.PacketError;

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            foreach (var iter in rewards)
            {
                var reward = iter.Value;
                long val = m_Owner.SweepIncreaseValue(reward.Value, stageRecord.Type) * count;
                forClient.Insert(reward.AssetType, reward.RewardID, val);
            }

            bool bSend = true;
            _AssetData cost = new _AssetData(CDefine.AssetType.Coin, stageRecord.CoinType.ToString(), -stageRecord.CoinCount * count);
            m_Owner.UpdateAssetData(cost, ref dbtran, bSend);

            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
            dbtran.Merge();

            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Use_Coin, "", stageRecord.CoinType.ToString(), count);

            CDBManager.Instance.QueryCharacterStageSweep(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, forClient, dbtran);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.stage_sweep, m_Owner.UserData, SJson.ObjectToJson(stage.m_Data), dbtran, count);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public void AfterQueryStageSweap(long sessionKey, CRewardInfo forClient, CDBMerge dbtran, Packet_Result.Result result)
        {
            m_Owner.ReportAssetData(dbtran);
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            CNetManager.Instance.P2C_ResultStageSweep(sessionKey, result);
        }

        public void AfterQueryStageClear(long sessionKey, CRewardInfo forClient, _StageData stageData, bool isClear, Packet_Result.Result result, _UserData userData)
        {
            CNetManager.Instance.P2C_ReportUserData(sessionKey, userData);
            CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
            CNetManager.Instance.P2C_ResultStageClear(sessionKey, stageData, isClear, forClient.GetList(), result);
        }

        public void AfterQueryUpgradeStageClear(long sessionKey, CRewardInfo forClient, _StageData stageData, bool isClear, Packet_Result.Result result, _UserData userData)
        {
            CNetManager.Instance.P2C_ReportUserData(sessionKey, userData);
            CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
            CNetManager.Instance.P2C_ResultUpgradeStageClear(sessionKey, stageData, isClear, forClient.GetList(), result);
        }

        public Packet_Result.Result ReqStageLoopChange(long _sessionKey, CDefine.eStageType type, bool isLoop)
        {
            var stageData = Find(type);
            if(stageData == null)
            {
                CLogger.Instance.Error("stageData is Null");
                return Packet_Result.Result.InValidData;
            }

            stageData.m_Data.loop = isLoop;

            CNetManager.Instance.P2C_ResultStageLoopChange(_sessionKey, stageData.m_Data, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

    }
}
