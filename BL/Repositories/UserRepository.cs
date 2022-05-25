using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System.Collections.Generic;

namespace BL.Repositories
{
    public interface IUserRepository
    { }

    public class UserRepository : Repository<MUser>, IUserRepository
    {
        public UserRepository(AppDBContext ctx) : base(ctx)
        { }

       
    }
}
