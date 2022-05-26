using System;
using DL.EntitiesV1.Blogs;
using DL.Enums;

namespace DL.DtosV1.Blogs
{
    public class BlogTimelineResult<T> where T : new()
    {
        public long Id { get; set; }
        public long TagId { get; set; }
        public EntityType BlogType { get; set; }
        public DateTime Created { get; set; }
        public string Subject { get; set; }
        public T AdditionalData { get; set; }

        public BlogTimelineResult(Blog blog)
        {
            Id = blog.Id;
            TagId = blog.TagId;
            BlogType = blog.EntityType;
            Created = blog.Created;
            Subject = blog.Subject;
            AdditionalData = new T();
        }
    }
}