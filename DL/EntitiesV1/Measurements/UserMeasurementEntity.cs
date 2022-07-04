using DL.Entities;

namespace DL.EntitiesV1.Measurements
{
    public class UserMeasurementEntity : BaseEntityV1
    {
        public MeasurementType MeasurementType { get; set; }
        public float MeasurementValue { get; set; }
        public int UserId { get; set; }
        public MUser User { get; set; }
    }
}