using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.PollDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IPollRepository
    {
        PagedList<MPoll> GetAllPoll(PollQueryPramter PollParameters);

    }

    public class PollRepository : Repository<MPoll>, IPollRepository
    {
        public PollRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MPoll> GetAllPoll(PollQueryPramter PollParameters)
        {
            IQueryable<MPoll> Poll = GetAll();
            //searhing
            SearchByPramter(ref Poll, PollParameters.fromDate, PollParameters.toDate, PollParameters.Question, PollParameters.BlogTypeId);

            return PagedList<MPoll>.ToPagedList(Poll,
                PollParameters.PageNumber,
                PollParameters.PageSize);


        }

        private void SearchByPramter(ref IQueryable<MPoll> Poll, DateTime? fromdate, DateTime? todate, string question, int blogTypeId)
        {
            if (Poll.Any() && fromdate != null)
            {
                Poll = Poll.Where(o => o.CreatedTime >= fromdate);
            }
            if (Poll.Any() && todate != null)
            {
                Poll = Poll.Where(o => o.CreatedTime <= todate);
            }
            if (!Poll.Any() && !string.IsNullOrWhiteSpace(question))
            {
                Poll = Poll.Where(o => o.Question.ToLower().Contains(question.Trim().ToLower()));
            }
            if (Poll.Any() && blogTypeId > 0)
            {
                Poll = Poll.Where(o => o.BlogTypeId == blogTypeId);
            }
            return;
        }

    }
}
