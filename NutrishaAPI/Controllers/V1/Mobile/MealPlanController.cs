using System.Threading.Tasks;
using DL.DtosV1.MealPlans;
using DL.Repositories.MealPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    public class MealPlanController : BaseMobileController
    {
        private readonly MobileMealPlanRepository _mobileMealPlanRepository;
        private readonly string _locale;
        public MealPlanController(MobileMealPlanRepository mobileMealPlanRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mobileMealPlanRepository = mobileMealPlanRepository;
            _locale = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"];
        }

        [HttpGet("GetTodayMeals")]
        public async Task<IActionResult> GetTodayMealsAsync()
        {
            return ItemResult(await _mobileMealPlanRepository.GetTodayMealsAsync());
        }

        [HttpGet("GetCurrentPlan")]
        public async Task<IActionResult> GetCurrentPlanAsync()
        {
            return ItemResult(await _mobileMealPlanRepository.GetCurrentPlanAsync());
        }

        [HttpGet("GetRecommendedMealsForSwap")]
        public async Task<IActionResult> GetRecommendedMealsAsync([FromQuery] SwapMealDto swapMealDto)
        {
            return ItemResult(await _mobileMealPlanRepository.GetRecommendedMealsForSwapAsync(swapMealDto));
        }

        [HttpPut("AddCupOfWaterToDay")]
        public async Task<IActionResult> AddCupOfWaterToDayAsync([FromBody] AddDrunkWaterCupDto dto)
        {
            await _mobileMealPlanRepository.AddCupOfWaterToDayAsync(dto.DayId, dto.NumberOfCups);
            return EmptyResult();
        }
        [HttpPut("AddIsExercisedToDay")]
        public async Task<IActionResult> AddIsExercisedToDayAsync([FromBody] AddIsExercisedDto dto)
        {
            await _mobileMealPlanRepository.AddIsExercisedAsync(dto.DayId, dto.IsExercised);
            return EmptyResult();
        }

        [HttpPut("UserCLickedIAmHungry")]
        public async Task<IActionResult> UserCLickedIAmHungryAsync([FromQuery] long planId)
        {
            await _mobileMealPlanRepository.UserCLickedIAmHungryAsync(planId);
            return EmptyResult();
        }

        [HttpPut("SwapMenu")]
        public async Task<IActionResult> SwapMenuAsync([FromQuery] long oldMenuId, [FromQuery] long swapWithMenuId)
        {
            var result = await _mobileMealPlanRepository.SwapMenuAsync(oldMenuId, swapWithMenuId);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }

        [HttpPut("SkipMeal")]
        public async Task<IActionResult> SkipMealAsync([FromQuery] long menuId)
        {
            var result = await _mobileMealPlanRepository.SkipMenuAsync(menuId);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }

        [HttpPut("MarkAsEaten")]
        public async Task<IActionResult> MarkAsEatenAsync([FromQuery] long menuId)
        {
            var result = await _mobileMealPlanRepository.MarkMenuAsEatenAsync(menuId);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }

        [HttpPut("MarkMenuMealAsEaten")]
        public async Task<IActionResult> MarkMenuMealAsEatenAsync([FromQuery] long mealId)
        {
            var result = await _mobileMealPlanRepository.MarkMenuMealAsEatenAsync(mealId);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }

        [HttpPut("AddExtraBiteMeal")]
        public async Task<IActionResult> AddExtraBiteMealAsync([FromBody] AddExtraBitesDto dto)
        {
            var result = await _mobileMealPlanRepository.AddExtraBiteMealAsync(dto,_locale);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }
    }
}