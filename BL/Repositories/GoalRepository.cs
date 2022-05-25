using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.GoalDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IGoalRepository
    {
        PagedList<MGoal> GetAllGoal(GoalQueryPramter GoalParameters);

    }

    public class GoalRepository : Repository<MGoal>, IGoalRepository
    {
        public GoalRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MGoal> GetAllGoal(GoalQueryPramter GoalParameters)
        {

             

            IQueryable<MGoal> Goal = GetAll().Include(dt => dt.GoalType);


            //searhing
            SearchByPramter(ref Goal,GoalParameters.FrequencyId,  GoalParameters.GoalTypeId);

            return PagedList<MGoal>.ToPagedList(Goal,
                GoalParameters.PageNumber,
                GoalParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MGoal> Goal, int? frequencyId, int? goalTypeId)
        {
            //if (Goal.Any() && sourceId > 0)
            //{
            //    Goal = Goal.Where(o => o.SourceId == sourceId);
            //}

            //if (Goal.Any() && adminId > 0)
            //{
            //    Goal = Goal.Where(o => o.AdminId == adminId);
            //}
            //if (Goal.Any() && customerId > 0)
            //{
            //    Goal = Goal.Where(o => o.CustomerId == customerId);
            //}
            if (Goal.Any() && frequencyId > 0)
            {
                Goal = Goal.Where(o => o.FrequencyId == frequencyId);
            }
            if (Goal.Any() && goalTypeId > 0)
            {
                Goal = Goal.Where(o => o.GoalTypeId == goalTypeId);
            }
            return;
        }


    }
}
