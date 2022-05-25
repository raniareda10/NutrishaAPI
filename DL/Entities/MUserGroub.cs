using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
   public class MUserGroup:BaseDomain
    {
        public int UserId { get; set; }
        public MUser User { get; set; }
        public int GroupId { get; set; }
        public MGroup Group { get; set; }
    }
}
