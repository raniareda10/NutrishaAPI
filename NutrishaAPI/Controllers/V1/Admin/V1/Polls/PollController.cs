using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DtosV1.Polls;
using DL.ResultModels;
using DL.Services.Blogs.Polls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Validations.Polls;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Polls
{
    [Authorize]
    public class PollController : BaseAdminV1Controller
    {
        private readonly PollService _pollService;

        public PollController(PollService pollService)
        {
            _pollService = pollService;
        }
        
        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync([FromBody] PostPollDto postPollDto)
        {
            var validateResult = postPollDto.IsValid();
            if (!validateResult.Success)
            {
                return InvalidResult(validateResult.Errors);
            }

            return ItemResult(await _pollService.PostAsync(postPollDto));
        }
        
        
        [HttpGet("GetPagedList")]
        public async Task<IActionResult> PostAsync([FromQuery] PagedModel model)
        {
            if (!model.IsValidPagedModel())
            {
                return InvalidResult(NonLocalizedErrorMessages.InValidPageCountOrSize);
            }
            
            var result = await _pollService.GetPagedListAsync(model);
            return PagedResult(result);
        }
    }
}