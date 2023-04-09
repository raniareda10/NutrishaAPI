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
                    NameAr = dislike.TitleAr,
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

        public async Task<DislikesDto> AddCustomAllergiesAsync(string dislikeMealName, string dislikeMealNameAr)
        {
            if (dislikeMealName == null || dislikeMealName == "")
            {
                dislikeMealName = dislikeMealNameAr;
            }
            if (dislikeMealNameAr == null || dislikeMealNameAr == "")
            {
                dislikeMealNameAr = dislikeMealName;
            }
            var disLikedMeal = CreateSharedDisLikedMeal(_currentUserService.UserId, dislikeMealName, dislikeMealNameAr,13);

            disLikedMeal.IsSelected = true;
            await _appDbContext.UserDislikes.AddAsync(disLikedMeal);
            await _appDbContext.SaveChangesAsync();

            return new DislikesDto()
            {
                Id = disLikedMeal.Id,
                Name = dislikeMealName,
                NameAr = dislikeMealNameAr,
                IsSelected = true,
                DislikeMealType = 13
            };
        }

        public async Task AddDefaultDislikesAsync(int userId)
        {

            await _appDbContext.AddRangeAsync(new object[]
          {
                CreateSharedDisLikedMeal(userId, "WhiteFish","السمك الابيض",0),
                CreateSharedDisLikedMeal(userId, "Tuna","تونه",1),
                CreateSharedDisLikedMeal(userId, "Salmon","سالمون",2),
                CreateSharedDisLikedMeal(userId, "Shrimp","جمبرى",3),
                CreateSharedDisLikedMeal(userId, "RedMeat","لحم احمر",4),
                CreateSharedDisLikedMeal(userId, "Poultry","دواجن",5),
                CreateSharedDisLikedMeal(userId, "Park","الخضار",6),
                CreateSharedDisLikedMeal(userId, "Eggs","بيض",7),
                CreateSharedDisLikedMeal(userId, "BlueCheese","جبنة زرقاء",8),
                CreateSharedDisLikedMeal(userId, "GoatCheese","جبن الماعز",9),
                CreateSharedDisLikedMeal(userId, "Mayonnaise","مايونيز",10),
                CreateSharedDisLikedMeal(userId, "Avocado","افوكادو",11),
                CreateSharedDisLikedMeal(userId, "Banana","الموز",12),
                CreateSharedDisLikedMeal(userId, "Other","اخرى",13),

          });

            await _appDbContext.SaveChangesAsync();
        }


        private UserDislikes CreateSharedDisLikedMeal(int userId, string title = null, string titleAr = null,int dislikeMealTypeId=0)
        {
            return new UserDislikes()
            {
                Created = DateTime.UtcNow,
                Title = title,
                TitleAr = titleAr,
                DislikeType = dislikeMealTypeId,
                UserId = userId
            };
        }


    }
}