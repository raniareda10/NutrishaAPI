using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DL.DTOs.UserDTOs
{
    public class FireBaseUserDTO
    {
        public UserStatusDTO approval { get; set; }
    }
}
