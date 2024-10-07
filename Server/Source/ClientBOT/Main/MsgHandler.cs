using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBOT
{
    public partial class CNetManager
    {
        public void SetupMsgHandler()
        {
            //X2X
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeat, new MsgHandlerDelegate(X2X_HeartBeat));
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeatAck, new MsgHandlerDelegate(X2X_HeartBeatAck));

            //P2C
            m_Handler.Add((ushort)Packet_C2P.Protocol.P2C_ResultEnterServer, new MsgHandlerDelegate(P2C_ResultEnterServer));
            m_Handler.Add((ushort)Packet_C2P.Protocol.P2C_ReportSendChatData, new MsgHandlerDelegate(P2C_ReportSendChatData));
        }
    }

    #region X2X
    public partial class CNetManager
    {
        //X2X
        public void X2X_HeartBeat(long sessionKey, byte[] packet)
        {
            m_NetSystem.Write(sessionKey, new Packet_X2X.X2X_HeartBeatAck());
        }

        public void X2X_HeartBeatAck(long sessionKey, byte[] packet)
        {
        }
    }

    #endregion

    #region C2P
    public partial class CNetManager
    {
        public void P2C_ResultEnterServer(long sessionKey, byte[] packet)
        {
            Packet_C2P.P2C_ResultEnterServer recvMsg = new Packet_C2P.P2C_ResultEnterServer();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            //if (sessionKey != m_PlaySession.SessionKey)
            //    return;

            //FormMain.RetUserData(recvMsg.m_UserData);
        }

        public void P2C_ReportSendChatData(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.P2C_ReportSendChatData();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;
        }
    }
    #endregion
}
