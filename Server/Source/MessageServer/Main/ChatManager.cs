using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global;
using SCommon;

namespace MessageServer
{
    public class ChatRoomKey
    {
        public long m_Channel = -1;
        public HashSet<long> m_ChatUsers = new HashSet<long>();
    }

    class CChatManager : SSingleton<CChatManager>
    {
        private Dictionary<KeyValuePair<CDefine.ChatType, long>, _ChatRoomData> m_ChatRooms = new Dictionary<KeyValuePair<CDefine.ChatType, long>, _ChatRoomData>();
        private Dictionary<long , Dictionary<long, ChatRoomKey>> m_ChatRoomKeys = new Dictionary<long, Dictionary<long, ChatRoomKey>>();

        STimer m_Timer = new STimer(600000);

        private long DefaultChatChannel = 1;

        public void Init()
        {
            //Setting default chatRoom 
            var key = new KeyValuePair<CDefine.ChatType, long>(CDefine.ChatType.All, 1);
            _ChatRoomData chatRoom = new _ChatRoomData();
            chatRoom.m_Channel = DefaultChatChannel;
            chatRoom.m_Type = CDefine.ChatType.All;

            m_ChatRooms.Add(key, chatRoom);
        }

        public void Update()
        {
            if (!m_Timer.Check())
                return;

            long curTime = SDateManager.Instance.CurrTime();
            List<long> expired = new List<long>();
            foreach (var iter in m_ChatRooms)
            {
                var room = iter.Value;
                if (room.m_Type == CDefine.ChatType.Persnal)
                {
                    Int64 time_left = curTime - room.m_UpdateTime;
                    if (time_left >= DefineTable.Instance.Value<long>("Delete_PersnalChatRoomTime"))
                        expired.Add(iter.Value.m_Channel);
                }
            }

            foreach (var exp in expired)
                DeleteChatRoom(exp);
        }

        public void RepSendChatData(_ChatData chatData)
        {
            if (chatData.type == CDefine.ChatType.Max)
                return;

            if (chatData.type == CDefine.ChatType.All)
            {
                //TODO : defulat set
                long chat_Channel = 1;
                var key = new KeyValuePair<CDefine.ChatType, long>(chatData.type, chat_Channel);
                if (m_ChatRooms.TryGetValue(key, out _ChatRoomData chatRoom))
                    EnqueueChatData(chatRoom, chatData);
            }
            else
            {
                var rooKey = FindChatRoomKey(chatData.sender, chatData.receiver);
                if (rooKey != null)
                {
                    var key = new KeyValuePair<CDefine.ChatType, long>(chatData.type, rooKey.m_Channel);
                    if (m_ChatRooms.TryGetValue(key, out _ChatRoomData chatRoom))
                        EnqueueChatData(chatRoom, chatData);
                }
                else
                {
                    CreateChatRoomAndSendChatData(chatData);
                }
            }
        }

        public void ReqChatList(long sessionKey, CDefine.ChatType type, long requesterUID)
        {
            List<_ChatRoomData> retChatRooms = new List<_ChatRoomData>();

            var key = new KeyValuePair<CDefine.ChatType, long>(CDefine.ChatType.All, DefaultChatChannel);
            if (m_ChatRooms.TryGetValue(key, out var allChatRoom))
                retChatRooms.Add(allChatRoom);

            if (m_ChatRoomKeys.TryGetValue(requesterUID, out var roomKeys))
            {
                foreach (var iter in roomKeys)
                {
                    var itKey = iter.Value;
                    if (m_ChatRooms.TryGetValue(key, out var finditRoom))
                        retChatRooms.Add(finditRoom);
                }
            }

            CNetManager.Instance.M2P_ResultChatList(sessionKey, retChatRooms, requesterUID, Packet_Result.Result.Success);
        }

        private _ChatRoomData CreateChatRoom(_ChatUserData sender, _ChatUserData receiver)
        {
            ChatRoomKey roomKey = CreateChatRoomKey(sender.m_UserID, receiver.m_UserID);

            _ChatRoomData chatRoom = new _ChatRoomData();
            chatRoom.m_Channel = roomKey.m_Channel;
            chatRoom.m_Type = CDefine.ChatType.Persnal;
            chatRoom.m_ChatUsers.Add(sender);
            chatRoom.m_ChatUsers.Add(receiver);

            var keyValuepair = new KeyValuePair<CDefine.ChatType, long>(chatRoom.m_Type, chatRoom.m_Channel);
            m_ChatRooms.Add(keyValuepair, chatRoom);

            return chatRoom;
        }

