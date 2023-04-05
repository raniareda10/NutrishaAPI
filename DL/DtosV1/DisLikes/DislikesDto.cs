using DL.EntitiesV1;

namespace DL.DtosV1.DisLikes
{
    public class DislikesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public bool IsSelected { get; set; }
        public DislikeMealType DislikeMealType { get; set; }
    }
}