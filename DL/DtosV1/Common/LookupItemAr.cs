namespace DL.DtosV1.Common
{
    public class LookupItemAr
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public LookupItemAr(long id, string name, string nameAr)
        {
            Id = id;
            Name = name;
            NameAr = nameAr;
        }
    }
}