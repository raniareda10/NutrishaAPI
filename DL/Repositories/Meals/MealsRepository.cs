using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Common;
using DL.DtosV1.Meals;
using DL.EntitiesV1;
using DL.EntitiesV1.Meals;
using DL.Extensions;
using DL.ResultModels;
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
                Ingredients = model.Ingredients.Select(i => new MealIngredientEntity()
                {
                    Created = DateTime.UtcNow,
                    UnitType = i.UnitType,
                    IngredientName = i.IngredientName,
                    Quantity = i.Quantity
                }).ToList(),
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


        public async Task<BaseServiceResult> EditAsync(EditMealDto model)
        {
            var result = new BaseServiceResult();
            var meal = await _dbContext.Meals.Include(m => m.Ingredients)
                .FirstOrDefaultAsync(meal => meal.Id == model.Id);

            if (meal == null)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidId);
                return result;
            }

            ;

            if (model.CoverImage != null)
            {
                meal.CoverImage = await _storageService.UploadFileAsync(model.CoverImage, "meals");
            }

            _dbContext.RemoveRange(meal.Ingredients);

            meal.Name = model.Name;
            meal.Allergies = model.Allergies;
            meal.Ingredients = model.Ingredients.Select(i => new MealIngredientEntity()
            {
                Created = DateTime.UtcNow,
                UnitType = i.UnitType,
                IngredientName = i.IngredientName,
                Quantity = i.Quantity
            }).ToList();

            meal.CockingTime = model.CockingTime;
            meal.PreparingTime = model.PreparingTime;
            meal.MealSteps = model.MealSteps;
            meal.MealType = model.MealType;

            _dbContext.Update(meal);
            await _dbContext.SaveChangesAsync();

            return result;
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

        public async Task<BaseServiceResult> DeleteAsync(long id)
        {
            var result = new BaseServiceResult();

            var isUsedInMealPlan = _dbContext.PlanDayMenuMeals.Any(m => m.MealId == id);
            if (isUsedInMealPlan)
            {
                result.Errors.Add("Cant delete This meal because it already used in meal template.");
                return result;
            }

            await _dbContext.Database.ExecuteSqlRawAsync($"Delete FROM Meals where Id = {id}");
            return result;
        }

        public async Task<object> GetByIdAsync(long id)
        {
            var meal = await _dbContext.Meals
                .Include(m => m.Ingredients)
                .AsNoTracking()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (meal == null) return null;

            return new
            {
                Id = meal.Id,
                Name = meal.Name,
                MealType = meal.MealType,
                CockingTime = meal.CockingTime,
                PreparingTime = meal.PreparingTime,
                CoverImage = meal.CoverImage,
                MealSteps = meal.MealSteps,
                Allergies = meal.Allergies,
                Ingredients = meal.Ingredients,
                CanEdit = !_dbContext.PlanDayMenuMeals.Any(m => m.MealId == id)
            };
        }

        public async Task<IEnumerable<string>> GetIngredientLookupAsync(string search)
        {
            var ingredient = _dbContext.IngredientLookups.Select(m => m.Name);

            if (!string.IsNullOrWhiteSpace(search))
            {
                ingredient = ingredient.Where(m => m.Contains(search));
            }

            return await ingredient.ToListAsync();
        }

        public async Task<BaseServiceResult> PostIngredientAsync(PostLookupItem postLookupItem)
        {
            var serviceResult = new BaseServiceResult();
            try
            {
                await _dbContext.AddAsync(new IngredientLookupEntity()
                {
                    Created = DateTime.UtcNow,
                    Name = postLookupItem.Name
                });

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                serviceResult.Errors.Add("This Ingredient Already Exists.");
            }


            return serviceResult;
        }
    }
}