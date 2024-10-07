using Global;
using Newtonsoft.Json.Converters;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OperTool
{
    public class SystemPostView
    {
        public long SystemPostView_PostID = -1;
        public string SystemPostView_PostTitle = string.Empty;
        public string SystemPostView_PostMsg = string.Empty;
        public DateTime SystemPostView_PostBeginTime = DateTime.MinValue;
        public DateTime SystemPostView_PostExpireTime = DateTime.MaxValue;
        public string SystemPostView_PostRewards = string.Empty;

        public SystemPostView(long systemPostView_PostID, string systemPostView_PostTitle, string systemPostView_PostMsg, DateTime systemPostView_PostBeginTime, DateTime systemPostView_PostExpireTime, string systemPostView_PostRewards)
        {
            SystemPostView_PostID = systemPostView_PostID;
            SystemPostView_PostTitle = systemPostView_PostTitle;
            SystemPostView_PostMsg = systemPostView_PostMsg;
            SystemPostView_PostBeginTime = systemPostView_PostBeginTime;
            SystemPostView_PostExpireTime = systemPostView_PostExpireTime;
            SystemPostView_PostRewards = systemPostView_PostRewards;
        }

        public SystemPostView(_PostData data)
        {
            SystemPostView_PostID = data.ID;
            SystemPostView_PostTitle = data.Title;
            SystemPostView_PostMsg = data.Msg;
            SystemPostView_PostBeginTime = SDateManager.Instance.TimeStampToLocalTime(data.beginTime);
            SystemPostView_PostExpireTime = SDateManager.Instance.TimeStampToLocalTime(data.expireTime);
            SystemPostView_PostRewards = SJson.ObjectToJson(data.Rewards);
        }

    }

    public class UserInfoView
    {
        public long UserInfoView_UID = -1;
        public string UserInfoView_DeviceID = string.Empty;
        public string UserInfoView_Name = string.Empty;
        public int UserInfoView_Level = -1;
        public long UserInfoView_Exp = -1;
        public int UserInfoView_LvPoint = -1;
        public DateTime UserInfoView_LoginTime = DateTime.MinValue;
        public DateTime UserInfoView_LogoutTime = DateTime.MaxValue;

        public UserInfoView(long userInfoView_UID, string userInfoView_DeviceID, string userInfoView_Name, int userInfoView_Level, long userInfoView_Exp, int userInfoView_LvPoint, DateTime userInfoView_LoginTime, DateTime userInfoView_LogoutTime)
        {
            UserInfoView_UID = userInfoView_UID;
            UserInfoView_DeviceID = userInfoView_DeviceID;
            UserInfoView_Name = userInfoView_Name;
            UserInfoView_Level = userInfoView_Level;
            UserInfoView_Exp = userInfoView_Exp;
            UserInfoView_LvPoint = userInfoView_LvPoint;
            UserInfoView_LoginTime = userInfoView_LoginTime;
            UserInfoView_LogoutTime = userInfoView_LogoutTime;
        }

        public UserInfoView(_UserData data)
        {
            UserInfoView_UID = data.m_UID;
            UserInfoView_DeviceID = data.m_DeviceID;
            UserInfoView_Name = data.m_Name;
            UserInfoView_Level = data.m_Level;
            UserInfoView_Exp = data.m_Exp;
            UserInfoView_LvPoint = data.m_LevelPoint;
            UserInfoView_LoginTime = SDateManager.Instance.TimeStampToLocalTime(data.m_LoginTime);
            UserInfoView_LogoutTime = SDateManager.Instance.TimeStampToLocalTime(data.m_LogoutTime);
        }
    }

    public class CoinView
    {
        public string UserCoinView_Type = string.Empty;
        public long UserCoinView_Value = -1;

        public CoinView(string userCoinView_Type, long userCoinView_Value)
        {
            UserCoinView_Type = userCoinView_Type;
            UserCoinView_Value = userCoinView_Value;
        }

        public CoinView(_AssetData data)
        {
            UserCoinView_Type = data.TableID;
            UserCoinView_Value = data.Count;
        }
    }

    public class ItemView
    {
        public long ItemView_ItemID = -1;
        public string ItemView_ItemTID = string.Empty;
        public string ItemView_ItemName = string.Empty;
        public long ItemView_Count = 0;
        public int ItemView_Lv = 0;
        public bool ItemView_LimitTime = false;
        public DateTime ItemView_ExpireTime = DateTime.MinValue;
        public long ItemView_HorseID = -1;
        public bool ItemView_IsUsed = false;
        public string ItemView_RandomOption = string.Empty;

        public ItemView(_ItemData data)
        {
            ItemView_ItemID = data.ItemID;
            ItemView_ItemTID = data.TableID;
            //todo : table
            ItemView_ItemName = data.TableID;
            ItemView_Count = data.Count;
            ItemView_Lv = data.Level;
            ItemView_LimitTime = data.IsLimitTime;
            ItemView_ExpireTime = data.ExpireTime;
            ItemView_HorseID = data.HorseID;
            ItemView_IsUsed = data.m_InUsed;
            ItemView_RandomOption = SJson.ObjectToJson(data.RandomOption);
        }

        public ItemView(string itemTID, long count)
        {
            ItemView_ItemTID = itemTID;
            //todo : table
            ItemView_ItemName = "";
            ItemView_Count = count;
        }
    }

    public class UserGachaView
    {
        public int UserGachaView_ID = -1;
        public string UserGachaView_Type = string.Empty;
        public int UserGachaView_Lv = -1;
        public long UserGachaView_Exp = -1;
        public int UserGachaView_Rewarded = -1;

        public UserGachaView(_GachaData data)
        {
            UserGachaView_ID = data.m_GroupID;
            //todo : gachaTable Type
            //UserGachaView_Type = 
            UserGachaView_Exp = data.m_Exp;
            UserGachaView_Rewarded = data.m_Rewarded;
        }
    }

    public class UserGrowthGoldView
    {
        public int UserGrowthGoldView_ID = -1;
        public string UserGrowthGoldView_Type = string.Empty;
        public int UserGrowthGoldView_LV = -1;

        public UserGrowthGoldView(int userGrowthGoldView_ID, string userGrowthGoldView_Type, int userGrowthGoldView_LV)
        {
            UserGrowthGoldView_ID = userGrowthGoldView_ID;
            UserGrowthGoldView_Type = userGrowthGoldView_Type;
            UserGrowthGoldView_LV = userGrowthGoldView_LV;
        }

        public UserGrowthGoldView(_LevelData data)
        {
            UserGrowthGoldView_ID = data.m_TableID;
            //todo : table
            UserGrowthGoldView_LV = data.m_UseCount;
        }
    }

    public class UserGrowthLevelView
    {
        public int UserGrowthLevelView_ID = -1;
        public string UserGrowthLevelView_Type = string.Empty;
        public int UserGrowthLevelView_Lv = -1;

        public UserGrowthLevelView(int userGrowthLevelView_ID, string userGrowthLevelView_Type, int userGrowthLevelView_Lv)
        {
            UserGrowthLevelView_ID = userGrowthLevelView_ID;
            UserGrowthLevelView_Type = userGrowthLevelView_Type;
            UserGrowthLevelView_Lv = userGrowthLevelView_Lv;
        }   

        public UserGrowthLevelView(_LevelData data)
        {
            UserGrowthLevelView_ID = data.m_TableID;
            //todo : table
            UserGrowthLevelView_Lv = data.m_UseCount;
        }
    }

    public class UserIAPReceiptView
    {
        public string UserIAPReceiptView_TransactionID = string.Empty;
        public string UserIAPReceiptView_StoreTyep = string.Empty;
        public string UserIAPReceiptView_ProductID = string.Empty;
        public string UserIAPReceiptView_ProductName = string.Empty;
        public long UserIAPReceiptView_Price = 0;
        public long UserIAPReceiptView_PostID = -1;
        public DateTime UserIAPReceiptView_UpdateTime = DateTime.MinValue;

        public UserIAPReceiptView(_ReceiptData data)
        {
            UserIAPReceiptView_TransactionID = data.m_TransactionID;
            UserIAPReceiptView_StoreTyep = data.m_StoreType.ToString();
            UserIAPReceiptView_ProductID = data.m_ProductID;
            UserIAPReceiptView_Price = data.m_Price;
            UserIAPReceiptView_PostID = data.m_PostID;
            UserIAPReceiptView_UpdateTime = data.m_UpateTime;
        }
    }

    public class UserPostView
    {
        public long     UserPostView_ID = -1;
        public string UserPostView_Type = string.Empty;
        public string UserPostView_Title = string.Empty;
        public string UserPostView_Msg = string.Empty;
        public bool UserPostView_IsRead = false;
        public bool UserPostView_IsRewarded = false;
        public DateTime UserPostView_BeginTime = DateTime.MinValue;
        public DateTime UserPostView_ExpireTime = DateTime.MaxValue;
        public string UserPostView_Reward = string.Empty;

        public UserPostView(long userPostView_ID, string userPostView_Type, string userPostview_Title, string userPostView_Msg, bool userPostView_IsRead, bool userPostView_IsRewarded, DateTime userPostView_BeginTime, DateTime userPostView_ExpireTime, string userPostView_Reward)
        {
            UserPostView_ID = userPostView_ID;
            UserPostView_Type = userPostView_Type;
            UserPostView_Title = userPostview_Title;
            UserPostView_Msg = userPostView_Msg;
            UserPostView_IsRead = userPostView_IsRead;
            UserPostView_IsRewarded = userPostView_IsRewarded;
            UserPostView_BeginTime = userPostView_BeginTime;
            UserPostView_ExpireTime = userPostView_ExpireTime;
            UserPostView_Reward = userPostView_Reward;
        }

        public UserPostView(_PostData data)
        {
            UserPostView_ID = data.ID;
            UserPostView_Type = data.Type.ToString();
            UserPostView_Title = data.Title;
            UserPostView_Msg = data.Msg;
            UserPostView_IsRead = data.IsRead;
            UserPostView_IsRewarded = data.IsRewarded;
            UserPostView_BeginTime = SDateManager.Instance.TimeStampToLocalTime(data.beginTime);
            UserPostView_ExpireTime = SDateManager.Instance.TimeStampToLocalTime(data.expireTime);
            UserPostView_Reward = SJson.ObjectToJson(data.Rewards);
        }
    }

    public class UserStageView
    {
        public string UserStageView_Type = string.Empty;
        public string UserStageView_CurID = string.Empty;
        public int UserStageView_CurChapter = -1;
        public int UserStageView_CurStage = -1;
        public string UserStageView_MaxID = string.Empty;
        public int UserStageView_MaxChapter = -1;
        public int UserStageView_MaxStage = -1;
        public long UserStageView_TotalClear = -1;

        public UserStageView(string userStageView_Type, string userStageView_CurID, int userStageView_CurChapter, int userStageView_CurStage, int userStageView_MaxChapter, int userStageView_MaxStage, long userStageView_TotalClear, string userStageView_MaxID)
        {
            UserStageView_Type = userStageView_Type;
            UserStageView_CurID = userStageView_CurID;
            UserStageView_CurChapter = userStageView_CurChapter;
            UserStageView_CurStage = userStageView_CurStage;
            UserStageView_MaxID = userStageView_MaxID;
            UserStageView_MaxChapter = userStageView_MaxChapter;
            UserStageView_MaxStage = userStageView_MaxStage;
            UserStageView_TotalClear = userStageView_TotalClear;
        }

        public UserStageView(_StageData data)
        {
            UserStageView_Type = data.type.ToString();
            UserStageView_CurID = data.curTID.ToString();
            //todo : table
            //UserStageView_CurChapter = userStageView_CurChapter;
            //UserStageView_CurStage = userStageView_CurStage;
            //UserStageView_MaxChapter = userStageView_MaxChapter;
            //UserStageView_MaxStage = userStageView_MaxStage;
            UserStageView_MaxID = data.maxTID.ToString();
            UserStageView_TotalClear = data.totalCnt;
        }
    }

    public class NoticeView
    {
        public string NoticeView_UID    = string.Empty;
        public string NoticeView_Msg    = string.Empty;
        public string NoticeView_Start  = string.Empty;
        public string NoticeView_End    = string.Empty;
        public string NoticeView_Loop   = string.Empty;
        public string NoticeView_Term   = string.Empty;

        public NoticeView(_NoticeData data)
        {
            NoticeView_UID = data.m_ID.ToString();
            NoticeView_Msg = data.m_Msg;
            NoticeView_Start = data.m_StartDate.ToString();
            NoticeView_End = data.m_EndDate.ToString();
            NoticeView_Loop = data.m_Loop.ToString();
            NoticeView_Term = data.m_Term.ToString();
        }
    }

    public class BlockView
    {
        public bool     BlockView_IsBlocked         = false;
        public string   BlockView_DeviceID          = string.Empty;
        public int      BlockView_Count             = 0;
        public DateTime BlockView_ExpTime           = DateTime.MinValue;

        public BlockView(string deviceID, int count, long expTime)
        {
            BlockView_DeviceID = deviceID;
            BlockView_Count = count;
            BlockView_ExpTime = SDateManager.Instance.TimeStampToLocalTime(expTime);
            BlockView_IsBlocked = !SDateManager.Instance.IsExpired(expTime);
        }

        public BlockView(_BlockUser data)
        {
            BlockView_DeviceID = data.DeviceID;
            BlockView_Count = data.Count;
            BlockView_ExpTime = SDateManager.Instance.TimeStampToLocalTime(data.ExpTime);
            BlockView_IsBlocked = !SDateManager.Instance.IsExpired(data.ExpTime);
        }
    }

    public class WhiteView
    {
        public string WhiteView_DeviceID = string.Empty;
        public DateTime WhiteView_CreateTime = DateTime.MinValue;

        public WhiteView(string deviceID, DateTime createTime)
        {
            WhiteView_DeviceID = deviceID;
            WhiteView_CreateTime = createTime.ToLocalTime();
        }
    }

    public class UserAccountView
    {
        public string UserAccountView_DeviceID = string.Empty;
        public long UserAccountView_UID = -1;
        public string UserAccountView_Name = string.Empty;

        public UserAccountView(string userAccountView_DeviceID, long userAccountView_UID, string userAccountView_Name)
        {
            UserAccountView_DeviceID = userAccountView_DeviceID;
            UserAccountView_UID = userAccountView_UID;
            UserAccountView_Name = userAccountView_Name;
        }
    }

    public class CouponView
    {
        public string CouponView_ID = string.Empty;
        public int CouponView_Count = -1;
        public int CouponView_UseLevel = 0;
        public DateTime CouponView_Begin = DateTime.MinValue;
        public DateTime CouponView_Expire = DateTime.MinValue;
        public string CouponView_Rewards = string.Empty;

        public CouponView(string couponView_ID, int couponView_Count, int couponView_UseLevel, DateTime couponView_Begin, DateTime couponView_End, string couponView_Rewards)
        {
            CouponView_ID = couponView_ID;
            CouponView_Count = couponView_Count;
            CouponView_UseLevel = couponView_UseLevel;
            CouponView_Begin = couponView_Begin;
            CouponView_Expire = couponView_End;
            CouponView_Rewards = couponView_Rewards;
        }

        public CouponView(_CouponData data)
        {
            CouponView_ID = data.m_CouponID;
            CouponView_Count = data.m_Count;
            CouponView_UseLevel = data.m_UseLevel;
            CouponView_Begin = SDateManager.Instance.TimeStampToLocalTime(data.m_BeginTime);
            CouponView_Expire = SDateManager.Instance.TimeStampToLocalTime(data.m_ExpireTime);
            CouponView_Rewards = SJson.ObjectToJson(data.m_Rewards);
        }
    }

    public class GrowthLevelView
    {
        public int GrowthLevelView_ID = 0;
        public string GrowthLevelView_Name = string.Empty;
        public int GrowthLevelView_Cnt = 0;

        public GrowthLevelView(_LevelData data)
        {
            GrowthLevelView_ID = data.m_TableID;
            //todo : table
            GrowthLevelView_Cnt = data.m_UseCount;
        }
    }

    public class ServerInfoView
    {
        public string ServerInfoView_Type = string.Empty;
        public bool ServerInfoView_Open = false;
        public string ServerInfoView_SessionKey = string.Empty;
        public string ServerInfoView_ServiceType = string.Empty;
        public long ServerInfoView_ClinetCount = -1;
        public string ServerInfoView_IP = string.Empty;
        public string ServerInfoView_DN = string.Empty;
        public ushort ServerInfoView_Port = 0;
        public int ServerInfoView_MainFPS = 0;
        public int ServerInfoView_DBFPS = 0;
        public int ServerInfoView_DBInputCount = 0;
        
        public ServerInfoView(string serverInfoView_Type, string serverInfoView_SessionKey, string serverInfoView_ServiceType, long serverInfoView_ClinetCount, string serverInfoView_IP, string serverInfoView_DN, ushort serverInfoView_Port, int serverInfoView_MainFPS, int serverInfoView_DBFPS, int serverInfoView_DBInputCount)
        {
            ServerInfoView_Type = serverInfoView_Type;
            ServerInfoView_SessionKey = serverInfoView_SessionKey;
            ServerInfoView_ServiceType = serverInfoView_ServiceType;
            ServerInfoView_ClinetCount = serverInfoView_ClinetCount;
            ServerInfoView_IP = serverInfoView_IP;
            ServerInfoView_DN = serverInfoView_DN;
            ServerInfoView_Port = serverInfoView_Port;
            ServerInfoView_MainFPS = serverInfoView_MainFPS;
            ServerInfoView_DBFPS = serverInfoView_DBFPS;
            ServerInfoView_DBInputCount = serverInfoView_DBInputCount;

        }

        public ServerInfoView(CServerInfo data)
        {
            ServerInfoView_Type = data.m_Type;
            ServerInfoView_Open = data.m_Open;
            ServerInfoView_SessionKey = data.m_ServerSessionKey.ToString();
            CDefine.eServiceType type = (CDefine.eServiceType)data.m_ServiceType;
            ServerInfoView_ServiceType = type.ToString();
            ServerInfoView_ClinetCount = data.m_ClientCount;
            ServerInfoView_IP = data.m_IP;
            ServerInfoView_DN = data.m_DN;
            ServerInfoView_Port = data.m_Port;
            ServerInfoView_MainFPS = data.m_MainFPS;
            ServerInfoView_DBFPS = data.m_DBFPS;
            ServerInfoView_DBInputCount = data.m_DBInputCount;
        }
    }

    public class GameLogView
    {
        public string GameLogView_LogID = string.Empty;
        public string GameLogView_DeviceID = string.Empty;
        public long GameLogView_UID = -1;
        public eLogType GameLogView_Type = 0;
        public string GameLogView_LogStr = string.Empty;
        public string GameLogView_UpdateCoins = string.Empty;
        public string GameLogView_UpdateItems = string.Empty;
        public int GameLogView_Count = 0;
        public DateTime GameLogView_Time = DateTime.MinValue;

        public GameLogView(SDB.LogBson data)
        {
            GameLogView_LogID = data.logID.ToString();
            GameLogView_DeviceID = data.DeviceID;
            GameLogView_UID = data.UID;
            GameLogView_Type = (eLogType)data.Type;
            GameLogView_LogStr = data.LogStr;
            GameLogView_UpdateCoins = data.Update_Coins;
            GameLogView_UpdateItems = data.Update_Items;
            GameLogView_Count = data.Count;
            GameLogView_Time = data.Time;
        }
    }
}
