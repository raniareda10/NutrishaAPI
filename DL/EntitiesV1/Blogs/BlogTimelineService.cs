using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Blogs;
using DL.EntitiesV1.Reactions;
using DL.Enums;
using DL.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DL.EntitiesV1.Blogs
{
    public class BlogTimelineService
    {
        private readonly AppDBContext _dbContext;

        public BlogTimelineService(AppDBContext DbContext)
        {
            _dbContext = DbContext;
        }

        
        public async Task<PagedResult<object>> GetTimelineAsync(BlogTimelinePagedModel model)
        {
            // TODO Enhance Query By Remove Joins And Convert Additional Data To JSON Objects
            var blogsQuery = _dbContext.Blogs
                .Include(b => b.Article)
                .Include(b => b.Poll)
                .ThenInclude(p => p.Questions)
                .AsQueryable();

            if (model.TagId.HasValue)
            {
                blogsQuery = blogsQuery.Where(b => b.TagId == model.TagId.Value);
            }

            var blogs = await blogsQuery.OrderByDescending(b => b.Created)
                .ToPagedListAsync(model);

            var result = new List<object>(blogs.Data.Count);
            result.AddRange(await BuildArticlesAsync(blogs.Data.Where(b => b.EntityType == EntityType.Article)));
            result.AddRange(await BuildPollsAsync(blogs.Data.Where(b => b.EntityType == EntityType.Poll)));

            return new PagedResult<object>()
            {
                Data = result,
                TotalRows = blogs.TotalRows
            };
        }

        private async Task<IEnumerable<BlogTimelineResult<PollTimelineResult>>> BuildPollsAsync(IEnumerable<Blog> polls)
        {
            long userId = 10;
            var pollsList = polls.ToList();
            var pollsIds = pollsList.Select(a => a.Id).ToList();

            var userAnswers = await _dbContext.PollAnswers.Where(answer =>
                    pollsIds.Contains(answer.PollQuestion.PollId) &&
                    answer.UserId == userId
                )
                .Select(a => new
                {
                    PollId = a.PollQuestion.PollId,
                    PollQuestionId = a.PollQuestionId
                })
                .ToDictionaryAsync(a => a.PollId, a => a.PollQuestionId);

            var list = new List<BlogTimelineResult<PollTimelineResult>>(pollsList.Count);
            foreach (var blog in pollsList)
            {
                var item = new BlogTimelineResult<PollTimelineResult>(blog);
                if (userAnswers.TryGetValue(blog.Id, out var selectedAnswerId))
                {
                    item.AdditionalData.SelectedAnswerId = selectedAnswerId;
                }

                item.AdditionalData.Questions = blog.Poll.Questions;

                list.Add(item);
            }

            return list;
        }

        private async Task<IEnumerable<BlogTimelineResult<ArticleTimelineResult>>> BuildArticlesAsync(
            IEnumerable<Blog> articles)
        {
            long userId = 10;
            var articlesList = articles.ToList();
            var articlesIds = articlesList.Select(a => a.Id).ToList();
            var reactions = await GetReactionsOnEntitiesAsync(articlesIds, userId);
            var list = new List<BlogTimelineResult<ArticleTimelineResult>>(articlesList.Count);
            foreach (var blog in articlesList)
            {
                var item = new BlogTimelineResult<ArticleTimelineResult>(blog);
                if (reactions.TryGetValue(blog.Id, out var reactionType))
                {
                    item.AdditionalData.ReactionType = reactionType;
                }

                item.AdditionalData.Description = blog.Article.Description;
                item.AdditionalData.HasCommented = false;
                item.AdditionalData.HasCommented = false;
                item.AdditionalData.Totals = blog.Totals;

                list.Add(item);
            }

            return list;
        }

        private async Task<Dictionary<long, ReactionType>> GetReactionsOnEntitiesAsync(IList<long> entityIds,
            long userId)
        {
            return await _dbContext.Reactions
                .Where(r =>
                    r.UserId == userId &&
                    entityIds.Contains(r.EntityId)
                )
                .ToDictionaryAsync(r => r.EntityId, r => r.ReactionType);
        }
    }
}