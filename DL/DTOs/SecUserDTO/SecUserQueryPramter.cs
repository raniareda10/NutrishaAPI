using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.SecUserDTO
{
   public class SecUserQueryPramter : PaginationParameters
    {
        //searching 
        public int Id { get; set; }
    }
}
