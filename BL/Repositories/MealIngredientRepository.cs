using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.MealIngredientDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IMealIngredientRepository
    {
        PagedList<MMealIngredient> GetAllMealIngredient(MealIngredientQueryPramter MealIngredientParameters);

    }

    public class MealIngredientRepository : Repository<MMealIngredient>, IMealIngredientRepository
    {
        public MealIngredientRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MMealIngredient> GetAllMealIngredient(MealIngredientQueryPramter MealIngredientParameters)
        {

             

            IQueryable<MMealIngredient> MealIngredient = GetAll().Include(dt => dt.Ingredient);


            //searhing
            SearchByPramter(ref MealIngredient,MealIngredientParameters.MealId,  MealIngredientParameters.IngredientId);

            return PagedList<MMealIngredient>.ToPagedList(MealIngredient,
                MealIngredientParameters.PageNumber,
                MealIngredientParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MMealIngredient> MealIngredient, int? userId, int? allergyId)
        {
            //if (MealIngredient.Any() && sourceId > 0)
            //{
            //    MealIngredient = MealIngredient.Where(o => o.SourceId == sourceId);
            //}

            //if (MealIngredient.Any() && adminId > 0)
            //{
            //    MealIngredient = MealIngredient.Where(o => o.AdminId == adminId);
            //}
            //if (MealIngredient.Any() && customerId > 0)
            //{
            //    MealIngredient = MealIngredient.Where(o => o.CustomerId == customerId);
            //}
            if (MealIngredient.Any() && userId > 0)
            {
                MealIngredient = MealIngredient.Where(o => o.MealId == userId);
            }
            if (MealIngredient.Any() && allergyId > 0)
            {
                MealIngredient = MealIngredient.Where(o => o.IngredientId == allergyId);
            }
            return;
        }


    }
}
