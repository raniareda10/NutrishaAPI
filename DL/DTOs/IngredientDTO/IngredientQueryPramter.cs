using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.IngredientDTO
{
   public class IngredientQueryPramter : PaginationParameters
    {
        //searching 
        public string Name { get; set; }
    }
}
