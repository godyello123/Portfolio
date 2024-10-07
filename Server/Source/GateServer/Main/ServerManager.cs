using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Global;
using SCommon;

namespace GateServer
{
    public class CServerManager : SSingleton<CServerManager>
    {
        private Dictionary<long, CSimpleServerInfo> m_ServerSessions = new Dictionary<long, CSimpleServerInfo>();

        public bool Insert(long sessionKey, CSimpleServerInfo info)
        {
            if (m_ServerSessions.ContainsKey(sessionKey))
                return false;
            
            m_ServerSessions.Add(sessionKey, info);
            return true;
        }

        public bool Remove(long sessionKey)
        {
            return m_ServerSessions.Remove(sessionKey);
        }

        public CSimpleServerInfo Find(long sessionKey)
        {
            if (m_ServerSessions.TryGetValue(sessionKey, out var retval))
                return retval;

            return null;
        }

        public Packet_Result.Result ReqServerConnect(long sessionKey, CSimpleServerInfo info)
        {
            if (!Insert(sessionKey, info))
                return Packet_Result.Result.PacketError;

            CNetManager.Instance.G2P_ResultServerConnect(sessionKey, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }
    }
}
