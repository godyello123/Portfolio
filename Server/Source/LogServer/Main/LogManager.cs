using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Global;
using MongoDB.Bson;
using SCommon;
using SDB;

namespace LogServer
{
    public class SLogManager : SSingleton<SLogManager>
    {
        private STimer m_Timer = new STimer();
        private ConcurrentQueue<LogBson> m_LogInputQueue = new ConcurrentQueue<LogBson>();
        private ConcurrentQueue<BsonDocument> m_LogOutputQueue = new ConcurrentQueue<BsonDocument>();
        private Thread m_Thread = null;
        private volatile bool m_Run = false;

        private int m_LogCount = 0;
        
        public void Start()
        {
            m_Run = true;

            m_Thread = new Thread(ThraedFunc);
            m_Thread.Start();
        }

        public void Stop()
        {
            if(m_Thread != null)
            {
                m_Thread.Join();
                m_Thread = null;
            }

            Logging();
        }
        

        public void Push(LogBson log)
        {
            m_LogInputQueue.Enqueue(log);
        }
        
        public void ThraedFunc()
        {
            while(m_Run)
            {
                while(m_LogInputQueue.TryDequeue(out LogBson log))
                {
                    m_LogOutputQueue.Enqueue(SBson.BsonObjectToBsonDocument(log));
                }

                Thread.Sleep(1);
            }
        }

        public void Update()
        {
            if (!m_Timer.Check())
                return;

            Logging();
        }

        public void Logging()
        {
            int cnt = 0;
            List<BsonDocument> retlist = new List<BsonDocument>();
            while (m_LogOutputQueue.TryDequeue(out BsonDocument doc))
            {
                retlist.Add(doc);
                if (cnt >= 10)
                {
                    SMongoDBManager.Instance.Insert(new InsertQueryList("", "", m_LogOutputQueue.ToList()));
                    cnt = 0;
                    retlist.Clear();
                }
            }

            if (retlist.Count > 0)
                SMongoDBManager.Instance.Insert(new InsertQueryList("", "", m_LogOutputQueue.ToList()));
        }
    }
}
