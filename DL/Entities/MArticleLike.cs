using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MArticleLike : BaseDomain
    {

        [Required]
        public int UserId { get; set; }
        //  public MUser User { get; set; }
        [Required]
        public int ArticleId { get; set; }
        public string Notes { get; set; }
        public MArticle Article { get; set; }
    }
}
