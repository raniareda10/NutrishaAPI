using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System.Collections.Generic;

namespace BL.Repositories
{
    public interface IGroupRepository
    { }

    public class GroupRepository : Repository<MGroup>, IGroupRepository
    {
        public GroupRepository(AppDBContext ctx) : base(ctx)
        { }

       
    }
}
