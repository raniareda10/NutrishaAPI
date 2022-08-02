using System;
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

        public async Task AddMealToShoppingCartAsync(long mealId)
        {
            var cart = await GetUserCartAsync();

            if (cart == null)
            {
                cart = new ShoppingCartEntity()
                {
                    Created = DateTime.UtcNow,
                    UserId = _currentUserService.UserId
                };

                await _dbContext.AddAsync(cart);
            }
            else
            {
                _dbContext.Update(cart);
            }

            var ingredients = await _dbContext.Meals.Where(m => m.Id == mealId)
                .Select(m => m.Ingredients)
                .FirstOrDefaultAsync();

            foreach (var mealIngredientEntity in ingredients)
            {
                AddToCart(cart, mealIngredientEntity.IngredientName, mealIngredientEntity.Quantity,
                    mealIngredientEntity.UnitType);
            }

            await _dbContext.SaveChangesAsync();
        }


        public async Task AddToShoppingCartAsync(MealIngredientDto mealIngredientEntity)
        {
            var cart = await GetUserCartAsync();

            if (cart == null)
            {
                cart = new ShoppingCartEntity()
                {
                    Created = DateTime.UtcNow,
                    UserId = _currentUserService.UserId
                };

                await _dbContext.AddAsync(cart);
            }
            else
            {
                _dbContext.Update(cart);
            }

            AddToCart(cart, mealIngredientEntity.IngredientName, mealIngredientEntity.Quantity,
                mealIngredientEntity.UnitType);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ShoppingCartEntity> GetUserCartAsync()
        {
            var cart = await _dbContext.ShoppingCarts
                .AsNoTracking()
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId);

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

        private void AddToCart(ShoppingCartEntity cart, string ingredientName, float quantity,
            IngredientUnitType unitType)
        {
            var cartItem = cart.Items.FirstOrDefault(item =>
                !item.IsBought &&
                string.Equals(item.ItemName, ingredientName,
                    StringComparison.CurrentCultureIgnoreCase));

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new ShoppingCartItemEntity()
                {
                    Created = DateTime.UtcNow,
                    Quantity = quantity,
                    UnitType = unitType,
                    ItemName = ingredientName,
                });
            }
        }
    }
}