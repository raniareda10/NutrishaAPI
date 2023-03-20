using System;

namespace DL.DtosV1.Users.Admins
{
    public class ManualAppSubscribeRequest
    {
        public int UserId { get; set; }
        public DateTime EndDate { get; set; }
        public double AmountPaid { get; set; }
    }
}