using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.UserAllergyDTO
{
   public class UserAllergyCreatDto
    {


        [Required]
        public int AllergyId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Notes { get; set; }
    }
}
