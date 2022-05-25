using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MRole: BaseDomain
    {
      

        /// <summary>
        /// Role Name 
        /// </summary>
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

    }
}
