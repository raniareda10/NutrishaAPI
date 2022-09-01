using System.Threading.Tasks;
using DL.DtosV1.Users.Admins;
using DL.Repositories.Users.Admins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;
using NutrishaAPI.Validations.Users;

namespace NutrishaAPI.Controllers.V1.Admin.V1
{
    [Authorize]
    [OnlyAdmins]
    public class AdminUserController : BaseAdminV1RoutingController
    {
        private readonly AdminUserRepository _adminAuthRepository;

        public AdminUserController(AdminUserRepository adminAuthRepository)
        {
            _adminAuthRepository = adminAuthRepository;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserAsync()
        {
            var serviceResult = await _adminAuthRepository.GetCurrentUserAsync();

            return ItemResult(serviceResult);
        }

        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUserAsync([FromBody] AssignRoleToUserDto assignRoleToUserDto)
        {
            await _adminAuthRepository.AssignRoleToUserAsync(assignRoleToUserDto);
            return EmptyResult();
        }

        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetAdminUserPagedListQueryDto query)
        {
            return PagedResult(await _adminAuthRepository.GetPagedListAsync(query));
        }


        [HttpPost("CreateAdminUser")]
        public async Task<IActionResult> CreateAdminUserAsync([FromBody] CreateAdminDto createAdminDto)
        {
            await _adminAuthRepository.CreateAdminUserAsync(createAdminDto);
            return EmptyResult();
        }
    }

  
}