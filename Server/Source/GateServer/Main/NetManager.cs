using System;
using System.Net;
using System.Collections.Generic;
using SCommon;
using SNetwork;
using System.Reflection;
using Global;
using Global.RestAPI;

namespace GateServer
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

            //FirebaseAdmin.Auth.FirebaseAuth a;
            SetupMsgHandler();
        }

        public void Start(ushort port)
		{
            CLogger.Instance.System(string.Format("Server Start - {0}", DateTime.Now.ToString()));
            CLogger.Instance.System(string.Format("GCMode - {0}", System.Runtime.GCSettings.IsServerGC ? "ServerGC" : "Default"));
            CLogger.Instance.System(string.Format("GCLatencyMode - {0}", System.Runtime.GCSettings.LatencyMode.ToString()));

            //ServerKey = CConfig.Instance.ServerKey;

            CRestManager.Instance.Init();
            CDBManager.Instance.Start();
            
            m_NetSystem.Start(port);
		}

        public void Stop()
        {
            m_Stop = true;
            CLogger.Instance.System("Shutdown start...");

            CRestManager.Instance.Stop();
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
			if(closeType != null)
                CLogger.Instance.Error(string.Format("Forced Client : [{0}]", closeType));
		}

		public void Write(long sessionKey, INetPacket writeMsg, int buffSize = 1024)
		{
            SendPacketCount++;
			m_NetSystem.Write(sessionKey, writeMsg, buffSize);
		}

		public void WriteLog(long sessionKey, INetPacket writeMsg, int buffSize = 8192)
		{
            LogPacketCount++;

            if (sessionKey <= -1)
				return;

            m_NetSystem.Write(sessionKey, writeMsg, buffSize);			
        }

		public bool WriteLog_Debug(long sessionKey, INetPacket writeMsg, int buffSize = 8192)
        {
			LogPacketCount++;

			if (sessionKey <= -1)
				return false;

			return m_NetSystem.Write(sessionKey , writeMsg , buffSize);
		}

		public void OnUpdate()
		{
			CheckServerSession();

            CDBManager.Instance.Update();
            CRestManager.Instance.Update();

            long check = DateTime.UtcNow.Ticks;
            if (m_FPS.Update())
			{
                List<CThreadState> threadStateList = new List<CThreadState>();

				int dbMinFPS = int.MaxValue;
				int dbMaxInputCount = int.MinValue;
				CThreadState threadState = new CThreadState();
				threadState = new CThreadState();
				threadState.ID = "System DB Thread";
				//threadState.FPS = CDBManager.Instance.GetAccountFPS();
				//threadState.InputCount = CDBManager.Instance.GetSystemInputQueueCount();
				threadStateList.Add(threadState);
				if(dbMinFPS > threadState.FPS) dbMinFPS = threadState.FPS;
				if(dbMaxInputCount < threadState.InputCount) dbMaxInputCount = threadState.InputCount;

				
                int[] fps = CDBManager.Instance.GetDBFPS();
				int[] inputQueueCount = CDBManager.Instance.GetDBInputQueueCount();
				
                for(int i = 0; i < fps.Length; i++)
				{
					threadState = new CThreadState();
					threadState.ID = string.Format("Game DB Thread[{0}]", i);
					threadState.FPS = fps[i];
					threadState.InputCount = inputQueueCount[i];
					threadStateList.Add(threadState);
					if(dbMinFPS > threadState.FPS) dbMinFPS = threadState.FPS;
					if(dbMaxInputCount < threadState.InputCount) dbMaxInputCount = threadState.InputCount;
				}

				threadState = new CThreadState();
				threadState.ID = "Ranking DB Thread";
				threadStateList.Add(threadState);
				if(dbMinFPS > threadState.FPS) dbMinFPS = threadState.FPS;
				if(dbMaxInputCount < threadState.InputCount) dbMaxInputCount = threadState.InputCount;

				threadState = new CThreadState();
				threadState.ID = "Log DB Thread";
				//threadState.FPS = CLogManager.Instance.GetFPS();
				//threadState.InputCount = CLogManager.Instance.GetInputQueueCount();
				threadStateList.Add(threadState);
				if(dbMinFPS > threadState.FPS) dbMinFPS = threadState.FPS;
				if(dbMaxInputCount < threadState.InputCount) dbMaxInputCount = threadState.InputCount;

    //            //todo CUserManager.Instance.GetCount() changed 
    //            m_FormMain.SetUIData(true, CUserManager.Instance.GetCount(), m_FPS.FPS, threadStateList, RecvPacketCount,
    //            SendPacketCount, LogPacketCount);
    //            RecvPacketCount = 0;
				//SendPacketCount = 0;
				//LogPacketCount = 0;
                //if (.Connected)
                //    P2M_ReportServerState(CConfig.Instance.Open, CUserManager.Instance.GetCount(), m_FPS.FPS, dbMinFPS, dbMaxInputCount);

    //            if (ReloadTable)
				//{
				//	//LoadTable();
				//	ReloadTable = false;
				//	CLogger.Instance.System("ReloadTable finished!");
				//}
            }
		}
        public void OnCloseSession(long sessionKey, CloseType type)
        {
            CLogger.Instance.System(string.Format("User Disconnected[{0}]", sessionKey.ToString()));
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

            CLogger.Instance.System(string.Format("User Connected [{0}] : {1}", sessionKey.ToString(), result.ToString()));
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

