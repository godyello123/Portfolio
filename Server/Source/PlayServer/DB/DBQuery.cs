using System;
using System.Collections.Generic;
using SCommon;
using SDB;
using Global;
using System.Data.SqlClient;
using System.Data;
using Packet_Result;
using System.Windows.Data;
using System.Windows.Forms;
using System.Security.Policy;
using System.Reflection;
using System.Windows.Forms.VisualStyles;

namespace PlayServer
{
    #region base

    public class DBCharacterLoadData
    {
        public string DeviceID = string.Empty;
        public _UserData UserData = new _UserData();
        public List<_StageData> StageDatas = new List<_StageData>();
        public List<_AssetData> CoinDatas = new List<_AssetData>();
        public List<_LevelData> GrowthGoldDatas = new List<_LevelData>();
        public List<_LevelData> GrowthLevelDatas = new List<_LevelData>();
        public _QuestBoard QuestMains = QuestTable.Instance.CopyDefault(CDefine.QuestType.Main);
        public List<_ItemData> Items = new List<_ItemData>();
        public List<_ItemEquipPresetData> EquipPresets = new List<_ItemEquipPresetData>();
        public List<_PlayerPref> PlayerPrefs = new List<_PlayerPref>();
        public List<_KnightEquipPresetData> KnightPresets = new List<_KnightEquipPresetData>();
        public List<_GachaData> GachaDatas = new List<_GachaData>();
        public List<_SkillData> SkillDatas = new List<_SkillData>();
        public List<_SkillEquipPresetData> SkillPresets = new List<_SkillEquipPresetData>();
        public List<_PostData> Posts = new List<_PostData>();
        public HashSet<string> Coupons = new HashSet<string>();
        public _RankReward RankReward = new _RankReward();
        public _QuestBoard QuestDaily = QuestTable.Instance.CopyDefault(CDefine.QuestType.Daily);
        public _QuestBoard QuestRepeat = QuestTable.Instance.CopyDefault(CDefine.QuestType.Repeat);
        public Dictionary<string, _QuestBoard> QuestCheckIn = QuestTable.Instance.CopyDefaults(CDefine.QuestType.CheckIn);
        public Dictionary<string, _QuestBoard> QuestPass = QuestTable.Instance.CopyDefaults(CDefine.QuestType.Pass);
        public List<_RelicData> Relics = new List<_RelicData>();
        public List<_AdsBUffData> AdsBuffs = new List<_AdsBUffData>();
        public List<_StageSkillData> StageSkills = new List<_StageSkillData>();
        public List<_EventData> EventDatas = new List<_EventData>();
        public List<_EventShopData> EventShopDatas = new List<_EventShopData>();
        public Dictionary<string, _QuestBoard> QuestEvent = new Dictionary<string, _QuestBoard>();
        public List<_EventRouletteData> EventRouletteDatas = new List<_EventRouletteData>();
    }


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

    #region QUERY

    //==================== PLAYER ===================
    public class CQueryCharacterCreate : IMsSqlQuery
    {
        //input
        private long m_SessionKey;
        private long m_UID;
        private int m_ProfileID;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private string m_DeviceID = string.Empty;

        struct DBParams
        {
            public long uid;
            public string name;
            public int profile_id;
        }

        public CQueryCharacterCreate(long sessionKey, long uid, int profileid)
        {
            m_SessionKey = sessionKey;
            m_UID = uid;
            m_ProfileID = profileid;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_create";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    uid = m_UID,
                    name = $"user_{m_UID}",
                    profile_id = m_ProfileID
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
                    m_Data.GetData(0, "uid", ref m_UID);
                    m_Data.GetData(0, "device_id", ref m_DeviceID);
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

            CLogger.Instance.Debug($"CQueryCharacterCreate Compleate : {m_UID}");

            if (m_Result != Packet_Result.Result.Success || string.IsNullOrEmpty(m_DeviceID))
                CNetManager.Instance.P2C_ResultEnterServer(m_SessionKey, (ushort)m_Result);
            else
                CNetManager.Instance.P2M_RequestEnterUser(m_SessionKey, m_UID, m_DeviceID);
        }
    }
    public class CQueryCharacterLoad : IMsSqlQuery
    {
        //input
        private long m_SessionKey;
        private long m_UID;
        private string m_DeviceID;
        
        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private DBCharacterLoadData m_DBLoadData = new DBCharacterLoadData();

        struct DBParams
        {
            public long uid;
        }

        public CQueryCharacterLoad(long sessionKey, long uid, string deviceid)
        {
            m_SessionKey = sessionKey;
            m_UID = uid;
            m_DeviceID = deviceid;
            m_DBLoadData.DeviceID = m_DeviceID;
        }
        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_login";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    uid = m_UID
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
                    //userinfo
                    int table_index = 0;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        m_Data.GetData(table_index, "uid", ref m_DBLoadData.UserData.m_UID, i);
                        m_Data.GetData(table_index, "device_id", ref m_DBLoadData.UserData.m_DeviceID, i);
                        m_Data.GetData(table_index, "name", ref m_DBLoadData.UserData.m_Name, i);
                        m_Data.GetData(table_index, "event_stage", ref m_DBLoadData.UserData.m_EventStageID, i);
                        m_Data.GetData(table_index, "level", ref m_DBLoadData.UserData.m_Level, i);
                        m_Data.GetData(table_index, "exp", ref m_DBLoadData.UserData.m_Exp, i);
                        m_Data.GetData(table_index, "level_point", ref m_DBLoadData.UserData.m_LevelPoint, i);
                        m_Data.GetData(table_index, "ad_skip", ref m_DBLoadData.UserData.m_ADSkip, i);
                        m_Data.GetData(table_index, "profile_id" ,ref m_DBLoadData.UserData.m_ProfileID, i);
                        m_Data.GetData(table_index, "login_time", ref m_DBLoadData.UserData.m_LoginTime, i);
                        m_Data.GetData(table_index, "logout_time", ref m_DBLoadData.UserData.m_LogoutTime, i);
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
                        m_Data.GetData(table_index, "is_loop", ref stageData.loop, i);

                        m_DBLoadData.StageDatas.Add(stageData);
                    }

                    //coin
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _AssetData coinData = new _AssetData();
                        coinData.Type = CDefine.AssetType.Coin;

                        m_Data.GetData(table_index, "type", ref coinData.TableID, i);
                        m_Data.GetData(table_index, "value", ref coinData.Count, i);

