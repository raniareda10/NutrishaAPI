using System;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Polls;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Polls;
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
        
        public async Task<PayloadServiceResult<long>> PostAsync(PostPollDto postPollDto)
        {
            var blog = new Blog()
            {
                Created = DateTime.UtcNow,
                OwnerId = _currentUserService.UserId,
                TagId = postPollDto.TagId,
                Poll = new Poll()
                {
                    Questions = postPollDto.Answers.Select(answer => new PollQuestion()
                    {
                        Created = DateTime.UtcNow,
                        Content = answer
                    }).ToList()
                }
            };

            await _dbContext.AddAsync(blog);
            await _dbContext.SaveChangesAsync();
            return new PayloadServiceResult<long>()
            {
                Data = blog.Id
            };
        }
    }
}