using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SCommon;
using PlayServer;
using SDB;

namespace Global.RestAPI
{
    class CRestAgent
    {
        private Thread m_Thread;
        private volatile bool m_bRun = false;
        private ConcurrentQueue<IRestQuery> m_WaitList = new ConcurrentQueue<IRestQuery>();
        private ConcurrentQueue<IRestQuery> m_CompleteList = new ConcurrentQueue<IRestQuery>();

        public CRestAgent()
        {
            m_Thread = new Thread(new ThreadStart(ThreadFunc));
            m_Thread.Start();
        }
        private void ThreadFunc()
        {
            m_bRun = true;
            while(m_bRun)
            {
                IRestQuery query = null;
                while (m_WaitList.TryDequeue(out query))
                {
                    query.Excute();
                    m_CompleteList.Enqueue(query);
                }

                Thread.Sleep(1);
            }
        }

        public void Stop()
        {
            m_bRun = false;

            if (m_Thread != null)
                m_Thread.Join();
        }

        public void Update()
        {
            while (true)
            {
                IRestQuery query = null;
                if (!m_CompleteList.TryDequeue(out query))
                    break;

                query.Complete();
            }
        }

        public void Insert(IRestQuery query)
        {
            m_WaitList.Enqueue(query);
        }
    }

    class CRestManager : SSingleton<CRestManager>
    {
        private List<CRestAgent> m_Agents = new List<CRestAgent>();

        public bool Init()
        {
            int threadCnt = CConfig.Instance.RestThreadCount;
            if (threadCnt < 1)
                threadCnt = Math.Max(Environment.ProcessorCount / 2, 1);

            for (int i = 0; i < threadCnt; ++i)
            {
                var agent = new CRestAgent();
                m_Agents.Add(agent);
            }

            return m_Agents.Count > 0;
        }

        public void Stop()
        {
            foreach (var agent in m_Agents)
                agent.Stop();

            m_Agents.Clear();
        }

        private void Insert(long session, IRestQuery query)
        {
            if (session < 0) return;

            int idx = (int)(session % m_Agents.Count); // not 0 div.

            m_Agents[idx].Insert(query);
        }

        public void Update()
        {
            foreach (var agent in m_Agents)
                agent.Update();
        }

        #region API

        //public void RestAuth(long session, string token, int serverID, string remoteIP, eUserType userType, string country_code, eAuth auth)
        //{
        //    Insert(session, new CRestAuth(session, token, serverID, remoteIP, userType, country_code, auth));
        //}

        //public void RestAuth(long session, _AuthInfo authInfo)
        //{
        //    Insert(session, new CRestAuth(session, authInfo));
        //}

        public void RestIAP(long session, long pcID, string receipt, LogBson log)
        {
            Insert(session, new CRestIAP(session, pcID, receipt, log));
        }



        #endregion API

   
    }
}
