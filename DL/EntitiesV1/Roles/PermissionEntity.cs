using System.Collections.Generic;

namespace DL.EntitiesV1.Roles
{
    public class PermissionEntity : BaseEntityV1
    {
        public string Name { get; set; }

        public ICollection<RolePermissionEntity> RolePermissions { get; set; }

        public PermissionEntity Parent { get; set; }
        public long? ParentId { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        // public string DisplayName { get; set; }
        //
        // public string DisplayNameEn { get; set; }
        // public string DisplayNameAr { get; set; }
        //
        // public long ParentId { get; set; }
        // public PermissionEntity Parent { get; set; }
    }
}