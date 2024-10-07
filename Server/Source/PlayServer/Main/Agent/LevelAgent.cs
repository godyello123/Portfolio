using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Global;
using SCommon;
using SDB;

namespace PlayServer
{
    public class CLevelAgent
    {
        private CUser m_Owner;
        
        public CLevelAgent(CUser owner) { m_Owner = owner; }

        private Dictionary<int, _LevelData> m_GoldGrowthStats = new Dictionary<int, _LevelData>();
        private Dictionary<int, _LevelData> m_LevelGrowthStats = new Dictionary<int, _LevelData>();

        public void Init(List<_LevelData> goldLevels, List<_LevelData> levelDatas)
        {
            Prepare();

            foreach(var iter in goldLevels)
                m_GoldGrowthStats[iter.m_TableID] = iter;

            foreach (var iter in levelDatas)
                m_LevelGrowthStats[iter.m_TableID] = iter;

            Begin();
        }

        private void Prepare()
        {
            var goldGrowthDefault = GrowthGoldTable.Instance.CopyDefault();
            foreach (var iter in goldGrowthDefault)
                m_GoldGrowthStats[iter.Value.m_TableID] = iter.Value;

            var levelGrowthDefault = GrowthLevelTable.Instance.CopyDefault();
            foreach (var iter in levelGrowthDefault)
                m_LevelGrowthStats[iter.Value.m_TableID] = iter.Value;
        }

        private void Begin()
        {

        }

        public List<_LevelData> GetGoldGrowthList()
        {
            return new List<_LevelData>(m_GoldGrowthStats.Values);
        }

        public List<_LevelData> GetLevelGrowthList()
        {
            return new List<_LevelData>(m_LevelGrowthStats.Values);
        }


        public _LevelData GetGoldGrowth(int groupID)
        {
            if (m_GoldGrowthStats.TryGetValue(groupID, out var retVal))
                return retVal;

            return null;
        }

        public _LevelData GetLevelGrowth(int groupID)
        {
            if (m_LevelGrowthStats.TryGetValue(groupID, out var retVal))
                return retVal;

            return null;
        }


        public void GetAbilGoldStatus(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            foreach(var iter in m_GoldGrowthStats)
            {
                var record = GrowthGoldTable.Instance.Find(iter.Value.m_TableID, iter.Value.m_UseCount);
                if (record == null)
                    continue;

                var calcAbil = new _AbilData();
                calcAbil.type = record.abil_type;
                calcAbil.val = record.total_value;

                CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
            }
        }
        public void GetAbilLvStatus(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            foreach(var iter in m_LevelGrowthStats)
            {
                var growthLevel = iter.Value;
                var record = GrowthLevelTable.Instance.Find(growthLevel.m_TableID);
                if (record == null)
                    continue;

                var calcAbil = new _AbilData();
                calcAbil.type = record.abil_type;
                calcAbil.val = record.up_value * growthLevel.m_UseCount;

                CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
            }
        }

        public Packet_Result.Result ReqGoldGrowthUp(long sessionKey, int groupID, int level)
        {
            //valid
            if (level < 1)
            {
                CLogger.Instance.Debug($"ReqGoldGrowthUp level < 1");
                return Packet_Result.Result.PacketError;
            }

            var curData = GetGoldGrowth(groupID);
            if (curData == null)
            {
                CLogger.Instance.Debug($"ReqGoldGrowthUp curData NULL");
                return Packet_Result.Result.PacketError;
            }

            var curRecord = GrowthGoldTable.Instance.Find(groupID, curData.m_UseCount);
            if (curRecord == null)
            {
                CLogger.Instance.Debug($"ReqGoldGrowthUp curRecord NULL");
                return Packet_Result.Result.PacketError;
            }

            var targetRecord = GrowthGoldTable.Instance.Find(groupID, level);
            if (targetRecord == null)
            {
                CLogger.Instance.Debug($"ReqGoldGrowthUp targetRecord NULL");
                return Packet_Result.Result.PacketError;
            }

            var asset = m_Owner.AssetAgent.Find(targetRecord.AssetType);
            if (asset == null)
            {
                CLogger.Instance.Debug($"ReqGoldGrowthUp asset NULL");
                return Packet_Result.Result.PacketError;
            }

            long needVal = targetRecord.AssetValue - curRecord.AssetValue;

            if (needVal < 1)
            {
                CLogger.Instance.Debug($"ReqGoldGrowthUp needVal < 1");
                return Packet_Result.Result.PacketError;
            }

            if (asset.Count < needVal)
            {
                CLogger.Instance.Debug($"ReqGoldGrowthUp asset.Count {asset.Count} / needVal : {needVal}");
                return Packet_Result.Result.LackAssetError;
            }

            CDBMerge dbtran = new CDBMerge();
            m_Owner.UpdateAssetData(new _AssetData(CDefine.AssetType.Coin, asset.TableID, -needVal), ref dbtran);
            curData.m_UseCount = targetRecord.Level;

            dbtran.Merge();

            CDBManager.Instance.QueryCharacterUpdateGoldGrowth(m_Owner.DBGUID, sessionKey, m_Owner.UID, curData, dbtran);

            //todo : log save bson
            var log = LogHelper.MakeLogBson(eLogType.growth_gold, m_Owner.UserData, LogHelper.ToJson(curData), dbtran, targetRecord.Level - curRecord.Level);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public void AfterQueryGoldGrowthUp(long sessionKey, Packet_Result.Result result, _LevelData levelData, List<_AssetData> assetDatas)
        {
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Growth_Gold, "", levelData.m_TableID.ToString(), levelData.m_UseCount);
            m_Owner.StatusAgent.Refresh(new List<eStatusCategory> { eStatusCategory.StatusGold });

            CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
            CNetManager.Instance.P2C_ResultGrowthGoldLevelUp(sessionKey, result, levelData);
        }

