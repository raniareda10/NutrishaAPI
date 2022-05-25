using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DL.DTOs.SharedDTO;
using Microsoft.AspNetCore.Http;
namespace DL.DTOs.UserDTOs
{
    public class UserResDTO
    {

     
        public string Name { get; set; }
        public int Id { get; set; }

        //Uniqe Properties

        public string Mobile { get; set; }
      //  public string Email { get; set; }
        public string Address { get; set; }
    }
}
