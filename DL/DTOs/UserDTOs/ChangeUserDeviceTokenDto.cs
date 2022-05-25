using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.UserDTOs
{
  public  class ChangeUserDeviceTokenDto
    {
    
        public int? UserId { get; set; }

        public int? DeviceTypeId { get; set; }
        public string DeviceToken { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
