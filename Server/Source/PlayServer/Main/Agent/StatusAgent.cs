using System;
using System.Numerics;
using System.Collections.Generic;
using SCommon;
using Global;
using System.Diagnostics;

namespace PlayServer
{
    public class CStatusAgent
    {
        private int Precision { get; } = 10000;
        private CUser m_Owner;
        private Dictionary<eStatusCategory, Dictionary<CDefine.EAbility, _AbilData>> m_Abils = new Dictionary<eStatusCategory, Dictionary<CDefine.EAbility, _AbilData>>();

        public CStatusAgent(CUser owner)
        {
            m_Owner = owner;
        }

        private void UpsertAbils(eStatusCategory cat, Dictionary<CDefine.EAbility, _AbilData> data)
        {
            m_Abils[cat] = data;
        }
        private void UpsertAbil(eStatusCategory cat, _AbilData data)
        {
            var hasAbils = FindAbils(cat);
            if (hasAbils == null) return;

            hasAbils[data.type] = data;
        }
        public Dictionary<CDefine.EAbility, _AbilData> FindAbils(eStatusCategory cat)
        {
            if (m_Abils.TryGetValue(cat, out Dictionary<CDefine.EAbility, _AbilData> retval))
                return retval;
            return null;
        }
        public _AbilData GetAbil(CDefine.EAbility type)
        {
            var hasAbil = FindAbil(eStatusCategory.Total, type);
            if (hasAbil == null)
                return new _AbilData(type, 0);

            return hasAbil;
        }

        public List<_AbilData> GetAbilList()
        {
            var retList = new List<_AbilData>();
            for(CDefine.EAbility i = CDefine.EAbility.Damage; i < CDefine.EAbility.Max; ++i)
            {
                var gather = GetAbil(i);
                if (gather == null)
                    continue;

                retList.Add(SCopy<_AbilData>.DeepCopy(gather));
            }

            return retList;
        }

        public _AbilData FindAbil(eStatusCategory cat, CDefine.EAbility type)
        {
            var hasAbils = FindAbils(cat);
            if (hasAbils != null)
            {
                if (hasAbils.TryGetValue(type, out _AbilData retval))
                    return retval;
            }
            return null;
        }

        public void RefreshAll(bool bSend = true)
        {
            List<eStatusCategory> cats = new List<eStatusCategory>();
            for (eStatusCategory i = eStatusCategory.beg; i <= eStatusCategory.end; ++i)
                cats.Add(i);

            Refresh(cats, bSend);
        }

        //public void Refresh(CItemRecord itemRecord)
        //{
        //    if (itemRecord == null) return;

        //    eStatusCategory refreshEquippedCat = CStatusAgent.GetEquippedCatFrom(itemRecord);
        //    eStatusCategory refreshOwnedCat = CStatusAgent.GetOwnedCatFrom(itemRecord);

        //    if (refreshEquippedCat == eStatusCategory.Max && refreshOwnedCat == eStatusCategory.Max)
        //        return;

        //    Refresh(new List<eStatusCategory> { refreshOwnedCat, refreshEquippedCat });
        //}

        public void Refresh(List<eStatusCategory> cats, bool bSend = true)
        {
            if (cats == null) return;

            foreach (var cat in cats)
            {
                switch (cat)
                {
                    //base
                    case eStatusCategory.Base: RefreshBase(); break;

                    //growth level
                    case eStatusCategory.StatusGold: RefreshStatusGold(); break;
                    case eStatusCategory.StatusLv: RefreshStatusLv(); break;

                    //item equip
                    case eStatusCategory.ItemEquipped_Base: RefreshItemEquipped_Base(); break;
                    case eStatusCategory.ItemEquipped_Knight: RefreshItemEquipped_Knight(); break;

                    //item owned
                    case eStatusCategory.ItemOwned_Knight: RefreshItemOwned_Knight(); break;
                    case eStatusCategory.ItemOwned_Weapon: RefreshItemOwned_Weapon(); break;
                    case eStatusCategory.ItemOwned_Defence: RefreshItemOwned_Defence(); break;
                    case eStatusCategory.ItemOwned_Chivalry: RefreshItemOwned_Chivalry(); break;
                    case eStatusCategory.ItemOwned_Custume: RefreshItemOwned_Custume(); break;

                    //relic
                    case eStatusCategory.Relic: RefreshRelic(); break;

                    //Buff
                    case eStatusCategory.Buff: RefreshBuffAbli(); break;

                }
            }

            RefreshTotal(bSend);
        }

