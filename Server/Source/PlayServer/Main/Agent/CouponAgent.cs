using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Global;
using SCommon;

namespace PlayServer
{
    public class CCouponAgent
    {
        private HashSet<string> m_UsedCoupons = new HashSet<string>();

        private CUser m_Owner;
        public CUser Owner { get => m_Owner; }

        public CCouponAgent(CUser owner) { m_Owner = owner; }

        public void Init(HashSet<string> usedcoupons)
        {
            foreach (var iter in usedcoupons)
                m_UsedCoupons.Add(iter);
        }

        public bool IsUsedCoupon(string coupon)
        {
            return m_UsedCoupons.Contains(coupon);
        }

        public Packet_Result.Result ReqUseCoupon(long sessionKey, string couponID)
        {
            if (string.IsNullOrEmpty(couponID))
                return Packet_Result.Result.IgnoreError;

            if (IsUsedCoupon(couponID))
                return Packet_Result.Result.InValidCoupon;

            CDBManager.Instance.QueryCharacterVaildCoupon(m_Owner.DBGUID, sessionKey, m_Owner.UID, couponID);
            return Packet_Result.Result.Success;
        }

        public void AfterQueryVaildCoupon(string couponID, List<_AssetData> rewards)
        {
            if(IsUsedCoupon(couponID))
            {
                CNetManager.Instance.P2C_ResultUseCoupon(m_Owner.SessionKey, new List<_AssetData>(), Packet_Result.Result.InValidCoupon);
                return;
            }

            m_UsedCoupons.Add(couponID);

            //process
            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            forClient.Insert(rewards);

            bool bSend = true;
            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);

            forClient.Merge();
            dbtran.Merge();

            //db use coupon
            string used_coupon = SJson.ObjectToJson(SCopy<HashSet<string>>.DeepCopy(m_UsedCoupons));
            CDBManager.Instance.QueryCharacterUseCoupon(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, couponID, used_coupon, forClient, dbtran);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.use_coupon, m_Owner.UserData, couponID, dbtran, 1);
            CGameLog.Instance.Insert(log);
        }

        public void AfterQueryUseCoupon(long sessionKey, CRewardInfo forClient, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ResultUseCoupon(sessionKey, forClient.GetList(), result);
        }
    }
}
