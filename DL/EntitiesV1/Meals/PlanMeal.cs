using System;

namespace DL.EntitiesV1.Meals
{
    public class PlanMeal : BaseEntityV1
    {
        public long MealId { get; set; }
        public MealEntity Meal { get; set; }

        public DayOfWeek Day { get; set; }
    }
}