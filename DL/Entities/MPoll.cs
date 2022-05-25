using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MPoll : BaseDomain
    {
        [Required]
        public int BlogTypeId { get; set; }

        [Required]
        [MinLength(3), MaxLength(250)]
        public string Question { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Color { get; set; }
        public string Notes { get; set; }
        [Required]
        public int SecUserId { get; set; }
    }
}
