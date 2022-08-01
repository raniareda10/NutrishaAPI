using System;
using System.Collections.Generic;
using System.Text;
using DL.EntitiesV1;

namespace DL.Entities
{
    public class MUserRoles : BaseEntity
    {
        public int UserId { get; set; }
        public MUser User { get; set; }
        public int RoleId { get; set; }
        public MRole Role { get; set; }
    }
}