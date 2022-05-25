using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.ShipperDTO
{
   public class ShipperCreatDto
    {


        [Required]
        [MinLength(3), MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MinLength(3), MaxLength(100)]
        public string UserName { get; set; }
        [Required]
        [MinLength(3), MaxLength(100)]
        public string Password { get; set; }
        [Required]
        [MinLength(3), MaxLength(50)]
        public string Mobile { get; set; }
        [MinLength(3), MaxLength(50)]
        public string Email { get; set; }
        [MinLength(3), MaxLength(300)]
        public string Address { get; set; }
       
        public int VehicleTypeId { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public Nullable<decimal> Longitude { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public Nullable<decimal> Latitude { get; set; }
        [MinLength(3), MaxLength(300)]
        public string Notes { get; set; }

    }
}
