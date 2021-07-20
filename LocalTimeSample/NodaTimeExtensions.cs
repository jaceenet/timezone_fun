using System;
using Gluu.TaskList.Cron;
using NodaTime;

namespace LocalTimeSample
{
    public static class NodaTimeExtensions
    {
        public static bool GetNext(this CronSchedule schedule, DateTimeZone zone, LocalDateTime last, out LocalDateTime localNext, out DateTime nextUtc)
        {
            var start = last.ToDateTimeUnspecified();
            var next = schedule.GetNext(start, out var nextLocal);
            
            if (next)
            {
                LocalDateTime localDateTime;

                try
                {
                    localDateTime = LocalDateTime.FromDateTime(nextLocal);
                    nextUtc = zone.AtLeniently(localDateTime).ToDateTimeUtc();
                    localNext = localDateTime;

                    return true;
                }
                catch(SkippedTimeException) //daylight skipped time
                {
                    var fixedLast = LocalDateTime.FromDateTime(nextLocal.AddMinutes(1));
                    return GetNext(schedule, zone, fixedLast, out localNext, out nextUtc);
                }
                catch(AmbiguousTimeException) //daylight issue
                {
                    var fixedLast = LocalDateTime.FromDateTime(nextLocal.AddMinutes(1));
                    return GetNext(schedule, zone, fixedLast, out localNext, out nextUtc);
                }
            }
            
            localNext = default;
            nextUtc = default;
            return false;
        }
        
        public static Instant UtcAsInstant(this DateTime dt)
        {
            if (dt.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("unsupported datetime, should be utc");
            }
            
            return Instant.FromUtc(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}