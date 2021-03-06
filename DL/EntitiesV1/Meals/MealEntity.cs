using System;

namespace DL.EntitiesV1.Meals
{
    public class MealEntity : BaseEntityV1
    {
        public string Name { get; set; }
        public MealType MealType { get; set; }
        public TimeSpan CockingTime { get; set; }
        public TimeSpan PreparingTime { get; set; }
        public string CoverImage { get; set; }
        public string MealSteps { get; set; }
        public string Allergies { get; set; }
        public string Ingredients { get; set; }
    }

    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack,
        Supplement
    }
}