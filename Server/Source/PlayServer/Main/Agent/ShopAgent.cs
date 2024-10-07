using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using SCommon;

namespace PlayServer
{
    public class CShopAgent
    {
        private CUser m_Owner;
        private Dictionary<int, _ShopData> m_Shopgoods = new Dictionary<int, _ShopData>();

        private Dictionary<long, Dictionary<int, _EventShopData>> m_EventShopgoods = new Dictionary<long, Dictionary<int, _EventShopData>>();

        public CUser Owner { get => m_Owner; }

        public CShopAgent(CUser owner) { m_Owner = owner; }

        public void Init(List<_ShopData> shopList, List<_EventShopData> eventShopDatas)
        {
            var copyDefault = ShopTable.Instance.CopyDefault();
            foreach (var iter in copyDefault)
                m_Shopgoods[iter.m_ShopID] = iter;

            foreach (var iter in shopList)
                m_Shopgoods[iter.m_ShopID] = iter;

            foreach(var iter in eventShopDatas)
            {
                var eventData = m_Owner.EventAgent.FindEvent(iter.m_EventUID);
                if (eventData == null)
                    continue;

                if (SDateManager.Instance.IsExpired(eventData.m_EndDate))
                    continue;

                if (m_EventShopgoods.TryGetValue(iter.m_EventUID, out var findDatas))
                {
                    findDatas[iter.m_EventShopID] = iter;
                }
                else
                {
                    var subDatas = new Dictionary<int, _EventShopData>();
                    subDatas.Add(iter.m_EventShopID, iter);
                    m_EventShopgoods.Add(iter.m_EventUID, subDatas);
                }

            }
        }

        public _ShopData FindShop(int shopID)
        {
            if (m_Shopgoods.TryGetValue(shopID, out var retVal))
                return retVal;

            return null;
        }

        public _EventShopData FindEventShop(long eventUID, int shopID)
        {
            if (m_EventShopgoods.TryGetValue(eventUID, out var findgoods))
            {
                if (findgoods.TryGetValue(shopID, out var retVal))
                    return retVal;
            }

            return null;
        }

        public List<_EventShopData> GetEventShopList()
        {
            var retVal = new List<_EventShopData>();
            foreach (var iter in m_EventShopgoods)
            {
                var itVals = iter.Value;
                foreach(var itSub in itVals)
                {
                    retVal.Add(itSub.Value);
                }
            }

            return retVal;
        }

        public void InsertEvent(long event_uid, int event_id, bool bSend)
        {
            var defaultDatas = EventShopTable.Instance.CopyDefaultEventShopDatas(event_uid, event_id);
            if (defaultDatas == null)
                return;

            List<_EventShopData> sendList = new List<_EventShopData>();
            foreach (var itSub in defaultDatas)
            {
                //var key = new KeyValuePair<long, int>(itSub.m_EventUID, itSub.m_EventShopID);
                if (m_EventShopgoods.TryGetValue(itSub.m_EventUID, out var findDatas))
                {
                    findDatas[itSub.m_EventShopID] = itSub;
                    sendList.Add(itSub);
                }
                else
                {
                    var subDatas = new Dictionary<int, _EventShopData>();
                    subDatas.Add(itSub.m_EventShopID, itSub);
                    m_EventShopgoods.Add(itSub.m_EventUID, subDatas);
                    sendList.Add(itSub);
                }
            }

            if(bSend)
                CNetManager.Instance.P2C_ReportEventShopData(m_Owner.SessionKey, sendList, CDefine.Modify.Add);
        }

        public bool AfterRestIAPTry(_IAPReceipt receipt, ref string error_str)
        {
            if (!receipt.IsValid())
            {
                CLogger.Instance.Error($"AfterRestIAPTry 1");
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                return false;
            }

            var record = ShopTable.Instance.FindIAP(receipt.m_ProductID);
            if(record == null)
            {
                CLogger.Instance.Error($"AfterRestIAPTry 2");
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                error_str = "FindIAP is null";
                return false;
            }

            var hasData = FindShop(record.ID);
            if (hasData == null)
            {
                CLogger.Instance.Error($"AfterRestIAPTry 3");
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                error_str = "FindShop is NUll";
                return false;
            }

            if(!record.IsBuyable(hasData.m_LimitCount +1))
            {
                CLogger.Instance.Error($"AfterRestIAPTry 4");
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                error_str = "data buyable Error";
                return false;
            }

            var rewards = RewardTable.Instance.Find(record.RewardID);
            if (rewards == null)
            {
                CLogger.Instance.Error($"AfterRestIAPTry 5");
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                error_str = "reward is NUll";
                return false;
            }

            //process
            CDBTranMail dbtranmail = new CDBTranMail();
            dbtranmail.m_PostData = PostTable.Instance.MakePost(record.PostID);
            foreach(var iter in rewards)
            {
                CLogger.Instance.Error($"AfterRestIAPTry 6");
                var reward = iter.Value;
                dbtranmail.m_PostData.Rewards.Add(reward.GetAssetData());
            }

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.iap_success, m_Owner.UserData, SCommon.SJson.ObjectToJson(receipt), null, 1);
            CGameLog.Instance.Insert(log);

            //query
            CDBManager.Instance.QueryCharacterShopIAPUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, hasData, receipt, dbtranmail.m_PostData);

