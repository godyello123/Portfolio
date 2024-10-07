using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Data;
using System.Windows.Forms;
using Global;
using Google.Apis.AndroidPublisher.v3.Data;
using Google.Protobuf.WellKnownTypes;
using SCommon;


namespace PlayServer
{
    public class CUser : CActor
    {
        private long m_SessionKey;
        private string m_DeviceID = string.Empty;
        private long m_TodayExpTime = -1;
        private string m_RemoteIP = string.Empty;
        private eUserState m_UserState = eUserState.TryLogin;

        private STimer m_Timer = new STimer(1000);
        private _UserData m_UserData;
        private CStageAgent m_StageAgent;
        private CPostAgent m_PostAgent;
        private CAssetAgent m_AssetAgent;
        private CLevelAgent m_LevelAgent;
        private CStatusAgent m_StatusAgent;
        private CQuestAgent m_QuestAgent;
        private CItemAgent m_ItemAgent;
        private CSkillAgent m_SkillAgent;
        private CCouponAgent m_CouponAgent;
        private CRelicAgent m_RelicAgent;
        private CPvpAgent m_PvpAgent;
        private CShopAgent m_ShopAgent;
        private CBuffAgent m_BuffAgent;
        private CEventAgent m_EventAgent;

        private Dictionary<CDefine.PlayerPref, _PlayerPref> m_Prefs = new Dictionary<CDefine.PlayerPref, _PlayerPref>();

        private Dictionary<int, _GachaData> m_GachaDatas = new Dictionary<int, _GachaData>();

        private _RankReward m_RankReward;

        public _UserData UserData { get => m_UserData; }
        public CStageAgent StageAgent { get => m_StageAgent; }
        public CPostAgent PostAgent { get => m_PostAgent; }
        public CAssetAgent AssetAgent { get => m_AssetAgent; }
        public CLevelAgent LevelAgent { get => m_LevelAgent; }
        public CStatusAgent StatusAgent { get => m_StatusAgent; }
        public CQuestAgent QuestAgent { get => m_QuestAgent; }
        public CItemAgent ItemAgent { get => m_ItemAgent; }
        public CSkillAgent SkillAgent { get => m_SkillAgent; }
        public CCouponAgent CouponAgent { get => m_CouponAgent; }
        public CRelicAgent RelicAgent { get => m_RelicAgent; }
        public CPvpAgent PvpAgent { get => m_PvpAgent; }
        public CShopAgent ShopAgent { get => m_ShopAgent; }
        public CBuffAgent BuffAgent { get => m_BuffAgent; }
        public CEventAgent EventAgent { get => m_EventAgent; }

        public long SessionKey { get => m_SessionKey; }
        public string DeviceID { get => m_DeviceID; }
        public long UID { get => m_UserData.m_UID; }
        public int DBGUID { get; set; }

        public CUser(long sessionKey, string deviceID)
        {
            m_SessionKey = sessionKey;
            m_DeviceID = deviceID;
            m_RemoteIP = CNetManager.Instance.GetSessionRemoteIP(sessionKey);

            m_UserData = new _UserData();
            m_RankReward = new _RankReward();
            m_StageAgent = new CStageAgent(this);
            m_PostAgent = new CPostAgent(this);
            m_AssetAgent = new CAssetAgent(this);
            m_LevelAgent = new CLevelAgent(this);
            m_StatusAgent = new CStatusAgent(this);
            m_QuestAgent = new CQuestAgent(this);
            m_ItemAgent = new CItemAgent(this);
            m_SkillAgent = new CSkillAgent(this);
            m_CouponAgent = new CCouponAgent(this);
            m_RelicAgent = new CRelicAgent(this);
            m_PvpAgent = new CPvpAgent(this);
            m_ShopAgent = new CShopAgent(this);
            m_BuffAgent = new CBuffAgent(this);
            m_EventAgent = new CEventAgent(this);
        }

        public _GachaData FindGacha(int _groupID)
        {
            if (m_GachaDatas.ContainsKey(_groupID))
                return m_GachaDatas[_groupID];

            return null;
        }

