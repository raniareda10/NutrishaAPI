using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.MealIngredientDTO
{
   public class MealIngredientQueryPramter : PaginationParameters
    {

        public int IngredientId { get; set; }
        public int MealId { get; set; }
    }
}
