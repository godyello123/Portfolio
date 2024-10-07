using System.Collections.Generic;
using Global;
using SCommon;

namespace PlayServer
{
    public class CDBTranMail
    {
        public _PostData m_PostData = new _PostData();

    }


    public class CDBMerge
    {
        public List<_AssetData> m_UpdateCoinList = new List<_AssetData>();
        public List<_ItemData> m_UpdateItemList = new List<_ItemData>();

        public List<_ItemData> m_DeleteItemlist = new List<_ItemData>();


        public List<_AssetData> GetUpdateCoinList()
        {
            return m_UpdateCoinList;
        }

        public List<_ItemData> GetUpdateItemList()
        {
            return m_UpdateItemList;
        }

        public List<_ItemData> GetDeleteItemList()
        {
            return m_DeleteItemlist;
        }

        public void InsertCoin(_AssetData coindata)
        {
            m_UpdateCoinList.Add(coindata);
        }

        public void InsertUpdateItem(_ItemData itemData)
        {
            _ItemData item = new _ItemData();
            item.ItemID = itemData.ItemID;
            item.TableID = itemData.TableID;
            item.Count = itemData.Count;

            m_UpdateItemList.Add(itemData);
        }

        public void InsertUpdateItem(long itemID, string tableID, long count)
        {
            _ItemData item = new _ItemData();
            item.ItemID = itemID;
            item.TableID = tableID;
            item.Count = count;

            m_UpdateItemList.Add(item);
        }

        public void InsertDeleteItem(long itemID, string tableID, long count)
        {
            _ItemData item = new _ItemData();
            item.ItemID = itemID;
            item.TableID = tableID;
            item.Count = count;

            m_DeleteItemlist.Add(item);
        }

        //public void InsertItem(CItemData itemData)
        //{
        //    CItemData item = new CItemData();
        //    item.ItemID = itemData.ItemID;
        //    item.TableID = itemData.TableID;
        //    item.Count = itemData.Count;

        //    m_ItemList.Add(itemData);
        //}

        //public void InsertItem(long itemID, string tableID, long count)
        //{
        //    CItemData item = new CItemData();
        //    item.ItemID = itemID;
        //    item.TableID = tableID;
        //    item.Count = count;

        //    m_ItemList.Add(item);
        //}


        public void Merge()
        {
            Dictionary<string, _AssetData> dic_coin_tmep = new Dictionary<string, _AssetData>();
            foreach (var iter in m_UpdateCoinList)
            {
                if (dic_coin_tmep.ContainsKey(iter.TableID))
                    dic_coin_tmep[iter.TableID].Count += iter.Count;
                else
                    dic_coin_tmep.Add(iter.TableID, new _AssetData(iter));
            }

            m_UpdateCoinList.Clear();
            foreach (var iter in dic_coin_tmep)
                m_UpdateCoinList.Add(iter.Value);


            Dictionary<string, _ItemData> dic_item_temp = new Dictionary<string, _ItemData>();
            foreach (var iter in m_UpdateItemList)
            {
                if (dic_item_temp.ContainsKey(iter.TableID))
                    dic_item_temp[iter.TableID].Count += iter.Count;
                else
                    dic_item_temp[iter.TableID] = iter;
            }

            m_UpdateItemList.Clear();
            foreach (var iter in dic_item_temp)
                m_UpdateItemList.Add(iter.Value);
        }

        public void CoinMerge()
        {
            Dictionary<string, _AssetData> dic_coin_tmep = new Dictionary<string, _AssetData>();
            foreach (var iter in m_UpdateCoinList)
            {
                if (dic_coin_tmep.ContainsKey(iter.TableID))
                    dic_coin_tmep[iter.TableID].Count += iter.Count;
                else
                    dic_coin_tmep.Add(iter.TableID, new _AssetData(iter));
            }

            m_UpdateCoinList.Clear();
            foreach (var iter in dic_coin_tmep)
                m_UpdateCoinList.Add(iter.Value);
        }

        public void ItemMerge()
        {
            Dictionary<string, _ItemData> dic_item_temp = new Dictionary<string, _ItemData>();
            foreach (var iter in m_UpdateItemList)
            {
                if (dic_item_temp.ContainsKey(iter.TableID))
                    dic_item_temp[iter.TableID].Count += iter.Count;
                else
                    dic_item_temp[iter.TableID] = iter;
            }

            m_UpdateItemList.Clear();
            foreach (var iter in dic_item_temp)
                m_UpdateItemList.Add(iter.Value);
        }
    }


