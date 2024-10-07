using System;
using System.Collections.Generic;
using SCommon;
using SDB;
using Global;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Policy;
using System.Windows.Data;

namespace PlayServer
{
    public class CDBManager : SSingleton<CDBManager>, IDisposable
    {
        private bool m_Disposed;

        private bool m_Run;

        private SMsSql[] m_GameDB;

        public int totalcount = 0;

        ~CDBManager()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (m_Disposed) return;
            if (disposing)
            {
                if (m_GameDB != null)
                {
                    foreach (var DB in m_GameDB)
                        DB.Dispose();

                    m_GameDB = null;
                }
            }
            m_Disposed = true;
        }

        public void Start()
        {
            if (m_Run) return;
            m_Run = true;

            string connectDBString;
            connectDBString =
                $"server = {CConfig.Instance.GameDB.m_Host}; " +
                $"uid = {CConfig.Instance.GameDB.m_ID}; " +
                $"pwd = {CConfig.Instance.GameDB.m_PW}; " +
                $"database = {CConfig.Instance.GameDB.m_Name}";

            string error_string = string.Empty;

            m_GameDB = new SMsSql[CConfig.Instance.DBThreadCount];
            for (int i = 0; i < m_GameDB.Length; i++)
            {
                m_GameDB[i] = new SMsSql();
                m_GameDB[i].Start(connectDBString, ref error_string);
            }

            if (false == string.IsNullOrEmpty(error_string))
                CLogger.Instance.System(error_string);
        }
        public void Stop()
        {
            if (m_GameDB != null) foreach (var DB in m_GameDB) DB.Stop();

            m_Run = false;
        }
        public void Update()
        {
            if (m_GameDB != null) foreach (var DB in m_GameDB) DB.Update();
        }

        public void InsertGameDB(int dbguid, IMsSqlQuery query)
        {
            m_GameDB[dbguid % m_GameDB.Length].Insert(query);
        }

        public void InsertGameDB(long sessionKey, IMsSqlQuery query)
        {
            m_GameDB[sessionKey % m_GameDB.Length].Insert(query);
        }


        public int[] GetGameDBFPS()
        {
            if (m_GameDB == null) return new int[0];
            int[] fps = new int[m_GameDB.Length];
            for (int i = 0; i < fps.Length; i++) fps[i] = m_GameDB[i].GetFPS();
            return fps;
        }

        public int[] GetGameDBInputQueueCount()
        {
            if (m_GameDB == null) return new int[0];
            int[] inputQueueCount = new int[m_GameDB.Length];
            for (int i = 0; i < inputQueueCount.Length; i++) inputQueueCount[i] = m_GameDB[i].GetInputQueueCount();
            return inputQueueCount;
        }

        public void SetParam(ref SqlCommand sqlCmd, ref ProcedureResult data, ref SMsSql agent, object obj)
        {
            Type type = obj.GetType();
            FieldInfo[] field = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            for (int i = 0; i < field.Length; ++i)
            {
                string parameterName = string.Format("@{0}", field[i].Name);
                sqlCmd.Parameters.Add(parameterName, agent.GetSqlDbType(field[i].FieldType)).Direction = ParameterDirection.Input;
                sqlCmd.Parameters[parameterName].Value = field[i].GetValue(obj);
            }

            data.m_sqlErrorNumber = sqlCmd.Parameters.Add("@sp_rtn", SqlDbType.Int);
            data.m_sqlErrorNumber.Direction = ParameterDirection.Output;

            // 출력인수 설정     
            data.m_sqlErrorMessage = sqlCmd.Parameters.Add("@sp_msg", SqlDbType.VarChar, 4000);
            data.m_sqlErrorMessage.Direction = ParameterDirection.Output;

            return;
        }

        #region Query
        //==================== PLAYER ===================
        public void QueryCharacterCreate(long sessionKey, long uid, int profile_id)
        {
            InsertGameDB(sessionKey, new CQueryCharacterCreate(sessionKey, uid, profile_id));
        }

        public void QueryCharacterLoad(long sessionKey, long uid, string deviceID)
        {
            InsertGameDB(sessionKey, new CQueryCharacterLoad(sessionKey, uid, deviceID));
        }

