using DL.DTOs.ArticleCommentDTO;
using DL.DTOs.PollDTO;
using DL.DTOs.VideoDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ArticleDTO
{
   public class AllBlogDto
    {
        public int Id { get; set; }

        public int BlogEntityTypeId { get; set; }
        public string Subject { get; set; }
        public string CoverImage { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public ArticleDto article { get; set; }
        public PollDto poll { get; set; }
        public VideoDto video { get; set; }
        public OwnerDto owner { get; set; }
    }
}
