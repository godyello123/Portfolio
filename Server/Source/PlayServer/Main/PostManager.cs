using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using Global;

namespace PlayServer
{
    class CPostManager : SSingleton<CPostManager>
    {
        STimer m_Timer = new STimer(50000);
        Dictionary<long, _PostData> m_Posts = new Dictionary<long, _PostData>();
    }
}
