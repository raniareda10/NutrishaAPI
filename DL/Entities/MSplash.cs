using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MSplash:EmptyBaseDomain
    {
        [Required]
        [MinLength(3), MaxLength(25)]
        public string Tag { get; set; }
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Logo { get; set; }
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Background { get; set; }

    }
}
