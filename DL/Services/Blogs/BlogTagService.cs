using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Blogs;
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
    }
}