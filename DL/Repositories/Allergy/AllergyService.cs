using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Allergies;
using DL.Entities;
using DL.EntitiesV1.Allergies;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Allergy
{
    public class AllergyService
    {
        private readonly AppDBContext _appDbContext;
        private readonly ICurrentUserService _currentUserService;

        public AllergyService(AppDBContext appDbContext, ICurrentUserService currentUserService)
        {
            _appDbContext = appDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<IList<AllergyDto>> GetAllAsync(string _locale)
        {
            if (_locale.Contains("ar"))
            {
                return await _appDbContext.UserAllergy
                .Where(allergy => allergy.UserId == _currentUserService.UserId)
                .Select(allergy => new AllergyDto()
                {
                    Id = allergy.Id,
                    Name = allergy.TitleAr,
                    NameAr = allergy.TitleAr,
                    IsSelected = allergy.IsSelected,
                    IsCreatedByUser = allergy.IsCreatedByUser
                }).ToListAsync();
            }
            else
            {
                return await _appDbContext.UserAllergy
                               .Where(allergy => allergy.UserId == _currentUserService.UserId)
                               .Select(allergy => new AllergyDto()
                               {
                                   Id = allergy.Id,
                                   Name = allergy.Title,
                                   NameAr = allergy.TitleAr,
                                   IsSelected = allergy.IsSelected,
                                   IsCreatedByUser = allergy.IsCreatedByUser
                               }).ToListAsync();

            }
            }

        
        public async Task<IList<string>> GetSelectAllergyNamesAsync(string _locale)
        {
            if (_locale.Contains("ar"))
            {
                return await _appDbContext.UserAllergy
                .Where(allergy => allergy.UserId == _currentUserService.UserId && allergy.IsSelected)
                .Select(allergy => allergy.TitleAr).ToListAsync();
            }
            else
            {
                return await _appDbContext.UserAllergy
                            .Where(allergy => allergy.UserId == _currentUserService.UserId && allergy.IsSelected)
                            .Select(allergy => allergy.Title).ToListAsync();

            }
           }
        
        public async Task<BaseServiceResult> PutAsync(PutAllergyDto putAllergyDto)
        {
            var userAllergies = await _appDbContext.UserAllergy
                .AsNoTracking()
                .Where(allergy => allergy.UserId == _currentUserService.UserId)
                .ToListAsync();

            //1,2,3,4,5
            // 1, 5
            foreach (var userAllergy in userAllergies)
            {
                if (putAllergyDto.AllergyIds.Contains(userAllergy.Id))
                {
                    if (userAllergy.IsSelected) continue;
                    
                    userAllergy.IsSelected = true;
                    _appDbContext.Update(userAllergy);
                }
                else
                {
                    if (!userAllergy.IsSelected) continue;
                    
                    userAllergy.IsSelected = false;
                    _appDbContext.Update(userAllergy);
                }
            }

            await _appDbContext.SaveChangesAsync();
            return new BaseServiceResult();
        }

        public async Task<AllergyDto> AddCustomAllergiesAsync(string allergyName, string allergyNameAr)
        {
            if(allergyName == null || allergyName == "")
            {
                allergyName = allergyNameAr;
            }
            if (allergyNameAr == null || allergyNameAr == "")
            {
                allergyNameAr = allergyName;
            }
            var userAllergy = CreateSharedUserAllergyEntity(_currentUserService.UserId, allergyName, allergyNameAr);
            userAllergy.IsCreatedByUser = true;
            userAllergy.IsSelected = true;
            await _appDbContext.UserAllergy.AddAsync(userAllergy);
            await _appDbContext.SaveChangesAsync();

            return new AllergyDto()
            {
                Id = userAllergy.Id,
                Name = allergyName,
                NameAr = allergyNameAr,
                IsSelected = true,
                IsCreatedByUser = true
            };
        }
        
        public async Task AddDefaultAllergiesToUser(int userId)
        {
            await _appDbContext.AddRangeAsync(new object[]
            {
                CreateSharedUserAllergyEntity(userId, "Dairy","البان"),
                CreateSharedUserAllergyEntity(userId, "Egg","بيض"),
                CreateSharedUserAllergyEntity(userId, "Fish","سمك"),
                CreateSharedUserAllergyEntity(userId, "Shellfish","المحار"),
                CreateSharedUserAllergyEntity(userId, "Peanuts","الفول السودانى"),
                CreateSharedUserAllergyEntity(userId, "Sesame","السمسم"),
                CreateSharedUserAllergyEntity(userId, "Gluten","الغولتين"),
            });

            await _appDbContext.SaveChangesAsync();
        }


        private UserAllergy CreateSharedUserAllergyEntity(int userId, string title, string titleAr)
        {
            return new UserAllergy()
            {
                Created = DateTime.UtcNow,
                UserId = userId,
                Title = title,
                 TitleAr = titleAr
            };
        }
    }
}