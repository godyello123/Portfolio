using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms;
using Global;
using Packet_Result;
using SCommon;
using ZstdSharp.Unsafe;

namespace PlayServer
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

        //C2P
        #region C2P
        public void P2C_ResultEnterServer(long sessionKey, ushort result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultEnterServer();
            sendMsg.m_Result = result;

            CLogger.Instance.Debug($"P2C_ResultEnterServer : {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportSendChatData(long sessionKey, _ChatData chatData)
        {
            var sendMsg = new Packet_C2P.P2C_ReportSendChatData();
            sendMsg.m_Chat = chatData;

            CLogger.Instance.Debug($"P2C_ReportSendChatData : {sessionKey}");


            if (sessionKey == -1)
                WriteAllToPlayer(sendMsg);
            else
                Write(sessionKey, sendMsg);
        }

        public void P2C_ReportBeginLoginData(long sessionKey)
        {
            var sendMsg = new Packet_C2P.P2C_ReportBeginLoginData();
            CLogger.Instance.Debug($"P2C_ReportBeginLoginData : {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportEndLoginData(long sessionKey)
        {
            var sendMsg = new Packet_C2P.P2C_ReportEndLoginData();
            CLogger.Instance.Debug($"P2C_ReportEndLoginData : {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportUserData(long sessionKey, _UserData userData)
        {
            var sendMsg = new Packet_C2P.P2C_ReportUserData();
            sendMsg.m_userData = userData;

            CLogger.Instance.Debug($"P2C_ReportUserData : {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportActorData(long sessionKey, List<_ActorData> actors)
        {
            var sendMsg = new Packet_C2P.P2C_ReportActorData();
            sendMsg.m_ActorDatas = actors;

            CLogger.Instance.Debug($"P2C_ReportActorData : {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportSkillData(long sessionKey, List<_SkillData> skills)
        {
            var sendMsg = new Packet_C2P.P2C_ReportSkillData();
            sendMsg.m_SkillDatas = skills;

            CLogger.Instance.Debug($"P2C_ReportSkillData : {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultStageStart(long sessionKey, _StageData stageData, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultStageStart();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = stageData;

            CLogger.Instance.Debug($"P2C_ResultStageStart : {sessionKey}");


            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultStageClear(long sessionKey, _StageData stageData, bool isClear, List<_AssetData> assetDatas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultStageClear();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_IsClear = isClear;
            sendMsg.m_AssetDatas = assetDatas;
            sendMsg.m_StageData = stageData;

            CLogger.Instance.Debug($"P2C_ResultStageClear : {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportRewardData(long sessionKey, List<_AssetData> assetDatas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportRewardData();
            sendMsg.m_AssetDatas = assetDatas;

            CLogger.Instance.Debug($"P2C_ReportRewardData : {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultChatList(long sessionKey, List<_ChatRoomData> roomDatas)
        {
            var sendMsg = new Packet_C2P.P2C_ResultChatDataList();
            sendMsg.m_ChatRooms = roomDatas;

            CLogger.Instance.Debug($"P2C_ResultChatList : {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultGrowthGoldLevelUp(long sessionKey, Packet_Result.Result result, _LevelData levelData)
        {
            var sendMsg = new Packet_C2P.P2C_ResultGoldGrowthUp();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = levelData;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportAbilData(long sessionKey, long guid, List<_AbilData> abils)
        {
            var sendMsg = new Packet_C2P.P2C_ReportAbilData();
            sendMsg.m_GUID = guid;
            foreach(var abil in abils)
            {
                if (abil.type == CDefine.EAbility.HP)
                    continue;

                sendMsg.m_Abils.Add(abil);
            }
            
            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportAbilData(long sessionKey, long guid, Dictionary<CDefine.EAbility, _AbilData> abils)
        {
            var sendMsg = new Packet_C2P.P2C_ReportAbilData();
            sendMsg.m_GUID = guid;
            foreach (var iter in abils)
            {
                var abil = iter.Value;
                if (abil.type == CDefine.EAbility.HP)
                    continue;

                sendMsg.m_Abils.Add(abil);
            }

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportAssetData(long sessionKey, List<_AssetData> assetDatas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportAssetData();
            sendMsg.m_AssetDatas = assetDatas;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultLevelGrowthUp(long sessionKey, _LevelData levelData, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultLevelGrowthUp();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = levelData;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultCheat(long sessionKey, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultCheat();
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportLoginData(long sessionKey, _UserData userData, List<_StageData> stageDatas, List<_LevelData> growthGolds, List<_LevelData> growthLevel,
            List<_AssetData> assetDatas, List<_ItemEquipPresetData> itempresets, List<_KnightEquipPresetData> knightpresets, List<_GachaData> gachaDatas,
            List<_SkillData> skillDatas, List<_SkillEquipPresetData> skillpreset, _RankReward rankreward, List<_RelicData> relics)
        {
            var sendMsg = new Packet_C2P.P2C_ReportLoginData();
            sendMsg.m_userData = userData;
            sendMsg.m_StageDatas = stageDatas;
            sendMsg.m_GrowthGolds = growthGolds;
            sendMsg.m_GrowthLevels = growthLevel;
            sendMsg.m_AssetDatas = assetDatas;
            sendMsg.m_ItemEquipPresets = itempresets;
            sendMsg.m_KnightEquipPresets = knightpresets;
            sendMsg.m_SkillEquipPresets = skillpreset;
            sendMsg.m_GachaDatas = gachaDatas;
            sendMsg.m_SkillDatas = skillDatas;
            sendMsg.m_RankReward = rankreward;
            sendMsg.m_RelicDatas = relics;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportQuestMissionData(long sessionKey, CDefine.Modify modify, string questID, _Mission mission)
        {
            var sendMsg = new Packet_C2P.P2C_ReportQuestMissionData();
            sendMsg.m_QuestID = questID;
            sendMsg.m_Modify = modify;
            sendMsg.m_Mission = SCopy<_Mission>.DeepCopy(mission);

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultMissionReward(long sessionKey, string questID, _Mission mission, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultMissionReward();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Mission = mission;
            sendMsg.m_Rewards = rewards;
            sendMsg.m_QuestID = questID;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportKick(long sessionKey, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ReportKick();
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultQuestCustomUpdate(long sessionKey, _Mission mission, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultQuestCustomUpdate();
            sendMsg.m_Mission = mission;
            sendMsg.m_Result = (ushort)result;

            CLogger.Instance.Debug($"P2C_ResultQuestCustomUpdate : {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultItemEquip(long sessionKey, _ItemEquipPresetData preset, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultItemEquip();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_PresetData = preset;

            CLogger.Instance.Debug($"P2C_ResultItemEquip: {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportItemEquipPresetData(long sessionKey, _ItemEquipPresetData presets)
        {
            var sendMsg = new Packet_C2P.P2C_ReportItemEquipPresetData();
            sendMsg.m_Preset = presets;

            CLogger.Instance.Debug($"P2C_ReportItemEquipPresetData: {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultSelectItemEquipPreset(long sessionKey, int idx, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultSelectItemEquipPreset();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_PresetIdx = idx;

            CLogger.Instance.Debug($"P2C_ResultSelectItemEquipPreset, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultSelectKnightEquipPreset(long sessionKey, int idx, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultSelectKnightEquipPreset();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_PresetIdx = idx;

            CLogger.Instance.Debug($"P2C_ResultSelectKnightEquipPreset, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultKnightEquip(long sessionKey, _KnightEquipPresetData preset, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultKnightEquip();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_PresetData = preset;

            CLogger.Instance.Debug($"P2C_ResultKnightEquip, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultKnightUpgrade(long sessionKey, _ItemData data, Packet_Result.Result _result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultKnightUpgrade();
            sendMsg.m_Data = data;
            sendMsg.m_Result = (ushort)_result;

            CLogger.Instance.Debug($"P2C_ResultKnightUpgrade , {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportKnightEquipPresetData(long sessionKey, _KnightEquipPresetData preset)
        {
            var sendMsg = new Packet_C2P.P2C_ReportKnightEquipPresetData();
            sendMsg.m_Preset = preset;

            CLogger.Instance.Debug($"P2C_ResultKnightUpgrade , {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportKnightEquipPresetData(long sessionKey, List<_KnightEquipPresetData> presets)
        {
            if (presets.Count < 1)
                return;

            foreach (var it in presets)
                P2C_ReportKnightEquipPresetData(sessionKey, it);
        }

        public void P2C_ResultItemEnchant(long sessionKey, _ItemData item, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultItemEnchant();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Item = item;

            CLogger.Instance.Debug($"P2C_ResultItemEnchant, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultItemCombine(long sessionKey, List<_ItemData> items, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultItemCombine();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Items = items;

            CLogger.Instance.Debug($"P2C_ResultItemCombine, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportItemDataList(long sessionKey, List<_ItemData> items)
        {
            var sendMsg = new Packet_C2P.P2C_ReportItemDataList();
            sendMsg.m_Items = items;

            CLogger.Instance.Debug($"P2C_ReportItemDataList, {sessionKey}");

            Write(sessionKey, sendMsg);
        }


        public void P2C_ReportPlayerPref(long sessionKey, List<_PlayerPref> prefs)
        {
            var sendMsg = new Packet_C2P.P2C_ReportPlayerPref();
            sendMsg.m_Prefs = prefs;

            CLogger.Instance.Debug($"P2C_ReportPlayerPref, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportPlayerPref(long sessionKey, Dictionary<CDefine.PlayerPref, _PlayerPref> prefs)
        {
            var sendMsg = new Packet_C2P.P2C_ReportPlayerPref();
            foreach (var iter in prefs)
                sendMsg.m_Prefs.Add(iter.Value);

            CLogger.Instance.Debug($"P2C_ReportPlayerPref,  {sessionKey}");

            Write(sessionKey, sendMsg);
        }


        public void P2C_ReportQuestBoardList(long sessionKey, List<_QuestBoard> boardList)
        {
            var sendMsg = new Packet_C2P.P2C_ReportQuestBoardList();
            sendMsg.m_QuestBorads = boardList;

            CLogger.Instance.Debug($"P2C_ReportQuestBoardList, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultSkillEquip(long sessionKey, _SkillEquipPresetData preset, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultSkillEquip();
            sendMsg.m_PresetData = preset;
            sendMsg.m_Result = (ushort)result;

            CLogger.Instance.Debug($"P2C_ResultSkillEquip,  {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultSelectSkillEquipPreset(long sessionKey, int idx, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultSelectSkillEquipPreset();
            sendMsg.m_PresetIdx = idx;
            sendMsg.m_Result = (ushort)result;

            CLogger.Instance.Debug($"P2C_ResultSelectSkillEquipPreset , {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultPlayerPrefSava(long sessionKey, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultPlayerPrefSava();
            sendMsg.m_Result = (ushort)result;

            CLogger.Instance.Debug($"P2C_ResultPlayerPrefSava, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportItem(long sessionKey, _ItemData item)
        {
            var sendMsg = new Packet_C2P.P2C_ReportItem();
            sendMsg.m_Item = item;

            CLogger.Instance.Debug($"P2C_ReportItemm, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultItemCombineAll(long sessionKey, List<_ItemData> items, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultItemCombineAll();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Items = items;

            CLogger.Instance.Debug($"P2C_ResultItemCombineAll, {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultGrowthLevelPointReset(long _sessionKey, _LevelData _levelData, Packet_Result.Result _result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultGrowthLevelPointReset();
            sendMsg.m_Result = (ushort)_result;
            sendMsg.m_Data = _levelData;

            CLogger.Instance.Debug($"P2C_ResultGrowthLevelPointReset, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultStageLoopChange(long _sessionKey, _StageData _stageData, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultStageLoopChange();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = _stageData;

            CLogger.Instance.Debug($"P2C_ResultStageLoopChange, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultItemGacha(long _sessionKey, _GachaData _gachaData, List<_AssetData> rewards, int gachaTID, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultItemGacha();
            sendMsg.m_Gacha = _gachaData;
            sendMsg.m_GachaRewards = rewards;
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_GachaID = gachaTID;

            CLogger.Instance.Debug($"P2C_ResultItemGacha, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultGachaLevelReward(long _sessionKey, _GachaData _gachaData, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultGachaLevelReward();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = _gachaData;
            sendMsg.m_Rewards = rewards;

            CLogger.Instance.Debug($"P2C_ResultGachaLevelReward, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultItemBreakthrough(long _sessionKey, _ItemData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultItemBreakthrough();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = data;

            CLogger.Instance.Debug($"P2C_ResultRandomOptionChange, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultRandomOptionChange(long _sessionKey, _ItemData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultRandomOptionChange();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = data;

            CLogger.Instance.Debug($"P2C_ResultRandomOptionChange, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultRandomOptionLock(long _sessionKey, _ItemData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultRandomOptionLock();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = data;

            CLogger.Instance.Debug($"P2C_ResultRandomOptionLock, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultItemConsume(long _sessionKey, _ItemData data, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultItemConsume();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_ItemData = data;
            sendMsg.m_Rewards = rewards;

            CLogger.Instance.Debug($"P2C_ResultItemConsume, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultPostBoxOpen(long _sessionKey, List<_PostData> posts, bool clientFlag, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultPostBoxOpen();
            sendMsg.m_PostBox = posts;
            sendMsg.m_UIOpen = clientFlag;
            sendMsg.m_Result = (ushort)result;

            CLogger.Instance.Debug($"P2C_ResultPostBoxOpen, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultPostReward(long _sessionKey, List<_PostData> postDatas, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultPostReward();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Posts = postDatas;
            sendMsg.m_Rewards = rewards;

            CLogger.Instance.Debug($"P2C_ResultPostRead, {_sessionKey}");

            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultPostRead(long sessionKey, List<_PostData> data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultPostRead();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Posts = data;

            Write(sessionKey, sendMsg);
        }

        //public void P2C_ResultPostRemove(long sessionKey, List<_PostData> data, Packet_Result.Result result)
        //{
        //    var sendMsg = new Packet_C2P.P2C_ResultPostRemove();
        //    sendMsg.m_Posts = data;
        //    sendMsg.m_Result = (ushort)result;

        //    Write(sessionKey, sendMsg);
        //}

        public void P2C_ResultItemSell(long _sessionKey, _ItemData data, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultItemSell();
            sendMsg.m_Rewards = rewards;
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = data;

            CLogger.Instance.Debug($"P2C_ResultItemSell, {_sessionKey}");
            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultSkillOpen(long _sessionKey, _SkillData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultSkillOpen();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_SkillData = data;

            CLogger.Instance.Debug($"P2C_ResultSkillOpen, {_sessionKey}");
            Write(_sessionKey, sendMsg);
        }

        public void P2C_ResultSkillLevelUp(long sessionKey, _SkillData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultSkillLevelUp();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_SkillData = data;

            CLogger.Instance.Debug($"P2C_ResultSkillLevelUp, {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultChangeNickName(long sessionKey, _UserData userData, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultChangeNickName();
            sendMsg.m_UserData = userData;
            sendMsg.m_Result = (ushort)result;

            CLogger.Instance.Debug($"P2C_ResultChangeNickName, {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultUseCoupon(long sessionKey, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultUseCoupon();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Rewards = rewards;

            CLogger.Instance.Debug($"P2C_ResultChangeNickName, {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportNotice(long sessionKey, _NoticeData data)
        {
            var sendMsg = new Packet_C2P.P2C_ReportNotice();
            sendMsg.m_NoticeData = data;

            CLogger.Instance.Debug($"P2C_ReportNotice, {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportNoticeErase(long sessionKey, long deleteID)
        {
            var sendMsg = new Packet_C2P.P2C_ReportNoticeErase();
            sendMsg.m_DeleteID = deleteID;

            CLogger.Instance.Debug($"P2C_ReportNoticeErase, {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportNoticeList(long sessionKey, List<_NoticeData> datas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportNoticeList();
            sendMsg.m_NoticeList = datas;

            CLogger.Instance.Debug($"P2C_ReportNoticeList, {sessionKey}");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultNoticeList(long sessionKey, List<_NoticeData> data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultNoticeList();
            sendMsg.m_Data = data;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultGetRank(long sessionKey, CDefine.ERankType type, _RankData rankData, List<_RankData> ranks, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultGetRank();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Ranks = ranks;
            sendMsg.m_userRank = rankData;
            sendMsg.m_RankType = type;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultRankReward(long sessionKey, _RankReward data, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultRankReward();
            sendMsg.m_RankRewardData = data;
            sendMsg.m_Rewards = rewards;
            sendMsg.m_Result = (ushort)result;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportRankReward(long sessionKey, _RankReward data)
        {
            var sendMsg = new Packet_C2P.P2C_ReportRankReward();
            sendMsg.m_RankReward = data;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportHotTime(long sessionKey, _HotTimeData data, bool remove)
        {
            var sendMsg = new Packet_C2P.P2C_ReportHotTime();
            sendMsg.m_HotTimeData = data;
            sendMsg.m_Remove = remove;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportHotTimeList(long sessionKey, List<_HotTimeData> datas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportHotTimeList();
            sendMsg.m_HotTimes = datas;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportHotTimeList(long sessionKey, Dictionary<long, _HotTimeData> datas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportHotTimeList();

            foreach (var iter in datas)
            {
                var itVal = iter.Value;
                sendMsg.m_HotTimes.Add(SCopy<_HotTimeData>.DeepCopy(itVal));
            }

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultMissionRewardList(long sessionKey, string questID, List<_Mission> missions, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultMissionRewardList();
            sendMsg.m_Missions = missions;
            sendMsg.m_Rewards = rewards;
            sendMsg.m_QuestID = questID;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportQuestMissionDataList(long sessionKey, string questID, CDefine.Modify modify, List<_Mission> missions)
        {
            var sendMsg = new Packet_C2P.P2C_ReportQuestMissionDataList();
            sendMsg.m_QuestID = questID;
            sendMsg.m_Modify = modify;
            sendMsg.m_Missions = missions;


            CLogger.Instance.System($"P2C_ReportQuestMissionDataList");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultViewUserInfo(long sessionKey, string name, int level, int stageTID, int profileid, _EquipViewData equipView, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultViewUserInfo();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Name = name;
            sendMsg.m_Level = level;
            sendMsg.m_StageTID = stageTID;
            sendMsg.m_EquipView = equipView;
            sendMsg.m_ProfileID = profileid;

            CLogger.Instance.System($"P2C_ResultViewUserInfo");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultStageSweep(long sessionKey, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultStageSweep();
            sendMsg.m_Result = (ushort)result;
            
            CLogger.Instance.System($"P2C_ResultStageSweep");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultRelicEnchant(long sessionKey, _RelicData relic, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultRelicEnchant();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_RelicData = relic;

            CLogger.Instance.System($"P2C_ResultRelicEnchant");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultPassReward(long sessionKey, string questID, _Mission mission, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultPassReward();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Mission = mission;
            sendMsg.m_QuestID = questID;
            sendMsg.m_Rewards = rewards;

            CLogger.Instance.System($"P2C_ResultPassReward");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultPassRewardList(long sessionKey, string questID, List<_Mission> missions, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultPassRewardList();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Missions = missions;
            sendMsg.m_QuestID = questID;
            sendMsg.m_Rewards = rewards;

            CLogger.Instance.System($"P2C_ResultPassRewardList");
            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportPassActive(long sessionKey, string questID, bool isActive)
        {
            var sendMsg = new Packet_C2P.P2C_ReportPassActive();
            sendMsg.m_QuestID = questID;
            sendMsg.m_IsPassActive = isActive;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultPvpStart(long sessionKey, List<_PvPUserData> datas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultPvpStart();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_PvpUserDatas = datas;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultPvpEnd(long sessionKey, long beforePoint, long afterPoint, bool iswin, List<_AssetData> rewards, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultPvpEnd();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_BeforePvpPoint = beforePoint;
            sendMsg.m_AfterPvpPoint = afterPoint;
            sendMsg.m_IsWin = iswin;
            sendMsg.m_Rewards = rewards;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultUpgradeStageStart(long sessionKey, _StageData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultUpgradeStageStart();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_Data = data;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultUpgradeStageClear(long sessionKey, _StageData stageData, bool isClear, List<_AssetData> assetDatas, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultUpgradeStageClear();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_IsClear = isClear;
            sendMsg.m_AssetDatas = assetDatas;
            sendMsg.m_StageData = stageData;

            CLogger.Instance.Debug($"P2C_ResultUpgradeStageClear : {sessionKey}");

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultIAPTry(long sessionKey, _ShopData shopData, string productID, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultIAPTry();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_IAPProductID = productID;
            sendMsg.m_Data = shopData;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportUserPost(long sessionKey, _PostData postData)
        {
            var sendMsg = new Packet_C2P.P2C_ReportUserPost();
            sendMsg.m_Data = postData;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportAdsBuffData(long sessionKey, List<_AdsBUffData> datas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportAdsBuffData();
            sendMsg.m_BuffList = datas;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultAdsBuffReward(long sessionKey, _AdsBUffData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultAdsBufReward();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_BuffData = data;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultAdsBuffMaxLevelComplete(long sessionKey, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultAdsBuffMaxLevelComplete();
            sendMsg.m_Result = (ushort)result;
            
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultAdsBuffList(long sessionKey, List<_AdsBUffData> buffs, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultAdsBuffList();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_AdsBuffList = buffs;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultStageSkillEquip(long sessionKey, _StageSkillData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultStageSkillEquip();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_StageSkillData = data;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportStageSkillEquipData(long sessionKey, List<_StageSkillData> stageSkills)
        {
            var sendMsg = new Packet_C2P.P2C_ReportStageSkillEquipData();
            sendMsg.m_StageSkillDatas = stageSkills;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultProfileChange(long sessionKey, int profileID, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultProfileChange();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_ProfileID = profileID;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportEventDataList(long sessionKey, List<_EventData> eventDatas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportEventDataList();
            sendMsg.m_EventDatas = eventDatas;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportEventShopDataList(long sessionKey, List<_EventShopData> eventShopDatas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportEventShopDataList();
            sendMsg.m_EventShopDatas = eventShopDatas;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultEventShopBuy(long sessionKey, _EventShopData shopData, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultEventShopBuy();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_EventShopData = shopData;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportEventData(long sessionKey, _EventData eventData, CDefine.Modify mofidy)
        {
            var sendMsg = new Packet_C2P.P2C_ReportEventData();
            sendMsg.m_EventData = eventData;
            sendMsg.m_Modify = mofidy;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportEventShopData(long sessionKey, List<_EventShopData> shopDatas, CDefine.Modify modify)
        {
            var sendMsg = new Packet_C2P.P2C_ReportEventShopData();
            sendMsg.m_EventShopDatas = shopDatas;
            sendMsg.m_Modify = modify;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportEventRouletteDataList(long sessionKey, List<_EventRouletteData> datas)
        {
            var sendMsg = new Packet_C2P.P2C_ReportEventRouletteDataList();
            sendMsg.m_EventRoulettes = datas;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ReportEventRoulette(long sessionKey, _EventRouletteData data, CDefine.Modify modify)
        {
            var sendMsg = new Packet_C2P.P2C_ReportEventRoulette();
            sendMsg.m_EventRoulette = data;
            sendMsg.m_Modify = modify;
            
            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultEventRouletteReward(long sessionKey, int picked_index, _EventRouletteData data, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultEventRouletteReward();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_EventRouletteData = data;
            sendMsg.m_PickedIndex = picked_index;

            Write(sessionKey, sendMsg);
        }

        public void P2C_ResultShopBuy(long sessionKey, _ShopData shopData, Packet_Result.Result result)
        {
            var sendMsg = new Packet_C2P.P2C_ResultShopBuy();
            sendMsg.m_Result = (ushort)result;
            sendMsg.m_ShopData = shopData;

            Write(sessionKey, sendMsg);
        }

        #endregion


        //P2M
        #region P2M
        public void P2M_RequestConnect(int clientCnt, CDefine.eServiceType serviceType, string ip, string dn, ushort port) 
        {
            var sendMsg = new Packet_P2M.P2M_RequestConnect();
            sendMsg.m_ServiceType = serviceType;
            sendMsg.m_ClinetCount = clientCnt;
            sendMsg.m_IP = ip;
            sendMsg.m_DN = dn;
            sendMsg.m_Port = port;

            WriteMessageServer(sendMsg);
        }

        public void P2M_ReportSendChatData(_ChatData chatData)
        {
            var sendMsg = new Packet_P2M.P2M_ReportSendChatData();
            sendMsg.m_Chat = chatData;

            WriteMessageServer(sendMsg);
        }

        public void P2M_RequestChatList(CDefine.ChatType type, long requestUID)
        {
            var sendMsg = new Packet_P2M.P2M_RequestChatList();
            sendMsg.m_Type = type;
            sendMsg.m_RequestUID = requestUID;

            WriteMessageServer(sendMsg);
        }

        public void P2M_RequestEnterUser(long userSession, long uid, string deviceID)
        {
            var sendMsg = new Packet_P2M.P2M_RequestEnterUser();
            sendMsg.m_UserSession = userSession;
            sendMsg.m_DeviceID = deviceID;
            sendMsg.m_UID = uid;

            CLogger.Instance.System($"P2M_RequestEnterUser");
            WriteMessageServer(sendMsg);
        }

        public void P2M_ReportServerState(ushort serviceType, int clientCnt, int mainfps, int dbfps, int dbinput)
        {
            var sendMsg = new Packet_P2M.P2M_ReportServerState();
            sendMsg.m_ServiceType = serviceType;
            sendMsg.m_ClientCount = clientCnt;
            sendMsg.m_MainFPS = mainfps;
            sendMsg.m_DBFPS = dbfps;
            sendMsg.m_DBInputCount = dbinput;

            WriteMessageServer(sendMsg);
        }

        public void P2M_RequestNoticeList(long userSession)
        {
            var sendMsg = new Packet_P2M.P2M_RequestNoticeList();
            sendMsg.m_UserSession = userSession;

            WriteMessageServer(sendMsg);
        }

        public void P2M_RequestGetRank(long userSession, CDefine.ERankType type, long uid)
        {
            var sendMsg = new Packet_P2M.P2M_RequestGetRank();
            sendMsg.m_UserSession = userSession;
            sendMsg.m_Type = type;
            sendMsg.m_UID = uid;

            WriteMessageServer(sendMsg);
        }

        public void P2M_ReportUserRankUpdate(long targetid, CDefine.ERankType type)
        {
            var sendMsg = new Packet_P2M.P2M_ReportUserRankUpdate();
            sendMsg.m_UID = targetid;
            sendMsg.m_Type = type;

            WriteMessageServer(sendMsg);
        }

        public void P2M_ReportUserOverViewUpdate(_UserOverViewData data)
        {
            var sendMsg = new Packet_P2M.P2M_ReportUserOverViewUpdate();
            sendMsg.m_OverViewData = data;

            WriteMessageServer(sendMsg);
        }
        public void P2M_ReportCheat(string cheat, List<string> param)
        {
            var sendMsg = new Packet_P2M.P2M_ReportCheat();
            sendMsg.m_Cheat = cheat;
            sendMsg.m_CheatParams = param;

            WriteMessageServer(sendMsg);
        }

        public void P2M_RequestPvpMatchStart(long userSession, string deviceID)
        {
            var sendMsg = new Packet_P2M.P2M_RequestPvpMatchStart();
            sendMsg.m_UserSession = userSession;
            sendMsg.m_DeviceID = deviceID;

            WriteMessageServer(sendMsg);
        }

        public void P2M_ReportUserLogout(long uid, string deviceID)
        {
            var sendMsg = new Packet_P2M.P2M_ReportUserLogout();
            sendMsg.m_DeviceID = deviceID;
            sendMsg.m_UID = uid;

            WriteMessageServer(sendMsg);
        }


        #endregion

        #region P2G
        //public void P2G_RequestServerConnect(CSimpleServerInfo info)
        //{
        //    var sendMsg = new Packet_P2G.P2G_RequestServerConnect();
        //    sendMsg.m_Data = info;

        //    WriteGateServer(sendMsg);
        //}

        //public void P2G_RequestIdentifyUser(long useruid, long userSessionKey)
        //{
        //    var sendMsg = new Packet_P2G.P2G_RequestIdentifyUser();
        //    sendMsg.m_UID = useruid;
        //    sendMsg.m_UserSessionKey = userSessionKey;

        //    WriteGateServer(sendMsg);
        //}

        //public void P2G_ReportLogoutUser(CSimpleUserInfo info)
        //{
        //    var sendMsg = new Packet_P2G.P2G_ReportLogoutUser();
        //    sendMsg.m_Data = info;

        //    WriteGateServer(sendMsg);
        //}

        #endregion
    }
}
