using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserGoalDTO
{
   public class UserGoalQueryPramter : PaginationParameters
    {

        //searching 
      //  public DateTime Date { get; set; }
        public int GoalId { get; set; }
        public int UserId { get; set; }
    }
}
