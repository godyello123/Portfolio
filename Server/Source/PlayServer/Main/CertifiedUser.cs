using System;
using System.Collections.Generic;
using SCommon;
using Global;

namespace PlayServer
{
    class CCertifiedUser
    {
        private long m_SessionKey = 0;
        private _CertifiedKey m_Certified = new _CertifiedKey();

        public long SessionKey { get { return m_SessionKey; } }
        public _CertifiedKey Certified { get { return m_Certified; } }

        public CCertifiedUser()
        {

        }

        public CCertifiedUser(long sessionKey, int serverId, string deviceid, Int64 accountId, _CertifiedKey certkey)
        {
            m_SessionKey = sessionKey;
            m_Certified.m_ServerKey= serverId;
            m_Certified.m_DeviceID = deviceid;
            m_Certified.m_AccountID = accountId;
            m_Certified.m_CertifiedState = certkey.m_CertifiedState;
            m_Certified.m_CertifiedTime = certkey.m_CertifiedTime;
        }

        public void Update()
        {
            DateTime Now = DateTime.UtcNow;
            TimeSpan espan = Now - m_Certified.m_CertifiedTime;

            if (espan.TotalMinutes >= 2)
                CertifiedTimeOut();
        }

        public void SetAccountID(Int64 accountID)
        {
            m_Certified.m_AccountID = accountID;
        }

        public void SetCertified(_CertifiedKey certKey)
        {
            m_Certified.m_CertifiedState = certKey.m_CertifiedState;
            m_Certified.m_CertifiedTime = certKey.m_CertifiedTime;
            m_Certified.m_ServerKey = certKey.m_ServerKey;
            m_Certified.m_DeviceID = certKey.m_DeviceID;
            m_Certified.m_AccountID = certKey.m_AccountID;
        }

        private void CertifiedTimeOut()
        {
            //loginServer Send
            CNetManager.Instance.RequestEraseUser(m_Certified.m_DeviceID, m_Certified.m_AccountID);

            CCertifiedUserManager.Instance.Erase(m_Certified.m_AccountID);
        }
    }
}
