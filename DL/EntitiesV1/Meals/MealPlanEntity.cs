using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DL.Entities;

namespace DL.EntitiesV1.Meals
{
    public class MealPlanEntity : BaseEntityV1
    {
        public int CreatedById { get; set; }
        public MUser CreatedBy { get; set; }
        public ICollection<PlanDayEntity> PlanDays { get; set; }
        public string Notes { get; set; }

        // User Plan Fields
        public int? UserId { get; set; }
        public MUser User { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        // Template Fields
        public string TemplateName { get; set; }
        public bool IsTemplate { get; set; }
    }
}