using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.FrequencyDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IFrequencyRepository
    {
        PagedList<MFrequency> GetAllFrequency(FrequencyQueryPramter FrequencyParameters);

    }

    public class FrequencyRepository : Repository<MFrequency>, IFrequencyRepository
    {
        public FrequencyRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MFrequency> GetAllFrequency(FrequencyQueryPramter FrequencyParameters)
        {

             

            IQueryable<MFrequency> Frequency = GetAll();


            //searhing
            SearchByPramter(ref Frequency, FrequencyParameters.Name);

            return PagedList<MFrequency>.ToPagedList(Frequency,
                FrequencyParameters.PageNumber,
                FrequencyParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MFrequency> Frequency, string Searchpramter)
        {
            if (!Frequency.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            Frequency = Frequency.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
