using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Blogs.Details;
using DL.DtosV1.Users;
using DL.Enums;
using Microsoft.EntityFrameworkCore;

namespace DL.Services.Blogs.Articles
{
    public class ArticleService : IBlogDetailsService
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public ArticleService(
            AppDBContext dbContext,
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        
        public async Task<object> GetByIdAsync(long id)
        {
            var blog = await _dbContext.Blogs
                .Include(b => b.Article)
                .Select(b => new ArticleDetails
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    Description = b.Article.Description,
                    Totals = b.Totals,
                    Media = b.Media,
                    Created = b.Created,
                    Owner = new OwnerDto()
                    {
                        Id = b.Owner.Id,
                        Name = b.Owner.Name,
                        ImageUrl = b.Owner.PersonalImage
                    }
                })
                .FirstOrDefaultAsync(b => b.Id == id);

            blog.ReactionType = await _dbContext.Reactions
                .Where(r => 
                    r.EntityId == id &&
                    r.EntityType == EntityType.Article &&
                    r.UserId == _currentUserService.UserId)
                .Select(r => r.ReactionType)
                .FirstOrDefaultAsync();

            return blog;
        }
    }

    public interface IBlogDetailsService
    {
        public Task<object> GetByIdAsync(long id);
    }
}