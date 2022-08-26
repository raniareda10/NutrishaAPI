using System;
using System.Collections.Generic;

namespace DL.EntitiesV1.Meals
{
    public class MealEntity : BaseEntityV1
    {
        public string Name { get; set; }
        public MealType MealType { get; set; }
        public string CockingTime { get; set; }
        public string PreparingTime { get; set; }
        public string CoverImage { get; set; }
        public string MealSteps { get; set; }
        public string Allergies { get; set; }
        // public bool IsRecommended { get; set; }
        public ICollection<MealIngredientEntity> Ingredients { get; set; }
    }

    public enum MealType
    {
        Breakfast = 0,
        Lunch = 1,
        Dinner = 2,
        Snack = 3,
        Supplement = 4,
        Water = 5,
        ExtraBites = 6,
        Recommended = 7
    }
}