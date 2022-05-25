using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.UserPollAnswerDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IUserPollAnswerRepository
    {
        PagedList<MUserPollAnswer> GetAllUserPollAnswer(UserPollAnswerQueryPramter UserPollAnswerParameters);

    }

    public class UserPollAnswerRepository : Repository<MUserPollAnswer>, IUserPollAnswerRepository
    {
        public UserPollAnswerRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MUserPollAnswer> GetAllUserPollAnswer(UserPollAnswerQueryPramter UserPollAnswerParameters)
        {

             

            IQueryable<MUserPollAnswer> UserPollAnswer = GetAll();


            //searhing
            SearchByPramter(ref UserPollAnswer,UserPollAnswerParameters.UserId,  UserPollAnswerParameters.PollId);

            return PagedList<MUserPollAnswer>.ToPagedList(UserPollAnswer,
                UserPollAnswerParameters.PageNumber,
                UserPollAnswerParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MUserPollAnswer> UserPollAnswer, int? userId, int? pollAnswerId)
        {
            //if (UserPollAnswer.Any() && sourceId > 0)
            //{
            //    UserPollAnswer = UserPollAnswer.Where(o => o.SourceId == sourceId);
            //}

            //if (UserPollAnswer.Any() && adminId > 0)
            //{
            //    UserPollAnswer = UserPollAnswer.Where(o => o.AdminId == adminId);
            //}
            //if (UserPollAnswer.Any() && customerId > 0)
            //{
            //    UserPollAnswer = UserPollAnswer.Where(o => o.CustomerId == customerId);
            //}
            if (UserPollAnswer.Any() && userId > 0)
            {
                UserPollAnswer = UserPollAnswer.Where(o => o.UserId == userId);
            }
            if (UserPollAnswer.Any() && pollAnswerId > 0)
            {
                UserPollAnswer = UserPollAnswer.Where(o => o.PollAnswerId == pollAnswerId);
            }
            return;
        }


    }
}
