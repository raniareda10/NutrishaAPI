using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Allergies;
using DL.Entities;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace DL.Services.Allergy
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

        public async Task<IList<AllergyDto>> GetSelectedAllergiesAsync()
        {
            return await _appDbContext.MUserAllergy
                .Where(allergy => allergy.UserId == _currentUserService.UserId)
                .Select(allergy => new AllergyDto()
                {
                    Id = allergy.AllergyId,
                    Name = allergy.Allergy.Name
                }).ToListAsync();
        }

        public async Task<IList<AllergyDto>> GetAllAsync()
        {
            return await _appDbContext.MAllergy
                .Select(allergy => new AllergyDto()
                {
                    Id = allergy.Id,
                    Name = allergy.Name
                }).ToListAsync();
        }

        public async Task<BaseServiceResult> PutAsync(PutAllergyDto putAllergyDto)
        {
            var userAllergies = await _appDbContext.MUserAllergy
                .Where(allergy => allergy.UserId == _currentUserService.UserId)
                // .Select(al => al.AllergyId)
                .ToListAsync();

            foreach (var userAllergy in userAllergies)
            {
                if (putAllergyDto.AllergyIds.Contains(userAllergy.AllergyId))
                {
                    putAllergyDto.AllergyIds.Remove(userAllergy.AllergyId);
                }
                else
                {
                    _appDbContext.Remove(userAllergy);
                }
            }

            var newAllergies = putAllergyDto.AllergyIds.Select(x => new MUserAllergy()
            {
                AllergyId = x,
                UserId = _currentUserService.UserId
            }).ToList();

            await _appDbContext.MUserAllergy.AddRangeAsync(newAllergies);
            await _appDbContext.SaveChangesAsync();
            return new BaseServiceResult();
        }
    }
}