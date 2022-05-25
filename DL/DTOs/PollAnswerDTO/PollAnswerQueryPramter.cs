using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.PollAnswerDTO
{
   public class PollAnswerQueryPramter : PaginationParameters
    {
        //searching 
        public string Name { get; set; }
    }
}
