using System.Collections.Generic;

namespace DL.DtosV1.Allergies
{
    public class PutAllergyDto
    {
        public HashSet<long> AllergyIds { get; set; }
    }
}