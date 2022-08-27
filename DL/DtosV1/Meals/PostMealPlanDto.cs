using System;
using System.Collections.Generic;

namespace DL.DtosV1.Meals
{
    public class PostMealPlanDto
    {
        public IList<MealPlanModel> Meals { get; set; }
        public int? UserId { get; set; }
        public long? TemplateId { get; set; }
        public DateTime? StartDate { get; set; }
        public string Notes { get; set; }

        public bool IsTemplate { get; set; }
        public string TemplateName { get; set; }
    }
}