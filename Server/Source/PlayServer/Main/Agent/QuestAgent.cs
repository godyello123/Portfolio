using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Global;
using Packet_C2P;
using SCommon;

namespace PlayServer
{
    //===================== BASE ===========================
    public class CQuestSystemEx_Base
    {
        protected CUser m_Owner;
        protected _QuestBoard m_Board = new _QuestBoard();
        protected Dictionary<string, List<_Mission>> m_Finder = new Dictionary<string, List<_Mission>>();

        public CQuestSystemEx_Base() { }
        public CQuestSystemEx_Base(CUser owner, _QuestBoard board)
        {
            m_Owner = owner;
            SetBoard(board);
            RefreshFinder();
        }

        public void SetBoard(_QuestBoard board)
        {
            m_Board = board;
        }

        public _QuestBoard Board { get => m_Board; }

        protected bool IsProcessing(_Mission mission)
        {
            if (mission.State == CDefine.MissionState.Processing)
                return true;

            if (m_Board.Type == CDefine.QuestType.Repeat)
                return true;

            return false;
        }

        public List<_Mission> Find(string condKey)
        {
            if (m_Finder.TryGetValue(condKey, out var ret))
                return ret;

            return null;
        }

        public void Update(string key, string target, long updateValue)
        {
            var foundMissions = Find(key);
            if (foundMissions == null)
                return;

            foreach (var mission in foundMissions)
            {
                if (!IsProcessing(mission))
                    continue;

                var mRecord = MissionTable.Instance.Find(mission.ID);
                if (mRecord == null)
                    continue;

                var condRecord = ConditionTable.Instance.Find(mRecord.ConditionID);
                if (condRecord == null)
                    continue;

                if (!condRecord.IsValidTarget(target))
                    continue;

                if (condRecord.IsSwapValue())
                    mission.Val = updateValue;
                else
                    mission.Val += updateValue;

                if (IsComplete(mission, mRecord))
                    Complete(mission);
                else
                    Excute(mission);
            }
        }

        public virtual void RefreshFinder()
        {
            m_Finder.Clear();

            foreach (var mission in m_Board.Missions)
            {
                var mRecord = MissionTable.Instance.Find(mission.ID);
                if (mRecord == null)
                    continue;

                var cond = ConditionTable.Instance.Find(mRecord.ConditionID);
                if (cond == null)
                    continue;

                if (m_Finder.ContainsKey(cond.Key()))
                    m_Finder[cond.Key()].Add(mission);
                else
                    m_Finder[cond.Key()] = new List<_Mission> { mission };
            }
        }

        public virtual bool IsComplete(_Mission mission, MissionRecord record)
        {
            if (mission.State != CDefine.MissionState.Processing)
                return false;

            var condRecord = ConditionTable.Instance.Find(record.ConditionID);
            if (condRecord == null)
                return false;

            mission.Val = Math.Min(mission.Val, condRecord.Value);

            return mission.Val >= condRecord.Value;
        }

        public virtual List<_Mission> GetModifyList()
        {
            var retList = new List<_Mission>();
            foreach (var iter in m_Board.Missions)
            {
                if (iter.Modifyed)
                    retList.Add(iter);
            }

            return retList;
        }

        public void ResetModify()
        {
            m_Board.Missions.ForEach(x => { x.Modifyed = false; });
        }

        public virtual void Refresh(bool bSend)
        {
            CheckCompletion();
            CheckNextMission(bSend);
            RefreshFinder();
            CheckSelfCheckable();
        }

        protected virtual void Excute(_Mission mission) { }
        protected virtual void Complete(_Mission mission) { }
        protected virtual bool CheckNextMission(bool bSend) { return true; }
        protected virtual void CheckCompletion() {}
        protected virtual void CheckSelfCheckable() {}
    }

    //===================== MAIN ===========================

    public class CQuestSystemEx_Main : CQuestSystemEx_Base
    {
        public CQuestSystemEx_Main() { }
        public CQuestSystemEx_Main(CUser owner, _QuestBoard board) : base(owner, board) { }

        protected override void Excute(_Mission mission)
        {
            mission.Modifyed = true;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        protected override void Complete(_Mission mission)
        {
            mission.Modifyed = true;
            mission.State = mission.State == CDefine.MissionState.Processing ? CDefine.MissionState.Complete : mission.State;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        public override void Refresh(bool bSend)
        {
            CheckCompletion();
            CheckNextMission(bSend);
            RefreshFinder();
            CheckSelfCheckable();
        }

        protected override bool CheckNextMission(bool bSend)
        {
            var first = m_Finder.FirstOrDefault();
            var missionEntry = first.Value;
            if (missionEntry == null)
                return false;

            var mission = missionEntry.First();
            if (mission == null)
                return false;

            if (mission.State != CDefine.MissionState.Rewarded)
                return false;

            var missionRecord = MissionTable.Instance.Find(mission.ID);
            if (missionRecord == null)
                return false;

            var nextRecord = MissionTable.Instance.Find(missionRecord.Next);
            if (nextRecord != null)
            {
                if(bSend)
                    CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Remove, Board.ID, SCopy<_Mission>.DeepCopy(mission));

                mission.ID = nextRecord.ID;
                mission.State = CDefine.MissionState.Processing;
                mission.Val = 0;
                mission.Modifyed = true;

                if(bSend)
                    CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Add, Board.ID, SCopy<_Mission>.DeepCopy(mission));

                return true;
            }

            if(bSend)
                CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, SCopy<_Mission>.DeepCopy(mission));

            return false;
        }

