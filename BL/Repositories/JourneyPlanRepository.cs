using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.JourneyPlanDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IJourneyPlanRepository
    {
        PagedList<MJourneyPlan> GetAllJourneyPlan(JourneyPlanQueryPramter JourneyPlanParameters);

    }

    public class JourneyPlanRepository : Repository<MJourneyPlan>, IJourneyPlanRepository
    {
        public JourneyPlanRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MJourneyPlan> GetAllJourneyPlan(JourneyPlanQueryPramter JourneyPlanParameters)
        {

             

            IQueryable<MJourneyPlan> JourneyPlan = GetAll();


            //searhing
            SearchByPramter(ref JourneyPlan, JourneyPlanParameters.Name);

            return PagedList<MJourneyPlan>.ToPagedList(JourneyPlan,
                JourneyPlanParameters.PageNumber,
                JourneyPlanParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MJourneyPlan> JourneyPlan, string Searchpramter)
        {
            if (!JourneyPlan.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            JourneyPlan = JourneyPlan.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
