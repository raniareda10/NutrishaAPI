using System;

namespace DL.Extensions
{
    public static class DatetimeExtensions
    {
        public static DateTime SetToStartOfDay(this DateTime startDay)
        {
            return startDay.AddHours(-startDay.Hour)
                .AddMinutes(-startDay.Minute)
                .AddSeconds(-startDay.Second)
                .AddMilliseconds(-startDay.Millisecond);
        }
        
        public static DateTime SetToEndOfDay(this DateTime startDay)
        {
            return startDay.SetToStartOfDay().AddDays(1);
        }
    }
}