using System;
using System.Collections.Generic;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Comments;
using DL.Enums;
using DL.HelperInterfaces;

namespace DL.DtosV1.Comments
{
    public class PostCommentDto : IContent, IEntity
    {
        public string Content { get; set; }
        public long EntityId { get; set; }
        public EntityType EntityType { get; set; }

        public Comment ToCommentEntity(int userId)
        {
            return new Comment()
            {
                Created = DateTime.Now,
                Content = Content,
                EntityId = EntityId,
                EntityType = EntityType,
                UserId = userId,
                Totals = new Dictionary<string, int>()
                {
                    {TotalKeys.Likes, 0},
                    // {TotalKeys.DisLikes, 0}
                }
            };
        }
    }
}