using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.MealStepsDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IMealStepsRepository
    {
        PagedList<MMealSteps> GetAllMealSteps(MealStepsQueryPramter MealStepsParameters);

    }

    public class MealStepsRepository : Repository<MMealSteps>, IMealStepsRepository
    {
        public MealStepsRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MMealSteps> GetAllMealSteps(MealStepsQueryPramter MealStepsParameters)
        {

             

            IQueryable<MMealSteps> MealSteps = GetAll().Include(dt => dt.Steps);


            //searhing
            SearchByPramter(ref MealSteps,MealStepsParameters.MealId,  MealStepsParameters.StepsId);

            return PagedList<MMealSteps>.ToPagedList(MealSteps,
                MealStepsParameters.PageNumber,
                MealStepsParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MMealSteps> MealSteps, int? userId, int? allergyId)
        {
            //if (MealSteps.Any() && sourceId > 0)
            //{
            //    MealSteps = MealSteps.Where(o => o.SourceId == sourceId);
            //}

            //if (MealSteps.Any() && adminId > 0)
            //{
            //    MealSteps = MealSteps.Where(o => o.AdminId == adminId);
            //}
            //if (MealSteps.Any() && customerId > 0)
            //{
            //    MealSteps = MealSteps.Where(o => o.CustomerId == customerId);
            //}
            if (MealSteps.Any() && userId > 0)
            {
                MealSteps = MealSteps.Where(o => o.MealId == userId);
            }
            if (MealSteps.Any() && allergyId > 0)
            {
                MealSteps = MealSteps.Where(o => o.StepsId == allergyId);
            }
            return;
        }


    }
}
