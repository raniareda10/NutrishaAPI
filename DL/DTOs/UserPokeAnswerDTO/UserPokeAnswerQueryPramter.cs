using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserPollAnswerDTO
{
   public class UserPollAnswerQueryPramter : PaginationParameters
    {

        public int PollId { get; set; }
        public int UserId { get; set; }
    }
}
