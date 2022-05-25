using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserDTOs
{
    public class ForgetPasswordByMobilDTO
    {
   
        public string Mobile { get; set; } 
        public int VerifyCode { get; set; }
        public string NewPassword { get; set; }

   
    }
}
