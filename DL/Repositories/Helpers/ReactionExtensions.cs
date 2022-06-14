using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Reactions;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Helpers
{
    public static class ReactionExtensions
    {
        public static async Task<Dictionary<long, ReactionType>> GetReactionsOnEntitiesAsync(
            this IQueryable<Reaction> query,
            IList<long> entityIds,
            long userId)
        {
            return await query
                .Where(r =>
                    r.UserId == userId &&
                    entityIds.Contains(r.EntityId)
                )
                .ToDictionaryAsync(r => r.EntityId, r => r.ReactionType);
        }

        public static string MapToTotalKey(this ReactionType type)
        {
            switch (type)
            {
                case ReactionType.Like:
                    return TotalKeys.Likes;
                // case ReactionType.DisLike:
                //     return TotalKeys.DisLikes;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ReactionType), type, null);
            }
        }
    }
}