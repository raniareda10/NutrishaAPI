using DL.Entities;

namespace DL.EntitiesV1.Users
{
    public class MobileUserPreventionEntity : BaseEntityV1
    {
        public MobilePreventionType PreventionType { get; set; }
        public int UserId { get; set; }
        public MUser User { get; set; }
    }
}