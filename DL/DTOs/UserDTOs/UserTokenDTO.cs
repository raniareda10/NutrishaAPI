using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace DL.DTOs.UserDTOs
{
    public class UserTokenDTO
    {
    
        public AllUserDTO User { get; set; }

        public string Token { get; set; }

    }
}
