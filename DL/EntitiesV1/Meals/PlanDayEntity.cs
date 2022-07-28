using System;
using System.Collections.Generic;
using DL.Entities;

namespace DL.EntitiesV1.Meals
{
    public class PlanDayEntity : BaseEntityV1
    {
        public DayOfWeek Day { get; set; }
        public ICollection<PlanDayMenuEntity> PlanMeals { get; set; }
        public long MealPlanId { get; set; }
        public MealPlan MealPlan { get; set; }
    }
}