//using System;
//using System.Threading;
//using System.Collections.Generic;
//using Couchbase;
//using Couchbase.Configuration.Client;
//using Couchbase.Core;
//using Couchbase.Core.Serialization;
//using Couchbase.Authentication;
//using Newtonsoft.Json;
//using SCommon;

//namespace SDB
//{
//	public interface ICouchbaseQuery
//	{
//		void Run(IBucket bucket);
//		void Complete();
//	}

//	public class SCouchbase : IDisposable
//	{
//		public delegate void LogDelegate(string log);

//		private bool m_Disposed;

//		private bool m_Run;
//		private Thread m_Thread;
//		private Queue<ICouchbaseQuery> m_InputQueue = new Queue<ICouchbaseQuery>();
//		private Queue<ICouchbaseQuery> m_OutputQueue = new Queue<ICouchbaseQuery>();
//		private Cluster m_Cluster;
//		private IBucket m_Bucket;

//		private SFPS m_FPS = new SFPS();
//		public int GetFPS() { return m_FPS.FPS; }
//		public int GetInputQueueCount() { lock (m_InputQueue) { return m_InputQueue.Count; } }

//		private static int LOOP_COUNT = 100;
//		private static float DELAY_QUERY_TIME = 0.5f;
//		private event LogDelegate m_LogDelegate;
//		public void RegisterLogDelegate(LogDelegate logDelegate) { m_LogDelegate += logDelegate; }

//		~SCouchbase()
//		{
//			Dispose(false);
//		}
//		public void Dispose()
//		{
//			Dispose(true);
//			GC.SuppressFinalize(this);
//		}
//		protected virtual void Dispose(bool disposing)
//		{
//			if(m_Disposed) return;
//			if(disposing)
//			{
//				if(m_Cluster != null) m_Cluster.Dispose();
//				if(m_Bucket != null) m_Bucket.Dispose();
//				m_Cluster = null;
//				m_Bucket = null;
//			}
//			m_Disposed = true;
//		}

//		private void ThreadFunc()
//		{
//			while(m_Run)
//			{
//				int count = 0;
//				while(true)
//				{
//					ICouchbaseQuery query = null;
//					lock (m_InputQueue) { if(m_InputQueue.Count > 0) query = m_InputQueue.Dequeue(); }
//					if(query == null) break;
//					DateTime time = DateTime.UtcNow;
//					query.Run(m_Bucket);
//					TimeSpan elapsed = DateTime.UtcNow - time;
//					if(elapsed.TotalSeconds > DELAY_QUERY_TIME && m_LogDelegate != null)
//						m_LogDelegate(String.Format("Query : {0}, Sec : {1}", query, elapsed.TotalSeconds));

//					lock (m_OutputQueue) { m_OutputQueue.Enqueue(query); }

//					if(++count == LOOP_COUNT) break;
//				}
//				m_FPS.Update();
//				Thread.Sleep(1);
//			}
//		}

//		public void Start(string bucket, List<string> uriList, string userID, string password)
//		{
//			Stop();

//			m_Run = true;

//			var config = new ClientConfiguration
//			{
//				Serializer = () =>
//				{
//					var serializerSettings = new JsonSerializerSettings();
//					var deserializerSettings = new JsonSerializerSettings();
//					return new DefaultSerializer(serializerSettings, deserializerSettings);
//				}
//			};
//			config.Servers = new List<Uri>();
//			foreach(var value in uriList) config.Servers.Add(new Uri(value));

//			m_Cluster = new Cluster(config);
//			m_Cluster.Authenticate(new PasswordAuthenticator(userID, password));
//			m_Bucket = m_Cluster.OpenBucket(bucket);

//			m_Thread = new Thread(new ThreadStart(ThreadFunc));
//			m_Thread.Start();
//		}

//		public void Stop()
//		{
//			if(!m_Run) return;

//			m_Run = false;

//			if(m_Thread != null)
//			{
//				m_Thread.Join();
//				m_Thread = null;
//			}
//		}

//		public void Update()
//		{
//			while(true)
//			{
//				ICouchbaseQuery query = null;
//				lock (m_OutputQueue) { if(m_OutputQueue.Count > 0) query = m_OutputQueue.Dequeue(); }
//				if(query == null) break;
//				query.Complete();
//			}
//		}

//		public void Insert(ICouchbaseQuery query) { lock (m_InputQueue) { m_InputQueue.Enqueue(query); } }
//	}

//	public class SQueryUtil
//	{
//		public static IOperationResult<T> Replace<T>(IBucket bucket, ref IOperationResult<T> ret, Action<T> replace)
//		{
//			while(true)
//			{
//				T data = ret.Value;
//				replace(data);
//				var retReplace = bucket.Replace(ret.Id, data, ret.Cas);
//				if(retReplace.Success) return retReplace;
//				if(retReplace.Status != Couchbase.IO.ResponseStatus.KeyExists) return retReplace;
//				Thread.Sleep(1);
//				ret = bucket.Get<T>(ret.Id);
//				if(!ret.Success) return ret;
//			}
//		}

//		public static IOperationResult<T> Upsert<T>(IBucket bucket, ref IOperationResult<T> ret, Action<T> update)
//		{
//			while(true)
//			{
//				T data = ret.Value;
//				update(data);
//				var retUpsert = bucket.Upsert(ret.Id, data, ret.Cas);
//				if(retUpsert.Success) return retUpsert;
//				if(retUpsert.Status != Couchbase.IO.ResponseStatus.KeyExists) return retUpsert;
//				Thread.Sleep(1);
//				ret = bucket.Get<T>(ret.Id);
//				if(!ret.Success) return ret;
//			}
//		}

//		public static IOperationResult<T> GetInsert<T>(IBucket bucket, string key, Func<T> insert)
//		{
//			var retGet = bucket.Get<T>(key);
//			if(!retGet.Success && retGet.Status == Couchbase.IO.ResponseStatus.KeyNotFound)
//			{
//				T data = insert();
//				var retInsert = bucket.Insert(key, data);
//				if(!retInsert.Success) return retInsert;
//				retGet = bucket.Get<T>(key);
//			}
//			return retGet;
//		}
//	}
//}
