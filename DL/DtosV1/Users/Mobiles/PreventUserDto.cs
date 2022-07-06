using DL.EntitiesV1.Users;

namespace DL.DtosV1.Users.Mobiles
{
    public class PreventUserDto
    {
        public int UserId { get; set; }
        public MobilePreventionType PreventionType { get; set; }
    }
}