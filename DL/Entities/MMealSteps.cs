using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MMealSteps : EmptyBaseDomain
    {

        [Required]
        public int StepsId { get; set; }
        [Required]
        public int MealId { get; set; }
        [Required]
        public int OrderNo { get; set; }

        public MFoodSteps Steps { get; set; }
    }
}
