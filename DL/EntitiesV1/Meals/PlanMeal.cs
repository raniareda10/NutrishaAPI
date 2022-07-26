using System;
using System.Collections.Generic;

namespace DL.EntitiesV1.Meals
{
    public class PlanMeal : BaseEntityV1
    {
        public long MealId { get; set; }
        public MealType MealType { get; set; }
        public ICollection<MealEntity> Meals { get; set; }
        
        public  PlanMealStatus PlanMealStatus { get; set; }
    }

    public class PlanDay
    {
        public DayOfWeek Day { get; set; }
        public ICollection<PlanMeal> PlanMeals { get; set; }
    }
    
    public class Plan
    public enum PlanMealStatus
    {
        Skipped,
        Eaten,
        Swapped
    }
}