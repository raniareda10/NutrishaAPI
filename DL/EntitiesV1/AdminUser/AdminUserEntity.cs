using System;
using System.Collections.Generic;
using DL.Entities;

namespace DL.EntitiesV1.AdminUser
{
    public class AdminUserEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PersonalImage { get; set; }
        public ICollection<MUserRoles> Roles { get; set; }
    }
}