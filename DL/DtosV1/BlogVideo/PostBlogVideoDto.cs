using System;
using System.Collections.Generic;
using DL.DtosV1.Storage;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Articles;
using DL.Enums;
using DL.HelperInterfaces;
using Newtonsoft.Json;

namespace DL.DtosV1.BlogVideo
{
    public class PostBlogVideoDto : IMedia
    {
        public string Subject { get; set; }
        public long TagId { get; set; }
        public IList<FormFileDto> Files { get; set; }
        public IList<ExternalMedia> ExternalMedia { get; set; }
        
        public Blog ToBLogVideo(int ownerId)
        {
            return new Blog()
            {
                Subject = Subject,
                Created = DateTime.UtcNow,
                OwnerId = ownerId,
                TagId = TagId,
                Totals = new Dictionary<string, int>()
                {
                    {TotalKeys.Views, 0},
                },
                EntityType = EntityType.BlogVideo
            };
        }
    }
}