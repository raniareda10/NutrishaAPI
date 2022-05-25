using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.ArticleAttachmentDTO
{
   public class IncludeArticleAttachmentDto
    {
        public int AttachmentTypeId { get; set; }
        public string Url { get; set; }
    }
}