        public void QueryCharacterUpdateGoldGrowth(int DBGUID, long sessionKey, long uid, _LevelData growthGold, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateGoldGrowth(sessionKey, uid, growthGold, dbtran));
        }

        public void QueryCharacterUpdateLevelGrowth(int DBGUID, long sessionKey, _LevelData levelGrowth, _UserData userData, Packet_C2P.Protocol protocol)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateLevelGrowth(sessionKey, levelGrowth, userData, protocol));
        }

        public void QueryCharacterLogout(long DBGUID, _UserData userData)
        {
            InsertGameDB(DBGUID, new CQueryCharacterLogout(userData));
        }

        public void QueryCharacterUpsertPlayerPref(int DBGUID, long sessionKey, _PlayerPref pref)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpsertPlayerPref(sessionKey, pref));
        }

        public void QueryCharacterUpdateNickName(int DBGUID, long sessionKey, long uid, string name, List<_AssetData> assetList)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateNickName(sessionKey, uid, name, assetList));
        }

        public void QueryCharacterViewUserInfo(long DBGUID, long sessionKey, long targetID)
        {
            InsertGameDB(DBGUID, new CQueryCharacterViewUserInfo(sessionKey, targetID));
        }

        //==================== STAGE ===================
        public void QueryCharacterUpdateStage(int DBGUID, long sessionKey, _UserData userData, _StageData stageData, CRewardInfo rewardInfo, CDBMerge dbtran, bool isclear)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateStage(sessionKey, userData, stageData, rewardInfo, dbtran, isclear));
        }

        public void QueryCharacterStageSave(int DBGUID, long sessionKey, long uid, _StageData stageData)
        {
            InsertGameDB(DBGUID, new CQueryCharacterStageSave(sessionKey, uid, stageData));
        }

        public void QueryCharacterStageSweep(int DBGUID, long sessionKey, long uid, CRewardInfo forClient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterStageSweep(sessionKey, uid, forClient, dbtran));
        }
        public void QueryCharacterUpdateUpgradeStage(int DBGUID, long sessionKey, _UserData userData, _StageData stageData, CRewardInfo rewardInfo, CDBMerge dbtran, bool isclear)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateUpgradeStage(sessionKey, userData, stageData, rewardInfo, dbtran, isclear));
        }

        //==================== KNIGHT ===================
        public void QueryCharacterUpdateKnight(int DBGUID, long _sessionKey, long _uid,  _ItemData item)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateKnight(_sessionKey, _uid, item));
        }

        public void QueryCharacterUpdateEquipKnightPreset(int DBGUID, long _sessionKey, long _uid, _KnightEquipPresetData _preset)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateEquipKnightPreset(_sessionKey, _uid, _preset));
        }

        //==================== COIN ===================
        public void QueryCharacterUpsertCoin(int DBGUID, long _sessionKey, long _uid, string _type, long _value)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpsertCoin(_sessionKey, _uid, _type, _value));
        }
        public void QueryCharacterUpdateCoin(int DBGUID, long _sessionKey, long _uid, string _type, long _val)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateCoin(_sessionKey, _uid, _type, _val));
        }

        public void QueryCharacterUpdateCoinList(long DBGUID, long sessionKey, long uid, List<_AssetData> rewards)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateCoinList(sessionKey, uid, rewards));
        }

        public void QueryCharacterUpdateItemCountList(int DBGUID, long _sessionKey, long _uid, CDBMerge _dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateItemCountList(_sessionKey, _uid, _dbtran));
        }
        //==================== GACHA ===================
        public void QueryCharacterUpdateGacha(int DBGUID, long _sessionKey, long _uid, _GachaData _gachaData, CRewardInfo _forClient, CDBMerge _dbtran, int gachatid)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateGacha(_sessionKey, _uid, _gachaData, _forClient, _dbtran, gachatid));
        }

        public void QueryCharacterUpdateGachaLevelReward(int DBGUID, long _sessionKey, long _uid, _GachaData _gachaData, CRewardInfo _forClient, CDBMerge _dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateGachaLevelReward(_sessionKey, _uid, _gachaData, _forClient, _dbtran));
        }
        //==================== ITEM ===================
        public void QueryCharacterUpdateItemBreakThrough(int DBGUID, long _sessionKey, long _uid, _ItemData item, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateItemBreakThrough(_sessionKey, _uid, item, dbtran));
        }

        public void QueryCharacterUpdateItemRandomOption(int DBGUID, long _sessionKey, long _uid, _ItemData item, CDBMerge dbtran, Packet_C2P.Protocol protocol)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateItemRandomOption(_sessionKey, _uid, item, dbtran, protocol));
        }

        public void QueryCharacterUpdateItemConsume(int DBGUID, long _sessionKey, long _uid, _ItemData item, CDBMerge dbtran, CRewardInfo forclient)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateItemConsume(_sessionKey, _uid, item, dbtran, forclient));
        }

        public void QueryCharacterUpdateItemSell(int DBGUID, long _sessionKey, long uid, _ItemData item, CRewardInfo forClient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateItemSell(_sessionKey, uid, item, forClient, dbtran));
        }
        public void QueryCharacterUpdateEquipPreset(int DBGUID, long sessionKey, long userUID, _ItemEquipPresetData preset)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateEquipPreset(sessionKey, userUID, preset));
        }
        public void QueryCharacterUpdateItemLevel(int DBGUID, long _sessionKey, long _uid, _ItemData _item, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateItemLevel(_sessionKey, _uid, _item, dbtran));
        }
        public void QueryCharacterUpdateItemCount(int DBGUID, long sessionKey, long userUID, _ItemData itemData)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateItemCount(sessionKey, userUID, itemData));
        }

        public void QueryCharacterItemKnightUpgrade(int DBGUID, long sessionKey, long userUID, _ItemData itemData, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterItemKnightUpgrade(sessionKey, userUID, itemData, dbtran));
        }

        //==================== SKILL ===================
        public void QueryCharacterUpdateSkillOpen(int DBGUID, long sessionKey, long uid, _SkillData skillData)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateSkillOpen(sessionKey, uid, skillData));
        }
        public void QueryCharacterUpdateSkillLevel(int DBGUID, long sessionKey, long uid, _SkillData skillData, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateSkillLevel(sessionKey, uid, skillData, dbtran));
        }
        public void QueryCharacterUpdateEquipSkillPreset(int DBGUID, long _sessionKey, long _uid, _SkillEquipPresetData _preset)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUpdateEquipSkillPreset(_sessionKey, _uid, _preset));
        }

        public void QueryCharacterStageSkillUpdate(int DBGUID, long sessionKey, long uid, _StageSkillData stageSkillData)
        {
            InsertGameDB(DBGUID, new CQueryCharacterStageSkillUpdate(sessionKey, uid, stageSkillData));
        }

        //==================== POST ===================
        public void QueryCharacterPostBoxOpen(int DBGUID, long _sessionKey, long _uid, bool clientFlag)
        {
            InsertGameDB(DBGUID, new CQueryCharacterPostBoxOpen(_sessionKey, _uid, clientFlag));
        }
        public void QueryCharacterPostRead(int DBGUID, long _sessionKey, long _uid, List<_PostData> readPosts, List<long> postids)
        {
            InsertGameDB(DBGUID, new CQueryCharacterPostRead(_sessionKey, _uid, readPosts, postids));
        }
        public void QuerySystemUpsertPost(long _sessionKey, _PostData post)
        {
            InsertGameDB(_sessionKey, new CQuerySystemUpsertPost(post));
        }

        public void QueryCharacterPostReward(int DBGUID, long _sessionKey, long _uid, List<_PostData> readPosts, List<long> postids, CDBMerge dbtran, CRewardInfo forClient)
        {
            InsertGameDB(DBGUID, new CQueryCharacterPostReward(_sessionKey, _uid, readPosts, postids, dbtran, forClient));
        }

        public void QueryCharacterPostRemove(long DBGUID, long sessionKey, long uid, List<_PostData> removePostData, List<long> dbRemoveList)
        {
            InsertGameDB(DBGUID, new CQueryCharacterPostRemove(sessionKey, uid, removePostData, dbRemoveList));
        }

        //==================== COUPON ===================
        public void QueryCharacterVaildCoupon(int DBGUID, long sessionKey, long uid, string couponID)
        {
            InsertGameDB(DBGUID, new CQueryCharacterVaildCoupon(sessionKey, uid, couponID));
        }
        public void QueryCharacterUseCoupon(int DBGUID, long sessionKey, long uid, string coupon_id, string used_coupons, CRewardInfo forClient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterUseCoupon(sessionKey, uid, coupon_id, used_coupons, forClient, dbtran));
        }
        public void QuerySystemCouponUpsert(int DBGUID, string couponID, int cnt, int use_level, long begin, long exp, List<_AssetData> rewards)
        {
            InsertGameDB(DBGUID, new CQuerySystemCouponUpsert(couponID, cnt, use_level, begin, exp, rewards));
        }

        //==================== RANK ====================
        public void QueryCharacterRankRewardRewarded(int DBGUID, long sessionKey, long uid, _RankReward rank, CRewardInfo forclient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterRankRewardRewarded(sessionKey,uid , rank, forclient, dbtran));
        }

        public void CQueryCharacterRankRewardUpdate(long DBGUID, long sessionKey, long uid, _RankReward rankReward)
        {
            InsertGameDB(DBGUID, new CQueryCharacterRankRewardUpdate(sessionKey, uid, rankReward));
        }

        //============QUEST_MAIN===========
        public void QueryCharacterQuestMainUpdate(int DBGUID, long sessionKey, long uid, _Mission mission)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestMainUpdate(sessionKey, uid, mission));
        }
        public void QueryCharacterQuestMainReward(int DBGUID, long sessionKey, long userUID, int removeID, _Mission mission, CRewardInfo forClient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestMainReward(sessionKey, userUID, removeID, mission, dbtran, forClient));
        }

        //============QUEST_REPEAT===========
        public void QueryCharacterQuestRepeatUpdate(int DBGUID, long sessionKey, long uid, List<_Mission> missions)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestRepeatUpdate(sessionKey, uid, missions));
        }
        public void QueryCharacterQuestRepeatReward(int DBGUID, long sessionKey, long uid, List<_Mission> missions, CRewardInfo forclient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestRepeatReward(sessionKey, uid, missions, forclient, dbtran));
        }

        //============QUEST_DAILY===========
        public void QueryCharacterQuestDailyUpdate(int DBGUID, long sessionKey, long uid, List<_Mission> missions, DateTime exp_time)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestDailyUpdate(sessionKey, uid, missions, exp_time));
        }
        public void QueryCharacterQuestDailyReward(int DBGUID, long sessionKey, long uid, List<_Mission> missions, DateTime exp_time, CRewardInfo forclient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestDailyReward(sessionKey, uid, missions, exp_time, forclient, dbtran));
        }

        //============ QUEST_CHECKIN ===========
        public void QueryCharacterQuestCheckInUpdate(int DBGUID, long sessionKey, long uid, string questID, _Mission mission, DateTime exp_time)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestCheckInUpdate(sessionKey, uid, questID, mission, exp_time));
        }
        public void QueryCharacterQuestCheckInReward(int DBGUID, long sessionKey, long uid, string questID, _Mission mission, DateTime exp_time, CRewardInfo forclient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestCheckInReward(sessionKey, uid, questID, mission, exp_time, forclient, dbtran));
        }
        
        //============ QUEST_PASS ===========
        public void QueryCharacterQuestPassUpdate(int DBGUID, long sessionKey, long uid, string questID, bool active, List<_Mission> missions)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestPassUpdate(sessionKey, uid, questID, active,missions));
        }
        public void QueryCharacterQuestPassReward(long DBGUID, long sessionKey, long uid, string questID, bool active ,List<_Mission> missions, CRewardInfo forClient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestPassReward(sessionKey, uid, questID, active, missions, forClient, dbtran));
        }
        
        //============ RELIC ===========
        public void QueryCharacterRelicEnchant(int DBGUID, long sessionKey, long uid, _RelicData relic, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterRelicEnchant(sessionKey, uid, relic, dbtran));
        }

        //============ IAP ==============
        public void QueryCharacterShopIAPUpdate(int DBGUID, long sessionKey, long uid, _ShopData shopData, _IAPReceipt iapreceipt, _PostData postData)
        {
            InsertGameDB(DBGUID, new CQueryCharacterShopIAPUpdate(sessionKey, uid, shopData, iapreceipt, postData));
        }

        //=========== AD Buff ===============
        public void QueryCharacterAdsBuffUpdate(long DBGUID, long sessionKey, long uid, _AdsBUffData buffData)
        {
            InsertGameDB(DBGUID, new CQueryCharacterAdsBuffUpdate(sessionKey, uid, buffData));
        }

        //============= Event ==================
        public void QueryCharacterEventShopUpdate(long DBGUID, long sessionKey, long uid, _EventShopData shopData, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterEventShopUpdate(sessionKey, uid, shopData, dbtran));
        }

        public void QueryCharacterEventUpdate(long DBGUID, long sessionkey, long uid, _EventData eventData, CDefine.CoinType type)
        {
            InsertGameDB(DBGUID, new CQueryCharacterEventUpdate(sessionkey, uid, eventData, type));
        }

        public void QueryCharacterQuestEventUpdate(int DBGUID, long sessionKey, long uid, List<_Mission> missions, DateTime exp_time, string quest_id)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestEventUpdate(sessionKey, uid, missions, exp_time, quest_id));
        }

        public void CQueryCharacterQuestEventReward(int DBGUID, long sessionKey, long uid, List<_Mission> missions, DateTime exp_time, CRewardInfo forclient, CDBMerge dbtran, string quest_id)
        {
            InsertGameDB(DBGUID, new CQueryCharacterQuestEventReward(sessionKey, uid, missions, exp_time, forclient, dbtran, quest_id));
        }

        public void QueryCharacterEventRouletteReward(int DBGUID, long sessionKey, long uid, int picked_index, _EventRouletteData rouletteData, CRewardInfo forClient, CDBMerge dbtran)
        {
            InsertGameDB(DBGUID, new CQueryCharacterEventRouletteReward(sessionKey, uid, picked_index, rouletteData, forClient, dbtran));
        }

        //==================== Shop ==========================
        public void QueryCharacterShopBuy(int DBGUID, long sessionKey, long uid, _ShopData shopData, CDBMerge dbtran, CRewardInfo forClient)
        {
            InsertGameDB(DBGUID, new CQueryCharacterShopBuy(sessionKey, uid, shopData, forClient, dbtran));
        }

        #endregion
    }
}
