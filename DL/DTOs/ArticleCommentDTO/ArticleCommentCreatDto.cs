using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ArticleCommentDTO
{
   public class ArticleCommentCreatDto
    {
        [Required]
        public int ArticleId { get; set; }

        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; }

    }
}
