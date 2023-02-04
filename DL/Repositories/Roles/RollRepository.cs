using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Common;
using DL.Entities;
using DL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Roles
{
    public class RollRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public RollRepository(
            AppDBContext dbContext,
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<dynamic>> GetAllRolesAsync()
        {
            return await _dbContext.MRoles.OrderByDescending(m => m.Name)
                .Select(r => new
                {
                    r.Id,
                    r.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(int? userId = null)
        {
            return await _dbContext.MUserRoles
                .Where(r => r.AdminUserId == userId)
                .OrderByDescending(m => m.Role.Name)
                .Select(r => r.Role.Name)
                .ToListAsync();
        }

        public async Task<long> PostAsync(string roleName)
        {
            var role = new MRole
            {
                // CreatedBy = _currentUserService.UserId,
                Name = roleName,
                // IsActive = true,
                // CreatedOn = DateTime.UtcNow,
            };

            await _dbContext.AddAsync(role);
            await _dbContext.SaveChangesAsync();

            return role.Id;
        }

        public async Task<PagedResult<LookupItem>> GetPagedListAsync(GetPagedListQueryModel queryModel)
        {
            var query = _dbContext
                .MRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.SearchWord))
            {
                query = query.Where(r => r.Name.Contains(queryModel.SearchWord));
            }

            return await query.Select(m => new LookupItem(m.Id, m.Name)).ToPagedListAsync(queryModel);
        }

        public async Task<RoleDetails> GetDetailsAsync(long id)
        {
            return await _dbContext.MRoles.Where(r => r.Id == id)
                .Select(r => new RoleDetails()
                {
                    RoleName = r.Name,
                    Permissions = r.RolePermissions.Select(rP => rP.Permission.Name)
                })
                .FirstOrDefaultAsync();
        }
    }

    public class RoleDetails
    {
        public string RoleName { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}