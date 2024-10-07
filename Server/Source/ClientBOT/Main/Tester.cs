using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBOT
{
    public class CTester
    {
        public class CClinetSession
        {
            public long m_SessionKey { get; set; }
            public bool Connected { get; set; }
            public bool Connecting { get; set; }
        }

        private CClinetSession m_ConnectSession = new CClinetSession();
        public CClinetSession ConnectSession => m_ConnectSession;

        public Action m_StageAction;
        public Action m_ContentsAction;

        public string m_DeviceID = string.Empty;
        public long m_UID = 0;
        public long ConnectDelay = 0;
        public long PlayTime = 0;

        public void Init(string deviceID)
        {
            m_DeviceID = deviceID;

            m_StageAction += StageAction;
            m_ContentsAction += ContentsAction;

            ConnectDelay = BOTSetting.Instance.ConnectDelay();
            PlayTime = BOTSetting.Instance.PlayTime();
        }

        private void ContentsAction()
        {
            //contents
        }

        private void StageAction()
        {
            //stage start
            
            //stage clear
        }

        public void Update()
        {
            if(m_ConnectSession.Connected)
            {
                Task.Run(() => 
                {
                    //stage packet
                });

                Task.Run(() => 
                { 
                    //contents packet
                });
            }
        }

        public void OnCloseSession(long sessionKey, CloseType type)
        {
            if(sessionKey == m_ConnectSession.m_SessionKey)
            {
                CNetManager.Instance.NetSystem.CloseSession(sessionKey);
                m_ConnectSession.m_SessionKey = -1;
                m_ConnectSession.Connecting = false;
                m_ConnectSession.Connected = false;
                CTesterManager.Instance.InsertRemove(m_ConnectSession.m_SessionKey);
            }
        }

        public void OnConnectSession(long sessionKey, bool result)
        {
            if (sessionKey == m_ConnectSession.m_SessionKey)
            {
                CNetManager.Instance.NetSystem.CloseSession(sessionKey);
                m_ConnectSession.m_SessionKey = -1;
                m_ConnectSession.Connecting = true;
                m_ConnectSession.Connected = true;
                CTesterManager.Instance.InsertRemove(m_ConnectSession.m_SessionKey);
            }
        }
    }
}
