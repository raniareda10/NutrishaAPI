using DL.Repositories.Users.Admins;
using Microsoft.AspNetCore.Mvc;
using NLog.Time;

namespace NutrishaAPI.Controllers.V1.Admin.V1
{
    public class BingController : BaseAdminV1RoutingController
    {
        [HttpGet("Bing")]
        public IActionResult Bing()
        {
            return Ok(true);
        }
    }
}