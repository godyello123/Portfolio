using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Global;
using SCommon;

namespace GateServer
{
    public class CSystemManager : SSingleton<CSystemManager>
    {
        private Dictionary<string, CBlockUser> m_BlockList = new Dictionary<string, CBlockUser>();
        private Dictionary<string, CWhiteUser> m_WhiteList = new Dictionary<string, CWhiteUser>();

        public void Init()
        {
            m_BlockList.Clear();

            CDBManager.Instance.QuerySystemDataLoad();
        }

        public void LoadSystemData(List<CBlockUser> blocks, List<CWhiteUser> whites)
        {
            foreach (var it in blocks)
                m_BlockList[it.m_DeviceID] = it;

            foreach (var it in whites)
                m_WhiteList[it.m_DeviceID] = it;
        }

        public CBlockUser FindBlockUser(string deviceID)
        {
            if (m_BlockList.TryGetValue(deviceID, out var retval))
                return retval;

            return null;
        }

        public CWhiteUser FindWhiteUwser(string deviceID)
        {
            if (m_WhiteList.TryGetValue(deviceID, out var retval))
                return retval;

            return null;
        }

        public void UpsertBlockUser(CBlockUser blockUser)
        {
            m_BlockList[blockUser.m_DeviceID] = blockUser;
        }

        public void RemoveWhiteUser(string targetid)
        {
            m_WhiteList.Remove(targetid);
        }
        
        public void UpsertWhiteUser(CWhiteUser whiteuser)
        {
            m_WhiteList[whiteuser.m_DeviceID] = whiteuser;
        }

        public bool IsBlockUser(string deviceid)
        {
            var banUser = FindBlockUser(deviceid);
            if (banUser == null)
                return false;

            if (SDateManager.Instance.IsExpired(banUser.m_ExpTime))
                return false;

            return true;
        }

        public bool IsWhiteUser(string deviceID)
        {
            var user = FindWhiteUwser(deviceID);
            if (user == null)
                return false;

            return true;
        }

    }
}
