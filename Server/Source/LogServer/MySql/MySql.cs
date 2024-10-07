//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
//using System.Data;
//using System.Reflection;
//using SCommon;
//using Global;
//using System.Threading;
//using SMySql;

//namespace LogServer
//{
//    public class CMySqlDBManager : SSingleton<CMySqlDBManager>, IDisposable
//    {
//        private bool m_Disposed= false;
//        private bool m_Run;
//        private SMySql[] m_LogDB;

//        public static DateTime m_curtime = CDefine.MinValue();
//        public static int m_ThredIndex = 0;

//        public static string InsertQuery = string.Empty;
//        public static string SelectQuery = string.Empty;

//        public int GetThreadIndex()
//        {
//            return 0;
//            if (m_ThredIndex >= m_LogDB.Length)
//                m_ThredIndex = 0;

//            int index = m_ThredIndex;
//            m_ThredIndex++;

//            return index;
//        }

//        ~CMySqlDBManager()
//        {
//            Dispose(false);
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (m_Disposed)
//                return;

//            if (disposing)
//            {
//                if (m_LogDB == null)
//                {
//                    foreach (var DB in m_LogDB)
//                    {
//                        DB.Dispose();
//                    }

//                    m_LogDB = null;
//                }
//            }
//        }

//        public MySqlConnection GetConnecter(int index)
//        {
//            if (index < 0 || index >= m_LogDB.Count())
//            {
//                return null;
//            }

//            return m_LogDB[index].GetAgent;
//        }

//        public void Start()
//        {
//            if (m_Run)
//                return;

//            m_Run = true;
//            string MySqlConnectStr = string.Empty;
//            //CConfig.Instance.LogDB.m_IP = "localhost";
//            //CConfig.Instance.LogDB.m_Schema = "test";
//            MySqlConnectStr = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", CConfig.Instance.LogDB.m_IP, CConfig.Instance.LogDB.m_Port, CConfig.Instance.LogDB.m_Schema, CConfig.Instance.LogDB.m_ID, CConfig.Instance.LogDB.m_Password);

//            try 
//            {
//                int ThreadCount = CConfig.Instance.LogThreadCount;
//                m_LogDB = new SMySql[ThreadCount];
//                for (int i = 0; i < ThreadCount; ++i)
//                {
//                    m_LogDB[i] = new SMySql();
//                    m_LogDB[i].Start(MySqlConnectStr);
//                }

//                m_curtime = DateTime.UtcNow;

//                InsertQuery = "Insert";
//                SelectQuery = "Select";
//            }
//            catch(Exception e)
//            {
//                string str = e.ToString();
//                CLogger.Instance.System(string.Format("Error : {0}", str));
//            }
           
//        }

//        public void Stop()
//        {
//            if (m_LogDB != null)
//            {
//                foreach (var DB in m_LogDB)
//                {
//                    DB.Stop();
//                }
//            }

//            m_Run = false;
//        }

//        public void Update()
//        {
//            if (m_LogDB != null)
//            {
//                foreach (var DB in m_LogDB)
//                {
//                    DB.Update();
//                }
//            }
//        }
//        public void SetParams<T>(ref MySqlDataAdapter adapter, ref ProcedureResult data, T obj)
//        {
//            //클래스 필드의 입력 인수 설정
//            Type objType = obj.GetType();
//            FieldInfo[] field = objType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

//            for (int i = 0; i < field.Length; ++i)
//            {
//                adapter.SelectCommand.Parameters.AddWithValue(field[i].Name, field[i].GetValue(obj)).Direction = ParameterDirection.Input;
//            }

//            //출력인수 설정
//            data.m_sqlErrorNumber = adapter.SelectCommand.Parameters.Add("sp_rtn", MySqlDbType.Int32);
//            data.m_sqlErrorNumber.Direction = ParameterDirection.Output;

//            data.m_sqlErrorMessage = adapter.SelectCommand.Parameters.Add("sp_msg", MySqlDbType.VarChar, 4000);
//            data.m_sqlErrorMessage.Direction = ParameterDirection.Output;

//            adapter.Fill(data.m_dataSet);
//        }

//        //public void SetParams<T>(ref MySqlCommand Command, ref ProcedureResult data, T obj)
//        //{
//        //    //클래스 필드의 입력 인수 설정
//        //    Type objType = obj.GetType();
//        //    FieldInfo[] field = objType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

//        //    for (int i = 0; i < field.Length; ++i)
//        //    {
//        //        Command.Parameters.AddWithValue(field[i].Name, field[i].GetValue(obj)).Direction = ParameterDirection.Input;
//        //    }

