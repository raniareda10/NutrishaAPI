using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserRiskDTO
{
   public class UserRiskQueryPramter : PaginationParameters
    {

        public int RiskId { get; set; }
        public int UserId { get; set; }
    }
}