                        m_DBLoadData.CoinDatas.Add(coinData);
                    }

                    //growthGold
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _LevelData ggData = new _LevelData();
                        m_Data.GetData(table_index, "id", ref ggData.m_TableID, i);
                        m_Data.GetData(table_index, "value", ref ggData.m_UseCount, i);

                        m_DBLoadData.GrowthGoldDatas.Add(ggData);
                    }

                    //growthLevel
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _LevelData glData = new _LevelData();
                        m_Data.GetData(table_index, "id", ref glData.m_TableID, i);
                        m_Data.GetData(table_index, "value", ref glData.m_UseCount, i);

                        m_DBLoadData.GrowthLevelDatas.Add(glData);
                    }

                    //quest_main
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
                        
                        m_DBLoadData.QuestMains.Missions.Clear();
                        m_DBLoadData.QuestMains.Missions.Add(mission);
                    }

                    //item
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _ItemData item = new _ItemData();
                        m_Data.GetData(table_index, "item_id", ref item.ItemID, i);
                        m_Data.GetData(table_index, "table_id", ref item.TableID, i);
                        m_Data.GetData(table_index, "level", ref item.Level, i);
                        m_Data.GetData(table_index, "count", ref item.Count, i);
                        //m_Data.GetData(table_index, "horse_id", ref item.HorseID, i);
                        m_Data.GetData(table_index, "in_used", ref item.m_InUsed, i);
                        string strRandOption = string.Empty;
                        m_Data.GetData(table_index, "random_option", ref strRandOption, i);
                        if (!string.IsNullOrEmpty(strRandOption)&& SJson.IsValidJson(strRandOption))
                            item.RandomOption = SCommon.SJson.JsonToObject<List<_RandomOption>>(strRandOption);

                        m_DBLoadData.Items.Add(item);
                    }

                    //equip preset
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _ItemEquipPresetData preset = new _ItemEquipPresetData();
                        m_Data.GetData(table_index, "idx", ref preset.m_Index, i);
                        string strSlots = string.Empty;
                        m_Data.GetData(table_index, "equip_slot", ref strSlots, i);
                        if (!string.IsNullOrEmpty(strSlots)&&SJson.IsValidJson(strSlots))
                            preset.m_EquipedSlots = SCommon.SJson.JsonToObject<List<_ItemEquipSlotData>>(strSlots);
                        m_Data.GetData(table_index, "is_enable", ref preset.m_IsEnable, i);

                        m_DBLoadData.EquipPresets.Add(preset);
                    }

                    //pref
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _PlayerPref pref = new _PlayerPref();
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type);
                        pref.m_Type = (CDefine.PlayerPref)type;
                        m_Data.GetData(table_index, "strval", ref pref.m_StrVal);

                        m_DBLoadData.PlayerPrefs.Add(pref);
                    }

                    //knight preset
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _KnightEquipPresetData preset = new _KnightEquipPresetData();
                        m_Data.GetData(table_index, "idx", ref preset.m_Index, i);
                        string strSlots = string.Empty;
                        m_Data.GetData(table_index, "equip_slot", ref strSlots, i);
                        if (!string.IsNullOrEmpty(strSlots)&&SJson.IsValidJson(strSlots))
                            preset.m_EquipedSlots = SCommon.SJson.JsonToObject<List<_KnightEquipSlotData>>(strSlots);
                        m_Data.GetData(table_index, "is_enable", ref preset.m_IsEnable, i);

                        m_DBLoadData.KnightPresets.Add(preset);
                    }

                    //gacha level
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _GachaData gachadata = new _GachaData();
                        m_Data.GetData(table_index, "id", ref gachadata.m_GroupID, i);
                        m_Data.GetData(table_index, "lv", ref gachadata.m_Level, i);
                        m_Data.GetData(table_index, "exp", ref gachadata.m_Exp, i);
                        m_Data.GetData(table_index, "rewarded", ref gachadata.m_Rewarded, i);

                        m_DBLoadData.GachaDatas.Add(gachadata);
                    }

                    //skill
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _SkillData data = new _SkillData();
                        m_Data.GetData(table_index, "group_id", ref data.GroupID, i);
                        m_Data.GetData(table_index, "tid", ref data.TID, i);
                        m_Data.GetData(table_index, "level", ref data.Lv, i);
                        m_Data.GetData(table_index, "is_learend", ref data.IsLearend, i);

                        m_DBLoadData.SkillDatas.Add(data);
                    }

                    //skill preset
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _SkillEquipPresetData preset = new _SkillEquipPresetData();
                        m_Data.GetData(table_index, "idx", ref preset.m_Index, i);
                        string strSlots = string.Empty;
                        m_Data.GetData(table_index, "equip_slot", ref strSlots, i);
                        if (!string.IsNullOrEmpty(strSlots) && SJson.IsValidJson(strSlots))
                            preset.m_EquipedSlots = SCommon.SJson.JsonToObject<List<_SkillEquipSlotData>>(strSlots);
                        m_Data.GetData(table_index, "is_enable", ref preset.m_IsEnable, i);

                        m_DBLoadData.SkillPresets.Add(preset);
                    }

                    //post
                    //++table_index;
                    //for(int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    //{
                    //    _PostData post = new _PostData();
                    //    m_Data.GetData(table_index, "id", ref post.ID, i);
                    //    ushort type = 0;
                    //    m_Data.GetData(table_index, "type", ref type, i);
                    //    post.Type = (CDefine.PostType)type;
                    //    m_Data.GetData(table_index, "title", ref post.Title, i);
                    //    m_Data.GetData(table_index, "msg", ref post.Msg, i);
                    //    m_Data.GetData(table_index, "is_read", ref post.IsRead, i);
                    //    m_Data.GetData(table_index, "is_reward", ref post.IsRewarded, i);
                    //    m_Data.GetData(table_index, "begin_time", ref post.beginTime, i);
                    //    m_Data.GetData(table_index, "expire_time", ref post.expireTime, i);
                    //    string str_rewards = "";
                    //    m_Data.GetData(table_index, "reward", ref str_rewards, i);
                    //    if (!string.IsNullOrEmpty(str_rewards) && SJson.IsValidJson(str_rewards))
                    //        post.Rewards = SCommon.SJson.JsonToObject<List<_AssetData>>(str_rewards);

                    //    m_DBLoadData.Posts.Add(post);
                    //}

                    //coupon
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        string coupon_str = string.Empty;
                        m_Data.GetData(table_index, "coupon_json", ref coupon_str, i);
                        if (!string.IsNullOrEmpty(coupon_str) && SJson.IsValidJson(coupon_str))
                            m_DBLoadData.Coupons = SJson.JsonToObject<HashSet<string>>(coupon_str);
                    }

                    //rank reward
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        ushort type = 0;
                        m_Data.GetData(table_index, "type", ref type, i);
                        m_DBLoadData.RankReward.m_Type = (CDefine.ERankType)type;
                        ushort state = 0;
                        m_Data.GetData(table_index, "state", ref state, i);
                        m_DBLoadData.RankReward.m_State = (CDefine.ERewardState)state;
                        m_Data.GetData(table_index, "val", ref m_DBLoadData.RankReward.m_RankValue, i);
                        m_Data.GetData(table_index, "exp_time", ref m_DBLoadData.RankReward.m_ExpTime, i);
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

                        int index = m_DBLoadData.QuestRepeat.Missions.FindIndex(x => x.ID == mission.ID);
                        if (index != -1)
                            m_DBLoadData.QuestRepeat.Missions[index] = mission;
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
                        m_Data.GetData(table_index, "exp_time", ref m_DBLoadData.QuestDaily.ExpTime, i);

                        int index = m_DBLoadData.QuestDaily.Missions.FindIndex(x => x.ID == mission.ID);
                        if (index != -1)
                            m_DBLoadData.QuestDaily.Missions[index] = mission;
                    }

                    //quest_check in
                    ++table_index;
                    for(int i = 0;i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
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

                        if (m_DBLoadData.QuestCheckIn.TryGetValue(quest_id, out var board))
                        {
                            m_Data.GetData(table_index, "exp_time", ref board.ExpTime, i);
                            board.Missions.Clear();
                            board.Missions.Add(mission);
                        }
                    }

                    //quest_pass
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
                        m_Data.GetData(table_index, "pass_rewarded",ref mission.PassRewarded, i);
                        string quest_id = string.Empty;
                        m_Data.GetData(table_index, "quest_id", ref quest_id, i);

                        if (m_DBLoadData.QuestPass.TryGetValue(quest_id, out var board))
                        {
                            int index = board.Missions.FindIndex(x => x.ID == mission.ID);
                            if (index != -1)
                            {
                                board.Missions[index] = mission;
                                m_Data.GetData(table_index, "pass_active", ref board.IsPassActivated, i);
                            }
                        }
                    }

                    //relic
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _RelicData relic = new _RelicData();
                        m_Data.GetData(table_index, "group_id",ref relic.m_GroupID, i);
                        m_Data.GetData(table_index, "lv", ref relic.m_Level, i);
                        m_Data.GetData(table_index, "bonus_prob", ref relic.m_BonusProb, i);

                        m_DBLoadData.Relics.Add(relic);
                    }

                    //adbuffs
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _AdsBUffData data = new _AdsBUffData();
                        m_Data.GetData(table_index, "buff_id", ref data.m_BuffID, i);
                        m_Data.GetData(table_index, "buff_lv", ref data.m_BuffLv, i);
                        m_Data.GetData(table_index, "buff_exp", ref data.m_BuffExp, i);
                        m_Data.GetData(table_index, "buff_exp_time", ref data.m_BuffExpTime, i);
                        m_Data.GetData(table_index, "watch_exp_time", ref data.m_WatchAdsDailyExpTime, i);
                        m_Data.GetData(table_index, "watch_count", ref data.m_WatchAdsDailyCount, i);

                        m_DBLoadData.AdsBuffs.Add(data);
                    }

                    //stage skill
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _StageSkillData data = new _StageSkillData();
                        ushort typeString = 0;
                        m_Data.GetData(table_index, "type", ref typeString, i);
                        data.m_Type = (CDefine.eStageType)typeString;

                        string strSlots = string.Empty;
                        m_Data.GetData(table_index, "equip_slot", ref strSlots, i);
                        if (!string.IsNullOrEmpty(strSlots) && SJson.IsValidJson(strSlots))
                            data.m_EquipedSlots = SCommon.SJson.JsonToObject<List<_SkillEquipSlotData>>(strSlots);
                        
                        m_DBLoadData.StageSkills.Add(data);
                    }

                    //event
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _EventData data = new _EventData();
                        m_Data.GetData(table_index, "event_uid", ref data.m_UID, i);
                        m_Data.GetData(table_index, "event_id", ref data.m_EventID, i);
                        m_Data.GetData(table_index, "start_date", ref data.m_StartDate, i);
                        m_Data.GetData(table_index, "end_date", ref data.m_EndDate, i);

                        m_DBLoadData.EventDatas.Add(data);
                    }

                    //quest event
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

                        var questReocrd = QuestTable.Instance.Find(quest_id);
                        if (questReocrd == null)
                            continue;

                        if (m_DBLoadData.QuestEvent.TryGetValue(quest_id, out var board))
                        {
                            board.Missions.Add(mission);
                            m_Data.GetData(table_index, "exp_time", ref board.ExpTime, i);
                        }
                        else
                        {
                            board = new _QuestBoard();
                            board.Type = questReocrd.Quest_Type;
                            m_Data.GetData(table_index, "exp_time", ref board.ExpTime, i);
                            board.Missions.Add(mission);
                            board.ID = quest_id;
                            m_DBLoadData.QuestEvent.Add(quest_id, board);
                        }
                    }

                    //event shop
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _EventShopData data = new _EventShopData();
                        m_Data.GetData(table_index, "event_uid", ref data.m_EventUID, i);
                        m_Data.GetData(table_index, "shop_id", ref data.m_EventShopID, i);
                        m_Data.GetData(table_index, "limit_count", ref data.m_BuyCount, i);

                        m_DBLoadData.EventShopDatas.Add(data);
                    }

                    //event roulette
                    ++table_index;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; ++i)
                    {
                        _EventRouletteData data = new _EventRouletteData();
                        m_Data.GetData(table_index, "event_uid", ref data.m_EventUID, i);
                        string temp = string.Empty;
                        m_Data.GetData(table_index, "exclude_list", ref temp, i);
                        if (SJson.IsValidJson(temp))
                            data.m_ExcludeIndexs = SJson.JsonToObject<List<int>>(temp);

                        m_DBLoadData.EventRouletteDatas.Add(data);
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
            if (!CNetManager.Instance.IsAliveSession(m_SessionKey))
                return;

            CLogger.Instance.Debug($"CQueryCharacterLoad Compleate : {m_UID}");

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultEnterServer(m_SessionKey, (ushort)m_Result);
            else
                CUserManager.Instance.EnterUser(m_SessionKey, m_DBLoadData);
        }
    }
    public class CQueryCharacterUpdateGoldGrowth : IMsSqlQuery
    {
        //input
        private long m_SessionKey;
        private long m_UID;
        private _LevelData m_GoldGrowth = new _LevelData();
        private CDBMerge m_DBTran = new CDBMerge();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long uid;
            public string update_coin;
            public int type;
            public long value;
        }

        public CQueryCharacterUpdateGoldGrowth(long sessionKey, long uid, _LevelData growthGold, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = uid;
            m_GoldGrowth = growthGold;
            m_DBTran = dbtran;
        }
        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_gold_growth_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    uid = m_UID,
                    update_coin = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateCoinList),
                    type = m_GoldGrowth.m_TableID,
                    value = m_GoldGrowth.m_UseCount
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.LevelAgent.AfterQueryGoldGrowthUp(m_SessionKey, m_Result, m_GoldGrowth, m_DBTran.GetUpdateCoinList());
        }
    }
    public class CQueryCharacterUpdateLevelGrowth : IMsSqlQuery
    {
        //input
        private long m_SessionKey;
        private long m_UID;
        private _LevelData m_LevelGrowth = new _LevelData();
        private _UserData m_UserData = new _UserData();
        private Packet_C2P.Protocol m_Protocol = Packet_C2P.Protocol.Max;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long uid;
            public int type;
            public long value;
            public int level_point;
        }

        public CQueryCharacterUpdateLevelGrowth(long sessionKey, _LevelData levelgrowth, _UserData userData, Packet_C2P.Protocol protocol)
        {
            m_SessionKey = sessionKey;
            m_UID = userData.m_UID;
            m_LevelGrowth = levelgrowth;
            m_UserData = userData;
            m_Protocol = protocol;
        }
        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_level_growth_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    uid = m_UID,
                    type = m_LevelGrowth.m_TableID,
                    value = m_LevelGrowth.m_UseCount,
                    level_point = m_UserData.m_LevelPoint
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.LevelAgent.AfterQueryLevelGrowthUp(m_SessionKey, m_Result, m_LevelGrowth, m_UserData, m_Protocol);
        }
    }
    public class CQueryCharacterLogout : IMsSqlQuery
    {
        //input
        private _UserData m_UserData = new _UserData();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long uid;
            public int user_level;
            public long user_exp;
            public int user_level_point;
            public bool user_ad_skip;
            public int user_profile_id;
        }

        public CQueryCharacterLogout(_UserData userData)
        {
            m_UserData = userData;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_logout";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    uid = m_UserData.m_UID,
                    user_level = m_UserData.m_Level,
                    user_exp = m_UserData.m_Exp,
                    user_level_point = m_UserData.m_LevelPoint,
                    user_ad_skip = m_UserData.m_ADSkip,
                    user_profile_id = m_UserData.m_ProfileID
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
            //don't anything
        }
    }
    public class CQueryCharacterUpsertPlayerPref : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public ushort type;
            public string strval;

            public void SetParams(long user_uid, _PlayerPref pref)
            {
                uid = user_uid;
                type = (ushort)pref.m_Type;
                strval = pref.m_StrVal;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpsertPlayerPref(long sessionKey, _PlayerPref pref)
        {
            m_SessionKey = sessionKey;
            m_Params.SetParams(m_UID, pref);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_pref_update";

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
            //nothing
        }
    }
    public class CQueryCharacterUpdateNickName : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string user_name;
            public string update_coins;

            public void SetParams(long userid, string name, string json_update_coins)
            {
                uid = userid;
                user_name = name;
                update_coins = json_update_coins;
            }
        }

        //input
        private long m_SessionKey = 0;
        private CDBMerge m_DBTran = new CDBMerge();
        private string m_Name = string.Empty;
        private DBParams m_Params = new DBParams();
        private List<_AssetData> m_Assets = new List<_AssetData>();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateNickName(long sessionKey, long uid, string name, List<_AssetData> assets)
        {
            m_SessionKey = sessionKey;
            m_Name = name;
            m_Assets = assets;
            string update_coins = SCommon.SJson.ObjectToJson(assets);
            m_Params.SetParams(uid, name, update_coins);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_nickname_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    switch (m_Data.GetErrorNumber())
                    {
                        case 1:
                            {
                                m_Result = Packet_Result.Result.Duplicated_Name;
                            }
                            break;
                        default:
                            {
                                m_Data.PrintErrorLog(sqlCmd.CommandText);
                                m_Result = Packet_Result.Result.DBError;
                            }
                            break;
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result == Packet_Result.Result.DBError)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            if (m_Result == Packet_Result.Result.Duplicated_Name)
            {
                CNetManager.Instance.P2C_ResultChangeNickName(m_SessionKey, user.UserData, m_Result);
                return;
            }

            user.AfterQueryChangeNickName(m_SessionKey, m_Name, m_Assets, m_Result);
        }
    }
    public class CQueryCharacterViewUserInfo : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            
            public void SetParams(long user_uid)
            {
                uid = user_uid;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private string m_TargetName = string.Empty;
        private int m_TargetLevel = 0;
        private int m_TargetStageTID = -1;
        private int m_TargetProfileID = 0;
        private ProcedureResult m_Data = new ProcedureResult();
        private _EquipViewData m_EquipViewData = new _EquipViewData();
        
        public CQueryCharacterViewUserInfo(long sessionKey, long userUID)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Params.SetParams(m_UID);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_view_user_info";

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
                    int table_index = 0;
                    m_Data.GetData(table_index, "name", ref m_TargetName);
                    m_Data.GetData(table_index, "level", ref m_TargetLevel);
                    m_Data.GetData(table_index, "max_tid", ref m_TargetStageTID);
                    m_Data.GetData(table_index, "profile_id", ref m_TargetProfileID);

                    //item preset
                    table_index++;
                    for(int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; i++)
                    {
                        _ItemData data = new _ItemData();
                        m_Data.GetData(table_index, "item_tid", ref data.TableID, i);
                        m_Data.GetData(table_index, "item_lv", ref data.Level, i);
                        string random_option = string.Empty;
                        m_Data.GetData(table_index, "item_options", ref random_option, i);
                        if (!string.IsNullOrEmpty(random_option) && SCommon.SJson.IsValidJson(random_option))
                            data.RandomOption = SJson.JsonToObject<List<_RandomOption>>(random_option);

                        m_EquipViewData.m_EquipItems.Add(data);
                    }

                    //knight preset
                    table_index++;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; i++)
                    {
                        _ItemData data = new _ItemData();
                        m_Data.GetData(table_index, "item_tid", ref data.TableID, i);
                        m_Data.GetData(table_index, "item_lv", ref data.Level, i);
                        string random_option = string.Empty;
                        m_Data.GetData(table_index, "item_options", ref random_option, i);
                        if (!string.IsNullOrEmpty(random_option) && SCommon.SJson.IsValidJson(random_option))
                            data.RandomOption = SJson.JsonToObject<List<_RandomOption>>(random_option);

                        m_EquipViewData.m_EquipKnights.Add(data);
                    }

                    //skill_preset
                    table_index++;
                    for (int i = 0; i < m_Data.m_dataSet.Tables[table_index].Rows.Count; i++)
                    {
                        _SkillData data = new _SkillData();
                        m_Data.GetData(table_index, "group_id", ref data.GroupID, i);
                        m_Data.GetData(table_index, "tid", ref data.TID, i);
                        m_Data.GetData(table_index, "level", ref data.Lv, i);

                        m_EquipViewData.m_EquipSkills.Add(data);
                    }

                    m_EquipViewData.m_UID = m_UID;
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            CNetManager.Instance.P2C_ResultViewUserInfo(m_SessionKey, m_TargetName, m_TargetLevel, m_TargetStageTID, m_TargetProfileID, m_EquipViewData, m_Result);
        }
    }

    //==================== STAGE ===================
    public class CQueryCharacterUpdateStage : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public ushort stage_type;
            public int cur_tid;
            public int max_tid;
            public long total_cnt;
            public bool is_loop;
            public string update_coin;
            public int user_level;
            public long user_exp;
            public int user_level_point;
            public int user_event_stage;
            public string update_item;

            public void SetDBParams(long _uid, ushort _stage_type, int _curTID, int _maxTID, long _total_cnt, bool _isloop,
                int _user_level, long _user_exp, int _level_point, int _event_stage, CDBMerge dbtran)
            {
                uid = _uid;
                stage_type = _stage_type;
                cur_tid = _curTID;
                max_tid = _maxTID;
                total_cnt = _total_cnt;
                is_loop = _isloop;
                user_level = _user_level;
                user_exp = _user_exp;
                user_level_point = _level_point;
                user_event_stage = _event_stage;
                update_coin = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_item = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        //input
        private long m_SessionKey;
        private long m_UID;
        private _StageData m_StageData = new _StageData();
        private CRewardInfo m_ForClient = new CRewardInfo();
        private _UserData m_UserData = new _UserData();
        private CDBMerge m_DBTran = new CDBMerge();
        private bool m_IsClear = false;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateStage(long sessionKey, _UserData userData, _StageData stageData, CRewardInfo forClient, CDBMerge dbtran, bool isclear)
        {
            m_SessionKey = sessionKey;
            m_UID = userData.m_UID;
            m_StageData = stageData;
            m_ForClient = forClient;
            m_IsClear = isclear;
            m_UserData = userData;
            m_DBTran = dbtran;
            m_Params.SetDBParams(m_UID, (ushort)m_StageData.type, m_StageData.curTID, m_StageData.maxTID, m_StageData.totalCnt, m_StageData.loop, m_UserData.m_Level,
                m_UserData.m_Exp, m_UserData.m_LevelPoint, m_UserData.m_EventStageID, m_DBTran);
        }
        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_stage_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.StageAgent.AfterQueryStageClear(m_SessionKey, m_ForClient, m_StageData, m_IsClear, m_Result, m_UserData);
        }
    }
    public class CQueryCharacterStageSave : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public ushort stage_type;
            public int cur_tid;
            public int max_tid;
            public long total_cnt;
            public bool is_loop;


            public void SetDBParams(long _uid, ushort _stage_type, int _curTID, int _maxTID, long _total_cnt, bool _isloop)
            {
                uid = _uid;
                stage_type = _stage_type;
                cur_tid = _curTID;
                max_tid = _maxTID;
                total_cnt = _total_cnt;
                is_loop = _isloop;
            }
        }

        //input
        private long m_SessionKey;
        private long m_UID;
        private _StageData m_StageData = new _StageData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterStageSave(long sessionKey, long uid, _StageData stageData)
        {
            m_SessionKey = sessionKey;
            m_UID = uid;
            m_StageData = stageData;
            m_Params.SetDBParams(m_UID, (ushort)m_StageData.type, m_StageData.curTID, m_StageData.maxTID, m_StageData.totalCnt, m_StageData.loop);
        }
        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_stage_save";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);

            //noting
        }
    }
    public class CQueryCharacterStageSweep : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_coin;
            public string update_item;

            public void SetDBParams(long user_uid, CDBMerge dbtran)
            {
                uid = user_uid;
                update_coin = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_item = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        //input
        private long m_SessionKey;
        private long m_UID;
        private CRewardInfo m_ForClient = new CRewardInfo();
        private CDBMerge m_DBTran = new CDBMerge();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterStageSweep(long sessionKey, long uid, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = uid;
            m_ForClient = forClient;
            m_DBTran = SCopy<CDBMerge>.DeepCopy(dbtran);
            m_Params.SetDBParams(m_UID, m_DBTran);
        }
        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_stage_sweep";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            }

            user.StageAgent.AfterQueryStageSweap(m_SessionKey, m_ForClient, m_DBTran, m_Result);
        }
    }

    public class CQueryCharacterUpdateUpgradeStage : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public ushort stage_type;
            public int cur_tid;
            public int max_tid;
            public long total_cnt;
            public bool is_loop;
            public string update_coin;
            public int user_level;
            public long user_exp;
            public int user_level_point;
            public int user_event_stage;
            public string update_item;

            public void SetDBParams(long _uid, ushort _stage_type, int _curTID, int _maxTID, long _total_cnt, bool _isloop,
                int _user_level, long _user_exp, int _level_point, int _event_stage, CDBMerge dbtran)
            {
                uid = _uid;
                stage_type = _stage_type;
                cur_tid = _curTID;
                max_tid = _maxTID;
                total_cnt = _total_cnt;
                is_loop = _isloop;
                user_level = _user_level;
                user_exp = _user_exp;
                user_level_point = _level_point;
                user_event_stage = _event_stage;
                update_coin = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_item = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        //input
        private long m_SessionKey;
        private long m_UID;
        private _StageData m_StageData = new _StageData();
        private CRewardInfo m_ForClient = new CRewardInfo();
        private _UserData m_UserData = new _UserData();
        private CDBMerge m_DBTran = new CDBMerge();
        private bool m_IsClear = false;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateUpgradeStage(long sessionKey, _UserData userData, _StageData stageData, CRewardInfo forClient, CDBMerge dbtran, bool isclear)
        {
            m_SessionKey = sessionKey;
            m_UID = userData.m_UID;
            m_StageData = stageData;
            m_ForClient = forClient;
            m_IsClear = isclear;
            m_UserData = userData;
            m_DBTran = dbtran;
            m_Params.SetDBParams(m_UID, (ushort)m_StageData.type, m_StageData.curTID, m_StageData.maxTID, m_StageData.totalCnt, m_StageData.loop, m_UserData.m_Level,
                m_UserData.m_Exp, m_UserData.m_LevelPoint, m_UserData.m_EventStageID, m_DBTran);
        }
        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_stage_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.StageAgent.AfterQueryUpgradeStageClear(m_SessionKey, m_ForClient, m_StageData, m_IsClear, m_Result, m_UserData);
        }
    }

    //==================== KNIGHT ===================
    public class CQueryCharacterUpdateEquipKnightPreset : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public int idx;
            public string slot_json;
            public bool is_enable;

            public void SetParams(long user_uid, _KnightEquipPresetData preset)
            {
                uid = user_uid;
                idx = preset.m_Index;
                slot_json = SCommon.SJson.ObjectToJson(preset.m_EquipedSlots);
                is_enable = preset.m_IsEnable;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _KnightEquipPresetData m_PresetData = new _KnightEquipPresetData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateEquipKnightPreset(long sessionKey, long userUID, _KnightEquipPresetData preset)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_PresetData = preset;
            m_Params.SetParams(m_UID, m_PresetData);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_knight_preset_update";

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
            //don't anything
        }
    }
    public class CQueryCharacterUpdateKnight : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public long itemid;
            public long horseid;
            public bool in_used;

            public void SetParams(long _uid, long _itemid, long _horseid, bool _used)
            {
                uid = _uid;
                itemid = _itemid;
                horseid = _horseid;
                in_used = _used;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();
        private _ItemData m_Item = new _ItemData();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateKnight(long sessionKey, long _useruid, _ItemData item)
        {
            m_SessionKey = sessionKey;
            m_UID = _useruid;
            m_Item = item;
            //m_Params.SetParams(m_UID, item.ItemID, item.HorseID, item.m_InUsed);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_knight_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);

            CNetManager.Instance.P2C_ReportItem(m_SessionKey, m_Item);
        }
    }

    //==================== COIN ===================
    public class CQueryCharacterUpdateCoin : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string type;
            public long value;

            public void SetParams(long user_uid, string _type, long _val)
            {
                uid = user_uid;
                type = _type;
                value = _val;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateCoin(long sessionKey, long userUID, string _type, long _val)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Params.SetParams(m_UID, _type, _val);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_coin_count_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);

            //report?
        }
    }
    public class CQueryCharacterUpdateCoinList : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_coins;

            public void SetParams(long user_uid, string _update_coins)
            {
                uid = user_uid;
                update_coins = _update_coins;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateCoinList(long sessionKey, long userUID, List<_AssetData> _updateAssets)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Params.SetParams(m_UID, SCommon.SJson.ObjectToJson(_updateAssets));
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_coin_count_list_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);

            //report?
        }
    }
    public class CQueryCharacterUpsertCoin : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string type;
            public long value;

            public void SetParams(long user_uid, string _type, long _val)
            {
                uid = user_uid;
                type = _type;
                value = _val;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpsertCoin(long sessionKey, long userUID, string _type, long _val)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Params.SetParams(m_UID, _type, _val);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_coin_upsert";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }

    //==================== GACHA ===================
    public class CQueryCharacterUpdateGacha : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_items;
            public string update_coins;
            public int gacha_id;
            public int gacha_lv;
            public long gacha_exp;
            public int gacha_rewarded;

            public void SetParams(long user_uid, string _json_update_items, string _json_update_coins, int _id, int _lv, long _exp, int _rewarded)
            {
                uid = user_uid;
                update_items = _json_update_items;
                update_coins = _json_update_coins;
                gacha_id = _id;
                gacha_lv = _lv;
                gacha_exp = _exp;
                gacha_rewarded = _rewarded;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _GachaData m_GachaData = new _GachaData();
        private CRewardInfo m_ForClient = new CRewardInfo();
        private CDBMerge m_DBTran = new CDBMerge();
        private DBParams m_Params = new DBParams();
        private int m_GachaTID = 0;


        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateGacha(long sessionKey, long userUID, _GachaData gachaData, CRewardInfo forClient, CDBMerge dbtran, int gachatid)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_GachaData = gachaData;
            m_ForClient = forClient;
            m_DBTran = dbtran;
            m_GachaTID = gachatid;

            string update_item = SCommon.SJson.ObjectToJson(dbtran.m_UpdateItemList);
            string update_coin = SCommon.SJson.ObjectToJson(dbtran.m_UpdateCoinList);
            m_Params.SetParams(m_UID, update_item, update_coin, m_GachaData.m_GroupID, m_GachaData.m_Level, m_GachaData.m_Exp, m_GachaData.m_Rewarded);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_gacha";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.ItemAgent.AfterQueryUpdateItemGacha(m_SessionKey, m_GachaData, m_ForClient, m_GachaTID, m_Result);
        }
    }
    public class CQueryCharacterUpdateGachaLevelReward : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_items;
            public string update_coins;
            public int gacha_id;
            public int gacha_lv;
            public long gacha_exp;
            public int gacha_rewarded;

            public void SetParams(long user_uid, string _json_update_items, string _json_update_coins, int _id, int _lv, long _exp, int _rewarded)
            {
                uid = user_uid;
                update_items = _json_update_items;
                update_coins = _json_update_coins;
                gacha_id = _id;
                gacha_lv = _lv;
                gacha_exp = _exp;
                gacha_rewarded = _rewarded;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _GachaData m_GachaData = new _GachaData();
        private CRewardInfo m_ForClient = new CRewardInfo();
        private CDBMerge m_DBTran = new CDBMerge();
        private DBParams m_Params = new DBParams();


        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateGachaLevelReward(long sessionKey, long userUID, _GachaData gachaData, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_GachaData = gachaData;
            m_ForClient = forClient;
            m_DBTran = dbtran;

            string update_item = SCommon.SJson.ObjectToJson(dbtran.m_UpdateItemList);
            string update_coin = SCommon.SJson.ObjectToJson(dbtran.m_UpdateCoinList);
            m_Params.SetParams(m_UID, update_item, update_coin, m_GachaData.m_GroupID, m_GachaData.m_Level, m_GachaData.m_Exp, m_GachaData.m_Rewarded);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_gacha_level_rewared";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);

            user.AfterQueryUpdateItemLevelGacha(m_SessionKey, m_GachaData, m_ForClient, m_Result);
        }
    }

    //==================== ITEM ===================
    public class CQueryCharacterUpdateItemBreakThrough : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_items;
            public string update_coins;
            public long item_uid;
            public string rand_option;

            public void SetParams(long user_uid, string _json_update_items, string _json_update_coins, long _id, string _roption)
            {
                uid = user_uid;
                update_items = _json_update_items;
                update_coins = _json_update_coins;
                item_uid = _id;
                rand_option = _roption;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _ItemData m_Item = new _ItemData();
        private CDBMerge m_DBTran = new CDBMerge();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateItemBreakThrough(long sessionKey, long userUID, _ItemData item, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_DBTran = dbtran;
            m_Item = item;
            string update_item = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateItemList);
            string update_coin = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateCoinList);
            string option = SCommon.SJson.ObjectToJson(m_Item.RandomOption);
            m_Params.SetParams(m_UID, update_item, update_coin, m_Item.ItemID, option);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_breakthrough";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            CNetManager.Instance.P2C_ResultItemBreakthrough(m_SessionKey, m_Item, m_Result);
        }
    }
    public class CQueryCharacterUpdateItemRandomOption : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_items;
            public string update_coins;
            public long item_uid;
            public string rand_option;

            public void SetParams(long user_uid, string _json_update_items, string _json_update_coins, long _id, string _roption)
            {
                uid = user_uid;
                update_items = _json_update_items;
                update_coins = _json_update_coins;
                item_uid = _id;
                rand_option = _roption;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _ItemData m_Item = new _ItemData();
        private CDBMerge m_DBTran = new CDBMerge();
        private DBParams m_Params = new DBParams();
        private Packet_C2P.Protocol m_Protocol;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateItemRandomOption(long sessionKey, long userUID, _ItemData item, CDBMerge dbtran, Packet_C2P.Protocol protocol)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_DBTran = dbtran;
            m_Item = item;
            m_Protocol = protocol;
            string update_item = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateItemList);
            string update_coin = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateCoinList);
            string option = SCommon.SJson.ObjectToJson(m_Item.RandomOption);
            m_Params.SetParams(m_UID, update_item, update_coin, m_Item.ItemID, option);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_rand_option_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            if (m_Protocol == Packet_C2P.Protocol.P2C_ResultRandomOptionChange)
                CNetManager.Instance.P2C_ResultRandomOptionChange(m_SessionKey, m_Item, m_Result);
            else if (m_Protocol == Packet_C2P.Protocol.P2C_ResultRandomOptionLock)
                CNetManager.Instance.P2C_ResultRandomOptionLock(m_SessionKey, m_Item, m_Result);
        }
    }
    public class CQueryCharacterUpdateItemConsume : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_items;
            public string update_coins;
            public string delete_items;


            public void SetParams(long user_uid, string _json_update_items, string _json_update_coins, string _json_delete_items)
            {
                uid = user_uid;
                update_items = _json_update_items;
                update_coins = _json_update_coins;
                delete_items = _json_delete_items;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _ItemData m_Item = new _ItemData();
        private CDBMerge m_DBTran = new CDBMerge();
        private CRewardInfo m_ForClient = new CRewardInfo();
        private DBParams m_Params = new DBParams();
        
        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateItemConsume(long sessionKey, long userUID, _ItemData item, CDBMerge dbtran, CRewardInfo forClient)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_DBTran = dbtran;
            m_Item = SCopy<_ItemData>.DeepCopy(item);
            string update_item = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateItemList);
            string update_coin = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateCoinList);
            string delete_item = SCommon.SJson.ObjectToJson(m_DBTran.m_DeleteItemlist);
            m_Params.SetParams(m_UID, update_item, update_coin, delete_item);
            m_ForClient = forClient;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_consume";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            CNetManager.Instance.P2C_ResultItemConsume(m_SessionKey, m_Item, m_ForClient.GetList(), m_Result);
        }
    }
    public class CQueryCharacterUpdateItemSell : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_coins;
            public string update_items;

            public void SetParams(long userid, string _json_update_coins, string _json_update_items)
            {
                uid = userid;
                update_coins = _json_update_coins;
                update_items = _json_update_items;
            }
        }

        //input
        private long m_SessionKey = 0;
        private _ItemData m_Item = new _ItemData();
        private CRewardInfo m_ForClient = new CRewardInfo();
        private CDBMerge m_DBtran = new CDBMerge();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateItemSell(long sessionKey, long uid, _ItemData item, CRewardInfo forclient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_Item = item;
            m_ForClient = forclient;
            m_DBtran = dbtran;
            string update_coins = SCommon.SJson.ObjectToJson(dbtran.m_UpdateCoinList);
            string update_items = SCommon.SJson.ObjectToJson(dbtran.m_UpdateItemList);
            m_Params.SetParams(uid, update_coins, update_items);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_sell";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.ItemAgent.AfterQueryUpdateItemSell(m_SessionKey, m_Item, m_ForClient, m_Result);

        }
    }
    public class CQueryCharacterUpdateItemLevel : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public long itemid;
            public string tableid;
            public int level;
            public long count;
            public string update_coins;
            public string update_items;

            public void SetParams(long user_uid, long _itemid, string _tableid, int _level, long _count, string _update_coins, string _update_items)
            {
                uid = user_uid;
                itemid = _itemid;
                tableid = _tableid;
                level = _level;
                count = _count;
                update_coins = _update_coins;
                update_items = _update_items;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _SkillEquipPresetData m_PresetData = new _SkillEquipPresetData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateItemLevel(long sessionKey, long userUID, _ItemData item, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            string update_coins = SCommon.SJson.ObjectToJson(dbtran.m_UpdateCoinList);
            string update_items = SCommon.SJson.ObjectToJson(dbtran.m_UpdateItemList);
            m_Params.SetParams(m_UID, item.ItemID, item.TableID, item.Level, item.Count, update_coins, update_items);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_level_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }
    public class CQueryCharacterUpdateItemCountList : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_items;

            public void SetParams(long user_uid, List<_ItemData> items)
            {
                uid = user_uid;
                update_items = SCommon.SJson.ObjectToJson(items);
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateItemCountList(long sessionKey, long userUID, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Params.SetParams(m_UID, dbtran.m_UpdateItemList);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_count_list_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }
    public class CQueryCharacterUpdateItemCount : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public long itemid;
            public string tableid;
            public int level;
            public long count;

            public void SetParams(long user_uid, _ItemData item)
            {
                uid = user_uid;
                itemid = item.ItemID;
                tableid = item.TableID;
                level = item.Level;
                count = item.Count;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateItemCount(long sessionKey, long userUID, _ItemData item)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Params.SetParams(m_UID, item);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_count_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }

    public class CQueryCharacterItemKnightUpgrade : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public long itemid;
            public string tableid;
            public int level;
            public long count;
            public string rand_option;
            public string update_coins;
            public string update_items;

            public void SetParams(long user_uid, _ItemData item, string _update_coins, string _update_items)
            {
                uid = user_uid;
                itemid = item.ItemID;
                tableid = item.TableID;
                level = item.Level;
                count = item.Count;
                rand_option = SJson.ObjectToJson(item.RandomOption); 
                update_coins = _update_coins;
                update_items = _update_items;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _SkillEquipPresetData m_PresetData = new _SkillEquipPresetData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterItemKnightUpgrade(long sessionKey, long userUID, _ItemData item, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            string update_coins = SCommon.SJson.ObjectToJson(dbtran.m_UpdateCoinList);
            string update_items = SCommon.SJson.ObjectToJson(dbtran.m_UpdateItemList);
            m_Params.SetParams(m_UID, item, update_coins, update_items);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_item_knight_upgrade";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }

    public class CQueryCharacterUpdateEquipPreset : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public int idx;
            public string slot_json;
            public bool is_enable;

            public void SetParams(long user_uid, _ItemEquipPresetData preset)
            {
                uid = user_uid;
                idx = preset.m_Index;
                slot_json = SCommon.SJson.ObjectToJson(preset.m_EquipedSlots);
                is_enable = preset.m_IsEnable;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _ItemEquipPresetData m_PresetData = new _ItemEquipPresetData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateEquipPreset(long sessionKey, long userUID, _ItemEquipPresetData preset)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_PresetData = preset;
            m_Params.SetParams(m_UID, m_PresetData);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_equip_prest_update";

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
            //don't anything
        }
    }

    //==================== SKILL ===================
    public class CQueryCharacterUpdateSkillOpen : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public int group_id;
            public int tid;
            public int level;
            public bool is_learend;

            public void SetParams(long userid, _SkillData skillData)
            {
                uid = userid;
                group_id = skillData.GroupID;
                tid = skillData.TID;
                level = skillData.Lv;
                is_learend = skillData.IsLearend;
            }
        }

        //input
        private long m_SessionKey = 0;
        private _SkillData m_SkillData = new _SkillData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateSkillOpen(long sessionKey, long uid, _SkillData skillData)
        {
            m_SessionKey = sessionKey;
            m_SkillData = skillData;
            m_Params.SetParams(uid, m_SkillData);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_skill_open";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.SkillAgent.AfterQuerySkillOpen(m_SessionKey, m_SkillData, m_Result);
        }
    }
    public class CQueryCharacterUpdateSkillLevel : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public int group_id;
            public int tid;
            public int level;
            public string update_coins;
            public string update_items;
            public string delete_items;

            public void SetParams(long userid, _SkillData skillData, string json_update_coins, string json_update_items, string json_delete_items)
            {
                uid = userid;
                group_id = skillData.GroupID;
                tid = skillData.TID;
                level = skillData.Lv;
                update_coins = json_update_coins;
                update_items = json_update_items;
                delete_items = json_delete_items;
            }
        }

        //input
        private long m_SessionKey = 0;
        private CDBMerge m_DBTran = new CDBMerge();
        private _SkillData m_SkillData = new _SkillData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateSkillLevel(long sessionKey, long uid, _SkillData skillData, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_SkillData = skillData;
            string update_coins = SCommon.SJson.ObjectToJson(dbtran.m_UpdateCoinList);
            string update_items = SCommon.SJson.ObjectToJson(dbtran.m_UpdateItemList);
            string delete_items = SCommon.SJson.ObjectToJson(dbtran.m_DeleteItemlist);
            m_Params.SetParams(uid, m_SkillData, update_coins, update_items, delete_items);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_skill_level_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.SkillAgent.AfterQuerySkillLevelUp(m_SessionKey, m_SkillData, m_Result);
        }
    }
    public class CQueryCharacterUpdateEquipSkillPreset : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public int idx;
            public string slot_json;
            public bool is_enable;

            public void SetParams(long user_uid, _SkillEquipPresetData preset)
            {
                uid = user_uid;
                idx = preset.m_Index;
                slot_json = SCommon.SJson.ObjectToJson(preset.m_EquipedSlots);
                is_enable = preset.m_IsEnable;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _SkillEquipPresetData m_PresetData = new _SkillEquipPresetData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterUpdateEquipSkillPreset(long sessionKey, long userUID, _SkillEquipPresetData preset)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_PresetData = preset;
            m_Params.SetParams(m_UID, m_PresetData);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_skill_preset_update";

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
            //don't anything
        }
    }

    public class CQueryCharacterStageSkillUpdate : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public int type;
            public string slot_json;
            
            public void SetParams(long user_uid, _StageSkillData data)
            {
                uid = user_uid;
                type = (int)data.m_Type;
                slot_json = SCommon.SJson.ObjectToJson(data.m_EquipedSlots);
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _StageSkillData m_StageSkillData = new _StageSkillData();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterStageSkillUpdate(long sessionKey, long userUID, _StageSkillData stageSkillData)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_StageSkillData = stageSkillData;
            m_Params.SetParams(m_UID, stageSkillData);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_stage_skill_update";

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
            //don't anything
        }
    }

    //==================== POST ====================
    public class CQueryCharacterPostBoxOpen : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;

            public void SetParams(long user_uid)
            {
                uid = user_uid;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();
        private List<_PostData> m_Posts = new List<_PostData>();
        private bool m_ClientFlag = false;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterPostBoxOpen(long sessionKey, long userUID, bool clientFlag)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Params.SetParams(m_UID);
            m_ClientFlag = clientFlag;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_post_load";

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
                        if (!string.IsNullOrEmpty(str_rewards))
                            post.Rewards = SCommon.SJson.JsonToObject<List<_AssetData>>(str_rewards);

                        m_Posts.Add(post);
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.PostAgent.AfterQueryPostBoxOpen(m_SessionKey, m_Posts, m_ClientFlag, m_Result);
        }
    }
    public class CQueryCharacterPostRead : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string read_posts;
            
            public void SetParams(long user_uid, string postids)
            {
                uid = user_uid;
                read_posts = postids;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();
        private List<_PostData> m_ReadPostDatas = new List<_PostData>();
        
        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterPostRead(long sessionKey, long userUID, List<_PostData> postDatas, List<long> postids)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_ReadPostDatas = postDatas;
            string update_post = SCommon.SJson.ObjectToJson(postids);
            m_Params.SetParams(m_UID, update_post);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_post_read";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.PostAgent.AfterQueryPostRead(m_SessionKey, m_ReadPostDatas, m_Result);
        }
    }
    public class CQueryCharacterPostReward : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string read_posts;
            public string update_items;
            public string update_coins;

            public void SetParams(long user_uid, string postids, string _json_update_itmes, string _json_update_coins)
            {
                uid = user_uid;
                read_posts = postids;
                update_items = _json_update_itmes;
                update_coins = _json_update_coins;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();
        private List<_PostData> m_ReadPostDatas = new List<_PostData>();
        private CRewardInfo m_ForClient = new CRewardInfo();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterPostReward(long sessionKey, long userUID, List<_PostData> postDatas, List<long> postids, CDBMerge dbtran, CRewardInfo forClient)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_ReadPostDatas = postDatas;
            string update_post = SCommon.SJson.ObjectToJson(postids);
            string update_items = SCommon.SJson.ObjectToJson(dbtran.m_UpdateItemList);
            string update_coins = SCommon.SJson.ObjectToJson(dbtran.m_UpdateCoinList);
            m_Params.SetParams(m_UID, update_post, update_items, update_coins);
            m_ForClient = forClient;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_post_reward";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.PostAgent.AfterQueryPostReward(m_SessionKey, m_ReadPostDatas, m_ForClient, m_Result);
        }
    }
    public class CQueryCharacterPostRemove : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string remove_posts;
            
            public void SetParams(long user_uid, string postids)
            {
                uid = user_uid;
                remove_posts = postids;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();
        private List<_PostData> m_RemovePostDatas = new List<_PostData>();
        
        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQueryCharacterPostRemove(long sessionKey, long userUID, List<_PostData> postDatas, List<long> dbRemoveIDs)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_RemovePostDatas = postDatas;
            string remove_posts = SCommon.SJson.ObjectToJson(dbRemoveIDs);
            m_Params.SetParams(m_UID, remove_posts);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_post_remove";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            //user.PostAgent.AfterQueryPostRemove(m_SessionKey, m_RemovePostDatas, m_Result);
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

            public void SetParams(_PostData post)
            {
                post_id = post.ID;
                type = (ushort)post.Type;
                title = post.Title;
                msg = post.Msg;
                begin_time = post.beginTime;
                expire_time = post.expireTime;
                reward = SCommon.SJson.ObjectToJson(post.Rewards);
            }
        }

        //input
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQuerySystemUpsertPost(_PostData postData)
        {
            m_Params.SetParams(postData);
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
            //notting
        }
    }

    //==================== COUPON ===================
    public class CQueryCharacterVaildCoupon : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string coupon_id;
            
            public void SetParams(long userid, string id)
            {
                uid = userid;
                coupon_id = id;
            }
        }

        //input
        private long m_SessionKey = 0;
        private long m_UserID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private string m_CouponID = string.Empty;
        private List<_AssetData> m_Rewards = new List<_AssetData>();

        public CQueryCharacterVaildCoupon(long sessionKey, long uid, string couponID)
        {
            m_SessionKey = sessionKey;
            m_UserID = uid;
            m_Params.SetParams(m_UserID, couponID);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_coupon_vaild";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    switch (m_Data.GetErrorNumber())
                    {
                        case 1:
                            {
                                m_Result = Packet_Result.Result.InValidCoupon;
                            }
                            break;
                        default:
                            {
                                m_Data.PrintErrorLog(sqlCmd.CommandText);
                                m_Result = Packet_Result.Result.DBError;
                            }
                            break;
                    }
                }
                else
                {
                    m_Data.GetData(0, "coupon_id", ref m_CouponID);
                    string str_rewards = "";
                    m_Data.GetData(0, "reward", ref str_rewards);
                    if (!string.IsNullOrEmpty(str_rewards))
                        m_Rewards = SCommon.SJson.JsonToObject<List<_AssetData>>(str_rewards);
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if(m_Result == Packet_Result.Result.DBError)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            if (m_Result == Packet_Result.Result.InValidCoupon)
            {
                CNetManager.Instance.P2C_ResultUseCoupon(m_SessionKey, new List<_AssetData>(), m_Result);
                return;
            }

            user.CouponAgent.AfterQueryVaildCoupon(m_CouponID, m_Rewards);
        }
    }
    public class CQueryCharacterUseCoupon : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string coupon_id;
            public string used_coupon_json;
            public string update_items;
            public string update_coins;

            public void SetParams(long userid, string id, string used_coupon, string items, string coins)
            {
                uid = userid;
                coupon_id = id;
                used_coupon_json = used_coupon;
                update_items = items; 
                update_coins = coins;
            }
        }

        //input
        private long m_SessionKey = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private CRewardInfo m_forClient = new CRewardInfo();

        public CQueryCharacterUseCoupon(long sessionKey, long uid, string couponID, string used_coupons, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_forClient = forClient;
            string update_itmes = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            string update_coins = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
            m_Params.SetParams(uid, couponID, used_coupons, update_itmes, update_coins);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_coupon_use";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, m_Params);

                // 실행. SqlDataAdapter 사용시 SqlConnection Open이 별도로 필요 없음.
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = sqlCmd;
                sqlAdapter.Fill(m_Data.m_dataSet);

                if (!m_Data.IsSuccess())
                {
                    switch (m_Data.GetErrorNumber())
                    {
                        case 1:
                            {
                                m_Result = Packet_Result.Result.InValidCoupon;
                            }
                            break;
                        default:
                            {
                                m_Data.PrintErrorLog(sqlCmd.CommandText);
                                m_Result = Packet_Result.Result.DBError;
                            }
                            break;
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            switch (m_Result)
            {
                case Result.DBError:
                    CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                    return;
                case Result.InValidCoupon:
                    CNetManager.Instance.P2C_ResultUseCoupon(m_SessionKey, new List<_AssetData>(), m_Result);
                    return;
            }

            user.CouponAgent.AfterQueryUseCoupon(m_SessionKey, m_forClient, m_Result);
        }
    }
    public class CQuerySystemCouponUpsert : IMsSqlQuery
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
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        public CQuerySystemCouponUpsert(string couponid, int count, int level, long begin, long exp, List<_AssetData> rewards)
        {
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
                sqlCmd.CommandText = "sp_system_coupon_upsert";

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
            //noting
        }
    }

    //==================== RANK =====================
    public class CQueryCharacterRankRewardRewarded : IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public string update_items;
            public string update_coins;
            public ushort rank_type;
            public ushort reward_state;
            public int rank_val;
            public long exp_time;

            public void SetParams(long user_id, string json_items, string json_coins, ushort type, ushort state, int val, long exptime)
            {
                uid = user_id;
                update_items = json_items;
                update_coins = json_coins;
                rank_type = type;
                reward_state = state;
                rank_val = val;
                exp_time = exptime;
            }
        }

        //input
        private long m_SessionKey = -1;
        private _RankReward m_RankReward = new _RankReward();
        private long m_UID = 0;
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        private CRewardInfo m_forClient = new CRewardInfo();
        private CDBMerge m_DBTran = new CDBMerge();

        public CQueryCharacterRankRewardRewarded(long sessionKey, long uid, _RankReward rewardData, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_RankReward = SCopy<_RankReward>.DeepCopy(rewardData);
            m_forClient = forClient;
            m_DBTran = dbtran;
            m_UID = uid;
            string update_items = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateItemList);
            string update_coins = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateCoinList);
            m_Params.SetParams(uid, update_items, update_coins, (ushort)m_RankReward.m_Type, (ushort)m_RankReward.m_State, m_RankReward.m_RankValue, m_RankReward.m_ExpTime);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_rank_reward_rewarded";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result == Packet_Result.Result.DBError)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.AfterQueryRankReward(m_SessionKey, m_RankReward, m_forClient, m_DBTran, m_Result);
        }
    }
    public class CQueryCharacterRankRewardUpdate: IMsSqlQuery
    {
        struct DBParams
        {
            public long uid;
            public ushort rank_type;
            public ushort reward_state;
            public int rank_val;
            public long exp_time;

            public void SetParams(long user_id, ushort type, ushort state, int val, long exptime)
            {
                uid = user_id;
                rank_type = state;
                reward_state = state;
                rank_val = val;
                exp_time = exptime;
            }
        }

        //input
        private long m_SessionKey = -1;
        private _RankReward m_RankReward = new _RankReward();
        private DBParams m_Params = new DBParams();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();
        
        public CQueryCharacterRankRewardUpdate(long sessionKey, long uid, _RankReward rewardData)
        {
            m_SessionKey = sessionKey;
            m_RankReward = SCopy<_RankReward>.DeepCopy(rewardData);
            m_Params.SetParams(uid, (ushort)m_RankReward.m_Type, (ushort)m_RankReward.m_State, m_RankReward.m_RankValue, m_RankReward.m_ExpTime);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_rank_reward_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result == Packet_Result.Result.DBError)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }
        }
    }

    //==================== QUEST_MAIN ==================
    public class CQueryCharacterQuestMainUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _Mission m_Mission = new _Mission();
        private CDefine.Modify m_Modify = CDefine.Modify.Max;
        private bool m_bSend = false;


        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long db_uid;
            public int db_mission_id;
            public int db_mission_idx;
            public long db_mission_val;
            public ushort db_mission_state;
        }

        public CQueryCharacterQuestMainUpdate(long sessionKey, long userUID, _Mission mission)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Mission = SCopy<_Mission>.DeepCopy(mission);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_main_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    db_uid = m_UID,
                    db_mission_id = m_Mission.ID,
                    db_mission_idx = m_Mission.Idx,
                    db_mission_val = m_Mission.Val,
                    db_mission_state = (ushort)m_Mission.State
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }
    public class CQueryCharacterQuestMainReward : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private int m_RemoveID = 0;
        private _Mission m_Mission = new _Mission();
        private CDBMerge m_DBTran = new CDBMerge();
        private CRewardInfo m_ForClient = new CRewardInfo();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long uid;
            public int remove_mission_id;
            public int mission_id;
            public int mission_idx;
            public long mission_val;
            public ushort mission_state;
            public string update_coin;
            public string update_item;

            public void SetParma(long _uid, int _removeID, int _mission_id, int _mission_idx, long _mission_val, ushort _mission_state,
                CDBMerge dbtran)
            {
                uid = _uid;
                remove_mission_id = _removeID;
                mission_id = _mission_id;
                mission_idx = _mission_idx;
                mission_val = _mission_val;
                mission_state = _mission_state;
                update_coin = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_item = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        public CQueryCharacterQuestMainReward(long sessionKey, long userUID, int removeID, _Mission mission, CDBMerge dbtran, CRewardInfo forClient)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_RemoveID = removeID;
            m_Mission = mission;
            m_DBTran = dbtran;
            m_ForClient = forClient;
            m_Params.SetParma(userUID, removeID, mission.ID, mission.Idx, mission.Val, (ushort)mission.State, dbtran);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_main_rewarded";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.QuestAgent.AfterQueryRewardMainMission(m_SessionKey, m_Mission, m_ForClient, m_DBTran, m_Result);
        }
    }

    //==================== QEUST_REPEAT ===================
    public class CQueryCharacterQuestRepeatUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private List<_Mission> m_Missions = new List<_Mission>();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long db_uid;
            public string db_mission_json;
        }

        public CQueryCharacterQuestRepeatUpdate(long sessionKey, long userUID, List<_Mission> mission)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Missions = mission;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_repeat_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    db_uid = m_UID,
                    db_mission_json = SCommon.SJson.ObjectToJson(m_Missions)
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }
    public class CQueryCharacterQuestRepeatReward : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private List<_Mission> m_Missions = new List<_Mission>();

        private CDBMerge m_DBTran = new CDBMerge();
        private CRewardInfo m_ForClient = new CRewardInfo();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long db_uid;
            public string db_mission_json;
            public string update_coin;
            public string update_item;

            public void SetParma(long uid, string mission_json, CDBMerge dbtran)
            {
                db_uid = uid;
                db_mission_json = mission_json;
                update_coin = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_item = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        public CQueryCharacterQuestRepeatReward(long sessionKey, long userUID, List<_Mission> missoins, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Missions = missoins;
            m_DBTran = SCopy<CDBMerge>.DeepCopy(dbtran);
            m_ForClient = forClient;
            m_Params.SetParma(userUID, SJson.ObjectToJson(m_Missions), m_DBTran);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_repeat_rewarded";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.QuestAgent.AfterQueryRewardRepeatMission(m_SessionKey, m_Missions, m_ForClient, m_DBTran, m_Result);
        }
    }


    //==================== QEUST_DAILY ===================
    public class CQueryCharacterQuestDailyUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private List<_Mission> m_Missions = new List<_Mission>();
        private DateTime m_ExpTime = DateTime.MinValue;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long db_uid;
            public string db_mission_json;
            public DateTime db_exp_time;
        }

        public CQueryCharacterQuestDailyUpdate(long sessionKey, long userUID, List<_Mission> mission, DateTime exp_time)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Missions = mission;
            m_ExpTime = exp_time;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_daily_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    db_uid = m_UID,
                    db_mission_json = SCommon.SJson.ObjectToJson(m_Missions),
                    db_exp_time = m_ExpTime
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }
    public class CQueryCharacterQuestDailyReward : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private List<_Mission> m_Missions = new List<_Mission>();

        private CDBMerge m_DBTran = new CDBMerge();
        private CRewardInfo m_ForClient = new CRewardInfo();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long db_uid;
            public string db_mission_json;
            public DateTime db_exp_time;
            public string update_coin;
            public string update_item;

            public void SetParma(long uid, string mission_json, DateTime exp_time, CDBMerge dbtran)
            {
                db_uid = uid;
                db_mission_json = mission_json;
                db_exp_time = exp_time;
                update_coin = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_item = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        public CQueryCharacterQuestDailyReward(long sessionKey, long userUID, List<_Mission> missoins, DateTime exp_time, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Missions = missoins;
            m_DBTran = SCopy<CDBMerge>.DeepCopy(dbtran);
            m_ForClient = forClient;
            m_Params.SetParma(userUID, SJson.ObjectToJson(m_Missions), exp_time, m_DBTran);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_daily_rewarded";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.QuestAgent.AfterQueryRewardDailyMissin(m_SessionKey, m_Missions, m_ForClient, m_DBTran, m_Result);
        }
    }


    //==================== QEUST_CHECKIN ===================
    public class CQueryCharacterQuestCheckInUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private string m_QuestID = string.Empty;
        private _Mission m_Mission = new _Mission();
        private DateTime m_ExpTime = DateTime.MinValue;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long db_uid;
            public string db_quest_id;
            public int db_mission_id;
            public int db_mission_idx;
            public long db_mission_val;
            public ushort db_mission_state;
            public DateTime db_exp_time;
        }

        public CQueryCharacterQuestCheckInUpdate(long sessionKey, long userUID, string questID, _Mission mission, DateTime exp_time)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_QuestID = questID;
            m_Mission = mission;
            m_ExpTime = exp_time;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_checkin_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    db_uid = m_UID,
                    db_quest_id = m_QuestID,
                    db_mission_id = m_Mission.ID,
                    db_mission_idx = m_Mission.Idx,
                    db_mission_val = m_Mission.Val,
                    db_mission_state = (ushort)m_Mission.State,
                    db_exp_time = m_ExpTime
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }
    public class CQueryCharacterQuestCheckInReward : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private string m_QuestID = string.Empty;
        private _Mission m_Mission = new _Mission();
        private DateTime m_ExpTime = DateTime.MinValue;
        private CRewardInfo m_forClient = new CRewardInfo();
        private CDBMerge m_DBTran = new CDBMerge();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();



        struct DBParams
        {
            public long db_uid;
            public string db_quest_id;
            public int db_mission_id;
            public int db_mission_idx;
            public long db_mission_val;
            public ushort db_mission_state;
            public DateTime db_exp_time;
            public string db_update_coin;
            public string db_update_item;
        }

        public CQueryCharacterQuestCheckInReward(long sessionKey, long userUID, string questID, _Mission mission, DateTime exp_time, CRewardInfo forClient, CDBMerge dBTran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_QuestID = questID;
            m_Mission = mission;
            m_ExpTime = exp_time;
            m_forClient = forClient;
            m_DBTran = dBTran;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_checkIn_rewarded";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    db_uid = m_UID,
                    db_quest_id = m_QuestID,
                    db_mission_id = m_Mission.ID,
                    db_mission_idx = m_Mission.Idx,
                    db_mission_val = m_Mission.Val,
                    db_mission_state = (ushort)m_Mission.State,
                    db_exp_time = m_ExpTime,
                    db_update_coin = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateCoinList),
                    db_update_item = SCommon.SJson.ObjectToJson(m_DBTran.m_UpdateItemList)
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.QuestAgent.AfterQueryRewardCheckInMission(m_SessionKey, m_Mission, m_forClient, m_DBTran, m_Result);
        }
    }


    //==================== QEUST_PASS ===================
    public class CQueryCharacterQuestPassUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private List<_Mission> m_Missions = new List<_Mission>();
        private string m_QuestID = string.Empty;
        private bool m_PassActive = false;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long db_uid;
            public string db_mission_json;
            public string db_quest_id;
            public bool db_pass_active;
        }

        public CQueryCharacterQuestPassUpdate(long sessionKey, long userUID, string questID, bool active,List<_Mission> mission)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Missions = mission;
            m_QuestID = questID;
            m_PassActive = active;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_pass_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    db_uid = m_UID,
                    db_mission_json = SCommon.SJson.ObjectToJson(m_Missions),
                    db_quest_id = m_QuestID,
                    db_pass_active = m_PassActive
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }
    public class CQueryCharacterQuestPassReward : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private List<_Mission> m_Missions = new List<_Mission>();
        private string m_QuestID = string.Empty;

        private CDBMerge m_DBTran = new CDBMerge();
        private CRewardInfo m_ForClient = new CRewardInfo();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long db_uid;
            public string db_quest_id;
            public string db_mission_json;
            public bool db_pass_active;
            public string update_coin;
            public string update_item;


            public void SetParma(long uid, string quest_id, string mission_json, bool active, CDBMerge dbtran)
            {
                db_uid = uid;
                db_quest_id = quest_id;
                db_mission_json = mission_json;
                db_pass_active = active;
                update_coin = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_item = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        public CQueryCharacterQuestPassReward(long sessionKey, long userUID, string quest_id , bool active , List<_Mission> missoins, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Missions = missoins;
            m_DBTran = SCopy<CDBMerge>.DeepCopy(dbtran);
            m_ForClient = forClient;
            m_QuestID = quest_id;
            m_Params.SetParma(userUID, quest_id, SJson.ObjectToJson(m_Missions), active, m_DBTran);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_pass_rewarded";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.QuestAgent.AfterQueryRewardPassMission(m_SessionKey, m_Missions, m_ForClient, m_DBTran, m_Result);
        }
    }
  
    //==================== RELIC ===================
    public class CQueryCharacterRelicEnchant : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _RelicData m_Relic = new _RelicData();
        private CDBMerge m_DBTran = new CDBMerge();
        

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long uid;
            public int group_id;
            public int lv;
            public int bonus_prob;
            public string update_coins;
            public string update_items;

            public void SetParma(long user_uid, int groupid, int level, int prob, CDBMerge dbtran)
            {
                uid = user_uid;
                group_id = groupid;
                lv = level;
                bonus_prob = prob;
                update_coins = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_items = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        public CQueryCharacterRelicEnchant(long sessionKey, long userUID, _RelicData relicData, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Relic = relicData;
            m_DBTran = SCopy<CDBMerge>.DeepCopy(dbtran);
            m_Params.SetParma(userUID, relicData.m_GroupID, relicData.m_Level, relicData.m_BonusProb, m_DBTran);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_relic_enchant";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.RelicAgent.AfterQueryRelicEnchant(m_SessionKey, m_Relic, m_Result);
        }
    }


    //========================== Shop IAP ============================
    public class CQueryCharacterShopIAPUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private CDBMerge m_DBTran = new CDBMerge();
        private _ShopData m_ShopData = new _ShopData();
        private _IAPReceipt m_IAPReceipt = new _IAPReceipt();
        private _PostData m_PostData = new _PostData();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long uid;
            public ushort store_type;
            public string transaction_id;
            public string product_id;

            public long record_id;
            public int bought_cnt;
            public long reset_time;

            public ushort mail_type;
            public long mail_uid;
            public string mail_title;
            public string mail_msg;
            public long mail_exp_time;
            public string mail_rewards;
	
            public void SetParma(long user_uid, ushort storetype, string tran_id, string productID, long recordid, int boughtCnt, long shopResetTime, 
                ushort mailType, long mailUID, string mailTitle, string mailMsg, long mailExpTime, string mailRewards)
            {
                uid = user_uid;
                store_type = storetype;
                transaction_id = tran_id;
                product_id = productID;
                record_id = recordid;
                bought_cnt = boughtCnt;
                reset_time = shopResetTime;
                mail_type = mailType;
                mail_uid = mailUID;
                mail_title = mailTitle;
                mail_msg = mailMsg;
                mail_exp_time = mailExpTime;
                mail_rewards = mailRewards;
            }
        }

        public CQueryCharacterShopIAPUpdate(long sessionKey, long userUID, _ShopData shopData, _IAPReceipt recieptData, _PostData postData)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_ShopData = shopData;
            m_IAPReceipt = recieptData;
            m_PostData = postData;
            m_Params.SetParma(userUID, (ushort)recieptData.m_StoreType, recieptData.m_OrderID, recieptData.m_ProductID, shopData.m_ShopID, shopData.m_LimitCount,
                shopData.m_ResetDate, (ushort)postData.Type, postData.ID, postData.Title, postData.Msg, postData.expireTime, SCommon.SJson.ObjectToJson(postData.Rewards));
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_user_shop_iap_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }

            user.ShopAgent.AfterQueryIAP(m_SessionKey, m_ShopData, m_IAPReceipt, m_PostData, m_Result);
        }
    }


    //========================== Ads Buff ============================
    public class CQueryCharacterAdsBuffUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _AdsBUffData m_AdsBuff = new _AdsBUffData();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long db_uid;
            public int db_buff_id;
            public int db_buff_lv;
            public int db_buff_exp;
            public long db_buff_exp_time;
            public long db_watch_exp_time;
            public int db_watch_count;

            public void SetParma(long uid, _AdsBUffData buffData)
            {
                db_uid = uid;
                db_buff_id = buffData.m_BuffID;
                db_buff_lv = buffData.m_BuffLv;
                db_buff_exp = buffData.m_BuffExp;
                db_buff_exp_time = buffData.m_BuffExpTime;
                db_watch_exp_time = buffData.m_WatchAdsDailyExpTime;
                db_watch_count = buffData.m_WatchAdsDailyCount;
            }
        }

        public CQueryCharacterAdsBuffUpdate(long sessionKey, long userUID, _AdsBUffData buffdata)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_AdsBuff = SCopy<_AdsBUffData>.DeepCopy(buffdata);
            m_Params.SetParma(userUID, m_AdsBuff);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_ads_buff_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
            {
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
                return;
            }
        }
    }


    //=============================Event==================================
    public class CQueryCharacterEventShopUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long uid;
            public long event_uid;
            public int shop_id;
            public int buy_cnt;
            public string update_items;
            public string update_coins;

            public void SetParma(long _uid, _EventShopData shopData, string items, string coins)
            {
                uid = _uid;
                event_uid = shopData.m_EventUID;
                shop_id = shopData.m_EventShopID;
                buy_cnt = shopData.m_BuyCount;
                update_items = items;
                update_coins = coins;
            }
        }

        public CQueryCharacterEventShopUpdate(long sessionKey, long userUID, _EventShopData shopData, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Params.SetParma(userUID, shopData, SJson.ObjectToJson(dbtran.m_UpdateItemList), SJson.ObjectToJson(dbtran.m_UpdateCoinList));
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_event_shop_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            //nothing
        }
    }


    public class CQueryCharacterEventUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _EventData m_EventData = new _EventData();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long uid;
            public long event_uid;
            public int event_id;
            public long start_date;
            public long end_date;
            public string coin_type;

            public void SetParma(long _uid, _EventData eventData, CDefine.CoinType type)
            {
                uid = _uid;
                event_uid = eventData.m_UID;
                event_id = eventData.m_EventID;
                start_date = eventData.m_StartDate;
                end_date = eventData.m_EndDate;
                coin_type = type.ToString();
            }
        }

        public CQueryCharacterEventUpdate(long sessionKey, long userUID, _EventData eventData, CDefine.CoinType type)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_EventData = SCopy<_EventData>.DeepCopy(eventData);
            m_Params.SetParma(userUID, m_EventData, type);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_event_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            //nothing
        }
    }

    public class CQueryCharacterQuestEventUpdate : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private List<_Mission> m_Missions = new List<_Mission>();
        private DateTime m_ExpTime = DateTime.MinValue;
        private string m_QuestID = string.Empty;

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        struct DBParams
        {
            public long db_uid;
            public string db_mission_json;
            public DateTime db_exp_time;
            public string db_quest_id;
        }

        public CQueryCharacterQuestEventUpdate(long sessionKey, long userUID, List<_Mission> mission, DateTime exp_time, string quest_id)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Missions = mission;
            m_ExpTime = exp_time;
            m_QuestID = quest_id;
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_event_update";

                // 입출력인수 설정
                CDBManager.Instance.SetParam(ref sqlCmd, ref m_Data, ref agent, new DBParams
                {
                    db_uid = m_UID,
                    db_mission_json = SCommon.SJson.ObjectToJson(m_Missions),
                    db_exp_time = m_ExpTime,
                    db_quest_id = m_QuestID,
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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
        }
    }
    public class CQueryCharacterQuestEventReward : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private List<_Mission> m_Missions = new List<_Mission>();
        private string m_QuestID = string.Empty;


        private CDBMerge m_DBTran = new CDBMerge();
        private CRewardInfo m_ForClient = new CRewardInfo();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long db_uid;
            public string db_mission_json;
            public DateTime db_exp_time;
            public string db_quest_id;
            public string update_coin;
            public string update_item;

            public void SetParma(long uid, string mission_json, DateTime exp_time, CDBMerge dbtran, string quest_id)
            {
                db_uid = uid;
                db_mission_json = mission_json;
                db_exp_time = exp_time;
                update_coin = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_item = SJson.ObjectToJson(dbtran.m_UpdateItemList);
                db_quest_id = quest_id;
            }
        }

        public CQueryCharacterQuestEventReward(long sessionKey, long userUID, List<_Mission> missoins, DateTime exp_time, CRewardInfo forClient, CDBMerge dbtran, string quest_id)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_Missions = missoins;
            m_DBTran = SCopy<CDBMerge>.DeepCopy(dbtran);
            m_ForClient = forClient;
            m_QuestID = quest_id;
            m_Params.SetParma(userUID, SJson.ObjectToJson(m_Missions), exp_time, m_DBTran, m_QuestID);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_quest_event_rewarded";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.QuestAgent.AfterQueryRewardEventMission(m_SessionKey, m_Missions, m_ForClient, m_DBTran, m_Result);
        }
    }

    public class CQueryCharacterEventRouletteReward : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _EventRouletteData m_RouletteData = new _EventRouletteData();
        private int m_PickedIndex = 0;

        private CDBMerge m_DBTran = new CDBMerge();
        private CRewardInfo m_ForClient = new CRewardInfo();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long uid;
            public long event_uid;
            public string exclude_list;
            public string update_items;
            public string update_coins;

            public void SetParma(long user_uid, _EventRouletteData rouletteData, CDBMerge dbtran)
            {
                uid = user_uid;
                event_uid = rouletteData.m_EventUID;
                exclude_list = SJson.ObjectToJson(rouletteData.m_ExcludeIndexs);
                update_coins = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_items = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        public CQueryCharacterEventRouletteReward(long sessionKey, long userUID, int picked_index, _EventRouletteData rouletteData, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_PickedIndex = picked_index;
            m_RouletteData = rouletteData;
            m_DBTran = SCopy<CDBMerge>.DeepCopy(dbtran);
            m_ForClient = forClient;
            m_Params.SetParma(userUID, m_RouletteData, m_DBTran);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_event_roulette_update";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.EventAgent.AfterQueryEventRouletteReward(m_SessionKey, m_PickedIndex, m_RouletteData, m_ForClient, m_Result);
        }
    }

    //====================================Shop=============================
    public class CQueryCharacterShopBuy : IMsSqlQuery
    {
        //input
        private long m_SessionKey = 0;
        private long m_UID = 0;
        private _ShopData m_ShopData = new _ShopData();

        private CDBMerge m_DBTran = new CDBMerge();
        private CRewardInfo m_ForClient = new CRewardInfo();

        //Output
        private Packet_Result.Result m_Result = Packet_Result.Result.Success;
        private ProcedureResult m_Data = new ProcedureResult();

        private DBParams m_Params = new DBParams();

        struct DBParams
        {
            public long uid;
            public int record_id;
            public int bought_cnt;
            public long reset_time;
            public string update_coins;
            public string update_items;

            public void SetParma(long user_uid, _ShopData shopData, CDBMerge dbtran)
            {
                uid = user_uid;
                record_id = shopData.m_ShopID;
                bought_cnt = shopData.m_LimitCount;
                reset_time = shopData.m_ResetDate;
                update_coins = SJson.ObjectToJson(dbtran.m_UpdateCoinList);
                update_items = SJson.ObjectToJson(dbtran.m_UpdateItemList);
            }
        }

        public CQueryCharacterShopBuy(long sessionKey, long userUID, _ShopData shopData, CRewardInfo forClient, CDBMerge dbtran)
        {
            m_SessionKey = sessionKey;
            m_UID = userUID;
            m_ShopData = shopData;
            m_DBTran = SCopy<CDBMerge>.DeepCopy(dbtran);
            m_ForClient = forClient;
            m_Params.SetParma(userUID, m_ShopData, m_DBTran);
        }

        public override void Run(SMsSql agent)
        {
            try
            {
                // DB연결 및 SP명 설정
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = agent.GetAgent;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_character_shop_buy";

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
            CUser user = CUserManager.Instance.FindbySessionKey(m_SessionKey);
            if (user == null)
                return;

            if (m_Result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ReportKick(m_SessionKey, m_Result);
            else
                user.ShopAgent.AfterQueryShopBuy(m_SessionKey, m_ShopData, m_ForClient, m_Result);
        }
    }

    #endregion
}

