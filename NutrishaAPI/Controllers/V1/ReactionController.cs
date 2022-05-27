using System.Threading.Tasks;
using DL.DtosV1.Reactions;
using DL.ResultModels;
using DL.Services.Reactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Bases;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Controllers.V1
{
    [Authorize]
    public class ReactionController : BaseMobileController
    {
        private readonly ReactionService _reactionService;

        public ReactionController(ReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostReactionAsync(UpdateReactionDto UpdateReactionDto)
        {
            if (!UpdateReactionDto.IsValidEntityId())
            {
                return InvalidResult(ErrorMessages.InvalidParameters);
            }
            
            var result = await _reactionService.PostReactionAsync(UpdateReactionDto);

            if (result.Success)
            {
                return ObjectResult(result.Data);
            }

            return InvalidResult(result.Errors);
        }
        
        
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteReactionAsync(UpdateReactionDto UpdateReactionDto)
        {
            if (!UpdateReactionDto.IsValidEntityId())
            {
                return InvalidResult(ErrorMessages.InvalidParameters);
            }
            
            var result = await _reactionService.DeleteReactionAsync(UpdateReactionDto);
            if (result.Success)
            {
                return ObjectResult(result.Data);
            }

            return InvalidResult(result.Errors);
        }
    }
}