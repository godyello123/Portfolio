//using System;
//using System.Collections.Generic;
//using SCommon;
//using SDB;
//using Global;

//namespace PlayServer
//{
//	public class CLogManager : SSingleton<CLogManager>
//	{
//		private bool m_Run;

//		private SRedis m_LogDB;

//		public void OnLog(string log)
//		{
//			CLogger.Instance.Error(log);
//		}
//		public void Start()
//		{
//			if(m_Run) return;
//			m_Run = true;

//			m_LogDB = new SRedis();
//			m_LogDB.RegisterLogDelegate(new SRedis.LogDelegate(OnLog));
//			m_LogDB.Start("localhost");
//		}
//		public void Stop()
//		{
//			if(m_LogDB != null) m_LogDB.Stop();

//			m_Run = false;
//		}
//		public void Update()
//		{
//			if(m_LogDB != null) m_LogDB.Update();
//		}
//		public int GetFPS()
//		{
//			return (m_LogDB != null) ? m_LogDB.GetFPS() : 0;
//		}
//		public int GetInputQueueCount()
//		{
//			return (m_LogDB != null) ? m_LogDB.GetInputQueueCount() : 0;
//		}

//		//쿼리 함수
//		public void QueryLog(CLogCreate log)
//		{
//			m_LogDB.Insert(new CQueryLog<CLogCreate>(log));
//		}
//		public void QueryLog(CLogConnect log)
//		{
//			m_LogDB.Insert(new CQueryLog<CLogConnect>(log));
//		}
//	}
//}
