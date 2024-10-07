using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;
using SCommon;
using SNetwork;

namespace PlayServer
{
    class CUserManager : SSingleton<CUserManager>
    {
        private Dictionary<long, CUser> m_UsersbySessionKey = new Dictionary<long, CUser>();
        private Dictionary<long, CUser> m_UsersbyUID = new Dictionary<long, CUser>();

        public Dictionary<long, CUser> UsersbySession { get => m_UsersbySessionKey; }
        public Dictionary<long,  CUser> UsersbyUID { get => m_UsersbyUID; }

       
        public void Init()
        {
            m_UsersbySessionKey.Clear();
            m_UsersbyUID.Clear();
        }
           

        public void Insert(CUser user)
        {
            InsertBySessionKey(user.SessionKey, user);
            InsertByUID(user.UserData.m_UID, user);
        }

        public void InsertBySessionKey(long sessionKey, CUser user)
        {
            if(!m_UsersbySessionKey.ContainsKey(sessionKey))
                m_UsersbySessionKey.Add(sessionKey, user);
        }

        public void InsertByUID(long accountID, CUser user)
        {
            if (!m_UsersbyUID.ContainsKey(accountID))
                m_UsersbyUID.Add(accountID, user);
        }

        public void Remove(CUser user)
        {
            RemoveBySessionKey(user.SessionKey);
            RemoveByUID(user.UserData.m_UID);

            CLogger.Instance.System($"Logout -- UID : {user.UserData.m_UID}");
        }

        public void RemoveBySessionKey(long sessionKey)
        {
            if (m_UsersbySessionKey.TryGetValue(sessionKey, out CUser findUser))
            {
                m_UsersbySessionKey.Remove(sessionKey);
                RemoveByUID(findUser.UserData.m_UID);
            }
        }

        public void RemoveByUID(long uid)
        {
            if (m_UsersbyUID.TryGetValue(uid, out CUser findUser))
            {
                m_UsersbyUID.Remove(findUser.UserData.m_UID);
                RemoveBySessionKey(findUser.SessionKey);
            }
        }

        public CUser FindbyUID(long uid)
        {
            if (m_UsersbyUID.TryGetValue(uid, out CUser ret))
                return ret;

            return null;
        }

        public CUser FindbySessionKey(long sessionKey)
        {
            if (m_UsersbySessionKey.TryGetValue(sessionKey, out CUser ret))
                return ret;

            return null;
        }

        public int GetCount()
        {
            return m_UsersbySessionKey.Count;
        }

        public void Update()
        {
            foreach(var iter in m_UsersbyUID)
            {
                var user = iter.Value;
                user.Update();
            }
        }

        public void EnterUser(long sessionKey, DBCharacterLoadData userDBData)
        {
            CLogger.Instance.Debug($"CUserManger EnterUser : {sessionKey}");

            CUser newUser = new CUser(sessionKey, userDBData.DeviceID);
            newUser.OnEnter(userDBData);
            Insert(newUser);
        }

        public void Stop()
        {
            foreach(var it in m_UsersbySessionKey.Values)
            {
                CNetManager.Instance.P2C_ReportKick(it.SessionKey, Packet_Result.Result.NotServiceError);
                it.Logout();
            }
        }

        public void DisConnect(long sessionKey)
        {
            var user = FindbySessionKey(sessionKey);
            if(user != null)
            {
                CLogger.Instance.System($"user : {user.UserData.m_UID} Logout!");
                user.Logout();
                Remove(user);
            }
        }

        public void Kick(long userUID, Packet_Result.Result result)
        {
            var user = FindbyUID(userUID);
            if (user == null)
                return;

            CNetManager.Instance.P2C_ReportKick(user.SessionKey, result);
        }
    }
}
