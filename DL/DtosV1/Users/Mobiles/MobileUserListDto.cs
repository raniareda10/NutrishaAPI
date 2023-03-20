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
        
        public DateTime? SubscriptionDate { get; set; }
        public string SubscriptionType { get; set; }
        public bool IsSubscribed { get; set; }
        public double? TotalPaidAmount { get; set; }
        public bool IsManuallySubscribed { get; set; }
    }
    
    
}