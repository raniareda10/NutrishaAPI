using System;
using System.Collections.Generic;
using System.Linq;
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
                .Include(m => m.Ingredients)
                .AsNoTracking()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
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