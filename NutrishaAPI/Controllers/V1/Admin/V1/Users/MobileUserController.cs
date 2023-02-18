using System.Threading.Tasks;
using DL.DtosV1.Users.Admins;
using DL.DtosV1.Users.Mobiles;
using DL.Repositories.MobileUser;
using DL.Repositories.Permissions;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;

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
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetUserMobilePagedListQueryModel model)
        {
            return PagedResult(await _mobileUserRepository.GetPagedListAsync(model));
        }

        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetailsAsync(int userId)
        {
            var result = await _mobileUserRepository.GetUserDetailsAsync(userId);
            return result == null ? InvalidResult(NonLocalizedErrorMessages.InvalidId) : ItemResult(result);
        }
        
        [HttpGet("UserMessageSeen")]
        public async Task<IActionResult> UserMessageSeenAsync(int userId)
        {
            await _mobileUserRepository.UserMessageSeenAsync(userId);
            return EmptyResult();
        }

        [HttpGet("GetUserPersonalDetails")]
        public async Task<IActionResult> GetUserPersonalDetailsAsync(int userId)
        {
            var result = await _mobileUserRepository.GetUserPersonalDetailsAsync(userId);
            return ItemResult(result);
        }

        [HttpPut("Ban")]
        [HasPermissionOnly(PermissionNames.CanBanAppUsers)]
        public async Task<IActionResult> BanUserAsync([FromQuery] int userId)
        {
            await _mobileUserRepository.SetUserBanFlagAsync(userId, true);
            return EmptyResult();
        }
        
        [HttpPut("UnBan")]
        [HasPermissionOnly(PermissionNames.CanBanAppUsers)]
        public async Task<IActionResult> UnBanUserAsync([FromQuery] int userId)
        {
            await _mobileUserRepository.SetUserBanFlagAsync(userId, false);
            return EmptyResult();
        }
        
        [HttpPost("Prevent")]
        public async Task<IActionResult> PreventUserAsync([FromBody] PreventUserDto preventUserDto)
        {
            await _mobileUserRepository.PreventUserAsync(preventUserDto);
            return EmptyResult();
        }
    }
}