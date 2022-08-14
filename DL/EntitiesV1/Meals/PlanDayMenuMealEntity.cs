using System;

namespace DL.EntitiesV1.Meals
{
    public class PlanDayMenuMealEntity : BaseEntityV1
    {
        public long? MealId { get; set; }
        public MealEntity Meal { get; set; }
        public string MealName { get; set; }
        public long PlanDayMenuId { get; set; }
        public PlanDayMenuEntity PlanDayMenu { get; set; }

        public bool IsEaten { get; set; }
    }
}