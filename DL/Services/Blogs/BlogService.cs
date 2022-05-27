using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Blogs;
using DL.DtosV1.Blogs.Timeline;
using DL.DtosV1.Polls;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Reactions;
using DL.Enums;
using DL.Extensions;
using DL.Services.Blogs.BlogDetails;
using DL.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DL.Services.Blogs
{
    public class BlogService
    {
        private readonly AppDBContext _dbContext;
        private readonly BlogDetailsFactory _blogDetailsFactory;
        private readonly ICurrentUserService _currentUserService;

        public BlogService(
            AppDBContext DbContext,
            BlogDetailsFactory blogDetailsFactory,
            ICurrentUserService currentUserService)
        {
            _dbContext = DbContext;
            _blogDetailsFactory = blogDetailsFactory;
            _currentUserService = currentUserService;
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
            
            // TODO Enhance Loop 1 Time Only    
            result.AddRange(await BuildArticlesAsync(blogs.Data.Where(b => b.EntityType == EntityType.Article)));
            result.AddRange(await BuildPollsAsync(blogs.Data.Where(b => b.EntityType == EntityType.Poll)));
            result.AddRange(blogs.Data.Where(b => b.EntityType == EntityType.BlogVideo).Select(
                b => new BlogTimelineResult<object>(b)));
            
            return new PagedResult<object>()
            {
                Data = result,
                TotalRows = blogs.TotalRows
            };
        }

        public async Task<object> GetBlogByIdAsync(long id, EntityType entityType)
        {
            return await _blogDetailsFactory.GetBlogDetailsService(entityType).GetByIdAsync(id);
        }
        
        private async Task<IEnumerable<BlogTimelineResult<PollTimelineResult>>> BuildPollsAsync(IEnumerable<Blog> polls)
        {
            var pollsList = polls.ToList();
            var pollsIds = pollsList.Select(a => a.Id).ToList();

            var userAnswers = await _dbContext.PollAnswers.Where(answer =>
                    pollsIds.Contains(answer.PollQuestion.PollId) &&
                    answer.UserId == _currentUserService.UserId
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

                item.AdditionalData.Questions = blog.Poll.Questions.Select(PollQuestionDto.FromPollQuestion);
                list.Add(item);
            }

            return list;
        }

        private async Task<IEnumerable<BlogTimelineResult<ArticleTimelineResult>>> BuildArticlesAsync(
            IEnumerable<Blog> articles)
        {
            var articlesList = articles.ToList();
            var articlesIds = articlesList.Select(a => a.Id).ToList();
            var reactions = await _dbContext
                .Reactions
                .GetReactionsOnEntitiesAsync(articlesIds, _currentUserService.UserId);
            
            var list = new List<BlogTimelineResult<ArticleTimelineResult>>(articlesList.Count);
            foreach (var blog in articlesList)
            {
                var item = new BlogTimelineResult<ArticleTimelineResult>(blog);
                if (reactions.TryGetValue(blog.Id, out var reactionType))
                {
                    item.AdditionalData.ReactionType = reactionType;
                }

                item.AdditionalData.Description = blog.Article.Description;
                list.Add(item);
            }

            return list;
        }
    }
}