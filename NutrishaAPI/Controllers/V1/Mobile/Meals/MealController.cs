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

  
        // [HttpGet("GetPagedList")]
        // public async Task<IActionResult> GetPagedListAsync([FromQuery] GetMealsPagedListQuery getPagedListQueryModel)
        // {
        //     var result = await _mealRepository.GetMealsAsync(getPagedListQueryModel);
        //     return PagedResult(result);
        // }
        //
        // [HttpGet("GetMealsLookup")]
        // public async Task<IActionResult> GetMealsLookupAsync()
        // {
        //     var result = await _mealRepository.GetMealsLookupAsync();
        //     return ItemResult(result);
        // }


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
        
        // [HttpGet("GetIngredientLookup")]
        // public async Task<IActionResult> GetIngredientLookupAsync([FromQuery] string searchWord)
        // {
        //     var result = await _mealRepository.GetIngredientLookupAsync(searchWord);
        //     return ItemResult(result);
        // }
    }
}