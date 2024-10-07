using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using SNetwork;
using Global;

namespace ClientBOT
{
    public partial class CNetManager : SSingleton<CNetManager>
    {
        private Form1 m_FormMain;
        public Form1 FormMain { set { m_FormMain = value; }  get { return m_FormMain; } }
        //public void WriteLog(string log) { if (m_FormMain != null) m_FormMain.WriteLog(log); }

        public delegate void MsgHandlerDelegate(long sessionKey, byte[] packet);

        private SNetSystem m_NetSystem = new SNetSystem();
        public SNetSystem NetSystem { get { return m_NetSystem; } }
        private Dictionary<ushort, MsgHandlerDelegate> m_Handler = new Dictionary<ushort, MsgHandlerDelegate>();
        public Dictionary<ushort, MsgHandlerDelegate> Handler { get { return m_Handler; } }

        public CNetManager()
        {
            SNetSystem.TimeoutBothSide = true;

            m_NetSystem.RegisterCloseSessionDelegate(new CloseSessionDelegate(OnCloseSession));
            m_NetSystem.RegisterConnectSessionDelegate(new ConnectSessionDelegate(OnConnectSession));
            m_NetSystem.RegisterRecvSessionDelegate(new RecvSessionDelegate(OnRecvSession));
            m_NetSystem.RegisterExceptionSessionDelegate(new ExceptionSessionDelegate(OnExceptionSession));

            SetupMsgHandler();
        }

        public void Start()
        {
            m_NetSystem.Start();
        }
        public void Stop()
        {
            m_NetSystem.Stop();
        }
        public void Update()
        {
            m_NetSystem.Update();
        }

        public void OnCloseSession(long sessionKey, CloseType type)
        {
            var tester = CTesterManager.Instance.Find(sessionKey);
            if (tester != null) tester.OnCloseSession(sessionKey, type);
        }
        public void OnConnectSession(long sessionKey, bool result)
        {
            var tester = CTesterManager.Instance.Find(sessionKey);
            if (tester != null) tester.OnConnectSession(sessionKey, result);
        }

        public void OnRecvSession(long sessionKey, byte[] packet)
        {
            if (packet.Length < 2)
            {
                m_NetSystem.CloseSession(sessionKey);
                return;
            }

            ushort protocol = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, 0));
            if (!m_Handler.ContainsKey(protocol))
            {
                return;
            }
            m_Handler[protocol](sessionKey, packet);
        }

        public void OnExceptionSession(long sessionKey, Exception e)
        {
            //WriteLog(string.Format("OnExceptionSession[{0}] : {1}", sessionKey.ToString(), e.Message));
        }

    }
}
