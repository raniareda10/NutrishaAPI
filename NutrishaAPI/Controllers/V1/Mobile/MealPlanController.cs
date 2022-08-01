using System.Threading.Tasks;
using DL.DtosV1.MealPlans;
using DL.Repositories.MealPlan;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    public class MealPlanController : BaseMobileController
    {
        private readonly MobileMealPlanRepository _mobileMealPlanRepository;

        public MealPlanController(MobileMealPlanRepository mobileMealPlanRepository)
        {
            _mobileMealPlanRepository = mobileMealPlanRepository;
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
        
        [HttpGet("GetRecommendedMeals")]
        public async Task<IActionResult> GetRecommendedMealsAsync([FromQuery] SwapMealDto swapMealDto)
        {
            return ItemResult(await _mobileMealPlanRepository.GetRecommendedMealsAsync(swapMealDto));
        }
    }
}