using System.Collections.Generic;
using DL.DtosV1.Polls;
using DL.EntitiesV1.Blogs.Polls;

namespace DL.DtosV1.Blogs
{
    public class PollTimelineResult
    {
        public long? SelectedAnswerId { get; set; }
        public IEnumerable<PollQuestionDto> Questions { get; set; }
    }
}