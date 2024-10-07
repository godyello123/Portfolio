using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;
using System.Windows.Forms;
using Amazon.Runtime.Internal.Util;
using Global;
using Microsoft.Extensions.Logging;
using ParquetSharp;
using SCommon;

namespace PlayServer
{
    public class CEventAgent
    {
        private STimer m_Timer = new STimer(30 * 3000);

        private CUser m_Owner;
        private Dictionary<long, _EventData> m_Events = new Dictionary<long, _EventData>();
        private List<long> m_ExpireList = new List<long>();

        private Dictionary<long, _EventRouletteData> m_Roulettes = new Dictionary<long, _EventRouletteData>();

        public CEventAgent(CUser owner)
        {
            m_Owner = owner;
        }

        public void Init(List<_EventData> eventDatas, List<_EventRouletteData> roulettesDatas)
        {
            foreach (var iter in eventDatas)
            {
                var record = EventTable.Instance.Find(iter.m_EventID);
                if (record == null)
                    continue;

                if (!CEventManager.Instance.isExist(iter.m_UID))
                    continue;

                if (SDateManager.Instance.IsExpired(iter.m_EndDate))
                {
                    m_ExpireList.Add(iter.m_UID);
                    continue;
                }

                m_Events[iter.m_UID] = iter;

                switch (record.EventType)
                {
                    case CDefine.EventType.CoinEvent:
                        m_Owner.ShopAgent.InsertEvent(iter.m_UID, iter.m_EventID, false);
                        break;
                    case CDefine.EventType.QuestEvent:
                        m_Owner.QuestAgent.InsertEvent(record.QuestID, iter.m_EndDate, false);
                        break;
                    case CDefine.EventType.RouletteEvent:
                        InsertRoulette(iter.m_UID, iter.m_EventID, false);
                        break;
                }
            }

            foreach (var iter in roulettesDatas)
            {
                var eventData = FindEvent(iter.m_EventUID);
                if (eventData == null) continue;

                if (SDateManager.Instance.IsExpired(eventData.m_EndDate))
                    continue;

                m_Roulettes[iter.m_EventUID] = iter;
            }
        }

        public void Refresh(bool bSend)
        {
            var active_events = CEventManager.Instance.GetActiveList();
            foreach (var iter in active_events)
            {
                var findData = FindEvent(iter.m_UID);
                if (findData != null)
                    continue;

                if (m_ExpireList.Contains(iter.m_UID))
                    continue;

                InsertEvent(iter.m_UID, iter.m_ID);
            }
        }

        public void InsertEvent(long eventuid, int eventid)
        {
            var record = EventTable.Instance.Find(eventid);
            if (record == null)
                return;

            _EventData eventData = new _EventData();
            eventData.m_UID = eventuid;
            eventData.m_EventID = eventid;
            eventData.m_StartDate = SDateManager.Instance.CurrTime();
            eventData.m_EndDate = SDateManager.Instance.CurrTime() + record.DuringTime;

            m_Events[eventData.m_UID] = eventData;

            CNetManager.Instance.P2C_ReportEventData(m_Owner.SessionKey, eventData, CDefine.Modify.Add);

            switch(record.EventType)
            {
                case CDefine.EventType.CoinEvent:
                    m_Owner.ShopAgent.InsertEvent(eventData.m_UID, eventData.m_EventID, true);
                    break;
                case CDefine.EventType.QuestEvent:
                    m_Owner.QuestAgent.InsertEvent(record.QuestID, eventData.m_EndDate, true);
                    break;
                case CDefine.EventType.RouletteEvent:
                    InsertRoulette(eventData.m_UID, eventid, true);
                    break;
            }

            CDBManager.Instance.QueryCharacterEventUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, eventData, record.EventDropCoinID);
        }

        public _EventData FindEvent(long eventUID)
        {
            if (m_Events.TryGetValue(eventUID, out _EventData retVal))
                return retVal;

            return null;
        }

        public _EventRouletteData FindRoulette(long eventUID)
        {
            if (m_Roulettes.TryGetValue(eventUID, out var retVal))
                return retVal;

            return null;
        }

        public List<_EventData> GetEventList()
        {
            var retVal = new List<_EventData>();
            foreach (var iter in m_Events)
                retVal.Add(iter.Value);

            return retVal;
        }

        public List<_EventRouletteData> GetEventRouletteList()
        {
            var retVal = new List<_EventRouletteData>();
            foreach (var iter in m_Roulettes)
                retVal.Add(iter.Value);

            return retVal;
        }

