using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using MongoDB.Driver.Core.Events;
using SDB;

namespace OperTool
{
    public class MongoDBManager : SSingleton<MongoDBManager>
    {
        private SMongoDB m_DB;

        public void Start(string connectionString, string dbname)
        {
            m_DB = new SMongoDB();
            m_DB.Start(connectionString, dbname);
        }

        public void Stop()
        {
            if (m_DB == null)
                return;

            m_DB.Stop();
        }

        public void Update()
        {
            if (m_DB == null)
                return;

            m_DB.Update();
        }

        public void Insert(IMongoDBQuery query)
        {
            if (m_DB == null)
                return;

            m_DB.Insert(query);
        }
    }
}
