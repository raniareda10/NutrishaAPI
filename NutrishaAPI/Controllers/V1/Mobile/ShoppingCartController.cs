using System.Threading.Tasks;
using DL.DtosV1.Meals;
using DL.DtosV1.ShoppingCart;
using DL.Repositories.ShoppingCart;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    public class ShoppingCartController : BaseMobileController
    {
        private readonly ShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(ShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("GetCurrentCart")]
        public async Task<IActionResult> GetCurrentCartAsync()
        {
            return ItemResult(await _shoppingCartRepository.GetUserCartAsync());
        }

        [HttpPost("AddMealIngredientsToCart")]
        public async Task<IActionResult> AddMealIngredientsToCartAsync(
            [FromBody] AddMealIngredientsToCartDto addMealIngredientsToCartDto)
        {
            if (addMealIngredientsToCartDto.MealId == 0)
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            
            await _shoppingCartRepository.AddMealToShoppingCartAsync(addMealIngredientsToCartDto.MealId);
            return EmptyResult();
        }

        [HttpPost("AddIngredientToCart")]
        public async Task<IActionResult> AddIngredientToCartAsync([FromBody] MealIngredientDto mealIngredientDto)
        {
            if (mealIngredientDto.MealId == 0 || string.IsNullOrWhiteSpace(mealIngredientDto.IngredientName))
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            
            await _shoppingCartRepository.AddToShoppingCartAsync(mealIngredientDto);
            return EmptyResult();
        }


        [HttpPut("MarkAsBought")]
        public async Task<IActionResult> MarkAsBoughtAsync([FromQuery] long itemId)
        {
            if (itemId == 0)
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            await _shoppingCartRepository.MarkAsBoughtAsync(itemId);
            return EmptyResult();
        }

        [HttpDelete("RemoveItem")]
        public async Task<IActionResult> RemoveItemAsync([FromQuery] long itemId)
        {
            if (itemId == 0)
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            
            await _shoppingCartRepository.RemoveItemAsync(itemId);
            return EmptyResult();
        }
    }
}