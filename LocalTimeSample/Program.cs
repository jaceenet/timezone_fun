using System;
using Gluu.TaskList.Cron;
using NodaTime;

namespace LocalTimeSample
{
    class Program
    {
        private static DateTimeZone dk = DateTimeZoneProviders.Tzdb["Europe/Copenhagen"];
        
        static void Main(string[] args)
        {
            //Info();
            GenerateSample(new DateTime(2021,3,27,0,0,0), "15 1,2,3 * * *", dk);
            GenerateSample(new DateTime(2021,10,30,0,0,0), "15 1,2,3 * * *", dk);
        }
        
        private static void GenerateSample(DateTime start, string cron, DateTimeZone zone)
        {
            
            // Calculate the offset to get UTC
            var localDateTime = LocalDateTime.FromDateTime(start);
            var utcStart = zone.AtStrictly(localDateTime).ToDateTimeUtc();

            // Simulate the next
            Console.WriteLine("Simulate task:");
            Console.WriteLine($"Start Local : {start:g}");
            Console.WriteLine($"Start UTC : {utcStart:g}");
            Console.WriteLine($"Cron : {cron}");
            Console.WriteLine($"Start Offset : {TimeSpan.FromTicks(dk.AtStrictly(localDateTime).Offset.Ticks).TotalHours:00}");
            
            for (int i = 0; i < 15; i++)
            {
                var schedule = CronSchedule.Parse(cron);
                
                if (!schedule.GetNext(dk, localDateTime, out var dt, out var utc))
                {
                    break;
                }
                
                Console.WriteLine($"Next schedule, Local: {dt:g} UTC: {utc:g}");
                localDateTime = dt.PlusMinutes(1);
            }
        }
        
        private static void Info()
        {
            var zone = dk;
            var startUtc = new DateTime(2021, 3, 27, 22, 0, 0, DateTimeKind.Utc);
            
            Console.WriteLine("");
            Console.WriteLine("See the tricky part of daylight switching...");
            Console.WriteLine("");

            var dpos = startUtc;
            
            for (int i = 0; i < 4*4; i++)
            {
                var dposlcoal = dpos.AddTicks(zone.GetUtcOffset(dpos.UtcAsInstant()).Ticks);
                Console.WriteLine($"UTC: {dpos:g}, Local: {dposlcoal:g}");
                dpos = dpos.AddMinutes(15);
            }
            
            dpos = new DateTime(2021, 10, 30, 22, 0, 0, DateTimeKind.Utc);
            
            for (int i = 0; i < 4*4; i++)
            {
                var dposlcoal = dpos.AddTicks(zone.GetUtcOffset(dpos.UtcAsInstant()).Ticks);
                Console.WriteLine($"UTC: {dpos:g}, Local: {dposlcoal:g}");
                dpos = dpos.AddMinutes(15);
                
            }
            
            var invalidTime = new LocalDateTime(2021, 3, 28, 2, 0, 0);
            Console.WriteLine($"Converted {invalidTime:g} to {zone.AtStrictly(invalidTime):g}");
        }

    }
}