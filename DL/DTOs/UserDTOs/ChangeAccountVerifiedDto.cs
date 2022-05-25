using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserDTOs
{
  public  class ChangeAccountVerifiedDto
    {
    
        public int? UserId { get; set; }
   
    //    public string Mobile { get; set; } = string.Empty;
     
        public bool IsAccountVerified { get; set; } = false;
        // [Required]

        //  public string OldPassword { get; set; }
    }
}
