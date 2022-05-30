using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Blogs;
using DL.DtosV1.BlogTags;
using DL.EntitiesV1.Blogs;
using Microsoft.EntityFrameworkCore;

namespace DL.Services.Blogs
{
    public class BlogTagService
    {
        private readonly AppDBContext _dbContext;

        public BlogTagService(AppDBContext DbContext)
        {
            _dbContext = DbContext;
        }


        public async Task<IList<BlogTagDto>> GetAllTags()
        {
            return await _dbContext.BlogTag.Select(t => BlogTagDto.FromBlogTag(t)).ToListAsync();
        }

        public async Task<long> Post(PostBlogTagDto postBlogTagDto)
        {
            var blogTag = new BlogTag()
            {
                Name = postBlogTagDto.Name,
                Color = postBlogTagDto.Color
            };

            await _dbContext.AddAsync(blogTag);
            await _dbContext.SaveChangesAsync();

            return blogTag.Id;
        }
    }
}