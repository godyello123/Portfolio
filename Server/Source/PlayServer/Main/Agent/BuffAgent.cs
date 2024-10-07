using System;
using System.Collections.Generic;
using System.Security.Policy;
using Global;
using ParquetSharp;
using SCommon;

namespace PlayServer
{
    public class CBuffAgent
    {
        private CUser m_Owner;
        private STimer m_Timer = new STimer(30 * 1000);

        private Dictionary<int, _AdsBUffData> m_AdsBuffs = new Dictionary<int, _AdsBUffData>();

        public CBuffAgent(CUser owner)
        {
            m_Owner = owner;
        }

        public void Init(List<_AdsBUffData> m_adsBuffs)
        {
            m_AdsBuffs = AdsBuffTable.Instance.CopyDefault();
            foreach (var iter in m_adsBuffs)
                m_AdsBuffs[iter.m_BuffID] = iter;

            Refresh(false);
        }

        public List<_AdsBUffData> GetAdsBuffList()
        {
            var retVal = new List<_AdsBUffData>();

            foreach(var iter in m_AdsBuffs)
            {
                var itVal = iter.Value;
                retVal.Add(itVal);
            }

            return retVal;
        }

        public _AdsBUffData FindAdsBuff(int buffid)
        {
            if (m_AdsBuffs.ContainsKey(buffid))
                return m_AdsBuffs[buffid];

            return null;
        }

        public void Refresh(bool bSend)
        {
            var retList = new List<_AdsBUffData>();

            foreach (var iter in m_AdsBuffs)
            {
                var itVal = iter.Value;
                if (SDateManager.Instance.IsExpired(itVal.m_BuffExpTime))
                {
                    itVal.m_BuffExpTime = -1;
                    itVal.m_Modifyed = true;
                }

                if (SDateManager.Instance.IsExpired(itVal.m_WatchAdsDailyExpTime))
                {
                    itVal.m_WatchAdsDailyExpTime = SDateManager.Instance.TomorrowTimestamp() + 1;
                    itVal.m_WatchAdsDailyCount = 0;
                    itVal.m_Modifyed = true;
                    retList.Add(itVal);
                }
            }
        }

        public void GetAbilBuff(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            foreach (var iter in m_AdsBuffs)
            {
                var buff = iter.Value;
                if (!SDateManager.Instance.IsExpired(buff.m_BuffExpTime))
                {
                    var record = AdsBuffTable.Instance.Find(buff.m_BuffID);
                    if (record == null)
                        continue;

                    CStatusAgent.UpsertAbil(ref rAbils, new _AbilData(record.Abil));
                }
            }
        }

        public void Update()
        {
            if (!m_Timer.Check())
                return;

            bool modifyed = false;
            var modifyList = new List<_AdsBUffData>();
            foreach (var iter in m_AdsBuffs)
            {
                var itVal = iter.Value;
                if (itVal.m_BuffExpTime <= -1)
                    continue;

                if (SDateManager.Instance.IsExpired(itVal.m_BuffExpTime))
                {
                    itVal.m_BuffExpTime = -1;
                    itVal.m_Modifyed = true;
                    modifyed = true;
                    modifyList.Add(itVal);
                }
            }

            if (modifyed)
            {
                m_Owner.StatusAgent.RefreshAll();
                //CNetManager.Instance.P2C_ReportAdsBuffData(m_Owner.SessionKey, modifyList);
            }
        }

        public void AdSkipBuff()
        {
            foreach(var iter in m_AdsBuffs)
            {
                var buff = iter.Value;
                buff.m_BuffExpTime = SDateManager.Instance.DateTimeToTimeStamp(CDefine.MaxTime);
                buff.m_Modifyed = true;
            }

        }

