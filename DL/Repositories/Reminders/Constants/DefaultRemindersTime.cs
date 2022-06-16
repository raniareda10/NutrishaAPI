using System;

namespace DL.Repositories.Reminders.Constants
{
    public class DefaultRemindersTime
    {
        public static readonly DateTime BreakFastTime =
            new DateTime(1997, 1, 1, 8, 9, 0, DateTimeKind.Utc);

        public static readonly DateTime LunchTime =
            new DateTime(1997, 1, 1, 11, 50, 0, DateTimeKind.Utc);

        public static readonly DateTime DinnerTime =
            new DateTime(1997, 1, 1, 18, 15, 0, DateTimeKind.Utc);

        public static readonly DateTime CheckWeightTime =
            new DateTime(1997, 1, 1, 17, 40, 0, DateTimeKind.Utc);
    }
}