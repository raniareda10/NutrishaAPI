using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.BlogVideo;
using DL.EntitiesV1.Blogs;
using DL.Enums;
using DL.ResultModels;
using DL.StorageServices;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Blogs.BlogVideo
{
    public class BlogVideoService
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStorageService _storageService;

        public BlogVideoService(
            AppDBContext dbContext,
            ICurrentUserService currentUserService,
            IStorageService storageService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _storageService = storageService;
        }

        public async Task<PayloadServiceResult<BlogVideoDetailsDto>> PostAsync(PostBlogVideoDto postBlogVideoDto)
        {
            var result = new PayloadServiceResult<BlogVideoDetailsDto>();
            var media = await _storageService.PrepareMediaAsync(postBlogVideoDto, EntityType.BlogVideo);

            var blog = new Blog()
            {
                Subject = postBlogVideoDto.Subject,
                Created = DateTime.UtcNow,
                OwnerId = _currentUserService.UserId,
                Totals = new Dictionary<string, int>()
                {
                    {TotalKeys.Views, 0}
                },
                TagId = postBlogVideoDto.TagId,
                Media = media
            };

            await _dbContext.AddAsync(blog);
            await _dbContext.SaveChangesAsync();

            result.Data = new BlogVideoDetailsDto
            {
                Id = blog.Id,
                TagId = blog.TagId.Value,
                Subject = blog.Subject,
                Media = blog.Media,
            };
            return result;
        }
        
        
    }
}