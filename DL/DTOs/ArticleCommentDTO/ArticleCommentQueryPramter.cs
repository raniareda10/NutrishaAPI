using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.ArticleCommentDTO
{
   public class ArticleCommentQueryPramter : PaginationParameters
    {

        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string Comment { get; set; }
        public int ArticleId { get; set; }
    }
}
