using System.Threading.Tasks;
using DL.DtosV1.Comments;
using DL.Repositories.Comments;
using DL.ResultModels;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Controllers.V1.Admin.V1
{
    public class CommentController : BaseAdminV1Controller
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetCommentsModel model)
        {
            if (!model.IsValidPagedModel() || !model.IsValidEntityId())
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);

            return PagedResult(await _commentService.GetPagedListAsync(model));
        }
        
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCommentAsync([FromQuery] long id)
        {
            if (id < 1) return InvalidResult(NonLocalizedErrorMessages.InvalidId);
            
            var result = await _commentService.DeleteCommentAsync(id, true);
            return result.Success ? EmptyResult() : InvalidResult(result.Errors);
        }
    }
}