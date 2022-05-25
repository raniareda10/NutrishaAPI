using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.BlogTypeDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IBlogTypeRepository
    {
        PagedList<MBlogType> GetAllBlogType(BlogTypeQueryPramter BlogTypeParameters);

    }

    public class BlogTypeRepository : Repository<MBlogType>, IBlogTypeRepository
    {
        public BlogTypeRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MBlogType> GetAllBlogType(BlogTypeQueryPramter BlogTypeParameters)
        {

             

            IQueryable<MBlogType> BlogType = GetAll();


            //searhing
            SearchByPramter(ref BlogType, BlogTypeParameters.Name);

            return PagedList<MBlogType>.ToPagedList(BlogType,
                BlogTypeParameters.PageNumber,
                BlogTypeParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MBlogType> BlogType, string Searchpramter)
        {
            if (!BlogType.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            BlogType = BlogType.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
