using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.UserGoalDTO
{
   public class UserGoalCreatDto
    {
       
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public bool Done { get; set; } = false;

        [Required]
        public int GoalId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Notes { get; set; }
    }
}
