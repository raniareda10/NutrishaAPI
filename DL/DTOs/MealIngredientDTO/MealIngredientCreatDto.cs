using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.MealIngredientDTO
{
   public class MealIngredientCreatDto
    {

        [Required]
        public int IngredientId { get; set; }
        [Required]
        public int MealId { get; set; }
        [Required]
        public int OrderNo { get; set; }
    }
}
