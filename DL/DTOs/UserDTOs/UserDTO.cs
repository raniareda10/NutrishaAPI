using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace DL.DTOs.UserDTOs
{
    public class UserDTO
    {

        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
        public int? DeviceTypeId { get; set; }
        public string DeviceToken { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;

    }
}
