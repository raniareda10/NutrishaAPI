using System;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Users.Admins;
using DL.Entities;
using DL.Extensions;
using DL.Repositories.Roles;
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
            ICurrentUserService currentUserService
        )
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<AdminUserModel> GetCurrentUserAsync()
        {
            var user = await _dbContext.GetUserAsync(u => u.Id == _currentUserService.UserId);
            return user;
        }


        public async Task AssignRoleToUserAsync(AssignRoleToUserDto assignRoleToUserDto)
        {
            await _dbContext.AddAsync(new MUserRoles
            {
                // CreatedBy = _currentUserService.UserId,
                RoleId = assignRoleToUserDto.RoleId,
                UserId = assignRoleToUserDto.UserId,
                Created = DateTime.UtcNow,
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<dynamic>> GetPagedListAsync(GetAdminUserPagedListQueryDto model)
        {
            var userQuery = _dbContext.MUser.Where(m => m.IsAdmin);

            if (!string.IsNullOrWhiteSpace(model.SearchWord))
            {
                userQuery = userQuery.Where(
                    m => m.Email.Contains(model.SearchWord) || m.Name.Contains(model.SearchWord));
            }

            if (model.RoleId.HasValue)
            {
                userQuery = userQuery.Where(m => m.Roles.Any(r => r.RoleId == model.RoleId));
            }

            return await userQuery
                .Select(u => (dynamic)new
                {
                    u.Name,
                    u.Email,
                    Roles = u.Roles.Select(r => r.Role.Name)
                })
                .ToPagedListAsync(model);
        }

        public async Task CreateAdminUserAsync(CreateAdminDto createAdminDto)
        {
            var user = new MUser()
            {
                Email = createAdminDto.Email
            };


            if (createAdminDto.RoleId.HasValue)
            {
                var role = new MUserRoles()
                {
                    Created = DateTime.UtcNow,
                    RoleId = createAdminDto.RoleId.Value,
                    UserId = user.Id
                };

                _dbContext.Add(role);
            }

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
        }


        public async Task UpdateAdminUserAsync(CreateAdminDto createAdminDto)
        {
            var user = _dbContext.MUser.FirstOrDefaultAsync(m => m.Id == createAdminDto.Id);

            if (createAdminDto.RoleId.HasValue)
            {
                var role = new MUserRoles()
                {
                    Created = DateTime.UtcNow,
                    RoleId = createAdminDto.RoleId.Value,
                    UserId = user.Id
                };

                _dbContext.Add(role);
            }

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}