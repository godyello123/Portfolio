using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using SCommon;

namespace Global
{
    public class JsonPreset
    {
		public long uid = -1;
		public int tid = -1;
    }
    public class GooglePlayClaim
    {
        public string kid;
        public string issuer;
        public string publickey;
        public string privatekey;
        public string audience;
        public string scope = "https://www.googleapis.com/auth/androidpublisher";
    };
    public class CServerDefine
	{
		public static int MaxCacheTime = 30;
		public static int WaitDeleteUser = 7;
		public static string AccountPrefix = "ba";
		public static string UserPrefix = "bu";


        public static long EmptySlotGUID = -1;


        public static Int64 GeneraterGUID()
        {
            var b_guid = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(b_guid, 0);
        }

        //DBIndex 함수
        public static int GetDBGUID(string id)
		{
			int dbGUID = 0;
			byte[] temp = System.Text.Encoding.UTF8.GetBytes(id);
			foreach(byte value in temp) dbGUID += value;
			return dbGUID;
		}

		//로그관련
		public enum ReasonType
		{
			Billing,
			Coupon,
			BuyUnit,
			RefreshUnitShop,
			BuyAsset,
			UnlockGacha,
			RunGacha,
			PayRunGacha,
			BonusGacha,
			ReadMail,
			Match,
			UnitLevelUp,
			Trade,
			DailyMission,
			Achievement,
			OverflowCard,
			KakaoDelivery,
			Max
		}

		//특수문자체크
		public static bool CheckSpecialCharacters(string text)
		{
			string str = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
			Regex rex = new Regex(str);
			return rex.IsMatch(text);
		}
	}

	public class CRandomID
	{
		public static string Create(string prefix = "")
		{
			string id = prefix;
			for(int j = 0; j < 4; j++) id += string.Format("{0:D4}", SRandom.Instance.Next() % 10000);
			return id;
		}
	}

	public class CDBKey
	{
		public static string GetGMListDBKey() { return "GMList"; }
		public static string GetEventListDBKey() { return "EventList"; }
		public static string GetTVReplayListDBKey(int tier) { return string.Format("TVReplayList_{0}", tier); }
		public static string GetBillingDBKey(string transactionID, string productID) { return string.Format("Billing_{0}_{1}", transactionID, productID); }
		public static string GetCouponDBKey(string couponID) { return string.Format("Coupon_{0}", couponID); }
		public static string GetCouponUseDBKey(string userID, string couponGroupID) { return string.Format("CouponUse_{0}_{1}", userID, couponGroupID); }
		public static string GetNicknameDBKey(string nickname) { return string.Format("Nickname_{0}", nickname); }
		public static string GetMailboxDBKey(string userID) { return string.Format("Mailbox_{0}", userID); }
		public static string GetFriendDBKey(string userID) { return string.Format("Friend_{0}", userID); }
		public static string GetTradeDBKey(string userID) { return string.Format("Trade_{0}", userID); }
		public static string GetInviteDBKey(string inviteID) { return string.Format("Invite_{0}", inviteID); }
		public static string GetInviteListDBKey(string userID) { return string.Format("InviteList_{0}", userID); }
	}

	public class CSelfCertVerify : IDisposable
	{
		private static volatile CSelfCertVerify s_Instance;
		private static Object s_Lock = new Object();
		public static CSelfCertVerify Instance
		{
			get
			{
				if(s_Instance == null)
				{
					lock(s_Lock)
					{
						if(s_Instance == null) s_Instance = new CSelfCertVerify();
					}
				}
				return s_Instance;
			}
		}

		private const string KEY = "GuestCertKey. Don't change this!";

		private bool m_Disposed;

		private RijndaelManaged m_AES;
		private ICryptoTransform m_Encryptor;
		private ICryptoTransform m_Decryptor;

		public CSelfCertVerify()
		{
			m_AES = new RijndaelManaged();
			m_AES.Key = UTF8Encoding.ASCII.GetBytes(KEY);
			m_AES.Mode = CipherMode.ECB;
			m_AES.Padding = PaddingMode.PKCS7;
			m_Encryptor = m_AES.CreateEncryptor();
			m_Decryptor = m_AES.CreateDecryptor();
		}
		~CSelfCertVerify()
		{
			Dispose(false);
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if(m_Disposed) return;
			if(disposing)
			{
				m_AES.Clear();
				m_AES = null;
				m_Encryptor.Dispose();
				m_Encryptor = null;
				m_Decryptor.Dispose();
				m_Decryptor = null;
			}
			m_Disposed = true;
		}