        protected override void CheckCompletion()
        {
            foreach (var iter in m_Finder)
            {
                var list = iter.Value;
                list.ForEach(mission =>
                {
                    var prevState = mission.State;
                    if (mission.State == CDefine.MissionState.Processing)
                    {
                        var missionRecord = MissionTable.Instance.Find(mission.ID);
                        if (missionRecord != null)
                        {
                            var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                            if (condRecord != null)
                            {
                                mission.State = mission.Val >= condRecord.Value ? CDefine.MissionState.Complete : mission.State;

                                if (prevState != mission.State)
                                    mission.Modifyed = true;
                            }
                        }
                    }
                });
            }
        }
        protected override void CheckSelfCheckable()
        {
            foreach (var iter in m_Finder)
            {
                var list = iter.Value;
                foreach (var mission in list)
                {
                    if (!IsProcessing(mission))
                        continue;

                    var missionRecord = MissionTable.Instance.Find(mission.ID);
                    if (missionRecord == null)
                        continue;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        continue;

                    mission.Val = Math.Min(m_Owner.CondValue(missionRecord.ConditionID), condRecord.Value);
                    mission.State = mission.Val >= condRecord.Value ? CDefine.MissionState.Complete : mission.State;
                    mission.Modifyed = true;

                    CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
                }
            }
        }
    }

    //===================== REPEAT ===========================
    public class CQuestSystemEx_Repeat : CQuestSystemEx_Base
    {
        public CQuestSystemEx_Repeat() { }
        public CQuestSystemEx_Repeat(CUser owner, _QuestBoard board) : base(owner, board) { }

        protected override void Excute(_Mission mission)
        {
            mission.Modifyed = true;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        protected override void Complete(_Mission mission)
        {
            mission.Modifyed = true;
            mission.State = mission.State == CDefine.MissionState.Processing ? CDefine.MissionState.Complete : mission.State;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        public override void Refresh(bool bSend)
        {
            CheckCompletion();
            CheckNextMission(bSend);
            RefreshFinder();
            CheckSelfCheckable();
        }

        protected override bool CheckNextMission(bool bSend) { return true; }

        protected override void CheckSelfCheckable() { }
    }

    //===================== DAILY ===========================

    public class CQuestSystemEx_Daily : CQuestSystemEx_Base
    {
        public CQuestSystemEx_Daily() { }
        public CQuestSystemEx_Daily(CUser owner, _QuestBoard board) : base(owner, board) { }

        protected override void Excute(_Mission mission)
        {
            mission.Modifyed = true;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        protected override void Complete(_Mission mission)
        {
            mission.Modifyed = true;
            mission.State = mission.State == CDefine.MissionState.Processing ? CDefine.MissionState.Complete : mission.State;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        public override void Refresh(bool bSend)
        {
            if (SDateManager.Instance.IsExpired(m_Board.ExpTime))
            {
                m_Board.Missions.ForEach(x =>
                {
                    x.State = CDefine.MissionState.Processing;
                    x.Val = 0;
                    x.Modifyed = true;
                });

                m_Board.ExpTime = SDateManager.Instance.ProdTomorrowToUtc();

                if (bSend)
                    CNetManager.Instance.P2C_ReportQuestMissionDataList(m_Owner.SessionKey, m_Board.ID, CDefine.Modify.Update, m_Board.Missions);
            }

            CheckCompletion();
            RefreshFinder();
        }

        protected override bool CheckNextMission(bool bSend) { return true; }

        protected override void CheckSelfCheckable() { }

        protected override void CheckCompletion()
        {
            foreach (var iter in m_Finder)
            {
                var list = iter.Value;
                list.ForEach(mission =>
                {
                    var prevState = mission.State;
                    if (mission.State == CDefine.MissionState.Processing)
                    {
                        var missionRecord = MissionTable.Instance.Find(mission.ID);
                        if (missionRecord != null)
                        {
                            var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                            if (condRecord != null)
                            {
                                mission.State = mission.Val >= condRecord.Value ? CDefine.MissionState.Complete : mission.State;

                                if (prevState != mission.State)
                                    mission.Modifyed = true;
                            }
                        }
                    }
                });
            }
        }
    }

    //===================== CHECK_IN ===========================
    public class CQuestSystemEx_CheckIn : CQuestSystemEx_Base
    {
        public CQuestSystemEx_CheckIn() { }
        public CQuestSystemEx_CheckIn(CUser owner, _QuestBoard board) : base(owner, board) { }

        protected override void Excute(_Mission mission)
        {
            mission.Modifyed = true;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        protected override void Complete(_Mission mission)
        {
            mission.Modifyed = true;
            mission.State = mission.State == CDefine.MissionState.Processing ? CDefine.MissionState.Complete : mission.State;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        public override void Refresh(bool bSend)
        {
            if (SDateManager.Instance.IsExpired(m_Board.ExpTime))
            {
                var first = m_Finder.FirstOrDefault();
                var missionEntry = first.Value;
                if (missionEntry == null)
                    return;

                var mission = missionEntry.First();
                if (mission == null)
                    return;

                //if (mission.State != CDefine.MissionState.Rewarded)
                //    return;

                var missionRecord = MissionTable.Instance.Find(mission.ID);
                if (missionRecord == null)
                    return;

                var nextRecord = MissionTable.Instance.Find(missionRecord.Next);
                if (nextRecord == null)
                {
                    nextRecord = MissionTable.Instance.CopyFirstMission(Board.Type, Board.ID);
                    if (nextRecord == null)
                        return;
                }

                if (bSend)
                    CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Remove, Board.ID, SCopy<_Mission>.DeepCopy(mission));

                mission.ID = nextRecord.ID;
                mission.State = CDefine.MissionState.Processing;
                mission.Val = 0;
                mission.Modifyed = true;

                m_Board.ExpTime = SDateManager.Instance.ProdTomorrowToUtc();

                if (bSend)
                    CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Add, Board.ID, SCopy<_Mission>.DeepCopy(mission));
            }

            CheckNextMission(bSend);
            RefreshFinder();
        }

        protected override void CheckCompletion()
        {
            foreach (var iter in m_Finder)
            {
                var list = iter.Value;
                list.ForEach(mission =>
                {
                    var prevState = mission.State;
                    if (mission.State == CDefine.MissionState.Processing)
                    {
                        var missionRecord = MissionTable.Instance.Find(mission.ID);
                        if (missionRecord != null)
                        {
                            var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                            if (condRecord != null)
                            {
                                mission.State = mission.Val >= condRecord.Value ? CDefine.MissionState.Complete : mission.State;

                                if (prevState != mission.State)
                                    mission.Modifyed = true;
                            }
                        }
                    }
                });
            }
        }

        protected override bool CheckNextMission(bool bSend)
        {
            return true;
        }

        protected override void CheckSelfCheckable()
        {
        }
    }

    //===================== PASS ===========================
    public class CQuestSystemEx_Pass : CQuestSystemEx_Base
    {
        public CQuestSystemEx_Pass() { }
        public CQuestSystemEx_Pass(CUser owner, _QuestBoard board) : base(owner, board) { }

        protected override void Excute(_Mission mission)
        {
            mission.Modifyed = true;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        protected override void Complete(_Mission mission)
        {
            mission.Modifyed = true;
            mission.State = mission.State == CDefine.MissionState.Processing ? CDefine.MissionState.Complete : mission.State;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        public override void Refresh(bool bSend)
        {
            CheckCompletion();
            CheckNextMission(bSend);
            RefreshFinder();
            CheckSelfCheckable();
        }

        protected override bool CheckNextMission(bool bSend)
        {
            return true;
        }

        protected override void CheckCompletion()
        {
            foreach (var iter in m_Finder)
            {
                var list = iter.Value;
                list.ForEach(mission =>
                {
                    var prevState = mission.State;
                    if (mission.State == CDefine.MissionState.Processing)
                    {
                        var missionRecord = MissionTable.Instance.Find(mission.ID);
                        if (missionRecord != null)
                        {
                            var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                            if (condRecord != null)
                            {
                                mission.State = mission.Val >= condRecord.Value ? CDefine.MissionState.Complete : mission.State;

                                if (prevState != mission.State)
                                    mission.Modifyed = true;
                            }
                        }
                    }
                });
            }
        }
        protected override void CheckSelfCheckable()
        {
            foreach (var iter in m_Finder)
            {
                var list = iter.Value;
                foreach (var mission in list)
                {
                    if (!IsProcessing(mission))
                        continue;

                    var missionRecord = MissionTable.Instance.Find(mission.ID);
                    if (missionRecord == null)
                        continue;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        continue;

                    if (condRecord.IsSelfCheckable())
                    {
                        mission.Val = Math.Min(m_Owner.CondValue(missionRecord.ConditionID), condRecord.Value);
                        mission.State = mission.Val >= condRecord.Value ? CDefine.MissionState.Complete : mission.State;
                    }

                    mission.Modifyed = true;
                }
            }
        }
    }

    public class CQuestSystemEx_Event : CQuestSystemEx_Base
    {
        public CQuestSystemEx_Event() { }
        public CQuestSystemEx_Event(CUser owner, _QuestBoard board) : base(owner, board) { }

        protected override void Excute(_Mission mission)
        {
            mission.Modifyed = true;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        protected override void Complete(_Mission mission)
        {
            mission.Modifyed = true;
            mission.State = mission.State == CDefine.MissionState.Processing ? CDefine.MissionState.Complete : mission.State;
            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, Board.ID, mission);
        }

        public override void Refresh(bool bSend)
        {
            CheckCompletion();
            CheckNextMission(bSend);
            RefreshFinder();
            CheckSelfCheckable();
        }

        protected override bool CheckNextMission(bool bSend)
        {
            return true;
        }

        protected override void CheckCompletion()
        {
            foreach (var iter in m_Finder)
            {
                var list = iter.Value;
                list.ForEach(mission =>
                {
                    var prevState = mission.State;
                    if (mission.State == CDefine.MissionState.Processing)
                    {
                        var missionRecord = MissionTable.Instance.Find(mission.ID);
                        if (missionRecord != null)
                        {
                            var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                            if (condRecord != null)
                            {
                                mission.State = mission.Val >= condRecord.Value ? CDefine.MissionState.Complete : mission.State;

                                if (prevState != mission.State)
                                    mission.Modifyed = true;
                            }
                        }
                    }
                });
            }
        }
        protected override void CheckSelfCheckable()
        {
            foreach (var iter in m_Finder)
            {
                var list = iter.Value;
                foreach (var mission in list)
                {
                    if (!IsProcessing(mission))
                        continue;

                    var missionRecord = MissionTable.Instance.Find(mission.ID);
                    if (missionRecord == null)
                        continue;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        continue;

                    if(condRecord.IsSelfCheckable())
                    {
                        mission.Val = Math.Min(m_Owner.CondValue(missionRecord.ConditionID), condRecord.Value);
                        mission.State = mission.Val >= condRecord.Value ? CDefine.MissionState.Complete : mission.State;
                    }

                    mission.Modifyed = true;
                }
            }
        }
    }


    public class CQuestAgent
    {
        private CUser m_Owner;
        private STimer m_Timer = new STimer(60 * 1000);

        public CQuestAgent(CUser user) { m_Owner = user; }
        public CUser Owner { get => m_Owner; }


        private Dictionary<KeyValuePair<CDefine.QuestType, string>, CQuestSystemEx_Base> m_QuestSystem = new Dictionary<KeyValuePair<CDefine.QuestType, string>, CQuestSystemEx_Base>();

        public CQuestSystemEx_Base FindSystem(CDefine.QuestType type, string questID)
        {
            var key = new KeyValuePair<CDefine.QuestType, string>(type, questID);
            if (m_QuestSystem.TryGetValue(key, out var retVal))
                return retVal;

            return null;
        }

        public List<T> FindSystem<T>(CDefine.QuestType type) where T : CQuestSystemEx_Base
        {
            var retList = new List<T>();
            var findIt = m_QuestSystem.Where(x => x.Value.Board.Type == type && x.Value is T);
            foreach (var iter in findIt)
                retList.Add(iter.Value as T);

            return retList;
        }

        public void Init(_QuestBoard mainboard, _QuestBoard repeatBoard, _QuestBoard dailyboard, Dictionary<string, _QuestBoard> checkIns,
            Dictionary<string, _QuestBoard> pass, Dictionary<string, _QuestBoard> eventboards)
        {
            m_QuestSystem[new KeyValuePair<CDefine.QuestType, string>(mainboard.Type, mainboard.ID)] = new CQuestSystemEx_Main(m_Owner, mainboard);
            m_QuestSystem[new KeyValuePair<CDefine.QuestType, string>(repeatBoard.Type, repeatBoard.ID)] = new CQuestSystemEx_Repeat(m_Owner, repeatBoard);
            m_QuestSystem[new KeyValuePair<CDefine.QuestType, string>(dailyboard.Type, dailyboard.ID)] = new CQuestSystemEx_Daily(m_Owner, dailyboard);
            
            foreach(var iter in checkIns)
            {
                var board = iter.Value;
                m_QuestSystem[new KeyValuePair<CDefine.QuestType, string>(board.Type, board.ID)] = new CQuestSystemEx_CheckIn(m_Owner, board);
            }

            foreach(var iter in pass)
            {
                var board = iter.Value;
                m_QuestSystem[new KeyValuePair<CDefine.QuestType, string>(board.Type, board.ID)] = new CQuestSystemEx_Pass(m_Owner, board);
            }

            foreach(var iter in eventboards)
            {
                var board = iter.Value;



                m_QuestSystem[new KeyValuePair<CDefine.QuestType, string>(board.Type, board.ID)] = new CQuestSystemEx_Event(m_Owner, board);
            }

            QuestSystemRefresh(false);
        }

        public void Update()
        {
            if (!m_Timer.Check())
                return;

            UpdateQuestBorad(CDefine.MissionCondition.Play_Time, "", "", 1);
        }

        private void QuestSystemRefresh(bool bSend)
        {
            foreach (var iter in m_QuestSystem)
            {
                var system = iter.Value;
                system.Refresh(bSend);
            }
        }

        public void RefreshDaily()
        {
            if (FindSystem<CQuestSystemEx_Daily>(CDefine.QuestType.Daily) is var daiiyQuests)
            {
                foreach (var iter in daiiyQuests)
                    iter.Refresh(true);
            }

            if(FindSystem<CQuestSystemEx_CheckIn>(CDefine.QuestType.CheckIn) is var checkinQuests)
            {
                foreach (var iter in checkinQuests)
                    iter.Refresh(true);
            }
        }


        public List<_QuestBoard> GetList()
        {
            var retList = new List<_QuestBoard>();
            foreach (var iter in m_QuestSystem)
            {
                var system = iter.Value;
                retList.Add(system.Board);
            }

            return retList;
        }

        public void UpdateQuestBorad(CDefine.MissionCondition conditionType, string param, string target, long updateValue)
        {
            foreach (var iter in m_QuestSystem)
            {
                var system = iter.Value;
                system.Update(ConditionRecord.Key(conditionType, param), target, updateValue);
            }
        }

        public Packet_Result.Result ReqQuestMissionReward(long sessionKey, string questID, int missionID)
        {
            var missionRecord = MissionTable.Instance.Find(missionID);
            if(missionRecord == null)
            {
                CLogger.Instance.System($"missionRecord missionRecord == null : {missionID}");
                return Packet_Result.Result.InValidRecord;
            }

            var questRecord = QuestTable.Instance.Find(missionRecord.Quest_Id);
            if(questRecord == null)
            {
                CLogger.Instance.System($"missionRecord questRecord == null : {missionRecord.Quest_Id}");
                return Packet_Result.Result.InValidRecord;
            }

            switch (questRecord.Quest_Type)
            {
                case CDefine.QuestType.Main:
                    return ReqMainMissionReward(sessionKey, questRecord.ID, missionID);
                case CDefine.QuestType.Repeat:
                    return ReqRepeatMissionReward(sessionKey, questRecord.ID, missionID);
                case CDefine.QuestType.Daily:
                    return ReqDailyMissionReward(sessionKey, questRecord.ID, missionID);
                case CDefine.QuestType.CheckIn:
                    return ReqCheckInMissionReward(sessionKey, questRecord.ID, missionID);
                case CDefine.QuestType.Pass:
                //return ReqPassMissionReward(sessionKey, questRecord.ID, missionID);
                case CDefine.QuestType.Event:
                    return ReqEventMissionReward(sessionKey, questRecord.ID, missionID);
                case CDefine.QuestType.Max:
                    break;
                default:
                    break;
            }

            return Packet_Result.Result.PacketError;
        }

        private Packet_Result.Result ReqMainMissionReward(long sessionKey, string questID, int missionID)
        {
            if (FindSystem(CDefine.QuestType.Main, questID) is CQuestSystemEx_Main questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward questType : {CDefine.QuestType.Main}");
                    return Packet_Result.Result.PacketError;
                }

                var mission = questSystem.Board.Find(missionID);
                if (mission == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward misison ID : {missionID}");
                    return Packet_Result.Result.PacketError;
                }

                if (mission.State != CDefine.MissionState.Complete)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward misson.State : {mission.State}");
                    return Packet_Result.Result.PacketError;
                }

                var missionRecord = MissionTable.Instance.Find(mission.ID);
                if (missionRecord == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward missionRecord : {mission.ID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var rewards = RewardTable.Instance.Find(missionRecord.Reward_Id);
                if (rewards == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward rewards NULL : {missionRecord.Reward_Id}");
                    return Packet_Result.Result.PacketError;
                }

                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();

                foreach (var iter in rewards)
                {
                    var reward = iter.Value;
                    forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                }

                forClient.UpdateRewardData(m_Owner, ref dbtran, true);
                dbtran.Merge();

                
                mission.State = CDefine.MissionState.Rewarded;
                mission.Modifyed = false;

                CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
                CNetManager.Instance.P2C_ResultMissionReward(sessionKey, questSystem.Board.ID , mission, forClient.GetList(), Packet_Result.Result.Success);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.quest_main, m_Owner.UserData, LogHelper.ToJson(mission), dbtran, 1);
                CGameLog.Instance.Insert(log);

                questSystem.Refresh(true);

                CDBManager.Instance.QueryCharacterQuestMainReward(m_Owner.DBGUID, sessionKey, m_Owner.UID, missionRecord.ID, SCopy<_Mission>.DeepCopy(mission), forClient, dbtran);

                return Packet_Result.Result.Success;
            }

            CLogger.Instance.Debug($"ReqQuestMissionReward");
            return Packet_Result.Result.PacketError;
        }

        private Packet_Result.Result ReqRepeatMissionReward(long sessionKey, string questID, int missionID)
        {
            if (FindSystem(CDefine.QuestType.Repeat, questID) is CQuestSystemEx_Repeat questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward Borad : {CDefine.QuestType.Repeat}");
                    return Packet_Result.Result.InValidData;
                }

                var mission = questSystem.Board.Find(missionID);
                if (mission == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward mission : {missionID}");
                    return Packet_Result.Result.InValidData;
                }

                if (mission.State != CDefine.MissionState.Complete)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward misson.State : {mission.State}");
                    return Packet_Result.Result.PacketError;
                }

                var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                if (curMissionRecord == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward missionRecord : {mission.ID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                if (condRecord == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward condRecord : {curMissionRecord.ConditionID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var clearCnt = mission.Val / condRecord.Value;
                if (clearCnt < 1)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward clearCnt : {clearCnt}");
                    return Packet_Result.Result.PacketError;
                }

                var rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
                if (rewards == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward rewards : {curMissionRecord.Reward_Id}");
                    return Packet_Result.Result.PacketError;
                }

                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();
                foreach (var iter in rewards)
                {
                    var reward = iter.Value;
                    forClient.Insert(reward.AssetType, reward.RewardID, reward.Value * clearCnt);
                }

                forClient.UpdateRewardData(m_Owner, ref dbtran, true);
                dbtran.Merge();

                mission.State = CDefine.MissionState.Processing;
                mission.Val = mission.Val % condRecord.Value;
                mission.Modifyed = false;

                var saveList = new List<_Mission>
                {
                    SCopy<_Mission>.DeepCopy(mission)
                };

                CDBManager.Instance.QueryCharacterQuestRepeatReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, saveList, forClient, dbtran);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.quest_repeat, m_Owner.UserData, LogHelper.ToJson(mission), dbtran, 1);
                CGameLog.Instance.Insert(log);

                questSystem.Refresh(true);

                CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
                CNetManager.Instance.P2C_ResultMissionReward(sessionKey, questSystem.Board.ID, mission, forClient.GetList(), Packet_Result.Result.Success);
                CNetManager.Instance.P2C_ReportQuestMissionData(sessionKey, CDefine.Modify.Update, questSystem.Board.ID, SCopy<_Mission>.DeepCopy(mission));
                return Packet_Result.Result.Success;
            }

            CLogger.Instance.Debug($"ReqRepeatMissionReward");
            return Packet_Result.Result.PacketError;
        }

        private Packet_Result.Result ReqDailyMissionReward(long sessionKey, string questID, int missionID)
        {
            if (FindSystem(CDefine.QuestType.Daily, questID) is CQuestSystemEx_Daily questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward Borad : {CDefine.QuestType.Repeat}");
                    return Packet_Result.Result.InValidData;
                }

                var mission = questSystem.Board.Find(missionID);
                if (mission == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward mission : {missionID}");
                    return Packet_Result.Result.InValidData;
                }

                if (mission.State != CDefine.MissionState.Complete)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward misson.State : {mission.State}");
                    return Packet_Result.Result.PacketError;
                }

                var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                if (curMissionRecord == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward missionRecord : {mission.ID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                if (condRecord == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward condRecord : {curMissionRecord.ConditionID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
                if (rewards == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward rewards : {curMissionRecord.Reward_Id}");
                    return Packet_Result.Result.PacketError;
                }

                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();
                foreach (var iter in rewards)
                {
                    var reward = iter.Value;
                    forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                }

                forClient.UpdateRewardData(m_Owner, ref dbtran, true);
                dbtran.Merge();

                mission.State = CDefine.MissionState.Rewarded;
                mission.Modifyed = false;

                var saveList = new List<_Mission>();
                saveList.Add(SCopy<_Mission>.DeepCopy(mission));

                CDBManager.Instance.QueryCharacterQuestDailyReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, saveList, questSystem.Board.ExpTime, forClient, dbtran);

                UpdateQuestBorad(CDefine.MissionCondition.Mission_Rewarded, "", CDefine.QuestType.Daily.ToString(), saveList.Count);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.quest_daily, m_Owner.UserData, LogHelper.ToJson(mission), dbtran, 1);
                CGameLog.Instance.Insert(log);

                CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
                CNetManager.Instance.P2C_ResultMissionReward(sessionKey, questSystem.Board.ID, mission, forClient.GetList(), Packet_Result.Result.Success);
                CNetManager.Instance.P2C_ReportQuestMissionData(sessionKey, CDefine.Modify.Update, questSystem.Board.ID, SCopy<_Mission>.DeepCopy(mission));
                return Packet_Result.Result.Success;
            }

            CLogger.Instance.Debug($"ReqRepeatMissionReward");
            return Packet_Result.Result.PacketError;
        }

        private Packet_Result.Result ReqCheckInMissionReward(long sessionKey, string questID, int missionID)
        {
            if (FindSystem(CDefine.QuestType.CheckIn, questID) is CQuestSystemEx_CheckIn questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward Borad : {CDefine.QuestType.Repeat}");
                    return Packet_Result.Result.InValidData;
                }

                var mission = questSystem.Board.Find(missionID);
                if (mission == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward mission : {missionID}");
                    return Packet_Result.Result.InValidData;
                }

                if (mission.State != CDefine.MissionState.Complete)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward misson.State : {mission.State}");
                    return Packet_Result.Result.PacketError;
                }

                var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                if (curMissionRecord == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward missionRecord : {mission.ID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                if (condRecord == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward condRecord : {curMissionRecord.ConditionID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
                if (rewards == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward rewards : {curMissionRecord.Reward_Id}");
                    return Packet_Result.Result.PacketError;
                }

                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();
                foreach (var iter in rewards)
                {
                    var reward = iter.Value;
                    forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                }

                forClient.UpdateRewardData(m_Owner, ref dbtran, true);
                dbtran.Merge();

                //questSystem.Board.ExpTime = SDateManager.Instance.ProdTomorrowToUtc();
                mission.State = CDefine.MissionState.Rewarded;
                mission.Modifyed = false;

                CDBManager.Instance.QueryCharacterQuestCheckInReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, questSystem.Board.ID, mission, questSystem.Board.ExpTime, forClient, dbtran);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.quest_checkin, m_Owner.UserData, LogHelper.ToJson(mission), dbtran, 1);
                CGameLog.Instance.Insert(log);

                CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
                CNetManager.Instance.P2C_ResultMissionReward(sessionKey, questSystem.Board.ID, mission, forClient.GetList(), Packet_Result.Result.Success);
                CNetManager.Instance.P2C_ReportQuestMissionData(sessionKey, CDefine.Modify.Update, questSystem.Board.ID, SCopy<_Mission>.DeepCopy(mission));

                return Packet_Result.Result.Success;
            }

            CLogger.Instance.System($"ReqRepeatMissionReward");
            return Packet_Result.Result.PacketError;
        }

        private Packet_Result.Result ReqEventMissionReward(long sessionKey, string questID, int missionID)
        {
            if (FindSystem(CDefine.QuestType.Event, questID) is CQuestSystemEx_Event questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.Debug($"ReqQuestMissionReward Borad : {CDefine.QuestType.Repeat}");
                    return Packet_Result.Result.InValidData;
                }

                if (SDateManager.Instance.IsExpired(questSystem.Board.ExpTime))
                    return Packet_Result.Result.PacketError;

                var mission = questSystem.Board.Find(missionID);
                if (mission == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward mission : {missionID}");
                    return Packet_Result.Result.InValidData;
                }

                if (mission.State != CDefine.MissionState.Complete)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward misson.State : {mission.State}");
                    return Packet_Result.Result.PacketError;
                }

                var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                if (curMissionRecord == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward missionRecord : {mission.ID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                if (condRecord == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward condRecord : {curMissionRecord.ConditionID}");
                    return Packet_Result.Result.InValidRecord;
                }

                var rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
                if (rewards == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionReward rewards : {curMissionRecord.Reward_Id}");
                    return Packet_Result.Result.PacketError;
                }

                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();
                foreach (var iter in rewards)
                {
                    var reward = iter.Value;
                    forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                }

                forClient.UpdateRewardData(m_Owner, ref dbtran, true);
                dbtran.Merge();

                mission.State = CDefine.MissionState.Rewarded;
                mission.Modifyed = false;

                CDBManager.Instance.CQueryCharacterQuestEventReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, new List<_Mission> { mission }, questSystem.Board.ExpTime, forClient, dbtran, questSystem.Board.ID);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.event_quest, m_Owner.UserData, LogHelper.ToJson(mission), dbtran, 1);
                CGameLog.Instance.Insert(log);

                CNetManager.Instance.P2C_ResultMissionReward(sessionKey, questSystem.Board.ID, mission, forClient.GetList(), Packet_Result.Result.Success);
                CNetManager.Instance.P2C_ReportQuestMissionData(sessionKey, CDefine.Modify.Update, questSystem.Board.ID, SCopy<_Mission>.DeepCopy(mission));

                return Packet_Result.Result.Success;
            }

            CLogger.Instance.System($"ReqRepeatMissionReward");
            return Packet_Result.Result.PacketError;
        }

        public Packet_Result.Result ReqPassReward(long sessionKey, string questID, int missionID, bool passReward)
        {
            if (!passReward)
                return ReqPassRewardNormal(sessionKey, questID, missionID);
            else
                return ReqPassRewardActive(sessionKey, questID, missionID);
        }

        private Packet_Result.Result ReqPassRewardNormal(long sessionKey, string questID, int missionID)
        {
            //valid
            var missionRecord = MissionTable.Instance.Find(missionID);
            if (missionRecord == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward missionRecord : {missionID}");
                return Packet_Result.Result.InValidRecord;
            }

            var questRecord = QuestTable.Instance.Find(missionRecord.Quest_Id);
            if (questRecord == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward questRecord == null : {missionID}");
                return Packet_Result.Result.InValidRecord;
            }

            var questSystem = FindSystem(questRecord.Quest_Type, questRecord.ID);
            if (questSystem == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward questSystem null : {questRecord.ID}");
                return Packet_Result.Result.InValidData;
            }

            var mission = questSystem.Board.Find(missionID);
            if (mission == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward mission : {missionID}");
                return Packet_Result.Result.InValidData;
            }

            var curMissionRecord = MissionTable.Instance.Find(mission.ID);
            if (curMissionRecord == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward missionRecord : {mission.ID}");
                return Packet_Result.Result.InValidRecord;
            }

            var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
            if (condRecord == null)
            {
                CLogger.Instance.System($"ReqQuestMissionReward condRecord : {curMissionRecord.ConditionID}");
                return Packet_Result.Result.InValidRecord;
            }

            if (mission.State != CDefine.MissionState.Complete)
            {
                CLogger.Instance.System($"ReqPassMissionReward misson.State : {mission.State}");
                return Packet_Result.Result.PacketError;
            }

            var normal_rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
            if (normal_rewards == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward rewards : {curMissionRecord.Reward_Id}");
                return Packet_Result.Result.PacketError;
            }

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            foreach (var iter in normal_rewards)
            {
                var reward = iter.Value;
                forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
            }

            mission.State = CDefine.MissionState.Rewarded;
            mission.Modifyed = false;

            forClient.UpdateRewardData(m_Owner, ref dbtran, true);
            dbtran.Merge();

            var saveList = new List<_Mission>
                {
                    SCopy<_Mission>.DeepCopy(mission)
                };

            CDBManager.Instance.QueryCharacterQuestPassReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, questSystem.Board.ID, questSystem.Board.IsPassActivated,saveList, forClient, dbtran);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.quest_pass, m_Owner.UserData, LogHelper.ToJson(mission), dbtran, 1);
            CGameLog.Instance.Insert(log);

            CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
            CNetManager.Instance.P2C_ResultPassReward(sessionKey, questSystem.Board.ID, mission, forClient.GetList(), Packet_Result.Result.Success);
            CNetManager.Instance.P2C_ReportQuestMissionData(sessionKey, CDefine.Modify.Update, questSystem.Board.ID, SCopy<_Mission>.DeepCopy(mission));

            return Packet_Result.Result.Success;
        }

        private Packet_Result.Result ReqPassRewardActive(long sessionKey, string questID, int missionID)
        {
            //valid
            var missionRecord = MissionTable.Instance.Find(missionID);
            if (missionRecord == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward missionRecord : {missionID}");
                return Packet_Result.Result.InValidRecord;
            }

            var questRecord = QuestTable.Instance.Find(missionRecord.Quest_Id);
            if (questRecord == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward questRecord == null : {missionID}");
                return Packet_Result.Result.InValidRecord;
            }

            var questSystem = FindSystem(questRecord.Quest_Type, questRecord.ID);
            if (questSystem == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward questSystem null : {questRecord.ID}");
                return Packet_Result.Result.InValidData;
            }

            var mission = questSystem.Board.Find(missionID);
            if (mission == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward mission : {missionID}");
                return Packet_Result.Result.InValidData;
            }

            var curMissionRecord = MissionTable.Instance.Find(mission.ID);
            if (curMissionRecord == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward missionRecord : {mission.ID}");
                return Packet_Result.Result.InValidRecord;
            }

            var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
            if (condRecord == null)
            {
                CLogger.Instance.System($"ReqQuestMissionReward condRecord : {curMissionRecord.ConditionID}");
                return Packet_Result.Result.InValidRecord;
            }

            if (mission.State == CDefine.MissionState.Processing)
            {
                CLogger.Instance.System($"ReqPassMissionReward misson.State : {mission.State}");
                return Packet_Result.Result.PacketError;
            }

            if (!questSystem.Board.IsPassActivated)
            {
                CLogger.Instance.System($"ReqPassMissionReward questSystem.Board.IsPassActivated : {questSystem.Board.IsPassActivated}");
                return Packet_Result.Result.PacketError;
            }

            if (mission.PassRewarded)
            {
                CLogger.Instance.System($"ReqPassMissionReward mission.PassRewarded : {mission.PassRewarded}");
                return Packet_Result.Result.PacketError;
            }

            var pass_rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Pass_Id);
            if (pass_rewards == null)
            {
                CLogger.Instance.System($"ReqPassMissionReward rewards : {curMissionRecord.Reward_Id}");
                return Packet_Result.Result.PacketError;
            }

            CRewardInfo forClient = new CRewardInfo();
            CDBMerge dbtran = new CDBMerge();

            foreach (var iter in pass_rewards)
            {
                var reward = iter.Value;
                forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
            }

            mission.PassRewarded = true;
            mission.Modifyed = false;

            forClient.UpdateRewardData(m_Owner, ref dbtran, true);
            dbtran.Merge();

            var saveList = new List<_Mission>
                {
                    SCopy<_Mission>.DeepCopy(mission)
                };

            CDBManager.Instance.QueryCharacterQuestPassReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, questSystem.Board.ID, questSystem.Board.IsPassActivated, saveList, forClient, dbtran);

            //todo : log save
            var log = LogHelper.MakeLogBson(eLogType.quest_pass, m_Owner.UserData, LogHelper.ToJson(mission), dbtran, 1);
            CGameLog.Instance.Insert(log);

            CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
            CNetManager.Instance.P2C_ResultPassReward(sessionKey, questSystem.Board.ID, mission, forClient.GetList(), Packet_Result.Result.Success);
            CNetManager.Instance.P2C_ReportQuestMissionData(sessionKey, CDefine.Modify.Update, questSystem.Board.ID, SCopy<_Mission>.DeepCopy(mission));

            return Packet_Result.Result.Success;
        }

        public Packet_Result.Result ReqQuestMissionRewardList(long sessionKey, string questID, List<int> missionIDs)
        {
            //valid
            if(missionIDs.Count < 1)
            {
                CLogger.Instance.System($"ReqQuestMissionRewardList missionIDs.Count < 1 : {missionIDs.Count}");
                return Packet_Result.Result.PacketError;
            }

            CDefine.QuestType type = CDefine.QuestType.Max;
            foreach (var iter in missionIDs)
            {
                var missionRecord = MissionTable.Instance.Find(iter);
                if (missionRecord == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionRewardList missionRecord == null : {iter}");
                    return Packet_Result.Result.PacketError;
                }

                var questRecord = QuestTable.Instance.Find(missionRecord.Quest_Id);
                if (questRecord == null)
                {
                    CLogger.Instance.System($"ReqQuestMissionRewardList questRecord == null : {missionRecord.Quest_Id}");
                    return Packet_Result.Result.PacketError;
                }

                if (type != CDefine.QuestType.Max)
                {
                    if(type != questRecord.Quest_Type)
                    {
                        CLogger.Instance.System($"ReqQuestMissionRewardList questRecord == null : {type}");
                        return Packet_Result.Result.PacketError;
                    }
                }


                type = questRecord.Quest_Type;
            }


            switch (type)
            {
                case CDefine.QuestType.Daily:
                    return ReqDailyMissionRewardList(sessionKey, questID, missionIDs);
                case CDefine.QuestType.Repeat:
                    return ReqRepeatMissonRewardList(sessionKey, questID, missionIDs);
                case CDefine.QuestType.Max:
                    break;
                default:
                    break;
            }

            return Packet_Result.Result.PacketError;
        }

        private Packet_Result.Result ReqRepeatMissonRewardList(long sessionKey, string questID,List<int> missionIDs)
        {
            //vaild
            if (FindSystem(CDefine.QuestType.Repeat, questID) is CQuestSystemEx_Repeat questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.System($"[ReqRepeatMissonRewardList]  questSystem : {CDefine.QuestType.Repeat}");
                    return Packet_Result.Result.InValidData;
                }

                if (missionIDs.Count < 1)
                {
                    CLogger.Instance.System($"[ReqRepeatMissonRewardList] mission Count : {missionIDs.Count}");
                    return Packet_Result.Result.InValidData;
                }


                List<_Mission> processList = new List<_Mission>();
                foreach (var iter in missionIDs)
                {
                    var mission = questSystem.Board.Find(iter);
                    if (mission == null)
                        continue;

                    if (mission.State != CDefine.MissionState.Complete)
                        continue;

                    var missionRecord = MissionTable.Instance.Find(mission.ID);
                    if (missionRecord == null)
                        continue;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        continue;

                    var cnt = mission.Val / condRecord.Value;
                    if (cnt < 1)
                        continue;

                    processList.Add(mission);
                }

                if (processList.Count < 1)
                {
                    CLogger.Instance.System($"[ReqRepeatMissonRewardList] processList.Count : {processList.Count}");
                    return Packet_Result.Result.NotFoundData;
                }

                //process
                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();
                List<_Mission> saveList = new List<_Mission>();

                foreach (var mission in processList)
                {
                    var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                    if (curMissionRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqRepeatMissonRewardList missionRecord : {mission.ID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                    if (condRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqRepeatMissonRewardList condRecord : {curMissionRecord.ConditionID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var clearCnt = mission.Val / condRecord.Value;
                    if (clearCnt < 1)
                    {
                        CLogger.Instance.Debug($"ReqRepeatMissonRewardList clearCnt : {clearCnt}");
                        return Packet_Result.Result.PacketError;
                    }

                    var rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
                    if (rewards == null)
                    {
                        CLogger.Instance.Debug($"ReqRepeatMissonRewardList rewards : {curMissionRecord.Reward_Id}");
                        return Packet_Result.Result.PacketError;
                    }

                    foreach (var iter in rewards)
                    {
                        var reward = iter.Value;
                        forClient.Insert(reward.AssetType, reward.RewardID, reward.Value * clearCnt);
                    }

                    mission.State = CDefine.MissionState.Processing;
                    mission.Val = mission.Val % condRecord.Value;

                    mission.Modifyed = false;
                    saveList.Add(SCopy<_Mission>.DeepCopy(mission));
                }

                bool bSend = true;
                forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
                dbtran.Merge();

                CDBManager.Instance.QueryCharacterQuestRepeatReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, saveList, forClient, dbtran);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.quest_repeat, m_Owner.UserData, LogHelper.ToJson(saveList), dbtran, saveList.Count);
                CGameLog.Instance.Insert(log);

                CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
                CNetManager.Instance.P2C_ResultMissionRewardList(sessionKey, questSystem.Board.ID, saveList, forClient.GetList(), Packet_Result.Result.Success);
                CNetManager.Instance.P2C_ReportQuestMissionDataList(sessionKey, questSystem.Board.ID, CDefine.Modify.Update, saveList);

                return Packet_Result.Result.Success;
            }

            CLogger.Instance.Debug($"ReqRepeatMissonRewardList");
            return Packet_Result.Result.PacketError;
        }

        private Packet_Result.Result ReqDailyMissionRewardList(long sessionKey, string questID, List<int> missionIDs)
        {
            if (FindSystem(CDefine.QuestType.Daily, questID) is CQuestSystemEx_Daily questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.System($"[ReqDailyMissionRewardList] : {CDefine.QuestType.Daily}");
                    return Packet_Result.Result.InValidData;
                }

                if (missionIDs.Count < 1)
                {
                    CLogger.Instance.System($"[ReqDailyMissionRewardList] mission Count : {missionIDs.Count}");
                    return Packet_Result.Result.InValidData;
                }


                List<_Mission> processList = new List<_Mission>();
                foreach (var iter in missionIDs)
                {
                    var mission = questSystem.Board.Find(iter);
                    if (mission == null)
                        continue;

                    if (mission.State != CDefine.MissionState.Complete)
                        continue;

                    var missionRecord = MissionTable.Instance.Find(mission.ID);
                    if (missionRecord == null)
                        continue;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        continue;

                    processList.Add(mission);
                }

                if (processList.Count < 1)
                {
                    CLogger.Instance.System($"[ReqDailyMissionRewardList] processList.Count : {processList.Count}");
                    return Packet_Result.Result.NotFoundData;
                }

                //process
                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();
                List<_Mission> saveList = new List<_Mission>();

                foreach (var mission in processList)
                {
                    var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                    if (curMissionRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqDailyMissionRewardList missionRecord : {mission.ID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                    if (condRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqDailyMissionRewardList condRecord : {curMissionRecord.ConditionID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
                    if (rewards == null)
                    {
                        CLogger.Instance.Debug($"ReqDailyMissionRewardList rewards : {curMissionRecord.Reward_Id}");
                        return Packet_Result.Result.PacketError;
                    }

                    foreach (var iter in rewards)
                    {
                        var reward = iter.Value;
                        forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                    }

                    mission.State = CDefine.MissionState.Rewarded;
                    mission.Modifyed = false;
                    saveList.Add(SCopy<_Mission>.DeepCopy(mission));
                }

                bool bSend = true;
                forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
                dbtran.Merge();

                CDBManager.Instance.QueryCharacterQuestDailyReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, saveList, questSystem.Board.ExpTime, forClient, dbtran);

                UpdateQuestBorad(CDefine.MissionCondition.Mission_Rewarded, "", CDefine.QuestType.Daily.ToString(), saveList.Count);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.quest_daily, m_Owner.UserData, LogHelper.ToJson(saveList), dbtran, saveList.Count);
                CGameLog.Instance.Insert(log);

                CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
                CNetManager.Instance.P2C_ResultMissionRewardList(sessionKey, questSystem.Board.ID, saveList, forClient.GetList(), Packet_Result.Result.Success);
                CNetManager.Instance.P2C_ReportQuestMissionDataList(sessionKey, questSystem.Board.ID, CDefine.Modify.Update, saveList);

                return Packet_Result.Result.Success;
            }

            CLogger.Instance.Debug($"ReqDailyMissionRewardList");
            return Packet_Result.Result.PacketError;
        }

        public Packet_Result.Result ReqPassRewardList(long sessionKey, string questID, bool pass_reward)
        {
            if (!pass_reward)
                return ReqPassRewardListNormal(sessionKey, questID);
            else
                return ReqPassRewardListActive(sessionKey, questID);
        }

        private Packet_Result.Result ReqPassRewardListNormal(long sessionKey, string questID)
        {
            if (FindSystem(CDefine.QuestType.Pass, questID) is CQuestSystemEx_Pass questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.System($"[ReqPassMissionRewardList] questSystem null : {questID}");
                    return Packet_Result.Result.InValidData;
                }

                List<_Mission> processList = new List<_Mission>();

                foreach (var mission in questSystem.Board.Missions)
                {
                    if (mission.State != CDefine.MissionState.Complete)
                        continue;

                    var missionRecord = MissionTable.Instance.Find(mission.ID);
                    if (missionRecord == null)
                        continue;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        continue;

                    processList.Add(mission);
                }

                if (processList.Count < 1)
                {
                    CLogger.Instance.System($"[ReqPassMissionRewardList] processList.Count : {processList.Count}");
                    return Packet_Result.Result.NotFoundData;
                }

                //process
                Dictionary<int, _Mission> saveMissions = new Dictionary<int, _Mission>();
                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();

                foreach (var mission in processList)
                {
                    var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                    if (curMissionRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList missionRecord : {mission.ID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                    if (condRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList condRecord : {curMissionRecord.ConditionID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var normal_rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
                    if (normal_rewards == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList rewards : {curMissionRecord.Reward_Id}");
                        return Packet_Result.Result.PacketError;
                    }

                    foreach (var iter in normal_rewards)
                    {
                        var reward = iter.Value;
                        forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                    }

                    mission.State = CDefine.MissionState.Rewarded;
                    mission.Modifyed = false;
                    saveMissions[mission.ID] = SCopy<_Mission>.DeepCopy(mission);
                }

                bool bSend = true;
                forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
                dbtran.Merge();

                var sendList = new List<_Mission>(saveMissions.Values);
                
                CDBManager.Instance.QueryCharacterQuestPassReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, questSystem.Board.ID, questSystem.Board.IsPassActivated, sendList, forClient, dbtran);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.quest_pass, m_Owner.UserData, LogHelper.ToJson(sendList), dbtran, sendList.Count);
                CGameLog.Instance.Insert(log);

                CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
                CNetManager.Instance.P2C_ResultPassRewardList(sessionKey, questSystem.Board.ID, sendList, forClient.GetList(), Packet_Result.Result.Success);
                CNetManager.Instance.P2C_ReportQuestMissionDataList(sessionKey, questSystem.Board.ID, CDefine.Modify.Update, sendList);

                return Packet_Result.Result.Success;
            }

            CLogger.Instance.Debug($"ReqPassMissionRewardList");
            return Packet_Result.Result.PacketError;
        }

        private Packet_Result.Result ReqPassRewardListActive(long sessionKey, string questID)
        {
            if (FindSystem(CDefine.QuestType.Pass, questID) is CQuestSystemEx_Pass questSystem)
            {
                if (questSystem == null)
                {
                    CLogger.Instance.System($"[ReqPassMissionRewardList] questSystem null : {questID}");
                    return Packet_Result.Result.InValidData;
                }

                if(!questSystem.Board.IsPassActivated)
                {
                    CLogger.Instance.System($"[ReqPassMissionRewardList] questSystem.Board.IsPassActivated : {questSystem.Board.IsPassActivated}");
                    return Packet_Result.Result.InValidData;
                }

                //normal reward vaild
                List<_Mission> normalprocessList = new List<_Mission>();
                foreach (var mission in questSystem.Board.Missions)
                {
                    if (mission.State != CDefine.MissionState.Complete)
                        continue;

                    var missionRecord = MissionTable.Instance.Find(mission.ID);
                    if (missionRecord == null)
                        continue;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        continue;

                    normalprocessList.Add(mission);
                }

                //pass active reward vaild
                List<_Mission> activeprocessList = new List<_Mission>();
                foreach (var mission in questSystem.Board.Missions)
                {
                    if (mission.PassRewarded)
                        continue;

                    if (mission.State == CDefine.MissionState.Processing || mission.State == CDefine.MissionState.Max)
                        continue;

                    var missionRecord = MissionTable.Instance.Find(mission.ID);
                    if (missionRecord == null)
                        continue;

                    var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
                    if (condRecord == null)
                        continue;

                    activeprocessList.Add(mission);
                }

                if (activeprocessList.Count < 1 && normalprocessList.Count < 1)
                {
                    CLogger.Instance.System($"[ReqPassMissionRewardList] processList.Count : {activeprocessList.Count}");
                    return Packet_Result.Result.NotFoundData;
                }

                //process
                Dictionary<int, _Mission> saveMission = new Dictionary<int, _Mission>();
                CRewardInfo forClient = new CRewardInfo();
                CDBMerge dbtran = new CDBMerge();

                //normal reward process
                foreach (var mission in normalprocessList)
                {
                    var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                    if (curMissionRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList missionRecord : {mission.ID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                    if (condRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList condRecord : {curMissionRecord.ConditionID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var normal_rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Id);
                    if (normal_rewards == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList rewards : {curMissionRecord.Reward_Id}");
                        return Packet_Result.Result.PacketError;
                    }

                    foreach (var iter in normal_rewards)
                    {
                        var reward = iter.Value;
                        forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                    }

                    mission.State = CDefine.MissionState.Rewarded;
                    mission.Modifyed = false;
                    saveMission[mission.ID] = SCopy<_Mission>.DeepCopy(mission);
                }

                //active pass reward process
                foreach (var mission in activeprocessList)
                {
                    var curMissionRecord = MissionTable.Instance.Find(mission.ID);
                    if (curMissionRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList missionRecord : {mission.ID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var condRecord = ConditionTable.Instance.Find(curMissionRecord.ConditionID);
                    if (condRecord == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList condRecord : {curMissionRecord.ConditionID}");
                        return Packet_Result.Result.InValidRecord;
                    }

                    var actived_rewards = RewardTable.Instance.Find(curMissionRecord.Reward_Pass_Id);
                    if (actived_rewards == null)
                    {
                        CLogger.Instance.Debug($"ReqPassMissionRewardList rewards : {curMissionRecord.Reward_Id}");
                        return Packet_Result.Result.PacketError;
                    }

                    foreach (var iter in actived_rewards)
                    {
                        var reward = iter.Value;
                        forClient.Insert(reward.AssetType, reward.RewardID, reward.Value);
                    }

                    mission.PassRewarded = true;
                    mission.Modifyed = false;
                    saveMission[mission.ID] = SCopy<_Mission>.DeepCopy(mission);
                }

                bool bSend = true;
                forClient.UpdateRewardData(m_Owner, ref dbtran, bSend);
                dbtran.Merge();

                var sendList = new List<_Mission>(saveMission.Values);
                
                CDBManager.Instance.QueryCharacterQuestPassReward(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, questSystem.Board.ID, questSystem.Board.IsPassActivated, sendList, forClient, dbtran);

                //todo : log save
                var log = LogHelper.MakeLogBson(eLogType.quest_pass, m_Owner.UserData, LogHelper.ToJson(sendList), dbtran, sendList.Count);
                CGameLog.Instance.Insert(log);

                CNetManager.Instance.P2C_ReportAssetData(sessionKey, m_Owner.AssetAgent.GetList());
                CNetManager.Instance.P2C_ResultPassRewardList(sessionKey, questSystem.Board.ID, sendList, forClient.GetList(), Packet_Result.Result.Success);
                CNetManager.Instance.P2C_ReportQuestMissionDataList(sessionKey, questSystem.Board.ID, CDefine.Modify.Update, sendList);

                return Packet_Result.Result.Success;
            }

            CLogger.Instance.Debug($"ReqPassMissionRewardList");
            return Packet_Result.Result.PacketError;
        }


        public Packet_Result.Result ReqQuestCustomUpdate(long sessionKey, int missionID)
        {
            var missionRecord = MissionTable.Instance.Find(missionID);
            if (missionRecord == null)
                return Packet_Result.Result.InValidRecord;

            var questRecord = QuestTable.Instance.Find(missionRecord.Quest_Id);
            if (questRecord == null)
                return Packet_Result.Result.PacketError;

            var questSystem = FindSystem(missionRecord.Quest_Type, questRecord.ID);
            if (questSystem == null)
                return Packet_Result.Result.InValidRecord;

            var mission = questSystem.Board.Find(missionID);
            if (mission == null)
                return Packet_Result.Result.InValidData;

            var conditionRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
            if (conditionRecord == null)
                return Packet_Result.Result.InValidRecord;

            if (mission.State != CDefine.MissionState.Processing)
                return Packet_Result.Result.InValidData;

            mission.Val = conditionRecord.Value;
            mission.State = CDefine.MissionState.Complete;

            switch (missionRecord.Quest_Type)
            {
                case CDefine.QuestType.Main:
                    {
                        CDBManager.Instance.QueryCharacterQuestMainUpdate(m_Owner.DBGUID, sessionKey, m_Owner.UID, SCopy<_Mission>.DeepCopy(mission));
                    }
                    break;
                //case CDefine.QuestType.Repeat:
                //    {
                //        List<CMission> saveList = new List<CMission>();
                //        saveList.Add(SCopy<CMission>.DeepCopy(mission));
                //        CDBManager.Instance.QueryCharacterQuestRepeatUpdate(m_Owner.DBGUID, sessionKey, m_Owner.GetUID(), saveList);
                //    }
                //    break;
                //case CDefine.QuestType.Daily:
                //    {
                //        List<CMission> saveList = new List<CMission>();
                //        saveList.Add(SCopy<CMission>.DeepCopy(mission));
                //        CDBManager.Instance.QueryCharacterQuestDailyUpdate(m_Owner.DBGUID, sessionKey, m_Owner.GetUID(), saveList, questSystem.Board.ExpTime);
                //    }
                //case CDefine.QuestType.CheckIn:
                //    break;
            }

            CNetManager.Instance.P2C_ResultQuestCustomUpdate(sessionKey, mission, Packet_Result.Result.Success);
            CNetManager.Instance.P2C_ReportQuestMissionData(sessionKey, CDefine.Modify.Update, questSystem.Board.ID, mission);

            return Packet_Result.Result.Success;
        }

        public void AfterQueryRewardMainMission(long sessionKey, _Mission mission, CRewardInfo forClient, CDBMerge dbtran, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            m_Owner.ReportAssetData(dbtran);
        }

        public void AfterQueryRewardRepeatMission(long sessionKey, List<_Mission> missions, CRewardInfo forClient, CDBMerge dbtran, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            m_Owner.ReportAssetData(dbtran);
        }

        public void AfterQueryRewardDailyMissin(long sessionKey, List<_Mission> mission, CRewardInfo forClient, CDBMerge dbtran, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            m_Owner.ReportAssetData(dbtran);
        }

        public void AfterQueryRewardEventMission(long sessionKey, List<_Mission> mission, CRewardInfo forClient, CDBMerge dbtran, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            m_Owner.ReportAssetData(dbtran);
        }

        public void AfterQueryRewardCheckInMission(long sessionKey, _Mission misison, CRewardInfo forClient, CDBMerge dbtran, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            m_Owner.ReportAssetData(dbtran);
            CLogger.Instance.System($"Check In Rewrad!! : [{misison.ID}]");
        }

        public void AfterQueryRewardPassMission(long sessionKey, List<_Mission> missions, CRewardInfo forClient, CDBMerge dbtran, Packet_Result.Result result)
        {
            CNetManager.Instance.P2C_ReportRewardData(sessionKey, forClient.GetList());
            m_Owner.ReportAssetData(dbtran);
        }

        public void CheatPassReset(string questID)
        {
            if(questID != "-1")
            {
                if(FindSystem(CDefine.QuestType.Pass, questID) is var system)
                {
                    system.Board.Missions.ForEach(x =>
                    {
                        x.State = CDefine.MissionState.Processing;
                        x.PassRewarded = false;
                        x.Val = 0;
                        x.Modifyed = true;
                    });

                    system.Refresh(true);
                }
            }
            else
            {
                if (FindSystem<CQuestSystemEx_Pass>(CDefine.QuestType.Pass) is var systems)
                {
                    foreach (var iter in systems)
                    {
                        iter.Board.Missions.ForEach(x =>
                        {
                            x.State = CDefine.MissionState.Processing;
                            x.PassRewarded = false;
                            x.Val = 0;
                            x.Modifyed = true;
                        });

                        iter.Refresh(true);
                    }
                }
            }
        }

        public void CheakMissionUpdate(int missionID, long value)
        {
            var missionRecord = MissionTable.Instance.Find(missionID);
            if (missionRecord == null)
                return;

            var questRecord = QuestTable.Instance.Find(missionRecord.Quest_Id);
            if (questRecord == null)
                return;

            var questSystems = FindSystem(questRecord.Quest_Type, questRecord.ID);
            if (questSystems == null)
                return;

            var mission = questSystems.Board.Find(missionID);
            if (mission == null)
                return;

            if (mission.State != CDefine.MissionState.Processing)
                return;

            var condRecord = ConditionTable.Instance.Find(missionRecord.ConditionID);
            if (condRecord == null)
                return;

            if (value == -1)
                mission.Val = condRecord.Value;
            else
                mission.Val = value;

            questSystems.Refresh(false);

            CNetManager.Instance.P2C_ReportQuestMissionData(m_Owner.SessionKey, CDefine.Modify.Update, questSystems.Board.ID, mission);
        }

        public void PassActiveOn(string questID)
        {
            var questSystem = FindSystem(CDefine.QuestType.Pass, questID);
            if (questSystem == null)
                return;

            questSystem.Board.IsPassActivated = true;

            var sendList = new List<_QuestBoard>();
            sendList.Add(SCopy<_QuestBoard>.DeepCopy(questSystem.Board));
            CDBManager.Instance.QueryCharacterQuestPassUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, questSystem.Board.ID, questSystem.Board.IsPassActivated, questSystem.Board.Missions);
            CNetManager.Instance.P2C_ReportPassActive(m_Owner.SessionKey, questSystem.Board.ID, questSystem.Board.IsPassActivated);

            questSystem.ResetModify();
        }

        public void InsertEvent(string event_quest_id, long event_end_time, bool bSend)
        {
            var questRecord = QuestTable.Instance.Find(event_quest_id);
            if (questRecord == null)
                return;

            var copyBoard = QuestTable.Instance.CopyDefaultEvent(event_quest_id, event_end_time);
            if (copyBoard == null)
                return;

            m_QuestSystem[new KeyValuePair<CDefine.QuestType, string>(questRecord.Quest_Type, questRecord.ID)] = new CQuestSystemEx_Event(m_Owner, copyBoard);

            if (bSend)
                CNetManager.Instance.P2C_ReportQuestBoardList(m_Owner.SessionKey, new List<_QuestBoard> { copyBoard });
        }

        public void SaveAtLogout()
        {
            foreach (var iter in m_QuestSystem)
            {
                var questSystem = iter.Value;
                switch (questSystem.Board.Type)
                {
                    case CDefine.QuestType.Main:
                        {
                            var savelist = questSystem.GetModifyList();
                            if (savelist.Count > 0)
                                CDBManager.Instance.QueryCharacterQuestMainUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, savelist.First());
                        }
                        break;
                    case CDefine.QuestType.Repeat:
                        {
                            var saveList = questSystem.GetModifyList();
                            if (saveList.Count > 0)
                                CDBManager.Instance.QueryCharacterQuestRepeatUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, saveList);
                        }
                        break;
                    case CDefine.QuestType.Daily:
                        {
                            var saveList = questSystem.GetModifyList();
                            if (saveList.Count > 0)
                                CDBManager.Instance.QueryCharacterQuestDailyUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, saveList, questSystem.Board.ExpTime);
                        }
                        break;
                    case CDefine.QuestType.CheckIn:
                        {
                            var savelist = questSystem.GetModifyList();
                            if (savelist.Count > 0)
                                CDBManager.Instance.QueryCharacterQuestCheckInUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, questSystem.Board.ID, savelist.First(), questSystem.Board.ExpTime);
                        }
                        break;
                    case CDefine.QuestType.Pass:
                        {
                            var saveList = questSystem.GetModifyList();
                            if (saveList.Count > 0)
                                CDBManager.Instance.QueryCharacterQuestPassUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, questSystem.Board.ID, questSystem.Board.IsPassActivated, questSystem.Board.Missions);
                        }
                        break;
                    case CDefine.QuestType.Event:
                        {
                            var saveList = questSystem.GetModifyList();
                            if (saveList.Count > 0)
                                CDBManager.Instance.QueryCharacterQuestEventUpdate(m_Owner.DBGUID, m_Owner.SessionKey, m_Owner.UID, saveList, questSystem.Board.ExpTime, questSystem.Board.ID);
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
