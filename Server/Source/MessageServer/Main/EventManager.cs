using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Global;
using Packet_P2M;
using SCommon;

namespace MessageServer
{

    public class CEventManager : SSingleton<CEventManager>
    {
        private STimer m_Timer = new STimer(10 * 1000, false);
        private bool m_Run = false;


        Dictionary<long, _EventData> m_ActiveEvents = new Dictionary<long, _EventData>();
        Dictionary<long, _EventData> m_DeActiveEvents = new Dictionary<long, _EventData>();

        public void Init()
        {
            CDBManager.Instance.QuerySystemEventLoad();
        }

        public void Update()
        {
            if (!m_Run)
                return;

            if (!m_Timer.Check())
                return;

            InActiveProcess();
        }

        public void Stop()
        {
            if (!m_Run) return;

            m_Run = false;

            m_ActiveEvents.Clear();
            m_DeActiveEvents.Clear();
        }

        public List<_EventData> GetActiveEvents()
        {
            var retList = new List<_EventData>();
            foreach(var iter in m_ActiveEvents)
            {
                var itVal = iter.Value;
                retList.Add(itVal);
            }

            return retList;
        }

        private void InActiveProcess()
        {
            List<long> eraseList = new List<long>();
            foreach(var iter in m_DeActiveEvents)
            {
                var itVal = iter.Value;
                if (!SDateManager.Instance.IsExpired(itVal.m_StartDate))
                    continue;

                if (SDateManager.Instance.IsExpired(itVal.m_EndDate))
                    continue;

                eraseList.Add(itVal.m_UID);

                Upsert(itVal);
            }

            foreach(var iter in eraseList)
                m_DeActiveEvents.Remove(iter);
        }

        private void Upsert(_EventData eventData)
        {
            m_ActiveEvents[eventData.m_UID] = eventData;

            foreach(var iter in CServerManager.Instance.Servers)
                CNetManager.Instance.M2P_ReportEvent(iter.Value.m_ServerSessionKey, eventData, false);
        }

        public void LoadEvent(List<_EventData> events)
        {
            foreach (var iter in events)
                Insert(iter);

            m_Run = true;
        }


        public void Insert(_EventData eventData)
        {
            if (m_ActiveEvents.ContainsKey(eventData.m_UID))
            {
                m_ActiveEvents[eventData.m_UID] = eventData;

                foreach (var iter in CServerManager.Instance.Servers)
                    CNetManager.Instance.M2P_ReportEvent(iter.Value.m_ServerSessionKey, eventData, false);
            }
            else
            {
                m_DeActiveEvents[eventData.m_UID] = eventData;
            }
        }

        public void Remove(_EventData eventData)
        {
            if (m_ActiveEvents.ContainsKey(eventData.m_UID))
            {
                m_ActiveEvents.Remove(eventData.m_UID);

                foreach (var iter in CServerManager.Instance.Servers)
                    CNetManager.Instance.M2P_ReportEvent(iter.Value.m_ServerSessionKey, eventData, true);
            }
            else
            {
                m_DeActiveEvents.Remove(eventData.m_UID);
            }
        }
    }
}
