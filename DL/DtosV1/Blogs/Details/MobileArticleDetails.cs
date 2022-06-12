using System;
using System.Collections.Generic;
using DL.DtosV1.Common;
using DL.DtosV1.Users;
using DL.EntitiesV1.Media;
using DL.EntitiesV1.Reactions;
using DL.HelperInterfaces;

namespace DL.DtosV1.Blogs.Details
{
    public class MobileArticleDetails : ITotal
    {
        public long Id { get; set; }
        public IDictionary<string, int> Totals { get; set; }
        public DateTime Created { get; set; }
        public LocalizedObject<string> Description { get; set; }
        public OwnerDto Owner { get; set; }
        public string Subject { get; set; }
        public ReactionType? ReactionType { get; set; }
        public IList<MediaFile> Media { get; set; }
    }
}