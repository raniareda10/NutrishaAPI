using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DtosV1.Comments;
using DL.DtosV1.Reactions;
using DL.ResultModels;
using DL.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Bases;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Controllers.V1.Comments
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
            if (!postCommentDto.IsValidEntityId())
            {
                return InvalidResult(ErrorMessages.InvalidParameters);
            }
            
            var result = await _commentService.PostCommentAsync(postCommentDto);
            return result.Success ? ObjectResult(result.Data) : InvalidResult(result.Errors);
        }


        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetCommentsModel model)
        {
            if (!model.IsValidPagedModel() || !model.IsValidEntityId())
                return InvalidResult(ErrorMessages.InvalidParameters);

            return PagedResult(await _commentService.GetPagedListAsync(model));
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCommentAsync(long id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }
    }
}