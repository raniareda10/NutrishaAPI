using System;
using DL.Entities;

namespace DL.DtosV1.Users.Mobiles
{
    public class MobileUserListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public DateTime Created { get; set; }
        public DateTime? SubscribeDate { get; set; }
        public int? TotalPaidAmount { get; set; }

    }
}