//using System;
//using System.Threading;
//using System.Collections.Generic;
//using StackExchange.Redis;
//using SCommon;

//namespace SDB
//{
//	public interface IRedisQuery
//	{
//		void Run(ConnectionMultiplexer Client);
//		void Complete();
//	}

//	public class SRedis
//	{
//		public delegate void LogDelegate(string Log);

//		private bool m_Run;
//		private Thread m_Thread;
//		private Queue<IRedisQuery> m_InputQueue = new Queue<IRedisQuery>();
//		private Queue<IRedisQuery> m_OutputQueue = new Queue<IRedisQuery>();
//		private ConnectionMultiplexer m_Client;

//		private SFPS m_FPS = new SFPS();
//		public int GetFPS() { return m_FPS.FPS; }
//		public int GetInputQueueCount() { lock (m_InputQueue) { return m_InputQueue.Count; } }

//		private static int LOOP_COUNT = 100;
//		private static float DELAY_QUERY_TIME = 0.5f;
//		private event LogDelegate m_LogDelegate;
//		public void RegisterLogDelegate(LogDelegate Delegate) { m_LogDelegate += Delegate; }

//		private void ThreadFunc()
//		{
//			while(m_Run)
//			{
//				int Count = 0;
//				while(true)
//				{
//					IRedisQuery Query = null;
//					lock (m_InputQueue) { if(m_InputQueue.Count > 0) Query = m_InputQueue.Dequeue(); }
//					if(Query == null) break;
//					DateTime Time = DateTime.UtcNow;
//					Query.Run(m_Client);
//					TimeSpan Elapsed = DateTime.UtcNow - Time;
//					if(Elapsed.TotalSeconds > DELAY_QUERY_TIME && m_LogDelegate != null)
//						m_LogDelegate(string.Format("Query : {0}, Sec : {1}", Query, Elapsed.TotalSeconds));

//					lock (m_OutputQueue) { m_OutputQueue.Enqueue(Query); }

//					if(++Count == LOOP_COUNT) break;
//				}
//				m_FPS.Update();
//				Thread.Sleep(1);
//			}
//		}

//		public void Start(string ConnectionString)
//		{
//			Stop();

//			m_Run = true;

//			m_Client = ConnectionMultiplexer.Connect(ConnectionString);

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
//				IRedisQuery Query = null;
//				lock (m_OutputQueue) { if(m_OutputQueue.Count > 0) Query = m_OutputQueue.Dequeue(); }
//				if(Query == null) break;
//				Query.Complete();
//			}
//		}

//		public void Insert(IRedisQuery Query) { lock (m_InputQueue) { m_InputQueue.Enqueue(Query); } }
//	}
//}
