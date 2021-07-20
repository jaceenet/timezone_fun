using System;
using System.Linq;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace LocalTimeSample
{
    public class iCalSample
    {
        public static void FindNext(string ical)
        {
            var cal = Calendar.Load(ical);

            if (cal.Events.Count != 1)
            {
                throw new Exception("Should only contain a single event");
            }

            var iCalEvent = cal.Events[0];
            Console.WriteLine(iCalEvent);
            
            var occurrences = iCalEvent.GetOccurrences(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
                
            Console.WriteLine("This is the list of upcomming events in UTC and local time");
            foreach (var occurrence in occurrences.Take(200))
            {
                DateTime utc = occurrence.Period.StartTime.AsUtc;
                Console.WriteLine($"{utc:g}, local: {occurrence.Period.StartTime:g}");
            }
        }

        public static string CreateEvent()
        {
            var cal = new Calendar();
            cal.Events.Add(GenerateEvent(DateTime.Parse("2021-07-05 02:00"), 3, "FREQ=MONTHLY;BYSETPOS=1;BYDAY=MO;INTERVAL=1"));
            
            var serializer = new CalendarSerializer(cal);
            var serializedCalendar = serializer.SerializeToString();
            
            Console.WriteLine($"Event.Timezone: {Program.tzdk}");
            
            Console.WriteLine("This should be saved to our new ical event property: ");
            Console.WriteLine(serializedCalendar);
            
            return serializedCalendar;
        }

        public static CalendarEvent GenerateEvent(
            DateTime dt,
            int hours,
            string rrule)
        {
            var e = new CalendarEvent()
            {
                Start = new CalDateTime(dt, Program.tzdk),
                End = new CalDateTime(dt.AddHours(hours), Program.tzdk)
            };

            e.RecurrenceRules.Add(new RecurrencePattern(rrule));
            return e;
        }
    }
}