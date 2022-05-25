using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ArticleAttachmentDTO
{
   public class ArticleAttachmentCreatDto
    {
        [Required]
        public int ArticleId { get; set; }
        //  public MUser User { get; set; }
        public int AttachmentTypeId { get; set; }
        [Required]
        public string Url { get; set; }
        public string Notes { get; set; }
    }
}
