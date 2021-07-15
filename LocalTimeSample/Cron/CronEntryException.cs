using System;

namespace Gluu.TaskList.Cron
{
    public class CronEntryException : Exception
    {
        public CronEntryException(string message)
            : base(message)
        {

        }
    }
}