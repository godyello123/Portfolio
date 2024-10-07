using System;
using System.Collections.Generic;
using SCommon;
using Global;

namespace PlayServer
{
    class CChatManager : SSingleton<CChatManager>
    {
        ////private Dictionary<eChatType, HashSet<Int64>> m_ChatUserList = new Dictionary<eChatType, HashSet<Int64>>();
        //private Dictionary<KeyValuePair<eChatType, int>, HashSet<long>> m_ChatUserList = new Dictionary<KeyValuePair<eChatType, int>, HashSet<long>>();

        //private STimer m_Timer = new STimer(1000);

        //public void Init()
        //{
        //    for( eChatType en = eChatType.All; en < eChatType.Max; ++en)
        //    {
        //        var globalChannel = new KeyValuePair<eChatType, int>(en, -1);
        //        m_ChatUserList.Add(globalChannel, new HashSet<Int64>());
        //    }
        //}

        //private bool IsInChatRoom(eChatType type, int channelID, long accountID)
        //{
        //    var key = new KeyValuePair<eChatType, int>(type, channelID);

        //    HashSet<long> retval = null;
        //    if (!m_ChatUserList.TryGetValue(key, out retval))
        //        return false;

        //    return retval.Contains(accountID);
        //}

        //private HashSet<long> Find(eChatType type, int channelID)
        //{
        //    var key = new KeyValuePair<eChatType, int>(type, channelID);
        //    if (m_ChatUserList.TryGetValue(key, out HashSet<long> retval))
        //        return retval;
        //    return null;
        //}

        //private HashSet<long> FindOrAdd(eChatType type, int channelID)
        //{
        //    var key = new KeyValuePair<eChatType, int>(type, channelID);

        //    HashSet<long> retval = null;
        //    if (!m_ChatUserList.TryGetValue(key, out retval))
        //    {
        //        retval = new HashSet<long>();
        //        m_ChatUserList.Add(key, retval);
        //    }
        //    return retval;
        //}

        //private void WriteAll(CDefine.eChatType type, int channelID, _ChatData msg)
        //{
        //    var participants = Find(type, channelID);
        //    if (participants == null) return;

        //    foreach (var cid in participants)
        //    {
        //        //var user = CUserManager.Instance.Find(cid);
        //        //if (user != null)
        //        //    CNetManager.Instance.P2C_ReportMessage(user.SessionKey, type, channelID, msg, CDefine.MinValue());
        //    }
        //}
        //public bool InsertChatUser(Int64 account_id, eChatType chat_type, int channelID)
        //{
        //    foreach(var it in m_ChatUserList)
        //    {
        //        var participants = it.Value;
        //        participants.Remove(account_id);
        //    }

        //    var chatRoom = FindOrAdd(chat_type, channelID);
        //    return chatRoom.Add(account_id);

        //    //if (false == m_ChatUserList.ContainsKey(chat_type))
        //    //    return false;

        //    //for( eChatType en = eChatType.Korean; en < eChatType.Max; ++en )
        //    //{
        //    //    if( true == m_ChatUserList[en].Contains(account_id) )
        //    //        m_ChatUserList[en].Remove(account_id);
        //    //}

        //    //return m_ChatUserList[chat_type].Add(account_id);
        //}

        ////public bool EraseChatUser(Int64 account_id, eChatType chat_type)
        ////{
        ////    if (false == m_ChatUserList.ContainsKey(chat_type))
        ////        return false;

        ////    return m_ChatUserList[chat_type].Remove(account_id);
        ////}

        //public void EraseChatUser(Int64 account_id)
        //{
        //    foreach ( var data in m_ChatUserList)
        //    {
        //        data.Value.Remove(account_id);
        //    }

        //    return;
        //}

        //public CDefine.eChatType GetChatType(Int64 account_id)
        //{
        //    foreach(var data in m_ChatUserList )
        //    {
        //        if (true == data.Value.Contains(account_id))
        //            return data.Key.Key;
        //    }

        //    return CDefine.eChatType.Max;
        //}

