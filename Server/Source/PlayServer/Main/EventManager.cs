using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using Global;
using System.Runtime.InteropServices.ComTypes;

namespace PlayServer
{
    public class CEvent
    {
        public long m_UID = -1;
        public int m_ID = -1;
        public long m_StartDate = -1;
        public long m_EndDate = -1;

        public CEvent(long uid, int id, long start, long end)
        {
            m_UID = uid;
            m_ID = id;
            m_StartDate = start;
            m_EndDate = end;
        }

        public CEvent(_EventData data)
        {
            m_UID = data.m_UID;
            m_ID = data.m_EventID;
            m_StartDate = data.m_StartDate;
            m_EndDate = data.m_EndDate;
        }
    }

    public class CEventManager : SSingleton<CEventManager>
    {
        public Dictionary<long, CEvent> m_Events = new Dictionary<long, CEvent>();

        public List<CEvent> GetActiveList()
        {
            var retVal = new List<CEvent>();
            foreach(var iter in m_Events)
            {
                var itVal = iter.Value;

                if (!SDateManager.Instance.IsExpired(itVal.m_StartDate))
                    continue;

                if (SDateManager.Instance.IsExpired(itVal.m_EndDate))
                    continue;
               
                retVal.Add(itVal);
            }

            return retVal;
        }

        public void Insert(_EventData eventData)
        {
            m_Events[eventData.m_UID] = new CEvent(eventData);
        }

        public void SetActiveEvent(List<_EventData> events)
        {
            foreach (var iter in events)
                Insert(iter);
        }
        
        public void Remove(long uid)
        {
            m_Events.Remove(uid);

            //user report;
        }

        public CEvent Find(long uid)
        {
            if (m_Events.TryGetValue(uid, out CEvent retVal))
                return retVal;

            return null;
        }

        public bool IsExpired(long uid)
        {
            var findData = Find(uid);
            if (findData == null)
                return true;

            if (SDateManager.Instance.IsExpired(findData.m_EndDate))
                return true;

            return false;
        }

        public bool isExist(long uid)
        {
            var findData = Find(uid);
            if (findData == null)
                return false;

            return true;
        }
    }
}
