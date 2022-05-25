using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.FoodStepsDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IFoodStepsRepository
    {
        PagedList<MFoodSteps> GetAllFoodSteps(FoodStepsQueryPramter FoodStepsParameters);

    }

    public class FoodStepsRepository : Repository<MFoodSteps>, IFoodStepsRepository
    {
        public FoodStepsRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MFoodSteps> GetAllFoodSteps(FoodStepsQueryPramter FoodStepsParameters)
        {

             

            IQueryable<MFoodSteps> FoodSteps = GetAll();


            //searhing
            SearchByPramter(ref FoodSteps, FoodStepsParameters.Name);

            return PagedList<MFoodSteps>.ToPagedList(FoodSteps,
                FoodStepsParameters.PageNumber,
                FoodStepsParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MFoodSteps> FoodSteps, string Searchpramter)
        {
            if (!FoodSteps.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            FoodSteps = FoodSteps.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
