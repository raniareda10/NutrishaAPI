using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.MediaTypeDTO
{
   public class MediaTypeQueryPramter : PaginationParameters
    {
        //searching 
        public string Name { get; set; }
    }
}
