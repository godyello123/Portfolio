using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Global;
using MongoDB.Bson;
using MongoDB.Driver.Core.Configuration;
using SCommon;
using SDB;

namespace PlayServer
{
    public class CGameLog : SSingleton<CGameLog>
    {
        private STimer m_Timer = new STimer(300 * 1000);
        private ConcurrentQueue<LogBson> m_Logs = new ConcurrentQueue<LogBson>();
        private readonly int m_LogSendCount = 1000;

        public void Start()
        {
        }

        public void Insert(LogBson log)
        {
            if (!CConfig.Instance.LogEnable)
                return;

            m_Logs.Enqueue(log);
            if (m_Logs.Count >= m_LogSendCount)
                Logging();
        }

        public void Update()
        {
            if (!m_Timer.Check())
                return;

            Logging();
        }

        public void Stop()
        {
            Logging();
        }

        private void Logging()
        {
            List<BsonDocument> list = new List<BsonDocument>();
            while(m_Logs.Count > 0)
            {
                if (m_Logs.TryDequeue(out var log))
                    list.Add(SBson.BsonObjectToBsonDocument(log));
            }

            CLogger.Instance.System($"Log Count : {list.Count}");

            if(list.Count > 0)
                CMongoDBManager.Instance.Insert(new InsertQueryMany(CConfig.Instance.MongoDBName, list));
        }
 
    }
}
