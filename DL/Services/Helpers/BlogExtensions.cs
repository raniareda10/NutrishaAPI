using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.EntitiesV1.Blogs;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace DL.Services.Helpers
{
    public static class BlogExtensions
    {
        public static async Task UpdateBlogTotalAsync(
            this AppDBContext appDbContext, 
            long blogId,
            string key, 
            bool increase = true)
        {
            var blog = await appDbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null)
            {
                return;
            }

            if (increase)
                blog.Totals[key]++;
            else
                blog.Totals[key]--;
            
            appDbContext.Blogs.Update(blog);
        }

        public static async Task<Blog> GetBlogByIdAsync(
            this IQueryable<Blog> blogs, 
            long id)
        { 
            return await blogs.FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}