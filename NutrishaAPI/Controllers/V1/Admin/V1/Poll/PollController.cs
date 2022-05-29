using System.Threading.Tasks;
using DL.DtosV1.Polls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Validations.Polls;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Poll
{
    [Authorize]
    public class PollController : BaseAdminV1Controller
    {
        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync([FromBody] PostPollDto postPollDto)
        {
            var validateResult = postPollDto.Validate();
            if (!validateResult.Success)
            {
                return InvalidResult(validateResult.Errors);
            }

            return ItemResult<string>(null);
        }
    }
}