		public byte[] Encrypt(byte[] src, int offset, int count)
		{
			lock(m_Encryptor)
			{
				return m_Encryptor.TransformFinalBlock(src, offset, count);
			}
		}
		public byte[] Decrypt(byte[] src, int offset, int count)
		{
			lock(m_Decryptor)
			{
				return m_Decryptor.TransformFinalBlock(src, offset, count);
			}
		}
		public string Encrypt(string src)
		{
			lock(m_Encryptor)
			{
				byte[] temp = Encoding.Unicode.GetBytes(src);
				temp = m_Encryptor.TransformFinalBlock(temp, 0, temp.Length);
				return Convert.ToBase64String(temp);
			}
		}
		public string Decrypt(string src)
		{
			lock(m_Decryptor)
			{
				byte[] temp = Convert.FromBase64String(src);
				temp = m_Decryptor.TransformFinalBlock(temp, 0, temp.Length);
				return Encoding.Unicode.GetString(temp, 0, temp.Length);
			}
		}
	}

	public enum eStageValid
    {
		Valid1,
		Valid2,
		Valid3,
        Max
    }

    public enum eAuctionItemDetail
    {
        Monster,
        Master,
		Costume,
		Max,
	}

    //public enum eServiceType
    //{
    //    Live,
    //    Dev,
    //    Maintenance,
    //    Max
    //}

    public enum eStatusCategory
    {
        Base,
        StatusGold,
        StatusLv,
        ItemEquipped_Base,
        ItemEquipped_Knight,
        ItemOwned_Knight,
        ItemOwned_Weapon,
        ItemOwned_Defence,
        ItemOwned_Chivalry,
        ItemOwned_Custume,
        Relic,
        Buff,
        Total,
        Cheat,
        
        Max,

        beg = Base,
        end = Total - 1,
    }

    public enum eSchedule
    {
        HotTime,
        Post,
        Max
    }

    public enum eUserState
    {
        TryLogin,
        Login,
        Max,
    }

    public enum eLogType
    {
        //user
        login = 0,
        logout,
        growth_gold,
        growth_level,
        rank_reward,
        iap_try,
        iap_fail,
        iap_success,
        cheat,

        //quest
        quest_main = 1000,
        quest_daily,
        quest_repeat,
        quest_checkin,
        quest_pass,

        //stage
        stage_clear = 2000,
        stage_sweep,

        //item
        item_gacha = 3000,
        item_gacha_reward,
        item_combine,
        item_enchant,
        item_breakthrougth,
        item_sell,
        item_consume,

        //content
        use_coupon = 4000,
        reward_post,
        relic_enchant,
        
        //event
        event_quest,
        event_coin,
        event_roulette,

        Max,
    }


#if SERVER_ONLY || MAIN_SERVER_ONLY
    //public class _AuctionPriceTagComp : IComparer<_AuctionPriceTagComp>
    //{
    //public long m_ItemTID = -1;
    //public int m_OptSlotCnt = 0;
    //public eItemGrade m_SpeGrade1 = eItemGrade.Max;
    //public eItemGrade m_SpeGrade2 = eItemGrade.Max;

    //public int Compare(_AuctionPriceTagComp x, _AuctionPriceTagComp y)
    //{
    //    int retval = x.m_ItemTID.CompareTo(y.m_ItemTID);
    //    if (retval != 0) return retval;

    //    retval = x.m_OptSlotCnt.CompareTo(y.m_OptSlotCnt);
    //    if (retval != 0) return retval;

    //    retval = x.m_SpeGrade1.CompareTo(y.m_SpeGrade1);
    //    if (retval != 0) return retval;

    //    retval = x.m_SpeGrade2.CompareTo(y.m_SpeGrade2);
    //    return retval;
    //}

    //static public _AuctionPriceTagComp CreateComp(_AuctionPriceTag data)
    //{
    //    _AuctionPriceTagComp retval = new _AuctionPriceTagComp();
    //    retval.m_ItemTID = data.m_ItemTID;
    //    retval.m_OptSlotCnt = data.m_OptSlotCnt;

    //    int property = 0;
    //    retval.m_SpeGrade1 = (data.m_SpecialOptions.Count <= property) ? eItemGrade.Max : data.m_SpecialOptions[property++];
    //    retval.m_SpeGrade2 = (data.m_SpecialOptions.Count <= property) ? eItemGrade.Max : data.m_SpecialOptions[property++];
    //    return retval;
    //}

    //static public _AuctionPriceTagComp CreateComp(_ItemData data)
    //{
    //    _AuctionPriceTagComp retval = new _AuctionPriceTagComp();
    //    //retval.m_ItemTID = data.TID;
    //    //retval.m_OptSlotCnt = data.option.Count;

    //    //List<eItemGrade> optList = new List<eItemGrade>();
    //    //foreach (var opt in data.special_option)
    //    //{
    //    //    var record = CSkinOptionTable.Instance.Find(opt.Key);
    //    //    if (record != null)
    //    //        optList.Add(record.m_Grade);
    //    //}

    //    //int property = 0;
    //    //retval.m_SpeGrade1 = (optList.Count <= property) ? eItemGrade.Max : optList[property++];
    //    //retval.m_SpeGrade2 = (optList.Count <= property) ? eItemGrade.Max : optList[property++];
    //    return retval;
    //}
    //}

#endif

}