        public void RepChatSend(Int64 account_id, CDefine.ChatType chatType, string msg, int channelID)
        {
            //CUser pUser = CUserManager.Instance.FindPlayer(account_id);
            //if (null == pUser || false == pUser.IsLogin())
            //    return;

            //_ChatData chat_data = new _ChatData();
            //chat_data.m_Name = pUser.PlayerData.m_Name;
            //chat_data.m_Msg = msg;
            //chat_data.m_AccountID = pUser.AccountID;
            //chat_data.m_ProfileID = pUser.ItemAgent.GetEquippedItemTID(eEquipPreset.Base, eItemDetailType.Profile);
            //chat_data.m_ServerId = pUser.ServerID;

            //if (chatType != eChatType.Guild)
            //{
            //    _StageData stage_data = pUser.StageAgent.GetStageData(eStageType.Main_Stage);
            //    if (null == stage_data)
            //        return;

            //    if (stage_data.final_stage_tid < CDefineTable.Instance.Find(eTableDefineType.Chat_Open_Stage).m_Int64Value)
            //        return;

            //    if (DateTime.UtcNow < pUser.m_ChatTime)
            //        return;

            //    if (!IsInChatRoom(chatType, channelID, account_id))
            //        return;

            //    //chatType = GetChatType(account_id);
            //    //if (eChatType.Max <= chatType)
            //    //    return;
            //}

            //if (msg.Length > CDefineTable.Instance.Find(eTableDefineType.Max_Chat_Msg).m_Int64Value)
            //{
            //    msg = msg.Substring(0, (int)CDefineTable.Instance.Find(eTableDefineType.Max_Chat_Msg).m_Int64Value - 1);
            //}

            //CNetManager.Instance.P2M_ReportChatSend(pUser.DeviceID, chatType, chat_data, channelID);
            //pUser.m_ChatTime = DateTime.UtcNow.AddSeconds(CDefineTable.Instance.Find(eTableDefineType.Chat_InputDelay).m_Int64Value);

            ////log
            //var log = LogHelper.PrepareLog(eLog.character_chat , pUser , null, 1);
            //log.SetTargetObj(chatType.ToString() , chat_data.m_AccountID , "");
            //log.SetSubStr(SJson.ObjectToJson(chat_data.m_Msg));
            //CNetManager.Instance.P2L_ReportLogging(log);

        }

        //public Packet_Result.Result ReqFriendChatSend(CUser pUser, string msg, Int64 target_id)
        //{
        //    if (target_id == CDefine.INVALID_ACCOUNT_ID)
        //        return Packet_Result.Result.PacketError;

        //    if (DateTime.UtcNow < pUser.m_ChatTime)
        //        return Packet_Result.Result.PacketError;

        //    var friend = pUser.FriendAgent.FindFriend(target_id);
        //    if (friend == null)
        //        return Packet_Result.Result.Friend_FriendDeleted;

        //    _ChatData chat_data = new _ChatData();
        //    chat_data.m_Name = pUser.PlayerData.m_Name;
        //    chat_data.m_Msg = msg;
        //    chat_data.m_AccountID = pUser.AccountID;
        //    chat_data.m_ProfileID = pUser.ItemAgent.GetEquippedItemTID(eEquipPreset.Base , eItemDetailType.Profile);
        //    chat_data.m_TargetID = target_id;
        //    chat_data.m_ServerId = pUser.ServerID;

        //    if (msg.Length > CDefineTable.Instance.Find(eTableDefineType.Max_Chat_Msg).m_Int64Value)
        //    {
        //        msg = msg.Substring(0 , (int)CDefineTable.Instance.Find(eTableDefineType.Max_Chat_Msg).m_Int64Value - 1);
        //    }

        //    CNetManager.Instance.P2M_ReportFriendChatSend(pUser.DeviceID , eChatType.Friend , chat_data);
        //    pUser.m_ChatTime = DateTime.UtcNow.AddSeconds(CDefineTable.Instance.Find(eTableDefineType.Chat_InputDelay).m_Int64Value);

        //    //log
        //    var log = LogHelper.PrepareLog(eLog.character_chat , pUser , null , 1);
        //    log.SetTargetObj(eChatType.Friend.ToString(), chat_data.m_AccountID , chat_data.m_TargetID.ToString());
        //    log.SetSubStr(SJson.ObjectToJson(chat_data.m_Msg));
        //    CNetManager.Instance.P2L_ReportLogging(log);

