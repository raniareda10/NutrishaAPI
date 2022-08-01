using System;
using DL.EntitiesV1.Meals;

namespace DL.DtosV1.MealPlans
{
    public class SwapMealDto
    {
        public long PlanId { get; set; }
        public MealType Type { get; set; }
        public DayOfWeek Day { get; set; }
    }
}