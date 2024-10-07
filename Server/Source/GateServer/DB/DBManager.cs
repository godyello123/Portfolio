using System;
using System.Collections.Generic;
using SCommon;
using SDB;
using Global;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace GateServer
{
    public class CDBManager : SSingleton<CDBManager>, IDisposable
    {
        private bool m_Disposed;

        private bool m_Run;

        private SMsSql[] m_DB;

        public int totalcount = 0;

        ~CDBManager()
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
                if (m_DB != null)
                {
                    foreach (var DB in m_DB)
                        DB.Dispose();

                    m_DB = null;
                }
            }
            m_Disposed = true;
        }

        public void Start()
        {
            if (m_Run) return;
            m_Run = true;

            string connectDBString;
            connectDBString = 
                $"server = {CConfig.Instance.SystemDB.m_Host}; " +
                $"uid = {CConfig.Instance.SystemDB.m_ID}; " +
                $"pwd = {CConfig.Instance.SystemDB.m_PW}; " +
                $"database = {CConfig.Instance.SystemDB.m_Name}";

            string error_string = string.Empty;

            m_DB = new SMsSql[CConfig.Instance.DBThreadCount];
            for (int i = 0; i < m_DB.Length; i++)
            {
                m_DB[i] = new SMsSql();
                m_DB[i].Start(connectDBString, ref error_string);
            }

            if (false == string.IsNullOrEmpty(error_string))
                CLogger.Instance.System(error_string);
        }
        public void Stop()
        {
            if (m_DB != null) foreach (var DB in m_DB) DB.Stop();

            m_Run = false;
        }
        public void Update()
        {
            if (m_DB != null) foreach (var DB in m_DB) DB.Update();
        }

        public void Insert(int dbguid, IMsSqlQuery query)
        {
            if (dbguid > m_DB.Length)
                dbguid = (int)dbguid % m_DB.Length;

            m_DB[dbguid].Insert(query);
        }

        public int[] GetDBFPS()
        {
            if (m_DB == null) return new int[0];
            int[] fps = new int[m_DB.Length];
            for (int i = 0; i < fps.Length; i++) fps[i] = m_DB[i].GetFPS();
            return fps;
        }

        public int[] GetDBInputQueueCount()
        {
            if (m_DB == null) return new int[0];
            int[] inputQueueCount = new int[m_DB.Length];
            for (int i = 0; i < inputQueueCount.Length; i++) inputQueueCount[i] = m_DB[i].GetInputQueueCount();
            return inputQueueCount;
        }

        public void SetParam(ref SqlCommand sqlCmd, ref ProcedureResult data, ref SMsSql agent, object obj)
        {
            Type type = obj.GetType();
            FieldInfo[] field = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            for (int i = 0; i < field.Length; ++i)
            {
                string parameterName = string.Format("@{0}", field[i].Name);
                sqlCmd.Parameters.Add(parameterName, agent.GetSqlDbType(field[i].FieldType)).Direction = ParameterDirection.Input;
                sqlCmd.Parameters[parameterName].Value = field[i].GetValue(obj);
            }

            data.m_sqlErrorNumber = sqlCmd.Parameters.Add("@sp_rtn", SqlDbType.Int);
            data.m_sqlErrorNumber.Direction = ParameterDirection.Output;

            // 출력인수 설정     
            data.m_sqlErrorMessage = sqlCmd.Parameters.Add("@sp_msg", SqlDbType.VarChar, 4000);
            data.m_sqlErrorMessage.Direction = ParameterDirection.Output;

            return;
        }

        public void SetParam(ref SqlCommand sqlCmd, ref ProcedureResult data, ref SMsSql agent, string paramName , DataTable table)
        {

        }

        #region Query
        public void QueryAccountAuth(long sessionKey, int serverKey, string deviceID, CDefine.AuthType authType)
        {
            int dbguid = CServerDefine.GetDBGUID(deviceID);
            Insert(dbguid, new CQueryAccountAuth(sessionKey, serverKey, deviceID, authType));
        }

        #endregion


    }
}
