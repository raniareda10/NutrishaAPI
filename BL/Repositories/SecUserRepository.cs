using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.SecUserDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface ISecUserRepository
    {
        PagedList<SecUser> GetAllSecUser(SecUserQueryPramter SecUserParameters);

    }

    public class SecUserRepository : Repository<SecUser>, ISecUserRepository
    {
        public SecUserRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<SecUser> GetAllSecUser(SecUserQueryPramter SecUserParameters)
        {

             

            IQueryable<SecUser> SecUser = GetAll();


            //searhing
            SearchByPramter(ref SecUser, SecUserParameters.Id);

            return PagedList<SecUser>.ToPagedList(SecUser,
                SecUserParameters.PageNumber,
                SecUserParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<SecUser> SecUser, int Searchpramter)
        {
            if (!SecUser.Any() || Searchpramter <= 0)
                return;
            SecUser = SecUser.Where(o => o.Id==Searchpramter);
        }


    }
}
