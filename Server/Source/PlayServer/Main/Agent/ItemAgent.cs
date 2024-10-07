using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using Google.Apis.AndroidPublisher.v3.Data;
using Microsoft.IdentityModel.Tokens;
using SCommon;
using SDB;
using static MongoDB.Driver.WriteConcern;

namespace PlayServer
{
    public class CItemAgent
    {
        private CUser m_Owner;
        private Dictionary<long, _ItemData> m_Items = new Dictionary<long, _ItemData>();
        private Dictionary<string, List<long>> m_ItemsTIDtoUID = new Dictionary<string, List<long>>();

        private Dictionary<long, _ItemData> m_WaitDBUpdateItems = new Dictionary<long, _ItemData>();

        private Dictionary<int, _ItemEquipPresetData> m_ItemEquipPresets = new Dictionary<int, _ItemEquipPresetData>();
        private Dictionary<int, _KnightEquipPresetData> m_KnightEquipPresets = new Dictionary<int, _KnightEquipPresetData>();
        private List<int> m_ItemPresetModify = new List<int>();
        private List<int> m_KnightPresetModify = new List<int>();

        public CUser Owner { get => m_Owner; }

        public CItemAgent(CUser owner) { m_Owner = owner; }

        public _ItemData CreateItem(string tableID, long count)
        {
            var itemRecord = ItemTable.Instance.Find(tableID);
            if (itemRecord == null)
                return null;

            _ItemData item = new _ItemData();
            item.ItemID = CServerDefine.GeneraterGUID();
            item.TableID = tableID;
            item.Level = 0;
            item.Count = count;

            return item;
        }

        public List<_ItemEquipPresetData> GetItemPresetList()
        {
            return new List<_ItemEquipPresetData>(m_ItemEquipPresets.Values);
        }

        public bool IsEquipSlotItem(CDefine.EquipSlotType _slotType)
        {
            foreach (var iter in m_ItemEquipPresets)
            {
                var preset = iter.Value;

                var slot = preset.m_EquipedSlots.Find((x) => { return x.Type == _slotType && x.ID != -1; });
                if (slot == null)
                    continue;

                var item = Find(slot.ID);
                if (item != null)
                    return true;
            }

            return false;
        }

        public List<_KnightEquipPresetData> GetKnightPresetList()
        {
            return new List<_KnightEquipPresetData>(m_KnightEquipPresets.Values);
        }

        public List<_ItemData> GetItemList()
        {
            return new List<_ItemData>(m_Items.Values);
        }

        public List<_ItemData> GetDBUpdaetList(CDBMerge dbtran)
        {
            dbtran.ItemMerge();
            var retList = new List<_ItemData>();
            foreach (var it in dbtran.m_UpdateItemList)
            {
                if (!m_Items.ContainsKey(it.ItemID)) continue;

                var item = SCopy<_ItemData>.DeepCopy(m_Items[it.ItemID]);
                retList.Add(item);
            }

            return retList;
        }

        public bool HasEnoughItem(string tableID, long cnt)
        {
            var item = FindItemToTableBegin(tableID);
            if (item == null)
                return false;

            if (item.Count < cnt)
                return false;

            return true;
        }

        private void Prepare()
        {
            //item slot preset
            int equip_max = DefineTable.Instance.Value<int>("Equip_Max_Preset");
            for (int i = 0; i < equip_max; ++i)
            {
                _ItemEquipPresetData preset = new _ItemEquipPresetData();
                preset.m_Index = (int)i;

                foreach (CDefine.EquipSlotType type in Enum.GetValues(typeof(CDefine.EquipSlotType)))
                {
                    if (type == CDefine.EquipSlotType.Max)
                        continue;

                    _ItemEquipSlotData slot = new _ItemEquipSlotData();
                    slot.Type = type;
                    slot.ID = -1;
                    preset.m_EquipedSlots.Add(slot);
                }

                m_ItemEquipPresets.Add(i, preset);
            }

            //knight slot preset
            int knight_slot_max = DefineTable.Instance.Value<int>("Knight_Max_Slot");
            for (int i = 0; i < equip_max; ++i)
            {
                _KnightEquipPresetData preset = new _KnightEquipPresetData();
                preset.m_Index = (int)i;

                for (int j = 0; j < knight_slot_max; ++j)
                {
                    _KnightEquipSlotData slot = new _KnightEquipSlotData();
                    slot.No = j;
                    slot.ID = -1;
                    preset.m_EquipedSlots.Add(slot);
                }

                m_KnightEquipPresets.Add(i, preset);
            }
        }

        public void Init(List<_ItemData> _items, List<_ItemEquipPresetData> _equippresets, List<_KnightEquipPresetData> _knightPresset)
        {
            Prepare();

            //load item
            foreach (var iter in _items)
                m_Items.Add(iter.ItemID, iter);

            foreach (var iter in m_Items)
            {
                var itItem = iter.Value;
                if (m_ItemsTIDtoUID.ContainsKey(itItem.TableID))
                    m_ItemsTIDtoUID[itItem.TableID].Add(itItem.ItemID);
                else
                {
                    List<long> uid_tid = new List<long>();
                    uid_tid.Add(itItem.ItemID);
                    m_ItemsTIDtoUID[itItem.TableID] = uid_tid;
                }
            }

            //load preset
            foreach (var iter in _equippresets)
            {
                if (m_ItemEquipPresets.TryGetValue(iter.m_Index, out var preset))
                {
                    m_ItemEquipPresets[iter.m_Index].m_Index = iter.m_Index;
                    m_ItemEquipPresets[iter.m_Index].m_IsEnable = iter.m_IsEnable;
                    foreach (var slot in iter.m_EquipedSlots)
                    {
                        var index = m_ItemEquipPresets[iter.m_Index].m_EquipedSlots.FindIndex(x => x.Type == slot.Type);
                        if (index != -1)
                            m_ItemEquipPresets[iter.m_Index].m_EquipedSlots[index] = slot;
                    }
                }
                else
                {
                    m_ItemEquipPresets.Add(iter.m_Index, iter);
                }
            }

            foreach (var iter in _knightPresset)
                m_KnightEquipPresets[iter.m_Index] = iter;

            RefreshSelectPreset();
        }

        public void GetAbilEquipped_Base(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            var preset = SelectItemPreset();
            foreach (var iter in preset.m_EquipedSlots)
            {
                var item = Find(iter.ID);
                if (item == null)
                    continue;

                var itemRecord = ItemTable.Instance.Find(item.TableID);
                if (itemRecord == null)
                    continue;

                if (itemRecord.LevelEquipAbils.TryGetValue(item.Level, out var getAbils))
                {
                    foreach (var abil in getAbils)
                    {
                        var calcAbil = new _AbilData();
                        calcAbil.type = abil.type;
                        calcAbil.val = abil.val;
                        CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                    }
                }

                foreach (var option in item.RandomOption)
                {
                    var randomOptionRecord = ItemRandomOptionTable.Instance.Find(option.id);
                    if (randomOptionRecord == null)
                        continue;

                    if (randomOptionRecord.Abils.Count <= option.idx)
                        continue;

                    if (option.idx < 0)
                        continue;

                    var calcAbil = new _AbilData();
                    calcAbil.type = randomOptionRecord.Abils[option.idx].Type;
                    calcAbil.val = randomOptionRecord.Abils[option.idx].Value;
                    CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                }
            }
        }

        public void GetAbilEquipped_Knight(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            var preset = SelectKnightPreset();
            foreach (var iter in preset.m_EquipedSlots)
            {
                var foundItem = Find(iter.ID);
                if (foundItem == null)
                    continue;

                var itemRecord = ItemTable.Instance.Find(foundItem.TableID);
                if (itemRecord == null)
                    continue;

                var knightUpRecord = KnightUpgradeTable.Instance.FindGroupID(itemRecord.EnchantID_Knight, foundItem.Level);
                if (knightUpRecord == null)
                    continue;

                foreach (var abil in knightUpRecord.EquipAbils)
                {
                    var calcAbil = new _AbilData();
                    calcAbil.type = abil.type;
                    calcAbil.val = abil.val;
                    CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                }

                foreach (var option in foundItem.RandomOption)
                {
                    var randomOptionRecord = ItemRandomOptionTable.Instance.Find(option.id);
                    if (randomOptionRecord == null)
                        continue;

                    if (randomOptionRecord.Abils.Count <= option.idx)
                        continue;

                    if (option.idx < 0)
                        continue;

                    var calcAbil = new _AbilData();
                    calcAbil.type = randomOptionRecord.Abils[option.idx].Type;
                    calcAbil.val = randomOptionRecord.Abils[option.idx].Value;
                    CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                }
            }
        }

