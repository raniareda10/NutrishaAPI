using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.DtosV1.Meals;
using DL.Repositories.Meals;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Meals
{
    public class MealPlanController : BaseAdminV1Controller
    {
        private readonly MealPlanRepository _mealRepository;

        public MealPlanController(MealPlanRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        [HttpPost("PostMealPlan")]
        public async Task<IActionResult> PostMealPlanAsync([FromBody] PostMealPlanDto postMealDto)
        {
            if (postMealDto.Meals == null ||
                postMealDto.Meals.Count is < 1 or > 7 ||
                postMealDto.Meals.Select(m => m.Day).Distinct().Count() < 7)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            switch (postMealDto.IsTemplate)
            {
                case true when string.IsNullOrWhiteSpace(postMealDto.TemplateName):
                    return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
                case false when !postMealDto.UserId.HasValue:
                    return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }
            
            var result = await _mealRepository.PostMealPlanAsync(postMealDto);
            return ItemResult(result);
        }

        [HttpGet("GetTemplatePagedList")]
        public async Task<IActionResult> GetTemplatePagedListAsync([FromQuery] GetPagedListQueryModel query)
        {
            var result = await _mealRepository.GetTemplatePagedListAsync(query);
            return PagedResult(result);
        }
    }
}