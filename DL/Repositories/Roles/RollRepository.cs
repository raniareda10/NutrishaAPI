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
                .Where(r => r.UserId == userId)
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

        public async Task<PagedResult<LookupItem>> GetPagedListAsync(GetPagedListQueryModel postRoleDto)
        {
            return await _dbContext
                .MRoles
                .Select(m => new LookupItem(m.Id, m.Name))
                .ToPagedListAsync(postRoleDto);
        }
    }
}