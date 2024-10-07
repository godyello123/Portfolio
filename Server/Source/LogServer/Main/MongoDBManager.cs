using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using SDB;

namespace LogServer
{
    public class SMongoDBManager : SSingleton<SMongoDBManager>
    {
        private SMongoDB m_DB;

        public void Start(string connectionString, string dbname)
        {
            string error_str = string.Empty;
            m_DB = new SMongoDB();
            m_DB.Start(connectionString, dbname, ref error_str);
        }

        public void Stop()
        {
            m_DB.Stop();
        }

        public void Update()
        {
            m_DB.Update();
        }

        public void Insert(IMongoDBQuery query)
        {
            m_DB.Insert(query);
        }
    }
}
