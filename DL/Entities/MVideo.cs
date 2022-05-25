using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MVideo : BaseDomain
    {
        [Required]
        public int BlogTypeId { get; set; }
        [Required]
        public int AttachmentTypeId { get; set; }
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Title { get; set; }
   
        public string Video { get; set; }
        public string Link { get; set; }
        public string CoverImage { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Notes { get; set; }
        [Required]
        public int SecUserId { get; set; }
    }
}
