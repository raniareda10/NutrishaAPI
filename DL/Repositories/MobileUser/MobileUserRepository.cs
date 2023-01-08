using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.MealPlans;
using DL.DtosV1.Users.Admins;
using DL.DtosV1.Users.Mobiles;
using DL.EntitiesV1.Measurements;
using DL.EntitiesV1.Users;
using DL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.MobileUser
{
    public class MobileUserRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public MobileUserRepository(AppDBContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<MobileUserListDto>> GetPagedListAsync(GetUserMobilePagedListQueryModel model)
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

            if (model.OnlyUserWithoutPlan)
            {
                query = query.Where(m => !m.Plans.Any() && m.SubscriptionType != null);
            }

            if (model.UserWithAboutToFinishPlan)
            {
                var currentDate = DateTime.UtcNow.AddHours(_currentUserService.UserTimeZoneDifference).Date;

                query = query.Where(m => m.Plans.Any(plan =>
                    plan.EndDate.HasValue &&
                    plan.EndDate.Value
                        .AddHours(_currentUserService.UserTimeZoneDifference)
                        .AddDays(-1) == currentDate));
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
                    SubscriptionDate = m.SubscriptionDate,
                    SubscriptionType = m.SubscriptionType,
                    TotalPaidAmount = m.TotalAmountPaid
                })
                .ToPagedListAsync(model);
        }

        public async Task<MobileUserDetailsDto> GetUserDetailsAsync(int mobileUserId)
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
                    SubscriptionDate = m.SubscriptionDate,
                    SubscriptionType = m.SubscriptionType,
                    TotalPaidAmount = m.TotalAmountPaid,
                    Totals = m.Totals,
                    Age = m.Age,
                    Height = m.Height,
                    Gender = m.Gender.Name,
                    LastMessage = m.LastMessage,
                    HasNewMessage = m.HasNewMessage,
                    
                })
                .FirstOrDefaultAsync(m => m.Id == mobileUserId);

            if (user == null)
            {
                return null;
            }

            var lastWeight = await _dbContext.UserMeasurements
                .OrderByDescending(m => m.Created)
                .Where(m => m.UserId == mobileUserId)
                .Where(m => m.MeasurementType == MeasurementType.Weight)
                .Select(w => w.MeasurementValue)
                .FirstOrDefaultAsync();

            user.UserRisks = await _dbContext.MUserRisk.Where(m => m.UserId == mobileUserId).Select(m => m.Risk.Name).ToListAsync();
            user.WeightLoss = await _dbContext.GetWeightLossAsync(mobileUserId, lastWeight);
            user.CurrentWeight = lastWeight;
            user.LastUsedTemplates = _dbContext
                .MealPlans
                .Where(plan => plan.UserId == mobileUserId)
                .OrderByDescending(plan => plan.StartDate)
                .Take(8)
                .Select(plan => new UserPlanTemplateDto
                {
                    StartDate = plan.StartDate,
                    TemplateName = plan.ParentTemplate.TemplateName
                }).ToList().OrderBy(template => template.StartDate).ToList();

            user.Allergies = await _dbContext.UserAllergy
                .Where(allergy => allergy.UserId == mobileUserId && allergy.IsSelected)
                .Select(allergy => allergy.Title).ToListAsync();

            user.Dislikes = await _dbContext.UserDislikes
                .Where(dislike => dislike.UserId == mobileUserId && dislike.IsSelected)
                .Select(dislike => dislike.Title).ToListAsync();

            var plans = await _dbContext.MealPlans.Where(m => m.UserId == mobileUserId)
                .Include(m => m.PlanDays)
                .ThenInclude(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .ThenInclude(m => m.Meal)
                .OrderByDescending(m => m.Created)
                .Take(2)
                .Select(m => new
                {
                    Id = m.Id,
                    Created = m.Created,
                    Notes = m.Notes,
                    Days = m.PlanDays.Select(day => new
                    {
                        day = day.Day,
                        WaterCount = day.TakenWaterCupsCount,
                        Menus = day.PlanMeals.Select(menu => new
                        {
                            Id = menu.Id,
                            IsEaten = menu.IsEaten,
                            IsSkipped = menu.IsSkipped,
                            IsSwapped = menu.IsSwapped,
                            MealType = menu.MealType,
                            Meals = menu.Meals.Select(meal => meal.Meal.Name ?? meal.MealName)
                        })
                    }),
                    NumberOfIAmHungryClicked = m.NumberOfIAmHungryClicked
                })
                .ToListAsync();

            user.UserMealPlans = new UserMealPlans();
            if (plans.Count == 0) return user;
            user.UserMealPlans.LastPlan = plans.First();

            if (plans.Count == 1)
            {
                return user;
            }

            user.UserMealPlans.PreviousPlan = plans.Last();
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

        public async Task UserSubscribedAsync(int userId, double amountPayed)
        {
            var user = await _dbContext.Database.ExecuteSqlRawAsync(
                @$"UPDATE MUSER 
                SET TotalAmountPaid = TotalAmountPaid + {amountPayed}, 
                    SubscriptionType = 'Pro',
                    SubscriptionDate = '{DateTime.UtcNow}'
                WHERE Id = {userId}");
        }

        public async Task UserPayedAsync(int userId, double amountPayed)
        {
            var user = await _dbContext.Database.ExecuteSqlRawAsync(
                @$"UPDATE MUSER 
                SET TotalAmountPaid = TotalAmountPaid + {amountPayed}
                WHERE Id = {userId}");
        }

        public async Task UserUnSubscribedAsync(int appUserId)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                @$"UPDATE MUSER 
                SET  SubscriptionType = null
                WHERE Id = {appUserId}");
        }

        public async Task<object> GetUserPersonalDetailsAsync(int userId)
        {
            var user = await _dbContext
                .MUser
                .Where(user => user.Id == userId)
                .Select(m => new
                {
                    Files = m.Files,
                    m.ActivityLevel,
                    m.NumberOfMealsPerDay,
                    EatReason = m.EatReason,
                    m.TargetWeight,
                    m.MedicineNames,
                    m.IsRegularMeasurer,
                    m.HasBaby,
                    Goal = _dbContext.MJourneyPlan
                        .Where(j => j.Id == m.JourneyPlanId)
                        .Select(j => j.Name)
                        .FirstOrDefault()
                }).FirstOrDefaultAsync();

            return user;
        }

        public async Task UserMessageSeenAsync(int userId)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                $"UPDATE MUser SET HasNewMessage = false AND LastMessage = null WHERE Id = {userId}");
        }

        public async Task UserSentMessageAsync(int userId, string message)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                $"UPDATE MUser SET HasNewMessage = true AND LastMessage = {message} WHERE Id = {userId}");
        }
    }
}