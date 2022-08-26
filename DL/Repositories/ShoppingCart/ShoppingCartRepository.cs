using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Meals;
using DL.EntitiesV1.Meals;
using DL.EntitiesV1.ShoppingCartEntity;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.ShoppingCart
{
    public class ShoppingCartRepository
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDBContext _dbContext;

        public ShoppingCartRepository(ICurrentUserService currentUserService, AppDBContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<object> GetUserCartAsync()
        {
            var cart = await InternalGetOrCreateUserCartAsync(includeMeals: true);
            return new
            {
                Items = cart.Items?.Select(m => new
                {
                    m.ItemName,
                    Meals = m.Meals?.Select(meal => meal.Meal.Name)
                })
            };
        }

        public async Task AddMealToShoppingCartAsync(long mealId)
        {
            var cart = await InternalGetOrCreateUserCartAsync(false);

            var ingredients = await _dbContext.Meals.Where(m => m.Id == mealId)
                .Select(m => m.Ingredients)
                .FirstOrDefaultAsync();

            if (ingredients == null || ingredients.Count == 0) return;

            foreach (var mealIngredientEntity in ingredients)
            {
                AddToCart(cart, mealIngredientEntity.IngredientName, mealId);
            }

            await _dbContext.SaveChangesAsync();
        }


        public async Task AddToShoppingCartAsync(MealIngredientDto mealIngredientEntity)
        {
            var cart = await InternalGetOrCreateUserCartAsync(false);
            AddToCart(cart, mealIngredientEntity.IngredientName, mealIngredientEntity.MealId);
            await _dbContext.SaveChangesAsync();
        }


        public async Task MarkAsBoughtAsync(long itemId)
        {
            var item = await GetItemByIdAsync(itemId);

            if (item == null)
            {
                return;
            }

            item.IsBought = true;
            _dbContext.ShoppingCartItems.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(long itemId)
        {
            var cart = await GetItemByIdAsync(itemId);

            if (cart == null)
            {
                return;
            }

            _dbContext.ShoppingCartItems.Remove(cart);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<ShoppingCartItemEntity> GetItemByIdAsync(long itemId)
        {
            return await _dbContext.ShoppingCartItems
                .Include(item => item.ShoppingCart)
                .Where(item => item.Id == itemId && item.ShoppingCart.UserId == _currentUserService.UserId)
                .FirstOrDefaultAsync();
        }

        private void AddToCart(ShoppingCartEntity cart,
            string ingredientName,
            long mealId
            // float quantity,
            // IngredientUnitType unitType
        )
        {
            var cartItem = cart.Items.FirstOrDefault(item =>
                !item.IsBought &&
                string.Equals(item.ItemName, ingredientName,
                    StringComparison.CurrentCultureIgnoreCase));

            if (cartItem == null)
            {
                cartItem = new ShoppingCartItemEntity()
                {
                    Created = DateTime.UtcNow,
                    ItemName = ingredientName,
                    Meals = new Collection<ShoppingCartItemMealEntity>()
                    {
                        new ShoppingCartItemMealEntity()
                        {
                            Created = DateTime.UtcNow,
                            MealId = mealId
                        }
                    }
                };
                cart.Items.Add(cartItem);
                return;
            }

            if (cartItem.Meals == null || cartItem.Meals.Count == 0)
            {
                cartItem.Meals = new Collection<ShoppingCartItemMealEntity>()
                {
                    new ShoppingCartItemMealEntity()
                    {
                        Created = DateTime.UtcNow,
                        MealId = mealId
                    }
                };

                return;
            }

            var isMealAlreadyAdded = cartItem.Meals.Any(m => m.MealId == mealId);

            if (isMealAlreadyAdded) return;

            cartItem.Meals.Add(new ShoppingCartItemMealEntity()
            {
                Created = DateTime.UtcNow,
                MealId = mealId
            });
        }

        private async Task<ShoppingCartEntity> InternalGetOrCreateUserCartAsync(bool readOnly = true,
            bool includeMeals = false)
        {
            var cartQuery = _dbContext.ShoppingCarts.AsQueryable();
            if (readOnly) cartQuery = cartQuery.AsNoTracking();

            if (includeMeals)
            {
                cartQuery = cartQuery.Include(m => m.Items)
                    .ThenInclude(m => m.Meals)
                    .ThenInclude(m => m.Meal);
            }
            else
            {
                cartQuery = cartQuery.Include(m => m.Items)
                    .ThenInclude(m => m.Meals);
            }

            var cart = await cartQuery.FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId);

            if (cart != null)
            {
                cart.Items = cart.Items.OrderByDescending(m => !m.IsBought).ToList();
                return cart;
            }

            cart = new ShoppingCartEntity()
            {
                Created = DateTime.UtcNow,
                UserId = _currentUserService.UserId
            };

            await _dbContext.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }
    }
}