//using System;
//using System.Collections.Generic;
//using StackExchange.Redis;
//using SCommon;
//using SDB;
//using Global;

//namespace PlayServer
//{
//    public class CQueryLog<T> : IRedisQuery where T : CLog
//    {
//        private T m_Log;

//        public CQueryLog(T log)
//        {
//            m_Log = log;
//        }

//        public void Run(ConnectionMultiplexer Client)
//        {
//            try
//            {
//            }
//            catch (Exception e)
//            {
//                CLogger.Instance.Error(e.ToString());
//            }
//        }
//        public void Complete()
//        {
//        }
//    }
//}
