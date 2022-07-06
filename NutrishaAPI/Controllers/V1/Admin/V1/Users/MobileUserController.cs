using System.Threading.Tasks;
using DL.CommonModels;
using DL.DtosV1.Users.Mobiles;
using DL.Repositories.MobileUser;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Users
{
    public class MobileUserController : BaseAdminV1Controller
    {
        private readonly MobileUserRepository _mobileUserRepository;

        public MobileUserController(MobileUserRepository mobileUserRepository)
        {
            _mobileUserRepository = mobileUserRepository;
        }

        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetPagedListQueryModel model)
        {
            return PagedResult(await _mobileUserRepository.GetPagedListAsync(model));
        }

        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetailsAsync(int userId)
        {
            var result = await _mobileUserRepository.GetUserDetailsAsync(userId);
            return result == null ? InvalidResult(NonLocalizedErrorMessages.InvalidId) : ItemResult(result);
        }

        [HttpPost("Prevent")]
        public async Task<IActionResult> PreventUserAsync([FromBody] PreventUserDto preventUserDto)
        {
            await _mobileUserRepository.PreventUserAsync(preventUserDto);
            return EmptyResult();
        }
    }
}