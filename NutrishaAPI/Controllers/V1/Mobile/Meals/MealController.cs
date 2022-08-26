using System.Linq;
using System.Threading.Tasks;
using DL.DtosV1.Common;
using DL.DtosV1.Meals;
using DL.ErrorMessages;
using DL.Repositories;
using DL.Repositories.Meals;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile.Meals
{
    public class MealController : BaseMobileController
    {
        private readonly MobileMealsService _mealRepository;

        public MealController(MobileMealsService mealRepository)
        {
            _mealRepository = mealRepository;
        }
        
        [HttpGet("GetRecommendedMeals")]
        public async Task<IActionResult> GetPagedListAsync()
        {
            var result = await _mealRepository.GetRecommendedMealsAsync();
            return ListResult(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] long id)
        {
            var result = await _mealRepository.GetByIdAsync(id);
            return ItemResult(result);
        }
        
        [HttpPut("MarkAsFavorite")]
        public async Task<IActionResult> MarkAsFavoriteAsync([FromQuery] long mealId)
        {
            await _mealRepository.MarkAsFavoriteAsync(mealId);
            return EmptyResult();
        }
    }
}