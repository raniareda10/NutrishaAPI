using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.SplashDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface ISplashRepository
    {
        PagedList<MSplash> GetAllSplash(SplashQueryPramter SplashParameters);

    }

    public class SplashRepository : Repository<MSplash>, ISplashRepository
    {
        public SplashRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MSplash> GetAllSplash(SplashQueryPramter SplashParameters)
        {

             

            IQueryable<MSplash> Splash = GetAll();


            //searhing
            SearchByPramter(ref Splash, SplashParameters.Tag);

            return PagedList<MSplash>.ToPagedList(Splash,
                SplashParameters.PageNumber,
                SplashParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MSplash> Splash, string Searchpramter)
        {
            if (!Splash.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            Splash = Splash.Where(o => o.Tag.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
