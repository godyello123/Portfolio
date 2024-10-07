using System;
using System.Collections.Generic;
using System.Security.Policy;
using Global;
using SCommon;

namespace PlayServer
{
    public class CPostAgent
    {
        private CUser m_Owner;

        private Dictionary<long, _PostData> m_PostBox = new Dictionary<long, _PostData>();
        public CPostAgent(CUser owner) { m_Owner = owner; }
        //todo : definetable 우편 갱신 시간
        public STimer m_Timer = new STimer(30 * 1000);//우편 재갱신 시간 --> 30초

        public void Init(List<_PostData> loadpostdatas)
        {
            m_PostBox.Clear();

            foreach (var it in loadpostdatas)
                m_PostBox[it.ID] = it;
        }

        public void Save()
        {

        }

        public _PostData Find(long _postID)
        {
            if (m_PostBox.TryGetValue(_postID, out var ret))
                return ret;

            return null;
        }

        public List<_PostData> GetList()
        {
            var retlist = new List<_PostData>();
            foreach(var iter in m_PostBox)
            {
                var postData = iter.Value;
                retlist.Add(postData);
            }
            
            return retlist;
        }

        public Packet_Result.Result ReqPostBoxOpen(long sessionKey, bool clientflag)
        {
            if (!m_Timer.Check())
            {
                CNetManager.Instance.P2C_ResultPostBoxOpen(sessionKey, GetList(), clientflag, Packet_Result.Result.Success);
            }
            else
            {
                m_PostBox.Clear();
                CDBManager.Instance.QueryCharacterPostBoxOpen(m_Owner.DBGUID, sessionKey, m_Owner.UID, clientflag);
            }

            return Packet_Result.Result.Success;
        }

        public void AfterQueryPostBoxOpen(long _sessionKey, List<_PostData> posts, bool clientflag, Packet_Result.Result result)
        {
            foreach (var iter in posts)
            {
                if (iter.Type != CDefine.PostType.Purchase)
                {
                    if (iter.IsRead && iter.IsRewarded)
                        continue;
                }

                m_PostBox[iter.ID] = iter;
            }

            CNetManager.Instance.P2C_ResultPostBoxOpen(_sessionKey, posts, clientflag, result);
        }

        public void AfterQueryPostRead(long _sessionKey, List<_PostData> postIds, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ResultPostRead(_sessionKey, postIds, result);
        }

        public void AfterQueryPostReward(long _sessionKey, List<_PostData> postIds, CRewardInfo forClient, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ResultPostReward(_sessionKey, postIds, forClient.GetList(), result);
        }

        //public void AfterQueryPostRemove(long sessionKey, List<_PostData> removelist, Packet_Result.Result result)
        //{
        //    CNetManager.Instance.P2C_ResultPostRemove(sessionKey, removelist, result);
        //}

        public Packet_Result.Result ReqPostReward(long sessionKey, List<long> postids)
        {
            //valid
            HashSet<long> postIDhashset = new HashSet<long>();
            foreach (var id in postids)
                postIDhashset.Add(id);


            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            bool bSend = true;

            List<_PostData> readPosts = new List<_PostData>();
            foreach (var postID in postIDhashset)
            {
                var postData = Find(postID);
                if (postData == null)
                    continue;

                if (postData.IsRewarded)
                    continue;

                if (!SDateManager.Instance.IsExpired(postData.beginTime))
                    continue;

                if (SDateManager.Instance.IsExpired(postData.expireTime))
                    continue;

                postData.IsRewarded = true;
                postData.IsRead = true;

                forClient.Insert(postData.Rewards);
                readPosts.Add(SCopy<_PostData>.DeepCopy(postData));
            }

            if(readPosts.Count < 1)
            {
                CLogger.Instance.System($"[ReqPostReward] Count < 1 : {readPosts.Count}");
                return Packet_Result.Result.IgnoreError;
            }

            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
            dbtran.Merge();

            List<long> rewardPostIDs = new List<long>();
            foreach (var iter in readPosts)
                rewardPostIDs.Add(iter.ID);

            CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
            CDBManager.Instance.QueryCharacterPostReward(m_Owner.DBGUID, sessionKey, m_Owner.UID, readPosts, rewardPostIDs, dbtran, forClient);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.reward_post, m_Owner.UserData, SJson.ObjectToJson(rewardPostIDs), dbtran, readPosts.Count);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqPostRead(long sessionKey, List<long> postids)
        {
            //valid
            HashSet<long> postIDhashset = new HashSet<long>();
            foreach (var id in postids)
                postIDhashset.Add(id);

            List<_PostData> readPosts = new List<_PostData>();
            foreach (var postID in postIDhashset)
            {
                var postData = Find(postID);
                if (postData == null)
                    continue;

                if (postData.IsRead)
                    continue;

                if (!SDateManager.Instance.IsExpired(postData.beginTime))
                    continue;

                if (SDateManager.Instance.IsExpired(postData.expireTime))
                    continue;

                postData.IsRead = true;
                readPosts.Add(SCopy<_PostData>.DeepCopy(postData));
            }

            if (readPosts.Count < 1)
            {
                CLogger.Instance.System($"[ReqPostRead] Count < 1 : {readPosts.Count}");
                return Packet_Result.Result.IgnoreError;
            }

            List<long> readPostIDs = new List<long>();
            foreach (var iter in readPosts)
                readPostIDs.Add(iter.ID);

            CDBManager.Instance.QueryCharacterPostRead(m_Owner.DBGUID, sessionKey, m_Owner.UID, readPosts, readPostIDs);

            return Packet_Result.Result.Success;
        }

        public void RepInsertSystemPost(_PostData postData)
        {
            if (m_PostBox.ContainsKey(postData.ID))
                return;

            CNetManager.Instance.P2C_ReportUserPost(m_Owner.SessionKey, postData);
        }
    }
}
