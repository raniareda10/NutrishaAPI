using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.NotificationDTO
{
   public class NotificationQueryPramter : PaginationParameters
    {

        //searching 
        public string Message { get; set; }
        public int SourceId { get; set; }
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public int? AdminId { get; set; }
    }
}
