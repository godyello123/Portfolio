using System;
using System.Net;
using System.Collections.Generic;
using SCommon;
using SNetwork;
using Global;

namespace LogServer
{
    public partial class CNetManager : SSingleton<CNetManager>
    {
        private Form1 m_Form;

        public Form1 Form { set { m_Form = value; }  get { return m_Form; } }

        public delegate void MsgHandlerDelegate(long sessionKey, byte[] packet);

        private bool m_Stop;
        private SNetSystem m_NetSystem = new SNetSystem();

        private Dictionary<ushort, MsgHandlerDelegate> m_Handler = new Dictionary<ushort, MsgHandlerDelegate>();
        private SFPS m_FPS = new SFPS();

        private STimer m_HeartBeatTimer = new STimer(5000, false);

        public int RecvPacketCount { get; set; }
        public int SendPacketCount { get; set; }
        
        public CNetManager()
        {
            SNetSystem.SendThreadSleep = 10;
            SNetSystem.TimeoutBothSide = true;
            SNetSystem.Timeout = CConfig.Instance.Timeout;
            m_NetSystem.RegisterUpdateDelegate(new UpdateDelegate(OnUpdate));
            m_NetSystem.RegisterAcceptSessionDelegate(new AcceptSessionDelegate(OnAcceptSession));
            m_NetSystem.RegisterCloseSessionDelegate(new CloseSessionDelegate(OnCloseSession));
            m_NetSystem.RegisterConnectSessionDelegate(new ConnectSessionDelegate(OnConnectSession));
            m_NetSystem.RegisterRecvSessionDelegate(new RecvSessionDelegate(OnRecvSession));
            m_NetSystem.RegisterExceptionSessionDelegate(new ExceptionSessionDelegate(OnExceptionSession));

            SetupMsgHandler();
        }

        public void Start()
        {
            SPipeServer.Instance.Init("LogServer_MBBI");

            CLogger.Instance.System(string.Format("LogServer Start - {0}", DateTime.UtcNow.ToString()));

            SMongoDBManager.Instance.Start(CConfig.Instance.MongoDB_ConnectString, CConfig.Instance.MongoDB_Name);

            m_NetSystem.Start(CConfig.Instance.Port);
        }

        public void Stop()
        {
            m_Stop = true;

            m_NetSystem.Stop();

            SMongoDBManager.Instance.Stop();

            SPipeServer.Instance.Stop();
        }

        public void CloseAllAcceptedSession()
        {
            m_NetSystem.CloseAllAcceptedSession();
        }

        public void CloseSession(long sessionKey, string CloseType = null)
        {
            m_NetSystem.CloseSession(sessionKey);

        }

        public void Write(long sessionKey, INetPacket writeMsg, int buffSize = 128)
        {
            SendPacketCount++;
            m_NetSystem.Write(sessionKey, writeMsg, buffSize);
        }

        public void OnUpdate()
        {
            CheckServerSession();
            SPipeServer.Instance.Update();
            SMongoDBManager.Instance.Update();

            if (m_FPS.Update())
            {
                //m_Form.SetUIData(RecvPacketCount, SLogManager.Instance.QueueCnt);
                RecvPacketCount = 0;
                SendPacketCount = 0;
            }
        }

        public void OnCloseSession(long sessionKey, CloseType etype)
        {
            CLogger.Instance.Debug(string.Format("OnCloseSession[{0}]", sessionKey.ToString()));
            m_Form.DeleteServerCount();
        }

        public void OnAcceptSession(long sessionKey)
        {
            if (m_NetSystem.FindSesssion(sessionKey) is SNetSession hasSession)
            {
                hasSession.IsServerSession = true;
            }

            CLogger.Instance.System(string.Format("OnAcceptSession[{0}]", sessionKey.ToString()));
            m_Form.AddServerCount();
        }

        public void OnConnectSession(long sessionKey, bool result)
        {
            CLogger.Instance.Debug(string.Format("OnConnectSession[{0}] : {1}", sessionKey.ToString(), result.ToString()));
            m_Form.DeleteServerCount();
        }

        public void OnRecvSession(long sessionKey, byte[] packet)
        {
            RecvPacketCount++;
            if (packet.Length < 2)
            {
                CLogger.Instance.Warning(string.Format("Packet Length Error : {0}", packet.Length.ToString()));
                m_NetSystem.CloseSession(sessionKey);
                return;
            }

            ushort protocol = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, 0));
            if (!m_Handler.ContainsKey(protocol))
            {
                CLogger.Instance.Warning(string.Format("Cannot find MsgHandler : Protocol[{0}]", protocol));
                return;
            }

            m_Handler[protocol](sessionKey, packet);
        }

        public void OnExceptionSession(long sessionKey, Exception e)
        {
            CLogger.Instance.Warning(string.Format("OnExceptionSession[{0}] : {1}", 1, e.Message));
        }

        private void CheckServerSession()
        {
            if (m_Stop)
                return;
        }

        
    }
}
