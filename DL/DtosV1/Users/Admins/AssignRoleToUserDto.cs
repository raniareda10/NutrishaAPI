using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DL.DtosV1.Users.Admins
{
    public class AssignRoleToUserDto
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }
}