using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.GoalDTO
{
   public class GoalQueryPramter : PaginationParameters
    {

        //searching 
      //  public DateTime Date { get; set; }
        public int GoalTypeId { get; set; }
        public int FrequencyId { get; set; }
    }
}
