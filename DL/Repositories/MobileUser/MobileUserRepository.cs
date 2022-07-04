using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;

namespace DL.Repositories.MobileUser
{
    public class MobileUserRepository
    {
        private readonly AppDBContext _dbContext;
        public MobileUserRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<dynamic> GetPagedListAsync()
        {
            return _dbContext.MUser.OrderBy(m => m.CreatedOn)
                .Select(m => new
                {
                    Id = m.Id,
                    m.Name,
                    m.Email,
                    PersonalImage = m.PersonalImage,
                });
        }


        public async Task GetUserDetailsAsync(int userId)
        {
            
        }
    }
}