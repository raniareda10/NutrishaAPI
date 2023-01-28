using System.Collections;
using System.Collections.Generic;
using DL.DtosV1.Users.Roles;
using DL.Entities;
using Newtonsoft.Json;

namespace DL.Repositories.Users.Models
{
    public class AdminUserModel
    {
        public long Id { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string PersonalImage { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; }
        public string RoleName { get; set; }
    }
}