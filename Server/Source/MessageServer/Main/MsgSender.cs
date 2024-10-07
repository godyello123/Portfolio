using System;
using System.Collections.Generic;
using System.Linq;
using SCommon;
using Global;
using StackExchange.Redis;
using System.Drawing.Design;
using System.Diagnostics.PerformanceData;
using System.Security.Policy;

namespace MessageServer
{
    public partial class CNetManager
    {
        public void M2P_ReportSendChatData(long toServer, _ChatData chatData)
        {
            var sendMsg = new Packet_P2M.M2P_ReportSendChatData();
            sendMsg.m_Chat = SCopy<_ChatData>.DeepCopy(chatData);

            if (toServer != -1)
                Write(toServer, sendMsg);
            else
                WriteAllToServer(sendMsg);
        }

        public void M2P_ResultChatList(long toServer, List<_ChatRoomData> chatRooms, long requester, Packet_Result.Result result)
        {
            var sendMsg = new Packet_P2M.M2P_ResultChatList();
            sendMsg.m_RequestUID = requester;
            sendMsg.m_ChatRooms = chatRooms;
            sendMsg.m_Result = (ushort)result;

            Write(toServer, sendMsg);
        }

        public void M2P_ReportUserKick(long toServer, long targetID, Packet_Result.Result result)
        {
            var sendMsg = new Packet_P2M.M2P_ReportUserKick();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_TargetID = targetID;

            Write(toServer, sendMsg);
        }

        public void M2P_ResultEnterUser(long toServer, long userSession, long uid, string deviceid, Packet_Result.Result result)
        {
            var sendMsg = new Packet_P2M.M2P_ResultEnterUser();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_UserSession = userSession;
            sendMsg.m_DeviceID = deviceid;
            sendMsg.m_UID = uid;

            Write(toServer, sendMsg);
        }

        public void M2P_ResultNoticeList(long toServer, long userSession, List<_NoticeData> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_P2M.M2P_ResultNoticeList();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Notices = datas;
            sendMsg.m_UserSession = userSession;

            Write(toServer, sendMsg);
        }

        public void M2P_ResultGetRank(long toServer, long userSession, CDefine.ERankType type, List<_RankData> datas, _RankData reqRank, Packet_Result.Result result)
        {
            var sendMsg = new Packet_P2M.M2P_ResultGetRank();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Type = type;
            sendMsg.m_ReqUserRank = reqRank;
            sendMsg.m_Ranks = datas;
            sendMsg.m_UserSession = userSession;

            Write(toServer, sendMsg);
        }

        public void M2P_ReportTopRank(CDefine.ERankType type, List<_RankData> datas)
        {
            var sendMsg = new Packet_P2M.M2P_ReportTopRank();
            sendMsg.m_Ranks = datas;
            sendMsg.m_Type = type;

            foreach (var iter in CServerManager.Instance.m_Servers)
                Write(iter.Key, sendMsg);
        }

        public void M2P_ReportHotTime(long sessionKey, _HotTimeData data, bool remove)
        {
            var sendMsg = new Packet_P2M.M2P_ReportHotTime();
            sendMsg.m_HotTimeData = SCopy<_HotTimeData>.DeepCopy(data);
            sendMsg.m_Remove = remove;


            if (sessionKey == -1)
            {
                foreach (var iter in CServerManager.Instance.m_Servers)
                    Write(iter.Key, sendMsg);
            }
            else
                Write(sessionKey, sendMsg);
        }

        public void M2P_ReportHotTimeList(long sessionKey, List<_HotTimeData> datas)
        {
            var sendMsg = new Packet_P2M.M2P_ReportHotTimeList();
            sendMsg.m_Datas = datas;

            if (sessionKey == -1)
            {
                foreach (var iter in CServerManager.Instance.m_Servers)
                    Write(iter.Key, sendMsg);
            }
            else
                Write(sessionKey, sendMsg);
        }

        public void M2P_ReportNotice(_NoticeData data, bool remove)
        {
            var sendMsg = new Packet_P2M.M2P_ReportNotice();
            sendMsg.m_NoticeData = data;
            sendMsg.m_IsErase = remove;

            foreach (var iter in CServerManager.Instance.m_Servers)
                Write(iter.Key, sendMsg);
        }

        public void M2P_ResultPvpMatchStart(long toServer, long userSession, List<_PvPUserData> pvp_userDatas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_P2M.M2P_ResultPvpMatchStart();
            sendMsg.m_PvpUserDatas = pvp_userDatas;
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_UserSession = userSession;

            Write(toServer, sendMsg);
        }

