using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserDTOs
{
    public class ForgetPasswordDTO
    {
     //   public int UserId { get; set; }
  
   
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int VerifyCode { get; set; }
        public string NewPassword { get; set; }

   
    }
}
