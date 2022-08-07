using System;
using DL.Entities;

namespace DL.EntitiesV1.Meals
{
    public class UserFavoriteMealEntity
    {
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public MUser User { get; set; }
        public long MealId { get; set; }
        public MealEntity Meal { get; set; }
    }
}