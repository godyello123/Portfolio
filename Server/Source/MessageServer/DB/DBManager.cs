using System;
using System.Collections.Generic;
using SCommon;
using SDB;
using Global;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Diagnostics.Contracts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using Microsoft.SqlServer.Server;

namespace MessageServer
{
    public class CDBManager : SSingleton<CDBManager>, IDisposable
    {
        private bool m_Disposed;

        private bool m_Run;

        private SMsSql[] m_SystemDB;
        private SMsSql m_GameDBSync;

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
                if (m_SystemDB != null)
                {
                    foreach (var DB in m_SystemDB)
                        DB.Dispose();

                    m_SystemDB = null;
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

            m_SystemDB = new SMsSql[CConfig.Instance.DBThreadCount];
            for (int i = 0; i < m_SystemDB.Length; i++)
            {
                m_SystemDB[i] = new SMsSql();
                m_SystemDB[i].Start(connectDBString, ref error_string);
            }

            string connectgameDBString = $"server = {CConfig.Instance.GameDB.m_Host}; uid = {CConfig.Instance.GameDB.m_ID}; pwd = {CConfig.Instance.GameDB.m_PW}; database = {CConfig.Instance.GameDB.m_Name}";
            m_GameDBSync = new SMsSql();
            m_GameDBSync.Start(connectgameDBString, ref error_string);

            if (false == string.IsNullOrEmpty(error_string))
                CLogger.Instance.System(error_string);
        }
        public void Stop()
        {
            if (m_SystemDB != null) foreach (var DB in m_SystemDB) DB.Stop();
            m_GameDBSync.Stop();

            m_Run = false;
        }
        public void Update()
        {
            if (m_SystemDB != null) foreach (var DB in m_SystemDB) DB.Update();
        }

        public void InsertSystemDB(int dbguid, IMsSqlQuery query)
        {
            m_SystemDB[dbguid % m_SystemDB.Length].Insert(query);
        }

        public void InsertGameDBSync(IMsSqlQuery query)
        {
            query.Run(m_GameDBSync);
            query.Complete();
        }

        public int[] GetSystemDBFPS()
        {
            if (m_SystemDB == null) return new int[0];
            int[] fps = new int[m_SystemDB.Length];
            for (int i = 0; i < fps.Length; i++) fps[i] = m_SystemDB[i].GetFPS();
            return fps;
        }

