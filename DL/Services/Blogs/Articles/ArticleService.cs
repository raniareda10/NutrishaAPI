using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Articles;
using DL.DtosV1.Blogs.Details;
using DL.DtosV1.Users;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Reactions;
using DL.Enums;
using DL.StorageServices;
using Microsoft.EntityFrameworkCore;

namespace DL.Services.Blogs.Articles
{
    public class ArticleService : IBlogDetailsService
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStorageService _storageService;

        public ArticleService(
            AppDBContext dbContext,
            ICurrentUserService currentUserService,
            IStorageService storageService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _storageService = storageService;
        }
        
        public async Task<object> GetByIdAsync(long id)
        {
            var article = await _dbContext.Blogs
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

            article.ReactionType = await _dbContext.Reactions
                .Where(r => 
                    r.EntityId == id &&
                    r.EntityType == EntityType.Article &&
                    r.UserId == _currentUserService.UserId)
                .Select(r => r.ReactionType as ReactionType?)
                .FirstOrDefaultAsync();

            return article;
        }

        public async Task<long> PostAsync(PostArticleDto postArticleDto)
        {
            var media = await _storageService.UploadAsync(
                postArticleDto, EntityType.Article);
            var article = postArticleDto.ToArticle(_currentUserService.UserId);
            article.Media = media;
            
            await _dbContext.AddAsync(article);
            await _dbContext.SaveChangesAsync();
            return article.Id;
        }
    }

    public interface IBlogDetailsService
    {
        public Task<object> GetByIdAsync(long id);
    }
}