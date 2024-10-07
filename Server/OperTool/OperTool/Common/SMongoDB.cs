using System;
using System.Threading;
using System.Collections.Generic;
using MongoDB.Driver;
using SCommon;
using MongoDB.Bson;
using System.Linq;
using System.Collections.Concurrent;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace SDB
{
	public interface IMongoDBQuery
	{
		void Run(SMongoDB agent);
		void Complete();
	}

	public class SMongoDB
	{
		public delegate void LogDelegate(string Log);

		private bool m_Run;
		private Thread m_Thread;
		//private Queue<IMongoDBQuery> m_InputQueue = new Queue<IMongoDBQuery>();
		//private Queue<IMongoDBQuery> m_OutputQueue = new Queue<IMongoDBQuery>();

        private ConcurrentQueue<IMongoDBQuery> m_InputQueue = new ConcurrentQueue<IMongoDBQuery>();
        private ConcurrentQueue<IMongoDBQuery> m_OutputQueue = new ConcurrentQueue<IMongoDBQuery>();

        private MongoClient m_Client;
		private IMongoDatabase m_Database;

		private SFPS m_FPS = new SFPS();
		public int GetFPS() { return m_FPS.FPS; }
		public int GetInputQueueCount() { lock (m_InputQueue) { return m_InputQueue.Count; } }

		private static int LOOP_COUNT = 100;
		private static float DELAY_QUERY_TIME = 0.5f;
		private event LogDelegate m_LogDelegate;
		public void RegisterLogDelegate(LogDelegate Delegate) { m_LogDelegate += Delegate; }

		private void ThreadFunc()
		{
			while(m_Run)
			{
				int Count = 0;
				while(true)
				{
					//IMongoDBQuery Query = null;
					//lock (m_InputQueue) { if(m_InputQueue.Count > 0) Query = m_InputQueue.Dequeue(); }
					//if(Query == null) break;
					//DateTime Time = DateTime.UtcNow;
					//Query.Run(this);
					//TimeSpan Elapsed = DateTime.UtcNow - Time;
					//if(Elapsed.TotalSeconds > DELAY_QUERY_TIME && m_LogDelegate != null)
					//	m_LogDelegate(string.Format("DB : {0}, Query : {1}, Sec : {2}", m_Database, Query, Elapsed.TotalSeconds));

					//lock (m_OutputQueue) { m_OutputQueue.Enqueue(Query); }

                    IMongoDBQuery query = null;
                    if (!m_InputQueue.TryDequeue(out query))
                        break;

                    DateTime Time = DateTime.UtcNow;
                    query.Run(this);
                    TimeSpan Elapsed = DateTime.UtcNow - Time;
                    if (Elapsed.TotalSeconds > DELAY_QUERY_TIME && m_LogDelegate != null)
                        m_LogDelegate(string.Format("DB : {0}, Query : {1}, Sec : {2}", m_Database, query, Elapsed.TotalSeconds));

                    m_OutputQueue.Enqueue(query);

                    if (++Count == LOOP_COUNT) break;

				}
				m_FPS.Update();
				Thread.Sleep(1);
			}
		}

		public void Start(string ConnectionString, string DBName)
		{
			Stop();

			m_Run = true;

			m_Client = new MongoClient(ConnectionString);
			m_Database = m_Client.GetDatabase(DBName);

			m_Thread = new Thread(new ThreadStart(ThreadFunc));
			m_Thread.Start();
		}

		public void Stop()
		{
			if(!m_Run) return;

			m_Run = false;

			if(m_Thread != null)
			{
				m_Thread.Join();
				m_Thread = null;
			}
		}

		public void Update()
		{
			while(true)
			{
                //IMongoDBQuery Query = null;
                //lock (m_OutputQueue) { if(m_OutputQueue.Count > 0) Query = m_OutputQueue.Dequeue(); }
                //if(Query == null) break;
                //Query.Complete();

                IMongoDBQuery query;
                if (!m_OutputQueue.TryDequeue(out query))
                    break;

                query.Complete();
            }
		}

		public void Insert(IMongoDBQuery Query) { lock (m_InputQueue) { m_InputQueue.Enqueue(Query); } }

        public bool IsDatabaseExists(string databaseName)
        {
            var databases = m_Client.ListDatabaseNames().ToList();
            return databases.Contains(databaseName);
        }

        public bool IsCollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter }).ToList();
            return collections.Any();
        }

        public IMongoDatabase GetDataBase(string databaseName)
        {
            return m_Client.GetDatabase(databaseName);
        }
    }

    #region FilterBuilder
    //========================== MongoDB Filter =====================
    public class MongoFilterBuilder
    {
        private readonly List<FilterDefinition<BsonDocument>> _filters;

        public MongoFilterBuilder()
        {
            _filters = new List<FilterDefinition<BsonDocument>>();
        }

        public MongoFilterBuilder Equal(string field, BsonValue value)
        {
            _filters.Add(Builders<BsonDocument>.Filter.Eq(field, value));
            return this;
        }

        public MongoFilterBuilder NotEqual(string field, BsonValue value)
        {
            _filters.Add(Builders<BsonDocument>.Filter.Ne(field, value));
            return this;
        }

        public MongoFilterBuilder GreaterThan(string field, BsonValue value)
        {
            _filters.Add(Builders<BsonDocument>.Filter.Gt(field, value));
            return this;
        }

        public MongoFilterBuilder LessThan(string field, BsonValue value)
        {
            _filters.Add(Builders<BsonDocument>.Filter.Lt(field, value));
            return this;
        }

        public MongoFilterBuilder GreaterThanOrEqual(string field, BsonValue value)
        {
            _filters.Add(Builders<BsonDocument>.Filter.Gte(field, value));
            return this;
        }

        public MongoFilterBuilder LessThanOrEqual(string field, BsonValue value)
        {
            _filters.Add(Builders<BsonDocument>.Filter.Lte(field, value));
            return this;
        }

        public MongoFilterBuilder In(string field, IEnumerable<BsonValue> values)
        {
            _filters.Add(Builders<BsonDocument>.Filter.In(field, values));
            return this;
        }

        public MongoFilterBuilder NotIn(string field, IEnumerable<BsonValue> values)
        {
            _filters.Add(Builders<BsonDocument>.Filter.Nin(field, values));
            return this;
        }

        public MongoFilterBuilder Regex(string field, string pattern)
        {
            _filters.Add(Builders<BsonDocument>.Filter.Regex(field, new BsonRegularExpression(pattern)));
            return this;
        }

        public MongoFilterBuilder Between(string field, DateTime start, DateTime end)
        {
            _filters.Add(Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Gte(field, start),
                Builders<BsonDocument>.Filter.Lte(field, end)
            ));
            return this;
        }

        public FilterDefinition<BsonDocument> Build()
        {
            return Builders<BsonDocument>.Filter.And(_filters);
        }
    }
    #endregion

    public static class SBson
    {
        public static BsonDocument BsonObjectToBsonDocument<T>(T obj) where T : BsonBase
        {
            return obj.ToBsonDocument();
        }

        public static T BsonDocumentToBsonObject<T>(BsonDocument bsonDocument) where T : BsonBase
        {
            return BsonSerializer.Deserialize<T>(bsonDocument);
        }
    }

    public class BsonBase
    {
        [BsonId]
        public ObjectId logID { get; set; }

        [BsonElement("time")]
        public DateTime Time { get; set; } = DateTime.MinValue;
    }

    public class LogBson : BsonBase
    {
        [BsonElement("deviceID")]
        public string DeviceID { get; set; }

        [BsonElement("uid")]
        public long UID { get; set; }

        [BsonElement("type")]
        public ushort Type { get; set; }

        [BsonElement("log")]
        public string LogStr { get; set; } = string.Empty;

        [BsonElement("coin")]
        public string Update_Coins { get; set; } = string.Empty;

        [BsonElement("item")]
        public string Update_Items { get; set; } = string.Empty;

        [BsonElement("cnt")]
        public int Count { get; set; }
    }

}
