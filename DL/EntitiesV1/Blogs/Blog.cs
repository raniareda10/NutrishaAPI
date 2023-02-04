using System;
using System.Collections.Generic;
using DL.Entities;
using DL.EntitiesV1.AdminUser;
using DL.EntitiesV1.Blogs.Articles;
using DL.EntitiesV1.Blogs.Polls;
using DL.EntitiesV1.Media;
using DL.Enums;
using DL.HelperInterfaces;

namespace DL.EntitiesV1.Blogs
{
    public class Blog : BaseEntityV1, ITotal
    {
        public Article Article { get; set; }
        public Poll Poll { get; set; }

        
        public DateTime Edited { get; set; }
        
        public string Subject { get; set; }
        public EntityType EntityType { get; set; }

        public Dictionary<string, int> Totals { get; set; }
        public int OwnerId { get; set; }
        public AdminUserEntity Owner { get; set; }
        public IList<MediaFile> Media { get; set; }

        public long? TagId { get; set; }
        public BlogTag Tag { get; set; }
    }
}