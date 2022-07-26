using System.Collections.Generic;
using DL.Entities;

namespace DL.EntitiesV1.Meals
{
    public class MealPlan : BaseEntityV1
    {
        public MUser User { get; set; }
        public int UserId { get; set; }

        public  ICollection<PlanDay> Days { get; set; }
        // public string Notes { get; set; }
    }
}