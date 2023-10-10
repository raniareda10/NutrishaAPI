using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.DtosV1.Meals;
using DL.Repositories.Meals;
using DL.Repositories.Users.Admins;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Meals
{
    public class MealPlanController : BaseAdminV1Controller
    {
        private readonly MealPlanRepository _mealRepository;
        private readonly AdminUserRepository _adminAuthRepository;
        public MealPlanController(MealPlanRepository mealRepository, AdminUserRepository adminAuthRepository)
        {
            _mealRepository = mealRepository;
            _adminAuthRepository = adminAuthRepository;
        }

        [HttpPost("PostMealPlan")]
        public async Task<IActionResult> PostMealPlanAsync([FromBody] PostMealPlanDto postMealDto)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
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
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
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

        [HttpPut("UpdateMealPlanNotes")]
        public async Task<IActionResult> UpdateMealPlanNotesAsync([FromQuery] long mealPlanId, [FromQuery] string doctorNotes)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            if (mealPlanId < 1)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }
           

            await _mealRepository.UpdateTemplateNotesAsync(mealPlanId, doctorNotes);
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

            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }

            var result = await _mealRepository.GetTemplatePagedListAsync(query);
            return PagedResult(result);
        }


        [HttpGet("GetTemplateById")]
        public async Task<IActionResult> GetTemplateByIdAsync([FromQuery] long id)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            var result = await _mealRepository.GetTemplateByIdAsync(id);

            return ItemResult(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteTemplateAsync([FromQuery] long id)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            var result = await _mealRepository.DeleteTemplateAsync(id);

            if (result.Success)
            {
                return EmptyResult();
            }

            return InvalidResult(result.Errors);
        }

  
    }
}