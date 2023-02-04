using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Users.Admins;
using DL.Entities;
using DL.EntitiesV1.AdminUser;
using DL.Extensions;
using DL.MailModels;
using DL.Repositories.Roles;
using DL.Repositories.Users.Models;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DL.Repositories.Users.Admins
{
    public class AdminUserRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMailService _mailService;

        private readonly string _adminUrl;

        public AdminUserRepository(
            AppDBContext dbContext,
            ICurrentUserService currentUserService,
            IMailService mailService,
            IConfiguration configuration
        )
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mailService = mailService;
            _adminUrl = configuration["AdminPanelUrl"];
        }

        public async Task<AdminUserModel> GetCurrentUserAsync()
        {
            var user = await _dbContext.GetAdminUserAsync(u => u.Id == _currentUserService.UserId);
            return user;
        }


        public async Task AssignRoleToUserAsync(AssignRoleToUserDto assignRoleToUserDto)
        {
            await _dbContext.AddAsync(new MUserRoles
            {
                // CreatedBy = _currentUserService.UserId,
                RoleId = assignRoleToUserDto.RoleId,
                AdminUserId = assignRoleToUserDto.UserId,
                Created = DateTime.UtcNow,
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<dynamic>> GetPagedListAsync(GetAdminUserPagedListQueryDto model)
        {
            var userQuery = _dbContext.AdminUsers
                .Where(m => m.Id != _currentUserService.UserId);

            if (!string.IsNullOrWhiteSpace(model.SearchWord))
            {
                userQuery = userQuery.Where(
                    m => m.Email.Contains(model.SearchWord) || m.Name.Contains(model.SearchWord));
            }

            if (!string.IsNullOrWhiteSpace(model.RoleName))
            {
                userQuery = userQuery.Where(m => m.Roles.Any(r => r.Role.Name == model.RoleName));
            }

            return await userQuery
                .Select(u => (dynamic)new
                {
                    u.Id,
                    Name = u.Name,
                    u.Email,
                    Roles = u.Roles.Select(r => r.Role.Name)
                })
                .ToPagedListAsync(model);
        }

        public async Task<object> GetByIdAsync(long id)
        {
            return await _dbContext.AdminUsers
                .Where(m => m.Id == id)
                .Select(u => new
                {
                    u.Id,
                    Name = u.Name,
                    u.Email,
                    RoleId = u.Roles.Select(r => r.Role.Id).First()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PayloadServiceResult<long>> CreateAdminUserAsync(CreateAdminDto createAdminDto)
        {
            var result = new PayloadServiceResult<long>();
            var isAdminUserExists = await _dbContext.AdminUsers.AnyAsync(m => m.Email == createAdminDto.Email);
            
            if (isAdminUserExists)
            {
                result.Errors.Add("This email already exists please choose another one.");
                return result;
            }
            

            // var password = Guid.NewGuid().ToString().Replace("-", "").Substring(15, 15);
            var password = createAdminDto.Password;
            var user = new AdminUserEntity()
            {
                Name = createAdminDto.UserName,
                Email = createAdminDto.Email,
                Password = PasswordHasher.HashPassword(password),
                Roles = new List<MUserRoles>()
                {
                    new MUserRoles()
                    {
                        Created = DateTime.UtcNow,
                        RoleId = createAdminDto.RoleId.Value,
                    }
                }
            };

            _dbContext.AdminUsers.Add(user);
            await _dbContext.SaveChangesAsync();
            await _mailService.SendEmailAsync(GenerateAdminCreatedMail(user.Email, password));
            result.Data = user.Id;
            return result;
        }


        public async Task UpdateAdminUserAsync(UpdateAdminDto updateAdminDto)
        {
            var isRoleExists = await _dbContext.MRoles.Where(r => r.Id == updateAdminDto.RoleId).AnyAsync();

            if (!isRoleExists) return;

            var userRoles = await _dbContext.MUserRoles.Where(r => r.AdminUserId == updateAdminDto.UserId).ToListAsync();

            _dbContext.RemoveRange(userRoles);
            _dbContext.MUserRoles.Add(new MUserRoles()
            {
                AdminUserId = updateAdminDto.UserId,
                RoleId = updateAdminDto.RoleId.Value
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(long id)
        {
            await _dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM MUser WHERE id = {id}");
        }
        
        private MailRequest GenerateAdminCreatedMail(string email, string password)
        {
            var mailRequest = new MailRequest
            {
                ToEmail = email,
                Subject = "You have been invited to nutrisha",
            };

            var body = "<h1>Welcome to nutrisha</h1>" +
                       $"<p>Please use these credential to login <a href={_adminUrl}>Nutrisha</a></p>" +
                       "<p>Your credential</p>" +
                       $"<p>Email: {email}</p>" +
                       $"<p>Password: {password}</p>" +
                       "<p style='color: red'>Please Consider Change Your password as fast as possible</p>";

            mailRequest.Body = body;
            return mailRequest;
        }

        
    }
}