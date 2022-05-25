using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.IngredientDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IIngredientRepository
    {
        PagedList<MIngredient> GetAllIngredient(IngredientQueryPramter IngredientParameters);

    }

    public class IngredientRepository : Repository<MIngredient>, IIngredientRepository
    {
        public IngredientRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MIngredient> GetAllIngredient(IngredientQueryPramter IngredientParameters)
        {

             

            IQueryable<MIngredient> Ingredient = GetAll();


            //searhing
            SearchByPramter(ref Ingredient, IngredientParameters.Name);

            return PagedList<MIngredient>.ToPagedList(Ingredient,
                IngredientParameters.PageNumber,
                IngredientParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MIngredient> Ingredient, string Searchpramter)
        {
            if (!Ingredient.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            Ingredient = Ingredient.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
