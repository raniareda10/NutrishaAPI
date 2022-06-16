using DL.Entities;

namespace DL.EntitiesV1.Allergies
{
    public class UserAllergy : BaseEntityV1
    {
        public int UserId { get; set; }
        public MUser User { get; set; }
        public bool IsSelected { get; set; }
        public string Title { get; set; }
        public bool IsCreatedByUser { get; set; }
    }
}