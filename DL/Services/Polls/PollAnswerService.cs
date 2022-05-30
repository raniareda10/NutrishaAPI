using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Polls;
using DL.DtosV1.Reactions;
using DL.EntitiesV1.Blogs.Polls;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;

namespace DL.Services.Polls
{
    public class PollAnswerService
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public PollAnswerService(
            AppDBContext DbContext,
            ICurrentUserService currentUserService)
        {
            _dbContext = DbContext;
            _currentUserService = currentUserService;
        }

        public async Task<PayloadServiceResult<IEnumerable<PollQuestionDto>>> PostAnswerAsync(PostAnswerDto postAnswerDto)
        {
            var result = new PayloadServiceResult<IEnumerable<PollQuestionDto>>();
            
            if (await IsAlreadyAnswered(postAnswerDto.PollId))
            {
                result.Errors.Add("You Can Answer Only One Time.");
                return result;
            }
            
            var questions =
                await _dbContext.PollQuestions.Where(q => q.PollId == postAnswerDto.PollId)
                    .ToListAsync();

            var selectedQuestion = questions.FirstOrDefault(q => q.Id == postAnswerDto.SelectedQuestionId);
            if (selectedQuestion == null)
            {
                result.Errors.Add("Please Select Correct Answer");
                return result;
            }

            selectedQuestion.SelectedCount++;

            var answer = new PollAnswer()
            {
                Created = DateTime.UtcNow,
                UserId = _currentUserService.UserId,
                PollId = postAnswerDto.PollId,
                PollQuestionId = postAnswerDto.SelectedQuestionId
            };

            _dbContext.PollQuestions.UpdateRange(questions);
            _dbContext.PollAnswers.Add(answer);
            await _dbContext.SaveChangesAsync();
            result.Data = questions.Select(PollQuestionDto.FromPollQuestion);
            return result;
        }


        private async Task<bool> IsAlreadyAnswered(long pollId)
        {
            return await _dbContext.PollAnswers.AnyAsync(
                a => a.PollId == pollId && 
                     a.UserId == _currentUserService.UserId);
        }
    }
}