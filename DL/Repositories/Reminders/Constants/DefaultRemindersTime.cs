using System;

namespace DL.Repositories.Reminders.Constants
{
    public class DefaultRemindersTime
    {
        public static readonly TimeSpan BreakFastTime = 
            new TimeSpan(8, 9, 0);

        public static readonly TimeSpan LunchTime =
            new TimeSpan(11, 50, 0);

        public static readonly TimeSpan DinnerTime =
            new TimeSpan(18, 15, 0);

        public static readonly TimeSpan CheckWeightTime =
            new TimeSpan(17, 40, 0);
    }
}