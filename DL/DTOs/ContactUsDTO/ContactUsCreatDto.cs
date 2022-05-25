using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.ContactUsDTO
{
   public class ContactUsCreatDto
    {

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MinLength(3), MaxLength(50)]
        public string Phone { get; set; }
        [Required]
        [MinLength(3), MaxLength(500)]
        public string Message { get; set; }

   
    }
}
