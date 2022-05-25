using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
   public class MRolesGroup:BaseDomain
    {
        public int RoleId { get; set; }
        public MRole Role { get; set; }
        public int GroupId { get; set; }
        public MGroup Group { get; set; }
    }
}
