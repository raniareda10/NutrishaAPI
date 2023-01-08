using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Entities
{
    public class MUserRisk : EmptyBaseDomain
    {
        public int UserId { get; set; }

        //  public MUser User { get; set; }
        public int RiskId { get; set; }
        public MRisk Risk { get; set; }
    }
}