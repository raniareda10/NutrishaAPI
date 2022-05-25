using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.GenderDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IGenderRepository
    {
        PagedList<MGender> GetAllGender(GenderQueryPramter GenderParameters);

    }

    public class GenderRepository : Repository<MGender>, IGenderRepository
    {
        public GenderRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MGender> GetAllGender(GenderQueryPramter GenderParameters)
        {

             

            IQueryable<MGender> Gender = GetAll();


            //searhing
            SearchByPramter(ref Gender, GenderParameters.Name);

            return PagedList<MGender>.ToPagedList(Gender,
                GenderParameters.PageNumber,
                GenderParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MGender> Gender, string Searchpramter)
        {
            if (!Gender.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            Gender = Gender.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
