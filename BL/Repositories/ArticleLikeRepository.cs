using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.ArticleLikeDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IArticleLikeRepository
    {
        PagedList<MArticleLike> GetAllArticleLike(ArticleLikeQueryPramter ArticleLikeParameters);

    }

    public class ArticleLikeRepository : Repository<MArticleLike>, IArticleLikeRepository
    {
        public ArticleLikeRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MArticleLike> GetAllArticleLike(ArticleLikeQueryPramter ArticleLikeParameters)
        {

             

            IQueryable<MArticleLike> ArticleLike = GetAll().Include(dt => dt.Article);


            //searhing
            SearchByPramter(ref ArticleLike,ArticleLikeParameters.UserId,  ArticleLikeParameters.ArticleId);

            return PagedList<MArticleLike>.ToPagedList(ArticleLike,
                ArticleLikeParameters.PageNumber,
                ArticleLikeParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MArticleLike> ArticleLike, int? userId, int? articleId)
        {
            //if (ArticleLike.Any() && sourceId > 0)
            //{
            //    ArticleLike = ArticleLike.Where(o => o.SourceId == sourceId);
            //}

            //if (ArticleLike.Any() && adminId > 0)
            //{
            //    ArticleLike = ArticleLike.Where(o => o.AdminId == adminId);
            //}
            //if (ArticleLike.Any() && customerId > 0)
            //{
            //    ArticleLike = ArticleLike.Where(o => o.CustomerId == customerId);
            //}
            if (ArticleLike.Any() && userId > 0)
            {
                ArticleLike = ArticleLike.Where(o => o.UserId == userId);
            }
            if (ArticleLike.Any() && articleId > 0)
            {
                ArticleLike = ArticleLike.Where(o => o.ArticleId == articleId);
            }
            return;
        }


    }
}
