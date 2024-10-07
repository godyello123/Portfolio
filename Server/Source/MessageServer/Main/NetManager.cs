using System;
using System.Net;
using System.Collections.Generic;
using SCommon;
using SNetwork;
using System.Reflection;
using Global;

namespace MessageServer
{
	public class CThreadState
	{
		public string ID { get; set; }
		public int FPS { get; set; }
		public int InputCount { get; set; }
	}

    public partial class CNetManager : SSingleton<CNetManager>
	{
		private FormMain m_FormMain;
		public FormMain FormMain { get { return m_FormMain; } set { m_FormMain = value; } }

		public class CClientSession
		{
			public long SessionKey { get; set; }
			public bool Connected { get; set; }
			public bool Connecting { get; set; }
			
			public CClientSession()
            {
				SessionKey = -1;
            }
		}

		public delegate void MsgHandlerDelegate(long sessionKey, byte[] packet);

		private bool m_Stop;
		private SNetSystem m_NetSystem = new SNetSystem();
		private Dictionary<ushort, MsgHandlerDelegate> m_Handler = new Dictionary<ushort, MsgHandlerDelegate>();
		private SFPS m_FPS = new SFPS();

		private STimer m_HeartBeatTimer = new STimer(5000, false);

		public int RecvPacketCount { get; set; }
		public int SendPacketCount { get; set; }

		public int LogPacketCount { get; set; }
		public STimer m_MatchViewTimer = new STimer(100);

		public bool ReloadTable { get; set; } = false;

        public CNetManager()
		{
			SNetSystem.SendThreadSleep = 10;
			SNetSystem.TimeoutBothSide = true;
			//SNetSystem.Timeout = CConfig.Instance.Timeout;
			m_NetSystem.RegisterUpdateDelegate(new UpdateDelegate(OnUpdate));
			m_NetSystem.RegisterCloseSessionDelegate(new CloseSessionDelegate(OnCloseSession));
			m_NetSystem.RegisterAcceptSessionDelegate(new AcceptSessionDelegate(OnAcceptSession));
			m_NetSystem.RegisterConnectSessionDelegate(new ConnectSessionDelegate(OnConnectSession));
			m_NetSystem.RegisterRecvSessionDelegate(new RecvSessionDelegate(OnRecvSession));
			m_NetSystem.RegisterExceptionSessionDelegate(new ExceptionSessionDelegate(OnExceptionSession));

            SetupMsgHandler();
        }

        public void Start(ushort port)
		{
            CLogger.Instance.System(string.Format("Server Start - {0}", DateTime.Now.ToString()));
            CLogger.Instance.System(string.Format("GCMode - {0}", System.Runtime.GCSettings.IsServerGC ? "ServerGC" : "Default"));
            CLogger.Instance.System(string.Format("GCLatencyMode - {0}", System.Runtime.GCSettings.LatencyMode.ToString()));

            CDBManager.Instance.Start();
            CTableLoader.Init();
            CSystemManager.Instance.Init();
            CRankingManager.Instance.Init();
            CScheduleManager.Instance.Init();
            CChatManager.Instance.Init();
            CEventManager.Instance.Init();

            m_NetSystem.Start(port);

            CLogger.Instance.System($"NetSystem Start...");
		}

        public void Stop()
        {
            m_Stop = true;
            CLogger.Instance.System("Shutdown start...");

            CSystemManager.Instance.Stop();
            CRankingManager.Instance.Stop();
            CScheduleManager.Instance.Stop();
            CEventManager.Instance.Stop();

            CDBManager.Instance.Stop();

            m_NetSystem.Stop();

            CLogger.Instance.System("NetSystem stoped...");

            CLogger.Instance.System("Shutdown finished!");

			//SPipeServer.Instance.Stop();
        }
        public void CloseAllAcceptedSession()
		{
			m_NetSystem.CloseAllAcceptedSession();
		}

		public void CloseSession(long sessionKey, string closeType = null)
		{
			m_NetSystem.CloseSession(sessionKey);
		}

		public void Write(long sessionKey, INetPacket writeMsg, int buffSize = 1024)
		{
            SendPacketCount++;
			m_NetSystem.Write(sessionKey, writeMsg, buffSize);
		}

        public void WriteAllToServer(INetPacket writeMsg)
        {
            foreach (var it in CServerManager.Instance.m_Servers)
            {
                var server = it.Value;
            	Write(it.Key, writeMsg);
            }
        }

