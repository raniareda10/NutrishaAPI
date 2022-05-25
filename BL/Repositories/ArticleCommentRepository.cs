using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.ArticleCommentDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IArticleCommentRepository
    {
        PagedList<MArticleComment> GetAllArticleComment(ArticleCommentQueryPramter ArticleCommentParameters);

    }

    public class ArticleCommentRepository : Repository<MArticleComment>, IArticleCommentRepository
    {
        public ArticleCommentRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MArticleComment> GetAllArticleComment(ArticleCommentQueryPramter ArticleCommentParameters)
        {

            IQueryable<MArticleComment> ArticleComment = GetAll();
            //searhing
            SearchByPramter(ref ArticleComment, ArticleCommentParameters.fromDate, ArticleCommentParameters.toDate, ArticleCommentParameters.Comment, ArticleCommentParameters.ArticleId);

            return PagedList<MArticleComment>.ToPagedList(ArticleComment,
                ArticleCommentParameters.PageNumber,
                ArticleCommentParameters.PageSize);
        }

        private void SearchByPramter(ref IQueryable<MArticleComment> ArticleComment, DateTime? fromdate=null, DateTime? todate=null, string comment = null,int? userId =0,int? articleId =0)
        {
            if (ArticleComment.Any() && fromdate != null)
            {
                ArticleComment = ArticleComment.Where(o => o.Date >= fromdate);
            }
            if (ArticleComment.Any() && todate != null)
            {
                ArticleComment = ArticleComment.Where(o => o.Date <= todate);
            }
            if (!ArticleComment.Any() &&  !string.IsNullOrWhiteSpace(comment))
            {
                ArticleComment = ArticleComment.Where(o => o.Comment.ToLower().Contains(comment.Trim().ToLower()));
            }
            //if (ArticleComment.Any() && userId > 0)
            //{
            //    ArticleComment = ArticleComment.Where(o => o.UserId == userId);
            //}

            if (ArticleComment.Any() && articleId > 0)
            {
                ArticleComment = ArticleComment.Where(o => o.ArticleId == articleId);
            }
            return;
        }


    }
}
