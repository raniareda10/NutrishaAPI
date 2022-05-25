using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.AllergyDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IAllergyRepository
    {
        PagedList<MAllergy> GetAllAllergy(AllergyQueryPramter AllergyParameters);

    }

    public class AllergyRepository : Repository<MAllergy>, IAllergyRepository
    {
        public AllergyRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MAllergy> GetAllAllergy(AllergyQueryPramter AllergyParameters)
        {

             

            IQueryable<MAllergy> Allergy = GetAll();


            //searhing
            SearchByPramter(ref Allergy, AllergyParameters.Name);

            return PagedList<MAllergy>.ToPagedList(Allergy,
                AllergyParameters.PageNumber,
                AllergyParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MAllergy> Allergy, string Searchpramter)
        {
            if (!Allergy.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            Allergy = Allergy.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
