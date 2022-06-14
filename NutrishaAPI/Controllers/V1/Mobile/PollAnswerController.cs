using System.Threading.Tasks;
using DL.DtosV1.Polls;
using DL.Repositories.Polls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class PollAnswerController : BaseMobileController
    {
        private readonly PollAnswerService _pollAnswerService;

        public PollAnswerController(PollAnswerService pollAnswerService)
        {
            _pollAnswerService = pollAnswerService;
        }
        
        [HttpPost("Post")]
        public async Task<IActionResult> PostReactionAsync(PostAnswerDto postAnswerDto)
        {
            var result = await _pollAnswerService.PostAnswerAsync(postAnswerDto);

            if (result.Success)
            {
                return ListResult(result.Data);
            }


            return InvalidResult(result.Errors);
        }
    }
}