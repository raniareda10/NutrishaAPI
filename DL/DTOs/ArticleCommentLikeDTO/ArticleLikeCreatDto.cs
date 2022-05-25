using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ArticleLikeDTO
{
   public class ArticleCommentLikeCreatDto
    {


        [Required]
        public int ArticleCommentId { get; set; }
    }
}
