﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Articles;
using DL.DtosV1.Blogs;
using DL.DtosV1.Blogs.Details;
using DL.DtosV1.Common;
using DL.DtosV1.Users;
using DL.Entities;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Measurements;
using DL.EntitiesV1.Media;
using DL.EntitiesV1.Reactions;
using DL.Enums;
using DL.Extensions;
using DL.ResultModels;
using DL.StorageServices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DL.Repositories.Blogs.Articles
{
    public class ArticleService : IBlogDetailsService
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStorageService _storageService;

        public ArticleService(
            AppDBContext dbContext,
            ICurrentUserService currentUserService,
            IStorageService storageService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _storageService = storageService;
        }

        public async Task<object> GetByIdAsync(long id)
        {
            var article = await _dbContext.Blogs
                .Include(b => b.Article)
                .Select(b => new MobileArticleDetails
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    DescriptionMapper = b.Article.Description,
                    Totals = b.Totals,
                    Media = b.Media,
                    Created = b.Created,
                    Owner = new OwnerDto()
                    {
                        Id = b.Owner.Id,
                        Name = b.Owner.Name,
                        ImageUrl = b.Owner.PersonalImage
                    }
                })
                .FirstOrDefaultAsync(b => b.Id == id);

            article.ReactionType = await _dbContext.Reactions
                .Where(r =>
                    r.EntityId == id &&
                    r.EntityType == EntityType.Article &&
                    r.UserId == _currentUserService.UserId)
                .Select(r => r.ReactionType as ReactionType?)
                .FirstOrDefaultAsync();

            article.Description = JsonConvert.DeserializeObject<LocalizedObject<string>>(article.DescriptionMapper);
            return article;
        }

        public async Task<long> PostAsync(PostArticleDto postArticleDto)
        {
            // var coverMedia = await _storageService.PrepareMediaAsync(postArticleDto.CoverImage, EntityType.Article);
            // var additionalMedia =
            //     await _storageService.PrepareMediaAsync(postArticleDto.AdditionalMedia, EntityType.Article);
            //

            var media = await _storageService.PrepareMediaAsync(
                postArticleDto, EntityType.Article);
            var article = postArticleDto.ToArticle(_currentUserService.UserId);
            article.Media = media;

            await _dbContext.AddAsync(article);
            await _dbContext.SaveChangesAsync();
            return article.Id;
        }

        public async Task PutAsync(EditArticleDto editArticleDto)
        {
            var currentArticle = await _dbContext.Blogs
                .Where(b => b.EntityType == EntityType.Article)
                .Where(b => b.Id == editArticleDto.Id)
                .Include(b => b.Article)
                .FirstOrDefaultAsync();

            if (currentArticle == null) return;

            var newMedia = await _storageService.PrepareMediaAsync(
                editArticleDto, EntityType.Article,
                currentArticle.Media,
                editArticleDto.DeletedMediaIds);

            currentArticle.Subject = editArticleDto.Subject;
            currentArticle.Article.Description = JsonConvert.SerializeObject(editArticleDto.Description);
            currentArticle.Media = newMedia;
            currentArticle.Edited = DateTime.UtcNow;
            currentArticle.TagId = editArticleDto.TagId;
            _dbContext.Blogs.Update(currentArticle);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<AdminArticleDetails> GetByIdForAdmin(long id)
        {
            var article = await _dbContext.Blogs
                .Select(b => new AdminArticleDetails()
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    DescriptionMapper = b.Article.Description,
                    Totals = b.Totals,
                    Media = b.Media,
                    Created = b.Created,
                    Tag = BlogTagDto.FromBlogTag(b.Tag),
                    Owner = new OwnerDto()
                    {
                        Id = b.Owner.Id,
                        Name = b.Owner.Name,
                        ImageUrl = b.Owner.PersonalImage
                    }
                })
                .FirstOrDefaultAsync(b => b.Id == id);

            article.Description = JsonConvert.DeserializeObject<LocalizedObject<string>>(article.DescriptionMapper);
            return article;
        }

        public async Task<PagedResult<ArticleListDto>> GetPagedListAsync(GetPagedListQueryModel queryModel)
        {
            var query = _dbContext.Blogs
                .Where(b => b.EntityType == EntityType.Article)
                .OrderByDescending(b => b.Created)
                .Select(b => new ArticleListDto
                {
                    Id = b.Id,
                    Subject = b.Subject,
                    DescriptionMapper = b.Article.Description,
                    Owner = new OwnerDto()
                    {
                        Id = b.Owner.Id,
                        Name = b.Owner.Name,
                        ImageUrl = b.Owner.PersonalImage
                    },
                    Created = b.Created,
                    Media = b.Media,
                    Totals = b.Totals,
                    Tag = new BlogTagDto()
                    {
                        Id = b.Tag.Id,
                        Name = b.Tag.Name,
                        Color = b.Tag.Color
                    }
                });

            if (!string.IsNullOrWhiteSpace(queryModel.SearchWord))
            {
                query = query.Where(s => s.Subject.Contains(queryModel.SearchWord) ||
                                         s.DescriptionMapper.Contains(queryModel.SearchWord));
            }

            var result = await query.ToPagedListAsync(queryModel);
            foreach (var articleListDto in result.Data)
            {
                articleListDto.Description =
                    JsonConvert.DeserializeObject<LocalizedObject<string>>(articleListDto.DescriptionMapper);
            }

            return result;
        }

        public async Task<BaseServiceResult> DeleteAsync(long id)
        {
            var blog = await _dbContext.Blogs
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return new BaseServiceResult()
                {
                    Errors = new[] { NonLocalizedErrorMessages.InvalidId }
                };
            }

            _dbContext.Remove(blog);
            await _dbContext.SaveChangesAsync();

            return new BaseServiceResult();
        }
    }


    public interface IBlogDetailsService
    {
        public Task<object> GetByIdAsync(long id);
    }
}