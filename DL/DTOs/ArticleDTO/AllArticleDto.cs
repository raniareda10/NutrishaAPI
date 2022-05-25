using DL.DTOs.ArticleAttachmentDTO;
using DL.DTOs.ArticleCommentDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ArticleDTO
{
   public class AllArticleDto
    {
        public int Id { get; set; }
        public int BlogTypeId { get; set; }
        public int MediaTypeId { get; set; }
        public string Subject { get; set; }
        public string CoverImage { get; set; }
        public string ArticleContent { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public int LikesCount { get; set; }
        public bool HasLike { get; set; }
        public int CommentsCount { get; set; }
        public bool HasComment { get; set; }
        public List<IncludeArticleAttachmentDto> attachment { get; set; }
        public string Notes { get; set; }
        public OwnerDto owner { get; set; }
        public List<IncludeArticleCommentDto> LstArticleComment { get; set; }
    }
}
