using System.Threading.Tasks;
using DL.DtosV1.Users.Mobiles;
using DL.Services.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class ProfileController : BaseMobileController
    {
        private readonly MobileProfileService _mobileProfileService;

        public ProfileController(MobileProfileService mobileProfileService)
        {
            _mobileProfileService = mobileProfileService;
        }
        [HttpPut("Put")]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateProfileDto updateProfileDto)
        {
            var serviceResult = await _mobileProfileService.PutAsync(updateProfileDto);
            return ItemResult(serviceResult.Data);
        }
    }

    
}