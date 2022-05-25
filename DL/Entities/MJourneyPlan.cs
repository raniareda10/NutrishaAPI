using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
   public class MJourneyPlan : EmptyBaseDomain
    {
        [Required]
        [MinLength(3), MaxLength(100)]
        public string Name { get; set; }
        public string Notes { get; set; }
    }
}
