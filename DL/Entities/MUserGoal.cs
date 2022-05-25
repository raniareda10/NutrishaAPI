using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MUserGoal:BaseDomain
    {
      
        public int UserId { get; set; }
        public MUser User { get; set; }
        public int GoalId { get; set; }
        public MGoal Goal { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public bool Done { get; set; } = false;
        public string Notes { get; set; }
    }
}
