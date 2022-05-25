using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System.Collections.Generic;

namespace BL.Repositories
{
    public interface IRolesGroupRepository
    { }

    public class RolesGroupRepository : Repository<MRolesGroup>, IRolesGroupRepository
    {
        public RolesGroupRepository(AppDBContext ctx) : base(ctx)
        { }

       
    }
}
