using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.MealStepsDTO
{
   public class MealStepsQueryPramter : PaginationParameters
    {

        public int StepsId { get; set; }
        public int MealId { get; set; }
    }
}
