using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.MealDTO
{
   public class MealCreatDto
    {

        [Required]
        [MinLength(3), MaxLength(250)]

        public string Name { get; set; }
        public string Image { get; set; }
        public string Quantity { get; set; }
        public decimal? kalory { get; set; }
        public string Notes { get; set; }

    }
}