		public void WriteLog(long sessionKey, INetPacket writeMsg, int buffSize = 8192)
		{
            LogPacketCount++;

            m_NetSystem.Write(sessionKey, writeMsg, buffSize);			
        }

		public bool WriteLog_Debug(long sessionKey, INetPacket writeMsg, int buffSize = 8192)
        {
			LogPacketCount++;

			return m_NetSystem.Write(sessionKey , writeMsg , buffSize);
		}

		public void OnUpdate()
		{
            CDBManager.Instance.Update();
            CRankingManager.Instance.Update();
            CSystemManager.Instance.Update();
            CScheduleManager.Instance.Update();
            CChatManager.Instance.Update();
            CEventManager.Instance.Update();

            if (m_FPS.Update())
                m_FormMain.SetUIData(m_FPS.FPS, CServerManager.Instance.Servers);
		}

		public void OnCloseSession(long sessionKey, CloseType type)
		{
            var closeServer = CServerManager.Instance.Find(sessionKey);
            if (closeServer != null)
            {
                CLogger.Instance.System($"OnCloseSession {sessionKey} : {closeServer.m_DN} : {closeServer.m_Port}");
                CServerManager.Instance.Remove(sessionKey);
            }

            CLogger.Instance.System($"OnCloseSession {sessionKey}");
        }

		public void OnAcceptSession(long sessionKey)
		{
			CLogger.Instance.System(string.Format("OnAcceptSession[{0}]", sessionKey.ToString()));
		}

		public void OnConnectSession(long sessionKey, bool result)
		{
            CLogger.Instance.System(string.Format("OnConnectSession[{0}] : {1}", sessionKey.ToString(), result.ToString()));
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

			if (m_Handler.TryGetValue(protocol, out MsgHandlerDelegate func))
            {
                func(sessionKey, packet);
            }
            else
            {
				CLogger.Instance.Warning(string.Format("Cannot find MsgHandler : Protocol[{0}]", protocol));
			}
        }
        public void OnRecvSession(long sessionKey, byte[] packet, int msgLen)
        {
            RecvPacketCount++;
            if (packet.Length < 2)
            {
                CLogger.Instance.Warning(string.Format("Packet Length Error : {0}", packet.Length.ToString()));
                m_NetSystem.CloseSession(sessionKey);
                return;
            }

            ushort protocol = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, 0));
            if (m_Handler.TryGetValue(protocol, out MsgHandlerDelegate func))
            {
                func(sessionKey, packet.AsSpan(2).ToArray());
            }
            else
            {
                CLogger.Instance.Warning(string.Format("Cannot find MsgHandler : Protocol[{0}]", protocol));
            }
        }
        public void OnExceptionSession(long SessionKey, Exception e)
		{
			CLogger.Instance.System(string.Format("OnExceptionSession[{0}] : {1}", SessionKey.ToString(), e.Message));
            if (SessionKey < 0)
            {
                //m_FormMain.Stop = true;
            }
		}

        private void CheckServerSession()
        {
            if (m_Stop) return;

            //HeartBeat 전송
            //if (m_HeartBeatTimer.Check())
            //{
            //	if (m_MainSession.Connected)
            //		CNetManager.Instance.X2X_HeartBeat(m_MainSession.SessionKey);

            //	if (CConfig.Instance.LogEnable && m_LogSession.Connected)
            //		CNetManager.Instance.X2X_HeartBeat(m_LogSession.SessionKey);
            //}

            //         //MainServer 연결
            //         if (!m_MainSession.Connected && !m_MainSession.Connecting)
            //         {
            //             m_MainSession.Connecting = true;
            //             m_MainSession.SessionKey = m_NetSystem.Connect(CConfig.Instance.MainServerIP, CConfig.Instance.MainServerPort);
            //         }

            //if (CConfig.Instance.LogEnable)
            //{
            //	//LogServer연결
            //	if (!m_LogSession.Connected && !m_LogSession.Connecting)
            //	{
            //		m_LogSession.Connecting = true;
            //		m_LogSession.SessionKey = m_NetSystem.Connect(CConfig.Instance.LogServerIP, CConfig.Instance.LogServerPort);
            //	}
            //}
        }

        public bool IsAliveSession(long sessionKey)
        {
            return m_NetSystem.IsAliveSession(sessionKey);
        }

        public string GetSessionRemoteIP(long sessionKey)
        {
			return m_NetSystem.GetSessionRemoteIP(sessionKey);
        }
    }
}

