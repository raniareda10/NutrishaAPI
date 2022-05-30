using System;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Polls;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Polls;
using DL.Enums;
using DL.ResultModels;

namespace DL.Services.Blogs.Polls
{
    public class PollService
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public PollService(
            AppDBContext dbContext,
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        
        public async Task<long> PostAsync(PostPollDto postPollDto)
        {
            var blog = new Blog()
            {
                Created = DateTime.UtcNow,
                OwnerId = _currentUserService.UserId,
                Subject = postPollDto.Question,
                Poll = new Poll()
                {
                    Questions = postPollDto.Answers.Select(answer => new PollQuestion()
                    {
                        Created = DateTime.UtcNow,
                        Content = answer
                    }).ToList(),
                    BackgroundColor = postPollDto.BackgroundColor
                },
                EntityType = EntityType.Poll
            };

            await _dbContext.AddAsync(blog);
            await _dbContext.SaveChangesAsync();
            
            return blog.Id;
        }
    }
}