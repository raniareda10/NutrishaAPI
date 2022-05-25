using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.NotificationUserDTO
{
   public class NotificationUserQueryPramter : PaginationParameters
    {

        //searching 
      //  public DateTime Date { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
    }
}
