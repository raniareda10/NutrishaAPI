using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MMealIngredient : EmptyBaseDomain
    {

        [Required]
        public int IngredientId { get; set; }
        [Required]
        public int MealId { get; set; }
        [Required]
        public int OrderNo { get; set; }
        public MIngredient Ingredient { get; set; }
    }
}
