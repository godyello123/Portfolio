using System;
using System.Collections.Generic;
using SNetwork;
using SCommon;
using Global;
using Packet_P2M;
using System.Security.Policy;
using System.Web;
using System.Runtime.CompilerServices;
using System.Reflection;
using Packet_Result;


namespace Packet_O2M
{

    public enum Protocol
    {
        O2M_RequestSearchUser = 40000,
        M2O_ResultSearchUser,
        O2M_RequestSystemPostLoad,
        M2O_ResultSystemPostLoad,
        O2M_RequestSystemPostSend,
        M2O_ResultSystemPostSend,
        O2M_RequestNoticeLoad,
        M2O_ResultNoticeLoad,
        O2M_RequestNoticeUpdate,
        M2O_ResultNoticeUpdate,
        O2M_RequestNoticeErase,
        M2O_ResultNoticeErase,
        O2M_RequestUserPostLoad,
        M2O_ResultUserPostLoad,
        O2M_RequestUserPostSend,
        M2O_ResultUserPostSend,
        O2M_RequestBlockUserLoad,
        M2O_ResultBlockUserLoad,
        O2M_ReportBlockUserUpsert,
        O2M_ReportBlockUserDelete,
        O2M_ReportWhiteUserInsert,
        O2M_ReportWhiteUserDelete,
        O2M_ReportUserKick,
        O2M_RequestCouponLoad,
        M2O_ResultCouponLoad,
        O2M_RequestCouponCreate,
        M2O_ResultCouponCreate,
        O2M_RequestUserGrowthLevelLoad,
        M2O_ResultUserGrowthLevelLoad,
        O2M_RequestUserGrowthGoldLoad,
        M2O_ResultUserGrowthGoldLoad,
        O2M_RequestUserGachaLoad,
        M2O_ResultUserGachaLoad,
        O2M_RequestUserQuestLoad,
        M2O_ResultUserQuestLoad,
        O2M_RequestUserRelicLoad,
        M2O_ResultUserRelicLoad,
        O2M_RequestUserSkillLoad,
        M2O_ResultUserSkillLoad,
        O2M_RequestUserShopLoad,
        M2O_ResultUserShopLoad,
        O2M_RequestUserPassLoad,
        M2O_ResultUserPassLoad,

        O2M_RequestServerStateLoad,
        M2O_ResultServerStateLoad,

        O2M_ReportChangeServerServiceType,
        

        Max
    }

