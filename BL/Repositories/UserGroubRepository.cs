using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System.Collections.Generic;

namespace BL.Repositories
{
    public interface IUserGroupRepository
    { }

    public class UserGroupRepository : Repository<MUserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(AppDBContext ctx) : base(ctx)
        { }

       
    }
}
