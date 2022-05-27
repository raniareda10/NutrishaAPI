using System;
using System.Collections.Generic;
using DL.DtosV1.Users;
using DL.EntitiesV1.Comments;
using DL.EntitiesV1.Reactions;
using DL.HelperInterfaces;

namespace DL.DtosV1.Comments
{
    public class CommentDto : ITotal
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }

        public OwnerDto Owner { get; set; }
        public ReactionType? ReactionType { get; set; }
        public IDictionary<string, int> Totals { get; set; }

        public static CommentDto FromCommentEntity(Comment c)
        {
            return new CommentDto()
            {
                Id = c.Id,
                Owner = new OwnerDto()
                {
                    Id = c.UserId,
                    Name = c.User?.Name,
                    ImageUrl = c.User?.PersonalImage
                },
                Created = c.Created,
                Content = c.Content,
                Totals = c.Totals
            };
        }
    }
}