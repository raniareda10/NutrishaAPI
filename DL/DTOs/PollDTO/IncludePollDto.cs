using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.PollDTO
{
   public class IncludePollDto
    {
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Question { get; set; }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; }
    }
}
