using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.MealPlans;
using DL.EntitiesV1.Dairies;
using DL.EntitiesV1.Meals;
using DL.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Dashboard
{
    public class CalenderService
    {
        private const int NumberOfDaysInWeek = 7;
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDBContext _appDbContext;

        public CalenderService(ICurrentUserService currentUserService, AppDBContext appDbContext)
        {
            _currentUserService = currentUserService;
            _appDbContext = appDbContext;
        }

        #region GetBusyDays

        public async Task<IEnumerable<int>> GetBusyDaysAsync(
            DateTime startDay,
            DateTime endDay,
            bool isSubscribed)
        {
            endDay = endDay.AddDays(1);

            return isSubscribed
                ? await GetSubscribedDaysAsync(startDay, endDay)
                : await GetUnSubscribedDaysAsync(startDay, endDay);
        }

        private async Task<IEnumerable<int>> GetSubscribedDaysAsync(DateTime startDay, DateTime endDay)
        {
            var days = new List<int>();

            var weekAgo = startDay.AddDays(-NumberOfDaysInWeek);
            endDay = endDay.AddDays(1);
            var planInTheMonth = await _appDbContext.MealPlans
                .AsNoTracking()
                .Where(p => p.UserId == _currentUserService.UserId)
                .Where(p => p.StartDate >= weekAgo)
                .Where(p => p.StartDate < endDay)
                .Select(m => m.StartDate)
                .ToListAsync();

            foreach (var startDate in planInTheMonth.Select(planStartDate =>
                         planStartDate.Value.AddHours(_currentUserService.UserTimeZoneDifference)))
            {
                for (var i = 1; i <= NumberOfDaysInWeek; i++)
                {
                    var mealPlanDayStartDate = startDate.AddDays(i);

                    if (mealPlanDayStartDate >= startDay && mealPlanDayStartDate < endDay)
                        days.Add(startDate.AddDays(i).Day);
                }
            }

            return days.Distinct();
        }

        private async Task<IEnumerable<int>> GetUnSubscribedDaysAsync(DateTime startDay, DateTime endDay)
        {
            var startDays = await _appDbContext.Dairies
                .AsNoTracking()
                .Where(d => d.UserId == _currentUserService.UserId)
                .Where(GetDairyFilter(startDay, endDay))
                .Select(m => m.Created)
                .ToListAsync();

            return startDays
                .Select(m => m.AddHours(_currentUserService.UserTimeZoneDifference).Day)
                .Distinct();
        }

        #endregion

        #region Get Day Details

        public async Task<object> GetDayDetailsAsync(DateTime day, bool isSubscribed)
        {
            return isSubscribed
                ? await GetSubscribedDayDetailsAsync(day)
                : await GetUnSubscribedDayDetailsAsync(day);
        }

        private async Task<object> GetSubscribedDayDetailsAsync(DateTime day)
        {
            var dayOfWeek = day.AddHours(_currentUserService.UserTimeZoneDifference).DayOfWeek;
            var start = day.AddDays(-NumberOfDaysInWeek);
            var endDay = day.AddDays(1);
            var planInTheMonth = await _appDbContext.PlanDays
                .AsNoTracking()
                .Include(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .ThenInclude(m => m.Meal)
                .Where(p => p.MealPlan.UserId == _currentUserService.UserId)
                .Where(d => d.MealPlan.StartDate >= start && d.MealPlan.StartDate < endDay)
                .Where(p => p.Day == dayOfWeek)
                .Select(m => m)
                .OrderByDescending(m => m)
                .FirstOrDefaultAsync();

            if (planInTheMonth is null) return null;

            return new
            {
                WaterTaken = CalculateWaterLitersFromNumberOfCups(planInTheMonth.TakenWaterCupsCount),
                IsExercised = planInTheMonth.IsExercised,
                Meals = planInTheMonth.PlanMeals
                    .Where(m => !m.IsSkipped&& m.IsEaten)
                    .Select(m => new
                {
                    MealType = m.MealType,
                    Meals = m.Meals.Select(meal => new
                    {
                        MenuId = m.Id,
                        MealId = meal.Meal?.Id,
                        MealName = meal.Meal?.Name ?? meal.MealName
                    })
                })
            };
        }

        private async Task<object> GetUnSubscribedDayDetailsAsync(DateTime day)
        {
            var endDay = day.AddDays(1);
            var startDays = await _appDbContext.Dairies
                .AsNoTracking()
                .Where(d => d.UserId == _currentUserService.UserId )
                .Where(GetDairyFilter(day, endDay))
                .Select(d => new
                {
                    d.Details,
                    d.Name,
                    d.Type
                })
                .ToListAsync();

            var data = startDays.GroupBy(m => m.Type)
                .ToDictionary(m => m.Key, d => d.Select(m => m).Select(m => new
                {
                    Id = (long?)null,
                    m.Name,
                    m.Details,
                }));

            var waterCup = data.TryGetValue(MealType.Water, out var water);

            var waterCupCount = waterCup ? water.Count() : 0;
            return new
            {
                WaterTaken = waterCupCount,
                Meals = data.Select(m => new
                {
                    MealType = m.Key,
                    Meals = m.Value.Select(d => new
                    {
                        MenuId = (long?)null,
                        MealId = (long?)null,
                        MealName = d.Name
                    }).ToList()
                })
            };
        }

        #endregion

        public async Task<object> GetDashboardDetailsAsync(DateTime day, bool isSubscribed)
        {
            var waterTaken = isSubscribed
                ? await GetSubscribedDashboardDetailsAsync(day)
                : await GetUnSubscribedDashboardDetailsAsync(day);
            var weightLoss = await _appDbContext.GetWeightLossAsync(_currentUserService.UserId);
            return new
            {
                WaterTaken = CalculateWaterLitersFromNumberOfCups(waterTaken),
                Points = weightLoss
            };
        }

        private async Task<int> GetSubscribedDashboardDetailsAsync(DateTime day)
        {
            var dayOfWeek = day.AddHours(_currentUserService.UserTimeZoneDifference).DayOfWeek;
            var currentDate = day.AddHours(_currentUserService.UserTimeZoneDifference);
            var start = day.AddDays(-NumberOfDaysInWeek);
            var endDay = day.AddDays(1);
            var waterTakenInDay = await _appDbContext.PlanDays
                .AsNoTracking()
                .Include(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .ThenInclude(m => m.Meal)
                .Where(p => p.MealPlan.UserId == _currentUserService.UserId)
                .Where(d => d.MealPlan.StartDate.Value.AddHours(_currentUserService.UserTimeZoneDifference) <= currentDate && d.MealPlan.EndDate.Value.AddHours(_currentUserService.UserTimeZoneDifference) >= currentDate)
                .Where(p => p.Day == dayOfWeek)
                .Select(m => m.TakenWaterCupsCount)
                .FirstOrDefaultAsync();

            return waterTakenInDay;
        }

        private async Task<int> GetUnSubscribedDashboardDetailsAsync(DateTime day)
        {
            return await _appDbContext.Dairies
                .AsNoTracking()
                .Where(d => d.UserId == _currentUserService.UserId)
                .Where(GetDairyFilter(day, day.AddDays(1)))
                .Where(d => d.Type == MealType.Water)
                .CountAsync();
        }


        private Expression<Func<MealPlanEntity, bool>> GetMealDayFilter(DateTime start, DateTime end)
        {
            // start = 2022-10-25 22:00:00Z
            // end = 2022-10-30 22:00:00Z
            // 18 22 -> 19
            var week = start.AddDays(-NumberOfDaysInWeek);
            end = end.AddDays(1);
            return d => d.StartDate >= week && d.StartDate < end;
        }

        private Expression<Func<DairyEntity, bool>> GetDairyFilter(DateTime start, DateTime end)
        {
            return d => d.Created >= start && d.Created < end;
        }

        private float CalculateWaterLitersFromNumberOfCups(int numberOfCups)
        {
            return (float)numberOfCups / 4;
        }
    }
}