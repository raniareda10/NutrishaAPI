using System.Linq;
using System.Threading.Tasks;
using DL.DtosV1.Common;
using DL.DtosV1.Meals;
using DL.ErrorMessages;
using DL.Repositories.Meals;
using DL.Repositories.Users.Admins;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Meals
{
    public class MealController : BaseAdminV1Controller
    {
        private readonly MealsRepository _mealRepository;
        private readonly AdminUserRepository _adminAuthRepository;
        public MealController(MealsRepository mealRepository, AdminUserRepository adminAuthRepository)
        {
            _mealRepository = mealRepository;
            _adminAuthRepository = adminAuthRepository;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromForm] PostMealDto postMealDto)
        {
            var user = _adminAuthRepository.GetCurrentUserAsync();
            if (user != null)
            {
                bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser(user.Id);
                if(isDeleted)
                {
                    return InvalidResult(NonLocalizedErrorMessages.DeletedUser);
                }
            }
            if (!postMealDto.IsValid())
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            var result = await _mealRepository.PostAsync(postMealDto);
            return ItemResult(result);
        }

        [HttpPut("Put")]
        public async Task<IActionResult> PutAsync([FromForm] EditMealDto editMealDto)
        {
            var user = _adminAuthRepository.GetCurrentUserAsync();
            if (user != null)
            {
                bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser(user.Id);
                if (isDeleted)
                {
                    return InvalidResult(NonLocalizedErrorMessages.DeletedUser);
                }
            }
            if (!editMealDto.IsValid())
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            var result = await _mealRepository.EditAsync(editMealDto);

            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] long id)
        {
            var user = _adminAuthRepository.GetCurrentUserAsync();
            if (user != null)
            {
                bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser(user.Id);
                if (isDeleted)
                {
                    return InvalidResult(NonLocalizedErrorMessages.DeletedUser);
                }
            }
            if (id < 1)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            var result = await _mealRepository.DeleteAsync(id);

            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }

        
        [HttpPost("PostIngredient")]
        public async Task<IActionResult> PostIngredient([FromBody] PostLookupItem postLookupItem)
        {
            var user = _adminAuthRepository.GetCurrentUserAsync();
            if (user != null)
            {
                bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser(user.Id);
                if (isDeleted)
                {
                    return InvalidResult(NonLocalizedErrorMessages.DeletedUser);
                }
            }
            var result = await _mealRepository.PostIngredientAsync(postLookupItem);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }

        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetMealsPagedListQuery getPagedListQueryModel)
        {
            var user = _adminAuthRepository.GetCurrentUserAsync();
            if (user != null)
            {
                bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser(user.Id);
                if (isDeleted)
                {
                    return InvalidResult(NonLocalizedErrorMessages.DeletedUser);
                }
            }
            var result = await _mealRepository.GetMealsAsync(getPagedListQueryModel);
            return PagedResult(result);
        }

        [HttpGet("GetMealsLookup")]
        public async Task<IActionResult> GetMealsLookupAsync()
        {
            var user = _adminAuthRepository.GetCurrentUserAsync();
            if (user != null)
            {
                bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser(user.Id);
                if (isDeleted)
                {
                    return InvalidResult(NonLocalizedErrorMessages.DeletedUser);
                }
            }
            var result = await _mealRepository.GetMealsLookupAsync();
            return ItemResult(result);
        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] long id)
        {
            var user = _adminAuthRepository.GetCurrentUserAsync();
            if (user != null)
            {
                bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser(user.Id);
                if (isDeleted)
                {
                    return InvalidResult(NonLocalizedErrorMessages.DeletedUser);
                }
            }
            var result = await _mealRepository.GetByIdAsync(id);
            return ItemResult(result);
        }

        [HttpGet("GetIngredientLookup")]
        public async Task<IActionResult> GetIngredientLookupAsync([FromQuery] string searchWord)
        {
            var user = _adminAuthRepository.GetCurrentUserAsync();
            if (user != null)
            {
                bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser(user.Id);
                if (isDeleted)
                {
                    return InvalidResult(NonLocalizedErrorMessages.DeletedUser);
                }
            }
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