using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Users
{
    public class MobileUserController : BaseAdminV1Controller
    {
        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync()
        {
            return Ok();
        }
        
        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetailsAsync(int userId)
        {
            return Ok();
        }
    }
}