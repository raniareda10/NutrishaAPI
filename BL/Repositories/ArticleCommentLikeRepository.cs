using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.ArticleCommentLikeDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IArticleCommentLikeRepository
    {
        PagedList<MArticleCommentLike> GetAllArticleCommentLike(ArticleCommentLikeQueryPramter ArticleCommentLikeParameters);

    }

    public class ArticleCommentLikeRepository : Repository<MArticleCommentLike>, IArticleCommentLikeRepository
    {
        public ArticleCommentLikeRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MArticleCommentLike> GetAllArticleCommentLike(ArticleCommentLikeQueryPramter ArticleCommentLikeParameters)
        {

             

            IQueryable<MArticleCommentLike> ArticleCommentLike = GetAll();


            //searhing
            SearchByPramter(ref ArticleCommentLike,ArticleCommentLikeParameters.UserId,  ArticleCommentLikeParameters.ArticleCommentId);

            return PagedList<MArticleCommentLike>.ToPagedList(ArticleCommentLike,
                ArticleCommentLikeParameters.PageNumber,
                ArticleCommentLikeParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MArticleCommentLike> ArticleCommentLike, int? userId, int? articleId)
        {
            //if (ArticleCommentLike.Any() && sourceId > 0)
            //{
            //    ArticleCommentLike = ArticleCommentLike.Where(o => o.SourceId == sourceId);
            //}

            //if (ArticleCommentLike.Any() && adminId > 0)
            //{
            //    ArticleCommentLike = ArticleCommentLike.Where(o => o.AdminId == adminId);
            //}
            //if (ArticleCommentLike.Any() && customerId > 0)
            //{
            //    ArticleCommentLike = ArticleCommentLike.Where(o => o.CustomerId == customerId);
            //}
            if (ArticleCommentLike.Any() && userId > 0)
            {
                ArticleCommentLike = ArticleCommentLike.Where(o => o.UserId == userId);
            }
            if (ArticleCommentLike.Any() && articleId > 0)
            {
                ArticleCommentLike = ArticleCommentLike.Where(o => o.ArticleCommentId == articleId);
            }
            return;
        }


    }
}
