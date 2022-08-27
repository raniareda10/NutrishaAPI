using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Meals;
using DL.EntitiesV1.Meals;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories
{
    public class MobileMealsService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDBContext _dbContext;

        public MobileMealsService(ICurrentUserService currentUserService, AppDBContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<object> GetByIdAsync(long id)
        {
            var meal = await _dbContext.Meals
                .Include(m => m.Ingredients)
                .AsNoTracking()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            var isFavorite =
                await _dbContext.UserFavoriteMeals.AnyAsync(m =>
                    m.MealId == id && m.UserId == _currentUserService.UserId);

            return new
            {
                Id = meal.Id,
                Created = meal.Created,
                Name = meal.Name,
                MealType = meal.MealType,
                CockingTime = meal.CockingTime,
                PreparingTime = meal.PreparingTime,
                CoverImage = meal.CoverImage,
                MealSteps = meal.MealSteps,
                Allergies = meal.Allergies,
                Ingredients = meal.Ingredients.Select(m => m.IngredientName),
                isFavorite = isFavorite
            };
        }

        public async Task MarkAsFavoriteAsync(long mealId)
        {
            var favorite = await _dbContext.UserFavoriteMeals.FirstOrDefaultAsync(m =>
                m.MealId == mealId && m.UserId == _currentUserService.UserId);

            if (favorite == null)
            {
                await _dbContext.UserFavoriteMeals.AddAsync(new UserFavoriteMealEntity()
                {
                    UserId = _currentUserService.UserId,
                    MealId = mealId,
                    Created = DateTime.UtcNow
                });
                await _dbContext.SaveChangesAsync();
                return;
            }

            _dbContext.UserFavoriteMeals.Remove(favorite);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MealLookupDto>> GetRecommendedMealsAsync()
        {
            return await _dbContext.Meals
                // .Include(meal => meal.Ingredients)
                .Where(m => m.MealType == MealType.Recommended)
                .OrderBy(m => Guid.NewGuid())
                .Skip(0)
                .Take(5)
                .Select(m => new MealLookupDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    CoverImage = m.CoverImage
                })
                .ToListAsync();
        }
    }
}