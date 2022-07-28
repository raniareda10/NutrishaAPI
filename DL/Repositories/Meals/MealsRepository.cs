using System;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Meals;
using DL.EntitiesV1.Meals;
using DL.Extensions;
using DL.StorageServices;
using Microsoft.EntityFrameworkCore;


namespace DL.Repositories.Meals
{
    public class MealsRepository
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDBContext _dbContext;
        private readonly IStorageService _storageService;

        public MealsRepository(ICurrentUserService currentUserService, AppDBContext dbContext,
            IStorageService storageService)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _storageService = storageService;
        }


        public async Task<object> GetMealsLookupAsync()
        {
            var allMeals = await _dbContext.Meals.OrderByDescending(m => m.Name)
                .ToListAsync();

            return allMeals.GroupBy(m => m.MealType)
                .ToDictionary(m => (int)m.Key, entities => entities.Select(m => new
                {
                    m.Id,
                    m.Name
                }));
        }

        public async Task<long> PostAsync(PostMealDto model)
        {
            var coverImage = await _storageService.UploadFileAsync(model.CoverImage, "meals");
            var meal = new MealEntity()
            {
                Name = model.Name,
                Allergies = model.Allergies,
                Ingredients = model.Ingredients,
                Created = DateTime.UtcNow,
                CockingTime = model.CockingTime,
                PreparingTime = model.PreparingTime,
                MealSteps = model.MealSteps,
                MealType = model.MealType,
                CoverImage = coverImage
            };

            await _dbContext.AddAsync(meal);
            await _dbContext.SaveChangesAsync();

            return meal.Id;
        }

        public async Task<PagedResult<MealListModelDto>> GetMealsAsync(GetMealsPagedListQuery model)
        {
            var query = _dbContext.Meals
                .AsNoTracking()
                .OrderByDescending(m => m.Created)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.SearchWord))
            {
                query = query
                    .Where(m => m.Name.Contains(model.SearchWord));
            }

            if (model.MealType.HasValue)
            {
                query = query
                    .Where(m => m.MealType == model.MealType);
            }

            return await query.Select(m => new MealListModelDto
            {
                Id = m.Id,
                Name = m.Name,
                MealType = m.MealType,
                CookingTime = m.CockingTime,
                PreparingTime = m.PreparingTime
            }).ToPagedListAsync(model);
        }

        public async Task<MealEntity> GetByIdAsync(long id)
        {
            return await _dbContext.Meals
                .AsNoTracking()
                .Where(m => m.Id == id)
                // .Select(m => new MealDetailsDto
                // {
                // })
                .FirstOrDefaultAsync();
        }

        public async Task<long> PostMealPlanAsync(PostMealPlanDto plans)
        {
            var currentDate = DateTime.UtcNow;
            var plan = new MealPlan
            {
                UserId = plans.UserId,
                Notes = plans.Notes,
                Created = currentDate,
                PlanDays = plans.Meals.Select(m => new PlanDayEntity()
                {
                    Created = currentDate,
                    Day = m.Day,
                    PlanMeals = m.Menus.Select(menu => new PlanDayMenuEntity
                    {
                        Created = currentDate,
                        MealType = menu.Type,
                        Meals = menu.MealIds.Select(mealId => new PlanDayMenuMealEntity()
                        {
                            Created = currentDate,
                            MealId = mealId
                        }).ToList()
                    }).ToList(),
                }).ToList(),
            };
            

            await _dbContext.AddAsync(plan);
            await _dbContext.SaveChangesAsync();

            return plan.Id;
        }

        // public async Task<dynamic> GetCurrentPlanAsync(int? userId)
        // {
        //     var plan = await _dbContext.MealPlans
        //         .AsNoTracking()
        //         .Include(p => p.Days)
        //         .ThenInclude(m => m.Meal)
        //         .Where(p => p.UserId == (userId ?? _currentUserService.UserId))
        //         .OrderByDescending(p => p.Created)
        //         .FirstOrDefaultAsync();
        //
        //     var result = plan.Meals.GroupBy(m => m.Day)
        //         .Select(p => new
        //         {
        //             Day = p.Key,
        //             Meals = p.Select(m => new
        //             {
        //                 m.Meal.Name,
        //                 m.Meal.MealType,
        //             })
        //         });
        //
        //     return result;
        // }

        // public async Task<dynamic> GetUserPlansAsync(int? userId)
        // {
        // var plans = await _dbContext.MealPlans
        //     .AsNoTracking()
        //     .Include(m => m.Days)
        //     .ThenInclude(m => m.PlanMeals)
        //     // .ThenInclude(m => m.Meal)
        //     // .OrderByDescending(m => m.Created)
        //     // .Take(2)
        //     .ToListAsync();
        //
        // if (plans.Count == 0)
        // {
        //     return null;
        // }
        // }
    }
}