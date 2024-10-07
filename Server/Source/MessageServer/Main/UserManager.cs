using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Global;
using SCommon;

namespace MessageServer
{
    public class CUserOverView
    {
        private Int64 m_ServerSessionKey = 0;
        private string m_DeviceID = string.Empty;
        private _UserOverViewData m_OverViewData = new _UserOverViewData();
        private eUserState m_UserState = eUserState.TryLogin;

        public long ServerSession { get => m_ServerSessionKey; }
        public long UID { get => m_OverViewData.m_UID; }
        public string DeviceID { get => m_DeviceID; }
        public _UserOverViewData OverView { get => m_OverViewData; }

        public CUserOverView(long serverSession, string deviceID, _UserOverViewData overviewData)
        {
            m_ServerSessionKey = serverSession;
            m_DeviceID = deviceID;
            m_OverViewData = overviewData;
        }

        public CUserOverView(long serverSession, long uid, string deviceID)
        {
            m_ServerSessionKey = serverSession;
            m_DeviceID = deviceID;
            m_OverViewData.m_UID = uid;
        }

        public void UpdateOverView(_UserOverViewData overviewData)
        {
            m_UserState = eUserState.Login;
            m_OverViewData = overviewData;
        }

        public bool IsLogin()
        {
            return m_UserState == eUserState.Login;
        }
    }

    public class CUserManager : SSingleton<CUserManager>
    {
        private SConcurrentDictionarySub<string, long, CUserOverView> m_UserOvreViews = new SConcurrentDictionarySub<string, long, CUserOverView>();
        public void Init()
        {
        }
    
        public CUserOverView MakeOverView(long serverSession, long uid, string deviceid)
        {
            return new CUserOverView(serverSession, uid, deviceid);
        }

        public CUserOverView FindUser(string deviceID)
        {
            return m_UserOvreViews.Find(deviceID);
        }

        public CUserOverView FindUser(long uid)
        {
            return m_UserOvreViews.Find(uid);
        }

        public bool Insert(CUserOverView data)
        {
            return m_UserOvreViews.Insert(data.DeviceID, data.UID, data);
        }

        public void Remove(string deviceID, long uid)
        {
            m_UserOvreViews.Erase(deviceID, uid);
        }

        public Packet_Result.Result ReqEnterUser(long serverSession, long userSession, long uid, string deviceId)
        {
            //duplicated
            var prevUser = FindUser(deviceId);
            if(prevUser != null)
            {
                if (CServerManager.Instance.Find(prevUser.ServerSession) is var prevServer)
                    CNetManager.Instance.M2P_ReportUserKick(prevUser.ServerSession, prevUser.UID, Packet_Result.Result.DuplicatedAccess);
            }

            //block
            if (CSystemManager.Instance.IsBlockUser(deviceId))
                return Packet_Result.Result.BlockUser;

            var curServer = CServerManager.Instance.Find(serverSession);
            if (curServer == null)
            {
                CNetManager.Instance.M2P_ReportUserKick(serverSession, uid, Packet_Result.Result.NotServiceError);
                CNetManager.Instance.CloseSession(serverSession);
                return Packet_Result.Result.NotServiceError;
            }

            //white
            if (!CSystemManager.Instance.IsLiveService())
            {
                if (!CSystemManager.Instance.IsWhiteUser(deviceId))
                    return Packet_Result.Result.NotServiceError;
            }

            CUserManager.Instance.Insert(MakeOverView(serverSession, uid, deviceId));
            CNetManager.Instance.M2P_ResultEnterUser(serverSession, userSession, uid, deviceId, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        public void ReqUpdateOverView(long serverSession, _UserOverViewData overview)
        {
            var foundData = FindUser(overview.m_UID);
            if (foundData == null)
                return;

            foundData.UpdateOverView(overview);

            int point = overview.m_PvpPoint;
            int maxStage = 0;
            if (overview.m_MaxMainStage > 0)
            {
                point += maxStage - 10000;
            }

            CRankingManager.Instance.InsertPvpMatchData(overview, foundData.DeviceID, point);
        }

        public void UserKick(string deviceID)
        {
            var user = FindUser(deviceID);
            if (user == null)
                return;

            CNetManager.Instance.M2P_ReportUserKick(user.ServerSession, user.UID, Packet_Result.Result.Success);
        }
    }
}
