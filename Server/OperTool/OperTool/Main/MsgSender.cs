using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using Global;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperTool
{
    public partial class CNetManager
    {
        //X2X
        #region X2X
        public void X2X_HeartBeat(long sessionKey)
        {
            var sendMsg = new Packet_X2X.X2X_HeartBeat();
            Write(sessionKey, sendMsg);
        }
        #endregion

        //O2M
        public void O2M_RequestSearchUser(string deviceID, string name, long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestSearchUser();
            sendMsg.m_Name = name;
            sendMsg.m_DeviceID = deviceID;
            sendMsg.m_UID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestSystemPostLoad(long begin, long end)
        {
            var sendMsg = new Packet_O2M.O2M_RequestSystemPostLoad();
            sendMsg.m_BeginTime = begin;
            sendMsg.m_EndTime = end;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestSystemPostSend(long postIID, CDefine.PostType type, string title, string msg, long begin, long expire, List<_AssetData> rewards)
        {
            var sendMsg = new Packet_O2M.O2M_RequestSystemPostSend();
            sendMsg.m_PostID = postIID;
            sendMsg.m_Type = type;
            sendMsg.m_Title = title;
            sendMsg.m_Msg = msg;
            sendMsg.m_BeginTime = begin;
            sendMsg.m_ExpireTime = expire;
            sendMsg.m_Rewards = rewards;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestNoticeLoad(DateTime start, DateTime end)
        {
            var sendMsg = new Packet_O2M.O2M_RequestNoticeLoad();
            sendMsg.m_StartDate = start;
            sendMsg.m_EndDate = end;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestNoticeUpdate(_NoticeData notice)
        {
            var sendMsg = new Packet_O2M.O2M_RequestNoticeUpdate();
            sendMsg.m_NoticeData = notice;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestNoticeErase(long noticeID)
        {
            var sendMsg = new Packet_O2M.O2M_RequestNoticeErase();
            sendMsg.m_RemoveID = noticeID;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestUserPostLoad(long userUID, long start, long end)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserPostLoad();
            sendMsg.m_UserUID = userUID;
            sendMsg.m_Start = start;
            sendMsg.m_End = end;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestUserPostSend(long userUID, long postIID, CDefine.PostType type, string title, string msg, long begin, long expire, List<_AssetData> rewards)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserPostSend();
            sendMsg.m_UserID = userUID;
            sendMsg.m_PostID = postIID;
            sendMsg.m_Type = type;
            sendMsg.m_Title = title;
            sendMsg.m_Msg = msg;
            sendMsg.m_BeginTime = begin;
            sendMsg.m_ExpireTime = expire;
            sendMsg.m_Rewards = rewards;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestBlockUserLoad()
        {
            var sendMsg = new Packet_O2M.O2M_RequestBlockUserLoad();

            Write(m_ServerSession.SessionKey, sendMsg);
        }
        
        public void O2M_ReprortBlockUserUpsert(List<_BlockUser> blocks)
        {
            var sendMsg = new Packet_O2M.O2M_ReprortBlockUserUpsert();
            sendMsg.m_BlockUsers = blocks;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_ReportBlockUserDelete(string deviceid)
        {
            var sendMsg = new Packet_O2M.O2M_ReportBlockUserDelete();
            sendMsg.m_DeviceID = deviceid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_ReportWhiteUserInsert(string deviceID)
        {
            var sendMsg = new Packet_O2M.O2M_ReportWhiteUserInsert();
            sendMsg.m_DeviceID = deviceID;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_ReportWhiteUserDelete(string deviceID)
        {
            var sendMsg = new Packet_O2M.O2M_ReportWhiteUserDelete();
            sendMsg.m_DeviceID = deviceID;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_ReportUserKick(string deviceID)
        {
            var sendMsg = new Packet_O2M.O2M_ReportUserKick();
            sendMsg.m_DeviceID = deviceID;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestCouponLoad(long begin, long end)
        {
            var sendMsg = new Packet_O2M.O2M_RequestCouponLoad();
            sendMsg.m_BeginTime = begin;
            sendMsg.m_EndTime = end;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestCouponCreate(string couponID, int cnt, int lv, long begin, long exp, List<_AssetData> rewards)
        {
            var sendMsg = new Packet_O2M.O2M_RequestCouponCreate();
            sendMsg.m_CouponID = couponID;
            sendMsg.m_Count = cnt;
            sendMsg.m_UseLevel = lv;
            sendMsg.m_BeginTime = begin;
            sendMsg.m_ExpireTime = exp;
            sendMsg.m_Rewards = rewards;

            Write(m_ServerSession.SessionKey, sendMsg);    
        }

        public void O2M_RequestUserGrowthLevelLoad(long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserGrowthLevelLoad();
            sendMsg.m_TargetUID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestUserGrowthGoldLoad(long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserGrowthGoldLoad();
            sendMsg.m_TargetUID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestUserGachaLoad(long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserGachaLoad();
            sendMsg.m_TargetUID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestUserQuestLoad(long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserQuestLoad();
            sendMsg.m_TargetUID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestUserRelicLoad(long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserRelicLoad();
            sendMsg.m_TargetUID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestUserSkillLoad(long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserSkillLoad();
            sendMsg.m_TargetUID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestUserShopLoad(long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserShopLoad();
            sendMsg.m_TargetUID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }
        public void O2M_RequestUserPassLoad(long uid)
        {
            var sendMsg = new Packet_O2M.O2M_RequestUserPassLoad();
            sendMsg.m_TargetUID = uid;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_RequestServerStateLoad()
        {
            var sendMsg = new Packet_O2M.O2M_RequestServerStateLoad();

            Write(m_ServerSession.SessionKey, sendMsg);
        }

        public void O2M_ReportChangeServerServiceType(bool bopen)
        {
            var sendMsg = new Packet_O2M.O2M_ReportChangeServerServiceType();
            sendMsg.m_Open = bopen;

            Write(m_ServerSession.SessionKey, sendMsg);
        }

    }
}
