using System.Collections.Generic;
using DL.EntitiesV1.Reactions;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace DL.DtosV1.Blogs
{
    public class ArticleTimelineResult
    {
        public string Description { get; set; }
        // public bool HasCommented { get; set; }
        public ReactionType? ReactionType { get; set; }
    }
}