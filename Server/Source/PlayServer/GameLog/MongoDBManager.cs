using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;
using MongoDB.Bson;
using SCommon;
using SDB;

namespace PlayServer
{
    public class CMongoDBManager : SSingleton<CMongoDBManager>
    {
        private SMongoDB m_DB;
        private bool m_Run = false;


        public void Start(string connectionString, string dbname)
        {
            if (!CConfig.Instance.LogEnable)
                return;

            string error_string = string.Empty;
            m_DB = new SMongoDB();
            m_DB.Start(connectionString, dbname, ref error_string);

            m_Run = true;
            if (!string.IsNullOrEmpty(error_string))
                CLogger.Instance.System(error_string);
            else
                CLogger.Instance.System("LogDB Start....");
        }

        public void Stop()
        {
            if (!m_Run) return;

            m_DB.Stop();
            m_Run = false;
        }

        public void Update()
        {
            if (!m_Run) return;

            m_DB.Update();
        }

        public void Insert(IMongoDBQuery query)
        {
            m_DB.Insert(query);
        }
    }
}
