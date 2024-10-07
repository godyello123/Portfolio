using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global;
using SCommon;
using StackExchange.Redis;

namespace MessageServer
{
    public class CServerManager : SSingleton<CServerManager>
    {
        public Dictionary<long, CServerInfo> m_Servers = new Dictionary<long, CServerInfo>();

        public Dictionary<long, CServerInfo> Servers { get { return m_Servers; } }

        public void Insert(string type, long sessionKey, int clientcnt, CDefine.eServiceType serviceType, string ip, string dn, ushort port)
        {
            CServerInfo server = new CServerInfo();
            server.m_Type = type;
            server.m_ServiceType = serviceType;
            server.m_ClientCount = clientcnt;
            server.m_IP = ip;
            server.m_DN = dn;
            server.m_Port = port;
            server.m_ServerSessionKey = sessionKey;
            server.m_Open = CSystemManager.Instance.Open;

            m_Servers[sessionKey] = server;

            CNetManager.Instance.M2P_ResultConnect(sessionKey, CEventManager.Instance.GetActiveEvents());
        }

        public void Remove(long sessionKey)
        {
            m_Servers.Remove(sessionKey);
        }

        public CServerInfo Find(long sessionKey)
        {
            if (m_Servers.TryGetValue(sessionKey, out var ret))
                return ret;

            return null;
        }

        public void SetServerState(long sessionKey, int clientCnt, int mainFPS, int dbFPS, int dbinpuCount)
        {
            var server = CServerManager.Instance.Find(sessionKey);
            if (server == null)
                return;

            server.m_ClientCount = clientCnt;
            server.m_MainFPS = mainFPS;
            server.m_DBFPS = dbFPS;
            server.m_DBInputCount = dbinpuCount;
            server.m_Open = CSystemManager.Instance.Open;
        }
    }
}
