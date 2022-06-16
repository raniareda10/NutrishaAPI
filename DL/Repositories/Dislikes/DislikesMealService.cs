using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Allergies;
using DL.DtosV1.DisLikes;
using DL.EntitiesV1;
using DL.Repositories.Dislikes.Constants;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace DL.Repositories.Dislikes
{
    public class DislikesMealService
    {
        private readonly AppDBContext _appDbContext;
        private readonly ICurrentUserService _currentUserService;

        public DislikesMealService(AppDBContext appDbContext,
            ICurrentUserService currentUserService)
        {
            _appDbContext = appDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<IList<DislikesDto>> GetAllAsync()
        {
            return await _appDbContext.UserDislikes
                .Where(dislike => dislike.UserId == _currentUserService.UserId)
                .Select(dislike => new DislikesDto()
                {
                    Id = dislike.Id,
                    Name = dislike.Title,
                    IsSelected = dislike.IsSelected,
                    DislikeMealType = dislike.DislikeType
                }).ToListAsync();
        }

        public async Task<List<string>> GetSelectAllergyNamesAsync()
        {
            return await _appDbContext.UserDislikes
                .Where(dislike => dislike.UserId == _currentUserService.UserId && dislike.IsSelected)
                .Select(dislike => dislike.Title)
                .ToListAsync();
        }
        
        public async Task<BaseServiceResult> PutAsync(PutDisLikesDto putAllergyDto)
        {
            var userAllergies = await _appDbContext.UserDislikes
                .AsNoTracking()
                .Where(allergy => allergy.UserId == _currentUserService.UserId)
                .ToListAsync();
            
            
            foreach (var userDislikedMeal in userAllergies)
            {
                if (putAllergyDto.DislikedMealIds.Contains(userDislikedMeal.Id))
                {
                    if (userDislikedMeal.IsSelected) continue;

                    userDislikedMeal.IsSelected = true;
                    _appDbContext.Update(userDislikedMeal);
                }
                else
                {
                    if (!userDislikedMeal.IsSelected) continue;

                    userDislikedMeal.IsSelected = false;
                    _appDbContext.Update(userDislikedMeal);
                }
            }

            await _appDbContext.SaveChangesAsync();
            return new BaseServiceResult();
        }

        public async Task<DislikesDto> AddCustomAllergiesAsync(string dislikeMealName)
        {
            var disLikedMeal = CreateSharedDisLikedMeal(_currentUserService.UserId, 
                DislikeMealType.Other, dislikeMealName);
            
            disLikedMeal.IsSelected = true;
            await _appDbContext.UserDislikes.AddAsync(disLikedMeal);
            await _appDbContext.SaveChangesAsync();

            return new DislikesDto()
            {
                Id = disLikedMeal.Id,
                Name = dislikeMealName,
                IsSelected = true,
                DislikeMealType = DislikeMealType.Other
            };
        }

        public async Task AddDefaultDislikesAsync(int userId)
        {
            await _appDbContext.UserDislikes.AddRangeAsync(DislikeMealConstants.DislikedMealsType
                .Select(type => CreateSharedDisLikedMeal(userId, type)));

            await _appDbContext.SaveChangesAsync();
        }


        private UserDislikes CreateSharedDisLikedMeal(int userId, DislikeMealType dislikeMealType, string title = null)
        {
            return new UserDislikes()
            {
                Created = DateTime.UtcNow,
                Title = title ?? dislikeMealType.ToString(),
                DislikeType = dislikeMealType,
                UserId = userId
            };
        }

        
    }
}