using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.GoalDTO
{
   public class IncludeGoalDto
    {
        [Required]
        [MinLength(3), MaxLength(100)]
        public string Title { get; set; }
        public string Notes { get; set; }
        public int GoalTypeId { get; set; }
        public int FrequencyId { get; set; }
    }
}
