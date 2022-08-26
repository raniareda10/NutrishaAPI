using System;
using System.Collections.Generic;

namespace DL.DtosV1.MealPlans
{
    public class PlanDayDto
    {
        public long Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public IEnumerable<object> Menus { get; set; }
    }
}