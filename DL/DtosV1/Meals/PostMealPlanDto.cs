using System;
using System.Collections.Generic;
using DL.EntitiesV1.Meals;

namespace DL.DtosV1.Meals
{
    public class PostMealPlanDto
    {
        public IList<MealPlanModel> Meals { get; set; }
        public int? UserId { get; set; }
        public string Notes { get; set; }

        public bool IsTemplate { get; set; }
        public string TemplateName { get; set; }
    }

    public class MealPlanModel
    {
        public DayOfWeek Day { get; set; }
        public IEnumerable<PlanDayMenuDto> Menus { get; set; }
        // public string Notes { get; set; }
    }

    public class PlanDayMenuDto
    {
        public MealType Type { get; set; }
        public IEnumerable<long> MealIds { get; set; }
    }
}