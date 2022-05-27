﻿using System;
using DL.DtosV1.Users;
using DL.EntitiesV1.Comments;
using DL.EntitiesV1.Reactions;

namespace DL.DtosV1.Comments
{
    public class CommentDto
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }

        public OwnerDto Owner { get; set; }
        public ReactionType? ReactionType { get; set; }
        
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
                Content = c.Content
            };
        }

       
    }
}