﻿using System.Linq;
using System.Threading.Tasks;
using DL.DtosV1.Common;
using DL.DtosV1.Meals;
using DL.ErrorMessages;
using DL.Repositories.Meals;
using DL.ResultModels;
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
            if (!postMealDto.IsValid())
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }
            
            var result = await _mealRepository.PostAsync(postMealDto);
            return ItemResult(result);
        }

        [HttpPost("PostIngredient")]
        public async Task<IActionResult> PostIngredient([FromBody] PostLookupItem postLookupItem)
        {
            var result = await _mealRepository.PostIngredientAsync(postLookupItem);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }
        
        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetMealsPagedListQuery getPagedListQueryModel)
        {
            var result = await _mealRepository.GetMealsAsync(getPagedListQueryModel);
            return PagedResult(result);
        }

        [HttpGet("GetMealsLookup")]
        public async Task<IActionResult> GetMealsLookupAsync()
        {
            var result = await _mealRepository.GetMealsLookupAsync();
            return ItemResult(result);
        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] long id)
        {
            var result = await _mealRepository.GetByIdAsync(id);
            return ItemResult(result);
        }
        
        [HttpGet("GetIngredientLookup")]
        public async Task<IActionResult> GetIngredientLookupAsync([FromQuery] string searchWord)
        {
            var result = await _mealRepository.GetIngredientLookupAsync(searchWord);
            return ItemResult(result);
        }


        
        // [HttpGet("GetCurrentPlan")]
        // public async Task<IActionResult> GetCurrentPlanAsync(int userId)
        // {
        //     var result = await _mealRepository.GetCurrentPlanAsync(userId);
        //     return ItemResult(result);
        // }
    }
}