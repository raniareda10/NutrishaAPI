using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.MealStepsDTO
{
   public class MealStepsCreatDto
    {

        [Required]
        public int StepsId { get; set; }
        [Required]
        public int MealId { get; set; }
        [Required]
        public int OrderNo { get; set; }
    }
}
