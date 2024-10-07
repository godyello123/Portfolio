using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using Global;


namespace Global
{
    public class RewardRecord : STableRecord<int>
    {
        public int GroupID { get; set; }
        public CDefine.AssetType AssetType { get; set; }
        public string RewardID { get; set; }
        public int Value { get; set; }

        public CDefine.CoinType RewardCoinID()
        {
            if (AssetType != CDefine.AssetType.Coin)
                return CDefine.CoinType.Max;

            if (Enum.TryParse<CDefine.CoinType>(RewardID, out var ret))
                return ret;

            return CDefine.CoinType.Max;
        }

        public int RewardItemID()
        {
            if (AssetType != CDefine.AssetType.Item)
                return -1;

            if (int.TryParse(RewardID, out var ret))
                return ret;

            return -1;
        }

        public _AssetData GetAssetData()
        {
            _AssetData ret = new _AssetData();
            ret.Type = AssetType;
            ret.TableID = RewardID;
            ret.Count = Value;

            return ret;
        }
    }

    class RewardTable : SPairKeyTable<RewardTable, int, int, RewardRecord>
    {
        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    RewardRecord record = new RewardRecord();
                    record.ID = reader.GetValue<int>(i, "ID");
                    record.GroupID = reader.GetValue<int>(i, "Group_ID", 0);
                    record.AssetType = reader.GetEnum<CDefine.AssetType>(i, "Asset_Type", CDefine.AssetType.Coin);
                    record.RewardID = reader.GetValue(i, "Reward_Id");
                    record.Value = reader.GetValue(i, "Reward_Value", 0);

                    Add(record.GroupID, record.ID, record);
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            }
        }

        public override void Prepare()
        {
        }

        //public List<CAssetData> GetRewardList(int rewardID)
        //{
        //    var ret = new List<CAssetData>();
        //    var rewards = Find(rewardID);

        //}
    }
}
