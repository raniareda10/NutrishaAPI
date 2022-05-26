using System.Collections.Generic;
using DL.EntitiesV1.Blogs.Polls;

namespace DL.DtosV1.Blogs
{
    public class PollTimelineResult
    {
        public long? SelectedAnswerId { get; set; }
        public IEnumerable<PollQuestion> Questions { get; set; }
    }
}