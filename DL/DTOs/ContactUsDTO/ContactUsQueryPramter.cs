using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.ContactUsDTO
{
   public class ContactUsQueryPramter : PaginationParameters
    {

        //searching 
        public string Name { get; set; }



    }
}
