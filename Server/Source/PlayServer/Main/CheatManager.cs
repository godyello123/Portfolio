using System;
using System.Collections.Generic;
using SNetwork;
using Global;
using Global.RestAPI;
using SCommon;
using Packet_C2P;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.AndroidPublisher.v3.Data;
using System.Linq;
using System.CodeDom;
using System.Windows.Media.Animation;
using System.Reflection;

namespace PlayServer
{
    public class GachaSimuData
    {
        public int TableID;
        public long Cnt;

        public GachaSimuData(int tid, long cnt) { TableID = tid; Cnt = cnt; }
    }

    class CCheatManager : SSingleton<CCheatManager>
    {
        delegate Packet_Result.Result CheatHandlerDelegate(CUser user, List<string> args);
        Dictionary<string, CheatHandlerDelegate> m_CheatHandler = new Dictionary<string, CheatHandlerDelegate>();

        public void Init()
        {
            Type type = typeof(CCheatManager);
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var methodPrams = method.GetParameters();
                if (methodPrams.Length < 2) continue;

                if (methodPrams[0].ParameterType == typeof(CUser) &&
                    methodPrams[1].ParameterType == typeof(List<string>))
                {
                    var del = Delegate.CreateDelegate(typeof(CheatHandlerDelegate), this, method) as CheatHandlerDelegate;
                    if (del != null)
                        m_CheatHandler[method.Name.ToLower()] = del;
                }
            }
        }

        public Packet_Result.Result ReqCheat(long sessionKey, CUser user, string cmd, List<string> args)
        {
            if (!m_CheatHandler.ContainsKey(cmd))
                return Packet_Result.Result.CheatParamError;

            var result = m_CheatHandler[cmd](user, args);
            if (result == Packet_Result.Result.Success)
            {
                //todo : log
                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.cheat, user.UserData, cmd, null, 1);
                CGameLog.Instance.Insert(log);

                CNetManager.Instance.P2C_ResultCheat(sessionKey, result);
            }

