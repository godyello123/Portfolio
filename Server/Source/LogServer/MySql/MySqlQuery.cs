//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;
//using MySql.Data.MySqlClient;
//using LogServer;
//using Global;

//namespace SMySql
//{
//    public class ProcedureResult
//    {
//        public DataSet m_dataSet = new DataSet();
//        public MySqlParameter m_sqlErrorNumber = new MySqlParameter();
//        public MySqlParameter m_sqlErrorMessage = new MySqlParameter();

//        ~ProcedureResult()
//        {
//            if (m_dataSet != null)
//                m_dataSet.Dispose();

//            m_dataSet = null;
//            m_sqlErrorNumber = null;
//            m_sqlErrorMessage = null;
//        }

//        public bool IsSuccess()
//        {

//            return m_sqlErrorNumber.Value != null && (int)m_sqlErrorNumber.Value == 0;
//        }

//        public bool IsData(int index = 0)
//        {
//            return m_dataSet != null && m_dataSet.Tables != null && m_dataSet.Tables.Count > index && m_dataSet.Tables[index].Rows.Count > 0;
//        }

//        //public string GetData(string name)
//        //{
//        //    if (IsData() == false)
//        //        return string.Empty;

//        //    foreach (DataTable table in m_dataSet.Tables)
//        //    {
//        //        foreach (DataRow row in table.Rows)
//        //        {
//        //            if (row[name] != null)
//        //                return row[name].ToString();
//        //        }
//        //    }

//        //    return string.Empty;
//        //}

//        //public string GetData(string columnCompare, string nameCompare, string column)
//        //{
//        //    if (IsData() == false)
//        //        return string.Empty;

//        //    foreach (DataTable table in m_dataSet.Tables)
//        //    {
//        //        foreach (DataRow row in table.Rows)
//        //        {
//        //            if (row[columnCompare] != null && row[columnCompare].Equals(nameCompare))
//        //                return row[column].ToString();
//        //        }
//        //    }

//        //    return string.Empty;
//        //}

//        public bool GetData(int index, string name, ref int arg)
//        {
//            if (IsData(index) == false)
//                return false;

//            foreach (DataRow row in m_dataSet.Tables[index].Rows)
//            {
//                if (row[name] != null)
//                {
//                    arg = int.Parse(row[name].ToString());
//                    return true;
//                }
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref int arg, int row_index)
//        {
//            if (IsData(index) == false)
//                return false;

//            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
//                return false;

//            DataRow row = m_dataSet.Tables[index].Rows[row_index];
//            if (row[name] != null)
//            {
//                arg = int.Parse(row[name].ToString());
//                return true;
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref DateTime arg)
//        {
//            if (IsData(index) == false)
//                return false;

//            foreach (DataRow row in m_dataSet.Tables[index].Rows)
//            {
//                if (row[name] != null)
//                {
//                    arg = DateTime.Parse(row[name].ToString());
//                    return true;
//                }
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref DateTime arg, int row_index)
//        {
//            if (IsData(index) == false)
//                return false;

//            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
//                return false;

//            DataRow row = m_dataSet.Tables[index].Rows[row_index];
//            if (row[name] != null)
//            {
//                arg = DateTime.Parse(row[name].ToString());
//                return true;
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref short arg)
//        {
//            if (IsData(index) == false)
//                return false;

//            foreach (DataRow row in m_dataSet.Tables[index].Rows)
//            {
//                if (row[name] != null)
//                {
//                    arg = short.Parse(row[name].ToString());
//                    return true;
//                }
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref short arg, int row_index)
//        {
//            if (IsData(index) == false)
//                return false;

//            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
//                return false;

//            DataRow row = m_dataSet.Tables[index].Rows[row_index];
//            if (row[name] != null)
//            {
//                arg = short.Parse(row[name].ToString());
//                return true;
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref ushort arg)
//        {
//            if (IsData(index) == false)
//                return false;

//            foreach (DataRow row in m_dataSet.Tables[index].Rows)
//            {
//                if (row[name] != null)
//                {
//                    arg = ushort.Parse(row[name].ToString());
//                    return true;
//                }
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref ushort arg, int row_index)
//        {
//            if (IsData(index) == false)
//                return false;

//            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
//                return false;

//            DataRow row = m_dataSet.Tables[index].Rows[row_index];
//            if (row[name] != null)
//            {
//                arg = ushort.Parse(row[name].ToString());
//                return true;
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref Int64 arg)
//        {
//            if (IsData(index) == false)
//                return false;