        private ChatRoomKey CreateChatRoomKey(long sender, long receiver)
        {
            ChatRoomKey roomKey = new ChatRoomKey();
            roomKey.m_Channel = CServerDefine.GeneraterGUID();

            if (m_ChatRoomKeys.TryGetValue(sender, out Dictionary<Int64, ChatRoomKey> sender_key_list))
            {
                if (false == sender_key_list.ContainsKey(receiver))
                    sender_key_list.Add(receiver, roomKey);
            }
            else
            {
                Dictionary<long, ChatRoomKey> new_key_list = new Dictionary<long, ChatRoomKey>();
                new_key_list.Add(receiver, roomKey);
                m_ChatRoomKeys.Add(sender, new_key_list);
            }

            if (m_ChatRoomKeys.TryGetValue(receiver, out Dictionary<Int64, ChatRoomKey> recv_key_list))
            {
                if (false == recv_key_list.ContainsKey(sender))
                    recv_key_list.Add(sender, roomKey);
            }
            else
            {
                Dictionary<Int64, ChatRoomKey> new_key_list = new Dictionary<Int64, ChatRoomKey>();
                new_key_list.Add(sender, roomKey);
                m_ChatRoomKeys.Add(receiver, new_key_list);
            }

            return roomKey;
        }

        private ChatRoomKey FindChatRoomKey(long sender, long receiver)
        {
            Dictionary<long, ChatRoomKey> keys;
            if(m_ChatRoomKeys.TryGetValue(sender, out keys))
            {
                if (keys.TryGetValue(receiver, out ChatRoomKey key))
                    return key;
            }

            if (m_ChatRoomKeys.TryGetValue(receiver, out keys))
            {
                if (keys.TryGetValue(sender, out ChatRoomKey key))
                    return key;
            }

            return null;
        }

        private void DeleteChatRoomKey(long sender, long receiver)
        {
            Dictionary<long, ChatRoomKey> keys;
            if (m_ChatRoomKeys.TryGetValue(sender, out keys))
            {
                if (keys.TryGetValue(receiver, out ChatRoomKey key))
                {
                    keys.Remove(receiver);
                    return;
                }
            }

            if (m_ChatRoomKeys.TryGetValue(receiver, out keys))
            {
                if (keys.TryGetValue(sender, out ChatRoomKey key))
                {
                    keys.Remove(sender);
                    return;
                }
            }
        }

        private void EnqueueChatData(_ChatRoomData chatRoom ,_ChatData chatData)
        {
            chatRoom.m_ChatDatas.Enqueue(chatData);

            int chatMaxCount = DefineTable.Instance.Value<int>("Max_Chat_Line");
            if (chatRoom.m_ChatDatas.Count >= chatMaxCount)
                chatRoom.m_ChatDatas.Dequeue();

            chatData.channel = chatRoom.m_Channel;
            chatData.chatTime = SDateManager.Instance.CurrTime();
            chatRoom.m_UpdateTime = chatData.chatTime;

            if(chatRoom.m_Type == CDefine.ChatType.All)
                CNetManager.Instance.M2P_ReportSendChatData(-1, chatData);
            else
            {
                foreach(var iter in chatRoom.m_ChatUsers)
                    CNetManager.Instance.M2P_ReportSendChatData(iter.m_ServerID, chatData);
            }
        }

        private void CreateChatRoomAndSendChatData(_ChatData chat)
        {
            var sendUser = CUserManager.Instance.FindUser(chat.sender);
            if (sendUser == null)
                return;

            var receiveUser = CUserManager.Instance.FindUser(chat.receiver);
            if (receiveUser == null)
                return;

            _ChatUserData sender = new _ChatUserData(sendUser.UID, sendUser.ServerSession);
            _ChatUserData receiver = new _ChatUserData(receiveUser.UID, receiveUser.ServerSession);

            var chatRoom = CreateChatRoom(sender, receiver);
            EnqueueChatData(chatRoom, chat);
        }

        private void DeleteChatRoom(long channel)
        {
            var key = new KeyValuePair<CDefine.ChatType, long>(CDefine.ChatType.Persnal, channel);
            m_ChatRooms.Remove(key);
        }
    }
}
