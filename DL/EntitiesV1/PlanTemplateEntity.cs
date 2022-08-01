using System.Collections.Generic;
using DL.EntitiesV1.Meals;

namespace DL.EntitiesV1
{
    public class PlanTemplateEntity
    {
        public ICollection<PlanDayEntity> PlanDays { get; set; }
        public string Notes { get; set; }
        public string Name { get; set; }
    }
}