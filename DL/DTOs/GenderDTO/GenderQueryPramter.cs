using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.GenderDTO
{
   public class GenderQueryPramter : PaginationParameters
    {
        //searching 
        public string Name { get; set; }
    }
}
