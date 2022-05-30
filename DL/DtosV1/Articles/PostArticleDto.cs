using System;
using System.Collections.Generic;
using DL.DtosV1.Storage;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Articles;
using DL.EntitiesV1.Media;
using DL.Enums;
using DL.HelperInterfaces;

namespace DL.DtosV1.Articles
{
    public class PostArticleDto : IMedia
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public long TagId { get; set; }
        
        public IList<FormFileDto> Files { get; set; }
        public IList<ExternalMedia> ExternalMedia { get; set; }

        public Blog ToArticle(int ownerId)
        {
            return new Blog()
            {
                Subject = Subject,
                Created = DateTime.UtcNow,
                OwnerId = ownerId,
                TagId = TagId,
                Totals = new Dictionary<string, int>()
                {
                    {TotalKeys.Likes, 0},
                    {TotalKeys.Comments, 0},
                },
                EntityType = EntityType.Article,
                Article = new Article()
                {
                    Description = Description
                }
            };
        }

    }
}