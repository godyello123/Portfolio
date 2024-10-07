using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCommon;
using Global;
using System.Diagnostics;
using System.Windows.Forms;


namespace Global
{
    public class ItemRecord : STableRecord<int>
    {
        public CDefine.EItemMainType MainType = CDefine.EItemMainType.Max;
        public CDefine.EItemDetailType DetailType = CDefine.EItemDetailType.Max;
        public CDefine.EItemGrade ItemGrade = CDefine.EItemGrade.Max;
        public CDefine.EquipSlotType EquipSlot = CDefine.EquipSlotType.Max;
        public bool Remove = false;
        public int StackCount = 0;
        public List<_AbilData> Abils = new List<_AbilData>();
        public List<_AbilData> EquipAbils = new List<_AbilData>();
        public int combine_linkID = 0;
        public int combine_cost = 0;
        public int EnchantID_Knight = 0;
        public int EnchantID_Item = 0;
        public int BreakthroughID = 0;
        public int GachaItemID = 0;
        public bool IsUse = false;
        public int EnchantID_Chivalry = 0;
        public string Price_AssetType = "";
        public int Price_AssetValue = 0;
        public int SkinID = 0;
        public int SkinOptionID = 0;

        //어빌
        public Dictionary<int, List<_AbilData>> LevelOwnedAbils = new Dictionary<int, List<_AbilData>>();
        public Dictionary<int, List<_AbilData>> LevelEquipAbils = new Dictionary<int, List<_AbilData>>();
    }

    public class ItemTable : STable<ItemTable, int, ItemRecord>
    {
        private Dictionary<CDefine.EItemMainType, List<ItemRecord>> m_ItemClassifyMainType = new Dictionary<CDefine.EItemMainType, List<ItemRecord>>();
        private Dictionary<CDefine.EItemDetailType, List<ItemRecord>> m_ItemClassifyDetailType = new Dictionary<CDefine.EItemDetailType, List<ItemRecord>>();

        public ItemRecord Find(string key)
        {
            if (int.TryParse(key, out int itemID))
                return Find(itemID);

            return null;
        }

        public override void Prepare()
        {
            foreach (var it in m_Table)
            {
                var itRecord = it.Value;

                switch (itRecord.MainType)
                {
                    case CDefine.EItemMainType.Weapon:
                    case CDefine.EItemMainType.Defence:
                        EnchantBaseAbils(ref itRecord); break;
                    case CDefine.EItemMainType.Knight:
                        EnchantKnightAbils(ref itRecord); break;
                    case CDefine.EItemMainType.Costume:
                        EnchantCustumeAbils(ref itRecord); break;
                    default:
                        break;
                }

                if (m_ItemClassifyMainType.ContainsKey(itRecord.MainType))
                    m_ItemClassifyMainType[itRecord.MainType].Add(itRecord);
                else
                    m_ItemClassifyMainType.Add(itRecord.MainType, new List<ItemRecord> { itRecord });

                if (m_ItemClassifyDetailType.ContainsKey(itRecord.DetailType))
                    m_ItemClassifyDetailType[itRecord.DetailType].Add(itRecord);
                else
                    m_ItemClassifyDetailType.Add(itRecord.DetailType, new List<ItemRecord> { itRecord });
            }
        }

