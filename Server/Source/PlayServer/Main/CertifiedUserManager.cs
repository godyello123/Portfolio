using System;
using System.Collections.Generic;
using SCommon;
using Global;

namespace PlayServer
{
    class CCertifiedUserManager : SSingleton<CCertifiedUserManager>
    {
        private Dictionary<Int64, CCertifiedUser> m_CertifiedTable = new Dictionary<Int64, CCertifiedUser>();
        private Dictionary<string, CCertifiedUser> m_CertifiedDeviceTable = new Dictionary<string, CCertifiedUser>();
        private List<_CertifiedKey> m_CertifiedList = new List<_CertifiedKey>();

        public Dictionary<Int64, CCertifiedUser> CertifiedTable { get { return m_CertifiedTable; } }

        public List<_CertifiedKey> GetCertifiedKeyList()
        {
            return m_CertifiedList;
        }

        public void Update()
        {
            foreach(var Data in m_CertifiedTable)
            {
                if(Data.Value.Certified.m_CertifiedState==eCertifiedState.Ready)
                    Data.Value.Update();
            }
        }

        public CCertifiedUser FindbyAccountID(Int64 accountID)
        {
            if (m_CertifiedTable.ContainsKey(accountID))
                return m_CertifiedTable[accountID];

            return null;
        }

        public CCertifiedUser FindbyDeviceID(string Deviceid)
        {
            if (m_CertifiedDeviceTable.ContainsKey(Deviceid))
                return m_CertifiedDeviceTable[Deviceid];

            return null;
        }

        public void Erase(Int64 accountid)
        {
            CCertifiedUser EraseUser = FindbyAccountID(accountid);
            if (EraseUser!=null)
            {
                m_CertifiedDeviceTable.Remove(EraseUser.Certified.m_DeviceID);
                m_CertifiedTable.Remove(accountid);
            }
        }
        public CCertifiedUser CreateCertifiedUser(long SessionKey, int ServerID, string DeviceID, Int64 AccountID, eCertifiedState eCertifiedState)
        {

            CCertifiedUser NewUser = new CCertifiedUser(SessionKey,ServerID,DeviceID,AccountID,new _CertifiedKey(eCertifiedState));

            if (!m_CertifiedTable.ContainsKey(AccountID))
            {
                m_CertifiedTable.Add(AccountID, NewUser);
                m_CertifiedList.Add(NewUser.Certified);
                m_CertifiedDeviceTable.Add(DeviceID, NewUser);
                return NewUser;
            }

            return null;
        }
    }
}
