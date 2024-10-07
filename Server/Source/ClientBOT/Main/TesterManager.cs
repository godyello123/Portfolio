using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;
using SCommon;

namespace ClientBOT
{
    class CTesterManager : SSingleton<CTesterManager>
    {
        private Dictionary<string, CTester> m_Testers = new Dictionary<string, CTester>();
        private List<long> m_RemoveList = new List<long>();


        public void Start()
        {
            int numCount = BOTSetting.Instance.m_TesterCount;
            string name = BOTSetting.Instance.m_TesterName;
            for (int i = 0; i < numCount; ++i)
            {
                CTester tester = new CTester();
                tester.m_DeviceID = name + "_" + i.ToString();
                tester.ConnectDelay = BOTSetting.Instance.ConnectDelay();
                tester.PlayTime = BOTSetting.Instance.PlayTime();
                m_Testers.Add(tester.m_DeviceID, tester);
            }
        }

        public CTester Find(string testerName)
        {
            if (m_Testers.ContainsKey(testerName))
                return m_Testers[testerName];

            return null;
        }

        public void Remove(long sessionKey)
        {
            var removeTester = Find(sessionKey);
            if (removeTester == null)
                return;

            m_Testers.Remove(removeTester.m_DeviceID);
        }

        public CTester Find(long sessionKey)
        {
            var retVal = m_Testers.FirstOrDefault(x => x.Value.ConnectSession.m_SessionKey == sessionKey);

            return retVal.Value;
        }

        private void RefreshTester()
        {
            foreach (var it in m_RemoveList)
                Remove(it);

            m_RemoveList.Clear();
        }

        public void InsertRemove(long sessionKey)
        {
            m_RemoveList.Add(sessionKey);
        }
    
        public void Update()
        {
            RefreshTester();

            foreach (var iter in m_Testers)
            {
                var tester = iter.Value;
                tester.Update();
            }
        }
    }
}
