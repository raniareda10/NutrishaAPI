using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.ArticleAttachmentDTO
{
   public class ArticleAttachmentQueryPramter : PaginationParameters
    {

        public int ArticleId { get; set; }
        //  public MUser User { get; set; }
        public int AttachmentTypeId { get; set; }
      
    }
}
