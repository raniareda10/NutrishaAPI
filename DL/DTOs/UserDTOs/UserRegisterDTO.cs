using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTO
{
   public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
    
        [Required]

        public string MobileNum { get; set; }


        public string Email { get; set; } = string.Empty;
        [Required]

        public string Password { get; set; } 



    }
}
