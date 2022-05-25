using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace DL.DTOs.UserDTOs
{
    public class UserWithTokenDTO
    {
    
        public UserTokenDTO data { get; set; }
    
        public int statusCode { get; set; } = 200;
        public bool done { get; set; } = true;
        public string errorMessage { get; set; }
    }
}
