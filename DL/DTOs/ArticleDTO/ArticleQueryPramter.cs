using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.ArticleDTO
{
   public class ArticleQueryPramter : PaginationParameters
    {

        //searching 
        public int BlogTypeId { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string Subject { get; set; }

    }
}
