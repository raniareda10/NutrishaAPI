using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Users.Admins;
using DL.Extensions;
using DL.Repositories.Users.Models;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Users.Admins
{
    public class AdminUserRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public AdminUserRepository(
            AppDBContext dbContext,
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        
        public async Task<AdminUserModel> GetCurrentUserAsync()
        {
            return await _dbContext.MUser.GetUserByIdAsync(_currentUserService.UserId);
        }
    }
}