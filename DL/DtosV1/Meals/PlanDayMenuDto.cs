using System.Collections.Generic;
using DL.EntitiesV1.Meals;

namespace DL.DtosV1.Meals
{
    public class PlanDayMenuDto
    {
        public MealType Type { get; set; }
        public IEnumerable<long> MealIds { get; set; }
    }
}