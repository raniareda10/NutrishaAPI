using System.Threading.Tasks;
using DL.CommonModels;
using DL.DtosV1.Articles;
using DL.DtosV1.BlogVideo;
using DL.Repositories.BlogVideo;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Validations.BLogVideo;

namespace NutrishaAPI.Controllers.V1.Admin.V1.BlogVideo
{
    [Authorize]
    public class BlogVideoController : BaseAdminV1Controller
    {
        private readonly BlogVideoRepository _blogVideoRepository;

        public BlogVideoController(BlogVideoRepository blogVideoRepository)
        {
            _blogVideoRepository = blogVideoRepository;
        }

        [HttpPost("Post")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue,
            MultipartHeadersLengthLimit = int.MaxValue)]
        public async Task<IActionResult> PostAsync([FromForm] PostBlogVideoDto postArticleDto)
        {
            var validationResult = postArticleDto.IsValid();
            if (!validationResult.Success)
            {
                return InvalidResult(validationResult.Errors);
            }

            var result = await _blogVideoRepository.PostAsync(postArticleDto);
            return ItemResult(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> PostAsync([FromQuery] long id)
        {
            if (id < 1)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidId);
            }

            var result = await _blogVideoRepository.GetByIdForAdmin(id);
            return ItemResult(result);
        }

        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] GetPagedListQueryModel model)
        {
            return PagedResult(await _blogVideoRepository.GetPagedListAsync(model));
        }

        [HttpPut("Put")]
        public async Task<IActionResult> PutAsync([FromForm] EditBlogVideo editBlogVideo)
        {
            // var validationResult = editArticleDto.IsValid();
            // if (!validationResult.Success)
            // {
            //     return InvalidResult(validationResult.Errors);
            // }

            await _blogVideoRepository.PutAsync(editBlogVideo);

            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteArticle([FromQuery] long id)
        {
            if (id < 1) return InvalidResult(NonLocalizedErrorMessages.InvalidId);

            var result = await _blogVideoRepository.DeleteAsync(id);
            return result.Success ? EmptyResult() : InvalidResult(NonLocalizedErrorMessages.InvalidId);
        }
    }
}