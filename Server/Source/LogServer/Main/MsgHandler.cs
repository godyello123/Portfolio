using System;
using System.Collections.Generic;
using SNetwork;
using Global;

namespace LogServer
{
    public partial class CNetManager
    {
        private  void SetupMsgHandler()
        {
            //X2X
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeat, new MsgHandlerDelegate(X2X_HeartBeat));
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeatAck, new MsgHandlerDelegate(X2X_HeartBeatAck));

            //P2L
            //m_Handler.Add((ushort)Packet_P2L.Protocol.P2L_RequestServerLogin, new MsgHandlerDelegate(P2L_RequestServerLogin));
            //m_Handler.Add((ushort)Packet_P2L.Protocol.P2L_ReportLogging, new MsgHandlerDelegate(P2L_ReportLogging));
        }
    }

    public partial class CNetManager
    {
        public void X2X_HeartBeat(long sessionKey,byte[] packet)
        {
            Write(sessionKey, new Packet_X2X.X2X_HeartBeat());
        }

        public void X2X_HeartBeatAck(long sessionKey,byte[] packet)
        {

        }
    }

    public partial class CNetManager
    {
        //public void P2L_RequestServerLogin(long sessionKey,byte[] packet)
        //{
        //    Packet_P2L.P2L_RequestServerLogin recvMsg = new Packet_P2L.P2L_RequestServerLogin();

        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

        //    long serverkey = recvMsg.m_ServerKey;
        //}

        //public void P2L_ReportLogging(long sessionKey, byte[] packet)
        //{
        //    var recvMsg = new Packet_P2L.P2L_ReportLogging();

        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

        //    //SLogManager.Instance.Push(recvMsg.m_Data);
        //}
    }
}
