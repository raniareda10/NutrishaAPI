using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.BlogTypeDTO
{
   public class BlogTypeCreatDto
    {

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }

    }
}
