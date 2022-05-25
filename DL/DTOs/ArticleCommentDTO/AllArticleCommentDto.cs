using DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ArticleCommentDTO
{
   public class AllArticleCommentDto
    {
        public int Id { get; set; }
        [Required]
        public int ArticleId { get; set; }
        //  public MUser User { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; }
        [Required]
        public int UserId { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }

        public List<MArticleComment> LstArticleCommentReply { get; set; }
    }
}
