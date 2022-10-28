﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Users.Admins;
using DL.Entities;
using DL.Extensions;
using DL.MailModels;
using DL.Repositories.Roles;
using DL.Repositories.Users.Models;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Users.Admins
{
    public class AdminUserRepository
    {
        private string DefaultPassword = "Nutrisha@?16";
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMailService _mailService;


        private readonly string adminUrl = "";

        public AdminUserRepository(
            AppDBContext dbContext,
            ICurrentUserService currentUserService,
            IMailService mailService
        )
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mailService = mailService;
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

            if (!string.IsNullOrWhiteSpace(model.RoleName))
            {
                userQuery = userQuery.Where(m => m.Roles.Any(r => r.Role.Name == model.RoleName));
            }

            return await userQuery
                .Select(u => (dynamic)new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    Roles = u.Roles.Select(r => r.Role.Name)
                })
                .ToPagedListAsync(model);
        }

        public async Task<object> GetByIdAsync(int id)
        {
            return await _dbContext.MUser
                .Where(m => m.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    RoleId = u.Roles.Select(r => r.Role.Id).First()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PayloadServiceResult<int>> CreateAdminUserAsync(CreateAdminDto createAdminDto)
        {
            var result = new PayloadServiceResult<int>();
            var isEmailExists = await _dbContext.MUser.AnyAsync(m => m.Email == createAdminDto.Email);

            if (isEmailExists)
            {
                result.Errors.Add("This email already exists please choose another one.");
                return result;
            }

            var password = Guid.NewGuid().ToString().Replace("-", "").Substring(15, 15);
            var user = new MUser()
            {
                Email = createAdminDto.Email,
                Password = PasswordHasher.HashPassword(password),
                IsAdmin = true,
                IsAccountVerified = true
            };

            var role = new MUserRoles()
            {
                Created = DateTime.UtcNow,
                RoleId = createAdminDto.RoleId.Value,
                UserId = user.Id
            };

            _dbContext.Add(role);
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            await _mailService.SendEmailAsync(GenerateAdminCreatedMail(user.Email, password));
            result.Data = user.Id;
            return result;
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


        private MailRequest GenerateAdminCreatedMail(string email, string password)
        {
            var mailRequest = new MailRequest
            {
                ToEmail = email,
                Subject = "You have been invited to nutrisha",
            };

            var body = "<h1>Welcome to nutrisha</h1>" +
                       $"<p>Please use these credential to login <a href={adminUrl}>Nutrisha</a></p>" +
                       "<p>Your credential</p>" +
                       $"<p>Email: {email}</p>" +
                       $"<p>Password: {password}</p>" +
                       "<p style='color: red'>Please Consider Change Your password as fast as possible</p>";

            mailRequest.Body = body;
            return mailRequest;
        }
    }
}