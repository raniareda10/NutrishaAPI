using System.Collections.Generic;

namespace DL.EntitiesV1.Meals
{
    public class PlanDayMenuEntity : BaseEntityV1
    {
        public MealType MealType { get; set; }
        public ICollection<PlanDayMenuMealEntity> Meals { get; set; }

        public long PlanDayId { get; set; }
        public PlanDayEntity PlanDay { get; set; }
    }
}