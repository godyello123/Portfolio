using MongoDB.Bson;
using MongoDB.Driver;
using OperTool.Form;
using SDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperTool
{
    public class FindQuery : IMongoDBQuery
    {
        private string m_DBName;
        private string m_Collection;
        private FilterDefinition<BsonDocument> m_Filter;
        private List<BsonDocument> m_OutList = new List<BsonDocument>();

        public FindQuery(string dbname, string colletion, FilterDefinition<BsonDocument> filter)
        {
            m_DBName = dbname;
            m_Collection = colletion;
            m_Filter = filter;
        }

        public void Run(SMongoDB client)
        {
            try
            {
                if (!client.IsDatabaseExists(m_DBName))
                    return;

                var db = client.GetDataBase(m_DBName);

                if (!client.IsCollectionExists(db, m_Collection))
                    return;

                var collection = db.GetCollection<BsonDocument>(m_Collection);

                m_OutList = collection.Find(m_Filter).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Complete()
        {
            int a = 0;

            var list = new List<LogBson>();
            foreach(var iter in m_OutList)
                list.Add(SBson.BsonDocumentToBsonObject<LogBson>(iter));

            FormManager.Instance.InsertMainFormWork(new WorkGameLog_GameLogLoad(list));
        }
    }
}
