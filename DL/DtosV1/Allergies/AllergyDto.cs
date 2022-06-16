namespace DL.DtosV1.Allergies
{
    public class AllergyDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCreatedByUser { get; set; }
    }
}