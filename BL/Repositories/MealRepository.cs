using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.MealDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IMealRepository
    {
        PagedList<MMeal> GetAllMeal(MealQueryPramter MealParameters);

    }

    public class MealRepository : Repository<MMeal>, IMealRepository
    {
        public MealRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MMeal> GetAllMeal(MealQueryPramter MealParameters)
        {

             

            IQueryable<MMeal> Meal = GetAll();


            //searhing
            SearchByPramter(ref Meal, MealParameters.Name);

            return PagedList<MMeal>.ToPagedList(Meal,
                MealParameters.PageNumber,
                MealParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MMeal> Meal, string Searchpramter)
        {
            if (!Meal.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            Meal = Meal.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
