using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.AttachmentTypeDTO
{
   public class AttachmentTypeCreatDto
    {

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }

    }
}