    public class O2M_RequestSearchUser : INetPacket
    {
        public string m_DeviceID = string.Empty;
        public string m_Name = string.Empty;
        public long m_UID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestSearchUser; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_DeviceID);
            reader.Read(ref m_Name);
            reader.Read(ref m_UID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_DeviceID);
            writer.Write(m_Name);
            writer.Write(m_UID);
        }
    }


    public class M2O_ResultSearchUser : INetPacket
    {
        public ushort m_Result = 0;
        public _UserData m_UserData = new _UserData();
        public List<_GachaData> m_GachaData = new List<_GachaData>();
        public List<_AssetData> m_Coins = new List<_AssetData>();
        public List<_StageData> m_Stages = new List<_StageData>();
        public List<_ReceiptData> m_Receipts = new List<_ReceiptData>();
        public bool m_IsBlock = false;
        public bool m_IsWhite = false;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultSearchUser; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_UserData);
            reader.Read(ref m_GachaData);
            reader.Read(ref m_Coins);
            reader.Read(ref m_Stages);
            reader.Read(ref m_Receipts);
            reader.Read(ref m_IsBlock);
            reader.Read(ref m_IsWhite);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_UserData);
            writer.Write(m_GachaData);
            writer.Write(m_Coins);
            writer.Write(m_Stages);
            writer.Write(m_Receipts);
            writer.Write(m_IsBlock);
            writer.Write(m_IsWhite);
        }
    }

    public class O2M_RequestSystemPostLoad : INetPacket
    {
        public long m_BeginTime = -1;
        public long m_EndTime = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestSystemPostLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_BeginTime);
            reader.Read(ref m_EndTime);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_BeginTime);
            writer.Write(m_EndTime);
        }
    }

    public class M2O_ResultSystemPostLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_PostData> m_SystemPosts = new List<_PostData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultSystemPostLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_SystemPosts);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_SystemPosts);
        }
    }

    public class O2M_RequestSystemPostSend : INetPacket
    {
        public CDefine.PostType m_Type = CDefine.PostType.Max;
        public long m_PostID = -1;
        public string m_Title = string.Empty;
        public string m_Msg = string.Empty;
        public long m_BeginTime = -1;
        public long m_ExpireTime = -1;
        public List<_AssetData> m_Rewards = new List<_AssetData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestSystemPostSend; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.ReadEnum(ref m_Type);
            reader.Read(ref m_PostID);
            reader.Read(ref m_Title);
            reader.Read(ref m_Msg);
            reader.Read(ref m_BeginTime);
            reader.Read(ref m_ExpireTime);
            reader.Read(ref m_Rewards);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.WriteEnum(m_Type);
            writer.Write(m_PostID);
            writer.Write(m_Title);
            writer.Write(m_Msg);
            writer.Write(m_BeginTime);
            writer.Write(m_ExpireTime);
            writer.Write(m_Rewards);
        }
    }

    public class M2O_ResultSystemPostSend : INetPacket
    {
        public ushort m_Result = 0;
        public _PostData m_PostData = new _PostData();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultSystemPostSend; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_PostData);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_PostData);
        }
    }

    public class O2M_RequestNoticeLoad : INetPacket
    {
        public DateTime m_StartDate = DateTime.MinValue;
        public DateTime m_EndDate = DateTime.MaxValue;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestNoticeLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_StartDate);
            reader.Read(ref m_EndDate);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_StartDate);
            writer.Write(m_EndDate);
        }
    }

    public class M2O_ResultNoticeLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_NoticeData> m_Notices = new List<_NoticeData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultNoticeLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Notices);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Notices);
        }
    }
    public class O2M_RequestNoticeUpdate : INetPacket
    {
        public _NoticeData m_NoticeData = new _NoticeData();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestNoticeUpdate; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_NoticeData);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_NoticeData);
        }
    }
    public class M2O_ResultNoticeUpdate : INetPacket
    {
        public ushort m_Result = 0;
        public _NoticeData m_NoticeData = new _NoticeData();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultNoticeUpdate; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_NoticeData);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_NoticeData);
        }
    }


    public class O2M_RequestNoticeErase : INetPacket
    {
        public long m_RemoveID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestNoticeErase; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_RemoveID);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_RemoveID);
        }
    }
    public class M2O_ResultNoticeErase : INetPacket
    {
        public ushort m_Result = 0;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultNoticeErase; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
        }
    }

    public class O2M_RequestUserPostLoad : INetPacket
    {
        public long m_UserUID = -1;
        public long m_Start = -1;
        public long m_End = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserPostLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_UserUID);
            reader.Read(ref m_Start);
            reader.Read(ref m_End);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_UserUID);
            writer.Write(m_Start);
            writer.Write(m_End);
        }
    }

    public class M2O_ResultUserPostLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_PostData> m_Posts = new List<_PostData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserPostLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Posts);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Posts);
        }
    }

    public class O2M_RequestUserPostSend : INetPacket
    {
        public long m_UserID = -1;
        public CDefine.PostType m_Type = CDefine.PostType.Max;
        public long m_PostID = -1;
        public string m_Title = string.Empty;
        public string m_Msg = string.Empty;
        public long m_BeginTime = -1;
        public long m_ExpireTime = -1;
        public List<_AssetData> m_Rewards = new List<_AssetData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserPostSend; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_UserID);
            reader.ReadEnum(ref m_Type);
            reader.Read(ref m_PostID);
            reader.Read(ref m_Title);
            reader.Read(ref m_Msg);
            reader.Read(ref m_BeginTime);
            reader.Read(ref m_ExpireTime);
            reader.Read(ref m_Rewards);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_UserID);
            writer.WriteEnum(m_Type);
            writer.Write(m_PostID);
            writer.Write(m_Title);
            writer.Write(m_Msg);
            writer.Write(m_BeginTime);
            writer.Write(m_ExpireTime);
            writer.Write(m_Rewards);
        }
    }

    public class M2O_ResultUserPostSend : INetPacket
    {
        public ushort m_Result = 0;
        public _PostData m_PostData = new _PostData();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserPostSend; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_PostData);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_PostData);
        }
    }
    
    public class O2M_RequestBlockUserLoad : INetPacket
    {
        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestBlockUserLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
        }

        void INetSerialize.Write(SNetWriter writer)
        {
        }
    }

    public class M2O_ResultBlockUserLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_BlockUser> m_BlockUsers = new List<_BlockUser>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultBlockUserLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_BlockUsers);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_BlockUsers);
        }
    }

    public class O2M_ReprortBlockUserUpsert : INetPacket
    {
        public List<_BlockUser> m_BlockUsers = new List<_BlockUser>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_ReportBlockUserUpsert; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_BlockUsers);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_BlockUsers);
        }
    }

    public class O2M_ReportBlockUserDelete : INetPacket
    {
        public string m_DeviceID = string.Empty;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_ReportBlockUserDelete; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_DeviceID);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_DeviceID);
        }
    }

    public class O2M_ReportWhiteUserInsert : INetPacket
    {
        public string m_DeviceID = string.Empty;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_ReportWhiteUserInsert; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_DeviceID);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_DeviceID);
        }
    }

    public class O2M_ReportWhiteUserDelete : INetPacket
    {
        public string m_DeviceID = string.Empty;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_ReportWhiteUserDelete; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_DeviceID);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_DeviceID);
        }
    }

    public class O2M_ReportUserKick : INetPacket
    {
        public string m_DeviceID = string.Empty;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_ReportUserKick; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_DeviceID);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_DeviceID);
        }
    }

    public class O2M_RequestCouponLoad : INetPacket
    {
        public long m_BeginTime = -1;
        public long m_EndTime = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestCouponLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_BeginTime);
            reader.Read(ref m_EndTime);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_BeginTime);
            writer.Write(m_EndTime);
        }
    }


    public class M2O_ResultCouponLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_CouponData> m_Coupons = new List<_CouponData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultCouponLoad; }

        void INetSerialize.Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Coupons);
        }

        void INetSerialize.Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Coupons);
        }
    }


    public class O2M_RequestCouponCreate : INetPacket
    {
        public string m_CouponID = string.Empty;
        public int m_Count = 0;
        public int m_UseLevel = 0;
        public long m_BeginTime = 0;
        public long m_ExpireTime = 0;
        public List<_AssetData> m_Rewards = new List<_AssetData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestCouponCreate; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_CouponID);
            reader.Read(ref m_Count);
            reader.Read(ref m_UseLevel);
            reader.Read(ref m_BeginTime);
            reader.Read(ref m_ExpireTime);
            reader.Read(ref m_Rewards);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_CouponID);
            writer.Write(m_Count);
            writer.Write(m_UseLevel);
            writer.Write(m_BeginTime);
            writer.Write(m_ExpireTime);
            writer.Write(m_Rewards);
        }
    }

    public class M2O_ResultCouponCreate : INetPacket
    {
        public ushort m_Result = 0;
        public _CouponData m_Coupon = new _CouponData();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultCouponCreate; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Coupon);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Coupon);
        }
    }

    public class O2M_RequestUserGrowthLevelLoad : INetPacket
    {
        public long m_TargetUID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserGrowthLevelLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_TargetUID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_TargetUID);
        }
    }

    public class M2O_ResultUserGrowthLevelLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_LevelData> m_Datas = new List<_LevelData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserGrowthLevelLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Datas);
        }
    }

    public class O2M_RequestUserGrowthGoldLoad : INetPacket
    {
        public long m_TargetUID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserGrowthGoldLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_TargetUID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_TargetUID);
        }
    }

    public class M2O_ResultUserGrowthGoldLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_LevelData> m_Datas = new List<_LevelData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserGrowthGoldLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Datas);
        }
    }


    public class O2M_RequestUserGachaLoad : INetPacket
    {
        public long m_TargetUID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserGachaLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_TargetUID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_TargetUID);
        }
    }

    public class M2O_ResultUserGachaLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_GachaData> m_Datas = new List<_GachaData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserGachaLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Datas);
        }
    }

    public class O2M_RequestUserQuestLoad : INetPacket
    {
        public long m_TargetUID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserQuestLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_TargetUID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_TargetUID);
        }
    }

    public class M2O_ResultUserQuestLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_QuestBoard> m_Datas = new List<_QuestBoard>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserQuestLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Datas);
        }
    }

    public class O2M_RequestUserRelicLoad : INetPacket
    {
        public long m_TargetUID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserRelicLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_TargetUID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_TargetUID);
        }
    }

    public class M2O_ResultUserRelicLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_RelicData> m_Datas = new List<_RelicData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserRelicLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Datas);
        }
    }

    public class O2M_RequestUserSkillLoad : INetPacket
    {
        public long m_TargetUID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserSkillLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_TargetUID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_TargetUID);
        }
    }

    public class M2O_ResultUserSkillLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_SkillData> m_Datas = new List<_SkillData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserSkillLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Datas);
        }
    }

    public class O2M_RequestUserShopLoad : INetPacket
    {
        public long m_TargetUID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserShopLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_TargetUID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_TargetUID);
        }
    }

    public class M2O_ResultUserShopLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_ShopData> m_Datas = new List<_ShopData>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserShopLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Datas);
        }
    }

    public class O2M_RequestUserPassLoad : INetPacket
    {
        public long m_TargetUID = -1;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestUserPassLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_TargetUID);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_TargetUID);
        }
    }

    public class M2O_ResultUserPassLoad : INetPacket
    {
        public ushort m_Result = 0;
        public List<_QuestBoard> m_Datas = new List<_QuestBoard>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultUserPassLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Datas);
        }
    }


    public class O2M_RequestServerStateLoad : INetPacket
    {
        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_RequestServerStateLoad; }

        public void Read(SNetReader reader)
        {
        }

        public void Write(SNetWriter writer)
        {
        }
    }

    public class M2O_ResultServerStateLoad : INetPacket
    {
        public ushort m_Result = 0;
        public bool m_Open = false;
        public List<CServerInfo> m_Datas = new List<CServerInfo>();

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.M2O_ResultServerStateLoad; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Result);
            reader.Read(ref m_Open);
            reader.Read(ref m_Datas);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Result);
            writer.Write(m_Open);
            writer.Write(m_Datas);
        }
    }
    
    public class O2M_ReportChangeServerServiceType : INetPacket
    {
        public bool m_Open = false;

        public CryptType GetCrypt() { return CryptType.None; }

        public ushort GetProtocol() { return (ushort)Protocol.O2M_ReportChangeServerServiceType; }

        public void Read(SNetReader reader)
        {
            reader.Read(ref m_Open);
        }

        public void Write(SNetWriter writer)
        {
            writer.Write(m_Open);
        }
    }
        

}
