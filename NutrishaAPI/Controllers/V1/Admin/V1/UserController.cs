using System.Threading.Tasks;
using DL.DtosV1.Users.Admins;
using DL.Services.Users.Admins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Validations.Users;

namespace NutrishaAPI.Controllers.V1.Admin.V1
{
    [AllowAnonymous]
    public class UserController : BaseAdminV1RoutingController
    {
        private readonly AdminUserService _adminUserService;

        public UserController(AdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(AdminLoginDto adminLoginDto)
        {
            var validateResult = adminLoginDto.IsValid();
            if (!validateResult.Success)
            {
                return InvalidResult(validateResult.Errors);
            }

            var serviceResult = await _adminUserService.LoginAsync(adminLoginDto);
            if (!serviceResult.Success)
            {
                return InvalidResult(serviceResult.Errors);
            }
            
            return ItemResult(serviceResult.Data);
        }
    }
}