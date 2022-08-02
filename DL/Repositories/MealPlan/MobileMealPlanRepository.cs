using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.MealPlans;
using DL.EntitiesV1.Meals;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Engines;

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
            var mealPlans = await _dbContext.MealPlans.AsNoTracking()
                .OrderByDescending(m => m.Created)
                .Include(m => m.PlanDays)
                .ThenInclude(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .ThenInclude(m => m.Meal)
                .Where(m => m.UserId == _currentUserService.UserId)
                .Take(1)
                .FirstOrDefaultAsync();

            var days = new List<object>(mealPlans.PlanDays.Count);
            foreach (var planDay in mealPlans.PlanDays)
            {
                var meals = new List<object>();
                foreach (var dayMenu in planDay.PlanMeals)
                {
                    // var PrepTime = dayMenu.Meals.Sum(m => m.Meal.PreparingTime);
                    meals.Add(new
                    {
                        Type = dayMenu.MealType,
                    });
                }

                days.Add(new
                {
                    Id = planDay.Id,
                    meals
                });
            }

            return new
            {
                PlanId = mealPlans.Id,
                days
            };
        }


        public async Task AddCupOfWaterToDayAsync(long dayId)
        {
            var day = await _dbContext.PlanDays
                .Where(d => d.Id == dayId && d.MealPlan.UserId == _currentUserService.UserId)
                .FirstOrDefaultAsync();

            day.TakenWaterCupsCount++;
            _dbContext.Update(day);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<object> GetTodayMealsAsync()
        {
            var currentDay = DateTime.UtcNow.DayOfWeek;

            var mealPlans = await _dbContext.MealPlans.AsNoTracking()
                .Include(m => m.PlanDays)
                .ThenInclude(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .ThenInclude(m => m.Meal)
                .Where(m => m.UserId == _currentUserService.UserId)
                .Take(1)
                .Select(plan => plan.PlanDays.First(day => day.Day == currentDay))
                .FirstOrDefaultAsync();


            var meals = mealPlans.PlanMeals.Select(m => new
            {
                m.Id,
                m.MealType,
                Status = m.Status,
                Meals = m.Meals.Select(meal => new
                {
                    Image = meal.Meal.CoverImage,
                    meal.Meal.Name
                })
            }).ToDictionary(m => m.MealType);
            return new
            {
                water = mealPlans.TakenWaterCupsCount,
                meals
            };
        }

        public async Task<object> GetRecommendedMealsAsync(SwapMealDto swapMealDto)
        {
            var plan = await _dbContext.PlanDays.AsNoTracking()
                .Include(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .Where(m => m.MealPlanId == swapMealDto.PlanId && m.Day != swapMealDto.Day)
                // .Where(m => m.PlanMeals.Any(menu => menu.MealType == swapMealDto.Type))
                .Select(m => m.PlanMeals.Select(menu => new
                {
                    menu.Id,
                    menu.MealType,
                    Meals = menu.Meals.Select(meal => new
                    {
                        meal.Meal.CoverImage,
                        meal.Meal.Name
                    })
                }))
                .ToListAsync();

            return plan;
        }
    }
}