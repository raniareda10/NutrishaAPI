using System.Collections.Generic;
using DL.DtosV1.MealPlans;
using DL.Entities;

namespace DL.DtosV1.Users.Mobiles
{
    public class MobileUserDetailsDto : MobileUserListDto
    {
        public IEnumerable<string> Allergies { get; set; }
        public IEnumerable<string> Dislikes { get; set; }
        public IDictionary<string, int> Totals { get; set; }
        public decimal? Height { get; set; }
        public float CurrentWeight { get; set; }
        public float WeightLoss { get; set; }
        public string Gender { get; set; }
        public decimal? Age { get; set; }
        public UserMealPlans UserMealPlans { get; set; }
        public IList<UserPlanTemplateDto> LastUsedTemplates { get; set; }
    }
}