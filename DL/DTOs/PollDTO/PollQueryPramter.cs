using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.PollDTO
{
   public class PollQueryPramter : PaginationParameters
    {

        public int BlogTypeId { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string Question { get; set; }
    }
}
