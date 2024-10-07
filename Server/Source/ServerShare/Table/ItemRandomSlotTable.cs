using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class ItemRandomSlotRecord : STableRecord<int>
    {
        public int OptionID = 0;
        public int SlotNum = 0;
        public int MaxSlotNum = 0;
        public _AbilData OwnedAbil = new _AbilData();
        public CDefine.CoinType Asset_Type = CDefine.CoinType.Max;
        public long DefaultAsset = 0;
        public long Asset_Increase = 0;
    }

    public class ItemRandomSlotTable : STable<ItemRandomSlotTable, int, ItemRandomSlotRecord>
    {
        public override void Prepare() { }

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    ItemRandomSlotRecord record = new ItemRandomSlotRecord();
                    record.ID = reader.GetValue<int>(i, "ID", 0);
                    record.OptionID = reader.GetValue<int>(i, "OptionID", 0);
                    record.SlotNum = reader.GetValue<int>(i, "SlotNum", 0);
                    record.OwnedAbil.type = reader.GetEnum<CDefine.EAbility>(i, "Abil");
                    record.OwnedAbil.val = reader.GetValue<double>(i, "Abil_value", 0);
                    record.Asset_Type = reader.GetEnum<CDefine.CoinType>(i, "Asset_Type", CDefine.CoinType.Max);
                    record.Asset_Increase = reader.GetValue<int>(i, "Asset_Increase", 0);
                    record.DefaultAsset = reader.GetValue<int>(i, "DefaultAsset", 0);
                    record.MaxSlotNum = reader.GetValue<int>(i, "MaxSlotNum");

                    Add(record.ID, record);
                }

                return true;
            }
            catch (Exception e)
            {
                SCrashManager.AddTableErrorString($"{this.GetType()} : {e.ToString()}");
                return false;
            }

            
        }
    }
}
