using Global;
using Google.Apis.AndroidPublisher.v3.Data;
using Newtonsoft.Json.Linq;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GateServer
{
    public class CUserManager : SSingleton<CUserManager>
    {
        private Dictionary<long, CSimpleUserInfo> m_AuthUsers = new Dictionary<long, CSimpleUserInfo>();
        
        public void Upsert(string deviceID, long uid)
        {
            CSimpleUserInfo info = new CSimpleUserInfo();
            info.m_DeviceID = deviceID;
            info.m_UID = uid;

            if (!m_AuthUsers.ContainsKey(info.m_UID))
                m_AuthUsers.Add(info.m_UID, info);
        }

        public bool Remove(CSimpleUserInfo info)
        {
            return m_AuthUsers.Remove(info.m_UID);
        }

        public CSimpleUserInfo Find(long uid)
        {
            if (m_AuthUsers.TryGetValue(uid, out var retval))
                return retval;

            return null;
        }

        public Packet_Result.Result ReqIdentifyUser(long toserver, long userUID, long userSessionKey)
        {
            var user = Find(userUID);
            if (user == null)
            {
                CNetManager.Instance.G2P_ResultIdentifyUser(toserver, userSessionKey, new CSimpleUserInfo(), Packet_Result.Result.UserIdentifyError);
            }
            else
            {
                //duplicate user
                var prevServer = CServerManager.Instance.Find(user.m_ServerSessionKey);
                if (prevServer != null)
                {
                    CNetManager.Instance.G2P_ReportUserKick(user.m_ServerSessionKey, user, Packet_Result.Result.DuplicatedAccess);
                    return Packet_Result.Result.DuplicatedAccess;
                }

                //ban check
                if (CSystemManager.Instance.IsBlockUser(user.m_DeviceID))
                {
                    CNetManager.Instance.G2P_ReportUserKick(user.m_ServerSessionKey, user, Packet_Result.Result.BlockUser);
                    return Packet_Result.Result.BlockUser;
                }

                //white check
                var curServer = CServerManager.Instance.Find(toserver);
                if(curServer == null)
                {
                    CNetManager.Instance.G2P_ReportUserKick(user.m_ServerSessionKey, user, Packet_Result.Result.NotServiceError);
                    return Packet_Result.Result.NotServiceError;
                }

                if (!curServer.m_Open)
                {
                    if (!CSystemManager.Instance.IsWhiteUser(user.m_DeviceID))
                    {
                        CNetManager.Instance.G2P_ReportUserKick(user.m_ServerSessionKey, user, Packet_Result.Result.NotServiceError);
                        return Packet_Result.Result.NotServiceError;
                    }
                }


                user.m_ServerSessionKey = toserver;
                CNetManager.Instance.G2P_ResultIdentifyUser(toserver, userSessionKey, user, Packet_Result.Result.Success);
            }

            return Packet_Result.Result.Success;
        }
    }
}
