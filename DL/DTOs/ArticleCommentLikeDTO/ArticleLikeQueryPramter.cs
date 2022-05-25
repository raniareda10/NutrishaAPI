using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.ArticleCommentLikeDTO
{
   public class ArticleCommentLikeQueryPramter : PaginationParameters
    {

        public int ArticleCommentId { get; set; }
        public int UserId { get; set; }
    }
}
