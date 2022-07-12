using System;

namespace DL.EntitiesV1.Meals
{
    public class MealEntity : BaseEntityV1
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public TimeSpan CockingTime { get; set; }
        public TimeSpan PreparingTime { get; set; }
        public string ImageUrl { get; set; }
        public string MealStep { get; set; }
        public string Allergies { get; set; }
    }
}