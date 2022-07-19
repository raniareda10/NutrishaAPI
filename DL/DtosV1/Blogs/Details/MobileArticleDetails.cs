using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DL.DtosV1.Common;
using DL.DtosV1.Users;
using DL.EntitiesV1.Media;
using DL.EntitiesV1.Reactions;
using DL.Enums;
using DL.HelperInterfaces;

namespace DL.DtosV1.Blogs.Details
{
    public class MobileArticleDetails : ITotal
    {
        public long Id { get; set; }
        public Dictionary<string, int> Totals { get; set; }
        public DateTime Created { get; set; }

        [JsonIgnore]
        public string DescriptionMapper { get; set; }
        public LocalizedObject<string> Description { get; set; }
        public OwnerDto Owner { get; set; }
        public string Subject { get; set; }
        public ReactionType? ReactionType { get; set; }
        public IList<MediaFile> Media { get; set; }
        public EntityType EntityType { get; set; } = EntityType.Article;
    }
}