using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
  public  class MContactUs : BaseDomain
    {
      
        /// <summary>
        /// User Nae
        /// </summary>
        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [MinLength(3), MaxLength(50)]
        public string Phone { get; set; }
        [MinLength(3), MaxLength(500)]
        public string Message { get; set; }
    }
}
