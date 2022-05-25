using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DL.DTOs.SharedDTO;
using Microsoft.AspNetCore.Http;
namespace DL.DTOs.UserDTOs
{
    public class UserMobileEmaiDTO
    {


        public string Mobile { get; set; }
  
        public string Email { get; set; }
    }
}