        public void M2P_ReportServerServiceType(long toServer, bool bopen)
        {
            var sendMsg = new Packet_P2M.M2P_ReportServerServiceType();
            sendMsg.m_Open = bopen;

            Write(toServer, sendMsg);
        }

        public void M2P_ReportInsertSystemPost(long toServer, _PostData data)
        {
            var sendMsg = new Packet_P2M.M2P_ReportInsertSystemPost();
            sendMsg.m_Data = data;

            Write(toServer, sendMsg);
        }

        public void M2P_ReportEvent(long toServer, _EventData eventData, bool remove)
        {
            var sendMsg = new Packet_P2M.M2P_ReportEvent();
            sendMsg.m_EventData = eventData;
            sendMsg.m_Remove = remove;

            Write(toServer, sendMsg);
        }

        public void M2P_ResultConnect(long toServer, List<_EventData> events)
        {
            var sendMsg = new Packet_P2M.M2P_ResultConnect();
            sendMsg.m_ActiveEvents = events;

            Write(toServer, sendMsg);
        }
    }



    //======================== O2M===========================
    public partial class CNetManager
    {
        public void M2O_ResultSearchUser(long sessionKey, _UserData userData, List<_GachaData> gachas, List<_AssetData> coins, List<_StageData> stages, List<_ReceiptData> receipts,
            bool isblock, bool iswhite, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultSearchUser();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_UserData = userData;
            sendMsg.m_GachaData = gachas;
            sendMsg.m_Coins = coins;
            sendMsg.m_Stages = stages;
            sendMsg.m_Receipts = receipts;
            sendMsg.m_IsBlock = isblock;
            sendMsg.m_IsWhite = iswhite;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultSystemPostLoad(long sessionKey, List<_PostData> systemPosts, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultSystemPostLoad();
            sendMsg.m_SystemPosts = systemPosts;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultSystemPostSend(long sessionKey, _PostData postData, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultSystemPostSend();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_PostData = postData;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultNoticeLoad(long sessionKey, List<_NoticeData> notices, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultNoticeLoad();
            sendMsg.m_Notices = notices;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultNoticeUpdate(long sessionKey, _NoticeData notice, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultNoticeUpdate();
            sendMsg.m_NoticeData = notice;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultNoticeErase(long sessionKey, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultNoticeErase();
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserPostLoad(long sessionKey, List<_PostData> postDatas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserPostLoad();
            sendMsg.m_Posts = postDatas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserPostSend(long sessionKey, _PostData postData, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserPostSend();
            sendMsg.m_PostData = postData;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultBlockUserLoad(long sessionKey, List<_BlockUser> blocks, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultBlockUserLoad();
            sendMsg.m_BlockUsers = blocks;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultCouponLoad(long sessionKey, List<_CouponData> coupons, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultCouponLoad();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Coupons = coupons;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultCouponCreate(long sessionKey, _CouponData coupon, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultCouponCreate();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Coupon = coupon;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserGrowthLevelLoad(long sessionKey, List<_LevelData> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserGrowthLevelLoad();
            sendMsg.m_Datas = datas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserGrowthGoldLoad(long sessionKey, List<_LevelData> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserGrowthGoldLoad();
            sendMsg.m_Datas = datas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserGachaLoad(long sessionKey, List<_GachaData> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserGachaLoad();
            sendMsg.m_Datas = datas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserQuestLoad(long sessionKey, List<_QuestBoard> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserQuestLoad();
            sendMsg.m_Datas = datas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserRelicLoad(long sessionKey, List<_RelicData> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserRelicLoad();
            sendMsg.m_Datas = datas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserSkillLoad(long sessionKey, List<_SkillData> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserSkillLoad();
            sendMsg.m_Datas = datas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserShopLoad(long sessionKey, List<_ShopData> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserShopLoad();
            sendMsg.m_Datas = datas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultUserPassLoad(long sessionKey, List<_QuestBoard> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultUserPassLoad();
            sendMsg.m_Datas = datas;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void M2O_ResultServerStateLoad(long sessionKey, Dictionary<long, CServerInfo> datas, bool bopen, Packet_Result.Result result)
        {
            var sendMsg = new Packet_O2M.M2O_ResultServerStateLoad();
            foreach(var iter in datas)
                sendMsg.m_Datas.Add(iter.Value);

            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Open = bopen;

            Write(sessionKey, sendMsg);
        }
    }
}