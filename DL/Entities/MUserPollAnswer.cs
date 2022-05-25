using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Entities
{
   public class MUserPollAnswer : EmptyBaseDomain
    {
      
        public int UserId { get; set; }
      //  public MUser User { get; set; }
        public int PollAnswerId { get; set; }
        
        public MPollAnswer PollAnswer { get; set; }
        public MUser User { get; set; }
    }
}
