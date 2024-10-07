using System;
using System.Collections.Generic;
using SNetwork;
using Global;
using Global.RestAPI;
using SCommon;
using System.Net.Sockets;
using System.Windows.Data;
using System.Windows.Forms;
using FirebaseAdmin.Auth;
using Packet_C2P;
using System.Security.Policy;
using System.Runtime.InteropServices;

namespace PlayServer
{
    public partial class CNetManager
    {
        private void SetupMsgHandler()
        {
            //X2X
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeat, new MsgHandlerDelegate(X2X_HeartBeat));
            m_Handler.Add((ushort)Packet_X2X.Protocol.X2X_HeartBeatAck, new MsgHandlerDelegate(X2X_HeartBeatAck));

            //C2P
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestEnterServer, new MsgHandlerDelegate(C2P_RequestEnterServer));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestSendChatData, new MsgHandlerDelegate(C2P_RequestSendChatData));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestStageStart, new MsgHandlerDelegate(C2P_RequestStageStart));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestStageClear, new MsgHandlerDelegate(C2P_RequestStageClear));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestChatDataList, new MsgHandlerDelegate(C2P_RequestChatDataList));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestGoldGrowthUp, new MsgHandlerDelegate(C2P_RequestGoldGrowthUp));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestLevelGrowthUp, new MsgHandlerDelegate(C2P_RequestLevelGrowthUp));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestMissionReward, new MsgHandlerDelegate(C2P_RequestMissionReward));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestQuestCustomUpdate, new MsgHandlerDelegate(C2P_RequestQuestCustomUpdate));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestItemEquip, new MsgHandlerDelegate(C2P_RequestItemEquip));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestItemEnchant, new MsgHandlerDelegate(C2P_RequestItemEnchant));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestItemCombine, new MsgHandlerDelegate(C2P_RequestItemCombine));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestSelectItemEquipPreset, new MsgHandlerDelegate(C2P_RequestSelectItemEquipPreset));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestKnightEquip, new MsgHandlerDelegate(C2P_RequestKnightEquip));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestSelectKnightEquipPreset, new MsgHandlerDelegate(C2P_RequestSelectKnightEquipPreset));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPlayerPrefSave, new MsgHandlerDelegate(C2P_RequestPlayerPrefSave));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestSkillEquip, new MsgHandlerDelegate(C2P_RequestSkillEquip));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestSelectSkillEquipPreset, new MsgHandlerDelegate(C2P_RequestSelectSkillEquipPreset));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestKnightUpgrade, new MsgHandlerDelegate(C2P_RequestKnightUpgrade));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestItemCombineAll, new MsgHandlerDelegate(C2P_RequestItemCombineAll));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestGrowthLevelPointReset, new MsgHandlerDelegate(C2P_RequestGrowthLevelPointReset));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestStageLoopChange, new MsgHandlerDelegate(C2P_RequestStageLoopChange));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestItemGacha, new MsgHandlerDelegate(C2P_RequestItemGacha));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestItemBreakthrough, new MsgHandlerDelegate(C2P_RequestItemBreakthrough));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestRandomOptionChange, new MsgHandlerDelegate(C2P_RequestRandomOptionChange));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestRandomOptionLock, new MsgHandlerDelegate(C2P_RequestRandomOptionLock));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestGachaLevelReward, new MsgHandlerDelegate(C2P_RequestGachaLevelReward));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestItemConsume, new MsgHandlerDelegate(C2P_RequestItemConsume));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPostRead, new MsgHandlerDelegate(C2P_RequestPostRead));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPostBoxOpen, new MsgHandlerDelegate(C2P_RequestPostBoxOpen));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestItemSell, new MsgHandlerDelegate(C2P_RequestItemSell));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestSkillOpen, new MsgHandlerDelegate(C2P_RequestSkillOpen));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestSkillLevelUp, new MsgHandlerDelegate(C2P_RequestSkillLevelUp));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestChangeNickName, new MsgHandlerDelegate(C2P_RequestChangeNickName));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestUseCoupon, new MsgHandlerDelegate(C2P_RequestUseCoupon));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestNoticeList, new MsgHandlerDelegate(C2P_RequestNoticeList));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestGetRank, new MsgHandlerDelegate(C2P_RequestGetRank));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestRankReward, new MsgHandlerDelegate(C2P_RequestRankReward));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestMissionRewardList, new MsgHandlerDelegate(C2P_RequestMissionRewardList));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestViewUserInfo, new MsgHandlerDelegate(C2P_RequestViewUserInfo));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestStageSweep, new MsgHandlerDelegate(C2P_RequestStageSweep));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestRelicEnchant, new MsgHandlerDelegate(C2P_RequestRelicEnchant));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPassReward, new MsgHandlerDelegate(C2P_RequestPassReward));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPassRewardList, new MsgHandlerDelegate(C2P_RequestPassRewardList));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPvpStart, new MsgHandlerDelegate(C2P_RequestPvpStart));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPvpEnd, new MsgHandlerDelegate(C2P_RequestPvpEnd));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestUpgradeStageStart, new MsgHandlerDelegate(C2P_RequestUpgradeStageStart));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestUpgradeStageClear, new MsgHandlerDelegate(C2P_RequestUpgradeStageClear));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPostReward, new MsgHandlerDelegate(C2P_RequestPostReward));
            // m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestPostRemove, new MsgHandlerDelegate(C2P_RequestPostRemove));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestIAPTry, new MsgHandlerDelegate(C2P_RequestIAPTry));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestAdsBuffReward, new MsgHandlerDelegate(C2P_RequestAdsBuffReward));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestAdsBuffMaxLevelComplete, new MsgHandlerDelegate(C2P_RequestAdsBuffMaxLevelComplete));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestAdsBuffList, new MsgHandlerDelegate(C2P_RequestAdsBuffList));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestStageSkillEquip, new MsgHandlerDelegate(C2P_RequestStageSkillEquip));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestProfileChange, new MsgHandlerDelegate(C2P_RequestProfileChange));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestEventShopBuy, new MsgHandlerDelegate(C2P_RequestEventShopBuy));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestEventRouletteReward, new MsgHandlerDelegate(C2P_RequestEventRouletteReward));
            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestShopBuy, new MsgHandlerDelegate(C2P_RequestShopBuy));

            
            //


            m_Handler.Add((ushort)Packet_C2P.Protocol.C2P_RequestCheat, new MsgHandlerDelegate(C2P_RequestCheat));
            
            //M2P
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ResultConnect, new MsgHandlerDelegate(M2P_ResultConnect));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportSendChatData, new MsgHandlerDelegate(M2P_ReportSendChatData));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ResultChatList, new MsgHandlerDelegate(M2P_ResultChatList));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ResultEnterUser, new MsgHandlerDelegate(M2P_ResultEnterUser));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportUserKick, new MsgHandlerDelegate(M2P_ReportUserKick));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportNotice, new MsgHandlerDelegate(M2P_ReportNotice));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ResultNoticeList, new MsgHandlerDelegate(M2P_ResultNoticeList));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ResultGetRank, new MsgHandlerDelegate(M2P_ResultGetRank));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportTopRank, new MsgHandlerDelegate(M2P_ReportTopRank));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportHotTime, new MsgHandlerDelegate(M2P_ReportHotTime));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportHotTimeList, new MsgHandlerDelegate(M2P_ReportHotTimeList));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ResultPvpMatchStart, new MsgHandlerDelegate(M2P_ResultPvpMatchStart));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportServerServiceType, new MsgHandlerDelegate(M2P_ReportServerServiceType));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportInsertSystemPost, new MsgHandlerDelegate(M2P_ReportInsertSystemPost));
            m_Handler.Add((ushort)Packet_P2M.Protocol.M2P_ReportEvent, new MsgHandlerDelegate(M2P_ReportEvent));
            



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
    //Packet

    #region C2P
    public partial class CNetManager
    {
        public void C2P_RequestEnterServer(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestEnterServer();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            CLogger.Instance.Debug($"C2P_RequestEnterServer : {recvMsg.m_UID}");

            CDBManager.Instance.QueryCharacterCreate(sessionKey, recvMsg.m_UID, DefaultPlayerTable.Instance.ProfileID());
        }

        public void C2P_RequestSendChatData(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestSendChatData();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;


            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestSendChatData");

            P2M_ReportSendChatData(recvMsg.m_Chat);
        }

        public void C2P_RequestStageStart(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestStageStart();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestStageStart");

            Packet_Result.Result result = user.StageAgent.ReqStageStart(sessionKey, recvMsg.m_TID);
            if(result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultStageStart(sessionKey, new _StageData(), result);
        }

        public void C2P_RequestStageClear(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestStageClear();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestStageClear");

            Packet_Result.Result result = user.StageAgent.ReqStageClear(sessionKey, recvMsg.m_TID, recvMsg.m_IsClear);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultStageClear(sessionKey, new _StageData(), false , new List<_AssetData>(), result);
        }

        public void C2P_RequestChatDataList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestChatDataList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestChatDataList");

            CNetManager.Instance.P2M_RequestChatList(recvMsg.Type, user.UserData.m_UID);
        }

        public void C2P_RequestGoldGrowthUp(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestGoldGrowthUp();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestGoldGrowthUp");

            Packet_Result.Result result = user.LevelAgent.ReqGoldGrowthUp(sessionKey, recvMsg.m_GroupID, recvMsg.m_Level);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultGrowthGoldLevelUp(sessionKey, result, new _LevelData());
        }

        public void C2P_RequestLevelGrowthUp(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestLevelGrowthUp();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestLevelGrowthUp");

            Packet_Result.Result result = user.LevelAgent.ReqLevelGrowthUp(sessionKey, recvMsg.m_TableID, recvMsg.m_UseCount);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultLevelGrowthUp(sessionKey, new _LevelData(), result);
        }

        public void C2P_RequestMissionReward(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestMissionReward();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestMissionReward");

            Packet_Result.Result result = user.QuestAgent.ReqQuestMissionReward(sessionKey, recvMsg.m_QuestID, recvMsg.m_MissionID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultMissionReward(sessionKey, "", new _Mission(), new List<_AssetData>(), result);
        }

        public void C2P_RequestQuestCustomUpdate(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestQuestCustomUpdate();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestQuestCustomUpdate");

            var result = user.QuestAgent.ReqQuestCustomUpdate(sessionKey, recvMsg.m_MissionID);
            if (result != Packet_Result.Result.Success)
                P2C_ResultQuestCustomUpdate(sessionKey, new _Mission(), result);
        }

        public void C2P_RequestItemEquip(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestItemEquip();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemEquip");

            var result = user.ItemAgent.ReqItemEquip(sessionKey, recvMsg.m_PresetIdx, recvMsg.m_ItemID, recvMsg.m_IsEquip);
            if (result != Packet_Result.Result.Success)
                P2C_ResultItemEquip(sessionKey, new _ItemEquipPresetData(), result);
        }

        public void C2P_RequestSelectItemEquipPreset(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestSelectItemEquipPreset();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;
            
            CLogger.Instance.Debug($"C2P_RequestSelectItemEquipPreset");

            var result = user.ItemAgent.ReqSelectItemEquipPreset(sessionKey, recvMsg.m_Idx);
            if (result != Packet_Result.Result.Success)
                P2C_ResultSelectItemEquipPreset(sessionKey, -1, result);
        }

        public void C2P_RequestKnightEquip(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestKnightEquip();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestKnightEquip");

            var result = user.ItemAgent.ReqKnightEquip(sessionKey, recvMsg.m_PresetIdx, recvMsg.m_SlotNo, recvMsg.m_ItemID, recvMsg.m_IsEquip);
            if (result != Packet_Result.Result.Success)
                P2C_ResultKnightEquip(sessionKey, new _KnightEquipPresetData(), result);
        }

        public void C2P_RequestSelectKnightEquipPreset(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestSelectKnightEquipPreset();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestSelectKnightEquipPreset");

            var result = user.ItemAgent.ReqSelectKnightEquipPreset(sessionKey, recvMsg.m_Idx);
            if (result != Packet_Result.Result.Success)
                P2C_ResultSelectKnightEquipPreset(sessionKey, -1, result);
        }

        public void C2P_RequestItemEnchant(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestItemEnchant();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemEnchant");

            var result = user.ItemAgent.ReqItemEnchant(sessionKey, recvMsg.m_itemID, recvMsg.m_Level);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultItemEnchant(sessionKey, new _ItemData(), result);
        }

        public void C2P_RequestItemCombine(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestItemCombine();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemCombine");

            var result = user.ItemAgent.ReqItemCombine(sessionKey, recvMsg.m_ItemID, recvMsg.m_Count);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultItemCombine(sessionKey, new List<_ItemData>(), result);
        }

        
        public void C2P_RequestPlayerPrefSave(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestPlayerPrefSave();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestPlayerPrefSave");

            var result = user.ReqPlayerPrefSave(sessionKey, recvMsg.m_Pref);
            if (result != Packet_Result.Result.Success)
                P2C_ResultPlayerPrefSava(sessionKey, result);
        }

        public void C2P_RequestSkillEquip(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestSkillEquip();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestSkillEquip");

            var result = user.SkillAgent.ReqSkillEquip(sessionKey, recvMsg.m_PresetIdx, recvMsg.m_SlotNo, recvMsg.m_GroupID , recvMsg.m_IsEquip);
            if (result != Packet_Result.Result.Success)
                P2C_ResultSkillEquip(sessionKey, new _SkillEquipPresetData(), result);
        }

        public void C2P_RequestSelectSkillEquipPreset(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestSelectSkillEquipPreset();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestSelectSkillEquipPreset");

            var result = user.SkillAgent.ReqSelectSkillEquipPreset(sessionKey, recvMsg.m_Idx);
            if (result != Packet_Result.Result.Success)
                P2C_ResultSelectSkillEquipPreset(sessionKey, -1, result);
        }


        public void C2P_RequestCheat(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestCheat();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestCheat");

            var result = CCheatManager.Instance.ReqCheat(sessionKey, user, recvMsg.m_CMD, recvMsg.m_Args);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultCheat(sessionKey, result);
        }

        public void C2P_RequestKnightUpgrade(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestKnightUpgrade();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestKnightUpgrade");

            var result = user.ItemAgent.ReqKnightUpgrade(sessionKey, recvMsg.m_KnightUID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultKnightUpgrade(sessionKey, new _ItemData(), result);
        }

        public void C2P_RequestItemCombineAll(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestItemCombineAll();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemCombineAll");

            var result = user.ItemAgent.ReqItemCombineAll(sessionKey, recvMsg.m_ItemIDs);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultItemCombineAll(sessionKey, new List<_ItemData>(), result);
        }

        public void C2P_RequestGrowthLevelPointReset(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestGrowthLevelPointReset();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestGrowthLevelPointReset");

            var result = user.LevelAgent.ReqGrowthLevelPointReset(sessionKey, recvMsg.m_TableID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultGrowthLevelPointReset(sessionKey, new _LevelData(), result);
        }


        public void C2P_RequestStageLoopChange(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestStageLoopChange();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestStageLoopChange");

            var result = user.StageAgent.ReqStageLoopChange(sessionKey, recvMsg.m_Type, recvMsg.m_IsLoop);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultStageLoopChange(sessionKey, new _StageData(), result);
        }

        public void C2P_RequestItemGacha(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestItemGacha();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemGacha");

            var result = user.ItemAgent.ReqItemGahca(sessionKey, recvMsg.m_GachaID, recvMsg.m_Multiple);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultItemGacha(sessionKey, new _GachaData() , new List<_AssetData>(), -1, result);
        }

        public void C2P_RequestGachaLevelReward(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestGachaLevelReward();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemGacha");

            var result = user.ItemAgent.ReqItemGachaLevelReward(sessionKey, recvMsg.m_GachaID, recvMsg.m_RewardLv);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultGachaLevelReward(sessionKey, new _GachaData(), new List<_AssetData>(), result);
        }


        public void C2P_RequestItemBreakthrough(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestItemBreakthrough();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemBreakthrough");

            var result = user.ItemAgent.ReqItemBreakthrough(sessionKey, recvMsg.m_ReqUID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultItemBreakthrough(sessionKey, new _ItemData(), result);
        }

        public void C2P_RequestRandomOptionChange(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestRandomOptionChange();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestRandomOptionChange");

            var result = user.ItemAgent.ReqItemRandomOptionChange(sessionKey, recvMsg.m_ReqID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultRandomOptionChange(sessionKey, new _ItemData(), result);
        }

        public void C2P_RequestRandomOptionLock(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestRandomOptionLock();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestRandomOptionLock");

            var result = user.ItemAgent.ReqItemRandomOptionLock(sessionKey, recvMsg.m_ReqUID, recvMsg.m_RandomOptionIdx);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultRandomOptionLock(sessionKey, new _ItemData(), result);
        }

        public void C2P_RequestItemConsume(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestItemConsume();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemConsume");

            var result = user.ItemAgent.ReqItemConsume(sessionKey, recvMsg.m_ReqID, recvMsg.m_Cnt);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultItemConsume(sessionKey, new _ItemData(), new List<_AssetData>(), result);
        }

        public void C2P_RequestPostRead(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestPostRead();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestPostRead");
            var result = user.PostAgent.ReqPostRead(sessionKey, recvMsg.m_PostIDs);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultPostRead(sessionKey, new List<_PostData>(), result);
        }

        public void C2P_RequestPostBoxOpen(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestPostBoxOpen();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestPostBoxOpen");
            var result = user.PostAgent.ReqPostBoxOpen(sessionKey, recvMsg.m_UIOpen);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultPostBoxOpen(sessionKey, new List<_PostData>(), recvMsg.m_UIOpen, result);
        }

        public void C2P_RequestItemSell(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestItemSell();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestItemSell");

            var result = user.ItemAgent.ReqItemSell(sessionKey, recvMsg.m_ReqID, recvMsg.m_Cnt);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultItemSell(sessionKey, new _ItemData(), new List<_AssetData>(), result);
        }

        public void C2P_RequestSkillOpen(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestSkillOpen();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestSkillOpen");
            var result = user.SkillAgent.ReqSkillOpen(sessionKey, recvMsg.m_GroupID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultSkillOpen(sessionKey, new _SkillData(), result);
        }

        public void C2P_RequestSkillLevelUp(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestSkillLevelUp();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestSkillLevelUp");
            var result = user.SkillAgent.ReqSkillLevelUp(sessionKey, recvMsg.m_GroupID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultSkillLevelUp(sessionKey, new _SkillData(), result);
        }

        public void C2P_RequestChangeNickName(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestChangeNickName();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            var result = user.ReqChangeNickName(sessionKey, recvMsg.m_Name);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultChangeNickName(sessionKey, new _UserData(), result);
        }

        public void C2P_RequestUseCoupon(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestUseCoupon();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.Debug($"C2P_RequestUseCoupon");
            var result = user.CouponAgent.ReqUseCoupon(sessionKey, recvMsg.m_CouponID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultUseCoupon(sessionKey, new List<_AssetData>(), result);
        }

        public void C2P_RequestNoticeList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestNoticeList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.System($"C2P_RequestNoticeList");
            CNetManager.Instance.P2M_RequestNoticeList(sessionKey);
        }

        public void C2P_RequestGetRank(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestGetRank();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            user.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Check_Ranking, "", recvMsg.m_RankType.ToString(), 1);
            CNetManager.Instance.P2M_RequestGetRank(sessionKey, recvMsg.m_RankType, user.UID);
        }
        
        public void C2P_RequestRankReward(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestRankReward();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.System($"C2P_RequestRankReward");
            var result = user.ReqRankReward(sessionKey, recvMsg.m_RankType);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultRankReward(sessionKey, new _RankReward(), new List<_AssetData>(), result);
        }

        public void C2P_RequestMissionRewardList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestMissionRewardList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            CLogger.Instance.System($"C2P_RequestMissionRewardList");
            var result = user.QuestAgent.ReqQuestMissionRewardList(sessionKey, recvMsg.m_QuestID, recvMsg.m_MissionIDs);
            if(result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultMissionRewardList(sessionKey, "", new List<_Mission>(), new List<_AssetData>(), result);
        }

        public void C2P_RequestViewUserInfo(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestViewUserInfo();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            if(user.UID == recvMsg.m_TargetUID)
            {
                CLogger.Instance.System($"C2P_RequestViewUserInfo user.GetUID() == recvMsg.m_TargetUID : {recvMsg.m_TargetUID}");
                CNetManager.Instance.P2C_ResultViewUserInfo(sessionKey, "", -1, -1, -1, new _EquipViewData(), Packet_Result.Result.IgnoreError);
            }

            CDBManager.Instance.QueryCharacterViewUserInfo(user.DBGUID, sessionKey, recvMsg.m_TargetUID);
        }

        public void C2P_RequestStageSweep(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestStageSweep();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            var result = user.StageAgent.ReqStageSweep(sessionKey, recvMsg.m_Type, recvMsg.m_StageID, recvMsg.m_Count);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultStageSweep(sessionKey, result);
        }

        public void C2P_RequestRelicEnchant(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestRelicEnchant();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            var result = user.RelicAgent.ReqRelicEnchant(sessionKey, recvMsg.m_GroupID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultRelicEnchant(sessionKey, new _RelicData(), result);
        }

        public void C2P_RequestPassReward(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestPassReward();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            var result = user.QuestAgent.ReqPassReward(sessionKey, recvMsg.m_QuestID, recvMsg.m_PassMissionID, recvMsg.m_PassReward);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultPassReward(sessionKey, "", new _Mission(), new List<_AssetData>(), result);
        }

        public void C2P_RequestPassRewardList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestPassRewardList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            var result = user.QuestAgent.ReqPassRewardList(sessionKey, recvMsg.m_QuestID, recvMsg.m_PassReward);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultPassRewardList(sessionKey, "", new List<_Mission>(), new List<_AssetData>(), result);
        }

        public void C2P_RequestPvpStart(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestPvpStart();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null)
                return;

            var result = user.PvpAgent.ReqPvpStart(sessionKey);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultPvpStart(sessionKey, new List<_PvPUserData>(), result);
        }

        public void C2P_RequestPvpEnd(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestPvpEnd();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.PvpAgent.ReqPvpEnd(sessionKey, recvMsg.m_IsWin);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultPvpEnd(sessionKey, -1, -1, false, new List<_AssetData>(), result);
        }

        public void C2P_RequestUpgradeStageStart(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestUpgradeStageStart();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.StageAgent.ReqUpgradeStageStart(sessionKey, recvMsg.m_TID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultUpgradeStageStart(sessionKey, new _StageData(), result);
        }

        public void C2P_RequestUpgradeStageClear(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestUpgradeStageClear();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.StageAgent.ReqUpgradeStageClear(sessionKey, recvMsg.m_TID, recvMsg.m_IsClear);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultUpgradeStageClear(sessionKey, new _StageData(), false, new List<_AssetData>(), result);
        }

        public void C2P_RequestPostReward(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestPostReward();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.PostAgent.ReqPostReward(sessionKey, recvMsg.m_PostIDs);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultPostReward(sessionKey, new List<_PostData>(), new List<_AssetData>(), result);

        }

        //public void C2P_RequestPostRemove(long sessionKey, byte[] packet)
        //{
        //    var recvMsg = new Packet_C2P.C2P_RequestPostRemove();
        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

        //    var user = CUserManager.Instance.FindbySessionKey(sessionKey);
        //    if (user == null || !user.IsLogin())
        //        return;

        //    //var result = user.PostAgent.ReqPostRemove(sessionKey, recvMsg.m_RemoveIDs);
        //    //if (result != Packet_Result.Result.Success)
        //    //    CNetManager.Instance.P2C_ResultPostRemove(sessionKey, new List<_PostData>(), result);
        //}

        public void C2P_RequestIAPTry(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestIAPTry();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var log = LogHelper.MakeLogBson(eLogType.iap_try, user.UserData, "", null, 1);
            CRestManager.Instance.RestIAP(sessionKey, user.UID, recvMsg.m_Receipt, log);

            //user.ShopAgent.IAPTest(sessionKey, recvMsg.m_Receipt);
        }

        public void C2P_RequestAdsBuffReward(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestAdsBuffReward();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.BuffAgent.ReqAdsBuffReward(sessionKey, recvMsg.m_AdsBuffID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultAdsBuffReward(sessionKey, new _AdsBUffData(), result);
        }

        public void C2P_RequestAdsBuffMaxLevelComplete(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestAdsBuffMaxLevelComplete();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.BuffAgent.ReqAdsBuffMaxLevelComplete(sessionKey);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultAdsBuffMaxLevelComplete(sessionKey, result);
        }

        public void C2P_RequestAdsBuffList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestAdsBuffList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.BuffAgent.ReqAdsBuffList(sessionKey);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultAdsBuffList(sessionKey, new List<_AdsBUffData>(), result);
        }

        public void C2P_RequestStageSkillEquip(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestStageSkillEquip();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.SkillAgent.ReqStageSkillEquip(sessionKey, recvMsg.m_StageSkill);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultStageSkillEquip(sessionKey, new _StageSkillData(), result);
        }

        public void C2P_RequestProfileChange(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestProfileChange();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.ReqProfileChange(sessionKey, recvMsg.m_ProfileID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultProfileChange(sessionKey, -1, result);
        }

        public void C2P_RequestEventShopBuy(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestEventShopBuy();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.ShopAgent.ReqEventShopBuy(sessionKey, recvMsg.m_EventUID, recvMsg.m_EventShopID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultEventShopBuy(sessionKey, new _EventShopData(), result);
        }

        public void C2P_RequestEventRouletteReward(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestEventRouletteReward();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.EventAgent.ReqEventRouletteReward(sessionKey, recvMsg.m_EventUID);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultEventRouletteReward(sessionKey, -1, new _EventRouletteData(), result);
        }

        public void C2P_RequestShopBuy(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_C2P.C2P_RequestShopBuy();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbySessionKey(sessionKey);
            if (user == null || !user.IsLogin())
                return;

            var result = user.ShopAgent.ReqShopBuy(sessionKey, recvMsg.m_ShopID, recvMsg.m_Count);
            if (result != Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultShopBuy(sessionKey, new _ShopData(), result);
        }
    }

    #endregion

    #region M2P
    public partial class CNetManager
    {
        public void M2P_ReportSendChatData(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportSendChatData();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (recvMsg.m_Chat.type == CDefine.ChatType.All)
            {
                CNetManager.Instance.P2C_ReportSendChatData(-1, recvMsg.m_Chat);
            }
            else
            {
                var receiver = CUserManager.Instance.FindbyUID(recvMsg.m_Chat.receiver);
                if (receiver == null)
                    return;

                CNetManager.Instance.P2C_ReportSendChatData(receiver.SessionKey, recvMsg.m_Chat);

                var sender = CUserManager.Instance.FindbyUID(recvMsg.m_Chat.sender);
                if (sender == null)
                    return;

                CNetManager.Instance.P2C_ReportSendChatData(sender.SessionKey, recvMsg.m_Chat);
            }
        }

        public void M2P_ResultConnect(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ResultConnect();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;


            CEventManager.Instance.SetActiveEvent(recvMsg.m_ActiveEvents);
        }

        public void M2P_ResultChatList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ResultChatList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            var user = CUserManager.Instance.FindbyUID(recvMsg.m_RequestUID);
            if (user == null)
                return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;


            CNetManager.Instance.P2C_ResultChatList(user.SessionKey, recvMsg.m_ChatRooms);
        }

        public void M2P_ResultEnterUser(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ResultEnterUser();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
                CNetManager.Instance.P2C_ResultEnterServer(recvMsg.m_UserSession, recvMsg.m_Result);
            else
                CDBManager.Instance.QueryCharacterLoad(recvMsg.m_UserSession, recvMsg.m_UID, recvMsg.m_DeviceID);
        }

        public void M2P_ReportUserKick(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportUserKick();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            CUserManager.Instance.Kick(recvMsg.m_TargetID, (Packet_Result.Result)recvMsg.m_Result);
        }

        public void M2P_ReportNotice(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportNotice();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;


            foreach(var iter in CUserManager.Instance.UsersbySession)
            {
                var user = iter.Value;
                if (!recvMsg.m_IsErase)
                    CNetManager.Instance.P2C_ReportNotice(user.SessionKey, recvMsg.m_NoticeData);
                else
                    CNetManager.Instance.P2C_ReportNoticeErase(user.SessionKey, recvMsg.m_NoticeData.m_ID);
            }
        }

        public void M2P_ResultNoticeList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ResultNoticeList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            CNetManager.Instance.P2C_ReportNoticeList(recvMsg.m_UserSession, recvMsg.m_Notices);
        } 

        public void M2P_ResultGetRank(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ResultGetRank();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            CNetManager.Instance.P2C_ResultGetRank(recvMsg.m_UserSession, recvMsg.m_Type, recvMsg.m_ReqUserRank, recvMsg.m_Ranks, (Packet_Result.Result)recvMsg.m_Result);
        }

        public void M2P_ReportTopRank(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportTopRank();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            //
        }

        public void M2P_ReportHotTime(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportHotTime();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            CLogger.Instance.System($"M2P_ReportHotTime");

            if (!recvMsg.m_Remove)
                CHotTimeManager.Instance.Insert(recvMsg.m_HotTimeData);
            else
                CHotTimeManager.Instance.Remove(recvMsg.m_HotTimeData);
        }

        public void M2P_ReportHotTimeList(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportHotTimeList();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            CLogger.Instance.System($"M2P_ReportHotTimeList");

            foreach (var iter in recvMsg.m_Datas)
                CHotTimeManager.Instance.Insert(iter);
        }

        public void M2P_ResultPvpMatchStart(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ResultPvpMatchStart();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            CLogger.Instance.System($"M2P_ResultPvpMatchStart");

            CNetManager.Instance.P2C_ResultPvpStart(recvMsg.m_UserSession, recvMsg.m_PvpUserDatas, (Packet_Result.Result)recvMsg.m_Result);
        }

        public void M2P_ReportServerServiceType(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportServerServiceType();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            if (!recvMsg.m_Open)
            {
                foreach(var iter in CUserManager.Instance.UsersbyUID)
                {
                    CUser user = iter.Value;
                    //todo : normal user kick
                    P2C_ReportKick(user.SessionKey, Packet_Result.Result.NotServiceError);
                }
            }
        }

        public void M2P_ReportInsertSystemPost(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportInsertSystemPost();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;

            foreach(var iter in CUserManager.Instance.UsersbyUID)
            {
                CUser user = iter.Value;
                user.PostAgent.RepInsertSystemPost(recvMsg.m_Data);
            }
        }

        public void M2P_ReportEvent(long sessionKey, byte[] packet)
        {
            var recvMsg = new Packet_P2M.M2P_ReportEvent();
            if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

            if (sessionKey != m_MessageServerSession.SessionKey)
                return;


            if(recvMsg.m_Remove)
                CEventManager.Instance.Remove(recvMsg.m_EventData.m_UID);
            else
                CEventManager.Instance.Insert(recvMsg.m_EventData);
        }
    }

    #endregion

    #region P2G
    public partial class CNetManager
    {
        //public void G2P_ResultServerConnect(long sessionKey, byte[] packet)
        //{
        //    var recvMsg = new Packet_P2G.G2P_ResultServerConnect();
        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

        //    //nothing
        //}

        //public void G2P_ResultIdentifyUser(long sessionKey, byte[] packet)
        //{
        //    var recvMsg = new Packet_P2G.G2P_ResultIdentifyUser();
        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

        //    if (m_GateServerSession.SessionKey != sessionKey)
        //        return;

        //    if (recvMsg.m_Result != (ushort)Packet_Result.Result.Success)
        //        CNetManager.Instance.P2C_ResultEnterServer(recvMsg.m_UserSessionKey, recvMsg.m_Result);
        //    else
        //        CDBManager.Instance.QueryCharacterCreate(recvMsg.m_UserSessionKey, recvMsg.m_Data.m_UID, recvMsg.m_Data.m_DeviceID);
        //}

        //public void G2P_ReportUserKick(long sessionKey, byte[] packet)
        //{
        //    var recvMsg = new Packet_P2G.G2P_ReportUserKick();
        //    if (!m_NetSystem.Read(sessionKey, recvMsg, packet)) return;

        //    if (m_GateServerSession.SessionKey != sessionKey)
        //        return;

        //    CUserManager.Instance.Kick(recvMsg.m_KickUser.m_UID, (Packet_Result.Result)recvMsg.m_Result);
        //}
    }

    #endregion
}
