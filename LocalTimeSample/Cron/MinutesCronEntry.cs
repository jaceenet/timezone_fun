namespace Gluu.TaskList.Cron
{
    public class MinutesCronEntry : CronEntryBase
    {
        public MinutesCronEntry(string expression)
        {
            Initialize(expression, 0, 59);
        }
    }
}