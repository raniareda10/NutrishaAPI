using System;
using System.Threading.Tasks;
using DL.DBContext;
using DL.Enums;
using DL.HelperInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DL.Services.Helpers
{
    public static class EntityExtensions
    {
        public static async Task<bool> IsEntityExistsAsync(this AppDBContext dbContext, long EntityId,
            EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Article:
                case EntityType.Poll:
                case EntityType.BlogVideo:
                    return await dbContext.Blogs.AnyAsync(b => b.Id == EntityId && b.EntityType == entityType);

                case EntityType.Comment:
                    return await dbContext.Comments.AnyAsync(c => c.Id == EntityId);
                default:
                    return false;
            }
        }
        
        public static Task<ITotal> GetEntityWithTotalAsync<T>(this AppDBContext dbContext, T entity) where T : IEntity
        {
            return GetEntityWithTotalAsync(dbContext, entity.EntityId, entity.EntityType);
        }
        
        public static async Task<ITotal> GetEntityWithTotalAsync(
            this AppDBContext dbContext,long entityId, EntityType entityType)
        {
            ITotal entityWithTotals;
            switch (entityType)
            {
                case EntityType.Article:
                {
                    entityWithTotals = await dbContext.Blogs.GetBlogByIdAsync(entityId);
                    break;
                }

                case EntityType.Comment:
                {
                    entityWithTotals = await dbContext.Comments
                        .FirstOrDefaultAsync(c => c.Id == entityId);
                    break;
                }
                default:
                    throw new ArgumentException("Not Supported Yet");
            }

            dbContext.Update(entityWithTotals);
            return entityWithTotals;
        }
    }
}