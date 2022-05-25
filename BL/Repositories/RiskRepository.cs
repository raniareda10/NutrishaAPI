using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.RiskDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IRiskRepository
    {
        PagedList<MRisk> GetAllRisk(RiskQueryPramter RiskParameters);

    }

    public class RiskRepository : Repository<MRisk>, IRiskRepository
    {
        public RiskRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MRisk> GetAllRisk(RiskQueryPramter RiskParameters)
        {

             

            IQueryable<MRisk> Risk = GetAll();


            //searhing
            SearchByPramter(ref Risk, RiskParameters.Name);

            return PagedList<MRisk>.ToPagedList(Risk,
                RiskParameters.PageNumber,
                RiskParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MRisk> Risk, string Searchpramter)
        {
            if (!Risk.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            Risk = Risk.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
