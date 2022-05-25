using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.ArticleAttachmentDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IArticleAttachmentRepository
    {
        PagedList<MArticleAttachment> GetAllArticleAttachment(ArticleAttachmentQueryPramter ArticleAttachmentParameters);

    }

    public class ArticleAttachmentRepository : Repository<MArticleAttachment>, IArticleAttachmentRepository
    {
        public ArticleAttachmentRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MArticleAttachment> GetAllArticleAttachment(ArticleAttachmentQueryPramter ArticleAttachmentParameters)
        {

            IQueryable<MArticleAttachment> ArticleAttachment = GetAll();
            //searhing
            SearchByPramter(ref ArticleAttachment, ArticleAttachmentParameters.AttachmentTypeId, ArticleAttachmentParameters.ArticleId);

            return PagedList<MArticleAttachment>.ToPagedList(ArticleAttachment,
                ArticleAttachmentParameters.PageNumber,
                ArticleAttachmentParameters.PageSize);
        }

        private void SearchByPramter(ref IQueryable<MArticleAttachment> ArticleAttachment,int? attachmentTypeId =0,int? articleId =0)
        {
       
            if (ArticleAttachment.Any() && attachmentTypeId > 0)
            {
                ArticleAttachment = ArticleAttachment.Where(o => o.AttachmentTypeId == attachmentTypeId);
            }

            if (ArticleAttachment.Any() && articleId > 0)
            {
                ArticleAttachment = ArticleAttachment.Where(o => o.ArticleId == articleId);
            }
            return;
        }


    }
}
