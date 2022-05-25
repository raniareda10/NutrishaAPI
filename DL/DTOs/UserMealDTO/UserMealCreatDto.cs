using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.UserMealDTO
{
   public class UserMealCreatDto
    {


        [Required]
        public int MealId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MealTypeId { get; set; }
        public string Notes { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public bool Eaten { get; set; }
        public bool Taken { get; set; }
    }
}
