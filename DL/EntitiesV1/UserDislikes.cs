using DL.Entities;

namespace DL.EntitiesV1
{
    public class UserDislikes : BaseEntityV1
    {
        public int UserId { get; set; }
        public MUser User { get; set; }
        public bool IsSelected { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public DislikeMealType DislikeType { get; set; }
    }
}