using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.AttachmentTypeDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IAttachmentTypeRepository
    {
        PagedList<MAttachmentType> GetAllAttachmentType(AttachmentTypeQueryPramter AttachmentTypeParameters);

    }

    public class AttachmentTypeRepository : Repository<MAttachmentType>, IAttachmentTypeRepository
    {
        public AttachmentTypeRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MAttachmentType> GetAllAttachmentType(AttachmentTypeQueryPramter AttachmentTypeParameters)
        {

             

            IQueryable<MAttachmentType> AttachmentType = GetAll();


            //searhing
            SearchByPramter(ref AttachmentType, AttachmentTypeParameters.Name);

            return PagedList<MAttachmentType>.ToPagedList(AttachmentType,
                AttachmentTypeParameters.PageNumber,
                AttachmentTypeParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MAttachmentType> AttachmentType, string Searchpramter)
        {
            if (!AttachmentType.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            AttachmentType = AttachmentType.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
