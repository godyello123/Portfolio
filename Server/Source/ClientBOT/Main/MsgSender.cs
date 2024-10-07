using System.Collections;
using System.Collections.Generic;
using Global;
using SNetwork;

namespace ClientBOT
{

    public partial class CNetManager
    {
        public void C2G_RequestAuth(int serverKey, string deviceID, CDefine.AuthType authType)
        {
            var sendMsg = new Packet_C2G.C2G_RequestAuth();
            sendMsg.m_AuthType = authType;
            sendMsg.ServerKey = serverKey;
            sendMsg.DeviceID = deviceID;


            //WriteGateServer(sendMsg);
        }

        public void C2P_RequestEnterServer(long uid)
        {
            var sendMsg = new Packet_C2P.C2P_RequestEnterServer();
            sendMsg.m_UID = uid;

            //WritePlayServer(sendMsg);
        }
        public void C2P_RequestStageStart(int _stageTid)
        {
            var sendMsg = new Packet_C2P.C2P_RequestStageStart();
            sendMsg.m_TID = _stageTid;

            //WritePlayServer(sendMsg);
        }
        public void C2P_RequestStageClear(int _stageTid, bool isClear)
        {
            var sendMsg = new Packet_C2P.C2P_RequestStageClear();
            sendMsg.m_TID = _stageTid;
            sendMsg.m_IsClear = isClear;

           // WritePlayServer(sendMsg);
        }
        public void C2P_RequestSendChatData(_ChatData _chatData)
        {
            var sendMsg = new Packet_C2P.C2P_RequestSendChatData();
            sendMsg.m_Chat = _chatData;

            //WritePlayServer(sendMsg);
        }
    }
}
