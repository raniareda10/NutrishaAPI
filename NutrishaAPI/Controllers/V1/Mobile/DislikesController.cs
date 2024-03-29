﻿using System.Threading.Tasks;
using BL.Repositories;
using DL.DtosV1.Allergies;
using DL.DtosV1.DisLikes;
using DL.Repositories.Allergy;
using DL.Repositories.Dislikes;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class DislikeMealController : BaseMobileController
    {
        private readonly DislikesMealService _dislikesMealService;
        private readonly string _locale;
        public DislikeMealController(DislikesMealService dislikesMealService, IHttpContextAccessor httpContextAccessor)
        {
            _dislikesMealService = dislikesMealService;
            _locale = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"];
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAllergiesAsync([FromHeader(Name = "Accept-Language")] string _locale)
        {
            return ListResult(await _dislikesMealService.GetAllAsync(_locale));
        }

        [HttpGet("GetSelectedDislikedMeals")]
        public async Task<IActionResult> GetSelectedDislikedMealsAsync([FromHeader(Name = "Accept-Language")] string _locale)
        {
            return ListResult(await _dislikesMealService.GetSelectAllergyNamesAsync(_locale));
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