            return true;
        }

        public void AfterQueryIAP(long sessionKey, _ShopData shopData, _IAPReceipt receiptData, _PostData postData, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportUserPost(sessionKey, postData);
            CNetManager.Instance.P2C_ResultIAPTry(sessionKey, shopData, receiptData.m_ProductID, result);
        }

        public void IAPTest(long sessionKey, string productID)
        {
            var record = ShopTable.Instance.FindIAP(productID);
            if (record == null)
            {
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                return;
            }

            var hasData = FindShop(record.ID);
            if (hasData == null)
            {
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                return;
            }

            if (!record.IsBuyable(hasData.m_LimitCount + 1))
            {
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                return;
            }

            var rewards = RewardTable.Instance.Find(record.RewardID);
            if (rewards == null)
            {
                CNetManager.Instance.P2C_ResultIAPTry(m_Owner.SessionKey, new _ShopData(), "", Packet_Result.Result.SystemError);
                return;
            }

            //process
            CDBTranMail dbtranmail = new CDBTranMail();
            dbtranmail.m_PostData = PostTable.Instance.MakePost(record.PostID);
            foreach (var iter in rewards)
            {
                var reward = iter.Value;
                dbtranmail.m_PostData.Rewards.Add(reward.GetAssetData());
            }

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.iap_success, m_Owner.UserData, SCommon.SJson.ObjectToJson(""), null, 1);
            CGameLog.Instance.Insert(log);

            var iap = new _IAPReceipt();
            iap.m_ProductID = productID;
            //query
            CDBManager.Instance.QueryCharacterShopIAPUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, hasData, iap, dbtranmail.m_PostData);
        }

        public void AfterQueryShopBuy(long sessionKey, _ShopData shopData, CRewardInfo forClient, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            CNetManager.Instance.P2C_ResultShopBuy(sessionKey, shopData, result);
        }

        public Packet_Result.Result ReqShopBuy(long sessionKey, int shopID, int cnt)
        {
            var shopData = FindShop(shopID);
            if (shopData == null)
                return Packet_Result.Result.InValidData;

            var shopRecord = ShopTable.Instance.Find(shopID);
            if (shopRecord == null)
                return Packet_Result.Result.InValidRecord;

            if (shopData.m_LimitCount >= shopRecord.LimitCount)
                return Packet_Result.Result.PacketError;

            if (shopRecord.PurchaseType == CDefine.ePurchaseType.Payment)
                return Packet_Result.Result.PacketError;

            CDBMerge dbtran = new CDBMerge();
            CRewardInfo forClient = new CRewardInfo();
            if (shopRecord.PurchaseType == CDefine.ePurchaseType.Asset)
            {
                if (!m_Owner.HasEnoughAsset(CDefine.AssetType.Coin, shopRecord.Purchase_CoinType.ToString(), shopRecord.Purchase_CoinValue))
                    return Packet_Result.Result.LackAssetError;

                _AssetData costAsset = new _AssetData(CDefine.AssetType.Coin, shopRecord.Purchase_CoinType.ToString(), -shopRecord.Purchase_CoinValue);
                m_Owner.UpdateAssetData(costAsset, ref dbtran, true);
            }

            var rewards = RewardTable.Instance.Find(shopRecord.RewardID);
            if (rewards == null)
                return Packet_Result.Result.PacketError;

            foreach (var iter in rewards)
            {
                var reward = iter.Value;
                forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
            }

            forClient.UpdateRewardData(m_Owner, ref dbtran, true);
            dbtran.Merge();

            shopData.m_LimitCount++;

            m_Owner.StatusAgent.Refresh(new List<eStatusCategory>{ eStatusCategory.ItemOwned_Custume});

            m_Owner.ReportAssetData(dbtran);

            CDBManager.Instance.QueryCharacterShopBuy(m_Owner.DBGUID, sessionKey, m_Owner.UID, shopData, dbtran, forClient);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqEventShopBuy(long sessionKey, long eventUID, int shopid)
        {
            //valid
            var findEventData = m_Owner.EventAgent.FindEvent(eventUID);
            if (findEventData == null)
                return Packet_Result.Result.InValidData;

            if (SDateManager.Instance.IsExpired(findEventData.m_EndDate))
                return Packet_Result.Result.PacketError;

            var eventRecord = EventTable.Instance.Find(findEventData.m_EventID);
            if (eventRecord == null)
                return Packet_Result.Result.InValidRecord;

            var findShopData = FindEventShop(eventUID, shopid);
            if (findShopData == null)
                return Packet_Result.Result.InValidData;

            var eventShopRecord = EventShopTable.Instance.Find(findShopData.m_EventShopID);
            if (eventShopRecord == null)
                return Packet_Result.Result.InValidRecord;

            if (findShopData.m_BuyCount >= eventShopRecord.LimitCount)
                return Packet_Result.Result.PacketError;

            if (!m_Owner.HasEnoughAsset(CDefine.AssetType.Coin, eventShopRecord.CostCoinType.ToString(), eventShopRecord.CostCoinValue))
                return Packet_Result.Result.LackAssetError;

            var rewards = RewardTable.Instance.Find(eventShopRecord.RewardID);
            if (rewards == null)
                return Packet_Result.Result.PacketError;

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            findShopData.m_BuyCount++;

            foreach (var iter in rewards)
            {
                var reward = iter.Value;
                forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
            }

            m_Owner.UpdateAssetData(new _AssetData(CDefine.AssetType.Coin, eventShopRecord.CostCoinType.ToString(), -eventShopRecord.CostCoinValue), ref dbtran);

            forClient.UpdateRewardData(m_Owner, ref dbtran, true);

            dbtran.Merge();

            CDBManager.Instance.QueryCharacterEventShopUpdate(m_Owner.DBGUID, sessionKey, m_Owner.UID, findShopData, dbtran);

            m_Owner.ReportAssetData(dbtran);

            CNetManager.Instance.P2C_ResultEventShopBuy(m_Owner.SessionKey, findShopData, Packet_Result.Result.Success);

            var log = LogHelper.MakeLogBson(eLogType.event_coin, m_Owner.UserData, SCommon.SJson.ObjectToJson(findShopData), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }
    }
}
