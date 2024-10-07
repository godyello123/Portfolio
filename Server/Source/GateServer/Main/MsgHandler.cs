using System;
using System.Collections.Generic;
using SNetwork;
using Global;
using Global.RestAPI;
using SCommon;
using System.Threading.Tasks;

namespace GateServer
{
    public partial class CNetManager
    {
        private void SetupMsgHandler()
        {
            //X2X
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeat, new MsgHandlerDelegate(X2X_HeartBeat));
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeatAck, new MsgHandlerDelegate(X2X_HeartBeatAck));

            //C2G
            m_Handler.Add((ushort)Packet_C2G.Protocol.C2G_RequestAuth, new MsgHandlerDelegate(C2G_RequestAuth));


            //P2G
            //m_Handler.Add((ushort)Packet_P2G.Protocol.P2G_RequestServerConnect, new MsgHandlerDelegate(P2G_RequestServerConnect));
            //m_Handler.Add((ushort)Packet_P2G.Protocol.P2G_RequestIdentifyUser, new MsgHandlerDelegate(P2G_RequestIdentifyUser));
            //m_Handler.Add((ushort)Packet_P2G.Protocol.P2G_ReportLogoutUser, new MsgHandlerDelegate(P2G_ReportLogoutUser));
        }
    }

    //X2X
    public partial class CNetManager
    {
        public void X2X_HeartBeat(long sessionKey, byte[] packet)
        {
            Write(sessionKey, new Packet_X2X.X2X_HeartBeatAck());
        }

        public void X2X_HeartBeatAck(long sessionKey, byte[] packet)
        {
        }
    }

    //Packet
  
    public partial class CNetManager
    {
        #region C2G
        public void C2G_RequestAuth(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2G.C2G_RequestAuth();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) 
                return;

            //firebase 인증 확인
            if (recvMsg.m_AuthType == CDefine.AuthType.Firebase)
                CRestManager.Instance.RestAuth(sessionKey, recvMsg.ServerKey, recvMsg.DeviceID);
            else
                CDBManager.Instance.QueryAccountAuth(sessionKey, recvMsg.ServerKey, recvMsg.DeviceID, recvMsg.m_AuthType);
        }
        #endregion

        #region P2G
        //public void P2G_RequestServerConnect(long sessionKey, byte[] packet)
        //{
        //    var recvMsg = new Packet_P2G.P2G_RequestServerConnect();
        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet))
        //        return;

        //    var result = CServerManager.Instance.ReqServerConnect(sessionKey, recvMsg.m_Data);
        //    if (result != Packet_Result.Result.Success)
        //        CNetManager.Instance.G2P_ResultServerConnect(sessionKey, result);
        //}

        //public void P2G_RequestIdentifyUser(long sessionKey, byte[] packet)
        //{
        //    var recvMsg = new Packet_P2G.P2G_RequestIdentifyUser();
        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet))
        //        return;

        //    var server = CServerManager.Instance.Find(sessionKey);
        //    if (server == null)
        //        return;

        //    var result = CUserManager.Instance.ReqIdentifyUser(sessionKey, recvMsg.m_UID, recvMsg.m_UserSessionKey);
        //    if (result != Packet_Result.Result.Success)
        //        CNetManager.Instance.G2P_ResultIdentifyUser(sessionKey, recvMsg.m_UserSessionKey, new CSimpleUserInfo(), result);
        //}

        //public void P2G_ReportLogoutUser(long sessionKey, byte[] packet)
        //{
        //    var recvMsg = new Packet_P2G.P2G_ReportLogoutUser();
        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet))
        //        return;

        //    var server = CServerManager.Instance.Find(sessionKey);
        //    if (server == null)
        //        return;

        //    CUserManager.Instance.Remove(recvMsg.m_Data);
        //}
        #endregion
    }
}
