using System.Threading.Tasks;
using BL.Repositories;
using DL.DtosV1.Allergies;
using DL.DtosV1.DisLikes;
using DL.Repositories.Allergy;
using DL.Repositories.Dislikes;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class DislikeMealController : BaseMobileController
    {
        private readonly DislikesMealService _dislikesMealService;

        public DislikeMealController(DislikesMealService dislikesMealService)
        {
            _dislikesMealService = dislikesMealService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAllergiesAsync()
        {
            return ListResult(await _dislikesMealService.GetAllAsync());
        }

        [HttpGet("GetSelectedDislikedMeals")]
        public async Task<IActionResult> GetSelectedDislikedMealsAsync()
        {
            return ListResult(await _dislikesMealService.GetSelectAllergyNamesAsync());
        }
        
        [HttpPut("Put")]
        public async Task<IActionResult> PutAsync(PutDisLikesDto putDisLikesDto)
        {
            await _dislikesMealService.PutAsync(putDisLikesDto);
            return EmptyResult();
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync([FromBody] PostDislikesDto postAllergyDto)
        {
            if (string.IsNullOrWhiteSpace(postAllergyDto.DislikedMealName))
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            
            var result = await _dislikesMealService.AddCustomAllergiesAsync(postAllergyDto.DislikedMealName, postAllergyDto.DislikedMealNameAr);
            return ItemResult(result);
        }
    }
}