        public override bool Load(string text)
        {
            Clear();

            SCSVReader reader = new SCSVReader();
            if (!reader.LoadFromString(text)) return false;

            for (int i = 0; i < reader.GetRowCount(); i++)
            {
                ItemRecord record = new ItemRecord();
                record.ID = reader.GetValue<int>(i, "ID");
                record.MainType = reader.GetEnum(i, "MainType", CDefine.EItemMainType.Max);
                record.DetailType = reader.GetEnum(i, "DetailType", CDefine.EItemDetailType.Max);
                record.ItemGrade = reader.GetEnum(i, "ItemGrade", CDefine.EItemGrade.Max);
                record.EquipSlot = reader.GetEnum(i, "EquipSlot", CDefine.EquipSlotType.Max);
                record.Remove = reader.GetValue(i, "Remove", false);
                record.StackCount = reader.GetValue(i, "StackCount", 0);
                record.combine_linkID = reader.GetValue(i, "combine_linkID", -1);
                record.combine_cost = reader.GetValue(i, "combine_cost", -1);
                record.EnchantID_Item = reader.GetValue(i, "EnchantID_Item", 0);
                record.EnchantID_Knight = reader.GetValue(i, "EnchantID_Knight", 0);
                record.BreakthroughID = reader.GetValue<int>(i, "BreakthroughID", 0);
                record.GachaItemID = reader.GetValue<int>(i, "GachaItemID", 0);
                record.IsUse = reader.GetValueBool(i, "IsUse");
                record.EnchantID_Chivalry = reader.GetValue(i, "EnchantID_Chivalry", 0);
                record.Price_AssetType = reader.GetValue(i, "Price_AssetType");
                record.Price_AssetValue = reader.GetValue(i, "Price_AssetValue", 0);
                record.SkinID = reader.GetValue(i, "SkinID", 0);
                record.SkinOptionID = reader.GetValue(i, "SkinOptionID", 0);

                int idx = 1;
                while (true)
                {
                    string strAbilType = "abil_" + idx.ToString();
                    if (!reader.DoesColumnExist(strAbilType))
                        break;

                    string strAbilValue = "abil_value_" + idx.ToString();
                    if (!reader.DoesColumnExist(strAbilValue))
                        break;

                    string strabilType_check = reader.GetValue(i, strAbilType);
                    string straiblvalue_check = reader.GetValue(i, strAbilValue);
                    if (string.IsNullOrEmpty(strabilType_check) || string.IsNullOrEmpty(straiblvalue_check))
                        break;

                    _AbilData abil = new _AbilData();
                    abil.type = reader.GetEnum(i, strAbilType, CDefine.EAbility.Max);
                    abil.val = reader.GetValue(i, strAbilValue, 0.0);

                    if (abil.type != CDefine.EAbility.Max)
                        record.Abils.Add(abil);

                    idx++;
                }

                idx = 1;
                while (true)
                {
                    string strEquipAbilType = "equip_abil_" + idx.ToString();
                    if (!reader.DoesColumnExist(strEquipAbilType))
                        break;


                    string strEquipAbilValue = "equip_abil_value_" + idx.ToString();
                    if (!reader.DoesColumnExist(strEquipAbilValue))
                        break;

                    string strEquipAbilType_check = reader.GetValue(i, strEquipAbilType);
                    string strEquipAbilValue_check = reader.GetValue(i, strEquipAbilValue);
                    if (string.IsNullOrEmpty(strEquipAbilType_check) || string.IsNullOrEmpty(strEquipAbilValue_check))
                        break;

                    _AbilData equipabil = new _AbilData();

                    equipabil.type = reader.GetEnum(i, strEquipAbilType, CDefine.EAbility.Max);
                    equipabil.val = reader.GetValue(i, strEquipAbilValue, 0.0);

                    if (equipabil.type != CDefine.EAbility.Max)
                        record.EquipAbils.Add(equipabil);

                    ++idx;
                }

                Add(record.ID, record);
            }

            return true;
        }

        public List<ItemRecord> Gaher(CDefine.EItemMainType _mainType)
        {
            if (m_ItemClassifyMainType.TryGetValue(_mainType, out var retval))
                return retval;

            return null;
        }

