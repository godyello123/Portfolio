using MongoDB.Driver.Core.Servers;
using OperTool.Form;
using SCommon;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OperTool
{
    public partial class CNetManager : SSingleton<CNetManager>
    {
        private Form1 m_FormMain;
        public Form1 FormMain { get { return m_FormMain; } set { m_FormMain = value; } }

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

        public CClientSession m_ServerSession = new CClientSession();

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
        }

        public void Start(ushort port = 0)
        {
            SetupMsgHandler();
            m_NetSystem.Start(port);
        }

        public void Stop()
        {
            m_Stop = true;
            m_NetSystem.Stop();
        }

        public long ConneectServer(string ip, ushort port)
        {
            m_ServerSession.SessionKey = m_NetSystem.Connect(ip, port);
            m_ServerSession.Connecting = true;

            return m_ServerSession.SessionKey;
        }

        public void DisConnectServer()
        {
            CloseSession(m_ServerSession.SessionKey);
            m_ServerSession.SessionKey = -1;
            m_ServerSession.Connected = false;
            m_ServerSession.Connecting = false;
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

            return m_NetSystem.Write(sessionKey, writeMsg, buffSize);
        }

        public void OnUpdate()
        {
            //CheckServerSession();
        }
        public void OnCloseSession(long sessionKey, CloseType type)
        {
            //CLogger.Instance.System(string.Format("User Disconnected[{0}]", sessionKey.ToString()));
            if(sessionKey == m_ServerSession.SessionKey)
            {
                m_ServerSession.SessionKey = -1;
                m_ServerSession.Connected = false;
                m_ServerSession.Connecting = false;

                FormManager.Instance.InsertMainFormWork(new WorkDisConnectServer());
            }
        }

        public void OnAcceptSession(long sessionKey)
        {
            //CLogger.Instance.System(string.Format("OnAcceptSession[{0}]", sessionKey.ToString()));
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

                if(sessionKey == m_ServerSession.SessionKey)
                {
                    m_ServerSession.Connected = true;
                    FormManager.Instance.ShowMainForm();
                }
            }

            //CLogger.Instance.System(string.Format("User Connected [{0}] : {1}", sessionKey.ToString(), result.ToString()));
        }

        public void OnRecvSession(long sessionKey, byte[] packet)
        {
            RecvPacketCount++;
            if (packet.Length < 2)
            {
                //CLogger.Instance.Warning(string.Format("Packet Length Error : {0}", packet.Length.ToString()));
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
                //CLogger.Instance.Warning(string.Format("Cannot find MsgHandler : Protocol[{0}]", protocol));
            }
        }
        public void OnRecvSession(long sessionKey, byte[] packet, int msgLen)
        {
            RecvPacketCount++;
            if (packet.Length < 2)
            {
                //CLogger.Instance.Warning(string.Format("Packet Length Error : {0}", packet.Length.ToString()));
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
                //CLogger.Instance.Warning(string.Format("Cannot find MsgHandler : Protocol[{0}]", protocol));
            }
        }
        public void OnExceptionSession(long SessionKey, Exception e)
        {
            //CLogger.Instance.System(string.Format("OnExceptionSession[{0}] : {1}", SessionKey.ToString(), e.Message));
            if (SessionKey < 0)
            {
                //m_FormMain.Stop = true;
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
