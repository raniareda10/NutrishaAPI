using System;
using System.Collections.Generic;
using DL.Entities;
using DL.EntitiesV1.Blogs.Polls;
using DL.Enums;

namespace DL.EntitiesV1.Blogs
{
    public class Blog : BaseEntityV1
    {
        public Article Article { get; set; }
        public Poll Poll { get; set; }

        public string Subject { get; set; }
        public EntityType EntityType { get; set; }
        public IDictionary<string, int> Totals { get; set; }

        public int OwnerId { get; set; }
        public MUser Owner { get; set; }
        public IList<MediaFile> Media { get; set; }
        
        public long TagId { get; set; }
        public BlogTag Tag { get; set; }
    }

    public class MediaFile
    {
        public MediaType MediaType { get; set; }
        public string Url { get; set; }
        public string[] Flags { get; set; }
    }

    public enum MediaType
    {
        Image = 0,
        Video = 1
    }
}