using DL.DtosV1.Common;
using DL.EntitiesV1.Reactions;

namespace DL.DtosV1.Blogs.Timeline
{
    public class ArticleTimelineResult
    {
        public LocalizedObject<string> Description { get; set; }
        // public bool HasCommented { get; set; }
        public ReactionType? ReactionType { get; set; }
    }
}