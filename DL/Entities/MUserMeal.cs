using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MUserMeal : EmptyBaseDomain
    {

        [Required]
        public int MealId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MealTypeId { get; set; }
        public string Notes { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool Eaten { get; set; }
        public bool Taken { get; set; }
        public MMeal Meal { get; set; }
    }
}
