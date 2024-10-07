using System;
using System.Collections.Generic;
using SCommon;
using SDB;
using Global;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Google.Apis.AndroidPublisher.v3.Data;
using System.Security.Cryptography.X509Certificates;

namespace GateServer
{
    #region base

    public class ProcedureResult
    {
        public DataSet m_dataSet = new DataSet();
        public SqlParameter m_sqlErrorNumber = new SqlParameter();
        public SqlParameter m_sqlErrorMessage = new SqlParameter();

        ~ProcedureResult()
        {
            if (m_dataSet != null)
                m_dataSet.Dispose();

            m_dataSet = null;
            m_sqlErrorNumber = null;
            m_sqlErrorMessage = null;
        }

        public bool IsSuccess()
        {
            return m_sqlErrorNumber.Value != null && (int)m_sqlErrorNumber.Value == 0;
        }

        public bool IsData(int index = 0)
        {
            return m_dataSet != null && m_dataSet.Tables != null && m_dataSet.Tables.Count > index && m_dataSet.Tables[index].Rows.Count > 0;
        }

        public void PrintErrorLog(string procedureName)
        {
            CLogger.Instance.Error($"{procedureName} : Error no = {GetErrorNumber()}, msg = {GetErrorMessage()}");
        }

        public bool GetData(int index, string name, ref int arg)
        {
            if (IsData(index) == false)
                return false;

            foreach (DataRow row in m_dataSet.Tables[index].Rows)
            {
                if (row[name] != null)
                {
                    try
                    {
                        arg = int.Parse(row[name].ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref int arg, int row_index)
        {
            if (IsData(index) == false)
                return false;

            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
                return false;

            DataRow row = m_dataSet.Tables[index].Rows[row_index];
            if (row[name] != null)
            {
                try
                {
                    arg = int.Parse(row[name].ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref DateTime arg)
        {
            if (IsData(index) == false)
                return false;

            foreach (DataRow row in m_dataSet.Tables[index].Rows)
            {
                if (row[name] != null)
                {
                    try
                    {
                        arg = DateTime.Parse(row[name].ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref DateTime arg, int row_index)
        {
            if (IsData(index) == false)
                return false;

            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
                return false;

            DataRow row = m_dataSet.Tables[index].Rows[row_index];
            if (row[name] != null)
            {
                try
                {
                    arg = DateTime.Parse(row[name].ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref short arg)
        {
            if (IsData(index) == false)
                return false;

            foreach (DataRow row in m_dataSet.Tables[index].Rows)
            {
                if (row[name] != null)
                {
                    try
                    {
                        arg = short.Parse(row[name].ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref short arg, int row_index)
        {
            if (IsData(index) == false)
                return false;

            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
                return false;

            DataRow row = m_dataSet.Tables[index].Rows[row_index];
            if (row[name] != null)
            {
                try
                {
                    arg = short.Parse(row[name].ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref ushort arg)
        {
            if (IsData(index) == false)
                return false;

            foreach (DataRow row in m_dataSet.Tables[index].Rows)
            {
                if (row[name] != null)
                {
                    try
                    {
                        arg = ushort.Parse(row[name].ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref ushort arg, int row_index)
        {
            if (IsData(index) == false)
                return false;

            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
                return false;

            DataRow row = m_dataSet.Tables[index].Rows[row_index];
            if (row[name] != null)
            {
                try
                {
                    arg = ushort.Parse(row[name].ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref Int64 arg)
        {
            if (IsData(index) == false)
                return false;

            foreach (DataRow row in m_dataSet.Tables[index].Rows)
            {
                if (row[name] != null)
                {
                    try
                    {
                        arg = Int64.Parse(row[name].ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref Int64 arg, int row_index)
        {
            if (IsData(index) == false)
                return false;

            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
                return false;

            DataRow row = m_dataSet.Tables[index].Rows[row_index];
            if (row[name] != null)
            {
                try
                {
                    arg = Int64.Parse(row[name].ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref string arg)
        {
            if (IsData(index) == false)
                return false;

            foreach (DataRow row in m_dataSet.Tables[index].Rows)
            {
                if (row[name] != null)
                {
                    try
                    {
                        arg = row[name].ToString();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref string arg, int row_index)
        {
            if (IsData(index) == false)
                return false;

            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
                return false;

            DataRow row = m_dataSet.Tables[index].Rows[row_index];
            if (row[name] != null)
            {
                try
                {
                    arg = row[name].ToString();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref bool arg)
        {
            if (IsData(index) == false)
                return false;

            foreach (DataRow row in m_dataSet.Tables[index].Rows)
            {
                if (row[name] != null)
                {
                    try
                    {
                        arg = bool.Parse(row[name].ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public bool GetData(int index, string name, ref bool arg, int row_index)
        {
            if (IsData(index) == false)
                return false;

            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
                return false;

            DataRow row = m_dataSet.Tables[index].Rows[row_index];
            if (row[name] != null)
            {
                try
                {
                    arg = bool.Parse(row[name].ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool GetData_Enum<T>(int index, string name, ref T arg) where T : struct, IConvertible
        {
            int iVal = 0;
            if (!GetData(index, name, ref iVal, 0))
                return false;

            arg = (T)(object)iVal;
            return true;
        }
        public bool GetData_Enum<T>(int index, string name, ref T arg, int row_index) where T : struct, IConvertible
        {
            int iVal = 0;
            if (!GetData(index, name, ref iVal, row_index))
                return false;

            arg = (T)(object)iVal;
            return true;
        }

        public void SetData(int index, string name, string value)
        {
            if (IsData() == false)
                return;

            DataTable table = m_dataSet.Tables[index];
            if (table.Columns.Contains(name) == false)
                table.Columns.Add(name);

            foreach (DataRow row in table.Rows)
            {
                if (row[name] != null)
                    row[name] = value;
            }
        }

        public int GetErrorNumber()
        {
            if (m_sqlErrorNumber == null)
                return -1;

            return m_sqlErrorNumber.Value == null ? -1 : (int)m_sqlErrorNumber.Value;
        }

        public string GetErrorMessage()
        {
            if (m_sqlErrorMessage == null || m_sqlErrorMessage.Value == null)
                return "Error Message null";

            return m_sqlErrorMessage.Value.ToString();
        }
    }
    #endregion

    public class CQueryAccountAuth : IMsSqlQuery
    {
        //input
        private long m_SessionKey;
        private int m_ServerKey;
        private string m_Token;
        private ushort m_AuthType;

        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private long m_AccountID;
        private string m_IP;
        private string m_DN;
        private int m_Port;

        struct DBParams
        {
            public int serverId;
            public string token;
            public ushort auth_type;
        }

        public CQueryAccountAuth(long sessionKey, int serverkey, string deviceid, CDefine.AuthType type)
        {
            m_SessionKey = sessionKey;
            m_ServerKey = serverkey;
            m_Token = deviceid;
            m_AuthType = (ushort)type;
        }
        
        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_account_auth";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    serverId = m_ServerKey,
                    token = m_Token,
                    auth_type = m_AuthType
                });

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
                    CLogger.Instance.Error(m_Data.GetErrorMessage());
                }
                else
                {
                    int table_index = 0;
                    m_Data.GetData(table_index, "uid", ref m_AccountID);
                    ++table_index;
                }

                sqlAdapter.Dispose();
                sqlCmd.Dispose();
            }
            catch (Exception e)
            {
                m_Result = Packet_Result.Result.DBError;
                CLogger.Instance.Error(e.ToString());
            }
        }

        public override void Complete()
        {
            if (!CNetManager.Instance.IsAliveSession(m_SessionKey))
                return;

            if(m_Result == Packet_Result.Result.Success)
                CNetManager.Instance.G2C_ResultAuth(m_SessionKey, (ushort)m_Result, m_AccountID, CConfig.Instance.PlayServerDN, CConfig.Instance.PlayServerPort);
            else
                CNetManager.Instance.G2C_ResultAuth(m_SessionKey, (ushort)m_Result, -1, "", (ushort)m_Port);
        }
    }

    //public class CQuerySystemDataLoad : IMsSqlQuery
    //{
    //    //input
     
    //    //Output 변수
    //    private Packet_Result.Result m_Result = Packet_Result.Result.Success;
    //    private ProcedureResult m_Data = new ProcedureResult();
    //    private List<CBlockUser> m_BlockList = new List<CBlockUser>();
    //    private List<CWhiteUser> m_WhiteList = new List<CWhiteUser>();


    //    public CQuerySystemDataLoad()
    //    {
    //    }

    //    public override void Run(SMsSql agent)
    //    {
    //        try
    //        {
    //            // DB연결 및 SP명 설정
    //            SqlCommand sqlCmd = new SqlCommand();
    //            sqlCmd.Connection = agent.GetAgent;
    //            sqlCmd.CommandType = CommandType.StoredProcedure;
    //            sqlCmd.CommandText = "sp_system_load";

    //            // 입출력인수 설정
    //            CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, this);
                
    //            // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
    //            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
    //            sqlAdapter.SelectCommand = sqlCmd;
    //            sqlAdapter.Fill(m_Data.m_dataSet);

    //            if (!m_Data.IsSuccess())
    //            {
    //                m_Data.PrintErrorLog(sqlCmd.CommandText);
    //                m_Result = Packet_Result.Result.DBError;
    //                CLogger.Instance.Error(m_Data.GetErrorMessage());
    //            }
    //            else
    //            {
    //                //blockuser
    //                int table_index = 0;
    //                for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
    //                {
    //                    CBlockUser data = new CBlockUser();
    //                    m_Data.GetData(table_index, "device_id", ref data.m_DeviceID, i);
    //                    m_Data.GetData(table_index, "cnt", ref data.m_Count, i);
    //                    m_Data.GetData(table_index, "exp_time", ref data.m_ExpTime, i);

    //                    m_BlockList.Add(data);
    //                }

    //                //white user
    //                ++table_index;
    //                for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
    //                {
    //                    CWhiteUser data = new CWhiteUser();
    //                    m_Data.GetData(table_index, "device_id", ref data.m_DeviceID, i);
    //                    m_Data.GetData(table_index, "create_time", ref data.m_CreateTime, i);

    //                    m_WhiteList.Add(data);
    //                }
    //            }

    //            sqlAdapter.Dispose();
    //            sqlCmd.Dispose();
    //        }
    //        catch (Exception e)
    //        {
    //            m_Result = Packet_Result.Result.DBError;
    //            CLogger.Instance.Error(e.ToString());
    //        }
    //    }

    //    public override void Complete()
    //    {
    //        if (m_Result != Packet_Result.Result.Success)
    //            return;

    //        CSystemManager.Instance.LoadSystemData(m_BlockList, m_WhiteList);
    //    }
    //}

    //public class CQuerySystemDeleteWhiteUser : IMsSqlQuery
    //{
    //    struct DBParams
    //    {
    //        public string device_id;

    //        public void SetParams(string targetid)
    //        {
    //            device_id = targetid;
    //        }
    //    }

    //    //input
    //    private string m_TargetDeviceID = string.Empty;
    //    private DBParams m_Params = new DBParams();

    //    //Output 변수
    //    private Packet_Result.Result m_Result = Packet_Result.Result.Success;
    //    private ProcedureResult m_Data = new ProcedureResult();
    //    private CWhiteUser m_Target = new CWhiteUser();

    //    public CQuerySystemDeleteWhiteUser(string targetID)
    //    {
    //        m_TargetDeviceID = targetID;
    //        m_Params.SetParams(targetID);
    //    }

    //    public override void Run(SMsSql agent)
    //    {
    //        try
    //        {
    //            // DB연결 및 SP명 설정
    //            SqlCommand sqlCmd = new SqlCommand();
    //            sqlCmd.Connection = agent.GetAgent;
    //            sqlCmd.CommandType = CommandType.StoredProcedure;
    //            sqlCmd.CommandText = "sp_system_delete_white_user";

    //            // 입출력인수 설정
    //            CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

    //            // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
    //            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
    //            sqlAdapter.SelectCommand = sqlCmd;
    //            sqlAdapter.Fill(m_Data.m_dataSet);

    //            if (!m_Data.IsSuccess())
    //            {
    //                m_Data.PrintErrorLog(sqlCmd.CommandText);
    //                m_Result = Packet_Result.Result.DBError;
    //                CLogger.Instance.Error(m_Data.GetErrorMessage());
    //            }

    //            sqlAdapter.Dispose();
    //            sqlCmd.Dispose();
    //        }
    //        catch (Exception e)
    //        {
    //            m_Result = Packet_Result.Result.DBError;
    //            CLogger.Instance.Error(e.ToString());
    //        }
    //    }

    //    public override void Complete()
    //    {
    //        if (m_Result != Packet_Result.Result.Success)
    //            return;

    //        CSystemManager.Instance.RemoveWhiteUser(m_TargetDeviceID);
    //    }
    //}

    //public class CQuerySystemDeleteBlockUser : IMsSqlQuery
    //{
    //    struct DBParams
    //    {
    //        public string device_id;

    //        public void SetParams(string targetid)
    //        {
    //            device_id = targetid;
    //        }
    //    }

    //    //input
    //    private string m_TargetDeviceID = string.Empty;
    //    private DBParams m_Params = new DBParams();


    //    //Output 변수
    //    private Packet_Result.Result m_Result = Packet_Result.Result.Success;
    //    private ProcedureResult m_Data = new ProcedureResult();
    //    private CBlockUser m_BlockUser = new CBlockUser();

    //    public CQuerySystemDeleteBlockUser(string targetID)
    //    {
    //        m_TargetDeviceID = targetID;
    //        m_Params.SetParams(targetID);
    //    }

    //    public override void Run(SMsSql agent)
    //    {
    //        try
    //        {
    //            // DB연결 및 SP명 설정
    //            SqlCommand sqlCmd = new SqlCommand();
    //            sqlCmd.Connection = agent.GetAgent;
    //            sqlCmd.CommandType = CommandType.StoredProcedure;
    //            sqlCmd.CommandText = "sp_system_delete_block_user";

    //            // 입출력인수 설정
    //            CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

    //            // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
    //            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
    //            sqlAdapter.SelectCommand = sqlCmd;
    //            sqlAdapter.Fill(m_Data.m_dataSet);

    //            if (!m_Data.IsSuccess())
    //            {
    //                m_Data.PrintErrorLog(sqlCmd.CommandText);
    //                m_Result = Packet_Result.Result.DBError;
    //                CLogger.Instance.Error(m_Data.GetErrorMessage());
    //            }
    //            else
    //            {
    //                //blockuser
    //                int table_index = 0;
    //                for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
    //                {
    //                    m_Data.GetData(table_index, "device_id", ref m_BlockUser.m_DeviceID, i);
    //                    m_Data.GetData(table_index, "cnt", ref m_BlockUser.m_Count, i);
    //                    m_Data.GetData(table_index, "exp_time", ref m_BlockUser.m_ExpTime, i);
    //                }
    //            }

    //            sqlAdapter.Dispose();
    //            sqlCmd.Dispose();
    //        }
    //        catch (Exception e)
    //        {
    //            m_Result = Packet_Result.Result.DBError;
    //            CLogger.Instance.Error(e.ToString());
    //        }
    //    }

    //    public override void Complete()
    //    {
    //        if (m_Result != Packet_Result.Result.Success)
    //            return;

    //        CSystemManager.Instance.UpsertBlockUser(m_BlockUser);
    //    }
    //}

    //public class CQuerySystemUpdateBlockUser : IMsSqlQuery
    //{
    //    struct DBParams
    //    {
    //        public string device_id;
    //        public int cnt;
    //        public long block_exp_time;

    //        public void SetParams(string targetid, int count, long exp_time)
    //        {
    //            device_id = targetid;
    //            cnt = count;
    //            block_exp_time = exp_time;
    //        }
    //    }

    //    //input
    //    private CBlockUser m_BlockUser = new CBlockUser();
    //    private DBParams m_Params = new DBParams();


    //    //Output 변수
    //    private Packet_Result.Result m_Result = Packet_Result.Result.Success;
    //    private ProcedureResult m_Data = new ProcedureResult();
        
    //    public CQuerySystemUpdateBlockUser(CBlockUser blockUser)
    //    {
    //        m_BlockUser = SCopy<CBlockUser>.DeepCopy(blockUser);
    //        m_Params.SetParams(m_BlockUser.m_DeviceID, m_BlockUser.m_Count, m_BlockUser.m_ExpTime);
    //    }

    //    public override void Run(SMsSql agent)
    //    {
    //        try
    //        {
    //            // DB연결 및 SP명 설정
    //            SqlCommand sqlCmd = new SqlCommand();
    //            sqlCmd.Connection = agent.GetAgent;
    //            sqlCmd.CommandType = CommandType.StoredProcedure;
    //            sqlCmd.CommandText = "sp_system_update_block_user";

    //            // 입출력인수 설정
    //            CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

    //            // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
    //            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
    //            sqlAdapter.SelectCommand = sqlCmd;
    //            sqlAdapter.Fill(m_Data.m_dataSet);

    //            if (!m_Data.IsSuccess())
    //            {
    //                m_Data.PrintErrorLog(sqlCmd.CommandText);
    //                m_Result = Packet_Result.Result.DBError;
    //                CLogger.Instance.Error(m_Data.GetErrorMessage());
    //            }
                
    //            sqlAdapter.Dispose();
    //            sqlCmd.Dispose();
    //        }
    //        catch (Exception e)
    //        {
    //            m_Result = Packet_Result.Result.DBError;
    //            CLogger.Instance.Error(e.ToString());
    //        }
    //    }

    //    public override void Complete()
    //    {
    //        if (m_Result != Packet_Result.Result.Success)
    //            return;

    //        CSystemManager.Instance.UpsertBlockUser(m_BlockUser);
    //    }
    //}

    //public class CQuerySystemUpdateWhiteUser : IMsSqlQuery
    //{
    //    struct DBParams
    //    {
    //        public string device_id;
            
    //        public void SetParams(string targetid)
    //        {
    //            device_id = targetid;
    //        }
    //    }

    //    //input
    //    private DBParams m_Params = new DBParams();


    //    //Output 변수
    //    private Packet_Result.Result m_Result = Packet_Result.Result.Success;
    //    private ProcedureResult m_Data = new ProcedureResult();
    //    private CWhiteUser m_WhiteUser = new CWhiteUser();

    //    public CQuerySystemUpdateWhiteUser(string targetID)
    //    {
    //        m_Params.SetParams(targetID);
    //    }

    //    public override void Run(SMsSql agent)
    //    {
    //        try
    //        {
    //            // DB연결 및 SP명 설정
    //            SqlCommand sqlCmd = new SqlCommand();
    //            sqlCmd.Connection = agent.GetAgent;
    //            sqlCmd.CommandType = CommandType.StoredProcedure;
    //            sqlCmd.CommandText = "sp_system_update_white_user";

    //            // 입출력인수 설정
    //            CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

    //            // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
    //            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
    //            sqlAdapter.SelectCommand = sqlCmd;
    //            sqlAdapter.Fill(m_Data.m_dataSet);

    //            if (!m_Data.IsSuccess())
    //            {
    //                m_Data.PrintErrorLog(sqlCmd.CommandText);
    //                m_Result = Packet_Result.Result.DBError;
    //                CLogger.Instance.Error(m_Data.GetErrorMessage());
    //            }
    //            else
    //            {
    //                m_Data.GetData(0, "device_id", ref m_WhiteUser.m_DeviceID);
    //                m_Data.GetData(0, "create_time", ref m_WhiteUser.m_CreateTime);
    //            }


    //            sqlAdapter.Dispose();
    //            sqlCmd.Dispose();
    //        }
    //        catch (Exception e)
    //        {
    //            m_Result = Packet_Result.Result.DBError;
    //            CLogger.Instance.Error(e.ToString());
    //        }
    //    }

    //    public override void Complete()
    //    {
    //        if (m_Result != Packet_Result.Result.Success)
    //            return;

    //        CSystemManager.Instance.UpsertWhiteUser(m_WhiteUser);
    //    }
    //}

    //public class CQueryServerWithCharacterList : IMsSqlQuery
    //{
    //    private Int64 m_SessionKey;
    //    private string m_UID;
    //    private _AuthInfo m_AuthInfo;

    //    //Output 변수
    //    private Packet_Result.Result m_Result;
    //    private ProcedureResult m_Data = new ProcedureResult();
    //    private List<_SimpleCharacterInfo> m_CharacterList = new List<_SimpleCharacterInfo>();
    //    private eUserRole m_UserRole = eUserRole.User;

    //    public CQueryServerWithCharacterList(Int64 sessionKey, string uid, _AuthInfo authInfo)
    //    {
    //        m_SessionKey = sessionKey;
    //        m_UID = uid;
    //        m_AuthInfo = authInfo;
    //    }

    //    struct DBParams
    //    {
    //        public string uid;
    //    }

    //    public override void Run(SMsSql agent)
    //    {
    //        try
    //        {
    //            // DB연결 및 SP명 설정
    //            SqlCommand sqlCmd = new SqlCommand();
    //            sqlCmd.Connection = agent.GetAgent;
    //            sqlCmd.CommandType = CommandType.StoredProcedure;
    //            sqlCmd.CommandText = "sp_account_character_load";

    //            // 입출력인수 설정
    //            CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
    //            {
    //                uid = m_UID

    //            });


    //            // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
    //            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
    //            sqlAdapter.SelectCommand = sqlCmd;
    //            sqlAdapter.Fill(m_Data.m_dataSet);

    //            if (!m_Data.IsSuccess())
    //            {
    //                m_Data.PrintErrorLog(sqlCmd.CommandText);
    //                m_Result = Packet_Result.Result.DBError;
    //            }
    //            else
    //            {
    //                //character list
    //                int table_index = 0;
    //                for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
    //                {
    //                    var characterInfo = new _SimpleCharacterInfo();

    //                    m_Data.GetData(table_index, "server_id", ref characterInfo.m_ServerID, i);
    //                    m_Data.GetData(table_index, "account_id", ref characterInfo.m_AccountID, i);

    //                    m_CharacterList.Add(characterInfo);
    //                }

    //                //user role
    //                ++table_index;
    //                m_Data.GetData_Enum(table_index, "roles", ref m_UserRole);
    //            }

    //            sqlAdapter.Dispose();
    //            sqlCmd.Dispose();
    //        }
    //        catch (Exception e)
    //        {
    //            m_Result = Packet_Result.Result.DBError;
    //            CLogger.Instance.Error(e.ToString());
    //        }
    //    }
    //    public override void Complete()
    //    {
    //        if (!CNetManager.Instance.IsAliveSession(m_SessionKey))
    //            return;

    //        //bool bShouldCharacterLoad = (m_CharacterList.Count > 0);
    //        //if (bShouldCharacterLoad)
    //        //{
    //        //    CDBManager.Instance.QueryLoadCharacterList(CServerDefine.GetDBGUID(m_UID), m_SessionKey, m_UID, m_AuthInfo, m_CharacterList, m_UserRole);
    //        //}
    //        //else
    //        //{
    //        //    CUserManager.Instance.AfterQueryServerWithCharacterList(m_SessionKey, m_UID, m_AuthInfo, m_CharacterList, m_UserRole);
    //        //}
    //    }
    //}

    //public class CQueryAuthAccount : IMsSqlQuery
    //{
    //    //Input 변수
    //    protected long m_SessionKey;
    //    protected string m_UID;
    //    protected int m_ServerID = -1;
    //    protected _AuthInfo m_AuthInfo;

    //    //Output 변수
    //    protected Packet_Result.Result m_Result;
    //    protected ProcedureResult m_Data = new ProcedureResult();
    //    protected long m_AccountID = CDefine.INVALID_ACCOUNT_ID;
    //    struct DBParams
    //    {
    //        public string uid;
    //        public int server_id;
    //        public uint auth_type;
    //    }

    //    public CQueryAuthAccount(long sessionKey, string uid, int serverID, _AuthInfo authInfo)
    //    {
    //        m_SessionKey = sessionKey;
    //        m_UID = uid;
    //        m_ServerID = serverID;
    //        m_AuthInfo = authInfo;
    //    }

    //    public override void Run(SMsSql agent)
    //    {
    //        try
    //        {
    //            // DB연결 및 SP명 설정
    //            SqlCommand sqlCmd = new SqlCommand();
    //            sqlCmd.Connection = agent.GetAgent;
    //            sqlCmd.CommandType = CommandType.StoredProcedure;
    //            sqlCmd.CommandText = "sp_account_auth";

    //            // 입출력인수 설정
    //            CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
    //            {
    //                uid = m_UID,
    //                server_id = m_ServerID,
    //                auth_type = (uint)m_AuthInfo.m_AuthType
    //            });

    //            // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
    //            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
    //            sqlAdapter.SelectCommand = sqlCmd;
    //            sqlAdapter.Fill(m_Data.m_dataSet);

    //            if (!m_Data.IsSuccess())
    //            {
    //                m_Data.PrintErrorLog(sqlCmd.CommandText);
    //                m_Result = Packet_Result.Result.DBError;
    //            }
    //            else
    //            {
    //                m_Data.GetData(0, "account_id", ref m_AccountID);
    //            }

    //            sqlAdapter.Dispose();
    //            sqlCmd.Dispose();
    //        }
    //        catch (Exception e)
    //        {
    //            m_Result = Packet_Result.Result.DBError;
    //            CLogger.Instance.Error(e.ToString());
    //        }
    //    }
    //    public override void Complete()
    //    {
    //        if (m_Result != Packet_Result.Result.Success)
    //        {
    //            //CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
    //            //return;
    //        }

    //        CUser user = CUserManager.Instance.FindUser(m_SessionKey);
    //        if (user == null || true == user.IsLogin())
    //        {
    //            CNetManager.Instance.P2C_ReportKick(m_SessionKey, Packet_Result.Result.AuthError);
    //            return;
    //        }

    //        if (CBlockManager.Instance.FindBlockUser(m_UID) is _BlockUser foundBlockData)
    //        {
    //            var result = (foundBlockData.m_State == eAccountState.Blocked) ? Packet_Result.Result.BlockedUser : Packet_Result.Result.Account_PendingDelete;

    //            DateTime delDate = foundBlockData.m_DeleteDate;
    //            bool isGDPR = foundBlockData.m_IsGDPR;
    //            int remainTime = 0;

    //            if (foundBlockData.m_State == eAccountState.PendingDelete)
    //            {
    //                remainTime = SDateManager.Instance.ToTimeLeft(foundBlockData.m_PendingExpTime);
    //            }
    //            else if (foundBlockData.m_State == eAccountState.Blocked)
    //            {
    //                remainTime = SDateManager.Instance.ToTimeLeft(foundBlockData.m_EndTime);
    //            }

    //            CNetManager.Instance.P2C_ResultLogin(m_SessionKey, result, m_AccountID, delDate, remainTime, isGDPR);

    //            if (foundBlockData.m_State == eAccountState.Blocked)
    //            {
    //                CNetManager.Instance.P2M_ReportUserLeave(user.DeviceID);
    //                //CNetManager.Instance.P2C_ReportKick(m_SessionKey, Packet_Result.Result.BlockedUser);
    //            }
    //        }
    //        else
    //        {
    //            CDBManager.Instance.QueryLoadPlayerData(user.DBGUID, user.SessionKey, m_AccountID, m_AuthInfo);
    //        }
    //    }
    //}

    //public class CQueryAccountCreate : CQueryAuthAccount
    //{
    //    private string m_NickName = string.Empty;
    //    public CQueryAccountCreate(long sessionKey, string uid, int serverID, _AuthInfo authInfo, string nickname)
    //        : base(sessionKey, uid, serverID, authInfo)
    //    {
    //        m_NickName = nickname;
    //    }

    //    public override void Complete()
    //    {
    //        //if (m_Result != Packet_Result.Result.Success)
    //        //{
    //        //    CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
    //        //    return;
    //        //}

    //        //CUser user = CUserManager.Instance.FindUser(m_SessionKey);
    //        //if (user == null || true == user.IsLogin())
    //        //{
    //        //    CNetManager.Instance.P2C_ReportKick(m_SessionKey, Packet_Result.Result.AuthError);
    //        //    return;
    //        //}

    //        //_PlayerData playerdata = new _PlayerData();
    //        //CRewardDBMerge db_merge = new CRewardDBMerge();
    //        //_EquipPresetData basePreset = CItemEquipPresetTable.Instance.CopyDefault(eEquipPreset.Base, 0);

    //        //CUserManager.Instance.CreateUserDefault(m_SessionKey, ref playerdata, ref db_merge, ref basePreset);
    //        //playerdata.m_ServerID = m_ServerID;
    //        //playerdata.m_DeviceID = m_UID;
    //        //playerdata.m_Name = m_NickName;
    //        //db_merge.DBMerge();

    //        //CDBManager.Instance.QueryCharacterCreate(user.DBGUID, m_SessionKey, m_AccountID, m_UID, playerdata, db_merge, basePreset, m_AuthInfo.m_CountryCode);
    //    }
    //}
}