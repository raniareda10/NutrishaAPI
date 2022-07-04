using DL.EntitiesV1.Measurements;

namespace DL.DtosV1.UserMeasurements
{
    public class PostUserMeasurement
    {
        public MeasurementType MeasurementType { get; set; }
        public float MeasurementValue { get; set; }
    }
}