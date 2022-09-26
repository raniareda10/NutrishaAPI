using DL.Entities;

namespace DL.DtosV1.Users.Mobiles
{
    public class AddAfterSubscriptionDetails
    {
        public ActivityLevel ActivityLevel { get; set; }
        public string NumberOfMealsPerDay { get; set; }
        public EatReasonFeel EatReason { get; set; }
        public float TargetWeight { get; set; }
        public string MedicineNames { get; set; }
        public bool IsRegularMeasurer { get; set; }
        public bool HasBaby { get; set; }
    }
}