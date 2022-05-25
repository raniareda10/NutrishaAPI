using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.ArticleLikeDTO
{
   public class ArticleLikeQueryPramter : PaginationParameters
    {

        public int ArticleId { get; set; }
        public int UserId { get; set; }
    }
}
