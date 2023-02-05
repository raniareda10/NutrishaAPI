using System.Threading.Tasks;
using DL.DtosV1.Reactions;
using DL.Repositories.Reactions;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;
using NutrishaAPI.Controllers.V1.Mobile.Bases;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class ReactionController : BaseMobileController
    {
        private readonly ReactionService _reactionService;

        public ReactionController(ReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [AlloyNotBannedOnly]
        [HttpPost("Post")]
        public async Task<IActionResult> PostReactionAsync(UpdateReactionDto updateReactionDto)
        {
            if (!updateReactionDto.IsValidEntityId())
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }
            
            var result = await _reactionService.PostReactionAsync(updateReactionDto);

            if (result.Success)
            {
                return ItemResult(result.Data);
            }

            return InvalidResult(result.Errors);
        }
        
        
        // Server Limitation HttpDelete Not Allowed
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteReactionAsync([FromBody] UpdateReactionDto UpdateReactionDto)
        {
            if (!UpdateReactionDto.IsValidEntityId())
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }
            
            var result = await _reactionService.DeleteReactionAsync(UpdateReactionDto);
            if (result.Success)
            {
                return ItemResult(result.Data);
            }

            return InvalidResult(result.Errors);
        }
    }
}