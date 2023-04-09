using System.Threading.Tasks;
using DL.DtosV1.Users.Admins;
using DL.Repositories.Users.Admins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Validations.Users;

namespace NutrishaAPI.Controllers.V1.Admin.V1
{
    [AllowAnonymous]
    public class AuthController : BaseAdminV1RoutingController
    {
        private readonly AdminAuthService _adminAuthService;

        public AuthController(AdminAuthService adminAuthService)
        {
            _adminAuthService = adminAuthService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(AdminLoginDto adminLoginDto)
        {


            var validateResult = adminLoginDto.IsValid();
            if (!validateResult.Success)
            {
                return InvalidResult(validateResult.Errors);
            }

            var serviceResult = await _adminAuthService.LoginAsync(adminLoginDto);
            if (!serviceResult.Success)
            {
                return InvalidResult(serviceResult.Errors);
            }

            return ItemResult(serviceResult.Data);
        }

        [HttpPost("RequestResetPassword")]
        public async Task<IActionResult> RequestResetPasswordAsync(RequestResetPasswordRequest requestResetPassword)
        {
            var serviceResult = await _adminAuthService.RequestResetPasswordAsync(requestResetPassword);
            if (!serviceResult.Success)
            {
                return InvalidResult(serviceResult.Errors);
            }

            return EmptyResult();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest requestResetPassword)
        {
            var serviceResult = await _adminAuthService.ResetPasswordAsync(requestResetPassword);
            if (!serviceResult.Success)
            {
                return InvalidResult(serviceResult.Errors);
            }

            return EmptyResult();
        }
        
        
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var serviceResult = await _adminAuthService.ChangePasswordAsync(changePasswordRequest);
            if (!serviceResult.Success)
            {
                return InvalidResult(serviceResult.Errors);
            }

            return EmptyResult();
        }
    }
}