//            foreach (DataRow row in m_dataSet.Tables[index].Rows)
//            {
//                if (row[name] != null)
//                {
//                    arg = Int64.Parse(row[name].ToString());
//                    return true;
//                }
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref Int64 arg, int row_index)
//        {
//            if (IsData(index) == false)
//                return false;

//            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
//                return false;

//            DataRow row = m_dataSet.Tables[index].Rows[row_index];
//            if (row[name] != null)
//            {
//                arg = Int64.Parse(row[name].ToString());
//                return true;
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref string arg)
//        {
//            if (IsData(index) == false)
//                return false;

//            foreach (DataRow row in m_dataSet.Tables[index].Rows)
//            {
//                if (row[name] != null)
//                {
//                    arg = row[name].ToString();
//                    return true;
//                }
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref string arg, int row_index)
//        {
//            if (IsData(index) == false)
//                return false;

//            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
//                return false;

//            DataRow row = m_dataSet.Tables[index].Rows[row_index];
//            if (row[name] != null)
//            {
//                arg = row[name].ToString();
//                return true;
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref bool arg)
//        {
//            if (IsData(index) == false)
//                return false;

//            foreach (DataRow row in m_dataSet.Tables[index].Rows)
//            {
//                if (row[name] != null)
//                {
//                    arg = bool.Parse(row[name].ToString());
//                    return true;
//                }
//            }

//            return false;
//        }

//        public bool GetData(int index, string name, ref bool arg, int row_index)
//        {
//            if (IsData(index) == false)
//                return false;

//            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
//                return false;

//            DataRow row = m_dataSet.Tables[index].Rows[row_index];
//            if (row[name] != null)
//            {
//                arg = bool.Parse(row[name].ToString());
//                return true;
//            }

//            return false;
//        }

//        public T GetData<T>(int index, string name, int row_index)
//        {
//            T Data = default(T);

//            if (IsData(index) == false)
//                return Data;

//            if (false == m_dataSet.Tables[index].Rows.Count > row_index)
//                return Data;

//            DataRow row = m_dataSet.Tables[index].Rows[row_index];

//            if (row[name] != null)
//            {
//                Data = (T)row[name];
//            }

//            return Data;
//        }

//        public void SetData(int index, string name, string value)
//        {
//            if (IsData() == false)
//                return;

//            DataTable table = m_dataSet.Tables[index];
//            if (table.Columns.Contains(name) == false)
//                table.Columns.Add(name);

//            foreach (DataRow row in table.Rows)
//            {
//                if (row[name] != null)
//                    row[name] = value;
//            }
//        }

//        public int GetErrorNumber()
//        {
//            if (m_sqlErrorNumber == null)
//                return -1;

//            return m_sqlErrorNumber.Value == null ? -1 : (int)m_sqlErrorNumber.Value;
//        }

//        public string GetErrorMessage()
//        {
//            if (m_sqlErrorMessage == null || m_sqlErrorMessage.Value == null)
//                return "Error Message null";

//            return m_sqlErrorMessage.Value.ToString();
//        }
//    }

//    #region Query
//    public class CMySqlQueryLogMessage : IMySqlQuery
//    {
//        public long db_id = 0;
//        public int db_type = 0;
//        public string db_log = string.Empty;
//        public DateTime db_time = Global.CDefine.MinValue();

//        private ProcedureResult m_Data = new ProcedureResult();
//        private string LogMainType = string.Empty;

//        public CMySqlQueryLogMessage(/*Base_Log LogMessage*/)
//        {
//            //db_id = LogMessage.ID;
//            //db_type = (int)LogMessage.DetailType;
//            //db_log = SCommon.SJson.ObjectToJson(LogMessage);
//            //db_time = LogMessage.TimeBinary;
//            //LogMainType = LogMessage.MainType.ToString();
//        }

//        public void Run(LogServer.SMySql agent)
//        {
//            try
//            {
//                string ProcedureString = string.Format("{0}_{1}", CMySqlDBManager.InsertQuery, LogMainType);
//                MySqlDataAdapter adapter = new MySqlDataAdapter(ProcedureString, agent.GetAgent);
//                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

//                CMySqlDBManager.Instance.SetParams(ref adapter, ref m_Data, this);

//                adapter.Dispose();
//            }
//            catch (Exception e)
//            {
//                string Error = e.ToString();
//                CLogger.Instance.System(string.Format("DB Error : {0}", Error));
//                CLogger.Instance.System(string.Format("Procedure : {0}", LogMainType));
//            }
//        }
        
//        public void Complete()
//        {
//            CNetManager.Instance.SendPacketCount++;
//        }
//    }
//    #endregion
//}
