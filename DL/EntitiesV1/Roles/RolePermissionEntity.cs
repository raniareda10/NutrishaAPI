
using DL.Entities;

namespace DL.EntitiesV1.Roles
{
    public class RolePermissionEntity : BaseEntityV1
    {
        public long PermissionId { get; set; }
        public PermissionEntity Permission { get; set; }
        public int RoleId { get; set; }
        public MRole Role { get; set; }
    }
}