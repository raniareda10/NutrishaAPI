using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
   public class MPollAnswer : EmptyBaseDomain
    {
        [Required]
        public int PollId { get; set; }
        [Required]
        [MinLength(3), MaxLength(100)]
        public string Name { get; set; }
        public string Color { get; set; }
        public string Notes { get; set; }
    }
}