        public void Update()
        {
            if (!m_Timer.Check())
                return;

            foreach (var iter in m_Events)
            {
                var eventData = iter.Value;

                if (!CEventManager.Instance.isExist(eventData.m_UID))
                    m_ExpireList.Add(eventData.m_UID);

                if (SDateManager.Instance.IsExpired(eventData.m_EndDate))
                    m_ExpireList.Add(eventData.m_UID);
            }

            foreach(var iter in m_ExpireList)
            {
                var expireData = FindEvent(iter);
                if (expireData == null)
                    continue;

                var eventRecord = EventTable.Instance.Find(expireData.m_EventID);
                if (eventRecord == null)
                    continue;

                if (eventRecord.EventType == CDefine.EventType.CoinEvent)
                {
                    
                }
                else if(eventRecord.EventType == CDefine.EventType.QuestEvent)
                {

                }

                m_Events.Remove(iter);
            }

            Refresh(true);
        }

        private void InsertRoulette(long eventUID, int eventID, bool bSend)
        {
            if (m_Roulettes.ContainsKey(eventUID))
                return;

            _EventRouletteData data = new _EventRouletteData();
            data.m_EventUID = eventUID;

            m_Roulettes.Add(eventUID, data);

            if (bSend)
                CNetManager.Instance.P2C_ReportEventRoulette(m_Owner.SessionKey, data, CDefine.Modify.Add);
        }

        public Packet_Result.Result ReqEventRouletteReward(long sessionKey, long eventUID)
        {
            var foundEventData = FindEvent(eventUID);
            if (foundEventData == null)
                return Packet_Result.Result.InValidData;

            if (SDateManager.Instance.IsExpired(foundEventData.m_EndDate))
                return Packet_Result.Result.PacketError;

            var eventRecord = EventTable.Instance.Find(foundEventData.m_EventID);
            if (eventRecord == null)
                return Packet_Result.Result.InValidRecord;

            if (eventRecord.EventType != CDefine.EventType.RouletteEvent)
                return Packet_Result.Result.PacketError;

            var rouletteData = FindRoulette(eventUID);
            if (rouletteData == null)
                return Packet_Result.Result.InValidData;

            var rouletteRecord = EventRouletteTable.Instance.Findfirst(foundEventData.m_EventID);
            if (rouletteRecord == null)
                return Packet_Result.Result.InValidRecord;

            if (!m_Owner.HasEnoughAsset(CDefine.AssetType.Coin, rouletteRecord.CostCoinType.ToString(), rouletteRecord.CostValue))
                return Packet_Result.Result.LackAssetError;

            var pickedRecord = EventRouletteTable.Instance.Roullet(eventRecord.ID, rouletteData.m_ExcludeIndexs);
            if (pickedRecord == null)
                return Packet_Result.Result.PacketError;

            var rewards = RewardTable.Instance.Find(pickedRecord.RewardID);
            if (rewards == null)
                return Packet_Result.Result.PacketError;

            rouletteData.m_ExcludeIndexs.Add(pickedRecord.Index);

            CDBMerge dbtran = new CDBMerge();
            CRewardInfo forClient = new CRewardInfo();

            bool bSend = true;
            _AssetData cost = new _AssetData(CDefine.AssetType.Coin, rouletteRecord.CostCoinType.ToString(), -rouletteRecord.CostValue);
            m_Owner.UpdateAssetData(cost, ref dbtran, bSend);

            foreach(var iter in rewards)
            {
                var itVal = iter.Value;
                forClient.Insert(itVal.AssetType, itVal.RewardID, itVal.Value);
            }

            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
            dbtran.Merge();

            m_Owner.ReportAssetData(dbtran);

            CDBManager.Instance.QueryCharacterEventRouletteReward(m_Owner.DBGUID, sessionKey, m_Owner.UID, pickedRecord.Index, rouletteData, forClient, dbtran);

            var log = LogHelper.MakeLogBson(eLogType.event_roulette, m_Owner.UserData, SCommon.SJson.ObjectToJson(rouletteData), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public void AfterQueryEventRouletteReward(long sessionKey, int pickedIndex, _EventRouletteData rouletteData, CRewardInfo forClient, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            CNetManager.Instance.P2C_ResultEventRouletteReward(sessionKey, pickedIndex, rouletteData, result);
        }

        public void Cheat_SetEventTime(int eventID, long time)
        {
            foreach(var iter in m_Events)
            {
                var eventData = iter.Value;
                if(eventData.m_EventID == eventID)
                {
                    eventData.m_EndDate = SDateManager.Instance.CurrTime() + time;
                    return;
                }
            }
        }
    }
}
