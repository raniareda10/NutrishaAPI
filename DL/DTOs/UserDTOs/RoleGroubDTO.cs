using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserDTOs
{
   public class RoleGroupDTO
    {
        [Required]

        public int RoleId { get; set; }
        [Required]

        public int GroupId { get; set; }

    }
}
