using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MArticle : BaseDomain
    {
        [Required]
        public int BlogTypeId { get; set; }
        
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Subject { get; set; }
        [Required]
        public string CoverImage { get; set; }
        public string ArticleContent { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Notes { get; set; }
        [Required]
        public int SecUserId { get; set; }
    }
}
