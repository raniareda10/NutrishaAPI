using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.SplashDTO
{
   public class SplashQueryPramter : PaginationParameters
    {
        //searching 
        public string Tag { get; set; }
    }
}
