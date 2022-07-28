using System.Collections.Generic;
using DL.Entities;

namespace DL.EntitiesV1.Meals
{
    public class MealPlan : BaseEntityV1
    {
        public int UserId { get; set; }
        public MUser User { get; set; }
        public ICollection<PlanDayEntity> PlanDays { get; set; }
        public string Notes { get; set; }
    }
}