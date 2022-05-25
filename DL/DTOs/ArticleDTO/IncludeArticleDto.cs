using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.ArticleDTO
{
   public class IncludePollDto
    {
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Subject { get; set; }
        [Required]
        public string CoverImage { get; set; }
        public string ArticleContent { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Notes { get; set; }
    }
}
