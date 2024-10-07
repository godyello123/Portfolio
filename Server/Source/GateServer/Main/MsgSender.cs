using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Global;
using SCommon;

namespace GateServer
{

    public partial class CNetManager
    {
        //X2X
        #region X2X
        public void X2X_HeartBeat(long sessionKey)
        {
            var sendMsg = new Packet_X2X.X2X_HeartBeat();
            Write(sessionKey, sendMsg);
        }
        #endregion

        #region C2G
        public void G2C_ResultAuth(long sessionKey, ushort result, long uid, string serverHost, ushort serverPort)
        {
            var sendMsg = new Packet_C2G.G2C_ResultAuth();
            sendMsg.m_Result = result;
            sendMsg.m_UID = uid;
            sendMsg.m_ServerHost = serverHost;
            sendMsg.m_ServerPort = serverPort;

            Write(sessionKey, sendMsg);
        }
        #endregion

        #region P2G
        //public void G2P_ResultServerConnect(long sessionKey, Packet_Result.Result result)
        //{
        //    var sendMsg = new Packet_P2G.G2P_ResultServerConnect();
        //    sendMsg.m_Result = (ushort)result;

        //    Write(sessionKey, sendMsg);
        //}

        //public void G2P_ResultIdentifyUser(long toServer, long userSessionKey, CSimpleUserInfo info, Packet_Result.Result result)
        //{
        //    var sendMsg = new Packet_P2G.G2P_ResultIdentifyUser();
        //    sendMsg.m_Result = (ushort)result;
        //    sendMsg.m_UserSessionKey = userSessionKey;
        //    sendMsg.m_Data = info;

        //    Write(toServer, sendMsg);
        //}

        //public void G2P_ReportUserKick(long sessionKey, CSimpleUserInfo kickuser, Packet_Result.Result result)
        //{
        //    var sendMsg = new Packet_P2G.G2P_ReportUserKick();
        //    sendMsg.m_KickUser = kickuser;
        //    sendMsg.m_Result = (ushort)result;

        //    Write(sessionKey, sendMsg);
        //}
        #endregion
    }
}
 