            return result;
        }

        private Packet_Result.Result Coin_Set(CUser user, List<string> args)
        {

            if (args.Count < 2)
                return Packet_Result.Result.PacketError;

            int index = 0;
            string idstr = args[index++];
            CDefine.CoinType coiintype;
            if (!Enum.TryParse(idstr, out coiintype))
                return Packet_Result.Result.PacketError;

            if (coiintype == CDefine.CoinType.Exp)
                return Packet_Result.Result.PacketError;

            long value;
            string valuestr = args[index++];
            if (!long.TryParse(valuestr, out value))
                return Packet_Result.Result.PacketError;

            _AssetData asset = new _AssetData(CDefine.AssetType.Coin, coiintype.ToString(), value);
            user.AssetAgent.Upsert(asset);

            CDBManager.Instance.QueryCharacterUpsertCoin(user.DBGUID, user.SessionKey, user.UserData.m_UID, asset.TableID, asset.Count);
            CNetManager.Instance.P2C_ReportAssetData(user.SessionKey, new List<_AssetData> { asset });
            CNetManager.Instance.P2C_ResultCheat(user.SessionKey, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Player_Level(CUser user, List<string> args)
        {
            if (args.Count < 1)
                return Packet_Result.Result.PacketError;

            int targetLevel = 0;
            int index = 0;
            string levelstr = args[index++];
            if (!int.TryParse(levelstr, out targetLevel))
                return Packet_Result.Result.PacketError;

            var userData = user.UserData;
            if (userData == null)
                return Packet_Result.Result.PacketError;

            var beginRecord = LevelUpTable.Instance.Find(userData.m_Level);
            if (beginRecord == null)
                return Packet_Result.Result.CheatParamError;

            int levelpoint = 0;
            for (int i = 1; i <= targetLevel; ++i)
            {
                var levelRcd = LevelUpTable.Instance.Find(i);
                if (levelRcd == null)
                    continue;

                levelpoint += levelRcd.point;
            }

            var record = LevelUpTable.Instance.Find(targetLevel);
            if (record == null)
                return Packet_Result.Result.PacketError;

            userData.m_Level = record.Level;
            userData.m_Exp = record.exp;
            userData.m_LevelPoint = levelpoint;

            CNetManager.Instance.P2C_ReportUserData(user.SessionKey, userData);
            CDBManager.Instance.QueryCharacterLogout(user.DBGUID, userData);
            
            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Add_Item(CUser user, List<string> args)
        {
            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int index = 0;
            string strItemTID = args[index++];
            var itemRecord = ItemTable.Instance.Find(strItemTID);

            long count = 0;
            if (!long.TryParse(args[index++], out count))
                return Packet_Result.Result.CheatParamError;

            CDBMerge dbtran = new CDBMerge();
            user.ItemAgent.InsertItem(strItemTID, count, ref dbtran, true);

            dbtran.Merge();

            var db_updateitems = dbtran.m_UpdateItemList;
            foreach (var iter in db_updateitems)
                CDBManager.Instance.QueryCharacterUpdateItemCount(user.DBGUID, user.SessionKey, user.UserData.m_UID, iter);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result All_For_One(CUser user, List<string> args)
        {
            //race condition 일어날 수 있지만 치트니까 무시
            Task.Run(() =>
            {
                int itemCount = 1000;
                CDBMerge dbtran = new CDBMerge();
                foreach (CDefine.EItemMainType type in Enum.GetValues(typeof(CDefine.EItemMainType)))
                {
                    var itemRecords = ItemTable.Instance.Gaher(type);
                    if (itemRecords == null)
                        continue;

                    foreach (var rcd in itemRecords)
                    {
                        if (rcd.MainType == CDefine.EItemMainType.Knight)
                        {
                            var item = new _AssetData(CDefine.AssetType.Item, rcd.ID.ToString(), 1);
                            user.UpdateAssetData(item, ref dbtran, true);
                        }
                        else
                        {
                            var item = new _AssetData(CDefine.AssetType.Item, rcd.ID.ToString(), itemCount);
                            user.UpdateAssetData(item, ref dbtran, true);
                        }
                    }
                }

                long value = 10000000000;
                for (CDefine.CoinType type = CDefine.CoinType.BlueDia; type < CDefine.CoinType.Max; type++)
                {
                    var coinRecord = CoinTable.Instance.Find(type.ToString());
                    if (coinRecord == null)
                        continue;

                    user.UpdateAssetData(new _AssetData(CDefine.AssetType.Coin, type.ToString(), value), ref dbtran);
                }

                dbtran.Merge();

                //CNetManager.Instance.P2C_ReportItemDataList(sessionKey, dbtran.m_UpdateItemList);
                CNetManager.Instance.P2C_ReportAssetData(user.SessionKey, dbtran.m_UpdateCoinList);
                CNetManager.Instance.P2C_ReportUserData(user.SessionKey, user.UserData);

                CDBManager.Instance.QueryCharacterUpdateItemCountList(user.DBGUID,
                    user.SessionKey, user.UserData.m_UID, dbtran);

                for (int i = 0; i < dbtran.m_UpdateCoinList.Count; ++i)
                {
                    var coin = dbtran.m_UpdateCoinList[i];
                    CDBManager.Instance.QueryCharacterUpdateCoin(user.DBGUID, user.SessionKey, user.UserData.m_UID, coin.TableID, coin.Count);
                }

                CNetManager.Instance.P2C_ResultCheat(user.SessionKey, Packet_Result.Result.Success);
            });

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Gacha_Simulation(CUser user, List<string> args)
        {
            if (args.Count < 3)
                return Packet_Result.Result.CheatParamError;

            int idx = 0;
            int gachaid;
            if (!int.TryParse(args[idx++], out gachaid))
                return Packet_Result.Result.CheatParamError;

            int lv;
            if (!int.TryParse(args[idx++], out lv))
                return Packet_Result.Result.CheatParamError;

            int cnt;
            if (!int.TryParse(args[idx++], out cnt))
                return Packet_Result.Result.CheatParamError;

            Task.Run(() => GachaSimulation(gachaid, cnt, lv, user));

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Reset_Daily(CUser user, List<string> args)
        {
            if (user.QuestAgent.FindSystem<CQuestSystemEx_Daily>(CDefine.QuestType.Daily) is var systems)
            {
                user.CheatResetDate();

                foreach (var system in systems)
                {
                    system.Board.ExpTime = SDateManager.Instance.ProdTodayToUtc();
                    //system.Refresh();
                }

                return Packet_Result.Result.Success;
            }

            return Packet_Result.Result.CheatParamError;
        }

        private Packet_Result.Result Next_CheckIn(CUser user, List<string> args)
        {
            if (user.QuestAgent.FindSystem<CQuestSystemEx_CheckIn>(CDefine.QuestType.CheckIn) is var systems)
            {
                if (systems == null)
                    return Packet_Result.Result.CheatParamError;

                user.CheatResetDate();

                foreach (var system in systems)
                {
                    system.Board.ExpTime = SDateManager.Instance.ProdTodayToUtc();
                }

                return Packet_Result.Result.Success;
            }

            return Packet_Result.Result.CheatParamError;
        }

        private Packet_Result.Result Reset_Rank(CUser user, List<string> args)
        {
            user.CheatResetDate();
            user.CheakRankReset();
            //user.RefreshRankReward(true);
            
            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Update_Misison(CUser user, List<string> args)
        {
            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int id = 0;
            int idx = 0;
            if (!int.TryParse(args[idx++], out id))
                return Packet_Result.Result.CheatParamError;

            long value = 0;
            if (!long.TryParse(args[idx++], out value))
                return Packet_Result.Result.CheatParamError;

            user.QuestAgent.CheakMissionUpdate(id, value);
            
            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Reset_Pass(CUser user, List<string> args)
        {
            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int id = 0;
            int idx = 0;
            if (!int.TryParse(args[idx++], out id))
                return Packet_Result.Result.CheatParamError;

            long value = 0;
            if (!long.TryParse(args[idx++], out value))
                return Packet_Result.Result.CheatParamError;

            user.QuestAgent.CheakMissionUpdate(id, value);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Update_Mission(CUser user, List<string> args)
        {
            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int id = 0;
            int idx = 0;
            if (!int.TryParse(args[idx++], out id))
                return Packet_Result.Result.CheatParamError;

            long value = 0;
            if (!long.TryParse(args[idx++], out value))
                return Packet_Result.Result.CheatParamError;

            user.QuestAgent.CheakMissionUpdate(id, value);

            return Packet_Result.Result.Success;
        }


        private Packet_Result.Result Pass_Active(CUser user, List<string> args)
        {
            if (args.Count < 1)
                return Packet_Result.Result.CheatParamError;

            user.QuestAgent.PassActiveOn(args[0]);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Set_Gacha_Level(CUser user, List<string> args)
        {
            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int idx = 0;
            int groupID = 0;
            if (!int.TryParse(args[idx++], out groupID))
                return Packet_Result.Result.CheatParamError;

            int level = 0;
            if (!int.TryParse(args[idx++], out level))
                return Packet_Result.Result.CheatParamError;

            var gachaLv = user.FindGacha(groupID);
            if (gachaLv == null)
                return Packet_Result.Result.CheatParamError;

            gachaLv.m_Level = level;

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Add_Mission(CUser user, List<string> args)
        {
            if (args.Count < 1)
                return Packet_Result.Result.CheatParamError;

            int id = 0;
            if (!int.TryParse(args[0], out id))
                return Packet_Result.Result.CheatParamError;

            MissionRecord missionRecord = MissionTable.Instance.Find(id);
            if (missionRecord == null)
                return Packet_Result.Result.CheatParamError;

            QuestRecord questRecord = QuestTable.Instance.Find(missionRecord.Quest_Id);
            if (questRecord == null)
                return Packet_Result.Result.CheatParamError;

            if (questRecord.Quest_Type == CDefine.QuestType.Main)
            {
                var questSystem = user.QuestAgent.FindSystem(questRecord.Quest_Type, questRecord.ID);
                if (questSystem == null)
                    return Packet_Result.Result.CheatParamError;

                var mission = questSystem.Board.Find(id);
                if (mission == null)
                    return Packet_Result.Result.CheatParamError;

                CNetManager.Instance.P2C_ReportQuestMissionData(user.SessionKey, CDefine.Modify.Remove, questRecord.ID, SCopy<_Mission>.DeepCopy(mission));

                mission.Val = 0;
                mission.State = CDefine.MissionState.Processing;
                mission.Modifyed = true;

                CNetManager.Instance.P2C_ReportQuestMissionData(user.SessionKey, CDefine.Modify.Add, questRecord.ID, SCopy<_Mission>.DeepCopy(mission));
            }
            else
            {
                var questSystem = user.QuestAgent.FindSystem(questRecord.Quest_Type, questRecord.ID);
                if (questSystem == null)
                    return Packet_Result.Result.CheatParamError;

                var mission = questSystem.Board.Find(id);
                if (mission == null)
                    return Packet_Result.Result.CheatParamError;

                mission.Val = 0;
                mission.State = CDefine.MissionState.Processing;
                mission.Modifyed = true;

                CNetManager.Instance.P2C_ReportQuestMissionData(user.SessionKey, CDefine.Modify.Update, questRecord.ID, SCopy<_Mission>.DeepCopy(mission));
            }

            CNetManager.Instance.P2C_ResultCheat(user.SessionKey, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Set_Mission(CUser user, List<string> args)
        {
            if (args.Count < 3)
                return Packet_Result.Result.CheatParamError;

            int idx = 0;
            string quest_id = args[idx++];

            int missionid = 0;
            if (!int.TryParse(args[idx++], out missionid))
                return Packet_Result.Result.CheatParamError;

            CDefine.MissionState state = CDefine.MissionState.Max;
            if (!Enum.TryParse(args[idx++], out state))
                return Packet_Result.Result.CheatParamError;


            if (quest_id == "-1")
            {
                var list = user.QuestAgent.GetList();
                foreach (var board in list)
                {
                    List<_Mission> sendMissons = new List<_Mission>();

                    foreach (var misison in board.Missions)
                    {
                        var missionRecord = MissionTable.Instance.Find(misison.ID);
                        if (missionRecord == null)
                            return Packet_Result.Result.CheatParamError;

                        var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                        if (condRecord == null)
                            return Packet_Result.Result.CheatParamError;

                        switch (state)
                        {
                            case CDefine.MissionState.Processing:
                                misison.Val = 0;
                                break;
                            case CDefine.MissionState.Rewarded:
                            case CDefine.MissionState.Complete:
                                misison.Val = condRecord.Value;
                                break;
                        }

                        sendMissons.Add(misison);
                    }

                    CNetManager.Instance.P2C_ReportQuestMissionDataList(user.SessionKey, board.ID, CDefine.Modify.Update, sendMissons);
                }
            }
            else
            {
                List<_Mission> sendMissons = new List<_Mission>();

                QuestRecord questRecord = QuestTable.Instance.Find(quest_id);
                if (questRecord == null)
                    return Packet_Result.Result.CheatParamError;

                var questSystem = user.QuestAgent.FindSystem(questRecord.Quest_Type, questRecord.ID);
                if (questSystem == null)
                    return Packet_Result.Result.CheatParamError;

                if (missionid == -1)
                {
                    foreach (var mission in questSystem.Board.Missions)
                    {
                        var findmission = questSystem.Board.Find(missionid);
                        if (findmission == null)
                            return Packet_Result.Result.CheatParamError;

                        var missionRecord = MissionTable.Instance.Find(findmission.ID);
                        if (missionRecord == null)
                            return Packet_Result.Result.CheatParamError;

                        var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                        if (condRecord == null)
                            return Packet_Result.Result.CheatParamError;

                        switch (state)
                        {
                            case CDefine.MissionState.Processing:
                                findmission.Val = 0;
                                break;
                            case CDefine.MissionState.Rewarded:
                            case CDefine.MissionState.Complete:
                                findmission.Val = condRecord.Value;
                                break;
                        }

                        sendMissons.Add(findmission);
                    }
                }
                else
                {
                    var findmission = questSystem.Board.Find(missionid);
                    if (findmission == null)
                        return Packet_Result.Result.CheatParamError;

                    var missionRecord = MissionTable.Instance.Find(findmission.ID);
                    if (missionRecord == null)
                        return Packet_Result.Result.CheatParamError;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        return Packet_Result.Result.CheatParamError;

                    switch (state)
                    {
                        case CDefine.MissionState.Processing:
                            findmission.Val = 0;
                            break;
                        case CDefine.MissionState.Rewarded:
                        case CDefine.MissionState.Complete:
                            findmission.Val = condRecord.Value;
                            break;
                    }

                    sendMissons.Add(findmission);
                }

                sendMissons.ForEach(x => { x.Modifyed = true; });
                CNetManager.Instance.P2C_ReportQuestMissionDataList(user.SessionKey, questSystem.Board.ID, CDefine.Modify.Update, sendMissons);
            }

            CNetManager.Instance.P2C_ResultCheat(user.SessionKey, Packet_Result.Result.Success);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Set_AD_Buff_Lv(CUser user, List<string> args)
        {

            if (args.Count < 1)
                return Packet_Result.Result.CheatParamError;

            int lv = 0;
            if (!int.TryParse(args[0], out lv))
                return Packet_Result.Result.CheatParamError;

            user.BuffAgent.CheatBuffLv(lv);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Add_AD_Buff_Time(CUser user, List<string> args)
        {

            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int idx = 0;
            int buffid = 0;
            if (!int.TryParse(args[idx++], out buffid))
                return Packet_Result.Result.CheatParamError;

            long time = 0;
            if (!long.TryParse(args[idx++], out time))
                return Packet_Result.Result.CheatParamError;

            user.BuffAgent.CheatBuffTime(buffid, time);
            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Set_Event_Time(CUser user, List<string> args)
        {

            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int idx = 0;
            int event_id = 0;
            if (!int.TryParse(args[idx++], out event_id))
                return Packet_Result.Result.CheatParamError;

            long time = 0;
            if (!long.TryParse(args[idx++], out time))
                return Packet_Result.Result.CheatParamError;

            user.EventAgent.Cheat_SetEventTime(event_id, time);
            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result EventRouletteReward(CUser user, List<string> args)
        {
            if (args.Count < 1)
                return Packet_Result.Result.CheatParamError;

            long eventuid;
            if (!long.TryParse(args[0], out eventuid))
                return Packet_Result.Result.CheatParamError;

            user.EventAgent.ReqEventRouletteReward(user.SessionKey, eventuid);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result Stage_Swep(CUser user, List<string> args)
        {
            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int stageid = 0;
            if (!int.TryParse(args[0], out stageid))
                return Packet_Result.Result.CheatParamError;

            int cnt = 0;
            if (!int.TryParse(args[1], out cnt))
                return Packet_Result.Result.CheatParamError;

            var record = StageTable.Instance.Find(stageid);
            if (record == null)
                return Packet_Result.Result.CheatParamError;


            user.StageAgent.ReqStageSweep(user.SessionKey, record.Type, stageid, cnt);

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ShopBuy(CUser user, List<string> args)
        {
            if (args.Count < 2)
                return Packet_Result.Result.CheatParamError;

            int idx = 0;
            int shopid = 0;
            if (!int.TryParse(args[idx++], out shopid))
                return Packet_Result.Result.CheatParamError;

            int cnt = 0;
            if (!int.TryParse(args[idx++], out cnt))
                return Packet_Result.Result.CheatParamError;


            user.ShopAgent.ReqShopBuy(user.SessionKey, shopid, cnt);

            return Packet_Result.Result.Success;
        }

        private void GachaSimulation(int gachaid, int cnt, int lv, CUser user)
        {
            if (cnt < 1)
                return;

            if (lv < 1)
                return;

            var gachaData = user.FindGacha(gachaid);
            if (gachaData == null)
                return;
           
            var gachaLvRecord = GachaLvTable.Instance.Find(gachaid, lv);
            if (gachaLvRecord == null)
                return;
            
            //int total_cnt
            int bonus_cnt = (gachaLvRecord.GachaBonus_CountNum == 0) ? 0 : cnt / gachaLvRecord.GachaBonus_CountNum;
            bonus_cnt = bonus_cnt * gachaLvRecord.GachaBonus_Num;
            cnt += bonus_cnt;

            gachaData.m_Exp += cnt;
            
            List<_AssetData> gacharewards = GachaProbTable.Instance.Roulette(gachaData.m_GroupID, gachaData.m_Level, cnt);
            if (gacharewards == null)
                return;
            
            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            
            forClient.Insert(gacharewards);
            int resultCnt = dbtran.m_UpdateItemList.Count;
            dbtran.Merge();

            SortedList<int, GachaSimuData> sortlist = new SortedList<int, GachaSimuData>();
            foreach(var iter in dbtran.m_UpdateItemList)
            {
                int tid = int.Parse(iter.TableID);
                sortlist.Add(tid ,new GachaSimuData(tid, iter.Count));
            }

            CLogger.Instance.User("================Gacha Simulation===============");
            CLogger.Instance.User($"========Gacha Result Count : {resultCnt}=======");
            foreach (var it in sortlist)
                CLogger.Instance.User($"{it.Value.TableID} : {it.Value.Cnt}");

            CLogger.Instance.User("===============================================");
        }
    }
}
