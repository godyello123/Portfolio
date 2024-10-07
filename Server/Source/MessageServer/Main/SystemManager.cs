using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global;
using SCommon;

namespace MessageServer
{
    public class CSystemManager : SSingleton<CSystemManager>
    {
        private STimer m_Timer = new STimer(10 * 1000, false);
        private bool m_Run = false;

        private bool m_Open = true;
        private Dictionary<string, _BlockUser> m_BlockList = new Dictionary<string, _BlockUser>();
        private Dictionary<string, CWhiteUser> m_WhiteList = new Dictionary<string, CWhiteUser>();

        //NOITCE
        private Dictionary<long, _NoticeData> m_ActiveNotices = new Dictionary<long, _NoticeData>();
        private Dictionary<long, _NoticeData> m_DeActiveNotices = new Dictionary<long, _NoticeData>();



        public bool Open { get => m_Open; }

        public void Init()
        {
            m_BlockList.Clear();

            m_Run = true;

            CDBManager.Instance.QuerySystemDataLoad();
        }

        public void Update()
        {
            if (!m_Run)
                return;

            if (!m_Timer.Check())
                return;

            //Notice Update
            CheckInActiveList();
            CheckActiveList();
        }

        public void Stop()
        {
            if (!m_Run) return;

            m_Run = false;

            m_BlockList.Clear();
            m_WhiteList.Clear();
            m_DeActiveNotices.Clear();
            m_ActiveNotices.Clear();
        }

        public void LoadSystemData(List<_BlockUser> blocks, List<CWhiteUser> whites, List<_NoticeData> notices, bool open)
        {
            foreach (var it in blocks)
                m_BlockList[it.DeviceID] = it;

            foreach (var it in whites)
                m_WhiteList[it.m_DeviceID] = it;

            foreach (var it in notices)
                PushNotice(it);

            m_Open = open;
        }

        //============================== ServiceType ======================================
        public bool IsLiveService()
        {
            return m_Open;
        }

        public void ReqServiceType(bool bopne)
        {
            m_Open = bopne;

            CDBManager.Instance.QuerySystemServiceTypeUpdate(m_Open);
        }

        // ============================== Block & White User ===============================
        #region Block&White User
        public _BlockUser FindBlockUser(string deviceID)
        {
            if (m_BlockList.TryGetValue(deviceID, out var retval))
                return retval;

            return null;
        }

        public CWhiteUser FindWhiteUwser(string deviceID)
        {
            if (m_WhiteList.TryGetValue(deviceID, out var retval))
                return retval;

            return null;
        }

        public void UpsertBlockUser(_BlockUser blockUser)
        {
            m_BlockList[blockUser.DeviceID] = blockUser;

            var user = CUserManager.Instance.FindUser(blockUser.DeviceID);
            if (user == null)
                return;

            CNetManager.Instance.M2P_ReportUserKick(user.ServerSession, user.UID, Packet_Result.Result.BlockUser);
        }

        public void RemoveWhiteUser(string targetid)
        {
            m_WhiteList.Remove(targetid);
        }

        public void UpsertWhiteUser(CWhiteUser whiteuser)
        {
            m_WhiteList[whiteuser.m_DeviceID] = whiteuser;
        }

        public bool IsBlockUser(string deviceid)
        {
            var banUser = FindBlockUser(deviceid);
            if (banUser == null)
                return false;

            if (SDateManager.Instance.IsExpired(banUser.ExpTime))
                return false;

            return true;
        }

        public bool IsWhiteUser(string deviceID)
        {
            var user = FindWhiteUwser(deviceID);
            if (user == null)
                return false;

            return true;
        }
        #endregion

        // ============================== Notice ===============================
        #region Notice 
        public void PushNotice(_NoticeData data)
        {
            if (m_ActiveNotices.ContainsKey(data.m_ID))
            {
                m_ActiveNotices[data.m_ID] = data;
            }
            else
            {
                m_DeActiveNotices[data.m_ID] = data;
            }
        }

        private void CheckInActiveList()
        {
            List<long> eraseList = new List<long>();
            foreach (var it in m_DeActiveNotices)
            {
                var hasData = it.Value;
                if (SDateManager.Instance.IsEnable(hasData.m_StartDate, hasData.m_EndDate))
                {
                    UpsertNotice(hasData);
                    eraseList.Add(hasData.m_ID);
                }
            }

            foreach (var key in eraseList)
                m_DeActiveNotices.Remove(key);
        }

        private void CheckActiveList()
        {
            List<_NoticeData> eraseList = new List<_NoticeData>();

            foreach (var it in m_ActiveNotices)
            {
                var hasData = it.Value;

                if (SDateManager.Instance.IsExpired(hasData.m_EndDate))
                    eraseList.Add(hasData);
            }

            foreach (var data in eraseList)
                EraseNotice(data);
        }

        private void UpsertNotice(_NoticeData data)
        {
            m_ActiveNotices[data.m_ID] = data;

            CNetManager.Instance.M2P_ReportNotice(data, false);
        }

        private void EraseNotice(_NoticeData data)
        {
            CNetManager.Instance.M2P_ReportNotice(data, true);

            m_ActiveNotices.Remove(data.m_ID);
        }

        public void EraseNotice(long id)
        {
            if(m_ActiveNotices.TryGetValue(id, out var notice))
            {
                CNetManager.Instance.M2P_ReportNotice(notice, true);

                m_ActiveNotices.Remove(notice.m_ID);
            }
            else if(m_DeActiveNotices.TryGetValue(id, out var innotice))
            {
                m_DeActiveNotices.Remove(innotice.m_ID);
            }
        }

        public List<_NoticeData> GetNoticeDataList()
        {
            List<_NoticeData> ret_list = new List<_NoticeData>();
            foreach (var iter in m_ActiveNotices)
            {
                var data = iter.Value;
                ret_list.Add(data);
            }

            foreach (var iter in m_DeActiveNotices)
            {
                var data = iter.Value;
                ret_list.Add(data);
            }

            return ret_list;
        }

        public Packet_Result.Result ReqNoticeList(long serverSession, long userSession)
        {
            List<_NoticeData> sendList = new List<_NoticeData>();
            foreach (var it in m_ActiveNotices)
                sendList.Add(it.Value);

            CNetManager.Instance.M2P_ResultNoticeList(serverSession, userSession, sendList, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        public List<_NoticeData> GetNoticeDataListbyOffset(int begin, int end)
        {
            var notice_list = GetNoticeDataList();
            if (begin >= notice_list.Count)
                return new List<_NoticeData>();

            int count = end - begin;
            return notice_list.GetRange(begin, Math.Min(count, notice_list.Count - 1));
        }


        #endregion
    }
}
