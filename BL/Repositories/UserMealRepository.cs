using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.UserMealDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IUserMealRepository
    {
        PagedList<MUserMeal> GetAllUserMeal(UserMealQueryPramter UserMealParameters);

    }

    public class UserMealRepository : Repository<MUserMeal>, IUserMealRepository
    {
        public UserMealRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MUserMeal> GetAllUserMeal(UserMealQueryPramter UserMealParameters)
        {

             

            IQueryable<MUserMeal> UserMeal = GetAll().Include(dt => dt.Meal);


            //searhing
            SearchByPramter(ref UserMeal,UserMealParameters.UserId,  UserMealParameters.MealId);

            return PagedList<MUserMeal>.ToPagedList(UserMeal,
                UserMealParameters.PageNumber,
                UserMealParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MUserMeal> UserMeal, int? userId, int? allergyId)
        {
            //if (UserMeal.Any() && sourceId > 0)
            //{
            //    UserMeal = UserMeal.Where(o => o.SourceId == sourceId);
            //}

            //if (UserMeal.Any() && adminId > 0)
            //{
            //    UserMeal = UserMeal.Where(o => o.AdminId == adminId);
            //}
            //if (UserMeal.Any() && customerId > 0)
            //{
            //    UserMeal = UserMeal.Where(o => o.CustomerId == customerId);
            //}
            if (UserMeal.Any() && userId > 0)
            {
                UserMeal = UserMeal.Where(o => o.UserId == userId);
            }
            if (UserMeal.Any() && allergyId > 0)
            {
                UserMeal = UserMeal.Where(o => o.MealId == allergyId);
            }
            return;
        }


    }
}
