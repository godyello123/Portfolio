using System;
using System.Collections.Generic;
using System.Threading;
using SCommon;
using Global;
using System.Linq;

namespace MessageServer
{
    class CPvpMatchingSearch : IComparer<CPvpMatchingSearch>
    {
        public int m_Point;
        public string m_DeviceID;
        public int Compare(CPvpMatchingSearch lh, CPvpMatchingSearch rh)
        {
            return lh.m_Point.CompareTo(rh.m_Point);
        }
    }

    class CRankMode
    {
        public CDefine.ERankType m_Type = CDefine.ERankType.Max;
        public Dictionary<Int64, _RankData> m_InputRankList = new Dictionary<Int64, _RankData>();
        public Dictionary<Int64, _RankData> m_RankList = new Dictionary<Int64, _RankData>();
        public List<_RankData> m_TopRankList = new List<_RankData>();  //리스트 노출 랭킹 100 정도?
        

        public CRankMode(CDefine.ERankType type)
        {
            m_Type = type;
        }

        public _RankData UpsertAtInputRankList(_RankData data)
        {
            m_InputRankList[data.m_UID] = data;
            return m_InputRankList[data.m_UID];
        }
        public _RankData FindAtInputRankList(long accountID)
        {
            _RankData retval;
            m_InputRankList.TryGetValue(accountID, out retval);
            return retval;
        }

        public _RankData FindAtRankList(long accountID)
        {
            _RankData retval;
            m_RankList.TryGetValue(accountID, out retval);
            return retval;
        }

        public bool RemoveAtInputRankList(long accountID)
        {
            return m_InputRankList.Remove(accountID);
        }
    }

    class CRankingManager : SSingleton<CRankingManager>
    {
        private bool m_Run;
        private Thread m_BackThread;
        private DateTime m_CheckDate = DateTime.MinValue;
        private bool m_IsSort = false;

        private Dictionary<CDefine.ERankType, CRankMode> m_RankDataList = new Dictionary<CDefine.ERankType, CRankMode>();

        //private Dictionary<int, Dictionary<string, CPvpMatchingSearch>> m_PvpMatchList = new Dictionary<int, Dictionary<string, CPvpMatchingSearch>>();
        public Dictionary<string, CPvpMatchingSearch> m_PvpMatchList = new Dictionary<string, CPvpMatchingSearch>();

        static readonly object _locker = new object();

        public void Init()
        {
            int fetchCnt = DefineTable.Instance.Value<int>("Main_Rank_Last");
            int minVal = DefineTable.Instance.Value<int>("Main_Rank_Standard");
            CDBManager.Instance.SyncQuerySystemLoadRankMainSync(CDefine.ERankType.Rank_MainStage, CDefine.eStageType.Main_Stage, fetchCnt, minVal);
            CDBManager.Instance.SyncQuerySystemLoadRankPvp(CDefine.ERankType.Rank_PvP, CDefine.CoinType.Pvp_Point, fetchCnt);

            Stop();

            m_Run = true;

            m_BackThread = new Thread(new ThreadStart(ThreadFunc));
            m_BackThread.Start();
        }

        private CRankMode FindOrAddRankMode(CDefine.ERankType type)
        {
            CRankMode retval = null;
            if (!m_RankDataList.TryGetValue(type, out retval))
            {
                retval = new CRankMode(type);
                m_RankDataList.Add(type, retval);
            }

            return retval;
        }
        public void AfterQuery_LoadRank(CDefine.ERankType type, List<_RankData> rank_list)
        {
            var hasRankMode = FindOrAddRankMode(type);

            foreach (var data in rank_list)
            {
                if (true == CSystemManager.Instance.IsBlockUser(data.m_DeviceID))
                    continue;

                hasRankMode.UpsertAtInputRankList(data);
            }
        }

        //public void AfterQuery_LoadRankPvp(_ServerInfo serverInfo, List<_RankInfo> rank_list)
        //{
        //    var hasRankMode = FindOrAddRankMode(eRankType.Rank_Pvp, serverInfo.m_ServerID);

        //    foreach (var data in rank_list)
        //    {
        //        if (true == CBlockManager.Instance.IsBlockUser(data.m_AccountID))
        //            continue;

        //        hasRankMode.UpsertAtInputRankList(data);
        //    }
        //}

        //public void UpdateUserGuild(long accountID, int guildUID, string guildName, int markID)
        //{
        //    lock (_locker)
        //    {
        //        foreach (var it in m_RankDataList)
        //        {
        //            var dic = it.Value;
        //            var rankInfo = dic.FindAtInputRankList(accountID);
        //            if (rankInfo == null)
        //                continue;

