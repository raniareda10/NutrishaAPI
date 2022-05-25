using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserMealDTO
{
   public class UserMealQueryPramter : PaginationParameters
    {

        public int MealId { get; set; }
        public int UserId { get; set; }
    }
}