        private void EnchantKnightAbils(ref ItemRecord itRecord)
        {
            //KnightEnchantRecord enchantRecord = KnightEnchantTable.Instance.Find(itRecord.EnchantID_Knight);
            //if (enchantRecord == null)
            //    return;

            //for (int i = 0; i <= enchantRecord.MaxLevel; i++)
            //{
            //    for (int j = 0; j < itRecord.Abils.Count; j++)
            //    {
            //        _AbilData abil = new _AbilData();
            //        abil.type = itRecord.Abils[j].type;
            //        abil.val = ItemEnchantTable.Instance.GetItemEnchantStepValue(itRecord.ItemGrade, itRecord.Abils[j].val, i, enchantRecord.m_Ablis[j]);

            //        if (itRecord.LevelOwnedAbils.ContainsKey(i))
            //            itRecord.LevelOwnedAbils[i].Add(abil);
            //        else
            //            itRecord.LevelOwnedAbils[i] = new List<_AbilData> { abil };
            //    }

            //    for (int j = 0; j < itRecord.EquipAbils.Count; j++)
            //    {
            //        _AbilData abil = new _AbilData();
            //        abil.type = itRecord.EquipAbils[j].type;
            //        abil.val = ItemEnchantTable.Instance.GetItemEnchantStepValue(itRecord.ItemGrade, itRecord.EquipAbils[j].val, i, enchantRecord.m_EquipAbils[j]);

            //        if (itRecord.LevelEquipAbils.ContainsKey(i))
            //            itRecord.LevelEquipAbils[i].Add(abil);
            //        else
            //            itRecord.LevelEquipAbils[i] = new List<_AbilData> { abil };
            //    }
            //}
        }

        private void EnchantBaseAbils(ref ItemRecord itRecord)
        {
            //ItemEnchantRecord enchantRecord = ItemEnchantTable.Instance.Find(itRecord.EnchantID_Item);
            //if (enchantRecord == null)
            //    return;

            //for (int i = 0; i <= enchantRecord.MaxLevel; i++)
            //{
            //    for (int j = 0; j < itRecord.Abils.Count; j++)
            //    {
            //        _AbilData abil = new _AbilData();
            //        abil.type = itRecord.Abils[j].type;
            //        abil.val = ItemEnchantTable.Instance.GetItemEnchantStepValue(itRecord.ItemGrade, itRecord.Abils[j].val, i, enchantRecord.m_Ablis[j]);

            //        if (itRecord.LevelOwnedAbils.ContainsKey(i))
            //            itRecord.LevelOwnedAbils[i].Add(abil);
            //        else
            //            itRecord.LevelOwnedAbils[i] = new List<_AbilData> { abil };
            //    }

            //    for (int j = 0; j < itRecord.EquipAbils.Count; j++)
            //    {
            //        _AbilData abil = new _AbilData();
            //        abil.type = itRecord.EquipAbils[j].type;
            //        abil.val = ItemEnchantTable.Instance.GetItemEnchantStepValue(itRecord.ItemGrade, itRecord.EquipAbils[j].val, i, enchantRecord.m_EquipAbils[j]);

            //        if (itRecord.LevelEquipAbils.ContainsKey(i))
            //            itRecord.LevelEquipAbils[i].Add(abil);
            //        else
            //            itRecord.LevelEquipAbils[i] = new List<_AbilData> { abil };
            //    }
            //}
        }

        private void EnchantCustumeAbils(ref ItemRecord itRecord)
        {
            for (int i = 0; i < itRecord.Abils.Count; i++)
            {
                _AbilData abil = new _AbilData();
                abil.type = itRecord.Abils[i].type;
                abil.val = itRecord.Abils[i].val;

                if (itRecord.LevelOwnedAbils.ContainsKey(i))
                    itRecord.LevelOwnedAbils[i].Add(abil);
                else
                    itRecord.LevelOwnedAbils[i] = new List<_AbilData> { abil };
            }

            for (int i = 0; i < itRecord.EquipAbils.Count; i++)
            {
                _AbilData abil = new _AbilData();
                abil.type = itRecord.EquipAbils[i].type;
                abil.val = itRecord.EquipAbils[i].val;

                if (itRecord.LevelEquipAbils.ContainsKey(i))
                    itRecord.LevelEquipAbils[i].Add(abil);
                else
                    itRecord.LevelEquipAbils[i] = new List<_AbilData> { abil };
            }
        }

        public List<string> GetItemTableIDEntrybyDetailType(CDefine.EItemDetailType type)
        {
            var retList = new List<string>();
            if(m_ItemClassifyDetailType.TryGetValue(type, out var list))
            {
                foreach (var iter in list)
                    retList.Add(iter.ID.ToString());
            }

            return retList; 
        }
    }
}