        public void GetAbilOwned_Knight(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            var records = ItemTable.Instance.Gaher(CDefine.EItemMainType.Knight);
            if (records == null)
                return;

            CDefine.EItemGrade optgrade;
            if (!Enum.TryParse<CDefine.EItemGrade>(DefineTable.Instance.Value<string>("RandomOption_Grade"), out optgrade))
                optgrade = CDefine.EItemGrade.Max;

            foreach (var rcd in records)
            {
                var item = FindItemToTableBegin(rcd.ID.ToString());
                if (item == null) continue;

                var itemRecord = ItemTable.Instance.Find(item.TableID);
                if (itemRecord == null)
                    continue;

                //random option
                if (item.RandomOption.Count > 0)
                {
                    var btRecord = ItemBreakthroughTable.Instance.Find(itemRecord.BreakthroughID);
                    if (btRecord == null)
                        continue;

                    var lastbtRecord = btRecord.Last();
                    if (lastbtRecord == null)
                        continue;

                    var slotRecord = ItemRandomSlotTable.Instance.Find(lastbtRecord.OptionTableID);
                    if (slotRecord == null)
                        continue;

                    int reachgradecnt = 0;
                    foreach (var opt in item.RandomOption)
                    {
                        var optionRecord = ItemRandomOptionTable.Instance.Find(opt.id);
                        if (optionRecord == null)
                            continue;

                        if (optgrade <= optionRecord.Grade)
                            reachgradecnt++;
                    }

                    if (slotRecord.MaxSlotNum <= reachgradecnt)
                    {
                        var calcAbil = new _AbilData();
                        calcAbil.type = slotRecord.OwnedAbil.type;
                        calcAbil.val = slotRecord.OwnedAbil.val;

                        CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                    }
                }

            }
        }

        public void GetAbilOwned_Weapon(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            var records = ItemTable.Instance.Gaher(CDefine.EItemMainType.Weapon);
            if (records == null) return;

            CDefine.EItemGrade optgrade;
            if (!Enum.TryParse<CDefine.EItemGrade>(DefineTable.Instance.Value<string>("RandomOption_Grade"), out optgrade))
                optgrade = CDefine.EItemGrade.Max;

            foreach (var rcd in records)
            {
                var item = FindItemToTableBegin(rcd.ID.ToString());
                if (item == null) continue;

                var itemRecord = ItemTable.Instance.Find(item.TableID);
                if (itemRecord == null)
                    continue;

                if (itemRecord.LevelOwnedAbils.TryGetValue(item.Level, out var getAbils))
                {
                    foreach (var abil in getAbils)
                    {
                        var calcAbil = new _AbilData();
                        calcAbil.type = abil.type;
                        calcAbil.val = abil.val;
                        CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                    }
                }

                //random option
                if (item.RandomOption.Count > 0)
                {
                    var btRecord = ItemBreakthroughTable.Instance.Find(itemRecord.BreakthroughID);
                    if (btRecord == null)
                        continue;

                    var lastbtRecord = btRecord.Last();
                    if (lastbtRecord == null)
                        continue;

                    var slotRecord = ItemRandomSlotTable.Instance.Find(lastbtRecord.OptionTableID);
                    if (slotRecord == null)
                        continue;

                    int reachgradecnt = 0;
                    foreach (var opt in item.RandomOption)
                    {
                        var optionRecord = ItemRandomOptionTable.Instance.Find(opt.id);
                        if (optionRecord == null)
                            continue;

                        if (optgrade <= optionRecord.Grade)
                            reachgradecnt++;
                    }

                    if (slotRecord.MaxSlotNum <= reachgradecnt)
                    {
                        var calcAbil = new _AbilData();
                        calcAbil.type = slotRecord.OwnedAbil.type;
                        calcAbil.val = slotRecord.OwnedAbil.val;

                        CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                    }
                }
            }
        }

        public void GetAbilOwned_Defence(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            var records = ItemTable.Instance.Gaher(CDefine.EItemMainType.Defence);
            if (records == null) return;

            CDefine.EItemGrade optgrade;
            if (!Enum.TryParse<CDefine.EItemGrade>(DefineTable.Instance.Value<string>("RandomOption_Grade"), out optgrade))
                optgrade = CDefine.EItemGrade.Max;

            foreach (var rcd in records)
            {
                var item = FindItemToTableBegin(rcd.ID.ToString());
                if (item == null) continue;

                var itemRecord = ItemTable.Instance.Find(item.TableID);
                if (itemRecord == null)
                    continue;

                if (itemRecord.LevelOwnedAbils.TryGetValue(item.Level, out var getAbils))
                {
                    foreach (var abil in getAbils)
                    {
                        var calcAbil = new _AbilData();
                        calcAbil.type = abil.type;
                        calcAbil.val = abil.val;
                        CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                    }
                }

                //random option
                if (item.RandomOption.Count > 0)
                {
                    var btRecord = ItemBreakthroughTable.Instance.Find(itemRecord.BreakthroughID);
                    if (btRecord == null)
                        continue;

                    var lastbtRecord = btRecord.Last();
                    if (lastbtRecord == null)
                        continue;

                    var slotRecord = ItemRandomSlotTable.Instance.Find(lastbtRecord.OptionTableID);
                    if (slotRecord == null)
                        continue;

                    int reachgradecnt = 0;
                    foreach (var opt in item.RandomOption)
                    {
                        var optionRecord = ItemRandomOptionTable.Instance.Find(opt.id);
                        if (optionRecord == null)
                            continue;

                        if (optgrade <= optionRecord.Grade)
                            reachgradecnt++;
                    }

                    if (slotRecord.MaxSlotNum <= reachgradecnt)
                    {
                        var calcAbil = new _AbilData();
                        calcAbil.type = slotRecord.OwnedAbil.type;
                        calcAbil.val = slotRecord.OwnedAbil.val;

                        CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                    }
                }
            }
        }

        public void GetAbilOwned_Chivalry(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            var records = ItemTable.Instance.Gaher(CDefine.EItemMainType.Chivalry);
            if (records == null) return;

            foreach (var rcd in records)
            {
                var item = FindItemToTableBegin(rcd.ID.ToString());
                if (item == null) continue;

                var itemRecord = ItemTable.Instance.Find(item.TableID);
                if (itemRecord == null)
                    continue;

                var chivalryEnchantRecord = ItemChivalryEnchantTable.Instance.Find(itemRecord.EnchantID_Chivalry, item.Level);
                if (chivalryEnchantRecord == null)
                    continue;

                var calcAbil = new _AbilData();
                calcAbil.type = chivalryEnchantRecord.Abil.type;
                calcAbil.val = chivalryEnchantRecord.Abil.val;

                CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
            }
        }

        public void GetAbilOwned_Custume(ref Dictionary<CDefine.EAbility, _AbilData> rAbils)
        {
            var records = ItemTable.Instance.Gaher(CDefine.EItemMainType.Costume);
            if (records == null) return;

            CDefine.EItemGrade optgrade;
            if (!Enum.TryParse<CDefine.EItemGrade>(DefineTable.Instance.Value<string>("RandomOption_Grade"), out optgrade))
                optgrade = CDefine.EItemGrade.Max;

            foreach (var rcd in records)
            {
                var item = FindItemToTableBegin(rcd.ID.ToString());
                if (item == null) continue;

                var itemRecord = ItemTable.Instance.Find(item.TableID);
                if (itemRecord == null)
                    continue;

                if (itemRecord.LevelOwnedAbils.TryGetValue(item.Level, out var getAbils))
                {
                    foreach (var abil in getAbils)
                    {
                        var calcAbil = new _AbilData();
                        calcAbil.type = abil.type;
                        calcAbil.val = abil.val;
                        CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                    }
                }

                //random option
                if (item.RandomOption.Count > 0)
                {
                    var slotRecord = ItemRandomSlotTable.Instance.Find(itemRecord.SkinOptionID);
                    if (slotRecord == null)
                        continue;

                    int reachgradecnt = 0;
                    foreach (var opt in item.RandomOption)
                    {
                        var optionRecord = ItemRandomOptionTable.Instance.Find(opt.id);
                        if (optionRecord == null)
                            continue;

                        if (optgrade <= optionRecord.Grade)
                            reachgradecnt++;
                    }

                    if (slotRecord.MaxSlotNum <= reachgradecnt)
                    {
                        var calcAbil = new _AbilData();
                        calcAbil.type = slotRecord.OwnedAbil.type;
                        calcAbil.val = slotRecord.OwnedAbil.val;

                        CStatusAgent.UpsertAbil(ref rAbils, calcAbil);
                    }
                }
            }
        }

        private void RefreshSelectPreset()
        {
            RefreshItemPreset();
            RefreshKnightPreset();
        }

        private void RefreshItemPreset()
        {
            bool is_check = false;
            foreach (var iter in m_ItemEquipPresets)
            {
                var preset = iter.Value;
                if (preset.m_IsEnable)
                    is_check = true;
            }

            if (!is_check)
            {
                var first = m_ItemEquipPresets.FirstOrDefault();
                var preset = first.Value;
                preset.m_IsEnable = true;
            }
        }


