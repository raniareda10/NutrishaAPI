using System;
using System.Collections.Generic;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Media;
using DL.Enums;

namespace DL.DtosV1.Blogs.Timeline
{
    public class BlogTimelineResult<T> where T : new()
    {
        public long Id { get; set; }
        public long TagId { get; set; }
        public EntityType EntityType { get; set; }
        public DateTime Created { get; set; }
        public string Subject { get; set; }
        
        public IDictionary<string, int> Totals { get; set; }
        public T AdditionalData { get; set; }
        public IList<MediaFile> Media { get; set; }

        public BlogTimelineResult(Blog blog)
        {
            Id = blog.Id;
            TagId = blog.TagId;
            EntityType = blog.EntityType;
            Created = blog.Created;
            Subject = blog.Subject;
            Media = blog.Media;
            Totals = blog.Totals;
            AdditionalData = new T();
        }

    }
}