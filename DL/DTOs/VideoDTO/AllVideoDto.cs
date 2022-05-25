using DL.DTOs.ArticleCommentDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.VideoDTO
{
   public class AllVideoDto
    {
        public int Id { get; set; }
        [Required]
        public int BlogTypeId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Video { get; set; }
        public string Link { get; set; }
        public string CoverImage { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Notes { get; set; }
       // public int SecUserId { get; set; }
        public string AdminName { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public List<MArticleComment> LstArticleComment { get; set; }
    }
}
