using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Permissions
{
    public class PermissionRepository
    {
        private readonly AppDBContext _appDbContext;

        public PermissionRepository(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> DoesUserHasPermissionsAsync(int userId, string permissionName)
        {
            return await _appDbContext.MUserRoles.Where(m => m.AdminUserId == userId)
                .Where(m => m.Role.RolePermissions.Any(p => p.Permission.Name == permissionName))
                .AnyAsync();
        }
    }
}