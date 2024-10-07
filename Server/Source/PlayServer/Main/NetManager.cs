using System;
using System.Net;
using System.Collections.Generic;
using SCommon;
using SNetwork;
using Global;
using System.Reflection;
using Global.RestAPI;

namespace PlayServer
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

        public CDefine.eServiceType ServiceType { get; set; }

        //public bool IsServerState(eServerState state) { return m_ServerState == state; }

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

        private CClientSession m_MessageServerSession = new CClientSession();
        
        public int RecvPacketCount { get; set; }
		public int SendPacketCount { get; set; }

		public int LogPacketCount { get; set; }
		public STimer m_MatchViewTimer = new STimer(100);

		public bool ReloadTable { get; set; } = false;

        
        public CNetManager()
		{
			SNetSystem.SendThreadSleep = 10;
			SNetSystem.TimeoutBothSide = true;
			SNetSystem.Timeout = CConfig.Instance.TimeOut;
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

            ServiceType = CConfig.Instance.ServiceType;

            CConfig.Instance.LoadGoogleIAP();
            CTableLoader.Init();
            CRestManager.Instance.Init();
            CCheatManager.Instance.Init();
            CGameLog.Instance.Start();
            CMongoDBManager.Instance.Start(CConfig.Instance.MongoDBConnection, CConfig.Instance.MongoDBName);
            CDBManager.Instance.Start();
            CLogger.Instance.System($"DBManger Start ...");
            CHotTimeManager.Instance.Init();
            CLogger.Instance.System($"Table Load..");

            CUserManager.Instance.Init();

            m_NetSystem.Start(port);
        }

        private bool IsMessageSession(long sessionKey)
        {
            return sessionKey == m_MessageServerSession.SessionKey;
        }

        public void Stop()
        {
            m_Stop = true;
            CLogger.Instance.System("Shutdown start...");

            CUserManager.Instance.Stop();

            m_NetSystem.Stop();

            CDBManager.Instance.Stop();

            CRestManager.Instance.Stop();

            CGameLog.Instance.Stop();
            CMongoDBManager.Instance.Stop();

            CLogger.Instance.System("NetSystem stoped...");

            CLogger.Instance.System("Shutdown finished!");

			SPipeServer.Instance.Stop();
        }
        public void CloseAllAcceptedSession()
		{
			m_NetSystem.CloseAllAcceptedSession();
		}

		public void CloseSession(long sessionKey, string closeType = null)
		{
			m_NetSystem.CloseSession(sessionKey);
			if(closeType != null) CLogger.Instance.Error(string.Format("Forced Client : [{0}]", closeType));
		}

		public void Write(long sessionKey, INetPacket writeMsg, int buffSize = 1024)
		{
            SendPacketCount++;
			m_NetSystem.Write(sessionKey, writeMsg, buffSize);
		}

		public void WriteAllToPlayer(INetPacket writeMsg)
		{
			foreach (var it in  CUserManager.Instance.UsersbySession)
			{
				var player = it.Value;
				Write(player.SessionKey, writeMsg);
			}
		}

        public void WriteMessageServer(INetPacket writeMsg)
        {
            Write(m_MessageServerSession.SessionKey, writeMsg);
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
            CheckServerSession();
            
            long check = DateTime.UtcNow.Ticks;
            CDBManager.Instance.Update();
            if (DateTime.UtcNow.Ticks - check > 500000)
                CLogger.Instance.System($"CDBManager tick delay : {DateTime.UtcNow.Ticks - check}");

            check = DateTime.UtcNow.Ticks;
            CRestManager.Instance.Update();
            if (DateTime.UtcNow.Ticks - check > 500000)
                CLogger.Instance.System($"CRestManager tick delay : {DateTime.UtcNow.Ticks - check}");

            check = DateTime.UtcNow.Ticks;
            CUserManager.Instance.Update();
            if (DateTime.UtcNow.Ticks - check > 500000)
                CLogger.Instance.System($"CUserManager tick delay : {DateTime.UtcNow.Ticks - check}");

            check = DateTime.UtcNow.Ticks;
            CGameLog.Instance.Update();
            if (DateTime.UtcNow.Ticks - check > 500000)
                CLogger.Instance.System($"CGameLog tick delay : {DateTime.UtcNow.Ticks - check}");

            check = DateTime.UtcNow.Ticks;
            CMongoDBManager.Instance.Update();
            if (DateTime.UtcNow.Ticks - check > 500000)
                CLogger.Instance.System($"CMongoDBManager tick delay : {DateTime.UtcNow.Ticks - check}");

            if (m_FPS.Update())
            {
                List<CThreadState> threadStateList = new List<CThreadState>();

                int dbMinFPS = int.MaxValue;
                int dbMaxInputCount = int.MinValue;
                CThreadState threadState = new CThreadState();
                threadState = new CThreadState();
                threadState.ID = "System DB Thread";
                threadStateList.Add(threadState);
                if (dbMinFPS > threadState.FPS) dbMinFPS = threadState.FPS;
                if (dbMaxInputCount < threadState.InputCount) dbMaxInputCount = threadState.InputCount;


                int[] fps = CDBManager.Instance.GetGameDBFPS();
                int[] inputQueueCount = CDBManager.Instance.GetGameDBInputQueueCount();

                for (int i = 0; i < fps.Length; i++)
                {
                    threadState = new CThreadState();
                    threadState.ID = string.Format("Game DB Thread[{0}]", i);
                    threadState.FPS = fps[i];
                    threadState.InputCount = inputQueueCount[i];
                    threadStateList.Add(threadState);
                    if (dbMinFPS > threadState.FPS) dbMinFPS = threadState.FPS;
                    if (dbMaxInputCount < threadState.InputCount) dbMaxInputCount = threadState.InputCount;
                }

                //threadState = new CThreadState();
                //threadState.ID = "Ranking DB Thread";
                //threadStateList.Add(threadState);
                //if (dbMinFPS > threadState.FPS) dbMinFPS = threadState.FPS;
                //if (dbMaxInputCount < threadState.InputCount) dbMaxInputCount = threadState.InputCount;

                threadState = new CThreadState();
                threadState.ID = "Log DB Thread";
                threadStateList.Add(threadState);
                if (dbMinFPS > threadState.FPS) dbMinFPS = threadState.FPS;
                if (dbMaxInputCount < threadState.InputCount) dbMaxInputCount = threadState.InputCount;

                m_FormMain.SetUIData(CConfig.Instance.ServiceType.ToString(), CUserManager.Instance.GetCount(), m_FPS.FPS, threadStateList, RecvPacketCount,
                SendPacketCount, LogPacketCount);
                RecvPacketCount = 0;
                SendPacketCount = 0;
                LogPacketCount = 0;
                
                if (ReloadTable)
                {
                    //LoadTable();
                    ReloadTable = false;
                    CLogger.Instance.System("ReloadTable finished!");
                }

                if (m_MessageServerSession.Connected)
                    CNetManager.Instance.P2M_ReportServerState((ushort)ServiceType, CUserManager.Instance.GetCount(), m_FPS.FPS, dbMinFPS, dbMaxInputCount);
            }
        }
		public void OnCloseSession(long sessionKey, CloseType type)
		{
            //message server 종료
            if(m_MessageServerSession.SessionKey == sessionKey)
            {
                CLogger.Instance.System(string.Format("MessageServer Disconnected[{0}]", sessionKey.ToString()));
                m_MessageServerSession.SessionKey = -1;
            }
            else
            {
                CLogger.Instance.System(string.Format("User Disconnected[{0}]", sessionKey.ToString()));
                CUserManager.Instance.DisConnect(sessionKey);
            }
        }

		public void OnAcceptSession(long sessionKey)
		{
			CLogger.Instance.System(string.Format("OnAcceptSession[{0}]", sessionKey.ToString()));
		}
		public void OnConnectSession(long sessionKey, bool result)
		{
			if (result)
			{
				var hasSession = m_NetSystem.FindSesssion(sessionKey);
				if (hasSession != null)
				{
					hasSession.IsServerSession = true;
				}
			}

            if (m_MessageServerSession.SessionKey == sessionKey)
            {
                if (result)
                {
                    CLogger.Instance.System(string.Format("MessageServer[{0}] Connected", sessionKey.ToString()));

                    m_MessageServerSession.Connected = true;
                    m_MessageServerSession.SessionKey = sessionKey;

                    CNetManager.Instance.P2M_RequestConnect(CUserManager.Instance.GetCount(), ServiceType, CConfig.Instance.IP, CConfig.Instance.DN, CConfig.Instance.PORT);
                }

                m_MessageServerSession.Connecting = false;
            }
            else
            {
                CLogger.Instance.System(string.Format("User Connected [{0}] : {1}", sessionKey.ToString(), result.ToString()));
            }
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
                m_FormMain.Stop = true;
            }
		}

        private void CheckServerSession()
        {
            if (m_Stop) return;

            //HeartBeat 전송
            if (m_HeartBeatTimer.Check())
            {
                if (m_MessageServerSession.Connected)
                    CNetManager.Instance.X2X_HeartBeat(m_MessageServerSession.SessionKey);
            }

            if (CConfig.Instance.IsMessageServer)
            {
                //MainServer 연결
                if (!m_MessageServerSession.Connected && !m_MessageServerSession.Connecting)
                {
                    m_MessageServerSession.Connecting = true;
                    m_MessageServerSession.SessionKey = m_NetSystem.Connect(CConfig.Instance.MessageServer.m_DN, CConfig.Instance.MessageServer.m_Port);
                }
            }
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

