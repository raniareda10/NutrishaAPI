using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.UserRiskDTO
{
   public class UserRiskCreatDto
    {


        [Required]
        public int RiskId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string Notes { get; set; }
    }
}
