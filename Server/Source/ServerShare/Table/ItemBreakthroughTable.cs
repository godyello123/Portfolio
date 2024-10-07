using Global;
using Microsoft.IdentityModel.Tokens;
using PlayServer;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global
{
    public class BreakthroughInfo
    {
        public int ItemEnchant_Level = 0;
        public int OptionTableID = 0;
        public List<_AssetData> Materials = new List<_AssetData>();

        public _AssetData Find(string _itemID)
        {
            var foundData = Materials.Find(x => x.TableID == _itemID);
            if (foundData == null)
                return null;

            return foundData;
        }

    }

    public class ItemBreakthroughRecord : STableRecord<int>
    {
        public int SlotMaxCount = 0;
        public List<BreakthroughInfo> BTinfo = new List<BreakthroughInfo>();

        public BreakthroughInfo Find(int _enchantlevel)
        {
            return BTinfo.Find((x) => { return x.ItemEnchant_Level == _enchantlevel; });
        }

        public BreakthroughInfo FindNearLevel(int _enchantLevel)
        {
            BreakthroughInfo ret = new BreakthroughInfo();
            foreach (var iter in BTinfo)
            {
                if (iter.ItemEnchant_Level <= _enchantLevel)
                    ret = iter;

                if (_enchantLevel < iter.ItemEnchant_Level)
                    return ret;
            }

            return ret;
        }

        public BreakthroughInfo Last()
        {
            if (BTinfo.Count < 1) return null;
            return BTinfo[BTinfo.Count - 1];
        }
    }

    public class ItemBreakthroughTable : STable<ItemBreakthroughTable, int, ItemBreakthroughRecord>
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
                    ItemBreakthroughRecord record = new ItemBreakthroughRecord();
                    record.ID = reader.GetValue<int>(i, "ID", 0);
                    record.SlotMaxCount = reader.GetValue<int>(i, "SlotMaxCount", 0);

                    for (int colidx = 1; true; colidx++)
                    {
                        BreakthroughInfo btinfo = new BreakthroughInfo();
                        string strItemEnchnatLevel = $"ItemEnchant_Level_{colidx}";
                        string strOptionTable = $"OptionTableID_{colidx}";

                        if (!reader.DoesColumnExist(strItemEnchnatLevel) || !reader.DoesColumnExist(strOptionTable))
                            break;

                        string strItemEnchnatLevel_check = reader.GetValue(i, strItemEnchnatLevel);
                        if (string.IsNullOrEmpty(strItemEnchnatLevel_check))
                            break;

                        string strOptionTable_check = reader.GetValue(i, strOptionTable);
                        if (string.IsNullOrEmpty(strOptionTable_check))
                            break;

                        btinfo.ItemEnchant_Level = reader.GetValue<int>(i, strItemEnchnatLevel, 0);
                        btinfo.OptionTableID = reader.GetValue<int>(i, strOptionTable, 0);

                        for (int costidx = 1; true; costidx++)
                        {
                            string strCostType = $"Level_{colidx}_CostAssetType_{costidx}";
                            string strCostItem = $"Level_{colidx}_CostItem_ID_{costidx}";
                            string strCostCnt = $"Level_{colidx}_CostItem_Count_{costidx}";

                            if (!reader.DoesColumnExist(strCostItem) || !reader.DoesColumnExist(strCostCnt) || !reader.DoesColumnExist(strCostType))
                                break;

                            string strCostType_check = reader.GetValue(i, strCostType);
                            if (string.IsNullOrEmpty(strCostType_check))
                                break;

                            string strCostItem_check = reader.GetValue(i, strCostItem);
                            if (string.IsNullOrEmpty(strCostItem_check))
                                break;

                            string strCostCnt_check = reader.GetValue(i, strCostCnt);
                            if (string.IsNullOrEmpty(strCostCnt_check))
                                break;

                            _AssetData mat = new _AssetData();
                            mat.Type = reader.GetEnum<CDefine.AssetType>(i, strCostType, CDefine.AssetType.Max);
                            mat.TableID = reader.GetValue(i, strCostItem, "");
                            mat.Count = reader.GetValue(i, strCostCnt, 0);

                            btinfo.Materials.Add(mat);
                        }

                        record.BTinfo.Add(btinfo);
                    }

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
