using System.Collections.Generic;
using DL.DtosV1.Polls;

namespace DL.DtosV1.Blogs.Timeline
{
    public class PollTimelineResult
    {
        public long? SelectedAnswerId { get; set; }
        public IEnumerable<PollQuestionDto> Questions { get; set; }
    }
}