using System.Threading.Tasks;
using DL.DtosV1.Comments;
using DL.ResultModels;
using DL.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;
using NutrishaAPI.Validations.Comments;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Controllers.V1.Mobile.Comments
{
    [Authorize]
    public class CommentController : BaseMobileController
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostCommentAsync(PostCommentDto postCommentDto)
        {
            var validationResult = postCommentDto.IsValid();
            if (!validationResult.Success)
            {
                return InvalidResult(validationResult.Errors);
            }
            
            var result = await _commentService.PostCommentAsync(postCommentDto);
            return result.Success ? ItemResult(result.Data) : InvalidResult(result.Errors);
        }


        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetCommentsModel model)
        {
            if (!model.IsValidPagedModel() || !model.IsValidEntityId())
                return InvalidResult(ErrorMessages.InvalidParameters);

            return PagedResult(await _commentService.GetPagedListAsync(model));
        }
        
        // Server Limitation HttpDelete Not Allowed
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteCommentAsync([FromQuery] long id)
        {
            if (id < 1) return InvalidResult(ErrorMessages.InvalidId);
            
            var result = await _commentService.DeleteCommentAsync(id);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }
    }
}