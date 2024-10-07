using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using SCommon;
using System.Data.SqlClient;
using System.Data;

namespace SDB
{
    public class IMsSqlQuery
    {
        public virtual void Run(SMsSql agent) { }
        public virtual void Complete() { }
    }

    public class SMsSql : IDisposable
    {
        public SqlDbType GetSqlDbType(Type t)
        {
            switch (t.Name)
            {
                case "Int32":
                case "int":
                    return SqlDbType.Int;
                case "Int64":
                    return SqlDbType.BigInt;
                case "String":
                    return SqlDbType.NVarChar;
                case "Byte":
                case "byte":
                    return SqlDbType.TinyInt;
                case "DateTime":
                    return SqlDbType.DateTime;
                case "float":
                    return SqlDbType.Real;
                case "Char":
                    return SqlDbType.Char;
            }

            return SqlDbType.Int;
        }

        public delegate void LogDelegate(string log);

        private bool m_Disposed;

        private volatile bool m_Run;
        private Thread m_Thread;
        private ConcurrentQueue<IMsSqlQuery> m_InputQueue = new ConcurrentQueue<IMsSqlQuery>();
        private ConcurrentQueue<IMsSqlQuery> m_OutputQueue = new ConcurrentQueue<IMsSqlQuery>();
        private SqlConnection m_DB;

        private SFPS m_FPS = new SFPS();
        public int GetFPS() { return m_FPS.FPS; }
        public int GetInputQueueCount() { return m_InputQueue.Count; }

        //private static int LOOP_COUNT = 100;
        private static float DELAY_QUERY_TIME = 0.5f;
        private event LogDelegate m_LogDelegate;
        public void RegisterLogDelegate(LogDelegate logDelegate) { m_LogDelegate += logDelegate; }

        public SqlConnection GetAgent { get { return m_DB; }  }

        ~SMsSql()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed) return;
            if (disposing)
            {
                if (m_DB != null) m_DB.Dispose();
                m_DB = null;
            }
            m_Disposed = true;
        }

        private void ThreadFunc()
        {
            while (m_Run || m_InputQueue.Count > 0)
            {
                while (m_InputQueue.TryDequeue(out IMsSqlQuery query))
                {
                    DateTime time = DateTime.UtcNow;
                    query.Run(this);
                    TimeSpan elapsed = DateTime.UtcNow - time;
                    if (elapsed.TotalSeconds > DELAY_QUERY_TIME && m_LogDelegate != null)
                        m_LogDelegate(String.Format("Query : {0}, Sec : {1}", query, elapsed.TotalSeconds));

                    m_OutputQueue.Enqueue(query);
                }

                m_FPS.Update();
                Thread.Sleep(1);
            }
        }

        public void Start(string connectStr, ref string error_string)
        {
            Stop();

            m_Run = true;

            try
            {
                m_DB = new SqlConnection(connectStr);
                m_DB.Open();
            }
            catch (Exception e)
            {
                error_string += e.Message;
                error_string += "\n";
            }

            m_Thread = new Thread(new ThreadStart(ThreadFunc));
            m_Thread.Start();
        }

        public void Stop()
        {
            if (!m_Run) return;

            m_Run = false;
            if (m_Thread != null)
            {
                m_Thread.Join();
                m_Thread = null;
            }

            m_DB.Close();

        }

        public void Update()
        {
            while (true)
            {
                IMsSqlQuery query = null;
                if (!m_OutputQueue.TryDequeue(out query))
                    break;

                query.Complete();
            }
        }

        public void Insert(IMsSqlQuery query) 
        { 
            m_InputQueue.Enqueue(query);
        }
    }
}
