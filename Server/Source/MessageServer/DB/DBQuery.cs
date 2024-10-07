using System;
using System.Collections.Generic;
using SCommon;
using SDB;
using Global;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using Packet_P2M;
using System.Web;

namespace MessageServer
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

    public class CQuerySystemDataLoad : IMsSqlQuery
    {
        //input

        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_BlockUser> m_BlockList = new List<_BlockUser>();
        private List<CWhiteUser> m_WhiteList = new List<CWhiteUser>();
        private List<_NoticeData> m_NoticeList = new List<_NoticeData>();
        private bool m_Open = true;

        public CQuerySystemDataLoad()
        {
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, this);

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
                    //blockuser
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _BlockUser data = new _BlockUser();
                        m_Data.GetData(table_index, "device_id", ref data.DeviceID, i);
                        m_Data.GetData(table_index, "cnt", ref data.Count, i);
                        m_Data.GetData(table_index, "exp_time", ref data.ExpTime, i);

                        m_BlockList.Add(data);
                    }

                    //white user
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        CWhiteUser data = new CWhiteUser();
                        m_Data.GetData(table_index, "device_id", ref data.m_DeviceID, i);
                        m_Data.GetData(table_index, "create_time", ref data.m_CreateTime, i);

                        m_WhiteList.Add(data);
                    }

                    //notice
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _NoticeData data = new _NoticeData();
                        m_Data.GetData(table_index, "notice_id", ref data.m_ID, i);
                        m_Data.GetData(table_index, "msg", ref data.m_Msg, i);
                        m_Data.GetData(table_index, "begin_time", ref data.m_StartDate, i);
                        m_Data.GetData(table_index, "expire_time", ref data.m_EndDate, i);
                        m_Data.GetData(table_index, "loop", ref data.m_Loop, i);
                        m_Data.GetData(table_index, "term", ref data.m_Term, i);

                        m_NoticeList.Add(data);
                    }

                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "type", ref m_Open, i);
                    }

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
            if (m_Result != Packet_Result.Result.Success)
                return;

            CSystemManager.Instance.LoadSystemData(m_BlockList, m_WhiteList, m_NoticeList, m_Open);
        }
    }

    public class CQuerySystemDeleteWhiteUser : IMsSqlQuery
    {
        struct DBParams
        {
            public string device_id;

            public void SetParams(string targetid)
            {
                device_id = targetid;
            }
        }

        //input
        private string m_TargetDeviceID = string.Empty;
        private DBParams m_Params = new DBParams();

        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private CWhiteUser m_Target = new CWhiteUser();

        public CQuerySystemDeleteWhiteUser(string targetID)
        {
            m_TargetDeviceID = targetID;
            m_Params.SetParams(targetID);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_delete_white_user";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
            if (m_Result != Packet_Result.Result.Success)
                return;

            CSystemManager.Instance.RemoveWhiteUser(m_TargetDeviceID);
        }
    }

    public class CQuerySystemDeleteBlockUser : IMsSqlQuery
    {
        struct DBParams
        {
            public string device_id;

            public void SetParams(string targetid)
            {
                device_id = targetid;
            }
        }

        //input
        private string m_TargetDeviceID = string.Empty;
        private DBParams m_Params = new DBParams();


        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private _BlockUser m_BlockUser = new _BlockUser();

        public CQuerySystemDeleteBlockUser(string targetID)
        {
            m_TargetDeviceID = targetID;
            m_Params.SetParams(targetID);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_delete_block_user";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //blockuser
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "device_id", ref m_BlockUser.DeviceID, i);
                        m_Data.GetData(table_index, "cnt", ref m_BlockUser.Count, i);
                        m_Data.GetData(table_index, "exp_time", ref m_BlockUser.ExpTime, i);
                    }
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
            if (m_Result != Packet_Result.Result.Success)
                return;

            CSystemManager.Instance.UpsertBlockUser(m_BlockUser);
        }
    }

    public class CQuerySystemUpdateBlockUser : IMsSqlQuery
    {
        struct DBParams
        {
            public string device_id;
            public int cnt;
            public long block_exp_time;

            public void SetParams(string targetid, int count, long exp_time)
            {
                device_id = targetid;
                cnt = count;
                block_exp_time = exp_time;
            }
        }

        //input
        private _BlockUser m_BlockUser = new _BlockUser();
        private DBParams m_Params = new DBParams();


        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQuerySystemUpdateBlockUser(_BlockUser blockUser)
        {
            m_BlockUser = SCopy<_BlockUser>.DeepCopy(blockUser);
            m_Params.SetParams(m_BlockUser.DeviceID, m_BlockUser.Count, m_BlockUser.ExpTime);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_update_block_user";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
            if (m_Result != Packet_Result.Result.Success)
                return;

            CSystemManager.Instance.UpsertBlockUser(m_BlockUser);
        }
    }

    public class CQuerySystemUpdateWhiteUser : IMsSqlQuery
    {
        struct DBParams
        {
            public string device_id;

            public void SetParams(string targetid)
            {
                device_id = targetid;
            }
        }

        //input
        private DBParams m_Params = new DBParams();


        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private CWhiteUser m_WhiteUser = new CWhiteUser();

        public CQuerySystemUpdateWhiteUser(string targetID)
        {
            m_Params.SetParams(targetID);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_update_white_user";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    m_Data.GetData(0, "device_id", ref m_WhiteUser.m_DeviceID);
                    m_Data.GetData(0, "create_time", ref m_WhiteUser.m_CreateTime);
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
            if (m_Result != Packet_Result.Result.Success)
                return;

            CSystemManager.Instance.UpsertWhiteUser(m_WhiteUser);
        }
    }

    public class CQuerySystemLoadRankMain : IMsSqlQuery
    {
        struct DBParams
        {
            public ushort db_stage_type;
            public int db_max_rank_count;
            public int db_min_value;

            public void SetParams(ushort stagetype, int maxRankcount,int min_val)
            {
                db_stage_type = stagetype;
                db_max_rank_count = maxRankcount;
                db_min_value = min_val;
            }
        }

        //input
        private DBParams m_Params = new DBParams();

        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private CDefine.ERankType m_RankType = CDefine.ERankType.Max;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_RankData> m_RankList = new List<_RankData>();

        public CQuerySystemLoadRankMain(CDefine.ERankType type, CDefine.eStageType stagetype, int maxRankcount, int min_val)
        {
            m_RankType = type;
            m_Params.SetParams((ushort)stagetype, maxRankcount, min_val);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_load_rank_main";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _RankData data = new _RankData();
                        m_Data.GetData(table_index, "uid", ref data.m_UID, i);
                        m_Data.GetData(table_index, "device_id", ref data.m_DeviceID, i);
                        m_Data.GetData(table_index, "name", ref data.m_Name, i);
                        m_Data.GetData(table_index, "max_tid", ref data.m_StageTID, i);
                        m_Data.GetData(table_index, "level", ref data.m_Level, i);
                        m_Data.GetData(table_index, "profile_id", ref data.m_ProfileID, i);

                        data.m_RankType = m_RankType;

                        m_RankList.Add(data);
                    }
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
            if (m_Result != Packet_Result.Result.Success)
                return;

            CRankingManager.Instance.AfterQuery_LoadRank(CDefine.ERankType.Rank_MainStage, m_RankList);
        }
    }

    public class CQuerySystemLoadRankPvp : IMsSqlQuery
    {
        struct DBParams
        {
            public string db_coin_type;
            public int db_max_rank_count;
            
            public void SetParams(string coin_type, int maxRankcount)
            {
                db_coin_type = coin_type;
                db_max_rank_count = maxRankcount;
            }
        }

        //input
        private DBParams m_Params = new DBParams();

        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private CDefine.ERankType m_RankType = CDefine.ERankType.Max;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_RankData> m_RankList = new List<_RankData>();

        public CQuerySystemLoadRankPvp(CDefine.ERankType type, CDefine.CoinType coin_type, int maxRankcount)
        {
            m_RankType = type;
            m_Params.SetParams(coin_type.ToString(), maxRankcount);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_load_rank_pvp";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _RankData data = new _RankData();
                        m_Data.GetData(table_index, "uid", ref data.m_UID, i);
                        m_Data.GetData(table_index, "device_id", ref data.m_DeviceID, i);
                        m_Data.GetData(table_index, "name", ref data.m_Name, i);
                        m_Data.GetData(table_index, "value", ref data.m_StageTID, i);
                        m_Data.GetData(table_index, "level", ref data.m_Level, i);
                        m_Data.GetData(table_index, "profile_id", ref data.m_ProfileID, i);
                        data.m_RankType = m_RankType;

                        m_RankList.Add(data);
                    }
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
            if (m_Result != Packet_Result.Result.Success)
                return;

            //CRankingManager.Instance.AfterQuery_LoadRank(m_RankType, m_RankList);
        }
    }

    public class CQuerySystemLoadSchedule : IMsSqlQuery
    {
        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private CDefine.ERankType m_RankType = CDefine.ERankType.Max;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<CSchedulerEx_Base> m_ScheduleList = new List<CSchedulerEx_Base>();

        public CQuerySystemLoadSchedule()
        {
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_load_schedule";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, this);

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
                    //hottime
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        CSchedulerEx_Base data = new CSchedulerEx_Base();
                        m_Data.GetData(table_index, "uid", ref data.m_UID, i);
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type, i);
                        data.m_Type = (eSchedule)type;
                        if (data.m_Type == eSchedule.Max)
                            continue;

                        m_Data.GetData(table_index, "param", ref data.m_Param, i);
                        m_Data.GetData(table_index, "val", ref data.m_Val, i);
                        m_Data.GetData(table_index, "start_date", ref data.m_StartDate, i);
                        m_Data.GetData(table_index, "end_date", ref data.m_EndDate, i);
                        
                        string dayOfweek = string.Empty;
                        m_Data.GetData(table_index, "day_week", ref dayOfweek, i);
                        if (!SJson.IsValidJson(dayOfweek))
                            continue;
                        
                        data.m_DayofWeek = SJson.JsonToObject<List<DayOfWeek>>(dayOfweek);

                        string start_time = string.Empty;
                        m_Data.GetData(table_index, "start_time", ref start_time, i);
                        if (string.IsNullOrEmpty(start_time))
                            continue;

                        TimeSpan ts_start;
                        if (!TimeSpan.TryParse(start_time, out ts_start))
                            continue;

                        data.m_StartTime = ts_start;

                        string end_time = string.Empty;
                        m_Data.GetData(table_index, "end_time", ref end_time, i);
                        if (string.IsNullOrEmpty(end_time))
                            continue;

                        TimeSpan ts_end;
                        if (!TimeSpan.TryParse(end_time, out ts_end))
                            continue;
                        data.m_EndTime = ts_end;

                        m_ScheduleList.Add(data);
                    }
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
            if (m_Result != Packet_Result.Result.Success)
                return;

            CScheduleManager.Instance.AfterQueryScheduleLoad(m_ScheduleList);
        }
    }

    public class CQuerySystemInsertSchedue : IMsSqlQuery
    {
        struct DBParams
        {
            public long db_uid;
            public ushort db_type;
            public string db_param;
            public string db_val;
            public DateTime db_start_date;
            public DateTime db_end_date;
            public string db_day_week;
            public string db_start_time;
            public string db_end_time;

            public void SetParams(long uid, ushort type, string param, string val, DateTime start_date, DateTime end_date, string day_of_week, string start_time, string end_time)
            {
                db_uid = uid;
                db_type = type;
                db_param = param;
                db_val = val;
                db_start_date = start_date;
                db_end_date = end_date;
                db_day_week = day_of_week;
                db_start_time = start_time;
                db_end_time = end_time;
            }
        }

        //Input
        private DBParams m_Params = new DBParams();
        private long m_UID = 0;
        private eSchedule m_Type = eSchedule.Max;
        private string m_Param = string.Empty;
        private string m_Val = string.Empty;
        private List<DayOfWeek> m_DayofWeek = new List<DayOfWeek>();
        private DateTime m_StartDate;
        private DateTime m_EndDate;
        private TimeSpan m_StartTime;
        private TimeSpan m_EndTime;


        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private CDefine.ERankType m_RankType = CDefine.ERankType.Max;
        private ProcedureResult m_Data = new ProcedureResult();
        
        public CQuerySystemInsertSchedue(long uid, eSchedule type, string param, string val, DateTime start_date, DateTime end_date, List<DayOfWeek> dayofweek, TimeSpan start_time,
            TimeSpan end_time)
        {
            m_UID = uid;
            m_Type = type;
            m_Param = param;
            m_Val = val;
            m_StartDate = start_date;
            m_EndDate = end_date;
            m_DayofWeek = dayofweek;
            m_StartTime = start_time;
            m_EndTime = end_time;

            m_Params.SetParams(uid, (ushort)type, param, val, start_date, end_date, SCommon.SJson.ObjectToJson(dayofweek), start_time.ToString(), end_time.ToString());
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_load_schedule";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, this);

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
            if (m_Result != Packet_Result.Result.Success)
                return;

            CScheduleManager.Instance.AfterQeuryInsertScheduler(m_UID, m_Type, m_Param, m_Val, m_StartDate, m_EndDate, m_DayofWeek, m_StartTime, m_EndTime);
        }
    }

    public class CQuerySystemUpsertPost : IMsSqlQuery
    {
        struct DBParams
        {
            public long post_id;
            public ushort type;
            public string title;
            public string msg;
            public long begin_time;
            public long expire_time;
            public string reward;

            public void SetParams(long id, ushort post_type, string post_title, string post_msg, long begin, long end, string post_reward)
            {
                post_id = id;
                type = post_type;
                title = post_title;
                msg = post_msg;
                begin_time = begin;
                expire_time = end;
                reward = post_reward;
            }
        }

        //input
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private _PostData m_PostData = new _PostData();

        public CQuerySystemUpsertPost(long id, CDefine.PostType type, string title, string msg, long begin, long end, string reward)
        {
            m_Params.SetParams(id, (ushort)type, title, msg, begin, end, reward);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_post_upsert";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //system post
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "id", ref m_PostData.ID, i);
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type, i);
                        m_PostData.Type = (CDefine.PostType)type;

                        m_Data.GetData(table_index, "title", ref m_PostData.Title, i);
                        m_Data.GetData(table_index, "msg", ref m_PostData.Msg, i);
                        m_Data.GetData(table_index, "begin_time", ref m_PostData.beginTime, i);
                        m_Data.GetData(table_index, "expire_time", ref m_PostData.expireTime, i);

                        string rewardString = string.Empty;
                        m_Data.GetData(table_index, "reward", ref rewardString, i);

                        if (SJson.IsValidJson(rewardString) && !string.IsNullOrEmpty(rewardString))
                            m_PostData.Rewards = SJson.JsonToObject<List<_AssetData>>(rewardString);
                    }
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
            foreach (var iter in CServerManager.Instance.Servers)
            {
                var server = iter.Value;
                CNetManager.Instance.M2P_ReportInsertSystemPost(server.m_ServerSessionKey, m_PostData);
            }
        }
    }




    //====================== OPERTOOL =======================
    public class CQueryyToolCharacterInfoLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string deviceid;
            public string name;

            public void SetParams(long user_id, string user_device_id, string user_name)
            {
                uid = user_id;
                deviceid = user_device_id;
                name = user_name;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_Params = new DBParams();


        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private _UserData m_UserData = new _UserData();
        private List<_GachaData> m_Gachas = new List<_GachaData>();
        private List<_AssetData> m_Coins = new List<_AssetData>();
        private List<_StageData> m_Stages = new List<_StageData>();
        private List<_ReceiptData> m_Receipts = new List<_ReceiptData>();


        public CQueryyToolCharacterInfoLoad(long sessionKey, long user_id, string user_device_id, string user_name)
        {
            m_SessionKey = sessionKey;
            m_Params.SetParams(user_id, user_device_id, user_name);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_character_info_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    if (m_Data.GetErrorNumber() == 1)
                    {
                        m_Result = Packet_Result.Result.NotFoundData;
                    }
                    else
                    {
                        m_Data.PrintErrorLog(sqlCmd.CommandText);
                        m_Result = Packet_Result.Result.DBError;
                        CLogger.Instance.Error(m_Data.GetErrorMessage());
                    }
                }
                else
                {
                    //user
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "uid", ref m_UserData.m_UID, i);
                        m_Data.GetData(table_index, "name", ref m_UserData.m_Name, i);
                        m_Data.GetData(table_index, "device_id", ref m_UserData.m_DeviceID, i);
                        m_Data.GetData(table_index, "level", ref m_UserData.m_Level, i);
                        m_Data.GetData(table_index, "exp", ref m_UserData.m_Exp, i);
                        m_Data.GetData(table_index, "level_point", ref m_UserData.m_LevelPoint, i);
                        m_Data.GetData(table_index, "login_time", ref m_UserData.m_LoginTime, i);
                        m_Data.GetData(table_index, "logout_time", ref m_UserData.m_LogoutTime, i);
                    }

                    //gacha 
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        var data = new _GachaData();
                        m_Data.GetData(table_index, "id", ref data.m_GroupID, i);
                        m_Data.GetData(table_index, "lv", ref data.m_Level, i);
                        m_Data.GetData(table_index, "exp", ref data.m_Exp, i);
                        m_Data.GetData(table_index, "rewarded", ref data.m_Rewarded, i);

                        m_Gachas.Add(data);
                    }

                    //coin
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        var data = new _AssetData();
                        data.Type = CDefine.AssetType.Coin;
                        m_Data.GetData(table_index, "type", ref data.TableID, i);
                        m_Data.GetData(table_index, "value", ref data.Count, i);

                        m_Coins.Add(data);
                    }

                    //stage
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _StageData stageData = new _StageData();
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type, i);
                        stageData.type = (CDefine.eStageType)type;

                        m_Data.GetData(table_index, "cur_tid", ref stageData.curTID, i);
                        m_Data.GetData(table_index, "max_tid", ref stageData.maxTID, i);
                        m_Data.GetData(table_index, "total_cnt", ref stageData.totalCnt, i);

                        m_Stages.Add(stageData);
                    }

                    //receipt
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        var data = new _ReceiptData();
                        m_Data.GetData(table_index, "transaction_id", ref data.m_TransactionID, i);
                        m_Data.GetData_Enum<CDefine.eStoreType>(table_index, "store_type", ref data.m_StoreType, i);
                        m_Data.GetData(table_index, "product_id", ref data.m_ProductID, i);
                        m_Data.GetData(table_index, "price", ref data.m_Price, i);
                        m_Data.GetData(table_index, "mail_guid", ref data.m_PostID, i);
                        m_Data.GetData(table_index, "dw_update_time", ref data.m_UpateTime, i);

                        m_Receipts.Add(data);
                    }
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
            bool isblock = CSystemManager.Instance.IsBlockUser(m_UserData.m_DeviceID);
            bool iswhite = CSystemManager.Instance.IsWhiteUser(m_UserData.m_DeviceID);
            CNetManager.Instance.M2O_ResultSearchUser(m_SessionKey, m_UserData, m_Gachas, m_Coins, m_Stages, m_Receipts, isblock, iswhite, m_Result);
        }
    }

    public class CQueryyToolSystemPostLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long begin;
            public long end;

            public void SetParams(long begintime, long endtime)
            {
                begin = begintime;
                end = endtime;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_PostData> m_SystemPosts = new List<_PostData>();


        public CQueryyToolSystemPostLoad(long sessionKey, long beginTime, long endTime)
        {
            m_SessionKey = sessionKey;
            m_Params.SetParams(beginTime, endTime);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_system_post_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //system post
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _PostData data = new _PostData();
                        m_Data.GetData(table_index, "id", ref data.ID, i);
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type, i);
                        data.Type = (CDefine.PostType)type;

                        m_Data.GetData(table_index, "title", ref data.Title, i);
                        m_Data.GetData(table_index, "msg", ref data.Msg, i);
                        m_Data.GetData(table_index, "begin_time", ref data.beginTime, i);
                        m_Data.GetData(table_index, "expire_time", ref data.expireTime, i);

                        string rewardString = string.Empty;
                        m_Data.GetData(table_index, "reward", ref rewardString, i);

                        if (SJson.IsValidJson(rewardString) && !string.IsNullOrEmpty(rewardString))
                            data.Rewards = SJson.JsonToObject<List<_AssetData>>(rewardString);

                        m_SystemPosts.Add(data);
                    }
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
            CNetManager.Instance.M2O_ResultSystemPostLoad(m_SessionKey, m_SystemPosts, m_Result);
        }
    }

    public class CQueryToolSystemUpsertPost : IMsSqlQuery
    {
        struct DBParams
        {
            public long post_id;
            public ushort type;
            public string title;
            public string msg;
            public long begin_time;
            public long expire_time;
            public string reward;

            public void SetParams(long id, ushort post_type, string post_title, string post_msg, long begin, long end, string post_reward)
            {
                post_id = id;
                type = post_type;
                title = post_title;
                msg = post_msg;
                begin_time = begin;
                expire_time = end;
                reward = post_reward;
            }
        }

        //input
        private long m_SessionKey;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private _PostData m_PostData = new _PostData();

        public CQueryToolSystemUpsertPost(long sessionKey, long id, CDefine.PostType type, string title, string msg, long begin, long end, string reward)
        {
            m_SessionKey = sessionKey;
            m_Params.SetParams(id, (ushort)type, title, msg, begin, end, reward);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_system_post_upsert";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //system post
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "id", ref m_PostData.ID, i);
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type, i);
                        m_PostData.Type = (CDefine.PostType)type;

                        m_Data.GetData(table_index, "title", ref m_PostData.Title, i);
                        m_Data.GetData(table_index, "msg", ref m_PostData.Msg, i);
                        m_Data.GetData(table_index, "begin_time", ref m_PostData.beginTime, i);
                        m_Data.GetData(table_index, "expire_time", ref m_PostData.expireTime, i);

                        string rewardString = string.Empty;
                        m_Data.GetData(table_index, "reward", ref rewardString, i);

                        if (SJson.IsValidJson(rewardString) && !string.IsNullOrEmpty(rewardString))
                            m_PostData.Rewards = SJson.JsonToObject<List<_AssetData>>(rewardString);
                    }
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
            CNetManager.Instance.M2O_ResultSystemPostSend(m_SessionKey, m_PostData, m_Result);
        }
    }

    public class CQueryToolSystemNoticeLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public DateTime start;
            public DateTime end;

            public void SetParams(DateTime start_date, DateTime end_date)
            {
                start = start_date;
                end = end_date;
            }
        }

        //input
        private long m_SessionKey;
        private DBParams m_Params = new DBParams();

        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_NoticeData> m_NoticeList = new List<_NoticeData>();


        public CQueryToolSystemNoticeLoad(long sessionKey, DateTime start_date, DateTime end_date)
        {
            m_SessionKey = sessionKey;
            m_Params.SetParams(start_date, end_date);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_system_notice_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //notice
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _NoticeData data = new _NoticeData();
                        m_Data.GetData(table_index, "notice_id", ref data.m_ID, i);
                        m_Data.GetData(table_index, "msg", ref data.m_Msg, i);
                        m_Data.GetData(table_index, "begin_time", ref data.m_StartDate, i);
                        m_Data.GetData(table_index, "expire_time", ref data.m_EndDate, i);
                        m_Data.GetData(table_index, "loop", ref data.m_Loop, i);
                        m_Data.GetData(table_index, "term", ref data.m_Term, i);

                        m_NoticeList.Add(data);
                    }
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
            CNetManager.Instance.M2O_ResultNoticeLoad(m_SessionKey, m_NoticeList, m_Result);
        }
    }

    public class CQueryToolSystemNoticeUpdate : IMsSqlQuery
    {
        struct DBParams
        {
            public long notice_id;
            public string notice_msg;
            public DateTime notice_start;
            public DateTime notice_end;
            public int notice_loop;
            public int notice_term;

            public void SetParams(long id, string msg, DateTime start_date, DateTime end_date, int loop, int term)
            {
                notice_id = id;
                notice_msg = msg;
                notice_start = start_date;
                notice_end = end_date;
                notice_loop = loop;
                notice_term = term;
            }
        }

        //input
        private long m_SessionKey;
        private DBParams m_Params = new DBParams();

        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private _NoticeData m_Notice = new _NoticeData();


        public CQueryToolSystemNoticeUpdate(long sessionKey, _NoticeData notice)
        {
            m_SessionKey = sessionKey;
            m_Params.SetParams(notice.m_ID, notice.m_Msg, notice.m_StartDate, notice.m_EndDate, notice.m_Loop, notice.m_Term);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_system_notice_upsert";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //notice
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "notice_id", ref m_Notice.m_ID, i);
                        m_Data.GetData(table_index, "msg", ref m_Notice.m_Msg, i);
                        m_Data.GetData(table_index, "begin_time", ref m_Notice.m_StartDate, i);
                        m_Data.GetData(table_index, "expire_time", ref m_Notice.m_EndDate, i);
                        m_Data.GetData(table_index, "loop", ref m_Notice.m_Loop, i);
                        m_Data.GetData(table_index, "term", ref m_Notice.m_Term, i);
                    }
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
            CSystemManager.Instance.PushNotice(m_Notice);
            CNetManager.Instance.M2O_ResultNoticeUpdate(m_SessionKey, m_Notice, m_Result);
        }
    }


    public class CQueryToolSystemNoticeErase : IMsSqlQuery
    {
        struct DBParams
        {
            public long notice_id;

            public void SetParams(long id)
            {
                notice_id = id;
            }
        }

        //input
        private long m_SessionKey;
        private DBParams m_Params = new DBParams();
        

        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private long m_NoticeID = -1;

        public CQueryToolSystemNoticeErase(long sessionKey, long noticeId)
        {
            m_SessionKey = sessionKey;
            m_NoticeID = noticeId;
            m_Params.SetParams(noticeId);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_system_notice_remove";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
            CSystemManager.Instance.EraseNotice(m_NoticeID);
            CNetManager.Instance.M2O_ResultNoticeErase(m_SessionKey, m_Result);
        }
    }

    public class CQueryToolUserPostLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long user_id;
            public long start_time;
            public long end_time;

            public void SetParams(long id, long start, long end)
            {
                user_id = id;
                start_time = start;
                end_time = end;
            }
        }

        //input
        private long m_SessionKey;
        private DBParams m_Params = new DBParams();


        //Output 변수
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_PostData> m_PostDatas = new List<_PostData>();

        public CQueryToolUserPostLoad(long sessionKey, long user_uid, long start, long end)
        {
            m_SessionKey = sessionKey;
            m_Params.SetParams(user_uid, start, end);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_character_post_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _PostData post = new _PostData();
                        m_Data.GetData(table_index, "id", ref post.ID, i);
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type, i);
                        post.Type = (CDefine.PostType)type;
                        m_Data.GetData(table_index, "title", ref post.Title, i);
                        m_Data.GetData(table_index, "msg", ref post.Msg, i);
                        m_Data.GetData(table_index, "is_read", ref post.IsRead, i);
                        m_Data.GetData(table_index, "is_reward", ref post.IsRewarded, i);
                        m_Data.GetData(table_index, "begin_time", ref post.beginTime, i);
                        m_Data.GetData(table_index, "expire_time", ref post.expireTime, i);
                        string str_rewards = "";
                        m_Data.GetData(table_index, "reward", ref str_rewards, i);
                        if (!string.IsNullOrEmpty(str_rewards) && SJson.IsValidJson(str_rewards))
                            post.Rewards = SCommon.SJson.JsonToObject<List<_AssetData>>(str_rewards);

                        m_PostDatas.Add(post);
                    }
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
            CNetManager.Instance.M2O_ResultUserPostLoad(m_SessionKey, m_PostDatas, m_Result);
        }
    }

    public class CQueryToolUserPostInsert : IMsSqlQuery
    {
        struct DBParams
        {
            public long user_uid;
            public long post_id;
            public ushort type;
            public string title;
            public string msg;
            public long begin_time;
            public long expire_time;
            public string reward;

            public void SetParams(long userid, long id, ushort post_type, string post_title, string post_msg, long begin, long end, string post_reward)
            {
                user_uid = userid;
                post_id = id;
                type = post_type;
                title = post_title;
                msg = post_msg;
                begin_time = begin;
                expire_time = end;
                reward = post_reward;
            }
        }

        //input
        private long m_SessionKey;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private _PostData m_PostData = new _PostData();

        public CQueryToolUserPostInsert(long sessionKey, long user_uid, long id, CDefine.PostType type, string title, string msg, long begin, long end, string reward)
        {
            m_SessionKey = sessionKey;
            m_Params.SetParams(user_uid, id, (ushort)type, title, msg, begin, end, reward);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_character_post_insert";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

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
                    //system post
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "id", ref m_PostData.ID, i);
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type, i);
                        m_PostData.Type = (CDefine.PostType)type;

                        m_Data.GetData(table_index, "title", ref m_PostData.Title, i);
                        m_Data.GetData(table_index, "msg", ref m_PostData.Msg, i);
                        m_Data.GetData(table_index, "is_read", ref m_PostData.IsRead, i);
                        m_Data.GetData(table_index, "is_reward", ref m_PostData.IsRewarded, i);
                        m_Data.GetData(table_index, "begin_time", ref m_PostData.beginTime, i);
                        m_Data.GetData(table_index, "expire_time", ref m_PostData.expireTime, i);

                        string rewardString = string.Empty;
                        m_Data.GetData(table_index, "reward", ref rewardString, i);

                        if (SJson.IsValidJson(rewardString) && !string.IsNullOrEmpty(rewardString))
                            m_PostData.Rewards = SJson.JsonToObject<List<_AssetData>>(rewardString);
                    }
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
            CNetManager.Instance.M2O_ResultUserPostSend(m_SessionKey, m_PostData, m_Result);
        }
    }

    public class CQueryToolBlockUserLoad : IMsSqlQuery
    {
        //input
        private long m_SessionKey;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_BlockUser> m_BlockUsers = new List<_BlockUser>();

        public CQueryToolBlockUserLoad(long sessionKey)
        {
            m_SessionKey = sessionKey;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_block_user_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, this);

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
                    //system post
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _BlockUser data = new _BlockUser();
                        m_Data.GetData(table_index, "device_id", ref data.DeviceID, i);
                        m_Data.GetData(table_index, "cnt", ref data.Count, i);
                        m_Data.GetData(table_index, "exp_time", ref data.ExpTime, i);

                        m_BlockUsers.Add(data);
                    }
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
            CNetManager.Instance.M2O_ResultBlockUserLoad(m_SessionKey, m_BlockUsers, m_Result);
        }
    }

    public class CQueryToolBlockUserUpsert : IMsSqlQuery
    {
        struct DBParams
        {
            public string block_str_json;
           
            public void SetParams(string block_json)
            {
                block_str_json = block_json;
            }
        }

        //input
        private DBParams m_DBParams = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_BlockUser> m_BlockUsers = new List<_BlockUser>();

        public CQueryToolBlockUserUpsert(List<_BlockUser> blocks)
        {
            m_BlockUsers = blocks;
            m_DBParams.SetParams(SJson.ObjectToJson(blocks));
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_system_block_user_upsert";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_DBParams);

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
            if (m_Result != Packet_Result.Result.Success)
                return;

            foreach (var iter in m_BlockUsers)
                CSystemManager.Instance.UpsertBlockUser(iter);
        }
    }

    public class CQueryToolCouponLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long begin_time;
            public long end_time;

            public void SetParams(long begin, long end)
            {
                begin_time = begin;
                end_time = end;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_DBParams = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_CouponData> m_Coupons = new List<_CouponData>();

        public CQueryToolCouponLoad(long sessionKey, long begin, long end)
        {
            m_SessionKey = sessionKey;
            m_DBParams.SetParams(begin, end);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_system_coupon_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_DBParams);

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
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        var data = new _CouponData();
                        m_Data.GetData(table_index, "coupon_id", ref data.m_CouponID, i);
                        m_Data.GetData(table_index, "cnt", ref data.m_Count, i);
                        m_Data.GetData(table_index, "use_level", ref data.m_UseLevel, i);
                        m_Data.GetData(table_index, "begin_time", ref data.m_BeginTime, i);
                        m_Data.GetData(table_index, "expire_time", ref data.m_ExpireTime, i);

                        string reward_json = string.Empty;
                        m_Data.GetData(table_index, "reward", ref reward_json, i);
                        if(SJson.IsValidJson(reward_json))
                            data.m_Rewards = SJson.JsonToObject<List<_AssetData>>(reward_json);

                        m_Coupons.Add(data);
                    }
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
            CNetManager.Instance.M2O_ResultCouponLoad(m_SessionKey, m_Coupons, m_Result);
        }
    }

    public class CQueryToolSystemCouponUpsert : IMsSqlQuery
    {
        struct DBParams
        {
            public string coupon_id;
            public int cnt;
            public int use_level;
            public long begin_time;
            public long expire_time;
            public string reward;

            public void SetParams(string id, int count, int level, long begin, long exp, string reward_json)
            {
                coupon_id = id;
                cnt = count;
                use_level = level;
                begin_time = begin;
                expire_time = exp;
                reward = reward_json;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private _CouponData m_Coupon = new _CouponData();

        public CQueryToolSystemCouponUpsert(long sessionkey, string couponid, int count, int level, long begin, long exp, List<_AssetData> rewards)
        {
            m_SessionKey = sessionkey;
            string reward_json = SJson.ObjectToJson(SCopy<List<_AssetData>>.DeepCopy(rewards));
            m_Params.SetParams(couponid, count, level, begin, exp, reward_json);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_system_coupon_upsert";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
                }
                else
                {
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "coupon_id", ref m_Coupon.m_CouponID, i);
                        m_Data.GetData(table_index, "cnt", ref m_Coupon.m_Count, i);
                        m_Data.GetData(table_index, "use_level", ref m_Coupon.m_UseLevel, i);
                        m_Data.GetData(table_index, "begin_time", ref m_Coupon.m_BeginTime, i);
                        m_Data.GetData(table_index, "expire_time", ref m_Coupon.m_ExpireTime, i);

                        string reward_json = string.Empty;
                        m_Data.GetData(table_index, "reward", ref reward_json, i);
                        if (SJson.IsValidJson(reward_json))
                            m_Coupon.m_Rewards = SJson.JsonToObject<List<_AssetData>>(reward_json);
                    }
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
            CNetManager.Instance.M2O_ResultCouponCreate(m_SessionKey, m_Coupon, m_Result);
            //noting
        }
    }

    public class CQueryToolCharacaterGrowthLevelLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long user_uid;
            
            public void SetParams(long userid)
            {
                user_uid = userid;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_LevelData> m_Levels = new List<_LevelData>();

        public CQueryToolCharacaterGrowthLevelLoad(long sessionkey, long uid)
        {
            m_SessionKey = sessionkey;
            m_Params.SetParams(uid);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_character_growthlevel_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
                }
                else
                {
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        var data = new _LevelData();
                        m_Data.GetData(table_index, "id", ref data.m_TableID, i);
                        m_Data.GetData(table_index, "value", ref data.m_UseCount, i);

                        m_Levels.Add(data);
                    }
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
            CNetManager.Instance.M2O_ResultUserGrowthLevelLoad(m_SessionKey, m_Levels, m_Result);
        }
    }

    public class CQueryToolCharacaterGrowthGoldLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long user_uid;

            public void SetParams(long userid)
            {
                user_uid = userid;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_LevelData> m_Levels = new List<_LevelData>();

        public CQueryToolCharacaterGrowthGoldLoad(long sessionkey, long uid)
        {
            m_SessionKey = sessionkey;
            m_Params.SetParams(uid);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_character_growthgold_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
                }
                else
                {
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        var data = new _LevelData();
                        m_Data.GetData(table_index, "id", ref data.m_TableID, i);
                        m_Data.GetData(table_index, "value", ref data.m_UseCount, i);

                        m_Levels.Add(data);
                    }
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
            CNetManager.Instance.M2O_ResultUserGrowthGoldLoad(m_SessionKey, m_Levels, m_Result);
        }
    }

    public class CQueryToolCharacaterGachaLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long user_uid;

            public void SetParams(long userid)
            {
                user_uid = userid;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_GachaData> m_Gachas = new List<_GachaData>();

        public CQueryToolCharacaterGachaLoad(long sessionkey, long uid)
        {
            m_SessionKey = sessionkey;
            m_Params.SetParams(uid);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_character_gacha_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
                }
                else
                {
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        var data = new _GachaData();
                        m_Data.GetData(table_index, "id", ref data.m_GroupID, i);
                        m_Data.GetData(table_index, "value", ref data.m_Level, i);
                        m_Data.GetData(table_index, "exp", ref data.m_Exp, i);
                        m_Data.GetData(table_index, "rewarded", ref data.m_Rewarded, i);

                        m_Gachas.Add(data);
                    }
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
            CNetManager.Instance.M2O_ResultUserGachaLoad(m_SessionKey, m_Gachas, m_Result);
        }
    }

    public class CQueryToolCharacaterQuestLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long user_uid;

            public void SetParams(long userid)
            {
                user_uid = userid;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private _QuestBoard m_QuestMain = QuestTable.Instance.CopyDefault(CDefine.QuestType.Main);
        private _QuestBoard m_QuestDaily = QuestTable.Instance.CopyDefault(CDefine.QuestType.Daily);
        private _QuestBoard m_QuestRepeat = QuestTable.Instance.CopyDefault(CDefine.QuestType.Repeat);
        private Dictionary<string, _QuestBoard> m_QuestCheckIn = QuestTable.Instance.CopyDefaults(CDefine.QuestType.CheckIn);

        public CQueryToolCharacaterQuestLoad(long sessionkey, long uid)
        {
            m_SessionKey = sessionkey;
            m_Params.SetParams(uid);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_character_gacha_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
                }
                else
                {
                    //quest_main
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _Mission mission = new _Mission();
                        m_Data.GetData(table_index, "id", ref mission.ID, i);
                        m_Data.GetData(table_index, "idx", ref mission.Idx, i);
                        m_Data.GetData(table_index, "val", ref mission.Val, i);
                        ushort stateType = 0;
                        m_Data.GetData(table_index, "state", ref stateType, i);
                        mission.State = (CDefine.MissionState)stateType;

                        m_QuestMain.Missions.Clear();
                        m_QuestMain.Missions.Add(mission);
                    }

                    //quest repeat
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _Mission mission = new _Mission();
                        m_Data.GetData(table_index, "id", ref mission.ID, i);
                        m_Data.GetData(table_index, "idx", ref mission.Idx, i);
                        m_Data.GetData(table_index, "val", ref mission.Val, i);
                        ushort stateType = 0;
                        m_Data.GetData(table_index, "state", ref stateType, i);
                        mission.State = (CDefine.MissionState)stateType;
                        m_Data.GetData(table_index, "pass_rewarded", ref mission.PassRewarded, i);

                        int index = m_QuestRepeat.Missions.FindIndex(x => x.ID == mission.ID);
                        if (index != -1)
                            m_QuestRepeat.Missions[index] = mission;
                    }

                    //quest daily
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _Mission mission = new _Mission();
                        m_Data.GetData(table_index, "id", ref mission.ID, i);
                        m_Data.GetData(table_index, "idx", ref mission.Idx, i);
                        m_Data.GetData(table_index, "val", ref mission.Val, i);
                        ushort stateType = 0;
                        m_Data.GetData(table_index, "state", ref stateType, i);
                        mission.State = (CDefine.MissionState)stateType;
                        m_Data.GetData(table_index, "exp_time", ref m_QuestDaily.ExpTime, i);

                        int index = m_QuestDaily.Missions.FindIndex(x => x.ID == mission.ID);
                        if (index != -1)
                            m_QuestDaily.Missions[index] = mission;
                    }

                    //quest_check in
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _Mission mission = new _Mission();
                        m_Data.GetData(table_index, "id", ref mission.ID, i);
                        m_Data.GetData(table_index, "idx", ref mission.Idx, i);
                        m_Data.GetData(table_index, "val", ref mission.Val, i);
                        ushort stateType = 0;
                        m_Data.GetData(table_index, "state", ref stateType, i);
                        mission.State = (CDefine.MissionState)stateType;
                        string quest_id = string.Empty;
                        m_Data.GetData(table_index, "quest_id", ref quest_id, i);

                        if (m_QuestCheckIn.TryGetValue(quest_id, out var board))
                        {
                            m_Data.GetData(table_index, "exp_time", ref board.ExpTime, i);
                            board.Missions.Clear();
                            board.Missions.Add(mission);
                        }
                    }
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
            var list = new List<_QuestBoard>
            {
                m_QuestMain,
                m_QuestRepeat,
                m_QuestDaily
            };

            list.AddRange(m_QuestCheckIn.Values);

            CNetManager.Instance.M2O_ResultUserQuestLoad(m_SessionKey, list, m_Result);
        }
    }

    public class CQueryToolCharacaterRelicLoad : IMsSqlQuery
    {
        struct DBParams
        {
            public long user_uid;

            public void SetParams(long userid)
            {
                user_uid = userid;
            }
        }

        //input
        private long m_SessionKey = -1;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_RelicData> m_Relics = new List<_RelicData>();

        public CQueryToolCharacaterRelicLoad(long sessionkey, long uid)
        {
            m_SessionKey = sessionkey;
            m_Params.SetParams(uid);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_tool_character_relic_load";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
                }
                else
                {
                    //quest_main
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _RelicData data = new _RelicData();
                        m_Data.GetData(table_index, "group_id", ref data.m_GroupID, i);
                        m_Data.GetData(table_index, "lv", ref data.m_Level, i);
                        m_Data.GetData(table_index, "bonus_prob", ref data.m_BonusProb, i);

                        m_Relics.Add(data);
                    }
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
            CNetManager.Instance.M2O_ResultUserRelicLoad(m_SessionKey, m_Relics, m_Result);
        }
    }

    public class CQuerySystemServiceTypeUpdate : IMsSqlQuery
    {
        struct DBParams
        {
            public bool serviceopen;

            public void SetParams(bool open)
            {
                serviceopen = open;
            }
        }

        //input
        private DBParams m_Params = new DBParams();
        private bool m_Open = true;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_RelicData> m_Relics = new List<_RelicData>();

        public CQuerySystemServiceTypeUpdate(bool bopen)
        {
            m_Open = bopen;
            m_Params.SetParams(m_Open);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_service_type_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
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
            foreach (var iter in CServerManager.Instance.Servers)
            {
                var server = iter.Value;
                CNetManager.Instance.M2P_ReportServerServiceType(server.m_ServerSessionKey, m_Open);
            }
        }
    }

    public class CQuerySystemEventLoad : IMsSqlQuery
    {
        //input
        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private List<_EventData> m_Events = new List<_EventData>();

        public CQuerySystemEventLoad()
        {
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_system_load_event";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, this);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    m_Data.PrintErrorLog(sqlCmd.CommandText);
                    m_Result = Packet_Result.Result.DBError;
                }
                else
                {
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _EventData data = new _EventData();
                        m_Data.GetData(table_index, "uid", ref data.m_UID, i);
                        m_Data.GetData(table_index, "event_id", ref data.m_EventID, i);
                        m_Data.GetData(table_index, "start_date", ref data.m_StartDate, i);
                        m_Data.GetData(table_index, "end_date", ref data.m_EndDate, i);

                        m_Events.Add(data);
                    }
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
            CEventManager.Instance.LoadEvent(m_Events);
        }
    }

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