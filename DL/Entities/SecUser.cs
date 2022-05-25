using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
  public  class SecUser:EmptyBaseDomain
    {

        /// <summary>
        /// User Nae
        /// </summary>
        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }
       
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [MinLength(3), MaxLength(300)]
        public string Notes { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int HospitalId { get; set; } = 0;
        public int? DoctorId { get; set; } = 0;
        public int? SafeId { get; set; } = 0;

    }
}