    public class CRewardInfo
    {
        private List<_AssetData> m_RewardDatas = new List<_AssetData>();
        
        public List<_AssetData> GetList()
        {
            return m_RewardDatas;
        }

        public void Insert(CDefine.AssetType type, string rewardID, long value)
        {
            m_RewardDatas.Add(new _AssetData(type, rewardID, value));
        }

        public void Insert(List<_AssetData> list)
        {
            foreach(var it in list)
            {
                var asset = SCommon.SCopy<_AssetData>.DeepCopy(it);
                m_RewardDatas.Add(asset);
            }
        }

        public void Merge()
        {
            var temp = new Dictionary<string, _AssetData>();

            foreach(var iter in m_RewardDatas)
            {
                if (temp.ContainsKey(iter.TableID))
                    temp[iter.TableID].Count += iter.Count;
                else
                    temp.Add(iter.TableID, new _AssetData(iter));
            }

            m_RewardDatas.Clear();
            foreach (var iter in temp)
                m_RewardDatas.Add(iter.Value);
        }

        public void Device()
        {

        }

        public void UpdateRewardData(CUser user, ref CDBMerge dbtran, bool bsend = false)
        {
            if (user == null)
                return;

            user.UpdateAssetData(m_RewardDatas, ref dbtran, bsend);
        }
    }

    public class CAssetAgent
    {
        private CUser m_Owner;
        public CAssetAgent(CUser user) { m_Owner = user; }
        public CUser Owner { get => m_Owner; }

        private Dictionary<string, _AssetData> m_Coins = new Dictionary<string, _AssetData>();
        
        private void Prepare()
        {
            for(CDefine.CoinType coinType = CDefine.CoinType.BlueDia; coinType < CDefine.CoinType.Max; ++coinType)
            {
                _AssetData asset = new _AssetData(CDefine.AssetType.Coin, coinType.ToString(), 0);
                m_Coins[asset.TableID] = asset;
            }
        }

        public List<_AssetData> GetList()
        {
            return new List<_AssetData>(m_Coins.Values);
        }

        public List<_AssetData> GetDBUpdateList(CDBMerge dbtran)
        {
            dbtran.CoinMerge();

            List<_AssetData> retList = new List<_AssetData>();
            foreach(var it in dbtran.m_UpdateCoinList)
            {
                if (!m_Coins.ContainsKey(it.TableID)) 
                    continue;

                var coin = SCopy<_AssetData>.DeepCopy(m_Coins[it.TableID]);
                retList.Add(coin);
            }

            return retList;
        }

        public _AssetData Find(string tableID)
        {
            if (m_Coins.TryGetValue(tableID, out var findVal))
                return findVal;

            return null;
        }

        public bool HasEnoughAsset(string tableID, long cnt)
        {
            var hasdata = Find(tableID);
            if (hasdata == null)
                return false;

            if (hasdata.Count < cnt)
                return false;

            return true;
        }

        public _AssetData Find(CDefine.CoinType type)
        {
            if (m_Coins.TryGetValue(type.ToString(), out var findval))
                return findval;

            return null;
        }

        public void Init(List<_AssetData> assetDatas)
        {
            Prepare();
            foreach (var iter in assetDatas)
                m_Coins[iter.TableID] = iter;
        }

        public void Upsert(_AssetData assetData)
        {
            if(m_Coins.ContainsKey(assetData.TableID))
                m_Coins[assetData.TableID] = assetData;
        }

        public void UpdateRewardAsset(List<_AssetData> assetDatas, ref CDBMerge dbtran)
        {
            foreach (var iter in assetDatas)
            {
                if (iter.Type == CDefine.AssetType.Coin)
                    UpdateRewardAsset(iter, ref dbtran);
            }
        }

        public void UpdateRewardAsset(_AssetData assetData, ref CDBMerge dbtran)
        {
            _AssetData asset;
            if (m_Coins.TryGetValue(assetData.TableID, out asset))
                asset.Count += assetData.Count;
            else
            {
                asset = new _AssetData(assetData);
                m_Coins.Add(asset.TableID, asset);
            }

            m_Owner.UpdateExp(assetData);
            dbtran.InsertCoin(assetData);
        }

        public void Save()
        {

        }

        public _AssetData ClearCoin(CDefine.CoinType type)
        {
            var findData = Find(type);
            if (findData == null)
                return null;

            findData.Count = 0;

            return findData;
        }
    }
}
