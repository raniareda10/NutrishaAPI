using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class ProfileController : BaseMobileController
    {
        [HttpPut("Put")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProfile updateProfile)
        {
            return EmptyResult();
        }
    }

    public class UpdateProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderId { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
    }
}