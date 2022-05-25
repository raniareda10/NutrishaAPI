using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MArticleCommentLike : BaseDomain
    {

        [Required]
        public int UserId { get; set; }
        //  public MUser User { get; set; }
        [Required]
        public int ArticleCommentId { get; set; }
        public string Notes { get; set; }
    }
}
