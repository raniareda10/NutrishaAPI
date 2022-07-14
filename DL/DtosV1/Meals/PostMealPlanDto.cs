using System;
using System.Collections.Generic;

namespace DL.DtosV1.Meals
{
    public class PostMealPlanDto
    {
        public IEnumerable<MealPlanModel> Meals { get; set; }
        public int UserId { get; set; }
        public string Notes { get; set; }
    }

    public class MealPlanModel
    {
        public DayOfWeek Day { get; set; }
        public IEnumerable<long> MealIds { get; set; }
    }
}