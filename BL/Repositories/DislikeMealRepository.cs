using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.DislikeMealDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IDislikeMealRepository
    {
        PagedList<MDislikeMeal> GetAllDislikeMeal(DislikeMealQueryPramter DislikeMealParameters);

    }

    public class DislikeMealRepository : Repository<MDislikeMeal>, IDislikeMealRepository
    {
        public DislikeMealRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MDislikeMeal> GetAllDislikeMeal(DislikeMealQueryPramter DislikeMealParameters)
        {

             

            IQueryable<MDislikeMeal> DislikeMeal = GetAll().Include(dt => dt.Meal);


            //searhing
            SearchByPramter(ref DislikeMeal,DislikeMealParameters.UserId,  DislikeMealParameters.MealId);

            return PagedList<MDislikeMeal>.ToPagedList(DislikeMeal,
                DislikeMealParameters.PageNumber,
                DislikeMealParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MDislikeMeal> DislikeMeal, int? userId, int? mealId)
        {
            //if (DislikeMeal.Any() && sourceId > 0)
            //{
            //    DislikeMeal = DislikeMeal.Where(o => o.SourceId == sourceId);
            //}

            //if (DislikeMeal.Any() && adminId > 0)
            //{
            //    DislikeMeal = DislikeMeal.Where(o => o.AdminId == adminId);
            //}
            //if (DislikeMeal.Any() && customerId > 0)
            //{
            //    DislikeMeal = DislikeMeal.Where(o => o.CustomerId == customerId);
            //}
            if (DislikeMeal.Any() && userId > 0)
            {
                DislikeMeal = DislikeMeal.Where(o => o.UserId == userId);
            }
            if (DislikeMeal.Any() && mealId > 0)
            {
                DislikeMeal = DislikeMeal.Where(o => o.MealId == mealId);
            }
            return;
        }


    }
}
