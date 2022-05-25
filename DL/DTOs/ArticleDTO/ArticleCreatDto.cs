using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ArticleDTO
{
   public class ArticleCreatDto
    {
        [Required]
        public int BlogTypeId { get; set; }
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Subject { get; set; }
        [Required]
        public IFormFile CoverImage { get; set; }
        public string ArticleContent { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Notes { get; set; }
        public int SecUserId { get; set; }
    }
}
