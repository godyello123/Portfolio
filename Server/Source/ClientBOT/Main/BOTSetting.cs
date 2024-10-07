using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;

namespace ClientBOT
{
    public class BOTSetting : SSingleton<BOTSetting>
    {
        public string m_Host = string.Empty;
        public ushort m_Port = 0;
        public string m_TesterName = string.Empty;
        public int m_TesterCount = 0;
        public int m_ConnectDelayMin = 0;
        public int m_ConenctDelayMax = 0;
        public int m_PlayTimeMin = 0;
        public int m_PlayTimeMax = 0; 

        public long ConnectDelay()
        {
            long retVal = SCommon.SRandom.Instance.Next(m_ConnectDelayMin, m_ConenctDelayMax) * 1000;
            return retVal;
        }

        public long PlayTime()
        {
            long retVal = SCommon.SRandom.Instance.Next(m_PlayTimeMin, m_PlayTimeMax) * 1000;
            return retVal;
        }
    }
}
