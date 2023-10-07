using System;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.MealPlans;
using DL.DtosV1.Users.Admins;
using DL.DtosV1.Users.Mobiles;
using DL.EntitiesV1.Measurements;
using DL.EntitiesV1.Payments;
using DL.EntitiesV1.Subscriptions;
using DL.EntitiesV1.Users;
using DL.Extensions;
using DL.Migrations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
                query = query.Where(m => !m.Plans.Any() && m.IsSubscribed);
            }

            if (model.UserWithAboutToFinishPlan)
            {
                // 13 2 2023 9:12 
                var currentDateTime = DateTime.UtcNow.AddHours(_currentUserService.UserTimeZoneDifference);

                // EndDate = 2023-02-13 22:00:00.0000000
                query = query.Where(m =>
                    m.IsSubscribed &&
                    m.Plans.Any() &&
                    m.Plans.Any(plan =>
                        plan.EndDate.HasValue &&
                        plan.EndDate.Value
                            .AddHours(_currentUserService.UserTimeZoneDifference)
                            .AddDays(-1) <= currentDateTime &&
                        plan.EndDate.Value
                            .AddHours(_currentUserService.UserTimeZoneDifference) < currentDateTime)
                );
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
                    TotalPaidAmount = m.TotalAmountPaid,
                    IsSubscribed = m.IsSubscribed,
                    IsManuallySubscribed = m.IsManuallySubscribed
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
                    IsSubscribed = m.IsSubscribed,
                    IsBanned = m.IsBanned,
                    IsManuallySubscribed = m.IsManuallySubscribed
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

            user.UserRisks = await _dbContext.MUserRisk.Where(m => m.UserId == mobileUserId).Select(m => m.Risk.Name)
                .ToListAsync();
            user.WeightLoss = await _dbContext.GetWeightLossAsync(mobileUserId, lastWeight);
            user.CurrentWeight = lastWeight;
            user.LastUsedTemplates = _dbContext
                .MealPlans
                .Where(plan => plan.UserId == mobileUserId)
                .OrderByDescending(plan => plan.StartDate)
                .Take(8)
                .Select(plan => new UserPlanTemplateDto
                {
                    MealPlanId = plan.Id,
                    StartDate = plan.StartDate,
                    TemplateName = plan.ParentTemplate.TemplateName,
                      DoctorNotes = plan.DoctorNotes,
                }).ToList().OrderBy(template => template.StartDate).ToList();

            user.Allergies = await _dbContext.UserAllergy
                .Where(allergy => allergy.UserId == mobileUserId && allergy.IsSelected)
                .Select(allergy => allergy.Title).ToListAsync();

            user.Dislikes = await _dbContext.UserDislikes
                .Where(dislike => dislike.UserId == mobileUserId && dislike.IsSelected)
                .Select(dislike => dislike.Title).ToListAsync();

            var plans = await _dbContext.MealPlans.Where(m => m.UserId == mobileUserId && m.EndDate > DateTime.Today)
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
                    DoctorNotes = m.DoctorNotes,
                    Days = m.PlanDays.Select(day => new
                    {
                        day = day.Day,
                        WaterCount = day.TakenWaterCupsCount,
                        IsExercised = day.IsExercised,
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


        public async Task SetUserBanFlagAsync(int userId, bool ban)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                @$"UPDATE MUser SET IsBanned = {(ban ? 1 : 0)} WHERE Id = {userId}");
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

        public async Task UserSubscribedAsync(int userId, double amountPayed)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                @$"UPDATE MUSER 
                SET 
                    SubscriptionDate = COALESCE(SubscriptionDate, '{DateTime.UtcNow}'),
                    IsSubscribed = 1,
                    TotalAmountPaid = TotalAmountPaid + {amountPayed}
                WHERE Id = {userId}");
        }

        public async Task UserRenewedAsync(int userId, double amountPayed)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                @$"UPDATE MUSER 
                SET 
                    SubscriptionDate = COALESCE(SubscriptionDate, '{DateTime.UtcNow}'),
                    IsSubscribed = 1,
                    TotalAmountPaid = TotalAmountPaid + {amountPayed}
                WHERE Id = {userId}");
        }

        public async Task UserUnSubscribedAsync(int appUserId)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                @$"UPDATE MUSER 
                SET  SubscriptionDate = NULL,
                IsSubscribed = 0
                WHERE Id = {appUserId}");
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

        public async Task MakePremiumAsync(ManualAppSubscribeRequest manualAppSubscribeRequest)
        {
            var user = await _dbContext.MUser.AsQueryable().Where(u => u.Id == manualAppSubscribeRequest.UserId)
                .FirstOrDefaultAsync();

            var currentData = DateTime.UtcNow;
            user.IsSubscribed = true;
            user.IsManuallySubscribed = true;
            user.SubscriptionDate = currentData;
            user.TotalAmountPaid += manualAppSubscribeRequest.AmountPaid;

            var payment = new PaymentHistoryEntity()
            {
                Created = currentData,
                UserId = manualAppSubscribeRequest.UserId,
                Currency = "EGP",
                Price = manualAppSubscribeRequest.AmountPaid,
                Type = "Manual",
                IsHandled = true,
                Event = JsonConvert.SerializeObject(manualAppSubscribeRequest)
            };

            await _dbContext.AddAsync(payment);
            await _dbContext.SubscriptionInfos.AddAsync(new SubscriptionInfoEntity()
            {
                UserId = manualAppSubscribeRequest.UserId,
                StartDate = currentData,
                EndDate = manualAppSubscribeRequest.EndDate,
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemovePremiumAsync(ManualAppSubscribeRequest manualAppSubscribeRequest)
        {
            var sqlScript = @$"
                    UPDATE MUser Set IsSubscribed = 0, 
                                     SubscriptionDate = null, 
                                     IsManuallySubscribed = 0 
                                 WHERE Id = {manualAppSubscribeRequest.UserId});
                    
                    DELETE FROM SubscriptionInfos WHERE UserId = {manualAppSubscribeRequest.UserId});";

            await _dbContext.Database.ExecuteSqlRawAsync(sqlScript);
        }
    }
}