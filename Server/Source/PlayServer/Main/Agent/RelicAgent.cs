using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using SCommon;

namespace PlayServer
{
    public class CRelicAgent
    {
        private CUser m_Owner;
        private Dictionary<int, _RelicData> m_Relics = new Dictionary<int, _RelicData>();
        public CUser Owner { get => m_Owner; }

        public CRelicAgent(CUser owner) { m_Owner = owner; }

        public List<_RelicData> GetList()
        {
            return new List<_RelicData>(m_Relics.Values);
        }

        public _RelicData Find(int groupID)
        {
            if (m_Relics.TryGetValue(groupID, out var retVal))
                return retVal;

            return null;
        }

        public void GetAbilRelic(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            foreach(var iter in m_Relics)
            {
                var relic = iter.Value;
                var relicRecord = RelicTable.Instance.Find(relic.m_GroupID, relic.m_Level);
                if (relicRecord == null)
                    continue;

                CStatusAgent.UpsertAbil(ref rAbils, relicRecord.Abils);
            }
        }

        public void Init(List<_RelicData> relics)
        {
            m_Relics = RelicTable.Instance.CopyDefaults();
            foreach (var iter in relics)
                m_Relics[iter.m_GroupID] = iter;
        }

        public Packet_Result.Result ReqRelicEnchant(long sessionKey, int groupID)
        {
            //process
            var relic  = Find(groupID);
            if(relic == null)
            {
                CLogger.Instance.System($"ReqRelicEnchant : {groupID}");
                return Packet_Result.Result.PacketError;
            }

            var relicRecord = RelicTable.Instance.Find(groupID, relic.m_Level);
            if(relicRecord == null)
            {
                CLogger.Instance.System($"ReqRelicEnchant relicRecord == null : {groupID}, {relic.m_Level}");
                return Packet_Result.Result.PacketError;
            }

            //todo : condition

            var nextRecord = RelicTable.Instance.Find(groupID, relic.m_Level + 1);
            if(nextRecord == null)
            {
                CLogger.Instance.System($"ReqRelicEnchant nextRecord = null : {groupID}, {relic.m_Level}");
                return Packet_Result.Result.AlreadyLast;
            }

            if(!m_Owner.HasEnoughAsset(relicRecord.CostAsset.Type, relicRecord.CostAsset.TableID, relicRecord.CostAsset.Count))
            {
                CLogger.Instance.System($"ReqRelicEnchant LackAsset Error : {relicRecord.CostAsset.Count}");
                return Packet_Result.Result.LackAssetError;
            }

            //process
            CDBMerge dbtran = new CDBMerge();
            _AssetData consumeAsset = new _AssetData(relicRecord.CostAsset.Type, relicRecord.CostAsset.TableID, -relicRecord.CostAsset.Count);

            bool bSend = true;
            m_Owner.UpdateAssetData(consumeAsset, ref dbtran, bSend);

            int prob = relicRecord.Prob + relic.m_BonusProb;
            int max_prob = DefineTable.Instance.Value<int>("MaxProb");
            int rand_prob = SRandom.Instance.Next(0, max_prob);
            if (rand_prob <= prob)
            {
                relic.m_Level = nextRecord.Level;
                relic.m_BonusProb = 0;
            }
            else
            {
                int bonus_prob = relic.m_BonusProb + relicRecord.BonusProb;
                int max_bonus_prob = max_prob - relicRecord.Prob;
                relic.m_BonusProb = Math.Min(bonus_prob, max_bonus_prob);
            }

            //todo : mission

            m_Owner.StatusAgent.Refresh(new List<eStatusCategory> { eStatusCategory.Relic });

            CDBManager.Instance.QueryCharacterRelicEnchant(m_Owner.DBGUID, sessionKey, m_Owner.UID, SCopy<_RelicData>.DeepCopy(relic), dbtran);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.relic_enchant, m_Owner.UserData, LogHelper.ToJson(relic), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public void AfterQueryRelicEnchant(long sessionKey, _RelicData relic, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
            CNetManager.Instance.P2C_ResultRelicEnchant(sessionKey, relic, result);
        }
    }
    
}
