using System;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Articles;
using DL.DtosV1.Blogs;
using DL.DtosV1.BlogVideo;
using DL.DtosV1.Users;
using DL.EntitiesV1.Blogs;
using DL.Enums;
using DL.Extensions;
using DL.ResultModels;
using DL.StorageServices;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.BlogVideo
{
    public class BlogVideoRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStorageService _storageService;

        public BlogVideoRepository(
            AppDBContext dbContext,
            ICurrentUserService currentUserService,
            IStorageService storageService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _storageService = storageService;
        }

        public async Task<long> PostAsync(PostBlogVideoDto postBlogVideoDto)
        {
            var media = await _storageService.PrepareMediaAsync(
                postBlogVideoDto, EntityType.BlogVideo);
            var bLogVideo = postBlogVideoDto.ToBLogVideo(_currentUserService.UserId);
            bLogVideo.Media = media;

            await _dbContext.AddAsync(bLogVideo);
            await _dbContext.SaveChangesAsync();
            return bLogVideo.Id;
        }
        public async Task PutAsync(EditBlogVideo editBlogVideo)
        {
            var currentArticle = await _dbContext.Blogs
                .Where(b => b.EntityType == EntityType.BlogVideo)
                .Where(b => b.Id == editBlogVideo.Id)
                .FirstOrDefaultAsync();

            if (currentArticle == null) return;
            
            var newMedia = await _storageService.PrepareMediaAsync(
                editBlogVideo, EntityType.Article,
                currentArticle.Media,
                editBlogVideo.DeletedMediaIds);

            currentArticle.Subject = editBlogVideo.Subject;
            currentArticle.Media = newMedia;
            currentArticle.Edited = DateTime.UtcNow;
            currentArticle.TagId = editBlogVideo.TagId;
            _dbContext.Blogs.Update(currentArticle);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<AdminArticleDetails> GetByIdForAdmin(long id)
        {
            var article = await _dbContext.Blogs
                .Select(b => new AdminArticleDetails()
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    Totals = b.Totals,
                    Media = b.Media,
                    Created = b.Created,
                    Tag = BlogTagDto.FromBlogTag(b.Tag),
                    Owner = new OwnerDto()
                    {
                        Id = b.Owner.Id,
                        Name = b.Owner.Name,
                        ImageUrl = b.Owner.PersonalImage
                    }
                })
                .FirstOrDefaultAsync(b => b.Id == id);

            return article;
        }

        public async Task<PagedResult<ArticleListDto>> GetPagedListAsync(GetPagedListQueryModel queryModel)
        {
            var query = _dbContext.Blogs
                .Where(b => b.EntityType == EntityType.BlogVideo)
                .OrderByDescending(b => b.Created)
                .Select(b => new ArticleListDto
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    Owner = new OwnerDto()
                    {
                        Id = b.Owner.Id,
                        Name = b.Owner.Name,
                        ImageUrl = b.Owner.PersonalImage
                    },
                    Created = b.Created,
                    Media = b.Media,
                    Totals = b.Totals,
                    Tag = new BlogTagDto()
                    {
                        Id = b.Tag.Id,
                        Name = b.Tag.Name,
                        Color = b.Tag.Color
                    }
                });

            if (!string.IsNullOrWhiteSpace(queryModel.SearchWord))
            {
                query = query.Where(s => s.Subject.Contains(queryModel.SearchWord));
            }

            var result = await query.ToPagedListAsync(queryModel);

            return result;
        }

        public async Task<BaseServiceResult> DeleteAsync(long id)
        {
            var blog = await _dbContext.Blogs
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return new BaseServiceResult()
                {
                    Errors = new[] { NonLocalizedErrorMessages.InvalidId }
                };
            }

            _dbContext.Remove(blog);
            await _dbContext.SaveChangesAsync();

            return new BaseServiceResult();
        }
    }
}