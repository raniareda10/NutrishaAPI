using System;
using DL.Entities;

namespace DL.EntitiesV1.Blogs.Polls
{
    public class PollAnswer : BaseEntityV1
    {
        public long? PollId { get; set; }
        public Poll Poll { get; set; }
        public long? PollQuestionId { get; set; }
        public PollQuestion PollQuestion { get; set; }
        
        public int UserId { get; set; }
        public MUser User { get; set; }
    }
}