﻿using System;

namespace DL.DtosV1.MealPlans
{
    public class UserPlanTemplateDto
    {
        public DateTime? StartDate { get; set; }
        public string TemplateName { get; set; }
        public long MealPlanId { get; set; }
        public string DoctorNotes { get; set; }
    }
}