        //    return Packet_Result.Result.Success;
        //}


        //public Packet_Result.Result ReqChatOpen(Int64 account_id, eChatType chat_type, bool open, int channelID)
        //{
        //    CUser pUser = CUserManager.Instance.FindPlayer(account_id);
        //    if( null == pUser || false == pUser.IsLogin())
        //        return Packet_Result.Result.PacketError;

        //    if (!(eChatType.Korean <= chat_type && chat_type < eChatType.Max))
        //        return Packet_Result.Result.PacketError;

        //    bool isGlobalChannel = (channelID == -1);
        //    if (!isGlobalChannel)
        //    {
        //        if (channelID != pUser.ServerID)
        //            return Packet_Result.Result.PacketError;
        //    }

        //    if ( true == open)
        //    {
        //        if (eChatType.Guild == chat_type)
        //        {
        //            if (!pUser.PlayerData.IsExistsGuild())
        //                return Packet_Result.Result.PacketError;
        //        }
        //        else
        //        {
        //            InsertChatUser(account_id, chat_type, channelID);
        //        }

        //        CNetManager.Instance.P2M_RequestGetChatList(pUser.DeviceID, chat_type, channelID);
        //    }
        //    else
        //    {
        //        _ChatRoomData chat_data = new _ChatRoomData(chat_type, channelID);
        //        CNetManager.Instance.P2C_ResultChatOpen(pUser.SessionKey, chat_data, open, Packet_Result.Result.Success);
        //    }

        //    return Packet_Result.Result.Success;
        //}

        //public void AfterRepChatSend(Int64 user_sessionkey, eChatType chat_type, int channelID, _ChatData chat_data, DateTime block_date, List<long> targetUsers)
        //{
        //    if( -1 == user_sessionkey)
        //    {
        //        if (chat_type == eChatType.Guild)
        //        {
        //            foreach(var targetAccountID in targetUsers)
        //            {
        //                CUser user = CUserManager.Instance.FindPlayer(targetAccountID);
        //                if (null != user && true == user.IsLogin())
        //                {
        //                    CNetManager.Instance.P2C_ReportChatData(user.SessionKey, chat_type, channelID, chat_data, CDefine.MinValue());
        //                }
        //            }
        //        }
        //        else if (chat_type == eChatType.Raid)
        //        {
        //            foreach (var targetAccountID in targetUsers)
        //            {
        //                CUser user = CUserManager.Instance.FindPlayer(targetAccountID);
        //                if (null != user && true == user.IsLogin())
        //                {
        //                    CNetManager.Instance.P2C_ReportChatData(user.SessionKey, chat_type, channelID, chat_data, CDefine.MinValue());
        //                }
        //            }
        //        }
        //        else
        //        {
        //            WriteAll(chat_type, channelID, chat_data);
        //        }
        //    }
        //    else
        //    {
        //        CUser user = CUserManager.Instance.FindUser(user_sessionkey);
        //        if (null != user && true == user.IsLogin())
        //        {
        //            CNetManager.Instance.P2C_ReportChatData(user.SessionKey, chat_type, channelID, chat_data, block_date);
        //        }
        //    }

        //    return;
        //}

        //public void AfterReqFriendChatSend(eChatType chat_type , _ChatData chat_data , DateTime block_date , Int64 targetUsers)
        //{
        //    CUser user = CUserManager.Instance.FindPlayer(targetUsers);
        //    if(user != null && true == user.IsLogin())
        //        CNetManager.Instance.P2C_ReportFriendChatData(user.SessionKey , chat_type , chat_data , CDefine.MinValue(), Packet_Result.Result.Success);
        //}

        //public void ReqFriendChatList(Int64 req_usre_id, List<_ChatRoomData> chat_list)
        //{
        //    CUser user = CUserManager.Instance.FindPlayer(req_usre_id);
        //    if (user != null && true == user.IsLogin())
        //        CNetManager.Instance.P2C_ReportFriendChatList(user.SessionKey , chat_list);
        //}
    }
}
