using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.GoalDTO
{
   public class GoalCreatDto
    {
        [Required]
        [MinLength(3), MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public int GoalTypeId { get; set; }
        [Required]
        public int FrequencyId { get; set; }

        public string Notes { get; set; }
    }
}
