using System;
using System.Net;
using System.Collections.Generic;
using SCommon;
using SNetwork;
using Global;

namespace PlayServer
{
    public class CHotTimeManager : SSingleton<CHotTimeManager>
    {
        private Dictionary<long, _HotTimeData> m_HotTimeBuffs = new Dictionary<long, _HotTimeData>();

        public void Init()
        {
            m_HotTimeBuffs.Clear();
        }

        public List<_HotTimeData> GetHotTimeList()
        {
            var retList = new List<_HotTimeData>();
            foreach (var iter in m_HotTimeBuffs)
                retList.Add(iter.Value);

            return retList;
        }

        public void Insert(_HotTimeData data)
        {
            m_HotTimeBuffs[data.m_UID] = data;
            BroadCast(data, false);
        }

        public void Remove(_HotTimeData data)
        {
            m_HotTimeBuffs.Remove(data.m_UID);
            BroadCast(data, true);
        }

        public double GetAbilValue(CDefine.EAbility type)
        {
            double retval = 0;
            foreach(var iter in m_HotTimeBuffs)
            {
                var itVal = iter.Value;
                if (itVal.m_Abil.type == type)
                    retval += itVal.m_Abil.val;
            }

            return retval;
        }

        public List<_AbilData> GetAbils()
        {
            List<_AbilData> retList = new List<_AbilData>();
            foreach(var iter in m_HotTimeBuffs)
            {
                var itVal = iter.Value;
                var copyAbil = SCommon.SCopy<_AbilData>.DeepCopy(itVal.m_Abil);
                retList.Add(copyAbil);
            }

            return retList;
        }

        private void BroadCast(_HotTimeData data, bool remove)
        {
            foreach(var it in CUserManager.Instance.UsersbySession)
            {
                var itVal = it.Value;
                CNetManager.Instance.P2C_ReportHotTime(itVal.SessionKey, data, remove);
            }
        }

        public void OnEnterUser(long sessionKey)
        {
            CNetManager.Instance.P2C_ReportHotTimeList(sessionKey, m_HotTimeBuffs);
        }
    }
}
