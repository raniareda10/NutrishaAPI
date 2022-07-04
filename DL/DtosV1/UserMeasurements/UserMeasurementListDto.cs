using System;
using System.Collections.Generic;
using DL.EntitiesV1.Measurements;

namespace DL.DtosV1.UserMeasurements
{

    public class UserMeasurements
    {
        public MeasurementType Type { get; set; }
        public IEnumerable<UserMeasurementListDto> Measurements { get; set; }
    }
    public class UserMeasurementListDto
    {
        public float MeasurementValue { get; set; }
        public DateTime Created { get; set; }
    }
}