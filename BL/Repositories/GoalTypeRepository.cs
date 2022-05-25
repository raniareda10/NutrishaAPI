using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.GoalTypeDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IGoalTypeRepository
    {
        PagedList<MGoalType> GetAllGoalType(GoalTypeQueryPramter GoalTypeParameters);

    }

    public class GoalTypeRepository : Repository<MGoalType>, IGoalTypeRepository
    {
        public GoalTypeRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MGoalType> GetAllGoalType(GoalTypeQueryPramter GoalTypeParameters)
        {

             

            IQueryable<MGoalType> GoalType = GetAll();


            //searhing
            SearchByPramter(ref GoalType, GoalTypeParameters.Name);

            return PagedList<MGoalType>.ToPagedList(GoalType,
                GoalTypeParameters.PageNumber,
                GoalTypeParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MGoalType> GoalType, string Searchpramter)
        {
            if (!GoalType.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            GoalType = GoalType.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
