using System;
using DL.Entities;

namespace DL.EntitiesV1.Subscriptions
{
    public class SubscriptionInfoEntity
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public MUser User { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}