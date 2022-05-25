using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.ArticleDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IArticleRepository
    {
        PagedList<MArticle> GetAllArticle(ArticleQueryPramter ArticleParameters);

    }

    public class ArticleRepository : Repository<MArticle>, IArticleRepository
    {
        public ArticleRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MArticle> GetAllArticle(ArticleQueryPramter ArticleParameters)
        {

            IQueryable<MArticle> Article = GetAll();
            //searhing
            SearchByPramter(ref Article, ArticleParameters.fromDate, ArticleParameters.toDate, ArticleParameters.Subject, ArticleParameters.BlogTypeId);

            return PagedList<MArticle>.ToPagedList(Article,
                ArticleParameters.PageNumber,
                ArticleParameters.PageSize);
        }

        private void SearchByPramter(ref IQueryable<MArticle> Article, DateTime? fromdate, DateTime? todate, string title,int blogTypeId)
        {
            if (Article.Any() && fromdate != null)
            {
                Article = Article.Where(o => o.CreatedTime >= fromdate);
            }
            if (Article.Any() && todate != null)
            {
                Article = Article.Where(o => o.CreatedTime <= todate);
            }
            if (!Article.Any() &&  !string.IsNullOrWhiteSpace(title))
            {
                Article = Article.Where(o => o.Subject.ToLower().Contains(title.Trim().ToLower()));
            }
            if (Article.Any() && blogTypeId >0)
            {
                Article = Article.Where(o => o.BlogTypeId == blogTypeId);
            }
            return;
        }


    }
}
