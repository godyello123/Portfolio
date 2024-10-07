using Global;
using SCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public class QuestRecord : STableRecord<string>
    {
        public CDefine.QuestType Quest_Type = CDefine.QuestType.Max;
        public int Quest_MaxAccept = 0;
    }

    public class QuestTable : STable<QuestTable, string , QuestRecord>
    {
        _QuestBoard m_DefaultMainQuest;
        _QuestBoard m_DefaultRepeatQuest;
        _QuestBoard m_DefaultDailyQuest;
        Dictionary<string, _QuestBoard> m_DefaultCheckIn;
        Dictionary<string, _QuestBoard> m_DefaultPass;
        Dictionary<string, _QuestBoard> m_DefaultEvent;

        public override void Prepare()
        {
            PrepareDefaulatMain();
            PrepareDefaultRepeat();
            PrepareDefaultDaily();
            PrepareDefaultCheckIn();
            PrepareDefaultPass();
            PrepareDefaultEvent();
        }

        public override bool Load(string text)
        {
            Clear();

            try
            {
                SCSVReader reader = new SCSVReader();
                if (!reader.LoadFromString(text)) return false;

                for (int i = 0; i < reader.GetRowCount(); i++)
                {
                    QuestRecord record = new QuestRecord();
                    record.ID = reader.GetValue(i, "Id");
                    record.Quest_Type = reader.GetEnum<CDefine.QuestType>(i, "Quest_Type", CDefine.QuestType.Max);
                    record.Quest_MaxAccept = reader.GetValue<int>(i, "Quest_MaxAccept", 0);

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

        private void PrepareDefaulatMain()
        {
            m_DefaultMainQuest = new _QuestBoard();

            var foundRecord = Table.FirstOrDefault(x => x.Value.Quest_Type == CDefine.QuestType.Main);
            var questRecord = foundRecord.Value;
            if (questRecord == null)
                return;

            var missions = MissionTable.Instance.Table.Where(x => x.Value.Quest_Id == questRecord.ID);
            if (missions == null)
                return;

            var firstMission = missions.OrderBy(x => x.Value.Order).FirstOrDefault();

            var misisonRecord = firstMission.Value;
            if (misisonRecord == null)
                return;

            m_DefaultMainQuest.Type = questRecord.Quest_Type;
            m_DefaultMainQuest.ID = questRecord.ID.ToString();

            var conditionRecord = ConditionTable.Instance.Find(misisonRecord.ConditionID);
            if (conditionRecord == null)
                return;

            _Mission mission = new _Mission();
            mission.ID = misisonRecord.ID;
            mission.State = CDefine.MissionState.Processing;
            mission.Val = 0;
            m_DefaultMainQuest.Missions.Add(mission);
        }

        private void PrepareDefaultRepeat()
        {
            m_DefaultRepeatQuest = new _QuestBoard();

            var findData = Table.FirstOrDefault(x => x.Value.Quest_Type == CDefine.QuestType.Repeat);
            var questRecord = findData.Value;
            if (questRecord == null)
                return;

            var itMissions = MissionTable.Instance.Table.Where(x => x.Value.Quest_Id == questRecord.ID);
            if (itMissions == null)
                return;

            m_DefaultRepeatQuest.Type = questRecord.Quest_Type;
            m_DefaultRepeatQuest.ID = questRecord.ID.ToString();

            foreach(var iter in itMissions)
            {
                var misisonRecord = iter.Value;
                if (misisonRecord == null)
                    continue;

                var mission = new _Mission();
                mission.ID = misisonRecord.ID;
                mission.State = CDefine.MissionState.Processing;
                mission.Val = 0;
                m_DefaultRepeatQuest.Missions.Add(mission);
            }
        }

        private void PrepareDefaultDaily()
        {
            m_DefaultDailyQuest = new _QuestBoard();

            var findData = Table.FirstOrDefault(x => x.Value.Quest_Type == CDefine.QuestType.Daily);
            var questRecord = findData.Value;
            if (questRecord == null)
                return;

            var itMissions = MissionTable.Instance.Table.Where(x => x.Value.Quest_Id == questRecord.ID);
            if (itMissions == null)
                return;

            m_DefaultDailyQuest.Type = questRecord.Quest_Type;
            m_DefaultDailyQuest.ID = questRecord.ID.ToString();

            foreach (var iter in itMissions)
            {
                var misisonRecord = iter.Value;
                if (misisonRecord == null)
                    continue;

                var mission = new _Mission();
                mission.ID = misisonRecord.ID;
                mission.State = CDefine.MissionState.Processing;
                mission.Val = 0;
                m_DefaultDailyQuest.Missions.Add(mission);
            }
        }

        private void PrepareDefaultCheckIn()
        {
            m_DefaultCheckIn = new Dictionary<string, _QuestBoard>();

            var foundRecords = Table.Where(x => x.Value.Quest_Type == CDefine.QuestType.CheckIn);

            foreach(var iter in foundRecords)
            {
                var questRecord = iter.Value;
                if (questRecord == null)
                    continue;

                var findMission = MissionTable.Instance.Table
                    .Where(x => x.Value.Quest_Id == questRecord.ID)
                    .OrderBy(x=>x.Value.Order)
                    .FirstOrDefault();

                if (findMission.Value == null)
                    continue;

                if (!m_DefaultCheckIn.TryGetValue(questRecord.ID, out var board))
                {
                    board = new _QuestBoard();
                    board.ID = questRecord.ID;
                    board.Type = questRecord.Quest_Type;

                    board.Missions.Add(new _Mission()
                    {
                        ID = findMission.Value.ID,
                        Val = 0,
                        State = CDefine.MissionState.Processing
                    });

                    m_DefaultCheckIn[questRecord.ID] = board;
                }
                else
                {
                    board.Missions.Add(new _Mission()
                    {
                        ID = findMission.Value.ID,
                        Val = 0,
                        State = CDefine.MissionState.Processing
                    });
                }
            }
        }

        private void PrepareDefaultPass()
        {
            m_DefaultPass = new Dictionary<string, _QuestBoard>();

            var foundRecords = Table.Where(x => x.Value.Quest_Type == CDefine.QuestType.Pass);

            foreach (var iter in foundRecords)
            {
                var questRecord = iter.Value;
                if (questRecord == null)
                    continue;

                var findMissions = MissionTable.Instance.Table.Where(x => x.Value.Quest_Id == questRecord.ID);
                if (findMissions.Count() < 1)
                    continue;

                foreach (var mission in findMissions)
                {
                    var misisonRecord = mission.Value;
                    if (misisonRecord == null)
                        continue;

                    if (!m_DefaultPass.TryGetValue(questRecord.ID, out var board))
                    {
                        board = new _QuestBoard();
                        board.ID = questRecord.ID;
                        board.Type = questRecord.Quest_Type;

                        board.Missions.Add(new _Mission()
                        {
                            ID = misisonRecord.ID,
                            Val = 0,
                            State = CDefine.MissionState.Processing
                        });

                        m_DefaultPass[questRecord.ID] = board;
                    }
                    else
                    {
                        board.Missions.Add(new _Mission()
                        {
                            ID = misisonRecord.ID,
                            Val = 0,
                            State = CDefine.MissionState.Processing
                        });
                    }

                }
            }
        }

        private void PrepareDefaultEvent()
        {
            m_DefaultEvent = new Dictionary<string, _QuestBoard>();

            var foundRecords = Table.Where(x => x.Value.Quest_Type == CDefine.QuestType.Event);

            foreach (var iter in foundRecords)
            {
                var questRecord = iter.Value;
                if (questRecord == null)
                    continue;

                var findMissions = MissionTable.Instance.Table.Where(x => x.Value.Quest_Id == questRecord.ID);
                if (findMissions.Count() < 1)
                    continue;

                foreach (var mission in findMissions)
                {
                    var misisonRecord = mission.Value;
                    if (misisonRecord == null)
                        continue;

                    if (!m_DefaultEvent.TryGetValue(questRecord.ID, out var board))
                    {
                        board = new _QuestBoard();
                        board.ID = questRecord.ID;
                        board.Type = questRecord.Quest_Type;

                        board.Missions.Add(new _Mission()
                        {
                            ID = misisonRecord.ID,
                            Val = 0,
                            State = CDefine.MissionState.Processing
                        });

                        m_DefaultEvent[questRecord.ID] = board;
                    }
                    else
                    {
                        board.Missions.Add(new _Mission()
                        {
                            ID = misisonRecord.ID,
                            Val = 0,
                            State = CDefine.MissionState.Processing
                        });
                    }
                }
            }
        }

        public _QuestBoard CopyDefault(CDefine.QuestType type)
        {
            switch (type)
            {
                case CDefine.QuestType.Main:
                    return SCommon.SCopy<_QuestBoard>.DeepCopy(m_DefaultMainQuest);
                case CDefine.QuestType.Daily:
                    {
                        var retBoard = SCommon.SCopy<_QuestBoard>.DeepCopy(m_DefaultDailyQuest);
                        retBoard.ExpTime = SDateManager.Instance.ProdTomorrowToUtc();
                        retBoard.Missions.ForEach(x => { x.Modifyed = true; });

                        return retBoard;
                    }
                case CDefine.QuestType.Repeat:
                    return SCommon.SCopy<_QuestBoard>.DeepCopy(m_DefaultRepeatQuest);
                case CDefine.QuestType.Max:
                    break;
                default:
                    break;
            }

            return new _QuestBoard();
        }

        public Dictionary<string, _QuestBoard> CopyDefaults(CDefine.QuestType type)
        {
            switch (type)
            {
                case CDefine.QuestType.CheckIn:
                    {
                        var retBoards = SCommon.SCopy<Dictionary<string, _QuestBoard>>.DeepCopy(m_DefaultCheckIn);
                        foreach(var it in retBoards)
                        {
                            var board = it.Value;
                            board.ExpTime = SDateManager.Instance.ProdTomorrowToUtc();
                            board.Missions.ForEach(x => { x.Modifyed = true; });
                        }

                        return retBoards;
                    }
                case CDefine.QuestType.Pass:
                    {
                        return SCommon.SCopy<Dictionary<string, _QuestBoard>>.DeepCopy(m_DefaultPass);
                    }
                case CDefine.QuestType.Max:
                    break;
                default:
                    break;
            }

            return new Dictionary<string, _QuestBoard>();
        }

        public _QuestBoard CopyDefaultEvent(string quest_id, long end_time)
        {
            if (m_DefaultEvent.TryGetValue(quest_id, out var findBoard))
            {
                var utc_now = SDateManager.Instance.CurrTime();
                var retBoard = SCopy<_QuestBoard>.DeepCopy(findBoard);
                retBoard.ExpTime = SDateManager.Instance.TimestampToUTC(end_time);
                retBoard.Missions.ForEach(x => { x.Modifyed = true; });

                return retBoard;
            }

            return null;
        }
    }
}
