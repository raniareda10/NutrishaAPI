using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserDTOs
{
  public  class ChangePasswordDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]

        public string NewPassword { get; set; }
       // [Required]

      //  public string OldPassword { get; set; }
    }
}
