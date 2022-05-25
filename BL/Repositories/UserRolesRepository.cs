using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System.Collections.Generic;

namespace BL.Repositories
{
    public interface IUserRolesRepository
    { }

    public class UserRolesRepository : Repository<MUserRoles>, IUserRolesRepository
    {
        public UserRolesRepository(AppDBContext ctx) : base(ctx)
        { }

       
    }
}
