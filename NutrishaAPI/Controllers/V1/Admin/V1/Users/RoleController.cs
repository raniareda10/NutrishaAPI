using System.Threading.Tasks;
using DL.CommonModels;
using DL.DtosV1.Users.Roles;
using DL.Repositories.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Users
{
    [Authorize]
    public class RoleController : BaseAdminV1Controller
    {
        private readonly RollRepository _roleRepository;

        public RoleController(RollRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync([FromBody] PostRoleDto postRoleDto)
        {
            var serviceResult = await _roleRepository.PostAsync(postRoleDto.RoleName);

            return ItemResult(serviceResult);
        }
        
        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetPagedListQueryModel query)
        {
            var pagedResult = await _roleRepository.GetPagedListAsync(query);
            return PagedResult(pagedResult);
        }
        
        [HttpGet("GetById")]
        public async Task<IActionResult> GetDetailsAsync([FromQuery] long id)
        {
            var res = await _roleRepository.GetDetailsAsync(id);
            return ItemResult(res);
        }
    }
}