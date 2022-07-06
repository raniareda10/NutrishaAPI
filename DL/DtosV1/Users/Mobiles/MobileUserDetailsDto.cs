using System.Collections.Generic;
using DL.Entities;

namespace DL.DtosV1.Users.Mobiles
{
    public class MobileUserDetailsDto : MobileUserListDto
    {
        public IEnumerable<string> Allergies { get; set; }
        public IEnumerable<string> Dislikes { get; set; }
        public IDictionary<string, int> Totals { get; set; }
        
    }
}