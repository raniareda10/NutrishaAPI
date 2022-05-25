using DL.DTOs.ArticleAttachmentDTO;
using DL.DTOs.ArticleCommentDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ArticleDTO
{
   public class ArticleDto
    {

        public string ArticleContent { get; set; }
        public int LikesCount { get; set; }
        public bool HasLike { get; set; }
        public int CommentsCount { get; set; }
        public bool HasComment { get; set; }
        public List<IncludeArticleAttachmentDto> attachment { get; set; }
    }
}
