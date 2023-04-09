using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using DL.DBContext;
using DL.DtosV1.Users.Admins;
using DL.EntitiesV1.AdminUser;
using DL.Extensions;
using DL.MailModels;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DL.Repositories.Users.Admins
{
    public class AdminAuthService
    {
        private readonly AppDBContext _dbContext;
        private readonly IMailService _mailService;
        private readonly ICurrentUserService _currentUserService;
        private readonly TokenService _tokenService;

        private readonly string _adminUserPath;
        private readonly int _resetPasswordExpiryInMinutes;

        public AdminAuthService(
            IConfiguration configuration,
            AppDBContext dbContext,
            IMailService mailService,
            ICurrentUserService currentUserService,
            TokenService tokenService)
        {
            _adminUserPath = configuration["AdminPanelUrl"] + "/auth/reset-password";
            _resetPasswordExpiryInMinutes = int.Parse(configuration["ResetPassword:TokenExpiryInMinutes"]);
            _dbContext = dbContext;
            _mailService = mailService;
            _currentUserService = currentUserService;
            _tokenService = tokenService;
        }

        public async Task<PayloadServiceResult<dynamic>> LoginAsync(AdminLoginDto adminLoginDto)
        {
            var result = new PayloadServiceResult<dynamic>();
            var currentUser = await _dbContext.GetAdminUserAsync(u =>
                u.Email == adminLoginDto.Email);

            if (currentUser == null ||
                !PasswordHasher.IsEqual(adminLoginDto.Password, currentUser.Password))
            {
                result.Errors.Add(NonLocalizedErrorMessages.WrongCredential);
                return result;
            }
            if (currentUser.IsDeleted)
            {
                result.Errors.Add(NonLocalizedErrorMessages.DeletedUser);
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

        public async Task<BaseServiceResult> RequestResetPasswordAsync(RequestResetPasswordRequest requestResetPassword)
        {
            var user = await _dbContext.AdminUsers.AsNoTracking().Where(m => m.Email == requestResetPassword.Email)
                .Select(
                    u => new
                    {
                        u.Id,
                        Name = u.Name
                    }).FirstOrDefaultAsync();

            if (user is null) return new BaseServiceResult();

            var oldTokenEntity = await _dbContext.ResetUserPassword.FirstOrDefaultAsync(u => u.AdminUserId == u.Id);

            if (oldTokenEntity is not null)
            {
                _dbContext.Remove(oldTokenEntity);
            }

            var bytes = new byte[128];
            RandomNumberGenerator.Create().GetBytes(bytes);
            var token = Convert.ToBase64String(bytes);
            var encoded = HttpUtility.UrlEncode(token);
            var newTokenEntity = new ResetUserPasswordEntity()
            {
                Token = token,
                Created = DateTime.UtcNow,
                AdminUserId = user.Id
            };

            await _dbContext.AddAsync(newTokenEntity);
            await _dbContext.SaveChangesAsync();

            await _mailService.SendEmailAsync(new MailRequest()
            {
                Subject = "Nutrisha",
                ToEmail = requestResetPassword.Email,
                Body =
                    $@"<h3>Hello {user.Name}</h3>
 <p>Go To This Link To Reset Your password Please: <a href='{_adminUserPath}?token={encoded}'>Click here</a></p>"
            });

            return new BaseServiceResult();
        }

        public async Task<BaseServiceResult> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            var tokenModel = await _dbContext
                .ResetUserPassword
                .FirstOrDefaultAsync(u => u.Token == resetPasswordRequest.Token);

            if (tokenModel is null)
            {
                return new BaseServiceResult()
                {
                    Errors = new List<string>()
                    {
                        "InvalidToken  Please try resetting password again"
                    }
                };
            }

            if (tokenModel.Created.AddMinutes(_resetPasswordExpiryInMinutes) <= DateTime.UtcNow)
            {
                _dbContext.Remove(tokenModel);
                await _dbContext.SaveChangesAsync();
                return new BaseServiceResult()
                {
                    Errors = new List<string>()
                    {
                        "InvalidToken Please try resetting password again"
                    }
                };
            }

            var hashedPassword = PasswordHasher.HashPassword(resetPasswordRequest.Password);
            var user = await _dbContext.AdminUsers.FirstOrDefaultAsync(u => u.Id == tokenModel.AdminUserId);
            user.Password = hashedPassword;
            _dbContext.Update(user);
            _dbContext.Remove(tokenModel);
            await _dbContext.SaveChangesAsync();

            return new BaseServiceResult();
        }

        public async Task<BaseServiceResult> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            var user = await _dbContext.AdminUsers.FirstOrDefaultAsync(m => m.Id == _currentUserService.UserId);
            var oldHashedPassword = PasswordHasher.HashPassword(changePasswordRequest.OldPassword);

            if (oldHashedPassword != user.Password)
            {
                return new BaseServiceResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid Old Password, Please enter correct password."
                    }
                };
            }

            var hashedPassword = PasswordHasher.HashPassword(changePasswordRequest.NewPassword);
            user.Password = hashedPassword;
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();

            return new BaseServiceResult();
        }
    }
}