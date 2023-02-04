using DL.Entities;
using DL.HelperInterfaces;

namespace DL.EntitiesV1.Payments
{
    public class PaymentHistoryEntity : BaseEntityV1
    {
        public int UserId { get; set; }
        public MUser User { get; set; }
        public string PaymentId { get; set; }
        public string Type { get; set; }
        public double? Price { get; set; }
        public string Currency { get; set; }
        public string Event { get; set; }
        public bool IsHandled { get; set; }
    }
}