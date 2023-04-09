using System.Threading.Tasks;
using DL.DtosV1.Users.Admins;
using DL.DtosV1.Users.Mobiles;
using DL.Repositories.MobileUser;
using DL.Repositories.Permissions;
using DL.Repositories.Users.Admins;
using DL.ResultModels;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Users
{
    public class MobileUserController : BaseAdminV1Controller
    {
        private readonly MobileUserRepository _mobileUserRepository;
        private readonly FirestoreDb _firestoreDb;
        private readonly AdminUserRepository _adminAuthRepository;
        public MobileUserController(MobileUserRepository mobileUserRepository, FirestoreDb firestoreDb, AdminUserRepository adminAuthRepository)
        {
            _mobileUserRepository = mobileUserRepository;
            _firestoreDb = firestoreDb;
            _adminAuthRepository = adminAuthRepository;
        }

        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetUserMobilePagedListQueryModel model)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            return PagedResult(await _mobileUserRepository.GetPagedListAsync(model));
        }

        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetailsAsync(int userId)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            var result = await _mobileUserRepository.GetUserDetailsAsync(userId);
            return result == null ? InvalidResult(NonLocalizedErrorMessages.InvalidId) : ItemResult(result);
        }

        [HttpGet("UserMessageSeen")]
        public async Task<IActionResult> UserMessageSeenAsync(int userId)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            await _mobileUserRepository.UserMessageSeenAsync(userId);
            return EmptyResult();
        }

        [HttpGet("GetUserPersonalDetails")]
        public async Task<IActionResult> GetUserPersonalDetailsAsync(int userId)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            var result = await _mobileUserRepository.GetUserPersonalDetailsAsync(userId);
            return ItemResult(result);
        }

        [HttpPut("Ban")]
        [HasPermissionOnly(PermissionNames.CanBanAppUsers)]
        public async Task<IActionResult> BanUserAsync([FromQuery] int userId)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            await _mobileUserRepository.SetUserBanFlagAsync(userId, true);
            return EmptyResult();
        }

        [HttpPut("UnBan")]
        [HasPermissionOnly(PermissionNames.CanBanAppUsers)]
        public async Task<IActionResult> UnBanUserAsync([FromQuery] int userId)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            await _mobileUserRepository.SetUserBanFlagAsync(userId, false);
            return EmptyResult();
        }

        [HttpPost("Prevent")]
        public async Task<IActionResult> PreventUserAsync([FromBody] PreventUserDto preventUserDto)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            await _mobileUserRepository.PreventUserAsync(preventUserDto);
            return EmptyResult();
        }

        [HttpPost("Test")]
        public async Task<IActionResult> MakePremiumAsync(
            [FromBody] ManualAppSubscribeRequest manualAppSubscribeRequest)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            await _mobileUserRepository.MakePremiumAsync(manualAppSubscribeRequest);
            await _firestoreDb.Collection("users").Document(manualAppSubscribeRequest.UserId.ToString()).CreateAsync(new
            {
                subscription = new
                {
                    expirationDate = manualAppSubscribeRequest.EndDate
                }
            });
            return EmptyResult();
        }
        
        [HttpPost("RemovePremium")]
        public async Task<IActionResult> RemovePremiumAsync(
            [FromBody] ManualAppSubscribeRequest manualAppSubscribeRequest)
        {
            bool isDeleted = _adminAuthRepository.CheckDeletedAdminUser();
            if (isDeleted)
            {
                return InvalidDeleteResult(NonLocalizedErrorMessages.DeletedUser);
            }
            await _mobileUserRepository.RemovePremiumAsync(manualAppSubscribeRequest);
            await _firestoreDb.Collection("users").Document(manualAppSubscribeRequest.UserId.ToString()).DeleteAsync();
            return EmptyResult();
        }
    }
}