        public List<_GachaData> GetGachalevelList()
        {
            return new List<_GachaData>(m_GachaDatas.Values);
        }

        private void Prepare()
        {
            var copyList = GachaLvTable.Instance.CopyDefault();
            foreach (var iter in copyList)
                m_GachaDatas[iter.m_GroupID] = iter;
        }

        public bool IsLogin()
        {
            return m_UserState == eUserState.Login ? true : false;
        }

        public void Init(_UserData userdata, List<_GachaData> gachas, List<_PlayerPref> prefs, _RankReward rankreward)
        {
            m_UserData = userdata;
            m_UserData.m_DefaultSkill = DefaultPlayerTable.Instance.CopyDefaultSkills();

            UpdateLevel();

            foreach (var iter in gachas)
                m_GachaDatas[iter.m_GroupID] = iter;

            foreach (var iter in m_GachaDatas)
                UpdateGachaLevel(iter.Value.m_GroupID);

            foreach (var iter in prefs)
                m_Prefs[iter.m_Type] = iter;

            m_RankReward = rankreward;
        }

        public bool HasEnoughAsset(CDefine.AssetType type, string tid, long cnt)
        {
            if (type == CDefine.AssetType.Coin)
                return AssetAgent.HasEnoughAsset(tid, cnt);
            else if (type == CDefine.AssetType.Item)
                return ItemAgent.HasEnoughItem(tid, cnt);

            return false;
        }


        public void OnEnter(DBCharacterLoadData dbloadData)
        {
            m_TodayExpTime = SDateManager.Instance.TomorrowTimestamp() + 1;

            DBGUID = CServerDefine.GetDBGUID(m_DeviceID);
            Prepare();

            Init(dbloadData.UserData, dbloadData.GachaDatas, dbloadData.PlayerPrefs, dbloadData.RankReward);
            InitActorData(m_UserData.m_UID, m_UserData.m_Name);

            m_StageAgent.Init(dbloadData.StageDatas);
            m_LevelAgent.Init(dbloadData.GrowthGoldDatas, dbloadData.GrowthLevelDatas);
            m_AssetAgent.Init(dbloadData.CoinDatas);
            m_ItemAgent.Init(dbloadData.Items, dbloadData.EquipPresets, dbloadData.KnightPresets);
            m_SkillAgent.Init(dbloadData.SkillPresets, dbloadData.SkillDatas, dbloadData.StageSkills);
            m_PostAgent.Init(dbloadData.Posts);
            m_CouponAgent.Init(dbloadData.Coupons);
            m_RelicAgent.Init(dbloadData.Relics);
            m_EventAgent.Init(dbloadData.EventDatas, dbloadData.EventRouletteDatas);
            m_BuffAgent.Init(dbloadData.AdsBuffs);
            
            RefreshRankReward(false);

            m_ShopAgent.Init(new List<_ShopData>(), dbloadData.EventShopDatas);
            m_QuestAgent.Init(dbloadData.QuestMains, dbloadData.QuestRepeat, dbloadData.QuestDaily, dbloadData.QuestCheckIn, dbloadData.QuestPass,
                dbloadData.QuestEvent);                     //QuesteAgent Init..     Mission Refresh

            ReportLoginData();

            RefreshUserOverView();

            CNetManager.Instance.P2C_ResultEnterServer(m_SessionKey, (ushort)Packet_Result.Result.Success);

            CLogger.Instance.System($"User Login : {m_UserData.m_UID}");

            m_UserState = eUserState.Login;

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.login, UserData, "", null, 1);
            CGameLog.Instance.Insert(log);
        }

