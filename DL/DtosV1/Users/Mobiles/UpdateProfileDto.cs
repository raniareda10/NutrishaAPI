using System;
using Microsoft.AspNetCore.Http;

namespace DL.DtosV1.Users.Mobiles
{
    public class UpdateProfileDto
    {
        public IFormFile PersonalImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public int GenderId { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public DateTime BirthDate { get; set; }
    }
}