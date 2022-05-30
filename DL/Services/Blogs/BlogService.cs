using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Blogs;
using DL.DtosV1.Blogs.Timeline;
using DL.DtosV1.Polls;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Polls;
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

            var blogs = await blogsQuery
                .OrderByDescending(b => b.Created)
                .ToPagedListAsync(model);

            var result = blogs.Data
                .Select(d => new BlogMapper<BlogTimelineResult<dynamic>>
                {
                    Blog = d,
                    Data = new BlogTimelineResult<dynamic>(d)
                }).ToList();

            // TODO Enhance Loop 1 Time Only    
            await BuildArticlesAsync(result.Where(b => b.Blog.EntityType == EntityType.Article));
            await BuildPollsAsync(result.Where(b => b.Blog.EntityType == EntityType.Poll));

            return new PagedResult<object>()
            {
                Data = result.Select(d => (dynamic) d.Data).ToList(),
                TotalRows = blogs.TotalRows
            };
        }

        public async Task<object> GetBlogByIdAsync(long id, EntityType entityType)
        {
            return await _blogDetailsFactory.GetBlogDetailsService(entityType).GetByIdAsync(id);
        }

        private async Task BuildPollsAsync(IEnumerable<BlogMapper<BlogTimelineResult<dynamic>>> polls)
        {
            var pollsList = polls.ToList();
            var pollsIds = pollsList.Select(a => a.Blog.Id).ToList();

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

            foreach (var blog in pollsList)
            {
                var additionalData = new PollTimelineResult();
                if (userAnswers.TryGetValue(blog.Blog.Id, out var selectedAnswerId))
                {
                    additionalData.SelectedAnswerId = selectedAnswerId;
                }

                additionalData.BackgroundColor = blog.Blog.Poll.BackgroundColor;
                var questions = blog.Blog.Poll.Questions as IEnumerable<PollQuestion>;
                additionalData.Questions = questions.Select(PollQuestionDto.FromPollQuestion);
                blog.Data.AdditionalData = additionalData;
            }
        }

        private async Task BuildArticlesAsync(
            IEnumerable<BlogMapper<BlogTimelineResult<dynamic>>> articles)
        {
            var articlesList = articles.ToList();
            var articlesIds = articlesList.Select(a => a.Blog.Id).ToList();
            var reactions = await _dbContext
                .Reactions
                .GetReactionsOnEntitiesAsync(articlesIds, _currentUserService.UserId);

            foreach (var blog in articlesList)
            {
                var additionalData = new ArticleTimelineResult();
                if (reactions.TryGetValue(blog.Blog.Id, out var reactionType))
                {
                    additionalData.ReactionType = reactionType;
                }

                additionalData.Description = blog.Blog.Article.Description;
                blog.Data.AdditionalData = additionalData;
            }
        }

        private class BlogMapper<T>
        {
            public Blog Blog { get; set; }
            public T Data { get; set; }
        }
    }
}