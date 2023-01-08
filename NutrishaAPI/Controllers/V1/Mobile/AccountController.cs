using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DL;
using DL.DBContext;
using DL.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    public class AccountController : BaseMobileController
    {
        private readonly AppDBContext _appDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStorageService _storageService;

        private readonly string _hostDomain;

        public AccountController(IConfiguration configuration, AppDBContext appDbContext,
            ICurrentUserService currentUserService,
            IStorageService storageService)
        {
            _appDbContext = appDbContext;
            _currentUserService = currentUserService;
            _storageService = storageService;
            _hostDomain = configuration["Domain"];
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            // For some reason deleting Muser in server throw conflict error
            // so i have to remove dependent entites first
            var user = await _appDbContext
                .MUser
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == _currentUserService.UserId);
            
            await _appDbContext.Database.ExecuteSqlRawAsync(@$"
                DELETE FROM Comments WHERE UserId = {_currentUserService.UserId};
                DELETE FROM ContactSupports WHERE UserId = {_currentUserService.UserId};
                DELETE FROM MDislikeMeal WHERE UserId = {_currentUserService.UserId};
                DELETE FROM MealPlans WHERE UserId = {_currentUserService.UserId};
                DELETE FROM MNotificationUser WHERE UserId = {_currentUserService.UserId};
                DELETE FROM MUserAllergy WHERE UserId = {_currentUserService.UserId};
                DELETE FROM MUserGoal WHERE UserId = {_currentUserService.UserId};
                DELETE FROM MUserGroups WHERE UserId = {_currentUserService.UserId};
                DELETE FROM MUserMeal WHERE UserId = {_currentUserService.UserId};
                DELETE FROM MUserRoles WHERE UserId = {_currentUserService.UserId};
                DELETE FROM Reactions WHERE UserId = {_currentUserService.UserId};
                DELETE FROM Reminders WHERE UserId = {_currentUserService.UserId};
                DELETE FROM ShoppingCarts WHERE UserId = {_currentUserService.UserId};
                DELETE FROM UserAllergy WHERE UserId = {_currentUserService.UserId};
                DELETE FROM UserDislikes WHERE UserId = {_currentUserService.UserId};
                DELETE FROM UserFavoriteMeals WHERE UserId = {_currentUserService.UserId};
                DELETE FROM UserMeasurements WHERE UserId = {_currentUserService.UserId};
                DELETE FROM UserPreventions WHERE UserId = {_currentUserService.UserId};

                DELETE FROM MUser WHERE Id = {_currentUserService.UserId};
            ");

            try
            {
                var currentDirectory = Directory.GetCurrentDirectory() + "/wwwroot";
                if (!string.IsNullOrWhiteSpace(user.PersonalImage))
                {
                    try
                    {
                        System.IO.File.Delete(user.PersonalImage.Replace(_hostDomain, currentDirectory));
                    }
                    catch (Exception e)
                    {
                        // ignore
                    }
                }

                DeleteDirectory($"{currentDirectory}/{"Users/" + _currentUserService.UserId}");
            }
            catch (Exception e)
            {
                // ignore
            }

            return EmptyResult();
        }

        [HttpPost("UploadMedia")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadMediaAsync([FromForm] FormFileCollection files)
        {
            if (files.Count == 0) return EmptyResult();
            var user = await _appDbContext.MUser.FirstOrDefaultAsync(m => m.Id == _currentUserService.UserId);
            var result = await _storageService.UploadFilesAsync(files, "Users/" + user.Id);

            if (user.Files is null)
            {
                user.Files = result;
            }
            else
            {
                user.Files.AddRange(result);
                user.Files = user.Files.OrderByDescending(f => f.Created).ToList();
            }

            _appDbContext.Update(user);
            await _appDbContext.SaveChangesAsync();
            return EmptyResult();
        }

        public void DeleteDirectory(string path)
        {
            var files = Directory.GetFiles(path);

            foreach (var directory in files)
            {
                System.IO.File.Delete(directory);
            }


            var directories = Directory.GetDirectories(path);

            foreach (var directory in directories)
            {
                DeleteDirectory(directory);
            }

            Directory.Delete(path);
        }
    }
}