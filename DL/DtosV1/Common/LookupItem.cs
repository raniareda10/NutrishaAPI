namespace DL.DtosV1.Common
{
    public class LookupItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public LookupItem(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}