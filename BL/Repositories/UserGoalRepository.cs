using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.UserGoalDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IUserGoalRepository
    {
        PagedList<MUserGoal> GetAllUserGoal(UserGoalQueryPramter UserGoalParameters);

    }

    public class UserGoalRepository : Repository<MUserGoal>, IUserGoalRepository
    {
        public UserGoalRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MUserGoal> GetAllUserGoal(UserGoalQueryPramter UserGoalParameters)
        {

             

            IQueryable<MUserGoal> UserGoal = GetAll().Include(dt => dt.Goal);


            //searhing
            SearchByPramter(ref UserGoal,UserGoalParameters.UserId,  UserGoalParameters.GoalId);

            return PagedList<MUserGoal>.ToPagedList(UserGoal,
                UserGoalParameters.PageNumber,
                UserGoalParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MUserGoal> UserGoal, int? userId, int? goalId)
        {
            //if (UserGoal.Any() && sourceId > 0)
            //{
            //    UserGoal = UserGoal.Where(o => o.SourceId == sourceId);
            //}

            //if (UserGoal.Any() && adminId > 0)
            //{
            //    UserGoal = UserGoal.Where(o => o.AdminId == adminId);
            //}
            //if (UserGoal.Any() && customerId > 0)
            //{
            //    UserGoal = UserGoal.Where(o => o.CustomerId == customerId);
            //}
            if (UserGoal.Any() && userId > 0)
            {
                UserGoal = UserGoal.Where(o => o.UserId == userId);
            }
            if (UserGoal.Any() && goalId > 0)
            {
                UserGoal = UserGoal.Where(o => o.GoalId == goalId);
            }
            return;
        }


    }
}
