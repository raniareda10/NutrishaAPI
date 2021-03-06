using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Users.Mobiles;
using DL.EntitiesV1.Users;
using DL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.MobileUser
{
    public class MobileUserRepository
    {
        private readonly AppDBContext _dbContext;

        public MobileUserRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<MobileUserListDto>> GetPagedListAsync(GetPagedListQueryModel model)
        {
            var query = _dbContext.MUser
                .Where(m => !m.IsAdmin)
                .OrderByDescending(m => m.CreatedOn).AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.SearchWord))
            {
                query = query.Where(m => m.Email.Contains(model.SearchWord) ||
                                         m.Mobile.Contains(model.SearchWord) ||
                                         m.Name.Contains(model.SearchWord));
            }

            return await query
                .Select(m => new MobileUserListDto
                {
                    Id = m.Id,
                    Email = m.Email,
                    Name = m.Name,
                    PhoneNumber = m.Mobile,
                    ProfileImage = m.PersonalImage,
                    Created = m.CreatedOn,
                    SubscribeDate = null,
                    TotalPaidAmount = null
                })
                .ToPagedListAsync(model);
        }


        public async Task<MobileUserDetailsDto> GetUserDetailsAsync(int userId)
        {
            var user = await _dbContext.MUser
                .Select(m => new MobileUserDetailsDto
                {
                    Id = m.Id,
                    Email = m.Email,
                    Name = m.Name,
                    PhoneNumber = m.Mobile,
                    ProfileImage = m.PersonalImage,
                    Created = m.CreatedOn,
                    SubscribeDate = null,
                    TotalPaidAmount = null,
                    Totals = m.Totals,
                    Age = m.Age,
                    Height = m.Height,
                    Gender = m.Gender.Name
                })
                .FirstOrDefaultAsync(m => m.Id == userId);

            if (user == null)
            {
                return null;
            }

            user.Allergies = await _dbContext.UserAllergy
                .Where(allergy => allergy.UserId == userId && allergy.IsSelected)
                .Select(allergy => allergy.Title).ToListAsync();

            user.Dislikes = await _dbContext.UserDislikes
                .Where(dislike => dislike.UserId == userId && dislike.IsSelected)
                .Select(dislike => dislike.Title).ToListAsync();

            return user;
        }

        public async Task PreventUserAsync(PreventUserDto preventUserDto)
        {
            var userPrevention = new MobileUserPreventionEntity()
            {
                Created = DateTime.UtcNow,
                UserId = preventUserDto.UserId,
                PreventionType = preventUserDto.PreventionType
            };

            await _dbContext.UserPreventions.AddAsync(userPrevention);
            await _dbContext.SaveChangesAsync();
        }
    }
}