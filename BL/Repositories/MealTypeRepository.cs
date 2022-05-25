using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.MealTypeDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IMealTypeRepository
    {
        PagedList<MMealType> GetAllMealType(MealTypeQueryPramter MealTypeParameters);

    }

    public class MealTypeRepository : Repository<MMealType>, IMealTypeRepository
    {
        public MealTypeRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MMealType> GetAllMealType(MealTypeQueryPramter MealTypeParameters)
        {

             

            IQueryable<MMealType> MealType = GetAll();


            //searhing
            SearchByPramter(ref MealType, MealTypeParameters.Name);

            return PagedList<MMealType>.ToPagedList(MealType,
                MealTypeParameters.PageNumber,
                MealTypeParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MMealType> MealType, string Searchpramter)
        {
            if (!MealType.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            MealType = MealType.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
