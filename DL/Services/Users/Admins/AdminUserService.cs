﻿using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Users.Admins;
using DL.ResultModels;
using DL.Services.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DL.Services.Users.Admins
{
    public class AdminUserService
    {
        private readonly AppDBContext _dbContext;
        private readonly TokenService _tokenService;

        public AdminUserService(
            AppDBContext dbContext,
            TokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        public async Task<PayloadServiceResult<dynamic>> LoginAsync(AdminLoginDto adminLoginDto)
        {
            var result = new PayloadServiceResult<dynamic>();
            var currentUser = await _dbContext.MUser
                .Where(u =>
                    u.IsAdmin &&
                    u.Email == adminLoginDto.Email
                )
                .Select(u => new AdminUserModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.Password,
                    Language = u.Language,
                    PersonalImage = u.PersonalImage
                })
                .FirstOrDefaultAsync();

            if (currentUser == null ||
                !PasswordHasher.IsEqual(adminLoginDto.Password, currentUser.Password))
            {
                result.Errors.Add(NonLocalizedErrorMessages.WrongCredential);
                return result;
            }

            var token = _tokenService.GenerateAdminToken(currentUser);

            result.Data = new
            {
                User = AdminLoggedInDto.FromAdminModel(currentUser),
                token = token
            };
            return result;
        }
    }
}