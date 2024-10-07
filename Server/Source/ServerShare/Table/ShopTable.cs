using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class ShopRecord : STableRecord<int>
    {
        public int PassID = -1;
        public long Period = -1;
        public string IAPProductID = string.Empty;
        public CDefine.ePurchaseType PurchaseType = CDefine.ePurchaseType.Max;
        public CDefine.CoinType Purchase_CoinType = CDefine.CoinType.Max;
        public int Purchase_CoinValue = 0;
        public long LimitCount = 0;
        public int RewardID = -1;
        public int PostID = -1;

        public bool IsBuyable(int cnt)
        {
            return (cnt <= LimitCount);
        }
    }

    public class ShopTable : STable<ShopTable, int, ShopRecord>
    {
        private Dictionary<string, ShopRecord> m_ForIAP = new Dictionary<string, ShopRecord>();

        private List<_ShopData> m_DefaultShop = new List<_ShopData>();

        public override void Prepare() 
        {
            foreach(var iter in Table)
            {
                _ShopData data = new _ShopData();
                data.m_ShopID = iter.Key;
                data.m_LimitCount = 0;
                data.m_ResetDate = -1;
                data.m_PeriodRewardDate = -1;

                m_DefaultShop.Add(data);
            }
        }
      
        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    ShopRecord record = new ShopRecord();
                    record.ID = reader.GetValue(i, "ID", 0);
                    record.PassID = reader.GetValue(i, "PassID", 0);
                    record.Period = reader.GetValue(i, "Period", 0);
                    record.LimitCount = reader.GetValue(i, "LimitCount", long.MaxValue);
                    record.IAPProductID = reader.GetValue(i, "IAPProductID");
                    record.PostID = reader.GetValue(i, "Post_ID", 0);
                    record.RewardID = reader.GetValue(i, "RewardID", 0);
                    record.PurchaseType = reader.GetEnum<CDefine.ePurchaseType>(i, "PurchaseType", CDefine.ePurchaseType.Max);
                    record.Purchase_CoinType = reader.GetEnum<CDefine.CoinType>(i, "Purchase_CoinType", CDefine.CoinType.Max);
                    record.Purchase_CoinValue = reader.GetValue<int>(i, "Purchase_CoinValue", 0);

                    Add(record.ID, record);

                    if (!string.IsNullOrEmpty(record.IAPProductID))
                    {
                        if (m_ForIAP.ContainsKey(record.IAPProductID) == true)
                        {
                            SCrashManager.AddTableErrorString(string.Format("Table : {0} / Columns : IAPProductID /CoulmnsValue : {2}", this.GetType().FullName, record.IAPProductID));
                        }
                        else
                        {
                            m_ForIAP.Add(record.IAPProductID, record);
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            } 
        }

        public ShopRecord FindIAP(string iapProductID)
        {
            if (m_ForIAP.TryGetValue(iapProductID, out var retVal))
                return retVal;

            return null;
        }

        public List<_ShopData> CopyDefault()
        {
            return SCopy<List<_ShopData>>.DeepCopy(m_DefaultShop);
        }

            
    }
}
