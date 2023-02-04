using System;
using System.Collections.Generic;
using System.Text;
using DL.EntitiesV1;
using DL.EntitiesV1.AdminUser;

namespace DL.Entities
{
    public class MUserRoles : BaseEntity
    {
        public int AdminUserId { get; set; }
        public AdminUserEntity AdminUser { get; set; }
        public int RoleId { get; set; }
        public MRole Role { get; set; }
    }
}