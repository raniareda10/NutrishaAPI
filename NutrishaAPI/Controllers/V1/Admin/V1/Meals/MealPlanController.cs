using System.Collections.Generic;
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
        
        [HttpPut("UpdateMealPlan")]
        public async Task<IActionResult> UpdateMealPlanAsync([FromBody] UpdateMealPlan updateMealDto)
        {
            if (updateMealDto.Id < 1)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            if (updateMealDto.Meals == null ||
                updateMealDto.Meals.Count is < 1 or > 7 ||
                updateMealDto.Meals.Select(m => m.Day).Distinct().Count() < 7)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            switch (updateMealDto.IsTemplate)
            {
                case true when string.IsNullOrWhiteSpace(updateMealDto.TemplateName):
                    return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
                case false when !updateMealDto.UserId.HasValue:
                    return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            await _mealRepository.UpdateTemplateAsync(updateMealDto);
            return EmptyResult();
        }
        
        // [HttpPut("UpdateMealPlan")]
        // public async Task<IActionResult> UpdateMealPlanAsync([FromBody] UpdateMealPlan postMealDto)
        // {
        //     if (postMealDto.Id < 1)
        //     {
        //         return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
        //     }
        //
        //     if (postMealDto.Meals == null ||
        //         postMealDto.Meals.Count is < 1 or > 7 ||
        //         postMealDto.Meals.Select(m => m.Day).Distinct().Count() < 7)
        //     {
        //         return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
        //     }
        //
        //     switch (postMealDto.IsTemplate)
        //     {
        //         case true when string.IsNullOrWhiteSpace(postMealDto.TemplateName):
        //             return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
        //         case false when !postMealDto.UserId.HasValue:
        //             return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
        //     }
        //     
        //     return ItemResult(await _mealRepository.UpdateTemplateAsync(postMealDto));
        // }
        //
        [HttpGet("GetTemplatePagedList")]
        public async Task<IActionResult> GetTemplatePagedListAsync([FromQuery] GetPagedListQueryModel query)
        {
            var result = await _mealRepository.GetTemplatePagedListAsync(query);
            return PagedResult(result);
        }


        [HttpGet("GetTemplateById")]
        public async Task<IActionResult> GetTemplateByIdAsync([FromQuery] long id)
        {
            var result = await _mealRepository.GetTemplateByIdAsync(id);

            return ItemResult(result);
        }
    }
}