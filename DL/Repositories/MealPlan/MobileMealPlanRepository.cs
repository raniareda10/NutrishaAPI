using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.MealPlans;
using DL.EntitiesV1.Meals;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.MealPlan
{
    public class MobileMealPlanRepository
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDBContext _dbContext;

        public MobileMealPlanRepository(ICurrentUserService currentUserService, AppDBContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<object> GetCurrentPlanAsync()
        {
            var currentDate = DateTime
                .UtcNow
                .AddHours(_currentUserService.UserTimeZoneDifference);

            var mealPlans = await _dbContext.MealPlans
                .AsNoTracking()
                .Where(m =>
                    m.StartDate.Value.AddHours(_currentUserService.UserTimeZoneDifference) <=
                    currentDate)
                .Where(m =>
                    m.EndDate.Value.AddHours(_currentUserService.UserTimeZoneDifference) >=
                    currentDate)
                .OrderByDescending(m => m.Created)
                .Include(m => m.PlanDays)
                .ThenInclude(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .ThenInclude(m => m.Meal)
                .Where(m => m.UserId == _currentUserService.UserId)
                .Take(1)
                .FirstOrDefaultAsync();

            if (mealPlans == null) return null;
            return new
            {
                PlanId = mealPlans.Id,
                Days = mealPlans.PlanDays
                    .OrderByDescending(m => m.Day)
                    .Select(day => new PlanDayDto()
                    {
                        Id = day.Id,
                        DayOfWeek = day.Day,
                        Menus = day.PlanMeals.OrderBy(m => m.MealType)
                            .Select(m => new
                            {
                                Id = m.Id,
                                MenuType = m.MealType,
                                Meals = m.Meals.Select(meal => new
                                {
                                    Id = meal.MealId,
                                    Image = meal.Meal?.CoverImage,
                                    Name = meal?.Meal?.Name ?? meal.MealName,
                                })
                            })
                    })
            };
        }

        public async Task<object> GetTodayMealsAsync()
        {
            var currentDate = DateTime
                .UtcNow
                .AddHours(_currentUserService.UserTimeZoneDifference);

            var currentDay = currentDate.DayOfWeek;
            var planDayEntity = await _dbContext.PlanDays
                .AsNoTracking()
                .Where(m => m.MealPlan.UserId == _currentUserService.UserId)
                .Where(m =>
                    m.MealPlan.StartDate.Value.AddHours(_currentUserService.UserTimeZoneDifference) <=
                    currentDate)
                .Where(m =>
                    m.MealPlan.EndDate.Value.AddHours(_currentUserService.UserTimeZoneDifference) >=
                    currentDate)
                .Where(p => p.Day == currentDay)
                .OrderByDescending(m => m.Created)
                .Take(1)
                .Include(m => m.MealPlan)
                .Include(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .ThenInclude(m => m.Meal)
                .FirstOrDefaultAsync();

            if (planDayEntity == null)
            {
                return null;
            }

            var meals = planDayEntity.PlanMeals
                .Select(m => new
                {
                    m.Id,
                    m.MealType,
                    IsSwapped = m.IsSwapped,
                    IsSkipped = m.IsSkipped,
                    IsEaten = m.IsEaten,
                    Meals = m.Meals.Select(meal => new
                    {
                        Id = meal.Id,
                        IsEaten = meal.IsEaten,
                        Meal = new
                        {
                            Id = meal.MealId,
                            Image = meal.Meal?.CoverImage,
                            Name = meal?.Meal?.Name ?? meal.MealName,
                        }
                    })
                })
                .ToDictionary(m => m.MealType);

            return new
            {
                PlanId = planDayEntity.MealPlanId,
                DayId = planDayEntity.Id,
                DayOfWeek = currentDay,
                TakenWaterCupsCount = planDayEntity.TakenWaterCupsCount,
                Meals = meals
            };
        }

        public async Task<object> GetRecommendedMealsForSwapAsync(SwapMealDto swapMealDto)
        {
            return await _dbContext.PlanDayMenus
                .AsNoTracking()
                .Where(m => m.PlanDay.MealPlan.UserId == _currentUserService.UserId)
                .Where(m => m.PlanDay.MealPlan.Id == swapMealDto.PlanId)
                .Where(m => m.MealType == swapMealDto.Type)
                .Where(m => m.PlanDay.Day != swapMealDto.Day)
                .Select(m => new
                {
                    Id = m.Id,
                    Day = m.PlanDay.Day,
                    Meals = m.Meals.Select(meal => new
                    {
                        meal.Meal.CoverImage,
                        meal.Meal.Name,
                    })
                }).ToListAsync();
        }


        public async Task AddCupOfWaterToDayAsync(long dayId, byte cupsCount)
        {
            if (cupsCount < 1)
            {
                cupsCount = 1;
            }

            var day = await _dbContext.PlanDays
                .Where(d => d.Id == dayId && d.MealPlan.UserId == _currentUserService.UserId)
                .FirstOrDefaultAsync();

            day.TakenWaterCupsCount += cupsCount;
            _dbContext.Update(day);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UserCLickedIAmHungryAsync(long planId)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                $"UPDATE MealPlans SET NumberOfIAmHungryClicked = NumberOfIAmHungryClicked + 1 " +
                $"WHERE Id = {planId} AND userId = {_currentUserService.UserId}");
        }

        public async Task<BaseServiceResult> SwapMenuAsync(long oldMenuId, long swapWithMenuId)
        {
            var result = new BaseServiceResult();
            if (oldMenuId == swapWithMenuId)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidParameters);
                return result;
            }

            var menus = await _dbContext.PlanDayMenus
                .Include(m => m.Meals)
                .Where(m =>
                    (m.Id == oldMenuId || m.Id == swapWithMenuId) &&
                    m.PlanDay.MealPlan.UserId == _currentUserService.UserId)
                .ToListAsync();

            var oldMenu = menus.FirstOrDefault(m => m.Id == oldMenuId);
            var newMenu = menus.FirstOrDefault(m => m.Id == swapWithMenuId);

            if (oldMenu == null || newMenu == null)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidParameters);
                return result;
            }

            if (oldMenu.PlanDayId == newMenu.PlanDayId)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidParameters);
                return result;
            }

            _dbContext.PlanDayMenus.Remove(oldMenu);

            await _dbContext.PlanDayMenus.AddAsync(new PlanDayMenuEntity()
            {
                IsSwapped = true,
                Meals = newMenu.Meals.Select(meals => new PlanDayMenuMealEntity()
                {
                    MealId = meals.MealId
                }).ToList(),
                MealType = oldMenu.MealType,
                PlanDayId = oldMenu.PlanDayId,
                Created = oldMenu.Created
            });

            await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<BaseServiceResult> SkipMenuAsync(long menuId)
        {
            var result = new BaseServiceResult();
            var menu = await GetMenuAsync(menuId);

            if (menu == null)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidParameters);
                return result;
            }


            menu.IsSkipped = !menu.IsSkipped;
            await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<BaseServiceResult> MarkMenuAsEatenAsync(long menuId)
        {
            var result = new BaseServiceResult();
            var menu = await GetMenuAsync(menuId);

            if (menu == null)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidParameters);
                return result;
            }

            menu.IsEaten = !menu.IsEaten;
            await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<BaseServiceResult> MarkMenuMealAsEatenAsync(long mealId)
        {
            var menu = await _dbContext.PlanDayMenuMeals.Where(m =>
                    m.Id == mealId &&
                    m.PlanDayMenu.PlanDay.MealPlan.UserId == _currentUserService.UserId)
                .FirstOrDefaultAsync();

            var result = new BaseServiceResult();

            if (menu == null)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidParameters);
                return result;
            }

            menu.IsEaten = !menu.IsEaten;
            await _dbContext.SaveChangesAsync();
            return result;
        }

        private async Task<PlanDayMenuEntity> GetMenuAsync(long menuId)
        {
            var menu = await _dbContext.PlanDayMenus.Where(m =>
                    m.Id == menuId && m.PlanDay.MealPlan.UserId == _currentUserService.UserId)
                .FirstOrDefaultAsync();

            return menu;
        }

        public async Task<BaseServiceResult> AddExtraBiteMealAsync(AddExtraBitesDto dto)
        {
            var result = new BaseServiceResult();
            var extraBitesMenu = await _dbContext.PlanDays
                .AsNoTracking()
                .Where(day => day.Id == dto.DayId && day.MealPlan.UserId == _currentUserService.UserId)
                .Select(m => new
                {
                    Menu = m.PlanMeals
                        .Where(entity => entity.MealType == MealType.ExtraBites)
                        .Select(planDayMenuEntity => new
                        {
                            planDayMenuEntity.Id,
                            NumberOfMeals = planDayMenuEntity.Meals.Count
                        }).FirstOrDefault()
                })
                .FirstOrDefaultAsync();
            
            if (extraBitesMenu == null)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidParameters);
                return result;
            }

            if (extraBitesMenu.Menu?.NumberOfMeals >= 3)
            {
                result.Errors.Add("You can add 3 bites only");
                return result;
            }
            
            var currentDate = DateTime.UtcNow;
            var meal = dto.MealId.HasValue
                ? new PlanDayMenuMealEntity()
                {
                    MealId = dto.MealId,
                    Created = currentDate
                }
                : new PlanDayMenuMealEntity()
                {
                    MealName = dto.MealName,
                    Created = currentDate
                };

            if (extraBitesMenu.Menu?.Id == null)
            {
                var menu = new PlanDayMenuEntity()
                {
                    PlanDayId = dto.DayId,
                    Created = currentDate,
                    MealType = MealType.ExtraBites,
                    Meals = new List<PlanDayMenuMealEntity>()
                };
                menu.Meals.Add(meal);
                await _dbContext.AddAsync(menu);
            }
            else
            {
                meal.PlanDayMenuId = extraBitesMenu.Menu.Id;
                await _dbContext.AddAsync(meal);
            }

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}