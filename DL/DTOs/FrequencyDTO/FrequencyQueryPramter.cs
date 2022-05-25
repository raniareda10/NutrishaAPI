using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.FrequencyDTO
{
   public class FrequencyQueryPramter : PaginationParameters
    {
        //searching 
        public string Name { get; set; }
    }
}
