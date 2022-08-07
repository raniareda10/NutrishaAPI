using System;
using System.Collections.Generic;

namespace DL.DtosV1.Meals
{
    public class MealPlanModel
    {
        public DayOfWeek Day { get; set; }
        public IEnumerable<PlanDayMenuDto> Menus { get; set; }
    }
}