        public int[] GetSystemDBInputQueueCount()
        {
            if (m_SystemDB == null) return new int[0];
            int[] inputQueueCount = new int[m_SystemDB.Length];
            for (int i = 0; i < inputQueueCount.Length; i++) inputQueueCount[i] = m_SystemDB[i].GetInputQueueCount();
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

        public void SetParam(ref SqlCommand sqlCmd, ref ProcedureResult data, ref SMsSql agent, string paramName, DataTable table)
        {

        }

        public int GetLeastInputQueueCount()
        {
            if (m_SystemDB == null || m_SystemDB.Length < 0)
                return 0;

            int retVal = 0;
            foreach (var iter in m_SystemDB)
                retVal = retVal > iter.GetInputQueueCount() ? iter.GetInputQueueCount() : retVal;

            return retVal;
        }

        #region Query

        public void QuerySystemDataLoad()
        {
            InsertSystemDB(1, new CQuerySystemDataLoad());
        }

        public void QuerySystemUpdateBlockUser(_BlockUser blockUser)
        {
            InsertSystemDB(0, new CQuerySystemUpdateBlockUser(blockUser));
        }

        public void QuerySystemDeleteBlockUser(string targetID)
        {
            InsertSystemDB(0, new CQuerySystemDeleteBlockUser(targetID));
        }

        public void QuerySystemDeleteWhiteUser(string targetID)
        {
            InsertSystemDB(0, new CQuerySystemDeleteWhiteUser(targetID));
        }

        public void QuerySystemUpdateWhiteUser(string targetID)
        {
            InsertSystemDB(0, new CQuerySystemUpdateWhiteUser(targetID));
        }

        public void SyncQuerySystemLoadRankMainSync(CDefine.ERankType ranktype, CDefine.eStageType stageType, int maxRankCnt, int minVal)
        {
            InsertGameDBSync(new CQuerySystemLoadRankMain(ranktype, stageType, maxRankCnt, minVal));
        }

        public void SyncQuerySystemLoadRankPvp(CDefine.ERankType ranktype, CDefine.CoinType coin_type, int maxRankCnt)
        {
            InsertGameDBSync(new CQuerySystemLoadRankPvp(ranktype, coin_type, maxRankCnt));
        }

        public void SyncQuerySystemLoadSchedule()
        {
            InsertGameDBSync(new CQuerySystemLoadSchedule());
        }

        public void SyncQuerySystemUpsertPost(long post_id, CDefine.PostType type, string title, string msg, long begin, long end, string reward)
        {
            InsertGameDBSync(new CQuerySystemUpsertPost(post_id, type, title, msg, begin, end, reward));
        }

        public void QuerySystemServiceTypeUpdate(bool bopne)
        {
            InsertSystemDB(GetLeastInputQueueCount(), new CQuerySystemServiceTypeUpdate(bopne));
        }

        //=======================Oper Tool ==============================
        public void QueryyToolCharacterInfoLoad(long sessionKey, long user_uid, string user_device_id, string user_name)
        {
            InsertGameDBSync(new CQueryyToolCharacterInfoLoad(sessionKey, user_uid, user_device_id, user_name));
        }

        public void QueryyToolSystemPostLoad(long sessionKey, long begin, long end)
        {
            InsertGameDBSync(new CQueryyToolSystemPostLoad(sessionKey, begin, end));
        }

        public void QueryToolSystemUpsertPost(long sessionkey, long post_id, CDefine.PostType type, string title, string msg, long begin, long end, string reward)
        {
            InsertGameDBSync(new CQueryToolSystemUpsertPost(sessionkey, post_id, type, title, msg, begin, end, reward));
        }

        public void QueryToolSystemNoticeLoad(long sessionKey, DateTime begin, DateTime end)
        {
            InsertSystemDB(GetLeastInputQueueCount(), new CQueryToolSystemNoticeLoad(sessionKey, begin, end));
        }

        public void QueryToolSystemNoticeUpdate(long sessionKey, _NoticeData notice)
        {
            InsertSystemDB(GetLeastInputQueueCount(), new CQueryToolSystemNoticeUpdate(sessionKey, notice));
        }

        public void QueryToolSystemNoticeErase(long sessionKey, long noticeID)
        {
            InsertSystemDB(GetLeastInputQueueCount(), new CQueryToolSystemNoticeErase(sessionKey, noticeID));
        }

        public void QueryToolUserPostLoad(long sessionKey, long uid, long start, long end)
        {
            InsertGameDBSync(new CQueryToolUserPostLoad(sessionKey, uid, start, end));
        }

        public void QueryToolUserPostInsert(long sessionkey, long userid, long post_id, CDefine.PostType type, string title, string msg, long begin, long end, string reward)
        {
            InsertGameDBSync(new CQueryToolUserPostInsert(sessionkey, userid, post_id, type, title, msg, begin, end, reward));
        }

        public void QueryToolBlockUserLoad(long sessionKey)
        {
            InsertSystemDB(GetLeastInputQueueCount(), new CQueryToolBlockUserLoad(sessionKey));
        }

        public void QueryToolBlockUserUpsert(List<_BlockUser> blocks)
        {
            InsertSystemDB(GetLeastInputQueueCount(), new CQueryToolBlockUserUpsert(blocks));
        }

        public void QueryToolCouponLoad(long sessionKey, long begin, long end)
        {
            InsertGameDBSync(new CQueryToolCouponLoad(sessionKey, begin, end));
        }

        public void QueryToolSystemCouponUpsert(long sessionKey, string couponID, int count, int lv, long begin, long exp, List<_AssetData> rewards)
        {
            InsertGameDBSync(new CQueryToolSystemCouponUpsert(sessionKey, couponID, count, lv, begin, exp, rewards));
        }

        public void QueryToolCharacaterGrowthLevelLoad(long sessionKey, long targetID)
        {
            InsertGameDBSync(new CQueryToolCharacaterGrowthLevelLoad(sessionKey, targetID));
        }

        public void QueryToolCharacaterGrowthGoldLoad(long sessionKey, long targetID)
        {
            InsertGameDBSync(new CQueryToolCharacaterGrowthGoldLoad(sessionKey, targetID));
        }

        public void QueryToolCharacaterGachaLoad(long sessionKey, long targetID)
        {
            InsertGameDBSync(new CQueryToolCharacaterGachaLoad(sessionKey, targetID));
        }

        public void QueryToolCharacaterQuestLoad(long sessionKey, long targetID)
        {
            InsertGameDBSync(new CQueryToolCharacaterQuestLoad(sessionKey, targetID));
        }

        public void QueryToolCharacaterRelicLoad(long sessionKey, long targetID)
        {
            InsertGameDBSync(new CQueryToolCharacaterRelicLoad(sessionKey, targetID));
        }

        public void QuerySystemEventLoad()
        {
            InsertGameDBSync(new CQuerySystemEventLoad());
        }


        #endregion


    }
}