        public Packet_Result.Result ReqLevelGrowthUp(long sessionKey, int tableID, int useCnt)
        {
            //valid
            if (useCnt < 1)
            {
                CLogger.Instance.Debug($"ReqLevelGrowthUp useCnt : {useCnt}");
                return Packet_Result.Result.PacketError;
            }

            var curData = GetLevelGrowth(tableID);
            if (curData == null)
            {
                CLogger.Instance.Debug($"ReqLevelGrowthUp curData NULL");
                return Packet_Result.Result.InValidData;
            }

            var curRecord = GrowthLevelTable.Instance.Find(curData.m_TableID);
            if (curRecord == null)
            {
                CLogger.Instance.Debug($"ReqLevelGrowthUp curRecord NULL");
                return Packet_Result.Result.InValidRecord;
            }

            if (curRecord.max_point < useCnt)
                return Packet_Result.Result.PacketError;
            
            int needPoint = useCnt - curData.m_UseCount;
            if (needPoint < 1)
            {
                CLogger.Instance.Debug($"ReqLevelGrowthUp needPoint : {needPoint}");
                return Packet_Result.Result.PacketError;
            }

            var userData = m_Owner.UserData;
            if (userData == null)
            {
                CLogger.Instance.Debug($"ReqLevelGrowthUp userData NULL");
                return Packet_Result.Result.PacketError;
            }

            if (userData.m_LevelPoint < needPoint)
            {
                CLogger.Instance.Debug($"ReqLevelGrowthUp userData.m_LevelPoint : {userData.m_LevelPoint} / needPoint : {needPoint}");
                return Packet_Result.Result.LackAssetError;
            }

            //process
            curData.m_UseCount = useCnt;
            userData.m_LevelPoint -= needPoint;

            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Growth_Level, "", curData.m_TableID.ToString(), curData.m_UseCount);

            CDBManager.Instance.QueryCharacterUpdateLevelGrowth(m_Owner.DBGUID, sessionKey, curData, userData, Packet_C2P.Protocol.C2P_RequestLevelGrowthUp);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.growth_level, m_Owner.UserData, LogHelper.ToJson(curData), null, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public void AfterQueryLevelGrowthUp(long sessionKey, Packet_Result.Result result, _LevelData levelData, _UserData userData, Packet_C2P.Protocol protocol)
        {
            m_Owner.StatusAgent.Refresh(new List<eStatusCategory> { eStatusCategory.StatusLv });

            CNetManager.Instance.P2C_ReportUserData(sessionKey, userData);

            if (protocol == Packet_C2P.Protocol.C2P_RequestLevelGrowthUp)
                CNetManager.Instance.P2C_ResultLevelGrowthUp(sessionKey, levelData, result);
            else if (protocol == Packet_C2P.Protocol.C2P_RequestGrowthLevelPointReset)
                CNetManager.Instance.P2C_ResultGrowthLevelPointReset(sessionKey, levelData, result);
        }

        public Packet_Result.Result ReqGrowthLevelPointReset(long _sessionKey, int _tableID)
        {
            //valid
            var levelData = GetLevelGrowth(_tableID);
            if (levelData == null)
                return Packet_Result.Result.PacketError;

            var user = m_Owner.UserData;
            if (user == null)
                return Packet_Result.Result.PacketError;

            if (levelData.m_UseCount < 1)
                return Packet_Result.Result.PacketError;

            var userCoin = m_Owner.AssetAgent.Find(CDefine.CoinType.BlueDia);
            if (userCoin == null)
                return Packet_Result.Result.PacketError;

            int needResetCost = DefineTable.Instance.Value<int>("Level_Point_Reset_Cost");
            if (needResetCost < 1)
                return Packet_Result.Result.PacketError;

            if (userCoin.Count < needResetCost)
                return Packet_Result.Result.PacketError;

            //process
            int giveBackPoint = levelData.m_UseCount;
            user.m_LevelPoint += giveBackPoint;
            levelData.m_UseCount -= giveBackPoint;

            CDBManager.Instance.QueryCharacterUpdateLevelGrowth(m_Owner.DBGUID, _sessionKey, levelData, user, Packet_C2P.Protocol.C2P_RequestGrowthLevelPointReset);

            //log

            return Packet_Result.Result.Success;
        }

        public void CheatGoldLevelReet()
        {
            foreach (var iter in m_GoldGrowthStats)
            {
                
            }


        }
    }
}