        private void RefreshKnightPreset()
        {
            bool is_check = false;
            foreach (var iter in m_KnightEquipPresets)
            {
                var preset = iter.Value;
                if (preset.m_IsEnable)
                    is_check = true;
            }

            if (!is_check)
            {
                var first = m_KnightEquipPresets.FirstOrDefault();
                var preset = first.Value;
                preset.m_IsEnable = true;
            }
        }

        private bool IsKnight(long itemUID)
        {
            var itemData = Find(itemUID);
            if (itemData == null)
                return false;

            var irecord = ItemTable.Instance.Find(itemData.TableID);
            if (irecord == null)
                return false;

            if (irecord.MainType != CDefine.EItemMainType.Knight)
                return false;

            if (irecord.DetailType == CDefine.EItemDetailType.Horse)
                return false;

            return true;
        }

        public void AutoItemEquip(_ItemData data)
        {
            var item = Find(data.ItemID);
            if (item == null)
                return;

            var record = ItemTable.Instance.Find(data.TableID);
            if (record == null)
                return;

            var preset = SelectItemPreset();

            var equipSlot = preset.m_EquipedSlots.Find((x) => { return x.Type == record.EquipSlot; });
            if (equipSlot == null)
                return;

            equipSlot.ID = item.ItemID;

            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Equip_Item, "", equipSlot.Type.ToString(), 1);

            m_Owner.StatusAgent.RefreshAll();

            m_Owner.RefreshUserOverView();
            CNetManager.Instance.P2C_ReportItemEquipPresetData(m_Owner.SessionKey, preset);
        }

        public List<_ItemData> ReportItemDataList(CDBMerge dbtran)
        {
            var retList = new List<_ItemData>();

            foreach(var iter in dbtran.m_UpdateItemList)
            {
                if (!m_Items.ContainsKey(iter.ItemID))
                    continue;

                var item = SCopy<_ItemData>.DeepCopy(m_Items[iter.ItemID]);
                retList.Add(item);
            }

            return retList;
        }

        public _ItemData Find(long itemUID)
        {
            if (m_Items.TryGetValue(itemUID, out var findVal))
                return findVal;

            return null;
        }

        public List<_ItemData> FindItemToTable(string tableID)
        {
            List<_ItemData> item_datas = new List<_ItemData>();
            if (m_ItemsTIDtoUID.ContainsKey(tableID))
            {
                foreach (var itemID in m_ItemsTIDtoUID[tableID])
                {
                    var item = Find(itemID);
                    if (item == null)
                        continue;

                    item_datas.Add(item);
                }
            }

            return item_datas;
        }

        public _ItemData FindItemToTableBegin(string tableID)
        {
            if (m_ItemsTIDtoUID.ContainsKey(tableID))
            {
                foreach (var itemID in m_ItemsTIDtoUID[tableID])
                {
                    var item = Find(itemID);
                    if (item == null)
                        continue;

                    return item;
                }
            }

            return null;
        }


        public void UpdateRewardAsset(_AssetData asset, ref CDBMerge dbtran, bool bsend = false)
        {
            var itemRecord = ItemTable.Instance.Find(asset.TableID);
            if (itemRecord == null)
                return;

            //todo : stack, nonstack  구분 해야함
            var foundItem = FindItemToTableBegin(asset.TableID);
            if (foundItem == null)
                InsertItem(asset.TableID, asset.Count, ref dbtran, bsend);
            else
                UpdateItemCount(foundItem.ItemID, asset.Count, ref dbtran, bsend);
        }

        public void InsertItem(string tableID, long cnt, ref CDBMerge dbtran, bool bsend = false)
        {
            var itemRecord = ItemTable.Instance.Find(tableID);
            if (itemRecord == null)
                return;

            //todo : stack , nonstack 구분 해야함

            var findItem = FindItemToTableBegin(tableID);
            if (findItem == null)
            {
                _ItemData item = CreateItem(tableID, cnt);

                if (!m_Items.ContainsKey(item.ItemID))
                    m_Items.Add(item.ItemID, item);

                List<long> item_uids = new List<long>();
                item_uids.Add(item.ItemID);
                m_ItemsTIDtoUID[item.TableID] = item_uids;

                dbtran.InsertUpdateItem(item.ItemID, item.TableID, item.Count);
                //dbtran.InsertItem(item.ItemID, item.TableID, item.Count);

                if (bsend)
                    CNetManager.Instance.P2C_ReportItem(m_Owner.SessionKey, item);
            }
            else
            {
                UpdateItemCount(findItem.ItemID, cnt, ref dbtran, bsend);
            }
        }

        public void EraseItem(_ItemData item)
        {
            m_Items.Remove(item.ItemID);
            m_ItemsTIDtoUID.Remove(item.TableID);
        }

        public void UpdateItemCount(long itemID, long cnt, ref CDBMerge dbtran, bool bsend)
        {
            var findItem = Find(itemID);
            if (findItem == null)
                return;

            findItem.Count += cnt;

            var itemRecord = ItemTable.Instance.Find(findItem.TableID);
            if (itemRecord.Remove && findItem.Count < 1)
            {
                EraseItem(findItem);
                dbtran.InsertDeleteItem(findItem.ItemID, findItem.TableID, findItem.Count);
            }
            else
            {
                dbtran.InsertUpdateItem(findItem.ItemID, findItem.TableID, cnt);
                //dbtran.InsertItem(findItem.ItemID, findItem.TableID, findItem.Count);
            }

            if (bsend)
                CNetManager.Instance.P2C_ReportItem(m_Owner.SessionKey, findItem);
        }


        public _ItemEquipPresetData FindItemPreset(int idx)
        {
            if (m_ItemEquipPresets.ContainsKey(idx))
                return m_ItemEquipPresets[idx];

            return null;
        }

        public _KnightEquipPresetData FindKnightPreset(int idx)
        {
            if (m_KnightEquipPresets.ContainsKey(idx))
                return m_KnightEquipPresets[idx];

            return null;
        }

        private _ItemEquipPresetData SelectItemPreset()
        {
            foreach(var it in m_ItemEquipPresets)
            {
                var preset = it.Value;
                if (preset.m_IsEnable)
                    return preset;
            }

            return null;
        }

        public List<_ItemData> GetEnablePresetItemList()
        {
            List<_ItemData> retList = new List<_ItemData>();
            var preset = SelectItemPreset();
            foreach(var iter in preset.m_EquipedSlots)
            {
                var item = Find(iter.ID);
                if (item == null)
                    continue;

                retList.Add(item);
            }

            return retList;
        }

        public List<_ItemData> GetEnablePresetKnightList()
        {
            List<_ItemData> retList = new List<_ItemData>();
            var preset = SelectKnightPreset();
            foreach(var iter in preset.m_EquipedSlots)
            {
                var item = Find(iter.ID);
                if (item == null)
                    continue;

                retList.Add(item);
            }

            return retList;
        }

        public List<_KnightData> GetEnableKnightList()
        {
            List<_KnightData> retList = new List<_KnightData>();
            var preset = SelectKnightPreset();
            foreach (var iter in preset.m_EquipedSlots)
            {
                var item = Find(iter.ID);
                if (item == null)
                    continue;

                _KnightData knight = new _KnightData();
                knight.m_SoliderID = item.TableID;
                knight.m_SoliderLv = item.Level;

                //var horse = Find(item.HorseID);
                //if (horse != null)
                //{
                //    knight.m_HorseID = horse.TableID;
                //    knight.m_HorseLv = horse.Level;
                //}

                retList.Add(knight);
            }

            return retList;
        }


        private _KnightEquipPresetData SelectKnightPreset()
        {
            foreach(var it in m_KnightEquipPresets)
            {
                var preset = it.Value;
                if (preset.m_IsEnable)
                    return preset;
            }

            return null;
        }

        public Packet_Result.Result ReqItemEquip(long sessionKey, int presetIdx, long reqUID, bool bEquip)
        {
            //valid
            var preset = FindItemPreset(presetIdx);
            if (preset == null)
                return Packet_Result.Result.PacketError;

            var item = Find(reqUID);
            if (item == null)
                return Packet_Result.Result.PacketError;

            var itemRecord = ItemTable.Instance.Find(item.TableID);
            if (itemRecord == null)
                return Packet_Result.Result.PacketError;

            var equipSlot = preset.m_EquipedSlots.Find((x) => { return x.Type == itemRecord.EquipSlot; });
            if (equipSlot == null)
                return Packet_Result.Result.PacketError;

            if (bEquip)
            {
                equipSlot.ID = item.ItemID;

                m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Equip_Item, "", equipSlot.Type.ToString(), 1);
            }
            else
            {
                equipSlot.ID = CServerDefine.EmptySlotGUID;
            }

            m_Owner.StatusAgent.Refresh(new List<eStatusCategory> { eStatusCategory.ItemEquipped_Base });

