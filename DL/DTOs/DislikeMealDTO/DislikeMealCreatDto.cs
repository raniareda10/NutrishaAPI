using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.DislikeMealDTO
{
   public class DislikeMealCreatDto
    {


        [Required]
        public int MealId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Notes { get; set; }
    }
}
