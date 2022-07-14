using System.Threading.Tasks;
using BL.Repositories;
using DL.DtosV1.Meals;
using DL.Repositories.Meals;
using Microsoft.AspNetCore.Mvc;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Meals
{
    public class MealController : BaseAdminV1Controller
    {
        private readonly MealsRepository _mealRepository;

        public MealController(MealsRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromForm] PostMealDto postMealDto)
        {
            var result = await _mealRepository.PostAsync(postMealDto);
            return ItemResult(result);
        }


        [HttpPost("PostMealPlan")]
        public async Task<IActionResult> PostMealPlanAsync([FromBody] PostMealPlanDto postMealDto)
        {
            var result = await _mealRepository.PostMealPlanAsync(postMealDto);
            return ItemResult(result);
        }
        
        [HttpGet("GetCurrentPlan")]
        public async Task<IActionResult> GetCurrentPlanAsync(int userId)
        {
            var result = await _mealRepository.GetCurrentPlanAsync(userId);
            return ItemResult(result);
        }
    }
}