        public Packet_Result.Result ReqAdsBuffReward(long sessionKey, int buffid)
        {
            //valid
            var adsBuff = FindAdsBuff(buffid);
            if (adsBuff == null)
                return Packet_Result.Result.InValidData;

            var record = AdsBuffTable.Instance.Find(adsBuff.m_BuffID);
            if (record == null)
                return Packet_Result.Result.InValidRecord;

            if (adsBuff.m_WatchAdsDailyCount >= record.MaxWatchAdsDailyCount)
                return Packet_Result.Result.PacketError;

            if (!SDateManager.Instance.IsExpired(adsBuff.m_BuffExpTime))
            {
                CNetManager.Instance.P2C_ReportAdsBuffData(sessionKey, GetAdsBuffList());

                return Packet_Result.Result.RemainBuffTime;
            }

            //process
            adsBuff.m_WatchAdsDailyCount++;

            //buff is expired?
            adsBuff.m_BuffExpTime = SDateManager.Instance.CurrTime() + record.Duration;
            
            adsBuff.m_BuffExp++;

            if (adsBuff.m_BuffExp >= record.MaxWatchAdsDailyCount)
            { 
                adsBuff.m_BuffLv = Math.Min(adsBuff.m_BuffLv + 1, record.MaxLv);
                adsBuff.m_BuffExp = 0;
            }

            //status
            m_Owner.StatusAgent.RefreshAll();

            adsBuff.m_Modifyed = false;

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Ads_Buff, "", "", 1);

            //db
            CDBManager.Instance.QueryCharacterAdsBuffUpdate(m_Owner.DBGUID, sessionKey, m_Owner.UID, adsBuff);

            CNetManager.Instance.P2C_ResultAdsBuffReward(sessionKey, adsBuff, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqAdsBuffMaxLevelComplete(long sessionKey)
        {
            if (m_Owner.UserData.m_ADSkip)
                return Packet_Result.Result.InValidData;

            int completCount = 0;
            foreach(var iter in m_AdsBuffs)
            {
                var buff = iter.Value;
                var record = AdsBuffTable.Instance.Find(buff.m_BuffID);
                if (record == null)
                    continue;

                if (buff.m_BuffLv >= record.MaxLv)
                    completCount++;
            }

            if(completCount < AdsBuffTable.Instance.GetCount())
                return Packet_Result.Result.PacketError;

            m_Owner.ADSkip();

            CNetManager.Instance.P2C_ResultAdsBuffMaxLevelComplete(sessionKey, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqAdsBuffList(long sessionKey)
        {
            CNetManager.Instance.P2C_ResultAdsBuffList(sessionKey, GetAdsBuffList(), Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public void SaveAtLogout()
        {
            foreach(var iter in m_AdsBuffs)
            {
                var buff = iter.Value;
                if(buff.m_Modifyed)
                    CDBManager.Instance.QueryCharacterAdsBuffUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, buff);
            }
        }

        public void CheatBuffLv(int buffLv)
        {
            foreach (var iter in m_AdsBuffs)
            {
                var buff = iter.Value;
                buff.m_BuffLv = buffLv;
                buff.m_Modifyed = true;
            }

            //CNetManager.Instance.P2C_ReportAdsBuffData(m_Owner.SessionKey, GetAdsBuffList());
        }

        public void CheatBuffTime(int buffid, long bufftime)
        {
            if (buffid == -1)
            {
                foreach(var iter in m_AdsBuffs)
                {
                    var itVal = iter.Value;

                    if (SDateManager.Instance.IsExpired(itVal.m_BuffExpTime))
                        itVal.m_BuffExpTime = SDateManager.Instance.CurrTime() + bufftime;
                    else
                        itVal.m_BuffExpTime += bufftime;
                }

                return;
            }

            var buff = FindAdsBuff(buffid);
            if (buff == null)
                return;

            if (SDateManager.Instance.IsExpired(buff.m_BuffExpTime))
                buff.m_BuffExpTime = SDateManager.Instance.CurrTime() + bufftime;
            else
                buff.m_BuffExpTime += bufftime;
        }
    }
}
