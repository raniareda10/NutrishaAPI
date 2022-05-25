using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Entities
{
   public class MUserAllergy : EmptyBaseDomain
    {
      
        public int UserId { get; set; }
      //  public MUser User { get; set; }
        public int AllergyId { get; set; }
        public MAllergy Allergy { get; set; }
    }
}
