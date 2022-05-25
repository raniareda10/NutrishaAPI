using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Entities
{
   public class MUserRoles:BaseDomain
    {
      
        public int UserId { get; set; }
        public MUser User { get; set; }
        public int RoleId { get; set; }
        public MRole Role { get; set; }
    }
}