        private void ReportLoginData()
        {
            //loginData begin
            CNetManager.Instance.P2C_ReportBeginLoginData(m_SessionKey);

            //notice
            CNetManager.Instance.P2M_RequestNoticeList(m_SessionKey);

            //send login data 
            CNetManager.Instance.P2C_ReportLoginData(m_SessionKey, m_UserData,
                m_StageAgent.GetList(), m_LevelAgent.GetGoldGrowthList(), m_LevelAgent.GetLevelGrowthList(),
                m_AssetAgent.GetList(), m_ItemAgent.GetItemPresetList(), m_ItemAgent.GetKnightPresetList(), GetGachalevelList(),
                m_SkillAgent.GetList(), m_SkillAgent.GetSkillPresetList(), m_RankReward, m_RelicAgent.GetList());

            CNetManager.Instance.P2C_ReportEventDataList(m_SessionKey, m_EventAgent.GetEventList());
            CNetManager.Instance.P2C_ReportEventShopDataList(m_SessionKey, m_ShopAgent.GetEventShopList());
            CNetManager.Instance.P2C_ReportEventRouletteDataList(m_SessionKey, m_EventAgent.GetEventRouletteList());
            CNetManager.Instance.P2C_ReportStageSkillEquipData(m_SessionKey, m_SkillAgent.GetStageSkillList());
            CNetManager.Instance.P2C_ReportItemDataList(m_SessionKey, m_ItemAgent.GetItemList());
            CNetManager.Instance.P2C_ReportQuestBoardList(m_SessionKey, m_QuestAgent.GetList());
            CNetManager.Instance.P2C_ReportPlayerPref(m_SessionKey, m_Prefs);
            CNetManager.Instance.P2C_ReportAdsBuffData(m_SessionKey, m_BuffAgent.GetAdsBuffList());

            StatusAgent.RefreshAll();

            CHotTimeManager.Instance.OnEnterUser(m_SessionKey);

            //load data end
            CNetManager.Instance.P2C_ReportEndLoginData(m_SessionKey);
            QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Checkin, "", "", 1);
        }

        public void Update()
        {
            if (!IsLogin())
                return;

            if (!m_Timer.Check())
                return;

            if (SDateManager.Instance.IsExpired(m_TodayExpTime))
            {
                RefreshRankReward(true);
                QuestAgent.RefreshDaily();
                BuffAgent.Refresh(true);

                QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Checkin, "", "", 1);
                m_TodayExpTime = SDateManager.Instance.TomorrowTimestamp() + 1;
                CLogger.Instance.System($"Next Day!!");
            }

            QuestAgent.Update();
            BuffAgent.Update();
            EventAgent.Update();
        }

        public long CorrectCoinValue(string coinType, long value)
        {
            CDefine.CoinType type = CDefine.CoinType.Max;
            if (!System.Enum.TryParse(coinType, out type))
                return value;

            switch (type)
            {
                case CDefine.CoinType.Gold:
                    {
                        double increaseVal =
                            StatusAgent.GetAbil(CDefine.EAbility.Stage_Gold_Up).val;

                        return (long)(value * increaseVal);
                    }
                case CDefine.CoinType.Exp:
                    {
                        double increaseVal =
                            StatusAgent.GetAbil(CDefine.EAbility.Stage_Exp_Up).val;

                        return (long)(value * increaseVal);
                    }
                default:
                    break;
            }

            return value;
        }

        public long SweepIncreaseValue(long value, CDefine.eStageType type)
        {
            switch (type)
            {
                case CDefine.eStageType.Gold_Stage:
                    return (long)(value * StatusAgent.GetAbil(CDefine.EAbility.Sweep_Gold_Stage_Up).val);
                case CDefine.eStageType.Enchant_Stage:
                    return (long)(value * StatusAgent.GetAbil(CDefine.EAbility.Sweep_Enchant_Stage_Up).val);
                case CDefine.eStageType.Knights_Stage:
                    return (long)(value * StatusAgent.GetAbil(CDefine.EAbility.Sweep_Knight_Stage_Up).val);
                case CDefine.eStageType.Relic_Stage:
                    return (long)(value * StatusAgent.GetAbil(CDefine.EAbility.Sweep_Relic_Stage_Up).val);
                default:
                    break;
            }

            return value;
        }

        public void UpdateAssetData(List<_AssetData> assetData, ref CDBMerge dbtran, bool bsend = false)
        {
            if (assetData == null) return;

            foreach (var iter in assetData)
            {
                switch (iter.Type)
                {
                    case CDefine.AssetType.Coin:
                        AssetAgent.UpdateRewardAsset(iter, ref dbtran);
                        break;
                    case CDefine.AssetType.Item:
                        ItemAgent.UpdateRewardAsset(iter, ref dbtran, bsend);
                        break;
                    case CDefine.AssetType.Max:
                        break;
                    default:
                        break;
                }
            }
        }

        public void UpdateAssetData(_AssetData assetData, ref CDBMerge dbtran, bool bsend = false)
        {
            if (assetData == null) return;

            switch (assetData.Type)
            {
                case CDefine.AssetType.Coin:
                    AssetAgent.UpdateRewardAsset(assetData, ref dbtran);
                    break;
                case CDefine.AssetType.Item:
                    ItemAgent.UpdateRewardAsset(assetData, ref dbtran, bsend);
                    break;
                case CDefine.AssetType.Max:
                    break;
                default:
                    break;
            }
        }

        public void UpdateExp(_AssetData asset)
        {
            if (asset == null) return;

            if (System.Enum.TryParse(asset.TableID, out CDefine.CoinType coinType))
            {
                if (coinType != CDefine.CoinType.Exp)
                    return;

                m_UserData.m_Exp += asset.Count;
                UpdateLevel();
            }
        }

        private void UpdateLevel()
        {
            int prevLevel = m_UserData.m_Level;

            int point = 0;
            var levelRecord = LevelUpTable.Instance.FindbyExp(m_UserData.m_Level, m_UserData.m_Exp, ref point);
            if (levelRecord == null)
                return;

            m_UserData.m_Level = levelRecord.Level;
            m_UserData.m_LevelPoint += point;

            if (prevLevel < m_UserData.m_Level)
                m_QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Player_Level, "", "", m_UserData.m_Level);
        }

        public void UpdateGachaLevel(int _gachaid)
        {
            var gachaData = FindGacha(_gachaid);
            if (gachaData == null)
                return;

            var gachalvRecord = GachaLvTable.Instance.FindbyExp(gachaData.m_GroupID, gachaData.m_Level, gachaData.m_Exp);
            if (gachalvRecord == null)
                return;

            gachaData.m_Level = gachalvRecord.GachaLv;
        }

        public Packet_Result.Result ReqPlayerPrefSave(long sessionKey, _PlayerPref saveData)
        {
            if (saveData.m_Type == CDefine.PlayerPref.Max)
                return Packet_Result.Result.InValidData;

            m_Prefs[saveData.m_Type] = saveData;

            CNetManager.Instance.P2C_ResultPlayerPrefSava(sessionKey, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }


        public void AfterQueryUpdateItemLevelGacha(long _sessionKey, _GachaData gachaData, CRewardInfo forClient, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(_sessionKey, forClient.GetList());
            CNetManager.Instance.P2C_ResultGachaLevelReward(_sessionKey, gachaData, forClient.GetList(), result);
        }

        public Packet_Result.Result ReqChangeNickName(long sessionKey, string name)
        {
            //name vaild
            if (string.IsNullOrEmpty(name))
                return Packet_Result.Result.IgnoreError;

            if (m_UserData.Equals(name))
                return Packet_Result.Result.IgnoreError;

            if (name.Length < DefineTable.Instance.Value<int>("MinName") || name.Length > DefineTable.Instance.Value<int>("MaxName"))
                return Packet_Result.Result.PacketError;

            if (!HasEnoughAsset(CDefine.AssetType.Coin, "BlueDia", DefineTable.Instance.Value<int>("NameChangeCost")))
                return Packet_Result.Result.LackAssetError;

            List<_AssetData> asset_list = new List<_AssetData>();
            asset_list.Add(new _AssetData(CDefine.AssetType.Coin, "BlueDia", -DefineTable.Instance.Value<int>("NameChangeCost")));
            
            CDBManager.Instance.QueryCharacterUpdateNickName(DBGUID, sessionKey, UID, name, asset_list);

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqProfileChange(long sessionKey, int profileID)
        {
            var record = ProfileTable.Instance.Find(profileID);
            if (record == null)
                return Packet_Result.Result.InValidRecord;

            //owned profile

            m_UserData.m_ProfileID = profileID;

            RefreshUserOverView();

            CNetManager.Instance.P2C_ResultProfileChange(sessionKey, m_UserData.m_ProfileID, Packet_Result.Result.Success);
            return Packet_Result.Result.Success;
        }

        public void AfterQueryChangeNickName(long _sessionKey, string changeName, List<_AssetData> assets, Packet_Result.Result result)
        {
            if (result == Packet_Result.Result.Success)
                UpdateNickName(changeName);

            CDBMerge dbtran = new CDBMerge();
            UpdateAssetData(assets, ref dbtran);

            ReportAssetData(dbtran);

            QuestAgent.UpdateQuestBorad(CDefine.MissionCondition.Change_NickName, "", "", 1);

            CNetManager.Instance.P2C_ResultChangeNickName(_sessionKey, UserData, result);
        }

        public void ADSkip()
        {
            m_UserData.m_ADSkip = true;
            m_BuffAgent.AdSkipBuff();

            m_StatusAgent.RefreshAll();
            CNetManager.Instance.P2C_ReportUserData(m_SessionKey, m_UserData);
            //CNetManager.Instance.P2C_ReportAdsBuffData(m_SessionKey, m_BuffAgent.GetAdsBuffList());
        }

        private void UpdateNickName(string name)
        {
            m_UserData.m_Name = name;
        }

        public _UserOverViewData SetOverView()
        {
            _UserOverViewData retVal = new _UserOverViewData();
            retVal.m_UID = m_UserData.m_UID;
            retVal.m_Name = m_UserData.m_Name;
            retVal.m_Level = m_UserData.m_Level;

            var stageData = StageAgent.Find(CDefine.eStageType.Main_Stage);
            if (stageData == null)
                return retVal;

            retVal.m_MaxMainStage = stageData.m_Data.maxTID;
            retVal.m_CurMainStage = stageData.m_Data.curTID;

            //todo condition
            retVal.m_PvpUserData.m_UID = retVal.m_UID;
            retVal.m_PvpUserData.m_Name = retVal.m_Name;
            retVal.m_PvpUserData.m_Level = retVal.m_Level;
            retVal.m_PvpUserData.m_AbilData = StatusAgent.GetAbilList();
            retVal.m_PvpUserData.m_KnightDatas = ItemAgent.GetEnableKnightList();
            retVal.m_PvpUserData.m_SkillData = SkillAgent.GetEnableSkillPresetList();
            retVal.m_PvpUserData.m_NormalSkillData = DefaultPlayerTable.Instance.CopyDefaultSkills();
            retVal.m_PvpUserData.m_SkinID = m_ActorData.m_SkinID;
            retVal.m_ProfileID = m_UserData.m_ProfileID;

            return retVal;
        }

        public void RefreshUserOverView()
        {
            var data = SetOverView();
            CNetManager.Instance.P2M_ReportUserOverViewUpdate(data);
        }

        public void RefreshRankReward(bool bSend)
        {
            var stage = StageAgent.Find(CDefine.eStageType.Main_Stage);
            if (stage == null)
                return;

            m_RankReward.m_RankValue = stage.m_Data.maxTID;
            m_RankReward.m_Type = CDefine.ERankType.Rank_MainStage;

            if (!SDateManager.Instance.IsExpired(m_RankReward.m_ExpTime))
                return;

            //todo : rank stage check
            m_RankReward.m_State = m_RankReward.m_RankValue >= 10001 ? CDefine.ERewardState.Rewardable : CDefine.ERewardState.None;

            m_RankReward.m_ExpTime = SDateManager.Instance.TomorrowTimestamp() + 1;

            if (bSend)
                CNetManager.Instance.P2C_ReportRankReward(SessionKey, m_RankReward);
        }

        public Packet_Result.Result ReqRankReward(long sessionKey, CDefine.ERankType type)
        {
            if (type != CDefine.ERankType.Rank_MainStage)
                return Packet_Result.Result.IgnoreError;

            if (m_RankReward.m_State != CDefine.ERewardState.Rewardable)
                return Packet_Result.Result.PacketError;

            var stageRecord = StageTable.Instance.Find(m_RankReward.m_RankValue);
            if (stageRecord == null)
                return Packet_Result.Result.InValidData;

            var rewards = RewardTable.Instance.Find(stageRecord.Fruition_RewardID);
            if (rewards == null)
                return Packet_Result.Result.PacketError;

            m_RankReward.m_State = CDefine.ERewardState.Rewarded;

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            foreach (var iter in rewards)
            {
                var reward = iter.Value;
                forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
            }

            bool bSend = true;
            forClient.UpdateRewardData(this, ref dbtran, bSend);
            dbtran.Merge();

            CDBManager.Instance.QueryCharacterRankRewardRewarded(DBGUID, sessionKey, UID, m_RankReward, forClient, dbtran);

            var log = LogHelper.MakeLogBson(eLogType.rank_reward, UserData, LogHelper.ToJson(m_RankReward), dbtran, 1);
            CGameLog.Instance.Insert(log);

            return Packet_Result.Result.Success;
        }

        public bool IsCondition(int condID)
        {
            var condRecord = ConditionTable.Instance.Find(condID);
            if (condRecord == null)
                return false;

            switch (condRecord.Type)
            {
                case CDefine.MissionCondition.Player_Level:
                    {
                        if (m_UserData.m_Level < condRecord.Value)
                            return false;

                        return true;
                    }
                case CDefine.MissionCondition.Stage_Clear:
                    {
                        if (condRecord.Target.Count < 1)
                            return false;

                        foreach (var iter in condRecord.Target)
                        {
                            CDefine.eStageType type;
                            if (!System.Enum.TryParse(iter, out type))
                                return false;

                            var userStageData = StageAgent.Find(type);
                            if (userStageData == null)
                                return false;

                            if (userStageData.m_Data.maxTID < condRecord.Value)
                                return false;
                        }

                        return true;
                    }
                case CDefine.MissionCondition.Growth_Gold:
                    {
                        if (condRecord.Target.Count < 1)
                            return false;

                        foreach (var it in condRecord.Target)
                        {
                            int id;
                            if (!int.TryParse(it, out id))
                                return false;

                            var goldLevel = LevelAgent.GetGoldGrowth(id);
                            if (goldLevel == null)
                                return false;

                            if (goldLevel.m_UseCount < condRecord.Value)
                                return false;
                        }

                        return true;
                    }
                //case CDefine.MissionCondition.Equip_Item:
                //    {
                //        foreach (var iter in condRecord.Target)
                //        {
                //            CDefine.EquipSlotType type;
                //            if (!System.Enum.TryParse(iter, out type))
                //                return false;

                //            if (ItemAgent.IsEquipSlotItem(type))
                //                return true;
                //        }

                //        return true;
                //    }
                case CDefine.MissionCondition.Equip_Knight:
                    {
                        var presetlist = SkillAgent.GetSkillPresetList();
                        if (presetlist == null)
                            return false;

                        foreach (var preset in presetlist)
                        {
                            foreach (var slot in preset.m_EquipedSlots)
                            {
                                if (slot.ID < 1)
                                    continue;

                                var item = ItemAgent.Find(slot.ID);
                                if (item != null)
                                    return true;
                            }
                        }

                        return false;
                    }
                case CDefine.MissionCondition.Equip_Skill:
                    {
                        var presetlist = SkillAgent.GetSkillPresetList();
                        if (presetlist == null)
                            return false;

                        foreach (var preset in presetlist)
                        {
                            foreach (var slot in preset.m_EquipedSlots)
                            {
                                var skill = SkillAgent.FindSkill(slot.ID);
                                if (skill != null)
                                    return true;
                            }
                        }

                        return true;
                    }
                case CDefine.MissionCondition.Gacha_Item:
                    {
                        foreach (var iter in condRecord.Target)
                        {
                            int gachid;
                            if (!int.TryParse(iter, out gachid))
                                return false;

                            var gacha = FindGacha(gachid);
                            if (gacha == null)
                                return false;

                            if (gacha.m_Exp < condRecord.Value)
                                return false;
                        }

                        return true;
                    }
                case CDefine.MissionCondition.Growth_Level:
                    {
                        if (condRecord.Target.Count < 1)
                            return false;

                        foreach (var it in condRecord.Target)
                        {
                            int id;
                            if (!int.TryParse(it, out id))
                                return false;

                            var goldLevel = LevelAgent.GetLevelGrowth(id);
                            if (goldLevel == null)
                                return false;

                            if (goldLevel.m_UseCount < condRecord.Value)
                                return false;
                        }

                        return true;
                    }
                case CDefine.MissionCondition.SkillLearn:
                    {
                        if (condRecord.Target.Count < 1)
                            return false;

                        foreach (var iter in condRecord.Target)
                        {
                            int skillID;
                            if (!int.TryParse(iter, out skillID))
                                return false;

                            var skill = SkillAgent.FindSkill(skillID);
                            if (skill == null)
                                return false;

                            if (!skill.IsLearend)
                                return false;
                        }

                        return true;
                    }
                default:
                    break;
            }

            return true;
        }

        public long CondValue(int condID)
        {
            var condRecord = ConditionTable.Instance.Find(condID);
            if (condRecord == null)
                return 0;

            switch (condRecord.Type)
            {
                case CDefine.MissionCondition.Player_Level:
                    {
                        var userData = UserData;
                        if (userData == null)
                            return 0;

                        return userData.m_Level;
                    }
                case CDefine.MissionCondition.Stage_Clear:
                    {
                        if (condRecord.Target.Count < 1)
                            return 0;

                        foreach (var iter in condRecord.Target)
                        {
                            CDefine.eStageType type;
                            if (!System.Enum.TryParse(iter, out type))
                                return 0;

                            var userStageData = StageAgent.Find(type);
                            if (userStageData == null)
                                return 0;

                            return userStageData.m_Data.maxTID;
                        }

                        return 0;
                    }
                case CDefine.MissionCondition.Equip_Skill:
                    {
                        var presetlist = SkillAgent.GetSkillPresetList();
                        if (presetlist == null)
                            return 0;

                        foreach (var preset in presetlist)
                        {
                            foreach (var slot in preset.m_EquipedSlots)
                            {
                                if (slot.ID < 1)
                                    continue;

                                var skill = SkillAgent.FindSkill(slot.ID);
                                if (skill == null)
                                    continue;

                                if (skill.IsLearend)
                                    return 1;
                            }
                        }

                        return 0;
                    }
                //case CDefine.MissionCondition.Equip_Item:
                //    {
                //        foreach (var iter in condRecord.Target)
                //        {
                //            CDefine.EquipSlotType type;
                //            if (!System.Enum.TryParse(iter, out type))
                //                return 0;

                //            if (ItemAgent.IsEquipSlotItem(type))
                //                return 1;
                //        }

                //        return 0;
                //    }
                case CDefine.MissionCondition.Equip_Knight:
                    {
                        var presetlist = ItemAgent.GetKnightPresetList();
                        if (presetlist == null)
                            return 0;

                        foreach (var preset in presetlist)
                        {
                            foreach (var slot in preset.m_EquipedSlots)
                            {
                                if (slot.ID < 1)
                                    continue;

                                var item = ItemAgent.Find(slot.ID);
                                if (item != null)
                                    return 1;
                            }
                        }

                        return 0;
                    }
                case CDefine.MissionCondition.Growth_Gold:
                    {
                        if (condRecord.Target.Count < 1)
                            return 0;

                        foreach (var it in condRecord.Target)
                        {
                            int id;
                            if (!int.TryParse(it, out id))
                                return 0;

                            var goldGrowth = LevelAgent.GetGoldGrowth(id);
                            if (goldGrowth == null)
                                return 0;

                            return goldGrowth.m_UseCount;
                        }

                        return 0;
                    }
                case CDefine.MissionCondition.Growth_Level:
                    {
                        if (condRecord.Target.Count < 1)
                            return 0;

                        foreach (var it in condRecord.Target)
                        {
                            int id;
                            if (!int.TryParse(it, out id))
                                return 0;

                            var levelGrowth = LevelAgent.GetLevelGrowth(id);
                            if (levelGrowth == null)
                                return 0;

                            return levelGrowth.m_UseCount;
                        }

                        return 0;
                    }
                case CDefine.MissionCondition.SkillLearn:
                    {
                        foreach (var iter in condRecord.Target)
                        {
                            int skillID;
                            if (!int.TryParse(iter, out skillID))
                                return 0;

                            var skill = SkillAgent.FindSkill(skillID);
                            if (skill == null)
                                return 0;

                            if (skill.IsLearend)
                                return 1;
                        }

                        return 0;
                    }
                case CDefine.MissionCondition.Gacha_Item:
                    {
                        foreach (var iter in condRecord.Target)
                        {
                            int gachid;
                            if (!int.TryParse(iter, out gachid))
                                return 0;

                            var gacha = FindGacha(gachid);
                            if (gacha == null)
                                return 0;

                            return gacha.m_Exp;
                        }

                        return 0;
                    }
                default:
                    break;
            }

            return 0;

        }

        public void AfterQueryRankReward(long sessionKey, _RankReward reward, CRewardInfo forclient, CDBMerge dbtran, Packet_Result.Result result)
        {
            ReportAssetData(dbtran);
            CNetManager.Instance.P2C_ResultRankReward(sessionKey, reward, forclient.GetList(), result);
        }

        public void ReportAssetData(CDBMerge dbtran)
        {
            List<_AssetData> sendList = new List<_AssetData>();
            foreach(var iter in dbtran.m_UpdateCoinList)
            {
                if (iter.Type != CDefine.AssetType.Coin)
                    continue;

                var findCoin = m_AssetAgent.Find(iter.TableID);
                if (findCoin == null)
                    continue;

                sendList.Add(findCoin);
            }

            if (sendList.Count > 0)
                CNetManager.Instance.P2C_ReportAssetData(m_SessionKey, sendList);
        }

        //public void ReportItemData(CDBMerge dbtran)
        //{
        //    List<_ItemData> sendList = new List<_ItemData>();
        //    foreach(var iter in dbtran.m_UpdateItemList)
        //    {
        //        var item = m_ItemAgent.Find(iter.ItemID);
        //        if (item == null)
        //            continue;

        //        sendList.Add(item);
        //    }

        //    if (sendList.Count > 0)
        //        CNetManager.Instance.P2C_ReportItemDataList(m_SessionKey, sendList);
        //}

        public void Logout()
        {
            //user messageServer Remove
            CNetManager.Instance.P2M_ReportUserLogout(UID, DeviceID);

            m_ItemAgent.SaveAtLogout();
            m_SkillAgent.SaveAtLogout();
            m_QuestAgent.SaveAtLogout();
            m_BuffAgent.SaveAtLogout();

            CDBManager.Instance.QueryCharacterLogout(m_SessionKey, m_UserData);
            CDBManager.Instance.CQueryCharacterRankRewardUpdate(DBGUID, m_SessionKey, UID, m_RankReward);

            foreach (var iter in m_Prefs)
            {
                _PlayerPref pref = iter.Value;
                CDBManager.Instance.QueryCharacterUpsertPlayerPref(DBGUID, m_SessionKey, pref);
            }

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.logout, UserData, "", null, 1);
            CGameLog.Instance.Insert(log);
        }

        public void CheatResetDate()
        {
            m_TodayExpTime = SDateManager.Instance.TodayTimestamp() + 1;
        }

        public void CheakRankReset()
        {
            m_RankReward.m_ExpTime = SDateManager.Instance.TodayTimestamp();
        }
    
    }
}