        //            rankInfo.m_GuildUID = guildUID;
        //            rankInfo.m_GuildName = guildName;
        //            rankInfo.m_GuildMarkID = markID;
        //            rankInfo.m_GuildRank = CGuildManager.Instance.PrevSeasonRank(guildUID);
        //        }
        //    }
        //}

        public void Stop()
        {
            if (!m_Run) return;

            m_Run = false;

            if (m_BackThread != null)
            {
                m_BackThread.Join();
                m_BackThread = null;
            }
        }

        private void ThreadFunc()
        {
            while (m_Run)
            {
                if (m_IsSort)
                    SortRank();

                Thread.Sleep(1);
            }
        }

        //public void RepAfterMainStageReward()
        //{
        //    CNetManager.Instance.M2P_ReportRankReward();
        //}

        public void SortRank()
        {
            lock (_locker)
            {
                CLogger.Instance.System($"Rank Sort begin");

                if (m_RankDataList.Count == 0)
                {
                    m_IsSort = false;
                    return;
                }

                foreach (var type_data in m_RankDataList)
                {
                    var list = type_data.Value.m_InputRankList.OrderByDescending(x => x.Value.m_StageTID).ThenBy(x => x.Value.m_Name).ToList();

                    type_data.Value.m_InputRankList.Clear();
                    type_data.Value.m_RankList.Clear();
                    type_data.Value.m_TopRankList.Clear();

                    int rank = 1;
                    foreach (var sort_rank in list)
                    {
                        _RankData rank_info = SCopy<_RankData>.DeepCopy(sort_rank.Value);
                        rank_info.m_Rank = rank++;

                        //표기할 랭킹
                        if (rank <= DefineTable.Instance.Value<int>("Display_Rank"))
                            type_data.Value.m_TopRankList.Add(rank_info);

                        //최대 계산 랭크
                        if (rank <= DefineTable.Instance.Value<int>("Main_Rank_Last"))
                        {
                            type_data.Value.m_RankList.Add(sort_rank.Value.m_UID, rank_info);
                            type_data.Value.m_InputRankList.Add(sort_rank.Value.m_UID, rank_info);
                        }
                    }
                }

                //ReportTopRank();

                m_IsSort = false;

                CLogger.Instance.System($"Rank Sort End");
            }
        }

        public void Update()
        {
            DateTime utc = DateTime.UtcNow;
            if (utc >= m_CheckDate && false == m_IsSort)
            {
                m_IsSort = true;
                //랭킹 갱신 시간
                m_CheckDate = DateTime.UtcNow.AddSeconds(DefineTable.Instance.Value<int>("Ranking_Refresh"));
            }
        }

        //public void UpdateGuildRank()
        //{
        //    lock (_locker)
        //    {
        //        foreach (var it in m_RankDataList)
        //        {
        //            var dic = it.Value;
        //            foreach (var it_rank in dic.m_InputRankList)
        //            {
        //                var rankInfo = it_rank.Value;
        //                rankInfo.m_GuildRank = CGuildManager.Instance.PrevSeasonRank(rankInfo.m_GuildUID);
        //            }
        //        }
        //    }
        //}

        public void DeleteInputRank(Int64 account_id)
        {
            lock (_locker)
            {
                foreach (var it in m_RankDataList)
                {
                    var rankMode = it.Value;
                    rankMode.RemoveAtInputRankList(account_id);
                }
            }
        }

        public void GetMyRank(CDefine.ERankType type, CUserOverView overview, ref _RankData rank_data)
        {
            lock (_locker)
            {
                if (null == overview)
                    return;

                var hasRankMode = FindOrAddRankMode(type);
                if (hasRankMode == null) 
                    return;

                var hasRank = hasRankMode.FindAtRankList(overview.UID);
                if (hasRank == null)
                {
                    MakeRankInfo(type, overview, ref rank_data);
                }
                else
                {
                    rank_data = hasRank;
                }
            }
        }

        public void MakeRankInfo(CDefine.ERankType type, CUserOverView user, ref _RankData rank_info)
        {
            rank_info.m_Level = user.OverView.m_Level;
            rank_info.m_DeviceID = user.DeviceID;
            rank_info.m_UID = user.UID;
            rank_info.m_Name = user.OverView.m_Name;
            rank_info.m_RankType = type;
            rank_info.m_UpdateTime = DateTime.UtcNow;
            rank_info.m_ProfileID = user.OverView.m_ProfileID;
            
            if (type == CDefine.ERankType.Rank_MainStage)
                rank_info.m_StageTID = user.OverView.m_MaxMainStage;
            //todo : pvp_rank set
        }

