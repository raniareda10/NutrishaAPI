using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MDislikeMeal : EmptyBaseDomain
    {
        [Required]
        public int UserId { get; set; }
        //  public MUser User { get; set; }
        [Required]
        public int MealId { get; set; }
        public string Notes { get; set; }

        public MMeal Meal { get; set; }

    }
}
