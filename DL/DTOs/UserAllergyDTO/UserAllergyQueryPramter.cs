using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserAllergyDTO
{
   public class UserAllergyQueryPramter : PaginationParameters
    {

        public int AllergyId { get; set; }
        public int UserId { get; set; }
    }
}