            m_Owner.RefreshUserOverView();
            //CNetManager.Instance.P2C_ReportItemEquipPresetData(sessionKey, preset);
            CNetManager.Instance.P2C_ResultItemEquip(sessionKey, preset, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqSelectItemEquipPreset(long sessionKey, int idx)
        {
            var preset = FindItemPreset(idx);
            if (preset == null)
                return Packet_Result.Result.InValidData;

            var prevSelectedPreset = SelectItemPreset();
            if (prevSelectedPreset == null)
                return Packet_Result.Result.PacketError;

            if (prevSelectedPreset.m_Index == preset.m_Index)
                return Packet_Result.Result.IgnoreError;

            prevSelectedPreset.m_IsEnable = false;
            preset.m_IsEnable = true;

            
            m_Owner.StatusAgent.Refresh(new List<eStatusCategory> { eStatusCategory.ItemEquipped_Base });
            m_Owner.RefreshUserOverView();

            //CNetManager.Instance.P2C_ReportItemEquipPresetData(sessionKey, preset);
            CNetManager.Instance.P2C_ResultSelectItemEquipPreset(sessionKey, preset.m_Index, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqSelectKnightEquipPreset(long sessionKey, int idx)
        {
            var preset = FindKnightPreset(idx);
            if (preset == null)
                return Packet_Result.Result.InValidData;

            var prevSelectedPreset = SelectKnightPreset();
            if (prevSelectedPreset == null)
                return Packet_Result.Result.PacketError;

            if (prevSelectedPreset.m_Index == preset.m_Index)
                return Packet_Result.Result.IgnoreError;

            prevSelectedPreset.m_IsEnable = false;
            preset.m_IsEnable = true;
            
            m_Owner.StatusAgent.Refresh(new List<eStatusCategory> { eStatusCategory.ItemEquipped_Knight });
            m_Owner.RefreshUserOverView();
            CNetManager.Instance.P2C_ResultSelectKnightEquipPreset(sessionKey, preset.m_Index, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqKnightEquip(long sessionKey, int presetNo, int slotNo, long reqID, bool bEquip)
        {
            //vaild
            var preset = FindKnightPreset(presetNo);
            if (preset == null)
                return Packet_Result.Result.InValidData;

            var equipSlot = preset.m_EquipedSlots.Find((x) => { return x.No == slotNo; });
            if (equipSlot == null)
                return Packet_Result.Result.InValidData;

            //process
            if (bEquip)
            {
                if (false == IsKnight(reqID))
                    return Packet_Result.Result.InValidData;

                var duplicateSlot = preset.m_EquipedSlots.Find((x) => { return x.ID == reqID; });
                if (duplicateSlot != null)
                    return Packet_Result.Result.PacketError;

                equipSlot.ID = reqID;

                m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Equip_Knight, "", "", 1);
            }
            else
            {
                if (equipSlot.ID == CServerDefine.EmptySlotGUID)
                    return Packet_Result.Result.PacketError;

                equipSlot.ID = CServerDefine.EmptySlotGUID;
            }

            m_Owner.StatusAgent.Refresh(new List<eStatusCategory>
            {
                eStatusCategory.ItemEquipped_Knight,
                eStatusCategory.ItemOwned_Knight
            });

            m_Owner.RefreshUserOverView();

            //CNetManager.Instance.P2C_ReportKnightEquipPresetData(sessionKey, preset);
            CNetManager.Instance.P2C_ResultKnightEquip(sessionKey, preset, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqKnightUpgrade(long _sessionKey, long kngihtUID)
        {
            var foundItem = Find(kngihtUID);
            if (foundItem == null)
                return Packet_Result.Result.PacketError;

            var itemRecord = ItemTable.Instance.Find(foundItem.TableID);
            if (itemRecord == null)
                return Packet_Result.Result.InValidRecord;

            var knightUpRecord = KnightUpgradeTable.Instance.FindGroupID(itemRecord.EnchantID_Knight, foundItem.Level);
            if (knightUpRecord == null)
                return Packet_Result.Result.InValidRecord;

            var knihhtnextReocrd = KnightUpgradeTable.Instance.Find(knightUpRecord.NextID);
            if (knihhtnextReocrd == null)
                return Packet_Result.Result.AlreadyLast;

            var slotRecord = ItemRandomSlotTable.Instance.Find(knihhtnextReocrd.SlotTableID);
            if (slotRecord == null)
                return Packet_Result.Result.InValidRecord;

            if (!m_Owner.HasEnoughAsset(CDefine.AssetType.Item, knightUpRecord.AccessoryMaterial.ToString(), knightUpRecord.AccessoryMaterial_Count))
                return Packet_Result.Result.LackAssetError;

            CDBMerge dbtran = new CDBMerge();
            CRewardInfo forClient = new CRewardInfo();

            bool bSend = true;
            _AssetData costAsset = new _AssetData(CDefine.AssetType.Item, knightUpRecord.AccessoryMaterial.ToString(), -knightUpRecord.AccessoryMaterial_Count);
            m_Owner.UpdateAssetData(costAsset, ref dbtran, bSend);

            foundItem.Level = knihhtnextReocrd.Star;

            for (int i = foundItem.RandomOption.Count; i < slotRecord.SlotNum; ++i)
            {
                var option = new _RandomOption();
                option.no = i;
                option.locked = false;
                option.id = -1;
                option.idx = -1;
                foundItem.RandomOption.Add(option);
            }

            dbtran.Merge();

            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Enchant_Item, "", itemRecord.MainType.ToString(), 1);
            m_Owner.StatusAgent.Refresh(new List<eStatusCategory> { eStatusCategory.ItemEquipped_Knight, eStatusCategory.ItemOwned_Knight });
            CDBManager.Instance.QueryCharacterItemKnightUpgrade(m_Owner.DBGUID, _sessionKey, m_Owner.UID, foundItem, dbtran);
            CNetManager.Instance.P2C_ResultKnightUpgrade(_sessionKey, foundItem, Packet_Result.Result.Success);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_enchant, m_Owner.UserData, "", dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqItemCombine(long _sessionKey, long _cunsumeid, int _count)
        {
            //valid
            if (_count < 1)
                return Packet_Result.Result.PacketError;

            var reqItem = Find(_cunsumeid);
            if (reqItem == null)
                return Packet_Result.Result.NotFoundData;

            var consumeitem_record = ItemTable.Instance.Find(reqItem.TableID);
            if (consumeitem_record == null)
                return Packet_Result.Result.InValidRecord;

            var nextitem_record = ItemTable.Instance.Find(consumeitem_record.combine_linkID);
            if (nextitem_record == null)
                return Packet_Result.Result.PacketError;

            if (consumeitem_record.combine_cost < 1)
                return Packet_Result.Result.PacketError;

            int needConsumeCount = consumeitem_record.combine_cost * _count;

            if (reqItem.Count < needConsumeCount)
                return Packet_Result.Result.LackAssetError;

            //process
            CDBMerge dbtran = new CDBMerge();

            InsertItem(reqItem.TableID, -needConsumeCount, ref dbtran, true);
            InsertItem(nextitem_record.ID.ToString(), _count, ref dbtran, true);

            dbtran.Merge();

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Item_Combine, "", nextitem_record.MainType.ToString(), _count);

            var resultItemList = ReportItemDataList(dbtran);

            CDBManager.Instance.QueryCharacterUpdateItemCountList(m_Owner.DBGUID, _sessionKey, m_Owner.UID, dbtran);
            CNetManager.Instance.P2C_ResultItemCombine(_sessionKey, resultItemList, Packet_Result.Result.Success);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_combine, m_Owner.UserData, "", dbtran, _count);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqItemCombineAll(long _sessionKey, List<long> _consumeids)
        {
            //valid
            if (_consumeids.Count < 1)
                return Packet_Result.Result.NotFoundData;

            Dictionary<long, long> temp = new Dictionary<long, long>();
            foreach (var it in _consumeids)
                temp[it] = it;

            _consumeids.Clear();

            foreach (var it in temp)
                _consumeids.Add(it.Value);

            CDefine.EItemMainType mainType = CDefine.EItemMainType.Max;
            CDefine.EItemDetailType subType = CDefine.EItemDetailType.Max;

            List<_ItemData> consumeList = new List<_ItemData>();
            foreach (var it in _consumeids)
            {
                var itemData = Find(it);
                if (itemData == null)
                    return Packet_Result.Result.InValidData;

                var itemRecord = ItemTable.Instance.Find(itemData.TableID);
                if (itemRecord == null)
                    return Packet_Result.Result.InValidRecord;

                if (mainType != CDefine.EItemMainType.Max && mainType != itemRecord.MainType)
                    return Packet_Result.Result.InValidData;

                if (subType != CDefine.EItemDetailType.Max && subType != itemRecord.DetailType)
                    return Packet_Result.Result.InValidData;

                mainType = itemRecord.MainType;
                subType = itemRecord.DetailType;

                if (itemRecord.combine_linkID < 0)
                    return Packet_Result.Result.PacketError;

                var resultRecord = ItemTable.Instance.Find(itemRecord.combine_linkID);
                if (resultRecord == null)
                    return Packet_Result.Result.PacketError;

                //0 division check
                if (itemRecord.combine_cost < 1)
                    return Packet_Result.Result.PacketError;

                long resultCount = itemData.Count / itemRecord.combine_cost;
                if (resultCount < 1)
                    return Packet_Result.Result.PacketError;

                long consumeCnt = itemRecord.combine_cost * resultCount;
                if (consumeCnt < 1)
                    return Packet_Result.Result.PacketError;

                if (itemData.Count < itemRecord.combine_cost)
                    return Packet_Result.Result.PacketError;

                consumeList.Add(itemData);
            }

            if (consumeList.Count < 1)
                return Packet_Result.Result.PacketError;

            //process
            
            bool bSend = true;
            long totalCnt = 0;
            CDBMerge dbtran = new CDBMerge();
            foreach (var it in consumeList)
            {
                var itemRecord = ItemTable.Instance.Find(it.TableID);
                if (itemRecord == null)
                    return Packet_Result.Result.InValidRecord;

                if (itemRecord.combine_linkID < 0)
                    return Packet_Result.Result.PacketError;

                var resultRecord = ItemTable.Instance.Find(itemRecord.combine_linkID);
                if (resultRecord == null)
                    return Packet_Result.Result.PacketError;

                //0 division check
                if (itemRecord.combine_cost < 1)
                    return Packet_Result.Result.PacketError;

                if (it.Count < itemRecord.combine_cost)
                    return Packet_Result.Result.PacketError;

                long resultCount = it.Count / itemRecord.combine_cost;
                if (resultCount < 1)
                    return Packet_Result.Result.PacketError;

                long consumeCnt = itemRecord.combine_cost * resultCount;
                if (consumeCnt < 1)
                    return Packet_Result.Result.PacketError;

                InsertItem(it.TableID, -consumeCnt, ref dbtran, bSend);
                InsertItem(resultRecord.ID.ToString(), resultCount, ref dbtran, bSend);

                totalCnt += resultCount;
            }

            dbtran.Merge();

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Item_Combine, "", mainType.ToString(), totalCnt);
            var resultItemList = ReportItemDataList(dbtran);

            CDBManager.Instance.QueryCharacterUpdateItemCountList(m_Owner.DBGUID, _sessionKey, m_Owner.UserData.m_UID, dbtran);
            CNetManager.Instance.P2C_ResultItemCombineAll(_sessionKey, resultItemList, Packet_Result.Result.Success);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_combine, m_Owner.UserData, "", dbtran, totalCnt);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqItemEnchant(long _sessionKey, long _reqID, int _targetlevel)
        {
            //valid
            var reqItem = Find(_reqID);
            if (reqItem == null)
            {
                CLogger.Instance.Debug($"reqItem == null - _reqID : {_reqID}");
                return Packet_Result.Result.PacketError;
            }

            if (reqItem.Level >= _targetlevel)
            {
                CLogger.Instance.Debug($"reqItem.Level >= _targetlevel - ItemLevle : {reqItem.Level}, tagetLevel : {_targetlevel}");
                return Packet_Result.Result.PacketError;
            }

            var itemRecord = ItemTable.Instance.Find(reqItem.TableID);
            if (itemRecord == null)
            {
                CLogger.Instance.Debug($"itemRecord == null - itemTableID : {reqItem.TableID}");
                return Packet_Result.Result.PacketError;
            }

            switch(itemRecord.MainType) 
            {
                case CDefine.EItemMainType.Weapon:
                case CDefine.EItemMainType.Defence:
                    return ReqItemEnchantBase(_sessionKey, _reqID, _targetlevel);
                case CDefine.EItemMainType.Chivalry:
                    return ReqItemChivalryEnchant(_sessionKey, _reqID);
            }

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ReqItemEnchantBase(long _sessionKey, long _reqID, int goalLevel)
        {
            var reqItem = Find(_reqID);
            if (reqItem == null)
            {
                CLogger.Instance.Debug($"reqItem == null - _reqID : {_reqID}");
                return Packet_Result.Result.PacketError;
            }

            if (reqItem.Level >= goalLevel)
            {
                CLogger.Instance.Debug($"reqItem.Level >= _targetlevel - ItemLevle : {reqItem.Level}, tagetLevel : {goalLevel}");
                return Packet_Result.Result.PacketError;
            }

            var itemRecord = ItemTable.Instance.Find(reqItem.TableID);
            if (itemRecord == null)
            {
                CLogger.Instance.Debug($"itemRecord == null - itemTableID : {reqItem.TableID}");
                return Packet_Result.Result.PacketError;
            }

            var enchantRecord = ItemEnchantTable.Instance.Find(itemRecord.EnchantID_Item);
            if (enchantRecord == null)
            {
                CLogger.Instance.Debug($"enchantRecord == null - enchantid : {itemRecord.EnchantID_Item}");
                return Packet_Result.Result.PacketError;
            }

            if (enchantRecord.MaxLevel < goalLevel)
            {
                CLogger.Instance.Debug($"enchantRecord.MaxLevel < _targetlevel - MaxLevel : {enchantRecord.MaxLevel} , targelevel : {goalLevel}");
                return Packet_Result.Result.PacketError;
            }

            var breakthroughRecord = ItemBreakthroughTable.Instance.Find(itemRecord.BreakthroughID);
            if (breakthroughRecord != null)
            {
                var btInfo = breakthroughRecord.FindNearLevel(goalLevel);
                if (btInfo != null)
                {
                    var slotRecord = ItemRandomSlotTable.Instance.Find(btInfo.OptionTableID);
                    if (slotRecord != null)
                    {
                        if (reqItem.RandomOption.Count != slotRecord.SlotNum)
                        {
                            if (btInfo.ItemEnchant_Level < goalLevel)
                            {
                                CLogger.Instance.Debug($"btInfo.ItemEnchant_Level < _targetlevel - {btInfo.ItemEnchant_Level} < {goalLevel}");
                                return Packet_Result.Result.PacketError;
                            }
                        }
                    }
                }
            }

            //process
            long needVal = 0;
            for (int i = reqItem.Level + 1; i <= goalLevel; ++i)
                needVal += CDefine.GetItemEnchantPrice(i, enchantRecord.Enchant_StartCost, enchantRecord.Enchant_IncreaseCost);

            if (!m_Owner.AssetAgent.HasEnoughAsset(enchantRecord.Enchant_CoinType, needVal))
            {
                CLogger.Instance.Debug($"not EnoughAsset - assetType : {enchantRecord.Enchant_CoinType}");
                return Packet_Result.Result.PacketError;
            }

            CDBMerge dbtran = new CDBMerge();
            _AssetData consumeAsset = new _AssetData(CDefine.AssetType.Coin, enchantRecord.Enchant_CoinType, -needVal);
            m_Owner.UpdateAssetData(consumeAsset, ref dbtran);

            //mission
            int missionVal = goalLevel - reqItem.Level;
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Enchant_Item, "", itemRecord.MainType.ToString(), missionVal);

            reqItem.Level = goalLevel;

            dbtran.Merge();

            //abil refresh
            m_Owner.StatusAgent.Refresh(new List<eStatusCategory>
            {
                eStatusCategory.ItemEquipped_Base, eStatusCategory.ItemOwned_Weapon, eStatusCategory.ItemOwned_Defence
            });

            CNetManager.Instance.P2C_ReportAssetData(_sessionKey, m_Owner.AssetAgent.GetList());
            CDBManager.Instance.QueryCharacterUpdateItemLevel(m_Owner.DBGUID, _sessionKey, m_Owner.UserData.m_UID, reqItem, dbtran);
            CNetManager.Instance.P2C_ResultItemEnchant(_sessionKey, reqItem, Packet_Result.Result.Success);

            //todo : log save
            //var log = LogHelper.MakeLog(eLogType.item_enchant, m_Owner.UserData, LogHelper.ToJson(reqItem), dbtran, goalLevel);
            //var log = LogHelper.MakeLogBson(eLogType.item_enchant, m_Owner.UserData, LogHelper.ToJson(reqItem), dbtran, goalLevel);
            //CMongoDBManager.Instance.Insert(new InsertQuery(CConfig.Instance.MongoDBName, "log", SBson.ObjectToBson(log)));

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ReqItemChivalryEnchant(long _sessionKey, long _reqUID)
        {
            var itemData = Find(_reqUID);
            if (itemData == null)
            {
                CLogger.Instance.Debug($"itemData == null, _reqUID : {_reqUID}");
                return Packet_Result.Result.NotFoundData;
            }

            var itemRecord = ItemTable.Instance.Find(itemData.TableID);
            if (itemRecord == null)
            {
                CLogger.Instance.Debug($"itemRecord == null, itemData.TableID : {itemData.TableID}");
                return Packet_Result.Result.InValidRecord;
            }

            int targetLevel = itemData.Level + 1;

            var chivalyEnchantRecord = ItemChivalryEnchantTable.Instance.Find(itemRecord.EnchantID_Chivalry, targetLevel);
            if (chivalyEnchantRecord == null)
            {
                CLogger.Instance.Debug($"chivalyEnchantRecord == null, itemRecord.EnchantID_Chivalry : {itemRecord.EnchantID_Chivalry}");
                return Packet_Result.Result.InValidRecord;
            }

            if (!HasEnoughItem(itemData.TableID, chivalyEnchantRecord.CostCount))
            {
                CLogger.Instance.Debug($"chivalyEnchantRecord == null, itemRecord.EnchantID_Chivalry : {itemRecord.EnchantID_Chivalry}");
                return Packet_Result.Result.InValidRecord;
            }

            bool bSend = true;
            CDBMerge dbtran = new CDBMerge();
            UpdateItemCount(itemData.ItemID, -chivalyEnchantRecord.CostCount, ref dbtran, bSend);

            if (chivalyEnchantRecord.Roulette())
                itemData.Level = chivalyEnchantRecord.EnchantLv;

            dbtran.Merge();

            m_Owner.StatusAgent.Refresh(new List<eStatusCategory>
            {
                eStatusCategory.ItemOwned_Chivalry
            });

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Enchant_Item, "", itemRecord.MainType.ToString(), 1);

            CDBManager.Instance.QueryCharacterUpdateItemLevel(m_Owner.DBGUID, _sessionKey, m_Owner.UserData.m_UID, itemData, dbtran);
            CNetManager.Instance.P2C_ResultItemEnchant(_sessionKey, itemData, Packet_Result.Result.Success);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_enchant, m_Owner.UserData, LogHelper.ToJson(itemData), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqItemBreakthrough(long _sessionKey, long _reqID)
        {
            //valid
            var reqItem = Find(_reqID);
            if (reqItem == null)
            {
                CLogger.Instance.Debug($"reqItem, _reqID : {_reqID}");
                return Packet_Result.Result.NotFoundData;
            }

            var itemRecord = ItemTable.Instance.Find(reqItem.TableID);
            if (itemRecord == null)
            {
                CLogger.Instance.Debug($"itemRecord == null , reqItem.TableID : {reqItem.TableID}");
                return Packet_Result.Result.InValidRecord;
            }

            var breakthrouthRecord = ItemBreakthroughTable.Instance.Find(itemRecord.BreakthroughID);
            if (breakthrouthRecord == null)
            {
                CLogger.Instance.Debug($"breakthrouthRecord == null, itemRecord.BreakthroughID : {itemRecord.BreakthroughID}");
                return Packet_Result.Result.InValidRecord;
            }

            var btInfo = breakthrouthRecord.FindNearLevel(reqItem.Level);
            if (btInfo == null)
            {
                CLogger.Instance.Debug($"btInfo == null, reqItem.Level : {reqItem.Level}");
                return Packet_Result.Result.InValidData;
            }

            foreach (var iter in btInfo.Materials)
            {
                if (!m_Owner.HasEnoughAsset(iter.Type, iter.TableID, iter.Count))
                {
                    CLogger.Instance.Debug($"not enough asset, Type : {iter.TableID}, Value : {iter.Count}");
                    return Packet_Result.Result.LackAssetError;
                }
            }

            var slotRecord = ItemRandomSlotTable.Instance.Find(btInfo.OptionTableID);
            if (slotRecord == null)
            {
                CLogger.Instance.Debug($"slotRecord == null , btInfo.OptionTableID : {btInfo.OptionTableID}");
                return Packet_Result.Result.InValidRecord;
            }

            if (reqItem.RandomOption.Count >= slotRecord.SlotNum)
            {
                CLogger.Instance.Debug($"reqItem.RandomOption.Count >= slotRecord.SlotNum, {reqItem.RandomOption.Count} > {slotRecord.SlotNum}");
                return Packet_Result.Result.InValidData;
            }


            //process
            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            bool bSend = true;
            foreach (var iter in btInfo.Materials)
            {
                var consumeMat = new _AssetData(iter.Type, iter.TableID, -iter.Count);
                m_Owner.UpdateAssetData(consumeMat, ref dbtran, bSend);
            }

            for (int i = reqItem.RandomOption.Count; i < slotRecord.SlotNum; ++i)
            {
                var option = new _RandomOption();
                option.no = i;
                option.locked = false;
                option.id = -1;
                option.idx = -1;
                reqItem.RandomOption.Add(option);
            }

            dbtran.Merge();

            m_Owner.StatusAgent.Refresh(new List<eStatusCategory>
            {
                eStatusCategory.ItemEquipped_Base, eStatusCategory.ItemEquipped_Knight,
                eStatusCategory.ItemOwned_Defence, eStatusCategory.ItemOwned_Weapon, eStatusCategory.ItemOwned_Knight
            });

            CNetManager.Instance.P2C_ReportAssetData(_sessionKey, m_Owner.AssetAgent.GetList());
            CDBManager.Instance.QueryCharacterUpdateItemBreakThrough(m_Owner.DBGUID, _sessionKey, m_Owner.UserData.m_UID, reqItem, dbtran);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_breakthrougth, m_Owner.UserData, LogHelper.ToJson(reqItem), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqItemRandomOptionChange(long _sessionKey, long _reqID)
        {
            //valid
            var reqItem = Find(_reqID);
            if (reqItem == null)
            {
                CLogger.Instance.Debug($"reqItem == null - _reqID : {_reqID}");
                return Packet_Result.Result.PacketError;
            }

            var itemRecord = ItemTable.Instance.Find(reqItem.TableID);
            if (itemRecord == null)
            {
                CLogger.Instance.Debug($"itemRecord == null - itemTalbeID : {reqItem.TableID}");
                return Packet_Result.Result.PacketError;
            }


            ItemRandomSlotRecord slotRecord = new ItemRandomSlotRecord();
            if (itemRecord.MainType == CDefine.EItemMainType.Costume)
            {
                slotRecord = ItemRandomSlotTable.Instance.Find(itemRecord.SkinOptionID);
            }
            else if(itemRecord.MainType == CDefine.EItemMainType.Knight)
            {
                var knightUpRecord = KnightUpgradeTable.Instance.FindGroupID(itemRecord.EnchantID_Knight, reqItem.Level);
                if (knightUpRecord == null)
                    return Packet_Result.Result.PacketError;

                slotRecord = ItemRandomSlotTable.Instance.Find(knightUpRecord.SlotTableID);
            }
            else
            {
                var breakthrouthRecord = ItemBreakthroughTable.Instance.Find(itemRecord.BreakthroughID);
                if (breakthrouthRecord == null)
                {
                    CLogger.Instance.Debug($"breakthrouthRecord == null - breakthroughID : {itemRecord.BreakthroughID}");
                    return Packet_Result.Result.PacketError;
                }

                var btInfo = breakthrouthRecord.FindNearLevel(reqItem.Level);
                if (btInfo == null)
                {
                    CLogger.Instance.Debug($"btInfo == null - reqItem.Level : {reqItem.Level}, slot : {reqItem.RandomOption.Count}");
                    return Packet_Result.Result.PacketError;
                }

                slotRecord = ItemRandomSlotTable.Instance.Find(btInfo.OptionTableID);
            }

            if (slotRecord == null)
                return Packet_Result.Result.PacketError;

            int lockedCnt = reqItem.RandomOption.Count(x => x.locked == true);
            long needVal = slotRecord.DefaultAsset + (lockedCnt * slotRecord.Asset_Increase);

            if (!m_Owner.AssetAgent.HasEnoughAsset(slotRecord.Asset_Type.ToString(), needVal))
            {
                CLogger.Instance.Debug($"not EnoughAsset - needAsset {slotRecord.Asset_Type}:{needVal}");
                return Packet_Result.Result.PacketError;
            }

            for (int i = 0; i < reqItem.RandomOption.Count; i++)
            {
                var optionRecord = ItemRandomOptionTable.Instance.GroupRoulette(slotRecord.OptionID);
                if (optionRecord == null)
                {
                    CLogger.Instance.Debug($"optionRecord == null - optionID : {slotRecord.OptionID}");
                    return Packet_Result.Result.PacketError;
                }

                if (reqItem.RandomOption[i].locked)
                    continue;

                var tuple = optionRecord.Roulette();
                reqItem.RandomOption[i].id = tuple.Item1;
                reqItem.RandomOption[i].idx = tuple.Item2;
            }

            CDBMerge dbtran = new CDBMerge();
            var needAsset = new _AssetData(CDefine.AssetType.Coin, slotRecord.Asset_Type.ToString(), -needVal);
            m_Owner.UpdateAssetData(needAsset, ref dbtran);
            dbtran.Merge();

            m_Owner.StatusAgent.Refresh(new List<eStatusCategory>
            {
                eStatusCategory.ItemEquipped_Base, eStatusCategory.ItemEquipped_Knight,
                eStatusCategory.ItemOwned_Knight, eStatusCategory.ItemOwned_Weapon,
                eStatusCategory.ItemOwned_Defence
            });

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Change_Random_Option, "", itemRecord.MainType.ToString(), 1);

            CNetManager.Instance.P2C_ReportAssetData(_sessionKey, m_Owner.AssetAgent.GetList());
            CDBManager.Instance.QueryCharacterUpdateItemRandomOption(m_Owner.DBGUID, _sessionKey, m_Owner.UserData.m_UID, reqItem, dbtran, Packet_C2P.Protocol.P2C_ResultRandomOptionChange);

            return Packet_Result.Result.Success;
        }

        public List<int> GatherChivalryGachaExceptList()
        {
            List<int> retList = new List<int>();
            var records = ItemTable.Instance.Gaher(CDefine.EItemMainType.Chivalry);

            foreach (var record in records)
            {
                var hasData = FindItemToTableBegin(record.ID.ToString());
                if (hasData == null)
                    continue;

                var enchantRecord = ItemChivalryEnchantTable.Instance.Find(record.EnchantID_Chivalry);
                if (enchantRecord == null)
                    continue;

                int max_lv = ItemChivalryEnchantTable.Instance.GetMaxLevel(record.EnchantID_Chivalry);
                if (max_lv < 1)
                    continue;

                if (hasData.Level >= max_lv)
                    retList.Add(record.ID);
            }

            return retList;
        }


        public Packet_Result.Result ReqItemRandomOptionLock(long _sessionKey, long _reqID, int idx)
        {
            //valid
            if (idx < 0)
                return Packet_Result.Result.PacketError;

            var reqitem = Find(_reqID);
            if (reqitem == null)
                return Packet_Result.Result.PacketError;

            var itemRecord = ItemTable.Instance.Find(reqitem.TableID);
            if (itemRecord == null)
                return Packet_Result.Result.PacketError;

            if (reqitem.RandomOption.Count < 1)
                return Packet_Result.Result.PacketError;

            //if (reqitem.RandomOption.Count <= idx)
            //    return Packet_Result.Result.PacketError;

            var randOption = reqitem.RandomOption.Find(x => x.no == idx);
            if (randOption == null)
                return Packet_Result.Result.PacketError;

            //process
            randOption.locked = !randOption.locked;

            CDBManager.Instance.QueryCharacterUpdateItemRandomOption(m_Owner.DBGUID, _sessionKey, m_Owner.UserData.m_UID, reqitem, new CDBMerge(), Packet_C2P.Protocol.P2C_ResultRandomOptionLock);
            return Packet_Result.Result.Success;

            //log
        }

        public Packet_Result.Result ReqItemConsume(long _sessionKey, long _reqID, int _cnt)
        {
            //valid
            if (_cnt < 1)
                return Packet_Result.Result.PacketError;

            //todo : cunsume max count
            //int consumeMaxCount = CDefineTable.Instance.Value<int>("Item_Consume_Max_Count");
            //if (consumeMaxCount < _cnt)
            //    return Packet_Result.Result.InValidData;

            var item = Find(_reqID);
            if (item == null)
            {
                CLogger.Instance.Debug($"item == null, _reqID : {_reqID}");
                return Packet_Result.Result.NotFoundData;
            }

            var itemRecord = ItemTable.Instance.Find(item.TableID);
            if (itemRecord == null)
            {
                CLogger.Instance.Debug($"itemRecord == null, item.TableID : {item.TableID}");
                return Packet_Result.Result.InValidRecord;
            }

            if (itemRecord.MainType != CDefine.EItemMainType.Consumables)
            {
                CLogger.Instance.Debug($"itemRecord.MainType != CDefine.EItemMainType.Consumables, itemRecord.MainType: {itemRecord.MainType}");
                return Packet_Result.Result.InValidData;
            }

            if (!itemRecord.IsUse)
            {
                CLogger.Instance.Debug($"!itemRecord.IsUse, itemRecord.IsUse : {itemRecord.IsUse}");
                return Packet_Result.Result.InValidData;
            }

            if (!HasEnoughItem(item.TableID, _cnt))
            {
                CLogger.Instance.Debug($"not EnoughItem, _cnt : {_cnt}");
                return Packet_Result.Result.LackAssetError;
            }

            //process
            CDBMerge dbtran = new CDBMerge();
            CRewardInfo forClient = new CRewardInfo();
            bool bSend = true;

            var rewards = DropTable.Instance.Roulette(itemRecord.GachaItemID, _cnt);
            if (rewards == null)
            {
                CLogger.Instance.Debug($"rewards == null, itemRecord.GachaItemID, _cnt : {itemRecord.GachaItemID}, {_cnt}");
                return Packet_Result.Result.PacketError;
            }

            forClient.Insert(rewards);
            UpdateItemCount(item.ItemID, -_cnt, ref dbtran, false);
            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
            dbtran.Merge();

            CNetManager.Instance.P2C_ReportAssetData(_sessionKey, m_Owner.AssetAgent.GetList());
            CNetManager.Instance.P2C_ReportRewardData(_sessionKey, forClient.GetList());

            //db
            CDBManager.Instance.QueryCharacterUpdateItemConsume(m_Owner.DBGUID, _sessionKey, m_Owner.UID, item, dbtran, forClient);

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Use_Item, "", itemRecord.MainType.ToString(), _cnt);
            
            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_consume, m_Owner.UserData, LogHelper.ToJson(item), dbtran, _cnt);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqItemGahca(long sessionKey, int gachaID, int cnt)
        {
            //vaild
            if (cnt < 0)
            {
                CLogger.Instance.Debug($"");
                return Packet_Result.Result.InValidData;
            }

            var gachaRecord = GachaTable.Instance.Find(gachaID);
            if (gachaRecord == null)
            {
                CLogger.Instance.Debug($"gachaRecord == null, _gachaID : {gachaID}");
                return Packet_Result.Result.InValidRecord;
            }

            int gachaCount = cnt == 0 ? 1 : cnt * 10;
            if (gachaCount > gachaRecord.GachaMaxCount)
            {
                CLogger.Instance.Debug($"gachaCount > gachaRecord.GachaMaxCount , {gachaCount}");
                return Packet_Result.Result.PacketError;
            }

            switch (gachaRecord.MainType)
            {
                case CDefine.EItemMainType.Weapon:
                case CDefine.EItemMainType.Defence:
                    return ReqItemGachaBase(sessionKey, gachaRecord, gachaCount);
                case CDefine.EItemMainType.Chivalry:
                    return ReqItemGachaChivalry(sessionKey, gachaRecord, gachaCount);
                default:
                    break;
            }

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ReqItemGachaChivalry(long sessionKey, GachaRecord gachaRecord, int cnt)
        {
            var gachaData = m_Owner.FindGacha(gachaRecord.LevelGroupID);
            if (gachaData == null)
            {
                CLogger.Instance.Debug($"gachaData == null, gachaRecord.LevelGroupID : {gachaRecord.LevelGroupID}");
                return Packet_Result.Result.InValidData;
            }

            int needValue = cnt * gachaRecord.Cost_Once;
            if (!m_Owner.AssetAgent.HasEnoughAsset(gachaRecord.AssetType, needValue))
            {
                CLogger.Instance.Debug($"gachaRecord.AssetType LackAsset, : {needValue}");
                return Packet_Result.Result.LackAssetError;
            }

            var gachaLvRecord = GachaLvTable.Instance.Find(gachaData.m_GroupID, gachaData.m_Level);
            if (gachaLvRecord == null)
            {
                CLogger.Instance.Debug($"gachaLvRecord == null, {gachaData.m_GroupID}:{gachaData.m_Level}");
                return Packet_Result.Result.InValidRecord;
            }

            //int total_cnt
            int bonus_cnt = (gachaLvRecord.GachaBonus_CountNum == 0) ? 0 : cnt / gachaLvRecord.GachaBonus_CountNum;
            bonus_cnt = bonus_cnt * gachaLvRecord.GachaBonus_Num;
            cnt += bonus_cnt;

            gachaData.m_Exp += cnt;
            m_Owner.UpdateGachaLevel(gachaData.m_GroupID);

            List<int> exceptList = GatherChivalryGachaExceptList();
            List<_AssetData> gacharewards = GachaProbTable.Instance.ChivalryRoullet(gachaData.m_GroupID, gachaData.m_Level, cnt, exceptList);
            if (gacharewards == null)
            {
                CLogger.Instance.Debug($"gachaReward == null");
                return Packet_Result.Result.PacketError;
            }

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            bool bSend = true;
            _AssetData cost = new _AssetData(CDefine.AssetType.Coin, gachaRecord.AssetType, -needValue);
            m_Owner.UpdateAssetData(cost, ref dbtran, bSend);

            forClient.Insert(gacharewards);
            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
            dbtran.Merge();

            if (gachaLvRecord.Merge)
                forClient.Merge();

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Gacha_Item, "", gachaData.m_GroupID.ToString(), gachaData.m_Exp);
            //

            CDBManager.Instance.QueryCharacterUpdateGacha(m_Owner.DBGUID, sessionKey, m_Owner.UID, gachaData, forClient, dbtran, gachaRecord.ID);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_gacha, m_Owner.UserData, LogHelper.ToJson(gachaData), dbtran, cnt);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ReqItemGachaBase(long sessionKey, GachaRecord gachaRecord, int cnt)
        {
            var gachaData = m_Owner.FindGacha(gachaRecord.LevelGroupID);
            if (gachaData == null)
            {
                CLogger.Instance.Debug($"gachaData == null, gachaRecord.LevelGroupID : {gachaRecord.LevelGroupID}");
                return Packet_Result.Result.InValidData;
            }

            int needValue = cnt * gachaRecord.Cost_Once;
            if (!m_Owner.AssetAgent.HasEnoughAsset(gachaRecord.AssetType, needValue))
            {
                CLogger.Instance.Debug($"gachaRecord.AssetType LackAsset, : {needValue}");
                return Packet_Result.Result.LackAssetError;
            }

            var gachaLvRecord = GachaLvTable.Instance.Find(gachaData.m_GroupID, gachaData.m_Level);
            if (gachaLvRecord == null)
            {
                CLogger.Instance.Debug($"gachaLvRecord == null, {gachaData.m_GroupID}:{gachaData.m_Level}");
                return Packet_Result.Result.InValidRecord;
            }

            //total_cnt
            int bonus_cnt = (gachaLvRecord.GachaBonus_CountNum == 0) ? 0 : cnt / gachaLvRecord.GachaBonus_CountNum;
            bonus_cnt = bonus_cnt * gachaLvRecord.GachaBonus_Num;
            cnt += bonus_cnt;

            List<_AssetData> gacharewards = GachaProbTable.Instance.Roulette(gachaData.m_GroupID, gachaData.m_Level, cnt);
            if (gacharewards == null)
            {
                CLogger.Instance.Debug($"gachaReward == null");
                return Packet_Result.Result.PacketError;
            }

            gachaData.m_Exp += cnt;
            m_Owner.UpdateGachaLevel(gachaData.m_GroupID);

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            bool bSend = true;
            _AssetData cost = new _AssetData(CDefine.AssetType.Coin, gachaRecord.AssetType, -needValue);
            m_Owner.UpdateAssetData(cost, ref dbtran, bSend);

            forClient.Insert(gacharewards);
            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
            dbtran.Merge();

            if (gachaLvRecord.Merge || cnt >= DefineTable.Instance.Value<int>("GachaMergeCount"))
                forClient.Merge();

            //mission
            m_Owner.QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Gacha_Item, "", gachaData.m_GroupID.ToString(), gachaData.m_Exp);
            
            CDBManager.Instance.QueryCharacterUpdateGacha(m_Owner.DBGUID, sessionKey, m_Owner.UID, gachaData, forClient, dbtran, gachaRecord.ID);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_gacha, m_Owner.UserData, LogHelper.ToJson(gachaData), dbtran, cnt);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqItemGachaLevelReward(long _sessionKey, int _gachaid, int _rewardlv)
        {
            var gachaRecord = GachaTable.Instance.Find(_gachaid);
            if (gachaRecord == null)
            {
                CLogger.Instance.Debug($"gachaRecord == null, gachaRecord == null: {_gachaid}");
                return Packet_Result.Result.InValidRecord;
            }

            var gachaData = m_Owner.FindGacha(gachaRecord.LevelGroupID);
            if (gachaData == null)
                return Packet_Result.Result.NotFoundData;

            if (gachaData.m_Level < _rewardlv)
                return Packet_Result.Result.PacketError;

            if (gachaData.m_Rewarded >= _rewardlv)
                return Packet_Result.Result.PacketError;

            if (gachaData.m_Rewarded + 1 < _rewardlv)
            {
                return Packet_Result.Result.PacketError;
            }

            GachaLvRecord lvRecord = GachaLvTable.Instance.Find(gachaData.m_GroupID, _rewardlv);
            if (lvRecord == null)
                return Packet_Result.Result.InValidRecord;

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            var rewards = RewardTable.Instance.Find(lvRecord.RewardID);
            if (rewards == null)
                return Packet_Result.Result.InValidData;

            foreach (var iter in rewards)
            {
                var reward = iter.Value;
                forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
            }

            bool bSend = true;
            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
            dbtran.Merge();

            gachaData.m_Rewarded = _rewardlv;
            CDBManager.Instance.QueryCharacterUpdateGachaLevelReward(m_Owner.DBGUID, _sessionKey, m_Owner.UID, gachaData, forClient, dbtran);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_gacha_reward, m_Owner.UserData, LogHelper.ToJson(gachaData), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public void AfterQueryUpdateItemGacha(long _sessionKey, _GachaData gachaData, CRewardInfo forClient, int gachaTID, Packet_Result.Result result)
        {
            //CNetManager.Instance.P2C_ReportRewardData(_sessionKey, forClient.GetList());
            CNetManager.Instance.P2C_ReportAssetData(_sessionKey, m_Owner.AssetAgent.GetList());
            CNetManager.Instance.P2C_ResultItemGacha(_sessionKey, gachaData, forClient.GetList(), gachaTID, result);
        }

        public void AfterQueryUpdateItemSell(long _sessionKey, _ItemData item, CRewardInfo forClient, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ResultItemSell(_sessionKey, item, forClient.GetList(), result);
        }

        public Packet_Result.Result ReqItemSell(long _sessionKey, long _reqID, int _cnt)
        {
            if (_cnt < 0)
            {
                return Packet_Result.Result.InValidData;
            }

            var hasData = Find(_reqID);
            if (hasData == null)
            {
                CLogger.Instance.Debug($"hasData == null, _reqID : {_reqID}");
                return Packet_Result.Result.NotFoundData;
            }

            var hasRecord = ItemTable.Instance.Find(hasData.TableID);
            if (hasRecord == null)
            {
                CLogger.Instance.Debug($"hasRecord == null, hasData.TableID : {hasData.TableID}");
                return Packet_Result.Result.InValidRecord;
            }

            if (hasRecord.Price_AssetValue < 1)
            {
                CLogger.Instance.Debug($"hasRecord.Price_AssetValue < 1, hasRecord.Price_AssetValue : {hasRecord.Price_AssetValue}");
                return Packet_Result.Result.InValidRecord;
            }

            if (!HasEnoughItem(hasData.TableID, _cnt))
            {
                CLogger.Instance.Debug($"not enoughItem, _cnt : {_cnt}");
                return Packet_Result.Result.LackAssetError;
            }

            int max_lv = ItemChivalryEnchantTable.Instance.GetMaxLevel(hasRecord.EnchantID_Chivalry);
            if (max_lv < 1)
            {
                CLogger.Instance.Debug($"max_lv < 1, max_lv : {max_lv}");
                return Packet_Result.Result.InValidRecord;
            }

            if (hasData.Level < max_lv)
            {
                CLogger.Instance.Debug($"hasData.Level < max_lv, hasData.Level : {hasData.Level}");
                return Packet_Result.Result.InValidData;
            }

            long rewardPrice = _cnt * hasRecord.Price_AssetValue;

            bool bSend = true;
            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();
            forClient.Insert(CDefine.AssetType.Coin, hasRecord.Price_AssetType, rewardPrice);
            UpdateItemCount(hasData.ItemID, -_cnt, ref dbtran, bSend);
            forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);

            forClient.Merge();
            dbtran.Merge();

            CDBManager.Instance.QueryCharacterUpdateItemSell(m_Owner.DBGUID, _sessionKey, m_Owner.UID, hasData, forClient, dbtran);

            //todo : log
            var log = LogHelper.MakeLogBson(eLogType.item_sell, m_Owner.UserData, LogHelper.ToJson(hasData), dbtran, _cnt);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }
    

        public void SaveAtLogout()
        {
            //preset
            foreach (var iter in m_ItemEquipPresets)
            {
                var itVal = iter.Value;
                CDBManager.Instance.QueryCharacterUpdateEquipPreset(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, itVal);
            }

            foreach(var iter in m_KnightEquipPresets)
            {
                var itVal = iter.Value;
                CDBManager.Instance.QueryCharacterUpdateEquipKnightPreset(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, itVal);
            }
        }
    }
}
