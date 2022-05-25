using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.PollAnswerDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IPollAnswerRepository
    {
        PagedList<MPollAnswer> GetAllPollAnswer(PollAnswerQueryPramter PollAnswerParameters);

    }

    public class PollAnswerRepository : Repository<MPollAnswer>, IPollAnswerRepository
    {
        public PollAnswerRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MPollAnswer> GetAllPollAnswer(PollAnswerQueryPramter PollAnswerParameters)
        {

             

            IQueryable<MPollAnswer> PollAnswer = GetAll();


            //searhing
            SearchByPramter(ref PollAnswer, PollAnswerParameters.Name);

            return PagedList<MPollAnswer>.ToPagedList(PollAnswer,
                PollAnswerParameters.PageNumber,
                PollAnswerParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MPollAnswer> PollAnswer, string Searchpramter)
        {
            if (!PollAnswer.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            PollAnswer = PollAnswer.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
