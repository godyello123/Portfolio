using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Global;
using SCommon;

namespace MessageServer
{
    public class CSchedulerEx_Base
    {
        public long m_UID;
        public eSchedule m_Type = eSchedule.Max;
        public DateTime m_StartDate;
        public DateTime m_EndDate;
        public List<DayOfWeek> m_DayofWeek = new List<DayOfWeek>();
        public TimeSpan m_StartTime;
        public TimeSpan m_EndTime;
        public string m_Param;
        public string m_Val;

        public virtual void Excute() { }
        public virtual void Complete() { }


        public virtual bool IsActive(DateTime utc_now)
        {
            if (utc_now.Date >= m_StartDate.Date && utc_now.Date <= m_EndDate.Date)
            {
                if (m_DayofWeek.Contains(utc_now.DayOfWeek))
                {
                    if (utc_now.TimeOfDay >= m_StartTime && utc_now.TimeOfDay < m_EndTime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual bool IsInActive(DateTime utc_now)
        {
            if (utc_now.Date >= m_StartDate.Date && utc_now.Date <= m_EndDate.Date)
            {
                if (m_DayofWeek.Contains(utc_now.DayOfWeek))
                {
                    if (utc_now.TimeOfDay > m_EndTime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class CSchedulerEx_HotTime : CSchedulerEx_Base
    {
        public _HotTimeData m_Data = new _HotTimeData();

        public override void Excute()
        {
            CLogger.Instance.System($"[Excute] {this.m_Type} : {this.m_UID}");

            DateTime utc_now = DateTime.UtcNow;
            m_Data.m_EndTime = new DateTime(utc_now.Year, utc_now.Month, utc_now.Day, m_EndTime.Hours, m_EndTime.Minutes, m_EndTime.Seconds);

            CNetManager.Instance.M2P_ReportHotTime(-1, m_Data, false);
        }

        public override void Complete()
        {
            CLogger.Instance.System($"[Complete] {this.m_Type} : {this.m_UID}");

            CNetManager.Instance.M2P_ReportHotTime(-1, m_Data, true);
        }
    }

    public class CSchedulerEx_ReservationPost : CSchedulerEx_Base
    {
        public string m_Title = string.Empty;
        public List<_AssetData> m_Rewards = new List<_AssetData>();

        public override void Excute()
        {
            CLogger.Instance.System($"[Excute] {this.m_Type} : {this.m_UID}");
            long begin = SDateManager.Instance.TodayTimestamp();
            long end = begin + 86400;

            CDBManager.Instance.SyncQuerySystemUpsertPost(-1, CDefine.PostType.Push, m_Title, m_Title, begin, end, SCommon.SJson.ObjectToJson(m_Rewards));
        }

        public override void Complete()
        {
        }
    }


    public class CScheduleManager : SSingleton<CScheduleManager>
    {
        private STimer m_Timer = new STimer(30 * 1000, false);
        private Dictionary<KeyValuePair<eSchedule, long>, CSchedulerEx_Base> m_ActiveSchedules = new Dictionary<KeyValuePair<eSchedule, long>, CSchedulerEx_Base>();
        private Dictionary<KeyValuePair<eSchedule, long>, CSchedulerEx_Base> m_InActiveSchedules = new Dictionary<KeyValuePair<eSchedule, long>, CSchedulerEx_Base>();

        private bool m_Run = false;

        public void Init()
        {
            m_ActiveSchedules.Clear();
            m_InActiveSchedules.Clear();

            m_Run = true;

            CDBManager.Instance.SyncQuerySystemLoadSchedule();
        }

        public void Update()
        {
            if (!m_Run) return;

            if (!m_Timer.Check()) return;

            CheckActive();
            CheckInActive();
        }

        public void Stop()
        {
            if (!m_Run) return;

            m_Run = false;

            m_ActiveSchedules.Clear();
            m_InActiveSchedules.Clear();
        }

        public void AfterQueryScheduleLoad(List<CSchedulerEx_Base> schedules)
        {
            foreach(var iter in schedules)
            {
                if (SDateManager.Instance.IsExpired(iter.m_EndDate))
                    continue;

                if (iter.m_Type == eSchedule.HotTime)
                    InsertScheduleHotTime(iter.m_UID, iter.m_Param, iter.m_Val, iter.m_StartDate, iter.m_EndDate, iter.m_DayofWeek, iter.m_StartTime, iter.m_EndTime);
                else if (iter.m_Type == eSchedule.Post)
                    InsertSchedulePost(iter.m_UID, iter.m_Param, iter.m_Val, iter.m_StartDate, iter.m_EndDate, iter.m_DayofWeek, iter.m_StartTime, iter.m_EndTime);
            }
        }

        public void AfterQeuryInsertScheduler(long uid, eSchedule type, string param, string val, DateTime start_date, DateTime end_date, List<DayOfWeek> dayofWeek, TimeSpan start_time, TimeSpan end_time)
        {
            if (type == eSchedule.HotTime)
                InsertScheduleHotTime(uid, param, val, start_date, end_date, dayofWeek, start_time, end_time);
            else if (type == eSchedule.Post)
                InsertSchedulePost(uid, param, val, start_date, end_date, dayofWeek, start_time, end_time);
        }

        private void InsertScheduleHotTime(long uid, string param, string val, DateTime start_date, DateTime end_date, List<DayOfWeek> dayofWeek, TimeSpan start_time, TimeSpan end_time)
        {
            var schedule = new CSchedulerEx_HotTime();
            schedule.m_UID = uid;
            schedule.m_Type = eSchedule.HotTime;
            schedule.m_Param = param;
            schedule.m_Val = val;
            schedule.m_StartDate = start_date;
            schedule.m_EndDate = end_date;
            schedule.m_DayofWeek = dayofWeek;
            schedule.m_StartTime = start_time;
            schedule.m_EndTime = end_time;


            CDefine.EAbility abilType = CDefine.EAbility.Max;
            if (!Enum.TryParse(param, out abilType))
                return;

            double abilVal = 0;
            if (!double.TryParse(val, out abilVal))
                return;

            schedule.m_Data.m_UID = schedule.m_UID;
            schedule.m_Data.m_Abil.type = abilType;
            schedule.m_Data.m_Abil.val = abilVal;

            var key = new KeyValuePair<eSchedule, long>(schedule.m_Type, schedule.m_UID);
            m_InActiveSchedules[key] = schedule;
        }

        private void InsertSchedulePost(long uid, string param, string val, DateTime start_date, DateTime end_date, List<DayOfWeek> dayofWeek, TimeSpan start_time, TimeSpan end_time)
        {
            var schedule = new CSchedulerEx_ReservationPost();
            schedule.m_UID = uid;
            schedule.m_Type = eSchedule.Post;
            schedule.m_Param = param;
            schedule.m_Val = val;
            schedule.m_StartDate = start_date;
            schedule.m_EndDate = end_date;
            schedule.m_DayofWeek = dayofWeek;
            schedule.m_StartTime = start_time;
            schedule.m_EndTime = end_time;

            schedule.m_Title = param;
            if (SJson.IsValidJson(val))
                schedule.m_Rewards = SJson.JsonToObject<List<_AssetData>>(val);

            var key = new KeyValuePair<eSchedule, long>(schedule.m_Type, schedule.m_UID);
            m_InActiveSchedules[key] = schedule;
        }

        public void CheckActive()
        {
            List<KeyValuePair<eSchedule, long>> eraselist = new List<KeyValuePair<eSchedule, long>>();
            DateTime utc_now = DateTime.UtcNow;
            foreach (var iter in m_ActiveSchedules)
            {
                var itVal = iter.Value;

                if (itVal.IsInActive(utc_now))
                {
                    itVal.Complete();
                    var key = new KeyValuePair<eSchedule, long>(itVal.m_Type, itVal.m_UID);
                    eraselist.Add(key);
                    m_InActiveSchedules.Add(key, itVal);
                }
            }

            foreach (var iter in eraselist)
                m_ActiveSchedules.Remove(iter);
        }

        public void CheckInActive()
        {
            List<KeyValuePair<eSchedule, long>> eraselist = new List<KeyValuePair<eSchedule, long>>();
            DateTime utc_now = DateTime.UtcNow;
            foreach (var iter in m_InActiveSchedules)
            {
                var itVal = iter.Value;

                if (itVal.IsActive(utc_now))
                {
                    itVal.Excute();
                    var key = new KeyValuePair<eSchedule, long>(itVal.m_Type, itVal.m_UID);
                    m_ActiveSchedules[key] = itVal;
                    eraselist.Add(key);
                }
            }

            foreach (var iter in eraselist)
                m_InActiveSchedules.Remove(iter);
        }

        public void ReqConnectPlayServer(long sessionKey)
        {
            var h_list = new List<_HotTimeData>();
            foreach (var iter in m_ActiveSchedules)
            {
                var itVal = iter.Value;
                if (itVal.m_Type != eSchedule.HotTime)
                    continue;

                if(itVal is CSchedulerEx_HotTime sch_hotTime)
                    h_list.Add(sch_hotTime.m_Data);
            }

            CNetManager.Instance.M2P_ReportHotTimeList(sessionKey, h_list);
        }
    }
}
