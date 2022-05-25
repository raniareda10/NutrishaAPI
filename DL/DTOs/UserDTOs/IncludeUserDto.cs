using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.UserDTO
{
   public class IncludeUserDto
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
      //  public string Email { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Notes { get; set; }

    }
}
