using System;

namespace SCommon
{
    public class SDateManager : SSingleton<SDateManager>
    {
        public SDateManager()
        {
            double offset = (DateTime.Now - DateTime.UtcNow).TotalSeconds;
            BaseUtcOffset = (int)Math.Round(offset, MidpointRounding.AwayFromZero);
        }
        private TimeZoneInfo ProdTimeZoneInfo { get; set; } = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
        private int BaseUtcOffset { get; }
        public long DateTimeToTimeStamp(DateTime date) { return ((DateTimeOffset)date).ToUnixTimeSeconds(); }
        public long CurrTime() { return DateTimeToTimeStamp(DateTime.UtcNow); }
        public int DatediffSec(DateTime start, DateTime end) { return (int)((end - start).TotalSeconds); }
        public int ToTimeLeft(DateTime date) { return Math.Max(0, DatediffSec(DateTime.UtcNow, date)); }
        public int ToTimeLeft(long time) { return Math.Max(0, (int)(time - CurrTime())); }
        public bool IsExpired(long time) { return time <= CurrTime(); }
        public bool IsLessThanToday(long time) { return time < TodayTimestamp(); }
        public bool IsToday(long time)
        {
            long datediffSec = time - TodayTimestamp();
            return (0 <= datediffSec && datediffSec < 86400);
        }

        public DateTime ConvertTimeFromUtc(DateTime date)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date, ProdTimeZoneInfo);
        }
        public DateTime ConvertTimeToUtc(DateTime date)
        {
            return TimeZoneInfo.ConvertTimeToUtc(date, ProdTimeZoneInfo);
        }
        public DateTime TimestampToUTC(long value)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(value);
            return dt;
        }
        public DateTime ProdToday()
        {
            return ConvertTimeFromUtc(DateTime.UtcNow).Date;
        }
        public DateTime ProdTodayToUtc()
        {
            return ConvertTimeToUtc(ProdToday());
        }
        public long TodayTimestamp()
        {
            return DateTimeToTimeStamp(ProdTodayToUtc());
        }
        public DateTime ProdTomorrow()
        {
            return ProdToday().AddDays(1);
        }
        public DateTime ProdTomorrowToUtc()
        {
            return ConvertTimeToUtc(ProdTomorrow());
        }
        public long TomorrowTimestamp()
        {
            return DateTimeToTimeStamp(ProdTomorrowToUtc());
        }

        public DateTime ProdThisWeeks(DayOfWeek dayOfWeek)
        {
            int addDay = ((int)ProdToday().DayOfWeek - (int)dayOfWeek);
            return ProdToday().AddDays(-addDay);
        }
        public DateTime ProdThisWeeksToUtc(DayOfWeek dayOfWeek)
        {
            return ConvertTimeToUtc(ProdThisWeeks(dayOfWeek));
        }
        public long ThisWeeksTimestamp(DayOfWeek dayOfWeek)
        {
            return DateTimeToTimeStamp(ProdThisWeeksToUtc(dayOfWeek));
        }
        public DateTime ProdNextWeeks(DayOfWeek dayOfWeek)
        {
            return ProdThisWeeks(dayOfWeek).AddDays(7);
        }
        public DateTime ProdNextWeeksToUtc(DayOfWeek dayOfWeek)
        {
            return ConvertTimeToUtc(ProdNextWeeks(dayOfWeek));
        }

        public long NextWeeksTimestamp(DayOfWeek dayOfWeek)
        {
            return DateTimeToTimeStamp(ProdNextWeeksToUtc(dayOfWeek));
        }

        public DateTime ProdFirstDayOfThisMonth()
        {
            var prodToday = ProdToday();
            int offsetDay = prodToday.Day - 1;

            return prodToday.AddDays(-offsetDay);
        }

        public DateTime ProdFirstDayOfThisMonthToUtc()
        {
            return ConvertTimeToUtc(ProdFirstDayOfThisMonth());
        }

        public long FirstDayOfThisMonthTimestamp()
        {
            return DateTimeToTimeStamp(ProdFirstDayOfThisMonthToUtc());
        }

        public DateTime ProdFirstDayOfNextMonth()
        {
            return ProdFirstDayOfThisMonth().AddMonths(1);
        }

        public DateTime ProdFirstDayOfNextMonthToUtc()
        {
            return ConvertTimeToUtc(ProdFirstDayOfNextMonth());
        }

        public long FirstDayOfNextMonthTimestamp()
        {
            return DateTimeToTimeStamp(ProdFirstDayOfNextMonthToUtc());
        }

        public bool IsLessThanToday(DateTime date)
        {//[todo:remove] 
            return IsLessThanToday(DateTimeToTimeStamp(date.AddSeconds(BaseUtcOffset)));
        }
        public bool IsToday(DateTime date)
        {//[todo:remove]
            return IsToday(DateTimeToTimeStamp(date.AddSeconds(BaseUtcOffset)));
        }
        public bool IsExpired(DateTime date)
        {//[todo:remove]
            if (date >= DateTime.MaxValue)
                return false;
            return IsExpired(DateTimeToTimeStamp(date.AddSeconds(BaseUtcOffset)));
        }
        public bool IsEnable(DateTime start, DateTime end)
        {//[todo:remove]
            return (start < DateTime.UtcNow && DateTime.UtcNow <= end);
        }

        public DateTime TimeStampToLocalTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