        private void RefreshBase()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            if (DefaultPlayerTable.Instance.GetDefaultRecord() is DefaultPlayerRecord playerRecord)
            {
                foreach (var it in playerRecord.DefaultAbils)
                    CStatusAgent.UpsertAbil(ref gatherAbils, it.Value);
            }

            UpsertAbils(eStatusCategory.Base, gatherAbils);
        }

        private void RefreshStatusGold()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.LevelAgent.GetAbilGoldStatus(ref gatherAbils);

            UpsertAbils(eStatusCategory.StatusGold, gatherAbils);
        }
        private void RefreshStatusLv()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.LevelAgent.GetAbilLvStatus(ref gatherAbils);

            UpsertAbils(eStatusCategory.StatusLv, gatherAbils);
        }

        private void RefreshItemEquipped_Base()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.ItemAgent.GetAbilEquipped_Base(ref gatherAbils);

            UpsertAbils(eStatusCategory.ItemEquipped_Base, gatherAbils);
        }

        private void RefreshItemEquipped_Knight()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.ItemAgent.GetAbilEquipped_Knight(ref gatherAbils);

            UpsertAbils(eStatusCategory.ItemEquipped_Knight, gatherAbils);
        }

        private void RefreshItemOwned_Knight()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.ItemAgent.GetAbilOwned_Knight(ref gatherAbils);

            UpsertAbils(eStatusCategory.ItemOwned_Knight, gatherAbils);
        }

        private void RefreshItemOwned_Weapon()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.ItemAgent.GetAbilOwned_Weapon(ref gatherAbils);

            UpsertAbils(eStatusCategory.ItemOwned_Weapon, gatherAbils);
        }

        private void RefreshItemOwned_Defence()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.ItemAgent.GetAbilOwned_Defence(ref gatherAbils);

            UpsertAbils(eStatusCategory.ItemOwned_Defence, gatherAbils);
        }

        private void RefreshItemOwned_Chivalry()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.ItemAgent.GetAbilOwned_Chivalry(ref gatherAbils);

            UpsertAbils(eStatusCategory.ItemOwned_Defence, gatherAbils);
        }

        private void RefreshItemOwned_Custume()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.ItemAgent.GetAbilOwned_Custume(ref gatherAbils);

            UpsertAbils(eStatusCategory.ItemOwned_Defence, gatherAbils);
        }

        private void RefreshRelic()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.RelicAgent.GetAbilRelic(ref gatherAbils);

            UpsertAbils(eStatusCategory.Relic, gatherAbils);
        }

        private void RefreshBuffAbli()
        {
            var gatherAbils = new Dictionary<CDefine.EAbility, _AbilData>();

            m_Owner.BuffAgent.GetAbilBuff(ref gatherAbils);

            UpsertAbils(eStatusCategory.Buff, gatherAbils);
        }

        //private void RefreshItemMastery()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilMastery(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemMastery, gatherAbils);
        //}


        //private void RefreshItemEquipped_Pet()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilEquipped_Pet(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilSetEffect_Pet(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemEquipped_Pet, gatherAbils);
        //}
        //private void RefreshItemEquipped_Acc()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    for (eEquipPreset i = eEquipPreset.beg_acc; i <= eEquipPreset.end_acc; ++i)
        //        m_Owner.ItemAgent.GetAbilEquipped_Acc(i, ref gatherAbils);

        //    m_Owner.ItemAgent.GetAbilSetEffect_Acc(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemEquipped_Acc, gatherAbils);
        //}
        //private void RefreshItemEquipped_Talisman()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilEuipped_TalismanBonus(ref gatherAbils);

        //    for (eEquipPreset i = eEquipPreset.beg_talisman; i <= eEquipPreset.end_talisman; ++i)
        //        m_Owner.ItemAgent.GetAbilEquipped_Talisman(i, ref gatherAbils);

        //    m_Owner.ItemAgent.GetAbilSetEffect_Talisman(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemEquipped_Talisman, gatherAbils);
        //}
        //private void RefreshItemEquipped_Figure()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    for (eEquipPreset i = eEquipPreset.beg_figure; i <= eEquipPreset.end_figure; ++i)
        //        m_Owner.ItemAgent.GetAbilEquipped_Figure(i, ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemEquipped_Figure, gatherAbils);
        //}

        //private void RefreshItemOwned()
        //{
        //    //[todo:preset] partially
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilOwned_Base(eItemMainType.Costume, eItemDetailType.Max, ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Skin(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Pet(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Acc(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Seal(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Title(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Treasure(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_BaseItemGrade(ref gatherAbils);

        //    m_Owner.ItemAgent.GetAbilOwned_Ego(eItemDetailType.Ego_Attack, ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Ego(eItemDetailType.Ego_Defence, ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemOwned, gatherAbils);
        //}

        //private void RefreshItemOwned_Base()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilOwned_Base(eItemMainType.Costume, eItemDetailType.Max, ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_BaseItemGrade(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Skin(ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Skin2(ref gatherAbils);

        //    m_Owner.ItemAgent.GetAbilOwned_Ego(eItemDetailType.Ego_Attack, ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Ego(eItemDetailType.Ego_Defence, ref gatherAbils);
        //    m_Owner.ItemAgent.GetAbilOwned_Ego(eItemDetailType.Ego_Soul, ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemOwned_Base, gatherAbils);
        //}

        //private void RefreshItemOwned_Pet()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilOwned_Pet(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemOwned_Pet, gatherAbils);
        //}
        //private void RefreshItemOwned_Acc()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilOwned_Acc(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemOwned_Acc, gatherAbils);
        //}
        //private void RefreshItemOwned_Seal()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilOwned_Seal(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemOwned_Seal, gatherAbils);
        //}

        //private void RefreshItemOwned_Title()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilOwned_Title(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemOwned_Title, gatherAbils);
        //}
        //private void RefreshItemOwned_Treasure()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilOwned_Treasure(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemOwned_Treasure, gatherAbils);
        //}
        //private void RefreshItemOwned_Talisman()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();
        //    m_Owner.ItemAgent.GetAbilOwned_Talisman(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.ItemOwned_Talisman, gatherAbils);
        //}

        //private void RefreshBuff()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.BuffAgent.GetAbils(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.Buff, gatherAbils);
        //}
        //private void RefreshSkill()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.SkillAgent.GetAbils(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.Skill, gatherAbils);
        //}

        //private void RefreshRuneGrade()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.RuneAgent.GetRuneGradeAbils(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.RuneGrade, gatherAbils);
        //}

        //private void RefreshFeature()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.FeatureAgent.GetFeatureAbils(ref gatherAbils);

        //    UpsertAbils(eStatusCategory.Feature, gatherAbils);
        //}

        //private void RefreshResonance()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();
        //    m_Owner.ResonanceAgent.GetResonanceAbils(ref gatherAbils);
        //    UpsertAbils(eStatusCategory.Resonance, gatherAbils);
        //}

        //private void RefreshItemEquipped_Rune()
        //{
        //    var gatherAbils = new Dictionary<CDefine.Ability, CAbilData>();

        //    m_Owner.ItemAgent.GetAbilEquipped_Rune(ref gatherAbils);
        //    UpsertAbils(eStatusCategory.ItemEquipped_Rune, gatherAbils);
        //}

        private void RefreshTotal(bool bSend)
        {
            var gatherAbils = AbilityTable.Instance.CopyDefault();
            for (eStatusCategory i = eStatusCategory.beg; i <= eStatusCategory.end; ++i)
            {
                var hasAbils = FindAbils(i);
                if (hasAbils != null)
                    CStatusAgent.UpsertAbil(ref gatherAbils, hasAbils);
            }

            
            UpsertAbils(eStatusCategory.Total, gatherAbils);

            BigInteger finallyHP = FinallyHP();
            BigInteger finallyDef = FinallyDef();
            BigInteger finallyDmg = FinallyDmg();
            BigInteger finallyDmgCri1 = (finallyDmg * (BigInteger)(GetAbil(CDefine.EAbility.Critical_Damage_1).val * Precision)) / Precision;
            BigInteger finallyDmgCri2 = (finallyDmgCri1 * (BigInteger)(GetAbil(CDefine.EAbility.Critical_Damage_2).val * Precision)) / Precision;

            UpsertAbil(eStatusCategory.Total, SplitBiginteger(CDefine.EAbility.Calc_Hp, finallyHP));
            UpsertAbil(eStatusCategory.Total, SplitBiginteger(CDefine.EAbility.Calc_Def, finallyDef));
            UpsertAbil(eStatusCategory.Total, SplitBiginteger(CDefine.EAbility.Calc_Damage, finallyDmg));
            UpsertAbil(eStatusCategory.Total, SplitBiginteger(CDefine.EAbility.Calc_Critical1_Damage, finallyDmgCri1));
            UpsertAbil(eStatusCategory.Total, SplitBiginteger(CDefine.EAbility.Calc_Critical2_Damage, finallyDmgCri2));
            
            CLogger.Instance.User($"============Abil Report log==========");

            foreach (var it in gatherAbils.Values)
                CLogger.Instance.User($"[{it.type}] : [{it.val}]");

            CLogger.Instance.User($"====================================");


            if (bSend)
                CNetManager.Instance.P2C_ReportAbilData(m_Owner.SessionKey, m_Owner.UserData.m_UID, gatherAbils);
        }

        //private void CheckAbilMax(ref Dictionary<CDefine.Ability, CAbilData> data)
        //{
        //    foreach (var it in CAbilityTable.Instance.MaxEntries)
        //    {
        //        if (data.TryGetValue(it.Key, out CAbilData abil))
        //            abil.val = Math.Min(it.Value, abil.val);
        //    }
        //}

        private BigInteger FinallyDmg()
        {

            BigInteger retval
                = (BigInteger)
                (GetAbil(CDefine.EAbility.Damage).val
                * GetAbil(CDefine.EAbility.Damage_Rate).val
                * GetAbil(CDefine.EAbility.Option_Attack_Rate).val);


            return retval;
        }

        private BigInteger FinallyHP()
        {
            BigInteger retval
                = (BigInteger)
                (GetAbil(CDefine.EAbility.MaxHP).val
                * GetAbil(CDefine.EAbility.HP_Rate).val);

            return retval;
        }
        private BigInteger FinallyDef()
        {
            BigInteger retval
                = (BigInteger)
                (GetAbil(CDefine.EAbility.Defence).val
                * GetAbil(CDefine.EAbility.Def_Rate).val);

            return retval;
        }

        public BigInteger ToBig(CDefine.EAbility type)
        {
            var abil = GetAbil(type);
            return ((BigInteger)abil.val * (BigInteger)(System.Math.Pow(10, abil.exponentOf10) * Precision)) / Precision;
        }

        private _AbilData SplitBiginteger(CDefine.EAbility type, BigInteger bigVal)
        {
            _AbilData retval = new _AbilData();
            retval.type = type;

            long maxVal = long.MaxValue / 100;
            if (-maxVal < bigVal && bigVal < maxVal)
            {
                if (long.TryParse(bigVal.ToString(), out long cvtVal))
                    retval.val = cvtVal;
                else
                    retval.val = (long)bigVal;

                retval.exponentOf10 = 0;
            }
            else
            {
                double baseIndices = BigInteger.Log10(maxVal);
                double bigAbilIndices = BigInteger.Log10(bigVal);

                int indicies = (int)(bigAbilIndices - baseIndices);

                double pow = Math.Pow(10, indicies);

                var divisor = (BigInteger)pow;
                if (divisor == 0) divisor = 1;

                BigInteger dividedVal = BigInteger.Divide(bigVal, divisor);
                if (dividedVal >= maxVal)
                {
                    //pow *= 10; //?
                    indicies += 1;
                    dividedVal /= 10;
                }

                retval.val = (double)dividedVal;
                retval.exponentOf10 = indicies;
            }

            return retval;
        }

        static public void UpsertAbil(ref Dictionary<CDefine.EAbility, _AbilData> rAbils, _AbilData abil)
        {
            _AbilData foundAbil;
            if (!rAbils.TryGetValue(abil.type, out foundAbil))
            {
                rAbils.Add(abil.type, new _AbilData(abil.type, 0));
                foundAbil = rAbils[abil.type];
            }

            foundAbil.val += abil.val;
        }
        static public void UpsertAbil(ref Dictionary<CDefine.EAbility, _AbilData> rAbils, List<_AbilData> abils)
        {
            foreach (var abil in abils)
                UpsertAbil(ref rAbils, abil);
        }
        static public void UpsertAbil(ref Dictionary<CDefine.EAbility, _AbilData> rAbils, Dictionary<CDefine.EAbility, _AbilData> abils)
        {
            foreach (var it in abils)
                UpsertAbil(ref rAbils, it.Value);
        }

        //static public eStatusCategory GetEquippedCatFrom(CItemRecord itemRecord)
        //{
        //    if (itemRecord == null) return eStatusCategory.Max;

        //    switch (itemRecord.m_MainType)
        //    {
        //        case eItemMainType.Pet: return eStatusCategory.ItemEquipped_Pet;
        //        case eItemMainType.Accessory: return eStatusCategory.ItemEquipped_Acc;
        //        case eItemMainType.Figure: return eStatusCategory.ItemEquipped_Figure;
        //        case eItemMainType.Rune: return eStatusCategory.ItemEquipped_Rune;
        //        case eItemMainType.Talisman: return eStatusCategory.ItemEquipped_Talisman;
        //    }

        //    return eStatusCategory.ItemEquipped_Base;
        //}
        //static public eStatusCategory GetOwnedCatFrom(CItemRecord itemRecord)
        //{
        //    if (itemRecord == null) return eStatusCategory.Max;

        //    switch (itemRecord.m_MainType)
        //    {
        //        case eItemMainType.Pet: return eStatusCategory.ItemOwned_Pet;
        //        case eItemMainType.Accessory: return eStatusCategory.ItemOwned_Acc;
        //        case eItemMainType.Seal: return eStatusCategory.ItemOwned_Seal;
        //        case eItemMainType.Title: return eStatusCategory.ItemOwned_Title;
        //        case eItemMainType.Treasure: return eStatusCategory.ItemOwned_Treasure;
        //        case eItemMainType.Talisman: return eStatusCategory.ItemOwned_Talisman;
        //    }

        //    return eStatusCategory.ItemOwned_Base;
        //}
        //static public eStatusCategory GetEquippedCatFrom(eEquipPreset presetType)
        //{
        //    switch (presetType)
        //    {
        //        case eEquipPreset.Base: return eStatusCategory.ItemEquipped_Base;

        //        case eEquipPreset.Pet: return eStatusCategory.ItemEquipped_Pet;

        //        case eEquipPreset.Ring:
        //        case eEquipPreset.Earring:
        //        case eEquipPreset.Hairpin:
        //            return eStatusCategory.ItemEquipped_Acc;

        //        case eEquipPreset.FigureMonster:
        //        case eEquipPreset.FigureMaster:
        //        case eEquipPreset.FigureCostume:
        //            return eStatusCategory.ItemEquipped_Figure;
        //        case eEquipPreset.Rune:
        //            return eStatusCategory.ItemEquipped_Rune;
        //        case eEquipPreset.TalismanLove:
        //        case eEquipPreset.TalismanHope:
        //        case eEquipPreset.TalismanLuck:
        //            return eStatusCategory.ItemEquipped_Talisman;
        //    }

        //    return eStatusCategory.Max;
        //}
        //public void Cheat_SuperPower(bool bEnable)
        //{
        //    if (!bEnable)
        //    {
        //        m_Abils.Remove(eStatusCategory.Cheat);
        //    }
        //    else
        //    {
        //        if (FindAbils(eStatusCategory.Cheat) != null)
        //            return;

        //        var defaults = CAbilityTable.Instance.CopyDefaults();
        //        foreach (var it in defaults)
        //        {
        //            var abil = it.Value;

        //            switch (abil.type)
        //            {
        //                case CDefine.Ability.Stage_Gold_Up:
        //                case CDefine.Ability.Stage_Exp_Up:
        //                case CDefine.Ability.Stage_Drop_Up:
        //                case CDefine.Ability.Stage_SkillBook_Up:
        //                case CDefine.Ability.Stage_AccDrop_Up:
        //                case CDefine.Ability.Stage_SealDrop_Up:
        //                case CDefine.Ability.Stage_Fame_Up:
        //                case CDefine.Ability.Stage_Enchant_Up:
        //                case CDefine.Ability.Stage_Gold_Up_Final:
        //                case CDefine.Ability.Stage_Exp_Up_Final:
        //                case CDefine.Ability.Moonstone_Drop_Up:
        //                case CDefine.Ability.Sweep_Enchant_Up:
        //                case CDefine.Ability.Sweep_PetValue_Up:
        //                case CDefine.Ability.Sweep_Pattern_Up:
        //                case CDefine.Ability.Sweep_Emblem_Up:
        //                case CDefine.Ability.Sweep_Option_Up:
        //                case CDefine.Ability.Event_GOH_Up:
        //                case CDefine.Ability.Stage_RuneDrop_Up:
        //                case CDefine.Ability.PowerStone_Drop_Up:
        //                case CDefine.Ability.Stage_AccDrop_Up_Final:
        //                case CDefine.Ability.Stage_Drop_Up_Final:
        //                case CDefine.Ability.Stage_Fame_Up_Final:
        //                case CDefine.Ability.Stage_SealDrop_Up_Final:
        //                case CDefine.Ability.Moonstone_Drop_Up_Final:
        //                case CDefine.Ability.Stage_SkillBook_Up_Final:
        //                    break;
        //                default:
        //                    abil.val = 100000000;
        //                    break;
        //            }
        //        }

        //        UpsertAbils(eStatusCategory.Cheat, defaults);
        //    }

        //    RefreshAll();
        //}

        //public void Cheat_SwapAbil(CAbilData abil)
        //{
        //    //[todo:preset]
        //    var hasCheatAbils = FindAbils(eStatusCategory.Cheat);
        //    if (hasCheatAbils == null)
        //    {
        //        UpsertAbils(eStatusCategory.Cheat, new Dictionary<CDefine.Ability, CAbilData>());
        //        hasCheatAbils = FindAbils(eStatusCategory.Cheat);
        //    }

        //    hasCheatAbils[abil.type] = abil;

        //    RefreshAll();
        //}
    }
}
