using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.GoalDTO
{
   public class AllGoalDto
    {
        [Required]
        [MinLength(3), MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public int GoalTypeId { get; set; }
        [Required]
        public int FrequencyId { get; set; }
        public string Notes { get; set; }

        public int GoalId { get; set; }
        public int UserId { get; set; }
    }
}
