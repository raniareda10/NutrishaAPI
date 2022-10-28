﻿using System.Threading.Tasks;
using DL.DtosV1.Users.Admins;
using DL.Repositories.Users.Admins;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;
using NutrishaAPI.Validations.Shared;

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

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            return id < 1
                ? InvalidResult(NonLocalizedErrorMessages.InvalidId)
                : ItemResult(await _adminAuthRepository.GetByIdAsync(id));
        }


        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync([FromBody] CreateAdminDto createAdminDto)
        {
            if (!createAdminDto.Email.IsValidEmail() || createAdminDto.RoleId is null or < 1)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            var result = await _adminAuthRepository.CreateAdminUserAsync(createAdminDto);
            return result.Success ? ItemResult(result.Data) : InvalidResult(result.Errors);
        }

        [HttpPost("Put")]
        public async Task<IActionResult> PutAsync([FromBody] CreateAdminDto createAdminDto)
        {
            await _adminAuthRepository.UpdateAdminUserAsync(createAdminDto);
            return EmptyResult();
        }
    }
}