        public void RepInsertRank(CDefine.ERankType type, CUserOverView user)
        {
            //valid
            if (true == m_IsSort)
                return; 

            if (null == user)
                return;

            var hasRankMode = FindOrAddRankMode(type);
            if (hasRankMode == null) return;

            _RankData latestRankInfo = new _RankData();
            MakeRankInfo(type, user, ref latestRankInfo);

            var hasInputRank = hasRankMode.FindAtInputRankList(user.UID);
            if (hasInputRank != null)
            {
                if (CDefine.ERankType.Rank_MainStage == type)
                {
                    if (hasInputRank.m_StageTID >= latestRankInfo.m_StageTID)
                        latestRankInfo.m_UpdateTime = hasInputRank.m_UpdateTime;
                }
                //else if (eRankType.Rank_Pvp == type)
                //{
                //    latestRankInfo.m_UpdateTime = hasInputRank.m_UpdateTime;
                //}
            }

            hasRankMode.UpsertAtInputRankList(latestRankInfo);
        }

        public Packet_Result.Result ReqGetRank(long toServer, long userSession, CUserOverView user, CDefine.ERankType rank_type)
        {
            //valid
            if (true == m_IsSort)
                return Packet_Result.Result.Fail_Get_Rank;

            if (null == user)
                return Packet_Result.Result.NotFoundData;

            var hasRankMode = FindOrAddRankMode(rank_type);
            if (hasRankMode == null)
                return Packet_Result.Result.NotFoundData;

            _RankData my_rank = new _RankData();
            GetMyRank(rank_type, user, ref my_rank);

            CNetManager.Instance.M2P_ResultGetRank(toServer, userSession, rank_type, hasRankMode.m_TopRankList, my_rank, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public void ReportTopRank()
        {
            foreach (var it in m_RankDataList)
            {
                var hasRankMode = it.Value;
                CNetManager.Instance.M2P_ReportTopRank(hasRankMode.m_Type, hasRankMode.m_TopRankList);
            }
        }

        public void InsertPvpMatchData(_UserOverViewData userData, string deviceID, int pvp_point)
        {
            if (false == m_PvpMatchList.ContainsKey(deviceID))
            {
                CPvpMatchingSearch data = new CPvpMatchingSearch();
                data.m_DeviceID = deviceID;
                data.m_Point = pvp_point;

                m_PvpMatchList.Add(deviceID, data);
            }
            else
            {
                m_PvpMatchList[deviceID].m_Point = pvp_point;
            }
        }

        public void DeletePvpMatchData(string device_id)
        {
            m_PvpMatchList.Remove(device_id);
        }

        public Packet_Result.Result ReqPvpMatching(CUserOverView user, long userSession)
        {
            //todo : pvp match user count
            List<string> match_user_list = SearchPvpMatchData(user.DeviceID);
            if (match_user_list.Count != 2)
                return Packet_Result.Result.Fail_Pvp_Match;

            List<_PvPUserData> pvp_user_data = new List<_PvPUserData>();

            int my_count = 0;
            foreach (var data in match_user_list)
            {
                var user_info = CUserManager.Instance.FindUser(data);
                if (null == user_info || false == user_info.IsLogin())
                    return Packet_Result.Result.Fail_Pvp_Match;

                if (true == string.Equals(user_info.DeviceID, user.DeviceID))
                    my_count++;

                pvp_user_data.Add(user_info.OverView.m_PvpUserData);
            }

            if (my_count != 1)
                return Packet_Result.Result.Fail_Pvp_Match;

            CNetManager.Instance.M2P_ResultPvpMatchStart(user.ServerSession, userSession, pvp_user_data, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public List<string> SearchPvpMatchData(string device_id)
        {
            List<string> match_user_list = new List<string>();

            //todo : define table pvp user Count
            int user_count = 2;
            if (m_PvpMatchList.Count < user_count)
                return match_user_list;

            if (false == m_PvpMatchList.ContainsKey(device_id))
                return match_user_list;

            var list = new List<CPvpMatchingSearch>(m_PvpMatchList.Values);
            list.Sort((x1, x2) => x2.m_Point.CompareTo(x1.m_Point));

            int index = 0;
            foreach (var data in list)
            {
                if (true == string.Equals(data.m_DeviceID, device_id))
                {
                    list.Remove(data);
                    break;
                }
                index++;
            }

            //todo : define Table pvp min user, max user
            int min = Math.Max(0, index - 100);
            int max = Math.Min(list.Count - 1, index + 100);

            if (list.Count < min || list.Count <= max)
                return match_user_list;

            match_user_list.Add(device_id);
            user_count--;

            while (user_count > 0)
            {
                int rand_index = SRandom.Instance.Next(min, max);

                match_user_list.Add(list[rand_index].m_DeviceID);
                list.RemoveAt(rand_index);
                max--;
                user_count--;
            }

            return match_user_list;
        }
    }
}