//        //    //출력인수 설정
//        //    data.m_sqlErrorNumber = Command.Parameters.Add("sp_rtn", MySqlDbType.Int32);
//        //    data.m_sqlErrorNumber.Direction = ParameterDirection.Output;

//        //    data.m_sqlErrorMessage = Command.Parameters.Add("sp_msg", MySqlDbType.VarChar, 4000);
//        //    data.m_sqlErrorMessage.Direction = ParameterDirection.Output;

//        //    adapter.Fill(data.m_dataSet);
//        //}
//        public void Insert(int Number, IMySqlQuery query)
//        {
//            return;
//            m_LogDB[Number % m_LogDB.Length].Insert(query);
//        }
//    }

//    public interface IMySqlQuery
//    {
//        void Run(SMySql agent);
//        void Complete();
//    }

//    public class SMySql
//    {
//        public delegate void LogDelegate(string log);

//        private bool m_Disposed = false;
//        private bool m_Run;
//        private Thread m_Thread;
//        private Queue<IMySqlQuery> m_InputQueue = new Queue<IMySqlQuery>();
//        private Queue<IMySqlQuery> m_OutputQueue = new Queue<IMySqlQuery>();
//        private MySqlConnection m_DB;


//        public int GetInputQueueCount()
//        {
//            lock (m_InputQueue)
//            {
//                return m_InputQueue.Count;
//            }
//        }

//        private static int LOOP_COUNT = 100;
//        private static float DELAY_QUERY_TIME = 0.5f;
//        private event LogDelegate m_LogDelegate;

//        public void RegisterLogDelegate(LogDelegate logDelegate)
//        {
//            m_LogDelegate += logDelegate;
//        }

//        public MySqlConnection GetAgent { get { return m_DB; } }

//        ~SMySql()
//        {
//            Dispose();
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (m_Disposed)
//                return;
//            if (disposing)
//            {
//                if (m_DB != null)
//                    m_DB.Dispose();
//                m_DB = null;
//            }
//        }

//        private void ThreadFunc()
//        {
//            while (m_Run)
//            {
//                int count = 0;
//                while (true)
//                {
//                    IMySqlQuery query = null;
//                    lock (m_InputQueue)
//                    {
//                        if (m_InputQueue.Count > 0)
//                            query = m_InputQueue.Dequeue();
//                    }

//                    if (query == null)
//                        break;

//                    DateTime time = DateTime.UtcNow;
//                    query.Run(this);

//                    TimeSpan elapsed = DateTime.UtcNow - time;
//                    //if (elapsed.TotalSeconds > DELAY_QUERY_TIME && m_LogDelegate != null)
//                        //m_LogDelegate(String.Format("MySql_Query : {0}, Sec : {1}", query, elapsed.TotalSeconds));

//                    lock (m_OutputQueue)
//                    {
//                        m_OutputQueue.Enqueue(query);
//                    }

//                    if (++count == LOOP_COUNT)
//                        break;
//                }
//                //m_FPS.Update();
//                Thread.Sleep(1);
//            }
//        }

//        public void Start(string ConnectStr)
//        {
//            //Stop();

//            //m_Run = true;
//            //try
//            //{
//            //    CLogger.Instance.System(ConnectStr);
//            //    m_DB = new MySqlConnection(ConnectStr);
//            //    m_DB.Open();
//            //}
//            //catch (Exception e)
//            //{
//            //    CLogger.Instance.Error(e.ToString());
//            //}

//            //m_Thread = new Thread(new ThreadStart(ThreadFunc));
//            //m_Thread.Start();
//        }

//        public void Stop()
//        {
//            if (!m_Run)
//                return;

//            m_Run = false;

//            if (m_Thread != null)
//            {
//                m_Thread.Join();
//                m_Thread = null;
//            }

//            m_DB.Close();
//        }

//        public void Update()
//        {
//            //while (true)
//            //{
//            //    IMySqlQuery query = null;
//            //    lock (m_OutputQueue)
//            //    {
//            //        if (m_OutputQueue.Count > 0)
//            //            query = m_OutputQueue.Dequeue();
//            //    }

//            //    if (query == null)
//            //        break;
//            //    query.Complete();
//            //}
//        }

//        public void Insert(IMySqlQuery query)
//        {
//            lock (m_InputQueue)
//            {
//                m_InputQueue.Enqueue(query);
//            }
//        }
//    }
//}


