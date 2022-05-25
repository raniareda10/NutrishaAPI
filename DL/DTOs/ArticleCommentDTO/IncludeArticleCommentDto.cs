using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.ArticleCommentDTO
{
   public class IncludeArticleCommentDto
    {
        public string Comment { get; set; }
        public int LikesCount { get; set; }
        public bool HasLike { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
