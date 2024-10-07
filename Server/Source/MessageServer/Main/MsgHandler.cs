using System;
using System.Collections.Generic;
using SNetwork;
//using Global;
//using Global.RestAPI;
using SCommon;
using Global;
using Packet_O2M;

namespace MessageServer
{
    public partial class CNetManager
    {
        private void SetupMsgHandler()
        {
            //X2X
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeat, new MsgHandlerDelegate(X2X_HeartBeat));
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeatAck, new MsgHandlerDelegate(X2X_HeartBeatAck));

            //P2M
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_RequestEnterUser, new MsgHandlerDelegate(P2M_RequestEnterUser));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_RequestConnect, new MsgHandlerDelegate(P2M_RequestConnect));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_ReportSendChatData, new MsgHandlerDelegate(P2M_ReportSendChatData));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_RequestChatList, new MsgHandlerDelegate(P2M_RequestChatList));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_ReportServerState, new MsgHandlerDelegate(P2M_ReportServerState));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_RequestNoticeList, new MsgHandlerDelegate(P2M_RequestNoticeList));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_RequestGetRank, new MsgHandlerDelegate(P2M_RequestGetRank));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_ReportUserOverViewUpdate, new MsgHandlerDelegate(P2M_ReportUserOverViewUpdate));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_ReportUserRankUpdate, new MsgHandlerDelegate(P2M_ReportUserRankUpdate));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_ReportCheat, new MsgHandlerDelegate(P2M_ReportCheat));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_RequestPvpMatchStart, new MsgHandlerDelegate(P2M_RequestPvpMatchStart));
            m_Handler.Add((ushort)Packet_P2M.Protocol.P2M_ReportUserLogout, new MsgHandlerDelegate(P2M_ReportUserLogout));


            //O2M
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestSearchUser, new MsgHandlerDelegate(O2M_RequestSearchUser));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestSystemPostLoad, new MsgHandlerDelegate(O2M_RequestSystemPostLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestSystemPostSend, new MsgHandlerDelegate(O2M_RequestSystemPostSend));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestNoticeLoad, new MsgHandlerDelegate(O2M_RequestNoticeLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestNoticeUpdate, new MsgHandlerDelegate(O2M_RequestNoticeUpdate));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestNoticeErase, new MsgHandlerDelegate(O2M_RequestNoticeErase));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestUserPostLoad, new MsgHandlerDelegate(O2M_RequestUserPostLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestUserPostSend, new MsgHandlerDelegate(O2M_RequestUserPostSend));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestBlockUserLoad, new MsgHandlerDelegate(O2M_RequestBlockUserLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_ReportBlockUserUpsert, new MsgHandlerDelegate(O2M_ReprortBlockUserUpsert));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_ReportBlockUserDelete, new MsgHandlerDelegate(O2M_ReportBlockUserDelete));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_ReportWhiteUserInsert, new MsgHandlerDelegate(O2M_ReportWhiteUserInsert));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_ReportWhiteUserDelete, new MsgHandlerDelegate(O2M_ReportWhiteUserDelete));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_ReportUserKick, new MsgHandlerDelegate(O2M_ReportUserKick));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestCouponLoad, new MsgHandlerDelegate(O2M_RequestCouponLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestCouponCreate, new MsgHandlerDelegate(O2M_RequestCouponCreate));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestUserGrowthGoldLoad, new MsgHandlerDelegate(O2M_RequestUserGrowthGoldLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestUserGrowthLevelLoad, new MsgHandlerDelegate(O2M_RequestUserGrowthLevelLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestUserGachaLoad, new MsgHandlerDelegate(O2M_RequestUserGachaLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestUserQuestLoad, new MsgHandlerDelegate(O2M_RequestUserQuestLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestUserRelicLoad, new MsgHandlerDelegate(O2M_RequestUserRelicLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_RequestServerStateLoad, new MsgHandlerDelegate(O2M_RequestServerStateLoad));
            m_Handler.Add((ushort)Packet_O2M.Protocol.O2M_ReportChangeServerServiceType, new MsgHandlerDelegate(O2M_ReportChangeServerServiceType));

            
        }
    }

    //X2X
    public partial class CNetManager
    {
        public void X2X_HeartBeat(long sessionKey, byte[] packet)
        {
            Write(sessionKey, new Packet_X2X.X2X_HeartBeatAck());
        }

        public void X2X_HeartBeatAck(long sessionKey, byte[] packet)
        {
        }
    }

    //P2M
    public partial class CNetManager
    {
        public void P2M_RequestConnect(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_RequestConnect();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CServerManager.Instance.Insert("PlayServer", sessionKey, recvMsg.m_ClinetCount, recvMsg.m_ServiceType, recvMsg.m_IP, recvMsg.m_DN, recvMsg.m_Port);
            CScheduleManager.Instance.ReqConnectPlayServer(sessionKey);
        }

        public void P2M_RequestEnterUser(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_RequestEnterUser();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var result = CUserManager.Instance.ReqEnterUser(sessionKey, recvMsg.m_UserSession, recvMsg.m_UID, recvMsg.m_DeviceID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.M2P_ResultEnterUser(sessionKey, recvMsg.m_UserSession, recvMsg.m_UID, recvMsg.m_DeviceID, result);
        }

        public void P2M_ReportSendChatData(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_ReportSendChatData();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CChatManager.Instance.RepSendChatData(recvMsg.m_Chat);
        }

        public void P2M_RequestChatList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_RequestChatList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CChatManager.Instance.ReqChatList(sessionKey, recvMsg.m_Type, recvMsg.m_RequestUID);
        }

        public void P2M_ReportServerState(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_ReportServerState();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CServerManager.Instance.SetServerState(sessionKey, recvMsg.m_ClientCount, recvMsg.m_MainFPS, recvMsg.m_DBFPS, recvMsg.m_DBInputCount);
        }

        public void P2M_RequestNoticeList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_RequestNoticeList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var server = CServerManager.Instance.Find(sessionKey);
            if (server == null) return;

            var result = CSystemManager.Instance.ReqNoticeList(sessionKey, recvMsg.m_UserSession);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.M2P_ResultNoticeList(sessionKey, recvMsg.m_UserSession, new List<_NoticeData>(), result);
        }

        public void P2M_RequestGetRank(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_RequestGetRank();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var server = CServerManager.Instance.Find(sessionKey);
            if (server == null) return;

            var overview = CUserManager.Instance.FindUser(recvMsg.m_UID);
            if (overview == null) return;

            CLogger.Instance.System($"P2M_RequestGetRank : {overview.OverView.m_Name}");

            var result = CRankingManager.Instance.ReqGetRank(sessionKey, recvMsg.m_UserSession, overview, recvMsg.m_Type);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.M2P_ResultGetRank(sessionKey, recvMsg.m_UserSession, recvMsg.m_Type, new List<_RankData>(), new _RankData(), result);
        }

        public void P2M_ReportUserRankUpdate(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_ReportUserRankUpdate();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var server = CServerManager.Instance.Find(sessionKey);
            if (server == null) return;

            var overview = CUserManager.Instance.FindUser(recvMsg.m_UID);
            if (overview == null) return;

            CLogger.Instance.System($"P2M_ReportUserRankUpdate : {overview.OverView.m_Name}");

            CRankingManager.Instance.RepInsertRank(recvMsg.m_Type, overview);
        }

        public void P2M_ReportUserOverViewUpdate(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_ReportUserOverViewUpdate();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var server = CServerManager.Instance.Find(sessionKey);
            if (server == null) return;

            CLogger.Instance.System($"P2M_ReportUserOverViewUpdate : {recvMsg.m_OverViewData.m_Name}");

            CUserManager.Instance.ReqUpdateOverView(sessionKey, recvMsg.m_OverViewData);
        }

        public void P2M_RequestPvpMatchStart(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_RequestPvpMatchStart();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var server = CServerManager.Instance.Find(sessionKey);
            if (server == null) return;

            var overview = CUserManager.Instance.FindUser(recvMsg.m_DeviceID);
            if (overview == null)
                return;

            var result = CRankingManager.Instance.ReqPvpMatching(overview, recvMsg.m_UserSession);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.M2P_ResultPvpMatchStart(sessionKey, recvMsg.m_UserSession, new List<_PvPUserData>(), result);
        }

        public void P2M_ReportUserLogout(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_ReportUserLogout();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var server = CServerManager.Instance.Find(sessionKey);
            if (server == null) return;

            //user logout process
            CUserManager.Instance.Remove(recvMsg.m_DeviceID, recvMsg.m_UID);
            CRankingManager.Instance.DeletePvpMatchData(recvMsg.m_DeviceID);
        }
        
        public void P2M_ReportCheat(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.P2M_ReportCheat();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CheatManager.Instance.ReqCheat(sessionKey, recvMsg.m_UserSession, recvMsg.m_Cheat, recvMsg.m_CheatParams);
        }

      
    }

    //============================O2M====================
    public partial class CNetManager
    {
        public void O2M_RequestSearchUser(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestSearchUser();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryyToolCharacterInfoLoad(sessionKey, recvMsg.m_UID, recvMsg.m_DeviceID, recvMsg.m_Name);
        }

        public void O2M_RequestSystemPostLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestSystemPostLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryyToolSystemPostLoad(sessionKey, recvMsg.m_BeginTime, recvMsg.m_EndTime);
        }

        public void O2M_RequestSystemPostSend(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestSystemPostSend();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolSystemUpsertPost(sessionKey, recvMsg.m_PostID, recvMsg.m_Type, recvMsg.m_Title, recvMsg.m_Msg, recvMsg.m_BeginTime, recvMsg.m_ExpireTime, SJson.ObjectToJson(recvMsg.m_Rewards));
        }

        public void O2M_RequestNoticeLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestNoticeLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolSystemNoticeLoad(sessionKey, recvMsg.m_StartDate, recvMsg.m_EndDate);
        }

        public void O2M_RequestNoticeUpdate(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestNoticeUpdate();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolSystemNoticeUpdate(sessionKey, recvMsg.m_NoticeData);
        }

        public void O2M_RequestNoticeErase(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestNoticeErase();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolSystemNoticeErase(sessionKey, recvMsg.m_RemoveID);
        }

        public void O2M_RequestUserPostLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestUserPostLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolUserPostLoad(sessionKey, recvMsg.m_UserUID, recvMsg.m_Start, recvMsg.m_End);
        }

        public void O2M_RequestUserPostSend(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestUserPostSend();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolUserPostInsert(sessionKey, recvMsg.m_UserID, recvMsg.m_PostID, recvMsg.m_Type, recvMsg.m_Title, recvMsg.m_Msg, recvMsg.m_BeginTime, recvMsg.m_ExpireTime, SJson.ObjectToJson(recvMsg.m_Rewards));
        }

        public void O2M_RequestBlockUserLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestBlockUserLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolBlockUserLoad(sessionKey);
        }

        public void O2M_ReprortBlockUserUpsert(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_ReprortBlockUserUpsert();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_BlockUsers.Count < 1)
                return;

            CDBManager.Instance.QueryToolBlockUserUpsert(recvMsg.m_BlockUsers);
        }

        public void O2M_ReportBlockUserDelete(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_ReportBlockUserDelete();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QuerySystemDeleteBlockUser(recvMsg.m_DeviceID);
        }

        public void O2M_ReportWhiteUserInsert(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_ReportWhiteUserInsert();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QuerySystemUpdateWhiteUser(recvMsg.m_DeviceID);
        }

        public void O2M_ReportWhiteUserDelete(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_ReportWhiteUserDelete();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QuerySystemDeleteWhiteUser(recvMsg.m_DeviceID);
        }

        public void O2M_ReportUserKick(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_ReportUserKick();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CUserManager.Instance.UserKick(recvMsg.m_DeviceID);
        }

        public void O2M_RequestCouponLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestCouponLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolCouponLoad(sessionKey, recvMsg.m_BeginTime, recvMsg.m_EndTime);
        }

        public void O2M_RequestCouponCreate(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestCouponCreate();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            //todo db
            CDBManager.Instance.QueryToolSystemCouponUpsert(sessionKey, recvMsg.m_CouponID, recvMsg.m_Count, recvMsg.m_UseLevel, recvMsg.m_BeginTime, recvMsg.m_ExpireTime, recvMsg.m_Rewards);
        }

        public void O2M_RequestUserGrowthLevelLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestUserGrowthLevelLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolCharacaterGrowthLevelLoad(sessionKey, recvMsg.m_TargetUID);
        }

        public void O2M_RequestUserGrowthGoldLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestUserGrowthGoldLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolCharacaterGrowthGoldLoad(sessionKey, recvMsg.m_TargetUID);
        }

        public void O2M_RequestUserGachaLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestUserGachaLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolCharacaterGachaLoad(sessionKey, recvMsg.m_TargetUID);
        }

        public void O2M_RequestUserQuestLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestUserQuestLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolCharacaterQuestLoad(sessionKey, recvMsg.m_TargetUID);
        }

        public void O2M_RequestUserRelicLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestUserRelicLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CDBManager.Instance.QueryToolCharacaterRelicLoad(sessionKey, recvMsg.m_TargetUID);
        }

        public void O2M_RequestServerStateLoad(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_RequestServerStateLoad();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CNetManager.Instance.M2O_ResultServerStateLoad(sessionKey, CServerManager.Instance.Servers, CSystemManager.Instance.Open, Packet_Result.Result.Success);
        }

        public void O2M_ReportChangeServerServiceType(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_O2M.O2M_ReportChangeServerServiceType();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CSystemManager.Instance.ReqServiceType(recvMsg.m_Open);
        }
    }
}
