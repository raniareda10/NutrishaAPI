using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DL.Entities;
using DL.EntitiesV1.AdminUser;

namespace DL.EntitiesV1.Meals
{
    public class MealPlanEntity : BaseEntityV1
    {
        public int CreatedById { get; set; }
        public AdminUserEntity CreatedBy { get; set; }
        public ICollection<PlanDayEntity> PlanDays { get; set; }
        public string Notes { get; set; }

        // User Plan Fields
        public int? UserId { get; set; }
        public MUser User { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public long? ParentTemplateId { get; set; }
        public MealPlanEntity ParentTemplate { get; set; }
        public byte NumberOfIAmHungryClicked { get; set; }
        
        // Template Fields
        public string TemplateName { get; set; }
        public bool IsTemplate { get; set; }
    }
}