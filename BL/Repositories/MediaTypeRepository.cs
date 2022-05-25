using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.MediaTypeDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IMediaTypeRepository
    {
        PagedList<MMediaType> GetAllMediaType(MediaTypeQueryPramter MediaTypeParameters);

    }

    public class MediaTypeRepository : Repository<MMediaType>, IMediaTypeRepository
    {
        public MediaTypeRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MMediaType> GetAllMediaType(MediaTypeQueryPramter MediaTypeParameters)
        {

             

            IQueryable<MMediaType> MediaType = GetAll();


            //searhing
            SearchByPramter(ref MediaType, MediaTypeParameters.Name);

            return PagedList<MMediaType>.ToPagedList(MediaType,
                MediaTypeParameters.PageNumber,
                MediaTypeParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MMediaType> MediaType, string Searchpramter)
        {
            if (!MediaType.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            MediaType = MediaType.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
