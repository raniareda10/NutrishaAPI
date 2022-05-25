using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.ArticleDTO
{
   public class BlogQueryPramter : PaginationParameters
    {
        public int BlogTypeId { get; set; }
    }
}
